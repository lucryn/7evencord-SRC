using System;
using Microsoft.Phone.Logging;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000006 RID: 6
	internal class PerfLog
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000022D0 File Offset: 0x000012D0
		internal static void BeginLogMarker(PerfMarkerEvents EventCode, string Message)
		{
			PerfLog.LogPerfMarker(65793, EventCode, Message);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022DE File Offset: 0x000012DE
		internal static void EndLogMarker(PerfMarkerEvents EventCode, string Message)
		{
			PerfLog.LogPerfMarker(131329, EventCode, Message);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000022EC File Offset: 0x000012EC
		internal static void InfoLogMarker(PerfMarkerEvents EventCode, string Message)
		{
			PerfLog.LogPerfMarker(257, EventCode, Message);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022FA File Offset: 0x000012FA
		private static void LogPerfMarker(LogFlags logFlag, PerfMarkerEvents EventCode, string Message)
		{
			Logger.YLogEvent(2147483654U, (uint)EventCode, logFlag, Message);
		}

		// Token: 0x04000010 RID: 16
		internal static string Panorama = "Panorama";

		// Token: 0x04000011 RID: 17
		internal static string PanoramaPanel = "PanoramaPanel";

		// Token: 0x04000012 RID: 18
		internal static string PanoramaItem = "PanoramaItem";

		// Token: 0x04000013 RID: 19
		internal static string PanningLayer = "PanningLayer";

		// Token: 0x04000014 RID: 20
		internal static string Pivot = "Pivot";

		// Token: 0x04000015 RID: 21
		internal static string PivotItem = "PivotItem";

		// Token: 0x04000016 RID: 22
		internal static string PivotHeadersControl = "PivotHeadersControl";
	}
}
