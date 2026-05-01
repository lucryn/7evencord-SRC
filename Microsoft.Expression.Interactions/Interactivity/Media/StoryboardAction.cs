using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace Microsoft.Expression.Interactivity.Media
{
	// Token: 0x02000024 RID: 36
	public abstract class StoryboardAction : TriggerAction<DependencyObject>
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00008851 File Offset: 0x00006A51
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00008863 File Offset: 0x00006A63
		public Storyboard Storyboard
		{
			get
			{
				return (Storyboard)base.GetValue(StoryboardAction.StoryboardProperty);
			}
			set
			{
				base.SetValue(StoryboardAction.StoryboardProperty, value);
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00008874 File Offset: 0x00006A74
		private static void OnStoryboardChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			StoryboardAction storyboardAction = sender as StoryboardAction;
			if (storyboardAction != null)
			{
				storyboardAction.OnStoryboardChanged(args);
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00008892 File Offset: 0x00006A92
		protected virtual void OnStoryboardChanged(DependencyPropertyChangedEventArgs args)
		{
		}

		// Token: 0x04000070 RID: 112
		public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register("Storyboard", typeof(Storyboard), typeof(StoryboardAction), new PropertyMetadata(new PropertyChangedCallback(StoryboardAction.OnStoryboardChanged)));
	}
}
