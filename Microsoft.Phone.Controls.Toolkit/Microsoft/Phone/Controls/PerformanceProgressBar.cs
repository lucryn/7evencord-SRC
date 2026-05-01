using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200005B RID: 91
	[TemplateVisualState(GroupName = "VisibilityStates", Name = "Hidden")]
	[TemplateVisualState(GroupName = "VisibilityStates", Name = "Normal")]
	public class PerformanceProgressBar : Control
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000ED39 File Offset: 0x0000CF39
		// (set) Token: 0x0600034A RID: 842 RVA: 0x0000ED4B File Offset: 0x0000CF4B
		public bool ActualIsIndeterminate
		{
			get
			{
				return (bool)base.GetValue(PerformanceProgressBar.ActualIsIndeterminateProperty);
			}
			set
			{
				base.SetValue(PerformanceProgressBar.ActualIsIndeterminateProperty, value);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000ED5E File Offset: 0x0000CF5E
		// (set) Token: 0x0600034C RID: 844 RVA: 0x0000ED70 File Offset: 0x0000CF70
		public bool IsIndeterminate
		{
			get
			{
				return (bool)base.GetValue(PerformanceProgressBar.IsIndeterminateProperty);
			}
			set
			{
				base.SetValue(PerformanceProgressBar.IsIndeterminateProperty, value);
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000ED84 File Offset: 0x0000CF84
		private static void OnIsIndeterminatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PerformanceProgressBar performanceProgressBar = d as PerformanceProgressBar;
			if (performanceProgressBar != null)
			{
				performanceProgressBar.OnIsIndeterminateChanged((bool)e.NewValue);
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000EDAD File Offset: 0x0000CFAD
		public PerformanceProgressBar()
		{
			base.DefaultStyleKey = typeof(PerformanceProgressBar);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000EDC8 File Offset: 0x0000CFC8
		public override void OnApplyTemplate()
		{
			if (this._visualStateGroup != null)
			{
				this._visualStateGroup.CurrentStateChanged -= new EventHandler<VisualStateChangedEventArgs>(this.OnVisualStateChanged);
			}
			base.OnApplyTemplate();
			this._visualStateGroup = VisualStates.TryGetVisualStateGroup(this, "VisibilityStates");
			if (this._visualStateGroup != null)
			{
				this._visualStateGroup.CurrentStateChanged += new EventHandler<VisualStateChangedEventArgs>(this.OnVisualStateChanged);
			}
			this.UpdateVisualStates(false);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000EE31 File Offset: 0x0000D031
		private void OnVisualStateChanged(object sender, VisualStateChangedEventArgs e)
		{
			if (this.ActualIsIndeterminate && e != null && e.NewState != null && e.NewState.Name == "Hidden" && !this.IsIndeterminate)
			{
				this.ActualIsIndeterminate = false;
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000EE6C File Offset: 0x0000D06C
		private void OnIsIndeterminateChanged(bool newValue)
		{
			if (newValue)
			{
				this.ActualIsIndeterminate = true;
			}
			else if (this.ActualIsIndeterminate && this._visualStateGroup == null)
			{
				this.ActualIsIndeterminate = false;
			}
			this.UpdateVisualStates(true);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000EE98 File Offset: 0x0000D098
		private void UpdateVisualStates(bool useTransitions)
		{
			VisualStateManager.GoToState(this, this.IsIndeterminate ? "Normal" : "Hidden", useTransitions);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000EEB8 File Offset: 0x0000D0B8
		private static T FindFirstChildOfType<T>(DependencyObject root) where T : class
		{
			Queue<DependencyObject> queue = new Queue<DependencyObject>();
			queue.Enqueue(root);
			while (0 < queue.Count)
			{
				DependencyObject dependencyObject = queue.Dequeue();
				int num = VisualTreeHelper.GetChildrenCount(dependencyObject) - 1;
				while (0 <= num)
				{
					DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, num);
					T t = child as T;
					if (t != null)
					{
						return t;
					}
					queue.Enqueue(child);
					num--;
				}
			}
			return default(T);
		}

		// Token: 0x040001BC RID: 444
		private const string VisualStateGroupName = "VisibilityStates";

		// Token: 0x040001BD RID: 445
		private const string NormalState = "Normal";

		// Token: 0x040001BE RID: 446
		private const string HiddenState = "Hidden";

		// Token: 0x040001BF RID: 447
		private VisualStateGroup _visualStateGroup;

		// Token: 0x040001C0 RID: 448
		public static readonly DependencyProperty ActualIsIndeterminateProperty = DependencyProperty.Register("ActualIsIndeterminate", typeof(bool), typeof(PerformanceProgressBar), new PropertyMetadata(false));

		// Token: 0x040001C1 RID: 449
		public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(PerformanceProgressBar), new PropertyMetadata(false, new PropertyChangedCallback(PerformanceProgressBar.OnIsIndeterminatePropertyChanged)));
	}
}
