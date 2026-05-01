using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000064 RID: 100
	public class JsonReaderException : Exception
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x000158D2 File Offset: 0x00013AD2
		// (set) Token: 0x060004D5 RID: 1237 RVA: 0x000158DA File Offset: 0x00013ADA
		public int LineNumber { get; private set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x000158E3 File Offset: 0x00013AE3
		// (set) Token: 0x060004D7 RID: 1239 RVA: 0x000158EB File Offset: 0x00013AEB
		public int LinePosition { get; private set; }

		// Token: 0x060004D8 RID: 1240 RVA: 0x000158F4 File Offset: 0x00013AF4
		public JsonReaderException()
		{
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x000158FC File Offset: 0x00013AFC
		public JsonReaderException(string message) : base(message)
		{
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00015905 File Offset: 0x00013B05
		public JsonReaderException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001590F File Offset: 0x00013B0F
		internal JsonReaderException(string message, Exception innerException, int lineNumber, int linePosition) : base(message, innerException)
		{
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}
	}
}
