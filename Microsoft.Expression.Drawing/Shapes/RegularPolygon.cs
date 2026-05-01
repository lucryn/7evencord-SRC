using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Expression.Media;

namespace Microsoft.Expression.Shapes
{
	// Token: 0x02000048 RID: 72
	public sealed class RegularPolygon : PrimitiveShape, IPolygonGeometrySourceParameters, IGeometrySourceParameters
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000F455 File Offset: 0x0000D655
		// (set) Token: 0x0600028A RID: 650 RVA: 0x0000F467 File Offset: 0x0000D667
		public double PointCount
		{
			get
			{
				return (double)base.GetValue(RegularPolygon.PointCountProperty);
			}
			set
			{
				base.SetValue(RegularPolygon.PointCountProperty, value);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000F47A File Offset: 0x0000D67A
		// (set) Token: 0x0600028C RID: 652 RVA: 0x0000F48C File Offset: 0x0000D68C
		public double InnerRadius
		{
			get
			{
				return (double)base.GetValue(RegularPolygon.InnerRadiusProperty);
			}
			set
			{
				base.SetValue(RegularPolygon.InnerRadiusProperty, value);
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000F49F File Offset: 0x0000D69F
		protected override IGeometrySource CreateGeometrySource()
		{
			return new PolygonGeometrySource();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000F52D File Offset: 0x0000D72D
		Stretch IGeometrySourceParameters.get_Stretch()
		{
			return base.Stretch;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000F535 File Offset: 0x0000D735
		Brush IGeometrySourceParameters.get_Stroke()
		{
			return base.Stroke;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000F53D File Offset: 0x0000D73D
		double IGeometrySourceParameters.get_StrokeThickness()
		{
			return base.StrokeThickness;
		}

		// Token: 0x040000D4 RID: 212
		public static readonly DependencyProperty PointCountProperty = DependencyProperty.Register("PointCount", typeof(double), typeof(RegularPolygon), new DrawingPropertyMetadata(6.0, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x040000D5 RID: 213
		public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(double), typeof(RegularPolygon), new DrawingPropertyMetadata(1.0, DrawingPropertyMetadataOptions.AffectsRender));
	}
}
