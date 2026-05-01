using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Media
{
	// Token: 0x02000041 RID: 65
	internal class LineArrowGeometrySource : GeometrySource<ILineArrowGeometrySourceParameters>
	{
		// Token: 0x0600023B RID: 571 RVA: 0x0000DFC8 File Offset: 0x0000C1C8
		protected override bool UpdateCachedGeometry(ILineArrowGeometrySourceParameters parameters)
		{
			bool flag = false;
			PathGeometry pathGeometry;
			flag |= GeometryHelper.EnsureGeometryType<PathGeometry>(out pathGeometry, ref this.cachedGeometry, () => new PathGeometry());
			flag |= pathGeometry.Figures.EnsureListCount(3, () => new PathFigure());
			Point startPoint = this.GetStartPoint(parameters);
			Point endPoint = this.GetEndPoint(parameters);
			Point middlePoint = this.GetMiddlePoint(parameters);
			PathFigure pathFigure = pathGeometry.Figures[0];
			flag |= pathFigure.SetIfDifferent(PathFigure.StartPointProperty, startPoint);
			flag |= pathFigure.SetIfDifferent(PathFigure.IsClosedProperty, false);
			flag |= pathFigure.SetIfDifferent(PathFigure.IsFilledProperty, false);
			flag |= pathFigure.Segments.EnsureListCount(1, () => new QuadraticBezierSegment());
			QuadraticBezierSegment dependencyObject;
			flag |= GeometryHelper.EnsureSegmentType<QuadraticBezierSegment>(out dependencyObject, pathFigure.Segments, 0, () => new QuadraticBezierSegment());
			flag |= dependencyObject.SetIfDifferent(QuadraticBezierSegment.Point1Property, middlePoint);
			flag |= dependencyObject.SetIfDifferent(QuadraticBezierSegment.Point2Property, endPoint);
			flag |= LineArrowGeometrySource.UpdateArrow(parameters.StartArrow, parameters.ArrowSize, pathGeometry.Figures[1], startPoint, startPoint.Subtract(middlePoint).Normalized());
			flag |= LineArrowGeometrySource.UpdateArrow(parameters.EndArrow, parameters.ArrowSize, pathGeometry.Figures[2], endPoint, endPoint.Subtract(middlePoint).Normalized());
			return true;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000E17C File Offset: 0x0000C37C
		private static bool UpdateArrow(ArrowType arrowType, double size, PathFigure figure, Point startPoint, Vector tangent)
		{
			bool flag = false;
			switch (arrowType)
			{
			case ArrowType.NoArrow:
				flag |= figure.SetIfDifferent(PathFigure.StartPointProperty, startPoint);
				flag |= figure.Segments.EnsureListCount(0, null);
				break;
			default:
			{
				Point[] pointTrio = LineArrowGeometrySource.GetPointTrio(startPoint, tangent, size);
				if (arrowType == ArrowType.StealthArrow)
				{
					List<Point> list = new List<Point>(pointTrio);
					list.Add(startPoint - tangent * size * 2.0 / 3.0);
					flag |= PathFigureHelper.SyncPolylineFigure(figure, list, true, true);
				}
				else
				{
					bool flag2 = arrowType == ArrowType.OpenArrow;
					flag |= PathFigureHelper.SyncPolylineFigure(figure, pointTrio, !flag2, !flag2);
				}
				break;
			}
			case ArrowType.OvalArrow:
			{
				Rect bounds;
				bounds..ctor(startPoint.X - size / 2.0, startPoint.Y - size / 2.0, size, size);
				bool flag3 = flag;
				bool isFilled = true;
				flag = (flag3 | PathFigureHelper.SyncEllipseFigure(figure, bounds, 1, isFilled));
				break;
			}
			}
			return flag;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000E27C File Offset: 0x0000C47C
		private static Point[] GetPointTrio(Point startPoint, Vector tangent, double size)
		{
			Vector vector = tangent.Perpendicular().Normalized() * 0.57735;
			return new Point[]
			{
				startPoint - tangent * size + vector * size,
				startPoint,
				startPoint - tangent * size - vector * size
			};
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000E304 File Offset: 0x0000C504
		private Point GetMiddlePoint(ILineArrowGeometrySourceParameters parameters)
		{
			Rect logicalBounds = base.LogicalBounds;
			double alpha = (parameters.BendAmount + 1.0) / 2.0;
			switch (parameters.StartCorner)
			{
			case CornerType.TopLeft:
				return GeometryHelper.Lerp(logicalBounds.BottomLeft(), logicalBounds.TopRight(), alpha);
			case CornerType.TopRight:
				return GeometryHelper.Lerp(logicalBounds.TopLeft(), logicalBounds.BottomRight(), alpha);
			case CornerType.BottomRight:
				return GeometryHelper.Lerp(logicalBounds.TopRight(), logicalBounds.BottomLeft(), alpha);
			case CornerType.BottomLeft:
				return GeometryHelper.Lerp(logicalBounds.BottomRight(), logicalBounds.TopLeft(), alpha);
			default:
				return logicalBounds.Center();
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000E3A4 File Offset: 0x0000C5A4
		private Point GetEndPoint(ILineArrowGeometrySourceParameters parameters)
		{
			Rect logicalBounds = base.LogicalBounds;
			switch (parameters.StartCorner)
			{
			case CornerType.TopLeft:
				return logicalBounds.BottomRight();
			case CornerType.TopRight:
				return logicalBounds.BottomLeft();
			case CornerType.BottomRight:
				return logicalBounds.TopLeft();
			case CornerType.BottomLeft:
				return logicalBounds.TopRight();
			default:
				return logicalBounds.BottomRight();
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000E3FC File Offset: 0x0000C5FC
		private Point GetStartPoint(ILineArrowGeometrySourceParameters parameters)
		{
			Rect logicalBounds = base.LogicalBounds;
			switch (parameters.StartCorner)
			{
			case CornerType.TopLeft:
				return logicalBounds.TopLeft();
			case CornerType.TopRight:
				return logicalBounds.TopRight();
			case CornerType.BottomRight:
				return logicalBounds.BottomRight();
			case CornerType.BottomLeft:
				return logicalBounds.BottomLeft();
			default:
				return logicalBounds.BottomRight();
			}
		}
	}
}
