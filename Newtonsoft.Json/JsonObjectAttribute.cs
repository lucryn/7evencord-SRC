using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000066 RID: 102
	[AttributeUsage(1036, AllowMultiple = false)]
	public sealed class JsonObjectAttribute : JsonContainerAttribute
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x000159A7 File Offset: 0x00013BA7
		// (set) Token: 0x060004E7 RID: 1255 RVA: 0x000159AF File Offset: 0x00013BAF
		public MemberSerialization MemberSerialization
		{
			get
			{
				return this._memberSerialization;
			}
			set
			{
				this._memberSerialization = value;
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000159B8 File Offset: 0x00013BB8
		public JsonObjectAttribute()
		{
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000159C0 File Offset: 0x00013BC0
		public JsonObjectAttribute(MemberSerialization memberSerialization)
		{
			this.MemberSerialization = memberSerialization;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000159CF File Offset: 0x00013BCF
		public JsonObjectAttribute(string id) : base(id)
		{
		}

		// Token: 0x0400013B RID: 315
		private MemberSerialization _memberSerialization;
	}
}
