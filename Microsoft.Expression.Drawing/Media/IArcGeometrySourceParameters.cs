using System;

namespace Microsoft.Expression.Media
{
	// Token: 0x02000029 RID: 41
	internal interface IArcGeometrySourceParameters : IGeometrySourceParameters
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001AA RID: 426
		double StartAngle { get; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001AB RID: 427
		double EndAngle { get; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001AC RID: 428
		double ArcThickness { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001AD RID: 429
		UnitType ArcThicknessUnit { get; }
	}
}
