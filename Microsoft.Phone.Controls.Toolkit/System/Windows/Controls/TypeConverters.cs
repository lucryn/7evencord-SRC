using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Phone.Controls.Properties;

namespace System.Windows.Controls
{
	// Token: 0x02000051 RID: 81
	internal static class TypeConverters
	{
		// Token: 0x0600030E RID: 782 RVA: 0x0000DE18 File Offset: 0x0000C018
		internal static bool CanConvertTo<T>(Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			return destinationType == typeof(string) || destinationType.IsAssignableFrom(typeof(T));
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000DE48 File Offset: 0x0000C048
		internal static object ConvertTo(TypeConverter converter, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value == null && !destinationType.IsValueType)
			{
				return null;
			}
			if (value != null && destinationType.IsAssignableFrom(value.GetType()))
			{
				return value;
			}
			throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Resources.TypeConverters_Convert_CannotConvert, new object[]
			{
				converter.GetType().Name,
				(value != null) ? value.GetType().FullName : "(null)",
				destinationType.GetType().Name
			}));
		}
	}
}
