using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace W7Cord.Views
{
	// Token: 0x02000016 RID: 22
	public class TermsPage : PhoneApplicationPage
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x00005670 File Offset: 0x00003870
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!this._contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/W7Cord;component/Views/TermsPage.xaml", 2));
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
				this.MainTransform = (TranslateTransform)base.FindName("MainTransform");
				this.ContentPanel = (StackPanel)base.FindName("ContentPanel");
				this.AcceptButton = (Button)base.FindName("AcceptButton");
				this.DeclineButton = (Button)base.FindName("DeclineButton");
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005718 File Offset: 0x00003918
		public TermsPage()
		{
			this.InitializeComponent();
			Storyboard storyboard = base.Resources["ShowTermsStoryboard"] as Storyboard;
			if (storyboard != null)
			{
				storyboard.Begin();
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000575C File Offset: 0x0000395C
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if (IsolatedStorageSettings.ApplicationSettings.Contains("TermsAccepted") && (bool)IsolatedStorageSettings.ApplicationSettings["TermsAccepted"])
			{
				base.NavigationService.Navigate(new Uri("/Views/LoginPage.xaml", 2));
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000057BA File Offset: 0x000039BA
		private void AcceptButton_Click(object sender, RoutedEventArgs e)
		{
			IsolatedStorageSettings.ApplicationSettings["TermsAccepted"] = true;
			IsolatedStorageSettings.ApplicationSettings.Save();
			base.NavigationService.Navigate(new Uri("/Views/LoginPage.xaml", 2));
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000057F8 File Offset: 0x000039F8
		private void DeclineButton_Click(object sender, RoutedEventArgs e)
		{
			while (base.NavigationService.CanGoBack)
			{
				base.NavigationService.RemoveBackEntry();
			}
			MessageBox.Show("You must agree to the terms to use this app. The app will now close.", "Terms Required", 0);
		}

		// Token: 0x04000065 RID: 101
		internal Grid LayoutRoot;

		// Token: 0x04000066 RID: 102
		internal TranslateTransform MainTransform;

		// Token: 0x04000067 RID: 103
		internal StackPanel ContentPanel;

		// Token: 0x04000068 RID: 104
		internal Button AcceptButton;

		// Token: 0x04000069 RID: 105
		internal Button DeclineButton;

		// Token: 0x0400006A RID: 106
		private bool _contentLoaded;
	}
}
