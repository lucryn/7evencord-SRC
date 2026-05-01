using System;
using W7Cord.Models;

namespace W7Cord.ViewModels
{
	// Token: 0x02000007 RID: 7
	public class GuildViewModel
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00003748 File Offset: 0x00001948
		public string Id
		{
			get
			{
				return this._model.id;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003768 File Offset: 0x00001968
		public string Name
		{
			get
			{
				return this._model.name;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00003788 File Offset: 0x00001988
		public string IconUrl
		{
			get
			{
				string result;
				if (string.IsNullOrEmpty(this._model.icon))
				{
					result = null;
				}
				else
				{
					result = CDN.GuildIconUrl(this._model.id, this._model.icon);
				}
				return result;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000037D0 File Offset: 0x000019D0
		public GuildViewModel(Guild model)
		{
			this._model = model;
		}

		// Token: 0x04000020 RID: 32
		private Guild _model;
	}
}
