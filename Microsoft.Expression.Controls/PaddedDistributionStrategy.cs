using System;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000019 RID: 25
	internal class PaddedDistributionStrategy : DistributionStrategy
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x000043B0 File Offset: 0x000025B0
		public override int ComputeAutoCapacity()
		{
			return int.MaxValue;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000043B8 File Offset: 0x000025B8
		public override void Initialize()
		{
			base.Step = 0.0;
			this.accumulated = -1.0;
			this.preventOverlap = (base.LayoutPath.FillBehavior == FillBehavior.NoOverlap);
			this.actualSpan = this.ComputeSpan(this.preventOverlap);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004409 File Offset: 0x00002609
		public override void OnPolylineBegin(PolylineData polyline)
		{
			if (this.accumulated == -1.0 && !this.preventOverlap && polyline.IsClosed)
			{
				this.preventOverlap = true;
				this.actualSpan = this.ComputeSpan(this.preventOverlap);
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004448 File Offset: 0x00002648
		public override bool ShouldBreak(int numberArranged)
		{
			if (base.ShouldBreak(numberArranged) || this.actualSpan == 0.0)
			{
				return true;
			}
			double num = this.accumulated;
			if (this.preventOverlap)
			{
				num += base.PathPanel.GetChildRadius(base.ChildIndex);
			}
			return MathHelper.GreaterThan(num, this.actualSpan);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000044A0 File Offset: 0x000026A0
		public override void OnPolylineCompleted(double remaingLength)
		{
			this.accumulated += remaingLength;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000044B0 File Offset: 0x000026B0
		public override void OnStepCompleted(double actualStep)
		{
			if (this.accumulated != -1.0)
			{
				this.accumulated += Math.Abs(actualStep);
			}
			else
			{
				this.accumulated = 0.0;
			}
			base.Step = double.NaN;
			if (base.ChildIndex < base.PathPanel.Count - 1)
			{
				base.Step = Math.Max(base.PathPanel.GetChildRadius(base.ChildIndex) + base.LayoutPath.Padding + base.PathPanel.GetChildRadius(base.ChildIndex + 1), 0.0);
				if (base.LayoutPath.Span < 0.0)
				{
					base.Step *= -1.0;
				}
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004588 File Offset: 0x00002788
		private double ComputeSpan(bool preventOverlap)
		{
			if (preventOverlap)
			{
				return Math.Max(0.0, Math.Abs(base.Span) - base.PathPanel.GetChildRadius(base.ChildIndex) - base.LayoutPath.Padding);
			}
			return Math.Abs(base.Span);
		}

		// Token: 0x04000054 RID: 84
		private double accumulated;

		// Token: 0x04000055 RID: 85
		private double actualSpan;

		// Token: 0x04000056 RID: 86
		private bool preventOverlap;
	}
}
