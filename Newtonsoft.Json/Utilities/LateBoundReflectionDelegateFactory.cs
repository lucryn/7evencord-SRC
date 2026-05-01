using System;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000099 RID: 153
	internal class LateBoundReflectionDelegateFactory : ReflectionDelegateFactory
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0001B3F9 File Offset: 0x000195F9
		internal static ReflectionDelegateFactory Instance
		{
			get
			{
				return LateBoundReflectionDelegateFactory._instance;
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001B42C File Offset: 0x0001962C
		public override MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			ConstructorInfo c = method as ConstructorInfo;
			if (c != null)
			{
				return (T o, object[] a) => c.Invoke(a);
			}
			return (T o, object[] a) => method.Invoke(o, a);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001B4BC File Offset: 0x000196BC
		public override Func<T> CreateDefaultConstructor<T>(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsValueType)
			{
				return () => (T)((object)ReflectionUtils.CreateInstance(type, new object[0]));
			}
			ConstructorInfo constructorInfo = ReflectionUtils.GetDefaultConstructor(type, true);
			return () => (T)((object)constructorInfo.Invoke(null));
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001B53C File Offset: 0x0001973C
		public override Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			return (T o) => propertyInfo.GetValue(o, null);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001B590 File Offset: 0x00019790
		public override Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			return (T o) => fieldInfo.GetValue(o);
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0001B5E4 File Offset: 0x000197E4
		public override Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			return delegate(T o, object v)
			{
				fieldInfo.SetValue(o, v);
			};
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0001B638 File Offset: 0x00019838
		public override Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			return delegate(T o, object v)
			{
				propertyInfo.SetValue(o, v, null);
			};
		}

		// Token: 0x0400021D RID: 541
		private static readonly LateBoundReflectionDelegateFactory _instance = new LateBoundReflectionDelegateFactory();
	}
}
