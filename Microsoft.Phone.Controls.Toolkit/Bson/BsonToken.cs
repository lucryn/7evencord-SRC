using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000082 RID: 130
	internal abstract class BsonToken
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000659 RID: 1625
		public abstract BsonType Type { get; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x00018B38 File Offset: 0x00016D38
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x00018B40 File Offset: 0x00016D40
		public BsonToken Parent { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00018B49 File Offset: 0x00016D49
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x00018B51 File Offset: 0x00016D51
		public int CalculatedSize { get; set; }
	}
}
