using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000089 RID: 137
	public class BsonObjectId
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x00018CDE File Offset: 0x00016EDE
		// (set) Token: 0x0600067D RID: 1661 RVA: 0x00018CE6 File Offset: 0x00016EE6
		public byte[] Value { get; private set; }

		// Token: 0x0600067E RID: 1662 RVA: 0x00018CEF File Offset: 0x00016EEF
		public BsonObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw new Exception("An ObjectId must be 12 bytes");
			}
			this.Value = value;
		}
	}
}
