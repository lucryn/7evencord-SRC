using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000013 RID: 19
	internal class DayDataSource : DataSource
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00004488 File Offset: 0x00002688
		protected override DateTime? GetRelativeTo(DateTime relativeDate, int delta)
		{
			int num = DateTime.DaysInMonth(relativeDate.Year, relativeDate.Month);
			int num2 = (num + relativeDate.Day - 1 + delta) % num + 1;
			return new DateTime?(new DateTime(relativeDate.Year, relativeDate.Month, num2, relativeDate.Hour, relativeDate.Minute, relativeDate.Second));
		}
	}
}
