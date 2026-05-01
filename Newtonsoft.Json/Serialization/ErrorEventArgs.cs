using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000054 RID: 84
	public class ErrorEventArgs : EventArgs
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x00011318 File Offset: 0x0000F518
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x00011320 File Offset: 0x0000F520
		public object CurrentObject { get; private set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00011329 File Offset: 0x0000F529
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x00011331 File Offset: 0x0000F531
		public ErrorContext ErrorContext { get; private set; }

		// Token: 0x06000427 RID: 1063 RVA: 0x0001133A File Offset: 0x0000F53A
		public ErrorEventArgs(object currentObject, ErrorContext errorContext)
		{
			this.CurrentObject = currentObject;
			this.ErrorContext = errorContext;
		}
	}
}
