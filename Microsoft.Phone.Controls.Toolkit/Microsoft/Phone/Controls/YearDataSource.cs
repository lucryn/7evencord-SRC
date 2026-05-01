using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000011 RID: 17
	internal class YearDataSource : DataSource
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x00004394 File Offset: 0x00002594
		protected override DateTime? GetRelativeTo(DateTime relativeDate, int delta)
		{
			if (1601 == relativeDate.Year || 3000 == relativeDate.Year)
			{
				return default(DateTime?);
			}
			int num = relativeDate.Year + delta;
			int num2 = Math.Min(relativeDate.Day, DateTime.DaysInMonth(num, relativeDate.Month));
			return new DateTime?(new DateTime(num, relativeDate.Month, num2, relativeDate.Hour, relativeDate.Minute, relativeDate.Second));
		}
	}
}
