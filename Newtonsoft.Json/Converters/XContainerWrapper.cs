using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000030 RID: 48
	internal class XContainerWrapper : XObjectWrapper
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000DE04 File Offset: 0x0000C004
		private XContainer Container
		{
			get
			{
				return (XContainer)base.WrappedNode;
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000DE11 File Offset: 0x0000C011
		public XContainerWrapper(XContainer container) : base(container)
		{
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000DE22 File Offset: 0x0000C022
		public override IList<IXmlNode> ChildNodes
		{
			get
			{
				return Enumerable.ToList<IXmlNode>(Enumerable.Select<XNode, IXmlNode>(this.Container.Nodes(), (XNode n) => XContainerWrapper.WrapNode(n)));
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000DE56 File Offset: 0x0000C056
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Container.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Container.Parent);
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000DE78 File Offset: 0x0000C078
		internal static IXmlNode WrapNode(XObject node)
		{
			if (node is XDocument)
			{
				return new XDocumentWrapper((XDocument)node);
			}
			if (node is XElement)
			{
				return new XElementWrapper((XElement)node);
			}
			if (node is XContainer)
			{
				return new XContainerWrapper((XContainer)node);
			}
			if (node is XProcessingInstruction)
			{
				return new XProcessingInstructionWrapper((XProcessingInstruction)node);
			}
			if (node is XText)
			{
				return new XTextWrapper((XText)node);
			}
			if (node is XComment)
			{
				return new XCommentWrapper((XComment)node);
			}
			if (node is XAttribute)
			{
				return new XAttributeWrapper((XAttribute)node);
			}
			return new XObjectWrapper(node);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000DF17 File Offset: 0x0000C117
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			this.Container.Add(newChild.WrappedNode);
			return newChild;
		}
	}
}
