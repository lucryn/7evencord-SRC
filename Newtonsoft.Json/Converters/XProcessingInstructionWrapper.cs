using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000034 RID: 52
	internal class XProcessingInstructionWrapper : XObjectWrapper
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000E145 File Offset: 0x0000C345
		private XProcessingInstruction ProcessingInstruction
		{
			get
			{
				return (XProcessingInstruction)base.WrappedNode;
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000E152 File Offset: 0x0000C352
		public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction) : base(processingInstruction)
		{
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0000E15B File Offset: 0x0000C35B
		public override string LocalName
		{
			get
			{
				return this.ProcessingInstruction.Target;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000E168 File Offset: 0x0000C368
		// (set) Token: 0x0600035C RID: 860 RVA: 0x0000E175 File Offset: 0x0000C375
		public override string Value
		{
			get
			{
				return this.ProcessingInstruction.Data;
			}
			set
			{
				this.ProcessingInstruction.Data = value;
			}
		}
	}
}
