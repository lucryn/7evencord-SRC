using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Microsoft.Expression.Interactivity.Media
{
	// Token: 0x02000026 RID: 38
	[CLSCompliant(false)]
	public class ControlStoryboardAction : StoryboardAction
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000088DA File Offset: 0x00006ADA
		// (set) Token: 0x0600015A RID: 346 RVA: 0x000088EC File Offset: 0x00006AEC
		public ControlStoryboardOption ControlStoryboardOption
		{
			get
			{
				return (ControlStoryboardOption)base.GetValue(ControlStoryboardAction.ControlStoryboardProperty);
			}
			set
			{
				base.SetValue(ControlStoryboardAction.ControlStoryboardProperty, value);
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000088FF File Offset: 0x00006AFF
		protected override void OnStoryboardChanged(DependencyPropertyChangedEventArgs args)
		{
			this.isPaused = false;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00008908 File Offset: 0x00006B08
		protected override void Invoke(object parameter)
		{
			if (base.AssociatedObject != null && base.Storyboard != null)
			{
				switch (this.ControlStoryboardOption)
				{
				case ControlStoryboardOption.Play:
					base.Storyboard.Begin();
					return;
				case ControlStoryboardOption.Stop:
					base.Storyboard.Stop();
					return;
				case ControlStoryboardOption.TogglePlayPause:
				{
					ClockState clockState = 2;
					try
					{
						clockState = base.Storyboard.GetCurrentState();
					}
					catch (InvalidOperationException)
					{
					}
					if (clockState == 2)
					{
						this.isPaused = false;
						base.Storyboard.Begin();
						return;
					}
					if (this.isPaused)
					{
						this.isPaused = false;
						base.Storyboard.Resume();
						return;
					}
					this.isPaused = true;
					base.Storyboard.Pause();
					return;
				}
				case ControlStoryboardOption.Pause:
					base.Storyboard.Pause();
					return;
				case ControlStoryboardOption.Resume:
					base.Storyboard.Resume();
					return;
				case ControlStoryboardOption.SkipToFill:
					base.Storyboard.SkipToFill();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x04000078 RID: 120
		public static readonly DependencyProperty ControlStoryboardProperty = DependencyProperty.Register("ControlStoryboardOption", typeof(ControlStoryboardOption), typeof(ControlStoryboardAction), null);

		// Token: 0x04000079 RID: 121
		private bool isPaused;
	}
}
