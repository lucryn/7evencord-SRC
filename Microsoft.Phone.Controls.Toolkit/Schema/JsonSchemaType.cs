using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000006 RID: 6
	[Flags]
	public enum JsonSchemaType
	{
		// Token: 0x04000021 RID: 33
		None = 0,
		// Token: 0x04000022 RID: 34
		String = 1,
		// Token: 0x04000023 RID: 35
		Float = 2,
		// Token: 0x04000024 RID: 36
		Integer = 4,
		// Token: 0x04000025 RID: 37
		Boolean = 8,
		// Token: 0x04000026 RID: 38
		Object = 16,
		// Token: 0x04000027 RID: 39
		Array = 32,
		// Token: 0x04000028 RID: 40
		Null = 64,
		// Token: 0x04000029 RID: 41
		Any = 127
	}
}
