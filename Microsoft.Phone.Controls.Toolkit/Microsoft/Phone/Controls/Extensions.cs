using System;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000018 RID: 24
	internal static class Extensions
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00004654 File Offset: 0x00002854
		public static bool Invert(this Matrix m, out Matrix outputMatrix)
		{
			double num = m.M11 * m.M22 - m.M12 * m.M21;
			if (num == 0.0)
			{
				outputMatrix = m;
				return false;
			}
			Matrix matrix = m;
			m.M11 = matrix.M22 / num;
			m.M12 = -1.0 * matrix.M12 / num;
			m.M21 = -1.0 * matrix.M21 / num;
			m.M22 = matrix.M11 / num;
			m.OffsetX = (matrix.OffsetY * matrix.M21 - matrix.OffsetX * matrix.M22) / num;
			m.OffsetY = (matrix.OffsetX * matrix.M12 - matrix.OffsetY * matrix.M11) / num;
			outputMatrix = m;
			return true;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004744 File Offset: 0x00002944
		public static bool Contains(this string s, string value, StringComparison comparison)
		{
			return s.IndexOf(value, comparison) >= 0;
		}
	}
}
