using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x02000015 RID: 21
	public class PanningTitleLayer : PanningLayer
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000087 RID: 135 RVA: 0x000033A4 File Offset: 0x000023A4
		private double WidthAdjustment
		{
			get
			{
				return (double)base.Owner.ViewportWidth * 0.625;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000033BC File Offset: 0x000023BC
		protected override double PanRate
		{
			get
			{
				double result = 1.0;
				if (base.Owner != null && base.ContentPresenter != null)
				{
					result = (Math.Max((double)base.Owner.ViewportWidth, base.ContentPresenter.ActualWidth + this.WidthAdjustment) - (double)(base.Owner.ViewportWidth / 5 * 4)) / (double)Math.Max(base.Owner.ViewportWidth, base.Owner.ItemsWidth);
				}
				return result;
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003438 File Offset: 0x00002438
		public override void Wraparound(int direction)
		{
			if (direction < 0)
			{
				int targetOffset = (int)((base.ActualOffset + base.ContentPresenter.ActualWidth + this.WidthAdjustment) / this.PanRate);
				base.GoTo(targetOffset, PanningLayer.Immediately, null);
				return;
			}
			int targetOffset2 = (int)((base.ActualOffset - base.ContentPresenter.ActualWidth - this.WidthAdjustment) / this.PanRate);
			base.GoTo(targetOffset2, PanningLayer.Immediately, null);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000034A8 File Offset: 0x000024A8
		protected override void UpdateWrappingRectangles()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_UPDATEWRAPPING, PerfLog.PanningLayer);
			ItemsPresenter itemsPresenter = base.Content as ItemsPresenter;
			bool flag = itemsPresenter == null || base.Owner.Panel == null || base.Owner.Panel.VisibleChildren.Count < 3;
			base.RightWraparound.Visibility = ((base.IsStatic || !flag) ? 1 : 0);
			if (!base.IsStatic && flag)
			{
				base.RightWraparound.Height = base.ContentPresenter.ActualHeight;
				base.RightWraparound.Width = (double)base.Owner.ViewportWidth;
				WriteableBitmap writeableBitmap = new WriteableBitmap(base.Owner.ViewportWidth, (int)base.ContentPresenter.ActualHeight);
				TranslateTransform translateTransform = new TranslateTransform();
				writeableBitmap.Render(base.ContentPresenter, translateTransform);
				writeableBitmap.Invalidate();
				base.RightWraparound.Fill = new ImageBrush
				{
					ImageSource = writeableBitmap
				};
			}
			int num = (int)((double)base.Owner.ViewportWidth * 0.1);
			base.RightWraparound.Margin = new Thickness(this.WidthAdjustment + (double)num, 0.0, 0.0, 0.0);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_UPDATEWRAPPING, PerfLog.PanningLayer);
		}
	}
}
