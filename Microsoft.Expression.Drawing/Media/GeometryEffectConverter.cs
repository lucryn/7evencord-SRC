using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Expression.Media
{
	// Token: 0x0200003C RID: 60
	public sealed class GeometryEffectConverter : TypeConverter
	{
		// Token: 0x06000233 RID: 563 RVA: 0x0000DE06 File Offset: 0x0000C006
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return typeof(string).IsAssignableFrom(sourceType);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000DE18 File Offset: 0x0000C018
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return typeof(string).IsAssignableFrom(destinationType);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000DE2C File Offset: 0x0000C02C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			GeometryEffect geometryEffect;
			if (text != null && GeometryEffectConverter.registeredEffects.TryGetValue(text, ref geometryEffect))
			{
				return geometryEffect.CloneCurrentValue();
			}
			return null;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (typeof(string).IsAssignableFrom(destinationType))
			{
				foreach (KeyValuePair<string, GeometryEffect> keyValuePair in GeometryEffectConverter.registeredEffects)
				{
					if ((keyValuePair.Value == null) ? (value == null) : keyValuePair.Value.Equals(value as GeometryEffect))
					{
						return keyValuePair.Key;
					}
				}
			}
			return null;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000DEEC File Offset: 0x0000C0EC
		// Note: this type is marked as 'beforefieldinit'.
		static GeometryEffectConverter()
		{
			Dictionary<string, GeometryEffect> dictionary = new Dictionary<string, GeometryEffect>();
			dictionary.Add("None", GeometryEffect.DefaultGeometryEffect);
			dictionary.Add("Sketch", new SketchGeometryEffect());
			GeometryEffectConverter.registeredEffects = dictionary;
		}

		// Token: 0x040000AA RID: 170
		private static Dictionary<string, GeometryEffect> registeredEffects;
	}
}
