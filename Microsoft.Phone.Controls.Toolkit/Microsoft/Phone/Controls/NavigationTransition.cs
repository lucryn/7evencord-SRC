using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000003 RID: 3
	public class NavigationTransition : DependencyObject
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		// (remove) Token: 0x06000002 RID: 2 RVA: 0x00002108 File Offset: 0x00000308
		public event RoutedEventHandler BeginTransition;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000003 RID: 3 RVA: 0x00002140 File Offset: 0x00000340
		// (remove) Token: 0x06000004 RID: 4 RVA: 0x00002178 File Offset: 0x00000378
		public event RoutedEventHandler EndTransition;

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000021AD File Offset: 0x000003AD
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000021BF File Offset: 0x000003BF
		public TransitionElement Backward
		{
			get
			{
				return (TransitionElement)base.GetValue(NavigationTransition.BackwardProperty);
			}
			set
			{
				base.SetValue(NavigationTransition.BackwardProperty, value);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000021CD File Offset: 0x000003CD
		// (set) Token: 0x06000008 RID: 8 RVA: 0x000021DF File Offset: 0x000003DF
		public TransitionElement Forward
		{
			get
			{
				return (TransitionElement)base.GetValue(NavigationTransition.ForwardProperty);
			}
			set
			{
				base.SetValue(NavigationTransition.ForwardProperty, value);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021ED File Offset: 0x000003ED
		internal void OnBeginTransition()
		{
			if (this.BeginTransition != null)
			{
				this.BeginTransition.Invoke(this, new RoutedEventArgs());
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002208 File Offset: 0x00000408
		internal void OnEndTransition()
		{
			if (this.EndTransition != null)
			{
				this.EndTransition.Invoke(this, new RoutedEventArgs());
			}
		}

		// Token: 0x0400000A RID: 10
		public static readonly DependencyProperty BackwardProperty = DependencyProperty.Register("Backward", typeof(TransitionElement), typeof(NavigationTransition), null);

		// Token: 0x0400000B RID: 11
		public static readonly DependencyProperty ForwardProperty = DependencyProperty.Register("Forward", typeof(TransitionElement), typeof(NavigationTransition), null);
	}
}
