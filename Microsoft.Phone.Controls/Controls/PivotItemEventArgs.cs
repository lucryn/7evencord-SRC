using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000022 RID: 34
	public class PivotItemEventArgs : EventArgs
	{
		// Token: 0x06000187 RID: 391 RVA: 0x00007F3D File Offset: 0x00006F3D
		public PivotItemEventArgs()
		{
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00007F45 File Offset: 0x00006F45
		public PivotItemEventArgs(PivotItem item) : this()
		{
			this.Item = item;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00007F54 File Offset: 0x00006F54
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00007F5C File Offset: 0x00006F5C
		public PivotItem Item { get; private set; }
	}
}
