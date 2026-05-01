using System;
using System.Windows;
using System.Windows.Markup;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x0200000B RID: 11
	[ContentProperty("Conditions")]
	public class ConditionalExpression : DependencyObject, ICondition
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000032F4 File Offset: 0x000014F4
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00003306 File Offset: 0x00001506
		public ForwardChaining ForwardChaining
		{
			get
			{
				return (ForwardChaining)base.GetValue(ConditionalExpression.ForwardChainingProperty);
			}
			set
			{
				base.SetValue(ConditionalExpression.ForwardChainingProperty, value);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003319 File Offset: 0x00001519
		public ConditionCollection Conditions
		{
			get
			{
				return (ConditionCollection)base.GetValue(ConditionalExpression.ConditionsProperty);
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000332B File Offset: 0x0000152B
		public ConditionalExpression()
		{
			base.SetValue(ConditionalExpression.ConditionsProperty, new ConditionCollection());
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003344 File Offset: 0x00001544
		public bool Evaluate()
		{
			bool flag = false;
			foreach (ComparisonCondition comparisonCondition in this.Conditions)
			{
				flag = comparisonCondition.Evaluate();
				if (!flag && this.ForwardChaining == ForwardChaining.And)
				{
					return flag;
				}
				if (flag && this.ForwardChaining == ForwardChaining.Or)
				{
					return flag;
				}
			}
			return flag;
		}

		// Token: 0x0400001C RID: 28
		public static readonly DependencyProperty ConditionsProperty = DependencyProperty.Register("Conditions", typeof(ConditionCollection), typeof(ConditionalExpression), new PropertyMetadata(null));

		// Token: 0x0400001D RID: 29
		public static readonly DependencyProperty ForwardChainingProperty = DependencyProperty.Register("ForwardChaining", typeof(ForwardChaining), typeof(ConditionalExpression), new PropertyMetadata(ForwardChaining.And));
	}
}
