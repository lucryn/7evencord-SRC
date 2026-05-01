using System;

namespace W7Cord.Models
{
	// Token: 0x02000011 RID: 17
	public class Message
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004F9C File Offset: 0x0000319C
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00004FB3 File Offset: 0x000031B3
		public string id { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004FBC File Offset: 0x000031BC
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00004FD3 File Offset: 0x000031D3
		public string content { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00004FDC File Offset: 0x000031DC
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00004FF3 File Offset: 0x000031F3
		public string channel_id { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00004FFC File Offset: 0x000031FC
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00005013 File Offset: 0x00003213
		public User author { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000501C File Offset: 0x0000321C
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00005033 File Offset: 0x00003233
		public string timestamp { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000503C File Offset: 0x0000323C
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00005053 File Offset: 0x00003253
		public string edited_timestamp { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000505C File Offset: 0x0000325C
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00005073 File Offset: 0x00003273
		public bool pinned { get; set; }
	}
}
