using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000009 RID: 9
	public class RollTransition : TransitionElement
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002A46 File Offset: 0x00000C46
		public override ITransition GetTransition(UIElement element)
		{
			return Transitions.Roll(element);
		}
	}
}
