using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x02000010 RID: 16
	public class PropertyChangedTrigger : TriggerBase<DependencyObject>
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003AB1 File Offset: 0x00001CB1
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00003ABE File Offset: 0x00001CBE
		public object Binding
		{
			get
			{
				return base.GetValue(PropertyChangedTrigger.BindingProperty);
			}
			set
			{
				base.SetValue(PropertyChangedTrigger.BindingProperty, value);
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003ACC File Offset: 0x00001CCC
		protected virtual void EvaluateBindingChange(object args)
		{
			base.InvokeActions(args);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003AD5 File Offset: 0x00001CD5
		protected override void OnAttached()
		{
			base.OnAttached();
			base.PreviewInvoke += new EventHandler<PreviewInvokeEventArgs>(this.OnPreviewInvoke);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003AEF File Offset: 0x00001CEF
		protected override void OnDetaching()
		{
			base.PreviewInvoke -= new EventHandler<PreviewInvokeEventArgs>(this.OnPreviewInvoke);
			this.OnDetaching();
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003B09 File Offset: 0x00001D09
		private void OnPreviewInvoke(object sender, PreviewInvokeEventArgs e)
		{
			DataBindingHelper.EnsureDataBindingOnActionsUpToDate(this);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003B14 File Offset: 0x00001D14
		private static void OnBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			PropertyChangedTrigger propertyChangedTrigger = (PropertyChangedTrigger)sender;
			propertyChangedTrigger.EvaluateBindingChange(args);
		}

		// Token: 0x04000024 RID: 36
		public static readonly DependencyProperty BindingProperty = DependencyProperty.Register("Binding", typeof(object), typeof(PropertyChangedTrigger), new PropertyMetadata(new PropertyChangedCallback(PropertyChangedTrigger.OnBindingChanged)));
	}
}
