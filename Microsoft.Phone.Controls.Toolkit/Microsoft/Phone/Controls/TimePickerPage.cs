using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Phone.Controls.Primitives;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000025 RID: 37
	public class TimePickerPage : DateTimePickerPageBase
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00006734 File Offset: 0x00004934
		public TimePickerPage()
		{
			this.InitializeComponent();
			this.PrimarySelector.DataSource = (DateTimeWrapper.CurrentCultureUsesTwentyFourHourClock() ? new TwentyFourHourDataSource() : new TwelveHourDataSource());
			this.SecondarySelector.DataSource = new MinuteDataSource();
			this.TertiarySelector.DataSource = new AmPmDataSource();
			base.InitializeDateTimePickerPage(this.PrimarySelector, this.SecondarySelector, this.TertiarySelector);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000067B0 File Offset: 0x000049B0
		protected override IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern()
		{
			return DateTimePickerPageBase.GetSelectorsOrderedByCulturePattern(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.ToUpperInvariant(), new char[]
			{
				'H',
				'M',
				'T'
			}, new LoopingSelector[]
			{
				this.PrimarySelector,
				this.SecondarySelector,
				this.TertiarySelector
			});
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000680A File Offset: 0x00004A0A
		protected override void OnOrientationChanged(OrientationChangedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.OnOrientationChanged(e);
			this.SystemTrayPlaceholder.Visibility = (((1 & e.Orientation) != null) ? 0 : 1);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000683C File Offset: 0x00004A3C
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Application.LoadComponent(this, new Uri("/Microsoft.Phone.Controls.Toolkit;component/DateTimePickers/TimePickerPage.xaml", 2));
			this.VisibilityStates = (VisualStateGroup)base.FindName("VisibilityStates");
			this.Open = (VisualState)base.FindName("Open");
			this.Closed = (VisualState)base.FindName("Closed");
			this.PlaneProjection = (PlaneProjection)base.FindName("PlaneProjection");
			this.SystemTrayPlaceholder = (Rectangle)base.FindName("SystemTrayPlaceholder");
			this.PrimarySelector = (LoopingSelector)base.FindName("PrimarySelector");
			this.SecondarySelector = (LoopingSelector)base.FindName("SecondarySelector");
			this.TertiarySelector = (LoopingSelector)base.FindName("TertiarySelector");
		}

		// Token: 0x04000074 RID: 116
		internal VisualStateGroup VisibilityStates;

		// Token: 0x04000075 RID: 117
		internal VisualState Open;

		// Token: 0x04000076 RID: 118
		internal VisualState Closed;

		// Token: 0x04000077 RID: 119
		internal PlaneProjection PlaneProjection;

		// Token: 0x04000078 RID: 120
		internal Rectangle SystemTrayPlaceholder;

		// Token: 0x04000079 RID: 121
		internal LoopingSelector PrimarySelector;

		// Token: 0x0400007A RID: 122
		internal LoopingSelector SecondarySelector;

		// Token: 0x0400007B RID: 123
		internal LoopingSelector TertiarySelector;

		// Token: 0x0400007C RID: 124
		private bool _contentLoaded;
	}
}
