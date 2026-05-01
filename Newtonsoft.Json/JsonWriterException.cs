using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200009D RID: 157
	public class JsonWriterException : Exception
	{
		// Token: 0x0600077B RID: 1915 RVA: 0x0001BFE4 File Offset: 0x0001A1E4
		public JsonWriterException()
		{
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0001BFEC File Offset: 0x0001A1EC
		public JsonWriterException(string message) : base(message)
		{
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0001BFF5 File Offset: 0x0001A1F5
		public JsonWriterException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
