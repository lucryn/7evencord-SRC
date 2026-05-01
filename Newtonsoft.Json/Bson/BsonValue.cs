using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000085 RID: 133
	internal class BsonValue : BsonToken
	{
		// Token: 0x06000669 RID: 1641 RVA: 0x00018C13 File Offset: 0x00016E13
		public BsonValue(object value, BsonType type)
		{
			this._value = value;
			this._type = type;
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x00018C29 File Offset: 0x00016E29
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00018C31 File Offset: 0x00016E31
		public override BsonType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x040001BA RID: 442
		private object _value;

		// Token: 0x040001BB RID: 443
		private BsonType _type;
	}
}
