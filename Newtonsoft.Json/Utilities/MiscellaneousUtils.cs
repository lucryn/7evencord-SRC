using System;
using System.Globalization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200003B RID: 59
	internal static class MiscellaneousUtils
	{
		// Token: 0x06000397 RID: 919 RVA: 0x0000F61C File Offset: 0x0000D81C
		public static bool ValueEquals(object objA, object objB)
		{
			if (objA == null && objB == null)
			{
				return true;
			}
			if (objA != null && objB == null)
			{
				return false;
			}
			if (objA == null && objB != null)
			{
				return false;
			}
			if (objA.GetType() == objB.GetType())
			{
				return objA.Equals(objB);
			}
			if (ConvertUtils.IsInteger(objA) && ConvertUtils.IsInteger(objB))
			{
				return Convert.ToDecimal(objA, CultureInfo.CurrentCulture).Equals(Convert.ToDecimal(objB, CultureInfo.CurrentCulture));
			}
			return (objA is double || objA is float || objA is decimal) && (objB is double || objB is float || objB is decimal) && MathUtils.ApproxEquals(Convert.ToDouble(objA, CultureInfo.CurrentCulture), Convert.ToDouble(objB, CultureInfo.CurrentCulture));
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000F6D4 File Offset: 0x0000D8D4
		public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string paramName, object actualValue, string message)
		{
			string text = message + Environment.NewLine + "Actual value was {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				actualValue
			});
			return new ArgumentOutOfRangeException(paramName, text);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000F710 File Offset: 0x0000D910
		public static bool TryAction<T>(Creator<T> creator, out T output)
		{
			ValidationUtils.ArgumentNotNull(creator, "creator");
			bool result;
			try
			{
				output = creator();
				result = true;
			}
			catch
			{
				output = default(T);
				result = false;
			}
			return result;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000F758 File Offset: 0x0000D958
		public static string ToString(object value)
		{
			if (value == null)
			{
				return "{null}";
			}
			if (!(value is string))
			{
				return value.ToString();
			}
			return "\"" + value.ToString() + "\"";
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000F788 File Offset: 0x0000D988
		public static byte[] HexToBytes(string hex)
		{
			string text = hex.Replace("-", string.Empty);
			byte[] array = new byte[text.Length / 2];
			int num = 4;
			int num2 = 0;
			string text2 = text;
			for (int i = 0; i < text2.Length; i++)
			{
				char c = text2.get_Chars(i);
				int num3 = (int)((c - '0') % ' ');
				if (num3 > 9)
				{
					num3 -= 7;
				}
				byte[] array2 = array;
				int num4 = num2;
				array2[num4] |= (byte)(num3 << num);
				num ^= 4;
				if (num != 0)
				{
					num2++;
				}
			}
			return array;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000F81A File Offset: 0x0000DA1A
		public static string BytesToHex(byte[] bytes)
		{
			return MiscellaneousUtils.BytesToHex(bytes, false);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000F824 File Offset: 0x0000DA24
		public static string BytesToHex(byte[] bytes, bool removeDashes)
		{
			string text = BitConverter.ToString(bytes);
			if (removeDashes)
			{
				text = text.Replace("-", "");
			}
			return text;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000F850 File Offset: 0x0000DA50
		public static int ByteArrayCompare(byte[] a1, byte[] a2)
		{
			int num = a1.Length.CompareTo(a2.Length);
			if (num != 0)
			{
				return num;
			}
			for (int i = 0; i < a1.Length; i++)
			{
				int num2 = a1[i].CompareTo(a2[i]);
				if (num2 != 0)
				{
					return num2;
				}
			}
			return 0;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000F898 File Offset: 0x0000DA98
		public static string GetPrefix(string qualifiedName)
		{
			string result;
			string text;
			MiscellaneousUtils.GetQualifiedNameParts(qualifiedName, out result, out text);
			return result;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000F8B0 File Offset: 0x0000DAB0
		public static string GetLocalName(string qualifiedName)
		{
			string text;
			string result;
			MiscellaneousUtils.GetQualifiedNameParts(qualifiedName, out text, out result);
			return result;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000F8C8 File Offset: 0x0000DAC8
		public static void GetQualifiedNameParts(string qualifiedName, out string prefix, out string localName)
		{
			int num = qualifiedName.IndexOf(':');
			if (num == -1 || num == 0 || qualifiedName.Length - 1 == num)
			{
				prefix = null;
				localName = qualifiedName;
				return;
			}
			prefix = qualifiedName.Substring(0, num);
			localName = qualifiedName.Substring(num + 1);
		}
	}
}
