using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000044 RID: 68
	public class JsonSerializationException : Exception
	{
		// Token: 0x060003C6 RID: 966 RVA: 0x0001023A File Offset: 0x0000E43A
		public JsonSerializationException()
		{
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00010242 File Offset: 0x0000E442
		public JsonSerializationException(string message) : base(message)
		{
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001024B File Offset: 0x0000E44B
		public JsonSerializationException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
