using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000024 RID: 36
	public abstract class JsonWriter : IDisposable
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000AF70 File Offset: 0x00009170
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0000AF78 File Offset: 0x00009178
		public bool CloseOutput { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000AF81 File Offset: 0x00009181
		protected internal int Top
		{
			get
			{
				return this._top;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000AF8C File Offset: 0x0000918C
		public WriteState WriteState
		{
			get
			{
				switch (this._currentState)
				{
				case JsonWriter.State.Start:
					return WriteState.Start;
				case JsonWriter.State.Property:
					return WriteState.Property;
				case JsonWriter.State.ObjectStart:
				case JsonWriter.State.Object:
					return WriteState.Object;
				case JsonWriter.State.ArrayStart:
				case JsonWriter.State.Array:
					return WriteState.Array;
				case JsonWriter.State.ConstructorStart:
				case JsonWriter.State.Constructor:
					return WriteState.Constructor;
				case JsonWriter.State.Closed:
					return WriteState.Closed;
				case JsonWriter.State.Error:
					return WriteState.Error;
				}
				throw new JsonWriterException("Invalid state: " + this._currentState);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000AFFC File Offset: 0x000091FC
		// (set) Token: 0x06000263 RID: 611 RVA: 0x0000B004 File Offset: 0x00009204
		public Formatting Formatting
		{
			get
			{
				return this._formatting;
			}
			set
			{
				this._formatting = value;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000B00D File Offset: 0x0000920D
		protected JsonWriter()
		{
			this._stack = new List<JTokenType>(8);
			this._stack.Add(JTokenType.None);
			this._currentState = JsonWriter.State.Start;
			this._formatting = Formatting.None;
			this.CloseOutput = true;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000B044 File Offset: 0x00009244
		private void Push(JTokenType value)
		{
			this._top++;
			if (this._stack.Count <= this._top)
			{
				this._stack.Add(value);
				return;
			}
			this._stack[this._top] = value;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000B094 File Offset: 0x00009294
		private JTokenType Pop()
		{
			JTokenType result = this.Peek();
			this._top--;
			return result;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000B0B7 File Offset: 0x000092B7
		private JTokenType Peek()
		{
			return this._stack[this._top];
		}

		// Token: 0x06000268 RID: 616
		public abstract void Flush();

		// Token: 0x06000269 RID: 617 RVA: 0x0000B0CA File Offset: 0x000092CA
		public virtual void Close()
		{
			this.AutoCompleteAll();
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000B0D2 File Offset: 0x000092D2
		public virtual void WriteStartObject()
		{
			this.AutoComplete(JsonToken.StartObject);
			this.Push(JTokenType.Object);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000B0E2 File Offset: 0x000092E2
		public virtual void WriteEndObject()
		{
			this.AutoCompleteClose(JsonToken.EndObject);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000B0EC File Offset: 0x000092EC
		public virtual void WriteStartArray()
		{
			this.AutoComplete(JsonToken.StartArray);
			this.Push(JTokenType.Array);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000B0FC File Offset: 0x000092FC
		public virtual void WriteEndArray()
		{
			this.AutoCompleteClose(JsonToken.EndArray);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000B106 File Offset: 0x00009306
		public virtual void WriteStartConstructor(string name)
		{
			this.AutoComplete(JsonToken.StartConstructor);
			this.Push(JTokenType.Constructor);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000B116 File Offset: 0x00009316
		public virtual void WriteEndConstructor()
		{
			this.AutoCompleteClose(JsonToken.EndConstructor);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000B120 File Offset: 0x00009320
		public virtual void WritePropertyName(string name)
		{
			this.AutoComplete(JsonToken.PropertyName);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000B129 File Offset: 0x00009329
		public virtual void WriteEnd()
		{
			this.WriteEnd(this.Peek());
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000B138 File Offset: 0x00009338
		public void WriteToken(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			int initialDepth;
			if (reader.TokenType == JsonToken.None)
			{
				initialDepth = -1;
			}
			else if (!this.IsStartToken(reader.TokenType))
			{
				initialDepth = reader.Depth + 1;
			}
			else
			{
				initialDepth = reader.Depth;
			}
			this.WriteToken(reader, initialDepth);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000B184 File Offset: 0x00009384
		internal void WriteToken(JsonReader reader, int initialDepth)
		{
			for (;;)
			{
				switch (reader.TokenType)
				{
				case JsonToken.None:
					goto IL_1B8;
				case JsonToken.StartObject:
					this.WriteStartObject();
					goto IL_1B8;
				case JsonToken.StartArray:
					this.WriteStartArray();
					goto IL_1B8;
				case JsonToken.StartConstructor:
				{
					string text = reader.Value.ToString();
					if (string.Compare(text, "Date", 4) == 0)
					{
						this.WriteConstructorDate(reader);
						goto IL_1B8;
					}
					this.WriteStartConstructor(reader.Value.ToString());
					goto IL_1B8;
				}
				case JsonToken.PropertyName:
					this.WritePropertyName(reader.Value.ToString());
					goto IL_1B8;
				case JsonToken.Comment:
					this.WriteComment(reader.Value.ToString());
					goto IL_1B8;
				case JsonToken.Raw:
					this.WriteRawValue((string)reader.Value);
					goto IL_1B8;
				case JsonToken.Integer:
					this.WriteValue(Convert.ToInt64(reader.Value, CultureInfo.InvariantCulture));
					goto IL_1B8;
				case JsonToken.Float:
					this.WriteValue(Convert.ToDouble(reader.Value, CultureInfo.InvariantCulture));
					goto IL_1B8;
				case JsonToken.String:
					this.WriteValue(reader.Value.ToString());
					goto IL_1B8;
				case JsonToken.Boolean:
					this.WriteValue(Convert.ToBoolean(reader.Value, CultureInfo.InvariantCulture));
					goto IL_1B8;
				case JsonToken.Null:
					this.WriteNull();
					goto IL_1B8;
				case JsonToken.Undefined:
					this.WriteUndefined();
					goto IL_1B8;
				case JsonToken.EndObject:
					this.WriteEndObject();
					goto IL_1B8;
				case JsonToken.EndArray:
					this.WriteEndArray();
					goto IL_1B8;
				case JsonToken.EndConstructor:
					this.WriteEndConstructor();
					goto IL_1B8;
				case JsonToken.Date:
					this.WriteValue((DateTime)reader.Value);
					goto IL_1B8;
				case JsonToken.Bytes:
					this.WriteValue((byte[])reader.Value);
					goto IL_1B8;
				}
				break;
				IL_1B8:
				if (initialDepth - 1 >= reader.Depth - (this.IsEndToken(reader.TokenType) ? 1 : 0) || !reader.Read())
				{
					return;
				}
			}
			throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", reader.TokenType, "Unexpected token type.");
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000B374 File Offset: 0x00009574
		private void WriteConstructorDate(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw new Exception("Unexpected end while reading date constructor.");
			}
			if (reader.TokenType != JsonToken.Integer)
			{
				throw new Exception("Unexpected token while reading date constructor. Expected Integer, got " + reader.TokenType);
			}
			long javaScriptTicks = (long)reader.Value;
			DateTime value = JsonConvert.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
			if (!reader.Read())
			{
				throw new Exception("Unexpected end while reading date constructor.");
			}
			if (reader.TokenType != JsonToken.EndConstructor)
			{
				throw new Exception("Unexpected token while reading date constructor. Expected EndConstructor, got " + reader.TokenType);
			}
			this.WriteValue(value);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000B40C File Offset: 0x0000960C
		private bool IsEndToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
			case JsonToken.EndArray:
			case JsonToken.EndConstructor:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000B438 File Offset: 0x00009638
		private bool IsStartToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.StartObject:
			case JsonToken.StartArray:
			case JsonToken.StartConstructor:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000B460 File Offset: 0x00009660
		private void WriteEnd(JTokenType type)
		{
			switch (type)
			{
			case JTokenType.Object:
				this.WriteEndObject();
				return;
			case JTokenType.Array:
				this.WriteEndArray();
				return;
			case JTokenType.Constructor:
				this.WriteEndConstructor();
				return;
			default:
				throw new JsonWriterException("Unexpected type when writing end: " + type);
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000B4AF File Offset: 0x000096AF
		private void AutoCompleteAll()
		{
			while (this._top > 0)
			{
				this.WriteEnd();
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000B4C4 File Offset: 0x000096C4
		private JTokenType GetTypeForCloseToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
				return JTokenType.Object;
			case JsonToken.EndArray:
				return JTokenType.Array;
			case JsonToken.EndConstructor:
				return JTokenType.Constructor;
			default:
				throw new JsonWriterException("No type for token: " + token);
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000B508 File Offset: 0x00009708
		private JsonToken GetCloseTokenForType(JTokenType type)
		{
			switch (type)
			{
			case JTokenType.Object:
				return JsonToken.EndObject;
			case JTokenType.Array:
				return JsonToken.EndArray;
			case JTokenType.Constructor:
				return JsonToken.EndConstructor;
			default:
				throw new JsonWriterException("No close token for type: " + type);
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000B54C File Offset: 0x0000974C
		private void AutoCompleteClose(JsonToken tokenBeingClosed)
		{
			int num = 0;
			for (int i = 0; i < this._top; i++)
			{
				int num2 = this._top - i;
				if (this._stack[num2] == this.GetTypeForCloseToken(tokenBeingClosed))
				{
					num = i + 1;
					break;
				}
			}
			if (num == 0)
			{
				throw new JsonWriterException("No token to close.");
			}
			for (int j = 0; j < num; j++)
			{
				JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
				if (this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
				{
					this.WriteIndent();
				}
				this.WriteEnd(closeTokenForType);
			}
			JTokenType jtokenType = this.Peek();
			switch (jtokenType)
			{
			case JTokenType.None:
				this._currentState = JsonWriter.State.Start;
				return;
			case JTokenType.Object:
				this._currentState = JsonWriter.State.Object;
				return;
			case JTokenType.Array:
				this._currentState = JsonWriter.State.Array;
				return;
			case JTokenType.Constructor:
				this._currentState = JsonWriter.State.Array;
				return;
			default:
				throw new JsonWriterException("Unknown JsonType: " + jtokenType);
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000B633 File Offset: 0x00009833
		protected virtual void WriteEnd(JsonToken token)
		{
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000B635 File Offset: 0x00009835
		protected virtual void WriteIndent()
		{
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000B637 File Offset: 0x00009837
		protected virtual void WriteValueDelimiter()
		{
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000B639 File Offset: 0x00009839
		protected virtual void WriteIndentSpace()
		{
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000B63C File Offset: 0x0000983C
		internal void AutoComplete(JsonToken tokenBeingWritten)
		{
			int num;
			switch (tokenBeingWritten)
			{
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				num = 7;
				break;
			default:
				num = (int)tokenBeingWritten;
				break;
			}
			JsonWriter.State state = JsonWriter.stateArray[num][(int)this._currentState];
			if (state == JsonWriter.State.Error)
			{
				throw new JsonWriterException("Token {0} in state {1} would result in an invalid JavaScript object.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					tokenBeingWritten.ToString(),
					this._currentState.ToString()
				}));
			}
			if ((this._currentState == JsonWriter.State.Object || this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.Constructor) && tokenBeingWritten != JsonToken.Comment)
			{
				this.WriteValueDelimiter();
			}
			else if (this._currentState == JsonWriter.State.Property && this._formatting == Formatting.Indented)
			{
				this.WriteIndentSpace();
			}
			WriteState writeState = this.WriteState;
			if ((tokenBeingWritten == JsonToken.PropertyName && writeState != WriteState.Start) || writeState == WriteState.Array || writeState == WriteState.Constructor)
			{
				this.WriteIndent();
			}
			this._currentState = state;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000B73E File Offset: 0x0000993E
		public virtual void WriteNull()
		{
			this.AutoComplete(JsonToken.Null);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000B748 File Offset: 0x00009948
		public virtual void WriteUndefined()
		{
			this.AutoComplete(JsonToken.Undefined);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000B752 File Offset: 0x00009952
		public virtual void WriteRaw(string json)
		{
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000B754 File Offset: 0x00009954
		public virtual void WriteRawValue(string json)
		{
			this.AutoComplete(JsonToken.Undefined);
			this.WriteRaw(json);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000B765 File Offset: 0x00009965
		public virtual void WriteValue(string value)
		{
			this.AutoComplete(JsonToken.String);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000B76F File Offset: 0x0000996F
		public virtual void WriteValue(int value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000B778 File Offset: 0x00009978
		[CLSCompliant(false)]
		public virtual void WriteValue(uint value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000B781 File Offset: 0x00009981
		public virtual void WriteValue(long value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000B78A File Offset: 0x0000998A
		[CLSCompliant(false)]
		public virtual void WriteValue(ulong value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000B793 File Offset: 0x00009993
		public virtual void WriteValue(float value)
		{
			this.AutoComplete(JsonToken.Float);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000B79C File Offset: 0x0000999C
		public virtual void WriteValue(double value)
		{
			this.AutoComplete(JsonToken.Float);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000B7A5 File Offset: 0x000099A5
		public virtual void WriteValue(bool value)
		{
			this.AutoComplete(JsonToken.Boolean);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000B7AF File Offset: 0x000099AF
		public virtual void WriteValue(short value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000B7B8 File Offset: 0x000099B8
		[CLSCompliant(false)]
		public virtual void WriteValue(ushort value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000B7C1 File Offset: 0x000099C1
		public virtual void WriteValue(char value)
		{
			this.AutoComplete(JsonToken.String);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000B7CB File Offset: 0x000099CB
		public virtual void WriteValue(byte value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000B7D4 File Offset: 0x000099D4
		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000B7DD File Offset: 0x000099DD
		public virtual void WriteValue(decimal value)
		{
			this.AutoComplete(JsonToken.Float);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000B7E6 File Offset: 0x000099E6
		public virtual void WriteValue(DateTime value)
		{
			this.AutoComplete(JsonToken.Date);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000B7F0 File Offset: 0x000099F0
		public virtual void WriteValue(DateTimeOffset value)
		{
			this.AutoComplete(JsonToken.Date);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000B7FA File Offset: 0x000099FA
		public virtual void WriteValue(Guid value)
		{
			this.AutoComplete(JsonToken.String);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000B804 File Offset: 0x00009A04
		public virtual void WriteValue(TimeSpan value)
		{
			this.AutoComplete(JsonToken.String);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000B80E File Offset: 0x00009A0E
		public virtual void WriteValue(int? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000B82D File Offset: 0x00009A2D
		[CLSCompliant(false)]
		public virtual void WriteValue(uint? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000B84C File Offset: 0x00009A4C
		public virtual void WriteValue(long? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000B86B File Offset: 0x00009A6B
		[CLSCompliant(false)]
		public virtual void WriteValue(ulong? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000B88A File Offset: 0x00009A8A
		public virtual void WriteValue(float? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000B8A9 File Offset: 0x00009AA9
		public virtual void WriteValue(double? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000B8C8 File Offset: 0x00009AC8
		public virtual void WriteValue(bool? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000B8E8 File Offset: 0x00009AE8
		public virtual void WriteValue(short? value)
		{
			short? num = value;
			int? num2 = (num != null) ? new int?((int)num.GetValueOrDefault()) : default(int?);
			if (num2 == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000B938 File Offset: 0x00009B38
		[CLSCompliant(false)]
		public virtual void WriteValue(ushort? value)
		{
			ushort? num = value;
			int? num2 = (num != null) ? new int?((int)num.GetValueOrDefault()) : default(int?);
			if (num2 == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000B988 File Offset: 0x00009B88
		public virtual void WriteValue(char? value)
		{
			char? c = value;
			int? num = (c != null) ? new int?((int)c.GetValueOrDefault()) : default(int?);
			if (num == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000B9D8 File Offset: 0x00009BD8
		public virtual void WriteValue(byte? value)
		{
			byte? b = value;
			int? num = (b != null) ? new int?((int)b.GetValueOrDefault()) : default(int?);
			if (num == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000BA28 File Offset: 0x00009C28
		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte? value)
		{
			sbyte? b = value;
			int? num = (b != null) ? new int?((int)b.GetValueOrDefault()) : default(int?);
			if (num == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000BA75 File Offset: 0x00009C75
		public virtual void WriteValue(decimal? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000BA94 File Offset: 0x00009C94
		public virtual void WriteValue(DateTime? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000BAB3 File Offset: 0x00009CB3
		public virtual void WriteValue(DateTimeOffset? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000BAD2 File Offset: 0x00009CD2
		public virtual void WriteValue(Guid? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000BAF1 File Offset: 0x00009CF1
		public virtual void WriteValue(TimeSpan? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000BB10 File Offset: 0x00009D10
		public virtual void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.AutoComplete(JsonToken.Bytes);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000BB24 File Offset: 0x00009D24
		public virtual void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.AutoComplete(JsonToken.String);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000BB40 File Offset: 0x00009D40
		public virtual void WriteValue(object value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			if (value is IConvertible)
			{
				IConvertible convertible = value as IConvertible;
				switch (convertible.GetTypeCode())
				{
				case 2:
					this.WriteNull();
					return;
				case 3:
					this.WriteValue(convertible.ToBoolean(CultureInfo.InvariantCulture));
					return;
				case 4:
					this.WriteValue(convertible.ToChar(CultureInfo.InvariantCulture));
					return;
				case 5:
					this.WriteValue(convertible.ToSByte(CultureInfo.InvariantCulture));
					return;
				case 6:
					this.WriteValue(convertible.ToByte(CultureInfo.InvariantCulture));
					return;
				case 7:
					this.WriteValue(convertible.ToInt16(CultureInfo.InvariantCulture));
					return;
				case 8:
					this.WriteValue(convertible.ToUInt16(CultureInfo.InvariantCulture));
					return;
				case 9:
					this.WriteValue(convertible.ToInt32(CultureInfo.InvariantCulture));
					return;
				case 10:
					this.WriteValue(convertible.ToUInt32(CultureInfo.InvariantCulture));
					return;
				case 11:
					this.WriteValue(convertible.ToInt64(CultureInfo.InvariantCulture));
					return;
				case 12:
					this.WriteValue(convertible.ToUInt64(CultureInfo.InvariantCulture));
					return;
				case 13:
					this.WriteValue(convertible.ToSingle(CultureInfo.InvariantCulture));
					return;
				case 14:
					this.WriteValue(convertible.ToDouble(CultureInfo.InvariantCulture));
					return;
				case 15:
					this.WriteValue(convertible.ToDecimal(CultureInfo.InvariantCulture));
					return;
				case 16:
					this.WriteValue(convertible.ToDateTime(CultureInfo.InvariantCulture));
					return;
				case 18:
					this.WriteValue(convertible.ToString(CultureInfo.InvariantCulture));
					return;
				}
			}
			else
			{
				if (value is DateTimeOffset)
				{
					this.WriteValue((DateTimeOffset)value);
					return;
				}
				if (value is byte[])
				{
					this.WriteValue((byte[])value);
					return;
				}
				if (value is Guid)
				{
					this.WriteValue((Guid)value);
					return;
				}
				if (value is Uri)
				{
					this.WriteValue((Uri)value);
					return;
				}
				if (value is TimeSpan)
				{
					this.WriteValue((TimeSpan)value);
					return;
				}
			}
			throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				value.GetType()
			}));
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000BD64 File Offset: 0x00009F64
		public virtual void WriteComment(string text)
		{
			this.AutoComplete(JsonToken.Comment);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000BD6D File Offset: 0x00009F6D
		public virtual void WriteWhitespace(string ws)
		{
			if (ws != null && !StringUtils.IsWhiteSpace(ws))
			{
				throw new JsonWriterException("Only white space characters should be used.");
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000BD85 File Offset: 0x00009F85
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000BD8E File Offset: 0x00009F8E
		private void Dispose(bool disposing)
		{
			if (this.WriteState != WriteState.Closed)
			{
				this.Close();
			}
		}

		// Token: 0x040000B8 RID: 184
		private static readonly JsonWriter.State[][] stateArray = new JsonWriter.State[][]
		{
			new JsonWriter.State[]
			{
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Property,
				JsonWriter.State.Error,
				JsonWriter.State.Property,
				JsonWriter.State.Property,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Property,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Object,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Property,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Object,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Object,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Array,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			}
		};

		// Token: 0x040000B9 RID: 185
		private int _top;

		// Token: 0x040000BA RID: 186
		private readonly List<JTokenType> _stack;

		// Token: 0x040000BB RID: 187
		private JsonWriter.State _currentState;

		// Token: 0x040000BC RID: 188
		private Formatting _formatting;

		// Token: 0x02000025 RID: 37
		private enum State
		{
			// Token: 0x040000BF RID: 191
			Start,
			// Token: 0x040000C0 RID: 192
			Property,
			// Token: 0x040000C1 RID: 193
			ObjectStart,
			// Token: 0x040000C2 RID: 194
			Object,
			// Token: 0x040000C3 RID: 195
			ArrayStart,
			// Token: 0x040000C4 RID: 196
			Array,
			// Token: 0x040000C5 RID: 197
			ConstructorStart,
			// Token: 0x040000C6 RID: 198
			Constructor,
			// Token: 0x040000C7 RID: 199
			Bytes,
			// Token: 0x040000C8 RID: 200
			Closed,
			// Token: 0x040000C9 RID: 201
			Error
		}
	}
}
