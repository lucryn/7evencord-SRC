using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x020000A3 RID: 163
	public class BsonReader : JsonReader
	{
		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0001D1DF File Offset: 0x0001B3DF
		// (set) Token: 0x060007ED RID: 2029 RVA: 0x0001D1E7 File Offset: 0x0001B3E7
		public bool JsonNet35BinaryCompatibility
		{
			get
			{
				return this._jsonNet35BinaryCompatibility;
			}
			set
			{
				this._jsonNet35BinaryCompatibility = value;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x0001D1F0 File Offset: 0x0001B3F0
		// (set) Token: 0x060007EF RID: 2031 RVA: 0x0001D1F8 File Offset: 0x0001B3F8
		public bool ReadRootValueAsArray
		{
			get
			{
				return this._readRootValueAsArray;
			}
			set
			{
				this._readRootValueAsArray = value;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x0001D201 File Offset: 0x0001B401
		// (set) Token: 0x060007F1 RID: 2033 RVA: 0x0001D209 File Offset: 0x0001B409
		public DateTimeKind DateTimeKindHandling
		{
			get
			{
				return this._dateTimeKindHandling;
			}
			set
			{
				this._dateTimeKindHandling = value;
			}
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0001D212 File Offset: 0x0001B412
		public BsonReader(Stream stream) : this(stream, false, 2)
		{
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0001D21D File Offset: 0x0001B41D
		public BsonReader(Stream stream, bool readRootValueAsArray, DateTimeKind dateTimeKindHandling)
		{
			ValidationUtils.ArgumentNotNull(stream, "stream");
			this._reader = new BinaryReader(stream);
			this._stack = new List<BsonReader.ContainerContext>();
			this._readRootValueAsArray = readRootValueAsArray;
			this._dateTimeKindHandling = dateTimeKindHandling;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0001D258 File Offset: 0x0001B458
		private string ReadElement()
		{
			this._currentElementType = this.ReadType();
			return this.ReadString();
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0001D27C File Offset: 0x0001B47C
		public override byte[] ReadAsBytes()
		{
			this.Read();
			if (this.IsWrappedInTypeObject())
			{
				byte[] array = this.ReadAsBytes();
				this.Read();
				this.SetToken(JsonToken.Bytes, array);
				return array;
			}
			if (this.TokenType == JsonToken.Null)
			{
				return null;
			}
			if (this.TokenType == JsonToken.Bytes)
			{
				return (byte[])this.Value;
			}
			throw new JsonReaderException("Error reading bytes. Expected bytes but got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				this.TokenType
			}));
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0001D300 File Offset: 0x0001B500
		private bool IsWrappedInTypeObject()
		{
			if (this.TokenType == JsonToken.StartObject)
			{
				this.Read();
				if (this.Value.ToString() == "$type")
				{
					this.Read();
					if (this.Value != null && this.Value.ToString().StartsWith("System.Byte[]"))
					{
						this.Read();
						if (this.Value.ToString() == "$value")
						{
							return true;
						}
					}
				}
				throw new JsonReaderException("Unexpected token when reading bytes: {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JsonToken.StartObject
				}));
			}
			return false;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0001D3A4 File Offset: 0x0001B5A4
		public override decimal? ReadAsDecimal()
		{
			this.Read();
			if (this.TokenType == JsonToken.Null)
			{
				return default(decimal?);
			}
			if (this.TokenType == JsonToken.Integer || this.TokenType == JsonToken.Float)
			{
				this.SetToken(JsonToken.Float, Convert.ToDecimal(this.Value, CultureInfo.InvariantCulture));
				return new decimal?((decimal)this.Value);
			}
			throw new JsonReaderException("Error reading decimal. Expected a number but got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				this.TokenType
			}));
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0001D438 File Offset: 0x0001B638
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			this.Read();
			if (this.TokenType == JsonToken.Null)
			{
				return default(DateTimeOffset?);
			}
			if (this.TokenType == JsonToken.Date)
			{
				this.SetToken(JsonToken.Date, new DateTimeOffset((DateTime)this.Value));
				return new DateTimeOffset?((DateTimeOffset)this.Value);
			}
			throw new JsonReaderException("Error reading date. Expected bytes but got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				this.TokenType
			}));
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0001D4C4 File Offset: 0x0001B6C4
		public override bool Read()
		{
			bool result;
			try
			{
				switch (this._bsonReaderState)
				{
				case BsonReader.BsonReaderState.Normal:
					result = this.ReadNormal();
					break;
				case BsonReader.BsonReaderState.ReferenceStart:
				case BsonReader.BsonReaderState.ReferenceRef:
				case BsonReader.BsonReaderState.ReferenceId:
					result = this.ReadReference();
					break;
				case BsonReader.BsonReaderState.CodeWScopeStart:
				case BsonReader.BsonReaderState.CodeWScopeCode:
				case BsonReader.BsonReaderState.CodeWScopeScope:
				case BsonReader.BsonReaderState.CodeWScopeScopeObject:
				case BsonReader.BsonReaderState.CodeWScopeScopeEnd:
					result = this.ReadCodeWScope();
					break;
				default:
					throw new JsonReaderException("Unexpected state: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						this._bsonReaderState
					}));
				}
			}
			catch (EndOfStreamException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0001D560 File Offset: 0x0001B760
		public override void Close()
		{
			base.Close();
			if (base.CloseInput && this._reader != null)
			{
				this._reader.Close();
			}
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0001D584 File Offset: 0x0001B784
		private bool ReadCodeWScope()
		{
			switch (this._bsonReaderState)
			{
			case BsonReader.BsonReaderState.CodeWScopeStart:
				this.SetToken(JsonToken.PropertyName, "$code");
				this._bsonReaderState = BsonReader.BsonReaderState.CodeWScopeCode;
				return true;
			case BsonReader.BsonReaderState.CodeWScopeCode:
				this.ReadInt32();
				this.SetToken(JsonToken.String, this.ReadLengthString());
				this._bsonReaderState = BsonReader.BsonReaderState.CodeWScopeScope;
				return true;
			case BsonReader.BsonReaderState.CodeWScopeScope:
			{
				if (base.CurrentState == JsonReader.State.PostValue)
				{
					this.SetToken(JsonToken.PropertyName, "$scope");
					return true;
				}
				base.SetToken(JsonToken.StartObject);
				this._bsonReaderState = BsonReader.BsonReaderState.CodeWScopeScopeObject;
				BsonReader.ContainerContext containerContext = new BsonReader.ContainerContext(BsonType.Object);
				this.PushContext(containerContext);
				containerContext.Length = this.ReadInt32();
				return true;
			}
			case BsonReader.BsonReaderState.CodeWScopeScopeObject:
			{
				bool flag = this.ReadNormal();
				if (flag && this.TokenType == JsonToken.EndObject)
				{
					this._bsonReaderState = BsonReader.BsonReaderState.CodeWScopeScopeEnd;
				}
				return flag;
			}
			case BsonReader.BsonReaderState.CodeWScopeScopeEnd:
				base.SetToken(JsonToken.EndObject);
				this._bsonReaderState = BsonReader.BsonReaderState.Normal;
				return true;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001D660 File Offset: 0x0001B860
		private bool ReadReference()
		{
			JsonReader.State currentState = base.CurrentState;
			switch (currentState)
			{
			case JsonReader.State.Property:
				if (this._bsonReaderState == BsonReader.BsonReaderState.ReferenceRef)
				{
					this.SetToken(JsonToken.String, this.ReadLengthString());
					return true;
				}
				if (this._bsonReaderState == BsonReader.BsonReaderState.ReferenceId)
				{
					this.SetToken(JsonToken.Bytes, this.ReadBytes(12));
					return true;
				}
				throw new JsonReaderException("Unexpected state when reading BSON reference: " + this._bsonReaderState);
			case JsonReader.State.ObjectStart:
				this.SetToken(JsonToken.PropertyName, "$ref");
				this._bsonReaderState = BsonReader.BsonReaderState.ReferenceRef;
				return true;
			default:
				if (currentState != JsonReader.State.PostValue)
				{
					throw new JsonReaderException("Unexpected state when reading BSON reference: " + base.CurrentState);
				}
				if (this._bsonReaderState == BsonReader.BsonReaderState.ReferenceRef)
				{
					this.SetToken(JsonToken.PropertyName, "$id");
					this._bsonReaderState = BsonReader.BsonReaderState.ReferenceId;
					return true;
				}
				if (this._bsonReaderState == BsonReader.BsonReaderState.ReferenceId)
				{
					base.SetToken(JsonToken.EndObject);
					this._bsonReaderState = BsonReader.BsonReaderState.Normal;
					return true;
				}
				throw new JsonReaderException("Unexpected state when reading BSON reference: " + this._bsonReaderState);
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0001D760 File Offset: 0x0001B960
		private bool ReadNormal()
		{
			switch (base.CurrentState)
			{
			case JsonReader.State.Start:
			{
				JsonToken token = (!this._readRootValueAsArray) ? JsonToken.StartObject : JsonToken.StartArray;
				BsonType type = (!this._readRootValueAsArray) ? BsonType.Object : BsonType.Array;
				base.SetToken(token);
				BsonReader.ContainerContext containerContext = new BsonReader.ContainerContext(type);
				this.PushContext(containerContext);
				containerContext.Length = this.ReadInt32();
				return true;
			}
			case JsonReader.State.Complete:
			case JsonReader.State.Closed:
				return false;
			case JsonReader.State.Property:
				this.ReadType(this._currentElementType);
				return true;
			case JsonReader.State.ObjectStart:
			case JsonReader.State.ArrayStart:
			case JsonReader.State.PostValue:
			{
				BsonReader.ContainerContext currentContext = this._currentContext;
				if (currentContext == null)
				{
					return false;
				}
				int num = currentContext.Length - 1;
				if (currentContext.Position < num)
				{
					if (currentContext.Type == BsonType.Array)
					{
						this.ReadElement();
						this.ReadType(this._currentElementType);
						return true;
					}
					this.SetToken(JsonToken.PropertyName, this.ReadElement());
					return true;
				}
				else
				{
					if (currentContext.Position != num)
					{
						throw new JsonReaderException("Read past end of current container context.");
					}
					if (this.ReadByte() != 0)
					{
						throw new JsonReaderException("Unexpected end of object byte value.");
					}
					this.PopContext();
					if (this._currentContext != null)
					{
						this.MovePosition(currentContext.Length);
					}
					JsonToken token2 = (currentContext.Type == BsonType.Object) ? JsonToken.EndObject : JsonToken.EndArray;
					base.SetToken(token2);
					return true;
				}
				break;
			}
			case JsonReader.State.ConstructorStart:
			case JsonReader.State.Constructor:
			case JsonReader.State.Error:
			case JsonReader.State.Finished:
				return false;
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0001D8B4 File Offset: 0x0001BAB4
		private void PopContext()
		{
			this._stack.RemoveAt(this._stack.Count - 1);
			if (this._stack.Count == 0)
			{
				this._currentContext = null;
				return;
			}
			this._currentContext = this._stack[this._stack.Count - 1];
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0001D90C File Offset: 0x0001BB0C
		private void PushContext(BsonReader.ContainerContext newContext)
		{
			this._stack.Add(newContext);
			this._currentContext = newContext;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0001D921 File Offset: 0x0001BB21
		private byte ReadByte()
		{
			this.MovePosition(1);
			return this._reader.ReadByte();
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001D938 File Offset: 0x0001BB38
		private void ReadType(BsonType type)
		{
			switch (type)
			{
			case BsonType.Number:
				this.SetToken(JsonToken.Float, this.ReadDouble());
				return;
			case BsonType.String:
			case BsonType.Symbol:
				this.SetToken(JsonToken.String, this.ReadLengthString());
				return;
			case BsonType.Object:
			{
				base.SetToken(JsonToken.StartObject);
				BsonReader.ContainerContext containerContext = new BsonReader.ContainerContext(BsonType.Object);
				this.PushContext(containerContext);
				containerContext.Length = this.ReadInt32();
				return;
			}
			case BsonType.Array:
			{
				base.SetToken(JsonToken.StartArray);
				BsonReader.ContainerContext containerContext2 = new BsonReader.ContainerContext(BsonType.Array);
				this.PushContext(containerContext2);
				containerContext2.Length = this.ReadInt32();
				return;
			}
			case BsonType.Binary:
				this.SetToken(JsonToken.Bytes, this.ReadBinary());
				return;
			case BsonType.Undefined:
				base.SetToken(JsonToken.Undefined);
				return;
			case BsonType.Oid:
			{
				byte[] value = this.ReadBytes(12);
				this.SetToken(JsonToken.Bytes, value);
				return;
			}
			case BsonType.Boolean:
			{
				bool flag = Convert.ToBoolean(this.ReadByte());
				this.SetToken(JsonToken.Boolean, flag);
				return;
			}
			case BsonType.Date:
			{
				long javaScriptTicks = this.ReadInt64();
				DateTime dateTime = JsonConvert.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
				DateTime dateTime2;
				switch (this.DateTimeKindHandling)
				{
				case 0:
					dateTime2 = DateTime.SpecifyKind(dateTime, 0);
					goto IL_14E;
				case 2:
					dateTime2 = dateTime.ToLocalTime();
					goto IL_14E;
				}
				dateTime2 = dateTime;
				IL_14E:
				this.SetToken(JsonToken.Date, dateTime2);
				return;
			}
			case BsonType.Null:
				base.SetToken(JsonToken.Null);
				return;
			case BsonType.Regex:
			{
				string text = this.ReadString();
				string text2 = this.ReadString();
				string value2 = "/" + text + "/" + text2;
				this.SetToken(JsonToken.String, value2);
				return;
			}
			case BsonType.Reference:
				base.SetToken(JsonToken.StartObject);
				this._bsonReaderState = BsonReader.BsonReaderState.ReferenceStart;
				return;
			case BsonType.Code:
				this.SetToken(JsonToken.String, this.ReadLengthString());
				return;
			case BsonType.CodeWScope:
				base.SetToken(JsonToken.StartObject);
				this._bsonReaderState = BsonReader.BsonReaderState.CodeWScopeStart;
				return;
			case BsonType.Integer:
				this.SetToken(JsonToken.Integer, (long)this.ReadInt32());
				return;
			case BsonType.TimeStamp:
			case BsonType.Long:
				this.SetToken(JsonToken.Integer, this.ReadInt64());
				return;
			default:
				throw new ArgumentOutOfRangeException("type", "Unexpected BsonType value: " + type);
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001DB4C File Offset: 0x0001BD4C
		private byte[] ReadBinary()
		{
			int count = this.ReadInt32();
			BsonBinaryType bsonBinaryType = (BsonBinaryType)this.ReadByte();
			if (bsonBinaryType == BsonBinaryType.Data && !this._jsonNet35BinaryCompatibility)
			{
				count = this.ReadInt32();
			}
			return this.ReadBytes(count);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001DB84 File Offset: 0x0001BD84
		private string ReadString()
		{
			this.EnsureBuffers();
			StringBuilder stringBuilder = null;
			int num = 0;
			int num2 = 0;
			int num4;
			for (;;)
			{
				int num3 = num2;
				byte b;
				while (num3 < 128 && (b = this._reader.ReadByte()) > 0)
				{
					this._byteBuffer[num3++] = b;
				}
				num4 = num3 - num2;
				num += num4;
				if (num3 < 128 && stringBuilder == null)
				{
					break;
				}
				int lastFullCharStop = this.GetLastFullCharStop(num3 - 1);
				int chars = Encoding.UTF8.GetChars(this._byteBuffer, 0, lastFullCharStop + 1, this._charBuffer, 0);
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(256);
				}
				stringBuilder.Append(this._charBuffer, 0, chars);
				if (lastFullCharStop < num4 - 1)
				{
					num2 = num4 - lastFullCharStop - 1;
					Array.Copy(this._byteBuffer, lastFullCharStop + 1, this._byteBuffer, 0, num2);
				}
				else
				{
					if (num3 < 128)
					{
						goto Block_6;
					}
					num2 = 0;
				}
			}
			int chars2 = Encoding.UTF8.GetChars(this._byteBuffer, 0, num4, this._charBuffer, 0);
			this.MovePosition(num + 1);
			return new string(this._charBuffer, 0, chars2);
			Block_6:
			this.MovePosition(num + 1);
			return stringBuilder.ToString();
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001DCA4 File Offset: 0x0001BEA4
		private string ReadLengthString()
		{
			int num = this.ReadInt32();
			this.MovePosition(num);
			string @string = this.GetString(num - 1);
			this._reader.ReadByte();
			return @string;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001DCD8 File Offset: 0x0001BED8
		private string GetString(int length)
		{
			if (length == 0)
			{
				return string.Empty;
			}
			this.EnsureBuffers();
			StringBuilder stringBuilder = null;
			int num = 0;
			int num2 = 0;
			int num4;
			for (;;)
			{
				int num3 = (length - num > 128 - num2) ? (128 - num2) : (length - num);
				num4 = this._reader.BaseStream.Read(this._byteBuffer, num2, num3);
				if (num4 == 0)
				{
					break;
				}
				num += num4;
				num4 += num2;
				if (num4 == length)
				{
					goto Block_4;
				}
				int lastFullCharStop = this.GetLastFullCharStop(num4 - 1);
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(length);
				}
				int chars = Encoding.UTF8.GetChars(this._byteBuffer, 0, lastFullCharStop + 1, this._charBuffer, 0);
				stringBuilder.Append(this._charBuffer, 0, chars);
				if (lastFullCharStop < num4 - 1)
				{
					num2 = num4 - lastFullCharStop - 1;
					Array.Copy(this._byteBuffer, lastFullCharStop + 1, this._byteBuffer, 0, num2);
				}
				else
				{
					num2 = 0;
				}
				if (num >= length)
				{
					goto Block_7;
				}
			}
			throw new EndOfStreamException("Unable to read beyond the end of the stream.");
			Block_4:
			int chars2 = Encoding.UTF8.GetChars(this._byteBuffer, 0, num4, this._charBuffer, 0);
			return new string(this._charBuffer, 0, chars2);
			Block_7:
			return stringBuilder.ToString();
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001DDF4 File Offset: 0x0001BFF4
		private int GetLastFullCharStop(int start)
		{
			int i = start;
			int num = 0;
			while (i >= 0)
			{
				num = this.BytesInSequence(this._byteBuffer[i]);
				if (num == 0)
				{
					i--;
				}
				else
				{
					if (num != 1)
					{
						i--;
						break;
					}
					break;
				}
			}
			if (num == start - i)
			{
				return start;
			}
			return i;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001DE38 File Offset: 0x0001C038
		private int BytesInSequence(byte b)
		{
			if (b <= BsonReader._seqRange1[1])
			{
				return 1;
			}
			if (b >= BsonReader._seqRange2[0] && b <= BsonReader._seqRange2[1])
			{
				return 2;
			}
			if (b >= BsonReader._seqRange3[0] && b <= BsonReader._seqRange3[1])
			{
				return 3;
			}
			if (b >= BsonReader._seqRange4[0] && b <= BsonReader._seqRange4[1])
			{
				return 4;
			}
			return 0;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001DE94 File Offset: 0x0001C094
		private void EnsureBuffers()
		{
			if (this._byteBuffer == null)
			{
				this._byteBuffer = new byte[128];
			}
			if (this._charBuffer == null)
			{
				int maxCharCount = Encoding.UTF8.GetMaxCharCount(128);
				this._charBuffer = new char[maxCharCount];
			}
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0001DEDD File Offset: 0x0001C0DD
		private double ReadDouble()
		{
			this.MovePosition(8);
			return this._reader.ReadDouble();
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001DEF1 File Offset: 0x0001C0F1
		private int ReadInt32()
		{
			this.MovePosition(4);
			return this._reader.ReadInt32();
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001DF05 File Offset: 0x0001C105
		private long ReadInt64()
		{
			this.MovePosition(8);
			return this._reader.ReadInt64();
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001DF19 File Offset: 0x0001C119
		private BsonType ReadType()
		{
			this.MovePosition(1);
			return (BsonType)this._reader.ReadSByte();
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001DF2D File Offset: 0x0001C12D
		private void MovePosition(int count)
		{
			this._currentContext.Position += count;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0001DF42 File Offset: 0x0001C142
		private byte[] ReadBytes(int count)
		{
			this.MovePosition(count);
			return this._reader.ReadBytes(count);
		}

		// Token: 0x04000235 RID: 565
		private const int MaxCharBytesSize = 128;

		// Token: 0x04000236 RID: 566
		private static readonly byte[] _seqRange1 = new byte[]
		{
			default(byte),
			127
		};

		// Token: 0x04000237 RID: 567
		private static readonly byte[] _seqRange2 = new byte[]
		{
			194,
			223
		};

		// Token: 0x04000238 RID: 568
		private static readonly byte[] _seqRange3 = new byte[]
		{
			224,
			239
		};

		// Token: 0x04000239 RID: 569
		private static readonly byte[] _seqRange4 = new byte[]
		{
			240,
			244
		};

		// Token: 0x0400023A RID: 570
		private readonly BinaryReader _reader;

		// Token: 0x0400023B RID: 571
		private readonly List<BsonReader.ContainerContext> _stack;

		// Token: 0x0400023C RID: 572
		private byte[] _byteBuffer;

		// Token: 0x0400023D RID: 573
		private char[] _charBuffer;

		// Token: 0x0400023E RID: 574
		private BsonType _currentElementType;

		// Token: 0x0400023F RID: 575
		private BsonReader.BsonReaderState _bsonReaderState;

		// Token: 0x04000240 RID: 576
		private BsonReader.ContainerContext _currentContext;

		// Token: 0x04000241 RID: 577
		private bool _readRootValueAsArray;

		// Token: 0x04000242 RID: 578
		private bool _jsonNet35BinaryCompatibility;

		// Token: 0x04000243 RID: 579
		private DateTimeKind _dateTimeKindHandling;

		// Token: 0x020000A4 RID: 164
		private enum BsonReaderState
		{
			// Token: 0x04000245 RID: 581
			Normal,
			// Token: 0x04000246 RID: 582
			ReferenceStart,
			// Token: 0x04000247 RID: 583
			ReferenceRef,
			// Token: 0x04000248 RID: 584
			ReferenceId,
			// Token: 0x04000249 RID: 585
			CodeWScopeStart,
			// Token: 0x0400024A RID: 586
			CodeWScopeCode,
			// Token: 0x0400024B RID: 587
			CodeWScopeScope,
			// Token: 0x0400024C RID: 588
			CodeWScopeScopeObject,
			// Token: 0x0400024D RID: 589
			CodeWScopeScopeEnd
		}

		// Token: 0x020000A5 RID: 165
		private class ContainerContext
		{
			// Token: 0x06000810 RID: 2064 RVA: 0x0001DFCE File Offset: 0x0001C1CE
			public ContainerContext(BsonType type)
			{
				this.Type = type;
			}

			// Token: 0x0400024E RID: 590
			public readonly BsonType Type;

			// Token: 0x0400024F RID: 591
			public int Length;

			// Token: 0x04000250 RID: 592
			public int Position;
		}
	}
}
