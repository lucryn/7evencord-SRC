using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace System.Windows.Interactivity
{
	// Token: 0x02000003 RID: 3
	public abstract class AttachableCollection<T> : DependencyObjectCollection<T>, IAttachedObject where T : DependencyObject, IAttachedObject
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020D0 File Offset: 0x000002D0
		protected DependencyObject AssociatedObject
		{
			get
			{
				return this.associatedObject;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020D8 File Offset: 0x000002D8
		internal AttachableCollection()
		{
			this.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
			this.snapshot = new Collection<T>();
		}

		// Token: 0x06000006 RID: 6
		protected abstract void OnAttached();

		// Token: 0x06000007 RID: 7
		protected abstract void OnDetaching();

		// Token: 0x06000008 RID: 8
		internal abstract void ItemAdded(T item);

		// Token: 0x06000009 RID: 9
		internal abstract void ItemRemoved(T item);

		// Token: 0x0600000A RID: 10 RVA: 0x0000210C File Offset: 0x0000030C
		[Conditional("DEBUG")]
		private void VerifySnapshotIntegrity()
		{
			bool flag = base.Count == this.snapshot.Count;
			if (flag)
			{
				for (int i = 0; i < base.Count; i++)
				{
					if (base[i] != this.snapshot[i])
					{
						return;
					}
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002164 File Offset: 0x00000364
		private void VerifyAdd(T item)
		{
			if (this.snapshot.Contains(item))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.DuplicateItemInCollectionExceptionMessage, new object[]
				{
					typeof(T).Name,
					base.GetType().Name
				}));
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021BC File Offset: 0x000003BC
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case 0:
				using (IEnumerator enumerator = e.NewItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						T t = (T)((object)obj);
						try
						{
							this.VerifyAdd(t);
							this.ItemAdded(t);
						}
						finally
						{
							this.snapshot.Insert(base.IndexOf(t), t);
						}
					}
					return;
				}
				break;
			case 1:
				goto IL_13A;
			case 2:
				break;
			case 3:
				return;
			case 4:
				goto IL_18D;
			default:
				return;
			}
			foreach (object obj2 in e.OldItems)
			{
				T t2 = (T)((object)obj2);
				this.ItemRemoved(t2);
				this.snapshot.Remove(t2);
			}
			using (IEnumerator enumerator3 = e.NewItems.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					object obj3 = enumerator3.Current;
					T t3 = (T)((object)obj3);
					try
					{
						this.VerifyAdd(t3);
						this.ItemAdded(t3);
					}
					finally
					{
						this.snapshot.Insert(base.IndexOf(t3), t3);
					}
				}
				return;
			}
			IL_13A:
			using (IEnumerator enumerator4 = e.OldItems.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					object obj4 = enumerator4.Current;
					T t4 = (T)((object)obj4);
					this.ItemRemoved(t4);
					this.snapshot.Remove(t4);
				}
				return;
			}
			IL_18D:
			foreach (T item in this.snapshot)
			{
				this.ItemRemoved(item);
			}
			this.snapshot = new Collection<T>();
			foreach (T item2 in this)
			{
				this.VerifyAdd(item2);
				this.ItemAdded(item2);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002438 File Offset: 0x00000638
		DependencyObject IAttachedObject.AssociatedObject
		{
			get
			{
				return this.AssociatedObject;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002440 File Offset: 0x00000640
		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != this.AssociatedObject)
			{
				if (this.AssociatedObject != null)
				{
					throw new InvalidOperationException();
				}
				if (Application.Current == null || Application.Current.RootVisual == null || !(bool)Application.Current.RootVisual.GetValue(DesignerProperties.IsInDesignModeProperty))
				{
					this.associatedObject = dependencyObject;
				}
				this.OnAttached();
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000249F File Offset: 0x0000069F
		public void Detach()
		{
			this.OnDetaching();
			this.associatedObject = null;
		}

		// Token: 0x04000001 RID: 1
		private Collection<T> snapshot;

		// Token: 0x04000002 RID: 2
		private DependencyObject associatedObject;
	}
}
