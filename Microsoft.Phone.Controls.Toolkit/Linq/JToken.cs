using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200001F RID: 31
	public abstract class JToken : IJEnumerable<JToken>, IEnumerable<JToken>, IEnumerable, IJsonLineInfo
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000081C7 File Offset: 0x000063C7
		public static JTokenEqualityComparer EqualityComparer
		{
			get
			{
				if (JToken._equalityComparer == null)
				{
					JToken._equalityComparer = new JTokenEqualityComparer();
				}
				return JToken._equalityComparer;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600017A RID: 378 RVA: 0x000081DF File Offset: 0x000063DF
		// (set) Token: 0x0600017B RID: 379 RVA: 0x000081E7 File Offset: 0x000063E7
		public JContainer Parent
		{
			[DebuggerStepThrough]
			get
			{
				return this._parent;
			}
			internal set
			{
				this._parent = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000081F0 File Offset: 0x000063F0
		public JToken Root
		{
			get
			{
				JContainer parent = this.Parent;
				if (parent == null)
				{
					return this;
				}
				while (parent.Parent != null)
				{
					parent = parent.Parent;
				}
				return parent;
			}
		}

		// Token: 0x0600017D RID: 381
		internal abstract JToken CloneToken();

		// Token: 0x0600017E RID: 382
		internal abstract bool DeepEquals(JToken node);

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600017F RID: 383
		public abstract JTokenType Type { get; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000180 RID: 384
		public abstract bool HasValues { get; }

		// Token: 0x06000181 RID: 385 RVA: 0x00008219 File Offset: 0x00006419
		public static bool DeepEquals(JToken t1, JToken t2)
		{
			return t1 == t2 || (t1 != null && t2 != null && t1.DeepEquals(t2));
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00008230 File Offset: 0x00006430
		// (set) Token: 0x06000183 RID: 387 RVA: 0x00008238 File Offset: 0x00006438
		public JToken Next
		{
			get
			{
				return this._next;
			}
			internal set
			{
				this._next = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00008241 File Offset: 0x00006441
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00008249 File Offset: 0x00006449
		public JToken Previous
		{
			get
			{
				return this._previous;
			}
			internal set
			{
				this._previous = value;
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008252 File Offset: 0x00006452
		internal JToken()
		{
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000825C File Offset: 0x0000645C
		public void AddAfterSelf(object content)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			int num = this._parent.IndexOfItem(this);
			this._parent.AddInternal(num + 1, content);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008298 File Offset: 0x00006498
		public void AddBeforeSelf(object content)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			int index = this._parent.IndexOfItem(this);
			this._parent.AddInternal(index, content);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000083D0 File Offset: 0x000065D0
		public IEnumerable<JToken> Ancestors()
		{
			for (JToken parent = this.Parent; parent != null; parent = parent.Parent)
			{
				yield return parent;
			}
			yield break;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000084FC File Offset: 0x000066FC
		public IEnumerable<JToken> AfterSelf()
		{
			if (this.Parent != null)
			{
				for (JToken o = this.Next; o != null; o = o.Next)
				{
					yield return o;
				}
			}
			yield break;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008624 File Offset: 0x00006824
		public IEnumerable<JToken> BeforeSelf()
		{
			for (JToken o = this.Parent.First; o != this; o = o.Next)
			{
				yield return o;
			}
			yield break;
		}

		// Token: 0x17000062 RID: 98
		public virtual JToken this[object key]
		{
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.GetType()
				}));
			}
			set
			{
				throw new InvalidOperationException("Cannot set child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.GetType()
				}));
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000086AC File Offset: 0x000068AC
		public virtual T Value<T>(object key)
		{
			JToken token = this[key];
			return token.Convert<JToken, T>();
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000086C8 File Offset: 0x000068C8
		public virtual JToken First
		{
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.GetType()
				}));
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000190 RID: 400 RVA: 0x000086FC File Offset: 0x000068FC
		public virtual JToken Last
		{
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.GetType()
				}));
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000872E File Offset: 0x0000692E
		public virtual JEnumerable<JToken> Children()
		{
			return JEnumerable<JToken>.Empty;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008735 File Offset: 0x00006935
		public JEnumerable<T> Children<T>() where T : JToken
		{
			return new JEnumerable<T>(Enumerable.OfType<T>(this.Children()));
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000874C File Offset: 0x0000694C
		public virtual IEnumerable<T> Values<T>()
		{
			throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				base.GetType()
			}));
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000877E File Offset: 0x0000697E
		public void Remove()
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			this._parent.RemoveItem(this);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000087A0 File Offset: 0x000069A0
		public void Replace(JToken value)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			this._parent.ReplaceItem(this, value);
		}

		// Token: 0x06000196 RID: 406
		public abstract void WriteTo(JsonWriter writer, params JsonConverter[] converters);

		// Token: 0x06000197 RID: 407 RVA: 0x000087C2 File Offset: 0x000069C2
		public override string ToString()
		{
			return this.ToString(Formatting.Indented, new JsonConverter[0]);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000087D4 File Offset: 0x000069D4
		public string ToString(Formatting formatting, params JsonConverter[] converters)
		{
			string result;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				this.WriteTo(new JsonTextWriter(stringWriter)
				{
					Formatting = formatting
				}, converters);
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00008828 File Offset: 0x00006A28
		private static JValue EnsureValue(JToken value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value is JProperty)
			{
				value = ((JProperty)value).Value;
			}
			return value as JValue;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00008860 File Offset: 0x00006A60
		private static string GetType(JToken token)
		{
			ValidationUtils.ArgumentNotNull(token, "token");
			if (token is JProperty)
			{
				token = ((JProperty)token).Value;
			}
			return token.Type.ToString();
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008892 File Offset: 0x00006A92
		private static bool IsNullable(JToken o)
		{
			return o.Type == JTokenType.Undefined || o.Type == JTokenType.Null;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000088AA File Offset: 0x00006AAA
		private static bool ValidateFloat(JToken o, bool nullable)
		{
			return o.Type == JTokenType.Float || o.Type == JTokenType.Integer || (nullable && JToken.IsNullable(o));
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000088CB File Offset: 0x00006ACB
		private static bool ValidateInteger(JToken o, bool nullable)
		{
			return o.Type == JTokenType.Integer || (nullable && JToken.IsNullable(o));
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000088E3 File Offset: 0x00006AE3
		private static bool ValidateDate(JToken o, bool nullable)
		{
			return o.Type == JTokenType.Date || (nullable && JToken.IsNullable(o));
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000088FC File Offset: 0x00006AFC
		private static bool ValidateBoolean(JToken o, bool nullable)
		{
			return o.Type == JTokenType.Boolean || (nullable && JToken.IsNullable(o));
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008915 File Offset: 0x00006B15
		private static bool ValidateString(JToken o)
		{
			return o.Type == JTokenType.String || o.Type == JTokenType.Comment || o.Type == JTokenType.Raw || JToken.IsNullable(o);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000893B File Offset: 0x00006B3B
		private static bool ValidateBytes(JToken o)
		{
			return o.Type == JTokenType.Bytes || JToken.IsNullable(o);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00008950 File Offset: 0x00006B50
		public static explicit operator bool(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateBoolean(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return Convert.ToBoolean(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000089A8 File Offset: 0x00006BA8
		public static explicit operator DateTimeOffset(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateDate(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return (DateTimeOffset)jvalue.Value;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000089FC File Offset: 0x00006BFC
		public static explicit operator bool?(JToken value)
		{
			if (value == null)
			{
				return default(bool?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateBoolean(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			if (jvalue.Value == null)
			{
				return default(bool?);
			}
			return new bool?(Convert.ToBoolean(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00008A78 File Offset: 0x00006C78
		public static explicit operator long(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return Convert.ToInt64(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00008AD0 File Offset: 0x00006CD0
		public static explicit operator DateTime?(JToken value)
		{
			if (value == null)
			{
				return default(DateTime?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateDate(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			if (jvalue.Value == null)
			{
				return default(DateTime?);
			}
			return new DateTime?(Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00008B4C File Offset: 0x00006D4C
		public static explicit operator DateTimeOffset?(JToken value)
		{
			if (value == null)
			{
				return default(DateTimeOffset?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateDate(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return (DateTimeOffset?)jvalue.Value;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008BAC File Offset: 0x00006DAC
		public static explicit operator decimal?(JToken value)
		{
			if (value == null)
			{
				return default(decimal?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateFloat(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			if (jvalue.Value == null)
			{
				return default(decimal?);
			}
			return new decimal?(Convert.ToDecimal(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00008C28 File Offset: 0x00006E28
		public static explicit operator double?(JToken value)
		{
			if (value == null)
			{
				return default(double?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateFloat(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return (double?)jvalue.Value;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00008C88 File Offset: 0x00006E88
		public static explicit operator int(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return Convert.ToInt32(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00008CE0 File Offset: 0x00006EE0
		public static explicit operator short(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return Convert.ToInt16(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00008D38 File Offset: 0x00006F38
		[CLSCompliant(false)]
		public static explicit operator ushort(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return Convert.ToUInt16(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00008D90 File Offset: 0x00006F90
		public static explicit operator int?(JToken value)
		{
			if (value == null)
			{
				return default(int?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			if (jvalue.Value == null)
			{
				return default(int?);
			}
			return new int?(Convert.ToInt32(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00008E0C File Offset: 0x0000700C
		public static explicit operator short?(JToken value)
		{
			if (value == null)
			{
				return default(short?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			if (jvalue.Value == null)
			{
				return default(short?);
			}
			return new short?(Convert.ToInt16(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00008E88 File Offset: 0x00007088
		[CLSCompliant(false)]
		public static explicit operator ushort?(JToken value)
		{
			if (value == null)
			{
				return default(ushort?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			if (jvalue.Value == null)
			{
				return default(ushort?);
			}
			return new ushort?((ushort)Convert.ToInt16(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00008F04 File Offset: 0x00007104
		public static explicit operator DateTime(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateDate(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00008F5C File Offset: 0x0000715C
		public static explicit operator long?(JToken value)
		{
			if (value == null)
			{
				return default(long?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			if (jvalue.Value == null)
			{
				return default(long?);
			}
			return new long?(Convert.ToInt64(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00008FD8 File Offset: 0x000071D8
		public static explicit operator float?(JToken value)
		{
			if (value == null)
			{
				return default(float?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateFloat(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			if (jvalue.Value == null)
			{
				return default(float?);
			}
			return new float?(Convert.ToSingle(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00009054 File Offset: 0x00007254
		public static explicit operator decimal(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateFloat(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return Convert.ToDecimal(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000090AC File Offset: 0x000072AC
		[CLSCompliant(false)]
		public static explicit operator uint?(JToken value)
		{
			if (value == null)
			{
				return default(uint?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			if (jvalue.Value == null)
			{
				return default(uint?);
			}
			return new uint?(Convert.ToUInt32(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00009128 File Offset: 0x00007328
		[CLSCompliant(false)]
		public static explicit operator ulong?(JToken value)
		{
			if (value == null)
			{
				return default(ulong?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			if (jvalue.Value == null)
			{
				return default(ulong?);
			}
			return new ulong?(Convert.ToUInt64(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000091A4 File Offset: 0x000073A4
		public static explicit operator double(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateFloat(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return Convert.ToDouble(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000091FC File Offset: 0x000073FC
		public static explicit operator float(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateFloat(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return Convert.ToSingle(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00009254 File Offset: 0x00007454
		public static explicit operator string(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateString(jvalue))
			{
				throw new ArgumentException("Can not convert {0} to String.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return Convert.ToString(jvalue.Value);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000092B4 File Offset: 0x000074B4
		[CLSCompliant(false)]
		public static explicit operator uint(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return Convert.ToUInt32(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000930C File Offset: 0x0000750C
		[CLSCompliant(false)]
		public static explicit operator ulong(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return Convert.ToUInt64(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009364 File Offset: 0x00007564
		public static explicit operator byte[](JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateBytes(jvalue))
			{
				throw new ArgumentException("Can not convert {0} to byte array.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(value)
				}));
			}
			return (byte[])jvalue.Value;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000093B4 File Offset: 0x000075B4
		public static implicit operator JToken(bool value)
		{
			return new JValue(value);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000093BC File Offset: 0x000075BC
		public static implicit operator JToken(DateTimeOffset value)
		{
			return new JValue(value);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000093C9 File Offset: 0x000075C9
		public static implicit operator JToken(bool? value)
		{
			return new JValue(value);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000093D6 File Offset: 0x000075D6
		public static implicit operator JToken(long value)
		{
			return new JValue(value);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000093DE File Offset: 0x000075DE
		public static implicit operator JToken(DateTime? value)
		{
			return new JValue(value);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000093EB File Offset: 0x000075EB
		public static implicit operator JToken(DateTimeOffset? value)
		{
			return new JValue(value);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000093F8 File Offset: 0x000075F8
		public static implicit operator JToken(decimal? value)
		{
			return new JValue(value);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00009405 File Offset: 0x00007605
		public static implicit operator JToken(double? value)
		{
			return new JValue(value);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00009412 File Offset: 0x00007612
		[CLSCompliant(false)]
		public static implicit operator JToken(short value)
		{
			return new JValue((long)value);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000941B File Offset: 0x0000761B
		[CLSCompliant(false)]
		public static implicit operator JToken(ushort value)
		{
			return new JValue((long)((ulong)value));
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00009424 File Offset: 0x00007624
		public static implicit operator JToken(int value)
		{
			return new JValue((long)value);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000942D File Offset: 0x0000762D
		public static implicit operator JToken(int? value)
		{
			return new JValue(value);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000943A File Offset: 0x0000763A
		public static implicit operator JToken(DateTime value)
		{
			return new JValue(value);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00009442 File Offset: 0x00007642
		public static implicit operator JToken(long? value)
		{
			return new JValue(value);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000944F File Offset: 0x0000764F
		public static implicit operator JToken(float? value)
		{
			return new JValue(value);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000945C File Offset: 0x0000765C
		public static implicit operator JToken(decimal value)
		{
			return new JValue(value);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00009469 File Offset: 0x00007669
		[CLSCompliant(false)]
		public static implicit operator JToken(short? value)
		{
			return new JValue(value);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00009476 File Offset: 0x00007676
		[CLSCompliant(false)]
		public static implicit operator JToken(ushort? value)
		{
			return new JValue(value);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00009483 File Offset: 0x00007683
		[CLSCompliant(false)]
		public static implicit operator JToken(uint? value)
		{
			return new JValue(value);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00009490 File Offset: 0x00007690
		[CLSCompliant(false)]
		public static implicit operator JToken(ulong? value)
		{
			return new JValue(value);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000949D File Offset: 0x0000769D
		public static implicit operator JToken(double value)
		{
			return new JValue(value);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000094A5 File Offset: 0x000076A5
		public static implicit operator JToken(float value)
		{
			return new JValue((double)value);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000094AE File Offset: 0x000076AE
		public static implicit operator JToken(string value)
		{
			return new JValue(value);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000094B6 File Offset: 0x000076B6
		[CLSCompliant(false)]
		public static implicit operator JToken(uint value)
		{
			return new JValue((long)((ulong)value));
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000094BF File Offset: 0x000076BF
		[CLSCompliant(false)]
		public static implicit operator JToken(ulong value)
		{
			return new JValue(value);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000094C7 File Offset: 0x000076C7
		public static implicit operator JToken(byte[] value)
		{
			return new JValue(value);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000094CF File Offset: 0x000076CF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000094D8 File Offset: 0x000076D8
		IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
		{
			return this.Children().GetEnumerator();
		}

		// Token: 0x060001D8 RID: 472
		internal abstract int GetDeepHashCode();

		// Token: 0x17000065 RID: 101
		IJEnumerable<JToken> IJEnumerable<JToken>.this[object key]
		{
			get
			{
				return this[key];
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000094FC File Offset: 0x000076FC
		public JsonReader CreateReader()
		{
			return new JTokenReader(this);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00009504 File Offset: 0x00007704
		internal static JToken FromObjectInternal(object o, JsonSerializer jsonSerializer)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
			JToken token;
			using (JTokenWriter jtokenWriter = new JTokenWriter())
			{
				jsonSerializer.Serialize(jtokenWriter, o);
				token = jtokenWriter.Token;
			}
			return token;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000955C File Offset: 0x0000775C
		public static JToken FromObject(object o)
		{
			return JToken.FromObjectInternal(o, new JsonSerializer());
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009569 File Offset: 0x00007769
		public static JToken FromObject(object o, JsonSerializer jsonSerializer)
		{
			return JToken.FromObjectInternal(o, jsonSerializer);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00009572 File Offset: 0x00007772
		public T ToObject<T>()
		{
			return this.ToObject<T>(new JsonSerializer());
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00009580 File Offset: 0x00007780
		public T ToObject<T>(JsonSerializer jsonSerializer)
		{
			ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
			T result;
			using (JTokenReader jtokenReader = new JTokenReader(this))
			{
				result = jsonSerializer.Deserialize<T>(jtokenReader);
			}
			return result;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000095C4 File Offset: 0x000077C4
		public static JToken ReadFrom(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw new Exception("Error reading JToken from JsonReader.");
			}
			if (reader.TokenType == JsonToken.StartObject)
			{
				return JObject.Load(reader);
			}
			if (reader.TokenType == JsonToken.StartArray)
			{
				return JArray.Load(reader);
			}
			if (reader.TokenType == JsonToken.PropertyName)
			{
				return JProperty.Load(reader);
			}
			if (reader.TokenType == JsonToken.StartConstructor)
			{
				return JConstructor.Load(reader);
			}
			if (!JsonReader.IsStartToken(reader.TokenType))
			{
				return new JValue(reader.Value);
			}
			throw new Exception("Error reading JToken from JsonReader. Unexpected token: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				reader.TokenType
			}));
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000967C File Offset: 0x0000787C
		public static JToken Parse(string json)
		{
			JsonReader jsonReader = new JsonTextReader(new StringReader(json));
			JToken result = JToken.Load(jsonReader);
			if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
			{
				throw new Exception("Additional text found in JSON string after parsing content.");
			}
			return result;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000096B9 File Offset: 0x000078B9
		public static JToken Load(JsonReader reader)
		{
			return JToken.ReadFrom(reader);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000096C1 File Offset: 0x000078C1
		internal void SetLineInfo(IJsonLineInfo lineInfo)
		{
			if (lineInfo == null || !lineInfo.HasLineInfo())
			{
				return;
			}
			this.SetLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000096E1 File Offset: 0x000078E1
		internal void SetLineInfo(int lineNumber, int linePosition)
		{
			this._lineNumber = new int?(lineNumber);
			this._linePosition = new int?(linePosition);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000096FB File Offset: 0x000078FB
		bool IJsonLineInfo.HasLineInfo()
		{
			return this._lineNumber != null && this._linePosition != null;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00009718 File Offset: 0x00007918
		int IJsonLineInfo.LineNumber
		{
			get
			{
				int? lineNumber = this._lineNumber;
				if (lineNumber == null)
				{
					return 0;
				}
				return lineNumber.GetValueOrDefault();
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00009740 File Offset: 0x00007940
		int IJsonLineInfo.LinePosition
		{
			get
			{
				int? linePosition = this._linePosition;
				if (linePosition == null)
				{
					return 0;
				}
				return linePosition.GetValueOrDefault();
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00009766 File Offset: 0x00007966
		public JToken SelectToken(string path)
		{
			return this.SelectToken(path, false);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00009770 File Offset: 0x00007970
		public JToken SelectToken(string path, bool errorWhenNoMatch)
		{
			JPath jpath = new JPath(path);
			return jpath.Evaluate(this, errorWhenNoMatch);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000978C File Offset: 0x0000798C
		public JToken DeepClone()
		{
			return this.CloneToken();
		}

		// Token: 0x040000AB RID: 171
		private JContainer _parent;

		// Token: 0x040000AC RID: 172
		private JToken _previous;

		// Token: 0x040000AD RID: 173
		private JToken _next;

		// Token: 0x040000AE RID: 174
		private static JTokenEqualityComparer _equalityComparer;

		// Token: 0x040000AF RID: 175
		private int? _lineNumber;

		// Token: 0x040000B0 RID: 176
		private int? _linePosition;
	}
}
