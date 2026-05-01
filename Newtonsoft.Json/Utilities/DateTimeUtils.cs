using System;
using System.Globalization;
using System.Xml;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200008C RID: 140
	internal static class DateTimeUtils
	{
		// Token: 0x0600069A RID: 1690 RVA: 0x00018F98 File Offset: 0x00017198
		public static string GetUtcOffsetText(this DateTime d)
		{
			TimeSpan utcOffset = d.GetUtcOffset();
			return utcOffset.Hours.ToString("+00;-00", CultureInfo.InvariantCulture) + ":" + utcOffset.Minutes.ToString("00;00", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00018FE8 File Offset: 0x000171E8
		public static TimeSpan GetUtcOffset(this DateTime d)
		{
			return TimeZoneInfo.Local.GetUtcOffset(d);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00018FF8 File Offset: 0x000171F8
		public static XmlDateTimeSerializationMode ToSerializationMode(DateTimeKind kind)
		{
			switch (kind)
			{
			case 0:
				return 2;
			case 1:
				return 1;
			case 2:
				return 0;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("kind", kind, "Unexpected DateTimeKind value.");
			}
		}
	}
}
