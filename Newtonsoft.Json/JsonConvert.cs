using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x0200009E RID: 158
	public static class JsonConvert
	{
		// Token: 0x0600077E RID: 1918 RVA: 0x0001C000 File Offset: 0x0001A200
		public static string ToString(DateTime value)
		{
			string result;
			using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
			{
				JsonConvert.WriteDateTimeString(stringWriter, value, value.GetUtcOffset(), value.Kind);
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0001C050 File Offset: 0x0001A250
		public static string ToString(DateTimeOffset value)
		{
			string result;
			using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
			{
				JsonConvert.WriteDateTimeString(stringWriter, value.UtcDateTime, value.Offset, 2);
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0001C0A0 File Offset: 0x0001A2A0
		internal static void WriteDateTimeString(TextWriter writer, DateTime value)
		{
			JsonConvert.WriteDateTimeString(writer, value, value.GetUtcOffset(), value.Kind);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0001C0B8 File Offset: 0x0001A2B8
		internal static void WriteDateTimeString(TextWriter writer, DateTime value, TimeSpan offset, DateTimeKind kind)
		{
			long num = JsonConvert.ConvertDateTimeToJavaScriptTicks(value, offset);
			writer.Write("\"\\/Date(");
			writer.Write(num);
			switch (kind)
			{
			case 0:
			case 2:
			{
				writer.Write((offset.Ticks >= 0L) ? "+" : "-");
				int num2 = Math.Abs(offset.Hours);
				if (num2 < 10)
				{
					writer.Write(0);
				}
				writer.Write(num2);
				int num3 = Math.Abs(offset.Minutes);
				if (num3 < 10)
				{
					writer.Write(0);
				}
				writer.Write(num3);
				break;
			}
			}
			writer.Write(")\\/\"");
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001C15D File Offset: 0x0001A35D
		private static long ToUniversalTicks(DateTime dateTime)
		{
			if (dateTime.Kind == 1)
			{
				return dateTime.Ticks;
			}
			return JsonConvert.ToUniversalTicks(dateTime, dateTime.GetUtcOffset());
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0001C180 File Offset: 0x0001A380
		private static long ToUniversalTicks(DateTime dateTime, TimeSpan offset)
		{
			if (dateTime.Kind == 1)
			{
				return dateTime.Ticks;
			}
			long num = dateTime.Ticks - offset.Ticks;
			if (num > 3155378975999999999L)
			{
				return 3155378975999999999L;
			}
			if (num < 0L)
			{
				return 0L;
			}
			return num;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0001C1D0 File Offset: 0x0001A3D0
		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, TimeSpan offset)
		{
			long universialTicks = JsonConvert.ToUniversalTicks(dateTime, offset);
			return JsonConvert.UniversialTicksToJavaScriptTicks(universialTicks);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0001C1EB File Offset: 0x0001A3EB
		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime)
		{
			return JsonConvert.ConvertDateTimeToJavaScriptTicks(dateTime, true);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0001C1F4 File Offset: 0x0001A3F4
		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, bool convertToUtc)
		{
			long universialTicks = convertToUtc ? JsonConvert.ToUniversalTicks(dateTime) : dateTime.Ticks;
			return JsonConvert.UniversialTicksToJavaScriptTicks(universialTicks);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0001C21C File Offset: 0x0001A41C
		private static long UniversialTicksToJavaScriptTicks(long universialTicks)
		{
			return (universialTicks - JsonConvert.InitialJavaScriptDateTicks) / 10000L;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0001C23C File Offset: 0x0001A43C
		internal static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks)
		{
			DateTime result;
			result..ctor(javaScriptTicks * 10000L + JsonConvert.InitialJavaScriptDateTicks, 1);
			return result;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0001C260 File Offset: 0x0001A460
		public static string ToString(bool value)
		{
			if (!value)
			{
				return JsonConvert.False;
			}
			return JsonConvert.True;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0001C270 File Offset: 0x0001A470
		public static string ToString(char value)
		{
			return JsonConvert.ToString(char.ToString(value));
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0001C27D File Offset: 0x0001A47D
		public static string ToString(Enum value)
		{
			return value.ToString("D");
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0001C28A File Offset: 0x0001A48A
		public static string ToString(int value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0001C299 File Offset: 0x0001A499
		public static string ToString(short value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0001C2A8 File Offset: 0x0001A4A8
		[CLSCompliant(false)]
		public static string ToString(ushort value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0001C2B7 File Offset: 0x0001A4B7
		[CLSCompliant(false)]
		public static string ToString(uint value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0001C2C6 File Offset: 0x0001A4C6
		public static string ToString(long value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0001C2D5 File Offset: 0x0001A4D5
		[CLSCompliant(false)]
		public static string ToString(ulong value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001C2E4 File Offset: 0x0001A4E4
		public static string ToString(float value)
		{
			return JsonConvert.EnsureDecimalPlace((double)value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0001C2FE File Offset: 0x0001A4FE
		public static string ToString(double value)
		{
			return JsonConvert.EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0001C317 File Offset: 0x0001A517
		private static string EnsureDecimalPlace(double value, string text)
		{
			if (double.IsNaN(value) || double.IsInfinity(value) || text.IndexOf('.') != -1 || text.IndexOf('E') != -1)
			{
				return text;
			}
			return text + ".0";
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001C34C File Offset: 0x0001A54C
		private static string EnsureDecimalPlace(string text)
		{
			if (text.IndexOf('.') != -1)
			{
				return text;
			}
			return text + ".0";
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0001C366 File Offset: 0x0001A566
		public static string ToString(byte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0001C375 File Offset: 0x0001A575
		[CLSCompliant(false)]
		public static string ToString(sbyte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001C384 File Offset: 0x0001A584
		public static string ToString(decimal value)
		{
			return JsonConvert.EnsureDecimalPlace(value.ToString(null, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0001C398 File Offset: 0x0001A598
		public static string ToString(Guid value)
		{
			return '"' + value.ToString("D", CultureInfo.InvariantCulture) + '"';
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0001C3BE File Offset: 0x0001A5BE
		public static string ToString(TimeSpan value)
		{
			return '"' + value.ToString() + '"';
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0001C3E0 File Offset: 0x0001A5E0
		public static string ToString(Uri value)
		{
			if (value == null)
			{
				return JsonConvert.Null;
			}
			return JsonConvert.ToString(value.ToString());
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001C3FC File Offset: 0x0001A5FC
		public static string ToString(string value)
		{
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0001C406 File Offset: 0x0001A606
		public static string ToString(string value, char delimter)
		{
			return JavaScriptUtils.ToEscapedJavaScriptString(value, delimter, true);
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0001C410 File Offset: 0x0001A610
		public static string ToString(object value)
		{
			if (value == null)
			{
				return JsonConvert.Null;
			}
			IConvertible convertible = value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case 2:
					return JsonConvert.Null;
				case 3:
					return JsonConvert.ToString(convertible.ToBoolean(CultureInfo.InvariantCulture));
				case 4:
					return JsonConvert.ToString(convertible.ToChar(CultureInfo.InvariantCulture));
				case 5:
					return JsonConvert.ToString(convertible.ToSByte(CultureInfo.InvariantCulture));
				case 6:
					return JsonConvert.ToString(convertible.ToByte(CultureInfo.InvariantCulture));
				case 7:
					return JsonConvert.ToString(convertible.ToInt16(CultureInfo.InvariantCulture));
				case 8:
					return JsonConvert.ToString(convertible.ToUInt16(CultureInfo.InvariantCulture));
				case 9:
					return JsonConvert.ToString(convertible.ToInt32(CultureInfo.InvariantCulture));
				case 10:
					return JsonConvert.ToString(convertible.ToUInt32(CultureInfo.InvariantCulture));
				case 11:
					return JsonConvert.ToString(convertible.ToInt64(CultureInfo.InvariantCulture));
				case 12:
					return JsonConvert.ToString(convertible.ToUInt64(CultureInfo.InvariantCulture));
				case 13:
					return JsonConvert.ToString(convertible.ToSingle(CultureInfo.InvariantCulture));
				case 14:
					return JsonConvert.ToString(convertible.ToDouble(CultureInfo.InvariantCulture));
				case 15:
					return JsonConvert.ToString(convertible.ToDecimal(CultureInfo.InvariantCulture));
				case 16:
					return JsonConvert.ToString(convertible.ToDateTime(CultureInfo.InvariantCulture));
				case 18:
					return JsonConvert.ToString(convertible.ToString(CultureInfo.InvariantCulture));
				}
			}
			else
			{
				if (value is DateTimeOffset)
				{
					return JsonConvert.ToString((DateTimeOffset)value);
				}
				if (value is Guid)
				{
					return JsonConvert.ToString((Guid)value);
				}
				if (value is Uri)
				{
					return JsonConvert.ToString((Uri)value);
				}
				if (value is TimeSpan)
				{
					return JsonConvert.ToString((TimeSpan)value);
				}
			}
			throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				value.GetType()
			}));
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0001C608 File Offset: 0x0001A808
		private static bool IsJsonPrimitiveTypeCode(TypeCode typeCode)
		{
			switch (typeCode)
			{
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
			case 16:
			case 18:
				return true;
			}
			return false;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0001C668 File Offset: 0x0001A868
		internal static bool IsJsonPrimitiveType(Type type)
		{
			if (ReflectionUtils.IsNullableType(type))
			{
				type = Nullable.GetUnderlyingType(type);
			}
			return type == typeof(DateTimeOffset) || type == typeof(byte[]) || type == typeof(Uri) || type == typeof(TimeSpan) || type == typeof(Guid) || JsonConvert.IsJsonPrimitiveTypeCode(Type.GetTypeCode(type));
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0001C6DC File Offset: 0x0001A8DC
		internal static bool IsJsonPrimitive(object value)
		{
			if (value == null)
			{
				return true;
			}
			IConvertible convertible = value as IConvertible;
			if (convertible != null)
			{
				return JsonConvert.IsJsonPrimitiveTypeCode(convertible.GetTypeCode());
			}
			return value is DateTimeOffset || value is byte[] || value is Uri || value is TimeSpan || value is Guid;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0001C737 File Offset: 0x0001A937
		public static string SerializeObject(object value)
		{
			return JsonConvert.SerializeObject(value, Formatting.None, null);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001C741 File Offset: 0x0001A941
		public static string SerializeObject(object value, Formatting formatting)
		{
			return JsonConvert.SerializeObject(value, formatting, null);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0001C74B File Offset: 0x0001A94B
		public static string SerializeObject(object value, params JsonConverter[] converters)
		{
			return JsonConvert.SerializeObject(value, Formatting.None, converters);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0001C758 File Offset: 0x0001A958
		public static string SerializeObject(object value, Formatting formatting, params JsonConverter[] converters)
		{
			JsonSerializerSettings settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings
			{
				Converters = converters
			} : null;
			return JsonConvert.SerializeObject(value, formatting, settings);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0001C788 File Offset: 0x0001A988
		public static string SerializeObject(object value, Formatting formatting, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
			StringBuilder stringBuilder = new StringBuilder(128);
			StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
			{
				jsonTextWriter.Formatting = formatting;
				jsonSerializer.Serialize(jsonTextWriter, value);
			}
			return stringWriter.ToString();
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0001C7EC File Offset: 0x0001A9EC
		public static object DeserializeObject(string value)
		{
			return JsonConvert.DeserializeObject(value, null, null);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0001C7F6 File Offset: 0x0001A9F6
		public static object DeserializeObject(string value, JsonSerializerSettings settings)
		{
			return JsonConvert.DeserializeObject(value, null, settings);
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0001C800 File Offset: 0x0001AA00
		public static object DeserializeObject(string value, Type type)
		{
			return JsonConvert.DeserializeObject(value, type, null);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0001C80A File Offset: 0x0001AA0A
		public static T DeserializeObject<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value, null);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0001C813 File Offset: 0x0001AA13
		public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject)
		{
			return JsonConvert.DeserializeObject<T>(value);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0001C81B File Offset: 0x0001AA1B
		public static T DeserializeObject<T>(string value, params JsonConverter[] converters)
		{
			return (T)((object)JsonConvert.DeserializeObject(value, typeof(T), converters));
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0001C833 File Offset: 0x0001AA33
		public static T DeserializeObject<T>(string value, JsonSerializerSettings settings)
		{
			return (T)((object)JsonConvert.DeserializeObject(value, typeof(T), settings));
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0001C84C File Offset: 0x0001AA4C
		public static object DeserializeObject(string value, Type type, params JsonConverter[] converters)
		{
			JsonSerializerSettings settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings
			{
				Converters = converters
			} : null;
			return JsonConvert.DeserializeObject(value, type, settings);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0001C87C File Offset: 0x0001AA7C
		public static object DeserializeObject(string value, Type type, JsonSerializerSettings settings)
		{
			StringReader reader = new StringReader(value);
			JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
			object result;
			using (JsonReader jsonReader = new JsonTextReader(reader))
			{
				result = jsonSerializer.Deserialize(jsonReader, type);
				if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
				{
					throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
				}
			}
			return result;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0001C8E0 File Offset: 0x0001AAE0
		public static void PopulateObject(string value, object target)
		{
			JsonConvert.PopulateObject(value, target, null);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0001C8EC File Offset: 0x0001AAEC
		public static void PopulateObject(string value, object target, JsonSerializerSettings settings)
		{
			StringReader reader = new StringReader(value);
			JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
			using (JsonReader jsonReader = new JsonTextReader(reader))
			{
				jsonSerializer.Populate(jsonReader, target);
				if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
				{
					throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
				}
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0001C950 File Offset: 0x0001AB50
		public static string SerializeXNode(XObject node)
		{
			return JsonConvert.SerializeXNode(node, Formatting.None);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0001C959 File Offset: 0x0001AB59
		public static string SerializeXNode(XObject node, Formatting formatting)
		{
			return JsonConvert.SerializeXNode(node, formatting, false);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0001C964 File Offset: 0x0001AB64
		public static string SerializeXNode(XObject node, Formatting formatting, bool omitRootObject)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter
			{
				OmitRootObject = omitRootObject
			};
			return JsonConvert.SerializeObject(node, formatting, new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001C993 File Offset: 0x0001AB93
		public static XDocument DeserializeXNode(string value)
		{
			return JsonConvert.DeserializeXNode(value, null);
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0001C99C File Offset: 0x0001AB9C
		public static XDocument DeserializeXNode(string value, string deserializeRootElementName)
		{
			return JsonConvert.DeserializeXNode(value, deserializeRootElementName, false);
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001C9A8 File Offset: 0x0001ABA8
		public static XDocument DeserializeXNode(string value, string deserializeRootElementName, bool writeArrayAttribute)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.DeserializeRootElementName = deserializeRootElementName;
			xmlNodeConverter.WriteArrayAttribute = writeArrayAttribute;
			return (XDocument)JsonConvert.DeserializeObject(value, typeof(XDocument), new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x04000220 RID: 544
		public static readonly string True = "true";

		// Token: 0x04000221 RID: 545
		public static readonly string False = "false";

		// Token: 0x04000222 RID: 546
		public static readonly string Null = "null";

		// Token: 0x04000223 RID: 547
		public static readonly string Undefined = "undefined";

		// Token: 0x04000224 RID: 548
		public static readonly string PositiveInfinity = "Infinity";

		// Token: 0x04000225 RID: 549
		public static readonly string NegativeInfinity = "-Infinity";

		// Token: 0x04000226 RID: 550
		public static readonly string NaN = "NaN";

		// Token: 0x04000227 RID: 551
		internal static readonly long InitialJavaScriptDateTicks = 621355968000000000L;
	}
}
