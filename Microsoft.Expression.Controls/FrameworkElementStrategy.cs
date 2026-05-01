using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000005 RID: 5
	internal class FrameworkElementStrategy : GeometryStrategy
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000029C8 File Offset: 0x00000BC8
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000029DA File Offset: 0x00000BDA
		private Visibility VisibilityListener
		{
			get
			{
				return (Visibility)base.GetValue(FrameworkElementStrategy.VisibilityListenerProperty);
			}
			set
			{
				base.SetValue(FrameworkElementStrategy.VisibilityListenerProperty, value);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000029ED File Offset: 0x00000BED
		// (set) Token: 0x06000029 RID: 41 RVA: 0x000029FF File Offset: 0x00000BFF
		private double WidthListener
		{
			get
			{
				return (double)base.GetValue(FrameworkElementStrategy.WidthListenerProperty);
			}
			set
			{
				base.SetValue(FrameworkElementStrategy.WidthListenerProperty, value);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002A12 File Offset: 0x00000C12
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00002A24 File Offset: 0x00000C24
		private double HeightListener
		{
			get
			{
				return (double)base.GetValue(FrameworkElementStrategy.HeightListenerProperty);
			}
			set
			{
				base.SetValue(FrameworkElementStrategy.HeightListenerProperty, value);
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002A38 File Offset: 0x00000C38
		public FrameworkElementStrategy(LayoutPath layoutPath) : base(layoutPath)
		{
			this.sourceElement = layoutPath.SourceElement;
			this.sourceElement.SizeChanged += new SizeChangedEventHandler(this.SourceElement_SizeChanged);
			base.SetListenerBinding(FrameworkElementStrategy.VisibilityListenerProperty, "Visibility");
			if (this.sourceElement.Parent is Canvas)
			{
				base.SetListenerBinding(FrameworkElementStrategy.WidthListenerProperty, "Width");
				base.SetListenerBinding(FrameworkElementStrategy.HeightListenerProperty, "Height");
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002AB1 File Offset: 0x00000CB1
		public override bool HasGeometryChanged()
		{
			return false;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002AB4 File Offset: 0x00000CB4
		protected Size Size
		{
			get
			{
				if (!this.size.HasValidArea())
				{
					this.size = new Size(this.sourceElement.ActualWidth, this.sourceElement.ActualHeight);
				}
				return this.size;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002AEC File Offset: 0x00000CEC
		protected override PathGeometry UpdateGeometry()
		{
			return new RectangleGeometry
			{
				Rect = new Rect(default(Point), this.Size)
			}.AsPathGeometry();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002B1F File Offset: 0x00000D1F
		public override void Unhook()
		{
			this.sourceElement.SizeChanged -= new SizeChangedEventHandler(this.SourceElement_SizeChanged);
			base.Unhook();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002B3E File Offset: 0x00000D3E
		public override IList<GeneralTransform> ComputeTransforms()
		{
			return null;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002B41 File Offset: 0x00000D41
		private void SourceElement_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.size = e.NewSize;
			base.LayoutPath.IsLayoutDirty = true;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002B5C File Offset: 0x00000D5C
		protected static void LayoutPropertyChangedInCavas(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElementStrategy frameworkElementStrategy = d as FrameworkElementStrategy;
			if (frameworkElementStrategy == null || frameworkElementStrategy.LayoutPath == null || !frameworkElementStrategy.LayoutPath.IsAttached)
			{
				return;
			}
			if (e.Property == FrameworkElementStrategy.WidthListenerProperty)
			{
				frameworkElementStrategy.size = new Size((double)e.NewValue, frameworkElementStrategy.size.Height);
			}
			else if (e.Property == FrameworkElementStrategy.HeightListenerProperty)
			{
				frameworkElementStrategy.size = new Size(frameworkElementStrategy.size.Width, (double)e.NewValue);
			}
			frameworkElementStrategy.LayoutPath.IsLayoutDirty = true;
		}

		// Token: 0x0400000A RID: 10
		private FrameworkElement sourceElement;

		// Token: 0x0400000B RID: 11
		private Size size;

		// Token: 0x0400000C RID: 12
		private static readonly DependencyProperty VisibilityListenerProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(FrameworkElementStrategy), new PropertyMetadata(new PropertyChangedCallback(GeometryStrategy.LayoutPropertyChanged)));

		// Token: 0x0400000D RID: 13
		private static readonly DependencyProperty WidthListenerProperty = DependencyProperty.Register("Width", typeof(double), typeof(FrameworkElementStrategy), new PropertyMetadata(new PropertyChangedCallback(FrameworkElementStrategy.LayoutPropertyChangedInCavas)));

		// Token: 0x0400000E RID: 14
		private static readonly DependencyProperty HeightListenerProperty = DependencyProperty.Register("Height", typeof(double), typeof(FrameworkElementStrategy), new PropertyMetadata(new PropertyChangedCallback(FrameworkElementStrategy.LayoutPropertyChangedInCavas)));
	}
}
