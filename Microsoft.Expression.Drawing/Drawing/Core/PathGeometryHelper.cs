using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x0200000A RID: 10
	internal static class PathGeometryHelper
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00003028 File Offset: 0x00001228
		internal static PathGeometry ConvertToPathGeometry(string abbreviatedGeometry)
		{
			if (abbreviatedGeometry == null)
			{
				throw new ArgumentNullException("abbreviatedGeometry");
			}
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = new PathFigureCollection();
			int num = 0;
			while (num < abbreviatedGeometry.Length && char.IsWhiteSpace(abbreviatedGeometry, num))
			{
				num++;
			}
			if (num < abbreviatedGeometry.Length && abbreviatedGeometry.get_Chars(num) == 'F')
			{
				num++;
				while (num < abbreviatedGeometry.Length && char.IsWhiteSpace(abbreviatedGeometry, num))
				{
					num++;
				}
				if (num == abbreviatedGeometry.Length || (abbreviatedGeometry.get_Chars(num) != '0' && abbreviatedGeometry.get_Chars(num) != '1'))
				{
					throw new FormatException();
				}
				pathGeometry.FillRule = ((abbreviatedGeometry.get_Chars(num) == '0') ? 0 : 1);
				num++;
			}
			new PathGeometryHelper.AbbreviatedGeometryParser(pathGeometry).Parse(abbreviatedGeometry, num);
			return pathGeometry;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000030EC File Offset: 0x000012EC
		public static PathGeometry AsPathGeometry(this Geometry original)
		{
			PathGeometry pathGeometry = original as PathGeometry;
			if (pathGeometry == null && (pathGeometry = PathGeometryHelper.ConvertToPathGeometry(original as RectangleGeometry)) == null && (pathGeometry = PathGeometryHelper.ConvertToPathGeometry(original as EllipseGeometry)) == null && (pathGeometry = PathGeometryHelper.ConvertToPathGeometry(original as LineGeometry)) == null && (pathGeometry = PathGeometryHelper.ConvertToPathGeometry(original as GeometryGroup)) == null)
			{
				return null;
			}
			return pathGeometry;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003142 File Offset: 0x00001342
		public static bool IsStroked(this PathSegment pathSegment)
		{
			return true;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003145 File Offset: 0x00001345
		public static bool IsSmoothJoin(this PathSegment pathSegment)
		{
			return false;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003148 File Offset: 0x00001348
		public static bool IsFrozen(this Geometry geometry)
		{
			return true;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000314C File Offset: 0x0000134C
		public static bool SyncPolylineGeometry(ref Geometry geometry, IList<Point> points, bool isClosed)
		{
			bool flag = false;
			PathGeometry pathGeometry = geometry as PathGeometry;
			PathFigure figure;
			if (pathGeometry == null || pathGeometry.Figures.Count != 1 || (figure = pathGeometry.Figures[0]) == null)
			{
				geometry = (pathGeometry = new PathGeometry());
				pathGeometry.Figures.Add(figure = new PathFigure());
				flag = true;
			}
			return flag | PathFigureHelper.SyncPolylineFigure(figure, points, isClosed, true);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000031B0 File Offset: 0x000013B0
		internal static Geometry FixPathGeometryBoundary(Geometry geometry)
		{
			PathGeometry pathGeometry = geometry as PathGeometry;
			if (pathGeometry != null)
			{
				PathFigureCollection figures = pathGeometry.Figures;
				pathGeometry.Figures = null;
				pathGeometry = PathGeometryHelper.ClonePathGeometry(pathGeometry);
				pathGeometry.Figures = figures;
				geometry = pathGeometry;
			}
			return geometry;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000031E8 File Offset: 0x000013E8
		public static Geometry CloneCurrentValue(this Geometry geometry)
		{
			if (geometry == null)
			{
				return null;
			}
			PathGeometry pathGeometry = geometry as PathGeometry;
			if (pathGeometry != null)
			{
				return PathGeometryHelper.ClonePathGeometry(pathGeometry);
			}
			LineGeometry lineGeometry = geometry as LineGeometry;
			if (lineGeometry != null)
			{
				return PathGeometryHelper.CloneLineGeometry(lineGeometry);
			}
			EllipseGeometry ellipseGeometry = geometry as EllipseGeometry;
			if (ellipseGeometry != null)
			{
				return PathGeometryHelper.CloneEllipseGeometry(ellipseGeometry);
			}
			RectangleGeometry rectangleGeometry = geometry as RectangleGeometry;
			if (rectangleGeometry != null)
			{
				return PathGeometryHelper.CloneRectangleGeometry(rectangleGeometry);
			}
			GeometryGroup geometryGroup = geometry as GeometryGroup;
			if (geometryGroup != null)
			{
				return PathGeometryHelper.CloneGeometryGroup(geometryGroup);
			}
			return geometry.DeepCopy<Geometry>();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003258 File Offset: 0x00001458
		private static PathGeometry ClonePathGeometry(PathGeometry pathGeometry)
		{
			PathGeometry pathGeometry2 = new PathGeometry
			{
				FillRule = pathGeometry.FillRule,
				Transform = pathGeometry.Transform.CloneTransform()
			};
			foreach (PathFigure pathFigure in pathGeometry.Figures)
			{
				pathGeometry2.Figures.Add(PathGeometryHelper.ClonePathFigure(pathFigure));
			}
			return pathGeometry2;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000032D8 File Offset: 0x000014D8
		private static PathFigure ClonePathFigure(PathFigure pathFigure)
		{
			PathFigure pathFigure2 = new PathFigure
			{
				IsClosed = pathFigure.IsClosed,
				IsFilled = pathFigure.IsFilled,
				StartPoint = pathFigure.StartPoint
			};
			foreach (PathSegment pathSegment in pathFigure.Segments)
			{
				pathFigure2.Segments.Add(PathGeometryHelper.ClonePathSegment(pathSegment));
			}
			return pathFigure2;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000335C File Offset: 0x0000155C
		private static PathSegment ClonePathSegment(PathSegment pathSegment)
		{
			if (pathSegment == null)
			{
				return null;
			}
			LineSegment lineSegment = pathSegment as LineSegment;
			if (lineSegment != null)
			{
				return PathGeometryHelper.CloneLineSegment(lineSegment);
			}
			BezierSegment bezierSegment = pathSegment as BezierSegment;
			if (bezierSegment != null)
			{
				return PathGeometryHelper.CloneBezierSegment(bezierSegment);
			}
			QuadraticBezierSegment quadraticBezierSegment = pathSegment as QuadraticBezierSegment;
			if (quadraticBezierSegment != null)
			{
				return PathGeometryHelper.CloneQuadraticBezierSegment(quadraticBezierSegment);
			}
			ArcSegment arcSegment = pathSegment as ArcSegment;
			if (arcSegment != null)
			{
				return PathGeometryHelper.CloneArcSegment(arcSegment);
			}
			PolyLineSegment polyLineSegment = pathSegment as PolyLineSegment;
			if (polyLineSegment != null)
			{
				return PathGeometryHelper.ClonePolyLineSegment(polyLineSegment);
			}
			PolyBezierSegment polyBezierSegment = pathSegment as PolyBezierSegment;
			if (polyBezierSegment != null)
			{
				return PathGeometryHelper.ClonePolyBezierSegment(polyBezierSegment);
			}
			PolyQuadraticBezierSegment polyQuadraticBezierSegment = pathSegment as PolyQuadraticBezierSegment;
			if (polyQuadraticBezierSegment != null)
			{
				return PathGeometryHelper.ClonePolyQuadraticBezierSegment(polyQuadraticBezierSegment);
			}
			return pathSegment.DeepCopy<PathSegment>();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000033F4 File Offset: 0x000015F4
		private static LineSegment CloneLineSegment(LineSegment lineSegment)
		{
			return new LineSegment
			{
				Point = lineSegment.Point
			};
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003418 File Offset: 0x00001618
		private static BezierSegment CloneBezierSegment(BezierSegment bezierSegment)
		{
			return new BezierSegment
			{
				Point1 = bezierSegment.Point1,
				Point2 = bezierSegment.Point2,
				Point3 = bezierSegment.Point3
			};
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003454 File Offset: 0x00001654
		private static QuadraticBezierSegment CloneQuadraticBezierSegment(QuadraticBezierSegment quadraticBezierSegment)
		{
			return new QuadraticBezierSegment
			{
				Point1 = quadraticBezierSegment.Point1,
				Point2 = quadraticBezierSegment.Point2
			};
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003484 File Offset: 0x00001684
		private static ArcSegment CloneArcSegment(ArcSegment arcSegment)
		{
			return new ArcSegment
			{
				IsLargeArc = arcSegment.IsLargeArc,
				Point = arcSegment.Point,
				RotationAngle = arcSegment.RotationAngle,
				Size = arcSegment.Size,
				SweepDirection = arcSegment.SweepDirection
			};
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000034D8 File Offset: 0x000016D8
		private static PolyLineSegment ClonePolyLineSegment(PolyLineSegment polyLineSegment)
		{
			PolyLineSegment polyLineSegment2 = new PolyLineSegment();
			foreach (Point point in polyLineSegment.Points)
			{
				polyLineSegment2.Points.Add(point);
			}
			return polyLineSegment2;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003534 File Offset: 0x00001734
		private static PolyBezierSegment ClonePolyBezierSegment(PolyBezierSegment polyBezierSegment)
		{
			PolyBezierSegment polyBezierSegment2 = new PolyBezierSegment();
			foreach (Point point in polyBezierSegment.Points)
			{
				polyBezierSegment2.Points.Add(point);
			}
			return polyBezierSegment2;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003590 File Offset: 0x00001790
		private static PolyQuadraticBezierSegment ClonePolyQuadraticBezierSegment(PolyQuadraticBezierSegment polyQuadraticBezierSegment)
		{
			PolyQuadraticBezierSegment polyQuadraticBezierSegment2 = new PolyQuadraticBezierSegment();
			foreach (Point point in polyQuadraticBezierSegment.Points)
			{
				polyQuadraticBezierSegment2.Points.Add(point);
			}
			return polyQuadraticBezierSegment2;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000035EC File Offset: 0x000017EC
		private static EllipseGeometry CloneEllipseGeometry(EllipseGeometry ellipseGeometry)
		{
			return new EllipseGeometry
			{
				Center = ellipseGeometry.Center,
				RadiusX = ellipseGeometry.RadiusX,
				RadiusY = ellipseGeometry.RadiusY,
				Transform = ellipseGeometry.Transform.CloneTransform()
			};
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003638 File Offset: 0x00001838
		private static RectangleGeometry CloneRectangleGeometry(RectangleGeometry rectangleGeometry)
		{
			return new RectangleGeometry
			{
				Rect = rectangleGeometry.Rect,
				RadiusX = rectangleGeometry.RadiusX,
				RadiusY = rectangleGeometry.RadiusY,
				Transform = rectangleGeometry.Transform.CloneTransform()
			};
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003684 File Offset: 0x00001884
		private static LineGeometry CloneLineGeometry(LineGeometry lineGeometry)
		{
			return new LineGeometry
			{
				StartPoint = lineGeometry.StartPoint,
				EndPoint = lineGeometry.EndPoint,
				Transform = lineGeometry.Transform.CloneTransform()
			};
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000036C4 File Offset: 0x000018C4
		private static GeometryGroup CloneGeometryGroup(GeometryGroup geometryGroup)
		{
			GeometryGroup geometryGroup2 = new GeometryGroup
			{
				FillRule = geometryGroup.FillRule,
				Transform = geometryGroup.Transform.CloneTransform()
			};
			foreach (Geometry geometry in geometryGroup.Children)
			{
				geometryGroup2.Children.Add(geometry.CloneCurrentValue());
			}
			return geometryGroup2;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003744 File Offset: 0x00001944
		private static PathGeometry ConvertToPathGeometry(EllipseGeometry ellipseGeometry)
		{
			if (ellipseGeometry != null)
			{
				Rect bounds = ellipseGeometry.Bounds;
				if (bounds.Size().HasValidArea())
				{
					Point point = GeometryHelper.Lerp(bounds.TopLeft(), bounds.TopRight(), 0.5);
					Point point2 = GeometryHelper.Lerp(bounds.BottomLeft(), bounds.BottomRight(), 0.5);
					Size size;
					size..ctor(ellipseGeometry.RadiusX, ellipseGeometry.RadiusY);
					PathGeometry pathGeometry = new PathGeometry();
					pathGeometry.Transform = ellipseGeometry.Transform;
					PathGeometry pathGeometry2 = pathGeometry;
					PathFigureCollection pathFigureCollection = new PathFigureCollection();
					PresentationFrameworkCollection<PathFigure> presentationFrameworkCollection = pathFigureCollection;
					PathFigure pathFigure = new PathFigure();
					pathFigure.StartPoint = point;
					pathFigure.IsClosed = true;
					pathFigure.IsFilled = true;
					PathFigure pathFigure2 = pathFigure;
					PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
					pathSegmentCollection.Add(new ArcSegment
					{
						Point = point2,
						IsLargeArc = true,
						Size = size,
						SweepDirection = 1
					});
					pathSegmentCollection.Add(new ArcSegment
					{
						Point = point,
						IsLargeArc = true,
						Size = size,
						SweepDirection = 1
					});
					pathFigure2.Segments = pathSegmentCollection;
					presentationFrameworkCollection.Add(pathFigure);
					pathGeometry2.Figures = pathFigureCollection;
					return pathGeometry;
				}
			}
			return null;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003878 File Offset: 0x00001A78
		private static PathGeometry ConvertToPathGeometry(RectangleGeometry rectangleGeometry)
		{
			if (rectangleGeometry == null)
			{
				return null;
			}
			Rect bounds = rectangleGeometry.Bounds;
			if (!bounds.Size().HasValidArea())
			{
				return null;
			}
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Transform = rectangleGeometry.Transform;
			PathFigure pathFigure = new PathFigure
			{
				IsClosed = true,
				IsFilled = true
			};
			pathGeometry.Figures.Add(pathFigure);
			if (rectangleGeometry.RadiusX * rectangleGeometry.RadiusY == 0.0)
			{
				pathFigure.StartPoint = bounds.TopLeft();
				PresentationFrameworkCollection<PathSegment> segments = pathFigure.Segments;
				PolyLineSegment polyLineSegment = new PolyLineSegment();
				PolyLineSegment polyLineSegment2 = polyLineSegment;
				PointCollection pointCollection = new PointCollection();
				pointCollection.Add(bounds.TopRight());
				pointCollection.Add(bounds.BottomRight());
				pointCollection.Add(bounds.BottomLeft());
				polyLineSegment2.Points = pointCollection;
				segments.Add(polyLineSegment);
				return pathGeometry;
			}
			bool flag = Math.Abs(rectangleGeometry.RadiusX) < bounds.Width / 2.0;
			bool flag2 = Math.Abs(rectangleGeometry.RadiusY) < bounds.Height / 2.0;
			double num = Math.Min(Math.Abs(rectangleGeometry.RadiusX), bounds.Width / 2.0);
			double num2 = Math.Min(Math.Abs(rectangleGeometry.RadiusY), bounds.Height / 2.0);
			Size cornerSize;
			cornerSize..ctor(num, num2);
			pathFigure.StartPoint = new Point(bounds.Left, bounds.Top + num2);
			pathFigure.Segments.Add(PathGeometryHelper.CreateCorner(new Point(bounds.Left + num, bounds.Top), cornerSize));
			if (flag)
			{
				pathFigure.Segments.Add(new LineSegment
				{
					Point = new Point(bounds.Right - num, bounds.Top)
				});
			}
			pathFigure.Segments.Add(PathGeometryHelper.CreateCorner(new Point(bounds.Right, bounds.Top + num2), cornerSize));
			if (flag2)
			{
				pathFigure.Segments.Add(new LineSegment
				{
					Point = new Point(bounds.Right, bounds.Bottom - num2)
				});
			}
			pathFigure.Segments.Add(PathGeometryHelper.CreateCorner(new Point(bounds.Right - num, bounds.Bottom), cornerSize));
			if (flag)
			{
				pathFigure.Segments.Add(new LineSegment
				{
					Point = new Point(bounds.Left + num, bounds.Bottom)
				});
			}
			pathFigure.Segments.Add(PathGeometryHelper.CreateCorner(new Point(bounds.Left, bounds.Bottom - num2), cornerSize));
			return pathGeometry;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003B34 File Offset: 0x00001D34
		private static ArcSegment CreateCorner(Point endPoint, Size cornerSize)
		{
			return new ArcSegment
			{
				IsLargeArc = false,
				SweepDirection = 1,
				Point = endPoint,
				Size = cornerSize
			};
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003B64 File Offset: 0x00001D64
		private static PathGeometry ConvertToPathGeometry(LineGeometry lineGeometry)
		{
			if (lineGeometry != null)
			{
				Rect bounds = lineGeometry.Bounds;
				if (bounds.Size().HasValidArea())
				{
					PathGeometry pathGeometry = new PathGeometry();
					pathGeometry.Transform = lineGeometry.Transform;
					PathGeometry pathGeometry2 = pathGeometry;
					PathFigureCollection pathFigureCollection = new PathFigureCollection();
					PresentationFrameworkCollection<PathFigure> presentationFrameworkCollection = pathFigureCollection;
					PathFigure pathFigure = new PathFigure();
					pathFigure.StartPoint = lineGeometry.StartPoint;
					pathFigure.IsClosed = false;
					pathFigure.IsFilled = false;
					PathFigure pathFigure2 = pathFigure;
					PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
					pathSegmentCollection.Add(new LineSegment
					{
						Point = lineGeometry.EndPoint
					});
					pathFigure2.Segments = pathSegmentCollection;
					presentationFrameworkCollection.Add(pathFigure);
					pathGeometry2.Figures = pathFigureCollection;
					return pathGeometry;
				}
			}
			return null;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003C00 File Offset: 0x00001E00
		private static PathGeometry ConvertToPathGeometry(GeometryGroup geometryGroup)
		{
			if (geometryGroup != null)
			{
				Rect bounds = geometryGroup.Bounds;
				if (bounds.Size().HasValidArea())
				{
					PathGeometry pathGeometry = new PathGeometry();
					pathGeometry.SetIfDifferent(PathGeometry.FillRuleProperty, geometryGroup.FillRule);
					LinkedList<Geometry> linkedList = new LinkedList<Geometry>();
					linkedList.AddLast(geometryGroup);
					while (linkedList.Count > 0)
					{
						LinkedListNode<Geometry> first = linkedList.First;
						Geometry value = first.Value;
						GeometryGroup geometryGroup2 = value as GeometryGroup;
						if (geometryGroup2 != null)
						{
							using (IEnumerator<Geometry> enumerator = geometryGroup2.Children.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									Geometry geometry = enumerator.Current;
									linkedList.AddAfter(first, geometry);
								}
								goto IL_F8;
							}
							goto IL_A3;
						}
						goto IL_A3;
						IL_F8:
						linkedList.RemoveFirst();
						continue;
						IL_A3:
						PathGeometry pathGeometry2 = value.AsPathGeometry();
						if (pathGeometry2 != null)
						{
							for (int i = pathGeometry2.Figures.Count - 1; i >= 0; i--)
							{
								PathFigure pathFigure = pathGeometry2.Figures[i];
								pathGeometry2.Figures.RemoveAt(i);
								pathGeometry.Figures.Add(pathFigure);
							}
							goto IL_F8;
						}
						goto IL_F8;
					}
					return pathGeometry;
				}
			}
			return null;
		}

		// Token: 0x0200000B RID: 11
		private class AbbreviatedGeometryParser
		{
			// Token: 0x06000097 RID: 151 RVA: 0x00003D2C File Offset: 0x00001F2C
			public AbbreviatedGeometryParser(PathGeometry geometry)
			{
				this.geometry = geometry;
			}

			// Token: 0x06000098 RID: 152 RVA: 0x00003D3C File Offset: 0x00001F3C
			public void Parse(string data, int startIndex)
			{
				this.buffer = data;
				this.length = data.Length;
				this.index = startIndex;
				bool flag = true;
				while (this.ReadToken())
				{
					char c = this.token;
					if (flag)
					{
						if (c != 'M' && c != 'm')
						{
							throw new FormatException();
						}
						flag = false;
					}
					char c2 = c;
					if (c2 <= 'Z')
					{
						if (c2 <= 'M')
						{
							switch (c2)
							{
							case 'A':
								break;
							case 'B':
								goto IL_364;
							case 'C':
								goto IL_225;
							default:
								if (c2 == 'H')
								{
									goto IL_193;
								}
								switch (c2)
								{
								case 'L':
									goto IL_165;
								case 'M':
									goto IL_11B;
								default:
									goto IL_364;
								}
								break;
							}
						}
						else
						{
							switch (c2)
							{
							case 'Q':
								goto IL_2BD;
							case 'R':
								goto IL_364;
							case 'S':
								goto IL_276;
							default:
								if (c2 == 'V')
								{
									goto IL_1DA;
								}
								if (c2 != 'Z')
								{
									goto IL_364;
								}
								goto IL_35B;
							}
						}
					}
					else if (c2 <= 'm')
					{
						switch (c2)
						{
						case 'a':
							break;
						case 'b':
							goto IL_364;
						case 'c':
							goto IL_225;
						default:
							if (c2 == 'h')
							{
								goto IL_193;
							}
							switch (c2)
							{
							case 'l':
								goto IL_165;
							case 'm':
								goto IL_11B;
							default:
								goto IL_364;
							}
							break;
						}
					}
					else
					{
						switch (c2)
						{
						case 'q':
							goto IL_2BD;
						case 'r':
							goto IL_364;
						case 's':
							goto IL_276;
						default:
							if (c2 == 'v')
							{
								goto IL_1DA;
							}
							if (c2 != 'z')
							{
								goto IL_364;
							}
							goto IL_35B;
						}
					}
					do
					{
						Size size = this.ReadSize(false);
						double rotationAngle = this.ReadDouble(true);
						bool isLargeArc = this.ReadBool01(true);
						SweepDirection sweepDirection = this.ReadBool01(true) ? 1 : 0;
						this.lastPoint = this.ReadPoint(c, true);
						this.ArcTo(size, rotationAngle, isLargeArc, sweepDirection, this.lastPoint);
					}
					while (this.IsNumber(true));
					this.EnsureFigure();
					continue;
					IL_11B:
					this.lastPoint = this.ReadPoint(c, false);
					this.BeginFigure(this.lastPoint);
					char command = 'M';
					while (this.IsNumber(true))
					{
						this.lastPoint = this.ReadPoint(command, false);
						this.LineTo(this.lastPoint);
						command = 'L';
					}
					continue;
					IL_165:
					this.EnsureFigure();
					do
					{
						this.lastPoint = this.ReadPoint(c, false);
						this.LineTo(this.lastPoint);
					}
					while (this.IsNumber(true));
					continue;
					IL_193:
					this.EnsureFigure();
					do
					{
						double num = this.ReadDouble(false);
						if (c == 'h')
						{
							num += this.lastPoint.X;
						}
						this.lastPoint.X = num;
						this.LineTo(this.lastPoint);
					}
					while (this.IsNumber(true));
					continue;
					IL_1DA:
					this.EnsureFigure();
					do
					{
						double num2 = this.ReadDouble(false);
						if (c == 'v')
						{
							num2 += this.lastPoint.Y;
						}
						this.lastPoint.Y = num2;
						this.LineTo(this.lastPoint);
					}
					while (this.IsNumber(true));
					continue;
					IL_225:
					this.EnsureFigure();
					do
					{
						Point point = this.ReadPoint(c, false);
						this.secondLastPoint = this.ReadPoint(c, true);
						this.lastPoint = this.ReadPoint(c, true);
						this.BezierTo(point, this.secondLastPoint, this.lastPoint);
					}
					while (this.IsNumber(true));
					continue;
					IL_276:
					this.EnsureFigure();
					do
					{
						Point smoothBeizerFirstPoint = this.GetSmoothBeizerFirstPoint();
						Point point2 = this.ReadPoint(c, false);
						this.lastPoint = this.ReadPoint(c, true);
						this.BezierTo(smoothBeizerFirstPoint, point2, this.lastPoint);
					}
					while (this.IsNumber(true));
					continue;
					IL_2BD:
					this.EnsureFigure();
					do
					{
						Point point3 = this.ReadPoint(c, false);
						this.lastPoint = this.ReadPoint(c, true);
						this.QuadraticBezierTo(point3, this.lastPoint);
					}
					while (this.IsNumber(true));
					continue;
					IL_35B:
					this.FinishFigure(true);
					continue;
					IL_364:
					throw new NotSupportedException();
				}
				this.FinishFigure(false);
			}

			// Token: 0x06000099 RID: 153 RVA: 0x000040C8 File Offset: 0x000022C8
			private bool ReadToken()
			{
				this.SkipWhitespace(false);
				if (this.index < this.length)
				{
					this.token = this.buffer.get_Chars(this.index++);
					return true;
				}
				return false;
			}

			// Token: 0x0600009A RID: 154 RVA: 0x00004110 File Offset: 0x00002310
			private Point ReadPoint(char command, bool allowComma)
			{
				double num = this.ReadDouble(allowComma);
				double num2 = this.ReadDouble(true);
				if (command >= 'a')
				{
					num += this.lastPoint.X;
					num2 += this.lastPoint.Y;
				}
				return new Point(num, num2);
			}

			// Token: 0x0600009B RID: 155 RVA: 0x00004158 File Offset: 0x00002358
			private Size ReadSize(bool allowComma)
			{
				double num = this.ReadDouble(allowComma);
				double num2 = this.ReadDouble(true);
				return new Size(num, num2);
			}

			// Token: 0x0600009C RID: 156 RVA: 0x0000417C File Offset: 0x0000237C
			private bool ReadBool01(bool allowComma)
			{
				double num = this.ReadDouble(allowComma);
				if (num == 0.0)
				{
					return false;
				}
				if (num == 1.0)
				{
					return true;
				}
				throw new FormatException();
			}

			// Token: 0x0600009D RID: 157 RVA: 0x000041B4 File Offset: 0x000023B4
			private double ReadDouble(bool allowComma)
			{
				if (!this.IsNumber(allowComma))
				{
					throw new FormatException();
				}
				bool flag = true;
				int i = this.index;
				if (this.index < this.length && (this.buffer.get_Chars(this.index) == '-' || this.buffer.get_Chars(this.index) == '+'))
				{
					this.index++;
				}
				if (this.index < this.length && this.buffer.get_Chars(this.index) == 'I')
				{
					this.index = Math.Min(this.index + 8, this.length);
					flag = false;
				}
				else if (this.index < this.length && this.buffer.get_Chars(this.index) == 'N')
				{
					this.index = Math.Min(this.index + 3, this.length);
					flag = false;
				}
				else
				{
					this.SkipDigits(false);
					if (this.index < this.length && this.buffer.get_Chars(this.index) == '.')
					{
						flag = false;
						this.index++;
						this.SkipDigits(false);
					}
					if (this.index < this.length && (this.buffer.get_Chars(this.index) == 'E' || this.buffer.get_Chars(this.index) == 'e'))
					{
						flag = false;
						this.index++;
						this.SkipDigits(true);
					}
				}
				if (flag && this.index <= i + 8)
				{
					int num = 1;
					if (this.buffer.get_Chars(i) == '+')
					{
						i++;
					}
					else if (this.buffer.get_Chars(i) == '-')
					{
						i++;
						num = -1;
					}
					int num2 = 0;
					while (i < this.index)
					{
						num2 = num2 * 10 + (int)(this.buffer.get_Chars(i) - '0');
						i++;
					}
					return (double)(num2 * num);
				}
				string text = this.buffer.Substring(i, this.index - i);
				double result;
				try
				{
					result = Convert.ToDouble(text, CultureInfo.InvariantCulture);
				}
				catch (FormatException)
				{
					throw new FormatException();
				}
				return result;
			}

			// Token: 0x0600009E RID: 158 RVA: 0x000043E0 File Offset: 0x000025E0
			private void SkipDigits(bool signAllowed)
			{
				if (signAllowed && this.index < this.length && (this.buffer.get_Chars(this.index) == '-' || this.buffer.get_Chars(this.index) == '+'))
				{
					this.index++;
				}
				while (this.index < this.length && this.buffer.get_Chars(this.index) >= '0' && this.buffer.get_Chars(this.index) <= '9')
				{
					this.index++;
				}
			}

			// Token: 0x0600009F RID: 159 RVA: 0x00004480 File Offset: 0x00002680
			private bool IsNumber(bool allowComma)
			{
				bool flag = this.SkipWhitespace(allowComma);
				if (this.index < this.length)
				{
					this.token = this.buffer.get_Chars(this.index);
					if (this.token == '.' || this.token == '-' || this.token == '+' || (this.token >= '0' && this.token <= '9') || this.token == 'I' || this.token == 'N')
					{
						return true;
					}
				}
				if (flag)
				{
					throw new FormatException();
				}
				return false;
			}

			// Token: 0x060000A0 RID: 160 RVA: 0x0000450C File Offset: 0x0000270C
			private bool SkipWhitespace(bool allowComma)
			{
				bool result = false;
				while (this.index < this.length)
				{
					char c = this.buffer.get_Chars(this.index);
					char c2 = c;
					switch (c2)
					{
					case '\t':
					case '\n':
					case '\r':
						break;
					case '\v':
					case '\f':
						goto IL_4F;
					default:
						if (c2 != ' ')
						{
							if (c2 != ',')
							{
								goto IL_4F;
							}
							if (!allowComma)
							{
								throw new FormatException();
							}
							result = true;
							allowComma = false;
						}
						break;
					}
					IL_65:
					this.index++;
					continue;
					IL_4F:
					if (c > ' ' && c <= 'z')
					{
						return result;
					}
					if (!char.IsWhiteSpace(c))
					{
						return result;
					}
					goto IL_65;
				}
				return false;
			}

			// Token: 0x060000A1 RID: 161 RVA: 0x0000459B File Offset: 0x0000279B
			private void BeginFigure(Point startPoint)
			{
				this.FinishFigure(false);
				this.EnsureFigure();
				this.figure.StartPoint = startPoint;
				this.figure.IsFilled = true;
			}

			// Token: 0x060000A2 RID: 162 RVA: 0x000045C2 File Offset: 0x000027C2
			private void EnsureFigure()
			{
				if (this.figure == null)
				{
					this.figure = new PathFigure();
					this.figure.Segments = new PathSegmentCollection();
				}
			}

			// Token: 0x060000A3 RID: 163 RVA: 0x000045E7 File Offset: 0x000027E7
			private void FinishFigure(bool figureExplicitlyClosed)
			{
				if (this.figure != null)
				{
					if (figureExplicitlyClosed)
					{
						this.figure.IsClosed = true;
					}
					this.geometry.Figures.Add(this.figure);
					this.figure = null;
				}
			}

			// Token: 0x060000A4 RID: 164 RVA: 0x00004620 File Offset: 0x00002820
			private void LineTo(Point point)
			{
				LineSegment lineSegment = new LineSegment();
				lineSegment.Point = point;
				this.figure.Segments.Add(lineSegment);
			}

			// Token: 0x060000A5 RID: 165 RVA: 0x0000464C File Offset: 0x0000284C
			private void BezierTo(Point point1, Point point2, Point point3)
			{
				BezierSegment bezierSegment = new BezierSegment();
				bezierSegment.Point1 = point1;
				bezierSegment.Point2 = point2;
				bezierSegment.Point3 = point3;
				this.figure.Segments.Add(bezierSegment);
			}

			// Token: 0x060000A6 RID: 166 RVA: 0x00004688 File Offset: 0x00002888
			private void QuadraticBezierTo(Point point1, Point point2)
			{
				QuadraticBezierSegment quadraticBezierSegment = new QuadraticBezierSegment();
				quadraticBezierSegment.Point1 = point1;
				quadraticBezierSegment.Point2 = point2;
				this.figure.Segments.Add(quadraticBezierSegment);
			}

			// Token: 0x060000A7 RID: 167 RVA: 0x000046BC File Offset: 0x000028BC
			private void ArcTo(Size size, double rotationAngle, bool isLargeArc, SweepDirection sweepDirection, Point point)
			{
				ArcSegment arcSegment = new ArcSegment();
				arcSegment.Size = size;
				arcSegment.RotationAngle = rotationAngle;
				arcSegment.IsLargeArc = isLargeArc;
				arcSegment.SweepDirection = sweepDirection;
				arcSegment.Point = point;
				this.figure.Segments.Add(arcSegment);
			}

			// Token: 0x060000A8 RID: 168 RVA: 0x00004708 File Offset: 0x00002908
			private Point GetSmoothBeizerFirstPoint()
			{
				Point result = this.lastPoint;
				if (this.figure.Segments.Count > 0)
				{
					BezierSegment bezierSegment = this.figure.Segments[this.figure.Segments.Count - 1] as BezierSegment;
					if (bezierSegment != null)
					{
						Point point = bezierSegment.Point2;
						result.X += this.lastPoint.X - point.X;
						result.Y += this.lastPoint.Y - point.Y;
					}
				}
				return result;
			}

			// Token: 0x04000029 RID: 41
			private PathGeometry geometry;

			// Token: 0x0400002A RID: 42
			private PathFigure figure;

			// Token: 0x0400002B RID: 43
			private Point lastPoint;

			// Token: 0x0400002C RID: 44
			private Point secondLastPoint;

			// Token: 0x0400002D RID: 45
			private string buffer;

			// Token: 0x0400002E RID: 46
			private int index;

			// Token: 0x0400002F RID: 47
			private int length;

			// Token: 0x04000030 RID: 48
			private char token;
		}
	}
}
