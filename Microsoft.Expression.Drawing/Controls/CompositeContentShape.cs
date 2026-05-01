using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Expression.Drawing.Core;
using Microsoft.Expression.Media;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000004 RID: 4
	public abstract class CompositeContentShape : ContentControl, IGeometrySourceParameters, IShape
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000020D0 File Offset: 0x000002D0
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000020D8 File Offset: 0x000002D8
		private Path PartPath { get; set; }

		// Token: 0x06000013 RID: 19 RVA: 0x000020F4 File Offset: 0x000002F4
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.PartPath = Enumerable.FirstOrDefault<Path>(this.FindVisualDesendent((Path child) => child.Name == "PART_Path"));
			this.GeometrySource.InvalidateGeometry(InvalidateGeometryReasons.TemplateChanged);
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002143 File Offset: 0x00000343
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002155 File Offset: 0x00000355
		public Brush Fill
		{
			get
			{
				return (Brush)base.GetValue(CompositeContentShape.FillProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.FillProperty, value);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002163 File Offset: 0x00000363
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002175 File Offset: 0x00000375
		public Brush Stroke
		{
			get
			{
				return (Brush)base.GetValue(CompositeContentShape.StrokeProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeProperty, value);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002183 File Offset: 0x00000383
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002195 File Offset: 0x00000395
		public double StrokeThickness
		{
			get
			{
				return (double)base.GetValue(CompositeContentShape.StrokeThicknessProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeThicknessProperty, value);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000021A8 File Offset: 0x000003A8
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000021BA File Offset: 0x000003BA
		public Stretch Stretch
		{
			get
			{
				return (Stretch)base.GetValue(CompositeContentShape.StretchProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StretchProperty, value);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000021CD File Offset: 0x000003CD
		// (set) Token: 0x0600001D RID: 29 RVA: 0x000021DF File Offset: 0x000003DF
		public PenLineCap StrokeStartLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeContentShape.StrokeStartLineCapProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeStartLineCapProperty, value);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000021F2 File Offset: 0x000003F2
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002204 File Offset: 0x00000404
		public PenLineCap StrokeEndLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeContentShape.StrokeEndLineCapProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeEndLineCapProperty, value);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002217 File Offset: 0x00000417
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002229 File Offset: 0x00000429
		public PenLineJoin StrokeLineJoin
		{
			get
			{
				return (PenLineJoin)base.GetValue(CompositeContentShape.StrokeLineJoinProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeLineJoinProperty, value);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000022 RID: 34 RVA: 0x0000223C File Offset: 0x0000043C
		// (set) Token: 0x06000023 RID: 35 RVA: 0x0000224E File Offset: 0x0000044E
		public double StrokeMiterLimit
		{
			get
			{
				return (double)base.GetValue(CompositeContentShape.StrokeMiterLimitProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeMiterLimitProperty, value);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002261 File Offset: 0x00000461
		// (set) Token: 0x06000025 RID: 37 RVA: 0x00002273 File Offset: 0x00000473
		public DoubleCollection StrokeDashArray
		{
			get
			{
				return (DoubleCollection)base.GetValue(CompositeContentShape.StrokeDashArrayProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeDashArrayProperty, value);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002281 File Offset: 0x00000481
		// (set) Token: 0x06000027 RID: 39 RVA: 0x00002293 File Offset: 0x00000493
		public PenLineCap StrokeDashCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeContentShape.StrokeDashCapProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeDashCapProperty, value);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000022A6 File Offset: 0x000004A6
		// (set) Token: 0x06000029 RID: 41 RVA: 0x000022B8 File Offset: 0x000004B8
		public double StrokeDashOffset
		{
			get
			{
				return (double)base.GetValue(CompositeContentShape.StrokeDashOffsetProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeDashOffsetProperty, value);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000022CB File Offset: 0x000004CB
		public Geometry RenderedGeometry
		{
			get
			{
				return this.GeometrySource.Geometry;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000022D8 File Offset: 0x000004D8
		public Thickness GeometryMargin
		{
			get
			{
				if (this.PartPath == null || this.PartPath.Data == null)
				{
					return default(Thickness);
				}
				return this.GeometrySource.LogicalBounds.Subtract(this.PartPath.Data.Bounds);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002324 File Offset: 0x00000524
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00002331 File Offset: 0x00000531
		public object InternalContent
		{
			get
			{
				return base.GetValue(CompositeContentShape.InternalContentProperty);
			}
			private set
			{
				base.SetValue(CompositeContentShape.InternalContentProperty, value);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600002E RID: 46 RVA: 0x00002340 File Offset: 0x00000540
		// (remove) Token: 0x0600002F RID: 47 RVA: 0x00002378 File Offset: 0x00000578
		public event EventHandler RenderedGeometryChanged;

		// Token: 0x06000030 RID: 48
		protected abstract IGeometrySource CreateGeometrySource();

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000023B0 File Offset: 0x000005B0
		private IGeometrySource GeometrySource
		{
			get
			{
				IGeometrySource result;
				if ((result = this.geometrySource) == null)
				{
					result = (this.geometrySource = this.CreateGeometrySource());
				}
				return result;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000023D6 File Offset: 0x000005D6
		public void InvalidateGeometry(InvalidateGeometryReasons reasons)
		{
			if (this.GeometrySource.InvalidateGeometry(reasons))
			{
				base.InvalidateArrange();
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000023EC File Offset: 0x000005EC
		protected override Size ArrangeOverride(Size finalSize)
		{
			if (this.GeometrySource.UpdateGeometry(this, finalSize.Bounds()) && !this.realizeGeometryScheduled)
			{
				this.realizeGeometryScheduled = true;
				base.LayoutUpdated += new EventHandler(this.OnLayoutUpdated);
			}
			return base.ArrangeOverride(finalSize);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000242A File Offset: 0x0000062A
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			this.realizeGeometryScheduled = false;
			base.LayoutUpdated -= new EventHandler(this.OnLayoutUpdated);
			this.RealizeGeometry();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000244C File Offset: 0x0000064C
		private void RealizeGeometry()
		{
			if (this.PartPath != null)
			{
				this.PartPath.Data = this.RenderedGeometry.CloneCurrentValue();
				this.PartPath.Margin = this.GeometryMargin;
			}
			if (this.RenderedGeometryChanged != null)
			{
				this.RenderedGeometryChanged.Invoke(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000024A4 File Offset: 0x000006A4
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			IFormattable formattable = base.Content as IFormattable;
			string text = base.Content as string;
			if (formattable != null || text != null)
			{
				TextBlock textBlock = this.InternalContent as TextBlock;
				if (textBlock == null)
				{
					textBlock = (this.InternalContent = new TextBlock());
				}
				textBlock.TextAlignment = 0;
				textBlock.TextWrapping = 2;
				textBlock.TextTrimming = 2;
				textBlock.Text = (text ?? formattable.ToString(null, null));
				return;
			}
			this.InternalContent = base.Content;
		}

		// Token: 0x04000001 RID: 1
		public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(CompositeContentShape), new PropertyMetadata(null));

		// Token: 0x04000002 RID: 2
		public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(CompositeContentShape), new PropertyMetadata(null));

		// Token: 0x04000003 RID: 3
		public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(CompositeContentShape), new DrawingPropertyMetadata(1.0, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x04000004 RID: 4
		public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(CompositeContentShape), new DrawingPropertyMetadata(1, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x04000005 RID: 5
		public static readonly DependencyProperty StrokeStartLineCapProperty = DependencyProperty.Register("StrokeStartLineCap", typeof(PenLineCap), typeof(CompositeContentShape), new PropertyMetadata(0));

		// Token: 0x04000006 RID: 6
		public static readonly DependencyProperty StrokeEndLineCapProperty = DependencyProperty.Register("StrokeEndLineCap", typeof(PenLineCap), typeof(CompositeContentShape), new PropertyMetadata(0));

		// Token: 0x04000007 RID: 7
		public static readonly DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register("StrokeLineJoin", typeof(PenLineJoin), typeof(CompositeContentShape), new PropertyMetadata(0));

		// Token: 0x04000008 RID: 8
		public static readonly DependencyProperty StrokeMiterLimitProperty = DependencyProperty.Register("StrokeMiterLimit", typeof(double), typeof(CompositeContentShape), new PropertyMetadata(10.0));

		// Token: 0x04000009 RID: 9
		public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(CompositeContentShape), new PropertyMetadata(null));

		// Token: 0x0400000A RID: 10
		public static readonly DependencyProperty StrokeDashCapProperty = DependencyProperty.Register("StrokeDashCap", typeof(PenLineCap), typeof(CompositeContentShape), new PropertyMetadata(0));

		// Token: 0x0400000B RID: 11
		public static readonly DependencyProperty StrokeDashOffsetProperty = DependencyProperty.Register("StrokeDashOffset", typeof(double), typeof(CompositeContentShape), new PropertyMetadata(0.0));

		// Token: 0x0400000C RID: 12
		public static readonly DependencyProperty InternalContentProperty = DependencyProperty.Register("InternalContent", typeof(object), typeof(CompositeContentShape), new PropertyMetadata(null));

		// Token: 0x0400000E RID: 14
		private IGeometrySource geometrySource;

		// Token: 0x0400000F RID: 15
		private bool realizeGeometryScheduled;
	}
}
