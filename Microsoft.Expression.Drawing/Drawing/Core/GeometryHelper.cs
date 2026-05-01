using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x0200001C RID: 28
	public static class GeometryHelper
	{
		// Token: 0x06000118 RID: 280 RVA: 0x00007C2D File Offset: 0x00005E2D
		internal static Rect Bounds(this Size size)
		{
			return new Rect(0.0, 0.0, size.Width, size.Height);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00007C54 File Offset: 0x00005E54
		internal static bool HasValidArea(this Size size)
		{
			return size.Width > 0.0 && size.Height > 0.0 && !double.IsInfinity(size.Width) && !double.IsInfinity(size.Height);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00007CA4 File Offset: 0x00005EA4
		internal static Size Size(this Rect rect)
		{
			return new Size(rect.Width, rect.Height);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00007CB9 File Offset: 0x00005EB9
		internal static Point TopLeft(this Rect rect)
		{
			return new Point(rect.Left, rect.Top);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00007CCE File Offset: 0x00005ECE
		internal static Point TopRight(this Rect rect)
		{
			return new Point(rect.Right, rect.Top);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00007CE3 File Offset: 0x00005EE3
		internal static Point BottomRight(this Rect rect)
		{
			return new Point(rect.Right, rect.Bottom);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00007CF8 File Offset: 0x00005EF8
		internal static Point BottomLeft(this Rect rect)
		{
			return new Point(rect.Left, rect.Bottom);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00007D0D File Offset: 0x00005F0D
		internal static Point Center(this Rect rect)
		{
			return new Point(rect.X + rect.Width / 2.0, rect.Y + rect.Height / 2.0);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00007D48 File Offset: 0x00005F48
		internal static Thickness Subtract(this Rect lhs, Rect rhs)
		{
			return new Thickness(rhs.Left - lhs.Left, rhs.Top - lhs.Top, lhs.Right - rhs.Right, lhs.Bottom - rhs.Bottom);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00007D96 File Offset: 0x00005F96
		internal static Rect Resize(this Rect rect, double ratio)
		{
			return rect.Resize(ratio, ratio);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00007DA0 File Offset: 0x00005FA0
		internal static Rect Resize(this Rect rect, double ratioX, double ratioY)
		{
			Point point = rect.Center();
			double num = rect.Width * ratioX;
			double num2 = rect.Height * ratioY;
			return new Rect(point.X - num / 2.0, point.Y - num2 / 2.0, num, num2);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00007DF5 File Offset: 0x00005FF5
		internal static Rect ActualBounds(this FrameworkElement element)
		{
			return new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00007E1A File Offset: 0x0000601A
		internal static Thickness Negate(this Thickness thickness)
		{
			return new Thickness(-thickness.Left, -thickness.Top, -thickness.Right, -thickness.Bottom);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00007E41 File Offset: 0x00006041
		internal static Vector Subtract(this Point lhs, Point rhs)
		{
			return new Vector(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00007E66 File Offset: 0x00006066
		internal static Point Plus(this Point lhs, Point rhs)
		{
			return new Point(lhs.X + rhs.X, lhs.Y + rhs.Y);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00007E8B File Offset: 0x0000608B
		internal static Point Minus(this Point lhs, Point rhs)
		{
			return new Point(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007EB0 File Offset: 0x000060B0
		internal static Vector Normalized(this Vector vector)
		{
			Vector vector2 = new Vector(vector.X, vector.Y);
			double length = vector2.Length;
			if (MathHelper.IsVerySmall(length))
			{
				return new Vector(0.0, 1.0);
			}
			return vector2 / length;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00007F04 File Offset: 0x00006104
		internal static void ApplyTransform(this IList<Point> points, GeneralTransform transform)
		{
			for (int i = 0; i < points.Count; i++)
			{
				points[i] = transform.Transform(points[i]);
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00007F36 File Offset: 0x00006136
		public static PathGeometry ConvertToPathGeometry(string abbreviatedGeometry)
		{
			return PathGeometryHelper.ConvertToPathGeometry(abbreviatedGeometry);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00007F40 File Offset: 0x00006140
		public static void FlattenFigure(PathFigure figure, IList<Point> points, double tolerance)
		{
			bool removeRepeat = false;
			PathFigureHelper.FlattenFigure(figure, points, tolerance, removeRepeat);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00007F58 File Offset: 0x00006158
		internal static Point Lerp(Point pointA, Point pointB, double alpha)
		{
			return new Point(MathHelper.Lerp(pointA.X, pointB.X, alpha), MathHelper.Lerp(pointA.Y, pointB.Y, alpha));
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00007F87 File Offset: 0x00006187
		internal static Vector Lerp(Vector vectorA, Vector vectorB, double alpha)
		{
			return new Vector(MathHelper.Lerp(vectorA.X, vectorB.X, alpha), MathHelper.Lerp(vectorA.Y, vectorB.Y, alpha));
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007FB6 File Offset: 0x000061B6
		internal static Rect Inflate(Rect rect, double offset)
		{
			return GeometryHelper.Inflate(rect, new Thickness(offset));
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00007FC4 File Offset: 0x000061C4
		internal static Rect Inflate(Rect rect, double offsetX, double offsetY)
		{
			return GeometryHelper.Inflate(rect, new Thickness(offsetX, offsetY, offsetX, offsetY));
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00007FD5 File Offset: 0x000061D5
		internal static Rect Inflate(Rect rect, Size size)
		{
			return GeometryHelper.Inflate(rect, new Thickness(size.Width, size.Height, size.Width, size.Height));
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00008000 File Offset: 0x00006200
		internal static Rect Inflate(Rect rect, Thickness thickness)
		{
			double num = rect.Width + thickness.Left + thickness.Right;
			double num2 = rect.Height + thickness.Top + thickness.Bottom;
			double num3 = rect.X - thickness.Left;
			if (num < 0.0)
			{
				num3 += num / 2.0;
				num = 0.0;
			}
			double num4 = rect.Y - thickness.Top;
			if (num2 < 0.0)
			{
				num4 += num2 / 2.0;
				num2 = 0.0;
			}
			return new Rect(num3, num4, num, num2);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000080B0 File Offset: 0x000062B0
		internal static Point GetArcPoint(double degree)
		{
			double num = degree * 3.141592653589793 / 180.0;
			return new Point(0.5 + 0.5 * Math.Sin(num), 0.5 - 0.5 * Math.Cos(num));
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000810C File Offset: 0x0000630C
		internal static Point GetArcPoint(double degree, Rect bound)
		{
			Point arcPoint = GeometryHelper.GetArcPoint(degree);
			return GeometryHelper.RelativeToAbsolutePoint(bound, arcPoint);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00008128 File Offset: 0x00006328
		internal static double GetArcAngle(Point point)
		{
			return Math.Atan2(point.Y - 0.5, point.X - 0.5) * 180.0 / 3.141592653589793 + 90.0;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000817C File Offset: 0x0000637C
		internal static double GetArcAngle(Point point, Rect bound)
		{
			Point point2 = GeometryHelper.AbsoluteToRelativePoint(bound, point);
			return GeometryHelper.GetArcAngle(point2);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00008198 File Offset: 0x00006398
		internal static Transform RelativeTransform(Rect from, Rect to)
		{
			Point point = from.Center();
			Point point2 = to.Center();
			TransformGroup transformGroup = new TransformGroup();
			TransformGroup transformGroup2 = transformGroup;
			TransformCollection transformCollection = new TransformCollection();
			transformCollection.Add(new TranslateTransform
			{
				X = -point.X,
				Y = -point.Y
			});
			transformCollection.Add(new ScaleTransform
			{
				ScaleX = MathHelper.SafeDivide(to.Width, from.Width, 1.0),
				ScaleY = MathHelper.SafeDivide(to.Height, from.Height, 1.0)
			});
			transformCollection.Add(new TranslateTransform
			{
				X = point2.X,
				Y = point2.Y
			});
			transformGroup2.Children = transformCollection;
			return transformGroup;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00008274 File Offset: 0x00006474
		internal static GeneralTransform RelativeTransform(UIElement from, UIElement to)
		{
			if (from == null || to == null)
			{
				return null;
			}
			GeneralTransform result;
			try
			{
				result = from.TransformToVisual(to);
			}
			catch (ArgumentException)
			{
				result = null;
			}
			catch (InvalidOperationException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000082BC File Offset: 0x000064BC
		internal static Point SafeTransform(GeneralTransform transform, Point point)
		{
			Point result = point;
			if (transform != null && transform.TryTransform(point, ref result))
			{
				return result;
			}
			return point;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000082DC File Offset: 0x000064DC
		internal static Point RelativeToAbsolutePoint(Rect bound, Point relative)
		{
			return new Point(bound.X + relative.X * bound.Width, bound.Y + relative.Y * bound.Height);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00008314 File Offset: 0x00006514
		internal static Point AbsoluteToRelativePoint(Rect bound, Point absolute)
		{
			return new Point(MathHelper.SafeDivide(absolute.X - bound.X, bound.Width, 1.0), MathHelper.SafeDivide(absolute.Y - bound.Y, bound.Height, 1.0));
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00008370 File Offset: 0x00006570
		internal static Rect GetStretchBound(Rect logicalBound, Stretch stretch, Size aspectRatio)
		{
			if (stretch == null)
			{
				stretch = 1;
			}
			if (stretch == 1 || !aspectRatio.HasValidArea())
			{
				return logicalBound;
			}
			Point point = logicalBound.Center();
			if (stretch == 2)
			{
				if (aspectRatio.Width * logicalBound.Height < logicalBound.Width * aspectRatio.Height)
				{
					logicalBound.Width = logicalBound.Height * aspectRatio.Width / aspectRatio.Height;
				}
				else
				{
					logicalBound.Height = logicalBound.Width * aspectRatio.Height / aspectRatio.Width;
				}
			}
			else if (stretch == 3)
			{
				if (aspectRatio.Width * logicalBound.Height < logicalBound.Width * aspectRatio.Height)
				{
					logicalBound.Height = logicalBound.Width * aspectRatio.Height / aspectRatio.Width;
				}
				else
				{
					logicalBound.Width = logicalBound.Height * aspectRatio.Width / aspectRatio.Height;
				}
			}
			return new Rect(point.X - logicalBound.Width / 2.0, point.Y - logicalBound.Height / 2.0, logicalBound.Width, logicalBound.Height);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000084A6 File Offset: 0x000066A6
		internal static Point Midpoint(Point lhs, Point rhs)
		{
			return new Point((lhs.X + rhs.X) / 2.0, (lhs.Y + rhs.Y) / 2.0);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000084DF File Offset: 0x000066DF
		internal static double Dot(Vector lhs, Vector rhs)
		{
			return lhs.X * rhs.X + lhs.Y * rhs.Y;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00008500 File Offset: 0x00006700
		internal static double Dot(Point lhs, Point rhs)
		{
			return lhs.X * rhs.X + lhs.Y * rhs.Y;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00008524 File Offset: 0x00006724
		internal static double Distance(Point lhs, Point rhs)
		{
			double num = lhs.X - rhs.X;
			double num2 = lhs.Y - rhs.Y;
			return Math.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00008560 File Offset: 0x00006760
		internal static double SquaredDistance(Point lhs, Point rhs)
		{
			double num = lhs.X - rhs.X;
			double num2 = lhs.Y - rhs.Y;
			return num * num + num2 * num2;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00008594 File Offset: 0x00006794
		internal static double Determinant(Point lhs, Point rhs)
		{
			return lhs.X * rhs.Y - lhs.Y * rhs.X;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000085B5 File Offset: 0x000067B5
		internal static Vector Normal(Point lhs, Point rhs)
		{
			return new Vector(lhs.Y - rhs.Y, rhs.X - lhs.X).Normalized();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000085DF File Offset: 0x000067DF
		internal static Vector Perpendicular(this Vector vector)
		{
			return new Vector(-vector.Y, vector.X);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000085F8 File Offset: 0x000067F8
		internal static bool GeometryEquals(Geometry firstGeometry, Geometry secondGeometry)
		{
			if (firstGeometry == secondGeometry)
			{
				return true;
			}
			if (firstGeometry == null || secondGeometry == null)
			{
				return false;
			}
			if (firstGeometry.GetType() != secondGeometry.GetType())
			{
				return false;
			}
			if (!firstGeometry.Transform.TransformEquals(secondGeometry.Transform))
			{
				return false;
			}
			PathGeometry pathGeometry = firstGeometry as PathGeometry;
			PathGeometry pathGeometry2 = secondGeometry as PathGeometry;
			if (pathGeometry != null && pathGeometry2 != null)
			{
				string text = pathGeometry.ToString();
				string text2 = pathGeometry2.ToString();
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
				{
					return text == text2;
				}
				return GeometryHelper.PathGeometryEquals(pathGeometry, pathGeometry2);
			}
			else
			{
				LineGeometry lineGeometry = firstGeometry as LineGeometry;
				LineGeometry lineGeometry2 = secondGeometry as LineGeometry;
				if (lineGeometry != null && lineGeometry2 != null)
				{
					return GeometryHelper.LineGeometryEquals(lineGeometry, lineGeometry2);
				}
				RectangleGeometry rectangleGeometry = firstGeometry as RectangleGeometry;
				RectangleGeometry rectangleGeometry2 = secondGeometry as RectangleGeometry;
				if (rectangleGeometry != null && rectangleGeometry2 != null)
				{
					return GeometryHelper.RectangleGeometryEquals(rectangleGeometry, rectangleGeometry2);
				}
				EllipseGeometry ellipseGeometry = firstGeometry as EllipseGeometry;
				EllipseGeometry ellipseGeometry2 = secondGeometry as EllipseGeometry;
				if (ellipseGeometry != null && ellipseGeometry2 != null)
				{
					return GeometryHelper.EllipseGeometryEquals(ellipseGeometry, ellipseGeometry2);
				}
				GeometryGroup geometryGroup = firstGeometry as GeometryGroup;
				GeometryGroup geometryGroup2 = secondGeometry as GeometryGroup;
				return geometryGroup != null && geometryGroup2 != null && GeometryHelper.GeometryGroupEquals(geometryGroup, geometryGroup2);
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00008704 File Offset: 0x00006904
		internal static bool PathGeometryEquals(PathGeometry firstGeometry, PathGeometry secondGeometry)
		{
			if (firstGeometry.FillRule != secondGeometry.FillRule)
			{
				return false;
			}
			if (firstGeometry.Figures.Count != secondGeometry.Figures.Count)
			{
				return false;
			}
			for (int i = 0; i < firstGeometry.Figures.Count; i++)
			{
				if (!GeometryHelper.PathFigureEquals(firstGeometry.Figures[i], secondGeometry.Figures[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00008774 File Offset: 0x00006974
		private static bool PathFigureEquals(PathFigure firstFigure, PathFigure secondFigure)
		{
			if (firstFigure.IsClosed != secondFigure.IsClosed)
			{
				return false;
			}
			if (firstFigure.IsFilled != secondFigure.IsFilled)
			{
				return false;
			}
			if (firstFigure.StartPoint != secondFigure.StartPoint)
			{
				return false;
			}
			for (int i = 0; i < firstFigure.Segments.Count; i++)
			{
				if (!GeometryHelper.PathSegmentEquals(firstFigure.Segments[i], secondFigure.Segments[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000087F0 File Offset: 0x000069F0
		private static bool PathSegmentEquals(PathSegment firstSegment, PathSegment secondSegment)
		{
			if (firstSegment == secondSegment)
			{
				return true;
			}
			if (firstSegment == null || secondSegment == null)
			{
				return false;
			}
			if (firstSegment.GetType() != secondSegment.GetType())
			{
				return false;
			}
			if (firstSegment.IsStroked() != secondSegment.IsStroked())
			{
				return false;
			}
			if (firstSegment.IsSmoothJoin() != secondSegment.IsSmoothJoin())
			{
				return false;
			}
			LineSegment lineSegment = firstSegment as LineSegment;
			LineSegment lineSegment2 = secondSegment as LineSegment;
			if (lineSegment != null && lineSegment2 != null)
			{
				return GeometryHelper.LineSegmentEquals(lineSegment, lineSegment2);
			}
			BezierSegment bezierSegment = firstSegment as BezierSegment;
			BezierSegment bezierSegment2 = secondSegment as BezierSegment;
			if (bezierSegment != null && bezierSegment2 != null)
			{
				return GeometryHelper.BezierSegmentEquals(bezierSegment, bezierSegment2);
			}
			QuadraticBezierSegment quadraticBezierSegment = firstSegment as QuadraticBezierSegment;
			QuadraticBezierSegment quadraticBezierSegment2 = secondSegment as QuadraticBezierSegment;
			if (quadraticBezierSegment != null && quadraticBezierSegment2 != null)
			{
				return GeometryHelper.QuadraticBezierSegmentEquals(quadraticBezierSegment, quadraticBezierSegment2);
			}
			ArcSegment arcSegment = firstSegment as ArcSegment;
			ArcSegment arcSegment2 = secondSegment as ArcSegment;
			if (arcSegment != null && arcSegment2 != null)
			{
				return GeometryHelper.ArcSegmentEquals(arcSegment, arcSegment2);
			}
			PolyLineSegment polyLineSegment = firstSegment as PolyLineSegment;
			PolyLineSegment polyLineSegment2 = secondSegment as PolyLineSegment;
			if (polyLineSegment != null && polyLineSegment2 != null)
			{
				return GeometryHelper.PolyLineSegmentEquals(polyLineSegment, polyLineSegment2);
			}
			PolyBezierSegment polyBezierSegment = firstSegment as PolyBezierSegment;
			PolyBezierSegment polyBezierSegment2 = secondSegment as PolyBezierSegment;
			if (polyBezierSegment != null && polyBezierSegment2 != null)
			{
				return GeometryHelper.PolyBezierSegmentEquals(polyBezierSegment, polyBezierSegment2);
			}
			PolyQuadraticBezierSegment polyQuadraticBezierSegment = firstSegment as PolyQuadraticBezierSegment;
			PolyQuadraticBezierSegment polyQuadraticBezierSegment2 = secondSegment as PolyQuadraticBezierSegment;
			return polyQuadraticBezierSegment != null && polyQuadraticBezierSegment2 != null && GeometryHelper.PolyQuadraticBezierSegmentEquals(polyQuadraticBezierSegment, polyQuadraticBezierSegment2);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000891E File Offset: 0x00006B1E
		private static bool LineSegmentEquals(LineSegment firstLineSegment, LineSegment secondLineSegment)
		{
			return firstLineSegment.Point == secondLineSegment.Point;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00008931 File Offset: 0x00006B31
		private static bool BezierSegmentEquals(BezierSegment firstBezierSegment, BezierSegment secondBezierSegment)
		{
			return firstBezierSegment.Point1 == secondBezierSegment.Point1 && firstBezierSegment.Point2 == secondBezierSegment.Point2 && firstBezierSegment.Point3 == secondBezierSegment.Point3;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000896C File Offset: 0x00006B6C
		private static bool QuadraticBezierSegmentEquals(QuadraticBezierSegment firstQuadraticBezierSegment, QuadraticBezierSegment secondQuadraticBezierSegment)
		{
			return firstQuadraticBezierSegment.Point1 == secondQuadraticBezierSegment.Point1 && firstQuadraticBezierSegment.Point1 == secondQuadraticBezierSegment.Point1;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00008994 File Offset: 0x00006B94
		private static bool ArcSegmentEquals(ArcSegment firstArcSegment, ArcSegment secondArcSegment)
		{
			return firstArcSegment.Point == secondArcSegment.Point && firstArcSegment.IsLargeArc == secondArcSegment.IsLargeArc && firstArcSegment.RotationAngle == secondArcSegment.RotationAngle && firstArcSegment.Size == secondArcSegment.Size && firstArcSegment.SweepDirection == secondArcSegment.SweepDirection;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000089F4 File Offset: 0x00006BF4
		private static bool PolyLineSegmentEquals(PolyLineSegment firstPolyLineSegment, PolyLineSegment secondPolyLineSegment)
		{
			if (firstPolyLineSegment.Points.Count != secondPolyLineSegment.Points.Count)
			{
				return false;
			}
			for (int i = 0; i < firstPolyLineSegment.Points.Count; i++)
			{
				if (firstPolyLineSegment.Points[i] != secondPolyLineSegment.Points[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00008A54 File Offset: 0x00006C54
		private static bool PolyBezierSegmentEquals(PolyBezierSegment firstPolyBezierSegment, PolyBezierSegment secondPolyBezierSegment)
		{
			if (firstPolyBezierSegment.Points.Count != secondPolyBezierSegment.Points.Count)
			{
				return false;
			}
			for (int i = 0; i < firstPolyBezierSegment.Points.Count; i++)
			{
				if (firstPolyBezierSegment.Points[i] != secondPolyBezierSegment.Points[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00008AB4 File Offset: 0x00006CB4
		private static bool PolyQuadraticBezierSegmentEquals(PolyQuadraticBezierSegment firstPolyQuadraticBezierSegment, PolyQuadraticBezierSegment secondPolyQuadraticBezierSegment)
		{
			if (firstPolyQuadraticBezierSegment.Points.Count != secondPolyQuadraticBezierSegment.Points.Count)
			{
				return false;
			}
			for (int i = 0; i < firstPolyQuadraticBezierSegment.Points.Count; i++)
			{
				if (firstPolyQuadraticBezierSegment.Points[i] != secondPolyQuadraticBezierSegment.Points[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00008B13 File Offset: 0x00006D13
		private static bool EllipseGeometryEquals(EllipseGeometry firstGeometry, EllipseGeometry secondGeometry)
		{
			return firstGeometry.Center == secondGeometry.Center && firstGeometry.RadiusX == secondGeometry.RadiusX && firstGeometry.RadiusY == secondGeometry.RadiusY;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00008B46 File Offset: 0x00006D46
		private static bool RectangleGeometryEquals(RectangleGeometry firstGeometry, RectangleGeometry secondGeometry)
		{
			return firstGeometry.Rect == secondGeometry.Rect && firstGeometry.RadiusX == secondGeometry.RadiusX && firstGeometry.RadiusY == secondGeometry.RadiusY;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00008B79 File Offset: 0x00006D79
		private static bool LineGeometryEquals(LineGeometry firstGeometry, LineGeometry secondGeometry)
		{
			return firstGeometry.StartPoint == secondGeometry.StartPoint && firstGeometry.EndPoint == secondGeometry.EndPoint;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00008BA4 File Offset: 0x00006DA4
		private static bool GeometryGroupEquals(GeometryGroup firstGeometry, GeometryGroup secondGeometry)
		{
			if (firstGeometry.FillRule != secondGeometry.FillRule)
			{
				return false;
			}
			if (firstGeometry.Children.Count != secondGeometry.Children.Count)
			{
				return false;
			}
			for (int i = 0; i < firstGeometry.Children.Count; i++)
			{
				if (!GeometryHelper.GeometryEquals(firstGeometry.Children[i], secondGeometry.Children[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00008C14 File Offset: 0x00006E14
		internal static bool EnsureGeometryType<T>(out T result, ref Geometry value, Func<T> factory) where T : Geometry
		{
			result = (value as T);
			if (result == null)
			{
				value = (result = factory.Invoke());
				return true;
			}
			return false;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00008C5C File Offset: 0x00006E5C
		internal static bool EnsureSegmentType<T>(out T result, IList<PathSegment> list, int index, Func<T> factory) where T : PathSegment
		{
			result = (list[index] as T);
			if (result == null)
			{
				list[index] = (result = factory.Invoke());
				return true;
			}
			return false;
		}
	}
}
