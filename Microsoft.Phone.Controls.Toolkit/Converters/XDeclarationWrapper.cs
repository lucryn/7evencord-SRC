using System;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200002F RID: 47
	internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x06000330 RID: 816 RVA: 0x0000DDAD File Offset: 0x0000BFAD
		public XDeclarationWrapper(XDeclaration declaration) : base(null)
		{
			this._declaration = declaration;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000DDBD File Offset: 0x0000BFBD
		public override XmlNodeType NodeType
		{
			get
			{
				return 17;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000DDC1 File Offset: 0x0000BFC1
		public string Version
		{
			get
			{
				return this._declaration.Version;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000DDCE File Offset: 0x0000BFCE
		// (set) Token: 0x06000334 RID: 820 RVA: 0x0000DDDB File Offset: 0x0000BFDB
		public string Encoding
		{
			get
			{
				return this._declaration.Encoding;
			}
			set
			{
				this._declaration.Encoding = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000DDE9 File Offset: 0x0000BFE9
		// (set) Token: 0x06000336 RID: 822 RVA: 0x0000DDF6 File Offset: 0x0000BFF6
		public string Standalone
		{
			get
			{
				return this._declaration.Standalone;
			}
			set
			{
				this._declaration.Standalone = value;
			}
		}

		// Token: 0x040000E0 RID: 224
		internal readonly XDeclaration _declaration;
	}
}
