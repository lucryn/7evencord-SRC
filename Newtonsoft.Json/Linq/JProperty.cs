using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000076 RID: 118
	public class JProperty : JContainer
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x000178C5 File Offset: 0x00015AC5
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x000178CD File Offset: 0x00015ACD
		public string Name
		{
			[DebuggerStepThrough]
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x000178D5 File Offset: 0x00015AD5
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x000178F4 File Offset: 0x00015AF4
		public new JToken Value
		{
			[DebuggerStepThrough]
			get
			{
				if (this.ChildrenTokens.Count <= 0)
				{
					return null;
				}
				return this.ChildrenTokens[0];
			}
			set
			{
				base.CheckReentrancy();
				JToken item = value ?? new JValue(null);
				if (this.ChildrenTokens.Count == 0)
				{
					this.InsertItem(0, item);
					return;
				}
				this.SetItem(0, item);
			}
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00017931 File Offset: 0x00015B31
		public JProperty(JProperty other) : base(other)
		{
			this._name = other.Name;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00017951 File Offset: 0x00015B51
		internal override JToken GetItem(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.Value;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00017964 File Offset: 0x00015B64
		internal override void SetItem(int index, JToken item)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (JContainer.IsTokenUnchanged(this.Value, item))
			{
				return;
			}
			if (base.Parent != null)
			{
				((JObject)base.Parent).InternalPropertyChanging(this);
			}
			base.SetItem(0, item);
			if (base.Parent != null)
			{
				((JObject)base.Parent).InternalPropertyChanged(this);
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x000179C4 File Offset: 0x00015BC4
		internal override bool RemoveItem(JToken item)
		{
			throw new Exception("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeof(JProperty)
			}));
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000179FC File Offset: 0x00015BFC
		internal override void RemoveItemAt(int index)
		{
			throw new Exception("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeof(JProperty)
			}));
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00017A34 File Offset: 0x00015C34
		internal override void InsertItem(int index, JToken item)
		{
			if (this.Value != null)
			{
				throw new Exception("{0} cannot have multiple values.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					typeof(JProperty)
				}));
			}
			base.InsertItem(0, item);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00017A7B File Offset: 0x00015C7B
		internal override bool ContainsItem(JToken item)
		{
			return this.Value == item;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00017A88 File Offset: 0x00015C88
		internal override void ClearItems()
		{
			throw new Exception("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeof(JProperty)
			}));
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00017AC0 File Offset: 0x00015CC0
		internal override bool DeepEquals(JToken node)
		{
			JProperty jproperty = node as JProperty;
			return jproperty != null && this._name == jproperty.Name && base.ContentsEqual(jproperty);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00017AF3 File Offset: 0x00015CF3
		internal override JToken CloneToken()
		{
			return new JProperty(this);
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00017AFB File Offset: 0x00015CFB
		public override JTokenType Type
		{
			[DebuggerStepThrough]
			get
			{
				return JTokenType.Property;
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00017AFE File Offset: 0x00015CFE
		internal JProperty(string name)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00017B23 File Offset: 0x00015D23
		public JProperty(string name, params object[] content) : this(name, content)
		{
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00017B30 File Offset: 0x00015D30
		public JProperty(string name, object content)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
			this.Value = (base.IsMultiContent(content) ? new JArray(content) : base.CreateFromContent(content));
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00017B7E File Offset: 0x00015D7E
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WritePropertyName(this._name);
			this.Value.WriteTo(writer, converters);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00017B99 File Offset: 0x00015D99
		internal override int GetDeepHashCode()
		{
			return this._name.GetHashCode() ^ ((this.Value != null) ? this.Value.GetDeepHashCode() : 0);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00017BC0 File Offset: 0x00015DC0
		public new static JProperty Load(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw new Exception("Error reading JProperty from JsonReader.");
			}
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw new Exception("Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					reader.TokenType
				}));
			}
			JProperty jproperty = new JProperty((string)reader.Value);
			jproperty.SetLineInfo(reader as IJsonLineInfo);
			jproperty.ReadTokenFrom(reader);
			return jproperty;
		}

		// Token: 0x0400018D RID: 397
		private readonly List<JToken> _content = new List<JToken>();

		// Token: 0x0400018E RID: 398
		private readonly string _name;
	}
}
