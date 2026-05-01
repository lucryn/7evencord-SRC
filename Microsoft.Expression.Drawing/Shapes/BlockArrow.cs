using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Expression.Media;

namespace Microsoft.Expression.Shapes
{
	// Token: 0x02000047 RID: 71
	public sealed class BlockArrow : PrimitiveShape, IBlockArrowGeometrySourceParameters, IGeometrySourceParameters
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000F311 File Offset: 0x0000D511
		// (set) Token: 0x0600027E RID: 638 RVA: 0x0000F323 File Offset: 0x0000D523
		public ArrowOrientation Orientation
		{
			get
			{
				return (ArrowOrientation)base.GetValue(BlockArrow.OrientationProperty);
			}
			set
			{
				base.SetValue(BlockArrow.OrientationProperty, value);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000F336 File Offset: 0x0000D536
		// (set) Token: 0x06000280 RID: 640 RVA: 0x0000F348 File Offset: 0x0000D548
		public double ArrowheadAngle
		{
			get
			{
				return (double)base.GetValue(BlockArrow.ArrowheadAngleProperty);
			}
			set
			{
				base.SetValue(BlockArrow.ArrowheadAngleProperty, value);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000F35B File Offset: 0x0000D55B
		// (set) Token: 0x06000282 RID: 642 RVA: 0x0000F36D File Offset: 0x0000D56D
		public double ArrowBodySize
		{
			get
			{
				return (double)base.GetValue(BlockArrow.ArrowBodySizeProperty);
			}
			set
			{
				base.SetValue(BlockArrow.ArrowBodySizeProperty, value);
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000F380 File Offset: 0x0000D580
		protected override IGeometrySource CreateGeometrySource()
		{
			return new BlockArrowGeometrySource();
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000F43D File Offset: 0x0000D63D
		Stretch IGeometrySourceParameters.get_Stretch()
		{
			return base.Stretch;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000F445 File Offset: 0x0000D645
		Brush IGeometrySourceParameters.get_Stroke()
		{
			return base.Stroke;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000F44D File Offset: 0x0000D64D
		double IGeometrySourceParameters.get_StrokeThickness()
		{
			return base.StrokeThickness;
		}

		// Token: 0x040000D1 RID: 209
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(ArrowOrientation), typeof(BlockArrow), new DrawingPropertyMetadata(ArrowOrientation.Right, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x040000D2 RID: 210
		public static readonly DependencyProperty ArrowheadAngleProperty = DependencyProperty.Register("ArrowheadAngle", typeof(double), typeof(BlockArrow), new DrawingPropertyMetadata(90.0, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x040000D3 RID: 211
		public static readonly DependencyProperty ArrowBodySizeProperty = DependencyProperty.Register("ArrowBodySize", typeof(double), typeof(BlockArrow), new DrawingPropertyMetadata(0.5, DrawingPropertyMetadataOptions.AffectsRender));
	}
}
