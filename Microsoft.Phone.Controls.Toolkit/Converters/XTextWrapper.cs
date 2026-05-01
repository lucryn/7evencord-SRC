using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000032 RID: 50
	internal class XTextWrapper : XObjectWrapper
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0000E0A1 File Offset: 0x0000C2A1
		private XText Text
		{
			get
			{
				return (XText)base.WrappedNode;
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000E0AE File Offset: 0x0000C2AE
		public XTextWrapper(XText text) : base(text)
		{
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0000E0B7 File Offset: 0x0000C2B7
		// (set) Token: 0x06000351 RID: 849 RVA: 0x0000E0C4 File Offset: 0x0000C2C4
		public override string Value
		{
			get
			{
				return this.Text.Value;
			}
			set
			{
				this.Text.Value = value;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000E0D2 File Offset: 0x0000C2D2
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Text.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Text.Parent);
			}
		}
	}
}
