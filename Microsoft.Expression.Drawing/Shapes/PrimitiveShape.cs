using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Expression.Drawing.Core;
using Microsoft.Expression.Media;

namespace Microsoft.Expression.Shapes
{
	// Token: 0x02000045 RID: 69
	public abstract class PrimitiveShape : Path, IGeometrySourceParameters, IShape
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000254 RID: 596 RVA: 0x0000EE1C File Offset: 0x0000D01C
		// (remove) Token: 0x06000255 RID: 597 RVA: 0x0000EE54 File Offset: 0x0000D054
		public event EventHandler RenderedGeometryChanged;

		// Token: 0x06000256 RID: 598
		protected abstract IGeometrySource CreateGeometrySource();

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000EE8C File Offset: 0x0000D08C
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

		// Token: 0x06000258 RID: 600 RVA: 0x0000EEB4 File Offset: 0x0000D0B4
		public void InvalidateGeometry(InvalidateGeometryReasons reasons)
		{
			if (this.GeometrySource.InvalidateGeometry(reasons))
			{
				base.InvalidateArrange();
				if (Application.Current != null && Application.Current.RootVisual != null && (bool)Application.Current.RootVisual.GetValue(DesignerProperties.IsInDesignModeProperty) && (reasons & InvalidateGeometryReasons.IsAnimated) != (InvalidateGeometryReasons)0 && this.GeometrySource.UpdateGeometry(this, this.ActualBounds()) && !this.realizeGeometryScheduled)
				{
					this.realizeGeometryScheduled = true;
					base.LayoutUpdated += new EventHandler(this.OnLayoutUpdated);
				}
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000EF3D File Offset: 0x0000D13D
		protected override Size MeasureOverride(Size availableSize)
		{
			return new Size(base.StrokeThickness, base.StrokeThickness);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000EF50 File Offset: 0x0000D150
		protected override Size ArrangeOverride(Size finalSize)
		{
			if (this.GeometrySource.UpdateGeometry(this, finalSize.Bounds()) && !this.realizeGeometryScheduled)
			{
				this.realizeGeometryScheduled = true;
				base.LayoutUpdated += new EventHandler(this.OnLayoutUpdated);
			}
			base.ArrangeOverride(finalSize);
			return finalSize;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000EF90 File Offset: 0x0000D190
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			this.realizeGeometryScheduled = false;
			base.LayoutUpdated -= new EventHandler(this.OnLayoutUpdated);
			this.RealizeGeometry();
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000EFB1 File Offset: 0x0000D1B1
		private void RealizeGeometry()
		{
			this.Data = this.GeometrySource.Geometry.CloneCurrentValue();
			if (this.RenderedGeometryChanged != null)
			{
				this.RenderedGeometryChanged.Invoke(this, EventArgs.Empty);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000EFE2 File Offset: 0x0000D1E2
		public Geometry RenderedGeometry
		{
			get
			{
				return this.GeometrySource.Geometry;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000EFF0 File Offset: 0x0000D1F0
		public Thickness GeometryMargin
		{
			get
			{
				if (this.Data == null)
				{
					return default(Thickness);
				}
				return this.GeometrySource.LogicalBounds.Subtract(this.Data.Bounds);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000F02A File Offset: 0x0000D22A
		// (set) Token: 0x06000260 RID: 608 RVA: 0x0000F03C File Offset: 0x0000D23C
		public Geometry Data
		{
			get
			{
				return (Geometry)base.GetValue(Path.DataProperty);
			}
			set
			{
				base.SetValue(Path.DataProperty, value);
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000F04A File Offset: 0x0000D24A
		protected PrimitiveShape()
		{
			this.SetListenerBinding(PrimitiveShape.StretchListenerProperty, "Stretch");
			this.SetListenerBinding(PrimitiveShape.ThicknessListenerProperty, "StrokeThickness");
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000F074 File Offset: 0x0000D274
		private void SetListenerBinding(DependencyProperty targetProperty, string sourceProperty)
		{
			base.SetBinding(targetProperty, new Binding(sourceProperty)
			{
				Source = this,
				Mode = 1
			});
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000F115 File Offset: 0x0000D315
		Stretch IGeometrySourceParameters.get_Stretch()
		{
			return base.Stretch;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000F11D File Offset: 0x0000D31D
		Brush IGeometrySourceParameters.get_Stroke()
		{
			return base.Stroke;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000F125 File Offset: 0x0000D325
		double IGeometrySourceParameters.get_StrokeThickness()
		{
			return base.StrokeThickness;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000F12D File Offset: 0x0000D32D
		Brush IShape.get_Fill()
		{
			return base.Fill;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000F135 File Offset: 0x0000D335
		void IShape.set_Fill(Brush A_1)
		{
			base.Fill = A_1;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000F13E File Offset: 0x0000D33E
		Brush IShape.get_Stroke()
		{
			return base.Stroke;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000F146 File Offset: 0x0000D346
		void IShape.set_Stroke(Brush A_1)
		{
			base.Stroke = A_1;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000F14F File Offset: 0x0000D34F
		double IShape.get_StrokeThickness()
		{
			return base.StrokeThickness;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000F157 File Offset: 0x0000D357
		void IShape.set_StrokeThickness(double A_1)
		{
			base.StrokeThickness = A_1;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000F160 File Offset: 0x0000D360
		Stretch IShape.get_Stretch()
		{
			return base.Stretch;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000F168 File Offset: 0x0000D368
		void IShape.set_Stretch(Stretch A_1)
		{
			base.Stretch = A_1;
		}

		// Token: 0x040000C9 RID: 201
		private IGeometrySource geometrySource;

		// Token: 0x040000CA RID: 202
		private bool realizeGeometryScheduled;

		// Token: 0x040000CB RID: 203
		private static readonly DependencyProperty StretchListenerProperty = DependencyProperty.Register("StretchListener", typeof(Stretch), typeof(PrimitiveShape), new DrawingPropertyMetadata(1, DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x040000CC RID: 204
		private static readonly DependencyProperty ThicknessListenerProperty = DependencyProperty.Register("ThicknessListener", typeof(double), typeof(PrimitiveShape), new DrawingPropertyMetadata(1.0, DrawingPropertyMetadataOptions.AffectsRender));
	}
}
