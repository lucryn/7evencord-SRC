using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Controls
{
	// Token: 0x0200000A RID: 10
	internal class LineStrategy : ShapeStrategy
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00003090 File Offset: 0x00001290
		// (set) Token: 0x0600004E RID: 78 RVA: 0x000030A2 File Offset: 0x000012A2
		private double X1Listener
		{
			get
			{
				return (double)base.GetValue(LineStrategy.X1ListenerProperty);
			}
			set
			{
				base.SetValue(LineStrategy.X1ListenerProperty, value);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004F RID: 79 RVA: 0x000030B5 File Offset: 0x000012B5
		// (set) Token: 0x06000050 RID: 80 RVA: 0x000030C7 File Offset: 0x000012C7
		private double X2Listener
		{
			get
			{
				return (double)base.GetValue(LineStrategy.X2ListenerProperty);
			}
			set
			{
				base.SetValue(LineStrategy.X2ListenerProperty, value);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000030DA File Offset: 0x000012DA
		// (set) Token: 0x06000052 RID: 82 RVA: 0x000030EC File Offset: 0x000012EC
		private double Y1Listener
		{
			get
			{
				return (double)base.GetValue(LineStrategy.Y1ListenerProperty);
			}
			set
			{
				base.SetValue(LineStrategy.Y1ListenerProperty, value);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000053 RID: 83 RVA: 0x000030FF File Offset: 0x000012FF
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00003111 File Offset: 0x00001311
		private double Y2Listener
		{
			get
			{
				return (double)base.GetValue(LineStrategy.Y2ListenerProperty);
			}
			set
			{
				base.SetValue(LineStrategy.Y2ListenerProperty, value);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003124 File Offset: 0x00001324
		public LineStrategy(LayoutPath layoutPath) : base(layoutPath)
		{
			this.sourceLine = (Line)layoutPath.SourceElement;
			base.SetListenerBinding(LineStrategy.X1ListenerProperty, "X1");
			base.SetListenerBinding(LineStrategy.X2ListenerProperty, "X2");
			base.SetListenerBinding(LineStrategy.Y1ListenerProperty, "Y1");
			base.SetListenerBinding(LineStrategy.Y2ListenerProperty, "Y2");
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000318C File Offset: 0x0000138C
		protected override PathGeometry UpdateGeometry()
		{
			return new LineGeometry
			{
				StartPoint = new Point(this.sourceLine.X1, this.sourceLine.Y1),
				EndPoint = new Point(this.sourceLine.X2, this.sourceLine.Y2)
			}.AsPathGeometry();
		}

		// Token: 0x04000017 RID: 23
		private Line sourceLine;

		// Token: 0x04000018 RID: 24
		private static readonly DependencyProperty X1ListenerProperty = DependencyProperty.Register("X1Listener", typeof(double), typeof(LineStrategy), new PropertyMetadata(new PropertyChangedCallback(GeometryStrategy.LayoutPropertyChanged)));

		// Token: 0x04000019 RID: 25
		private static readonly DependencyProperty X2ListenerProperty = DependencyProperty.Register("X2Listener", typeof(double), typeof(LineStrategy), new PropertyMetadata(new PropertyChangedCallback(GeometryStrategy.LayoutPropertyChanged)));

		// Token: 0x0400001A RID: 26
		private static readonly DependencyProperty Y1ListenerProperty = DependencyProperty.Register("Y1Listener", typeof(double), typeof(LineStrategy), new PropertyMetadata(new PropertyChangedCallback(GeometryStrategy.LayoutPropertyChanged)));

		// Token: 0x0400001B RID: 27
		private static readonly DependencyProperty Y2ListenerProperty = DependencyProperty.Register("Y2Listener", typeof(double), typeof(LineStrategy), new PropertyMetadata(new PropertyChangedCallback(GeometryStrategy.LayoutPropertyChanged)));
	}
}
