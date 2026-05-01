using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008D RID: 141
	public class JsonDictionaryContract : JsonContract
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x00019036 File Offset: 0x00017236
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x0001903E File Offset: 0x0001723E
		public Func<string, string> PropertyNameResolver { get; set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x00019047 File Offset: 0x00017247
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x0001904F File Offset: 0x0001724F
		internal Type DictionaryKeyType { get; private set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00019058 File Offset: 0x00017258
		// (set) Token: 0x060006A2 RID: 1698 RVA: 0x00019060 File Offset: 0x00017260
		internal Type DictionaryValueType { get; private set; }

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001906C File Offset: 0x0001726C
		public JsonDictionaryContract(Type underlyingType) : base(underlyingType)
		{
			Type type;
			Type type2;
			if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IDictionary), out this._genericCollectionDefinitionType))
			{
				type = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				type2 = this._genericCollectionDefinitionType.GetGenericArguments()[1];
			}
			else
			{
				ReflectionUtils.GetDictionaryKeyValueTypes(base.UnderlyingType, out type, out type2);
			}
			this.DictionaryKeyType = type;
			this.DictionaryValueType = type2;
			if (this.DictionaryValueType != null)
			{
				this._isDictionaryValueTypeNullableType = ReflectionUtils.IsNullableType(this.DictionaryValueType);
			}
			if (this.IsTypeGenericDictionaryInterface(base.UnderlyingType))
			{
				base.CreatedType = ReflectionUtils.MakeGenericType(typeof(Dictionary), new Type[]
				{
					type,
					type2
				});
				return;
			}
			if (base.UnderlyingType == typeof(IDictionary))
			{
				base.CreatedType = typeof(Dictionary<object, object>);
			}
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00019144 File Offset: 0x00017344
		internal IWrappedDictionary CreateWrapper(object dictionary)
		{
			if (dictionary is IDictionary && (this.DictionaryValueType == null || !this._isDictionaryValueTypeNullableType))
			{
				return new DictionaryWrapper<object, object>((IDictionary)dictionary);
			}
			if (this._genericWrapperType == null)
			{
				this._genericWrapperType = ReflectionUtils.MakeGenericType(typeof(DictionaryWrapper<, >), new Type[]
				{
					this.DictionaryKeyType,
					this.DictionaryValueType
				});
				ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[]
				{
					this._genericCollectionDefinitionType
				});
				this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(constructor);
			}
			return (IWrappedDictionary)this._genericWrapperCreator(null, new object[]
			{
				dictionary
			});
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x000191F8 File Offset: 0x000173F8
		private bool IsTypeGenericDictionaryInterface(Type type)
		{
			if (!type.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(IDictionary);
		}

		// Token: 0x040001CB RID: 459
		private readonly bool _isDictionaryValueTypeNullableType;

		// Token: 0x040001CC RID: 460
		private readonly Type _genericCollectionDefinitionType;

		// Token: 0x040001CD RID: 461
		private Type _genericWrapperType;

		// Token: 0x040001CE RID: 462
		private MethodCall<object, object> _genericWrapperCreator;
	}
}
