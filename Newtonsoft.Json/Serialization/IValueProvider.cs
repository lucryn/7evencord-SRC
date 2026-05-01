using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000071 RID: 113
	public interface IValueProvider
	{
		// Token: 0x060005A7 RID: 1447
		void SetValue(object target, object value);

		// Token: 0x060005A8 RID: 1448
		object GetValue(object target);
	}
}
