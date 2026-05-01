using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200006E RID: 110
	public class JTokenReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x06000540 RID: 1344 RVA: 0x000166E8 File Offset: 0x000148E8
		public JTokenReader(JToken token)
		{
			ValidationUtils.ArgumentNotNull(token, "token");
			this._root = token;
			this._current = token;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001670C File Offset: 0x0001490C
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
			if (this.TokenType == JsonToken.String)
			{
				string text = (string)this.Value;
				byte[] value = (text.Length == 0) ? new byte[0] : Convert.FromBase64String(text);
				this.SetToken(JsonToken.Bytes, value);
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

		// Token: 0x06000542 RID: 1346 RVA: 0x000167C4 File Offset: 0x000149C4
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

		// Token: 0x06000543 RID: 1347 RVA: 0x00016868 File Offset: 0x00014A68
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

		// Token: 0x06000544 RID: 1348 RVA: 0x000168FC File Offset: 0x00014AFC
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

		// Token: 0x06000545 RID: 1349 RVA: 0x00016988 File Offset: 0x00014B88
		public override bool Read()
		{
			if (base.CurrentState == JsonReader.State.Start)
			{
				this.SetToken(this._current);
				return true;
			}
			JContainer jcontainer = this._current as JContainer;
			if (jcontainer != null && this._parent != jcontainer)
			{
				return this.ReadInto(jcontainer);
			}
			return this.ReadOver(this._current);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x000169D8 File Offset: 0x00014BD8
		private bool ReadOver(JToken t)
		{
			if (t == this._root)
			{
				return this.ReadToEnd();
			}
			JToken next = t.Next;
			if (next != null && next != t && t != t.Parent.Last)
			{
				this._current = next;
				this.SetToken(this._current);
				return true;
			}
			if (t.Parent == null)
			{
				return this.ReadToEnd();
			}
			return this.SetEnd(t.Parent);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00016A41 File Offset: 0x00014C41
		private bool ReadToEnd()
		{
			return false;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x00016A44 File Offset: 0x00014C44
		private bool IsEndElement
		{
			get
			{
				return this._current == this._parent;
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00016A54 File Offset: 0x00014C54
		private JsonToken? GetEndToken(JContainer c)
		{
			switch (c.Type)
			{
			case JTokenType.Object:
				return new JsonToken?(JsonToken.EndObject);
			case JTokenType.Array:
				return new JsonToken?(JsonToken.EndArray);
			case JTokenType.Constructor:
				return new JsonToken?(JsonToken.EndConstructor);
			case JTokenType.Property:
				return default(JsonToken?);
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", c.Type, "Unexpected JContainer type.");
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00016AC0 File Offset: 0x00014CC0
		private bool ReadInto(JContainer c)
		{
			JToken first = c.First;
			if (first == null)
			{
				return this.SetEnd(c);
			}
			this.SetToken(first);
			this._current = first;
			this._parent = c;
			return true;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00016AF8 File Offset: 0x00014CF8
		private bool SetEnd(JContainer c)
		{
			JsonToken? endToken = this.GetEndToken(c);
			if (endToken != null)
			{
				base.SetToken(endToken.Value);
				this._current = c;
				this._parent = c;
				return true;
			}
			return this.ReadOver(c);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00016B3C File Offset: 0x00014D3C
		private void SetToken(JToken token)
		{
			switch (token.Type)
			{
			case JTokenType.Object:
				base.SetToken(JsonToken.StartObject);
				return;
			case JTokenType.Array:
				base.SetToken(JsonToken.StartArray);
				return;
			case JTokenType.Constructor:
				base.SetToken(JsonToken.StartConstructor);
				return;
			case JTokenType.Property:
				this.SetToken(JsonToken.PropertyName, ((JProperty)token).Name);
				return;
			case JTokenType.Comment:
				this.SetToken(JsonToken.Comment, ((JValue)token).Value);
				return;
			case JTokenType.Integer:
				this.SetToken(JsonToken.Integer, ((JValue)token).Value);
				return;
			case JTokenType.Float:
				this.SetToken(JsonToken.Float, ((JValue)token).Value);
				return;
			case JTokenType.String:
				this.SetToken(JsonToken.String, ((JValue)token).Value);
				return;
			case JTokenType.Boolean:
				this.SetToken(JsonToken.Boolean, ((JValue)token).Value);
				return;
			case JTokenType.Null:
				this.SetToken(JsonToken.Null, ((JValue)token).Value);
				return;
			case JTokenType.Undefined:
				this.SetToken(JsonToken.Undefined, ((JValue)token).Value);
				return;
			case JTokenType.Date:
				this.SetToken(JsonToken.Date, ((JValue)token).Value);
				return;
			case JTokenType.Raw:
				this.SetToken(JsonToken.Raw, ((JValue)token).Value);
				return;
			case JTokenType.Bytes:
				this.SetToken(JsonToken.Bytes, ((JValue)token).Value);
				return;
			case JTokenType.Guid:
				this.SetToken(JsonToken.String, this.SafeToString(((JValue)token).Value));
				return;
			case JTokenType.Uri:
				this.SetToken(JsonToken.String, this.SafeToString(((JValue)token).Value));
				return;
			case JTokenType.TimeSpan:
				this.SetToken(JsonToken.String, this.SafeToString(((JValue)token).Value));
				return;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", token.Type, "Unexpected JTokenType.");
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00016CF8 File Offset: 0x00014EF8
		private string SafeToString(object value)
		{
			if (value == null)
			{
				return null;
			}
			return value.ToString();
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00016D08 File Offset: 0x00014F08
		bool IJsonLineInfo.HasLineInfo()
		{
			if (base.CurrentState == JsonReader.State.Start)
			{
				return false;
			}
			IJsonLineInfo jsonLineInfo = this.IsEndElement ? null : this._current;
			return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x00016D3C File Offset: 0x00014F3C
		int IJsonLineInfo.LineNumber
		{
			get
			{
				if (base.CurrentState == JsonReader.State.Start)
				{
					return 0;
				}
				IJsonLineInfo jsonLineInfo = this.IsEndElement ? null : this._current;
				if (jsonLineInfo != null)
				{
					return jsonLineInfo.LineNumber;
				}
				return 0;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00016D70 File Offset: 0x00014F70
		int IJsonLineInfo.LinePosition
		{
			get
			{
				if (base.CurrentState == JsonReader.State.Start)
				{
					return 0;
				}
				IJsonLineInfo jsonLineInfo = this.IsEndElement ? null : this._current;
				if (jsonLineInfo != null)
				{
					return jsonLineInfo.LinePosition;
				}
				return 0;
			}
		}

		// Token: 0x0400014A RID: 330
		private readonly JToken _root;

		// Token: 0x0400014B RID: 331
		private JToken _parent;

		// Token: 0x0400014C RID: 332
		private JToken _current;
	}
}
