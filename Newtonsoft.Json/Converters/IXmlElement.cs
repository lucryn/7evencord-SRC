using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200002D RID: 45
	internal interface IXmlElement : IXmlNode
	{
		// Token: 0x06000323 RID: 803
		void SetAttributeNode(IXmlNode attribute);

		// Token: 0x06000324 RID: 804
		string GetPrefixOfNamespace(string namespaceURI);
	}
}
