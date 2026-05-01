using System;
using System.Collections.Generic;
using System.Windows;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x02000022 RID: 34
	internal class MarchLocation
	{
		// Token: 0x0600017E RID: 382 RVA: 0x00009960 File Offset: 0x00007B60
		public static MarchLocation Create(MarchStopReason reason, int index, double before, double after, double remain)
		{
			double num = before + after;
			return new MarchLocation
			{
				Reason = reason,
				Index = index,
				Remain = remain,
				Before = MathHelper.EnsureRange(before, new double?(0.0), new double?(num)),
				After = MathHelper.EnsureRange(after, new double?(0.0), new double?(num)),
				Ratio = MathHelper.EnsureRange(MathHelper.SafeDivide(before, num, 0.0), new double?(0.0), new double?(1.0))
			};
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00009A05 File Offset: 0x00007C05
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00009A0D File Offset: 0x00007C0D
		public MarchStopReason Reason { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00009A16 File Offset: 0x00007C16
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00009A1E File Offset: 0x00007C1E
		public int Index { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00009A27 File Offset: 0x00007C27
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00009A2F File Offset: 0x00007C2F
		public double Ratio { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00009A38 File Offset: 0x00007C38
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00009A40 File Offset: 0x00007C40
		public double Before { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00009A49 File Offset: 0x00007C49
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00009A51 File Offset: 0x00007C51
		public double After { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00009A5A File Offset: 0x00007C5A
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00009A62 File Offset: 0x00007C62
		public double Remain { get; private set; }

		// Token: 0x0600018B RID: 395 RVA: 0x00009A6B File Offset: 0x00007C6B
		public Point GetPoint(IList<Point> points)
		{
			return GeometryHelper.Lerp(points[this.Index], points[this.Index + 1], this.Ratio);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00009A92 File Offset: 0x00007C92
		public Vector GetNormal(PolylineData polyline, double cornerRadius = 0.0)
		{
			return polyline.SmoothNormal(this.Index, this.Ratio, cornerRadius);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00009AA7 File Offset: 0x00007CA7
		public double GetArcLength(IList<double> accumulatedLengths)
		{
			return MathHelper.Lerp(accumulatedLengths[this.Index], accumulatedLengths[this.Index + 1], this.Ratio);
		}
	}
}
