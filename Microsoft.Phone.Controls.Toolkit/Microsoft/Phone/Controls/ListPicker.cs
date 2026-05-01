using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls.Properties;
using Microsoft.Phone.Shell;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200000F RID: 15
	[TemplateVisualState(GroupName = "PickerStates", Name = "Normal")]
	[TemplatePart(Name = "ItemsPresenter", Type = typeof(ItemsPresenter))]
	[TemplatePart(Name = "ItemsPresenterHost", Type = typeof(Canvas))]
	[TemplatePart(Name = "FullModePopup", Type = typeof(Popup))]
	[TemplatePart(Name = "FullModeSelector", Type = typeof(Selector))]
	[TemplateVisualState(GroupName = "PickerStates", Name = "Expanded")]
	[TemplatePart(Name = "ItemsPresenterTranslateTransform", Type = typeof(TranslateTransform))]
	public class ListPicker : ItemsControl
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000069 RID: 105 RVA: 0x00002D4C File Offset: 0x00000F4C
		// (remove) Token: 0x0600006A RID: 106 RVA: 0x00002D84 File Offset: 0x00000F84
		public event SelectionChangedEventHandler SelectionChanged;

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002DB9 File Offset: 0x00000FB9
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00002DCB File Offset: 0x00000FCB
		public ListPickerMode ListPickerMode
		{
			get
			{
				return (ListPickerMode)base.GetValue(ListPicker.ListPickerModeProperty);
			}
			set
			{
				base.SetValue(ListPicker.ListPickerModeProperty, value);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002DDE File Offset: 0x00000FDE
		private static void OnListPickerModeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((ListPicker)o).OnListPickerModeChanged((ListPickerMode)e.OldValue, (ListPickerMode)e.NewValue);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002E68 File Offset: 0x00001068
		private void OnListPickerModeChanged(ListPickerMode oldValue, ListPickerMode newValue)
		{
			if (this._frame == null)
			{
				this._frame = (Application.Current.RootVisual as PhoneApplicationFrame);
				if (this._frame != null)
				{
					this._frame.AddHandler(UIElement.ManipulationCompletedEvent, new EventHandler<ManipulationCompletedEventArgs>(this.HandleFrameManipulationCompleted), true);
				}
			}
			if (ListPickerMode.Full == oldValue && !DesignerProperties.IsInDesignTool)
			{
				if (this._fullModePopupPart != null)
				{
					this._fullModePopupPart.IsOpen = false;
				}
				if (this._fullModeSelectorPart != null)
				{
					this._fullModeSelectorPart.SelectionChanged -= new SelectionChangedEventHandler(this.HandleFullModeSelectorPartSelectionChanged);
					this._fullModeSelectorPart.Loaded -= new RoutedEventHandler(this.HandleFullModeSelectorPartLoaded);
					this._fullModeSelectorPart.ItemsSource = null;
				}
				Action action = delegate()
				{
					try
					{
						SystemTray.IsVisible = this._savedSystemTrayIsVisible;
					}
					catch (InvalidOperationException)
					{
					}
				};
				try
				{
					action.Invoke();
				}
				catch (InvalidOperationException)
				{
					base.Dispatcher.BeginInvoke(action);
				}
				if (this._page != null && this._page.ApplicationBar != null)
				{
					this._page.ApplicationBar.IsVisible = this._savedApplicationBarIsVisible;
				}
				if (this._frame != null)
				{
					this._frame.OrientationChanged -= new EventHandler<OrientationChangedEventArgs>(this.HandleFrameOrientationChanged);
				}
			}
			if ((ListPickerMode.Expanded == oldValue || ListPickerMode.Full == oldValue) && this._page != null)
			{
				this._page.BackKeyPress -= new EventHandler<CancelEventArgs>(this.HandlePageBackKeyPress);
				this._page = null;
			}
			if ((ListPickerMode.Expanded == newValue || ListPickerMode.Full == newValue) && this._frame != null)
			{
				this._page = (this._frame.Content as PhoneApplicationPage);
				if (this._page != null)
				{
					this._page.BackKeyPress += new EventHandler<CancelEventArgs>(this.HandlePageBackKeyPress);
				}
			}
			if (ListPickerMode.Full == newValue && !DesignerProperties.IsInDesignTool)
			{
				Action action2 = delegate()
				{
					try
					{
						this._savedSystemTrayIsVisible = SystemTray.IsVisible;
						SystemTray.IsVisible = false;
					}
					catch (InvalidOperationException)
					{
					}
				};
				try
				{
					action2.Invoke();
				}
				catch (InvalidOperationException)
				{
					base.Dispatcher.BeginInvoke(action2);
				}
				if (this._frame != null)
				{
					this.AdjustPopupChildForCurrentOrientation(this._frame);
					this._frame.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(this.HandleFrameOrientationChanged);
					if (this._page != null && this._page.ApplicationBar != null)
					{
						this._savedApplicationBarIsVisible = this._page.ApplicationBar.IsVisible;
						this._page.ApplicationBar.IsVisible = false;
					}
				}
				if (this._fullModeSelectorPart != null)
				{
					this._fullModeSelectorPart.ItemsSource = base.Items;
					this._fullModeSelectorPart.SelectionChanged += new SelectionChangedEventHandler(this.HandleFullModeSelectorPartSelectionChanged);
					this._fullModeSelectorPart.Loaded += new RoutedEventHandler(this.HandleFullModeSelectorPartLoaded);
				}
				if (this._fullModePopupPart != null)
				{
					this._fullModePopupPart.IsOpen = true;
				}
			}
			this.SizeForAppropriateView(ListPickerMode.Full != oldValue);
			this.GoToStates(true);
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003138 File Offset: 0x00001338
		// (set) Token: 0x06000070 RID: 112 RVA: 0x0000314A File Offset: 0x0000134A
		public int SelectedIndex
		{
			get
			{
				return (int)base.GetValue(ListPicker.SelectedIndexProperty);
			}
			set
			{
				base.SetValue(ListPicker.SelectedIndexProperty, value);
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000315D File Offset: 0x0000135D
		private static void OnSelectedIndexChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((ListPicker)o).OnSelectedIndexChanged((int)e.OldValue, (int)e.NewValue);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003184 File Offset: 0x00001384
		private void OnSelectedIndexChanged(int oldValue, int newValue)
		{
			if (base.Items.Count > newValue && (0 >= base.Items.Count || newValue >= 0) && (base.Items.Count != 0 || newValue == -1))
			{
				if (!this._updatingSelection)
				{
					this._updatingSelection = true;
					this.SelectedItem = ((-1 != newValue) ? base.Items[newValue] : null);
					this._updatingSelection = false;
				}
				if (-1 != oldValue)
				{
					ListPickerItem listPickerItem = (ListPickerItem)base.ItemContainerGenerator.ContainerFromIndex(oldValue);
					if (listPickerItem != null)
					{
						listPickerItem.IsSelected = false;
					}
				}
				return;
			}
			if (base.Template == null && 0 <= newValue)
			{
				this._deferredSelectedIndex = newValue;
				return;
			}
			throw new InvalidOperationException(Resources.InvalidSelectedIndex);
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003230 File Offset: 0x00001430
		// (set) Token: 0x06000074 RID: 116 RVA: 0x0000323D File Offset: 0x0000143D
		public object SelectedItem
		{
			get
			{
				return base.GetValue(ListPicker.SelectedItemProperty);
			}
			set
			{
				base.SetValue(ListPicker.SelectedItemProperty, value);
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000324B File Offset: 0x0000144B
		private static void OnSelectedItemChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((ListPicker)o).OnSelectedItemChanged(e.OldValue, e.NewValue);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003268 File Offset: 0x00001468
		private void OnSelectedItemChanged(object oldValue, object newValue)
		{
			int num = (newValue != null) ? base.Items.IndexOf(newValue) : -1;
			if (-1 == num && 0 < base.Items.Count)
			{
				throw new InvalidOperationException(Resources.InvalidSelectedItem);
			}
			if (!this._updatingSelection)
			{
				this._updatingSelection = true;
				this.SelectedIndex = num;
				this._updatingSelection = false;
			}
			if (this.ListPickerMode != ListPickerMode.Normal)
			{
				this.ListPickerMode = ListPickerMode.Normal;
			}
			else
			{
				this.SizeForAppropriateView(false);
			}
			SelectionChangedEventHandler selectionChanged = this.SelectionChanged;
			if (selectionChanged != null)
			{
				IList list = (oldValue == null) ? new object[0] : new object[]
				{
					oldValue
				};
				IList list2 = (newValue == null) ? new object[0] : new object[]
				{
					newValue
				};
				selectionChanged.Invoke(this, new SelectionChangedEventArgs(list, list2));
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0000332A File Offset: 0x0000152A
		// (set) Token: 0x06000078 RID: 120 RVA: 0x0000333C File Offset: 0x0000153C
		public DataTemplate FullModeItemTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ListPicker.FullModeItemTemplateProperty);
			}
			set
			{
				base.SetValue(ListPicker.FullModeItemTemplateProperty, value);
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000334A File Offset: 0x0000154A
		private static void OnShadowOrFullModeItemTemplateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((ListPicker)o).OnShadowOrFullModeItemTemplateChanged();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003357 File Offset: 0x00001557
		private void OnShadowOrFullModeItemTemplateChanged()
		{
			base.SetValue(ListPicker.ActualFullModeItemTemplateProperty, this.FullModeItemTemplate ?? base.ItemTemplate);
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003374 File Offset: 0x00001574
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00003381 File Offset: 0x00001581
		public object Header
		{
			get
			{
				return base.GetValue(ListPicker.HeaderProperty);
			}
			set
			{
				base.SetValue(ListPicker.HeaderProperty, value);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600007D RID: 125 RVA: 0x0000338F File Offset: 0x0000158F
		// (set) Token: 0x0600007E RID: 126 RVA: 0x000033A1 File Offset: 0x000015A1
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ListPicker.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(ListPicker.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600007F RID: 127 RVA: 0x000033AF File Offset: 0x000015AF
		// (set) Token: 0x06000080 RID: 128 RVA: 0x000033BC File Offset: 0x000015BC
		public object FullModeHeader
		{
			get
			{
				return base.GetValue(ListPicker.FullModeHeaderProperty);
			}
			set
			{
				base.SetValue(ListPicker.FullModeHeaderProperty, value);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000081 RID: 129 RVA: 0x000033CA File Offset: 0x000015CA
		// (set) Token: 0x06000082 RID: 130 RVA: 0x000033DC File Offset: 0x000015DC
		public int ItemCountThreshold
		{
			get
			{
				return (int)base.GetValue(ListPicker.ItemCountThresholdProperty);
			}
			set
			{
				base.SetValue(ListPicker.ItemCountThresholdProperty, value);
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000033EF File Offset: 0x000015EF
		private static void OnItemCountThresholdChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((ListPicker)o).OnItemCountThresholdChanged((int)e.NewValue);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003408 File Offset: 0x00001608
		private void OnItemCountThresholdChanged(int newValue)
		{
			if (newValue < 0)
			{
				throw new ArgumentOutOfRangeException("ItemCountThreshold");
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003444 File Offset: 0x00001644
		public ListPicker()
		{
			base.DefaultStyleKey = typeof(ListPicker);
			Storyboard.SetTargetProperty(this._heightAnimation, new PropertyPath(FrameworkElement.HeightProperty));
			Storyboard.SetTargetProperty(this._translateAnimation, new PropertyPath(TranslateTransform.YProperty));
			Duration duration = TimeSpan.FromSeconds(0.2);
			this._heightAnimation.Duration = duration;
			this._translateAnimation.Duration = duration;
			IEasingFunction easingFunction = new ExponentialEase
			{
				EasingMode = 2,
				Exponent = 4.0
			};
			this._heightAnimation.EasingFunction = easingFunction;
			this._translateAnimation.EasingFunction = easingFunction;
			base.Unloaded += delegate(object A_1, RoutedEventArgs A_2)
			{
				if (this._frame != null)
				{
					this._frame.ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(this.HandleFrameManipulationCompleted);
					this._frame = null;
				}
			};
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003534 File Offset: 0x00001734
		public override void OnApplyTemplate()
		{
			if (this._itemsPresenterHostParent != null)
			{
				this._itemsPresenterHostParent.SizeChanged -= new SizeChangedEventHandler(this.HandleItemsPresenterHostParentSizeChanged);
			}
			this._storyboard.Stop();
			base.OnApplyTemplate();
			this._itemsPresenterPart = (base.GetTemplateChild("ItemsPresenter") as ItemsPresenter);
			this._itemsPresenterTranslateTransformPart = (base.GetTemplateChild("ItemsPresenterTranslateTransform") as TranslateTransform);
			this._itemsPresenterHostPart = (base.GetTemplateChild("ItemsPresenterHost") as Canvas);
			this._fullModePopupPart = (base.GetTemplateChild("FullModePopup") as Popup);
			this._fullModeSelectorPart = (base.GetTemplateChild("FullModeSelector") as Selector);
			this._itemsPresenterHostParent = ((this._itemsPresenterHostPart != null) ? (this._itemsPresenterHostPart.Parent as FrameworkElement) : null);
			if (this._itemsPresenterHostParent != null)
			{
				this._itemsPresenterHostParent.SizeChanged += new SizeChangedEventHandler(this.HandleItemsPresenterHostParentSizeChanged);
			}
			if (this._itemsPresenterHostPart != null)
			{
				Storyboard.SetTarget(this._heightAnimation, this._itemsPresenterHostPart);
				if (!this._storyboard.Children.Contains(this._heightAnimation))
				{
					this._storyboard.Children.Add(this._heightAnimation);
				}
			}
			else if (this._storyboard.Children.Contains(this._heightAnimation))
			{
				this._storyboard.Children.Remove(this._heightAnimation);
			}
			if (this._itemsPresenterTranslateTransformPart != null)
			{
				Storyboard.SetTarget(this._translateAnimation, this._itemsPresenterTranslateTransformPart);
				if (!this._storyboard.Children.Contains(this._translateAnimation))
				{
					this._storyboard.Children.Add(this._translateAnimation);
				}
			}
			else if (this._storyboard.Children.Contains(this._translateAnimation))
			{
				this._storyboard.Children.Remove(this._translateAnimation);
			}
			if (this._fullModePopupPart != null)
			{
				UIElement child = this._fullModePopupPart.Child;
				this._fullModePopupPart.Child = null;
				this._fullModePopupPart = new Popup();
				this._fullModePopupPart.Child = child;
			}
			base.SetBinding(ListPicker.ShadowItemTemplateProperty, new Binding("ItemTemplate")
			{
				Source = this
			});
			if (-1 != this._deferredSelectedIndex)
			{
				this.SelectedIndex = this._deferredSelectedIndex;
				this._deferredSelectedIndex = -1;
			}
			this.GoToStates(false);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003788 File Offset: 0x00001988
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is ListPickerItem;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003793 File Offset: 0x00001993
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ListPickerItem();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000379C File Offset: 0x0000199C
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			ContentControl contentControl = (ContentControl)element;
			contentControl.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.HandleContainerManipulationCompleted);
			contentControl.SizeChanged += new SizeChangedEventHandler(this.HandleListPickerItemSizeChanged);
			if (object.Equals(item, this.SelectedItem))
			{
				this.SizeForAppropriateView(false);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000037F4 File Offset: 0x000019F4
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			ContentControl contentControl = (ContentControl)element;
			contentControl.ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(this.HandleContainerManipulationCompleted);
			contentControl.SizeChanged -= new SizeChangedEventHandler(this.HandleListPickerItemSizeChanged);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003840 File Offset: 0x00001A40
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (0 < base.Items.Count && this.SelectedItem == null)
			{
				if (base.GetBindingExpression(ListPicker.SelectedIndexProperty) == null && base.GetBindingExpression(ListPicker.SelectedItemProperty) == null)
				{
					this.SelectedIndex = 0;
				}
			}
			else if (base.Items.Count == 0)
			{
				this.SelectedIndex = -1;
				this.ListPickerMode = ListPickerMode.Normal;
			}
			else if (base.Items.Count <= this.SelectedIndex)
			{
				this.SelectedIndex = base.Items.Count - 1;
			}
			else if (!object.Equals(base.Items[this.SelectedIndex], this.SelectedItem))
			{
				int num = base.Items.IndexOf(this.SelectedItem);
				if (-1 == num)
				{
					this.SelectedItem = base.Items[0];
				}
				else
				{
					this.SelectedIndex = num;
				}
			}
			base.Dispatcher.BeginInvoke(delegate()
			{
				this.SizeForAppropriateView(false);
			});
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003944 File Offset: 0x00001B44
		protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.OnManipulationCompleted(e);
			DependencyObject dependencyObject = e.OriginalSource as DependencyObject;
			while (dependencyObject != null)
			{
				if (this._itemsPresenterHostPart == dependencyObject)
				{
					if (this.ListPickerMode == ListPickerMode.Normal && 0 < base.Items.Count)
					{
						this.ListPickerMode = ((base.Items.Count <= this.ItemCountThreshold) ? ListPickerMode.Expanded : ListPickerMode.Full);
						e.Handled = true;
						return;
					}
					break;
				}
				else
				{
					dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
				}
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000039C4 File Offset: 0x00001BC4
		private void HandleItemsPresenterHostParentSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this._itemsPresenterPart != null)
			{
				this._itemsPresenterPart.Width = e.NewSize.Width;
			}
			this._itemsPresenterHostParent.Clip = new RectangleGeometry
			{
				Rect = new Rect(default(Point), e.NewSize)
			};
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003A20 File Offset: 0x00001C20
		private void HandleListPickerItemSizeChanged(object sender, SizeChangedEventArgs e)
		{
			ContentControl contentControl = (ContentControl)sender;
			if (object.Equals(base.ItemContainerGenerator.ItemFromContainer(contentControl), this.SelectedItem))
			{
				this.SizeForAppropriateView(false);
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003A54 File Offset: 0x00001C54
		private void HandleFullModeSelectorPartSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this._fullModeSelectorPart != null)
			{
				if (this.SelectedItem != this._fullModeSelectorPart.SelectedItem)
				{
					this.SelectedItem = this._fullModeSelectorPart.SelectedItem;
					return;
				}
				this.ListPickerMode = ListPickerMode.Normal;
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003AAC File Offset: 0x00001CAC
		private void HandleFullModeSelectorPartLoaded(object sender, RoutedEventArgs e)
		{
			if (this._fullModeSelectorPart != null)
			{
				ContentControl contentControl = this._fullModeSelectorPart.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) as ContentControl;
				if (contentControl == null)
				{
					base.Dispatcher.BeginInvoke(delegate()
					{
						this.HandleFullModeSelectorPartLoaded(sender, e);
					});
				}
				else
				{
					Brush brush = Application.Current.Resources["PhoneAccentBrush"] as Brush;
					if (brush != null)
					{
						contentControl.Foreground = brush;
					}
				}
				ListBox listBox = this._fullModeSelectorPart as ListBox;
				if (listBox != null)
				{
					listBox.ScrollIntoView(this.SelectedItem);
				}
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003B60 File Offset: 0x00001D60
		private void HandlePageBackKeyPress(object sender, CancelEventArgs e)
		{
			this.ListPickerMode = ListPickerMode.Normal;
			e.Cancel = true;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003B70 File Offset: 0x00001D70
		private void HandleFrameOrientationChanged(object sender, OrientationChangedEventArgs e)
		{
			this.AdjustPopupChildForCurrentOrientation((PhoneApplicationFrame)sender);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003B80 File Offset: 0x00001D80
		private void AdjustPopupChildForCurrentOrientation(PhoneApplicationFrame frame)
		{
			if (this._fullModePopupPart != null)
			{
				FrameworkElement frameworkElement = this._fullModePopupPart.Child as FrameworkElement;
				if (frameworkElement != null)
				{
					double actualWidth = frame.ActualWidth;
					double actualHeight = frame.ActualHeight;
					bool flag = 1 == (1 & frame.Orientation);
					TransformGroup transformGroup = new TransformGroup();
					PageOrientation orientation = frame.Orientation;
					if (orientation != 18)
					{
						if (orientation == 34)
						{
							transformGroup.Children.Add(new RotateTransform
							{
								Angle = -90.0
							});
							transformGroup.Children.Add(new TranslateTransform
							{
								Y = actualHeight
							});
						}
					}
					else
					{
						transformGroup.Children.Add(new RotateTransform
						{
							Angle = 90.0
						});
						transformGroup.Children.Add(new TranslateTransform
						{
							X = actualWidth
						});
					}
					frameworkElement.RenderTransform = transformGroup;
					frameworkElement.Width = (flag ? actualWidth : actualHeight);
					frameworkElement.Height = (flag ? actualHeight : actualWidth);
					Border border = frameworkElement as Border;
					if (border != null)
					{
						PageOrientation orientation2 = frame.Orientation;
						if (orientation2 == 5)
						{
							border.Padding = new Thickness(0.0, 32.0, 0.0, 0.0);
							return;
						}
						if (orientation2 == 18)
						{
							border.Padding = new Thickness(72.0, 0.0, 0.0, 0.0);
							return;
						}
						if (orientation2 != 34)
						{
							return;
						}
						border.Padding = new Thickness(0.0, 0.0, 72.0, 0.0);
					}
				}
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003D4C File Offset: 0x00001F4C
		private void SizeForAppropriateView(bool animate)
		{
			switch (this.ListPickerMode)
			{
			case ListPickerMode.Normal:
				this.SizeForNormalMode(animate);
				break;
			case ListPickerMode.Expanded:
				this.SizeForExpandedMode();
				break;
			}
			this._storyboard.Begin();
			if (!animate)
			{
				this._storyboard.SkipToFill();
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003D9C File Offset: 0x00001F9C
		private void SizeForNormalMode(bool animate)
		{
			ContentControl contentControl = (ContentControl)base.ItemContainerGenerator.ContainerFromItem(this.SelectedItem);
			if (contentControl != null)
			{
				if (0.0 < contentControl.ActualHeight)
				{
					this.SetContentHeight(contentControl.ActualHeight + contentControl.Margin.Top + contentControl.Margin.Bottom);
				}
				if (this._itemsPresenterTranslateTransformPart != null)
				{
					if (!animate)
					{
						this._itemsPresenterTranslateTransformPart.Y = 0.0;
					}
					this._translateAnimation.To = new double?(contentControl.Margin.Top - LayoutInformation.GetLayoutSlot(contentControl).Top);
					this._translateAnimation.From = (animate ? default(double?) : this._translateAnimation.To);
				}
			}
			else
			{
				this.SetContentHeight(0.0);
			}
			ListPickerItem listPickerItem = (ListPickerItem)base.ItemContainerGenerator.ContainerFromIndex(this.SelectedIndex);
			if (listPickerItem != null)
			{
				listPickerItem.IsSelected = false;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003EA8 File Offset: 0x000020A8
		private void SizeForExpandedMode()
		{
			if (this._itemsPresenterPart != null)
			{
				this.SetContentHeight(this._itemsPresenterPart.ActualHeight);
			}
			if (this._itemsPresenterTranslateTransformPart != null)
			{
				this._translateAnimation.To = new double?(0.0);
			}
			ListPickerItem listPickerItem = (ListPickerItem)base.ItemContainerGenerator.ContainerFromIndex(this.SelectedIndex);
			if (listPickerItem != null)
			{
				listPickerItem.IsSelected = true;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003F10 File Offset: 0x00002110
		private void SetContentHeight(double height)
		{
			if (this._itemsPresenterHostPart != null && !double.IsNaN(height))
			{
				double height2 = this._itemsPresenterHostPart.Height;
				this._heightAnimation.From = new double?(double.IsNaN(height2) ? height : height2);
				this._heightAnimation.To = new double?(height);
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003F68 File Offset: 0x00002168
		private void HandleFrameManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			if (ListPickerMode.Expanded == this.ListPickerMode)
			{
				DependencyObject dependencyObject = e.OriginalSource as DependencyObject;
				DependencyObject dependencyObject2 = this._itemsPresenterHostPart ?? this;
				while (dependencyObject != null)
				{
					if (dependencyObject2 == dependencyObject)
					{
						return;
					}
					dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
				}
				this.ListPickerMode = ListPickerMode.Normal;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003FB0 File Offset: 0x000021B0
		private void HandleContainerManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			if (ListPickerMode.Expanded == this.ListPickerMode)
			{
				ContentControl contentControl = (ContentControl)sender;
				this.SelectedItem = base.ItemContainerGenerator.ItemFromContainer(contentControl);
				this.ListPickerMode = ListPickerMode.Normal;
				e.Handled = true;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003FF0 File Offset: 0x000021F0
		private void GoToStates(bool useTransitions)
		{
			switch (this.ListPickerMode)
			{
			case ListPickerMode.Normal:
				VisualStateManager.GoToState(this, "Normal", useTransitions);
				return;
			case ListPickerMode.Expanded:
				VisualStateManager.GoToState(this, "Expanded", useTransitions);
				break;
			case ListPickerMode.Full:
				break;
			default:
				return;
			}
		}

		// Token: 0x0400001E RID: 30
		private const string ItemsPresenterPartName = "ItemsPresenter";

		// Token: 0x0400001F RID: 31
		private const string ItemsPresenterTranslateTransformPartName = "ItemsPresenterTranslateTransform";

		// Token: 0x04000020 RID: 32
		private const string ItemsPresenterHostPartName = "ItemsPresenterHost";

		// Token: 0x04000021 RID: 33
		private const string FullModePopupPartName = "FullModePopup";

		// Token: 0x04000022 RID: 34
		private const string FullModeSelectorPartName = "FullModeSelector";

		// Token: 0x04000023 RID: 35
		private const string PickerStatesGroupName = "PickerStates";

		// Token: 0x04000024 RID: 36
		private const string PickerStatesNormalStateName = "Normal";

		// Token: 0x04000025 RID: 37
		private const string PickerStatesExpandedStateName = "Expanded";

		// Token: 0x04000026 RID: 38
		private readonly DoubleAnimation _heightAnimation = new DoubleAnimation();

		// Token: 0x04000027 RID: 39
		private readonly DoubleAnimation _translateAnimation = new DoubleAnimation();

		// Token: 0x04000028 RID: 40
		private readonly Storyboard _storyboard = new Storyboard();

		// Token: 0x04000029 RID: 41
		private PhoneApplicationFrame _frame;

		// Token: 0x0400002A RID: 42
		private PhoneApplicationPage _page;

		// Token: 0x0400002B RID: 43
		private FrameworkElement _itemsPresenterHostParent;

		// Token: 0x0400002C RID: 44
		private Canvas _itemsPresenterHostPart;

		// Token: 0x0400002D RID: 45
		private ItemsPresenter _itemsPresenterPart;

		// Token: 0x0400002E RID: 46
		private Popup _fullModePopupPart;

		// Token: 0x0400002F RID: 47
		private Selector _fullModeSelectorPart;

		// Token: 0x04000030 RID: 48
		private TranslateTransform _itemsPresenterTranslateTransformPart;

		// Token: 0x04000031 RID: 49
		private bool _updatingSelection;

		// Token: 0x04000032 RID: 50
		private bool _savedSystemTrayIsVisible;

		// Token: 0x04000033 RID: 51
		private bool _savedApplicationBarIsVisible;

		// Token: 0x04000034 RID: 52
		private int _deferredSelectedIndex = -1;

		// Token: 0x04000036 RID: 54
		public static readonly DependencyProperty ListPickerModeProperty = DependencyProperty.Register("ListPickerMode", typeof(ListPickerMode), typeof(ListPicker), new PropertyMetadata(ListPickerMode.Normal, new PropertyChangedCallback(ListPicker.OnListPickerModeChanged)));

		// Token: 0x04000037 RID: 55
		public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(ListPicker), new PropertyMetadata(-1, new PropertyChangedCallback(ListPicker.OnSelectedIndexChanged)));

		// Token: 0x04000038 RID: 56
		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(ListPicker), new PropertyMetadata(null, new PropertyChangedCallback(ListPicker.OnSelectedItemChanged)));

		// Token: 0x04000039 RID: 57
		private static readonly DependencyProperty ShadowItemTemplateProperty = DependencyProperty.Register("ShadowItemTemplate", typeof(DataTemplate), typeof(ListPicker), new PropertyMetadata(null, new PropertyChangedCallback(ListPicker.OnShadowOrFullModeItemTemplateChanged)));

		// Token: 0x0400003A RID: 58
		public static readonly DependencyProperty FullModeItemTemplateProperty = DependencyProperty.Register("FullModeItemTemplate", typeof(DataTemplate), typeof(ListPicker), new PropertyMetadata(null, new PropertyChangedCallback(ListPicker.OnShadowOrFullModeItemTemplateChanged)));

		// Token: 0x0400003B RID: 59
		private static readonly DependencyProperty ActualFullModeItemTemplateProperty = DependencyProperty.Register("ActualFullModeItemTemplate", typeof(DataTemplate), typeof(ListPicker), null);

		// Token: 0x0400003C RID: 60
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(ListPicker), null);

		// Token: 0x0400003D RID: 61
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(ListPicker), null);

		// Token: 0x0400003E RID: 62
		public static readonly DependencyProperty FullModeHeaderProperty = DependencyProperty.Register("FullModeHeader", typeof(object), typeof(ListPicker), null);

		// Token: 0x0400003F RID: 63
		public static readonly DependencyProperty ItemCountThresholdProperty = DependencyProperty.Register("ItemCountThreshold", typeof(int), typeof(ListPicker), new PropertyMetadata(5, new PropertyChangedCallback(ListPicker.OnItemCountThresholdChanged)));
	}
}
