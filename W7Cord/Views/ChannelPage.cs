using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using W7Cord.Models;
using W7Cord.ViewModels;

namespace W7Cord.Views
{
	// Token: 0x02000002 RID: 2
	public class ChannelPage : PhoneApplicationPage
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!this._contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/W7Cord;component/Views/ChannelPage.xaml", 2));
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
				this.ChannelTitleText = (TextBlock)base.FindName("ChannelTitleText");
				this.ChannelSubtitleText = (TextBlock)base.FindName("ChannelSubtitleText");
				this.ContentPanel = (Grid)base.FindName("ContentPanel");
				this.MessagesList = (ListBox)base.FindName("MessagesList");
				this.MessageTextBox = (TextBox)base.FindName("MessageTextBox");
				this.RefreshButton = (ApplicationBarIconButton)base.FindName("RefreshButton");
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002124 File Offset: 0x00000324
		public ChannelPage()
		{
			this.InitializeComponent();
			this._messages = new ObservableCollection<MessageViewModel>();
			this.MessagesList.ItemsSource = this._messages;
			this.MessageTextBox.KeyDown += new KeyEventHandler(this.MessageTextBox_KeyDown);
			this._liveUpdateTimer = new DispatcherTimer();
			this._liveUpdateTimer.Interval = TimeSpan.FromSeconds(5.0);
			this._liveUpdateTimer.Tick += new EventHandler(this.LiveUpdateTimer_Tick);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000021C8 File Offset: 0x000003C8
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if (base.NavigationContext.QueryString.ContainsKey("channelId"))
			{
				this._channelId = base.NavigationContext.QueryString["channelId"];
			}
			if (base.NavigationContext.QueryString.ContainsKey("title"))
			{
				this._channelTitle = base.NavigationContext.QueryString["title"];
				this.ChannelTitleText.Text = this._channelTitle;
				this.ChannelSubtitleText.Text = "text channel";
			}
			this.LoadMessages();
			this.StartLiveUpdates();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002281 File Offset: 0x00000481
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			this.StopLiveUpdates();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002294 File Offset: 0x00000494
		private void StartLiveUpdates()
		{
			if (!this._isLiveUpdatesEnabled)
			{
				this._isLiveUpdatesEnabled = true;
				this._liveUpdateTimer.Start();
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022C4 File Offset: 0x000004C4
		private void StopLiveUpdates()
		{
			if (this._isLiveUpdatesEnabled)
			{
				this._isLiveUpdatesEnabled = false;
				this._liveUpdateTimer.Stop();
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000024E8 File Offset: 0x000006E8
		private void LiveUpdateTimer_Tick(object sender, EventArgs e)
		{
			if (this._isLiveUpdatesEnabled && !string.IsNullOrEmpty(this._channelId))
			{
				if (App.DiscordWebClient == null)
				{
					string token = (string)IsolatedStorageSettings.ApplicationSettings["LoginToken"];
					string bridgeUrl = (string)IsolatedStorageSettings.ApplicationSettings["BridgeUrl"];
					App.DiscordWebClient = new DiscordWebClient(token, bridgeUrl);
				}
				App.DiscordWebClient.GetMessages(this._channelId, 10, delegate(Message[] messages)
				{
					Deployment.Current.Dispatcher.BeginInvoke(delegate()
					{
						if (messages != null && messages.Length > 0)
						{
							string currentUserId = "";
							if (IsolatedStorageSettings.ApplicationSettings.Contains("UserId"))
							{
								currentUserId = (string)IsolatedStorageSettings.ApplicationSettings["UserId"];
							}
							bool flag = false;
							string id = messages[0].id;
							if (string.IsNullOrEmpty(this._lastMessageId))
							{
								this._lastMessageId = id;
							}
							else
							{
								for (int i = messages.Length - 1; i >= 0; i--)
								{
									if (messages[i].id == this._lastMessageId)
									{
										break;
									}
									flag = true;
									MessageViewModel messageViewModel = new MessageViewModel(messages[i], currentUserId);
									this._messages.Add(messageViewModel);
								}
								if (flag)
								{
									this._lastMessageId = id;
									if (this.MessagesList.Items.Count > 0)
									{
										this.MessagesList.ScrollIntoView(this.MessagesList.Items[this.MessagesList.Items.Count - 1]);
									}
									this.SetMessageTemplates();
								}
							}
						}
					});
				}, delegate(string error)
				{
					Debug.WriteLine("Live update error: " + error);
				});
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000025A0 File Offset: 0x000007A0
		private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == 3)
			{
				e.Handled = true;
				this.SendMessage();
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000027C0 File Offset: 0x000009C0
		private void LoadMessages()
		{
			this.ShowLoading(true);
			this._messages.Clear();
			this._lastMessageId = "";
			if (App.DiscordWebClient == null)
			{
				string token = (string)IsolatedStorageSettings.ApplicationSettings["LoginToken"];
				string bridgeUrl = (string)IsolatedStorageSettings.ApplicationSettings["BridgeUrl"];
				App.DiscordWebClient = new DiscordWebClient(token, bridgeUrl);
			}
			App.DiscordWebClient.GetMessages(this._channelId, 30, delegate(Message[] messages)
			{
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					if (messages != null)
					{
						string currentUserId = "";
						if (IsolatedStorageSettings.ApplicationSettings.Contains("UserId"))
						{
							currentUserId = (string)IsolatedStorageSettings.ApplicationSettings["UserId"];
						}
						if (messages.Length > 0)
						{
							this._lastMessageId = messages[0].id;
						}
						for (int i = messages.Length - 1; i >= 0; i--)
						{
							MessageViewModel messageViewModel = new MessageViewModel(messages[i], currentUserId);
							this._messages.Add(messageViewModel);
						}
						if (this.MessagesList.Items.Count > 0)
						{
							this.MessagesList.ScrollIntoView(this.MessagesList.Items[this.MessagesList.Items.Count - 1]);
						}
						this.SetMessageTemplates();
					}
					this.ShowLoading(false);
				});
			}, delegate(string error)
			{
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					this.ShowLoading(false);
					MessageBox.Show("Error loading messages: " + error);
				});
			});
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002860 File Offset: 0x00000A60
		private void SetMessageTemplates()
		{
			for (int i = 0; i < this.MessagesList.Items.Count; i++)
			{
				MessageViewModel messageViewModel = this.MessagesList.Items[i] as MessageViewModel;
				if (messageViewModel != null)
				{
					ListBoxItem listBoxItem = this.MessagesList.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
					if (listBoxItem != null)
					{
						ContentControl contentControl = this.FindVisualChild<ContentControl>(listBoxItem);
						if (contentControl != null)
						{
							if (messageViewModel.IsOutgoing)
							{
								contentControl.ContentTemplate = (DataTemplate)base.Resources["OutgoingTemplate"];
							}
							else
							{
								contentControl.ContentTemplate = (DataTemplate)base.Resources["IncomingTemplate"];
							}
						}
					}
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002944 File Offset: 0x00000B44
		private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
		{
			int i = 0;
			while (i < VisualTreeHelper.GetChildrenCount(parent))
			{
				DependencyObject child = VisualTreeHelper.GetChild(parent, i);
				T result;
				if (child != null && child is T)
				{
					result = (T)((object)child);
				}
				else
				{
					T t = this.FindVisualChild<T>(child);
					if (t == null)
					{
						i++;
						continue;
					}
					result = t;
				}
				return result;
			}
			return default(T);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000029BF File Offset: 0x00000BBF
		private void SendButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendMessage();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002B80 File Offset: 0x00000D80
		private void SendMessage()
		{
			string text = this.MessageTextBox.Text.Trim();
			if (!string.IsNullOrEmpty(text))
			{
				string messageToSend = text;
				this.MessageTextBox.Text = "";
				App.DiscordWebClient.SendMessage(this._channelId, messageToSend, delegate(Message msg)
				{
					Deployment.Current.Dispatcher.BeginInvoke(delegate()
					{
						string currentUserId = "";
						if (IsolatedStorageSettings.ApplicationSettings.Contains("UserId"))
						{
							currentUserId = (string)IsolatedStorageSettings.ApplicationSettings["UserId"];
						}
						MessageViewModel messageViewModel = new MessageViewModel(msg, currentUserId);
						this._messages.Add(messageViewModel);
						this._lastMessageId = msg.id;
						if (this.MessagesList.Items.Count > 0)
						{
							this.MessagesList.ScrollIntoView(this.MessagesList.Items[this.MessagesList.Items.Count - 1]);
						}
						this.SetMessageTemplates();
					});
				}, delegate(string error)
				{
					Deployment.Current.Dispatcher.BeginInvoke(delegate()
					{
						this.MessageTextBox.Text = messageToSend;
						MessageBox.Show("Error sending message: " + error);
					});
				});
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002C04 File Offset: 0x00000E04
		private void RefreshButton_Click(object sender, EventArgs e)
		{
			this.LoadMessages();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002C11 File Offset: 0x00000E11
		private void ShowLoading(bool show)
		{
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
			});
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002C43 File Offset: 0x00000E43
		private void UploadButton_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Upload not implemented yet");
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002C51 File Offset: 0x00000E51
		private void PinToStart_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Pin to start not implemented yet");
		}

		// Token: 0x04000001 RID: 1
		internal Grid LayoutRoot;

		// Token: 0x04000002 RID: 2
		internal TextBlock ChannelTitleText;

		// Token: 0x04000003 RID: 3
		internal TextBlock ChannelSubtitleText;

		// Token: 0x04000004 RID: 4
		internal Grid ContentPanel;

		// Token: 0x04000005 RID: 5
		internal ListBox MessagesList;

		// Token: 0x04000006 RID: 6
		internal TextBox MessageTextBox;

		// Token: 0x04000007 RID: 7
		internal ApplicationBarIconButton RefreshButton;

		// Token: 0x04000008 RID: 8
		private bool _contentLoaded;

		// Token: 0x04000009 RID: 9
		private string _channelId;

		// Token: 0x0400000A RID: 10
		private string _channelTitle;

		// Token: 0x0400000B RID: 11
		private ObservableCollection<MessageViewModel> _messages;

		// Token: 0x0400000C RID: 12
		private DispatcherTimer _liveUpdateTimer;

		// Token: 0x0400000D RID: 13
		private string _lastMessageId = "";

		// Token: 0x0400000E RID: 14
		private bool _isLiveUpdatesEnabled = false;
	}
}
