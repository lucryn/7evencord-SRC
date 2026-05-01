using System;
using System.IO;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000026 RID: 38
	public class JsonTextWriter : JsonWriter
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000BFB4 File Offset: 0x0000A1B4
		private Base64Encoder Base64Encoder
		{
			get
			{
				if (this._base64Encoder == null)
				{
					this._base64Encoder = new Base64Encoder(this._writer);
				}
				return this._base64Encoder;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000BFD5 File Offset: 0x0000A1D5
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0000BFDD File Offset: 0x0000A1DD
		public int Indentation
		{
			get
			{
				return this._indentation;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Indentation value must be greater than 0.");
				}
				this._indentation = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000BFF5 File Offset: 0x0000A1F5
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0000BFFD File Offset: 0x0000A1FD
		public char QuoteChar
		{
			get
			{
				return this._quoteChar;
			}
			set
			{
				if (value != '"' && value != '\'')
				{
					throw new ArgumentException("Invalid JavaScript string quote character. Valid quote characters are ' and \".");
				}
				this._quoteChar = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000C01B File Offset: 0x0000A21B
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x0000C023 File Offset: 0x0000A223
		public char IndentChar
		{
			get
			{
				return this._indentChar;
			}
			set
			{
				this._indentChar = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000C02C File Offset: 0x0000A22C
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0000C034 File Offset: 0x0000A234
		public bool QuoteName
		{
			get
			{
				return this._quoteName;
			}
			set
			{
				this._quoteName = value;
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000C03D File Offset: 0x0000A23D
		public JsonTextWriter(TextWriter textWriter)
		{
			if (textWriter == null)
			{
				throw new ArgumentNullException("textWriter");
			}
			this._writer = textWriter;
			this._quoteChar = '"';
			this._quoteName = true;
			this._indentChar = ' ';
			this._indentation = 2;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000C078 File Offset: 0x0000A278
		public override void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000C085 File Offset: 0x0000A285
		public override void Close()
		{
			base.Close();
			if (base.CloseOutput && this._writer != null)
			{
				this._writer.Close();
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000C0A8 File Offset: 0x0000A2A8
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this._writer.Write("{");
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000C0C0 File Offset: 0x0000A2C0
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this._writer.Write("[");
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000C0D8 File Offset: 0x0000A2D8
		public override void WriteStartConstructor(string name)
		{
			base.WriteStartConstructor(name);
			this._writer.Write("new ");
			this._writer.Write(name);
			this._writer.Write("(");
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000C110 File Offset: 0x0000A310
		protected override void WriteEnd(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
				this._writer.Write("}");
				return;
			case JsonToken.EndArray:
				this._writer.Write("]");
				return;
			case JsonToken.EndConstructor:
				this._writer.Write(")");
				return;
			default:
				throw new JsonWriterException("Invalid JsonToken: " + token);
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000C17E File Offset: 0x0000A37E
		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			JavaScriptUtils.WriteEscapedJavaScriptString(this._writer, name, this._quoteChar, this._quoteName);
			this._writer.Write(':');
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000C1AC File Offset: 0x0000A3AC
		protected override void WriteIndent()
		{
			if (base.Formatting == Formatting.Indented)
			{
				this._writer.Write(Environment.NewLine);
				int num = base.Top * this._indentation;
				for (int i = 0; i < num; i++)
				{
					this._writer.Write(this._indentChar);
				}
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000C1FD File Offset: 0x0000A3FD
		protected override void WriteValueDelimiter()
		{
			this._writer.Write(',');
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000C20C File Offset: 0x0000A40C
		protected override void WriteIndentSpace()
		{
			this._writer.Write(' ');
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000C21B File Offset: 0x0000A41B
		private void WriteValueInternal(string value, JsonToken token)
		{
			this._writer.Write(value);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000C229 File Offset: 0x0000A429
		public override void WriteNull()
		{
			base.WriteNull();
			this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000C23E File Offset: 0x0000A43E
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.WriteValueInternal(JsonConvert.Undefined, JsonToken.Undefined);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000C253 File Offset: 0x0000A453
		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			this._writer.Write(json);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000C268 File Offset: 0x0000A468
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			if (value == null)
			{
				this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
				return;
			}
			JavaScriptUtils.WriteEscapedJavaScriptString(this._writer, value, this._quoteChar, true);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000C295 File Offset: 0x0000A495
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000C2AB File Offset: 0x0000A4AB
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000C2C1 File Offset: 0x0000A4C1
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000C2D7 File Offset: 0x0000A4D7
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000C2ED File Offset: 0x0000A4ED
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000C303 File Offset: 0x0000A503
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000C319 File Offset: 0x0000A519
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Boolean);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000C330 File Offset: 0x0000A530
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000C346 File Offset: 0x0000A546
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000C35C File Offset: 0x0000A55C
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000C372 File Offset: 0x0000A572
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000C388 File Offset: 0x0000A588
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000C39E File Offset: 0x0000A59E
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000C3B4 File Offset: 0x0000A5B4
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			JsonConvert.WriteDateTimeString(this._writer, value);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000C3CC File Offset: 0x0000A5CC
		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			if (value != null)
			{
				this._writer.Write(this._quoteChar);
				this.Base64Encoder.Encode(value, 0, value.Length);
				this.Base64Encoder.Flush();
				this._writer.Write(this._quoteChar);
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000C420 File Offset: 0x0000A620
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Date);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000C437 File Offset: 0x0000A637
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000C44E File Offset: 0x0000A64E
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000C465 File Offset: 0x0000A665
		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000C47C File Offset: 0x0000A67C
		public override void WriteComment(string text)
		{
			base.WriteComment(text);
			this._writer.Write("/*");
			this._writer.Write(text);
			this._writer.Write("*/");
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000C4B1 File Offset: 0x0000A6B1
		public override void WriteWhitespace(string ws)
		{
			base.WriteWhitespace(ws);
			this._writer.Write(ws);
		}

		// Token: 0x040000CA RID: 202
		private readonly TextWriter _writer;

		// Token: 0x040000CB RID: 203
		private Base64Encoder _base64Encoder;

		// Token: 0x040000CC RID: 204
		private char _indentChar;

		// Token: 0x040000CD RID: 205
		private int _indentation;

		// Token: 0x040000CE RID: 206
		private char _quoteChar;

		// Token: 0x040000CF RID: 207
		private bool _quoteName;
	}
}
