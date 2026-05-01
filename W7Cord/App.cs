using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Phone.Controls;
using W7Cord.Models;
using W7Cord.ViewModels;

namespace W7Cord
{
	// Token: 0x02000003 RID: 3
	public class App : Application
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002C60 File Offset: 0x00000E60
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!this._contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/W7Cord;component/App.xaml", 2));
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002C98 File Offset: 0x00000E98
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002CAE File Offset: 0x00000EAE
		public static DiscordWebClient DiscordWebClient { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002CB8 File Offset: 0x00000EB8
		public static MainViewModel ViewModel
		{
			get
			{
				if (App._viewModel == null)
				{
					App._viewModel = new MainViewModel();
				}
				return App._viewModel;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002CE8 File Offset: 0x00000EE8
		public App()
		{
			base.Startup += new StartupEventHandler(this.Application_Startup);
			base.Exit += new EventHandler(this.Application_Exit);
			base.UnhandledException += new EventHandler<ApplicationUnhandledExceptionEventArgs>(this.Application_UnhandledException);
			this.InitializeComponent();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002D40 File Offset: 0x00000F40
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			PhoneApplicationFrame phoneApplicationFrame = new PhoneApplicationFrame();
			base.RootVisual = phoneApplicationFrame;
			if (!IsolatedStorageSettings.ApplicationSettings.Contains("TermsAccepted") || !(bool)IsolatedStorageSettings.ApplicationSettings["TermsAccepted"])
			{
				phoneApplicationFrame.Navigate(new Uri("/Views/TermsPage.xaml", 2));
			}
			else
			{
				phoneApplicationFrame.Navigate(new Uri("/Views/LoginPage.xaml", 2));
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002DB2 File Offset: 0x00000FB2
		private void Application_Exit(object sender, EventArgs e)
		{
			App.DiscordWebClient = null;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002DBC File Offset: 0x00000FBC
		private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			if (Debugger.IsAttached)
			{
				Debugger.Break();
			}
		}

		// Token: 0x04000011 RID: 17
		private bool _contentLoaded;

		// Token: 0x04000012 RID: 18
		private static MainViewModel _viewModel;
	}
}
