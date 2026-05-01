using System;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200005D RID: 93
	public class LinkUnlinkEventArgs : EventArgs
	{
		// Token: 0x06000355 RID: 853 RVA: 0x0000EFA1 File Offset: 0x0000D1A1
		public LinkUnlinkEventArgs(ContentPresenter cp)
		{
			this.ContentPresenter = cp;
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000EFB0 File Offset: 0x0000D1B0
		// (set) Token: 0x06000357 RID: 855 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
		public ContentPresenter ContentPresenter { get; private set; }
	}
}
