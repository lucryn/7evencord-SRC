using System;
using System.ComponentModel;
using W7Cord.Models;

namespace W7Cord.ViewModels
{
	// Token: 0x02000008 RID: 8
	public class ChannelViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000037E4 File Offset: 0x000019E4
		public string Id
		{
			get
			{
				string id;
				if (this._isDM)
				{
					id = this._dmChannel.id;
				}
				else
				{
					id = this._channel.id;
				}
				return id;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000041 RID: 65 RVA: 0x0000381C File Offset: 0x00001A1C
		public string Name
		{
			get
			{
				string result;
				if (this._isDM && this._dmChannel.Recipient != null)
				{
					result = this._dmChannel.Recipient.username;
				}
				else if (this._channel != null)
				{
					result = this._channel.name;
				}
				else
				{
					result = "";
				}
				return result;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00003880 File Offset: 0x00001A80
		public string Topic
		{
			get
			{
				string result;
				if (this._channel != null && !string.IsNullOrEmpty(this._channel.topic))
				{
					result = this._channel.topic;
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000038C4 File Offset: 0x00001AC4
		public string IconUrl
		{
			get
			{
				string result;
				if (this._isDM && this._dmChannel.Recipient != null)
				{
					User recipient = this._dmChannel.Recipient;
					result = CDN.AvatarUrl(recipient.id, recipient.avatar, recipient.discriminator);
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000044 RID: 68 RVA: 0x0000391C File Offset: 0x00001B1C
		public User Recipient
		{
			get
			{
				User result;
				if (this._isDM && this._dmChannel.Recipient != null)
				{
					result = this._dmChannel.Recipient;
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000045 RID: 69 RVA: 0x0000395C File Offset: 0x00001B5C
		public int Type
		{
			get
			{
				int result;
				if (this._isDM)
				{
					result = 1;
				}
				else
				{
					result = this._channel.type;
				}
				return result;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000046 RID: 70 RVA: 0x0000398C File Offset: 0x00001B8C
		public bool IsTextChannel
		{
			get
			{
				return this.Type == 0;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000039A8 File Offset: 0x00001BA8
		public bool IsVoiceChannel
		{
			get
			{
				return this.Type == 2;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000039C4 File Offset: 0x00001BC4
		public bool IsCategory
		{
			get
			{
				return this.Type == 4;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000039E0 File Offset: 0x00001BE0
		public bool IsDM
		{
			get
			{
				return this._isDM;
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000039F8 File Offset: 0x00001BF8
		public ChannelViewModel(Channel channel)
		{
			this._channel = channel;
			this._isDM = false;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003A11 File Offset: 0x00001C11
		public ChannelViewModel(DirectMessageChannel dmChannel)
		{
			this._dmChannel = dmChannel;
			this._isDM = true;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600004C RID: 76 RVA: 0x00003A2C File Offset: 0x00001C2C
		// (remove) Token: 0x0600004D RID: 77 RVA: 0x00003A68 File Offset: 0x00001C68
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600004E RID: 78 RVA: 0x00003AA4 File Offset: 0x00001CA4
		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x04000021 RID: 33
		private Channel _channel;

		// Token: 0x04000022 RID: 34
		private DirectMessageChannel _dmChannel;

		// Token: 0x04000023 RID: 35
		private bool _isDM;
	}
}
