using System;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200008B RID: 139
	internal abstract class ReflectionDelegateFactory
	{
		// Token: 0x06000691 RID: 1681 RVA: 0x00018EE8 File Offset: 0x000170E8
		public Func<T, object> CreateGet<T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return this.CreateGet<T>(propertyInfo);
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return this.CreateGet<T>(fieldInfo);
			}
			throw new Exception("Could not create getter for {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				memberInfo
			}));
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00018F3C File Offset: 0x0001713C
		public Action<T, object> CreateSet<T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return this.CreateSet<T>(propertyInfo);
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return this.CreateSet<T>(fieldInfo);
			}
			throw new Exception("Could not create setter for {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				memberInfo
			}));
		}

		// Token: 0x06000693 RID: 1683
		public abstract MethodCall<T, object> CreateMethodCall<T>(MethodBase method);

		// Token: 0x06000694 RID: 1684
		public abstract Func<T> CreateDefaultConstructor<T>(Type type);

		// Token: 0x06000695 RID: 1685
		public abstract Func<T, object> CreateGet<T>(PropertyInfo propertyInfo);

		// Token: 0x06000696 RID: 1686
		public abstract Func<T, object> CreateGet<T>(FieldInfo fieldInfo);

		// Token: 0x06000697 RID: 1687
		public abstract Action<T, object> CreateSet<T>(FieldInfo fieldInfo);

		// Token: 0x06000698 RID: 1688
		public abstract Action<T, object> CreateSet<T>(PropertyInfo propertyInfo);
	}
}
