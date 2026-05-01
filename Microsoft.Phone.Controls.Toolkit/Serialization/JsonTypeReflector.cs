using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000018 RID: 24
	internal static class JsonTypeReflector
	{
		// Token: 0x06000135 RID: 309 RVA: 0x000078D4 File Offset: 0x00005AD4
		private static Type GetTypeConverterTypeFromAttribute(ICustomAttributeProvider attributeProvider)
		{
			TypeConverterAttribute attribute = JsonTypeReflector.GetAttribute<TypeConverterAttribute>(attributeProvider);
			if (attribute == null)
			{
				return null;
			}
			return Type.GetType(attribute.ConverterTypeName);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000078F8 File Offset: 0x00005AF8
		private static Type GetTypeConverterType(ICustomAttributeProvider attributeProvider)
		{
			return JsonTypeReflector.TypeConverterTypeCache.Get(attributeProvider);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007905 File Offset: 0x00005B05
		public static JsonContainerAttribute GetJsonContainerAttribute(Type type)
		{
			return CachedAttributeGetter<JsonContainerAttribute>.GetAttribute(type);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000790D File Offset: 0x00005B0D
		public static JsonObjectAttribute GetJsonObjectAttribute(Type type)
		{
			return JsonTypeReflector.GetJsonContainerAttribute(type) as JsonObjectAttribute;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000791A File Offset: 0x00005B1A
		public static JsonArrayAttribute GetJsonArrayAttribute(Type type)
		{
			return JsonTypeReflector.GetJsonContainerAttribute(type) as JsonArrayAttribute;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007928 File Offset: 0x00005B28
		public static DataContractAttribute GetDataContractAttribute(Type type)
		{
			DataContractAttribute dataContractAttribute = null;
			Type type2 = type;
			while (dataContractAttribute == null && type2 != null)
			{
				dataContractAttribute = CachedAttributeGetter<DataContractAttribute>.GetAttribute(type2);
				type2 = type2.BaseType;
			}
			return dataContractAttribute;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00007950 File Offset: 0x00005B50
		public static DataMemberAttribute GetDataMemberAttribute(MemberInfo memberInfo)
		{
			if (memberInfo.MemberType == 4)
			{
				return CachedAttributeGetter<DataMemberAttribute>.GetAttribute(memberInfo);
			}
			PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
			DataMemberAttribute attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(propertyInfo);
			if (attribute == null && propertyInfo.IsVirtual())
			{
				Type type = propertyInfo.DeclaringType;
				while (attribute == null && type != null)
				{
					PropertyInfo propertyInfo2 = (PropertyInfo)ReflectionUtils.GetMemberInfoFromType(type, propertyInfo);
					if (propertyInfo2 != null && propertyInfo2.IsVirtual())
					{
						attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(propertyInfo2);
					}
					type = type.BaseType;
				}
			}
			return attribute;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000079BC File Offset: 0x00005BBC
		public static MemberSerialization GetObjectMemberSerialization(Type objectType)
		{
			JsonObjectAttribute jsonObjectAttribute = JsonTypeReflector.GetJsonObjectAttribute(objectType);
			if (jsonObjectAttribute != null)
			{
				return jsonObjectAttribute.MemberSerialization;
			}
			DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(objectType);
			if (dataContractAttribute != null)
			{
				return MemberSerialization.OptIn;
			}
			return MemberSerialization.OptOut;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000079E7 File Offset: 0x00005BE7
		private static Type GetJsonConverterType(ICustomAttributeProvider attributeProvider)
		{
			return JsonTypeReflector.JsonConverterTypeCache.Get(attributeProvider);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000079F4 File Offset: 0x00005BF4
		private static Type GetJsonConverterTypeFromAttribute(ICustomAttributeProvider attributeProvider)
		{
			JsonConverterAttribute attribute = JsonTypeReflector.GetAttribute<JsonConverterAttribute>(attributeProvider);
			if (attribute == null)
			{
				return null;
			}
			return attribute.ConverterType;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00007A14 File Offset: 0x00005C14
		public static JsonConverter GetJsonConverter(ICustomAttributeProvider attributeProvider, Type targetConvertedType)
		{
			Type jsonConverterType = JsonTypeReflector.GetJsonConverterType(attributeProvider);
			if (jsonConverterType == null)
			{
				return null;
			}
			JsonConverter jsonConverter = JsonConverterAttribute.CreateJsonConverterInstance(jsonConverterType);
			if (!jsonConverter.CanConvert(targetConvertedType))
			{
				throw new JsonSerializationException("JsonConverter {0} on {1} is not compatible with member type {2}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					jsonConverter.GetType().Name,
					attributeProvider,
					targetConvertedType.Name
				}));
			}
			return jsonConverter;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007A78 File Offset: 0x00005C78
		public static TypeConverter GetTypeConverter(Type type)
		{
			Type typeConverterType = JsonTypeReflector.GetTypeConverterType(type);
			if (typeConverterType != null)
			{
				return (TypeConverter)ReflectionUtils.CreateInstance(typeConverterType, new object[0]);
			}
			return null;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00007AA4 File Offset: 0x00005CA4
		private static T GetAttribute<T>(Type type) where T : Attribute
		{
			T attribute = ReflectionUtils.GetAttribute<T>(type, true);
			if (attribute != null)
			{
				return attribute;
			}
			foreach (Type attributeProvider in type.GetInterfaces())
			{
				attribute = ReflectionUtils.GetAttribute<T>(attributeProvider, true);
				if (attribute != null)
				{
					return attribute;
				}
			}
			return default(T);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00007B04 File Offset: 0x00005D04
		private static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
		{
			T attribute = ReflectionUtils.GetAttribute<T>(memberInfo, true);
			if (attribute != null)
			{
				return attribute;
			}
			foreach (Type targetType in memberInfo.DeclaringType.GetInterfaces())
			{
				MemberInfo memberInfoFromType = ReflectionUtils.GetMemberInfoFromType(targetType, memberInfo);
				if (memberInfoFromType != null)
				{
					attribute = ReflectionUtils.GetAttribute<T>(memberInfoFromType, true);
					if (attribute != null)
					{
						return attribute;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00007B74 File Offset: 0x00005D74
		public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T : Attribute
		{
			Type type = attributeProvider as Type;
			if (type != null)
			{
				return JsonTypeReflector.GetAttribute<T>(type);
			}
			MemberInfo memberInfo = attributeProvider as MemberInfo;
			if (memberInfo != null)
			{
				return JsonTypeReflector.GetAttribute<T>(memberInfo);
			}
			return ReflectionUtils.GetAttribute<T>(attributeProvider, true);
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00007BAA File Offset: 0x00005DAA
		public static bool DynamicCodeGeneration
		{
			get
			{
				if (JsonTypeReflector._dynamicCodeGeneration == null)
				{
					JsonTypeReflector._dynamicCodeGeneration = new bool?(false);
				}
				return JsonTypeReflector._dynamicCodeGeneration.Value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00007BCD File Offset: 0x00005DCD
		public static ReflectionDelegateFactory ReflectionDelegateFactory
		{
			get
			{
				return LateBoundReflectionDelegateFactory.Instance;
			}
		}

		// Token: 0x0400008D RID: 141
		public const string IdPropertyName = "$id";

		// Token: 0x0400008E RID: 142
		public const string RefPropertyName = "$ref";

		// Token: 0x0400008F RID: 143
		public const string TypePropertyName = "$type";

		// Token: 0x04000090 RID: 144
		public const string ValuePropertyName = "$value";

		// Token: 0x04000091 RID: 145
		public const string ArrayValuesPropertyName = "$values";

		// Token: 0x04000092 RID: 146
		public const string ShouldSerializePrefix = "ShouldSerialize";

		// Token: 0x04000093 RID: 147
		public const string SpecifiedPostfix = "Specified";

		// Token: 0x04000094 RID: 148
		private static readonly ThreadSafeStore<ICustomAttributeProvider, Type> JsonConverterTypeCache = new ThreadSafeStore<ICustomAttributeProvider, Type>(new Func<ICustomAttributeProvider, Type>(JsonTypeReflector.GetJsonConverterTypeFromAttribute));

		// Token: 0x04000095 RID: 149
		private static readonly ThreadSafeStore<ICustomAttributeProvider, Type> TypeConverterTypeCache = new ThreadSafeStore<ICustomAttributeProvider, Type>(new Func<ICustomAttributeProvider, Type>(JsonTypeReflector.GetTypeConverterTypeFromAttribute));

		// Token: 0x04000096 RID: 150
		private static bool? _dynamicCodeGeneration;
	}
}
