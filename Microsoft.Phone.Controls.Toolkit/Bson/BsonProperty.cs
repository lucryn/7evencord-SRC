using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000088 RID: 136
	internal class BsonProperty
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x00018CB4 File Offset: 0x00016EB4
		// (set) Token: 0x06000678 RID: 1656 RVA: 0x00018CBC File Offset: 0x00016EBC
		public BsonString Name { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x00018CC5 File Offset: 0x00016EC5
		// (set) Token: 0x0600067A RID: 1658 RVA: 0x00018CCD File Offset: 0x00016ECD
		public BsonToken Value { get; set; }
	}
}
