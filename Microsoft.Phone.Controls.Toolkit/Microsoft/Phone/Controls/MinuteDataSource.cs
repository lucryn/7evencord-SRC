using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000015 RID: 21
	internal class MinuteDataSource : DataSource
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00004554 File Offset: 0x00002754
		protected override DateTime? GetRelativeTo(DateTime relativeDate, int delta)
		{
			int num = 60;
			int num2 = (num + relativeDate.Minute + delta) % num;
			return new DateTime?(new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, relativeDate.Hour, num2, 0));
		}
	}
}
