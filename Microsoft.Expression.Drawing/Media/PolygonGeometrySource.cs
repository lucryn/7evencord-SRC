using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Media
{
	// Token: 0x02000043 RID: 67
	internal class PolygonGeometrySource : GeometrySource<IPolygonGeometrySourceParameters>
	{
		// Token: 0x06000248 RID: 584 RVA: 0x0000E45C File Offset: 0x0000C65C
		protected override Rect ComputeLogicalBounds(Rect layoutBounds, IGeometrySourceParameters parameters)
		{
			Rect logicalBound = base.ComputeLogicalBounds(layoutBounds, parameters);
			return GeometryHelper.GetStretchBound(logicalBound, parameters.Stretch, new Size(1.0, 1.0));
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000E498 File Offset: 0x0000C698
		protected override bool UpdateCachedGeometry(IPolygonGeometrySourceParameters parameters)
		{
			bool flag = false;
			int num = Math.Max(3, Math.Min(100, (int)Math.Round(parameters.PointCount)));
			double num2 = 360.0 / (double)num;
			double num3 = Math.Max(0.0, Math.Min(1.0, parameters.InnerRadius));
			if (num3 < 1.0)
			{
				double num4 = Math.Cos(3.141592653589793 / (double)num);
				double ratio = num3 * num4;
				double num5 = num2 / 2.0;
				this.cachedPoints.EnsureListCount(num * 2, null);
				Rect bound = base.LogicalBounds.Resize(ratio);
				for (int i = 0; i < num; i++)
				{
					double num6 = num2 * (double)i;
					this.cachedPoints[i * 2] = GeometryHelper.GetArcPoint(num6, base.LogicalBounds);
					this.cachedPoints[i * 2 + 1] = GeometryHelper.GetArcPoint(num6 + num5, bound);
				}
			}
			else
			{
				this.cachedPoints.EnsureListCount(num, null);
				for (int j = 0; j < num; j++)
				{
					double degree = num2 * (double)j;
					this.cachedPoints[j] = GeometryHelper.GetArcPoint(degree, base.LogicalBounds);
				}
			}
			return flag | PathGeometryHelper.SyncPolylineGeometry(ref this.cachedGeometry, this.cachedPoints, true);
		}

		// Token: 0x040000C0 RID: 192
		private List<Point> cachedPoints = new List<Point>();
	}
}
