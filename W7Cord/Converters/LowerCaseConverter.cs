using System;
using System.Globalization;
using System.Windows.Data;

namespace W7Cord.Converters
{
	// Token: 0x02000013 RID: 19
	public class LowerCaseConverter : IValueConverter
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00005588 File Offset: 0x00003788
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (value == null)
			{
				result = "";
			}
			else
			{
				result = value.ToString().ToLower();
			}
			return result;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000055B8 File Offset: 0x000037B8
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
