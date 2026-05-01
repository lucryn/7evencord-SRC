using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace System.Windows.Interactivity
{
	// Token: 0x0200000A RID: 10
	public static class DependencyObjectHelper
	{
		// Token: 0x0600002B RID: 43 RVA: 0x000028E0 File Offset: 0x00000AE0
		public static IEnumerable<DependencyObject> GetSelfAndAncestors(this DependencyObject dependencyObject)
		{
			while (dependencyObject != null)
			{
				yield return dependencyObject;
				dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
			}
			yield break;
		}
	}
}
