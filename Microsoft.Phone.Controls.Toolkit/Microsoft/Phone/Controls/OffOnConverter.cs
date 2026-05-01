using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Phone.Controls.Properties;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200001C RID: 28
	public class OffOnConverter : IValueConverter
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x000048E8 File Offset: 0x00002AE8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (targetType != typeof(object))
			{
				throw new ArgumentException(Resources.UnexpectedType, "targetType");
			}
			if (!(value is bool?) && value != null)
			{
				throw new ArgumentException(Resources.UnexpectedType, "value");
			}
			if (!((bool?)value == true))
			{
				return Resources.Off;
			}
			return Resources.On;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004962 File Offset: 0x00002B62
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
