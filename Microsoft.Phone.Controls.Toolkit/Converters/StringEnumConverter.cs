using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000A1 RID: 161
	public class StringEnumConverter : JsonConverter
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x0001CA55 File Offset: 0x0001AC55
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x0001CA5D File Offset: 0x0001AC5D
		public bool CamelCaseText { get; set; }

		// Token: 0x060007BC RID: 1980 RVA: 0x0001CA74 File Offset: 0x0001AC74
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			Enum @enum = (Enum)value;
			string text = @enum.ToString("G");
			if (char.IsNumber(text.get_Chars(0)) || text.get_Chars(0) == '-')
			{
				writer.WriteValue(value);
				return;
			}
			BidirectionalDictionary<string, string> enumNameMap = this.GetEnumNameMap(@enum.GetType());
			string text2;
			enumNameMap.TryGetByFirst(text, out text2);
			text2 = (text2 ?? text);
			if (this.CamelCaseText)
			{
				string[] array = Enumerable.ToArray<string>(Enumerable.Select<string, string>(text2.Split(new char[]
				{
					','
				}), (string item) => StringUtils.ToCamelCase(item.Trim())));
				text2 = string.Join(", ", array);
			}
			writer.WriteValue(text2);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0001CB38 File Offset: 0x0001AD38
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
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
				if (reader.TokenType == JsonToken.String)
				{
					BidirectionalDictionary<string, string> enumNameMap = this.GetEnumNameMap(type);
					string text;
					enumNameMap.TryGetBySecond(reader.Value.ToString(), out text);
					text = (text ?? reader.Value.ToString());
					return Enum.Parse(type, text, true);
				}
				if (reader.TokenType == JsonToken.Integer)
				{
					return ConvertUtils.ConvertOrCast(reader.Value, CultureInfo.InvariantCulture, type);
				}
				throw new Exception("Unexpected token when parsing enum. Expected String or Integer, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					reader.TokenType
				}));
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0001CC1C File Offset: 0x0001AE1C
		private BidirectionalDictionary<string, string> GetEnumNameMap(Type t)
		{
			BidirectionalDictionary<string, string> bidirectionalDictionary;
			if (!this._enumMemberNamesPerType.TryGetValue(t, ref bidirectionalDictionary))
			{
				lock (this._enumMemberNamesPerType)
				{
					if (this._enumMemberNamesPerType.TryGetValue(t, ref bidirectionalDictionary))
					{
						return bidirectionalDictionary;
					}
					bidirectionalDictionary = new BidirectionalDictionary<string, string>(StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase);
					foreach (FieldInfo fieldInfo in t.GetFields())
					{
						string name = fieldInfo.Name;
						string text = Enumerable.SingleOrDefault<string>(Enumerable.Select<EnumMemberAttribute, string>(Enumerable.Cast<EnumMemberAttribute>(fieldInfo.GetCustomAttributes(typeof(EnumMemberAttribute), true)), (EnumMemberAttribute a) => a.Value)) ?? fieldInfo.Name;
						string text2;
						if (bidirectionalDictionary.TryGetBySecond(text, out text2))
						{
							throw new Exception("Enum name '{0}' already exists on enum '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
							{
								text,
								t.Name
							}));
						}
						bidirectionalDictionary.Add(name, text);
					}
					this._enumMemberNamesPerType[t] = bidirectionalDictionary;
				}
				return bidirectionalDictionary;
			}
			return bidirectionalDictionary;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001CD54 File Offset: 0x0001AF54
		public override bool CanConvert(Type objectType)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
			return type.IsEnum;
		}

		// Token: 0x0400022D RID: 557
		private readonly Dictionary<Type, BidirectionalDictionary<string, string>> _enumMemberNamesPerType = new Dictionary<Type, BidirectionalDictionary<string, string>>();
	}
}
