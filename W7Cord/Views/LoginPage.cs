using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using W7Cord.Models;

namespace W7Cord.Views
{
	// Token: 0x0200000C RID: 12
	public class LoginPage : PhoneApplicationPage
	{
		// Token: 0x06000066 RID: 102 RVA: 0x000046D2 File Offset: 0x000028D2
		public LoginPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000046F0 File Offset: 0x000028F0
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			Storyboard storyboard = base.Resources["ShowLoginStoryboard"] as Storyboard;
			if (storyboard != null)
			{
				storyboard.Begin();
			}
			if (IsolatedStorageSettings.ApplicationSettings.Contains("LoginToken"))
			{
				string text = (string)IsolatedStorageSettings.ApplicationSettings["LoginToken"];
				if (!string.IsNullOrEmpty(text))
				{
					this.NavigateToMain();
				}
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004924 File Offset: 0x00002B24
		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			string token = this.TokenTextBox.Password.Trim();
			if (string.IsNullOrEmpty(token))
			{
				this.ShowError("Please enter your Discord token");
			}
			else
			{
				string baseUrl = this._bridgeUrl;
				if (!baseUrl.EndsWith("/"))
				{
					baseUrl += "/";
				}
				CDN.SetProxyBaseUrl(baseUrl);
				Debug.WriteLine("Base URL: " + baseUrl);
				Debug.WriteLine("Token length: " + token.Length);
				this.LoginButton.IsEnabled = false;
				this.ErrorText.Visibility = 1;
				this._testClient = new DiscordWebClient(token, baseUrl);
				if (this.TestBrowserHost.Child != null)
				{
					this.TestBrowserHost.Child = null;
				}
				this.TestBrowserHost.Child = this._testClient.GetBrowser();
				this._testClient.GetCurrentUser(delegate(User user)
				{
					Deployment.Current.Dispatcher.BeginInvoke(delegate()
					{
						Debug.WriteLine("SUCCESS! User: " + user.username);
						IsolatedStorageSettings.ApplicationSettings["LoginToken"] = token;
						IsolatedStorageSettings.ApplicationSettings["BridgeUrl"] = baseUrl;
						IsolatedStorageSettings.ApplicationSettings["UserId"] = user.id;
						IsolatedStorageSettings.ApplicationSettings["Username"] = user.username;
						IsolatedStorageSettings.ApplicationSettings.Save();
						App.DiscordWebClient = this._testClient;
						this.NavigateToMain();
					});
				}, delegate(string error)
				{
					Deployment.Current.Dispatcher.BeginInvoke(delegate()
					{
						Debug.WriteLine("ERROR: " + error);
						this.LoginButton.IsEnabled = true;
						this.ShowError("Login failed: " + error);
					});
				});
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004A80 File Offset: 0x00002C80
		private void TestProxyButton_Click(object sender, RoutedEventArgs e)
		{
			string text = this._bridgeUrl;
			if (!text.EndsWith("/"))
			{
				text += "/";
			}
			this.TestProxyButton.IsEnabled = false;
			this.ErrorText.Visibility = 1;
			DiscordWebClient discordWebClient = new DiscordWebClient("test_token", text);
			if (this.TestBrowserHost.Child != null)
			{
				this.TestBrowserHost.Child = null;
			}
			this.TestBrowserHost.Child = discordWebClient.GetBrowser();
			MessageBox.Show("Testing bridge connection...", "Test", 0);
			this.TestProxyButton.IsEnabled = true;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004B25 File Offset: 0x00002D25
		private void NavigateToMain()
		{
			base.NavigationService.Navigate(new Uri("/Views/MainPage.xaml", 2));
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004B3F File Offset: 0x00002D3F
		private void ShowError(string message)
		{
			this.ErrorText.Text = message;
			this.ErrorText.Visibility = 0;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004B5C File Offset: 0x00002D5C
		private void TokenTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004B60 File Offset: 0x00002D60
		private void TokenTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(this.TokenTextBox.Password))
			{
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004B88 File Offset: 0x00002D88
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!this._contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/W7Cord;component/Views/LoginPage.xaml", 2));
				this.ShowLoginStoryboard = (Storyboard)base.FindName("ShowLoginStoryboard");
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
				this.MainTransform = (TranslateTransform)base.FindName("MainTransform");
				this.ContentPanel = (StackPanel)base.FindName("ContentPanel");
				this.TokenTextBox = (PasswordBox)base.FindName("TokenTextBox");
				this.PasswordWatermark = (TextBlock)base.FindName("PasswordWatermark");
				this.LoginButton = (Button)base.FindName("LoginButton");
				this.TestProxyButton = (Button)base.FindName("TestProxyButton");
				this.ErrorText = (TextBlock)base.FindName("ErrorText");
				this.TestBrowserHost = (Border)base.FindName("TestBrowserHost");
			}
		}

		// Token: 0x04000032 RID: 50
		private DiscordWebClient _testClient;

		// Token: 0x04000033 RID: 51
		private readonly string _bridgeUrl = "http://hazeulmanataga120499.42web.io/";

		// Token: 0x04000034 RID: 52
		internal Storyboard ShowLoginStoryboard;

		// Token: 0x04000035 RID: 53
		internal Grid LayoutRoot;

		// Token: 0x04000036 RID: 54
		internal TranslateTransform MainTransform;

		// Token: 0x04000037 RID: 55
		internal StackPanel ContentPanel;

		// Token: 0x04000038 RID: 56
		internal PasswordBox TokenTextBox;

		// Token: 0x04000039 RID: 57
		internal TextBlock PasswordWatermark;

		// Token: 0x0400003A RID: 58
		internal Button LoginButton;

		// Token: 0x0400003B RID: 59
		internal Button TestProxyButton;

		// Token: 0x0400003C RID: 60
		internal TextBlock ErrorText;

		// Token: 0x0400003D RID: 61
		internal Border TestBrowserHost;

		// Token: 0x0400003E RID: 62
		private bool _contentLoaded;
	}
}
