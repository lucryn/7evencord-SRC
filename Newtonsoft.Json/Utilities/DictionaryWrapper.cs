using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200000D RID: 13
	internal class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IWrappedDictionary, IDictionary, ICollection, IEnumerable
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00005452 File Offset: 0x00003652
		public DictionaryWrapper(IDictionary dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._dictionary = dictionary;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000546C File Offset: 0x0000366C
		public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._genericDictionary = dictionary;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00005486 File Offset: 0x00003686
		public void Add(TKey key, TValue value)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add(key, value);
				return;
			}
			this._dictionary.Add(key, value);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000054B5 File Offset: 0x000036B5
		public bool ContainsKey(TKey key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.ContainsKey(key);
			}
			return this._dictionary.Contains(key);
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000081 RID: 129 RVA: 0x000054DD File Offset: 0x000036DD
		public ICollection<TKey> Keys
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Keys;
				}
				return Enumerable.ToList<TKey>(Enumerable.Cast<TKey>(this._dictionary.Keys));
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00005508 File Offset: 0x00003708
		public bool Remove(TKey key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.Remove(key);
			}
			if (this._dictionary.Contains(key))
			{
				this._dictionary.Remove(key);
				return true;
			}
			return false;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00005548 File Offset: 0x00003748
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.TryGetValue(key, ref value);
			}
			if (!this._dictionary.Contains(key))
			{
				value = default(TValue);
				return false;
			}
			value = (TValue)((object)this._dictionary[key]);
			return true;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000055A4 File Offset: 0x000037A4
		public ICollection<TValue> Values
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Values;
				}
				return Enumerable.ToList<TValue>(Enumerable.Cast<TValue>(this._dictionary.Values));
			}
		}

		// Token: 0x1700000A RID: 10
		public TValue this[TKey key]
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary[key];
				}
				return (TValue)((object)this._dictionary[key]);
			}
			set
			{
				if (this._genericDictionary != null)
				{
					this._genericDictionary[key] = value;
					return;
				}
				this._dictionary[key] = value;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000562B File Offset: 0x0000382B
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add(item);
				return;
			}
			((IList)this._dictionary).Add(item);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005659 File Offset: 0x00003859
		public void Clear()
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Clear();
				return;
			}
			this._dictionary.Clear();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000567A File Offset: 0x0000387A
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.Contains(item);
			}
			return ((IList)this._dictionary).Contains(item);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000056A8 File Offset: 0x000038A8
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.CopyTo(array, arrayIndex);
				return;
			}
			foreach (object obj in this._dictionary)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey)((object)dictionaryEntry.Key), (TValue)((object)dictionaryEntry.Value));
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00005740 File Offset: 0x00003940
		public int Count
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Count;
				}
				return this._dictionary.Count;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00005761 File Offset: 0x00003961
		public bool IsReadOnly
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.IsReadOnly;
				}
				return this._dictionary.IsReadOnly;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005784 File Offset: 0x00003984
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.Remove(item);
			}
			if (!this._dictionary.Contains(item.Key))
			{
				return true;
			}
			object obj = this._dictionary[item.Key];
			if (object.Equals(obj, item.Value))
			{
				this._dictionary.Remove(item.Key);
				return true;
			}
			return false;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005828 File Offset: 0x00003A28
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.GetEnumerator();
			}
			return Enumerable.Select<DictionaryEntry, KeyValuePair<TKey, TValue>>(Enumerable.Cast<DictionaryEntry>(this._dictionary), (DictionaryEntry de) => new KeyValuePair<TKey, TValue>((TKey)((object)de.Key), (TValue)((object)de.Value))).GetEnumerator();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000587B File Offset: 0x00003A7B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005883 File Offset: 0x00003A83
		void IDictionary.Add(object key, object value)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add((TKey)((object)key), (TValue)((object)value));
				return;
			}
			this._dictionary.Add(key, value);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000058B2 File Offset: 0x00003AB2
		bool IDictionary.Contains(object key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.ContainsKey((TKey)((object)key));
			}
			return this._dictionary.Contains(key);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000058DA File Offset: 0x00003ADA
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			if (this._genericDictionary != null)
			{
				return new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this._genericDictionary.GetEnumerator());
			}
			return this._dictionary.GetEnumerator();
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00005905 File Offset: 0x00003B05
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this._genericDictionary == null && this._dictionary.IsFixedSize;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000591C File Offset: 0x00003B1C
		ICollection IDictionary.Keys
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return Enumerable.ToList<TKey>(this._genericDictionary.Keys);
				}
				return this._dictionary.Keys;
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005942 File Offset: 0x00003B42
		public void Remove(object key)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Remove((TKey)((object)key));
				return;
			}
			this._dictionary.Remove(key);
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000096 RID: 150 RVA: 0x0000596B File Offset: 0x00003B6B
		ICollection IDictionary.Values
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return Enumerable.ToList<TValue>(this._genericDictionary.Values);
				}
				return this._dictionary.Values;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00005991 File Offset: 0x00003B91
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000059BE File Offset: 0x00003BBE
		object IDictionary.Item
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary[(TKey)((object)key)];
				}
				return this._dictionary[key];
			}
			set
			{
				if (this._genericDictionary != null)
				{
					this._genericDictionary[(TKey)((object)key)] = (TValue)((object)value);
					return;
				}
				this._dictionary[key] = value;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000059ED File Offset: 0x00003BED
		void ICollection.CopyTo(Array array, int index)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
				return;
			}
			this._dictionary.CopyTo(array, index);
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00005A17 File Offset: 0x00003C17
		bool ICollection.IsSynchronized
		{
			get
			{
				return this._genericDictionary == null && this._dictionary.IsSynchronized;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00005A2E File Offset: 0x00003C2E
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

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00005A50 File Offset: 0x00003C50
		public object UnderlyingDictionary
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary;
				}
				return this._dictionary;
			}
		}

		// Token: 0x0400003A RID: 58
		private readonly IDictionary _dictionary;

		// Token: 0x0400003B RID: 59
		private readonly IDictionary<TKey, TValue> _genericDictionary;

		// Token: 0x0400003C RID: 60
		private object _syncRoot;

		// Token: 0x0200000E RID: 14
		private struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x0600009E RID: 158 RVA: 0x00005A67 File Offset: 0x00003C67
			public DictionaryEnumerator(IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				ValidationUtils.ArgumentNotNull(e, "e");
				this._e = e;
			}

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x0600009F RID: 159 RVA: 0x00005A7B File Offset: 0x00003C7B
			public DictionaryEntry Entry
			{
				get
				{
					return (DictionaryEntry)this.Current;
				}
			}

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x060000A0 RID: 160 RVA: 0x00005A88 File Offset: 0x00003C88
			public object Key
			{
				get
				{
					return this.Entry.Key;
				}
			}

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x060000A1 RID: 161 RVA: 0x00005AA4 File Offset: 0x00003CA4
			public object Value
			{
				get
				{
					return this.Entry.Value;
				}
			}

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x060000A2 RID: 162 RVA: 0x00005AC0 File Offset: 0x00003CC0
			public object Current
			{
				get
				{
					KeyValuePair<TEnumeratorKey, TEnumeratorValue> keyValuePair = this._e.Current;
					object obj = keyValuePair.Key;
					KeyValuePair<TEnumeratorKey, TEnumeratorValue> keyValuePair2 = this._e.Current;
					return new DictionaryEntry(obj, keyValuePair2.Value);
				}
			}

			// Token: 0x060000A3 RID: 163 RVA: 0x00005B07 File Offset: 0x00003D07
			public bool MoveNext()
			{
				return this._e.MoveNext();
			}

			// Token: 0x060000A4 RID: 164 RVA: 0x00005B14 File Offset: 0x00003D14
			public void Reset()
			{
				this._e.Reset();
			}

			// Token: 0x0400003E RID: 62
			private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;
		}
	}
}
