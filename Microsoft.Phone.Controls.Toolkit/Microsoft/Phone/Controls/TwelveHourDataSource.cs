using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000014 RID: 20
	internal class TwelveHourDataSource : DataSource
	{
		// Token: 0x060000AE RID: 174 RVA: 0x000044F4 File Offset: 0x000026F4
		protected override DateTime? GetRelativeTo(DateTime relativeDate, int delta)
		{
			int num = 12;
			int num2 = (num + relativeDate.Hour + delta) % num;
			num2 += ((num <= relativeDate.Hour) ? num : 0);
			return new DateTime?(new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, num2, relativeDate.Minute, 0));
		}
	}
}
