using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200002C RID: 44
	internal interface IXmlDeclaration : IXmlNode
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600031E RID: 798
		string Version { get; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600031F RID: 799
		// (set) Token: 0x06000320 RID: 800
		string Encoding { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000321 RID: 801
		// (set) Token: 0x06000322 RID: 802
		string Standalone { get; set; }
	}
}
