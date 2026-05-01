using System;
using System.Windows;
using Microsoft.Expression.Media;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000009 RID: 9
	public sealed class LineArrow : CompositeShape, ILineArrowGeometrySourceParameters, IGeometrySourceParameters
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00002E20 File Offset: 0x00001020
		protected override IGeometrySource CreateGeometrySource()
		{
			return new LineArrowGeometrySource();
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002E27 File Offset: 0x00001027
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00002E39 File Offset: 0x00001039
		public double BendAmount
		{
			get
			{
				return (double)base.GetValue(LineArrow.BendAmountProperty);
			}
			set
			{
				base.SetValue(LineArrow.BendAmountProperty, value);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00002E4C File Offset: 0x0000104C
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00002E5E File Offset: 0x0000105E
		public ArrowType StartArrow
		{
			get
			{
				return (ArrowType)base.GetValue(LineArrow.StartArrowProperty);
			}
			set
			{
				base.SetValue(LineArrow.StartArrowProperty, value);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00002E71 File Offset: 0x00001071
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00002E83 File Offset: 0x00001083
		public ArrowType EndArrow
		{
			get
			{
				return (ArrowType)base.GetValue(LineArrow.EndArrowProperty);
			}
			set
			{
				base.SetValue(LineArrow.EndArrowProperty, value);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00002E96 File Offset: 0x00001096
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00002EA8 File Offset: 0x000010A8
		public CornerType StartCorner
		{
			get
			{
				return (CornerType)base.GetValue(LineArrow.StartCornerProperty);
			}
			set
			{
				base.SetValue(LineArrow.StartCornerProperty, value);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002EBB File Offset: 0x000010BB
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00002ECD File Offset: 0x000010CD
		public double ArrowSize
		{
			get
			{
				return (double)base.GetValue(LineArrow.ArrowSizeProperty);
			}
			set
			{
				base.SetValue(LineArrow.ArrowSizeProperty, value);
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002EE0 File Offset: 0x000010E0
		public LineArrow()
		{
			base.DefaultStyleKey = typeof(LineArrow);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002EF8 File Offset: 0x000010F8
		protected override Size MeasureOverride(Size availableSize)
		{
			return base.MeasureOverride(new Size(0.0, 0.0));
		}

		// Token: 0x04000024 RID: 36
		public static readonly DependencyProperty BendAmountProperty = DependencyProperty.Register("BendAmount", typeof(double), typeof(LineArrow), new DrawingPropertyMetadata(0.5, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x04000025 RID: 37
		public static readonly DependencyProperty StartArrowProperty = DependencyProperty.Register("StartArrow", typeof(ArrowType), typeof(LineArrow), new DrawingPropertyMetadata(ArrowType.NoArrow, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x04000026 RID: 38
		public static readonly DependencyProperty EndArrowProperty = DependencyProperty.Register("EndArrow", typeof(ArrowType), typeof(LineArrow), new DrawingPropertyMetadata(ArrowType.Arrow, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x04000027 RID: 39
		public static readonly DependencyProperty StartCornerProperty = DependencyProperty.Register("StartCorner", typeof(CornerType), typeof(LineArrow), new DrawingPropertyMetadata(CornerType.TopLeft, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x04000028 RID: 40
		public static readonly DependencyProperty ArrowSizeProperty = DependencyProperty.Register("ArrowSize", typeof(double), typeof(LineArrow), new DrawingPropertyMetadata(10.0, DrawingPropertyMetadataOptions.AffectsRender));
	}
}
