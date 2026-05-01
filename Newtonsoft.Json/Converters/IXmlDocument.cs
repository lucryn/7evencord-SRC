using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200002B RID: 43
	internal interface IXmlDocument : IXmlNode
	{
		// Token: 0x06000312 RID: 786
		IXmlNode CreateComment(string text);

		// Token: 0x06000313 RID: 787
		IXmlNode CreateTextNode(string text);

		// Token: 0x06000314 RID: 788
		IXmlNode CreateCDataSection(string data);

		// Token: 0x06000315 RID: 789
		IXmlNode CreateWhitespace(string text);

		// Token: 0x06000316 RID: 790
		IXmlNode CreateSignificantWhitespace(string text);

		// Token: 0x06000317 RID: 791
		IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone);

		// Token: 0x06000318 RID: 792
		IXmlNode CreateProcessingInstruction(string target, string data);

		// Token: 0x06000319 RID: 793
		IXmlElement CreateElement(string elementName);

		// Token: 0x0600031A RID: 794
		IXmlElement CreateElement(string qualifiedName, string namespaceURI);

		// Token: 0x0600031B RID: 795
		IXmlNode CreateAttribute(string name, string value);

		// Token: 0x0600031C RID: 796
		IXmlNode CreateAttribute(string qualifiedName, string namespaceURI, string value);

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600031D RID: 797
		IXmlElement DocumentElement { get; }
	}
}
