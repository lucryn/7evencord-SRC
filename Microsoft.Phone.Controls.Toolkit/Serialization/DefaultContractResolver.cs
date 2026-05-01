using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008E RID: 142
	public class DefaultContractResolver : IContractResolver
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x00019223 File Offset: 0x00017423
		internal static IContractResolver Instance
		{
			get
			{
				return DefaultContractResolver._instance;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0001922A File Offset: 0x0001742A
		public bool DynamicCodeGeneration
		{
			get
			{
				return JsonTypeReflector.DynamicCodeGeneration;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x00019231 File Offset: 0x00017431
		// (set) Token: 0x060006A9 RID: 1705 RVA: 0x00019239 File Offset: 0x00017439
		public BindingFlags DefaultMembersSearchFlags { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x00019242 File Offset: 0x00017442
		// (set) Token: 0x060006AB RID: 1707 RVA: 0x0001924A File Offset: 0x0001744A
		public bool SerializeCompilerGeneratedMembers { get; set; }

		// Token: 0x060006AC RID: 1708 RVA: 0x00019253 File Offset: 0x00017453
		public DefaultContractResolver() : this(false)
		{
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0001925C File Offset: 0x0001745C
		public DefaultContractResolver(bool shareCache)
		{
			this.DefaultMembersSearchFlags = 20;
			this._sharedCache = shareCache;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00019273 File Offset: 0x00017473
		private Dictionary<ResolverContractKey, JsonContract> GetCache()
		{
			if (this._sharedCache)
			{
				return DefaultContractResolver._sharedContractCache;
			}
			return this._instanceContractCache;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00019289 File Offset: 0x00017489
		private void UpdateCache(Dictionary<ResolverContractKey, JsonContract> cache)
		{
			if (this._sharedCache)
			{
				DefaultContractResolver._sharedContractCache = cache;
				return;
			}
			this._instanceContractCache = cache;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000192A4 File Offset: 0x000174A4
		public virtual JsonContract ResolveContract(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			ResolverContractKey resolverContractKey = new ResolverContractKey(base.GetType(), type);
			Dictionary<ResolverContractKey, JsonContract> cache = this.GetCache();
			JsonContract jsonContract;
			if (cache == null || !cache.TryGetValue(resolverContractKey, ref jsonContract))
			{
				jsonContract = this.CreateContract(type);
				lock (DefaultContractResolver._typeContractCacheLock)
				{
					cache = this.GetCache();
					Dictionary<ResolverContractKey, JsonContract> dictionary = (cache != null) ? new Dictionary<ResolverContractKey, JsonContract>(cache) : new Dictionary<ResolverContractKey, JsonContract>();
					dictionary[resolverContractKey] = jsonContract;
					this.UpdateCache(dictionary);
				}
			}
			return jsonContract;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00019354 File Offset: 0x00017554
		protected virtual List<MemberInfo> GetSerializableMembers(Type objectType)
		{
			DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(objectType);
			List<MemberInfo> list = Enumerable.ToList<MemberInfo>(Enumerable.Where<MemberInfo>(ReflectionUtils.GetFieldsAndProperties(objectType, this.DefaultMembersSearchFlags), (MemberInfo m) => !ReflectionUtils.IsIndexedProperty(m)));
			List<MemberInfo> list2 = Enumerable.ToList<MemberInfo>(Enumerable.Where<MemberInfo>(ReflectionUtils.GetFieldsAndProperties(objectType, 60), (MemberInfo m) => !ReflectionUtils.IsIndexedProperty(m)));
			List<MemberInfo> list3 = new List<MemberInfo>();
			foreach (MemberInfo memberInfo in list2)
			{
				if (this.SerializeCompilerGeneratedMembers || !memberInfo.IsDefined(typeof(CompilerGeneratedAttribute), true))
				{
					if (list.Contains(memberInfo))
					{
						list3.Add(memberInfo);
					}
					else if (JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(memberInfo) != null)
					{
						list3.Add(memberInfo);
					}
					else if (dataContractAttribute != null && JsonTypeReflector.GetAttribute<DataMemberAttribute>(memberInfo) != null)
					{
						list3.Add(memberInfo);
					}
				}
			}
			return list3;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0001947C File Offset: 0x0001767C
		protected virtual JsonObjectContract CreateObjectContract(Type objectType)
		{
			JsonObjectContract jsonObjectContract = new JsonObjectContract(objectType);
			this.InitializeContract(jsonObjectContract);
			jsonObjectContract.MemberSerialization = JsonTypeReflector.GetObjectMemberSerialization(objectType);
			jsonObjectContract.Properties.AddRange(this.CreateProperties(jsonObjectContract.UnderlyingType, jsonObjectContract.MemberSerialization));
			if (Enumerable.Any<ConstructorInfo>(objectType.GetConstructors(52), (ConstructorInfo c) => c.IsDefined(typeof(JsonConstructorAttribute), true)))
			{
				ConstructorInfo attributeConstructor = this.GetAttributeConstructor(objectType);
				if (attributeConstructor != null)
				{
					jsonObjectContract.OverrideConstructor = attributeConstructor;
					jsonObjectContract.ConstructorParameters.AddRange(this.CreateConstructorParameters(attributeConstructor, jsonObjectContract.Properties));
				}
			}
			else if (jsonObjectContract.DefaultCreator == null || jsonObjectContract.DefaultCreatorNonPublic)
			{
				ConstructorInfo parametrizedConstructor = this.GetParametrizedConstructor(objectType);
				if (parametrizedConstructor != null)
				{
					jsonObjectContract.ParametrizedConstructor = parametrizedConstructor;
					jsonObjectContract.ConstructorParameters.AddRange(this.CreateConstructorParameters(parametrizedConstructor, jsonObjectContract.Properties));
				}
			}
			return jsonObjectContract;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00019568 File Offset: 0x00017768
		private ConstructorInfo GetAttributeConstructor(Type objectType)
		{
			IList<ConstructorInfo> list = Enumerable.ToList<ConstructorInfo>(Enumerable.Where<ConstructorInfo>(objectType.GetConstructors(52), (ConstructorInfo c) => c.IsDefined(typeof(JsonConstructorAttribute), true)));
			if (list.Count > 1)
			{
				throw new Exception("Multiple constructors with the JsonConstructorAttribute.");
			}
			if (list.Count == 1)
			{
				return list[0];
			}
			return null;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000195CC File Offset: 0x000177CC
		private ConstructorInfo GetParametrizedConstructor(Type objectType)
		{
			IList<ConstructorInfo> constructors = objectType.GetConstructors(20);
			if (constructors.Count == 1)
			{
				return constructors[0];
			}
			return null;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x000195F4 File Offset: 0x000177F4
		protected virtual IList<JsonProperty> CreateConstructorParameters(ConstructorInfo constructor, JsonPropertyCollection memberProperties)
		{
			ParameterInfo[] parameters = constructor.GetParameters();
			JsonPropertyCollection jsonPropertyCollection = new JsonPropertyCollection(constructor.DeclaringType);
			foreach (ParameterInfo parameterInfo in parameters)
			{
				JsonProperty jsonProperty = memberProperties.GetClosestMatchProperty(parameterInfo.Name);
				if (jsonProperty != null && jsonProperty.PropertyType != parameterInfo.ParameterType)
				{
					jsonProperty = null;
				}
				JsonProperty jsonProperty2 = this.CreatePropertyFromConstructorParameter(jsonProperty, parameterInfo);
				if (jsonProperty2 != null)
				{
					jsonPropertyCollection.AddProperty(jsonProperty2);
				}
			}
			return jsonPropertyCollection;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00019668 File Offset: 0x00017868
		protected virtual JsonProperty CreatePropertyFromConstructorParameter(JsonProperty matchingMemberProperty, ParameterInfo parameterInfo)
		{
			JsonProperty jsonProperty = new JsonProperty();
			jsonProperty.PropertyType = parameterInfo.ParameterType;
			bool flag;
			bool flag2;
			this.SetPropertySettingsFromAttributes(jsonProperty, parameterInfo, parameterInfo.Name, parameterInfo.Member.DeclaringType, MemberSerialization.OptOut, out flag, out flag2);
			jsonProperty.Readable = false;
			jsonProperty.Writable = true;
			if (matchingMemberProperty != null)
			{
				jsonProperty.PropertyName = ((jsonProperty.PropertyName != parameterInfo.Name) ? jsonProperty.PropertyName : matchingMemberProperty.PropertyName);
				jsonProperty.Converter = (jsonProperty.Converter ?? matchingMemberProperty.Converter);
				jsonProperty.MemberConverter = (jsonProperty.MemberConverter ?? matchingMemberProperty.MemberConverter);
				jsonProperty.DefaultValue = (jsonProperty.DefaultValue ?? matchingMemberProperty.DefaultValue);
				jsonProperty.Required = ((jsonProperty.Required != Required.Default) ? jsonProperty.Required : matchingMemberProperty.Required);
				JsonProperty jsonProperty2 = jsonProperty;
				bool? isReference = jsonProperty.IsReference;
				jsonProperty2.IsReference = ((isReference != null) ? new bool?(isReference.GetValueOrDefault()) : matchingMemberProperty.IsReference);
				JsonProperty jsonProperty3 = jsonProperty;
				NullValueHandling? nullValueHandling = jsonProperty.NullValueHandling;
				jsonProperty3.NullValueHandling = ((nullValueHandling != null) ? new NullValueHandling?(nullValueHandling.GetValueOrDefault()) : matchingMemberProperty.NullValueHandling);
				JsonProperty jsonProperty4 = jsonProperty;
				DefaultValueHandling? defaultValueHandling = jsonProperty.DefaultValueHandling;
				jsonProperty4.DefaultValueHandling = ((defaultValueHandling != null) ? new DefaultValueHandling?(defaultValueHandling.GetValueOrDefault()) : matchingMemberProperty.DefaultValueHandling);
				JsonProperty jsonProperty5 = jsonProperty;
				ReferenceLoopHandling? referenceLoopHandling = jsonProperty.ReferenceLoopHandling;
				jsonProperty5.ReferenceLoopHandling = ((referenceLoopHandling != null) ? new ReferenceLoopHandling?(referenceLoopHandling.GetValueOrDefault()) : matchingMemberProperty.ReferenceLoopHandling);
				JsonProperty jsonProperty6 = jsonProperty;
				ObjectCreationHandling? objectCreationHandling = jsonProperty.ObjectCreationHandling;
				jsonProperty6.ObjectCreationHandling = ((objectCreationHandling != null) ? new ObjectCreationHandling?(objectCreationHandling.GetValueOrDefault()) : matchingMemberProperty.ObjectCreationHandling);
				JsonProperty jsonProperty7 = jsonProperty;
				TypeNameHandling? typeNameHandling = jsonProperty.TypeNameHandling;
				jsonProperty7.TypeNameHandling = ((typeNameHandling != null) ? new TypeNameHandling?(typeNameHandling.GetValueOrDefault()) : matchingMemberProperty.TypeNameHandling);
			}
			return jsonProperty;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00019840 File Offset: 0x00017A40
		protected virtual JsonConverter ResolveContractConverter(Type objectType)
		{
			return JsonTypeReflector.GetJsonConverter(objectType, objectType);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00019849 File Offset: 0x00017A49
		private Func<object> GetDefaultCreator(Type createdType)
		{
			return JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(createdType);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00019858 File Offset: 0x00017A58
		private void InitializeContract(JsonContract contract)
		{
			JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(contract.UnderlyingType);
			if (jsonContainerAttribute != null)
			{
				contract.IsReference = jsonContainerAttribute._isReference;
			}
			else
			{
				DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(contract.UnderlyingType);
				if (dataContractAttribute != null && dataContractAttribute.IsReference)
				{
					contract.IsReference = new bool?(true);
				}
			}
			contract.Converter = this.ResolveContractConverter(contract.UnderlyingType);
			contract.InternalConverter = JsonSerializer.GetMatchingConverter(DefaultContractResolver.BuiltInConverters, contract.UnderlyingType);
			if (ReflectionUtils.HasDefaultConstructor(contract.CreatedType, true) || contract.CreatedType.IsValueType)
			{
				contract.DefaultCreator = this.GetDefaultCreator(contract.CreatedType);
				contract.DefaultCreatorNonPublic = (!contract.CreatedType.IsValueType && ReflectionUtils.GetDefaultConstructor(contract.CreatedType) == null);
			}
			this.ResolveCallbackMethods(contract, contract.UnderlyingType);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001992C File Offset: 0x00017B2C
		private void ResolveCallbackMethods(JsonContract contract, Type t)
		{
			if (t.BaseType != null)
			{
				this.ResolveCallbackMethods(contract, t.BaseType);
			}
			MethodInfo methodInfo;
			MethodInfo methodInfo2;
			MethodInfo methodInfo3;
			MethodInfo methodInfo4;
			MethodInfo methodInfo5;
			this.GetCallbackMethodsForType(t, out methodInfo, out methodInfo2, out methodInfo3, out methodInfo4, out methodInfo5);
			if (methodInfo != null)
			{
				contract.OnSerializing = methodInfo;
			}
			if (methodInfo2 != null)
			{
				contract.OnSerialized = methodInfo2;
			}
			if (methodInfo3 != null)
			{
				contract.OnDeserializing = methodInfo3;
			}
			if (methodInfo4 != null)
			{
				contract.OnDeserialized = methodInfo4;
			}
			if (methodInfo5 != null)
			{
				contract.OnError = methodInfo5;
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00019994 File Offset: 0x00017B94
		private void GetCallbackMethodsForType(Type type, out MethodInfo onSerializing, out MethodInfo onSerialized, out MethodInfo onDeserializing, out MethodInfo onDeserialized, out MethodInfo onError)
		{
			onSerializing = null;
			onSerialized = null;
			onDeserializing = null;
			onDeserialized = null;
			onError = null;
			foreach (MethodInfo methodInfo in type.GetMethods(54))
			{
				if (!methodInfo.ContainsGenericParameters)
				{
					Type type2 = null;
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnSerializingAttribute), onSerializing, ref type2))
					{
						onSerializing = methodInfo;
					}
					if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnSerializedAttribute), onSerialized, ref type2))
					{
						onSerialized = methodInfo;
					}
					if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnDeserializingAttribute), onDeserializing, ref type2))
					{
						onDeserializing = methodInfo;
					}
					if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnDeserializedAttribute), onDeserialized, ref type2))
					{
						onDeserialized = methodInfo;
					}
					if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnErrorAttribute), onError, ref type2))
					{
						onError = methodInfo;
					}
				}
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00019A78 File Offset: 0x00017C78
		protected virtual JsonDictionaryContract CreateDictionaryContract(Type objectType)
		{
			JsonDictionaryContract jsonDictionaryContract = new JsonDictionaryContract(objectType);
			this.InitializeContract(jsonDictionaryContract);
			jsonDictionaryContract.PropertyNameResolver = new Func<string, string>(this.ResolvePropertyName);
			return jsonDictionaryContract;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00019AA8 File Offset: 0x00017CA8
		protected virtual JsonArrayContract CreateArrayContract(Type objectType)
		{
			JsonArrayContract jsonArrayContract = new JsonArrayContract(objectType);
			this.InitializeContract(jsonArrayContract);
			return jsonArrayContract;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00019AC4 File Offset: 0x00017CC4
		protected virtual JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
		{
			JsonPrimitiveContract jsonPrimitiveContract = new JsonPrimitiveContract(objectType);
			this.InitializeContract(jsonPrimitiveContract);
			return jsonPrimitiveContract;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00019AE0 File Offset: 0x00017CE0
		protected virtual JsonLinqContract CreateLinqContract(Type objectType)
		{
			JsonLinqContract jsonLinqContract = new JsonLinqContract(objectType);
			this.InitializeContract(jsonLinqContract);
			return jsonLinqContract;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00019AFC File Offset: 0x00017CFC
		protected virtual JsonStringContract CreateStringContract(Type objectType)
		{
			JsonStringContract jsonStringContract = new JsonStringContract(objectType);
			this.InitializeContract(jsonStringContract);
			return jsonStringContract;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00019B18 File Offset: 0x00017D18
		protected virtual JsonContract CreateContract(Type objectType)
		{
			Type type = ReflectionUtils.EnsureNotNullableType(objectType);
			if (JsonConvert.IsJsonPrimitiveType(type))
			{
				return this.CreatePrimitiveContract(type);
			}
			if (JsonTypeReflector.GetJsonObjectAttribute(type) != null)
			{
				return this.CreateObjectContract(type);
			}
			if (JsonTypeReflector.GetJsonArrayAttribute(type) != null)
			{
				return this.CreateArrayContract(type);
			}
			if (type == typeof(JToken) || type.IsSubclassOf(typeof(JToken)))
			{
				return this.CreateLinqContract(type);
			}
			if (CollectionUtils.IsDictionaryType(type))
			{
				return this.CreateDictionaryContract(type);
			}
			if (typeof(IEnumerable).IsAssignableFrom(type))
			{
				return this.CreateArrayContract(type);
			}
			if (DefaultContractResolver.CanConvertToString(type))
			{
				return this.CreateStringContract(type);
			}
			return this.CreateObjectContract(type);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00019BC4 File Offset: 0x00017DC4
		internal static bool CanConvertToString(Type type)
		{
			TypeConverter converter = ConvertUtils.GetConverter(type);
			return (converter != null && converter.GetType() != typeof(TypeConverter) && converter.CanConvertTo(typeof(string))) || (type == typeof(Type) || type.IsSubclassOf(typeof(Type))) || (type == typeof(Guid) || type == typeof(Uri) || type == typeof(TimeSpan));
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00019C4C File Offset: 0x00017E4C
		private static bool IsValidCallback(MethodInfo method, ParameterInfo[] parameters, Type attributeType, MethodInfo currentCallback, ref Type prevAttributeType)
		{
			if (!method.IsDefined(attributeType, false))
			{
				return false;
			}
			if (currentCallback != null)
			{
				throw new Exception("Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					method,
					currentCallback,
					DefaultContractResolver.GetClrTypeFullName(method.DeclaringType),
					attributeType
				}));
			}
			if (prevAttributeType != null)
			{
				throw new Exception("Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					prevAttributeType,
					attributeType,
					DefaultContractResolver.GetClrTypeFullName(method.DeclaringType),
					method
				}));
			}
			if (method.IsVirtual)
			{
				throw new Exception("Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					method,
					DefaultContractResolver.GetClrTypeFullName(method.DeclaringType),
					attributeType
				}));
			}
			if (method.ReturnType != typeof(void))
			{
				throw new Exception("Serialization Callback '{1}' in type '{0}' must return void.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					DefaultContractResolver.GetClrTypeFullName(method.DeclaringType),
					method
				}));
			}
			if (attributeType == typeof(OnErrorAttribute))
			{
				if (parameters == null || parameters.Length != 2 || parameters[0].ParameterType != typeof(StreamingContext) || parameters[1].ParameterType != typeof(ErrorContext))
				{
					throw new Exception("Serialization Error Callback '{1}' in type '{0}' must have two parameters of type '{2}' and '{3}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						DefaultContractResolver.GetClrTypeFullName(method.DeclaringType),
						method,
						typeof(StreamingContext),
						typeof(ErrorContext)
					}));
				}
			}
			else if (parameters == null || parameters.Length != 1 || parameters[0].ParameterType != typeof(StreamingContext))
			{
				throw new Exception("Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					DefaultContractResolver.GetClrTypeFullName(method.DeclaringType),
					method,
					typeof(StreamingContext)
				}));
			}
			prevAttributeType = attributeType;
			return true;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00019E50 File Offset: 0x00018050
		internal static string GetClrTypeFullName(Type type)
		{
			if (type.IsGenericTypeDefinition || !type.ContainsGenericParameters)
			{
				return type.FullName;
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[]
			{
				type.Namespace,
				type.Name
			});
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00019EC8 File Offset: 0x000180C8
		protected virtual IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			List<MemberInfo> serializableMembers = this.GetSerializableMembers(type);
			if (serializableMembers == null)
			{
				throw new JsonSerializationException("Null collection of seralizable members returned.");
			}
			JsonPropertyCollection jsonPropertyCollection = new JsonPropertyCollection(type);
			foreach (MemberInfo member in serializableMembers)
			{
				JsonProperty jsonProperty = this.CreateProperty(member, memberSerialization);
				if (jsonProperty != null)
				{
					jsonPropertyCollection.AddProperty(jsonProperty);
				}
			}
			return Enumerable.ToList<JsonProperty>(Enumerable.OrderBy<JsonProperty, int>(jsonPropertyCollection, delegate(JsonProperty p)
			{
				int? order = p.Order;
				if (order == null)
				{
					return -1;
				}
				return order.GetValueOrDefault();
			}));
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00019F6C File Offset: 0x0001816C
		protected virtual IValueProvider CreateMemberValueProvider(MemberInfo member)
		{
			return new ReflectionValueProvider(member);
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00019F74 File Offset: 0x00018174
		protected virtual JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			JsonProperty jsonProperty = new JsonProperty();
			jsonProperty.PropertyType = ReflectionUtils.GetMemberUnderlyingType(member);
			jsonProperty.ValueProvider = this.CreateMemberValueProvider(member);
			bool flag;
			bool canSetReadOnly;
			this.SetPropertySettingsFromAttributes(jsonProperty, member, member.Name, member.DeclaringType, memberSerialization, out flag, out canSetReadOnly);
			jsonProperty.Readable = ReflectionUtils.CanReadMemberValue(member, flag);
			jsonProperty.Writable = ReflectionUtils.CanSetMemberValue(member, flag, canSetReadOnly);
			jsonProperty.ShouldSerialize = this.CreateShouldSerializeTest(member);
			this.SetIsSpecifiedActions(jsonProperty, member, flag);
			return jsonProperty;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00019FEC File Offset: 0x000181EC
		private void SetPropertySettingsFromAttributes(JsonProperty property, ICustomAttributeProvider attributeProvider, string name, Type declaringType, MemberSerialization memberSerialization, out bool allowNonPublicAccess, out bool hasExplicitAttribute)
		{
			hasExplicitAttribute = false;
			DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(declaringType);
			DataMemberAttribute dataMemberAttribute;
			if (dataContractAttribute != null && attributeProvider is MemberInfo)
			{
				dataMemberAttribute = JsonTypeReflector.GetDataMemberAttribute((MemberInfo)attributeProvider);
			}
			else
			{
				dataMemberAttribute = null;
			}
			JsonPropertyAttribute attribute = JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(attributeProvider);
			if (attribute != null)
			{
				hasExplicitAttribute = true;
			}
			bool flag = JsonTypeReflector.GetAttribute<JsonIgnoreAttribute>(attributeProvider) != null;
			string propertyName;
			if (attribute != null && attribute.PropertyName != null)
			{
				propertyName = attribute.PropertyName;
			}
			else if (dataMemberAttribute != null && dataMemberAttribute.Name != null)
			{
				propertyName = dataMemberAttribute.Name;
			}
			else
			{
				propertyName = name;
			}
			property.PropertyName = this.ResolvePropertyName(propertyName);
			property.UnderlyingName = name;
			if (attribute != null)
			{
				property.Required = attribute.Required;
				property.Order = attribute._order;
			}
			else if (dataMemberAttribute != null)
			{
				property.Required = (dataMemberAttribute.IsRequired ? Required.AllowNull : Required.Default);
				property.Order = ((dataMemberAttribute.Order != -1) ? new int?(dataMemberAttribute.Order) : default(int?));
			}
			else
			{
				property.Required = Required.Default;
			}
			property.Ignored = (flag || (memberSerialization == MemberSerialization.OptIn && attribute == null && dataMemberAttribute == null));
			property.Converter = JsonTypeReflector.GetJsonConverter(attributeProvider, property.PropertyType);
			property.MemberConverter = JsonTypeReflector.GetJsonConverter(attributeProvider, property.PropertyType);
			DefaultValueAttribute attribute2 = JsonTypeReflector.GetAttribute<DefaultValueAttribute>(attributeProvider);
			property.DefaultValue = ((attribute2 != null) ? attribute2.Value : null);
			property.NullValueHandling = ((attribute != null) ? attribute._nullValueHandling : default(NullValueHandling?));
			property.DefaultValueHandling = ((attribute != null) ? attribute._defaultValueHandling : default(DefaultValueHandling?));
			property.ReferenceLoopHandling = ((attribute != null) ? attribute._referenceLoopHandling : default(ReferenceLoopHandling?));
			property.ObjectCreationHandling = ((attribute != null) ? attribute._objectCreationHandling : default(ObjectCreationHandling?));
			property.TypeNameHandling = ((attribute != null) ? attribute._typeNameHandling : default(TypeNameHandling?));
			property.IsReference = ((attribute != null) ? attribute._isReference : default(bool?));
			allowNonPublicAccess = false;
			if ((this.DefaultMembersSearchFlags & 32) == 32)
			{
				allowNonPublicAccess = true;
			}
			if (attribute != null)
			{
				allowNonPublicAccess = true;
			}
			if (dataMemberAttribute != null)
			{
				allowNonPublicAccess = true;
				hasExplicitAttribute = true;
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001A224 File Offset: 0x00018424
		private Predicate<object> CreateShouldSerializeTest(MemberInfo member)
		{
			MethodInfo method = member.DeclaringType.GetMethod("ShouldSerialize" + member.Name, new Type[0]);
			if (method == null || method.ReturnType != typeof(bool))
			{
				return null;
			}
			MethodCall<object, object> shouldSerializeCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return (object o) => (bool)shouldSerializeCall(o, new object[0]);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001A2A8 File Offset: 0x000184A8
		private void SetIsSpecifiedActions(JsonProperty property, MemberInfo member, bool allowNonPublicAccess)
		{
			MemberInfo memberInfo = member.DeclaringType.GetProperty(member.Name + "Specified");
			if (memberInfo == null)
			{
				memberInfo = member.DeclaringType.GetField(member.Name + "Specified");
			}
			if (memberInfo == null || ReflectionUtils.GetMemberUnderlyingType(memberInfo) != typeof(bool))
			{
				return;
			}
			Func<object, object> specifiedPropertyGet = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(memberInfo);
			property.GetIsSpecified = ((object o) => (bool)specifiedPropertyGet.Invoke(o));
			if (ReflectionUtils.CanSetMemberValue(memberInfo, allowNonPublicAccess, false))
			{
				property.SetIsSpecified = JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(memberInfo);
			}
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001A34A File Offset: 0x0001854A
		protected internal virtual string ResolvePropertyName(string propertyName)
		{
			return propertyName;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001A350 File Offset: 0x00018550
		// Note: this type is marked as 'beforefieldinit'.
		static DefaultContractResolver()
		{
			List<JsonConverter> list = new List<JsonConverter>();
			list.Add(new BinaryConverter());
			list.Add(new KeyValuePairConverter());
			list.Add(new XmlNodeConverter());
			list.Add(new BsonObjectIdConverter());
			DefaultContractResolver.BuiltInConverters = list;
			DefaultContractResolver._typeContractCacheLock = new object();
		}

		// Token: 0x040001D2 RID: 466
		private static readonly IContractResolver _instance = new DefaultContractResolver(true);

		// Token: 0x040001D3 RID: 467
		private static readonly IList<JsonConverter> BuiltInConverters;

		// Token: 0x040001D4 RID: 468
		private static Dictionary<ResolverContractKey, JsonContract> _sharedContractCache;

		// Token: 0x040001D5 RID: 469
		private static readonly object _typeContractCacheLock;

		// Token: 0x040001D6 RID: 470
		private Dictionary<ResolverContractKey, JsonContract> _instanceContractCache;

		// Token: 0x040001D7 RID: 471
		private readonly bool _sharedCache;
	}
}
