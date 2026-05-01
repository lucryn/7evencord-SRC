using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000019 RID: 25
	public class PanoramaItem : ContentControl
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00004B27 File Offset: 0x00003B27
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00004B2F File Offset: 0x00003B2F
		internal int StartPosition { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00004B38 File Offset: 0x00003B38
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00004B40 File Offset: 0x00003B40
		internal int ItemWidth { get; set; }

		// Token: 0x060000DE RID: 222 RVA: 0x00004B49 File Offset: 0x00003B49
		public PanoramaItem()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.PanoramaItem);
			base.DefaultStyleKey = typeof(PanoramaItem);
			base.Loaded += new RoutedEventHandler(this.PanoramaItem_Loaded);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004B82 File Offset: 0x00003B82
		private void PanoramaItem_Loaded(object sender, RoutedEventArgs e)
		{
			base.Loaded -= new RoutedEventHandler(this.PanoramaItem_Loaded);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.PanoramaItem);
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00004BA5 File Offset: 0x00003BA5
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00004BB2 File Offset: 0x00003BB2
		public object Header
		{
			get
			{
				return base.GetValue(PanoramaItem.HeaderProperty);
			}
			set
			{
				base.SetValue(PanoramaItem.HeaderProperty, value);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00004BC0 File Offset: 0x00003BC0
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00004BD2 File Offset: 0x00003BD2
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(PanoramaItem.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(PanoramaItem.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00004BE0 File Offset: 0x00003BE0
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00004BF2 File Offset: 0x00003BF2
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(PanoramaItem.OrientationProperty);
			}
			set
			{
				base.SetValue(PanoramaItem.OrientationProperty, value);
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004C08 File Offset: 0x00003C08
		protected override Size MeasureOverride(Size availableSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.PanoramaItem);
			Size result = base.MeasureOverride(availableSize);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.PanoramaItem);
			return result;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004C3C File Offset: 0x00003C3C
		protected override Size ArrangeOverride(Size finalSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.PanoramaItem);
			Size result = base.ArrangeOverride(finalSize);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.PanoramaItem);
			return result;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004C70 File Offset: 0x00003C70
		private static void OnOrientationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			PanoramaItem panoramaItem = (PanoramaItem)obj;
			panoramaItem.InvalidateMeasure();
			FrameworkElement frameworkElement = VisualTreeHelper.GetParent(panoramaItem) as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.InvalidateMeasure();
			}
		}

		// Token: 0x0400006E RID: 110
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(PanoramaItem), null);

		// Token: 0x0400006F RID: 111
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(PanoramaItem), null);

		// Token: 0x04000070 RID: 112
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(PanoramaItem), new PropertyMetadata(0, new PropertyChangedCallback(PanoramaItem.OnOrientationChanged)));
	}
}
