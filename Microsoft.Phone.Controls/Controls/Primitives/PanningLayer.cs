using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x02000014 RID: 20
	[TemplatePart(Name = "LeftWraparound", Type = typeof(Rectangle))]
	[TemplatePart(Name = "PanningTransform", Type = typeof(TranslateTransform))]
	[TemplatePart(Name = "RightWraparound", Type = typeof(Rectangle))]
	[TemplatePart(Name = "ContentPresenter", Type = typeof(ContentPresenter))]
	[TemplatePart(Name = "LocalTransform", Type = typeof(TranslateTransform))]
	public class PanningLayer : ContentControl
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002DDC File Offset: 0x00001DDC
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00002DE4 File Offset: 0x00001DE4
		protected TranslateTransform LocalTransform { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002DED File Offset: 0x00001DED
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00002DF5 File Offset: 0x00001DF5
		protected TranslateTransform PanningTransform { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002DFE File Offset: 0x00001DFE
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00002E06 File Offset: 0x00001E06
		protected Rectangle LeftWraparound { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00002E0F File Offset: 0x00001E0F
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00002E17 File Offset: 0x00001E17
		protected Rectangle RightWraparound { get; set; }

		// Token: 0x06000073 RID: 115 RVA: 0x00002E20 File Offset: 0x00001E20
		public PanningLayer()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.PanningLayer);
			base.DefaultStyleKey = typeof(PanningLayer);
			base.Loaded += new RoutedEventHandler(this.PanningLayer_Loaded);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002E80 File Offset: 0x00001E80
		private void PanningLayer_Loaded(object sender, RoutedEventArgs e)
		{
			base.Loaded -= new RoutedEventHandler(this.PanningLayer_Loaded);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.PanningLayer);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002EA4 File Offset: 0x00001EA4
		public override void OnApplyTemplate()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_APPLYTEMPLATE, PerfLog.PanningLayer);
			base.OnApplyTemplate();
			this.LocalTransform = (base.GetTemplateChild("LocalTransform") as TranslateTransform);
			this.PanningTransform = (base.GetTemplateChild("PanningTransform") as TranslateTransform);
			this.LeftWraparound = (base.GetTemplateChild("LeftWraparound") as Rectangle);
			this.RightWraparound = (base.GetTemplateChild("RightWraparound") as Rectangle);
			this.ContentPresenter = (base.GetTemplateChild("ContentPresenter") as ContentPresenter);
			this.animator = ((this.PanningTransform != null) ? new TransformAnimator(this.PanningTransform) : null);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_APPLYTEMPLATE, PerfLog.PanningLayer);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002F60 File Offset: 0x00001F60
		protected override Size MeasureOverride(Size availableSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.PanningLayer);
			Size result = base.MeasureOverride(availableSize);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.PanningLayer);
			return result;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002F94 File Offset: 0x00001F94
		protected override Size ArrangeOverride(Size finalSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.PanningLayer);
			Size result = base.ArrangeOverride(finalSize);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.PanningLayer);
			return result;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002FC8 File Offset: 0x00001FC8
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00002FD0 File Offset: 0x00001FD0
		protected internal ContentPresenter ContentPresenter
		{
			get
			{
				return this.contentPresenter;
			}
			set
			{
				if (this.contentPresenter != null)
				{
					this.contentPresenter.SizeChanged -= new SizeChangedEventHandler(this.OnContentSizeChanged);
				}
				this.contentPresenter = value;
				if (this.contentPresenter != null)
				{
					this.contentPresenter.SizeChanged += new SizeChangedEventHandler(this.OnContentSizeChanged);
				}
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003022 File Offset: 0x00002022
		protected virtual double PanRate
		{
			get
			{
				return 1.0;
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000302D File Offset: 0x0000202D
		public void GoTo(int targetOffset, Duration duration, Action completionAction)
		{
			if (this.animator != null && !this.IsStatic)
			{
				this.animator.GoTo((double)((int)((double)targetOffset * this.PanRate)), duration, this._easingFunction, completionAction);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003060 File Offset: 0x00002060
		public virtual void Wraparound(int direction)
		{
			if (direction < 0)
			{
				this.GoTo((int)((this.ActualOffset + this.ContentPresenter.ActualWidth) / this.PanRate), PanningLayer.Immediately, null);
				return;
			}
			this.GoTo((int)((this.ActualOffset - this.ContentPresenter.ActualWidth) / this.PanRate), PanningLayer.Immediately, null);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000030BE File Offset: 0x000020BE
		internal void Refresh()
		{
			this.UpdateWrappingRectangles();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000030C8 File Offset: 0x000020C8
		protected virtual void UpdateWrappingRectangles()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_UPDATEWRAPPING, PerfLog.PanningLayer);
			ItemsPresenter itemsPresenter = base.Content as ItemsPresenter;
			bool flag = itemsPresenter == null || this.Owner.Panel == null || this.Owner.Panel.VisibleChildren.Count < 3;
			this.LeftWraparound.Visibility = (this.RightWraparound.Visibility = ((this.IsStatic || !flag) ? 1 : 0));
			if (!this.IsStatic && flag)
			{
				this.RightWraparound.Height = (this.LeftWraparound.Height = this.ContentPresenter.ActualHeight);
				this.RightWraparound.Width = (this.LeftWraparound.Width = (double)this.Owner.ViewportWidth);
				this.LeftWraparound.Margin = (this.RightWraparound.Margin = this.ContentPresenter.Margin);
				WriteableBitmap writeableBitmap = new WriteableBitmap(this.Owner.ViewportWidth, (int)this.ContentPresenter.ActualHeight);
				TranslateTransform translateTransform = new TranslateTransform();
				writeableBitmap.Render(this.ContentPresenter, translateTransform);
				writeableBitmap.Invalidate();
				this.RightWraparound.Fill = new ImageBrush
				{
					ImageSource = writeableBitmap
				};
				writeableBitmap = new WriteableBitmap(this.Owner.ViewportWidth, (int)this.ContentPresenter.ActualHeight);
				translateTransform.X = (double)this.Owner.ViewportWidth - this.ContentPresenter.ActualWidth;
				writeableBitmap.Render(this.ContentPresenter, translateTransform);
				writeableBitmap.Invalidate();
				this.LeftWraparound.Fill = new ImageBrush
				{
					ImageSource = writeableBitmap
				};
			}
			if (this.LocalTransform != null)
			{
				double num = (this.LeftWraparound.Visibility == null) ? (-this.LeftWraparound.Width - this.LeftWraparound.Margin.Left) : 0.0;
				this.LocalTransform.X = (this.IsStatic ? 0.0 : (num - this.LeftWraparound.Margin.Right));
			}
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_UPDATEWRAPPING, PerfLog.PanningLayer);
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000330D File Offset: 0x0000230D
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00003315 File Offset: 0x00002315
		internal bool IsStatic
		{
			get
			{
				return this.isStatic;
			}
			set
			{
				if (value != this.isStatic)
				{
					this.isStatic = value;
					if (this.isStatic)
					{
						this.ActualOffset = 0.0;
						return;
					}
					this.UpdateWrappingRectangles();
				}
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003345 File Offset: 0x00002345
		// (set) Token: 0x06000082 RID: 130 RVA: 0x0000334D File Offset: 0x0000234D
		internal Panorama Owner { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003356 File Offset: 0x00002356
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00003375 File Offset: 0x00002375
		internal double ActualOffset
		{
			get
			{
				if (this.PanningTransform == null)
				{
					return 0.0;
				}
				return this.PanningTransform.X;
			}
			private set
			{
				if (this.PanningTransform != null)
				{
					this.PanningTransform.X = value;
				}
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000338B File Offset: 0x0000238B
		private void OnContentSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.UpdateWrappingRectangles();
		}

		// Token: 0x04000037 RID: 55
		private const string LocalTransformName = "LocalTransform";

		// Token: 0x04000038 RID: 56
		private const string PanningTransformName = "PanningTransform";

		// Token: 0x04000039 RID: 57
		private const string LeftWraparoundName = "LeftWraparound";

		// Token: 0x0400003A RID: 58
		private const string RightWraparoundName = "RightWraparound";

		// Token: 0x0400003B RID: 59
		private const string ContentPresenterName = "ContentPresenter";

		// Token: 0x0400003C RID: 60
		protected static readonly Duration Immediately = new Duration(TimeSpan.Zero);

		// Token: 0x0400003D RID: 61
		private readonly IEasingFunction _easingFunction = new ExponentialEase
		{
			Exponent = 5.0
		};

		// Token: 0x0400003E RID: 62
		private ContentPresenter contentPresenter;

		// Token: 0x0400003F RID: 63
		private TransformAnimator animator;

		// Token: 0x04000040 RID: 64
		private bool isStatic;
	}
}
