using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000046 RID: 70
	public class SlideTransition : TransitionElement
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600023A RID: 570 RVA: 0x00009A24 File Offset: 0x00007C24
		// (set) Token: 0x0600023B RID: 571 RVA: 0x00009A36 File Offset: 0x00007C36
		public SlideTransitionMode Mode
		{
			get
			{
				return (SlideTransitionMode)base.GetValue(SlideTransition.ModeProperty);
			}
			set
			{
				base.SetValue(SlideTransition.ModeProperty, value);
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00009A49 File Offset: 0x00007C49
		public override ITransition GetTransition(UIElement element)
		{
			return Transitions.Slide(element, this.Mode);
		}

		// Token: 0x040000EA RID: 234
		public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(SlideTransitionMode), typeof(SlideTransition), null);
	}
}
