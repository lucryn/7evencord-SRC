using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Phone.Controls.Properties;

namespace System.Windows.Controls
{
	// Token: 0x0200007A RID: 122
	public class TimeTypeConverter : TypeConverter
	{
		// Token: 0x060004D5 RID: 1237 RVA: 0x00014E0A File Offset: 0x0001300A
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			return Type.GetTypeCode(sourceType) == 18;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00014E16 File Offset: 0x00013016
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return Type.GetTypeCode(destinationType) == 18 || TypeConverters.CanConvertTo<DateTime?>(destinationType);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00014E2C File Offset: 0x0001302C
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				return null;
			}
			string text = source as string;
			if (text == null)
			{
				string text2 = string.Format(CultureInfo.CurrentCulture, Resources.TypeConverters_Convert_CannotConvert, new object[]
				{
					base.GetType().Name,
					source,
					typeof(DateTime).Name
				});
				throw new InvalidCastException(text2);
			}
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			DateTime dateTime;
			foreach (string text3 in TimeTypeConverter._timeFormats)
			{
				if (DateTime.TryParseExact(text, text3, CultureInfo.InvariantCulture, 8, ref dateTime))
				{
					return DateTime.Now.Date.Add(dateTime.TimeOfDay);
				}
			}
			foreach (string text4 in TimeTypeConverter._dateFormats)
			{
				foreach (string text5 in TimeTypeConverter._timeFormats)
				{
					if (DateTime.TryParseExact(text, string.Format(CultureInfo.InvariantCulture, "{0} {1}", new object[]
					{
						text4,
						text5
					}), CultureInfo.InvariantCulture, 0, ref dateTime))
					{
						return dateTime;
					}
				}
			}
			foreach (string text6 in TimeTypeConverter._dateFormats)
			{
				if (DateTime.TryParseExact(text, text6, CultureInfo.InvariantCulture, 8, ref dateTime))
				{
					return dateTime;
				}
			}
			string text7 = string.Format(CultureInfo.CurrentCulture, Resources.TypeConverters_Convert_CannotConvert, new object[]
			{
				base.GetType().Name,
				text,
				typeof(DateTime).Name
			});
			throw new FormatException(text7);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00015004 File Offset: 0x00013204
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				if (value == null)
				{
					return string.Empty;
				}
				if (value is DateTime)
				{
					return ((DateTime)value).ToString("HH:mm:ss", new CultureInfo("en-US"));
				}
			}
			return TypeConverters.ConvertTo(this, value, destinationType);
		}

		// Token: 0x0400028B RID: 651
		private static readonly string[] _timeFormats = new string[]
		{
			"h:mm tt",
			"h:mm:ss tt",
			"HH:mm",
			"HH:mm:ss",
			"H:mm",
			"H:mm:ss"
		};

		// Token: 0x0400028C RID: 652
		private static readonly string[] _dateFormats = new string[]
		{
			"M/d/yyyy"
		};
	}
}
