using System;
using System.Runtime.InteropServices;

namespace System.Windows.Controls
{
	// Token: 0x02000026 RID: 38
	internal static class NumericExtensions
	{
		// Token: 0x06000127 RID: 295 RVA: 0x0000691C File Offset: 0x00004B1C
		public static bool IsNaN(this double value)
		{
			NumericExtensions.NanUnion nanUnion = new NumericExtensions.NanUnion
			{
				FloatingValue = value
			};
			ulong num = nanUnion.IntegerValue & 18442240474082181120UL;
			if (num != 9218868437227405312UL && num != 18442240474082181120UL)
			{
				return false;
			}
			ulong num2 = nanUnion.IntegerValue & 4503599627370495UL;
			return num2 != 0UL;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006981 File Offset: 0x00004B81
		public static bool IsGreaterThan(double left, double right)
		{
			return left > right && !NumericExtensions.AreClose(left, right);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006994 File Offset: 0x00004B94
		public static bool AreClose(double left, double right)
		{
			if (left == right)
			{
				return true;
			}
			double num = (Math.Abs(left) + Math.Abs(right) + 10.0) * 2.220446049250313E-16;
			double num2 = left - right;
			return -num < num2 && num > num2;
		}

		// Token: 0x02000027 RID: 39
		[StructLayout(2)]
		private struct NanUnion
		{
			// Token: 0x0400007D RID: 125
			[FieldOffset(0)]
			internal double FloatingValue;

			// Token: 0x0400007E RID: 126
			[FieldOffset(0)]
			internal ulong IntegerValue;
		}
	}
}
