using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005E RID: 94
	internal static class EnumUtils
	{
		// Token: 0x06000459 RID: 1113 RVA: 0x00011D18 File Offset: 0x0000FF18
		public static T Parse<T>(string enumMemberName) where T : struct
		{
			return EnumUtils.Parse<T>(enumMemberName, false);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00011D21 File Offset: 0x0000FF21
		public static T Parse<T>(string enumMemberName, bool ignoreCase) where T : struct
		{
			ValidationUtils.ArgumentTypeIsEnum(typeof(T), "T");
			return (T)((object)Enum.Parse(typeof(T), enumMemberName, ignoreCase));
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00011D68 File Offset: 0x0000FF68
		public static bool TryParse<T>(string enumMemberName, bool ignoreCase, out T value) where T : struct
		{
			ValidationUtils.ArgumentTypeIsEnum(typeof(T), "T");
			return MiscellaneousUtils.TryAction<T>(() => EnumUtils.Parse<T>(enumMemberName, ignoreCase), out value);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00011DBC File Offset: 0x0000FFBC
		public static IList<T> GetFlagsValues<T>(T value) where T : struct
		{
			Type typeFromHandle = typeof(T);
			if (!typeFromHandle.IsDefined(typeof(FlagsAttribute), false))
			{
				throw new Exception("Enum type {0} is not a set of flags.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					typeFromHandle
				}));
			}
			Type underlyingType = Enum.GetUnderlyingType(value.GetType());
			ulong num = Convert.ToUInt64(value, CultureInfo.InvariantCulture);
			EnumValues<ulong> namesAndValues = EnumUtils.GetNamesAndValues<T>();
			IList<T> list = new List<T>();
			foreach (EnumValue<ulong> enumValue in namesAndValues)
			{
				if ((num & enumValue.Value) == enumValue.Value && enumValue.Value != 0UL)
				{
					list.Add((T)((object)Convert.ChangeType(enumValue.Value, underlyingType, CultureInfo.CurrentCulture)));
				}
			}
			if (list.Count == 0 && Enumerable.SingleOrDefault<EnumValue<ulong>>(namesAndValues, (EnumValue<ulong> v) => v.Value == 0UL) != null)
			{
				list.Add(default(T));
			}
			return list;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00011EE8 File Offset: 0x000100E8
		public static EnumValues<ulong> GetNamesAndValues<T>() where T : struct
		{
			return EnumUtils.GetNamesAndValues<ulong>(typeof(T));
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00011EF9 File Offset: 0x000100F9
		public static EnumValues<TUnderlyingType> GetNamesAndValues<TEnum, TUnderlyingType>() where TEnum : struct where TUnderlyingType : struct
		{
			return EnumUtils.GetNamesAndValues<TUnderlyingType>(typeof(TEnum));
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00011F0C File Offset: 0x0001010C
		public static EnumValues<TUnderlyingType> GetNamesAndValues<TUnderlyingType>(Type enumType) where TUnderlyingType : struct
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			ValidationUtils.ArgumentTypeIsEnum(enumType, "enumType");
			IList<object> values = EnumUtils.GetValues(enumType);
			IList<string> names = EnumUtils.GetNames(enumType);
			EnumValues<TUnderlyingType> enumValues = new EnumValues<TUnderlyingType>();
			for (int i = 0; i < values.Count; i++)
			{
				try
				{
					enumValues.Add(new EnumValue<TUnderlyingType>(names[i], (TUnderlyingType)((object)Convert.ChangeType(values[i], typeof(TUnderlyingType), CultureInfo.CurrentCulture))));
				}
				catch (OverflowException ex)
				{
					throw new Exception(string.Format(CultureInfo.InvariantCulture, "Value from enum with the underlying type of {0} cannot be added to dictionary with a value type of {1}. Value was too large: {2}", new object[]
					{
						Enum.GetUnderlyingType(enumType),
						typeof(TUnderlyingType),
						Convert.ToUInt64(values[i], CultureInfo.InvariantCulture)
					}), ex);
				}
			}
			return enumValues;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00011FF8 File Offset: 0x000101F8
		public static IList<T> GetValues<T>()
		{
			return Enumerable.ToList<T>(Enumerable.Cast<T>(EnumUtils.GetValues(typeof(T))));
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001201C File Offset: 0x0001021C
		public static IList<object> GetValues(Type enumType)
		{
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
			}
			List<object> list = new List<object>();
			IEnumerable<FieldInfo> enumerable = Enumerable.Where<FieldInfo>(enumType.GetFields(), (FieldInfo field) => field.IsLiteral);
			foreach (FieldInfo fieldInfo in enumerable)
			{
				object value = fieldInfo.GetValue(enumType);
				list.Add(value);
			}
			return list;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000120C4 File Offset: 0x000102C4
		public static IList<string> GetNames<T>()
		{
			return EnumUtils.GetNames(typeof(T));
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000120E0 File Offset: 0x000102E0
		public static IList<string> GetNames(Type enumType)
		{
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
			}
			List<string> list = new List<string>();
			IEnumerable<FieldInfo> enumerable = Enumerable.Where<FieldInfo>(enumType.GetFields(), (FieldInfo field) => field.IsLiteral);
			foreach (FieldInfo fieldInfo in enumerable)
			{
				list.Add(fieldInfo.Name);
			}
			return list;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00012180 File Offset: 0x00010380
		public static TEnumType GetMaximumValue<TEnumType>(Type enumType) where TEnumType : IConvertible, IComparable<TEnumType>
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			Type underlyingType = Enum.GetUnderlyingType(enumType);
			if (!typeof(TEnumType).IsAssignableFrom(underlyingType))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "TEnumType is not assignable from the enum's underlying type of {0}.", new object[]
				{
					underlyingType.Name
				}));
			}
			ulong num = 0UL;
			IList<object> values = EnumUtils.GetValues(enumType);
			if (enumType.IsDefined(typeof(FlagsAttribute), false))
			{
				using (IEnumerator<object> enumerator = values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						TEnumType tenumType = (TEnumType)((object)obj);
						num |= tenumType.ToUInt64(CultureInfo.InvariantCulture);
					}
					goto IL_102;
				}
			}
			foreach (object obj2 in values)
			{
				TEnumType tenumType2 = (TEnumType)((object)obj2);
				ulong num2 = tenumType2.ToUInt64(CultureInfo.InvariantCulture);
				if (num.CompareTo(num2) == -1)
				{
					num = num2;
				}
			}
			IL_102:
			return (TEnumType)((object)Convert.ChangeType(num, typeof(TEnumType), CultureInfo.InvariantCulture));
		}
	}
}
