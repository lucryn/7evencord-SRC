using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000AA RID: 170
	public class BinaryConverter : JsonConverter
	{
		// Token: 0x06000818 RID: 2072 RVA: 0x0001E05C File Offset: 0x0001C25C
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			byte[] byteArray = this.GetByteArray(value);
			writer.WriteValue(byteArray);
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001E084 File Offset: 0x0001C284
		private byte[] GetByteArray(object value)
		{
			throw new Exception("Unexpected value type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				value.GetType()
			}));
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0001E0B8 File Offset: 0x0001C2B8
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (ReflectionUtils.IsNullableType(objectType))
			{
				Nullable.GetUnderlyingType(objectType);
			}
			if (reader.TokenType != JsonToken.Null)
			{
				if (reader.TokenType == JsonToken.StartArray)
				{
					this.ReadByteArray(reader);
				}
				else
				{
					if (reader.TokenType != JsonToken.String)
					{
						throw new Exception("Unexpected token parsing binary. Expected String or StartArray, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							reader.TokenType
						}));
					}
					string text = reader.Value.ToString();
					Convert.FromBase64String(text);
				}
				throw new Exception("Unexpected object type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					objectType
				}));
			}
			if (!ReflectionUtils.IsNullable(objectType))
			{
				throw new Exception("Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					objectType
				}));
			}
			return null;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001E188 File Offset: 0x0001C388
		private byte[] ReadByteArray(JsonReader reader)
		{
			List<byte> list = new List<byte>();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.Comment:
					continue;
				case JsonToken.Raw:
					break;
				case JsonToken.Integer:
					list.Add(Convert.ToByte(reader.Value, CultureInfo.InvariantCulture));
					continue;
				default:
					if (tokenType == JsonToken.EndArray)
					{
						return list.ToArray();
					}
					break;
				}
				throw new Exception("Unexpected token when reading bytes: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					reader.TokenType
				}));
			}
			throw new Exception("Unexpected end when reading bytes.");
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0001E21B File Offset: 0x0001C41B
		public override bool CanConvert(Type objectType)
		{
			return false;
		}
	}
}
