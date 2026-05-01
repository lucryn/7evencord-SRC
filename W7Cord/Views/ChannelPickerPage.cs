using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using W7Cord.Models;
using W7Cord.ViewModels;

namespace W7Cord.Views
{
	// Token: 0x02000012 RID: 18
	public class ChannelPickerPage : PhoneApplicationPage
	{
		// Token: 0x060000AE RID: 174 RVA: 0x00005084 File Offset: 0x00003284
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!this._contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/W7Cord;component/Views/ChannelPickerPage.xaml", 2));
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
				this.GuildNameText = (TextBlock)base.FindName("GuildNameText");
				this.ContentPanel = (Grid)base.FindName("ContentPanel");
				this.ChannelsList = (ListBox)base.FindName("ChannelsList");
				this.LoadingOverlay = (Grid)base.FindName("LoadingOverlay");
				this.RefreshButton = (ApplicationBarIconButton)base.FindName("RefreshButton");
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005142 File Offset: 0x00003342
		public ChannelPickerPage()
		{
			this.InitializeComponent();
			this._channels = new ObservableCollection<ChannelViewModel>();
			this.ChannelsList.ItemsSource = this._channels;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005174 File Offset: 0x00003374
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if (base.NavigationContext.QueryString.ContainsKey("id"))
			{
				this._guildId = base.NavigationContext.QueryString["id"];
			}
			if (IsolatedStorageSettings.ApplicationSettings.Contains("Guilds"))
			{
				List<Guild> list = (List<Guild>)IsolatedStorageSettings.ApplicationSettings["Guilds"];
				if (list != null)
				{
					foreach (Guild guild in list)
					{
						if (guild.id == this._guildId)
						{
							this._guildName = guild.name;
							break;
						}
					}
				}
			}
			this.GuildNameText.Text = (string.IsNullOrEmpty(this._guildName) ? "channels" : this._guildName);
			this.LoadChannels();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000053B4 File Offset: 0x000035B4
		private void LoadChannels()
		{
			this.ShowLoading(true);
			this._channels.Clear();
			if (App.DiscordWebClient == null)
			{
				string token = (string)IsolatedStorageSettings.ApplicationSettings["LoginToken"];
				string bridgeUrl = (string)IsolatedStorageSettings.ApplicationSettings["BridgeUrl"];
				App.DiscordWebClient = new DiscordWebClient(token, bridgeUrl);
			}
			App.DiscordWebClient.GetChannels(this._guildId, delegate(Channel[] channels)
			{
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					if (channels != null)
					{
						int num = Math.Min(channels.Length, 30);
						for (int i = 0; i < num; i++)
						{
							this._channels.Add(new ChannelViewModel(channels[i]));
						}
					}
					this.ShowLoading(false);
				});
			}, delegate(string error)
			{
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					this.ShowLoading(false);
					MessageBox.Show("Error loading channels: " + error);
				});
			});
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005448 File Offset: 0x00003648
		private void ChannelsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.ChannelsList.SelectedItem != null)
			{
				ChannelViewModel channelViewModel = this.ChannelsList.SelectedItem as ChannelViewModel;
				this.ChannelsList.SelectedItem = null;
				if (channelViewModel != null && channelViewModel.Type == 0)
				{
					base.NavigationService.Navigate(new Uri(string.Format("/Views/ChannelPage.xaml?channelId={0}&title={1}", channelViewModel.Id, channelViewModel.Name), 2));
				}
				else if (channelViewModel != null && channelViewModel.Type == 2)
				{
					MessageBox.Show("Voice channels are not supported yet");
				}
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000054F3 File Offset: 0x000036F3
		private void RefreshButton_Click(object sender, EventArgs e)
		{
			this.LoadChannels();
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005548 File Offset: 0x00003748
		private void ShowLoading(bool show)
		{
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
				if (this.LoadingOverlay != null)
				{
					this.LoadingOverlay.Visibility = (show ? 0 : 1);
				}
			});
		}

		// Token: 0x0400005B RID: 91
		internal Grid LayoutRoot;

		// Token: 0x0400005C RID: 92
		internal TextBlock GuildNameText;

		// Token: 0x0400005D RID: 93
		internal Grid ContentPanel;

		// Token: 0x0400005E RID: 94
		internal ListBox ChannelsList;

		// Token: 0x0400005F RID: 95
		internal Grid LoadingOverlay;

		// Token: 0x04000060 RID: 96
		internal ApplicationBarIconButton RefreshButton;

		// Token: 0x04000061 RID: 97
		private bool _contentLoaded;

		// Token: 0x04000062 RID: 98
		private string _guildId;

		// Token: 0x04000063 RID: 99
		private string _guildName;

		// Token: 0x04000064 RID: 100
		private ObservableCollection<ChannelViewModel> _channels;
	}
}
