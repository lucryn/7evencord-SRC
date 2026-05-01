using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls.Primitives;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200003C RID: 60
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(MenuItem))]
	[TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "Focused", GroupName = "FocusStates")]
	[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
	public class MenuItem : HeaderedItemsControl
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060001B6 RID: 438 RVA: 0x00008700 File Offset: 0x00006900
		// (remove) Token: 0x060001B7 RID: 439 RVA: 0x00008738 File Offset: 0x00006938
		public event RoutedEventHandler Click;

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000876D File Offset: 0x0000696D
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x00008775 File Offset: 0x00006975
		internal MenuBase ParentMenuBase { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000877E File Offset: 0x0000697E
		// (set) Token: 0x060001BB RID: 443 RVA: 0x00008790 File Offset: 0x00006990
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(MenuItem.CommandProperty);
			}
			set
			{
				base.SetValue(MenuItem.CommandProperty, value);
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000879E File Offset: 0x0000699E
		private static void OnCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((MenuItem)o).OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000087C3 File Offset: 0x000069C3
		private void OnCommandChanged(ICommand oldValue, ICommand newValue)
		{
			if (oldValue != null)
			{
				oldValue.CanExecuteChanged -= new EventHandler(this.HandleCanExecuteChanged);
			}
			if (newValue != null)
			{
				newValue.CanExecuteChanged += new EventHandler(this.HandleCanExecuteChanged);
			}
			this.UpdateIsEnabled(true);
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001BE RID: 446 RVA: 0x000087F6 File Offset: 0x000069F6
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00008803 File Offset: 0x00006A03
		public object CommandParameter
		{
			get
			{
				return base.GetValue(MenuItem.CommandParameterProperty);
			}
			set
			{
				base.SetValue(MenuItem.CommandParameterProperty, value);
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008811 File Offset: 0x00006A11
		private static void OnCommandParameterChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((MenuItem)o).OnCommandParameterChanged();
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000881E File Offset: 0x00006A1E
		private void OnCommandParameterChanged()
		{
			this.UpdateIsEnabled(true);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00008828 File Offset: 0x00006A28
		public MenuItem()
		{
			base.DefaultStyleKey = typeof(MenuItem);
			base.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.HandleIsEnabledChanged);
			base.Loaded += new RoutedEventHandler(this.HandleLoaded);
			this.UpdateIsEnabled(false);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00008876 File Offset: 0x00006A76
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.ChangeVisualState(false);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008885 File Offset: 0x00006A85
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			this._isFocused = true;
			this.ChangeVisualState(true);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000889C File Offset: 0x00006A9C
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			this._isFocused = false;
			this.ChangeVisualState(true);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000088B3 File Offset: 0x00006AB3
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			base.Focus();
			this.ChangeVisualState(true);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000088CA File Offset: 0x00006ACA
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.ParentMenuBase != null)
			{
				this.ParentMenuBase.Focus();
			}
			this.ChangeVisualState(true);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000088EE File Offset: 0x00006AEE
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (!e.Handled)
			{
				this.OnClick();
				e.Handled = true;
			}
			base.OnMouseLeftButtonUp(e);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000891A File Offset: 0x00006B1A
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (!e.Handled && 3 == e.Key)
			{
				this.OnClick();
				e.Handled = true;
			}
			base.OnKeyDown(e);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000894F File Offset: 0x00006B4F
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008958 File Offset: 0x00006B58
		protected virtual void OnClick()
		{
			ContextMenu contextMenu = this.ParentMenuBase as ContextMenu;
			if (contextMenu != null)
			{
				contextMenu.ChildMenuItemClicked();
			}
			RoutedEventHandler click = this.Click;
			if (click != null)
			{
				click.Invoke(this, new RoutedEventArgs());
			}
			if (this.Command != null && this.Command.CanExecute(this.CommandParameter))
			{
				this.Command.Execute(this.CommandParameter);
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000089BC File Offset: 0x00006BBC
		private void HandleCanExecuteChanged(object sender, EventArgs e)
		{
			this.UpdateIsEnabled(true);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000089C5 File Offset: 0x00006BC5
		private void UpdateIsEnabled(bool changeVisualState)
		{
			base.IsEnabled = (this.Command == null || this.Command.CanExecute(this.CommandParameter));
			if (changeVisualState)
			{
				this.ChangeVisualState(true);
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000089F3 File Offset: 0x00006BF3
		private void HandleIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.ChangeVisualState(true);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000089FC File Offset: 0x00006BFC
		private void HandleLoaded(object sender, RoutedEventArgs e)
		{
			this.ChangeVisualState(false);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00008A08 File Offset: 0x00006C08
		protected virtual void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (this._isFocused && base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Focused", useTransitions);
				return;
			}
			VisualStateManager.GoToState(this, "Unfocused", useTransitions);
		}

		// Token: 0x040000B4 RID: 180
		private bool _isFocused;

		// Token: 0x040000B5 RID: 181
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(MenuItem), new PropertyMetadata(null, new PropertyChangedCallback(MenuItem.OnCommandChanged)));

		// Token: 0x040000B6 RID: 182
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(MenuItem), new PropertyMetadata(null, new PropertyChangedCallback(MenuItem.OnCommandParameterChanged)));
	}
}
