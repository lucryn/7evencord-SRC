using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000AF RID: 175
	public class KeyValuePairConverter : JsonConverter
	{
		// Token: 0x0600082D RID: 2093 RVA: 0x0001E3D8 File Offset: 0x0001C5D8
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Type type = value.GetType();
			PropertyInfo property = type.GetProperty("Key");
			PropertyInfo property2 = type.GetProperty("Value");
			writer.WriteStartObject();
			writer.WritePropertyName("Key");
			serializer.Serialize(writer, ReflectionUtils.GetMemberValue(property, value));
			writer.WritePropertyName("Value");
			serializer.Serialize(writer, ReflectionUtils.GetMemberValue(property2, value));
			writer.WriteEndObject();
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001E444 File Offset: 0x0001C644
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			bool flag = ReflectionUtils.IsNullableType(objectType);
			if (reader.TokenType != JsonToken.Null)
			{
				Type type = flag ? Nullable.GetUnderlyingType(objectType) : objectType;
				IList<Type> genericArguments = type.GetGenericArguments();
				Type objectType2 = genericArguments[0];
				Type objectType3 = genericArguments[1];
				object obj = null;
				object obj2 = null;
				reader.Read();
				while (reader.TokenType == JsonToken.PropertyName)
				{
					string text;
					if ((text = reader.Value.ToString()) == null)
					{
						goto IL_AC;
					}
					if (!(text == "Key"))
					{
						if (!(text == "Value"))
						{
							goto IL_AC;
						}
						reader.Read();
						obj2 = serializer.Deserialize(reader, objectType3);
					}
					else
					{
						reader.Read();
						obj = serializer.Deserialize(reader, objectType2);
					}
					IL_B2:
					reader.Read();
					continue;
					IL_AC:
					reader.Skip();
					goto IL_B2;
				}
				return ReflectionUtils.CreateInstance(type, new object[]
				{
					obj,
					obj2
				});
			}
			if (!flag)
			{
				throw new Exception("Could not deserialize Null to KeyValuePair.");
			}
			return null;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0001E530 File Offset: 0x0001C730
		public override bool CanConvert(Type objectType)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
			return type.IsValueType && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair);
		}
	}
}
