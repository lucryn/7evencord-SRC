using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200003E RID: 62
	internal sealed class InteractionHelper
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00008B70 File Offset: 0x00006D70
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00008B78 File Offset: 0x00006D78
		public Control Control { get; private set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00008B81 File Offset: 0x00006D81
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00008B89 File Offset: 0x00006D89
		public bool IsFocused { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00008B92 File Offset: 0x00006D92
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00008B9A File Offset: 0x00006D9A
		public bool IsMouseOver { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00008BA3 File Offset: 0x00006DA3
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00008BAB File Offset: 0x00006DAB
		public bool IsReadOnly { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00008BB4 File Offset: 0x00006DB4
		// (set) Token: 0x060001DE RID: 478 RVA: 0x00008BBC File Offset: 0x00006DBC
		public bool IsPressed { get; private set; }

		// Token: 0x060001DF RID: 479 RVA: 0x00008BC5 File Offset: 0x00006DC5
		public InteractionHelper(Control control)
		{
			this.Control = control;
			this._updateVisualState = (control as IUpdateVisualState);
			control.Loaded += new RoutedEventHandler(this.OnLoaded);
			control.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.OnIsEnabledChanged);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00008C04 File Offset: 0x00006E04
		private void UpdateVisualState(bool useTransitions)
		{
			if (this._updateVisualState != null)
			{
				this._updateVisualState.UpdateVisualState(useTransitions);
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008C1C File Offset: 0x00006E1C
		public void UpdateVisualStateBase(bool useTransitions)
		{
			if (!this.Control.IsEnabled)
			{
				VisualStates.GoToState(this.Control, useTransitions, new string[]
				{
					"Disabled",
					"Normal"
				});
			}
			else if (this.IsReadOnly)
			{
				VisualStates.GoToState(this.Control, useTransitions, new string[]
				{
					"ReadOnly",
					"Normal"
				});
			}
			else if (this.IsPressed)
			{
				VisualStates.GoToState(this.Control, useTransitions, new string[]
				{
					"Pressed",
					"MouseOver",
					"Normal"
				});
			}
			else if (this.IsMouseOver)
			{
				VisualStates.GoToState(this.Control, useTransitions, new string[]
				{
					"MouseOver",
					"Normal"
				});
			}
			else
			{
				VisualStates.GoToState(this.Control, useTransitions, new string[]
				{
					"Normal"
				});
			}
			if (this.IsFocused)
			{
				VisualStates.GoToState(this.Control, useTransitions, new string[]
				{
					"Focused",
					"Unfocused"
				});
				return;
			}
			VisualStates.GoToState(this.Control, useTransitions, new string[]
			{
				"Unfocused"
			});
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00008D63 File Offset: 0x00006F63
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			this.UpdateVisualState(false);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00008D6C File Offset: 0x00006F6C
		private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!(bool)e.NewValue)
			{
				this.IsPressed = false;
				this.IsMouseOver = false;
				this.IsFocused = false;
			}
			this.UpdateVisualState(true);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00008DA5 File Offset: 0x00006FA5
		public void OnIsReadOnlyChanged(bool value)
		{
			this.IsReadOnly = value;
			if (!value)
			{
				this.IsPressed = false;
				this.IsMouseOver = false;
				this.IsFocused = false;
			}
			this.UpdateVisualState(true);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00008DCD File Offset: 0x00006FCD
		public void OnApplyTemplateBase()
		{
			this.UpdateVisualState(false);
		}

		// Token: 0x040000B8 RID: 184
		private const double SequentialClickThresholdInMilliseconds = 500.0;

		// Token: 0x040000B9 RID: 185
		private const double SequentialClickThresholdInPixelsSquared = 9.0;

		// Token: 0x040000BA RID: 186
		private IUpdateVisualState _updateVisualState;
	}
}
