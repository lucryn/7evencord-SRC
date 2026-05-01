using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200002E RID: 46
	internal class XObjectWrapper : IXmlNode
	{
		// Token: 0x06000325 RID: 805 RVA: 0x0000DD65 File Offset: 0x0000BF65
		public XObjectWrapper(XObject xmlObject)
		{
			this._xmlObject = xmlObject;
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000DD74 File Offset: 0x0000BF74
		public object WrappedNode
		{
			get
			{
				return this._xmlObject;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0000DD7C File Offset: 0x0000BF7C
		public virtual XmlNodeType NodeType
		{
			get
			{
				return this._xmlObject.NodeType;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000DD89 File Offset: 0x0000BF89
		public virtual string LocalName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000DD8C File Offset: 0x0000BF8C
		public virtual IList<IXmlNode> ChildNodes
		{
			get
			{
				return new List<IXmlNode>();
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000DD93 File Offset: 0x0000BF93
		public virtual IList<IXmlNode> Attributes
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000DD96 File Offset: 0x0000BF96
		public virtual IXmlNode ParentNode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0000DD99 File Offset: 0x0000BF99
		// (set) Token: 0x0600032D RID: 813 RVA: 0x0000DD9C File Offset: 0x0000BF9C
		public virtual string Value
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000DDA3 File Offset: 0x0000BFA3
		public virtual IXmlNode AppendChild(IXmlNode newChild)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000DDAA File Offset: 0x0000BFAA
		public virtual string NamespaceURI
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040000DF RID: 223
		private readonly XObject _xmlObject;
	}
}
