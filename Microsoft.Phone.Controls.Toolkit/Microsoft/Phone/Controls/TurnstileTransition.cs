using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000033 RID: 51
	public class TurnstileTransition : TransitionElement
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00008134 File Offset: 0x00006334
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00008146 File Offset: 0x00006346
		public TurnstileTransitionMode Mode
		{
			get
			{
				return (TurnstileTransitionMode)base.GetValue(TurnstileTransition.ModeProperty);
			}
			set
			{
				base.SetValue(TurnstileTransition.ModeProperty, value);
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00008159 File Offset: 0x00006359
		public override ITransition GetTransition(UIElement element)
		{
			return Transitions.Turnstile(element, this.Mode);
		}

		// Token: 0x0400009E RID: 158
		public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(TurnstileTransitionMode), typeof(TurnstileTransition), null);
	}
}
