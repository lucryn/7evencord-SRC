using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x020000A2 RID: 162
	public class BsonWriter : JsonWriter
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x0001CD8C File Offset: 0x0001AF8C
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x0001CD99 File Offset: 0x0001AF99
		public DateTimeKind DateTimeKindHandling
		{
			get
			{
				return this._writer.DateTimeKindHandling;
			}
			set
			{
				this._writer.DateTimeKindHandling = value;
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0001CDA7 File Offset: 0x0001AFA7
		public BsonWriter(Stream stream)
		{
			ValidationUtils.ArgumentNotNull(stream, "stream");
			this._writer = new BsonBinaryWriter(stream);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0001CDC6 File Offset: 0x0001AFC6
		public override void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0001CDD3 File Offset: 0x0001AFD3
		protected override void WriteEnd(JsonToken token)
		{
			base.WriteEnd(token);
			this.RemoveParent();
			if (base.Top == 0)
			{
				this._writer.WriteToken(this._root);
			}
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0001CDFB File Offset: 0x0001AFFB
		public override void WriteComment(string text)
		{
			throw new JsonWriterException("Cannot write JSON comment as BSON.");
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0001CE07 File Offset: 0x0001B007
		public override void WriteStartConstructor(string name)
		{
			throw new JsonWriterException("Cannot write JSON constructor as BSON.");
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0001CE13 File Offset: 0x0001B013
		public override void WriteRaw(string json)
		{
			throw new JsonWriterException("Cannot write raw JSON as BSON.");
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0001CE1F File Offset: 0x0001B01F
		public override void WriteRawValue(string json)
		{
			throw new JsonWriterException("Cannot write raw JSON as BSON.");
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0001CE2B File Offset: 0x0001B02B
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new BsonArray());
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0001CE3E File Offset: 0x0001B03E
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new BsonObject());
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0001CE51 File Offset: 0x0001B051
		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			this._propertyName = name;
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0001CE61 File Offset: 0x0001B061
		public override void Close()
		{
			base.Close();
			if (base.CloseOutput && this._writer != null)
			{
				this._writer.Close();
			}
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0001CE84 File Offset: 0x0001B084
		private void AddParent(BsonToken container)
		{
			this.AddToken(container);
			this._parent = container;
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0001CE94 File Offset: 0x0001B094
		private void RemoveParent()
		{
			this._parent = this._parent.Parent;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0001CEA7 File Offset: 0x0001B0A7
		private void AddValue(object value, BsonType type)
		{
			this.AddToken(new BsonValue(value, type));
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0001CEB8 File Offset: 0x0001B0B8
		internal void AddToken(BsonToken token)
		{
			if (this._parent != null)
			{
				if (this._parent is BsonObject)
				{
					((BsonObject)this._parent).Add(this._propertyName, token);
					this._propertyName = null;
					return;
				}
				((BsonArray)this._parent).Add(token);
				return;
			}
			else
			{
				if (token.Type != BsonType.Object && token.Type != BsonType.Array)
				{
					throw new JsonWriterException("Error writing {0} value. BSON must start with an Object or Array.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						token.Type
					}));
				}
				this._parent = token;
				this._root = token;
				return;
			}
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0001CF56 File Offset: 0x0001B156
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddValue(null, BsonType.Null);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0001CF67 File Offset: 0x0001B167
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddValue(null, BsonType.Undefined);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0001CF77 File Offset: 0x0001B177
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			if (value == null)
			{
				this.AddValue(null, BsonType.Null);
				return;
			}
			this.AddToken(new BsonString(value, true));
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0001CF9A File Offset: 0x0001B19A
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0001CFB1 File Offset: 0x0001B1B1
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			if (value > 2147483647U)
			{
				throw new JsonWriterException("Value is too large to fit in a signed 32 bit integer. BSON does not support unsigned values.");
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0001CFDB File Offset: 0x0001B1DB
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0001CFF2 File Offset: 0x0001B1F2
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			if (value > 9223372036854775807UL)
			{
				throw new JsonWriterException("Value is too large to fit in a signed 64 bit integer. BSON does not support unsigned values.");
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0001D020 File Offset: 0x0001B220
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0001D036 File Offset: 0x0001B236
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0001D04C File Offset: 0x0001B24C
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Boolean);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001D062 File Offset: 0x0001B262
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0001D079 File Offset: 0x0001B279
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001D090 File Offset: 0x0001B290
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0001D0AC File Offset: 0x0001B2AC
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0001D0C3 File Offset: 0x0001B2C3
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0001D0DA File Offset: 0x0001B2DA
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0001D0F0 File Offset: 0x0001B2F0
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0001D107 File Offset: 0x0001B307
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0001D11E File Offset: 0x0001B31E
		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Binary);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0001D12F File Offset: 0x0001B32F
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0001D151 File Offset: 0x0001B351
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0001D173 File Offset: 0x0001B373
		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0001D18E File Offset: 0x0001B38E
		public void WriteObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw new Exception("An object id must be 12 bytes");
			}
			base.AutoComplete(JsonToken.Undefined);
			this.AddValue(value, BsonType.Oid);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001D1BD File Offset: 0x0001B3BD
		public void WriteRegex(string pattern, string options)
		{
			ValidationUtils.ArgumentNotNull(pattern, "pattern");
			base.AutoComplete(JsonToken.Undefined);
			this.AddToken(new BsonRegex(pattern, options));
		}

		// Token: 0x04000231 RID: 561
		private readonly BsonBinaryWriter _writer;

		// Token: 0x04000232 RID: 562
		private BsonToken _root;

		// Token: 0x04000233 RID: 563
		private BsonToken _parent;

		// Token: 0x04000234 RID: 564
		private string _propertyName;
	}
}
