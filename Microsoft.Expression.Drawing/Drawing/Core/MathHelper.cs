using System;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x0200001D RID: 29
	internal static class MathHelper
	{
		// Token: 0x06000155 RID: 341 RVA: 0x00008CAB File Offset: 0x00006EAB
		public static bool IsVerySmall(double value)
		{
			return Math.Abs(value) < 1E-06;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00008CBE File Offset: 0x00006EBE
		public static bool AreClose(double value1, double value2)
		{
			return value1 == value2 || MathHelper.IsVerySmall(value1 - value2);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00008CCE File Offset: 0x00006ECE
		public static bool GreaterThan(double value1, double value2)
		{
			return value1 > value2 && !MathHelper.AreClose(value1, value2);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00008CE0 File Offset: 0x00006EE0
		public static bool GreaterThanOrClose(double value1, double value2)
		{
			return value1 > value2 || MathHelper.AreClose(value1, value2);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00008CEF File Offset: 0x00006EEF
		public static bool LessThan(double value1, double value2)
		{
			return value1 < value2 && !MathHelper.AreClose(value1, value2);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00008D01 File Offset: 0x00006F01
		public static bool LessThanOrClose(double value1, double value2)
		{
			return value1 < value2 || MathHelper.AreClose(value1, value2);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00008D10 File Offset: 0x00006F10
		public static double SafeDivide(double lhs, double rhs, double fallback)
		{
			if (!MathHelper.IsVerySmall(rhs))
			{
				return lhs / rhs;
			}
			return fallback;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00008D1F File Offset: 0x00006F1F
		public static double Lerp(double x, double y, double alpha)
		{
			return x * (1.0 - alpha) + y * alpha;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00008D32 File Offset: 0x00006F32
		public static double EnsureRange(double value, double? min, double? max)
		{
			if (min != null && value < min.Value)
			{
				return min.Value;
			}
			if (max != null && value > max.Value)
			{
				return max.Value;
			}
			return value;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00008D6B File Offset: 0x00006F6B
		public static double Hypotenuse(double x, double y)
		{
			return Math.Sqrt(x * x + y * y);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00008D79 File Offset: 0x00006F79
		public static double DoubleFromMantissaAndExponent(double x, int exp)
		{
			return x * Math.Pow(2.0, (double)exp);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00008D8D File Offset: 0x00006F8D
		public static bool IsFiniteDouble(double x)
		{
			return !double.IsInfinity(x) && !double.IsNaN(x);
		}

		// Token: 0x04000052 RID: 82
		public const double Epsilon = 1E-06;

		// Token: 0x04000053 RID: 83
		public const double TwoPI = 6.283185307179586;

		// Token: 0x04000054 RID: 84
		public const double PentagramInnerRadius = 0.47211;
	}
}
