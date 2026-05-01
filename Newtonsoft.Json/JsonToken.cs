using System;

namespace Newtonsoft.Json
{
	// Token: 0x020000A8 RID: 168
	public enum JsonToken
	{
		// Token: 0x04000258 RID: 600
		None,
		// Token: 0x04000259 RID: 601
		StartObject,
		// Token: 0x0400025A RID: 602
		StartArray,
		// Token: 0x0400025B RID: 603
		StartConstructor,
		// Token: 0x0400025C RID: 604
		PropertyName,
		// Token: 0x0400025D RID: 605
		Comment,
		// Token: 0x0400025E RID: 606
		Raw,
		// Token: 0x0400025F RID: 607
		Integer,
		// Token: 0x04000260 RID: 608
		Float,
		// Token: 0x04000261 RID: 609
		String,
		// Token: 0x04000262 RID: 610
		Boolean,
		// Token: 0x04000263 RID: 611
		Null,
		// Token: 0x04000264 RID: 612
		Undefined,
		// Token: 0x04000265 RID: 613
		EndObject,
		// Token: 0x04000266 RID: 614
		EndArray,
		// Token: 0x04000267 RID: 615
		EndConstructor,
		// Token: 0x04000268 RID: 616
		Date,
		// Token: 0x04000269 RID: 617
		Bytes
	}
}
