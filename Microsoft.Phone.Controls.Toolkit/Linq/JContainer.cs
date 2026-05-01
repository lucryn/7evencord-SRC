using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000020 RID: 32
	public abstract class JContainer : JToken, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IList, ICollection, IEnumerable, INotifyCollectionChanged
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060001EB RID: 491 RVA: 0x00009794 File Offset: 0x00007994
		// (remove) Token: 0x060001EC RID: 492 RVA: 0x000097CC File Offset: 0x000079CC
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001ED RID: 493
		protected abstract IList<JToken> ChildrenTokens { get; }

		// Token: 0x060001EE RID: 494 RVA: 0x00009801 File Offset: 0x00007A01
		internal JContainer()
		{
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000980C File Offset: 0x00007A0C
		internal JContainer(JContainer other)
		{
			ValidationUtils.ArgumentNotNull(other, "c");
			foreach (JToken content in other)
			{
				this.Add(content);
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00009868 File Offset: 0x00007A68
		internal void CheckReentrancy()
		{
			if (this._busy)
			{
				throw new InvalidOperationException("Cannot change {0} during a collection change event.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.GetType()
				}));
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000098A4 File Offset: 0x00007AA4
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
			if (collectionChanged != null)
			{
				this._busy = true;
				try
				{
					collectionChanged.Invoke(this, e);
				}
				finally
				{
					this._busy = false;
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x000098E4 File Offset: 0x00007AE4
		public override bool HasValues
		{
			get
			{
				return this.ChildrenTokens.Count > 0;
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000098F4 File Offset: 0x00007AF4
		internal bool ContentsEqual(JContainer container)
		{
			JToken jtoken = this.First;
			JToken jtoken2 = container.First;
			if (jtoken == jtoken2)
			{
				return true;
			}
			while (jtoken != null || jtoken2 != null)
			{
				if (jtoken == null || jtoken2 == null || !jtoken.DeepEquals(jtoken2))
				{
					return false;
				}
				jtoken = ((jtoken != this.Last) ? jtoken.Next : null);
				jtoken2 = ((jtoken2 != container.Last) ? jtoken2.Next : null);
			}
			return true;
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00009955 File Offset: 0x00007B55
		public override JToken First
		{
			get
			{
				return Enumerable.FirstOrDefault<JToken>(this.ChildrenTokens);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00009962 File Offset: 0x00007B62
		public override JToken Last
		{
			get
			{
				return Enumerable.LastOrDefault<JToken>(this.ChildrenTokens);
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000996F File Offset: 0x00007B6F
		public override JEnumerable<JToken> Children()
		{
			return new JEnumerable<JToken>(this.ChildrenTokens);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000997C File Offset: 0x00007B7C
		public override IEnumerable<T> Values<T>()
		{
			return this.ChildrenTokens.Convert<JToken, T>();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00009C08 File Offset: 0x00007E08
		public IEnumerable<JToken> Descendants()
		{
			foreach (JToken o in this.ChildrenTokens)
			{
				yield return o;
				JContainer c = o as JContainer;
				if (c != null)
				{
					foreach (JToken d in c.Descendants())
					{
						yield return d;
					}
				}
			}
			yield break;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00009C25 File Offset: 0x00007E25
		internal bool IsMultiContent(object content)
		{
			return content is IEnumerable && !(content is string) && !(content is JToken) && !(content is byte[]);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00009C50 File Offset: 0x00007E50
		internal JToken EnsureParentToken(JToken item)
		{
			if (item == null)
			{
				return new JValue(null);
			}
			if (item.Parent != null)
			{
				item = item.CloneToken();
			}
			else
			{
				JContainer jcontainer = this;
				while (jcontainer.Parent != null)
				{
					jcontainer = jcontainer.Parent;
				}
				if (item == jcontainer)
				{
					item = item.CloneToken();
				}
			}
			return item;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00009C99 File Offset: 0x00007E99
		internal int IndexOfItem(JToken item)
		{
			return this.ChildrenTokens.IndexOf(item, JContainer.JTokenReferenceEqualityComparer.Instance);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00009CAC File Offset: 0x00007EAC
		internal virtual void InsertItem(int index, JToken item)
		{
			if (index > this.ChildrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index must be within the bounds of the List.");
			}
			this.CheckReentrancy();
			item = this.EnsureParentToken(item);
			JToken jtoken = (index == 0) ? null : this.ChildrenTokens[index - 1];
			JToken jtoken2 = (index == this.ChildrenTokens.Count) ? null : this.ChildrenTokens[index];
			this.ValidateToken(item, null);
			item.Parent = this;
			item.Previous = jtoken;
			if (jtoken != null)
			{
				jtoken.Next = item;
			}
			item.Next = jtoken2;
			if (jtoken2 != null)
			{
				jtoken2.Previous = item;
			}
			this.ChildrenTokens.Insert(index, item);
			if (this.CollectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(0, item, index));
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00009D70 File Offset: 0x00007F70
		internal virtual void RemoveItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= this.ChildrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			this.CheckReentrancy();
			JToken jtoken = this.ChildrenTokens[index];
			JToken jtoken2 = (index == 0) ? null : this.ChildrenTokens[index - 1];
			JToken jtoken3 = (index == this.ChildrenTokens.Count - 1) ? null : this.ChildrenTokens[index + 1];
			if (jtoken2 != null)
			{
				jtoken2.Next = jtoken3;
			}
			if (jtoken3 != null)
			{
				jtoken3.Previous = jtoken2;
			}
			jtoken.Parent = null;
			jtoken.Previous = null;
			jtoken.Next = null;
			this.ChildrenTokens.RemoveAt(index);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(1, jtoken, index));
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00009E3C File Offset: 0x0000803C
		internal virtual bool RemoveItem(JToken item)
		{
			int num = this.IndexOfItem(item);
			if (num >= 0)
			{
				this.RemoveItemAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00009E5F File Offset: 0x0000805F
		internal virtual JToken GetItem(int index)
		{
			return this.ChildrenTokens[index];
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00009E70 File Offset: 0x00008070
		internal virtual void SetItem(int index, JToken item)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= this.ChildrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			JToken jtoken = this.ChildrenTokens[index];
			if (JContainer.IsTokenUnchanged(jtoken, item))
			{
				return;
			}
			this.CheckReentrancy();
			item = this.EnsureParentToken(item);
			this.ValidateToken(item, jtoken);
			JToken jtoken2 = (index == 0) ? null : this.ChildrenTokens[index - 1];
			JToken jtoken3 = (index == this.ChildrenTokens.Count - 1) ? null : this.ChildrenTokens[index + 1];
			item.Parent = this;
			item.Previous = jtoken2;
			if (jtoken2 != null)
			{
				jtoken2.Next = item;
			}
			item.Next = jtoken3;
			if (jtoken3 != null)
			{
				jtoken3.Previous = item;
			}
			this.ChildrenTokens[index] = item;
			jtoken.Parent = null;
			jtoken.Previous = null;
			jtoken.Next = null;
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(2, item, jtoken, index));
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00009F70 File Offset: 0x00008170
		internal virtual void ClearItems()
		{
			this.CheckReentrancy();
			foreach (JToken jtoken in this.ChildrenTokens)
			{
				jtoken.Parent = null;
				jtoken.Previous = null;
				jtoken.Next = null;
			}
			this.ChildrenTokens.Clear();
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(4));
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00009FE8 File Offset: 0x000081E8
		internal virtual void ReplaceItem(JToken existing, JToken replacement)
		{
			if (existing == null || existing.Parent != this)
			{
				return;
			}
			int index = this.IndexOfItem(existing);
			this.SetItem(index, replacement);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000A012 File Offset: 0x00008212
		internal virtual bool ContainsItem(JToken item)
		{
			return this.IndexOfItem(item) != -1;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000A024 File Offset: 0x00008224
		internal virtual void CopyItemsTo(Array array, int arrayIndex)
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
			if (this.Count > array.Length - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			foreach (JToken jtoken in this.ChildrenTokens)
			{
				array.SetValue(jtoken, arrayIndex + num);
				num++;
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000A0CC File Offset: 0x000082CC
		internal static bool IsTokenUnchanged(JToken currentValue, JToken newValue)
		{
			JValue jvalue = currentValue as JValue;
			return jvalue != null && ((jvalue.Type == JTokenType.Null && newValue == null) || jvalue.Equals(newValue));
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000A0FC File Offset: 0x000082FC
		internal virtual void ValidateToken(JToken o, JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type == JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					o.GetType(),
					base.GetType()
				}));
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000A14C File Offset: 0x0000834C
		public virtual void Add(object content)
		{
			this.AddInternal(this.ChildrenTokens.Count, content);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000A160 File Offset: 0x00008360
		public void AddFirst(object content)
		{
			this.AddInternal(0, content);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000A16C File Offset: 0x0000836C
		internal void AddInternal(int index, object content)
		{
			if (this.IsMultiContent(content))
			{
				IEnumerable enumerable = (IEnumerable)content;
				int num = index;
				using (IEnumerator enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object content2 = enumerator.Current;
						this.AddInternal(num, content2);
						num++;
					}
					return;
				}
			}
			JToken item = this.CreateFromContent(content);
			this.InsertItem(index, item);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000A1EC File Offset: 0x000083EC
		internal JToken CreateFromContent(object content)
		{
			if (content is JToken)
			{
				return (JToken)content;
			}
			return new JValue(content);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000A203 File Offset: 0x00008403
		public JsonWriter CreateWriter()
		{
			return new JTokenWriter(this);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000A20B File Offset: 0x0000840B
		public void ReplaceAll(object content)
		{
			this.ClearItems();
			this.Add(content);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000A21A File Offset: 0x0000841A
		public void RemoveAll()
		{
			this.ClearItems();
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000A224 File Offset: 0x00008424
		internal void ReadTokenFrom(JsonReader r)
		{
			int depth = r.Depth;
			if (!r.Read())
			{
				throw new Exception("Error reading {0} from JsonReader.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.GetType().Name
				}));
			}
			this.ReadContentFrom(r);
			int depth2 = r.Depth;
			if (depth2 > depth)
			{
				throw new Exception("Unexpected end of content while loading {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.GetType().Name
				}));
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000A2A8 File Offset: 0x000084A8
		internal void ReadContentFrom(JsonReader r)
		{
			ValidationUtils.ArgumentNotNull(r, "r");
			IJsonLineInfo lineInfo = r as IJsonLineInfo;
			JContainer jcontainer = this;
			for (;;)
			{
				if (jcontainer is JProperty && ((JProperty)jcontainer).Value != null)
				{
					if (jcontainer == this)
					{
						break;
					}
					jcontainer = jcontainer.Parent;
				}
				switch (r.TokenType)
				{
				case JsonToken.None:
					goto IL_224;
				case JsonToken.StartObject:
				{
					JObject jobject = new JObject();
					jobject.SetLineInfo(lineInfo);
					jcontainer.Add(jobject);
					jcontainer = jobject;
					goto IL_224;
				}
				case JsonToken.StartArray:
				{
					JArray jarray = new JArray();
					jarray.SetLineInfo(lineInfo);
					jcontainer.Add(jarray);
					jcontainer = jarray;
					goto IL_224;
				}
				case JsonToken.StartConstructor:
				{
					JConstructor jconstructor = new JConstructor(r.Value.ToString());
					jconstructor.SetLineInfo(jconstructor);
					jcontainer.Add(jconstructor);
					jcontainer = jconstructor;
					goto IL_224;
				}
				case JsonToken.PropertyName:
				{
					string name = r.Value.ToString();
					JProperty jproperty = new JProperty(name);
					jproperty.SetLineInfo(lineInfo);
					JObject jobject2 = (JObject)jcontainer;
					JProperty jproperty2 = jobject2.Property(name);
					if (jproperty2 == null)
					{
						jcontainer.Add(jproperty);
					}
					else
					{
						jproperty2.Replace(jproperty);
					}
					jcontainer = jproperty;
					goto IL_224;
				}
				case JsonToken.Comment:
				{
					JValue jvalue = JValue.CreateComment(r.Value.ToString());
					jvalue.SetLineInfo(lineInfo);
					jcontainer.Add(jvalue);
					goto IL_224;
				}
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					JValue jvalue = new JValue(r.Value);
					jvalue.SetLineInfo(lineInfo);
					jcontainer.Add(jvalue);
					goto IL_224;
				}
				case JsonToken.Null:
				{
					JValue jvalue = new JValue(null, JTokenType.Null);
					jvalue.SetLineInfo(lineInfo);
					jcontainer.Add(jvalue);
					goto IL_224;
				}
				case JsonToken.Undefined:
				{
					JValue jvalue = new JValue(null, JTokenType.Undefined);
					jvalue.SetLineInfo(lineInfo);
					jcontainer.Add(jvalue);
					goto IL_224;
				}
				case JsonToken.EndObject:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_224;
				case JsonToken.EndArray:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_224;
				case JsonToken.EndConstructor:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_224;
				}
				goto Block_4;
				IL_224:
				if (!r.Read())
				{
					return;
				}
			}
			return;
			Block_4:
			throw new InvalidOperationException("The JsonReader should not be on a token of type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				r.TokenType
			}));
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000A4E4 File Offset: 0x000086E4
		internal int ContentsHashCode()
		{
			int num = 0;
			foreach (JToken jtoken in this.ChildrenTokens)
			{
				num ^= jtoken.GetDeepHashCode();
			}
			return num;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000A538 File Offset: 0x00008738
		int IList<JToken>.IndexOf(JToken item)
		{
			return this.IndexOfItem(item);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000A541 File Offset: 0x00008741
		void IList<JToken>.Insert(int index, JToken item)
		{
			this.InsertItem(index, item);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000A54B File Offset: 0x0000874B
		void IList<JToken>.RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000A554 File Offset: 0x00008754
		// (set) Token: 0x06000215 RID: 533 RVA: 0x0000A55D File Offset: 0x0000875D
		JToken IList<JToken>.Item
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

		// Token: 0x06000216 RID: 534 RVA: 0x0000A567 File Offset: 0x00008767
		void ICollection<JToken>.Add(JToken item)
		{
			this.Add(item);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000A570 File Offset: 0x00008770
		void ICollection<JToken>.Clear()
		{
			this.ClearItems();
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000A578 File Offset: 0x00008778
		bool ICollection<JToken>.Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000A581 File Offset: 0x00008781
		void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000A58B File Offset: 0x0000878B
		bool ICollection<JToken>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000A58E File Offset: 0x0000878E
		bool ICollection<JToken>.Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000A597 File Offset: 0x00008797
		private JToken EnsureValue(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is JToken)
			{
				return (JToken)value;
			}
			throw new ArgumentException("Argument is not a JToken.");
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000A5B7 File Offset: 0x000087B7
		int IList.Add(object value)
		{
			this.Add(this.EnsureValue(value));
			return this.Count - 1;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000A5CE File Offset: 0x000087CE
		void IList.Clear()
		{
			this.ClearItems();
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000A5D6 File Offset: 0x000087D6
		bool IList.Contains(object value)
		{
			return this.ContainsItem(this.EnsureValue(value));
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000A5E5 File Offset: 0x000087E5
		int IList.IndexOf(object value)
		{
			return this.IndexOfItem(this.EnsureValue(value));
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000A5F4 File Offset: 0x000087F4
		void IList.Insert(int index, object value)
		{
			this.InsertItem(index, this.EnsureValue(value));
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000A604 File Offset: 0x00008804
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000A607 File Offset: 0x00008807
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000A60A File Offset: 0x0000880A
		void IList.Remove(object value)
		{
			this.RemoveItem(this.EnsureValue(value));
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000A61A File Offset: 0x0000881A
		void IList.RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000A623 File Offset: 0x00008823
		// (set) Token: 0x06000227 RID: 551 RVA: 0x0000A62C File Offset: 0x0000882C
		object IList.Item
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, this.EnsureValue(value));
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000A63C File Offset: 0x0000883C
		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyItemsTo(array, index);
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000A646 File Offset: 0x00008846
		public int Count
		{
			get
			{
				return this.ChildrenTokens.Count;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000A653 File Offset: 0x00008853
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000A656 File Offset: 0x00008856
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x040000B2 RID: 178
		private object _syncRoot;

		// Token: 0x040000B3 RID: 179
		private bool _busy;

		// Token: 0x02000021 RID: 33
		private class JTokenReferenceEqualityComparer : IEqualityComparer<JToken>
		{
			// Token: 0x0600022C RID: 556 RVA: 0x0000A678 File Offset: 0x00008878
			public bool Equals(JToken x, JToken y)
			{
				return object.ReferenceEquals(x, y);
			}

			// Token: 0x0600022D RID: 557 RVA: 0x0000A681 File Offset: 0x00008881
			public int GetHashCode(JToken obj)
			{
				if (obj == null)
				{
					return 0;
				}
				return obj.GetHashCode();
			}

			// Token: 0x040000B4 RID: 180
			public static readonly JContainer.JTokenReferenceEqualityComparer Instance = new JContainer.JTokenReferenceEqualityComparer();
		}
	}
}
