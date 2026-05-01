using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200006B RID: 107
	public class ErrorContext
	{
		// Token: 0x0600050B RID: 1291 RVA: 0x00016248 File Offset: 0x00014448
		internal ErrorContext(object originalObject, object member, Exception error)
		{
			this.OriginalObject = originalObject;
			this.Member = member;
			this.Error = error;
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x00016265 File Offset: 0x00014465
		// (set) Token: 0x0600050D RID: 1293 RVA: 0x0001626D File Offset: 0x0001446D
		public Exception Error { get; private set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x00016276 File Offset: 0x00014476
		// (set) Token: 0x0600050F RID: 1295 RVA: 0x0001627E File Offset: 0x0001447E
		public object OriginalObject { get; private set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x00016287 File Offset: 0x00014487
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x0001628F File Offset: 0x0001448F
		public object Member { get; private set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x00016298 File Offset: 0x00014498
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x000162A0 File Offset: 0x000144A0
		public bool Handled { get; set; }
	}
}
