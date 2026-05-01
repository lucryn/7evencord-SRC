using System;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x02000024 RID: 36
	internal class RandomEngine
	{
		// Token: 0x06000191 RID: 401 RVA: 0x00009DF0 File Offset: 0x00007FF0
		public RandomEngine(long seed)
		{
			this.Initialize(seed);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00009DFF File Offset: 0x00007FFF
		public double NextGaussian(double mean, double variance)
		{
			return this.Gaussian() * variance + mean;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00009E0B File Offset: 0x0000800B
		public double NextUniform(double min, double max)
		{
			return this.Uniform() * (max - min) + min;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00009E19 File Offset: 0x00008019
		private void Initialize(long seed)
		{
			this.random = new Random((int)seed);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00009E28 File Offset: 0x00008028
		private double Uniform()
		{
			return this.random.NextDouble();
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00009E38 File Offset: 0x00008038
		private double Gaussian()
		{
			if (this.anotherSample != null)
			{
				double value = this.anotherSample.Value;
				this.anotherSample = default(double?);
				return value;
			}
			double num;
			double num2;
			double num3;
			do
			{
				num = 2.0 * this.Uniform() - 1.0;
				num2 = 2.0 * this.Uniform() - 1.0;
				num3 = num * num + num2 * num2;
			}
			while (num3 >= 1.0);
			double num4 = Math.Sqrt(-2.0 * Math.Log(num3) / num3);
			this.anotherSample = new double?(num * num4);
			return num2 * num4;
		}

		// Token: 0x0400006B RID: 107
		private Random random;

		// Token: 0x0400006C RID: 108
		private double? anotherSample;
	}
}
