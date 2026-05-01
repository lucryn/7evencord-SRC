using System;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000072 RID: 114
	public class ReflectionValueProvider : IValueProvider
	{
		// Token: 0x060005A9 RID: 1449 RVA: 0x0001766D File Offset: 0x0001586D
		public ReflectionValueProvider(MemberInfo memberInfo)
		{
			ValidationUtils.ArgumentNotNull(memberInfo, "memberInfo");
			this._memberInfo = memberInfo;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00017688 File Offset: 0x00015888
		public void SetValue(object target, object value)
		{
			try
			{
				ReflectionUtils.SetMemberValue(this._memberInfo, target, value);
			}
			catch (Exception innerException)
			{
				throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					this._memberInfo.Name,
					target.GetType()
				}), innerException);
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x000176EC File Offset: 0x000158EC
		public object GetValue(object target)
		{
			object memberValue;
			try
			{
				memberValue = ReflectionUtils.GetMemberValue(this._memberInfo, target);
			}
			catch (Exception innerException)
			{
				throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					this._memberInfo.Name,
					target.GetType()
				}), innerException);
			}
			return memberValue;
		}

		// Token: 0x0400016F RID: 367
		private readonly MemberInfo _memberInfo;
	}
}
