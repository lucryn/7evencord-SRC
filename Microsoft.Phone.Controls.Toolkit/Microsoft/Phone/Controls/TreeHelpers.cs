using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200002F RID: 47
	internal static class TreeHelpers
	{
		// Token: 0x06000185 RID: 389 RVA: 0x00007DE8 File Offset: 0x00005FE8
		public static IEnumerable<FrameworkElement> GetVisualAncestors(this FrameworkElement node)
		{
			for (FrameworkElement parent = node.GetVisualParent(); parent != null; parent = parent.GetVisualParent())
			{
				yield return parent;
			}
			yield break;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00007E05 File Offset: 0x00006005
		public static FrameworkElement GetVisualParent(this FrameworkElement node)
		{
			return VisualTreeHelper.GetParent(node) as FrameworkElement;
		}
	}
}
