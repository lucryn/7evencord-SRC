using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000018 RID: 24
	public sealed class LayoutPathCapacityConverter : TypeConverter
	{
		// Token: 0x060000BC RID: 188 RVA: 0x000042D4 File Offset: 0x000024D4
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(double) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000042FC File Offset: 0x000024FC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				string text2 = text.ToUpper(CultureInfo.InvariantCulture);
				if (text2 == "AUTO")
				{
					return double.NaN;
				}
				return double.Parse(text, culture);
			}
			else
			{
				if (value is double)
				{
					return value;
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000435C File Offset: 0x0000255C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType != typeof(string) || !(value is double))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			double num = (double)value;
			if (double.IsNaN(num))
			{
				return "Auto";
			}
			return num.ToString(culture);
		}
	}
}
