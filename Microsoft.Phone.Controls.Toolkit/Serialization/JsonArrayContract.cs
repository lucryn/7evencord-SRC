using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200001C RID: 28
	public class JsonArrayContract : JsonContract
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00007F00 File Offset: 0x00006100
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00007F08 File Offset: 0x00006108
		internal Type CollectionItemType { get; private set; }

		// Token: 0x06000171 RID: 369 RVA: 0x00007F14 File Offset: 0x00006114
		public JsonArrayContract(Type underlyingType) : base(underlyingType)
		{
			if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(ICollection), out this._genericCollectionDefinitionType))
			{
				this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
			}
			else if (underlyingType.IsGenericType && underlyingType.GetGenericTypeDefinition() == typeof(IEnumerable))
			{
				this._genericCollectionDefinitionType = typeof(IEnumerable);
				this.CollectionItemType = underlyingType.GetGenericArguments()[0];
			}
			else
			{
				this.CollectionItemType = ReflectionUtils.GetCollectionItemType(base.UnderlyingType);
			}
			if (this.CollectionItemType != null)
			{
				this._isCollectionItemTypeNullableType = ReflectionUtils.IsNullableType(this.CollectionItemType);
			}
			if (this.IsTypeGenericCollectionInterface(base.UnderlyingType))
			{
				base.CreatedType = ReflectionUtils.MakeGenericType(typeof(List), new Type[]
				{
					this.CollectionItemType
				});
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007FF0 File Offset: 0x000061F0
		internal IWrappedCollection CreateWrapper(object list)
		{
			if ((list is IList && (this.CollectionItemType == null || !this._isCollectionItemTypeNullableType)) || base.UnderlyingType.IsArray)
			{
				return new CollectionWrapper<object>((IList)list);
			}
			if (this._genericCollectionDefinitionType != null)
			{
				this.EnsureGenericWrapperCreator();
				return (IWrappedCollection)this._genericWrapperCreator(null, new object[]
				{
					list
				});
			}
			IList list2 = Enumerable.ToList<object>(Enumerable.Cast<object>((IEnumerable)list));
			if (this.CollectionItemType != null)
			{
				Array array = Array.CreateInstance(this.CollectionItemType, list2.Count);
				for (int i = 0; i < list2.Count; i++)
				{
					array.SetValue(list2[i], i);
				}
				list2 = array;
			}
			return new CollectionWrapper<object>(list2);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000080AC File Offset: 0x000062AC
		private void EnsureGenericWrapperCreator()
		{
			if (this._genericWrapperType == null)
			{
				this._genericWrapperType = ReflectionUtils.MakeGenericType(typeof(CollectionWrapper<>), new Type[]
				{
					this.CollectionItemType
				});
				Type type;
				if (ReflectionUtils.InheritsGenericDefinition(this._genericCollectionDefinitionType, typeof(List)) || this._genericCollectionDefinitionType.GetGenericTypeDefinition() == typeof(IEnumerable))
				{
					type = ReflectionUtils.MakeGenericType(typeof(ICollection), new Type[]
					{
						this.CollectionItemType
					});
				}
				else
				{
					type = this._genericCollectionDefinitionType;
				}
				ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[]
				{
					type
				});
				this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(constructor);
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00008170 File Offset: 0x00006370
		private bool IsTypeGenericCollectionInterface(Type type)
		{
			if (!type.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(IList) || genericTypeDefinition == typeof(ICollection) || genericTypeDefinition == typeof(IEnumerable);
		}

		// Token: 0x040000A6 RID: 166
		private readonly bool _isCollectionItemTypeNullableType;

		// Token: 0x040000A7 RID: 167
		private readonly Type _genericCollectionDefinitionType;

		// Token: 0x040000A8 RID: 168
		private Type _genericWrapperType;

		// Token: 0x040000A9 RID: 169
		private MethodCall<object, object> _genericWrapperCreator;
	}
}
