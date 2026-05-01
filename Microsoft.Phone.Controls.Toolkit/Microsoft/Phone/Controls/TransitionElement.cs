using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000007 RID: 7
	public abstract class TransitionElement : DependencyObject
	{
		// Token: 0x06000039 RID: 57
		public abstract ITransition GetTransition(UIElement element);
	}
}
