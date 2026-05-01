using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000012 RID: 18
	internal class MonthDataSource : DataSource
	{
		// Token: 0x060000AA RID: 170 RVA: 0x0000441C File Offset: 0x0000261C
		protected override DateTime? GetRelativeTo(DateTime relativeDate, int delta)
		{
			int num = 12;
			int num2 = (num + relativeDate.Month - 1 + delta) % num + 1;
			int num3 = Math.Min(relativeDate.Day, DateTime.DaysInMonth(relativeDate.Year, num2));
			return new DateTime?(new DateTime(relativeDate.Year, num2, num3, relativeDate.Hour, relativeDate.Minute, relativeDate.Second));
		}
	}
}
