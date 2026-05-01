using System;
using System.Collections.Generic;
using System.Windows;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x02000025 RID: 37
	internal class SimpleSegment
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00009EE2 File Offset: 0x000080E2
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00009EEA File Offset: 0x000080EA
		public SimpleSegment.SegmentType Type { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00009EF3 File Offset: 0x000080F3
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00009EFB File Offset: 0x000080FB
		public Point[] Points { get; private set; }

		// Token: 0x0600019B RID: 411 RVA: 0x00009F04 File Offset: 0x00008104
		public void Flatten(IList<Point> resultPolyline, double tolerance, IList<double> resultParameters)
		{
			switch (this.Type)
			{
			case SimpleSegment.SegmentType.Line:
				resultPolyline.Add(this.Points[1]);
				if (resultParameters != null)
				{
					resultParameters.Add(1.0);
					return;
				}
				break;
			case SimpleSegment.SegmentType.CubicBeizer:
				BezierCurveFlattener.FlattenCubic(this.Points, tolerance, resultPolyline, true, resultParameters);
				break;
			default:
				return;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00009F60 File Offset: 0x00008160
		private SimpleSegment()
		{
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00009F68 File Offset: 0x00008168
		public static SimpleSegment Create(Point point0, Point point1)
		{
			return new SimpleSegment
			{
				Type = SimpleSegment.SegmentType.Line,
				Points = new Point[]
				{
					point0,
					point1
				}
			};
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00009FAC File Offset: 0x000081AC
		public static SimpleSegment Create(Point point0, Point point1, Point point2)
		{
			Point point3 = GeometryHelper.Lerp(point0, point1, 0.6666666666666666);
			Point point4 = GeometryHelper.Lerp(point1, point2, 0.3333333333333333);
			return new SimpleSegment
			{
				Type = SimpleSegment.SegmentType.CubicBeizer,
				Points = new Point[]
				{
					point0,
					point3,
					point4,
					point2
				}
			};
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000A02C File Offset: 0x0000822C
		public static SimpleSegment Create(Point point0, Point point1, Point point2, Point point3)
		{
			return new SimpleSegment
			{
				Type = SimpleSegment.SegmentType.CubicBeizer,
				Points = new Point[]
				{
					point0,
					point1,
					point2,
					point3
				}
			};
		}

		// Token: 0x02000026 RID: 38
		public enum SegmentType
		{
			// Token: 0x04000070 RID: 112
			Line,
			// Token: 0x04000071 RID: 113
			CubicBeizer
		}
	}
}
