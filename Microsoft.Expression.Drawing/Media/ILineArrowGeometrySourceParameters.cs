using System;

namespace Microsoft.Expression.Media
{
	// Token: 0x02000008 RID: 8
	internal interface ILineArrowGeometrySourceParameters : IGeometrySourceParameters
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000069 RID: 105
		double BendAmount { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600006A RID: 106
		double ArrowSize { get; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600006B RID: 107
		ArrowType StartArrow { get; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600006C RID: 108
		ArrowType EndArrow { get; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600006D RID: 109
		CornerType StartCorner { get; }
	}
}
