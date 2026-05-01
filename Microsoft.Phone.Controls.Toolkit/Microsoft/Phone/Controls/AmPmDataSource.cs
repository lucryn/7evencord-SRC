using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000016 RID: 22
	internal class AmPmDataSource : DataSource
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x000045A4 File Offset: 0x000027A4
		protected override DateTime? GetRelativeTo(DateTime relativeDate, int delta)
		{
			int num = 24;
			int num2 = relativeDate.Hour + delta * (num / 2);
			if (num2 < 0 || num <= num2)
			{
				return default(DateTime?);
			}
			return new DateTime?(new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, num2, relativeDate.Minute, 0));
		}
	}
}
