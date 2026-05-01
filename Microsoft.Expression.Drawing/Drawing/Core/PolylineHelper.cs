using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x02000023 RID: 35
	internal static class PolylineHelper
	{
		// Token: 0x0600018F RID: 399 RVA: 0x00009AD8 File Offset: 0x00007CD8
		public static void PathMarch(PolylineData polyline, double startArcLength, double cornerThreshold, Func<MarchLocation, double> stopCallback)
		{
			if (polyline == null)
			{
				throw new ArgumentNullException("polyline");
			}
			int count = polyline.Count;
			if (count <= 1)
			{
				throw new ArgumentOutOfRangeException("polyline");
			}
			bool flag = false;
			double num = startArcLength;
			double num2 = 0.0;
			int num3 = 0;
			double num4 = Math.Cos(cornerThreshold * 3.141592653589793 / 180.0);
			for (;;)
			{
				double num5 = polyline.Lengths[num3];
				if (!MathHelper.IsFiniteDouble(num))
				{
					break;
				}
				if (MathHelper.IsVerySmall(num))
				{
					num = stopCallback.Invoke(MarchLocation.Create(MarchStopReason.CompleteStep, num3, num2, num5 - num2, num));
					flag = true;
				}
				else if (MathHelper.GreaterThan(num, 0.0))
				{
					if (MathHelper.LessThanOrClose(num + num2, num5))
					{
						num2 += num;
						num = stopCallback.Invoke(MarchLocation.Create(MarchStopReason.CompleteStep, num3, num2, num5 - num2, 0.0));
						flag = true;
					}
					else if (num3 < count - 2)
					{
						num3++;
						double num6 = num5 - num2;
						num -= num6;
						num2 = 0.0;
						if (flag && num4 != 1.0 && polyline.Angles[num3] > num4)
						{
							num5 = polyline.Lengths[num3];
							num = stopCallback.Invoke(MarchLocation.Create(MarchStopReason.CornerPoint, num3, num2, num5 - num2, num));
						}
					}
					else
					{
						double num7 = num5 - num2;
						num -= num7;
						num5 = polyline.Lengths[num3];
						num2 = polyline.Lengths[num3];
						num = stopCallback.Invoke(MarchLocation.Create(MarchStopReason.CompletePolyline, num3, num2, num5 - num2, num));
						flag = true;
					}
				}
				else if (MathHelper.LessThan(num, 0.0))
				{
					if (MathHelper.GreaterThanOrClose(num + num2, 0.0))
					{
						num2 += num;
						num = stopCallback.Invoke(MarchLocation.Create(MarchStopReason.CompleteStep, num3, num2, num5 - num2, 0.0));
						flag = true;
					}
					else if (num3 > 0)
					{
						num3--;
						num += num2;
						num2 = polyline.Lengths[num3];
						if (flag && num4 != 1.0 && polyline.Angles[num3 + 1] > num4)
						{
							num5 = polyline.Lengths[num3];
							num = stopCallback.Invoke(MarchLocation.Create(MarchStopReason.CornerPoint, num3, num2, num5 - num2, num));
						}
					}
					else
					{
						num += num2;
						num5 = polyline.Lengths[num3];
						num2 = 0.0;
						num = stopCallback.Invoke(MarchLocation.Create(MarchStopReason.CompletePolyline, num3, num2, num5 - num2, num));
						flag = true;
					}
				}
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00009D68 File Offset: 0x00007F68
		public static IEnumerable<PolylineData> GetWrappedPolylines(IList<PolylineData> lines, ref double startArcLength)
		{
			int num = 0;
			for (int i = 0; i < lines.Count; i++)
			{
				num = i;
				startArcLength -= lines[i].TotalLength;
				if (MathHelper.LessThanOrClose(startArcLength, 0.0))
				{
					break;
				}
			}
			if (!MathHelper.LessThanOrClose(startArcLength, 0.0))
			{
				throw new ArgumentOutOfRangeException("startArcLength");
			}
			startArcLength += lines[num].TotalLength;
			return Enumerable.Concat<PolylineData>(Enumerable.Skip<PolylineData>(lines, num), Enumerable.Take<PolylineData>(lines, num + 1));
		}
	}
}
