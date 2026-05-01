using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000086 RID: 134
	internal class BsonString : BsonValue
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x00018C39 File Offset: 0x00016E39
		// (set) Token: 0x0600066D RID: 1645 RVA: 0x00018C41 File Offset: 0x00016E41
		public int ByteCount { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x00018C4A File Offset: 0x00016E4A
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x00018C52 File Offset: 0x00016E52
		public bool IncludeLength { get; set; }

		// Token: 0x06000670 RID: 1648 RVA: 0x00018C5B File Offset: 0x00016E5B
		public BsonString(object value, bool includeLength) : base(value, BsonType.String)
		{
			this.IncludeLength = includeLength;
		}
	}
}
