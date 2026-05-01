using System;

namespace Microsoft.Expression.Media
{
	// Token: 0x0200002E RID: 46
	internal interface IBlockArrowGeometrySourceParameters : IGeometrySourceParameters
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001E3 RID: 483
		ArrowOrientation Orientation { get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001E4 RID: 484
		double ArrowheadAngle { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001E5 RID: 485
		double ArrowBodySize { get; }
	}
}
