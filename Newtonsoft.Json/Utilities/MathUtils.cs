using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200007B RID: 123
	internal class MathUtils
	{
		// Token: 0x0600060F RID: 1551 RVA: 0x000184A2 File Offset: 0x000166A2
		public static int IntLength(int i)
		{
			if (i < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (i == 0)
			{
				return 1;
			}
			return (int)Math.Floor(Math.Log10((double)i)) + 1;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x000184C2 File Offset: 0x000166C2
		public static int HexToInt(char h)
		{
			if (h >= '0' && h <= '9')
			{
				return (int)(h - '0');
			}
			if (h >= 'a' && h <= 'f')
			{
				return (int)(h - 'a' + '\n');
			}
			if (h >= 'A' && h <= 'F')
			{
				return (int)(h - 'A' + '\n');
			}
			return -1;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x000184F8 File Offset: 0x000166F8
		public static char IntToHex(int n)
		{
			if (n <= 9)
			{
				return (char)(n + 48);
			}
			return (char)(n - 10 + 97);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00018510 File Offset: 0x00016710
		public static int GetDecimalPlaces(double value)
		{
			int num = 10;
			double num2 = Math.Pow(0.1, (double)num);
			if (value == 0.0)
			{
				return 0;
			}
			int num3 = 0;
			while (value - Math.Floor(value) > num2 && num3 < num)
			{
				value *= 10.0;
				num3++;
			}
			return num3;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00018564 File Offset: 0x00016764
		public static int? Min(int? val1, int? val2)
		{
			if (val1 == null)
			{
				return val2;
			}
			if (val2 == null)
			{
				return val1;
			}
			return new int?(Math.Min(val1.Value, val2.Value));
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00018594 File Offset: 0x00016794
		public static int? Max(int? val1, int? val2)
		{
			if (val1 == null)
			{
				return val2;
			}
			if (val2 == null)
			{
				return val1;
			}
			return new int?(Math.Max(val1.Value, val2.Value));
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x000185C4 File Offset: 0x000167C4
		public static double? Min(double? val1, double? val2)
		{
			if (val1 == null)
			{
				return val2;
			}
			if (val2 == null)
			{
				return val1;
			}
			return new double?(Math.Min(val1.Value, val2.Value));
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x000185F4 File Offset: 0x000167F4
		public static double? Max(double? val1, double? val2)
		{
			if (val1 == null)
			{
				return val2;
			}
			if (val2 == null)
			{
				return val1;
			}
			return new double?(Math.Max(val1.Value, val2.Value));
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00018624 File Offset: 0x00016824
		public static bool ApproxEquals(double d1, double d2)
		{
			return Math.Abs(d1 - d2) < Math.Abs(d1) * 1E-06;
		}
	}
}
