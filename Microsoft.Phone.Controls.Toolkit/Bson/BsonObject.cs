using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000083 RID: 131
	internal class BsonObject : BsonToken, IEnumerable<BsonProperty>, IEnumerable
	{
		// Token: 0x0600065F RID: 1631 RVA: 0x00018B64 File Offset: 0x00016D64
		public void Add(string name, BsonToken token)
		{
			this._children.Add(new BsonProperty
			{
				Name = new BsonString(name, false),
				Value = token
			});
			token.Parent = this;
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x00018B9E File Offset: 0x00016D9E
		public override BsonType Type
		{
			get
			{
				return BsonType.Object;
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00018BA1 File Offset: 0x00016DA1
		public IEnumerator<BsonProperty> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00018BB3 File Offset: 0x00016DB3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040001B8 RID: 440
		private readonly List<BsonProperty> _children = new List<BsonProperty>();
	}
}
