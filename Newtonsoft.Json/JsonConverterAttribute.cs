using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000093 RID: 147
	[AttributeUsage(3484, AllowMultiple = false)]
	public sealed class JsonConverterAttribute : Attribute
	{
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0001A3BB File Offset: 0x000185BB
		public Type ConverterType
		{
			get
			{
				return this._converterType;
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001A3C3 File Offset: 0x000185C3
		public JsonConverterAttribute(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this._converterType = converterType;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001A3E0 File Offset: 0x000185E0
		internal static JsonConverter CreateJsonConverterInstance(Type converterType)
		{
			JsonConverter result;
			try
			{
				result = (JsonConverter)Activator.CreateInstance(converterType);
			}
			catch (Exception ex)
			{
				throw new Exception("Error creating {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					converterType
				}), ex);
			}
			return result;
		}

		// Token: 0x040001EF RID: 495
		private readonly Type _converterType;
	}
}
