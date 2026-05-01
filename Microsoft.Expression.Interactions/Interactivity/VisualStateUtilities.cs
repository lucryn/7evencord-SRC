using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Expression.Interactivity.Core;

namespace Microsoft.Expression.Interactivity
{
	// Token: 0x0200002C RID: 44
	public static class VisualStateUtilities
	{
		// Token: 0x0600017B RID: 379 RVA: 0x00009024 File Offset: 0x00007224
		public static bool GoToState(FrameworkElement element, string stateName, bool useTransitions)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(stateName))
			{
				Control control = element as Control;
				if (control != null)
				{
					control.ApplyTemplate();
					result = VisualStateManager.GoToState(control, stateName, useTransitions);
				}
				else
				{
					result = ExtendedVisualStateManager.GoToElementState(element, stateName, useTransitions);
				}
			}
			return result;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00009064 File Offset: 0x00007264
		public static IList GetVisualStateGroups(FrameworkElement targetObject)
		{
			IList list = new List<VisualStateGroup>();
			if (targetObject != null)
			{
				list = VisualStateManager.GetVisualStateGroups(targetObject);
				if (list.Count == 0)
				{
					int childrenCount = VisualTreeHelper.GetChildrenCount(targetObject);
					if (childrenCount > 0)
					{
						FrameworkElement frameworkElement = VisualTreeHelper.GetChild(targetObject, 0) as FrameworkElement;
						list = VisualStateManager.GetVisualStateGroups(frameworkElement);
					}
				}
			}
			return list;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000090AC File Offset: 0x000072AC
		public static bool TryFindNearestStatefulControl(FrameworkElement contextElement, out FrameworkElement resolvedControl)
		{
			FrameworkElement frameworkElement = contextElement;
			if (frameworkElement == null)
			{
				resolvedControl = null;
				return false;
			}
			FrameworkElement frameworkElement2 = frameworkElement.Parent as FrameworkElement;
			bool result = true;
			while (!VisualStateUtilities.HasVisualStateGroupsDefined(frameworkElement) && VisualStateUtilities.ShouldContinueTreeWalk(frameworkElement2))
			{
				frameworkElement = frameworkElement2;
				frameworkElement2 = (frameworkElement2.Parent as FrameworkElement);
			}
			if (VisualStateUtilities.HasVisualStateGroupsDefined(frameworkElement))
			{
				FrameworkElement frameworkElement3 = VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement;
				if (frameworkElement3 != null && frameworkElement3 is Control)
				{
					frameworkElement = frameworkElement3;
				}
			}
			else
			{
				result = false;
			}
			resolvedControl = frameworkElement;
			return result;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000911A File Offset: 0x0000731A
		private static bool HasVisualStateGroupsDefined(FrameworkElement frameworkElement)
		{
			return frameworkElement != null && VisualStateManager.GetVisualStateGroups(frameworkElement).Count != 0;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00009134 File Offset: 0x00007334
		internal static FrameworkElement FindNearestStatefulControl(FrameworkElement contextElement)
		{
			FrameworkElement result = null;
			VisualStateUtilities.TryFindNearestStatefulControl(contextElement, out result);
			return result;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00009150 File Offset: 0x00007350
		private static bool ShouldContinueTreeWalk(FrameworkElement element)
		{
			if (element == null)
			{
				return false;
			}
			if (element is UserControl)
			{
				return false;
			}
			if (element.Parent == null)
			{
				FrameworkElement frameworkElement = VisualStateUtilities.FindTemplatedParent(element);
				if (frameworkElement == null || (!(frameworkElement is Control) && !(frameworkElement is ContentPresenter)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00009191 File Offset: 0x00007391
		private static FrameworkElement FindTemplatedParent(FrameworkElement parent)
		{
			return VisualTreeHelper.GetParent(parent) as FrameworkElement;
		}
	}
}
