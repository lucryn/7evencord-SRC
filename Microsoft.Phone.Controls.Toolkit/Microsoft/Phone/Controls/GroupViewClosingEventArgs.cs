using System;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200005F RID: 95
	public class GroupViewClosingEventArgs : EventArgs
	{
		// Token: 0x0600035B RID: 859 RVA: 0x0000EFE1 File Offset: 0x0000D1E1
		internal GroupViewClosingEventArgs(ItemsControl itemsControl, object selectedGroup)
		{
			this.ItemsControl = itemsControl;
			this.SelectedGroup = selectedGroup;
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0000EFF7 File Offset: 0x0000D1F7
		// (set) Token: 0x0600035D RID: 861 RVA: 0x0000EFFF File Offset: 0x0000D1FF
		public ItemsControl ItemsControl { get; private set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000F008 File Offset: 0x0000D208
		// (set) Token: 0x0600035F RID: 863 RVA: 0x0000F010 File Offset: 0x0000D210
		public object SelectedGroup { get; private set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0000F019 File Offset: 0x0000D219
		// (set) Token: 0x06000361 RID: 865 RVA: 0x0000F021 File Offset: 0x0000D221
		public bool Cancel { get; set; }
	}
}
