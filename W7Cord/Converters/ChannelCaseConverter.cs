using System;
using System.Globalization;
using System.Windows.Data;

namespace W7Cord.Converters
{
	// Token: 0x02000015 RID: 21
	public class ChannelCaseConverter : IValueConverter
	{
		// Token: 0x060000BD RID: 189 RVA: 0x00005608 File Offset: 0x00003808
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (value == null)
			{
				result = "";
			}
			else
			{
				string text = value.ToString();
				if (text.StartsWith("#"))
				{
					text = text.Substring(1);
				}
				result = "#" + text.ToLower();
			}
			return result;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000565E File Offset: 0x0000385E
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
