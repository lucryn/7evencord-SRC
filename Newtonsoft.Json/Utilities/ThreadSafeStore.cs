using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000067 RID: 103
	internal class ThreadSafeStore<TKey, TValue>
	{
		// Token: 0x060004EB RID: 1259 RVA: 0x000159D8 File Offset: 0x00013BD8
		public ThreadSafeStore(Func<TKey, TValue> creator)
		{
			if (creator == null)
			{
				throw new ArgumentNullException("creator");
			}
			this._creator = creator;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00015A00 File Offset: 0x00013C00
		public TValue Get(TKey key)
		{
			if (this._store == null)
			{
				return this.AddValue(key);
			}
			TValue result;
			if (!this._store.TryGetValue(key, ref result))
			{
				return this.AddValue(key);
			}
			return result;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00015A38 File Offset: 0x00013C38
		private TValue AddValue(TKey key)
		{
			TValue tvalue = this._creator.Invoke(key);
			TValue result2;
			lock (this._lock)
			{
				if (this._store == null)
				{
					this._store = new Dictionary<TKey, TValue>();
					this._store[key] = tvalue;
				}
				else
				{
					TValue result;
					if (this._store.TryGetValue(key, ref result))
					{
						return result;
					}
					Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(this._store);
					dictionary[key] = tvalue;
					this._store = dictionary;
				}
				result2 = tvalue;
			}
			return result2;
		}

		// Token: 0x0400013C RID: 316
		private readonly object _lock = new object();

		// Token: 0x0400013D RID: 317
		private Dictionary<TKey, TValue> _store;

		// Token: 0x0400013E RID: 318
		private readonly Func<TKey, TValue> _creator;
	}
}
