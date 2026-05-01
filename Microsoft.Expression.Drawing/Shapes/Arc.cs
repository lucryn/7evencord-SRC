using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Expression.Media;

namespace Microsoft.Expression.Shapes
{
	// Token: 0x02000046 RID: 70
	public sealed class Arc : PrimitiveShape, IArcGeometrySourceParameters, IGeometrySourceParameters
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000F171 File Offset: 0x0000D371
		// (set) Token: 0x06000270 RID: 624 RVA: 0x0000F183 File Offset: 0x0000D383
		public double StartAngle
		{
			get
			{
				return (double)base.GetValue(Arc.StartAngleProperty);
			}
			set
			{
				base.SetValue(Arc.StartAngleProperty, value);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000F196 File Offset: 0x0000D396
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000F1A8 File Offset: 0x0000D3A8
		public double EndAngle
		{
			get
			{
				return (double)base.GetValue(Arc.EndAngleProperty);
			}
			set
			{
				base.SetValue(Arc.EndAngleProperty, value);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000F1BB File Offset: 0x0000D3BB
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000F1CD File Offset: 0x0000D3CD
		public double ArcThickness
		{
			get
			{
				return (double)base.GetValue(Arc.ArcThicknessProperty);
			}
			set
			{
				base.SetValue(Arc.ArcThicknessProperty, value);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000F1E0 File Offset: 0x0000D3E0
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000F1F2 File Offset: 0x0000D3F2
		public UnitType ArcThicknessUnit
		{
			get
			{
				return (UnitType)base.GetValue(Arc.ArcThicknessUnitProperty);
			}
			set
			{
				base.SetValue(Arc.ArcThicknessUnitProperty, value);
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000F205 File Offset: 0x0000D405
		protected override IGeometrySource CreateGeometrySource()
		{
			return new ArcGeometrySource();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000F2F9 File Offset: 0x0000D4F9
		Stretch IGeometrySourceParameters.get_Stretch()
		{
			return base.Stretch;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000F301 File Offset: 0x0000D501
		Brush IGeometrySourceParameters.get_Stroke()
		{
			return base.Stroke;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000F309 File Offset: 0x0000D509
		double IGeometrySourceParameters.get_StrokeThickness()
		{
			return base.StrokeThickness;
		}

		// Token: 0x040000CD RID: 205
		public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc), new DrawingPropertyMetadata(0.0, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x040000CE RID: 206
		public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc), new DrawingPropertyMetadata(90.0, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x040000CF RID: 207
		public static readonly DependencyProperty ArcThicknessProperty = DependencyProperty.Register("ArcThickness", typeof(double), typeof(Arc), new DrawingPropertyMetadata(0.0, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x040000D0 RID: 208
		public static readonly DependencyProperty ArcThicknessUnitProperty = DependencyProperty.Register("ArcThicknessUnit", typeof(UnitType), typeof(Arc), new DrawingPropertyMetadata(UnitType.Pixel, DrawingPropertyMetadataOptions.AffectsRender));
	}
}
