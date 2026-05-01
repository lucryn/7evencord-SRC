using System;
using System.Globalization;
using System.Windows.Data;

namespace W7Cord.Converters
{
	// Token: 0x02000014 RID: 20
	public class UpperCaseConverter : IValueConverter
	{
		// Token: 0x060000BA RID: 186 RVA: 0x000055C8 File Offset: 0x000037C8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (value == null)
			{
				result = "";
			}
			else
			{
				result = value.ToString().ToUpper();
			}
			return result;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000055F8 File Offset: 0x000037F8
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
