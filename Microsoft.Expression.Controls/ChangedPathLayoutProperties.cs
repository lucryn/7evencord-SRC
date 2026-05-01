using System;

namespace Microsoft.Expression.Controls
{
	// Token: 0x0200000E RID: 14
	[Flags]
	public enum ChangedPathLayoutProperties
	{
		// Token: 0x04000027 RID: 39
		None = 0,
		// Token: 0x04000028 RID: 40
		LayoutPathIndex = 1,
		// Token: 0x04000029 RID: 41
		GlobalIndex = 2,
		// Token: 0x0400002A RID: 42
		LocalIndex = 4,
		// Token: 0x0400002B RID: 43
		GlobalOffset = 8,
		// Token: 0x0400002C RID: 44
		LocalOffset = 16,
		// Token: 0x0400002D RID: 45
		NormalAngle = 32,
		// Token: 0x0400002E RID: 46
		OrientationAngle = 64,
		// Token: 0x0400002F RID: 47
		IsArranged = 128
	}
}
