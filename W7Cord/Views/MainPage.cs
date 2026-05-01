using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using W7Cord.Models;
using W7Cord.ViewModels;

namespace W7Cord.Views
{
	// Token: 0x02000005 RID: 5
	public class MainPage : PhoneApplicationPage
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002EEC File Offset: 0x000010EC
		public MainPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002F00 File Offset: 0x00001100
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if (!IsolatedStorageSettings.ApplicationSettings.Contains("LoginToken"))
			{
				base.NavigationService.Navigate(new Uri("/Views/LoginPage.xaml", 2));
			}
			else
			{
				if (this._viewModel == null)
				{
					this._viewModel = App.ViewModel;
					base.DataContext = this._viewModel;
				}
				this.LoadData();
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002F74 File Offset: 0x00001174
		private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
		{
			if (base.NavigationService.CanGoBack)
			{
				while (base.NavigationService.CanGoBack)
				{
					base.NavigationService.RemoveBackEntry();
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000030C8 File Offset: 0x000012C8
		private void LoadData()
		{
			if (IsolatedStorageSettings.ApplicationSettings.Contains("BridgeUrl"))
			{
				string text = (string)IsolatedStorageSettings.ApplicationSettings["BridgeUrl"];
				if (!text.EndsWith("/"))
				{
					text += "/";
				}
				CDN.SetProxyBaseUrl(text);
			}
			if (App.DiscordWebClient == null)
			{
				string token = (string)IsolatedStorageSettings.ApplicationSettings["LoginToken"];
				string text = (string)IsolatedStorageSettings.ApplicationSettings["BridgeUrl"];
				if (!text.EndsWith("/"))
				{
					text += "/";
				}
				App.DiscordWebClient = new DiscordWebClient(token, text);
				if (this.WebBrowserHost != null)
				{
					this.WebBrowserHost.Child = App.DiscordWebClient.GetBrowser();
				}
			}
			this.ShowLoading(true);
			App.DiscordWebClient.GetGuilds(18, delegate(Guild[] guilds)
			{
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					this._viewModel.Guilds.Clear();
					Guild[] guilds;
					if (guilds != null)
					{
						List<Guild> list = new List<Guild>();
						foreach (Guild guild in guilds)
						{
							this._viewModel.Guilds.Add(new GuildViewModel(guild));
							list.Add(guild);
						}
						IsolatedStorageSettings.ApplicationSettings["Guilds"] = list;
						IsolatedStorageSettings.ApplicationSettings.Save();
					}
					this.LoadDMs();
				});
			}, delegate(string error)
			{
				this.ShowLoading(false);
				this.ShowError("Guild Error: " + error);
			});
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000032B8 File Offset: 0x000014B8
		private void LoadDMs()
		{
			if (App.DiscordWebClient == null)
			{
				string token = (string)IsolatedStorageSettings.ApplicationSettings["LoginToken"];
				string text = (string)IsolatedStorageSettings.ApplicationSettings["BridgeUrl"];
				if (!text.EndsWith("/"))
				{
					text += "/";
				}
				App.DiscordWebClient = new DiscordWebClient(token, text);
			}
			App.DiscordWebClient.GetDirectMessageChannels(delegate(DirectMessageChannel[] dms)
			{
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					this._viewModel.DirectMessages.Clear();
					DirectMessageChannel[] dms;
					if (dms != null)
					{
						foreach (DirectMessageChannel dmChannel in dms)
						{
							this._viewModel.DirectMessages.Add(new ChannelViewModel(dmChannel));
						}
					}
					this.ShowLoading(false);
				});
			}, delegate(string error)
			{
				this.ShowLoading(false);
				this.ShowError("DM Error: " + error);
			});
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000334C File Offset: 0x0000154C
		private void GuildsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.GuildsList.SelectedItem != null)
			{
				GuildViewModel guildViewModel = this.GuildsList.SelectedItem as GuildViewModel;
				this.GuildsList.SelectedItem = null;
				if (guildViewModel != null)
				{
					base.NavigationService.Navigate(new Uri(string.Format("/Views/ChannelPickerPage.xaml?id={0}", guildViewModel.Id), 2));
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000033BC File Offset: 0x000015BC
		private void DMsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.DMsList.SelectedItem != null)
			{
				ChannelViewModel channelViewModel = this.DMsList.SelectedItem as ChannelViewModel;
				this.DMsList.SelectedItem = null;
				if (channelViewModel != null)
				{
					base.NavigationService.Navigate(new Uri(string.Format("/Views/ChannelPage.xaml?channelId={0}&title={1}", channelViewModel.Id, channelViewModel.Name), 2));
				}
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003431 File Offset: 0x00001631
		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			this.LoadData();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000343C File Offset: 0x0000163C
		private void SignOutButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to sign out?", "Sign Out", 1);
			if (messageBoxResult == 1)
			{
				if (IsolatedStorageSettings.ApplicationSettings.Remove("LoginToken"))
				{
					IsolatedStorageSettings.ApplicationSettings.Remove("UserId");
					IsolatedStorageSettings.ApplicationSettings.Remove("Username");
					IsolatedStorageSettings.ApplicationSettings.Remove("BridgeUrl");
					IsolatedStorageSettings.ApplicationSettings.Remove("Guilds");
					IsolatedStorageSettings.ApplicationSettings.Save();
					App.DiscordWebClient = null;
					base.NavigationService.Navigate(new Uri("/Views/LoginPage.xaml", 2));
				}
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000034EF File Offset: 0x000016EF
		private void ShowLoading(bool show)
		{
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
			});
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003544 File Offset: 0x00001744
		private void ShowError(string error)
		{
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
				MessageBox.Show("Error: " + error);
			});
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000357C File Offset: 0x0000177C
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!this._contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/W7Cord;component/Views/MainPage.xaml", 2));
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
				this.GuildsList = (ListBox)base.FindName("GuildsList");
				this.DMsList = (ListBox)base.FindName("DMsList");
				this.RefreshButton = (Button)base.FindName("RefreshButton");
				this.SignOutButton = (Button)base.FindName("SignOutButton");
				this.WebBrowserHost = (Border)base.FindName("WebBrowserHost");
			}
		}

		// Token: 0x04000014 RID: 20
		private MainViewModel _viewModel;

		// Token: 0x04000015 RID: 21
		internal Grid LayoutRoot;

		// Token: 0x04000016 RID: 22
		internal ListBox GuildsList;

		// Token: 0x04000017 RID: 23
		internal ListBox DMsList;

		// Token: 0x04000018 RID: 24
		internal Button RefreshButton;

		// Token: 0x04000019 RID: 25
		internal Button SignOutButton;

		// Token: 0x0400001A RID: 26
		internal Border WebBrowserHost;

		// Token: 0x0400001B RID: 27
		private bool _contentLoaded;
	}
}
