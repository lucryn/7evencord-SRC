using System;
using System.Globalization;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x02000005 RID: 5
	public class DateTimeWrapper
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000267A File Offset: 0x0000087A
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002682 File Offset: 0x00000882
		public DateTime DateTime { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000268C File Offset: 0x0000088C
		public string YearNumber
		{
			get
			{
				return this.DateTime.ToString("yyyy", CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000026B4 File Offset: 0x000008B4
		public string MonthNumber
		{
			get
			{
				return this.DateTime.ToString("MM", CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000026DC File Offset: 0x000008DC
		public string MonthName
		{
			get
			{
				return this.DateTime.ToString("MMMM", CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002704 File Offset: 0x00000904
		public string DayNumber
		{
			get
			{
				return this.DateTime.ToString("dd", CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000272C File Offset: 0x0000092C
		public string DayName
		{
			get
			{
				return this.DateTime.ToString("dddd", CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002754 File Offset: 0x00000954
		public string HourNumber
		{
			get
			{
				return this.DateTime.ToString(DateTimeWrapper.CurrentCultureUsesTwentyFourHourClock() ? "%H" : "%h", CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002788 File Offset: 0x00000988
		public string MinuteNumber
		{
			get
			{
				return this.DateTime.ToString("mm", CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000027B0 File Offset: 0x000009B0
		public string AmPmString
		{
			get
			{
				return this.DateTime.ToString("tt", CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000027D5 File Offset: 0x000009D5
		public DateTimeWrapper(DateTime dateTime)
		{
			this.DateTime = dateTime;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000027E4 File Offset: 0x000009E4
		public static bool CurrentCultureUsesTwentyFourHourClock()
		{
			return !CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.Contains("t");
		}
	}
}
