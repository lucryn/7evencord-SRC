using System;
using W7Cord.Models;

namespace W7Cord.ViewModels
{
	// Token: 0x02000009 RID: 9
	public class MessageViewModel
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003AD4 File Offset: 0x00001CD4
		public string Content
		{
			get
			{
				return this._model.content;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003AF4 File Offset: 0x00001CF4
		public string AuthorName
		{
			get
			{
				string result;
				if (this._model.author != null)
				{
					result = this._model.author.username;
				}
				else
				{
					result = "System";
				}
				return result;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003B30 File Offset: 0x00001D30
		public string AvatarUrl
		{
			get
			{
				string result;
				if (this._model.author != null && !string.IsNullOrEmpty(this._model.author.avatar))
				{
					result = CDN.AvatarUrl(this._model.author.id, this._model.author.avatar, this._model.author.discriminator);
				}
				else
				{
					result = CDN.DefaultAvatarUrl(0);
				}
				return result;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003BAC File Offset: 0x00001DAC
		public bool IsOutgoing
		{
			get
			{
				return !string.IsNullOrEmpty(this._currentUserId) && this._model.author != null && this._model.author.id == this._currentUserId;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003C08 File Offset: 0x00001E08
		public string Timestamp
		{
			get
			{
				return this._model.timestamp;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003C25 File Offset: 0x00001E25
		public MessageViewModel(Message model, string currentUserId = null)
		{
			this._model = model;
			this._currentUserId = currentUserId;
		}

		// Token: 0x04000025 RID: 37
		private Message _model;

		// Token: 0x04000026 RID: 38
		private string _currentUserId;
	}
}
