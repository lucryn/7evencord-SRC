using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200009B RID: 155
	public class JValue : JToken, IEquatable<JValue>, IFormattable, IComparable, IComparable<JValue>
	{
		// Token: 0x06000753 RID: 1875 RVA: 0x0001B68B File Offset: 0x0001988B
		internal JValue(object value, JTokenType type)
		{
			this._value = value;
			this._valueType = type;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001B6A1 File Offset: 0x000198A1
		public JValue(JValue other) : this(other.Value, other.Type)
		{
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0001B6B5 File Offset: 0x000198B5
		public JValue(long value) : this(value, JTokenType.Integer)
		{
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0001B6C4 File Offset: 0x000198C4
		[CLSCompliant(false)]
		public JValue(ulong value) : this(value, JTokenType.Integer)
		{
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0001B6D3 File Offset: 0x000198D3
		public JValue(double value) : this(value, JTokenType.Float)
		{
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0001B6E2 File Offset: 0x000198E2
		public JValue(DateTime value) : this(value, JTokenType.Date)
		{
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0001B6F2 File Offset: 0x000198F2
		public JValue(bool value) : this(value, JTokenType.Boolean)
		{
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0001B702 File Offset: 0x00019902
		public JValue(string value) : this(value, JTokenType.String)
		{
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0001B70C File Offset: 0x0001990C
		public JValue(Guid value) : this(value, JTokenType.String)
		{
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0001B71B File Offset: 0x0001991B
		public JValue(Uri value) : this(value, JTokenType.String)
		{
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0001B725 File Offset: 0x00019925
		public JValue(TimeSpan value) : this(value, JTokenType.String)
		{
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0001B734 File Offset: 0x00019934
		public JValue(object value) : this(value, JValue.GetValueType(default(JTokenType?), value))
		{
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0001B758 File Offset: 0x00019958
		internal override bool DeepEquals(JToken node)
		{
			JValue jvalue = node as JValue;
			return jvalue != null && JValue.ValuesEquals(this, jvalue);
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0001B778 File Offset: 0x00019978
		public override bool HasValues
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0001B77C File Offset: 0x0001997C
		private static int Compare(JTokenType valueType, object objA, object objB)
		{
			if (objA == null && objB == null)
			{
				return 0;
			}
			if (objA != null && objB == null)
			{
				return 1;
			}
			if (objA == null && objB != null)
			{
				return -1;
			}
			switch (valueType)
			{
			case JTokenType.Comment:
			case JTokenType.String:
			case JTokenType.Raw:
			{
				string text = Convert.ToString(objA, CultureInfo.InvariantCulture);
				string text2 = Convert.ToString(objB, CultureInfo.InvariantCulture);
				return text.CompareTo(text2);
			}
			case JTokenType.Integer:
				if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
				{
					return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
				}
				if (objA is float || objB is float || objA is double || objB is double)
				{
					return JValue.CompareFloat(objA, objB);
				}
				return Convert.ToInt64(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToInt64(objB, CultureInfo.InvariantCulture));
			case JTokenType.Float:
				return JValue.CompareFloat(objA, objB);
			case JTokenType.Boolean:
			{
				bool flag = Convert.ToBoolean(objA, CultureInfo.InvariantCulture);
				bool flag2 = Convert.ToBoolean(objB, CultureInfo.InvariantCulture);
				return flag.CompareTo(flag2);
			}
			case JTokenType.Date:
			{
				if (objA is DateTime)
				{
					DateTime dateTime = Convert.ToDateTime(objA, CultureInfo.InvariantCulture);
					DateTime dateTime2 = Convert.ToDateTime(objB, CultureInfo.InvariantCulture);
					return dateTime.CompareTo(dateTime2);
				}
				if (!(objB is DateTimeOffset))
				{
					throw new ArgumentException("Object must be of type DateTimeOffset.");
				}
				DateTimeOffset dateTimeOffset = (DateTimeOffset)objA;
				DateTimeOffset dateTimeOffset2 = (DateTimeOffset)objB;
				return dateTimeOffset.CompareTo(dateTimeOffset2);
			}
			case JTokenType.Bytes:
			{
				if (!(objB is byte[]))
				{
					throw new ArgumentException("Object must be of type byte[].");
				}
				byte[] array = objA as byte[];
				byte[] array2 = objB as byte[];
				if (array == null)
				{
					return -1;
				}
				if (array2 == null)
				{
					return 1;
				}
				return MiscellaneousUtils.ByteArrayCompare(array, array2);
			}
			case JTokenType.Guid:
			{
				if (!(objB is Guid))
				{
					throw new ArgumentException("Object must be of type Guid.");
				}
				Guid guid = (Guid)objA;
				Guid guid2 = (Guid)objB;
				return guid.CompareTo(guid2);
			}
			case JTokenType.Uri:
			{
				if (!(objB is Uri))
				{
					throw new ArgumentException("Object must be of type Uri.");
				}
				Uri uri = (Uri)objA;
				Uri uri2 = (Uri)objB;
				return Comparer<string>.Default.Compare(uri.ToString(), uri2.ToString());
			}
			case JTokenType.TimeSpan:
			{
				if (!(objB is TimeSpan))
				{
					throw new ArgumentException("Object must be of type TimeSpan.");
				}
				TimeSpan timeSpan = (TimeSpan)objA;
				TimeSpan timeSpan2 = (TimeSpan)objB;
				return timeSpan.CompareTo(timeSpan2);
			}
			}
			throw MiscellaneousUtils.CreateArgumentOutOfRangeException("valueType", valueType, "Unexpected value type: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				valueType
			}));
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0001BA14 File Offset: 0x00019C14
		private static int CompareFloat(object objA, object objB)
		{
			double d = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
			double num = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
			if (MathUtils.ApproxEquals(d, num))
			{
				return 0;
			}
			return d.CompareTo(num);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0001BA4C File Offset: 0x00019C4C
		internal override JToken CloneToken()
		{
			return new JValue(this);
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0001BA54 File Offset: 0x00019C54
		public static JValue CreateComment(string value)
		{
			return new JValue(value, JTokenType.Comment);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0001BA5D File Offset: 0x00019C5D
		public static JValue CreateString(string value)
		{
			return new JValue(value, JTokenType.String);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0001BA68 File Offset: 0x00019C68
		private static JTokenType GetValueType(JTokenType? current, object value)
		{
			if (value == null)
			{
				return JTokenType.Null;
			}
			if (value == DBNull.Value)
			{
				return JTokenType.Null;
			}
			if (value is string)
			{
				return JValue.GetStringValueType(current);
			}
			if (value is long || value is int || value is short || value is sbyte || value is ulong || value is uint || value is ushort || value is byte)
			{
				return JTokenType.Integer;
			}
			if (value is Enum)
			{
				return JTokenType.Integer;
			}
			if (value is double || value is float || value is decimal)
			{
				return JTokenType.Float;
			}
			if (value is DateTime)
			{
				return JTokenType.Date;
			}
			if (value is DateTimeOffset)
			{
				return JTokenType.Date;
			}
			if (value is byte[])
			{
				return JTokenType.Bytes;
			}
			if (value is bool)
			{
				return JTokenType.Boolean;
			}
			if (value is Guid)
			{
				return JTokenType.Guid;
			}
			if (value is Uri)
			{
				return JTokenType.Uri;
			}
			if (value is TimeSpan)
			{
				return JTokenType.TimeSpan;
			}
			throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				value.GetType()
			}));
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0001BB70 File Offset: 0x00019D70
		private static JTokenType GetStringValueType(JTokenType? current)
		{
			if (current == null)
			{
				return JTokenType.String;
			}
			JTokenType value = current.Value;
			if (value == JTokenType.Comment || value == JTokenType.String || value == JTokenType.Raw)
			{
				return current.Value;
			}
			return JTokenType.String;
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x0001BBA6 File Offset: 0x00019DA6
		public override JTokenType Type
		{
			get
			{
				return this._valueType;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x0001BBAE File Offset: 0x00019DAE
		// (set) Token: 0x0600076A RID: 1898 RVA: 0x0001BBB8 File Offset: 0x00019DB8
		public new object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				Type type = (this._value != null) ? this._value.GetType() : null;
				Type type2 = (value != null) ? value.GetType() : null;
				if (type != type2)
				{
					this._valueType = JValue.GetValueType(new JTokenType?(this._valueType), value);
				}
				this._value = value;
			}
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0001BC0C File Offset: 0x00019E0C
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			JTokenType valueType = this._valueType;
			if (valueType == JTokenType.Comment)
			{
				writer.WriteComment(this._value.ToString());
				return;
			}
			switch (valueType)
			{
			case JTokenType.Null:
				writer.WriteNull();
				return;
			case JTokenType.Undefined:
				writer.WriteUndefined();
				return;
			case JTokenType.Raw:
				writer.WriteRawValue((this._value != null) ? this._value.ToString() : null);
				return;
			}
			JsonConverter matchingConverter;
			if (this._value != null && (matchingConverter = JsonSerializer.GetMatchingConverter(converters, this._value.GetType())) != null)
			{
				matchingConverter.WriteJson(writer, this._value, new JsonSerializer());
				return;
			}
			switch (this._valueType)
			{
			case JTokenType.Integer:
				writer.WriteValue(Convert.ToInt64(this._value, CultureInfo.InvariantCulture));
				return;
			case JTokenType.Float:
				writer.WriteValue(Convert.ToDouble(this._value, CultureInfo.InvariantCulture));
				return;
			case JTokenType.String:
				writer.WriteValue((this._value != null) ? this._value.ToString() : null);
				return;
			case JTokenType.Boolean:
				writer.WriteValue(Convert.ToBoolean(this._value, CultureInfo.InvariantCulture));
				return;
			case JTokenType.Date:
				if (this._value is DateTimeOffset)
				{
					writer.WriteValue((DateTimeOffset)this._value);
					return;
				}
				writer.WriteValue(Convert.ToDateTime(this._value, CultureInfo.InvariantCulture));
				return;
			case JTokenType.Bytes:
				writer.WriteValue((byte[])this._value);
				return;
			case JTokenType.Guid:
			case JTokenType.Uri:
			case JTokenType.TimeSpan:
				writer.WriteValue((this._value != null) ? this._value.ToString() : null);
				return;
			}
			throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", this._valueType, "Unexpected token type.");
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0001BDD4 File Offset: 0x00019FD4
		internal override int GetDeepHashCode()
		{
			int num = (this._value != null) ? this._value.GetHashCode() : 0;
			return this._valueType.GetHashCode() ^ num;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0001BE0A File Offset: 0x0001A00A
		private static bool ValuesEquals(JValue v1, JValue v2)
		{
			return v1 == v2 || (v1._valueType == v2._valueType && JValue.Compare(v1._valueType, v1._value, v2._value) == 0);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0001BE3C File Offset: 0x0001A03C
		public bool Equals(JValue other)
		{
			return other != null && JValue.ValuesEquals(this, other);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0001BE4C File Offset: 0x0001A04C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			JValue jvalue = obj as JValue;
			if (jvalue != null)
			{
				return this.Equals(jvalue);
			}
			return base.Equals(obj);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0001BE77 File Offset: 0x0001A077
		public override int GetHashCode()
		{
			if (this._value == null)
			{
				return 0;
			}
			return this._value.GetHashCode();
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0001BE8E File Offset: 0x0001A08E
		public override string ToString()
		{
			if (this._value == null)
			{
				return string.Empty;
			}
			return this._value.ToString();
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0001BEA9 File Offset: 0x0001A0A9
		public string ToString(string format)
		{
			return this.ToString(format, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0001BEB7 File Offset: 0x0001A0B7
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0001BEC4 File Offset: 0x0001A0C4
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (this._value == null)
			{
				return string.Empty;
			}
			IFormattable formattable = this._value as IFormattable;
			if (formattable != null)
			{
				return formattable.ToString(format, formatProvider);
			}
			return this._value.ToString();
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0001BF04 File Offset: 0x0001A104
		int IComparable.CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			object objB = (obj is JValue) ? ((JValue)obj).Value : obj;
			return JValue.Compare(this._valueType, this._value, objB);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0001BF3F File Offset: 0x0001A13F
		public int CompareTo(JValue obj)
		{
			if (obj == null)
			{
				return 1;
			}
			return JValue.Compare(this._valueType, this._value, obj._value);
		}

		// Token: 0x0400021E RID: 542
		private JTokenType _valueType;

		// Token: 0x0400021F RID: 543
		private object _value;
	}
}
