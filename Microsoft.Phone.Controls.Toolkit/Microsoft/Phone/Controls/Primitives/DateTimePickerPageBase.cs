using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls.Properties;
using Microsoft.Phone.Shell;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x02000024 RID: 36
	public abstract class DateTimePickerPageBase : PhoneApplicationPage, IDateTimePickerPage
	{
		// Token: 0x06000110 RID: 272 RVA: 0x0000606C File Offset: 0x0000426C
		protected void InitializeDateTimePickerPage(LoopingSelector primarySelector, LoopingSelector secondarySelector, LoopingSelector tertiarySelector)
		{
			if (primarySelector == null)
			{
				throw new ArgumentNullException("primarySelector");
			}
			if (secondarySelector == null)
			{
				throw new ArgumentNullException("secondarySelector");
			}
			if (tertiarySelector == null)
			{
				throw new ArgumentNullException("tertiarySelector");
			}
			this._primarySelectorPart = primarySelector;
			this._secondarySelectorPart = secondarySelector;
			this._tertiarySelectorPart = tertiarySelector;
			this._primarySelectorPart.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.HandleDataSourceSelectionChanged);
			this._secondarySelectorPart.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.HandleDataSourceSelectionChanged);
			this._tertiarySelectorPart.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.HandleDataSourceSelectionChanged);
			this._primarySelectorPart.IsExpandedChanged += new DependencyPropertyChangedEventHandler(this.HandleSelectorIsExpandedChanged);
			this._secondarySelectorPart.IsExpandedChanged += new DependencyPropertyChangedEventHandler(this.HandleSelectorIsExpandedChanged);
			this._tertiarySelectorPart.IsExpandedChanged += new DependencyPropertyChangedEventHandler(this.HandleSelectorIsExpandedChanged);
			this._primarySelectorPart.Visibility = 1;
			this._secondarySelectorPart.Visibility = 1;
			this._tertiarySelectorPart.Visibility = 1;
			int num = 0;
			foreach (LoopingSelector loopingSelector in this.GetSelectorsOrderedByCulturePattern())
			{
				Grid.SetColumn(loopingSelector, num);
				loopingSelector.Visibility = 0;
				num++;
			}
			FrameworkElement frameworkElement = VisualTreeHelper.GetChild(this, 0) as FrameworkElement;
			if (frameworkElement != null)
			{
				foreach (object obj in VisualStateManager.GetVisualStateGroups(frameworkElement))
				{
					VisualStateGroup visualStateGroup = (VisualStateGroup)obj;
					if ("VisibilityStates" == visualStateGroup.Name)
					{
						foreach (object obj2 in visualStateGroup.States)
						{
							VisualState visualState = (VisualState)obj2;
							if ("Closed" == visualState.Name && visualState.Storyboard != null)
							{
								this._closedStoryboard = visualState.Storyboard;
								this._closedStoryboard.Completed += new EventHandler(this.HandleClosedStoryboardCompleted);
							}
						}
					}
				}
			}
			if (base.ApplicationBar != null)
			{
				foreach (object obj3 in base.ApplicationBar.Buttons)
				{
					IApplicationBarIconButton applicationBarIconButton = obj3 as IApplicationBarIconButton;
					if (applicationBarIconButton != null)
					{
						if ("DONE" == applicationBarIconButton.Text)
						{
							applicationBarIconButton.Text = Resources.DateTimePickerDoneText;
							applicationBarIconButton.Click += new EventHandler(this.HandleDoneButtonClick);
						}
						else if ("CANCEL" == applicationBarIconButton.Text)
						{
							applicationBarIconButton.Text = Resources.DateTimePickerCancelText;
							applicationBarIconButton.Click += new EventHandler(this.HandleCancelButtonClick);
						}
					}
				}
			}
			VisualStateManager.GoToState(this, "Open", true);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000639C File Offset: 0x0000459C
		private void HandleDataSourceSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DataSource dataSource = (DataSource)sender;
			this._primarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
			this._secondarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
			this._tertiarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000063F4 File Offset: 0x000045F4
		private void HandleSelectorIsExpandedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				this._primarySelectorPart.IsExpanded = (sender == this._primarySelectorPart);
				this._secondarySelectorPart.IsExpanded = (sender == this._secondarySelectorPart);
				this._tertiarySelectorPart.IsExpanded = (sender == this._tertiarySelectorPart);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000644B File Offset: 0x0000464B
		private void HandleDoneButtonClick(object sender, EventArgs e)
		{
			this._value = new DateTime?(((DateTimeWrapper)this._primarySelectorPart.DataSource.SelectedItem).DateTime);
			this.ClosePickerPage();
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00006478 File Offset: 0x00004678
		private void HandleCancelButtonClick(object sender, EventArgs e)
		{
			this._value = default(DateTime?);
			this.ClosePickerPage();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000648C File Offset: 0x0000468C
		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			e.Cancel = true;
			this.ClosePickerPage();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000064A9 File Offset: 0x000046A9
		private void ClosePickerPage()
		{
			if (this._closedStoryboard != null)
			{
				VisualStateManager.GoToState(this, "Closed", true);
				return;
			}
			this.HandleClosedStoryboardCompleted(null, null);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000064C9 File Offset: 0x000046C9
		private void HandleClosedStoryboardCompleted(object sender, EventArgs e)
		{
			base.NavigationService.GoBack();
		}

		// Token: 0x06000118 RID: 280
		protected abstract IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern();

		// Token: 0x06000119 RID: 281 RVA: 0x00006500 File Offset: 0x00004700
		protected static IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern(string pattern, char[] patternCharacters, LoopingSelector[] selectors)
		{
			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}
			if (patternCharacters == null)
			{
				throw new ArgumentNullException("patternCharacters");
			}
			if (selectors == null)
			{
				throw new ArgumentNullException("selectors");
			}
			if (patternCharacters.Length != selectors.Length)
			{
				throw new ArgumentException("Arrays must contain the same number of elements.");
			}
			List<Tuple<int, LoopingSelector>> list = new List<Tuple<int, LoopingSelector>>(patternCharacters.Length);
			for (int i = 0; i < patternCharacters.Length; i++)
			{
				list.Add(new Tuple<int, LoopingSelector>(pattern.IndexOf(patternCharacters[i]), selectors[i]));
			}
			return Enumerable.Where<LoopingSelector>(Enumerable.Select<Tuple<int, LoopingSelector>, LoopingSelector>(Enumerable.OrderBy<Tuple<int, LoopingSelector>, int>(Enumerable.Where<Tuple<int, LoopingSelector>>(list, (Tuple<int, LoopingSelector> p) => -1 != p.Item1), (Tuple<int, LoopingSelector> p) => p.Item1), (Tuple<int, LoopingSelector> p) => p.Item2), (LoopingSelector s) => null != s);
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00006601 File Offset: 0x00004801
		// (set) Token: 0x0600011B RID: 283 RVA: 0x0000660C File Offset: 0x0000480C
		public DateTime? Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				DateTimeWrapper selectedItem = new DateTimeWrapper(this._value.GetValueOrDefault(DateTime.Now));
				this._primarySelectorPart.DataSource.SelectedItem = selectedItem;
				this._secondarySelectorPart.DataSource.SelectedItem = selectedItem;
				this._tertiarySelectorPart.DataSource.SelectedItem = selectedItem;
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000666C File Offset: 0x0000486C
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.OnNavigatedFrom(e);
			if ("app://external/" == e.Uri.ToString())
			{
				base.State["DateTimePickerPageBase_State_Value"] = this.Value;
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000066C0 File Offset: 0x000048C0
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.OnNavigatedTo(e);
			if (base.State.ContainsKey("DateTimePickerPageBase_State_Value"))
			{
				this.Value = (base.State["DateTimePickerPageBase_State_Value"] as DateTime?);
				if (base.NavigationService.CanGoBack)
				{
					base.NavigationService.GoBack();
				}
			}
		}

		// Token: 0x04000067 RID: 103
		private const string VisibilityGroupName = "VisibilityStates";

		// Token: 0x04000068 RID: 104
		private const string OpenVisibilityStateName = "Open";

		// Token: 0x04000069 RID: 105
		private const string ClosedVisibilityStateName = "Closed";

		// Token: 0x0400006A RID: 106
		private const string StateKey_Value = "DateTimePickerPageBase_State_Value";

		// Token: 0x0400006B RID: 107
		private LoopingSelector _primarySelectorPart;

		// Token: 0x0400006C RID: 108
		private LoopingSelector _secondarySelectorPart;

		// Token: 0x0400006D RID: 109
		private LoopingSelector _tertiarySelectorPart;

		// Token: 0x0400006E RID: 110
		private Storyboard _closedStoryboard;

		// Token: 0x0400006F RID: 111
		private DateTime? _value;
	}
}
