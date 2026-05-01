using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000036 RID: 54
	internal class XElementWrapper : XContainerWrapper, IXmlElement, IXmlNode
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000364 RID: 868 RVA: 0x0000E1F9 File Offset: 0x0000C3F9
		private XElement Element
		{
			get
			{
				return (XElement)base.WrappedNode;
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000E206 File Offset: 0x0000C406
		public XElementWrapper(XElement element) : base(element)
		{
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000E210 File Offset: 0x0000C410
		public void SetAttributeNode(IXmlNode attribute)
		{
			XObjectWrapper xobjectWrapper = (XObjectWrapper)attribute;
			this.Element.Add(xobjectWrapper.WrappedNode);
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000E23D File Offset: 0x0000C43D
		public override IList<IXmlNode> Attributes
		{
			get
			{
				return Enumerable.ToList<IXmlNode>(Enumerable.Cast<IXmlNode>(Enumerable.Select<XAttribute, XAttributeWrapper>(this.Element.Attributes(), (XAttribute a) => new XAttributeWrapper(a))));
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000368 RID: 872 RVA: 0x0000E276 File Offset: 0x0000C476
		// (set) Token: 0x06000369 RID: 873 RVA: 0x0000E283 File Offset: 0x0000C483
		public override string Value
		{
			get
			{
				return this.Element.Value;
			}
			set
			{
				this.Element.Value = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600036A RID: 874 RVA: 0x0000E291 File Offset: 0x0000C491
		public override string LocalName
		{
			get
			{
				return this.Element.Name.LocalName;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0000E2A3 File Offset: 0x0000C4A3
		public override string NamespaceURI
		{
			get
			{
				return this.Element.Name.NamespaceName;
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000E2B5 File Offset: 0x0000C4B5
		public string GetPrefixOfNamespace(string namespaceURI)
		{
			return this.Element.GetPrefixOfNamespace(namespaceURI);
		}
	}
}
