using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000011 RID: 17
	public abstract class JsonReader : IDisposable
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00005B21 File Offset: 0x00003D21
		protected JsonReader.State CurrentState
		{
			get
			{
				return this._currentState;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00005B29 File Offset: 0x00003D29
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00005B31 File Offset: 0x00003D31
		public bool CloseInput { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00005B3A File Offset: 0x00003D3A
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00005B42 File Offset: 0x00003D42
		public virtual char QuoteChar
		{
			get
			{
				return this._quoteChar;
			}
			protected internal set
			{
				this._quoteChar = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00005B4B File Offset: 0x00003D4B
		public virtual JsonToken TokenType
		{
			get
			{
				return this._token;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00005B53 File Offset: 0x00003D53
		public virtual object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00005B5B File Offset: 0x00003D5B
		public virtual Type ValueType
		{
			get
			{
				return this._valueType;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00005B64 File Offset: 0x00003D64
		public virtual int Depth
		{
			get
			{
				int num = this._top - 1;
				if (JsonReader.IsStartToken(this.TokenType))
				{
					return num - 1;
				}
				return num;
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005B8C File Offset: 0x00003D8C
		protected JsonReader()
		{
			this._currentState = JsonReader.State.Start;
			this._stack = new List<JTokenType>();
			this.CloseInput = true;
			this.Push(JTokenType.None);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005BB4 File Offset: 0x00003DB4
		private void Push(JTokenType value)
		{
			this._stack.Add(value);
			this._top++;
			this._currentTypeContext = value;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005BD8 File Offset: 0x00003DD8
		private JTokenType Pop()
		{
			JTokenType result = this.Peek();
			this._stack.RemoveAt(this._stack.Count - 1);
			this._top--;
			this._currentTypeContext = this._stack[this._top - 1];
			return result;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005C2C File Offset: 0x00003E2C
		private JTokenType Peek()
		{
			return this._currentTypeContext;
		}

		// Token: 0x060000B6 RID: 182
		public abstract bool Read();

		// Token: 0x060000B7 RID: 183
		public abstract byte[] ReadAsBytes();

		// Token: 0x060000B8 RID: 184
		public abstract decimal? ReadAsDecimal();

		// Token: 0x060000B9 RID: 185
		public abstract DateTimeOffset? ReadAsDateTimeOffset();

		// Token: 0x060000BA RID: 186 RVA: 0x00005C34 File Offset: 0x00003E34
		public void Skip()
		{
			if (JsonReader.IsStartToken(this.TokenType))
			{
				int depth = this.Depth;
				while (this.Read() && depth < this.Depth)
				{
				}
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005C66 File Offset: 0x00003E66
		protected void SetToken(JsonToken newToken)
		{
			this.SetToken(newToken, null);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005C70 File Offset: 0x00003E70
		protected virtual void SetToken(JsonToken newToken, object value)
		{
			this._token = newToken;
			switch (newToken)
			{
			case JsonToken.StartObject:
				this._currentState = JsonReader.State.ObjectStart;
				this.Push(JTokenType.Object);
				break;
			case JsonToken.StartArray:
				this._currentState = JsonReader.State.ArrayStart;
				this.Push(JTokenType.Array);
				break;
			case JsonToken.StartConstructor:
				this._currentState = JsonReader.State.ConstructorStart;
				this.Push(JTokenType.Constructor);
				break;
			case JsonToken.PropertyName:
				this._currentState = JsonReader.State.Property;
				this.Push(JTokenType.Property);
				break;
			case JsonToken.Raw:
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				this._currentState = JsonReader.State.PostValue;
				break;
			case JsonToken.EndObject:
				this.ValidateEnd(JsonToken.EndObject);
				this._currentState = JsonReader.State.PostValue;
				break;
			case JsonToken.EndArray:
				this.ValidateEnd(JsonToken.EndArray);
				this._currentState = JsonReader.State.PostValue;
				break;
			case JsonToken.EndConstructor:
				this.ValidateEnd(JsonToken.EndConstructor);
				this._currentState = JsonReader.State.PostValue;
				break;
			}
			JTokenType jtokenType = this.Peek();
			if (jtokenType == JTokenType.Property && this._currentState == JsonReader.State.PostValue)
			{
				this.Pop();
			}
			if (value != null)
			{
				this._value = value;
				this._valueType = value.GetType();
				return;
			}
			this._value = null;
			this._valueType = null;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005D90 File Offset: 0x00003F90
		private void ValidateEnd(JsonToken endToken)
		{
			JTokenType jtokenType = this.Pop();
			if (this.GetTypeForCloseToken(endToken) != jtokenType)
			{
				throw new JsonReaderException("JsonToken {0} is not valid for closing JsonType {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					endToken,
					jtokenType
				}));
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005DE0 File Offset: 0x00003FE0
		protected void SetStateBasedOnCurrent()
		{
			JTokenType jtokenType = this.Peek();
			switch (jtokenType)
			{
			case JTokenType.None:
				this._currentState = JsonReader.State.Finished;
				return;
			case JTokenType.Object:
				this._currentState = JsonReader.State.Object;
				return;
			case JTokenType.Array:
				this._currentState = JsonReader.State.Array;
				return;
			case JTokenType.Constructor:
				this._currentState = JsonReader.State.Constructor;
				return;
			default:
				throw new JsonReaderException("While setting the reader state back to current object an unexpected JsonType was encountered: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					jtokenType
				}));
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005E58 File Offset: 0x00004058
		internal static bool IsPrimitiveToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				return true;
			}
			return false;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005EA0 File Offset: 0x000040A0
		internal static bool IsStartToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.None:
			case JsonToken.Comment:
			case JsonToken.Raw:
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.EndObject:
			case JsonToken.EndArray:
			case JsonToken.EndConstructor:
			case JsonToken.Date:
			case JsonToken.Bytes:
				return false;
			case JsonToken.StartObject:
			case JsonToken.StartArray:
			case JsonToken.StartConstructor:
			case JsonToken.PropertyName:
				return true;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("token", token, "Unexpected JsonToken value.");
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005F18 File Offset: 0x00004118
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
				throw new JsonReaderException("Not a valid close JsonToken: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					token
				}));
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005F69 File Offset: 0x00004169
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005F72 File Offset: 0x00004172
		protected virtual void Dispose(bool disposing)
		{
			if (this._currentState != JsonReader.State.Closed && disposing)
			{
				this.Close();
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005F86 File Offset: 0x00004186
		public virtual void Close()
		{
			this._currentState = JsonReader.State.Closed;
			this._token = JsonToken.None;
			this._value = null;
			this._valueType = null;
		}

		// Token: 0x04000043 RID: 67
		private JsonToken _token;

		// Token: 0x04000044 RID: 68
		private object _value;

		// Token: 0x04000045 RID: 69
		private Type _valueType;

		// Token: 0x04000046 RID: 70
		private char _quoteChar;

		// Token: 0x04000047 RID: 71
		private JsonReader.State _currentState;

		// Token: 0x04000048 RID: 72
		private JTokenType _currentTypeContext;

		// Token: 0x04000049 RID: 73
		private int _top;

		// Token: 0x0400004A RID: 74
		private readonly List<JTokenType> _stack;

		// Token: 0x02000012 RID: 18
		protected enum State
		{
			// Token: 0x0400004D RID: 77
			Start,
			// Token: 0x0400004E RID: 78
			Complete,
			// Token: 0x0400004F RID: 79
			Property,
			// Token: 0x04000050 RID: 80
			ObjectStart,
			// Token: 0x04000051 RID: 81
			Object,
			// Token: 0x04000052 RID: 82
			ArrayStart,
			// Token: 0x04000053 RID: 83
			Array,
			// Token: 0x04000054 RID: 84
			Closed,
			// Token: 0x04000055 RID: 85
			PostValue,
			// Token: 0x04000056 RID: 86
			ConstructorStart,
			// Token: 0x04000057 RID: 87
			Constructor,
			// Token: 0x04000058 RID: 88
			Error,
			// Token: 0x04000059 RID: 89
			Finished
		}
	}
}
