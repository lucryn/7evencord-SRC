using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Media
{
	// Token: 0x02000044 RID: 68
	public sealed class SketchGeometryEffect : GeometryEffect
	{
		// Token: 0x0600024B RID: 587 RVA: 0x0000E5FF File Offset: 0x0000C7FF
		protected override GeometryEffect DeepCopy()
		{
			return new SketchGeometryEffect();
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000E606 File Offset: 0x0000C806
		public override bool Equals(GeometryEffect geometryEffect)
		{
			return geometryEffect is SketchGeometryEffect;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000E614 File Offset: 0x0000C814
		protected override bool UpdateCachedGeometry(Geometry input)
		{
			bool flag = false;
			PathGeometry pathGeometry = input.AsPathGeometry();
			if (pathGeometry != null)
			{
				flag |= this.UpdateSketchGeometry(pathGeometry);
			}
			else
			{
				this.cachedGeometry = input;
			}
			return flag;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000E700 File Offset: 0x0000C900
		private bool UpdateSketchGeometry(PathGeometry inputPath)
		{
			bool flag = false;
			PathGeometry pathGeometry;
			flag |= GeometryHelper.EnsureGeometryType<PathGeometry>(out pathGeometry, ref this.cachedGeometry, () => new PathGeometry());
			flag |= pathGeometry.Figures.EnsureListCount(inputPath.Figures.Count, () => new PathFigure());
			RandomEngine random = new RandomEngine(this.randomSeed);
			for (int i = 0; i < inputPath.Figures.Count; i++)
			{
				PathFigure pathFigure = inputPath.Figures[i];
				bool isClosed = pathFigure.IsClosed;
				bool isFilled = pathFigure.IsFilled;
				if (pathFigure.Segments.Count == 0)
				{
					flag |= pathGeometry.Figures[i].SetIfDifferent(PathFigure.StartPointProperty, pathFigure.StartPoint);
					flag |= pathGeometry.Figures[i].Segments.EnsureListCount(0, null);
				}
				else
				{
					List<Point> list = new List<Point>(pathFigure.Segments.Count * 3);
					foreach (SimpleSegment simpleSegment in this.GetEffectiveSegments(pathFigure))
					{
						List<Point> list2 = new List<Point>();
						list2.Add(simpleSegment.Points[0]);
						SimpleSegment simpleSegment2 = simpleSegment;
						double tolerance = 0.0;
						IList<double> resultParameters = null;
						simpleSegment2.Flatten(list2, tolerance, resultParameters);
						PolylineData polyline = new PolylineData(list2);
						if (list2.Count > 1 && polyline.TotalLength > 4.0)
						{
							double num = polyline.TotalLength / 8.0;
							int sampleCount = (int)Math.Max(2.0, Math.Ceiling(num));
							double interval = polyline.TotalLength / (double)sampleCount;
							double scale = interval / 8.0;
							List<Point> samplePoints = new List<Point>(sampleCount);
							List<Vector> sampleNormals = new List<Vector>(sampleCount);
							int sampleIndex = 0;
							PolylineHelper.PathMarch(polyline, 0.0, 0.0, delegate(MarchLocation location)
							{
								if (location.Reason == MarchStopReason.CompletePolyline)
								{
									return double.NaN;
								}
								if (location.Reason != MarchStopReason.CompleteStep)
								{
									return location.Remain;
								}
								if (sampleIndex++ == sampleCount)
								{
									return double.NaN;
								}
								samplePoints.Add(location.GetPoint(polyline.Points));
								sampleNormals.Add(location.GetNormal(polyline, 0.0));
								return interval;
							});
							SketchGeometryEffect.DisturbPoints(random, scale, samplePoints, sampleNormals);
							list.AddRange(samplePoints);
						}
						else
						{
							list.AddRange(list2);
							list.RemoveLast<Point>();
						}
					}
					if (!isClosed)
					{
						list.Add(pathFigure.Segments.Last<PathSegment>().GetLastPoint());
					}
					flag |= PathFigureHelper.SyncPolylineFigure(pathGeometry.Figures[i], list, isClosed, isFilled);
				}
			}
			if (flag)
			{
				this.cachedGeometry = PathGeometryHelper.FixPathGeometryBoundary(this.cachedGeometry);
			}
			return flag;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000EA3C File Offset: 0x0000CC3C
		private static void DisturbPoints(RandomEngine random, double scale, IList<Point> points, IList<Vector> normals)
		{
			int count = points.Count;
			for (int i = 1; i < count; i++)
			{
				double num = random.NextGaussian(0.0, 1.0 * scale);
				double num2 = random.NextUniform(-0.5, 0.5) * scale;
				points[i] = new Point(points[i].X + normals[i].X * num2 - normals[i].Y * num, points[i].Y + normals[i].X * num + normals[i].Y * num2);
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000EDD0 File Offset: 0x0000CFD0
		private IEnumerable<SimpleSegment> GetEffectiveSegments(PathFigure pathFigure)
		{
			Point lastPoint = pathFigure.StartPoint;
			foreach (PathSegmentData data in pathFigure.AllSegments())
			{
				foreach (SimpleSegment segment in data.PathSegment.GetSimpleSegments(data.StartPoint))
				{
					yield return segment;
					lastPoint = segment.Points.Last<Point>();
				}
			}
			if (pathFigure.IsClosed)
			{
				yield return SimpleSegment.Create(lastPoint, pathFigure.StartPoint);
			}
			yield break;
		}

		// Token: 0x040000C1 RID: 193
		private const double expectedLengthMean = 8.0;

		// Token: 0x040000C2 RID: 194
		private const double normalDisturbVariance = 0.5;

		// Token: 0x040000C3 RID: 195
		private const double tangentDisturbVariance = 1.0;

		// Token: 0x040000C4 RID: 196
		private const double bsplineWeight = 0.05;

		// Token: 0x040000C5 RID: 197
		private readonly long randomSeed = DateTime.Now.Ticks;
	}
}
