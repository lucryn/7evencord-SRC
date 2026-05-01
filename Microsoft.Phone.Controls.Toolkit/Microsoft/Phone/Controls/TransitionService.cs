using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200000C RID: 12
	public static class TransitionService
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00002A8E File Offset: 0x00000C8E
		public static NavigationInTransition GetNavigationInTransition(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (NavigationInTransition)element.GetValue(TransitionService.NavigationInTransitionProperty);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002AAE File Offset: 0x00000CAE
		public static NavigationOutTransition GetNavigationOutTransition(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (NavigationOutTransition)element.GetValue(TransitionService.NavigationOutTransitionProperty);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002ACE File Offset: 0x00000CCE
		public static void SetNavigationInTransition(UIElement element, NavigationInTransition value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TransitionService.NavigationInTransitionProperty, value);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002AEA File Offset: 0x00000CEA
		public static void SetNavigationOutTransition(UIElement element, NavigationOutTransition value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TransitionService.NavigationOutTransitionProperty, value);
		}

		// Token: 0x0400001A RID: 26
		public static readonly DependencyProperty NavigationInTransitionProperty = DependencyProperty.RegisterAttached("NavigationInTransition", typeof(NavigationInTransition), typeof(TransitionService), null);

		// Token: 0x0400001B RID: 27
		public static readonly DependencyProperty NavigationOutTransitionProperty = DependencyProperty.RegisterAttached("NavigationOutTransition", typeof(NavigationOutTransition), typeof(TransitionService), null);
	}
}
