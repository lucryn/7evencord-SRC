using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace Microsoft.Expression.Interactivity.Media
{
	// Token: 0x02000021 RID: 33
	public class PlaySoundAction : TriggerAction<DependencyObject>
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000861F File Offset: 0x0000681F
		// (set) Token: 0x06000149 RID: 329 RVA: 0x00008631 File Offset: 0x00006831
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(PlaySoundAction.SourceProperty);
			}
			set
			{
				base.SetValue(PlaySoundAction.SourceProperty, value);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600014A RID: 330 RVA: 0x0000863F File Offset: 0x0000683F
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00008651 File Offset: 0x00006851
		public double Volume
		{
			get
			{
				return (double)base.GetValue(PlaySoundAction.VolumeProperty);
			}
			set
			{
				base.SetValue(PlaySoundAction.VolumeProperty, value);
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00008664 File Offset: 0x00006864
		protected virtual void SetMediaElementProperties(MediaElement mediaElement)
		{
			if (mediaElement != null)
			{
				mediaElement.Source = this.Source;
				mediaElement.Volume = this.Volume;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000086C0 File Offset: 0x000068C0
		protected override void Invoke(object parameter)
		{
			if (this.Source == null || base.AssociatedObject == null)
			{
				return;
			}
			Popup popup = new Popup();
			MediaElement mediaElement = new MediaElement();
			popup.Child = mediaElement;
			mediaElement.Visibility = 1;
			this.SetMediaElementProperties(mediaElement);
			mediaElement.MediaEnded += delegate(object A_1, RoutedEventArgs A_2)
			{
				popup.Child = null;
				popup.IsOpen = false;
			};
			mediaElement.MediaFailed += delegate(object A_1, ExceptionRoutedEventArgs A_2)
			{
				popup.Child = null;
				popup.IsOpen = false;
			};
			popup.IsOpen = true;
		}

		// Token: 0x0400006E RID: 110
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(PlaySoundAction), null);

		// Token: 0x0400006F RID: 111
		public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register("Volume", typeof(double), typeof(PlaySoundAction), new PropertyMetadata(0.5));
	}
}
