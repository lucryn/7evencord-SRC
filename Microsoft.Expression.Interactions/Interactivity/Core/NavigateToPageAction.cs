using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x02000020 RID: 32
	[DefaultTrigger(typeof(UIElement), typeof(EventTrigger), "MouseLeftButtonDown")]
	[DefaultTrigger(typeof(ButtonBase), typeof(EventTrigger), "Click")]
	public class NavigateToPageAction : TargetedTriggerAction<FrameworkElement>
	{
		// Token: 0x06000142 RID: 322 RVA: 0x00008590 File Offset: 0x00006790
		protected override void Invoke(object parameter)
		{
			INavigate navigate = Application.Current.RootVisual as INavigate;
			navigate.Navigate(new Uri(this.TargetPage, 2));
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000143 RID: 323 RVA: 0x000085C0 File Offset: 0x000067C0
		// (set) Token: 0x06000144 RID: 324 RVA: 0x000085D2 File Offset: 0x000067D2
		public string TargetPage
		{
			get
			{
				return (string)base.GetValue(NavigateToPageAction.TargetPageProperty);
			}
			set
			{
				base.SetValue(NavigateToPageAction.TargetPageProperty, value);
			}
		}

		// Token: 0x0400006D RID: 109
		public static readonly DependencyProperty TargetPageProperty = DependencyProperty.Register("TargetPage", typeof(string), typeof(NavigateToPageAction), new PropertyMetadata(string.Empty));
	}
}
