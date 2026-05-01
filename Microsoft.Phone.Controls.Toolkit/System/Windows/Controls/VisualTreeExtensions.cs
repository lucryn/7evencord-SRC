using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace System.Windows.Controls
{
	// Token: 0x0200001E RID: 30
	internal static class VisualTreeExtensions
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x00004AA0 File Offset: 0x00002CA0
		internal static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject parent)
		{
			int childCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int counter = 0; counter < childCount; counter++)
			{
				yield return VisualTreeHelper.GetChild(parent, counter);
			}
			yield break;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004C2C File Offset: 0x00002E2C
		internal static IEnumerable<FrameworkElement> GetLogicalChildrenBreadthFirst(this FrameworkElement parent)
		{
			Queue<FrameworkElement> queue = new Queue<FrameworkElement>(Enumerable.OfType<FrameworkElement>(parent.GetVisualChildren()));
			while (queue.Count > 0)
			{
				FrameworkElement element = queue.Dequeue();
				yield return element;
				foreach (FrameworkElement frameworkElement in Enumerable.OfType<FrameworkElement>(element.GetVisualChildren()))
				{
					queue.Enqueue(frameworkElement);
				}
			}
			yield break;
		}
	}
}
