using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000002 RID: 2
	internal abstract class DistributionStrategy
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		// (set) Token: 0x06000002 RID: 2 RVA: 0x000020D8 File Offset: 0x000002D8
		private protected PathPanel PathPanel { protected get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020E1 File Offset: 0x000002E1
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020E9 File Offset: 0x000002E9
		private protected LayoutPath LayoutPath { protected get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020F2 File Offset: 0x000002F2
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000020FA File Offset: 0x000002FA
		private protected int ChildIndex { protected get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002103 File Offset: 0x00000303
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000210B File Offset: 0x0000030B
		private protected double Span { protected get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002114 File Offset: 0x00000314
		// (set) Token: 0x0600000A RID: 10 RVA: 0x0000211C File Offset: 0x0000031C
		protected double Step { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002125 File Offset: 0x00000325
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000212D File Offset: 0x0000032D
		private protected int Capacity { protected get; private set; }

		// Token: 0x0600000D RID: 13 RVA: 0x00002136 File Offset: 0x00000336
		public virtual void Initialize()
		{
		}

		// Token: 0x0600000E RID: 14
		public abstract int ComputeAutoCapacity();

		// Token: 0x0600000F RID: 15 RVA: 0x00002138 File Offset: 0x00000338
		public virtual bool ShouldBreak(int numberArranged)
		{
			return numberArranged >= this.Capacity;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002146 File Offset: 0x00000346
		public virtual void OnPolylineBegin(PolylineData polyline)
		{
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002148 File Offset: 0x00000348
		public virtual void OnPolylineCompleted(double remaingLength)
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000214A File Offset: 0x0000034A
		public virtual void OnStepCompleted(double actualStepDistance)
		{
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000214C File Offset: 0x0000034C
		public static int Distribute(PathPanel pathPanel, int pathIndex, int childIndex)
		{
			if (pathPanel == null)
			{
				throw new ArgumentNullException("pathPanel");
			}
			if (pathIndex < 0 || pathIndex >= pathPanel.LayoutPaths.Count)
			{
				throw new ArgumentOutOfRangeException("pathIndex");
			}
			if (childIndex < 0 || childIndex >= pathPanel.Count)
			{
				throw new ArgumentOutOfRangeException("childIndex");
			}
			if (pathPanel.ValidPaths == null || pathPanel.ValidPaths.Count == 0)
			{
				throw new InvalidOperationException();
			}
			LayoutPath layoutPath = pathPanel.LayoutPaths[pathIndex];
			if (!layoutPath.IsAttached || layoutPath.Polylines.Count == 0)
			{
				return childIndex;
			}
			if (layoutPath.Distribution == Distribution.Even)
			{
				return new EvenDistributionStrategy().DistributeInternal(pathPanel, pathIndex, childIndex);
			}
			return new PaddedDistributionStrategy().DistributeInternal(pathPanel, pathIndex, childIndex);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002404 File Offset: 0x00000604
		private int DistributeInternal(PathPanel pathPanel, int pathIndex, int childIndex)
		{
			this.PathPanel = pathPanel;
			this.ChildIndex = childIndex;
			this.LayoutPath = this.PathPanel.LayoutPaths[pathIndex];
			this.Capacity = (double.IsNaN(this.LayoutPath.Capacity) ? this.ComputeAutoCapacity() : ((int)Math.Round(this.LayoutPath.Capacity)));
			this.Capacity = Math.Min(this.Capacity, pathPanel.Count - childIndex);
			if (this.Capacity <= 0)
			{
				this.LayoutPath.ActualCapacity = 0.0;
				return childIndex;
			}
			double totalLength = this.LayoutPath.TotalLength;
			double start = this.LayoutPath.Start * totalLength % totalLength;
			if (this.LayoutPath.Start != 0.0 && MathHelper.IsVerySmall(start))
			{
				if (this.LayoutPath.Start > 0.0)
				{
					start = totalLength;
				}
				else
				{
					start = 0.0;
				}
			}
			if (MathHelper.LessThan(start, 0.0))
			{
				start += totalLength;
			}
			this.Span = MathHelper.EnsureRange(this.LayoutPath.Span, new double?(-1.0), new double?(1.0)) * totalLength;
			bool isReversed = this.LayoutPath.Span < 0.0;
			IEnumerable<PolylineData> enumerable = PolylineHelper.GetWrappedPolylines(this.LayoutPath.Polylines, ref start);
			if (isReversed)
			{
				enumerable = Enumerable.Reverse<PolylineData>(enumerable);
				start = Enumerable.First<PolylineData>(enumerable).TotalLength - start;
			}
			this.Initialize();
			int numberArranged = 0;
			PolylineData line;
			foreach (PolylineData line2 in enumerable)
			{
				line = line2;
				if (this.ShouldBreak(numberArranged))
				{
					break;
				}
				bool isFirstStep = true;
				double remaining = line.TotalLength;
				if (isReversed)
				{
					start = remaining - start;
				}
				this.OnPolylineBegin(line);
				PolylineHelper.PathMarch(line, start, 0.0, delegate(MarchLocation loc)
				{
					if (loc.Reason == MarchStopReason.CompletePolyline)
					{
						if (isFirstStep)
						{
							start -= remaining;
							isFirstStep = false;
						}
						else
						{
							start = Math.Abs(this.Step) - remaining;
						}
						this.OnPolylineCompleted(remaining);
						return double.NaN;
					}
					if (loc.Reason == MarchStopReason.CornerPoint)
					{
						return loc.Remain;
					}
					double num = this.Step;
					if (isFirstStep)
					{
						if (!isReversed)
						{
							remaining -= start;
							num = start;
						}
						else
						{
							remaining = start;
							num = line.TotalLength - start;
						}
					}
					else
					{
						remaining -= Math.Abs(num);
					}
					this.OnStepCompleted(num);
					isFirstStep = false;
					if (this.ShouldBreak(numberArranged))
					{
						return double.NaN;
					}
					this.PathPanel.ArrangeChild(this.ChildIndex, pathIndex, line, loc, numberArranged);
					numberArranged++;
					this.ChildIndex++;
					return this.Step;
				});
			}
			this.LayoutPath.ActualCapacity = (double)numberArranged;
			return this.ChildIndex;
		}
	}
}
