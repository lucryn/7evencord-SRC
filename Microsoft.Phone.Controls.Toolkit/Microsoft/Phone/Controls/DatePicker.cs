using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000063 RID: 99
	public class DatePicker : DateTimePickerBase
	{
		// Token: 0x06000378 RID: 888 RVA: 0x0000F7A0 File Offset: 0x0000D9A0
		public DatePicker()
		{
			base.DefaultStyleKey = typeof(DatePicker);
			base.Value = new DateTime?(DateTime.Now.Date);
		}
	}
}
