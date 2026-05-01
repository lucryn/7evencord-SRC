using System;

namespace W7Cord.Models
{
	// Token: 0x0200000E RID: 14
	public class Guild
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00004D68 File Offset: 0x00002F68
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00004D7F File Offset: 0x00002F7F
		public string id { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00004D88 File Offset: 0x00002F88
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00004D9F File Offset: 0x00002F9F
		public string name { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00004DA8 File Offset: 0x00002FA8
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00004DBF File Offset: 0x00002FBF
		public string icon { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00004DC8 File Offset: 0x00002FC8
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00004DDF File Offset: 0x00002FDF
		public bool owner { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00004DE8 File Offset: 0x00002FE8
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00004DFF File Offset: 0x00002FFF
		public long permissions { get; set; }
	}
}
