using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Microsoft.Expression.Interactivity
{
	// Token: 0x0200002A RID: 42
	internal static class TypeConverterHelper
	{
		// Token: 0x06000173 RID: 371 RVA: 0x00008CCC File Offset: 0x00006ECC
		internal static object DoConversionFrom(TypeConverter converter, object value)
		{
			object result = value;
			try
			{
				if (converter != null && value != null && converter.CanConvertFrom(value.GetType()))
				{
					result = converter.ConvertFrom(value);
				}
			}
			catch (Exception e)
			{
				if (!TypeConverterHelper.ShouldEatException(e))
				{
					throw;
				}
			}
			return result;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00008D18 File Offset: 0x00006F18
		private static bool ShouldEatException(Exception e)
		{
			bool flag = false;
			if (e.InnerException != null)
			{
				flag |= TypeConverterHelper.ShouldEatException(e.InnerException);
			}
			return flag | e is FormatException;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00008D4C File Offset: 0x00006F4C
		internal static TypeConverter GetTypeConverter(Type type)
		{
			TypeConverterAttribute typeConverterAttribute = (TypeConverterAttribute)Attribute.GetCustomAttribute(type, typeof(TypeConverterAttribute), false);
			if (typeConverterAttribute != null)
			{
				try
				{
					Type type2 = Type.GetType(typeConverterAttribute.ConverterTypeName, false);
					if (type2 != null)
					{
						return Activator.CreateInstance(type2) as TypeConverter;
					}
				}
				catch
				{
				}
			}
			return new TypeConverterHelper.ExtendedStringConverter(type);
		}

		// Token: 0x0200002B RID: 43
		private class ExtendedStringConverter : TypeConverter
		{
			// Token: 0x06000176 RID: 374 RVA: 0x00008DB0 File Offset: 0x00006FB0
			public ExtendedStringConverter(Type type)
			{
				this.type = type;
			}

			// Token: 0x06000177 RID: 375 RVA: 0x00008DBF File Offset: 0x00006FBF
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return (typeof(IConvertible).IsAssignableFrom(this.type) && typeof(IConvertible).IsAssignableFrom(destinationType)) || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x06000178 RID: 376 RVA: 0x00008DF4 File Offset: 0x00006FF4
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || sourceType == typeof(uint) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x06000179 RID: 377 RVA: 0x00008E1C File Offset: 0x0000701C
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				IConvertible convertible = value as IConvertible;
				if (convertible != null && typeof(IConvertible).IsAssignableFrom(destinationType))
				{
					return convertible.ToType(destinationType, CultureInfo.InvariantCulture);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			// Token: 0x0600017A RID: 378 RVA: 0x00008E60 File Offset: 0x00007060
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string text = value as string;
				if (text != null)
				{
					Type type = this.type;
					Type underlyingType = Nullable.GetUnderlyingType(type);
					if (underlyingType != null)
					{
						if (string.Equals(text, "null", 5))
						{
							return null;
						}
						type = underlyingType;
					}
					else if (type.IsGenericType)
					{
						return base.ConvertFrom(context, culture, value);
					}
					object obj = new object();
					object obj2 = obj;
					if (type == typeof(bool))
					{
						obj2 = bool.Parse(text);
					}
					else if (type.IsEnum)
					{
						obj2 = Enum.Parse(this.type, text, false);
					}
					else
					{
						StringBuilder stringBuilder = new StringBuilder();
						string text2 = "clr-namespace:" + type.Namespace + ";assembly=" + type.Assembly.FullName.Split(new char[]
						{
							','
						})[0];
						stringBuilder.Append("<ContentControl xmlns='http://schemas.microsoft.com/client/2007' xmlns:c='" + text2 + "'>\n");
						stringBuilder.Append("<c:" + type.Name + ">\n");
						stringBuilder.Append(text);
						stringBuilder.Append("</c:" + type.Name + ">\n");
						stringBuilder.Append("</ContentControl>");
						ContentControl contentControl = XamlReader.Load(stringBuilder.ToString()) as ContentControl;
						if (contentControl != null)
						{
							obj2 = contentControl.Content;
						}
					}
					if (obj2 != obj)
					{
						return obj2;
					}
				}
				else if (value is uint)
				{
					if (this.type == typeof(bool))
					{
						return (uint)value != 0U;
					}
					if (this.type.IsEnum)
					{
						return Enum.Parse(this.type, Enum.GetName(this.type, value), false);
					}
				}
				return base.ConvertFrom(context, culture, value);
			}

			// Token: 0x04000080 RID: 128
			private Type type;
		}
	}
}
