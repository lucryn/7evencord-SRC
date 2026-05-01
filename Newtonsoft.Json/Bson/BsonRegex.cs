using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000087 RID: 135
	internal class BsonRegex : BsonToken
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00018C6C File Offset: 0x00016E6C
		// (set) Token: 0x06000672 RID: 1650 RVA: 0x00018C74 File Offset: 0x00016E74
		public BsonString Pattern { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x00018C7D File Offset: 0x00016E7D
		// (set) Token: 0x06000674 RID: 1652 RVA: 0x00018C85 File Offset: 0x00016E85
		public BsonString Options { get; set; }

		// Token: 0x06000675 RID: 1653 RVA: 0x00018C8E File Offset: 0x00016E8E
		public BsonRegex(string pattern, string options)
		{
			this.Pattern = new BsonString(pattern, false);
			this.Options = new BsonString(options, false);
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x00018CB0 File Offset: 0x00016EB0
		public override BsonType Type
		{
			get
			{
				return BsonType.Regex;
			}
		}
	}
}
