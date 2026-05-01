using System;
using System.Globalization;
using System.Windows.Data;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000012 RID: 18
	public class IsArrangedToScaleConverter : IValueConverter
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00003903 File Offset: 0x00001B03
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000390A File Offset: 0x00001B0A
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return object.Equals(true, value) ? 1 : 0;
		}
	}
}
