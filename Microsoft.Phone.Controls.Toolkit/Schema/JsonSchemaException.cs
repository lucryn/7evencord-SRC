using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000098 RID: 152
	public class JsonSchemaException : Exception
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0001B3A3 File Offset: 0x000195A3
		// (set) Token: 0x06000742 RID: 1858 RVA: 0x0001B3AB File Offset: 0x000195AB
		public int LineNumber { get; private set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x0001B3B4 File Offset: 0x000195B4
		// (set) Token: 0x06000744 RID: 1860 RVA: 0x0001B3BC File Offset: 0x000195BC
		public int LinePosition { get; private set; }

		// Token: 0x06000745 RID: 1861 RVA: 0x0001B3C5 File Offset: 0x000195C5
		public JsonSchemaException()
		{
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0001B3CD File Offset: 0x000195CD
		public JsonSchemaException(string message) : base(message)
		{
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001B3D6 File Offset: 0x000195D6
		public JsonSchemaException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001B3E0 File Offset: 0x000195E0
		internal JsonSchemaException(string message, Exception innerException, int lineNumber, int linePosition) : base(message, innerException)
		{
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}
	}
}
