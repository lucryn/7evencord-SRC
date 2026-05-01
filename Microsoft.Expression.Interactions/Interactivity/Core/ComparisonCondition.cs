using System;
using System.Windows;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x02000006 RID: 6
	public class ComparisonCondition : DependencyObject
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002F81 File Offset: 0x00001181
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00002F8E File Offset: 0x0000118E
		public object LeftOperand
		{
			get
			{
				return base.GetValue(ComparisonCondition.LeftOperandProperty);
			}
			set
			{
				base.SetValue(ComparisonCondition.LeftOperandProperty, value);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002F9C File Offset: 0x0000119C
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00002FA9 File Offset: 0x000011A9
		public object RightOperand
		{
			get
			{
				return base.GetValue(ComparisonCondition.RightOperandProperty);
			}
			set
			{
				base.SetValue(ComparisonCondition.RightOperandProperty, value);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002FB7 File Offset: 0x000011B7
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002FC9 File Offset: 0x000011C9
		public ComparisonConditionType Operator
		{
			get
			{
				return (ComparisonConditionType)base.GetValue(ComparisonCondition.OperatorProperty);
			}
			set
			{
				base.SetValue(ComparisonCondition.OperatorProperty, value);
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002FDC File Offset: 0x000011DC
		public bool Evaluate()
		{
			this.EnsureBindingUpToDate();
			return ComparisonLogic.EvaluateImpl(this.LeftOperand, this.Operator, this.RightOperand);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002FFB File Offset: 0x000011FB
		private void EnsureBindingUpToDate()
		{
			DataBindingHelper.EnsureBindingUpToDate(this, ComparisonCondition.LeftOperandProperty);
			DataBindingHelper.EnsureBindingUpToDate(this, ComparisonCondition.OperatorProperty);
			DataBindingHelper.EnsureBindingUpToDate(this, ComparisonCondition.RightOperandProperty);
		}

		// Token: 0x0400000F RID: 15
		public static readonly DependencyProperty LeftOperandProperty = DependencyProperty.Register("LeftOperand", typeof(object), typeof(ComparisonCondition), new PropertyMetadata(null));

		// Token: 0x04000010 RID: 16
		public static readonly DependencyProperty OperatorProperty = DependencyProperty.Register("Operator", typeof(ComparisonConditionType), typeof(ComparisonCondition), new PropertyMetadata(ComparisonConditionType.Equal));

		// Token: 0x04000011 RID: 17
		public static readonly DependencyProperty RightOperandProperty = DependencyProperty.Register("RightOperand", typeof(object), typeof(ComparisonCondition), new PropertyMetadata(null));
	}
}
