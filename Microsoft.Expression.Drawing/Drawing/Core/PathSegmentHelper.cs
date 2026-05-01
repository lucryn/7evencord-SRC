using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x0200000C RID: 12
	internal static class PathSegmentHelper
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x000047A4 File Offset: 0x000029A4
		public static PathSegment ArcToBezierSegments(ArcSegment arcSegment, Point startPoint)
		{
			bool isStroked = arcSegment.IsStroked();
			Point[] array;
			int num;
			PathSegmentHelper.ArcToBezierHelper.ArcToBezier(startPoint.X, startPoint.Y, arcSegment.Size.Width, arcSegment.Size.Height, arcSegment.RotationAngle, arcSegment.IsLargeArc, arcSegment.SweepDirection == 1, arcSegment.Point.X, arcSegment.Point.Y, out array, out num);
			if (num == -1)
			{
				return null;
			}
			if (num == 0)
			{
				return PathSegmentHelper.CreateLineSegment(arcSegment.Point, isStroked);
			}
			if (num == 1)
			{
				return PathSegmentHelper.CreateBezierSegment(array[0], array[1], array[2], isStroked);
			}
			return PathSegmentHelper.CreatePolyBezierSegment(array, 0, num * 3, isStroked);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000486F File Offset: 0x00002A6F
		private static void SetIsStroked(this PathSegment segment, bool isStroked)
		{
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004874 File Offset: 0x00002A74
		public static LineSegment CreateLineSegment(Point point, bool isStroked = true)
		{
			LineSegment lineSegment = new LineSegment();
			lineSegment.Point = point;
			lineSegment.SetIsStroked(isStroked);
			return lineSegment;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004898 File Offset: 0x00002A98
		public static QuadraticBezierSegment CreateQuadraticBezierSegment(Point point1, Point point2, bool isStroked = true)
		{
			QuadraticBezierSegment quadraticBezierSegment = new QuadraticBezierSegment();
			quadraticBezierSegment.Point1 = point1;
			quadraticBezierSegment.Point2 = point2;
			quadraticBezierSegment.SetIsStroked(isStroked);
			return quadraticBezierSegment;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000048C4 File Offset: 0x00002AC4
		public static BezierSegment CreateBezierSegment(Point point1, Point point2, Point point3, bool isStroked = true)
		{
			BezierSegment bezierSegment = new BezierSegment();
			bezierSegment.Point1 = point1;
			bezierSegment.Point2 = point2;
			bezierSegment.Point3 = point3;
			bezierSegment.SetIsStroked(isStroked);
			return bezierSegment;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000048F4 File Offset: 0x00002AF4
		public static PolyBezierSegment CreatePolyBezierSegment(IList<Point> points, int start, int count, bool isStroked = true)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			count = count / 3 * 3;
			if (count < 0 || points.Count < start + count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			PolyBezierSegment polyBezierSegment = new PolyBezierSegment();
			polyBezierSegment.Points = new PointCollection();
			for (int i = 0; i < count; i++)
			{
				polyBezierSegment.Points.Add(points[start + i]);
			}
			polyBezierSegment.SetIsStroked(isStroked);
			return polyBezierSegment;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000496C File Offset: 0x00002B6C
		public static PolyQuadraticBezierSegment CreatePolyQuadraticBezierSegment(IList<Point> points, int start, int count, bool isStroked = true)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			count = count / 2 * 2;
			if (count < 0 || points.Count < start + count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			PolyQuadraticBezierSegment polyQuadraticBezierSegment = new PolyQuadraticBezierSegment();
			polyQuadraticBezierSegment.Points = new PointCollection();
			for (int i = 0; i < count; i++)
			{
				polyQuadraticBezierSegment.Points.Add(points[start + i]);
			}
			polyQuadraticBezierSegment.SetIsStroked(isStroked);
			return polyQuadraticBezierSegment;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000049E4 File Offset: 0x00002BE4
		public static PolyLineSegment CreatePolylineSegment(IList<Point> points, int start, int count, bool isStroked = true)
		{
			if (count < 0 || points.Count < start + count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			PolyLineSegment polyLineSegment = new PolyLineSegment();
			polyLineSegment.Points = new PointCollection();
			for (int i = 0; i < count; i++)
			{
				polyLineSegment.Points.Add(points[start + i]);
			}
			polyLineSegment.SetIsStroked(isStroked);
			return polyLineSegment;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004A44 File Offset: 0x00002C44
		public static ArcSegment CreateArcSegment(Point point, Size size, bool isLargeArc, bool clockwise, double rotationAngle = 0.0, bool isStroked = true)
		{
			ArcSegment arcSegment = new ArcSegment();
			arcSegment.SetIfDifferent(ArcSegment.PointProperty, point);
			arcSegment.SetIfDifferent(ArcSegment.SizeProperty, size);
			arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, isLargeArc);
			arcSegment.SetIfDifferent(ArcSegment.SweepDirectionProperty, clockwise ? 1 : 0);
			arcSegment.SetIfDifferent(ArcSegment.RotationAngleProperty, rotationAngle);
			arcSegment.SetIsStroked(isStroked);
			return arcSegment;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004AC4 File Offset: 0x00002CC4
		public static bool SyncPolylineSegment(PathSegmentCollection collection, int index, IList<Point> points, int start, int count)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (index < 0 || index >= collection.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (points.Count < start + count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			bool flag = false;
			PolyLineSegment polyLineSegment;
			if ((polyLineSegment = (collection[index] as PolyLineSegment)) == null)
			{
				polyLineSegment = (collection[index] = new PolyLineSegment());
				flag = true;
			}
			flag |= polyLineSegment.Points.EnsureListCount(count, null);
			for (int i = 0; i < count; i++)
			{
				if (polyLineSegment.Points[i] != points[i + start])
				{
					polyLineSegment.Points[i] = points[i + start];
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004BB0 File Offset: 0x00002DB0
		public static bool SyncPolyBezierSegment(PathSegmentCollection collection, int index, IList<Point> points, int start, int count)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (index < 0 || index >= collection.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (points.Count < start + count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			bool result = false;
			count = count / 3 * 3;
			PolyBezierSegment polyBezierSegment;
			if ((polyBezierSegment = (collection[index] as PolyBezierSegment)) == null)
			{
				polyBezierSegment = (collection[index] = new PolyBezierSegment());
				result = true;
			}
			polyBezierSegment.Points.EnsureListCount(count, null);
			for (int i = 0; i < count; i++)
			{
				if (polyBezierSegment.Points[i] != points[i + start])
				{
					polyBezierSegment.Points[i] = points[i + start];
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004CA1 File Offset: 0x00002EA1
		public static bool IsEmpty(this PathSegment segment)
		{
			return segment.GetPointCount() == 0;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004CAC File Offset: 0x00002EAC
		public static int GetPointCount(this PathSegment segment)
		{
			if (segment is ArcSegment)
			{
				return 1;
			}
			if (segment is LineSegment)
			{
				return 1;
			}
			if (segment is QuadraticBezierSegment)
			{
				return 2;
			}
			if (segment is BezierSegment)
			{
				return 3;
			}
			PolyLineSegment polyLineSegment;
			if ((polyLineSegment = (segment as PolyLineSegment)) != null)
			{
				return polyLineSegment.Points.Count;
			}
			PolyQuadraticBezierSegment polyQuadraticBezierSegment;
			if ((polyQuadraticBezierSegment = (segment as PolyQuadraticBezierSegment)) != null)
			{
				return polyQuadraticBezierSegment.Points.Count / 2 * 2;
			}
			PolyBezierSegment polyBezierSegment;
			if ((polyBezierSegment = (segment as PolyBezierSegment)) != null)
			{
				return polyBezierSegment.Points.Count / 3 * 3;
			}
			return 0;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004D2C File Offset: 0x00002F2C
		public static Point GetLastPoint(this PathSegment segment)
		{
			return segment.GetPoint(-1);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004D35 File Offset: 0x00002F35
		public static Point GetPoint(this PathSegment segment, int index)
		{
			return PathSegmentHelper.PathSegmentImplementation.Create(segment).GetPoint(index);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004D43 File Offset: 0x00002F43
		public static void FlattenSegment(this PathSegment segment, IList<Point> points, Point start, double tolerance)
		{
			PathSegmentHelper.PathSegmentImplementation.Create(segment, start).Flatten(points, tolerance);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004F08 File Offset: 0x00003108
		public static IEnumerable<SimpleSegment> GetSimpleSegments(this PathSegment segment, Point start)
		{
			PathSegmentHelper.PathSegmentImplementation implementation = PathSegmentHelper.PathSegmentImplementation.Create(segment, start);
			foreach (SimpleSegment simpleSegment in implementation.GetSimpleSegments())
			{
				yield return simpleSegment;
			}
			yield break;
		}

		// Token: 0x0200000D RID: 13
		private static class ArcToBezierHelper
		{
			// Token: 0x060000BA RID: 186 RVA: 0x00004F2C File Offset: 0x0000312C
			public static void ArcToBezier(double xStart, double yStart, double xRadius, double yRadius, double rRotation, bool fLargeArc, bool fSweepUp, double xEnd, double yEnd, out Point[] pPt, out int cPieces)
			{
				double num = 1E-06;
				pPt = new Point[12];
				double num2 = num * num;
				bool flag = false;
				cPieces = -1;
				double num3 = 0.5 * (xEnd - xStart);
				double num4 = 0.5 * (yEnd - yStart);
				double num5 = num3 * num3 + num4 * num4;
				if (num5 < num2)
				{
					return;
				}
				if (!PathSegmentHelper.ArcToBezierHelper.AcceptRadius(num5, num2, ref xRadius) || !PathSegmentHelper.ArcToBezierHelper.AcceptRadius(num5, num2, ref yRadius))
				{
					cPieces = 0;
					return;
				}
				double num6;
				double num7;
				if (Math.Abs(rRotation) < num)
				{
					num6 = 1.0;
					num7 = 0.0;
				}
				else
				{
					rRotation = -rRotation * 3.141592653589793 / 180.0;
					num6 = Math.Cos(rRotation);
					num7 = Math.Sin(rRotation);
					double num8 = num3 * num6 - num4 * num7;
					num4 = num3 * num7 + num4 * num6;
					num3 = num8;
				}
				num3 /= xRadius;
				num4 /= yRadius;
				num5 = num3 * num3 + num4 * num4;
				double num11;
				double num10;
				if (num5 > 1.0)
				{
					double num9 = Math.Sqrt(num5);
					xRadius *= num9;
					yRadius *= num9;
					num10 = (num11 = 0.0);
					flag = true;
					num3 /= num9;
					num4 /= num9;
				}
				else
				{
					double num12 = Math.Sqrt((1.0 - num5) / num5);
					if (fLargeArc != fSweepUp)
					{
						num11 = -num12 * num4;
						num10 = num12 * num3;
					}
					else
					{
						num11 = num12 * num4;
						num10 = -num12 * num3;
					}
				}
				Point point;
				point..ctor(-num3 - num11, -num4 - num10);
				Point point2;
				point2..ctor(num3 - num11, num4 - num10);
				Matrix matrix;
				matrix..ctor(num6 * xRadius, -num7 * xRadius, num7 * yRadius, num6 * yRadius, 0.5 * (xEnd + xStart), 0.5 * (yEnd + yStart));
				if (!flag)
				{
					matrix.OffsetX += matrix.M11 * num11 + matrix.M21 * num10;
					matrix.OffsetY += matrix.M12 * num11 + matrix.M22 * num10;
				}
				double num13;
				double num14;
				PathSegmentHelper.ArcToBezierHelper.GetArcAngle(point, point2, fLargeArc, fSweepUp, out num13, out num14, out cPieces);
				double num15 = PathSegmentHelper.ArcToBezierHelper.GetBezierDistance(num13, 1.0);
				if (!fSweepUp)
				{
					num15 = -num15;
				}
				Point rhs;
				rhs..ctor(-num15 * point.Y, num15 * point.X);
				int num16 = 0;
				pPt = new Point[cPieces * 3];
				Point point4;
				for (int i = 1; i < cPieces; i++)
				{
					Point point3;
					point3..ctor(point.X * num13 - point.Y * num14, point.X * num14 + point.Y * num13);
					point4..ctor(-num15 * point3.Y, num15 * point3.X);
					pPt[num16++] = matrix.Transform(point.Plus(rhs));
					pPt[num16++] = matrix.Transform(point3.Minus(point4));
					pPt[num16++] = matrix.Transform(point3);
					point = point3;
					rhs = point4;
				}
				point4..ctor(-num15 * point2.Y, num15 * point2.X);
				pPt[num16++] = matrix.Transform(point.Plus(rhs));
				pPt[num16++] = matrix.Transform(point2.Minus(point4));
				pPt[num16] = new Point(xEnd, yEnd);
			}

			// Token: 0x060000BB RID: 187 RVA: 0x000052D0 File Offset: 0x000034D0
			private static void GetArcAngle(Point ptStart, Point ptEnd, bool fLargeArc, bool fSweepUp, out double rCosArcAngle, out double rSinArcAngle, out int cPieces)
			{
				rCosArcAngle = GeometryHelper.Dot(ptStart, ptEnd);
				rSinArcAngle = GeometryHelper.Determinant(ptStart, ptEnd);
				if (rCosArcAngle >= 0.0)
				{
					if (!fLargeArc)
					{
						cPieces = 1;
						return;
					}
					cPieces = 4;
				}
				else if (fLargeArc)
				{
					cPieces = 3;
				}
				else
				{
					cPieces = 2;
				}
				double num = Math.Atan2(rSinArcAngle, rCosArcAngle);
				if (fSweepUp)
				{
					if (num < 0.0)
					{
						num += 6.283185307179586;
					}
				}
				else if (num > 0.0)
				{
					num -= 6.283185307179586;
				}
				num /= (double)cPieces;
				rCosArcAngle = Math.Cos(num);
				rSinArcAngle = Math.Sin(num);
			}

			// Token: 0x060000BC RID: 188 RVA: 0x00005374 File Offset: 0x00003574
			private static double GetBezierDistance(double rDot, double rRadius = 1.0)
			{
				double num = rRadius * rRadius;
				double result = 0.0;
				double num2 = 0.5 * (num + rDot);
				if (num2 >= 0.0)
				{
					double num3 = num - num2;
					if (num3 > 0.0)
					{
						double num4 = Math.Sqrt(num3);
						double num5 = 4.0 * (rRadius - Math.Sqrt(num2)) / 3.0;
						if (num5 <= num4 * 1E-06)
						{
							result = 0.0;
						}
						else
						{
							result = num5 / num4;
						}
					}
				}
				return result;
			}

			// Token: 0x060000BD RID: 189 RVA: 0x00005404 File Offset: 0x00003604
			private static bool AcceptRadius(double rHalfChord2, double rFuzz2, ref double rRadius)
			{
				bool flag = rRadius * rRadius > rHalfChord2 * rFuzz2;
				if (flag && rRadius < 0.0)
				{
					rRadius = -rRadius;
				}
				return flag;
			}
		}

		// Token: 0x0200000E RID: 14
		private abstract class PathSegmentImplementation
		{
			// Token: 0x17000037 RID: 55
			// (get) Token: 0x060000BE RID: 190 RVA: 0x00005432 File Offset: 0x00003632
			// (set) Token: 0x060000BF RID: 191 RVA: 0x0000543A File Offset: 0x0000363A
			public Point Start { get; private set; }

			// Token: 0x060000C0 RID: 192
			public abstract void Flatten(IList<Point> points, double tolerance);

			// Token: 0x060000C1 RID: 193
			public abstract Point GetPoint(int index);

			// Token: 0x060000C2 RID: 194
			public abstract IEnumerable<SimpleSegment> GetSimpleSegments();

			// Token: 0x060000C3 RID: 195 RVA: 0x00005444 File Offset: 0x00003644
			public static PathSegmentHelper.PathSegmentImplementation Create(PathSegment segment, Point start)
			{
				PathSegmentHelper.PathSegmentImplementation pathSegmentImplementation = PathSegmentHelper.PathSegmentImplementation.Create(segment);
				pathSegmentImplementation.Start = start;
				return pathSegmentImplementation;
			}

			// Token: 0x060000C4 RID: 196 RVA: 0x00005460 File Offset: 0x00003660
			public static PathSegmentHelper.PathSegmentImplementation Create(PathSegment segment)
			{
				PathSegmentHelper.PathSegmentImplementation result;
				if ((result = PathSegmentHelper.BezierSegmentImplementation.Create(segment as BezierSegment)) == null && (result = PathSegmentHelper.LineSegmentImplementation.Create(segment as LineSegment)) == null && (result = PathSegmentHelper.ArcSegmentImplementation.Create(segment as ArcSegment)) == null && (result = PathSegmentHelper.PolyLineSegmentImplementation.Create(segment as PolyLineSegment)) == null && (result = PathSegmentHelper.PolyBezierSegmentImplementation.Create(segment as PolyBezierSegment)) == null && (result = PathSegmentHelper.QuadraticBezierSegmentImplementation.Create(segment as QuadraticBezierSegment)) == null && (result = PathSegmentHelper.PolyQuadraticBezierSegmentImplementation.Create(segment as PolyQuadraticBezierSegment)) == null)
				{
					throw new NotImplementedException();
				}
				return result;
			}
		}

		// Token: 0x0200000F RID: 15
		private class BezierSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			// Token: 0x060000C6 RID: 198 RVA: 0x000054E8 File Offset: 0x000036E8
			public static PathSegmentHelper.PathSegmentImplementation Create(BezierSegment source)
			{
				if (source != null)
				{
					return new PathSegmentHelper.BezierSegmentImplementation
					{
						segment = source
					};
				}
				return null;
			}

			// Token: 0x060000C7 RID: 199 RVA: 0x00005508 File Offset: 0x00003708
			public override void Flatten(IList<Point> points, double tolerance)
			{
				Point[] controlPoints = new Point[]
				{
					base.Start,
					this.segment.Point1,
					this.segment.Point2,
					this.segment.Point3
				};
				List<Point> list = new List<Point>();
				BezierCurveFlattener.FlattenCubic(controlPoints, tolerance, list, true, null);
				points.AddRange(list);
			}

			// Token: 0x060000C8 RID: 200 RVA: 0x0000558C File Offset: 0x0000378C
			public override Point GetPoint(int index)
			{
				if (index < -1 || index > 2)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index == 0)
				{
					return this.segment.Point1;
				}
				if (index == 1)
				{
					return this.segment.Point2;
				}
				return this.segment.Point3;
			}

			// Token: 0x060000C9 RID: 201 RVA: 0x000056D8 File Offset: 0x000038D8
			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				yield return SimpleSegment.Create(base.Start, this.segment.Point1, this.segment.Point2, this.segment.Point3);
				yield break;
			}

			// Token: 0x04000032 RID: 50
			private BezierSegment segment;
		}

		// Token: 0x02000010 RID: 16
		private class QuadraticBezierSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			// Token: 0x060000CB RID: 203 RVA: 0x00005700 File Offset: 0x00003900
			public static PathSegmentHelper.PathSegmentImplementation Create(QuadraticBezierSegment source)
			{
				if (source != null)
				{
					return new PathSegmentHelper.QuadraticBezierSegmentImplementation
					{
						segment = source
					};
				}
				return null;
			}

			// Token: 0x060000CC RID: 204 RVA: 0x00005720 File Offset: 0x00003920
			public override void Flatten(IList<Point> points, double tolerance)
			{
				Point[] controlPoints = new Point[]
				{
					base.Start,
					this.segment.Point1,
					this.segment.Point2
				};
				List<Point> list = new List<Point>();
				BezierCurveFlattener.FlattenQuadratic(controlPoints, tolerance, list, true, null);
				points.AddRange(list);
			}

			// Token: 0x060000CD RID: 205 RVA: 0x0000578D File Offset: 0x0000398D
			public override Point GetPoint(int index)
			{
				if (index < -1 || index > 1)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index == 0)
				{
					return this.segment.Point1;
				}
				return this.segment.Point2;
			}

			// Token: 0x060000CE RID: 206 RVA: 0x000058B8 File Offset: 0x00003AB8
			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				yield return SimpleSegment.Create(base.Start, this.segment.Point1, this.segment.Point2);
				yield break;
			}

			// Token: 0x04000033 RID: 51
			private QuadraticBezierSegment segment;
		}

		// Token: 0x02000011 RID: 17
		private class PolyBezierSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			// Token: 0x060000D0 RID: 208 RVA: 0x000058E0 File Offset: 0x00003AE0
			public static PathSegmentHelper.PathSegmentImplementation Create(PolyBezierSegment source)
			{
				if (source != null)
				{
					return new PathSegmentHelper.PolyBezierSegmentImplementation
					{
						segment = source
					};
				}
				return null;
			}

			// Token: 0x060000D1 RID: 209 RVA: 0x00005900 File Offset: 0x00003B00
			public override void Flatten(IList<Point> points, double tolerance)
			{
				Point point = base.Start;
				int num = this.segment.Points.Count / 3 * 3;
				for (int i = 0; i < num; i += 3)
				{
					Point[] controlPoints = new Point[]
					{
						point,
						this.segment.Points[i],
						this.segment.Points[i + 1],
						this.segment.Points[i + 2]
					};
					List<Point> list = new List<Point>();
					BezierCurveFlattener.FlattenCubic(controlPoints, tolerance, list, true, null);
					points.AddRange(list);
					point = this.segment.Points[i + 2];
				}
			}

			// Token: 0x060000D2 RID: 210 RVA: 0x000059E0 File Offset: 0x00003BE0
			public override Point GetPoint(int index)
			{
				int num = this.segment.Points.Count / 3 * 3;
				if (index < -1 || index > num - 1)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index != -1)
				{
					return this.segment.Points[index];
				}
				return this.segment.Points[num - 1];
			}

			// Token: 0x060000D3 RID: 211 RVA: 0x00005BE8 File Offset: 0x00003DE8
			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				Point point0 = base.Start;
				IList<Point> points = this.segment.Points;
				int count = this.segment.Points.Count / 3;
				for (int i = 0; i < count; i++)
				{
					int k3 = i * 3;
					yield return SimpleSegment.Create(point0, points[k3], points[k3 + 1], points[k3 + 2]);
					point0 = points[k3 + 2];
				}
				yield break;
			}

			// Token: 0x04000034 RID: 52
			private PolyBezierSegment segment;
		}

		// Token: 0x02000012 RID: 18
		private class PolyQuadraticBezierSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			// Token: 0x060000D5 RID: 213 RVA: 0x00005C10 File Offset: 0x00003E10
			public static PathSegmentHelper.PathSegmentImplementation Create(PolyQuadraticBezierSegment source)
			{
				if (source != null)
				{
					return new PathSegmentHelper.PolyQuadraticBezierSegmentImplementation
					{
						segment = source
					};
				}
				return null;
			}

			// Token: 0x060000D6 RID: 214 RVA: 0x00005C30 File Offset: 0x00003E30
			public override void Flatten(IList<Point> points, double tolerance)
			{
				Point point = base.Start;
				int num = this.segment.Points.Count / 2 * 2;
				for (int i = 0; i < num; i += 2)
				{
					Point[] controlPoints = new Point[]
					{
						point,
						this.segment.Points[i],
						this.segment.Points[i + 1]
					};
					List<Point> list = new List<Point>();
					BezierCurveFlattener.FlattenQuadratic(controlPoints, tolerance, list, true, null);
					points.AddRange(list);
					point = this.segment.Points[i + 1];
				}
			}

			// Token: 0x060000D7 RID: 215 RVA: 0x00005CF0 File Offset: 0x00003EF0
			public override Point GetPoint(int index)
			{
				int num = this.segment.Points.Count / 2 * 2;
				if (index < -1 || index > num - 1)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index != -1)
				{
					return this.segment.Points[index];
				}
				return this.segment.Points[num - 1];
			}

			// Token: 0x060000D8 RID: 216 RVA: 0x00005EE0 File Offset: 0x000040E0
			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				Point point0 = base.Start;
				IList<Point> points = this.segment.Points;
				int count = this.segment.Points.Count / 2;
				for (int i = 0; i < count; i++)
				{
					int k2 = i * 2;
					yield return SimpleSegment.Create(point0, points[k2], points[k2 + 1]);
					point0 = points[k2 + 1];
				}
				yield break;
			}

			// Token: 0x04000035 RID: 53
			private PolyQuadraticBezierSegment segment;
		}

		// Token: 0x02000013 RID: 19
		private class ArcSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			// Token: 0x060000DA RID: 218 RVA: 0x00005F08 File Offset: 0x00004108
			public static PathSegmentHelper.PathSegmentImplementation Create(ArcSegment source)
			{
				if (source != null)
				{
					return new PathSegmentHelper.ArcSegmentImplementation
					{
						segment = source
					};
				}
				return null;
			}

			// Token: 0x060000DB RID: 219 RVA: 0x00005F28 File Offset: 0x00004128
			public override void Flatten(IList<Point> points, double tolerance)
			{
				PathSegment pathSegment = PathSegmentHelper.ArcToBezierSegments(this.segment, base.Start);
				if (pathSegment != null)
				{
					pathSegment.FlattenSegment(points, base.Start, tolerance);
				}
			}

			// Token: 0x060000DC RID: 220 RVA: 0x00005F58 File Offset: 0x00004158
			public override Point GetPoint(int index)
			{
				if (index < -1 || index > 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this.segment.Point;
			}

			// Token: 0x060000DD RID: 221 RVA: 0x00005F78 File Offset: 0x00004178
			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				PathSegment pathSegment = PathSegmentHelper.ArcToBezierSegments(this.segment, base.Start);
				if (pathSegment != null)
				{
					return pathSegment.GetSimpleSegments(base.Start);
				}
				return Enumerable.Empty<SimpleSegment>();
			}

			// Token: 0x04000036 RID: 54
			private ArcSegment segment;
		}

		// Token: 0x02000014 RID: 20
		private class LineSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			// Token: 0x060000DF RID: 223 RVA: 0x00005FB4 File Offset: 0x000041B4
			public static PathSegmentHelper.PathSegmentImplementation Create(LineSegment source)
			{
				if (source != null)
				{
					return new PathSegmentHelper.LineSegmentImplementation
					{
						segment = source
					};
				}
				return null;
			}

			// Token: 0x060000E0 RID: 224 RVA: 0x00005FD4 File Offset: 0x000041D4
			public override void Flatten(IList<Point> points, double tolerance)
			{
				points.Add(this.segment.Point);
			}

			// Token: 0x060000E1 RID: 225 RVA: 0x00005FE7 File Offset: 0x000041E7
			public override Point GetPoint(int index)
			{
				if (index < -1 || index > 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this.segment.Point;
			}

			// Token: 0x060000E2 RID: 226 RVA: 0x000060F4 File Offset: 0x000042F4
			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				yield return SimpleSegment.Create(base.Start, this.segment.Point);
				yield break;
			}

			// Token: 0x04000037 RID: 55
			private LineSegment segment;
		}

		// Token: 0x02000015 RID: 21
		private class PolyLineSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			// Token: 0x060000E4 RID: 228 RVA: 0x0000611C File Offset: 0x0000431C
			public static PathSegmentHelper.PathSegmentImplementation Create(PolyLineSegment source)
			{
				if (source != null)
				{
					return new PathSegmentHelper.PolyLineSegmentImplementation
					{
						segment = source
					};
				}
				return null;
			}

			// Token: 0x060000E5 RID: 229 RVA: 0x0000613C File Offset: 0x0000433C
			public override void Flatten(IList<Point> points, double tolerance)
			{
				points.AddRange(this.segment.Points);
			}

			// Token: 0x060000E6 RID: 230 RVA: 0x00006150 File Offset: 0x00004350
			public override Point GetPoint(int index)
			{
				if (index < -1 || index > this.segment.Points.Count - 1)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index != -1)
				{
					return this.segment.Points[index];
				}
				return this.segment.Points.Last<Point>();
			}

			// Token: 0x060000E7 RID: 231 RVA: 0x00006368 File Offset: 0x00004568
			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				Point point0 = base.Start;
				foreach (Point point in this.segment.Points)
				{
					yield return SimpleSegment.Create(point0, point);
					point0 = point;
				}
				yield break;
			}

			// Token: 0x04000038 RID: 56
			private PolyLineSegment segment;
		}
	}
}
