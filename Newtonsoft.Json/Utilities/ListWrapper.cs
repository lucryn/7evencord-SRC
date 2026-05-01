using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200007D RID: 125
	internal class ListWrapper<T> : CollectionWrapper<T>, IList<T>, ICollection<T>, IEnumerable<T>, IWrappedList, IList, ICollection, IEnumerable
	{
		// Token: 0x0600061A RID: 1562 RVA: 0x00018648 File Offset: 0x00016848
		public ListWrapper(IList list) : base(list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			if (list is IList<T>)
			{
				this._genericList = (IList<T>)list;
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00018670 File Offset: 0x00016870
		public ListWrapper(IList<T> list) : base(list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			this._genericList = list;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001868B File Offset: 0x0001688B
		public int IndexOf(T item)
		{
			if (this._genericList != null)
			{
				return this._genericList.IndexOf(item);
			}
			return this.IndexOf(item);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x000186AE File Offset: 0x000168AE
		public void Insert(int index, T item)
		{
			if (this._genericList != null)
			{
				this._genericList.Insert(index, item);
				return;
			}
			this.Insert(index, item);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x000186D3 File Offset: 0x000168D3
		public void RemoveAt(int index)
		{
			if (this._genericList != null)
			{
				this._genericList.RemoveAt(index);
				return;
			}
			this.RemoveAt(index);
		}

		// Token: 0x17000122 RID: 290
		public T this[int index]
		{
			get
			{
				if (this._genericList != null)
				{
					return this._genericList[index];
				}
				return (T)((object)this[index]);
			}
			set
			{
				if (this._genericList != null)
				{
					this._genericList[index] = value;
					return;
				}
				this[index] = value;
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00018739 File Offset: 0x00016939
		public override void Add(T item)
		{
			if (this._genericList != null)
			{
				this._genericList.Add(item);
				return;
			}
			base.Add(item);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00018757 File Offset: 0x00016957
		public override void Clear()
		{
			if (this._genericList != null)
			{
				this._genericList.Clear();
				return;
			}
			base.Clear();
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00018773 File Offset: 0x00016973
		public override bool Contains(T item)
		{
			if (this._genericList != null)
			{
				return this._genericList.Contains(item);
			}
			return base.Contains(item);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00018791 File Offset: 0x00016991
		public override void CopyTo(T[] array, int arrayIndex)
		{
			if (this._genericList != null)
			{
				this._genericList.CopyTo(array, arrayIndex);
				return;
			}
			base.CopyTo(array, arrayIndex);
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x000187B1 File Offset: 0x000169B1
		public override int Count
		{
			get
			{
				if (this._genericList != null)
				{
					return this._genericList.Count;
				}
				return base.Count;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x000187CD File Offset: 0x000169CD
		public override bool IsReadOnly
		{
			get
			{
				if (this._genericList != null)
				{
					return this._genericList.IsReadOnly;
				}
				return base.IsReadOnly;
			}
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000187EC File Offset: 0x000169EC
		public override bool Remove(T item)
		{
			if (this._genericList != null)
			{
				return this._genericList.Remove(item);
			}
			bool flag = base.Contains(item);
			if (flag)
			{
				base.Remove(item);
			}
			return flag;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00018822 File Offset: 0x00016A22
		public override IEnumerator<T> GetEnumerator()
		{
			if (this._genericList != null)
			{
				return this._genericList.GetEnumerator();
			}
			return base.GetEnumerator();
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x0001883E File Offset: 0x00016A3E
		public object UnderlyingList
		{
			get
			{
				if (this._genericList != null)
				{
					return this._genericList;
				}
				return base.UnderlyingCollection;
			}
		}

		// Token: 0x040001A9 RID: 425
		private readonly IList<T> _genericList;
	}
}
