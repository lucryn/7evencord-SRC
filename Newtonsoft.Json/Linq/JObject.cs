using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000022 RID: 34
	public class JObject : JContainer, IDictionary<string, JToken>, ICollection<KeyValuePair<string, JToken>>, IEnumerable<KeyValuePair<string, JToken>>, IEnumerable, INotifyPropertyChanged
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000A6A2 File Offset: 0x000088A2
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._properties;
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000231 RID: 561 RVA: 0x0000A6AC File Offset: 0x000088AC
		// (remove) Token: 0x06000232 RID: 562 RVA: 0x0000A6E4 File Offset: 0x000088E4
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000233 RID: 563 RVA: 0x0000A719 File Offset: 0x00008919
		public JObject()
		{
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000A731 File Offset: 0x00008931
		public JObject(JObject other) : base(other)
		{
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000A74A File Offset: 0x0000894A
		public JObject(params object[] content) : this(content)
		{
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000A753 File Offset: 0x00008953
		public JObject(object content)
		{
			this.Add(content);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000A774 File Offset: 0x00008974
		internal override bool DeepEquals(JToken node)
		{
			JObject jobject = node as JObject;
			return jobject != null && base.ContentsEqual(jobject);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000A794 File Offset: 0x00008994
		internal override void InsertItem(int index, JToken item)
		{
			if (item != null && item.Type == JTokenType.Comment)
			{
				return;
			}
			base.InsertItem(index, item);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000A7AC File Offset: 0x000089AC
		internal override void ValidateToken(JToken o, JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type != JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					o.GetType(),
					base.GetType()
				}));
			}
			JProperty jproperty = (JProperty)o;
			if (existing != null)
			{
				JProperty jproperty2 = (JProperty)existing;
				if (jproperty.Name == jproperty2.Name)
				{
					return;
				}
			}
			if (this._properties.Dictionary != null && this._properties.Dictionary.TryGetValue(jproperty.Name, ref existing))
			{
				throw new ArgumentException("Can not add property {0} to {1}. Property with the same name already exists on object.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					jproperty.Name,
					base.GetType()
				}));
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000A877 File Offset: 0x00008A77
		internal void InternalPropertyChanged(JProperty childProperty)
		{
			this.OnPropertyChanged(childProperty.Name);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(2, childProperty, childProperty, base.IndexOfItem(childProperty)));
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000A89A File Offset: 0x00008A9A
		internal void InternalPropertyChanging(JProperty childProperty)
		{
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000A89C File Offset: 0x00008A9C
		internal override JToken CloneToken()
		{
			return new JObject(this);
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000A8A4 File Offset: 0x00008AA4
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Object;
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000A8A7 File Offset: 0x00008AA7
		public IEnumerable<JProperty> Properties()
		{
			return Enumerable.Cast<JProperty>(this.ChildrenTokens);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000A8B4 File Offset: 0x00008AB4
		public JProperty Property(string name)
		{
			if (this._properties.Dictionary == null)
			{
				return null;
			}
			if (name == null)
			{
				return null;
			}
			JToken jtoken;
			this._properties.Dictionary.TryGetValue(name, ref jtoken);
			return (JProperty)jtoken;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000A8F7 File Offset: 0x00008AF7
		public JEnumerable<JToken> PropertyValues()
		{
			return new JEnumerable<JToken>(Enumerable.Select<JProperty, JToken>(this.Properties(), (JProperty p) => p.Value));
		}

		// Token: 0x17000076 RID: 118
		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				string text = key as string;
				if (text == null)
				{
					throw new ArgumentException("Accessed JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				return this[text];
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				string text = key as string;
				if (text == null)
				{
					throw new ArgumentException("Set JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				this[text] = value;
			}
		}

		// Token: 0x17000077 RID: 119
		public JToken this[string propertyName]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(propertyName, "propertyName");
				JProperty jproperty = this.Property(propertyName);
				if (jproperty == null)
				{
					return null;
				}
				return jproperty.Value;
			}
			set
			{
				JProperty jproperty = this.Property(propertyName);
				if (jproperty != null)
				{
					jproperty.Value = value;
					return;
				}
				this.Add(new JProperty(propertyName, value));
				this.OnPropertyChanged(propertyName);
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000AA28 File Offset: 0x00008C28
		public new static JObject Load(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw new Exception("Error reading JObject from JsonReader.");
			}
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw new Exception("Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					reader.TokenType
				}));
			}
			JObject jobject = new JObject();
			jobject.SetLineInfo(reader as IJsonLineInfo);
			jobject.ReadTokenFrom(reader);
			return jobject;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000AAAC File Offset: 0x00008CAC
		public new static JObject Parse(string json)
		{
			JsonReader jsonReader = new JsonTextReader(new StringReader(json));
			JObject result = JObject.Load(jsonReader);
			if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
			{
				throw new Exception("Additional text found in JSON string after parsing content.");
			}
			return result;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000AAE9 File Offset: 0x00008CE9
		public new static JObject FromObject(object o)
		{
			return JObject.FromObject(o, new JsonSerializer());
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000AAF8 File Offset: 0x00008CF8
		public new static JObject FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jtoken != null && jtoken.Type != JTokenType.Object)
			{
				throw new ArgumentException("Object serialized to {0}. JObject instance expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					jtoken.Type
				}));
			}
			return (JObject)jtoken;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000AB4C File Offset: 0x00008D4C
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartObject();
			foreach (JToken jtoken in this.ChildrenTokens)
			{
				JProperty jproperty = (JProperty)jtoken;
				jproperty.WriteTo(writer, converters);
			}
			writer.WriteEndObject();
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000ABAC File Offset: 0x00008DAC
		public void Add(string propertyName, JToken value)
		{
			this.Add(new JProperty(propertyName, value));
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000ABBB File Offset: 0x00008DBB
		bool IDictionary<string, JToken>.ContainsKey(string key)
		{
			return this._properties.Dictionary != null && this._properties.Dictionary.ContainsKey(key);
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000ABDD File Offset: 0x00008DDD
		ICollection<string> IDictionary<string, JToken>.Keys
		{
			get
			{
				return this._properties.Dictionary.Keys;
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000ABF0 File Offset: 0x00008DF0
		public bool Remove(string propertyName)
		{
			JProperty jproperty = this.Property(propertyName);
			if (jproperty == null)
			{
				return false;
			}
			jproperty.Remove();
			return true;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000AC14 File Offset: 0x00008E14
		public bool TryGetValue(string propertyName, out JToken value)
		{
			JProperty jproperty = this.Property(propertyName);
			if (jproperty == null)
			{
				value = null;
				return false;
			}
			value = jproperty.Value;
			return true;
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000AC3A File Offset: 0x00008E3A
		ICollection<JToken> IDictionary<string, JToken>.Values
		{
			get
			{
				return this._properties.Dictionary.Values;
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000AC4C File Offset: 0x00008E4C
		void ICollection<KeyValuePair<string, JToken>>.Add(KeyValuePair<string, JToken> item)
		{
			this.Add(new JProperty(item.Key, item.Value));
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000AC67 File Offset: 0x00008E67
		void ICollection<KeyValuePair<string, JToken>>.Clear()
		{
			base.RemoveAll();
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000AC70 File Offset: 0x00008E70
		bool ICollection<KeyValuePair<string, JToken>>.Contains(KeyValuePair<string, JToken> item)
		{
			JProperty jproperty = this.Property(item.Key);
			return jproperty != null && jproperty.Value == item.Value;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000ACA0 File Offset: 0x00008EA0
		void ICollection<KeyValuePair<string, JToken>>.CopyTo(KeyValuePair<string, JToken>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
			}
			if (arrayIndex >= array.Length)
			{
				throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
			}
			if (base.Count > array.Length - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			foreach (JToken jtoken in this.ChildrenTokens)
			{
				JProperty jproperty = (JProperty)jtoken;
				array[arrayIndex + num] = new KeyValuePair<string, JToken>(jproperty.Name, jproperty.Value);
				num++;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000AD5C File Offset: 0x00008F5C
		bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000AD5F File Offset: 0x00008F5F
		bool ICollection<KeyValuePair<string, JToken>>.Remove(KeyValuePair<string, JToken> item)
		{
			if (!this.Contains(item))
			{
				return false;
			}
			this.Remove(item.Key);
			return true;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000AD7B File Offset: 0x00008F7B
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000AED8 File Offset: 0x000090D8
		public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
		{
			foreach (JToken jtoken in this.ChildrenTokens)
			{
				JProperty property = (JProperty)jtoken;
				yield return new KeyValuePair<string, JToken>(property.Name, property.Value);
			}
			yield break;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000AEF4 File Offset: 0x000090F4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x040000B5 RID: 181
		private JObject.JPropertKeyedCollection _properties = new JObject.JPropertKeyedCollection(StringComparer.Ordinal);

		// Token: 0x02000023 RID: 35
		private class JPropertKeyedCollection : KeyedCollection<string, JToken>
		{
			// Token: 0x0600025A RID: 602 RVA: 0x0000AF10 File Offset: 0x00009110
			public JPropertKeyedCollection(IEqualityComparer<string> comparer) : base(comparer)
			{
			}

			// Token: 0x0600025B RID: 603 RVA: 0x0000AF19 File Offset: 0x00009119
			protected override string GetKeyForItem(JToken item)
			{
				return ((JProperty)item).Name;
			}

			// Token: 0x0600025C RID: 604 RVA: 0x0000AF28 File Offset: 0x00009128
			protected override void InsertItem(int index, JToken item)
			{
				if (this.Dictionary == null)
				{
					base.InsertItem(index, item);
					return;
				}
				string keyForItem = this.GetKeyForItem(item);
				this.Dictionary[keyForItem] = item;
				base.Items.Insert(index, item);
			}

			// Token: 0x1700007B RID: 123
			// (get) Token: 0x0600025D RID: 605 RVA: 0x0000AF68 File Offset: 0x00009168
			public IDictionary<string, JToken> Dictionary
			{
				get
				{
					return base.Dictionary;
				}
			}
		}
	}
}
