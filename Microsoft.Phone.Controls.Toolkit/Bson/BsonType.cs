using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000078 RID: 120
	internal enum BsonType : sbyte
	{
		// Token: 0x04000190 RID: 400
		Number = 1,
		// Token: 0x04000191 RID: 401
		String,
		// Token: 0x04000192 RID: 402
		Object,
		// Token: 0x04000193 RID: 403
		Array,
		// Token: 0x04000194 RID: 404
		Binary,
		// Token: 0x04000195 RID: 405
		Undefined,
		// Token: 0x04000196 RID: 406
		Oid,
		// Token: 0x04000197 RID: 407
		Boolean,
		// Token: 0x04000198 RID: 408
		Date,
		// Token: 0x04000199 RID: 409
		Null,
		// Token: 0x0400019A RID: 410
		Regex,
		// Token: 0x0400019B RID: 411
		Reference,
		// Token: 0x0400019C RID: 412
		Code,
		// Token: 0x0400019D RID: 413
		Symbol,
		// Token: 0x0400019E RID: 414
		CodeWScope,
		// Token: 0x0400019F RID: 415
		Integer,
		// Token: 0x040001A0 RID: 416
		TimeStamp,
		// Token: 0x040001A1 RID: 417
		Long,
		// Token: 0x040001A2 RID: 418
		MinKey = -1,
		// Token: 0x040001A3 RID: 419
		MaxKey = 127
	}
}
