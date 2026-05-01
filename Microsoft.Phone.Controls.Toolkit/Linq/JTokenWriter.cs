using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200006D RID: 109
	public class JTokenWriter : JsonWriter
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00016390 File Offset: 0x00014590
		public JToken Token
		{
			get
			{
				if (this._token != null)
				{
					return this._token;
				}
				return this._value;
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000163A7 File Offset: 0x000145A7
		public JTokenWriter(JContainer container)
		{
			ValidationUtils.ArgumentNotNull(container, "container");
			this._token = container;
			this._parent = container;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x000163C8 File Offset: 0x000145C8
		public JTokenWriter()
		{
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000163D0 File Offset: 0x000145D0
		public override void Flush()
		{
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x000163D2 File Offset: 0x000145D2
		public override void Close()
		{
			base.Close();
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x000163DA File Offset: 0x000145DA
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new JObject());
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x000163ED File Offset: 0x000145ED
		private void AddParent(JContainer container)
		{
			if (this._parent == null)
			{
				this._token = container;
			}
			else
			{
				this._parent.Add(container);
			}
			this._parent = container;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00016413 File Offset: 0x00014613
		private void RemoveParent()
		{
			this._parent = this._parent.Parent;
			if (this._parent != null && this._parent.Type == JTokenType.Property)
			{
				this._parent = this._parent.Parent;
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001644D File Offset: 0x0001464D
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new JArray());
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00016460 File Offset: 0x00014660
		public override void WriteStartConstructor(string name)
		{
			base.WriteStartConstructor(name);
			this.AddParent(new JConstructor(name));
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00016475 File Offset: 0x00014675
		protected override void WriteEnd(JsonToken token)
		{
			this.RemoveParent();
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001647D File Offset: 0x0001467D
		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			this.AddParent(new JProperty(name));
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00016492 File Offset: 0x00014692
		private void AddValue(object value, JsonToken token)
		{
			this.AddValue(new JValue(value), token);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x000164A1 File Offset: 0x000146A1
		internal void AddValue(JValue value, JsonToken token)
		{
			if (this._parent != null)
			{
				this._parent.Add(value);
				if (this._parent.Type == JTokenType.Property)
				{
					this._parent = this._parent.Parent;
					return;
				}
			}
			else
			{
				this._value = value;
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x000164DE File Offset: 0x000146DE
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddValue(null, JsonToken.Null);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x000164EF File Offset: 0x000146EF
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddValue(null, JsonToken.Undefined);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00016500 File Offset: 0x00014700
		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			this.AddValue(new JRaw(json), JsonToken.Raw);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00016516 File Offset: 0x00014716
		public override void WriteComment(string text)
		{
			base.WriteComment(text);
			this.AddValue(JValue.CreateComment(text), JsonToken.Comment);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001652C File Offset: 0x0001472C
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			this.AddValue(value ?? string.Empty, JsonToken.String);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00016547 File Offset: 0x00014747
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001655D File Offset: 0x0001475D
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00016573 File Offset: 0x00014773
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00016589 File Offset: 0x00014789
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001659F File Offset: 0x0001479F
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000165B5 File Offset: 0x000147B5
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x000165CB File Offset: 0x000147CB
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Boolean);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000165E2 File Offset: 0x000147E2
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x000165F8 File Offset: 0x000147F8
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001660E File Offset: 0x0001480E
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			this.AddValue(value.ToString(), JsonToken.String);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00016626 File Offset: 0x00014826
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001663C File Offset: 0x0001483C
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00016652 File Offset: 0x00014852
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00016668 File Offset: 0x00014868
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Date);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001667F File Offset: 0x0001487F
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Date);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00016696 File Offset: 0x00014896
		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Bytes);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x000166A8 File Offset: 0x000148A8
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x000166BF File Offset: 0x000148BF
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x000166D6 File Offset: 0x000148D6
		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x04000147 RID: 327
		private JContainer _token;

		// Token: 0x04000148 RID: 328
		private JContainer _parent;

		// Token: 0x04000149 RID: 329
		private JValue _value;
	}
}
