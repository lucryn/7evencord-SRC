using System;

namespace System
{
	// Token: 0x0200000B RID: 11
	internal class Tuple<T1, T2>
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002A56 File Offset: 0x00000C56
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002A5E File Offset: 0x00000C5E
		public T1 Item1 { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002A67 File Offset: 0x00000C67
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002A6F File Offset: 0x00000C6F
		public T2 Item2 { get; private set; }

		// Token: 0x0600004C RID: 76 RVA: 0x00002A78 File Offset: 0x00000C78
		public Tuple(T1 item1, T2 item2)
		{
			this.Item1 = item1;
			this.Item2 = item2;
		}
	}
}
