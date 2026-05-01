using System;

namespace W7Cord.Models
{
	// Token: 0x0200000F RID: 15
	public class Channel
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00004E10 File Offset: 0x00003010
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00004E27 File Offset: 0x00003027
		public string id { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00004E30 File Offset: 0x00003030
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00004E47 File Offset: 0x00003047
		public string name { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00004E50 File Offset: 0x00003050
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00004E67 File Offset: 0x00003067
		public int type { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00004E70 File Offset: 0x00003070
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00004E87 File Offset: 0x00003087
		public string guild_id { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00004E90 File Offset: 0x00003090
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00004EA7 File Offset: 0x000030A7
		public int position { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004EB0 File Offset: 0x000030B0
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00004EC7 File Offset: 0x000030C7
		public string topic { get; set; }
	}
}
