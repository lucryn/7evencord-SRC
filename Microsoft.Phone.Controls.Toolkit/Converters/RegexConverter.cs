using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Bson;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000077 RID: 119
	public class RegexConverter : JsonConverter
	{
		// Token: 0x060005ED RID: 1517 RVA: 0x00017C44 File Offset: 0x00015E44
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Regex regex = (Regex)value;
			BsonWriter bsonWriter = writer as BsonWriter;
			if (bsonWriter != null)
			{
				this.WriteBson(bsonWriter, regex);
				return;
			}
			this.WriteJson(writer, regex);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00017C73 File Offset: 0x00015E73
		private bool HasFlag(RegexOptions options, RegexOptions flag)
		{
			return (options & flag) == flag;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00017C7C File Offset: 0x00015E7C
		private void WriteBson(BsonWriter writer, Regex regex)
		{
			string text = null;
			if (this.HasFlag(regex.Options, 1))
			{
				text += "i";
			}
			if (this.HasFlag(regex.Options, 2))
			{
				text += "m";
			}
			if (this.HasFlag(regex.Options, 16))
			{
				text += "s";
			}
			text += "u";
			if (this.HasFlag(regex.Options, 4))
			{
				text += "x";
			}
			writer.WriteRegex(regex.ToString(), text);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00017D14 File Offset: 0x00015F14
		private void WriteJson(JsonWriter writer, Regex regex)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("Pattern");
			writer.WriteValue(regex.ToString());
			writer.WritePropertyName("Options");
			writer.WriteValue(regex.Options);
			writer.WriteEndObject();
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00017D60 File Offset: 0x00015F60
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			BsonReader bsonReader = reader as BsonReader;
			if (bsonReader != null)
			{
				return this.ReadBson(bsonReader);
			}
			return this.ReadJson(reader);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00017D88 File Offset: 0x00015F88
		private object ReadBson(BsonReader reader)
		{
			string text = (string)reader.Value;
			int num = text.LastIndexOf("/");
			string text2 = text.Substring(1, num - 1);
			string text3 = text.Substring(num + 1);
			RegexOptions regexOptions = 0;
			string text4 = text3;
			for (int i = 0; i < text4.Length; i++)
			{
				char c = text4.get_Chars(i);
				char c2 = c;
				if (c2 <= 'm')
				{
					if (c2 != 'i')
					{
						if (c2 == 'm')
						{
							regexOptions |= 2;
						}
					}
					else
					{
						regexOptions |= 1;
					}
				}
				else if (c2 != 's')
				{
					if (c2 == 'x')
					{
						regexOptions |= 4;
					}
				}
				else
				{
					regexOptions |= 16;
				}
			}
			return new Regex(text2, regexOptions);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00017E38 File Offset: 0x00016038
		private Regex ReadJson(JsonReader reader)
		{
			reader.Read();
			reader.Read();
			string text = (string)reader.Value;
			reader.Read();
			reader.Read();
			int num = Convert.ToInt32(reader.Value, CultureInfo.InvariantCulture);
			reader.Read();
			return new Regex(text, num);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00017E8C File Offset: 0x0001608C
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Regex);
		}
	}
}
