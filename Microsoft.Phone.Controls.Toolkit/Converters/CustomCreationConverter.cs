using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000A9 RID: 169
	public abstract class CustomCreationConverter<T> : JsonConverter
	{
		// Token: 0x06000812 RID: 2066 RVA: 0x0001DFE6 File Offset: 0x0001C1E6
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001DFF4 File Offset: 0x0001C1F4
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			T t = this.Create(objectType);
			if (t == null)
			{
				throw new JsonSerializationException("No object created.");
			}
			serializer.Populate(reader, t);
			return t;
		}

		// Token: 0x06000814 RID: 2068
		public abstract T Create(Type objectType);

		// Token: 0x06000815 RID: 2069 RVA: 0x0001E03C File Offset: 0x0001C23C
		public override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x0001E04E File Offset: 0x0001C24E
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
	}
}
