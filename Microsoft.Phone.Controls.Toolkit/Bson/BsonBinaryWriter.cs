using System;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000005 RID: 5
	internal class BsonBinaryWriter
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002241 File Offset: 0x00000441
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002249 File Offset: 0x00000449
		public DateTimeKind DateTimeKindHandling { get; set; }

		// Token: 0x0600000A RID: 10 RVA: 0x00002252 File Offset: 0x00000452
		public BsonBinaryWriter(Stream stream)
		{
			this.DateTimeKindHandling = 1;
			this._writer = new BinaryWriter(stream);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000226D File Offset: 0x0000046D
		public void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000227A File Offset: 0x0000047A
		public void Close()
		{
			this._writer.Close();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002287 File Offset: 0x00000487
		public void WriteToken(BsonToken t)
		{
			this.CalculateSize(t);
			this.WriteTokenInternal(t);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002298 File Offset: 0x00000498
		private void WriteTokenInternal(BsonToken t)
		{
			switch (t.Type)
			{
			case BsonType.Number:
			{
				BsonValue bsonValue = (BsonValue)t;
				this._writer.Write(Convert.ToDouble(bsonValue.Value, CultureInfo.InvariantCulture));
				return;
			}
			case BsonType.String:
			{
				BsonString bsonString = (BsonString)t;
				this.WriteString((string)bsonString.Value, bsonString.ByteCount, new int?(bsonString.CalculatedSize - 4));
				return;
			}
			case BsonType.Object:
			{
				BsonObject bsonObject = (BsonObject)t;
				this._writer.Write(bsonObject.CalculatedSize);
				foreach (BsonProperty bsonProperty in bsonObject)
				{
					this._writer.Write((sbyte)bsonProperty.Value.Type);
					this.WriteString((string)bsonProperty.Name.Value, bsonProperty.Name.ByteCount, default(int?));
					this.WriteTokenInternal(bsonProperty.Value);
				}
				this._writer.Write(0);
				return;
			}
			case BsonType.Array:
			{
				BsonArray bsonArray = (BsonArray)t;
				this._writer.Write(bsonArray.CalculatedSize);
				int num = 0;
				foreach (BsonToken bsonToken in bsonArray)
				{
					this._writer.Write((sbyte)bsonToken.Type);
					this.WriteString(num.ToString(CultureInfo.InvariantCulture), MathUtils.IntLength(num), default(int?));
					this.WriteTokenInternal(bsonToken);
					num++;
				}
				this._writer.Write(0);
				return;
			}
			case BsonType.Binary:
			{
				BsonValue bsonValue2 = (BsonValue)t;
				byte[] array = (byte[])bsonValue2.Value;
				this._writer.Write(array.Length);
				this._writer.Write(0);
				this._writer.Write(array);
				return;
			}
			case BsonType.Undefined:
			case BsonType.Null:
				return;
			case BsonType.Oid:
			{
				BsonValue bsonValue3 = (BsonValue)t;
				byte[] array2 = (byte[])bsonValue3.Value;
				this._writer.Write(array2);
				return;
			}
			case BsonType.Boolean:
			{
				BsonValue bsonValue4 = (BsonValue)t;
				this._writer.Write((bool)bsonValue4.Value);
				return;
			}
			case BsonType.Date:
			{
				BsonValue bsonValue5 = (BsonValue)t;
				long num2;
				if (bsonValue5.Value is DateTime)
				{
					DateTime dateTime = (DateTime)bsonValue5.Value;
					if (this.DateTimeKindHandling == 1)
					{
						dateTime = dateTime.ToUniversalTime();
					}
					else if (this.DateTimeKindHandling == 2)
					{
						dateTime = dateTime.ToLocalTime();
					}
					num2 = JsonConvert.ConvertDateTimeToJavaScriptTicks(dateTime, false);
				}
				else
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)bsonValue5.Value;
					num2 = JsonConvert.ConvertDateTimeToJavaScriptTicks(dateTimeOffset.UtcDateTime, dateTimeOffset.Offset);
				}
				this._writer.Write(num2);
				return;
			}
			case BsonType.Regex:
			{
				BsonRegex bsonRegex = (BsonRegex)t;
				this.WriteString((string)bsonRegex.Pattern.Value, bsonRegex.Pattern.ByteCount, default(int?));
				this.WriteString((string)bsonRegex.Options.Value, bsonRegex.Options.ByteCount, default(int?));
				return;
			}
			case BsonType.Integer:
			{
				BsonValue bsonValue6 = (BsonValue)t;
				this._writer.Write(Convert.ToInt32(bsonValue6.Value, CultureInfo.InvariantCulture));
				return;
			}
			case BsonType.Long:
			{
				BsonValue bsonValue7 = (BsonValue)t;
				this._writer.Write(Convert.ToInt64(bsonValue7.Value, CultureInfo.InvariantCulture));
				return;
			}
			}
			throw new ArgumentOutOfRangeException("t", "Unexpected token when writing BSON: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				t.Type
			}));
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002690 File Offset: 0x00000890
		private void WriteString(string s, int byteCount, int? calculatedlengthPrefix)
		{
			if (calculatedlengthPrefix != null)
			{
				this._writer.Write(calculatedlengthPrefix.Value);
			}
			this.WriteUtf8Bytes(s, byteCount);
			this._writer.Write(0);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000026C4 File Offset: 0x000008C4
		public void WriteUtf8Bytes(string s, int byteCount)
		{
			if (s != null)
			{
				if (this._largeByteBuffer == null)
				{
					this._largeByteBuffer = new byte[256];
					this._maxChars = 256 / BsonBinaryWriter.Encoding.GetMaxByteCount(1);
				}
				if (byteCount <= 256)
				{
					BsonBinaryWriter.Encoding.GetBytes(s, 0, s.Length, this._largeByteBuffer, 0);
					this._writer.Write(this._largeByteBuffer, 0, byteCount);
					return;
				}
				byte[] bytes = BsonBinaryWriter.Encoding.GetBytes(s);
				this._writer.Write(bytes);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002751 File Offset: 0x00000951
		private int CalculateSize(int stringByteCount)
		{
			return stringByteCount + 1;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002758 File Offset: 0x00000958
		private int CalculateSizeWithLength(int stringByteCount, bool includeSize)
		{
			int num = includeSize ? 5 : 1;
			return num + stringByteCount;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002770 File Offset: 0x00000970
		private int CalculateSize(BsonToken t)
		{
			switch (t.Type)
			{
			case BsonType.Number:
				return 8;
			case BsonType.String:
			{
				BsonString bsonString = (BsonString)t;
				string text = (string)bsonString.Value;
				bsonString.ByteCount = ((text != null) ? BsonBinaryWriter.Encoding.GetByteCount(text) : 0);
				bsonString.CalculatedSize = this.CalculateSizeWithLength(bsonString.ByteCount, bsonString.IncludeLength);
				return bsonString.CalculatedSize;
			}
			case BsonType.Object:
			{
				BsonObject bsonObject = (BsonObject)t;
				int num = 4;
				foreach (BsonProperty bsonProperty in bsonObject)
				{
					int num2 = 1;
					num2 += this.CalculateSize(bsonProperty.Name);
					num2 += this.CalculateSize(bsonProperty.Value);
					num += num2;
				}
				num++;
				bsonObject.CalculatedSize = num;
				return num;
			}
			case BsonType.Array:
			{
				BsonArray bsonArray = (BsonArray)t;
				int num3 = 4;
				int num4 = 0;
				foreach (BsonToken t2 in bsonArray)
				{
					num3++;
					num3 += this.CalculateSize(MathUtils.IntLength(num4));
					num3 += this.CalculateSize(t2);
					num4++;
				}
				num3++;
				bsonArray.CalculatedSize = num3;
				return bsonArray.CalculatedSize;
			}
			case BsonType.Binary:
			{
				BsonValue bsonValue = (BsonValue)t;
				byte[] array = (byte[])bsonValue.Value;
				bsonValue.CalculatedSize = 5 + array.Length;
				return bsonValue.CalculatedSize;
			}
			case BsonType.Undefined:
			case BsonType.Null:
				return 0;
			case BsonType.Oid:
				return 12;
			case BsonType.Boolean:
				return 1;
			case BsonType.Date:
				return 8;
			case BsonType.Regex:
			{
				BsonRegex bsonRegex = (BsonRegex)t;
				int num5 = 0;
				num5 += this.CalculateSize(bsonRegex.Pattern);
				num5 += this.CalculateSize(bsonRegex.Options);
				bsonRegex.CalculatedSize = num5;
				return bsonRegex.CalculatedSize;
			}
			case BsonType.Integer:
				return 4;
			case BsonType.Long:
				return 8;
			}
			throw new ArgumentOutOfRangeException("t", "Unexpected token when writing BSON: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				t.Type
			}));
		}

		// Token: 0x0400001B RID: 27
		private static readonly Encoding Encoding = new UTF8Encoding(false);

		// Token: 0x0400001C RID: 28
		private readonly BinaryWriter _writer;

		// Token: 0x0400001D RID: 29
		private byte[] _largeByteBuffer;

		// Token: 0x0400001E RID: 30
		private int _maxChars;
	}
}
