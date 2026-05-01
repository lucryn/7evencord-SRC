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
	// Token: 0x02000007 RID: 7
	public abstract class CompositeShape : Control, IGeometrySourceParameters, IShape
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000043 RID: 67 RVA: 0x0000284D File Offset: 0x00000A4D
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002855 File Offset: 0x00000A55
		private Path PartPath { get; set; }

		// Token: 0x06000045 RID: 69 RVA: 0x00002870 File Offset: 0x00000A70
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.PartPath = Enumerable.FirstOrDefault<Path>(this.FindVisualDesendent((Path child) => child.Name == "PART_Path"));
			this.GeometrySource.InvalidateGeometry(InvalidateGeometryReasons.TemplateChanged);
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000028BF File Offset: 0x00000ABF
		// (set) Token: 0x06000047 RID: 71 RVA: 0x000028D1 File Offset: 0x00000AD1
		public Brush Fill
		{
			get
			{
				return (Brush)base.GetValue(CompositeShape.FillProperty);
			}
			set
			{
				base.SetValue(CompositeShape.FillProperty, value);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000028DF File Offset: 0x00000ADF
		// (set) Token: 0x06000049 RID: 73 RVA: 0x000028F1 File Offset: 0x00000AF1
		public Brush Stroke
		{
			get
			{
				return (Brush)base.GetValue(CompositeShape.StrokeProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeProperty, value);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000028FF File Offset: 0x00000AFF
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002911 File Offset: 0x00000B11
		public double StrokeThickness
		{
			get
			{
				return (double)base.GetValue(CompositeShape.StrokeThicknessProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeThicknessProperty, value);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002924 File Offset: 0x00000B24
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00002936 File Offset: 0x00000B36
		public Stretch Stretch
		{
			get
			{
				return (Stretch)base.GetValue(CompositeShape.StretchProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StretchProperty, value);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002949 File Offset: 0x00000B49
		// (set) Token: 0x0600004F RID: 79 RVA: 0x0000295B File Offset: 0x00000B5B
		public PenLineCap StrokeStartLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeShape.StrokeStartLineCapProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeStartLineCapProperty, value);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000050 RID: 80 RVA: 0x0000296E File Offset: 0x00000B6E
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00002980 File Offset: 0x00000B80
		public PenLineCap StrokeEndLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeShape.StrokeEndLineCapProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeEndLineCapProperty, value);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002993 File Offset: 0x00000B93
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000029A5 File Offset: 0x00000BA5
		public PenLineJoin StrokeLineJoin
		{
			get
			{
				return (PenLineJoin)base.GetValue(CompositeShape.StrokeLineJoinProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeLineJoinProperty, value);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000029B8 File Offset: 0x00000BB8
		// (set) Token: 0x06000055 RID: 85 RVA: 0x000029CA File Offset: 0x00000BCA
		public double StrokeMiterLimit
		{
			get
			{
				return (double)base.GetValue(CompositeShape.StrokeMiterLimitProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeMiterLimitProperty, value);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000029DD File Offset: 0x00000BDD
		// (set) Token: 0x06000057 RID: 87 RVA: 0x000029EF File Offset: 0x00000BEF
		public DoubleCollection StrokeDashArray
		{
			get
			{
				return (DoubleCollection)base.GetValue(CompositeShape.StrokeDashArrayProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeDashArrayProperty, value);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000029FD File Offset: 0x00000BFD
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00002A0F File Offset: 0x00000C0F
		public PenLineCap StrokeDashCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeShape.StrokeDashCapProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeDashCapProperty, value);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002A22 File Offset: 0x00000C22
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002A34 File Offset: 0x00000C34
		public double StrokeDashOffset
		{
			get
			{
				return (double)base.GetValue(CompositeShape.StrokeDashOffsetProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeDashOffsetProperty, value);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002A47 File Offset: 0x00000C47
		public Geometry RenderedGeometry
		{
			get
			{
				return this.GeometrySource.Geometry;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002A54 File Offset: 0x00000C54
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

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600005E RID: 94 RVA: 0x00002AA0 File Offset: 0x00000CA0
		// (remove) Token: 0x0600005F RID: 95 RVA: 0x00002AD8 File Offset: 0x00000CD8
		public event EventHandler RenderedGeometryChanged;

		// Token: 0x06000060 RID: 96
		protected abstract IGeometrySource CreateGeometrySource();

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002B10 File Offset: 0x00000D10
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

		// Token: 0x06000062 RID: 98 RVA: 0x00002B36 File Offset: 0x00000D36
		public void InvalidateGeometry(InvalidateGeometryReasons reasons)
		{
			if (this.GeometrySource.InvalidateGeometry(reasons))
			{
				base.InvalidateArrange();
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002B4C File Offset: 0x00000D4C
		protected override Size ArrangeOverride(Size finalSize)
		{
			if (this.GeometrySource.UpdateGeometry(this, finalSize.Bounds()) && !this.realizeGeometryScheduled)
			{
				this.realizeGeometryScheduled = true;
				base.LayoutUpdated += new EventHandler(this.OnLayoutUpdated);
			}
			return base.ArrangeOverride(finalSize);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002B8A File Offset: 0x00000D8A
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			this.realizeGeometryScheduled = false;
			base.LayoutUpdated -= new EventHandler(this.OnLayoutUpdated);
			this.RealizeGeometry();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002BAC File Offset: 0x00000DAC
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

		// Token: 0x04000014 RID: 20
		public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(CompositeShape), new PropertyMetadata(null));

		// Token: 0x04000015 RID: 21
		public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(CompositeShape), new PropertyMetadata(null));

		// Token: 0x04000016 RID: 22
		public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(CompositeShape), new DrawingPropertyMetadata(1.0, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x04000017 RID: 23
		public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(CompositeShape), new DrawingPropertyMetadata(1, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x04000018 RID: 24
		public static readonly DependencyProperty StrokeStartLineCapProperty = DependencyProperty.Register("StrokeStartLineCap", typeof(PenLineCap), typeof(CompositeShape), new PropertyMetadata(0));

		// Token: 0x04000019 RID: 25
		public static readonly DependencyProperty StrokeEndLineCapProperty = DependencyProperty.Register("StrokeEndLineCap", typeof(PenLineCap), typeof(CompositeShape), new PropertyMetadata(0));

		// Token: 0x0400001A RID: 26
		public static readonly DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register("StrokeLineJoin", typeof(PenLineJoin), typeof(CompositeShape), new PropertyMetadata(0));

		// Token: 0x0400001B RID: 27
		public static readonly DependencyProperty StrokeMiterLimitProperty = DependencyProperty.Register("StrokeMiterLimit", typeof(double), typeof(CompositeShape), new PropertyMetadata(10.0));

		// Token: 0x0400001C RID: 28
		public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(CompositeShape), new PropertyMetadata(null));

		// Token: 0x0400001D RID: 29
		public static readonly DependencyProperty StrokeDashCapProperty = DependencyProperty.Register("StrokeDashCap", typeof(PenLineCap), typeof(CompositeShape), new PropertyMetadata(0));

		// Token: 0x0400001E RID: 30
		public static readonly DependencyProperty StrokeDashOffsetProperty = DependencyProperty.Register("StrokeDashOffset", typeof(double), typeof(CompositeShape), new PropertyMetadata(0.0));

		// Token: 0x04000020 RID: 32
		private IGeometrySource geometrySource;

		// Token: 0x04000021 RID: 33
		private bool realizeGeometryScheduled;
	}
}
