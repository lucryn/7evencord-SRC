using System;

namespace W7Cord.Models
{
	// Token: 0x02000010 RID: 16
	public class DirectMessageChannel
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00004ED8 File Offset: 0x000030D8
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00004EEF File Offset: 0x000030EF
		public string id { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004EF8 File Offset: 0x000030F8
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00004F0F File Offset: 0x0000310F
		public int type { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00004F18 File Offset: 0x00003118
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00004F2F File Offset: 0x0000312F
		public User[] recipients { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00004F38 File Offset: 0x00003138
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00004F4F File Offset: 0x0000314F
		public string last_message_id { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00004F58 File Offset: 0x00003158
		public User Recipient
		{
			get
			{
				User result;
				if (this.recipients != null && this.recipients.Length > 0)
				{
					result = this.recipients[0];
				}
				else
				{
					result = null;
				}
				return result;
			}
		}
	}
}
