using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x0200004E RID: 78
	[TemplatePart(Name = "SwitchRoot", Type = typeof(Grid))]
	[TemplateVisualState(Name = "Checked", GroupName = "CheckStates")]
	[TemplateVisualState(Name = "Dragging", GroupName = "CheckStates")]
	[TemplatePart(Name = "SwitchThumb", Type = typeof(FrameworkElement))]
	[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
	[TemplatePart(Name = "SwitchTrack", Type = typeof(Grid))]
	[TemplatePart(Name = "SwitchBackground", Type = typeof(UIElement))]
	[TemplateVisualState(Name = "Unchecked", GroupName = "CheckStates")]
	public class ToggleSwitchButton : ToggleButton
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000D854 File Offset: 0x0000BA54
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0000D866 File Offset: 0x0000BA66
		public Brush SwitchForeground
		{
			get
			{
				return (Brush)base.GetValue(ToggleSwitchButton.SwitchForegroundProperty);
			}
			set
			{
				base.SetValue(ToggleSwitchButton.SwitchForegroundProperty, value);
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000D874 File Offset: 0x0000BA74
		public ToggleSwitchButton()
		{
			base.DefaultStyleKey = typeof(ToggleSwitchButton);
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000D88C File Offset: 0x0000BA8C
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x0000D8AD File Offset: 0x0000BAAD
		private double Translation
		{
			get
			{
				if (this._backgroundTranslation != null)
				{
					return this._backgroundTranslation.X;
				}
				return this._thumbTranslation.X;
			}
			set
			{
				if (this._backgroundTranslation != null)
				{
					this._backgroundTranslation.X = value;
				}
				if (this._thumbTranslation != null)
				{
					this._thumbTranslation.X = value;
				}
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000D8D8 File Offset: 0x0000BAD8
		private void ChangeVisualState(bool useTransitions)
		{
			if (base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			if (this._isDragging)
			{
				VisualStateManager.GoToState(this, "Dragging", useTransitions);
				return;
			}
			if (base.IsChecked == true)
			{
				VisualStateManager.GoToState(this, "Checked", useTransitions);
				return;
			}
			VisualStateManager.GoToState(this, "Unchecked", useTransitions);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000D958 File Offset: 0x0000BB58
		protected override void OnToggle()
		{
			base.IsChecked = new bool?(!(base.IsChecked == true));
			this.ChangeVisualState(true);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000D998 File Offset: 0x0000BB98
		public override void OnApplyTemplate()
		{
			if (this._track != null)
			{
				this._track.SizeChanged -= new SizeChangedEventHandler(this.SizeChangedHandler);
			}
			if (this._thumb != null)
			{
				this._thumb.SizeChanged -= new SizeChangedEventHandler(this.SizeChangedHandler);
			}
			base.OnApplyTemplate();
			this._root = (base.GetTemplateChild("SwitchRoot") as Grid);
			UIElement uielement = base.GetTemplateChild("SwitchBackground") as UIElement;
			this._backgroundTranslation = ((uielement == null) ? null : (uielement.RenderTransform as TranslateTransform));
			this._track = (base.GetTemplateChild("SwitchTrack") as Grid);
			this._thumb = (base.GetTemplateChild("SwitchThumb") as Border);
			this._thumbTranslation = ((this._thumb == null) ? null : (this._thumb.RenderTransform as TranslateTransform));
			if (this._root != null && this._track != null && this._thumb != null && (this._backgroundTranslation != null || this._thumbTranslation != null))
			{
				GestureListener gestureListener = GestureService.GetGestureListener(this._root);
				gestureListener.DragStarted += new EventHandler<DragStartedGestureEventArgs>(this.DragStartedHandler);
				gestureListener.DragDelta += new EventHandler<DragDeltaGestureEventArgs>(this.DragDeltaHandler);
				gestureListener.DragCompleted += new EventHandler<DragCompletedGestureEventArgs>(this.DragCompletedHandler);
				this._track.SizeChanged += new SizeChangedEventHandler(this.SizeChangedHandler);
				this._thumb.SizeChanged += new SizeChangedEventHandler(this.SizeChangedHandler);
			}
			this.ChangeVisualState(false);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000DB1C File Offset: 0x0000BD1C
		private void DragStartedHandler(object sender, DragStartedGestureEventArgs e)
		{
			e.Handled = true;
			this._isDragging = true;
			this._dragTranslation = this.Translation;
			this.ChangeVisualState(true);
			this.Translation = this._dragTranslation;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000DB4C File Offset: 0x0000BD4C
		private void DragDeltaHandler(object sender, DragDeltaGestureEventArgs e)
		{
			e.Handled = true;
			if (e.Direction == 1 && e.HorizontalChange != 0.0)
			{
				this._wasDragged = true;
				this._dragTranslation += e.HorizontalChange;
				this.Translation = Math.Max(0.0, Math.Min(this._checkedTranslation, this._dragTranslation));
			}
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000DBBC File Offset: 0x0000BDBC
		private void DragCompletedHandler(object sender, DragCompletedGestureEventArgs e)
		{
			e.Handled = true;
			this._isDragging = false;
			bool flag = false;
			if (this._wasDragged)
			{
				double num = (base.IsChecked == true) ? this._checkedTranslation : 0.0;
				if (this.Translation != num)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				this.OnClick();
			}
			this._wasDragged = false;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000DC30 File Offset: 0x0000BE30
		private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
		{
			this._track.Clip = new RectangleGeometry
			{
				Rect = new Rect(0.0, 0.0, this._track.ActualWidth, this._track.ActualHeight)
			};
			this._checkedTranslation = this._track.ActualWidth - this._thumb.ActualWidth - this._thumb.Margin.Left - this._thumb.Margin.Right;
		}

		// Token: 0x0400014E RID: 334
		private const string CommonStates = "CommonStates";

		// Token: 0x0400014F RID: 335
		private const string NormalState = "Normal";

		// Token: 0x04000150 RID: 336
		private const string DisabledState = "Disabled";

		// Token: 0x04000151 RID: 337
		private const string CheckStates = "CheckStates";

		// Token: 0x04000152 RID: 338
		private const string CheckedState = "Checked";

		// Token: 0x04000153 RID: 339
		private const string DraggingState = "Dragging";

		// Token: 0x04000154 RID: 340
		private const string UncheckedState = "Unchecked";

		// Token: 0x04000155 RID: 341
		private const string SwitchRootPart = "SwitchRoot";

		// Token: 0x04000156 RID: 342
		private const string SwitchBackgroundPart = "SwitchBackground";

		// Token: 0x04000157 RID: 343
		private const string SwitchTrackPart = "SwitchTrack";

		// Token: 0x04000158 RID: 344
		private const string SwitchThumbPart = "SwitchThumb";

		// Token: 0x04000159 RID: 345
		private const double _uncheckedTranslation = 0.0;

		// Token: 0x0400015A RID: 346
		public static readonly DependencyProperty SwitchForegroundProperty = DependencyProperty.Register("SwitchForeground", typeof(Brush), typeof(ToggleSwitchButton), new PropertyMetadata(null));

		// Token: 0x0400015B RID: 347
		private TranslateTransform _backgroundTranslation;

		// Token: 0x0400015C RID: 348
		private TranslateTransform _thumbTranslation;

		// Token: 0x0400015D RID: 349
		private Grid _root;

		// Token: 0x0400015E RID: 350
		private Grid _track;

		// Token: 0x0400015F RID: 351
		private FrameworkElement _thumb;

		// Token: 0x04000160 RID: 352
		private double _checkedTranslation;

		// Token: 0x04000161 RID: 353
		private double _dragTranslation;

		// Token: 0x04000162 RID: 354
		private bool _wasDragged;

		// Token: 0x04000163 RID: 355
		private bool _isDragging;
	}
}
