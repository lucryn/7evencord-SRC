using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x02000022 RID: 34
	public class RemoveElementAction : TargetedTriggerAction<FrameworkElement>
	{
		// Token: 0x0600014F RID: 335 RVA: 0x000087B0 File Offset: 0x000069B0
		protected override void Invoke(object parameter)
		{
			if (base.AssociatedObject != null && base.Target != null)
			{
				DependencyObject parent = base.Target.Parent;
				Panel panel = parent as Panel;
				if (panel != null)
				{
					panel.Children.Remove(base.Target);
					return;
				}
				ContentControl contentControl = parent as ContentControl;
				if (contentControl != null)
				{
					if (contentControl.Content == base.Target)
					{
						contentControl.Content = null;
					}
					return;
				}
				ItemsControl itemsControl = parent as ItemsControl;
				if (itemsControl != null)
				{
					itemsControl.Items.Remove(base.Target);
					return;
				}
				if (parent != null)
				{
					throw new InvalidOperationException(ExceptionStringTable.UnsupportedRemoveTargetExceptionMessage);
				}
			}
		}
	}
}
