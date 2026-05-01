using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200000B RID: 11
	internal static class ReflectionUtils
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000041B8 File Offset: 0x000023B8
		public static bool IsVirtual(this PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			MethodInfo methodInfo = propertyInfo.GetGetMethod();
			if (methodInfo != null && methodInfo.IsVirtual)
			{
				return true;
			}
			methodInfo = propertyInfo.GetSetMethod();
			return methodInfo != null && methodInfo.IsVirtual;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000041F9 File Offset: 0x000023F9
		public static Type GetObjectType(object v)
		{
			if (v == null)
			{
				return null;
			}
			return v.GetType();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00004206 File Offset: 0x00002406
		public static string GetTypeName(Type t, FormatterAssemblyStyle assemblyFormat)
		{
			return ReflectionUtils.GetTypeName(t, assemblyFormat, null);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004210 File Offset: 0x00002410
		public static string GetTypeName(Type t, FormatterAssemblyStyle assemblyFormat, SerializationBinder binder)
		{
			string fullyQualifiedTypeName;
			if (binder != null)
			{
				string text;
				string text2;
				binder.BindToName(t, out text, out text2);
				fullyQualifiedTypeName = text2 + ((text == null) ? "" : (", " + text));
			}
			else
			{
				fullyQualifiedTypeName = t.AssemblyQualifiedName;
			}
			switch (assemblyFormat)
			{
			case FormatterAssemblyStyle.Simple:
				return ReflectionUtils.RemoveAssemblyDetails(fullyQualifiedTypeName);
			case FormatterAssemblyStyle.Full:
				return t.AssemblyQualifiedName;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004278 File Offset: 0x00002478
		private static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
			{
				char c = fullyQualifiedTypeName.get_Chars(i);
				char c2 = c;
				if (c2 != ',')
				{
					switch (c2)
					{
					case '[':
						flag = false;
						flag2 = false;
						stringBuilder.Append(c);
						goto IL_77;
					case ']':
						flag = false;
						flag2 = false;
						stringBuilder.Append(c);
						goto IL_77;
					}
					if (!flag2)
					{
						stringBuilder.Append(c);
					}
				}
				else if (!flag)
				{
					flag = true;
					stringBuilder.Append(c);
				}
				else
				{
					flag2 = true;
				}
				IL_77:;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004310 File Offset: 0x00002510
		public static bool IsInstantiatableType(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return !t.IsAbstract && !t.IsInterface && !t.IsArray && !t.IsGenericTypeDefinition && t != typeof(void) && ReflectionUtils.HasDefaultConstructor(t);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004362 File Offset: 0x00002562
		public static bool HasDefaultConstructor(Type t)
		{
			return ReflectionUtils.HasDefaultConstructor(t, false);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000436B File Offset: 0x0000256B
		public static bool HasDefaultConstructor(Type t, bool nonPublic)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return t.IsValueType || ReflectionUtils.GetDefaultConstructor(t, nonPublic) != null;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000438F File Offset: 0x0000258F
		public static ConstructorInfo GetDefaultConstructor(Type t)
		{
			return ReflectionUtils.GetDefaultConstructor(t, false);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004398 File Offset: 0x00002598
		public static ConstructorInfo GetDefaultConstructor(Type t, bool nonPublic)
		{
			BindingFlags bindingFlags = 16;
			if (nonPublic)
			{
				bindingFlags |= 32;
			}
			return t.GetConstructor(bindingFlags | 4, null, new Type[0], null);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000043C1 File Offset: 0x000025C1
		public static bool IsNullable(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return !t.IsValueType || ReflectionUtils.IsNullableType(t);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000043DE File Offset: 0x000025DE
		public static bool IsNullableType(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004407 File Offset: 0x00002607
		public static Type EnsureNotNullableType(Type t)
		{
			if (!ReflectionUtils.IsNullableType(t))
			{
				return t;
			}
			return Nullable.GetUnderlyingType(t);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000441C File Offset: 0x0000261C
		public static bool IsUnitializedValue(object value)
		{
			if (value == null)
			{
				return true;
			}
			object obj = ReflectionUtils.CreateUnitializedValue(value.GetType());
			return value.Equals(obj);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004444 File Offset: 0x00002644
		public static object CreateUnitializedValue(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsGenericTypeDefinition)
			{
				throw new ArgumentException("Type {0} is a generic type definition and cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type
				}), "type");
			}
			if (type.IsClass || type.IsInterface || type == typeof(void))
			{
				return null;
			}
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			throw new ArgumentException("Type {0} cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				type
			}), "type");
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000044DD File Offset: 0x000026DD
		public static bool IsPropertyIndexed(PropertyInfo property)
		{
			ValidationUtils.ArgumentNotNull(property, "property");
			return !CollectionUtils.IsNullOrEmpty<ParameterInfo>(property.GetIndexParameters());
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000044F8 File Offset: 0x000026F8
		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition)
		{
			Type type2;
			return ReflectionUtils.ImplementsGenericDefinition(type, genericInterfaceDefinition, out type2);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004510 File Offset: 0x00002710
		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition, out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericInterfaceDefinition, "genericInterfaceDefinition");
			if (!genericInterfaceDefinition.IsInterface || !genericInterfaceDefinition.IsGenericTypeDefinition)
			{
				throw new ArgumentNullException("'{0}' is not a generic interface definition.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					genericInterfaceDefinition
				}));
			}
			if (type.IsInterface && type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericInterfaceDefinition == genericTypeDefinition)
				{
					implementingType = type;
					return true;
				}
			}
			foreach (Type type2 in type.GetInterfaces())
			{
				if (type2.IsGenericType)
				{
					Type genericTypeDefinition2 = type2.GetGenericTypeDefinition();
					if (genericInterfaceDefinition == genericTypeDefinition2)
					{
						implementingType = type2;
						return true;
					}
				}
			}
			implementingType = null;
			return false;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000045C8 File Offset: 0x000027C8
		public static bool AssignableToTypeName(this Type type, string fullTypeName, out Type match)
		{
			for (Type type2 = type; type2 != null; type2 = type2.BaseType)
			{
				if (string.Equals(type2.FullName, fullTypeName, 4))
				{
					match = type2;
					return true;
				}
			}
			foreach (Type type3 in type.GetInterfaces())
			{
				if (string.Equals(type3.Name, fullTypeName, 4))
				{
					match = type;
					return true;
				}
			}
			match = null;
			return false;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004634 File Offset: 0x00002834
		public static bool AssignableToTypeName(this Type type, string fullTypeName)
		{
			Type type2;
			return type.AssignableToTypeName(fullTypeName, out type2);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000464C File Offset: 0x0000284C
		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition)
		{
			Type type2;
			return ReflectionUtils.InheritsGenericDefinition(type, genericClassDefinition, out type2);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004664 File Offset: 0x00002864
		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition, out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericClassDefinition, "genericClassDefinition");
			if (!genericClassDefinition.IsClass || !genericClassDefinition.IsGenericTypeDefinition)
			{
				throw new ArgumentNullException("'{0}' is not a generic class definition.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					genericClassDefinition
				}));
			}
			return ReflectionUtils.InheritsGenericDefinitionInternal(type, genericClassDefinition, out implementingType);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000046C0 File Offset: 0x000028C0
		private static bool InheritsGenericDefinitionInternal(Type currentType, Type genericClassDefinition, out Type implementingType)
		{
			if (currentType.IsGenericType)
			{
				Type genericTypeDefinition = currentType.GetGenericTypeDefinition();
				if (genericClassDefinition == genericTypeDefinition)
				{
					implementingType = currentType;
					return true;
				}
			}
			if (currentType.BaseType == null)
			{
				implementingType = null;
				return false;
			}
			return ReflectionUtils.InheritsGenericDefinitionInternal(currentType.BaseType, genericClassDefinition, out implementingType);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004700 File Offset: 0x00002900
		public static Type GetCollectionItemType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsArray)
			{
				return type.GetElementType();
			}
			Type type2;
			if (ReflectionUtils.ImplementsGenericDefinition(type, typeof(IEnumerable), out type2))
			{
				if (type2.IsGenericTypeDefinition)
				{
					throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						type
					}));
				}
				return type2.GetGenericArguments()[0];
			}
			else
			{
				if (typeof(IEnumerable).IsAssignableFrom(type))
				{
					return null;
				}
				throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type
				}));
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000047A4 File Offset: 0x000029A4
		public static void GetDictionaryKeyValueTypes(Type dictionaryType, out Type keyType, out Type valueType)
		{
			ValidationUtils.ArgumentNotNull(dictionaryType, "type");
			Type type;
			if (ReflectionUtils.ImplementsGenericDefinition(dictionaryType, typeof(IDictionary), out type))
			{
				if (type.IsGenericTypeDefinition)
				{
					throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						dictionaryType
					}));
				}
				Type[] genericArguments = type.GetGenericArguments();
				keyType = genericArguments[0];
				valueType = genericArguments[1];
				return;
			}
			else
			{
				if (typeof(IDictionary).IsAssignableFrom(dictionaryType))
				{
					keyType = null;
					valueType = null;
					return;
				}
				throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					dictionaryType
				}));
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004844 File Offset: 0x00002A44
		public static Type GetDictionaryValueType(Type dictionaryType)
		{
			Type type;
			Type result;
			ReflectionUtils.GetDictionaryKeyValueTypes(dictionaryType, out type, out result);
			return result;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000485C File Offset: 0x00002A5C
		public static Type GetDictionaryKeyType(Type dictionaryType)
		{
			Type result;
			Type type;
			ReflectionUtils.GetDictionaryKeyValueTypes(dictionaryType, out result, out type);
			return result;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004874 File Offset: 0x00002A74
		public static bool ItemsUnitializedValue<T>(IList<T> list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			Type collectionItemType = ReflectionUtils.GetCollectionItemType(list.GetType());
			if (collectionItemType.IsValueType)
			{
				object obj = ReflectionUtils.CreateUnitializedValue(collectionItemType);
				for (int i = 0; i < list.Count; i++)
				{
					T t = list[i];
					if (!t.Equals(obj))
					{
						return false;
					}
				}
			}
			else
			{
				if (!collectionItemType.IsClass)
				{
					throw new Exception("Type {0} is neither a ValueType or a Class.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						collectionItemType
					}));
				}
				for (int j = 0; j < list.Count; j++)
				{
					object obj2 = list[j];
					if (obj2 != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000492C File Offset: 0x00002B2C
		public static Type GetMemberUnderlyingType(MemberInfo member)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			MemberTypes memberType = member.MemberType;
			switch (memberType)
			{
			case 2:
				return ((EventInfo)member).EventHandlerType;
			case 3:
				break;
			case 4:
				return ((FieldInfo)member).FieldType;
			default:
				if (memberType == 16)
				{
					return ((PropertyInfo)member).PropertyType;
				}
				break;
			}
			throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo or EventInfo", "member");
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000499C File Offset: 0x00002B9C
		public static bool IsIndexedProperty(MemberInfo member)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			PropertyInfo propertyInfo = member as PropertyInfo;
			return propertyInfo != null && ReflectionUtils.IsIndexedProperty(propertyInfo);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000049C6 File Offset: 0x00002BC6
		public static bool IsIndexedProperty(PropertyInfo property)
		{
			ValidationUtils.ArgumentNotNull(property, "property");
			return property.GetIndexParameters().Length > 0;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000049E0 File Offset: 0x00002BE0
		public static object GetMemberValue(MemberInfo member, object target)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			MemberTypes memberType = member.MemberType;
			if (memberType != 4)
			{
				if (memberType == 16)
				{
					try
					{
						return ((PropertyInfo)member).GetValue(target, null);
					}
					catch (TargetParameterCountException ex)
					{
						throw new ArgumentException("MemberInfo '{0}' has index parameters".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							member.Name
						}), ex);
					}
				}
				throw new ArgumentException("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					CultureInfo.InvariantCulture,
					member.Name
				}), "member");
			}
			return ((FieldInfo)member).GetValue(target);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004AA4 File Offset: 0x00002CA4
		public static void SetMemberValue(MemberInfo member, object target, object value)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			MemberTypes memberType = member.MemberType;
			if (memberType == 4)
			{
				((FieldInfo)member).SetValue(target, value);
				return;
			}
			if (memberType != 16)
			{
				throw new ArgumentException("MemberInfo '{0}' must be of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					member.Name
				}), "member");
			}
			((PropertyInfo)member).SetValue(target, value, null);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004B20 File Offset: 0x00002D20
		public static bool CanReadMemberValue(MemberInfo member, bool nonPublic)
		{
			MemberTypes memberType = member.MemberType;
			if (memberType == 4)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				return nonPublic || fieldInfo.IsPublic;
			}
			if (memberType != 16)
			{
				return false;
			}
			PropertyInfo propertyInfo = (PropertyInfo)member;
			return propertyInfo.CanRead && (nonPublic || propertyInfo.GetGetMethod(nonPublic) != null);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004B7C File Offset: 0x00002D7C
		public static bool CanSetMemberValue(MemberInfo member, bool nonPublic, bool canSetReadOnly)
		{
			MemberTypes memberType = member.MemberType;
			if (memberType == 4)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				return (!fieldInfo.IsInitOnly || canSetReadOnly) && (nonPublic || fieldInfo.IsPublic);
			}
			if (memberType != 16)
			{
				return false;
			}
			PropertyInfo propertyInfo = (PropertyInfo)member;
			return propertyInfo.CanWrite && (nonPublic || propertyInfo.GetSetMethod(nonPublic) != null);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004BE5 File Offset: 0x00002DE5
		public static List<MemberInfo> GetFieldsAndProperties<T>(BindingFlags bindingAttr)
		{
			return ReflectionUtils.GetFieldsAndProperties(typeof(T), bindingAttr);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004D58 File Offset: 0x00002F58
		public static List<MemberInfo> GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
		{
			List<MemberInfo> list = new List<MemberInfo>();
			list.AddRange(ReflectionUtils.GetFields(type, bindingAttr));
			list.AddRange(ReflectionUtils.GetProperties(type, bindingAttr));
			List<MemberInfo> list2 = new List<MemberInfo>(list.Count);
			var enumerable = Enumerable.Select(Enumerable.GroupBy<MemberInfo, string>(list, (MemberInfo m) => m.Name), (IGrouping<string, MemberInfo> g) => new
			{
				Count = Enumerable.Count<MemberInfo>(g),
				Members = Enumerable.Cast<MemberInfo>(g)
			});
			foreach (var <>f__AnonymousType in enumerable)
			{
				if (<>f__AnonymousType.Count == 1)
				{
					list2.Add(Enumerable.First<MemberInfo>(<>f__AnonymousType.Members));
				}
				else
				{
					IEnumerable<MemberInfo> enumerable2 = Enumerable.Where<MemberInfo>(<>f__AnonymousType.Members, (MemberInfo m) => !ReflectionUtils.IsOverridenGenericMember(m, bindingAttr) || m.Name == "Item");
					list2.AddRange(enumerable2);
				}
			}
			return list2;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004E78 File Offset: 0x00003078
		private static bool IsOverridenGenericMember(MemberInfo memberInfo, BindingFlags bindingAttr)
		{
			if (memberInfo.MemberType != 4 && memberInfo.MemberType != 16)
			{
				throw new ArgumentException("Member must be a field or property.");
			}
			Type declaringType = memberInfo.DeclaringType;
			if (!declaringType.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = declaringType.GetGenericTypeDefinition();
			if (genericTypeDefinition == null)
			{
				return false;
			}
			MemberInfo[] member = genericTypeDefinition.GetMember(memberInfo.Name, bindingAttr);
			if (member.Length == 0)
			{
				return false;
			}
			Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(member[0]);
			return memberUnderlyingType.IsGenericParameter;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004EE9 File Offset: 0x000030E9
		public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T : Attribute
		{
			return ReflectionUtils.GetAttribute<T>(attributeProvider, true);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004EF4 File Offset: 0x000030F4
		public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
		{
			T[] attributes = ReflectionUtils.GetAttributes<T>(attributeProvider, inherit);
			return CollectionUtils.GetSingleItem<T>(attributes, true);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004F10 File Offset: 0x00003110
		public static T[] GetAttributes<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
		{
			ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			if (attributeProvider is Type)
			{
				return (T[])((Type)attributeProvider).GetCustomAttributes(typeof(T), inherit);
			}
			if (attributeProvider is Assembly)
			{
				return (T[])Attribute.GetCustomAttributes((Assembly)attributeProvider, typeof(T), inherit);
			}
			if (attributeProvider is MemberInfo)
			{
				return (T[])Attribute.GetCustomAttributes((MemberInfo)attributeProvider, typeof(T), inherit);
			}
			if (attributeProvider is Module)
			{
				return (T[])Attribute.GetCustomAttributes((Module)attributeProvider, typeof(T), inherit);
			}
			if (attributeProvider is ParameterInfo)
			{
				return (T[])Attribute.GetCustomAttributes((ParameterInfo)attributeProvider, typeof(T), inherit);
			}
			return (T[])attributeProvider.GetCustomAttributes(typeof(T), inherit);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004FF2 File Offset: 0x000031F2
		public static string GetNameAndAssessmblyName(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return t.FullName + ", " + t.Assembly.GetName().Name;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00005020 File Offset: 0x00003220
		public static Type MakeGenericType(Type genericTypeDefinition, params Type[] innerTypes)
		{
			ValidationUtils.ArgumentNotNull(genericTypeDefinition, "genericTypeDefinition");
			ValidationUtils.ArgumentNotNullOrEmpty<Type>(innerTypes, "innerTypes");
			ValidationUtils.ArgumentConditionTrue(genericTypeDefinition.IsGenericTypeDefinition, "genericTypeDefinition", "Type {0} is not a generic type definition.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				genericTypeDefinition
			}));
			return genericTypeDefinition.MakeGenericType(innerTypes);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00005078 File Offset: 0x00003278
		public static object CreateGeneric(Type genericTypeDefinition, Type innerType, params object[] args)
		{
			return ReflectionUtils.CreateGeneric(genericTypeDefinition, new Type[]
			{
				innerType
			}, args);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000050A6 File Offset: 0x000032A6
		public static object CreateGeneric(Type genericTypeDefinition, IList<Type> innerTypes, params object[] args)
		{
			return ReflectionUtils.CreateGeneric(genericTypeDefinition, innerTypes, (Type t, IList<object> a) => ReflectionUtils.CreateInstance(t, Enumerable.ToArray<object>(a)), args);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000050D0 File Offset: 0x000032D0
		public static object CreateGeneric(Type genericTypeDefinition, IList<Type> innerTypes, Func<Type, IList<object>, object> instanceCreator, params object[] args)
		{
			ValidationUtils.ArgumentNotNull(genericTypeDefinition, "genericTypeDefinition");
			ValidationUtils.ArgumentNotNullOrEmpty<Type>(innerTypes, "innerTypes");
			ValidationUtils.ArgumentNotNull(instanceCreator, "createInstance");
			Type type = ReflectionUtils.MakeGenericType(genericTypeDefinition, Enumerable.ToArray<Type>(innerTypes));
			return instanceCreator.Invoke(type, args);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00005113 File Offset: 0x00003313
		public static bool IsCompatibleValue(object value, Type type)
		{
			if (value == null)
			{
				return ReflectionUtils.IsNullable(type);
			}
			return type.IsAssignableFrom(value.GetType());
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00005130 File Offset: 0x00003330
		public static object CreateInstance(Type type, params object[] args)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			return Activator.CreateInstance(type, args);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005144 File Offset: 0x00003344
		public static void SplitFullyQualifiedTypeName(string fullyQualifiedTypeName, out string typeName, out string assemblyName)
		{
			int? assemblyDelimiterIndex = ReflectionUtils.GetAssemblyDelimiterIndex(fullyQualifiedTypeName);
			if (assemblyDelimiterIndex != null)
			{
				typeName = fullyQualifiedTypeName.Substring(0, assemblyDelimiterIndex.Value).Trim();
				assemblyName = fullyQualifiedTypeName.Substring(assemblyDelimiterIndex.Value + 1, fullyQualifiedTypeName.Length - assemblyDelimiterIndex.Value - 1).Trim();
				return;
			}
			typeName = fullyQualifiedTypeName;
			assemblyName = null;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000051A4 File Offset: 0x000033A4
		private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
		{
			int num = 0;
			for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
			{
				char c = fullyQualifiedTypeName.get_Chars(i);
				char c2 = c;
				if (c2 != ',')
				{
					switch (c2)
					{
					case '[':
						num++;
						break;
					case ']':
						num--;
						break;
					}
				}
				else if (num == 0)
				{
					return new int?(i);
				}
			}
			return default(int?);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005214 File Offset: 0x00003414
		public static MemberInfo GetMemberInfoFromType(Type targetType, MemberInfo memberInfo)
		{
			BindingFlags bindingFlags = 60;
			MemberTypes memberType = memberInfo.MemberType;
			if (memberType == 16)
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				Type[] array = Enumerable.ToArray<Type>(Enumerable.Select<ParameterInfo, Type>(propertyInfo.GetIndexParameters(), (ParameterInfo p) => p.ParameterType));
				return targetType.GetProperty(propertyInfo.Name, bindingFlags, null, propertyInfo.PropertyType, array, null);
			}
			return Enumerable.SingleOrDefault<MemberInfo>(targetType.GetMember(memberInfo.Name, memberInfo.MemberType, bindingFlags));
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005294 File Offset: 0x00003494
		public static IEnumerable<FieldInfo> GetFields(Type targetType, BindingFlags bindingAttr)
		{
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			List<MemberInfo> list = new List<MemberInfo>(targetType.GetFields(bindingAttr));
			ReflectionUtils.GetChildPrivateFields(list, targetType, bindingAttr);
			return Enumerable.Cast<FieldInfo>(list);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000052D0 File Offset: 0x000034D0
		private static void GetChildPrivateFields(IList<MemberInfo> initialFields, Type targetType, BindingFlags bindingAttr)
		{
			if ((bindingAttr & 32) != null)
			{
				BindingFlags bindingFlags = bindingAttr.RemoveFlag(16);
				while ((targetType = targetType.BaseType) != null)
				{
					IEnumerable<MemberInfo> collection = Enumerable.Cast<MemberInfo>(Enumerable.Where<FieldInfo>(targetType.GetFields(bindingFlags), (FieldInfo f) => f.IsPrivate));
					initialFields.AddRange(collection);
				}
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005330 File Offset: 0x00003530
		public static IEnumerable<PropertyInfo> GetProperties(Type targetType, BindingFlags bindingAttr)
		{
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			List<PropertyInfo> list = new List<PropertyInfo>(targetType.GetProperties(bindingAttr));
			ReflectionUtils.GetChildPrivateProperties(list, targetType, bindingAttr);
			for (int i = 0; i < list.Count; i++)
			{
				PropertyInfo propertyInfo = list[i];
				if (propertyInfo.DeclaringType != targetType)
				{
					PropertyInfo propertyInfo2 = (PropertyInfo)ReflectionUtils.GetMemberInfoFromType(propertyInfo.DeclaringType, propertyInfo);
					list[i] = propertyInfo2;
				}
			}
			return list;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000539A File Offset: 0x0000359A
		public static BindingFlags RemoveFlag(this BindingFlags bindingAttr, BindingFlags flag)
		{
			if ((bindingAttr & flag) != flag)
			{
				return bindingAttr;
			}
			return bindingAttr ^ flag;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000053C8 File Offset: 0x000035C8
		private static void GetChildPrivateProperties(IList<PropertyInfo> initialProperties, Type targetType, BindingFlags bindingAttr)
		{
			if ((bindingAttr & 32) != null)
			{
				BindingFlags bindingFlags = bindingAttr.RemoveFlag(16);
				while ((targetType = targetType.BaseType) != null)
				{
					PropertyInfo[] properties = targetType.GetProperties(bindingFlags);
					for (int i = 0; i < properties.Length; i++)
					{
						PropertyInfo nonPublicProperty2 = properties[i];
						PropertyInfo nonPublicProperty = nonPublicProperty2;
						int num = initialProperties.IndexOf((PropertyInfo p) => p.Name == nonPublicProperty.Name);
						if (num == -1)
						{
							initialProperties.Add(nonPublicProperty);
						}
						else
						{
							initialProperties[num] = nonPublicProperty;
						}
					}
				}
			}
		}
	}
}
