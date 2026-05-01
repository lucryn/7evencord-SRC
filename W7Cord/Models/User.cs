using System;

namespace W7Cord.Models
{
	// Token: 0x0200000D RID: 13
	public class User
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00004CA0 File Offset: 0x00002EA0
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00004CB7 File Offset: 0x00002EB7
		public string id { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00004CC0 File Offset: 0x00002EC0
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00004CD7 File Offset: 0x00002ED7
		public string username { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00004CE0 File Offset: 0x00002EE0
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00004CF7 File Offset: 0x00002EF7
		public string discriminator { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00004D00 File Offset: 0x00002F00
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00004D17 File Offset: 0x00002F17
		public string avatar { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00004D20 File Offset: 0x00002F20
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00004D37 File Offset: 0x00002F37
		public string email { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00004D40 File Offset: 0x00002F40
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00004D57 File Offset: 0x00002F57
		public bool verified { get; set; }
	}
}
