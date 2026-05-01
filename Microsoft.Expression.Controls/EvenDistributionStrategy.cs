using System;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000003 RID: 3
	internal class EvenDistributionStrategy : DistributionStrategy
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000026F4 File Offset: 0x000008F4
		public override int ComputeAutoCapacity()
		{
			return (int)Math.Ceiling((double)base.PathPanel.Count / (double)base.PathPanel.ValidPaths.Count);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000271C File Offset: 0x0000091C
		public override void OnPolylineBegin(PolylineData polyline)
		{
			base.Step = double.NaN;
			if (base.Capacity > 1)
			{
				if (polyline.IsClosed || base.LayoutPath.FillBehavior == FillBehavior.NoOverlap)
				{
					base.Step = base.Span / (double)base.Capacity;
					return;
				}
				base.Step = base.Span / (double)(base.Capacity - 1);
			}
		}
	}
}
