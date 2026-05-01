using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Media
{
	// Token: 0x0200002C RID: 44
	internal class ArcGeometrySource : GeometrySource<IArcGeometrySourceParameters>
	{
		// Token: 0x060001BF RID: 447 RVA: 0x0000A760 File Offset: 0x00008960
		protected override Rect ComputeLogicalBounds(Rect layoutBounds, IGeometrySourceParameters parameters)
		{
			Rect logicalBound = base.ComputeLogicalBounds(layoutBounds, parameters);
			return GeometryHelper.GetStretchBound(logicalBound, parameters.Stretch, new Size(1.0, 1.0));
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000A79C File Offset: 0x0000899C
		protected override bool UpdateCachedGeometry(IArcGeometrySourceParameters parameters)
		{
			bool flag = false;
			this.NormalizeThickness(parameters);
			bool relativeMode = parameters.ArcThicknessUnit == UnitType.Percent;
			bool flag2 = MathHelper.AreClose(parameters.StartAngle, parameters.EndAngle);
			double num = ArcGeometrySource.NormalizeAngle(parameters.StartAngle);
			double num2 = ArcGeometrySource.NormalizeAngle(parameters.EndAngle);
			if (num2 < num)
			{
				num2 += 360.0;
			}
			bool flag3 = this.relativeThickness == 1.0;
			bool flag4 = this.relativeThickness == 0.0;
			if (flag2)
			{
				flag |= this.UpdateZeroAngleGeometry(relativeMode, num);
			}
			else if (MathHelper.IsVerySmall((num2 - num) % 360.0))
			{
				if (flag4 || flag3)
				{
					flag |= this.UpdateEllipseGeometry(flag3);
				}
				else
				{
					flag |= this.UpdateFullRingGeometry(relativeMode);
				}
			}
			else if (flag3)
			{
				flag |= this.UpdatePieGeometry(num, num2);
			}
			else if (flag4)
			{
				flag |= this.UpdateOpenArcGeometry(num, num2);
			}
			else
			{
				flag |= this.UpdateRingArcGeometry(relativeMode, num, num2);
			}
			return flag;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000A898 File Offset: 0x00008A98
		private void NormalizeThickness(IArcGeometrySourceParameters parameters)
		{
			double num = base.LogicalBounds.Width / 2.0;
			double num2 = base.LogicalBounds.Height / 2.0;
			double num3 = Math.Min(num, num2);
			double num4 = parameters.ArcThickness;
			if (parameters.ArcThicknessUnit == UnitType.Pixel)
			{
				num4 = MathHelper.SafeDivide(num4, num3, 0.0);
			}
			this.relativeThickness = MathHelper.EnsureRange(num4, new double?(0.0), new double?(1.0));
			this.absoluteThickness = num3 * this.relativeThickness;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000A940 File Offset: 0x00008B40
		private bool UpdateZeroAngleGeometry(bool relativeMode, double angle)
		{
			bool flag = false;
			Point arcPoint = GeometryHelper.GetArcPoint(angle, base.LogicalBounds);
			Rect logicalBounds = base.LogicalBounds;
			double num = logicalBounds.Width / 2.0;
			double num2 = logicalBounds.Height / 2.0;
			Point point;
			if (relativeMode || MathHelper.AreClose(num, num2))
			{
				Rect bound = base.LogicalBounds.Resize(1.0 - this.relativeThickness);
				point = GeometryHelper.GetArcPoint(angle, bound);
			}
			else
			{
				double intersect = ArcGeometrySource.InnerCurveSelfIntersect(num, num2, this.absoluteThickness);
				double[] array = ArcGeometrySource.ComputeAngleRanges(num, num2, intersect, angle, angle);
				double num3 = array[0] * 3.141592653589793 / 180.0;
				Vector vector = new Vector(num2 * Math.Sin(num3), -num * Math.Cos(num3));
				point = GeometryHelper.GetArcPoint(array[0], base.LogicalBounds) - vector.Normalized() * this.absoluteThickness;
			}
			LineGeometry dependencyObject;
			flag |= GeometryHelper.EnsureGeometryType<LineGeometry>(out dependencyObject, ref this.cachedGeometry, () => new LineGeometry());
			flag |= dependencyObject.SetIfDifferent(LineGeometry.StartPointProperty, arcPoint);
			return flag | dependencyObject.SetIfDifferent(LineGeometry.EndPointProperty, point);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000AAB8 File Offset: 0x00008CB8
		private bool UpdateEllipseGeometry(bool isFilled)
		{
			bool flag = false;
			double num = MathHelper.Lerp(base.LogicalBounds.Top, base.LogicalBounds.Bottom, 0.5);
			Point point;
			point..ctor(base.LogicalBounds.Left, num);
			Point point2;
			point2..ctor(base.LogicalBounds.Right, num);
			PathGeometry pathGeometry;
			flag |= GeometryHelper.EnsureGeometryType<PathGeometry>(out pathGeometry, ref this.cachedGeometry, () => new PathGeometry());
			flag |= pathGeometry.Figures.EnsureListCount(1, () => new PathFigure());
			PathFigure pathFigure = pathGeometry.Figures[0];
			flag |= pathFigure.SetIfDifferent(PathFigure.IsClosedProperty, true);
			flag |= pathFigure.SetIfDifferent(PathFigure.IsFilledProperty, isFilled);
			flag |= pathFigure.Segments.EnsureListCount(2, () => new ArcSegment());
			flag |= pathFigure.SetIfDifferent(PathFigure.StartPointProperty, point);
			ArcSegment dependencyObject;
			flag |= GeometryHelper.EnsureSegmentType<ArcSegment>(out dependencyObject, pathFigure.Segments, 0, () => new ArcSegment());
			ArcSegment dependencyObject2;
			flag |= GeometryHelper.EnsureSegmentType<ArcSegment>(out dependencyObject2, pathFigure.Segments, 1, () => new ArcSegment());
			Size size;
			size..ctor(base.LogicalBounds.Width / 2.0, base.LogicalBounds.Height / 2.0);
			flag |= dependencyObject.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
			flag |= dependencyObject.SetIfDifferent(ArcSegment.SizeProperty, size);
			flag |= dependencyObject.SetIfDifferent(ArcSegment.SweepDirectionProperty, 1);
			flag |= dependencyObject.SetIfDifferent(ArcSegment.PointProperty, point2);
			flag |= dependencyObject2.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
			flag |= dependencyObject2.SetIfDifferent(ArcSegment.SizeProperty, size);
			flag |= dependencyObject2.SetIfDifferent(ArcSegment.SweepDirectionProperty, 1);
			return flag | dependencyObject2.SetIfDifferent(ArcSegment.PointProperty, point);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000AD48 File Offset: 0x00008F48
		private bool UpdateFullRingGeometry(bool relativeMode)
		{
			bool flag = false;
			PathGeometry pathGeometry;
			flag |= GeometryHelper.EnsureGeometryType<PathGeometry>(out pathGeometry, ref this.cachedGeometry, () => new PathGeometry());
			flag |= pathGeometry.SetIfDifferent(PathGeometry.FillRuleProperty, 0);
			flag |= pathGeometry.Figures.EnsureListCount(2, () => new PathFigure());
			flag |= PathFigureHelper.SyncEllipseFigure(pathGeometry.Figures[0], base.LogicalBounds, 1, true);
			Rect logicalBounds = base.LogicalBounds;
			double num = logicalBounds.Width / 2.0;
			double num2 = logicalBounds.Height / 2.0;
			if (relativeMode || MathHelper.AreClose(num, num2))
			{
				Rect bounds = base.LogicalBounds.Resize(1.0 - this.relativeThickness);
				flag |= PathFigureHelper.SyncEllipseFigure(pathGeometry.Figures[1], bounds, 0, true);
			}
			else
			{
				flag |= pathGeometry.Figures[1].SetIfDifferent(PathFigure.IsClosedProperty, true);
				flag |= pathGeometry.Figures[1].SetIfDifferent(PathFigure.IsFilledProperty, true);
				Point point = default(Point);
				double intersect = ArcGeometrySource.InnerCurveSelfIntersect(num, num2, this.absoluteThickness);
				double[] angles = ArcGeometrySource.ComputeAngleRanges(num, num2, intersect, 360.0, 0.0);
				flag |= this.SyncPieceWiseInnerCurves(pathGeometry.Figures[1], 0, ref point, angles);
				flag |= pathGeometry.Figures[1].SetIfDifferent(PathFigure.StartPointProperty, point);
			}
			return flag;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000AF02 File Offset: 0x00009102
		private static void IncreaseDuplicatedIndex(IList<double> values, ref int index)
		{
			while (index < values.Count - 1 && values[index] == values[index + 1])
			{
				index++;
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000AF2C File Offset: 0x0000912C
		private static void DecreaseDuplicatedIndex(IList<double> values, ref int index)
		{
			while (index > 0 && values[index] == values[index - 1])
			{
				index--;
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000AF50 File Offset: 0x00009150
		internal static double[] ComputeAngleRanges(double radiusX, double radiusY, double intersect, double start, double end)
		{
			List<double> list = new List<double>();
			list.Add(start);
			list.Add(end);
			list.Add(intersect);
			list.Add(180.0 - intersect);
			list.Add(180.0 + intersect);
			list.Add(360.0 - intersect);
			list.Add(360.0 + intersect);
			list.Add(540.0 - intersect);
			list.Add(540.0 + intersect);
			list.Add(720.0 - intersect);
			List<double> list2 = list;
			list2.Sort();
			int num = list2.IndexOf(start);
			int num2 = list2.IndexOf(end);
			if (num2 == num)
			{
				num2++;
			}
			else if (start < end)
			{
				ArcGeometrySource.IncreaseDuplicatedIndex(list2, ref num);
				ArcGeometrySource.DecreaseDuplicatedIndex(list2, ref num2);
			}
			else if (start > end)
			{
				ArcGeometrySource.DecreaseDuplicatedIndex(list2, ref num);
				ArcGeometrySource.IncreaseDuplicatedIndex(list2, ref num2);
			}
			List<double> list3 = new List<double>();
			if (num < num2)
			{
				for (int i = num; i <= num2; i++)
				{
					list3.Add(list2[i]);
				}
			}
			else
			{
				for (int j = num; j >= num2; j--)
				{
					list3.Add(list2[j]);
				}
			}
			double num3 = ArcGeometrySource.EnsureFirstQuadrant((list3[0] + list3[1]) / 2.0);
			if ((radiusX < radiusY && num3 < intersect) || (radiusX > radiusY && num3 > intersect))
			{
				list3.RemoveAt(0);
			}
			if (list3.Count % 2 == 1)
			{
				list3.RemoveLast<double>();
			}
			if (list3.Count == 0)
			{
				int num4 = Math.Min(num, num2) - 1;
				if (num4 < 0)
				{
					num4 = Math.Max(num, num2) + 1;
				}
				list3.Add(list2[num4]);
				list3.Add(list2[num4]);
			}
			return list3.ToArray();
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000B122 File Offset: 0x00009322
		internal static double EnsureFirstQuadrant(double angle)
		{
			angle = Math.Abs(angle % 180.0);
			if (angle <= 90.0)
			{
				return angle;
			}
			return 180.0 - angle;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000B150 File Offset: 0x00009350
		private bool UpdatePieGeometry(double start, double end)
		{
			bool flag = false;
			PathGeometry pathGeometry = this.cachedGeometry as PathGeometry;
			PathFigure pathFigure;
			ArcSegment arcSegment;
			LineSegment dependencyObject;
			if (pathGeometry == null || pathGeometry.Figures.Count != 1 || (pathFigure = pathGeometry.Figures[0]).Segments.Count != 2 || (arcSegment = (pathFigure.Segments[0] as ArcSegment)) == null || (dependencyObject = (pathFigure.Segments[1] as LineSegment)) == null)
			{
				(this.cachedGeometry = new PathGeometry()).Figures.Add(pathFigure = new PathFigure());
				pathFigure.Segments.Add(arcSegment = new ArcSegment());
				pathFigure.Segments.Add(dependencyObject = new LineSegment());
				pathFigure.IsClosed = true;
				arcSegment.SweepDirection = 1;
				flag = true;
			}
			flag |= pathFigure.SetIfDifferent(PathFigure.StartPointProperty, GeometryHelper.GetArcPoint(start, base.LogicalBounds));
			flag |= arcSegment.SetIfDifferent(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(end, base.LogicalBounds));
			flag |= arcSegment.SetIfDifferent(ArcSegment.SizeProperty, ArcGeometrySource.GetArcSize(base.LogicalBounds));
			flag |= arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180.0);
			return flag | dependencyObject.SetIfDifferent(LineSegment.PointProperty, base.LogicalBounds.Center());
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000B2B4 File Offset: 0x000094B4
		private bool UpdateOpenArcGeometry(double start, double end)
		{
			bool flag = false;
			PathGeometry pathGeometry = this.cachedGeometry as PathGeometry;
			PathFigure pathFigure;
			ArcSegment arcSegment;
			if (pathGeometry == null || pathGeometry.Figures.Count != 1 || (pathFigure = pathGeometry.Figures[0]).Segments.Count != 1 || (arcSegment = (pathFigure.Segments[0] as ArcSegment)) == null)
			{
				(this.cachedGeometry = new PathGeometry()).Figures.Add(pathFigure = new PathFigure());
				pathFigure.Segments.Add(arcSegment = new ArcSegment());
				pathFigure.IsClosed = false;
				arcSegment.SweepDirection = 1;
				flag = true;
			}
			flag |= pathFigure.SetIfDifferent(PathFigure.StartPointProperty, GeometryHelper.GetArcPoint(start, base.LogicalBounds));
			flag |= pathFigure.SetIfDifferent(PathFigure.IsFilledProperty, false);
			flag |= arcSegment.SetIfDifferent(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(end, base.LogicalBounds));
			flag |= arcSegment.SetIfDifferent(ArcSegment.SizeProperty, ArcGeometrySource.GetArcSize(base.LogicalBounds));
			return flag | arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180.0);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000B414 File Offset: 0x00009614
		private bool UpdateRingArcGeometry(bool relativeMode, double start, double end)
		{
			bool flag = false;
			PathGeometry pathGeometry;
			flag |= GeometryHelper.EnsureGeometryType<PathGeometry>(out pathGeometry, ref this.cachedGeometry, () => new PathGeometry());
			flag |= pathGeometry.SetIfDifferent(PathGeometry.FillRuleProperty, 1);
			flag |= pathGeometry.Figures.EnsureListCount(1, () => new PathFigure());
			PathFigure pathFigure = pathGeometry.Figures[0];
			flag |= pathFigure.SetIfDifferent(PathFigure.IsClosedProperty, true);
			flag |= pathFigure.SetIfDifferent(PathFigure.IsFilledProperty, true);
			flag |= pathFigure.SetIfDifferent(PathFigure.StartPointProperty, GeometryHelper.GetArcPoint(start, base.LogicalBounds));
			flag |= pathFigure.Segments.EnsureListCountAtLeast(3, () => new ArcSegment());
			ArcSegment dependencyObject;
			flag |= GeometryHelper.EnsureSegmentType<ArcSegment>(out dependencyObject, pathFigure.Segments, 0, () => new ArcSegment());
			flag |= dependencyObject.SetIfDifferent(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(end, base.LogicalBounds));
			flag |= dependencyObject.SetIfDifferent(ArcSegment.SizeProperty, new Size(base.LogicalBounds.Width / 2.0, base.LogicalBounds.Height / 2.0));
			flag |= dependencyObject.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180.0);
			flag |= dependencyObject.SetIfDifferent(ArcSegment.SweepDirectionProperty, 1);
			LineSegment dependencyObject2;
			flag |= GeometryHelper.EnsureSegmentType<LineSegment>(out dependencyObject2, pathFigure.Segments, 1, () => new LineSegment());
			Rect logicalBounds = base.LogicalBounds;
			double num = logicalBounds.Width / 2.0;
			double num2 = logicalBounds.Height / 2.0;
			if (relativeMode || MathHelper.AreClose(num, num2))
			{
				Rect bound = base.LogicalBounds.Resize(1.0 - this.relativeThickness);
				flag |= dependencyObject2.SetIfDifferent(LineSegment.PointProperty, GeometryHelper.GetArcPoint(end, bound));
				flag |= pathFigure.Segments.EnsureListCount(3, () => new ArcSegment());
				ArcSegment dependencyObject3;
				flag |= GeometryHelper.EnsureSegmentType<ArcSegment>(out dependencyObject3, pathFigure.Segments, 2, () => new ArcSegment());
				flag |= dependencyObject3.SetIfDifferent(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(start, bound));
				flag |= dependencyObject3.SetIfDifferent(ArcSegment.SizeProperty, ArcGeometrySource.GetArcSize(bound));
				flag |= dependencyObject3.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180.0);
				flag |= dependencyObject3.SetIfDifferent(ArcSegment.SweepDirectionProperty, 0);
			}
			else
			{
				Point point = default(Point);
				double intersect = ArcGeometrySource.InnerCurveSelfIntersect(num, num2, this.absoluteThickness);
				double[] angles = ArcGeometrySource.ComputeAngleRanges(num, num2, intersect, end, start);
				flag |= this.SyncPieceWiseInnerCurves(pathFigure, 2, ref point, angles);
				flag |= dependencyObject2.SetIfDifferent(LineSegment.PointProperty, point);
			}
			return flag;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000B7A4 File Offset: 0x000099A4
		private bool SyncPieceWiseInnerCurves(PathFigure figure, int index, ref Point firstPoint, params double[] angles)
		{
			bool flag = false;
			int num = angles.Length;
			Rect logicalBounds = base.LogicalBounds;
			double offset = this.absoluteThickness;
			flag |= figure.Segments.EnsureListCount(index + num / 2, () => new PolyBezierSegment());
			for (int i = 0; i < num / 2; i++)
			{
				IList<Point> list = ArcGeometrySource.ComputeOneInnerCurve(angles[i * 2], angles[i * 2 + 1], logicalBounds, offset);
				if (i == 0)
				{
					firstPoint = list[0];
				}
				flag |= PathSegmentHelper.SyncPolyBezierSegment(figure.Segments, index + i, list, 1, list.Count - 1);
			}
			return flag;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000B854 File Offset: 0x00009A54
		private static IList<Point> ComputeOneInnerCurve(double start, double end, Rect bounds, double offset)
		{
			double num = bounds.Width / 2.0;
			double num2 = bounds.Height / 2.0;
			Point point = bounds.Center();
			start = start * 3.141592653589793 / 180.0;
			end = end * 3.141592653589793 / 180.0;
			double num3 = 0.17453292519943295;
			int num4 = (int)Math.Ceiling(Math.Abs(end - start) / num3);
			num4 = Math.Max(2, num4);
			List<Point> list = new List<Point>(num4);
			List<Vector> list2 = new List<Vector>(num4);
			Point point2 = default(Point);
			Point point3 = default(Point);
			Vector vector = default(Vector);
			Vector vector2 = default(Vector);
			Vector vector3 = default(Vector);
			Vector vector4 = default(Vector);
			for (int i = 0; i < num4; i++)
			{
				double num5 = MathHelper.Lerp(start, end, (double)i / (double)(num4 - 1));
				double num6 = Math.Sin(num5);
				double num7 = Math.Cos(num5);
				point2.X = point.X + num * num6;
				point2.Y = point.Y - num2 * num7;
				vector.X = num * num7;
				vector.Y = num2 * num6;
				vector2.X = -num2 * num6;
				vector2.Y = num * num7;
				double num8 = num2 * num2 * num6 * num6 + num * num * num7 * num7;
				double num9 = Math.Sqrt(num8);
				double num10 = 2.0 * num6 * num7 * (num2 * num2 - num * num);
				vector3.X = -num2 * num7;
				vector3.Y = -num * num6;
				point3.X = point2.X + offset * vector2.X / num9;
				point3.Y = point2.Y + offset * vector2.Y / num9;
				vector4.X = vector.X + offset / num9 * (vector3.X - 0.5 * vector2.X / num8 * num10);
				vector4.Y = vector.Y + offset / num9 * (vector3.Y - 0.5 * vector2.Y / num8 * num10);
				list.Add(point3);
				list2.Add(-vector4.Normalized());
			}
			List<Point> list3 = new List<Point>(num4 * 3 + 1);
			list3.Add(list[0]);
			for (int j = 1; j < num4; j++)
			{
				point2 = list[j - 1];
				point3 = list[j];
				double scalar = GeometryHelper.Distance(point2, point3) / 3.0;
				list3.Add(point2 + list2[j - 1] * scalar);
				list3.Add(point3 - list2[j] * scalar);
				list3.Add(point3);
			}
			return list3;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000BB5C File Offset: 0x00009D5C
		internal static double InnerCurveSelfIntersect(double radiusX, double radiusY, double thickness)
		{
			double num = 0.0;
			double num2 = 1.5707963267948966;
			bool flag = radiusX <= radiusY;
			Vector vector = default(Vector);
			while (!ArcGeometrySource.AreCloseEnough(num, num2))
			{
				double num3 = (num + num2) / 2.0;
				double num4 = Math.Cos(num3);
				double num5 = Math.Sin(num3);
				vector.X = radiusY * num5;
				vector.Y = radiusX * num4;
				vector.Normalize();
				if (flag)
				{
					double num6 = radiusX * num5 - vector.X * thickness;
					if (num6 > 0.0)
					{
						num2 = num3;
					}
					else if (num6 < 0.0)
					{
						num = num3;
					}
				}
				else
				{
					double num7 = radiusY * num4 - vector.Y * thickness;
					if (num7 < 0.0)
					{
						num2 = num3;
					}
					else if (num7 > 0.0)
					{
						num = num3;
					}
				}
			}
			double num8 = (num + num2) / 2.0;
			if (ArcGeometrySource.AreCloseEnough(num8, 0.0))
			{
				return 0.0;
			}
			if (!ArcGeometrySource.AreCloseEnough(num8, 1.5707963267948966))
			{
				return num8 * 180.0 / 3.141592653589793;
			}
			return 90.0;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000BCA2 File Offset: 0x00009EA2
		private static bool AreCloseEnough(double angleA, double angleB)
		{
			return Math.Abs(angleA - angleB) < 0.001;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000BCB7 File Offset: 0x00009EB7
		private static Size GetArcSize(Rect bound)
		{
			return new Size(bound.Width / 2.0, bound.Height / 2.0);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000BCE0 File Offset: 0x00009EE0
		private static double NormalizeAngle(double degree)
		{
			if (degree < 0.0 || degree > 360.0)
			{
				degree %= 360.0;
				if (degree < 0.0)
				{
					degree += 360.0;
				}
			}
			return degree;
		}

		// Token: 0x0400007A RID: 122
		private double relativeThickness;

		// Token: 0x0400007B RID: 123
		private double absoluteThickness;
	}
}
