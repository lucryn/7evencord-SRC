using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000008 RID: 8
	internal class RectangleStrategy : ShapeStrategy
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002E9B File Offset: 0x0000109B
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002EAD File Offset: 0x000010AD
		private double RadiusXListener
		{
			get
			{
				return (double)base.GetValue(RectangleStrategy.RadiusXListenerProperty);
			}
			set
			{
				base.SetValue(RectangleStrategy.RadiusXListenerProperty, value);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002EC0 File Offset: 0x000010C0
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002ED2 File Offset: 0x000010D2
		private double RadiusYListener
		{
			get
			{
				return (double)base.GetValue(RectangleStrategy.RadiusYListenerProperty);
			}
			set
			{
				base.SetValue(RectangleStrategy.RadiusYListenerProperty, value);
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002EE5 File Offset: 0x000010E5
		public RectangleStrategy(LayoutPath layoutPath) : base(layoutPath)
		{
			this.sourceRectangle = (Rectangle)layoutPath.SourceElement;
			base.SetListenerBinding(RectangleStrategy.RadiusXListenerProperty, "RadiusX");
			base.SetListenerBinding(RectangleStrategy.RadiusYListenerProperty, "RadiusY");
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002F1F File Offset: 0x0000111F
		public override IList<GeneralTransform> ComputeTransforms()
		{
			return null;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002F24 File Offset: 0x00001124
		protected override PathGeometry UpdateGeometry()
		{
			return new RectangleGeometry
			{
				Rect = new Rect(default(Point), base.Size),
				RadiusX = this.sourceRectangle.RadiusX,
				RadiusY = this.sourceRectangle.RadiusY
			}.AsPathGeometry();
		}

		// Token: 0x04000014 RID: 20
		private Rectangle sourceRectangle;

		// Token: 0x04000015 RID: 21
		private static readonly DependencyProperty RadiusXListenerProperty = DependencyProperty.Register("RadiusXListener", typeof(double), typeof(RectangleStrategy), new PropertyMetadata(new PropertyChangedCallback(GeometryStrategy.LayoutPropertyChanged)));

		// Token: 0x04000016 RID: 22
		private static readonly DependencyProperty RadiusYListenerProperty = DependencyProperty.Register("RadiusYListener", typeof(double), typeof(RectangleStrategy), new PropertyMetadata(new PropertyChangedCallback(GeometryStrategy.LayoutPropertyChanged)));
	}
}
