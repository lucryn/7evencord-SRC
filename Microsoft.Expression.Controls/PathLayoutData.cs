using System;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000010 RID: 16
	public class PathLayoutData
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00003873 File Offset: 0x00001A73
		// (set) Token: 0x06000077 RID: 119 RVA: 0x0000387B File Offset: 0x00001A7B
		public int LayoutPathIndex { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003884 File Offset: 0x00001A84
		// (set) Token: 0x06000079 RID: 121 RVA: 0x0000388C File Offset: 0x00001A8C
		public int GlobalIndex { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003895 File Offset: 0x00001A95
		// (set) Token: 0x0600007B RID: 123 RVA: 0x0000389D File Offset: 0x00001A9D
		public int LocalIndex { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600007C RID: 124 RVA: 0x000038A6 File Offset: 0x00001AA6
		// (set) Token: 0x0600007D RID: 125 RVA: 0x000038AE File Offset: 0x00001AAE
		public double GlobalOffset { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000038B7 File Offset: 0x00001AB7
		// (set) Token: 0x0600007F RID: 127 RVA: 0x000038BF File Offset: 0x00001ABF
		public double LocalOffset { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000080 RID: 128 RVA: 0x000038C8 File Offset: 0x00001AC8
		// (set) Token: 0x06000081 RID: 129 RVA: 0x000038D0 File Offset: 0x00001AD0
		public double NormalAngle { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000038D9 File Offset: 0x00001AD9
		// (set) Token: 0x06000083 RID: 131 RVA: 0x000038E1 File Offset: 0x00001AE1
		public double OrientationAngle { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000038EA File Offset: 0x00001AEA
		// (set) Token: 0x06000085 RID: 133 RVA: 0x000038F2 File Offset: 0x00001AF2
		public bool IsArranged { get; set; }
	}
}
