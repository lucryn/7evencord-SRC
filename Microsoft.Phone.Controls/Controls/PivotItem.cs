using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls.Primitives;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000021 RID: 33
	[TemplateVisualState(Name = "Right", GroupName = "Position States")]
	[TemplateVisualState(Name = "Left", GroupName = "Position States")]
	[TemplateVisualState(Name = "Center", GroupName = "Position States")]
	public class PivotItem : ContentControl
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00007D23 File Offset: 0x00006D23
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00007D30 File Offset: 0x00006D30
		public object Header
		{
			get
			{
				return base.GetValue(PivotItem.HeaderProperty);
			}
			set
			{
				base.SetValue(PivotItem.HeaderProperty, value);
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007D40 File Offset: 0x00006D40
		public PivotItem()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_STARTUP, PerfLog.PivotItem);
			base.DefaultStyleKey = typeof(PivotItem);
			base.Loaded += new RoutedEventHandler(this.PivotItem_Loaded);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007D90 File Offset: 0x00006D90
		private void PivotItem_Loaded(object sender, RoutedEventArgs e)
		{
			base.Loaded -= new RoutedEventHandler(this.PivotItem_Loaded);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_STARTUP, PerfLog.PivotItem);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00007DB4 File Offset: 0x00006DB4
		protected override Size ArrangeOverride(Size finalSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_ARRANGE, PerfLog.PivotItem);
			Size result = base.ArrangeOverride(finalSize);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_ARRANGE, PerfLog.PivotItem);
			return result;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007DE8 File Offset: 0x00006DE8
		protected override Size MeasureOverride(Size availableSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_MEASURE, PerfLog.PivotItem);
			Size result = base.MeasureOverride(availableSize);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_MEASURE, PerfLog.PivotItem);
			return result;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007E1C File Offset: 0x00006E1C
		public override void OnApplyTemplate()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_APPLYTEMPLATE, PerfLog.PivotItem);
			base.OnApplyTemplate();
			AnimationDirection? firstAnimation = this._firstAnimation;
			this.MoveTo(AnimationDirection.Center);
			if (firstAnimation != null)
			{
				this.MoveTo(firstAnimation.Value);
			}
			this._firstAnimation = new AnimationDirection?(AnimationDirection.Center);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_APPLYTEMPLATE, PerfLog.PivotItem);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007E80 File Offset: 0x00006E80
		internal void MoveTo(AnimationDirection direction)
		{
			bool flag = direction != AnimationDirection.Center;
			this._direction = direction;
			if (this._firstAnimation == null && flag)
			{
				this._firstAnimation = new AnimationDirection?(direction);
				return;
			}
			this.UpdateVisualStates(flag);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007EC0 File Offset: 0x00006EC0
		private void UpdateVisualStates(bool useTransitions)
		{
			VisualStateManager.GoToState(this, PivotItem.StateNameFromAnimationDirection(this._direction), useTransitions);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00007ED8 File Offset: 0x00006ED8
		private static string StateNameFromAnimationDirection(AnimationDirection direction)
		{
			switch (direction)
			{
			case AnimationDirection.Center:
				return "Center";
			case AnimationDirection.Left:
				return "Left";
			case AnimationDirection.Right:
				return "Right";
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x040000C7 RID: 199
		private const string PivotPositionsGroupName = "Position States";

		// Token: 0x040000C8 RID: 200
		private const string PivotStateCenter = "Center";

		// Token: 0x040000C9 RID: 201
		private const string PivotStateLeft = "Left";

		// Token: 0x040000CA RID: 202
		private const string PivotStateRight = "Right";

		// Token: 0x040000CB RID: 203
		private const string ContentName = "Content";

		// Token: 0x040000CC RID: 204
		private AnimationDirection? _firstAnimation = default(AnimationDirection?);

		// Token: 0x040000CD RID: 205
		private AnimationDirection _direction;

		// Token: 0x040000CE RID: 206
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(PivotItem), new PropertyMetadata(null));
	}
}
