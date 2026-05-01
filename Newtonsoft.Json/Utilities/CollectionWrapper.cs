using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200004B RID: 75
	internal class CollectionWrapper<T> : ICollection<T>, IEnumerable<T>, IWrappedCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x060003E1 RID: 993 RVA: 0x00010542 File Offset: 0x0000E742
		public CollectionWrapper(IList list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			if (list is ICollection<T>)
			{
				this._genericCollection = (ICollection<T>)list;
				return;
			}
			this._list = list;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00010571 File Offset: 0x0000E771
		public CollectionWrapper(ICollection<T> list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			this._genericCollection = list;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001058B File Offset: 0x0000E78B
		public virtual void Add(T item)
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.Add(item);
				return;
			}
			this._list.Add(item);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000105B4 File Offset: 0x0000E7B4
		public virtual void Clear()
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.Clear();
				return;
			}
			this._list.Clear();
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000105D5 File Offset: 0x0000E7D5
		public virtual bool Contains(T item)
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.Contains(item);
			}
			return this._list.Contains(item);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x000105FD File Offset: 0x0000E7FD
		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.CopyTo(array, arrayIndex);
				return;
			}
			this._list.CopyTo(array, arrayIndex);
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00010622 File Offset: 0x0000E822
		public virtual int Count
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.Count;
				}
				return this._list.Count;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00010643 File Offset: 0x0000E843
		public virtual bool IsReadOnly
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.IsReadOnly;
				}
				return this._list.IsReadOnly;
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00010664 File Offset: 0x0000E864
		public virtual bool Remove(T item)
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.Remove(item);
			}
			bool flag = this._list.Contains(item);
			if (flag)
			{
				this._list.Remove(item);
			}
			return flag;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x000106AD File Offset: 0x0000E8AD
		public virtual IEnumerator<T> GetEnumerator()
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.GetEnumerator();
			}
			return Enumerable.Cast<T>(this._list).GetEnumerator();
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x000106D3 File Offset: 0x0000E8D3
		IEnumerator IEnumerable.GetEnumerator()
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.GetEnumerator();
			}
			return this._list.GetEnumerator();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x000106F4 File Offset: 0x0000E8F4
		int IList.Add(object value)
		{
			CollectionWrapper<T>.VerifyValueType(value);
			this.Add((T)((object)value));
			return this.Count - 1;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00010710 File Offset: 0x0000E910
		bool IList.Contains(object value)
		{
			return CollectionWrapper<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00010728 File Offset: 0x0000E928
		int IList.IndexOf(object value)
		{
			if (this._genericCollection != null)
			{
				throw new Exception("Wrapped ICollection<T> does not support IndexOf.");
			}
			if (CollectionWrapper<T>.IsCompatibleObject(value))
			{
				return this._list.IndexOf((T)((object)value));
			}
			return -1;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0001075D File Offset: 0x0000E95D
		void IList.RemoveAt(int index)
		{
			if (this._genericCollection != null)
			{
				throw new Exception("Wrapped ICollection<T> does not support RemoveAt.");
			}
			this._list.RemoveAt(index);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001077E File Offset: 0x0000E97E
		void IList.Insert(int index, object value)
		{
			if (this._genericCollection != null)
			{
				throw new Exception("Wrapped ICollection<T> does not support Insert.");
			}
			CollectionWrapper<T>.VerifyValueType(value);
			this._list.Insert(index, (T)((object)value));
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000107B0 File Offset: 0x0000E9B0
		bool IList.IsFixedSize
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.IsReadOnly;
				}
				return this._list.IsFixedSize;
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x000107D1 File Offset: 0x0000E9D1
		void IList.Remove(object value)
		{
			if (CollectionWrapper<T>.IsCompatibleObject(value))
			{
				this.Remove((T)((object)value));
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x000107E8 File Offset: 0x0000E9E8
		// (set) Token: 0x060003F4 RID: 1012 RVA: 0x00010809 File Offset: 0x0000EA09
		object IList.Item
		{
			get
			{
				if (this._genericCollection != null)
				{
					throw new Exception("Wrapped ICollection<T> does not support indexer.");
				}
				return this._list[index];
			}
			set
			{
				if (this._genericCollection != null)
				{
					throw new Exception("Wrapped ICollection<T> does not support indexer.");
				}
				CollectionWrapper<T>.VerifyValueType(value);
				this._list[index] = (T)((object)value);
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0001083B File Offset: 0x0000EA3B
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			this.CopyTo((T[])array, arrayIndex);
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0001084A File Offset: 0x0000EA4A
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0001084D File Offset: 0x0000EA4D
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

		// Token: 0x060003F8 RID: 1016 RVA: 0x00010870 File Offset: 0x0000EA70
		private static void VerifyValueType(object value)
		{
			if (!CollectionWrapper<T>.IsCompatibleObject(value))
			{
				throw new ArgumentException("The value '{0}' is not of type '{1}' and cannot be used in this generic collection.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					value,
					typeof(T)
				}), "value");
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x000108B8 File Offset: 0x0000EAB8
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && (!typeof(T).IsValueType || ReflectionUtils.IsNullableType(typeof(T))));
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x000108EA File Offset: 0x0000EAEA
		public object UnderlyingCollection
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection;
				}
				return this._list;
			}
		}

		// Token: 0x0400010D RID: 269
		private readonly IList _list;

		// Token: 0x0400010E RID: 270
		private readonly ICollection<T> _genericCollection;

		// Token: 0x0400010F RID: 271
		private object _syncRoot;
	}
}
