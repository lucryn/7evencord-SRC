using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000017 RID: 23
	internal class TwentyFourHourDataSource : DataSource
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x00004604 File Offset: 0x00002804
		protected override DateTime? GetRelativeTo(DateTime relativeDate, int delta)
		{
			int num = 24;
			int num2 = (num + relativeDate.Hour + delta) % num;
			return new DateTime?(new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, num2, relativeDate.Minute, 0));
		}
	}
}
