using System;
using System.Globalization;
using System.Windows;
using System.Windows.Interactivity;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x0200001C RID: 28
	public class GoToStateAction : TargetedTriggerAction<FrameworkElement>
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000100 RID: 256 RVA: 0x000074D3 File Offset: 0x000056D3
		// (set) Token: 0x06000101 RID: 257 RVA: 0x000074E5 File Offset: 0x000056E5
		public bool UseTransitions
		{
			get
			{
				return (bool)base.GetValue(GoToStateAction.UseTransitionsProperty);
			}
			set
			{
				base.SetValue(GoToStateAction.UseTransitionsProperty, value);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000102 RID: 258 RVA: 0x000074F8 File Offset: 0x000056F8
		// (set) Token: 0x06000103 RID: 259 RVA: 0x0000750A File Offset: 0x0000570A
		public string StateName
		{
			get
			{
				return (string)base.GetValue(GoToStateAction.StateNameProperty);
			}
			set
			{
				base.SetValue(GoToStateAction.StateNameProperty, value);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00007518 File Offset: 0x00005718
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00007520 File Offset: 0x00005720
		private FrameworkElement StateTarget { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000752C File Offset: 0x0000572C
		private bool IsTargetObjectSet
		{
			get
			{
				return base.ReadLocalValue(TargetedTriggerAction.TargetObjectProperty) != DependencyProperty.UnsetValue;
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00007550 File Offset: 0x00005750
		protected override void OnTargetChanged(FrameworkElement oldTarget, FrameworkElement newTarget)
		{
			base.OnTargetChanged(oldTarget, newTarget);
			FrameworkElement frameworkElement = null;
			if (string.IsNullOrEmpty(base.TargetName) && !this.IsTargetObjectSet)
			{
				if (!VisualStateUtilities.TryFindNearestStatefulControl(base.AssociatedObject as FrameworkElement, out frameworkElement) && frameworkElement != null)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.GoToStateActionTargetHasNoStateGroups, new object[]
					{
						frameworkElement.Name
					}));
				}
			}
			else
			{
				frameworkElement = base.Target;
			}
			this.StateTarget = frameworkElement;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000075C9 File Offset: 0x000057C9
		protected override void Invoke(object parameter)
		{
			if (base.AssociatedObject != null)
			{
				this.InvokeImpl(this.StateTarget);
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000075DF File Offset: 0x000057DF
		internal void InvokeImpl(FrameworkElement stateTarget)
		{
			if (stateTarget != null)
			{
				VisualStateUtilities.GoToState(stateTarget, this.StateName, this.UseTransitions);
			}
		}

		// Token: 0x0400005A RID: 90
		public static readonly DependencyProperty UseTransitionsProperty = DependencyProperty.Register("UseTransitions", typeof(bool), typeof(GoToStateAction), new PropertyMetadata(true));

		// Token: 0x0400005B RID: 91
		public static readonly DependencyProperty StateNameProperty = DependencyProperty.Register("StateName", typeof(string), typeof(GoToStateAction), new PropertyMetadata(string.Empty));
	}
}
