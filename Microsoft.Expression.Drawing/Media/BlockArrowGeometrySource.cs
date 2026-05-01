using System;
using System.Windows;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Media
{
	// Token: 0x02000030 RID: 48
	internal class BlockArrowGeometrySource : GeometrySource<IBlockArrowGeometrySourceParameters>
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x0000BD44 File Offset: 0x00009F44
		protected override bool UpdateCachedGeometry(IBlockArrowGeometrySourceParameters parameters)
		{
			bool flag = false;
			BlockArrowGeometrySource.ArrowBuilder builder = BlockArrowGeometrySource.GetBuilder(parameters.Orientation);
			double num = builder.ArrowLength(base.LogicalBounds);
			double num2 = builder.ArrowWidth(base.LogicalBounds);
			double num3 = num2 / 2.0 / num;
			double num4 = MathHelper.EnsureRange(parameters.ArrowheadAngle, new double?(0.0), new double?(180.0));
			double num5 = Math.Tan(num4 * 3.141592653589793 / 180.0 / 2.0);
			if (num5 < num3)
			{
				this.EnsurePoints(3);
				this.points[0] = builder.ComputePointA(num, num2);
				this.points[1] = builder.ComputePointB(num, num);
				this.points[2] = builder.GetMirrorPoint(this.points[1], num2);
			}
			else
			{
				double offset = num2 / 2.0 / num5;
				double num6 = MathHelper.EnsureRange(parameters.ArrowBodySize, new double?(0.0), new double?(1.0));
				double thickness = num2 / 2.0 * (1.0 - num6);
				this.EnsurePoints(7);
				this.points[0] = builder.ComputePointA(num, num2);
				this.points[1] = builder.ComputePointB(num, offset);
				PointPair pointPair = builder.ComputePointCD(num, offset, thickness);
				this.points[2] = pointPair.Item1;
				this.points[3] = pointPair.Item2;
				this.points[4] = builder.GetMirrorPoint(this.points[3], num2);
				this.points[5] = builder.GetMirrorPoint(this.points[2], num2);
				this.points[6] = builder.GetMirrorPoint(this.points[1], num2);
			}
			for (int i = 0; i < this.points.Length; i++)
			{
				Point[] array = this.points;
				int num7 = i;
				array[num7].X = array[num7].X + base.LogicalBounds.Left;
				Point[] array2 = this.points;
				int num8 = i;
				array2[num8].Y = array2[num8].Y + base.LogicalBounds.Top;
			}
			return flag | PathGeometryHelper.SyncPolylineGeometry(ref this.cachedGeometry, this.points, true);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000C008 File Offset: 0x0000A208
		private static BlockArrowGeometrySource.ArrowBuilder GetBuilder(ArrowOrientation orientation)
		{
			switch (orientation)
			{
			case ArrowOrientation.Left:
				return new BlockArrowGeometrySource.LeftArrowBuilder();
			case ArrowOrientation.Up:
				return new BlockArrowGeometrySource.UpArrowBuilder();
			case ArrowOrientation.Down:
				return new BlockArrowGeometrySource.DownArrowBuilder();
			}
			return new BlockArrowGeometrySource.RightArrowBuilder();
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000C046 File Offset: 0x0000A246
		private void EnsurePoints(int count)
		{
			if (this.points == null || this.points.Length != count)
			{
				this.points = new Point[count];
			}
		}

		// Token: 0x04000093 RID: 147
		private Point[] points;

		// Token: 0x02000031 RID: 49
		private abstract class ArrowBuilder
		{
			// Token: 0x060001EB RID: 491
			public abstract double ArrowLength(Rect bounds);

			// Token: 0x060001EC RID: 492
			public abstract double ArrowWidth(Rect bounds);

			// Token: 0x060001ED RID: 493
			public abstract Point GetMirrorPoint(Point point, double width);

			// Token: 0x060001EE RID: 494
			public abstract Point ComputePointA(double length, double width);

			// Token: 0x060001EF RID: 495
			public abstract Point ComputePointB(double length, double offset);

			// Token: 0x060001F0 RID: 496
			public abstract PointPair ComputePointCD(double length, double offset, double thickness);
		}

		// Token: 0x02000032 RID: 50
		private abstract class HorizontalArrowBuilder : BlockArrowGeometrySource.ArrowBuilder
		{
			// Token: 0x060001F2 RID: 498 RVA: 0x0000C077 File Offset: 0x0000A277
			public override double ArrowLength(Rect bounds)
			{
				return bounds.Width;
			}

			// Token: 0x060001F3 RID: 499 RVA: 0x0000C080 File Offset: 0x0000A280
			public override double ArrowWidth(Rect bounds)
			{
				return bounds.Height;
			}

			// Token: 0x060001F4 RID: 500 RVA: 0x0000C089 File Offset: 0x0000A289
			public override Point GetMirrorPoint(Point point, double width)
			{
				return new Point(point.X, width - point.Y);
			}
		}

		// Token: 0x02000033 RID: 51
		private abstract class VerticalArrowBuilder : BlockArrowGeometrySource.ArrowBuilder
		{
			// Token: 0x060001F6 RID: 502 RVA: 0x0000C0A8 File Offset: 0x0000A2A8
			public override double ArrowLength(Rect bounds)
			{
				return bounds.Height;
			}

			// Token: 0x060001F7 RID: 503 RVA: 0x0000C0B1 File Offset: 0x0000A2B1
			public override double ArrowWidth(Rect bounds)
			{
				return bounds.Width;
			}

			// Token: 0x060001F8 RID: 504 RVA: 0x0000C0BA File Offset: 0x0000A2BA
			public override Point GetMirrorPoint(Point point, double width)
			{
				return new Point(width - point.X, point.Y);
			}
		}

		// Token: 0x02000034 RID: 52
		private class LeftArrowBuilder : BlockArrowGeometrySource.HorizontalArrowBuilder
		{
			// Token: 0x060001FA RID: 506 RVA: 0x0000C0D9 File Offset: 0x0000A2D9
			public override Point ComputePointA(double length, double width)
			{
				return new Point(0.0, width / 2.0);
			}

			// Token: 0x060001FB RID: 507 RVA: 0x0000C0F4 File Offset: 0x0000A2F4
			public override Point ComputePointB(double length, double offset)
			{
				return new Point(offset, 0.0);
			}

			// Token: 0x060001FC RID: 508 RVA: 0x0000C105 File Offset: 0x0000A305
			public override PointPair ComputePointCD(double length, double offset, double thickness)
			{
				return new PointPair(new Point(offset, thickness), new Point(length, thickness));
			}
		}

		// Token: 0x02000035 RID: 53
		private class RightArrowBuilder : BlockArrowGeometrySource.HorizontalArrowBuilder
		{
			// Token: 0x060001FE RID: 510 RVA: 0x0000C122 File Offset: 0x0000A322
			public override Point ComputePointA(double length, double width)
			{
				return new Point(length, width / 2.0);
			}

			// Token: 0x060001FF RID: 511 RVA: 0x0000C135 File Offset: 0x0000A335
			public override Point ComputePointB(double length, double offset)
			{
				return new Point(length - offset, 0.0);
			}

			// Token: 0x06000200 RID: 512 RVA: 0x0000C148 File Offset: 0x0000A348
			public override PointPair ComputePointCD(double length, double offset, double thickness)
			{
				return new PointPair(new Point(length - offset, thickness), new Point(0.0, thickness));
			}
		}

		// Token: 0x02000036 RID: 54
		private class UpArrowBuilder : BlockArrowGeometrySource.VerticalArrowBuilder
		{
			// Token: 0x06000202 RID: 514 RVA: 0x0000C16F File Offset: 0x0000A36F
			public override Point ComputePointA(double length, double width)
			{
				return new Point(width / 2.0, 0.0);
			}

			// Token: 0x06000203 RID: 515 RVA: 0x0000C18A File Offset: 0x0000A38A
			public override Point ComputePointB(double length, double offset)
			{
				return new Point(0.0, offset);
			}

			// Token: 0x06000204 RID: 516 RVA: 0x0000C19B File Offset: 0x0000A39B
			public override PointPair ComputePointCD(double length, double offset, double thickness)
			{
				return new PointPair(new Point(thickness, offset), new Point(thickness, length));
			}
		}

		// Token: 0x02000037 RID: 55
		private class DownArrowBuilder : BlockArrowGeometrySource.VerticalArrowBuilder
		{
			// Token: 0x06000206 RID: 518 RVA: 0x0000C1B8 File Offset: 0x0000A3B8
			public override Point ComputePointA(double length, double width)
			{
				return new Point(width / 2.0, length);
			}

			// Token: 0x06000207 RID: 519 RVA: 0x0000C1CB File Offset: 0x0000A3CB
			public override Point ComputePointB(double length, double offset)
			{
				return new Point(0.0, length - offset);
			}

			// Token: 0x06000208 RID: 520 RVA: 0x0000C1DE File Offset: 0x0000A3DE
			public override PointPair ComputePointCD(double length, double offset, double thickness)
			{
				return new PointPair(new Point(thickness, length - offset), new Point(thickness, 0.0));
			}
		}
	}
}
