using System;

namespace Microsoft.Expression.Media
{
	// Token: 0x0200003D RID: 61
	[Flags]
	public enum InvalidateGeometryReasons
	{
		// Token: 0x040000AC RID: 172
		PropertyChanged = 1,
		// Token: 0x040000AD RID: 173
		IsAnimated = 2,
		// Token: 0x040000AE RID: 174
		ChildInvalidated = 4,
		// Token: 0x040000AF RID: 175
		ParentInvalidated = 8,
		// Token: 0x040000B0 RID: 176
		TemplateChanged = 16
	}
}
