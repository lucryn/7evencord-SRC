using System;
using System.Windows;

namespace Microsoft.Expression.Media
{
	// Token: 0x02000005 RID: 5
	internal interface ICalloutGeometrySourceParameters : IGeometrySourceParameters
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600003A RID: 58
		CalloutStyle CalloutStyle { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600003B RID: 59
		Point AnchorPoint { get; }
	}
}
