using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000091 RID: 145
	public enum WriteState
	{
		// Token: 0x040001E5 RID: 485
		Error,
		// Token: 0x040001E6 RID: 486
		Closed,
		// Token: 0x040001E7 RID: 487
		Object,
		// Token: 0x040001E8 RID: 488
		Array,
		// Token: 0x040001E9 RID: 489
		Constructor,
		// Token: 0x040001EA RID: 490
		Property,
		// Token: 0x040001EB RID: 491
		Start
	}
}
