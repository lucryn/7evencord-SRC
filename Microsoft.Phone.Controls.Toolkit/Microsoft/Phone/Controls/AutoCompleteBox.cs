using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000066 RID: 102
	[StyleTypedProperty(Property = "TextBoxStyle", StyleTargetType = typeof(TextBox))]
	[TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
	[TemplateVisualState(Name = "PopupOpened", GroupName = "PopupStates")]
	[TemplateVisualState(Name = "PopupClosed", GroupName = "PopupStates")]
	[TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
	[TemplatePart(Name = "SelectionAdapter", Type = typeof(ISelectionAdapter))]
	[TemplatePart(Name = "Selector", Type = typeof(Selector))]
	[TemplatePart(Name = "Text", Type = typeof(TextBox))]
	[TemplatePart(Name = "Popup", Type = typeof(Popup))]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ListBox))]
	[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "Valid", GroupName = "ValidationStates")]
	[ContentProperty("ItemsSource")]
	[TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "Focused", GroupName = "FocusStates")]
	[TemplateVisualState(Name = "Pressed", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "InvalidFocused", GroupName = "ValidationStates")]
	[TemplateVisualState(Name = "InvalidUnfocused", GroupName = "ValidationStates")]
	public class AutoCompleteBox : Control, IUpdateVisualState
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0000F819 File Offset: 0x0000DA19
		// (set) Token: 0x06000382 RID: 898 RVA: 0x0000F821 File Offset: 0x0000DA21
		internal InteractionHelper Interaction { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000383 RID: 899 RVA: 0x0000F82A File Offset: 0x0000DA2A
		// (set) Token: 0x06000384 RID: 900 RVA: 0x0000F83C File Offset: 0x0000DA3C
		public int MinimumPrefixLength
		{
			get
			{
				return (int)base.GetValue(AutoCompleteBox.MinimumPrefixLengthProperty);
			}
			set
			{
				base.SetValue(AutoCompleteBox.MinimumPrefixLengthProperty, value);
			}
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000F850 File Offset: 0x0000DA50
		private static void OnMinimumPrefixLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			int num = (int)e.NewValue;
			if (num < 0 && num != -1)
			{
				throw new ArgumentOutOfRangeException("MinimumPrefixLength");
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0000F87D File Offset: 0x0000DA7D
		// (set) Token: 0x06000387 RID: 903 RVA: 0x0000F88F File Offset: 0x0000DA8F
		public int MinimumPopulateDelay
		{
			get
			{
				return (int)base.GetValue(AutoCompleteBox.MinimumPopulateDelayProperty);
			}
			set
			{
				base.SetValue(AutoCompleteBox.MinimumPopulateDelayProperty, value);
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000F8A4 File Offset: 0x0000DAA4
		private static void OnMinimumPopulateDelayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AutoCompleteBox autoCompleteBox = d as AutoCompleteBox;
			if (autoCompleteBox._ignorePropertyChange)
			{
				autoCompleteBox._ignorePropertyChange = false;
				return;
			}
			int num = (int)e.NewValue;
			if (num < 0)
			{
				autoCompleteBox._ignorePropertyChange = true;
				d.SetValue(e.Property, e.OldValue);
			}
			if (autoCompleteBox._delayTimer != null)
			{
				autoCompleteBox._delayTimer.Stop();
				if (num == 0)
				{
					autoCompleteBox._delayTimer = null;
				}
			}
			if (num > 0 && autoCompleteBox._delayTimer == null)
			{
				autoCompleteBox._delayTimer = new DispatcherTimer();
				autoCompleteBox._delayTimer.Tick += new EventHandler(autoCompleteBox.PopulateDropDown);
			}
			if (num > 0 && autoCompleteBox._delayTimer != null)
			{
				autoCompleteBox._delayTimer.Interval = TimeSpan.FromMilliseconds((double)num);
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000389 RID: 905 RVA: 0x0000F95D File Offset: 0x0000DB5D
		// (set) Token: 0x0600038A RID: 906 RVA: 0x0000F96F File Offset: 0x0000DB6F
		public bool IsTextCompletionEnabled
		{
			get
			{
				return (bool)base.GetValue(AutoCompleteBox.IsTextCompletionEnabledProperty);
			}
			set
			{
				base.SetValue(AutoCompleteBox.IsTextCompletionEnabledProperty, value);
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600038B RID: 907 RVA: 0x0000F982 File Offset: 0x0000DB82
		// (set) Token: 0x0600038C RID: 908 RVA: 0x0000F994 File Offset: 0x0000DB94
		public DataTemplate ItemTemplate
		{
			get
			{
				return base.GetValue(AutoCompleteBox.ItemTemplateProperty) as DataTemplate;
			}
			set
			{
				base.SetValue(AutoCompleteBox.ItemTemplateProperty, value);
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0000F9A2 File Offset: 0x0000DBA2
		// (set) Token: 0x0600038E RID: 910 RVA: 0x0000F9B4 File Offset: 0x0000DBB4
		public Style ItemContainerStyle
		{
			get
			{
				return base.GetValue(AutoCompleteBox.ItemContainerStyleProperty) as Style;
			}
			set
			{
				base.SetValue(AutoCompleteBox.ItemContainerStyleProperty, value);
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000F9C2 File Offset: 0x0000DBC2
		// (set) Token: 0x06000390 RID: 912 RVA: 0x0000F9D4 File Offset: 0x0000DBD4
		public Style TextBoxStyle
		{
			get
			{
				return base.GetValue(AutoCompleteBox.TextBoxStyleProperty) as Style;
			}
			set
			{
				base.SetValue(AutoCompleteBox.TextBoxStyleProperty, value);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000F9E2 File Offset: 0x0000DBE2
		// (set) Token: 0x06000392 RID: 914 RVA: 0x0000F9F4 File Offset: 0x0000DBF4
		public double MaxDropDownHeight
		{
			get
			{
				return (double)base.GetValue(AutoCompleteBox.MaxDropDownHeightProperty);
			}
			set
			{
				base.SetValue(AutoCompleteBox.MaxDropDownHeightProperty, value);
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000FA08 File Offset: 0x0000DC08
		private static void OnMaxDropDownHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AutoCompleteBox autoCompleteBox = d as AutoCompleteBox;
			if (autoCompleteBox._ignorePropertyChange)
			{
				autoCompleteBox._ignorePropertyChange = false;
				return;
			}
			double num = (double)e.NewValue;
			if (num < 0.0)
			{
				autoCompleteBox._ignorePropertyChange = true;
				autoCompleteBox.SetValue(e.Property, e.OldValue);
			}
			autoCompleteBox.OnMaxDropDownHeightChanged(num);
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000FA67 File Offset: 0x0000DC67
		// (set) Token: 0x06000395 RID: 917 RVA: 0x0000FA79 File Offset: 0x0000DC79
		public bool IsDropDownOpen
		{
			get
			{
				return (bool)base.GetValue(AutoCompleteBox.IsDropDownOpenProperty);
			}
			set
			{
				base.SetValue(AutoCompleteBox.IsDropDownOpenProperty, value);
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000FA8C File Offset: 0x0000DC8C
		private static void OnIsDropDownOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AutoCompleteBox autoCompleteBox = d as AutoCompleteBox;
			if (autoCompleteBox._ignorePropertyChange)
			{
				autoCompleteBox._ignorePropertyChange = false;
				return;
			}
			bool oldValue = (bool)e.OldValue;
			bool flag = (bool)e.NewValue;
			if (flag)
			{
				autoCompleteBox.TextUpdated(autoCompleteBox.Text, true);
			}
			else
			{
				autoCompleteBox.ClosingDropDown(oldValue);
			}
			autoCompleteBox.UpdateVisualState(true);
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000397 RID: 919 RVA: 0x0000FAEA File Offset: 0x0000DCEA
		// (set) Token: 0x06000398 RID: 920 RVA: 0x0000FAFC File Offset: 0x0000DCFC
		public IEnumerable ItemsSource
		{
			get
			{
				return base.GetValue(AutoCompleteBox.ItemsSourceProperty) as IEnumerable;
			}
			set
			{
				base.SetValue(AutoCompleteBox.ItemsSourceProperty, value);
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000FB0C File Offset: 0x0000DD0C
		private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AutoCompleteBox autoCompleteBox = d as AutoCompleteBox;
			autoCompleteBox.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0000FB3E File Offset: 0x0000DD3E
		// (set) Token: 0x0600039B RID: 923 RVA: 0x0000FB4B File Offset: 0x0000DD4B
		public object SelectedItem
		{
			get
			{
				return base.GetValue(AutoCompleteBox.SelectedItemProperty);
			}
			set
			{
				base.SetValue(AutoCompleteBox.SelectedItemProperty, value);
			}
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000FB5C File Offset: 0x0000DD5C
		private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AutoCompleteBox autoCompleteBox = d as AutoCompleteBox;
			if (autoCompleteBox._ignorePropertyChange)
			{
				autoCompleteBox._ignorePropertyChange = false;
				return;
			}
			if (autoCompleteBox._skipSelectedItemTextUpdate)
			{
				autoCompleteBox._skipSelectedItemTextUpdate = false;
			}
			else
			{
				autoCompleteBox.OnSelectedItemChanged(e.NewValue);
			}
			List<object> list = new List<object>();
			if (e.OldValue != null)
			{
				list.Add(e.OldValue);
			}
			List<object> list2 = new List<object>();
			if (e.NewValue != null)
			{
				list2.Add(e.NewValue);
			}
			autoCompleteBox.OnSelectionChanged(new SelectionChangedEventArgs(list, list2));
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000FBE4 File Offset: 0x0000DDE4
		private void OnSelectedItemChanged(object newItem)
		{
			string value;
			if (newItem == null)
			{
				value = this.SearchText;
			}
			else
			{
				value = this.FormatValue(newItem, true);
			}
			this.UpdateTextValue(value);
			if (this.TextBox != null && this.Text != null)
			{
				this.TextBox.SelectionStart = this.Text.Length;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0000FC33 File Offset: 0x0000DE33
		// (set) Token: 0x0600039F RID: 927 RVA: 0x0000FC45 File Offset: 0x0000DE45
		public string Text
		{
			get
			{
				return base.GetValue(AutoCompleteBox.TextProperty) as string;
			}
			set
			{
				base.SetValue(AutoCompleteBox.TextProperty, value);
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000FC54 File Offset: 0x0000DE54
		private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AutoCompleteBox autoCompleteBox = d as AutoCompleteBox;
			autoCompleteBox.TextUpdated((string)e.NewValue, false);
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0000FC7B File Offset: 0x0000DE7B
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x0000FC90 File Offset: 0x0000DE90
		public string SearchText
		{
			get
			{
				return (string)base.GetValue(AutoCompleteBox.SearchTextProperty);
			}
			private set
			{
				try
				{
					this._allowWrite = true;
					base.SetValue(AutoCompleteBox.SearchTextProperty, value);
				}
				finally
				{
					this._allowWrite = false;
				}
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000FCCC File Offset: 0x0000DECC
		private static void OnSearchTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AutoCompleteBox autoCompleteBox = d as AutoCompleteBox;
			if (autoCompleteBox._ignorePropertyChange)
			{
				autoCompleteBox._ignorePropertyChange = false;
				return;
			}
			if (!autoCompleteBox._allowWrite)
			{
				autoCompleteBox._ignorePropertyChange = true;
				autoCompleteBox.SetValue(e.Property, e.OldValue);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000FD13 File Offset: 0x0000DF13
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x0000FD25 File Offset: 0x0000DF25
		public AutoCompleteFilterMode FilterMode
		{
			get
			{
				return (AutoCompleteFilterMode)base.GetValue(AutoCompleteBox.FilterModeProperty);
			}
			set
			{
				base.SetValue(AutoCompleteBox.FilterModeProperty, value);
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000FD38 File Offset: 0x0000DF38
		private static void OnFilterModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AutoCompleteBox autoCompleteBox = d as AutoCompleteBox;
			AutoCompleteFilterMode autoCompleteFilterMode = (AutoCompleteFilterMode)e.NewValue;
			if (autoCompleteFilterMode != AutoCompleteFilterMode.Contains && autoCompleteFilterMode != AutoCompleteFilterMode.ContainsCaseSensitive && autoCompleteFilterMode != AutoCompleteFilterMode.ContainsOrdinal && autoCompleteFilterMode != AutoCompleteFilterMode.ContainsOrdinalCaseSensitive && autoCompleteFilterMode != AutoCompleteFilterMode.Custom && autoCompleteFilterMode != AutoCompleteFilterMode.Equals && autoCompleteFilterMode != AutoCompleteFilterMode.EqualsCaseSensitive && autoCompleteFilterMode != AutoCompleteFilterMode.EqualsOrdinal && autoCompleteFilterMode != AutoCompleteFilterMode.EqualsOrdinalCaseSensitive && autoCompleteFilterMode != AutoCompleteFilterMode.None && autoCompleteFilterMode != AutoCompleteFilterMode.StartsWith && autoCompleteFilterMode != AutoCompleteFilterMode.StartsWithCaseSensitive && autoCompleteFilterMode != AutoCompleteFilterMode.StartsWithOrdinal && autoCompleteFilterMode != AutoCompleteFilterMode.StartsWithOrdinalCaseSensitive)
			{
				autoCompleteBox.SetValue(e.Property, e.OldValue);
			}
			AutoCompleteFilterMode filterMode = (AutoCompleteFilterMode)e.NewValue;
			autoCompleteBox.TextFilter = AutoCompleteSearch.GetFilter(filterMode);
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000FDC2 File Offset: 0x0000DFC2
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x0000FDD4 File Offset: 0x0000DFD4
		public AutoCompleteFilterPredicate<object> ItemFilter
		{
			get
			{
				return base.GetValue(AutoCompleteBox.ItemFilterProperty) as AutoCompleteFilterPredicate<object>;
			}
			set
			{
				base.SetValue(AutoCompleteBox.ItemFilterProperty, value);
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000FDE4 File Offset: 0x0000DFE4
		private static void OnItemFilterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AutoCompleteBox autoCompleteBox = d as AutoCompleteBox;
			if (!(e.NewValue is AutoCompleteFilterPredicate<object>))
			{
				autoCompleteBox.FilterMode = AutoCompleteFilterMode.None;
				return;
			}
			autoCompleteBox.FilterMode = AutoCompleteFilterMode.Custom;
			autoCompleteBox.TextFilter = null;
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003AA RID: 938 RVA: 0x0000FE1F File Offset: 0x0000E01F
		// (set) Token: 0x060003AB RID: 939 RVA: 0x0000FE31 File Offset: 0x0000E031
		public AutoCompleteFilterPredicate<string> TextFilter
		{
			get
			{
				return base.GetValue(AutoCompleteBox.TextFilterProperty) as AutoCompleteFilterPredicate<string>;
			}
			set
			{
				base.SetValue(AutoCompleteBox.TextFilterProperty, value);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0000FE3F File Offset: 0x0000E03F
		// (set) Token: 0x060003AD RID: 941 RVA: 0x0000FE51 File Offset: 0x0000E051
		public InputScope InputScope
		{
			get
			{
				return (InputScope)base.GetValue(AutoCompleteBox.InputScopeProperty);
			}
			set
			{
				base.SetValue(AutoCompleteBox.InputScopeProperty, value);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0000FE5F File Offset: 0x0000E05F
		// (set) Token: 0x060003AF RID: 943 RVA: 0x0000FE67 File Offset: 0x0000E067
		private PopupHelper DropDownPopup { get; set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0000FE70 File Offset: 0x0000E070
		private static bool IsCompletionEnabled
		{
			get
			{
				PhoneApplicationFrame phoneApplicationFrame;
				return PhoneHelper.TryGetPhoneApplicationFrame(out phoneApplicationFrame) && phoneApplicationFrame.IsPortrait();
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0000FE8E File Offset: 0x0000E08E
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x0000FE98 File Offset: 0x0000E098
		internal TextBox TextBox
		{
			get
			{
				return this._text;
			}
			set
			{
				if (this._text != null)
				{
					this._text.SelectionChanged -= new RoutedEventHandler(this.OnTextBoxSelectionChanged);
					this._text.TextChanged -= new TextChangedEventHandler(this.OnTextBoxTextChanged);
				}
				this._text = value;
				if (this._text != null)
				{
					this._text.SelectionChanged += new RoutedEventHandler(this.OnTextBoxSelectionChanged);
					this._text.TextChanged += new TextChangedEventHandler(this.OnTextBoxTextChanged);
					if (this.Text != null)
					{
						this.UpdateTextValue(this.Text);
					}
				}
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0000FF2C File Offset: 0x0000E12C
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x0000FF34 File Offset: 0x0000E134
		protected internal ISelectionAdapter SelectionAdapter
		{
			get
			{
				return this._adapter;
			}
			set
			{
				if (this._adapter != null)
				{
					this._adapter.SelectionChanged -= new SelectionChangedEventHandler(this.OnAdapterSelectionChanged);
					this._adapter.Commit -= new RoutedEventHandler(this.OnAdapterSelectionComplete);
					this._adapter.Cancel -= new RoutedEventHandler(this.OnAdapterSelectionCanceled);
					this._adapter.Cancel -= new RoutedEventHandler(this.OnAdapterSelectionComplete);
					this._adapter.ItemsSource = null;
				}
				this._adapter = value;
				if (this._adapter != null)
				{
					this._adapter.SelectionChanged += new SelectionChangedEventHandler(this.OnAdapterSelectionChanged);
					this._adapter.Commit += new RoutedEventHandler(this.OnAdapterSelectionComplete);
					this._adapter.Cancel += new RoutedEventHandler(this.OnAdapterSelectionCanceled);
					this._adapter.Cancel += new RoutedEventHandler(this.OnAdapterSelectionComplete);
					this._adapter.ItemsSource = this._view;
				}
			}
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x060003B5 RID: 949 RVA: 0x00010030 File Offset: 0x0000E230
		// (remove) Token: 0x060003B6 RID: 950 RVA: 0x00010068 File Offset: 0x0000E268
		public event RoutedEventHandler TextChanged;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060003B7 RID: 951 RVA: 0x000100A0 File Offset: 0x0000E2A0
		// (remove) Token: 0x060003B8 RID: 952 RVA: 0x000100D8 File Offset: 0x0000E2D8
		public event PopulatingEventHandler Populating;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060003B9 RID: 953 RVA: 0x00010110 File Offset: 0x0000E310
		// (remove) Token: 0x060003BA RID: 954 RVA: 0x00010148 File Offset: 0x0000E348
		public event PopulatedEventHandler Populated;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x060003BB RID: 955 RVA: 0x00010180 File Offset: 0x0000E380
		// (remove) Token: 0x060003BC RID: 956 RVA: 0x000101B8 File Offset: 0x0000E3B8
		public event RoutedPropertyChangingEventHandler<bool> DropDownOpening;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x060003BD RID: 957 RVA: 0x000101F0 File Offset: 0x0000E3F0
		// (remove) Token: 0x060003BE RID: 958 RVA: 0x00010228 File Offset: 0x0000E428
		public event RoutedPropertyChangedEventHandler<bool> DropDownOpened;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x060003BF RID: 959 RVA: 0x00010260 File Offset: 0x0000E460
		// (remove) Token: 0x060003C0 RID: 960 RVA: 0x00010298 File Offset: 0x0000E498
		public event RoutedPropertyChangingEventHandler<bool> DropDownClosing;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x060003C1 RID: 961 RVA: 0x000102D0 File Offset: 0x0000E4D0
		// (remove) Token: 0x060003C2 RID: 962 RVA: 0x00010308 File Offset: 0x0000E508
		public event RoutedPropertyChangedEventHandler<bool> DropDownClosed;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060003C3 RID: 963 RVA: 0x00010340 File Offset: 0x0000E540
		// (remove) Token: 0x060003C4 RID: 964 RVA: 0x00010378 File Offset: 0x0000E578
		public event SelectionChangedEventHandler SelectionChanged;

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x000103AD File Offset: 0x0000E5AD
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x000103C4 File Offset: 0x0000E5C4
		public Binding ValueMemberBinding
		{
			get
			{
				if (this._valueBindingEvaluator == null)
				{
					return null;
				}
				return this._valueBindingEvaluator.ValueBinding;
			}
			set
			{
				this._valueBindingEvaluator = new BindingEvaluator<string>(value);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x000103D2 File Offset: 0x0000E5D2
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x000103EE File Offset: 0x0000E5EE
		public string ValueMemberPath
		{
			get
			{
				if (this.ValueMemberBinding == null)
				{
					return null;
				}
				return this.ValueMemberBinding.Path.Path;
			}
			set
			{
				this.ValueMemberBinding = ((value == null) ? null : new Binding(value));
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00010444 File Offset: 0x0000E644
		public AutoCompleteBox()
		{
			base.DefaultStyleKey = typeof(AutoCompleteBox);
			base.Loaded += delegate(object sender, RoutedEventArgs e)
			{
				base.ApplyTemplate();
			};
			base.Loaded += delegate(object A_1, RoutedEventArgs A_2)
			{
				PhoneApplicationFrame phoneApplicationFrame;
				if (PhoneHelper.TryGetPhoneApplicationFrame(out phoneApplicationFrame))
				{
					phoneApplicationFrame.OrientationChanged += delegate(object A_1, OrientationChangedEventArgs A_2)
					{
						this.IsDropDownOpen = false;
					};
				}
			};
			base.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.ControlIsEnabledChanged);
			this.Interaction = new InteractionHelper(this);
			this.ClearView();
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000104C0 File Offset: 0x0000E6C0
		protected override Size ArrangeOverride(Size finalSize)
		{
			Size result = base.ArrangeOverride(finalSize);
			if (this.DropDownPopup != null)
			{
				this.DropDownPopup.Arrange(new Size?(finalSize));
			}
			return result;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000104F0 File Offset: 0x0000E6F0
		public override void OnApplyTemplate()
		{
			if (this.TextBox != null)
			{
				this.TextBox.RemoveHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.OnUIElementKeyDown));
				this.TextBox.RemoveHandler(UIElement.KeyUpEvent, new KeyEventHandler(this.OnUIElementKeyUp));
			}
			if (this.DropDownPopup != null)
			{
				this.DropDownPopup.Closed -= new EventHandler(this.DropDownPopup_Closed);
				this.DropDownPopup.FocusChanged -= new EventHandler(this.OnDropDownFocusChanged);
				this.DropDownPopup.UpdateVisualStates -= new EventHandler(this.OnDropDownPopupUpdateVisualStates);
				this.DropDownPopup.BeforeOnApplyTemplate();
				this.DropDownPopup = null;
			}
			base.OnApplyTemplate();
			Popup popup = base.GetTemplateChild("Popup") as Popup;
			if (popup != null)
			{
				this.DropDownPopup = new PopupHelper(this, popup);
				this.DropDownPopup.MaxDropDownHeight = this.MaxDropDownHeight;
				this.DropDownPopup.AfterOnApplyTemplate();
				this.DropDownPopup.Closed += new EventHandler(this.DropDownPopup_Closed);
				this.DropDownPopup.FocusChanged += new EventHandler(this.OnDropDownFocusChanged);
				this.DropDownPopup.UpdateVisualStates += new EventHandler(this.OnDropDownPopupUpdateVisualStates);
			}
			this.SelectionAdapter = this.GetSelectionAdapterPart();
			this.TextBox = (base.GetTemplateChild("Text") as TextBox);
			if (this.TextBox != null)
			{
				this.TextBox.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.OnUIElementKeyDown), true);
				this.TextBox.AddHandler(UIElement.KeyUpEvent, new KeyEventHandler(this.OnUIElementKeyUp), true);
			}
			this.Interaction.OnApplyTemplateBase();
			if (this.IsDropDownOpen && this.DropDownPopup != null && !this.DropDownPopup.IsOpen)
			{
				this.OpeningDropDown(false);
			}
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000106B7 File Offset: 0x0000E8B7
		private void OnDropDownPopupUpdateVisualStates(object sender, EventArgs e)
		{
			this.UpdateVisualState(true);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000106C0 File Offset: 0x0000E8C0
		private void OnDropDownFocusChanged(object sender, EventArgs e)
		{
			this.FocusChanged(this.HasFocus());
		}

		// Token: 0x060003CE RID: 974 RVA: 0x000106D0 File Offset: 0x0000E8D0
		private void ClosingDropDown(bool oldValue)
		{
			bool flag = false;
			if (this.DropDownPopup != null)
			{
				flag = this.DropDownPopup.UsesClosingVisualState;
			}
			RoutedPropertyChangingEventArgs<bool> routedPropertyChangingEventArgs = new RoutedPropertyChangingEventArgs<bool>(AutoCompleteBox.IsDropDownOpenProperty, oldValue, false, true);
			this.OnDropDownClosing(routedPropertyChangingEventArgs);
			if (this._view == null || this._view.Count == 0)
			{
				flag = false;
			}
			if (routedPropertyChangingEventArgs.Cancel)
			{
				this._ignorePropertyChange = true;
				base.SetValue(AutoCompleteBox.IsDropDownOpenProperty, oldValue);
			}
			else if (!flag)
			{
				this.CloseDropDown(oldValue, false);
			}
			this.UpdateVisualState(true);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00010754 File Offset: 0x0000E954
		private void OpeningDropDown(bool oldValue)
		{
			if (!AutoCompleteBox.IsCompletionEnabled)
			{
				return;
			}
			RoutedPropertyChangingEventArgs<bool> routedPropertyChangingEventArgs = new RoutedPropertyChangingEventArgs<bool>(AutoCompleteBox.IsDropDownOpenProperty, oldValue, true, true);
			this.OnDropDownOpening(routedPropertyChangingEventArgs);
			if (routedPropertyChangingEventArgs.Cancel)
			{
				this._ignorePropertyChange = true;
				base.SetValue(AutoCompleteBox.IsDropDownOpenProperty, oldValue);
			}
			else
			{
				this.OpenDropDown(oldValue, true);
			}
			this.UpdateVisualState(true);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000107AF File Offset: 0x0000E9AF
		private void DropDownPopup_Closed(object sender, EventArgs e)
		{
			if (this.IsDropDownOpen)
			{
				this.IsDropDownOpen = false;
			}
			if (this._popupHasOpened)
			{
				this.OnDropDownClosed(new RoutedPropertyChangedEventArgs<bool>(true, false));
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000107D5 File Offset: 0x0000E9D5
		private void FocusChanged(bool hasFocus)
		{
			if (hasFocus)
			{
				if (this.TextBox != null && this.TextBox.SelectionLength == 0)
				{
					this.TextBox.Focus();
					return;
				}
			}
			else
			{
				this.IsDropDownOpen = false;
				this._userCalledPopulate = false;
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001080C File Offset: 0x0000EA0C
		protected bool HasFocus()
		{
			DependencyObject parent;
			for (DependencyObject dependencyObject = FocusManager.GetFocusedElement() as DependencyObject; dependencyObject != null; dependencyObject = parent)
			{
				if (object.ReferenceEquals(dependencyObject, this))
				{
					return true;
				}
				parent = VisualTreeHelper.GetParent(dependencyObject);
				if (parent == null)
				{
					FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
					if (frameworkElement != null)
					{
						parent = frameworkElement.Parent;
					}
				}
			}
			return false;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00010852 File Offset: 0x0000EA52
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			this.FocusChanged(this.HasFocus());
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00010867 File Offset: 0x0000EA67
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			this.FocusChanged(this.HasFocus());
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001087C File Offset: 0x0000EA7C
		private void ControlIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!(bool)e.NewValue)
			{
				this.IsDropDownOpen = false;
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000108A0 File Offset: 0x0000EAA0
		protected virtual ISelectionAdapter GetSelectionAdapterPart()
		{
			ISelectionAdapter selectionAdapter = null;
			Selector selector = base.GetTemplateChild("Selector") as Selector;
			if (selector != null)
			{
				selectionAdapter = (selector as ISelectionAdapter);
				if (selectionAdapter == null)
				{
					selectionAdapter = new SelectorSelectionAdapter(selector);
				}
			}
			if (selectionAdapter == null)
			{
				selectionAdapter = (base.GetTemplateChild("SelectionAdapter") as ISelectionAdapter);
			}
			return selectionAdapter;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000108EC File Offset: 0x0000EAEC
		private void PopulateDropDown(object sender, EventArgs e)
		{
			if (this._delayTimer != null)
			{
				this._delayTimer.Stop();
			}
			this.SearchText = this.Text;
			PopulatingEventArgs populatingEventArgs = new PopulatingEventArgs(this.SearchText);
			this.OnPopulating(populatingEventArgs);
			if (!populatingEventArgs.Cancel)
			{
				this.PopulateComplete();
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001093C File Offset: 0x0000EB3C
		protected virtual void OnPopulating(PopulatingEventArgs e)
		{
			PopulatingEventHandler populating = this.Populating;
			if (populating != null)
			{
				populating(this, e);
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001095C File Offset: 0x0000EB5C
		protected virtual void OnPopulated(PopulatedEventArgs e)
		{
			PopulatedEventHandler populated = this.Populated;
			if (populated != null)
			{
				populated(this, e);
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001097C File Offset: 0x0000EB7C
		protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			SelectionChangedEventHandler selectionChanged = this.SelectionChanged;
			if (selectionChanged != null)
			{
				selectionChanged.Invoke(this, e);
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001099C File Offset: 0x0000EB9C
		protected virtual void OnDropDownOpening(RoutedPropertyChangingEventArgs<bool> e)
		{
			RoutedPropertyChangingEventHandler<bool> dropDownOpening = this.DropDownOpening;
			if (dropDownOpening != null)
			{
				dropDownOpening(this, e);
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000109BC File Offset: 0x0000EBBC
		protected virtual void OnDropDownOpened(RoutedPropertyChangedEventArgs<bool> e)
		{
			RoutedPropertyChangedEventHandler<bool> dropDownOpened = this.DropDownOpened;
			if (dropDownOpened != null)
			{
				dropDownOpened.Invoke(this, e);
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000109DC File Offset: 0x0000EBDC
		protected virtual void OnDropDownClosing(RoutedPropertyChangingEventArgs<bool> e)
		{
			RoutedPropertyChangingEventHandler<bool> dropDownClosing = this.DropDownClosing;
			if (dropDownClosing != null)
			{
				dropDownClosing(this, e);
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000109FC File Offset: 0x0000EBFC
		protected virtual void OnDropDownClosed(RoutedPropertyChangedEventArgs<bool> e)
		{
			RoutedPropertyChangedEventHandler<bool> dropDownClosed = this.DropDownClosed;
			if (dropDownClosed != null)
			{
				dropDownClosed.Invoke(this, e);
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00010A1C File Offset: 0x0000EC1C
		private string FormatValue(object value, bool clearDataContext)
		{
			string result = this.FormatValue(value);
			if (clearDataContext && this._valueBindingEvaluator != null)
			{
				this._valueBindingEvaluator.ClearDataContext();
			}
			return result;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00010A48 File Offset: 0x0000EC48
		protected virtual string FormatValue(object value)
		{
			if (this._valueBindingEvaluator != null)
			{
				return this._valueBindingEvaluator.GetDynamicValue(value) ?? string.Empty;
			}
			if (value != null)
			{
				return value.ToString();
			}
			return string.Empty;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00010A78 File Offset: 0x0000EC78
		protected virtual void OnTextChanged(RoutedEventArgs e)
		{
			RoutedEventHandler textChanged = this.TextChanged;
			if (textChanged != null)
			{
				textChanged.Invoke(this, e);
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00010A97 File Offset: 0x0000EC97
		private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
		{
			this.TextUpdated(this._text.Text, true);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00010AAB File Offset: 0x0000ECAB
		private void OnTextBoxSelectionChanged(object sender, RoutedEventArgs e)
		{
			if (this._ignoreTextSelectionChange || this._inputtingText)
			{
				return;
			}
			this._textSelectionStart = this._text.SelectionStart;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00010ACF File Offset: 0x0000ECCF
		private void OnUIElementKeyDown(object sender, KeyEventArgs e)
		{
			this._inputtingText = true;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00010AD8 File Offset: 0x0000ECD8
		private void OnUIElementKeyUp(object sender, KeyEventArgs e)
		{
			this._inputtingText = false;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00010AE4 File Offset: 0x0000ECE4
		private void UpdateTextValue(string value)
		{
			this.UpdateTextValue(value, default(bool?));
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00010B04 File Offset: 0x0000ED04
		private void UpdateTextValue(string value, bool? userInitiated)
		{
			if ((userInitiated == null || userInitiated == true) && this.Text != value)
			{
				this._ignoreTextPropertyChange++;
				this.Text = value;
				this.OnTextChanged(new RoutedEventArgs());
			}
			if ((userInitiated == null || userInitiated == false) && this.TextBox != null && this.TextBox.Text != value)
			{
				this._ignoreTextPropertyChange++;
				this.TextBox.Text = (value ?? string.Empty);
				if (this.Text == value || this.Text == null)
				{
					this.OnTextChanged(new RoutedEventArgs());
				}
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00010BE0 File Offset: 0x0000EDE0
		private void TextUpdated(string newText, bool userInitiated)
		{
			if (this._ignoreTextPropertyChange > 0)
			{
				this._ignoreTextPropertyChange--;
				return;
			}
			if (newText == null)
			{
				newText = string.Empty;
			}
			if (this.IsTextCompletionEnabled && this.TextBox != null && this.TextBox.SelectionLength > 0 && this.TextBox.SelectionStart != this.TextBox.Text.Length)
			{
				return;
			}
			bool flag = newText.Length >= this.MinimumPrefixLength && this.MinimumPrefixLength >= 0;
			this._userCalledPopulate = (flag && userInitiated);
			this.UpdateTextValue(newText, new bool?(userInitiated));
			if (!flag)
			{
				this.SearchText = string.Empty;
				if (this.SelectedItem != null)
				{
					this._skipSelectedItemTextUpdate = true;
				}
				this.SelectedItem = null;
				if (this.IsDropDownOpen)
				{
					this.IsDropDownOpen = false;
				}
				return;
			}
			this._ignoreTextSelectionChange = true;
			if (this._delayTimer != null)
			{
				this._delayTimer.Start();
				return;
			}
			this.PopulateDropDown(this, EventArgs.Empty);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00010CE0 File Offset: 0x0000EEE0
		public void PopulateComplete()
		{
			this.RefreshView();
			PopulatedEventArgs e = new PopulatedEventArgs(new ReadOnlyCollection<object>(this._view));
			this.OnPopulated(e);
			if (this.SelectionAdapter != null && this.SelectionAdapter.ItemsSource != this._view)
			{
				this.SelectionAdapter.ItemsSource = this._view;
			}
			bool flag = this._userCalledPopulate && this._view.Count > 0;
			if (flag != this.IsDropDownOpen)
			{
				this._ignorePropertyChange = true;
				this.IsDropDownOpen = flag;
			}
			if (this.IsDropDownOpen)
			{
				this.OpeningDropDown(false);
				if (this.DropDownPopup != null)
				{
					this.DropDownPopup.Arrange(default(Size?));
				}
			}
			else
			{
				this.ClosingDropDown(true);
			}
			this.UpdateTextCompletion(this._userCalledPopulate);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00010DA8 File Offset: 0x0000EFA8
		private void UpdateTextCompletion(bool userInitiated)
		{
			object obj = null;
			string text = this.Text;
			if (this._view.Count > 0)
			{
				if (this.IsTextCompletionEnabled && this.TextBox != null && userInitiated)
				{
					int length = this.TextBox.Text.Length;
					int selectionStart = this.TextBox.SelectionStart;
					if (selectionStart == text.Length && selectionStart > this._textSelectionStart)
					{
						object obj2 = (this.FilterMode == AutoCompleteFilterMode.StartsWith || this.FilterMode == AutoCompleteFilterMode.StartsWithCaseSensitive) ? this._view[0] : this.TryGetMatch(text, this._view, AutoCompleteSearch.GetFilter(AutoCompleteFilterMode.StartsWith));
						if (obj2 != null)
						{
							obj = obj2;
							string text2 = this.FormatValue(obj2, true);
							int num = Math.Min(text2.Length, this.Text.Length);
							if (AutoCompleteSearch.Equals(this.Text.Substring(0, num), text2.Substring(0, num)))
							{
								this.UpdateTextValue(text2);
								this.TextBox.SelectionStart = length;
								this.TextBox.SelectionLength = text2.Length - length;
							}
						}
					}
				}
				else
				{
					obj = this.TryGetMatch(text, this._view, AutoCompleteSearch.GetFilter(AutoCompleteFilterMode.EqualsCaseSensitive));
				}
			}
			if (this.SelectedItem != obj)
			{
				this._skipSelectedItemTextUpdate = true;
			}
			this.SelectedItem = obj;
			if (this._ignoreTextSelectionChange)
			{
				this._ignoreTextSelectionChange = false;
				if (this.TextBox != null && !this._inputtingText)
				{
					this._textSelectionStart = this.TextBox.SelectionStart;
				}
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00010F28 File Offset: 0x0000F128
		private object TryGetMatch(string searchText, ObservableCollection<object> view, AutoCompleteFilterPredicate<string> predicate)
		{
			if (view != null && view.Count > 0)
			{
				foreach (object obj in view)
				{
					if (predicate(searchText, this.FormatValue(obj)))
					{
						return obj;
					}
				}
			}
			return null;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00010F8C File Offset: 0x0000F18C
		private void ClearView()
		{
			if (this._view == null)
			{
				this._view = new ObservableCollection<object>();
				return;
			}
			this._view.Clear();
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00010FB0 File Offset: 0x0000F1B0
		private void RefreshView()
		{
			if (this._items == null)
			{
				this.ClearView();
				return;
			}
			string search = this.Text ?? string.Empty;
			bool flag = this.TextFilter != null;
			bool flag2 = this.FilterMode == AutoCompleteFilterMode.Custom && this.TextFilter == null;
			int num = 0;
			int num2 = this._view.Count;
			List<object> items = this._items;
			foreach (object obj in items)
			{
				bool flag3 = !flag && !flag2;
				if (!flag3)
				{
					flag3 = (flag ? this.TextFilter(search, this.FormatValue(obj)) : this.ItemFilter(search, obj));
				}
				if (num2 > num && flag3 && this._view[num] == obj)
				{
					num++;
				}
				else if (flag3)
				{
					if (num2 > num && this._view[num] != obj)
					{
						this._view.RemoveAt(num);
						this._view.Insert(num, obj);
						num++;
					}
					else
					{
						if (num == num2)
						{
							this._view.Add(obj);
						}
						else
						{
							this._view.Insert(num, obj);
						}
						num++;
						num2++;
					}
				}
				else if (num2 > num && this._view[num] == obj)
				{
					this._view.RemoveAt(num);
					num2--;
				}
			}
			if (this._valueBindingEvaluator != null)
			{
				this._valueBindingEvaluator.ClearDataContext();
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00011190 File Offset: 0x0000F390
		private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			INotifyCollectionChanged notifyCollectionChanged = oldValue as INotifyCollectionChanged;
			if (notifyCollectionChanged != null && this._collectionChangedWeakEventListener != null)
			{
				this._collectionChangedWeakEventListener.Detach();
				this._collectionChangedWeakEventListener = null;
			}
			INotifyCollectionChanged newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
			if (newValueINotifyCollectionChanged != null)
			{
				this._collectionChangedWeakEventListener = new WeakEventListener<AutoCompleteBox, object, NotifyCollectionChangedEventArgs>(this);
				this._collectionChangedWeakEventListener.OnEventAction = delegate(AutoCompleteBox instance, object source, NotifyCollectionChangedEventArgs eventArgs)
				{
					instance.ItemsSourceCollectionChanged(source, eventArgs);
				};
				this._collectionChangedWeakEventListener.OnDetachAction = delegate(WeakEventListener<AutoCompleteBox, object, NotifyCollectionChangedEventArgs> weakEventListener)
				{
					newValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(weakEventListener.OnEvent);
				};
				newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(this._collectionChangedWeakEventListener.OnEvent);
			}
			this._items = ((newValue == null) ? null : new List<object>(Enumerable.ToList<object>(Enumerable.Cast<object>(newValue))));
			this.ClearView();
			if (this.SelectionAdapter != null && this.SelectionAdapter.ItemsSource != this._view)
			{
				this.SelectionAdapter.ItemsSource = this._view;
			}
			if (this.IsDropDownOpen)
			{
				this.RefreshView();
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x000112A8 File Offset: 0x0000F4A8
		private void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == 1 && e.OldItems != null)
			{
				for (int i = 0; i < e.OldItems.Count; i++)
				{
					this._items.RemoveAt(e.OldStartingIndex);
				}
			}
			if (e.Action == null && e.NewItems != null && this._items.Count >= e.NewStartingIndex)
			{
				for (int j = 0; j < e.NewItems.Count; j++)
				{
					this._items.Insert(e.NewStartingIndex + j, e.NewItems[j]);
				}
			}
			if (e.Action == 2 && e.NewItems != null && e.OldItems != null)
			{
				for (int k = 0; k < e.NewItems.Count; k++)
				{
					this._items[e.NewStartingIndex] = e.NewItems[k];
				}
			}
			if (e.Action == 1 || e.Action == 2)
			{
				for (int l = 0; l < e.OldItems.Count; l++)
				{
					this._view.Remove(e.OldItems[l]);
				}
			}
			if (e.Action == 4)
			{
				this.ClearView();
				if (this.ItemsSource != null)
				{
					this._items = new List<object>(Enumerable.ToList<object>(Enumerable.Cast<object>(this.ItemsSource)));
				}
			}
			this.RefreshView();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00011409 File Offset: 0x0000F609
		private void OnAdapterSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.SelectedItem = this._adapter.SelectedItem;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001141C File Offset: 0x0000F61C
		private void OnAdapterSelectionComplete(object sender, RoutedEventArgs e)
		{
			this.IsDropDownOpen = false;
			this.UpdateTextCompletion(false);
			if (this.TextBox != null)
			{
				this.TextBox.Select(this.TextBox.Text.Length, 0);
			}
			base.Focus();
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00011457 File Offset: 0x0000F657
		private void OnAdapterSelectionCanceled(object sender, RoutedEventArgs e)
		{
			this.UpdateTextValue(this.SearchText);
			this.UpdateTextCompletion(false);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001146C File Offset: 0x0000F66C
		private void OnMaxDropDownHeightChanged(double newValue)
		{
			if (this.DropDownPopup != null)
			{
				this.DropDownPopup.MaxDropDownHeight = newValue;
				this.DropDownPopup.Arrange(default(Size?));
			}
			this.UpdateVisualState(true);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000114A8 File Offset: 0x0000F6A8
		private void OpenDropDown(bool oldValue, bool newValue)
		{
			if (this.DropDownPopup != null)
			{
				this.DropDownPopup.IsOpen = true;
			}
			this._popupHasOpened = true;
			this.OnDropDownOpened(new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue));
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000114D2 File Offset: 0x0000F6D2
		private void CloseDropDown(bool oldValue, bool newValue)
		{
			if (this._popupHasOpened)
			{
				if (this.SelectionAdapter != null)
				{
					this.SelectionAdapter.SelectedItem = null;
				}
				if (this.DropDownPopup != null)
				{
					this.DropDownPopup.IsOpen = false;
				}
				this.OnDropDownClosed(new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue));
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00011514 File Offset: 0x0000F714
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.OnKeyDown(e);
			if (e.Handled || !base.IsEnabled)
			{
				return;
			}
			if (this.IsDropDownOpen)
			{
				if (this.SelectionAdapter != null)
				{
					this.SelectionAdapter.HandleKeyDown(e);
					if (e.Handled)
					{
						return;
					}
				}
				if (e.Key == 8)
				{
					this.OnAdapterSelectionCanceled(this, new RoutedEventArgs());
					e.Handled = true;
				}
			}
			else if (e.Key == 17)
			{
				this.IsDropDownOpen = true;
				e.Handled = true;
			}
			Key key = e.Key;
			if (key == 3)
			{
				this.OnAdapterSelectionComplete(this, new RoutedEventArgs());
				e.Handled = true;
				return;
			}
			if (key != 59)
			{
				return;
			}
			this.IsDropDownOpen = !this.IsDropDownOpen;
			e.Handled = true;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x000115DD File Offset: 0x0000F7DD
		void IUpdateVisualState.UpdateVisualState(bool useTransitions)
		{
			this.UpdateVisualState(useTransitions);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x000115E6 File Offset: 0x0000F7E6
		internal virtual void UpdateVisualState(bool useTransitions)
		{
			VisualStateManager.GoToState(this, this.IsDropDownOpen ? "PopupOpened" : "PopupClosed", useTransitions);
			this.Interaction.UpdateVisualStateBase(useTransitions);
		}

		// Token: 0x040001D1 RID: 465
		private const string ElementSelectionAdapter = "SelectionAdapter";

		// Token: 0x040001D2 RID: 466
		private const string ElementSelector = "Selector";

		// Token: 0x040001D3 RID: 467
		private const string ElementPopup = "Popup";

		// Token: 0x040001D4 RID: 468
		private const string ElementTextBox = "Text";

		// Token: 0x040001D5 RID: 469
		private const string ElementTextBoxStyle = "TextBoxStyle";

		// Token: 0x040001D6 RID: 470
		private const string ElementItemContainerStyle = "ItemContainerStyle";

		// Token: 0x040001D7 RID: 471
		private List<object> _items;

		// Token: 0x040001D8 RID: 472
		private ObservableCollection<object> _view;

		// Token: 0x040001D9 RID: 473
		private int _ignoreTextPropertyChange;

		// Token: 0x040001DA RID: 474
		private bool _ignorePropertyChange;

		// Token: 0x040001DB RID: 475
		private bool _ignoreTextSelectionChange;

		// Token: 0x040001DC RID: 476
		private bool _skipSelectedItemTextUpdate;

		// Token: 0x040001DD RID: 477
		private int _textSelectionStart;

		// Token: 0x040001DE RID: 478
		private bool _inputtingText;

		// Token: 0x040001DF RID: 479
		private bool _userCalledPopulate;

		// Token: 0x040001E0 RID: 480
		private bool _popupHasOpened;

		// Token: 0x040001E1 RID: 481
		private DispatcherTimer _delayTimer;

		// Token: 0x040001E2 RID: 482
		private bool _allowWrite;

		// Token: 0x040001E3 RID: 483
		private BindingEvaluator<string> _valueBindingEvaluator;

		// Token: 0x040001E4 RID: 484
		private WeakEventListener<AutoCompleteBox, object, NotifyCollectionChangedEventArgs> _collectionChangedWeakEventListener;

		// Token: 0x040001E5 RID: 485
		public static readonly DependencyProperty MinimumPrefixLengthProperty = DependencyProperty.Register("MinimumPrefixLength", typeof(int), typeof(AutoCompleteBox), new PropertyMetadata(1, new PropertyChangedCallback(AutoCompleteBox.OnMinimumPrefixLengthPropertyChanged)));

		// Token: 0x040001E6 RID: 486
		public static readonly DependencyProperty MinimumPopulateDelayProperty = DependencyProperty.Register("MinimumPopulateDelay", typeof(int), typeof(AutoCompleteBox), new PropertyMetadata(new PropertyChangedCallback(AutoCompleteBox.OnMinimumPopulateDelayPropertyChanged)));

		// Token: 0x040001E7 RID: 487
		public static readonly DependencyProperty IsTextCompletionEnabledProperty = DependencyProperty.Register("IsTextCompletionEnabled", typeof(bool), typeof(AutoCompleteBox), new PropertyMetadata(false, null));

		// Token: 0x040001E8 RID: 488
		public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(AutoCompleteBox), new PropertyMetadata(null));

		// Token: 0x040001E9 RID: 489
		public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(AutoCompleteBox), new PropertyMetadata(null, null));

		// Token: 0x040001EA RID: 490
		public static readonly DependencyProperty TextBoxStyleProperty = DependencyProperty.Register("TextBoxStyle", typeof(Style), typeof(AutoCompleteBox), new PropertyMetadata(null));

		// Token: 0x040001EB RID: 491
		public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(AutoCompleteBox), new PropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(AutoCompleteBox.OnMaxDropDownHeightPropertyChanged)));

		// Token: 0x040001EC RID: 492
		public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(AutoCompleteBox), new PropertyMetadata(false, new PropertyChangedCallback(AutoCompleteBox.OnIsDropDownOpenPropertyChanged)));

		// Token: 0x040001ED RID: 493
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(AutoCompleteBox), new PropertyMetadata(new PropertyChangedCallback(AutoCompleteBox.OnItemsSourcePropertyChanged)));

		// Token: 0x040001EE RID: 494
		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(AutoCompleteBox), new PropertyMetadata(new PropertyChangedCallback(AutoCompleteBox.OnSelectedItemPropertyChanged)));

		// Token: 0x040001EF RID: 495
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteBox), new PropertyMetadata(string.Empty, new PropertyChangedCallback(AutoCompleteBox.OnTextPropertyChanged)));

		// Token: 0x040001F0 RID: 496
		public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText", typeof(string), typeof(AutoCompleteBox), new PropertyMetadata(string.Empty, new PropertyChangedCallback(AutoCompleteBox.OnSearchTextPropertyChanged)));

		// Token: 0x040001F1 RID: 497
		public static readonly DependencyProperty FilterModeProperty = DependencyProperty.Register("FilterMode", typeof(AutoCompleteFilterMode), typeof(AutoCompleteBox), new PropertyMetadata(AutoCompleteFilterMode.StartsWith, new PropertyChangedCallback(AutoCompleteBox.OnFilterModePropertyChanged)));

		// Token: 0x040001F2 RID: 498
		public static readonly DependencyProperty ItemFilterProperty = DependencyProperty.Register("ItemFilter", typeof(AutoCompleteFilterPredicate<object>), typeof(AutoCompleteBox), new PropertyMetadata(new PropertyChangedCallback(AutoCompleteBox.OnItemFilterPropertyChanged)));

		// Token: 0x040001F3 RID: 499
		public static readonly DependencyProperty TextFilterProperty = DependencyProperty.Register("TextFilter", typeof(AutoCompleteFilterPredicate<string>), typeof(AutoCompleteBox), new PropertyMetadata(AutoCompleteSearch.GetFilter(AutoCompleteFilterMode.StartsWith)));

		// Token: 0x040001F4 RID: 500
		public static readonly DependencyProperty InputScopeProperty = DependencyProperty.Register("InputScope", typeof(InputScope), typeof(AutoCompleteBox), null);

		// Token: 0x040001F5 RID: 501
		private TextBox _text;

		// Token: 0x040001F6 RID: 502
		private ISelectionAdapter _adapter;
	}
}
