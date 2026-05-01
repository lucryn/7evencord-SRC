using System;
using System.Collections.Generic;
using System.Windows;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x02000020 RID: 32
	internal class PolylineData
	{
		// Token: 0x0600016E RID: 366 RVA: 0x0000944A File Offset: 0x0000764A
		public PolylineData(IList<Point> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			if (points.Count <= 1)
			{
				throw new ArgumentOutOfRangeException("points");
			}
			this.points = points;
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000947B File Offset: 0x0000767B
		public bool IsClosed
		{
			get
			{
				return this.points[0] == this.points.Last<Point>();
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00009499 File Offset: 0x00007699
		public int Count
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000171 RID: 369 RVA: 0x000094A8 File Offset: 0x000076A8
		public double TotalLength
		{
			get
			{
				double? num = this.totalLength;
				if (num == null)
				{
					return this.ComputeTotalLength();
				}
				return num.GetValueOrDefault();
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000172 RID: 370 RVA: 0x000094D5 File Offset: 0x000076D5
		public IList<Point> Points
		{
			get
			{
				return this.points;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000094DD File Offset: 0x000076DD
		public IList<double> Lengths
		{
			get
			{
				return this.lengths ?? this.ComputeLengths();
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000094EF File Offset: 0x000076EF
		public IList<Vector> Normals
		{
			get
			{
				return this.normals ?? this.ComputeNormals();
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00009501 File Offset: 0x00007701
		public IList<double> Angles
		{
			get
			{
				return this.angles ?? this.ComputeAngles();
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00009513 File Offset: 0x00007713
		public IList<double> AccumulatedLength
		{
			get
			{
				return this.accumulates ?? this.ComputeAccumulatedLength();
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00009528 File Offset: 0x00007728
		public Vector Difference(int index)
		{
			int num = (index + 1) % this.Count;
			return this.points[num].Subtract(this.points[index]);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00009560 File Offset: 0x00007760
		public Vector SmoothNormal(int index, double fraction, double cornerRadius)
		{
			if (cornerRadius > 0.0)
			{
				double num = this.Lengths[index];
				if (MathHelper.IsVerySmall(num))
				{
					int num2 = index - 1;
					if (num2 < 0 && this.IsClosed)
					{
						num2 = this.Count - 1;
					}
					int num3 = index + 1;
					if (this.IsClosed && num3 >= this.Count - 1)
					{
						num3 = 0;
					}
					if (num2 < 0 || num3 >= this.Count)
					{
						return this.Normals[index];
					}
					return GeometryHelper.Lerp(this.Normals[num3], this.Normals[num2], 0.5).Normalized();
				}
				else
				{
					double num4 = Math.Min(cornerRadius / num, 0.5);
					if (fraction <= num4)
					{
						int num5 = index - 1;
						if (this.IsClosed && num5 == -1)
						{
							num5 = this.Count - 1;
						}
						if (num5 >= 0)
						{
							double alpha = (num4 - fraction) / (2.0 * num4);
							return GeometryHelper.Lerp(this.Normals[index], this.Normals[num5], alpha).Normalized();
						}
					}
					else if (fraction >= 1.0 - num4)
					{
						int num6 = index + 1;
						if (this.IsClosed && num6 >= this.Count - 1)
						{
							num6 = 0;
						}
						if (num6 < this.Count)
						{
							double alpha2 = (fraction + num4 - 1.0) / (2.0 * num4);
							return GeometryHelper.Lerp(this.Normals[index], this.Normals[num6], alpha2).Normalized();
						}
					}
				}
			}
			return this.Normals[index];
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000096FC File Offset: 0x000078FC
		private IList<double> ComputeLengths()
		{
			this.lengths = new double[this.Count];
			for (int i = 0; i < this.Count; i++)
			{
				this.lengths[i] = this.Difference(i).Length;
			}
			return this.lengths;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000974C File Offset: 0x0000794C
		private IList<Vector> ComputeNormals()
		{
			this.normals = new Vector[this.points.Count];
			for (int i = 0; i < this.Count - 1; i++)
			{
				this.normals[i] = GeometryHelper.Normal(this.points[i], this.points[i + 1]);
			}
			this.normals[this.Count - 1] = this.normals[this.Count - 2];
			return this.normals;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000097DC File Offset: 0x000079DC
		private IList<double> ComputeAngles()
		{
			this.angles = new double[this.Count];
			for (int i = 1; i < this.Count - 1; i++)
			{
				this.angles[i] = -GeometryHelper.Dot(this.Normals[i - 1], this.Normals[i]);
			}
			if (this.IsClosed)
			{
				double num = -GeometryHelper.Dot(this.Normals[0], this.Normals[this.Count - 2]);
				this.angles[0] = (this.angles[this.Count - 1] = num);
			}
			else
			{
				this.angles[0] = (this.angles[this.Count - 1] = 1.0);
			}
			return this.angles;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000098C0 File Offset: 0x00007AC0
		private IList<double> ComputeAccumulatedLength()
		{
			this.accumulates = new double[this.Count];
			this.accumulates[0] = 0.0;
			for (int i = 1; i < this.Count; i++)
			{
				this.accumulates[i] = this.accumulates[i - 1] + this.Lengths[i - 1];
			}
			this.totalLength = new double?(this.accumulates.Last<double>());
			return this.accumulates;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00009949 File Offset: 0x00007B49
		private double ComputeTotalLength()
		{
			this.ComputeAccumulatedLength();
			return this.totalLength.Value;
		}

		// Token: 0x0400005B RID: 91
		private IList<Point> points;

		// Token: 0x0400005C RID: 92
		private IList<Vector> normals;

		// Token: 0x0400005D RID: 93
		private IList<double> angles;

		// Token: 0x0400005E RID: 94
		private IList<double> lengths;

		// Token: 0x0400005F RID: 95
		private IList<double> accumulates;

		// Token: 0x04000060 RID: 96
		private double? totalLength;
	}
}
