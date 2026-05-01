using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Microsoft.Expression.Interactivity.Media
{
	// Token: 0x02000028 RID: 40
	public class StoryboardCompletedTrigger : StoryboardTrigger
	{
		// Token: 0x06000165 RID: 357 RVA: 0x00008AA2 File Offset: 0x00006CA2
		protected override void OnDetaching()
		{
			base.OnDetaching();
			if (base.Storyboard != null)
			{
				base.Storyboard.Completed -= new EventHandler(this.Storyboard_Completed);
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00008ACC File Offset: 0x00006CCC
		protected override void OnStoryboardChanged(DependencyPropertyChangedEventArgs args)
		{
			Storyboard storyboard = args.OldValue as Storyboard;
			Storyboard storyboard2 = args.NewValue as Storyboard;
			if (storyboard != storyboard2)
			{
				if (storyboard != null)
				{
					storyboard.Completed -= new EventHandler(this.Storyboard_Completed);
				}
				if (storyboard2 != null)
				{
					storyboard2.Completed += new EventHandler(this.Storyboard_Completed);
				}
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00008B21 File Offset: 0x00006D21
		private void Storyboard_Completed(object sender, EventArgs e)
		{
			base.InvokeActions(e);
		}
	}
}
