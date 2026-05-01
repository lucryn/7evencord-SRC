using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Markup;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x0200000C RID: 12
	[ContentProperty("Condition")]
	public class ConditionBehavior : Behavior<TriggerBase>
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004C RID: 76 RVA: 0x0000341C File Offset: 0x0000161C
		// (set) Token: 0x0600004D RID: 77 RVA: 0x0000342E File Offset: 0x0000162E
		public ICondition Condition
		{
			get
			{
				return (ICondition)base.GetValue(ConditionBehavior.ConditionProperty);
			}
			set
			{
				base.SetValue(ConditionBehavior.ConditionProperty, value);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003444 File Offset: 0x00001644
		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.PreviewInvoke += new EventHandler<PreviewInvokeEventArgs>(this.OnPreviewInvoke);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003463 File Offset: 0x00001663
		protected override void OnDetaching()
		{
			base.AssociatedObject.PreviewInvoke -= new EventHandler<PreviewInvokeEventArgs>(this.OnPreviewInvoke);
			base.OnDetaching();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003482 File Offset: 0x00001682
		private void OnPreviewInvoke(object sender, PreviewInvokeEventArgs e)
		{
			if (this.Condition != null)
			{
				e.Cancelling = !this.Condition.Evaluate();
			}
		}

		// Token: 0x0400001E RID: 30
		public static readonly DependencyProperty ConditionProperty = DependencyProperty.Register("Condition", typeof(ICondition), typeof(ConditionBehavior), new PropertyMetadata(null));
	}
}
