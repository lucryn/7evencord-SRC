using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000013 RID: 19
	public interface IJsonLineInfo
	{
		// Token: 0x060000C5 RID: 197
		bool HasLineInfo();

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000C6 RID: 198
		int LineNumber { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000C7 RID: 199
		int LinePosition { get; }
	}
}
