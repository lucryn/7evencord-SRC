using System;
using System.Globalization;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000057 RID: 87
	public class TimePicker : DateTimePickerBase
	{
		// Token: 0x0600033C RID: 828 RVA: 0x0000E707 File Offset: 0x0000C907
		public TimePicker()
		{
			base.DefaultStyleKey = typeof(TimePicker);
			base.Value = new DateTime?(DateTime.Now);
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000E730 File Offset: 0x0000C930
		protected override string ValueStringFormatFallback
		{
			get
			{
				if (this._fallbackValueStringFormat == null)
				{
					string text = CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.Replace(":ss", "");
					this._fallbackValueStringFormat = "{0:" + text + "}";
				}
				return this._fallbackValueStringFormat;
			}
		}

		// Token: 0x04000179 RID: 377
		private string _fallbackValueStringFormat;
	}
}
