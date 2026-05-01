using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using Microsoft.Phone.Controls.Primitives;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000056 RID: 86
	[TemplatePart(Name = "DateTimeButton", Type = typeof(ButtonBase))]
	public class DateTimePickerBase : Control
	{
		// Token: 0x1400001E RID: 30
		// (add) Token: 0x0600031F RID: 799 RVA: 0x0000E15C File Offset: 0x0000C35C
		// (remove) Token: 0x06000320 RID: 800 RVA: 0x0000E194 File Offset: 0x0000C394
		public event EventHandler<DateTimeValueChangedEventArgs> ValueChanged;

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000321 RID: 801 RVA: 0x0000E1C9 File Offset: 0x0000C3C9
		// (set) Token: 0x06000322 RID: 802 RVA: 0x0000E1DB File Offset: 0x0000C3DB
		[TypeConverter(typeof(TimeTypeConverter))]
		public DateTime? Value
		{
			get
			{
				return (DateTime?)base.GetValue(DateTimePickerBase.ValueProperty);
			}
			set
			{
				base.SetValue(DateTimePickerBase.ValueProperty, value);
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000E1EE File Offset: 0x0000C3EE
		private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((DateTimePickerBase)o).OnValueChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000E213 File Offset: 0x0000C413
		private void OnValueChanged(DateTime? oldValue, DateTime? newValue)
		{
			this.UpdateValueString();
			this.OnValueChanged(new DateTimeValueChangedEventArgs(oldValue, newValue));
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000E228 File Offset: 0x0000C428
		protected virtual void OnValueChanged(DateTimeValueChangedEventArgs e)
		{
			EventHandler<DateTimeValueChangedEventArgs> valueChanged = this.ValueChanged;
			if (valueChanged != null)
			{
				valueChanged.Invoke(this, e);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000E247 File Offset: 0x0000C447
		// (set) Token: 0x06000327 RID: 807 RVA: 0x0000E259 File Offset: 0x0000C459
		public string ValueString
		{
			get
			{
				return (string)base.GetValue(DateTimePickerBase.ValueStringProperty);
			}
			private set
			{
				base.SetValue(DateTimePickerBase.ValueStringProperty, value);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000E267 File Offset: 0x0000C467
		// (set) Token: 0x06000329 RID: 809 RVA: 0x0000E279 File Offset: 0x0000C479
		public string ValueStringFormat
		{
			get
			{
				return (string)base.GetValue(DateTimePickerBase.ValueStringFormatProperty);
			}
			set
			{
				base.SetValue(DateTimePickerBase.ValueStringFormatProperty, value);
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000E287 File Offset: 0x0000C487
		private static void OnValueStringFormatChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((DateTimePickerBase)o).OnValueStringFormatChanged();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000E294 File Offset: 0x0000C494
		private void OnValueStringFormatChanged()
		{
			this.UpdateValueString();
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0000E29C File Offset: 0x0000C49C
		// (set) Token: 0x0600032D RID: 813 RVA: 0x0000E2A9 File Offset: 0x0000C4A9
		public object Header
		{
			get
			{
				return base.GetValue(DateTimePickerBase.HeaderProperty);
			}
			set
			{
				base.SetValue(DateTimePickerBase.HeaderProperty, value);
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0000E2B7 File Offset: 0x0000C4B7
		// (set) Token: 0x0600032F RID: 815 RVA: 0x0000E2C9 File Offset: 0x0000C4C9
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DateTimePickerBase.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(DateTimePickerBase.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000E2D7 File Offset: 0x0000C4D7
		// (set) Token: 0x06000331 RID: 817 RVA: 0x0000E2E9 File Offset: 0x0000C4E9
		public Uri PickerPageUri
		{
			get
			{
				return (Uri)base.GetValue(DateTimePickerBase.PickerPageUriProperty);
			}
			set
			{
				base.SetValue(DateTimePickerBase.PickerPageUriProperty, value);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000E2F7 File Offset: 0x0000C4F7
		protected virtual string ValueStringFormatFallback
		{
			get
			{
				return "{0}";
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000E308 File Offset: 0x0000C508
		public override void OnApplyTemplate()
		{
			if (this._dateButtonPart != null)
			{
				this._dateButtonPart.Click -= new RoutedEventHandler(this.HandleDateButtonClick);
			}
			base.OnApplyTemplate();
			this._dateButtonPart = (base.GetTemplateChild("DateTimeButton") as ButtonBase);
			if (this._dateButtonPart != null)
			{
				this._dateButtonPart.Click += new RoutedEventHandler(this.HandleDateButtonClick);
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000E36F File Offset: 0x0000C56F
		private void HandleDateButtonClick(object sender, RoutedEventArgs e)
		{
			this.OpenPickerPage();
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000E378 File Offset: 0x0000C578
		private void UpdateValueString()
		{
			this.ValueString = string.Format(CultureInfo.CurrentCulture, this.ValueStringFormat ?? this.ValueStringFormatFallback, new object[]
			{
				this.Value
			});
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000E3BC File Offset: 0x0000C5BC
		private void OpenPickerPage()
		{
			if (null == this.PickerPageUri)
			{
				throw new ArgumentException("PickerPageUri property must not be null.");
			}
			if (this._frame == null)
			{
				this._frame = (Application.Current.RootVisual as PhoneApplicationFrame);
				if (this._frame != null)
				{
					this._frameContentWhenOpened = this._frame.Content;
					UIElement uielement = this._frameContentWhenOpened as UIElement;
					if (uielement != null)
					{
						this._savedNavigationInTransition = TransitionService.GetNavigationInTransition(uielement);
						TransitionService.SetNavigationInTransition(uielement, null);
						this._savedNavigationOutTransition = TransitionService.GetNavigationOutTransition(uielement);
						TransitionService.SetNavigationOutTransition(uielement, null);
					}
					this._frame.Navigated += new NavigatedEventHandler(this.HandleFrameNavigated);
					this._frame.NavigationStopped += new NavigationStoppedEventHandler(this.HandleFrameNavigationStoppedOrFailed);
					this._frame.NavigationFailed += new NavigationFailedEventHandler(this.HandleFrameNavigationStoppedOrFailed);
					this._frame.Navigate(this.PickerPageUri);
				}
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000E4AC File Offset: 0x0000C6AC
		private void ClosePickerPage()
		{
			if (this._frame != null)
			{
				this._frame.Navigated -= new NavigatedEventHandler(this.HandleFrameNavigated);
				this._frame.NavigationStopped -= new NavigationStoppedEventHandler(this.HandleFrameNavigationStoppedOrFailed);
				this._frame.NavigationFailed -= new NavigationFailedEventHandler(this.HandleFrameNavigationStoppedOrFailed);
				UIElement uielement = this._frameContentWhenOpened as UIElement;
				if (uielement != null)
				{
					TransitionService.SetNavigationInTransition(uielement, this._savedNavigationInTransition);
					this._savedNavigationInTransition = null;
					TransitionService.SetNavigationOutTransition(uielement, this._savedNavigationOutTransition);
					this._savedNavigationOutTransition = null;
				}
				this._frame = null;
				this._frameContentWhenOpened = null;
			}
			if (this._dateTimePickerPage != null)
			{
				if (this._dateTimePickerPage.Value != null)
				{
					this.Value = new DateTime?(this._dateTimePickerPage.Value.Value);
				}
				this._dateTimePickerPage = null;
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000E590 File Offset: 0x0000C790
		private void HandleFrameNavigated(object sender, NavigationEventArgs e)
		{
			if (e.Content == this._frameContentWhenOpened)
			{
				this.ClosePickerPage();
				return;
			}
			if (this._dateTimePickerPage == null)
			{
				this._dateTimePickerPage = (e.Content as IDateTimePickerPage);
				if (this._dateTimePickerPage != null)
				{
					this._dateTimePickerPage.Value = new DateTime?(this.Value.GetValueOrDefault(DateTime.Now));
				}
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000E5F6 File Offset: 0x0000C7F6
		private void HandleFrameNavigationStoppedOrFailed(object sender, EventArgs e)
		{
			this.ClosePickerPage();
		}

		// Token: 0x0400016B RID: 363
		private const string ButtonPartName = "DateTimeButton";

		// Token: 0x0400016C RID: 364
		private ButtonBase _dateButtonPart;

		// Token: 0x0400016D RID: 365
		private PhoneApplicationFrame _frame;

		// Token: 0x0400016E RID: 366
		private object _frameContentWhenOpened;

		// Token: 0x0400016F RID: 367
		private NavigationInTransition _savedNavigationInTransition;

		// Token: 0x04000170 RID: 368
		private NavigationOutTransition _savedNavigationOutTransition;

		// Token: 0x04000171 RID: 369
		private IDateTimePickerPage _dateTimePickerPage;

		// Token: 0x04000173 RID: 371
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(DateTime?), typeof(DateTimePickerBase), new PropertyMetadata(null, new PropertyChangedCallback(DateTimePickerBase.OnValueChanged)));

		// Token: 0x04000174 RID: 372
		public static readonly DependencyProperty ValueStringProperty = DependencyProperty.Register("ValueString", typeof(string), typeof(DateTimePickerBase), null);

		// Token: 0x04000175 RID: 373
		public static readonly DependencyProperty ValueStringFormatProperty = DependencyProperty.Register("ValueStringFormat", typeof(string), typeof(DateTimePickerBase), new PropertyMetadata(null, new PropertyChangedCallback(DateTimePickerBase.OnValueStringFormatChanged)));

		// Token: 0x04000176 RID: 374
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(DateTimePickerBase), null);

		// Token: 0x04000177 RID: 375
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(DateTimePickerBase), null);

		// Token: 0x04000178 RID: 376
		public static readonly DependencyProperty PickerPageUriProperty = DependencyProperty.Register("PickerPageUri", typeof(Uri), typeof(DateTimePickerBase), null);
	}
}
