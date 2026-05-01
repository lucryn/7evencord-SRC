using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000058 RID: 88
	public class JArray : JContainer, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x00011742 File Offset: 0x0000F942
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x0001174A File Offset: 0x0000F94A
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Array;
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0001174D File Offset: 0x0000F94D
		public JArray()
		{
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00011760 File Offset: 0x0000F960
		public JArray(JArray other) : base(other)
		{
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00011774 File Offset: 0x0000F974
		public JArray(params object[] content) : this(content)
		{
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0001177D File Offset: 0x0000F97D
		public JArray(object content)
		{
			this.Add(content);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00011798 File Offset: 0x0000F998
		internal override bool DeepEquals(JToken node)
		{
			JArray jarray = node as JArray;
			return jarray != null && base.ContentsEqual(jarray);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000117B8 File Offset: 0x0000F9B8
		internal override JToken CloneToken()
		{
			return new JArray(this);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000117C0 File Offset: 0x0000F9C0
		public new static JArray Load(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw new Exception("Error reading JArray from JsonReader.");
			}
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw new Exception("Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					reader.TokenType
				}));
			}
			JArray jarray = new JArray();
			jarray.SetLineInfo(reader as IJsonLineInfo);
			jarray.ReadTokenFrom(reader);
			return jarray;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00011838 File Offset: 0x0000FA38
		public new static JArray Parse(string json)
		{
			JsonReader jsonReader = new JsonTextReader(new StringReader(json));
			JArray result = JArray.Load(jsonReader);
			if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
			{
				throw new Exception("Additional text found in JSON string after parsing content.");
			}
			return result;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00011875 File Offset: 0x0000FA75
		public new static JArray FromObject(object o)
		{
			return JArray.FromObject(o, new JsonSerializer());
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00011884 File Offset: 0x0000FA84
		public new static JArray FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jtoken.Type != JTokenType.Array)
			{
				throw new ArgumentException("Object serialized to {0}. JArray instance expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					jtoken.Type
				}));
			}
			return (JArray)jtoken;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x000118D4 File Offset: 0x0000FAD4
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartArray();
			foreach (JToken jtoken in this.ChildrenTokens)
			{
				jtoken.WriteTo(writer, converters);
			}
			writer.WriteEndArray();
		}

		// Token: 0x170000D2 RID: 210
		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JArray values with invalid key value: {0}. Array position index expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				return this.GetItem((int)key);
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Set JArray values with invalid key value: {0}. Array position index expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				this.SetItem((int)key, value);
			}
		}

		// Token: 0x170000D3 RID: 211
		public JToken this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, value);
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000119EA File Offset: 0x0000FBEA
		public int IndexOf(JToken item)
		{
			return base.IndexOfItem(item);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x000119F3 File Offset: 0x0000FBF3
		public void Insert(int index, JToken item)
		{
			this.InsertItem(index, item);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000119FD File Offset: 0x0000FBFD
		public void RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00011A06 File Offset: 0x0000FC06
		public void Add(JToken item)
		{
			this.Add(item);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00011A0F File Offset: 0x0000FC0F
		public void Clear()
		{
			this.ClearItems();
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00011A17 File Offset: 0x0000FC17
		public bool Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00011A20 File Offset: 0x0000FC20
		void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00011A2A File Offset: 0x0000FC2A
		bool ICollection<JToken>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00011A2D File Offset: 0x0000FC2D
		public bool Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00011A36 File Offset: 0x0000FC36
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		// Token: 0x0400011F RID: 287
		private IList<JToken> _values = new List<JToken>();
	}
}
