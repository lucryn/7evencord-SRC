using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000027 RID: 39
	public class JsonTextReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000C4C6 File Offset: 0x0000A6C6
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0000C4D7 File Offset: 0x0000A6D7
		public CultureInfo Culture
		{
			get
			{
				return this._culture ?? CultureInfo.InvariantCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000C4E0 File Offset: 0x0000A6E0
		public JsonTextReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this._reader = reader;
			this._buffer = new StringBuffer(4096);
			this._currentLineNumber = 1;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000C514 File Offset: 0x0000A714
		private void ParseString(char quote)
		{
			this.ReadStringIntoBuffer(quote);
			if (this._readType == JsonTextReader.ReadType.ReadAsBytes)
			{
				byte[] value;
				if (this._buffer.Position == 0)
				{
					value = new byte[0];
				}
				else
				{
					value = Convert.FromBase64CharArray(this._buffer.GetInternalBuffer(), 0, this._buffer.Position);
					this._buffer.Position = 0;
				}
				this.SetToken(JsonToken.Bytes, value);
				return;
			}
			string text = this._buffer.ToString();
			this._buffer.Position = 0;
			if (text.StartsWith("/Date(", 4) && text.EndsWith(")/", 4))
			{
				this.ParseDate(text);
				return;
			}
			this.SetToken(JsonToken.String, text);
			this.QuoteChar = quote;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000C5C8 File Offset: 0x0000A7C8
		private void ReadStringIntoBuffer(char quote)
		{
			char c;
			for (;;)
			{
				c = this.MoveNext();
				char c2 = c;
				if (c2 <= '"')
				{
					if (c2 != '\0')
					{
						if (c2 != '"')
						{
							goto IL_2C1;
						}
					}
					else
					{
						if (this._end)
						{
							break;
						}
						this._buffer.Append('\0');
						continue;
					}
				}
				else if (c2 != '\'')
				{
					if (c2 != '\\')
					{
						goto IL_2C1;
					}
					if ((c = this.MoveNext()) == '\0' && this._end)
					{
						goto IL_26D;
					}
					char c3 = c;
					if (c3 <= '\\')
					{
						if (c3 <= '\'')
						{
							if (c3 != '"' && c3 != '\'')
							{
								goto Block_10;
							}
						}
						else if (c3 != '/')
						{
							if (c3 != '\\')
							{
								goto Block_12;
							}
							this._buffer.Append('\\');
							continue;
						}
						this._buffer.Append(c);
						continue;
					}
					if (c3 <= 'f')
					{
						if (c3 == 'b')
						{
							this._buffer.Append('\b');
							continue;
						}
						if (c3 != 'f')
						{
							goto Block_15;
						}
						this._buffer.Append('\f');
						continue;
					}
					else
					{
						if (c3 != 'n')
						{
							switch (c3)
							{
							case 'r':
								this._buffer.Append('\r');
								continue;
							case 't':
								this._buffer.Append('\t');
								continue;
							case 'u':
							{
								char[] array = new char[4];
								for (int i = 0; i < array.Length; i++)
								{
									if ((c = this.MoveNext()) == '\0' && this._end)
									{
										goto IL_1BB;
									}
									array[i] = c;
								}
								char value = Convert.ToChar(int.Parse(new string(array), 515, NumberFormatInfo.InvariantInfo));
								this._buffer.Append(value);
								continue;
							}
							}
							goto Block_17;
						}
						this._buffer.Append('\n');
						continue;
					}
				}
				if (c == quote)
				{
					return;
				}
				this._buffer.Append(c);
				continue;
				IL_2C1:
				this._buffer.Append(c);
			}
			throw this.CreateJsonReaderException("Unterminated string. Expected delimiter: {0}. Line {1}, position {2}.", new object[]
			{
				quote,
				this._currentLineNumber,
				this._currentLinePosition
			});
			Block_10:
			Block_12:
			Block_15:
			Block_17:
			goto IL_225;
			IL_1BB:
			throw this.CreateJsonReaderException("Unexpected end while parsing unicode character. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
			IL_225:
			throw this.CreateJsonReaderException("Bad JSON escape sequence: {0}. Line {1}, position {2}.", new object[]
			{
				"\\" + c,
				this._currentLineNumber,
				this._currentLinePosition
			});
			IL_26D:
			throw this.CreateJsonReaderException("Unterminated string. Expected delimiter: {0}. Line {1}, position {2}.", new object[]
			{
				quote,
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000C8A8 File Offset: 0x0000AAA8
		private JsonReaderException CreateJsonReaderException(string format, params object[] args)
		{
			string message = format.FormatWith(CultureInfo.InvariantCulture, args);
			return new JsonReaderException(message, null, this._currentLineNumber, this._currentLinePosition);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000C8D8 File Offset: 0x0000AAD8
		private TimeSpan ReadOffset(string offsetText)
		{
			bool flag = offsetText.get_Chars(0) == '-';
			int num = int.Parse(offsetText.Substring(1, 2), 7, CultureInfo.InvariantCulture);
			int num2 = 0;
			if (offsetText.Length >= 5)
			{
				num2 = int.Parse(offsetText.Substring(3, 2), 7, CultureInfo.InvariantCulture);
			}
			TimeSpan result = TimeSpan.FromHours((double)num) + TimeSpan.FromMinutes((double)num2);
			if (flag)
			{
				result = result.Negate();
			}
			return result;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000C944 File Offset: 0x0000AB44
		private void ParseDate(string text)
		{
			string text2 = text.Substring(6, text.Length - 8);
			DateTimeKind dateTimeKind = 1;
			int num = text2.IndexOf('+', 1);
			if (num == -1)
			{
				num = text2.IndexOf('-', 1);
			}
			TimeSpan timeSpan = TimeSpan.Zero;
			if (num != -1)
			{
				dateTimeKind = 2;
				timeSpan = this.ReadOffset(text2.Substring(num));
				text2 = text2.Substring(0, num);
			}
			long javaScriptTicks = long.Parse(text2, 7, CultureInfo.InvariantCulture);
			DateTime dateTime = JsonConvert.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
			if (this._readType == JsonTextReader.ReadType.ReadAsDateTimeOffset)
			{
				this.SetToken(JsonToken.Date, new DateTimeOffset(dateTime.Add(timeSpan).Ticks, timeSpan));
				return;
			}
			DateTime dateTime2;
			switch (dateTimeKind)
			{
			case 0:
				dateTime2 = DateTime.SpecifyKind(dateTime.ToLocalTime(), 0);
				goto IL_CA;
			case 2:
				dateTime2 = dateTime.ToLocalTime();
				goto IL_CA;
			}
			dateTime2 = dateTime;
			IL_CA:
			this.SetToken(JsonToken.Date, dateTime2);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000CA2C File Offset: 0x0000AC2C
		private char MoveNext()
		{
			int num = this._reader.Read();
			int num2 = num;
			if (num2 != -1)
			{
				if (num2 != 10)
				{
					if (num2 != 13)
					{
						this._currentLinePosition++;
					}
					else
					{
						if (this._reader.Peek() == 10)
						{
							this._reader.Read();
						}
						this._currentLineNumber++;
						this._currentLinePosition = 0;
					}
				}
				else
				{
					this._currentLineNumber++;
					this._currentLinePosition = 0;
				}
				return (char)num;
			}
			this._end = true;
			return '\0';
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000CAB9 File Offset: 0x0000ACB9
		private bool HasNext()
		{
			return this._reader.Peek() != -1;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000CACC File Offset: 0x0000ACCC
		private int PeekNext()
		{
			return this._reader.Peek();
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000CAD9 File Offset: 0x0000ACD9
		public override bool Read()
		{
			this._readType = JsonTextReader.ReadType.Read;
			return this.ReadInternal();
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000CAE8 File Offset: 0x0000ACE8
		private bool IsWrappedInTypeObject()
		{
			this._readType = JsonTextReader.ReadType.Read;
			if (this.TokenType == JsonToken.StartObject)
			{
				int currentLineNumber = this._currentLineNumber;
				int currentLinePosition = this._currentLinePosition;
				this.ReadInternal();
				if (this.Value.ToString() == "$type")
				{
					this.ReadInternal();
					if (this.Value != null && this.Value.ToString().StartsWith("System.Byte[]"))
					{
						this.ReadInternal();
						if (this.Value.ToString() == "$value")
						{
							return true;
						}
					}
				}
				throw this.CreateJsonReaderException("Unexpected token when reading bytes: {0}. Line {1}, position {2}.", new object[]
				{
					JsonToken.StartObject,
					currentLineNumber,
					currentLinePosition
				});
			}
			return false;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000CBAC File Offset: 0x0000ADAC
		public override byte[] ReadAsBytes()
		{
			this._readType = JsonTextReader.ReadType.ReadAsBytes;
			while (this.ReadInternal())
			{
				if (this.TokenType != JsonToken.Comment)
				{
					if (this.IsWrappedInTypeObject())
					{
						byte[] array = this.ReadAsBytes();
						this.ReadInternal();
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
					if (this.TokenType == JsonToken.StartArray)
					{
						List<byte> list = new List<byte>();
						while (this.ReadInternal())
						{
							JsonToken tokenType = this.TokenType;
							switch (tokenType)
							{
							case JsonToken.Comment:
								continue;
							case JsonToken.Raw:
								break;
							case JsonToken.Integer:
								list.Add(Convert.ToByte(this.Value, CultureInfo.InvariantCulture));
								continue;
							default:
								if (tokenType == JsonToken.EndArray)
								{
									byte[] array2 = list.ToArray();
									this.SetToken(JsonToken.Bytes, array2);
									return array2;
								}
								break;
							}
							throw this.CreateJsonReaderException("Unexpected token when reading bytes: {0}. Line {1}, position {2}.", new object[]
							{
								this.TokenType,
								this._currentLineNumber,
								this._currentLinePosition
							});
						}
						throw this.CreateJsonReaderException("Unexpected end when reading bytes: Line {0}, position {1}.", new object[]
						{
							this._currentLineNumber,
							this._currentLinePosition
						});
					}
					throw this.CreateJsonReaderException("Unexpected token when reading bytes: {0}. Line {1}, position {2}.", new object[]
					{
						this.TokenType,
						this._currentLineNumber,
						this._currentLinePosition
					});
				}
			}
			throw this.CreateJsonReaderException("Unexpected end when reading bytes: Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000CD70 File Offset: 0x0000AF70
		public override decimal? ReadAsDecimal()
		{
			this._readType = JsonTextReader.ReadType.ReadAsDecimal;
			while (this.ReadInternal())
			{
				if (this.TokenType != JsonToken.Comment)
				{
					if (this.TokenType == JsonToken.Null)
					{
						return default(decimal?);
					}
					if (this.TokenType == JsonToken.Float)
					{
						return (decimal?)this.Value;
					}
					decimal num;
					if (this.TokenType == JsonToken.String && decimal.TryParse((string)this.Value, 111, this.Culture, ref num))
					{
						this.SetToken(JsonToken.Float, num);
						return new decimal?(num);
					}
					throw this.CreateJsonReaderException("Unexpected token when reading decimal: {0}. Line {1}, position {2}.", new object[]
					{
						this.TokenType,
						this._currentLineNumber,
						this._currentLinePosition
					});
				}
			}
			throw this.CreateJsonReaderException("Unexpected end when reading decimal: Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000CE68 File Offset: 0x0000B068
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			this._readType = JsonTextReader.ReadType.ReadAsDateTimeOffset;
			while (this.ReadInternal())
			{
				if (this.TokenType != JsonToken.Comment)
				{
					if (this.TokenType == JsonToken.Null)
					{
						return default(DateTimeOffset?);
					}
					if (this.TokenType == JsonToken.Date)
					{
						return new DateTimeOffset?((DateTimeOffset)this.Value);
					}
					DateTimeOffset dateTimeOffset;
					if (this.TokenType == JsonToken.String && DateTimeOffset.TryParse((string)this.Value, this.Culture, 0, ref dateTimeOffset))
					{
						this.SetToken(JsonToken.Date, dateTimeOffset);
						return new DateTimeOffset?(dateTimeOffset);
					}
					throw this.CreateJsonReaderException("Unexpected token when reading date: {0}. Line {1}, position {2}.", new object[]
					{
						this.TokenType,
						this._currentLineNumber,
						this._currentLinePosition
					});
				}
			}
			throw this.CreateJsonReaderException("Unexpected end when reading date: Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000CF64 File Offset: 0x0000B164
		private bool ReadInternal()
		{
			char c;
			for (;;)
			{
				char? lastChar = this._lastChar;
				int? num = (lastChar != null) ? new int?((int)lastChar.GetValueOrDefault()) : default(int?);
				if (num != null)
				{
					c = this._lastChar.Value;
					this._lastChar = default(char?);
				}
				else
				{
					c = this.MoveNext();
				}
				if (c == '\0' && this._end)
				{
					break;
				}
				switch (base.CurrentState)
				{
				case JsonReader.State.Start:
				case JsonReader.State.Property:
				case JsonReader.State.ArrayStart:
				case JsonReader.State.Array:
				case JsonReader.State.ConstructorStart:
				case JsonReader.State.Constructor:
					goto IL_A0;
				case JsonReader.State.Complete:
				case JsonReader.State.Closed:
				case JsonReader.State.Error:
					continue;
				case JsonReader.State.ObjectStart:
				case JsonReader.State.Object:
					goto IL_A8;
				case JsonReader.State.PostValue:
					if (this.ParsePostValue(c))
					{
						return true;
					}
					continue;
				}
				goto Block_4;
			}
			return false;
			Block_4:
			throw this.CreateJsonReaderException("Unexpected state: {0}. Line {1}, position {2}.", new object[]
			{
				base.CurrentState,
				this._currentLineNumber,
				this._currentLinePosition
			});
			IL_A0:
			return this.ParseValue(c);
			IL_A8:
			return this.ParseObject(c);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000D074 File Offset: 0x0000B274
		private bool ParsePostValue(char currentChar)
		{
			for (;;)
			{
				char c = currentChar;
				if (c <= ')')
				{
					switch (c)
					{
					case '\t':
					case '\n':
					case '\r':
						break;
					case '\v':
					case '\f':
						goto IL_7C;
					default:
						if (c != ' ')
						{
							if (c != ')')
							{
								goto IL_7C;
							}
							goto IL_62;
						}
						break;
					}
				}
				else if (c <= '/')
				{
					if (c == ',')
					{
						goto IL_74;
					}
					if (c != '/')
					{
						goto IL_7C;
					}
					goto IL_6C;
				}
				else
				{
					if (c == ']')
					{
						goto IL_58;
					}
					if (c == '}')
					{
						break;
					}
					goto IL_7C;
				}
				IL_BD:
				if ((currentChar = this.MoveNext()) == '\0' && this._end)
				{
					return false;
				}
				continue;
				IL_7C:
				if (!char.IsWhiteSpace(currentChar))
				{
					goto Block_9;
				}
				goto IL_BD;
			}
			base.SetToken(JsonToken.EndObject);
			return true;
			IL_58:
			base.SetToken(JsonToken.EndArray);
			return true;
			IL_62:
			base.SetToken(JsonToken.EndConstructor);
			return true;
			IL_6C:
			this.ParseComment();
			return true;
			IL_74:
			base.SetStateBasedOnCurrent();
			return false;
			Block_9:
			throw this.CreateJsonReaderException("After parsing a value an unexpected character was encountered: {0}. Line {1}, position {2}.", new object[]
			{
				currentChar,
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000D158 File Offset: 0x0000B358
		private bool ParseObject(char currentChar)
		{
			for (;;)
			{
				char c = currentChar;
				if (c <= ' ')
				{
					switch (c)
					{
					case '\t':
					case '\n':
					case '\r':
						break;
					case '\v':
					case '\f':
						goto IL_47;
					default:
						if (c != ' ')
						{
							goto IL_47;
						}
						break;
					}
				}
				else
				{
					if (c == '/')
					{
						goto IL_3F;
					}
					if (c == '}')
					{
						break;
					}
					goto IL_47;
				}
				IL_57:
				if ((currentChar = this.MoveNext()) == '\0' && this._end)
				{
					return false;
				}
				continue;
				IL_47:
				if (!char.IsWhiteSpace(currentChar))
				{
					goto Block_5;
				}
				goto IL_57;
			}
			base.SetToken(JsonToken.EndObject);
			return true;
			IL_3F:
			this.ParseComment();
			return true;
			Block_5:
			return this.ParseProperty(currentChar);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000D1D0 File Offset: 0x0000B3D0
		private bool ParseProperty(char firstChar)
		{
			char c = firstChar;
			char c2;
			if (this.ValidIdentifierChar(c))
			{
				c2 = '\0';
				c = this.ParseUnquotedProperty(c);
			}
			else
			{
				if (c != '"' && c != '\'')
				{
					throw this.CreateJsonReaderException("Invalid property identifier character: {0}. Line {1}, position {2}.", new object[]
					{
						c,
						this._currentLineNumber,
						this._currentLinePosition
					});
				}
				c2 = c;
				this.ReadStringIntoBuffer(c2);
				c = this.MoveNext();
			}
			if (c != ':')
			{
				c = this.MoveNext();
				this.EatWhitespace(c, false, out c);
				if (c != ':')
				{
					throw this.CreateJsonReaderException("Invalid character after parsing property name. Expected ':' but got: {0}. Line {1}, position {2}.", new object[]
					{
						c,
						this._currentLineNumber,
						this._currentLinePosition
					});
				}
			}
			this.SetToken(JsonToken.PropertyName, this._buffer.ToString());
			this.QuoteChar = c2;
			this._buffer.Position = 0;
			return true;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
		private bool ValidIdentifierChar(char value)
		{
			return char.IsLetterOrDigit(value) || value == '_' || value == '$';
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000D2DC File Offset: 0x0000B4DC
		private char ParseUnquotedProperty(char firstChar)
		{
			this._buffer.Append(firstChar);
			char c;
			while ((c = this.MoveNext()) != '\0' || !this._end)
			{
				if (char.IsWhiteSpace(c) || c == ':')
				{
					return c;
				}
				if (!this.ValidIdentifierChar(c))
				{
					throw this.CreateJsonReaderException("Invalid JavaScript property identifier character: {0}. Line {1}, position {2}.", new object[]
					{
						c,
						this._currentLineNumber,
						this._currentLinePosition
					});
				}
				this._buffer.Append(c);
			}
			throw this.CreateJsonReaderException("Unexpected end when parsing unquoted property name. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000D398 File Offset: 0x0000B598
		private bool ParseValue(char currentChar)
		{
			for (;;)
			{
				char c = currentChar;
				if (c <= 'N')
				{
					if (c <= '"')
					{
						switch (c)
						{
						case '\t':
						case '\n':
						case '\r':
							break;
						case '\v':
						case '\f':
							goto IL_1FC;
						default:
							switch (c)
							{
							case ' ':
								break;
							case '!':
								goto IL_1FC;
							case '"':
								goto IL_D9;
							default:
								goto IL_1FC;
							}
							break;
						}
					}
					else
					{
						switch (c)
						{
						case '\'':
							goto IL_D9;
						case '(':
						case '*':
						case '+':
						case '.':
							goto IL_1FC;
						case ')':
							goto IL_1F2;
						case ',':
							goto IL_1E8;
						case '-':
							goto IL_197;
						case '/':
							goto IL_1B2;
						default:
							if (c == 'I')
							{
								goto IL_18F;
							}
							if (c != 'N')
							{
								goto IL_1FC;
							}
							goto IL_187;
						}
					}
				}
				else if (c <= 'f')
				{
					switch (c)
					{
					case '[':
						goto IL_1CB;
					case '\\':
						goto IL_1FC;
					case ']':
						goto IL_1DE;
					default:
						if (c != 'f')
						{
							goto IL_1FC;
						}
						goto IL_EA;
					}
				}
				else
				{
					if (c == 'n')
					{
						goto IL_F2;
					}
					switch (c)
					{
					case 't':
						goto IL_E2;
					case 'u':
						goto IL_1BA;
					default:
						switch (c)
						{
						case '{':
							goto IL_1C2;
						case '|':
							goto IL_1FC;
						case '}':
							goto IL_1D4;
						default:
							goto IL_1FC;
						}
						break;
					}
				}
				IL_25D:
				if ((currentChar = this.MoveNext()) == '\0' && this._end)
				{
					return false;
				}
				continue;
				IL_1FC:
				if (!char.IsWhiteSpace(currentChar))
				{
					goto Block_17;
				}
				goto IL_25D;
			}
			IL_D9:
			this.ParseString(currentChar);
			return true;
			IL_E2:
			this.ParseTrue();
			return true;
			IL_EA:
			this.ParseFalse();
			return true;
			IL_F2:
			if (this.HasNext())
			{
				char c2 = (char)this.PeekNext();
				if (c2 == 'u')
				{
					this.ParseNull();
				}
				else
				{
					if (c2 != 'e')
					{
						throw this.CreateJsonReaderException("Unexpected character encountered while parsing value: {0}. Line {1}, position {2}.", new object[]
						{
							currentChar,
							this._currentLineNumber,
							this._currentLinePosition
						});
					}
					this.ParseConstructor();
				}
				return true;
			}
			throw this.CreateJsonReaderException("Unexpected end. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
			IL_187:
			this.ParseNumberNaN();
			return true;
			IL_18F:
			this.ParseNumberPositiveInfinity();
			return true;
			IL_197:
			if (this.PeekNext() == 73)
			{
				this.ParseNumberNegativeInfinity();
			}
			else
			{
				this.ParseNumber(currentChar);
			}
			return true;
			IL_1B2:
			this.ParseComment();
			return true;
			IL_1BA:
			this.ParseUndefined();
			return true;
			IL_1C2:
			base.SetToken(JsonToken.StartObject);
			return true;
			IL_1CB:
			base.SetToken(JsonToken.StartArray);
			return true;
			IL_1D4:
			base.SetToken(JsonToken.EndObject);
			return true;
			IL_1DE:
			base.SetToken(JsonToken.EndArray);
			return true;
			IL_1E8:
			base.SetToken(JsonToken.Undefined);
			return true;
			IL_1F2:
			base.SetToken(JsonToken.EndConstructor);
			return true;
			Block_17:
			if (char.IsNumber(currentChar) || currentChar == '-' || currentChar == '.')
			{
				this.ParseNumber(currentChar);
				return true;
			}
			throw this.CreateJsonReaderException("Unexpected character encountered while parsing value: {0}. Line {1}, position {2}.", new object[]
			{
				currentChar,
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000D61C File Offset: 0x0000B81C
		private bool EatWhitespace(char initialChar, bool oneOrMore, out char finalChar)
		{
			bool flag = false;
			char c = initialChar;
			while (c == ' ' || char.IsWhiteSpace(c))
			{
				flag = true;
				c = this.MoveNext();
			}
			finalChar = c;
			return !oneOrMore || flag;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000D650 File Offset: 0x0000B850
		private void ParseConstructor()
		{
			if (this.MatchValue('n', "new", true))
			{
				char c = this.MoveNext();
				if (this.EatWhitespace(c, true, out c))
				{
					while (char.IsLetter(c))
					{
						this._buffer.Append(c);
						c = this.MoveNext();
					}
					this.EatWhitespace(c, false, out c);
					if (c != '(')
					{
						throw this.CreateJsonReaderException("Unexpected character while parsing constructor: {0}. Line {1}, position {2}.", new object[]
						{
							c,
							this._currentLineNumber,
							this._currentLinePosition
						});
					}
					string value = this._buffer.ToString();
					this._buffer.Position = 0;
					this.SetToken(JsonToken.StartConstructor, value);
				}
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000D70C File Offset: 0x0000B90C
		private void ParseNumber(char firstChar)
		{
			char c = firstChar;
			bool flag = false;
			do
			{
				if (this.IsSeperator(c))
				{
					flag = true;
					this._lastChar = new char?(c);
				}
				else
				{
					this._buffer.Append(c);
				}
			}
			while (!flag && ((c = this.MoveNext()) != '\0' || !this._end));
			string text = this._buffer.ToString();
			bool flag2 = firstChar == '0' && !text.StartsWith("0.", 5);
			object value;
			JsonToken newToken;
			if (this._readType == JsonTextReader.ReadType.ReadAsDecimal)
			{
				if (flag2)
				{
					long num = text.StartsWith("0x", 5) ? Convert.ToInt64(text, 16) : Convert.ToInt64(text, 8);
					value = Convert.ToDecimal(num);
				}
				else
				{
					value = decimal.Parse(text, 239, CultureInfo.InvariantCulture);
				}
				newToken = JsonToken.Float;
			}
			else if (flag2)
			{
				value = (text.StartsWith("0x", 5) ? Convert.ToInt64(text, 16) : Convert.ToInt64(text, 8));
				newToken = JsonToken.Integer;
			}
			else if (text.IndexOf(".", 5) != -1 || text.IndexOf("e", 5) != -1)
			{
				value = Convert.ToDouble(text, CultureInfo.InvariantCulture);
				newToken = JsonToken.Float;
			}
			else
			{
				try
				{
					value = Convert.ToInt64(text, CultureInfo.InvariantCulture);
				}
				catch (OverflowException innerException)
				{
					throw new JsonReaderException("JSON integer {0} is too large or small for an Int64.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						text
					}), innerException);
				}
				newToken = JsonToken.Integer;
			}
			this._buffer.Position = 0;
			this.SetToken(newToken, value);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000D89C File Offset: 0x0000BA9C
		private void ParseComment()
		{
			char c = this.MoveNext();
			if (c == '*')
			{
				while ((c = this.MoveNext()) != '\0' || !this._end)
				{
					if (c == '*')
					{
						if ((c = this.MoveNext()) != '\0' || !this._end)
						{
							if (c == '/')
							{
								IL_95:
								this.SetToken(JsonToken.Comment, this._buffer.ToString());
								this._buffer.Position = 0;
								return;
							}
							this._buffer.Append('*');
							this._buffer.Append(c);
						}
					}
					else
					{
						this._buffer.Append(c);
					}
				}
				goto IL_95;
			}
			throw this.CreateJsonReaderException("Error parsing comment. Expected: *. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000D95C File Offset: 0x0000BB5C
		private bool MatchValue(char firstChar, string value)
		{
			char c = firstChar;
			int num = 0;
			while (c == value.get_Chars(num))
			{
				num++;
				if (num >= value.Length || ((c = this.MoveNext()) == '\0' && this._end))
				{
					break;
				}
			}
			return num == value.Length;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000D9A0 File Offset: 0x0000BBA0
		private bool MatchValue(char firstChar, string value, bool noTrailingNonSeperatorCharacters)
		{
			bool flag = this.MatchValue(firstChar, value);
			if (!noTrailingNonSeperatorCharacters)
			{
				return flag;
			}
			int num = this.PeekNext();
			char c = (num != -1) ? ((char)num) : '\0';
			return flag && (c == '\0' || this.IsSeperator(c));
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000D9E4 File Offset: 0x0000BBE4
		private bool IsSeperator(char c)
		{
			if (c <= ')')
			{
				switch (c)
				{
				case '\t':
				case '\n':
				case '\r':
					break;
				case '\v':
				case '\f':
					goto IL_7A;
				default:
					if (c != ' ')
					{
						if (c != ')')
						{
							goto IL_7A;
						}
						if (base.CurrentState == JsonReader.State.Constructor || base.CurrentState == JsonReader.State.ConstructorStart)
						{
							return true;
						}
						return false;
					}
					break;
				}
				return true;
			}
			if (c <= '/')
			{
				if (c != ',')
				{
					if (c != '/')
					{
						goto IL_7A;
					}
					return this.HasNext() && this.PeekNext() == 42;
				}
			}
			else if (c != ']' && c != '}')
			{
				goto IL_7A;
			}
			return true;
			IL_7A:
			if (char.IsWhiteSpace(c))
			{
				return true;
			}
			return false;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000DA78 File Offset: 0x0000BC78
		private void ParseTrue()
		{
			if (this.MatchValue('t', JsonConvert.True, true))
			{
				this.SetToken(JsonToken.Boolean, true);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing boolean value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000DAD4 File Offset: 0x0000BCD4
		private void ParseNull()
		{
			if (this.MatchValue('n', JsonConvert.Null, true))
			{
				base.SetToken(JsonToken.Null);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing null value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000DB2C File Offset: 0x0000BD2C
		private void ParseUndefined()
		{
			if (this.MatchValue('u', JsonConvert.Undefined, true))
			{
				base.SetToken(JsonToken.Undefined);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing undefined value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000DB84 File Offset: 0x0000BD84
		private void ParseFalse()
		{
			if (this.MatchValue('f', JsonConvert.False, true))
			{
				this.SetToken(JsonToken.Boolean, false);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing boolean value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000DBE0 File Offset: 0x0000BDE0
		private void ParseNumberNegativeInfinity()
		{
			if (this.MatchValue('-', JsonConvert.NegativeInfinity, true))
			{
				this.SetToken(JsonToken.Float, double.NegativeInfinity);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing negative infinity value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000DC44 File Offset: 0x0000BE44
		private void ParseNumberPositiveInfinity()
		{
			if (this.MatchValue('I', JsonConvert.PositiveInfinity, true))
			{
				this.SetToken(JsonToken.Float, double.PositiveInfinity);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing positive infinity value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000DCA8 File Offset: 0x0000BEA8
		private void ParseNumberNaN()
		{
			if (this.MatchValue('N', JsonConvert.NaN, true))
			{
				this.SetToken(JsonToken.Float, double.NaN);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing NaN value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000DD0A File Offset: 0x0000BF0A
		public override void Close()
		{
			base.Close();
			if (base.CloseInput && this._reader != null)
			{
				this._reader.Close();
			}
			if (this._buffer != null)
			{
				this._buffer.Clear();
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000DD40 File Offset: 0x0000BF40
		public bool HasLineInfo()
		{
			return true;
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000DD43 File Offset: 0x0000BF43
		public int LineNumber
		{
			get
			{
				if (base.CurrentState == JsonReader.State.Start)
				{
					return 0;
				}
				return this._currentLineNumber;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000DD55 File Offset: 0x0000BF55
		public int LinePosition
		{
			get
			{
				return this._currentLinePosition;
			}
		}

		// Token: 0x040000D0 RID: 208
		private const int LineFeedValue = 10;

		// Token: 0x040000D1 RID: 209
		private const int CarriageReturnValue = 13;

		// Token: 0x040000D2 RID: 210
		private readonly TextReader _reader;

		// Token: 0x040000D3 RID: 211
		private readonly StringBuffer _buffer;

		// Token: 0x040000D4 RID: 212
		private char? _lastChar;

		// Token: 0x040000D5 RID: 213
		private int _currentLinePosition;

		// Token: 0x040000D6 RID: 214
		private int _currentLineNumber;

		// Token: 0x040000D7 RID: 215
		private bool _end;

		// Token: 0x040000D8 RID: 216
		private JsonTextReader.ReadType _readType;

		// Token: 0x040000D9 RID: 217
		private CultureInfo _culture;

		// Token: 0x02000028 RID: 40
		private enum ReadType
		{
			// Token: 0x040000DB RID: 219
			Read,
			// Token: 0x040000DC RID: 220
			ReadAsBytes,
			// Token: 0x040000DD RID: 221
			ReadAsDecimal,
			// Token: 0x040000DE RID: 222
			ReadAsDateTimeOffset
		}
	}
}
