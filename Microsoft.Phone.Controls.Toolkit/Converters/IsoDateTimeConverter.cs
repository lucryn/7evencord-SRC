using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000094 RID: 148
	public class IsoDateTimeConverter : DateTimeConverterBase
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0001A430 File Offset: 0x00018630
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x0001A438 File Offset: 0x00018638
		public DateTimeStyles DateTimeStyles
		{
			get
			{
				return this._dateTimeStyles;
			}
			set
			{
				this._dateTimeStyles = value;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0001A441 File Offset: 0x00018641
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x0001A452 File Offset: 0x00018652
		public string DateTimeFormat
		{
			get
			{
				return this._dateTimeFormat ?? string.Empty;
			}
			set
			{
				this._dateTimeFormat = StringUtils.NullEmptyString(value);
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0001A460 File Offset: 0x00018660
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x0001A471 File Offset: 0x00018671
		public CultureInfo Culture
		{
			get
			{
				return this._culture ?? CultureInfo.CurrentCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001A47C File Offset: 0x0001867C
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			string value2;
			if (value is DateTime)
			{
				DateTime dateTime = (DateTime)value;
				if ((this._dateTimeStyles & 16) == 16 || (this._dateTimeStyles & 64) == 64)
				{
					dateTime = dateTime.ToUniversalTime();
				}
				value2 = dateTime.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", this.Culture);
			}
			else
			{
				if (!(value is DateTimeOffset))
				{
					throw new Exception("Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						ReflectionUtils.GetObjectType(value)
					}));
				}
				DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
				if ((this._dateTimeStyles & 16) == 16 || (this._dateTimeStyles & 64) == 64)
				{
					dateTimeOffset = dateTimeOffset.ToUniversalTime();
				}
				value2 = dateTimeOffset.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", this.Culture);
			}
			writer.WriteValue(value2);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001A558 File Offset: 0x00018758
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			bool flag = ReflectionUtils.IsNullableType(objectType);
			Type type = flag ? Nullable.GetUnderlyingType(objectType) : objectType;
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullableType(objectType))
				{
					throw new Exception("Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						objectType
					}));
				}
				return null;
			}
			else
			{
				if (reader.TokenType != JsonToken.String)
				{
					throw new Exception("Unexpected token parsing date. Expected String, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						reader.TokenType
					}));
				}
				string text = reader.Value.ToString();
				if (string.IsNullOrEmpty(text) && flag)
				{
					return null;
				}
				if (type == typeof(DateTimeOffset))
				{
					if (!string.IsNullOrEmpty(this._dateTimeFormat))
					{
						return DateTimeOffset.ParseExact(text, this._dateTimeFormat, this.Culture, this._dateTimeStyles);
					}
					return DateTimeOffset.Parse(text, this.Culture, this._dateTimeStyles);
				}
				else
				{
					if (!string.IsNullOrEmpty(this._dateTimeFormat))
					{
						return DateTime.ParseExact(text, this._dateTimeFormat, this.Culture, this._dateTimeStyles);
					}
					return DateTime.Parse(text, this.Culture, this._dateTimeStyles);
				}
			}
		}

		// Token: 0x040001F0 RID: 496
		private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

		// Token: 0x040001F1 RID: 497
		private DateTimeStyles _dateTimeStyles = 128;

		// Token: 0x040001F2 RID: 498
		private string _dateTimeFormat;

		// Token: 0x040001F3 RID: 499
		private CultureInfo _culture;
	}
}
