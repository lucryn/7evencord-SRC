using System;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200005E RID: 94
	public class GroupViewOpenedEventArgs : EventArgs
	{
		// Token: 0x06000358 RID: 856 RVA: 0x0000EFC1 File Offset: 0x0000D1C1
		internal GroupViewOpenedEventArgs(ItemsControl itemsControl)
		{
			this.ItemsControl = itemsControl;
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000EFD0 File Offset: 0x0000D1D0
		// (set) Token: 0x0600035A RID: 858 RVA: 0x0000EFD8 File Offset: 0x0000D1D8
		public ItemsControl ItemsControl { get; private set; }
	}
}
