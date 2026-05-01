using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000035 RID: 53
	internal class XAttributeWrapper : XObjectWrapper
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000E183 File Offset: 0x0000C383
		private XAttribute Attribute
		{
			get
			{
				return (XAttribute)base.WrappedNode;
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000E190 File Offset: 0x0000C390
		public XAttributeWrapper(XAttribute attribute) : base(attribute)
		{
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0000E199 File Offset: 0x0000C399
		// (set) Token: 0x06000360 RID: 864 RVA: 0x0000E1A6 File Offset: 0x0000C3A6
		public override string Value
		{
			get
			{
				return this.Attribute.Value;
			}
			set
			{
				this.Attribute.Value = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000361 RID: 865 RVA: 0x0000E1B4 File Offset: 0x0000C3B4
		public override string LocalName
		{
			get
			{
				return this.Attribute.Name.LocalName;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000E1C6 File Offset: 0x0000C3C6
		public override string NamespaceURI
		{
			get
			{
				return this.Attribute.Name.NamespaceName;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000E1D8 File Offset: 0x0000C3D8
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Attribute.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Attribute.Parent);
			}
		}
	}
}
