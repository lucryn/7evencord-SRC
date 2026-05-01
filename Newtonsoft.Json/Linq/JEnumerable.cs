using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000042 RID: 66
	public struct JEnumerable<T> : IJEnumerable<T>, IEnumerable<T>, IEnumerable where T : JToken
	{
		// Token: 0x060003AB RID: 939 RVA: 0x0000FA48 File Offset: 0x0000DC48
		public JEnumerable(IEnumerable<T> enumerable)
		{
			ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
			this._enumerable = enumerable;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000FA5C File Offset: 0x0000DC5C
		public IEnumerator<T> GetEnumerator()
		{
			return this._enumerable.GetEnumerator();
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000FA69 File Offset: 0x0000DC69
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x170000C0 RID: 192
		public IJEnumerable<JToken> this[object key]
		{
			get
			{
				return new JEnumerable<JToken>(this._enumerable.Values(key));
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000FA89 File Offset: 0x0000DC89
		public override bool Equals(object obj)
		{
			return obj is JEnumerable<T> && this._enumerable.Equals(((JEnumerable<T>)obj)._enumerable);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000FAAB File Offset: 0x0000DCAB
		public override int GetHashCode()
		{
			return this._enumerable.GetHashCode();
		}

		// Token: 0x04000103 RID: 259
		public static readonly JEnumerable<T> Empty = new JEnumerable<T>(Enumerable.Empty<T>());

		// Token: 0x04000104 RID: 260
		private IEnumerable<T> _enumerable;
	}
}
