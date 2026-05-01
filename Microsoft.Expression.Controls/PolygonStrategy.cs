using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Microsoft.Expression.Controls
{
	// Token: 0x0200000B RID: 11
	internal class PolygonStrategy : ShapeStrategy
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000032C5 File Offset: 0x000014C5
		// (set) Token: 0x06000059 RID: 89 RVA: 0x000032D7 File Offset: 0x000014D7
		private PointCollection PointsListener
		{
			get
			{
				return (PointCollection)base.GetValue(PolygonStrategy.PointsListenerProperty);
			}
			set
			{
				base.SetValue(PolygonStrategy.PointsListenerProperty, value);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000032E5 File Offset: 0x000014E5
		public PolygonStrategy(LayoutPath layoutPath) : base(layoutPath)
		{
			this.sourcePolygon = (Polygon)layoutPath.SourceElement;
			base.SetListenerBinding(PolygonStrategy.PointsListenerProperty, "Points");
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000330F File Offset: 0x0000150F
		public override bool HasGeometryChanged()
		{
			return base.HasGeometryChanged() || !PolylineStrategy.PointCollectionsEqual(this.oldPoints, this.sourcePolygon.Points);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003334 File Offset: 0x00001534
		protected override PathGeometry UpdateGeometry()
		{
			this.oldPoints = PolylineStrategy.ClonePointCollection(this.sourcePolygon.Points);
			return PolylineStrategy.CreatePolylinePathGeometry(this.oldPoints, true);
		}

		// Token: 0x0400001C RID: 28
		private PointCollection oldPoints;

		// Token: 0x0400001D RID: 29
		private Polygon sourcePolygon;

		// Token: 0x0400001E RID: 30
		private static readonly DependencyProperty PointsListenerProperty = DependencyProperty.Register("PointsListener", typeof(PointCollection), typeof(PolygonStrategy), new PropertyMetadata(new PropertyChangedCallback(GeometryStrategy.LayoutPropertyChanged)));
	}
}
