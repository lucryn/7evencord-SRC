using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000008 RID: 8
	public class RotateTransition : TransitionElement
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003B RID: 59 RVA: 0x000029E5 File Offset: 0x00000BE5
		// (set) Token: 0x0600003C RID: 60 RVA: 0x000029F7 File Offset: 0x00000BF7
		public RotateTransitionMode Mode
		{
			get
			{
				return (RotateTransitionMode)base.GetValue(RotateTransition.ModeProperty);
			}
			set
			{
				base.SetValue(RotateTransition.ModeProperty, value);
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002A0A File Offset: 0x00000C0A
		public override ITransition GetTransition(UIElement element)
		{
			return Transitions.Rotate(element, this.Mode);
		}

		// Token: 0x04000017 RID: 23
		public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(RotateTransitionMode), typeof(RotateTransition), null);
	}
}
