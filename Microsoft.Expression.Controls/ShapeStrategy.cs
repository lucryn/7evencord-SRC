using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000006 RID: 6
	internal class ShapeStrategy : FrameworkElementStrategy
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002CA1 File Offset: 0x00000EA1
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002CB3 File Offset: 0x00000EB3
		private Stretch StretchListener
		{
			get
			{
				return (Stretch)base.GetValue(ShapeStrategy.StretchListenerProperty);
			}
			set
			{
				base.SetValue(ShapeStrategy.StretchListenerProperty, value);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002CC6 File Offset: 0x00000EC6
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002CD8 File Offset: 0x00000ED8
		private double StrokeThicknessListener
		{
			get
			{
				return (double)base.GetValue(ShapeStrategy.StrokeThicknessListenerProperty);
			}
			set
			{
				base.SetValue(ShapeStrategy.StrokeThicknessListenerProperty, value);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002CEB File Offset: 0x00000EEB
		public ShapeStrategy(LayoutPath layoutPath) : base(layoutPath)
		{
			this.sourceShape = (Shape)layoutPath.SourceElement;
			base.SetListenerBinding(ShapeStrategy.StretchListenerProperty, "Stretch");
			base.SetListenerBinding(ShapeStrategy.StrokeThicknessListenerProperty, "StrokeThickness");
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002D25 File Offset: 0x00000F25
		protected override PathGeometry UpdateGeometry()
		{
			return base.UpdateGeometry();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002D30 File Offset: 0x00000F30
		public override IList<GeneralTransform> ComputeTransforms()
		{
			IList<GeneralTransform> list = base.ComputeTransforms() ?? new List<GeneralTransform>();
			list.Add(this.sourceShape.GeometryTransform);
			return list;
		}

		// Token: 0x0400000F RID: 15
		private Shape sourceShape;

		// Token: 0x04000010 RID: 16
		private static readonly DependencyProperty StretchListenerProperty = DependencyProperty.Register("StretchListener", typeof(Stretch), typeof(ShapeStrategy), new PropertyMetadata(new PropertyChangedCallback(GeometryStrategy.LayoutPropertyChanged)));

		// Token: 0x04000011 RID: 17
		private static readonly DependencyProperty StrokeThicknessListenerProperty = DependencyProperty.Register("StrokeThicknessListener", typeof(double), typeof(ShapeStrategy), new PropertyMetadata(new PropertyChangedCallback(GeometryStrategy.LayoutPropertyChanged)));
	}
}
