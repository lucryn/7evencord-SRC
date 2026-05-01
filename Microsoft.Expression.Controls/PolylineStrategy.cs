using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Controls
{
	// Token: 0x0200000C RID: 12
	internal class PolylineStrategy : ShapeStrategy
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005E RID: 94 RVA: 0x0000338E File Offset: 0x0000158E
		// (set) Token: 0x0600005F RID: 95 RVA: 0x000033A0 File Offset: 0x000015A0
		private PointCollection PointsListener
		{
			get
			{
				return (PointCollection)base.GetValue(PolylineStrategy.PointsListenerProperty);
			}
			set
			{
				base.SetValue(PolylineStrategy.PointsListenerProperty, value);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000033AE File Offset: 0x000015AE
		public PolylineStrategy(LayoutPath layoutPath) : base(layoutPath)
		{
			this.sourcePolyline = (Polyline)layoutPath.SourceElement;
			base.SetListenerBinding(PolylineStrategy.PointsListenerProperty, "Points");
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000033D8 File Offset: 0x000015D8
		public override bool HasGeometryChanged()
		{
			return base.HasGeometryChanged() || !PolylineStrategy.PointCollectionsEqual(this.oldPoints, this.sourcePolyline.Points);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000033FD File Offset: 0x000015FD
		protected override PathGeometry UpdateGeometry()
		{
			this.oldPoints = PolylineStrategy.ClonePointCollection(this.sourcePolyline.Points);
			return PolylineStrategy.CreatePolylinePathGeometry(this.oldPoints, false);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003424 File Offset: 0x00001624
		public static PointCollection ClonePointCollection(PointCollection points)
		{
			if (points == null)
			{
				return null;
			}
			PointCollection pointCollection = new PointCollection();
			foreach (Point point in points)
			{
				pointCollection.Add(point);
			}
			return pointCollection;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003478 File Offset: 0x00001678
		public static bool PointCollectionsEqual(PointCollection collectionOne, PointCollection collectionTwo)
		{
			if (collectionOne == collectionTwo)
			{
				return true;
			}
			if (collectionOne == null || collectionTwo == null)
			{
				return false;
			}
			if (collectionOne.Count != collectionTwo.Count)
			{
				return false;
			}
			for (int i = 0; i < collectionOne.Count; i++)
			{
				if (collectionOne[i] != collectionTwo[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000034CC File Offset: 0x000016CC
		public static PathGeometry CreatePolylinePathGeometry(PointCollection points, bool isClosed)
		{
			if (points == null || points.Count == 0)
			{
				return new PathGeometry();
			}
			PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
			if (points.Count > 1)
			{
				pathSegmentCollection.Add(PathSegmentHelper.CreatePolylineSegment(points, 1, points.Count - 1, false));
			}
			PathGeometry pathGeometry = new PathGeometry();
			PathGeometry pathGeometry2 = pathGeometry;
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(new PathFigure
			{
				StartPoint = points[0],
				IsClosed = isClosed,
				Segments = pathSegmentCollection
			});
			pathGeometry2.Figures = pathFigureCollection;
			return pathGeometry;
		}

		// Token: 0x0400001F RID: 31
		private PointCollection oldPoints;

		// Token: 0x04000020 RID: 32
		private Polyline sourcePolyline;

		// Token: 0x04000021 RID: 33
		private static readonly DependencyProperty PointsListenerProperty = DependencyProperty.Register("PointsListener", typeof(PointCollection), typeof(PolylineStrategy), new PropertyMetadata(new PropertyChangedCallback(GeometryStrategy.LayoutPropertyChanged)));
	}
}
