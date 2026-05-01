using System;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x0200001F RID: 31
	internal sealed class PathSegmentData
	{
		// Token: 0x06000169 RID: 361 RVA: 0x00009412 File Offset: 0x00007612
		public PathSegmentData(Point startPoint, PathSegment pathSegment)
		{
			this.PathSegment = pathSegment;
			this.StartPoint = startPoint;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00009428 File Offset: 0x00007628
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00009430 File Offset: 0x00007630
		public Point StartPoint { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00009439 File Offset: 0x00007639
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00009441 File Offset: 0x00007641
		public PathSegment PathSegment { get; private set; }
	}
}
