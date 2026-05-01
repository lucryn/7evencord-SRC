using System;
using System.Windows;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x02000012 RID: 18
	public class DataTrigger : PropertyChangedTrigger
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003B7A File Offset: 0x00001D7A
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00003B87 File Offset: 0x00001D87
		public object Value
		{
			get
			{
				return base.GetValue(DataTrigger.ValueProperty);
			}
			set
			{
				base.SetValue(DataTrigger.ValueProperty, value);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003B95 File Offset: 0x00001D95
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00003BA7 File Offset: 0x00001DA7
		public ComparisonConditionType Comparison
		{
			get
			{
				return (ComparisonConditionType)base.GetValue(DataTrigger.ComparisonProperty);
			}
			set
			{
				base.SetValue(DataTrigger.ComparisonProperty, value);
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003BC2 File Offset: 0x00001DC2
		protected override void EvaluateBindingChange(object args)
		{
			if (this.Compare())
			{
				base.InvokeActions(args);
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003BD4 File Offset: 0x00001DD4
		private static void OnValueChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			DataTrigger dataTrigger = (DataTrigger)sender;
			dataTrigger.EvaluateBindingChange(args);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003BF4 File Offset: 0x00001DF4
		private static void OnComparisonChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			DataTrigger dataTrigger = (DataTrigger)sender;
			dataTrigger.EvaluateBindingChange(args);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003C14 File Offset: 0x00001E14
		private bool Compare()
		{
			return base.AssociatedObject != null && ComparisonLogic.EvaluateImpl(base.Binding, this.Comparison, this.Value);
		}

		// Token: 0x04000025 RID: 37
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(DataTrigger), new PropertyMetadata(new PropertyChangedCallback(DataTrigger.OnValueChanged)));

		// Token: 0x04000026 RID: 38
		public static readonly DependencyProperty ComparisonProperty = DependencyProperty.Register("Comparison", typeof(ComparisonConditionType), typeof(DataTrigger), new PropertyMetadata(new PropertyChangedCallback(DataTrigger.OnComparisonChanged)));
	}
}
