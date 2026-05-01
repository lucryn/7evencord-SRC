using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000002 RID: 2
	public class ValidationEventArgs : EventArgs
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		internal ValidationEventArgs(JsonSchemaException ex)
		{
			ValidationUtils.ArgumentNotNull(ex, "ex");
			this._ex = ex;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x0000206A File Offset: 0x0000026A
		public JsonSchemaException Exception
		{
			get
			{
				return this._ex;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002072 File Offset: 0x00000272
		public string Message
		{
			get
			{
				return this._ex.Message;
			}
		}

		// Token: 0x04000001 RID: 1
		private readonly JsonSchemaException _ex;
	}
}
