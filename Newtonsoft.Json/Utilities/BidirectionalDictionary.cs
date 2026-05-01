using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000AC RID: 172
	internal class BidirectionalDictionary<TFirst, TSecond>
	{
		// Token: 0x0600081F RID: 2079 RVA: 0x0001E2A5 File Offset: 0x0001C4A5
		public BidirectionalDictionary() : this(EqualityComparer<TFirst>.Default, EqualityComparer<TSecond>.Default)
		{
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001E2B7 File Offset: 0x0001C4B7
		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer)
		{
			this._firstToSecond = new Dictionary<TFirst, TSecond>(firstEqualityComparer);
			this._secondToFirst = new Dictionary<TSecond, TFirst>(secondEqualityComparer);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0001E2D8 File Offset: 0x0001C4D8
		public void Add(TFirst first, TSecond second)
		{
			if (this._firstToSecond.ContainsKey(first) || this._secondToFirst.ContainsKey(second))
			{
				throw new ArgumentException("Duplicate first or second");
			}
			this._firstToSecond.Add(first, second);
			this._secondToFirst.Add(second, first);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001E326 File Offset: 0x0001C526
		public bool TryGetByFirst(TFirst first, out TSecond second)
		{
			return this._firstToSecond.TryGetValue(first, ref second);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001E335 File Offset: 0x0001C535
		public bool TryGetBySecond(TSecond second, out TFirst first)
		{
			return this._secondToFirst.TryGetValue(second, ref first);
		}

		// Token: 0x0400028C RID: 652
		private readonly IDictionary<TFirst, TSecond> _firstToSecond;

		// Token: 0x0400028D RID: 653
		private readonly IDictionary<TSecond, TFirst> _secondToFirst;
	}
}
