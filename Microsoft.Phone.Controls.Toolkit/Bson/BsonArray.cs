using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000084 RID: 132
	internal class BsonArray : BsonToken, IEnumerable<BsonToken>, IEnumerable
	{
		// Token: 0x06000664 RID: 1636 RVA: 0x00018BCE File Offset: 0x00016DCE
		public void Add(BsonToken token)
		{
			this._children.Add(token);
			token.Parent = this;
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00018BE3 File Offset: 0x00016DE3
		public override BsonType Type
		{
			get
			{
				return BsonType.Array;
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00018BE6 File Offset: 0x00016DE6
		public IEnumerator<BsonToken> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00018BF8 File Offset: 0x00016DF8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040001B9 RID: 441
		private readonly List<BsonToken> _children = new List<BsonToken>();
	}
}
