using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using Microsoft.Phone.Controls.Properties;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000052 RID: 82
	public class LengthConverter : TypeConverter
	{
		// Token: 0x06000311 RID: 785 RVA: 0x0000DEDC File Offset: 0x0000C0DC
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			switch (Type.GetTypeCode(sourceType))
			{
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
			case 18:
				return true;
			}
			return false;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000DF30 File Offset: 0x0000C130
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				string text = string.Format(CultureInfo.CurrentCulture, Resources.TypeConverters_ConvertFrom_CannotConvertFromType, new object[]
				{
					base.GetType().Name,
					"null"
				});
				throw new NotSupportedException(text);
			}
			string text2 = source as string;
			if (text2 != null)
			{
				if (string.Compare(text2, "Auto", 5) == 0)
				{
					return double.NaN;
				}
				string text3 = text2;
				double num = 1.0;
				foreach (KeyValuePair<string, double> keyValuePair in LengthConverter.UnitToPixelConversions)
				{
					if (text3.EndsWith(keyValuePair.Key, 4))
					{
						num = keyValuePair.Value;
						text3 = text2.Substring(0, text3.Length - keyValuePair.Key.Length);
						break;
					}
				}
				try
				{
					return num * Convert.ToDouble(text3, cultureInfo);
				}
				catch (FormatException)
				{
					string text4 = string.Format(CultureInfo.CurrentCulture, Resources.TypeConverters_Convert_CannotConvert, new object[]
					{
						base.GetType().Name,
						text2,
						typeof(double).Name
					});
					throw new FormatException(text4);
				}
			}
			return Convert.ToDouble(source, cultureInfo);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000E09C File Offset: 0x0000C29C
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return TypeConverters.CanConvertTo<double>(destinationType);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (value is double)
			{
				double num = (double)value;
				if (destinationType == typeof(string))
				{
					if (!num.IsNaN())
					{
						return Convert.ToString(num, cultureInfo);
					}
					return "Auto";
				}
			}
			return TypeConverters.ConvertTo(this, value, destinationType);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000E0F0 File Offset: 0x0000C2F0
		// Note: this type is marked as 'beforefieldinit'.
		static LengthConverter()
		{
			Dictionary<string, double> dictionary = new Dictionary<string, double>();
			dictionary.Add("px", 1.0);
			dictionary.Add("in", 96.0);
			dictionary.Add("cm", 37.79527559055118);
			dictionary.Add("pt", 1.3333333333333333);
			LengthConverter.UnitToPixelConversions = dictionary;
		}

		// Token: 0x0400016A RID: 362
		private static Dictionary<string, double> UnitToPixelConversions;
	}
}
