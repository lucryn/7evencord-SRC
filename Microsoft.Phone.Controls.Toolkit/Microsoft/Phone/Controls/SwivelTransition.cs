using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000067 RID: 103
	public class SwivelTransition : TransitionElement
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0001194C File Offset: 0x0000FB4C
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x0001195E File Offset: 0x0000FB5E
		public SwivelTransitionMode Mode
		{
			get
			{
				return (SwivelTransitionMode)base.GetValue(SwivelTransition.ModeProperty);
			}
			set
			{
				base.SetValue(SwivelTransition.ModeProperty, value);
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00011971 File Offset: 0x0000FB71
		public override ITransition GetTransition(UIElement element)
		{
			return Transitions.Swivel(element, this.Mode);
		}

		// Token: 0x04000202 RID: 514
		public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(SwivelTransitionMode), typeof(SwivelTransition), null);
	}
}
