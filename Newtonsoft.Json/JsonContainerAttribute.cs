using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000065 RID: 101
	[AttributeUsage(1028, AllowMultiple = false)]
	public abstract class JsonContainerAttribute : Attribute
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00015928 File Offset: 0x00013B28
		// (set) Token: 0x060004DD RID: 1245 RVA: 0x00015930 File Offset: 0x00013B30
		public string Id { get; set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00015939 File Offset: 0x00013B39
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x00015941 File Offset: 0x00013B41
		public string Title { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x0001594A File Offset: 0x00013B4A
		// (set) Token: 0x060004E1 RID: 1249 RVA: 0x00015952 File Offset: 0x00013B52
		public string Description { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0001595C File Offset: 0x00013B5C
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x00015982 File Offset: 0x00013B82
		public bool IsReference
		{
			get
			{
				return this._isReference ?? false;
			}
			set
			{
				this._isReference = new bool?(value);
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00015990 File Offset: 0x00013B90
		protected JsonContainerAttribute()
		{
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00015998 File Offset: 0x00013B98
		protected JsonContainerAttribute(string id)
		{
			this.Id = id;
		}

		// Token: 0x04000137 RID: 311
		internal bool? _isReference;
	}
}
