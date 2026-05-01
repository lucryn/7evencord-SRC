using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000049 RID: 73
	internal static class ValidationUtils
	{
		// Token: 0x060003D0 RID: 976 RVA: 0x00010314 File Offset: 0x0000E514
		public static void ArgumentNotNullOrEmpty(string value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (value.Length == 0)
			{
				throw new ArgumentException("'{0}' cannot be empty.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					parameterName
				}), parameterName);
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00010358 File Offset: 0x0000E558
		public static void ArgumentNotNullOrEmptyOrWhitespace(string value, string parameterName)
		{
			ValidationUtils.ArgumentNotNullOrEmpty(value, parameterName);
			if (StringUtils.IsWhiteSpace(value))
			{
				throw new ArgumentException("'{0}' cannot only be whitespace.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					parameterName
				}), parameterName);
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00010398 File Offset: 0x0000E598
		public static void ArgumentTypeIsEnum(Type enumType, string parameterName)
		{
			ValidationUtils.ArgumentNotNull(enumType, "enumType");
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("Type {0} is not an Enum.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					enumType
				}), parameterName);
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000103DC File Offset: 0x0000E5DC
		public static void ArgumentNotNullOrEmpty<T>(ICollection<T> collection, string parameterName)
		{
			ValidationUtils.ArgumentNotNullOrEmpty<T>(collection, parameterName, "Collection '{0}' cannot be empty.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				parameterName
			}));
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001040B File Offset: 0x0000E60B
		public static void ArgumentNotNullOrEmpty<T>(ICollection<T> collection, string parameterName, string message)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (collection.Count == 0)
			{
				throw new ArgumentException(message, parameterName);
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00010428 File Offset: 0x0000E628
		public static void ArgumentNotNullOrEmpty(ICollection collection, string parameterName)
		{
			ValidationUtils.ArgumentNotNullOrEmpty(collection, parameterName, "Collection '{0}' cannot be empty.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				parameterName
			}));
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00010457 File Offset: 0x0000E657
		public static void ArgumentNotNullOrEmpty(ICollection collection, string parameterName, string message)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (collection.Count == 0)
			{
				throw new ArgumentException(message, parameterName);
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00010473 File Offset: 0x0000E673
		public static void ArgumentNotNull(object value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001047F File Offset: 0x0000E67F
		public static void ArgumentNotNegative(int value, string parameterName)
		{
			if (value <= 0)
			{
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, value, "Argument cannot be negative.");
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00010497 File Offset: 0x0000E697
		public static void ArgumentNotNegative(int value, string parameterName, string message)
		{
			if (value <= 0)
			{
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, value, message);
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000104AB File Offset: 0x0000E6AB
		public static void ArgumentNotZero(int value, string parameterName)
		{
			if (value == 0)
			{
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, value, "Argument cannot be zero.");
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000104C2 File Offset: 0x0000E6C2
		public static void ArgumentNotZero(int value, string parameterName, string message)
		{
			if (value == 0)
			{
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, value, message);
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000104D8 File Offset: 0x0000E6D8
		public static void ArgumentIsPositive<T>(T value, string parameterName) where T : struct, IComparable<T>
		{
			if (value.CompareTo(default(T)) != 1)
			{
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, value, "Positive number required.");
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00010510 File Offset: 0x0000E710
		public static void ArgumentIsPositive(int value, string parameterName, string message)
		{
			if (value > 0)
			{
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, value, message);
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00010524 File Offset: 0x0000E724
		public static void ObjectNotDisposed(bool disposed, Type objectType)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(objectType.Name);
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00010535 File Offset: 0x0000E735
		public static void ArgumentConditionTrue(bool condition, string parameterName, string message)
		{
			if (!condition)
			{
				throw new ArgumentException(message, parameterName);
			}
		}

		// Token: 0x04000109 RID: 265
		public const string EmailAddressRegex = "^([a-zA-Z0-9_'+*$%\\^&!\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9:]{2,4})+$";

		// Token: 0x0400010A RID: 266
		public const string CurrencyRegex = "(^\\$?(?!0,?\\d)\\d{1,3}(,?\\d{3})*(\\.\\d\\d)?)$";

		// Token: 0x0400010B RID: 267
		public const string DateRegex = "^(((0?[1-9]|[12]\\d|3[01])[\\.\\-\\/](0?[13578]|1[02])[\\.\\-\\/]((1[6-9]|[2-9]\\d)?\\d{2}|\\d))|((0?[1-9]|[12]\\d|30)[\\.\\-\\/](0?[13456789]|1[012])[\\.\\-\\/]((1[6-9]|[2-9]\\d)?\\d{2}|\\d))|((0?[1-9]|1\\d|2[0-8])[\\.\\-\\/]0?2[\\.\\-\\/]((1[6-9]|[2-9]\\d)?\\d{2}|\\d))|(29[\\.\\-\\/]0?2[\\.\\-\\/]((1[6-9]|[2-9]\\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00|[048])))$";

		// Token: 0x0400010C RID: 268
		public const string NumericRegex = "\\d*";
	}
}
