using System;
using System.Windows;
using Microsoft.Phone.Controls.Properties;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000043 RID: 67
	public class RoutedPropertyChangingEventArgs<T> : RoutedEventArgs
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000997B File Offset: 0x00007B7B
		// (set) Token: 0x0600022E RID: 558 RVA: 0x00009983 File Offset: 0x00007B83
		public DependencyProperty Property { get; private set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000998C File Offset: 0x00007B8C
		// (set) Token: 0x06000230 RID: 560 RVA: 0x00009994 File Offset: 0x00007B94
		public T OldValue { get; private set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000999D File Offset: 0x00007B9D
		// (set) Token: 0x06000232 RID: 562 RVA: 0x000099A5 File Offset: 0x00007BA5
		public T NewValue { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000233 RID: 563 RVA: 0x000099AE File Offset: 0x00007BAE
		// (set) Token: 0x06000234 RID: 564 RVA: 0x000099B6 File Offset: 0x00007BB6
		public bool IsCancelable { get; private set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000235 RID: 565 RVA: 0x000099BF File Offset: 0x00007BBF
		// (set) Token: 0x06000236 RID: 566 RVA: 0x000099C7 File Offset: 0x00007BC7
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				if (this.IsCancelable)
				{
					this._cancel = value;
					return;
				}
				if (value)
				{
					throw new InvalidOperationException(Resources.RoutedPropertyChangingEventArgs_CancelSet_InvalidOperation);
				}
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000237 RID: 567 RVA: 0x000099E7 File Offset: 0x00007BE7
		// (set) Token: 0x06000238 RID: 568 RVA: 0x000099EF File Offset: 0x00007BEF
		public bool InCoercion { get; set; }

		// Token: 0x06000239 RID: 569 RVA: 0x000099F8 File Offset: 0x00007BF8
		public RoutedPropertyChangingEventArgs(DependencyProperty property, T oldValue, T newValue, bool isCancelable)
		{
			this.Property = property;
			this.OldValue = oldValue;
			this.NewValue = newValue;
			this.IsCancelable = isCancelable;
			this.Cancel = false;
		}

		// Token: 0x040000D0 RID: 208
		private bool _cancel;
	}
}
