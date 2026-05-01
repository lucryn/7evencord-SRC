using System;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x02000016 RID: 22
	public class PanningBackgroundLayer : PanningLayer
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003608 File Offset: 0x00002608
		protected override double PanRate
		{
			get
			{
				double result = 1.0;
				if (base.Owner != null && base.ContentPresenter != null)
				{
					result = (Math.Max((double)base.Owner.ViewportWidth, base.ContentPresenter.ActualWidth) - (double)(base.Owner.ViewportWidth / 5 * 4)) / (double)Math.Max(base.Owner.ViewportWidth, base.Owner.ItemsWidth);
				}
				return result;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000367C File Offset: 0x0000267C
		protected override void UpdateWrappingRectangles()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_UPDATEWRAPPING, "Background Panning Layer");
			base.LeftWraparound.Visibility = (base.RightWraparound.Visibility = (base.IsStatic ? 1 : 0));
			if (!base.IsStatic && base.Owner.Background != null)
			{
				base.RightWraparound.Height = (base.LeftWraparound.Height = base.ContentPresenter.ActualHeight);
				base.RightWraparound.Width = (base.LeftWraparound.Width = (double)base.Owner.ViewportWidth);
				base.LeftWraparound.Margin = (base.RightWraparound.Margin = base.ContentPresenter.Margin);
				base.RightWraparound.Fill = base.Owner.Background;
				base.LeftWraparound.Fill = base.Owner.Background;
			}
			if (base.LocalTransform != null)
			{
				double num = (base.LeftWraparound.Visibility == null) ? (-base.LeftWraparound.Width - base.LeftWraparound.Margin.Left) : 0.0;
				base.LocalTransform.X = (base.IsStatic ? 0.0 : (num - base.LeftWraparound.Margin.Right));
			}
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_UPDATEWRAPPING, "Background Panning Layer");
		}
	}
}
