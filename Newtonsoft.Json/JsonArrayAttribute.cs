using System;

namespace Newtonsoft.Json
{
	// Token: 0x020000AE RID: 174
	[AttributeUsage(1028, AllowMultiple = false)]
	public sealed class JsonArrayAttribute : JsonContainerAttribute
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x0001E3A7 File Offset: 0x0001C5A7
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x0001E3AF File Offset: 0x0001C5AF
		public bool AllowNullItems
		{
			get
			{
				return this._allowNullItems;
			}
			set
			{
				this._allowNullItems = value;
			}
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0001E3B8 File Offset: 0x0001C5B8
		public JsonArrayAttribute()
		{
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0001E3C0 File Offset: 0x0001C5C0
		public JsonArrayAttribute(bool allowNullItems)
		{
			this._allowNullItems = allowNullItems;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0001E3CF File Offset: 0x0001C5CF
		public JsonArrayAttribute(string id) : base(id)
		{
		}

		// Token: 0x04000290 RID: 656
		private bool _allowNullItems;
	}
}
