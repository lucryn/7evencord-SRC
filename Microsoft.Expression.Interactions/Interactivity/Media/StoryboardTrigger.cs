using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace Microsoft.Expression.Interactivity.Media
{
	// Token: 0x02000027 RID: 39
	public abstract class StoryboardTrigger : TriggerBase<DependencyObject>
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00008A1A File Offset: 0x00006C1A
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00008A2C File Offset: 0x00006C2C
		public Storyboard Storyboard
		{
			get
			{
				return (Storyboard)base.GetValue(StoryboardTrigger.StoryboardProperty);
			}
			set
			{
				base.SetValue(StoryboardTrigger.StoryboardProperty, value);
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00008A3C File Offset: 0x00006C3C
		private static void OnStoryboardChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			StoryboardTrigger storyboardTrigger = sender as StoryboardTrigger;
			if (storyboardTrigger != null)
			{
				storyboardTrigger.OnStoryboardChanged(args);
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00008A5A File Offset: 0x00006C5A
		protected virtual void OnStoryboardChanged(DependencyPropertyChangedEventArgs args)
		{
		}

		// Token: 0x0400007A RID: 122
		public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register("Storyboard", typeof(Storyboard), typeof(StoryboardTrigger), new PropertyMetadata(new PropertyChangedCallback(StoryboardTrigger.OnStoryboardChanged)));
	}
}
