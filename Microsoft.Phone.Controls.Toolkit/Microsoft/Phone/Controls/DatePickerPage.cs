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
	// Token: 0x02000079 RID: 121
	public class DatePickerPage : DateTimePickerPageBase
	{
		// Token: 0x060004D1 RID: 1233 RVA: 0x00014C30 File Offset: 0x00012E30
		public DatePickerPage()
		{
			this.InitializeComponent();
			this.PrimarySelector.DataSource = new YearDataSource();
			this.SecondarySelector.DataSource = new MonthDataSource();
			this.TertiarySelector.DataSource = new DayDataSource();
			base.InitializeDateTimePickerPage(this.PrimarySelector, this.SecondarySelector, this.TertiarySelector);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00014CA0 File Offset: 0x00012EA0
		protected override IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern()
		{
			return DateTimePickerPageBase.GetSelectorsOrderedByCulturePattern(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpperInvariant(), new char[]
			{
				'Y',
				'M',
				'D'
			}, new LoopingSelector[]
			{
				this.PrimarySelector,
				this.SecondarySelector,
				this.TertiarySelector
			});
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00014CFA File Offset: 0x00012EFA
		protected override void OnOrientationChanged(OrientationChangedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.OnOrientationChanged(e);
			this.SystemTrayPlaceholder.Visibility = (((1 & e.Orientation) != null) ? 0 : 1);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00014D2C File Offset: 0x00012F2C
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Application.LoadComponent(this, new Uri("/Microsoft.Phone.Controls.Toolkit;component/DateTimePickers/DatePickerPage.xaml", 2));
			this.VisibilityStates = (VisualStateGroup)base.FindName("VisibilityStates");
			this.Open = (VisualState)base.FindName("Open");
			this.Closed = (VisualState)base.FindName("Closed");
			this.PlaneProjection = (PlaneProjection)base.FindName("PlaneProjection");
			this.SystemTrayPlaceholder = (Rectangle)base.FindName("SystemTrayPlaceholder");
			this.SecondarySelector = (LoopingSelector)base.FindName("SecondarySelector");
			this.TertiarySelector = (LoopingSelector)base.FindName("TertiarySelector");
			this.PrimarySelector = (LoopingSelector)base.FindName("PrimarySelector");
		}

		// Token: 0x04000282 RID: 642
		internal VisualStateGroup VisibilityStates;

		// Token: 0x04000283 RID: 643
		internal VisualState Open;

		// Token: 0x04000284 RID: 644
		internal VisualState Closed;

		// Token: 0x04000285 RID: 645
		internal PlaneProjection PlaneProjection;

		// Token: 0x04000286 RID: 646
		internal Rectangle SystemTrayPlaceholder;

		// Token: 0x04000287 RID: 647
		internal LoopingSelector SecondarySelector;

		// Token: 0x04000288 RID: 648
		internal LoopingSelector TertiarySelector;

		// Token: 0x04000289 RID: 649
		internal LoopingSelector PrimarySelector;

		// Token: 0x0400028A RID: 650
		private bool _contentLoaded;
	}
}
