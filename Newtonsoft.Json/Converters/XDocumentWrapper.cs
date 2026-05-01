using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000031 RID: 49
	internal class XDocumentWrapper : XContainerWrapper, IXmlDocument, IXmlNode
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000DF2B File Offset: 0x0000C12B
		private XDocument Document
		{
			get
			{
				return (XDocument)base.WrappedNode;
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000DF38 File Offset: 0x0000C138
		public XDocumentWrapper(XDocument document) : base(document)
		{
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000DF44 File Offset: 0x0000C144
		public override IList<IXmlNode> ChildNodes
		{
			get
			{
				IList<IXmlNode> childNodes = base.ChildNodes;
				if (this.Document.Declaration != null)
				{
					childNodes.Insert(0, new XDeclarationWrapper(this.Document.Declaration));
				}
				return childNodes;
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000DF7D File Offset: 0x0000C17D
		public IXmlNode CreateComment(string text)
		{
			return new XObjectWrapper(new XComment(text));
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000DF8A File Offset: 0x0000C18A
		public IXmlNode CreateTextNode(string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000DF97 File Offset: 0x0000C197
		public IXmlNode CreateCDataSection(string data)
		{
			return new XObjectWrapper(new XCData(data));
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000DFA4 File Offset: 0x0000C1A4
		public IXmlNode CreateWhitespace(string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000DFB1 File Offset: 0x0000C1B1
		public IXmlNode CreateSignificantWhitespace(string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000DFBE File Offset: 0x0000C1BE
		public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new XDeclarationWrapper(new XDeclaration(version, encoding, standalone));
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000DFCD File Offset: 0x0000C1CD
		public IXmlNode CreateProcessingInstruction(string target, string data)
		{
			return new XProcessingInstructionWrapper(new XProcessingInstruction(target, data));
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000DFDB File Offset: 0x0000C1DB
		public IXmlElement CreateElement(string elementName)
		{
			return new XElementWrapper(new XElement(elementName));
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000DFF0 File Offset: 0x0000C1F0
		public IXmlElement CreateElement(string qualifiedName, string namespaceURI)
		{
			string localName = MiscellaneousUtils.GetLocalName(qualifiedName);
			return new XElementWrapper(new XElement(XName.Get(localName, namespaceURI)));
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000E015 File Offset: 0x0000C215
		public IXmlNode CreateAttribute(string name, string value)
		{
			return new XAttributeWrapper(new XAttribute(name, value));
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000E028 File Offset: 0x0000C228
		public IXmlNode CreateAttribute(string qualifiedName, string namespaceURI, string value)
		{
			string localName = MiscellaneousUtils.GetLocalName(qualifiedName);
			return new XAttributeWrapper(new XAttribute(XName.Get(localName, namespaceURI), value));
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000E04E File Offset: 0x0000C24E
		public IXmlElement DocumentElement
		{
			get
			{
				if (this.Document.Root == null)
				{
					return null;
				}
				return new XElementWrapper(this.Document.Root);
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000E070 File Offset: 0x0000C270
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			XDeclarationWrapper xdeclarationWrapper = newChild as XDeclarationWrapper;
			if (xdeclarationWrapper != null)
			{
				this.Document.Declaration = xdeclarationWrapper._declaration;
				return xdeclarationWrapper;
			}
			return base.AppendChild(newChild);
		}
	}
}
