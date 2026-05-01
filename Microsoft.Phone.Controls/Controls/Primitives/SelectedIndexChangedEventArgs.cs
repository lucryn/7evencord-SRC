using System;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x02000023 RID: 35
	internal class SelectedIndexChangedEventArgs : EventArgs
	{
		// Token: 0x0600018B RID: 395 RVA: 0x00007F65 File Offset: 0x00006F65
		public SelectedIndexChangedEventArgs(int index)
		{
			this.SelectedIndex = index;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00007F74 File Offset: 0x00006F74
		// (set) Token: 0x0600018D RID: 397 RVA: 0x00007F7C File Offset: 0x00006F7C
		public int SelectedIndex { get; private set; }
	}
}
