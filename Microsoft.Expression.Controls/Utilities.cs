using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Microsoft.Expression.Controls
{
	// Token: 0x0200001D RID: 29
	internal static class Utilities
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00005AB0 File Offset: 0x00003CB0
		public static Panel GetItemsHost(this ItemsControl control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			DependencyObject dependencyObject = control.ItemContainerGenerator.ContainerFromIndex(0);
			if (dependencyObject != null)
			{
				return VisualTreeHelper.GetParent(dependencyObject) as Panel;
			}
			FrameworkElement frameworkElement = Enumerable.FirstOrDefault<DependencyObject>(control.GetVisualChildren()) as FrameworkElement;
			if (frameworkElement != null)
			{
				ItemsPresenter itemsPresenter = Enumerable.FirstOrDefault<ItemsPresenter>(Enumerable.OfType<ItemsPresenter>(frameworkElement.GetLogicalDescendents()));
				if (itemsPresenter != null && VisualTreeHelper.GetChildrenCount(itemsPresenter) > 0)
				{
					return VisualTreeHelper.GetChild(itemsPresenter, 0) as Panel;
				}
			}
			return null;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005CDC File Offset: 0x00003EDC
		internal static IEnumerable<T> TraverseBreadthFirst<T>(T initialNode, Func<T, IEnumerable<T>> getChildNodes, Func<T, bool> traversePredicate)
		{
			Queue<T> queue = new Queue<T>();
			queue.Enqueue(initialNode);
			while (queue.Count > 0)
			{
				T node = queue.Dequeue();
				if (traversePredicate.Invoke(node))
				{
					yield return node;
					IEnumerable<T> childNodes = getChildNodes.Invoke(node);
					foreach (T t in childNodes)
					{
						queue.Enqueue(t);
					}
				}
			}
			yield break;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005D07 File Offset: 0x00003F07
		public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return Enumerable.Skip<DependencyObject>(element.GetVisualChildrenAndSelfIterator(), 1);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005E58 File Offset: 0x00004058
		private static IEnumerable<DependencyObject> GetVisualChildrenAndSelfIterator(this DependencyObject element)
		{
			yield return element;
			int count = VisualTreeHelper.GetChildrenCount(element);
			for (int i = 0; i < count; i++)
			{
				yield return VisualTreeHelper.GetChild(element, i);
			}
			yield break;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000061F0 File Offset: 0x000043F0
		internal static IEnumerable<FrameworkElement> GetLogicalChildren(this FrameworkElement parent)
		{
			Popup popup = parent as Popup;
			if (popup != null)
			{
				FrameworkElement popupChild = popup.Child as FrameworkElement;
				if (popupChild != null)
				{
					yield return popupChild;
				}
			}
			ItemsControl itemsControl = parent as ItemsControl;
			if (itemsControl != null)
			{
				foreach (FrameworkElement logicalChild in Enumerable.OfType<FrameworkElement>(Enumerable.Select<int, DependencyObject>(Enumerable.Range(0, itemsControl.Items.Count), (int index) => itemsControl.ItemContainerGenerator.ContainerFromIndex(index))))
				{
					yield return logicalChild;
				}
			}
			string name = parent.Name;
			Queue<FrameworkElement> queue = new Queue<FrameworkElement>(Enumerable.OfType<FrameworkElement>(parent.GetVisualChildren()));
			while (queue.Count > 0)
			{
				FrameworkElement element = queue.Dequeue();
				if (element.Parent == parent || element is UserControl)
				{
					yield return element;
				}
				else
				{
					foreach (FrameworkElement frameworkElement in Enumerable.OfType<FrameworkElement>(element.GetVisualChildren()))
					{
						queue.Enqueue(frameworkElement);
					}
				}
			}
			yield break;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006218 File Offset: 0x00004418
		internal static IEnumerable<FrameworkElement> GetLogicalDescendents(this FrameworkElement parent)
		{
			return Utilities.TraverseBreadthFirst<FrameworkElement>(parent, (FrameworkElement node) => node.GetLogicalChildren(), (FrameworkElement node) => true);
		}
	}
}
