using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000050 RID: 80
	public static class ContextMenuService
	{
		// Token: 0x0600030A RID: 778 RVA: 0x0000DD5D File Offset: 0x0000BF5D
		public static ContextMenu GetContextMenu(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (ContextMenu)element.GetValue(ContextMenuService.ContextMenuProperty);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000DD7D File Offset: 0x0000BF7D
		public static void SetContextMenu(DependencyObject element, ContextMenu value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.ContextMenuProperty, value);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000DD9C File Offset: 0x0000BF9C
		private static void OnContextMenuChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = o as FrameworkElement;
			if (frameworkElement != null)
			{
				ContextMenu contextMenu = e.OldValue as ContextMenu;
				if (contextMenu != null)
				{
					contextMenu.Owner = null;
				}
				ContextMenu contextMenu2 = e.NewValue as ContextMenu;
				if (contextMenu2 != null)
				{
					contextMenu2.Owner = frameworkElement;
				}
			}
		}

		// Token: 0x04000169 RID: 361
		public static readonly DependencyProperty ContextMenuProperty = DependencyProperty.RegisterAttached("ContextMenu", typeof(ContextMenu), typeof(ContextMenuService), new PropertyMetadata(null, new PropertyChangedCallback(ContextMenuService.OnContextMenuChanged)));
	}
}
