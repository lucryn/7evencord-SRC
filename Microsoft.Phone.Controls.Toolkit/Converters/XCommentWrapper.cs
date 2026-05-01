using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000033 RID: 51
	internal class XCommentWrapper : XObjectWrapper
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0000E0F3 File Offset: 0x0000C2F3
		private XComment Text
		{
			get
			{
				return (XComment)base.WrappedNode;
			}
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000E100 File Offset: 0x0000C300
		public XCommentWrapper(XComment text) : base(text)
		{
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000E109 File Offset: 0x0000C309
		// (set) Token: 0x06000356 RID: 854 RVA: 0x0000E116 File Offset: 0x0000C316
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

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000E124 File Offset: 0x0000C324
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
