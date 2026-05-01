using System;
using System.Collections.Generic;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200002A RID: 42
	internal interface IXmlNode
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000308 RID: 776
		XmlNodeType NodeType { get; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000309 RID: 777
		string LocalName { get; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600030A RID: 778
		IList<IXmlNode> ChildNodes { get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600030B RID: 779
		IList<IXmlNode> Attributes { get; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600030C RID: 780
		IXmlNode ParentNode { get; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600030D RID: 781
		// (set) Token: 0x0600030E RID: 782
		string Value { get; set; }

		// Token: 0x0600030F RID: 783
		IXmlNode AppendChild(IXmlNode newChild);

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000310 RID: 784
		string NamespaceURI { get; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000311 RID: 785
		object WrappedNode { get; }
	}
}
