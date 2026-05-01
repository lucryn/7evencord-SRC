using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000039 RID: 57
	internal enum BsonBinaryType : byte
	{
		// Token: 0x040000F0 RID: 240
		Binary,
		// Token: 0x040000F1 RID: 241
		Function,
		// Token: 0x040000F2 RID: 242
		[Obsolete("This type has been deprecated in the BSON specification. Use Binary instead.")]
		Data,
		// Token: 0x040000F3 RID: 243
		Uuid,
		// Token: 0x040000F4 RID: 244
		Md5 = 5,
		// Token: 0x040000F5 RID: 245
		UserDefined = 128
	}
}
