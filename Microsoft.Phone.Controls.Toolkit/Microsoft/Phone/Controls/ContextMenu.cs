using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Shell;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000072 RID: 114
	[TemplateVisualState(GroupName = "VisibilityStates", Name = "Closed")]
	[TemplateVisualState(GroupName = "VisibilityStates", Name = "Open")]
	public class ContextMenu : MenuBase
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0001305A File Offset: 0x0001125A
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x00013064 File Offset: 0x00011264
		internal DependencyObject Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				if (this._owner != null)
				{
					FrameworkElement frameworkElement = this._owner as FrameworkElement;
					if (frameworkElement != null)
					{
						GestureListener gestureListener = GestureService.GetGestureListener(frameworkElement);
						gestureListener.Hold -= new EventHandler<GestureEventArgs>(this.HandleOwnerHold);
						frameworkElement.Loaded -= new RoutedEventHandler(this.HandleOwnerLoaded);
						frameworkElement.Unloaded -= new RoutedEventHandler(this.HandleOwnerUnloaded);
						this.HandleOwnerUnloaded(null, null);
					}
				}
				this._owner = value;
				if (this._owner != null)
				{
					FrameworkElement frameworkElement2 = this._owner as FrameworkElement;
					if (frameworkElement2 != null)
					{
						GestureListener gestureListener2 = GestureService.GetGestureListener(frameworkElement2);
						gestureListener2.Hold += new EventHandler<GestureEventArgs>(this.HandleOwnerHold);
						frameworkElement2.Loaded += new RoutedEventHandler(this.HandleOwnerLoaded);
						frameworkElement2.Unloaded += new RoutedEventHandler(this.HandleOwnerUnloaded);
						DependencyObject dependencyObject = frameworkElement2;
						while (dependencyObject != null)
						{
							dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
							if (dependencyObject != null && dependencyObject == this._rootVisual)
							{
								this.HandleOwnerLoaded(null, null);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x00013151 File Offset: 0x00011351
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x00013163 File Offset: 0x00011363
		public bool IsZoomEnabled
		{
			get
			{
				return (bool)base.GetValue(ContextMenu.IsZoomEnabledProperty);
			}
			set
			{
				base.SetValue(ContextMenu.IsZoomEnabledProperty, value);
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x00013176 File Offset: 0x00011376
		// (set) Token: 0x06000473 RID: 1139 RVA: 0x00013188 File Offset: 0x00011388
		[TypeConverter(typeof(LengthConverter))]
		public double VerticalOffset
		{
			get
			{
				return (double)base.GetValue(ContextMenu.VerticalOffsetProperty);
			}
			set
			{
				base.SetValue(ContextMenu.VerticalOffsetProperty, value);
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001319B File Offset: 0x0001139B
		private static void OnHorizontalVerticalOffsetChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((ContextMenu)o).UpdateContextMenuPlacement();
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x000131A8 File Offset: 0x000113A8
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x000131BA File Offset: 0x000113BA
		public bool IsOpen
		{
			get
			{
				return (bool)base.GetValue(ContextMenu.IsOpenProperty);
			}
			set
			{
				base.SetValue(ContextMenu.IsOpenProperty, value);
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x000131CD File Offset: 0x000113CD
		private static void OnIsOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((ContextMenu)o).OnIsOpenChanged((bool)e.NewValue);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000131E6 File Offset: 0x000113E6
		private void OnIsOpenChanged(bool newValue)
		{
			if (!this._settingIsOpen)
			{
				if (newValue)
				{
					this.OpenPopup(this._mousePosition);
					return;
				}
				this.ClosePopup();
			}
		}

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06000479 RID: 1145 RVA: 0x00013208 File Offset: 0x00011408
		// (remove) Token: 0x0600047A RID: 1146 RVA: 0x00013240 File Offset: 0x00011440
		public event RoutedEventHandler Opened;

		// Token: 0x0600047B RID: 1147 RVA: 0x00013278 File Offset: 0x00011478
		protected virtual void OnOpened(RoutedEventArgs e)
		{
			this.GoToVisualState("Open", true);
			RoutedEventHandler opened = this.Opened;
			if (opened != null)
			{
				opened.Invoke(this, e);
			}
		}

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x0600047C RID: 1148 RVA: 0x000132A4 File Offset: 0x000114A4
		// (remove) Token: 0x0600047D RID: 1149 RVA: 0x000132DC File Offset: 0x000114DC
		public event RoutedEventHandler Closed;

		// Token: 0x0600047E RID: 1150 RVA: 0x00013314 File Offset: 0x00011514
		protected virtual void OnClosed(RoutedEventArgs e)
		{
			this.GoToVisualState("Closed", true);
			RoutedEventHandler closed = this.Closed;
			if (closed != null)
			{
				closed.Invoke(this, e);
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001333F File Offset: 0x0001153F
		public ContextMenu()
		{
			base.DefaultStyleKey = typeof(ContextMenu);
			base.LayoutUpdated += new EventHandler(this.HandleLayoutUpdated);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00013374 File Offset: 0x00011574
		public override void OnApplyTemplate()
		{
			if (this._openingStoryboard != null)
			{
				this._openingStoryboard.Completed -= new EventHandler(this.HandleStoryboardCompleted);
				this._openingStoryboard = null;
			}
			this._openingStoryboardPlaying = false;
			base.OnApplyTemplate();
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
							if ("Open" == visualState.Name && visualState.Storyboard != null)
							{
								this._openingStoryboard = visualState.Storyboard;
								this._openingStoryboard.Completed += new EventHandler(this.HandleStoryboardCompleted);
							}
						}
					}
				}
			}
			this.GoToVisualState("Closed", false);
			if (this.IsOpen)
			{
				this.GoToVisualState("Open", true);
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000134CC File Offset: 0x000116CC
		private void HandleStoryboardCompleted(object sender, EventArgs e)
		{
			this._openingStoryboardPlaying = false;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000134D8 File Offset: 0x000116D8
		private void GoToVisualState(string stateName, bool useTransitions)
		{
			if ("Open" == stateName && this._openingStoryboard != null)
			{
				this._openingStoryboardPlaying = true;
				this._openingStoryboardReleaseThreshold = DateTime.UtcNow.AddSeconds(0.3);
			}
			VisualStateManager.GoToState(this, stateName, useTransitions);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00013526 File Offset: 0x00011726
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			e.Handled = true;
			base.OnMouseLeftButtonDown(e);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00013544 File Offset: 0x00011744
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			Key key = e.Key;
			if (key != 8)
			{
				switch (key)
				{
				case 15:
					this.FocusNextItem(false);
					e.Handled = true;
					break;
				case 17:
					this.FocusNextItem(true);
					e.Handled = true;
					break;
				}
			}
			else
			{
				this.ClosePopup();
				e.Handled = true;
			}
			base.OnKeyDown(e);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000135B5 File Offset: 0x000117B5
		private void HandleLayoutUpdated(object sender, EventArgs e)
		{
			if (Application.Current.RootVisual != null)
			{
				this.InitializeRootVisual();
				base.LayoutUpdated -= new EventHandler(this.HandleLayoutUpdated);
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x000135DB File Offset: 0x000117DB
		private void HandleRootVisualMouseMove(object sender, MouseEventArgs e)
		{
			this._mousePosition = e.GetPosition(null);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x000135EA File Offset: 0x000117EA
		private void HandleRootVisualManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			if (this._openingStoryboardPlaying && DateTime.UtcNow <= this._openingStoryboardReleaseThreshold)
			{
				this.IsOpen = false;
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001360D File Offset: 0x0001180D
		private void HandleOwnerHold(object sender, GestureEventArgs e)
		{
			if (!this.IsOpen)
			{
				this.OpenPopup(e.GetPosition(null));
				e.Handled = true;
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001362B File Offset: 0x0001182B
		private static void OnApplicationBarMirrorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((ContextMenu)o).OnApplicationBarMirrorChanged((IApplicationBar)e.OldValue, (IApplicationBar)e.NewValue);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00013650 File Offset: 0x00011850
		private void OnApplicationBarMirrorChanged(IApplicationBar oldValue, IApplicationBar newValue)
		{
			if (oldValue != null)
			{
				oldValue.StateChanged -= new EventHandler<ApplicationBarStateChangedEventArgs>(this.HandleEventThatClosesContextMenu);
			}
			if (newValue != null)
			{
				newValue.StateChanged += new EventHandler<ApplicationBarStateChangedEventArgs>(this.HandleEventThatClosesContextMenu);
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001367C File Offset: 0x0001187C
		private void HandleEventThatClosesContextMenu(object sender, EventArgs e)
		{
			this.IsOpen = false;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00013688 File Offset: 0x00011888
		private void HandleOwnerLoaded(object sender, RoutedEventArgs e)
		{
			if (this._page == null)
			{
				this.InitializeRootVisual();
				if (this._rootVisual != null)
				{
					this._page = (this._rootVisual.Content as PhoneApplicationPage);
					if (this._page != null)
					{
						this._page.BackKeyPress += new EventHandler<CancelEventArgs>(this.HandlePageBackKeyPress);
						base.SetBinding(ContextMenu.ApplicationBarMirrorProperty, new Binding
						{
							Source = this._page,
							Path = new PropertyPath("ApplicationBar", new object[0])
						});
					}
				}
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00013715 File Offset: 0x00011915
		private void HandleOwnerUnloaded(object sender, RoutedEventArgs e)
		{
			if (this._page != null)
			{
				this._page.BackKeyPress -= new EventHandler<CancelEventArgs>(this.HandlePageBackKeyPress);
				base.ClearValue(ContextMenu.ApplicationBarMirrorProperty);
				this._page = null;
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00013748 File Offset: 0x00011948
		private void HandlePageBackKeyPress(object sender, CancelEventArgs e)
		{
			if (this.IsOpen)
			{
				this.IsOpen = false;
				e.Cancel = true;
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00013760 File Offset: 0x00011960
		private static GeneralTransform SafeTransformToVisual(UIElement element, UIElement visual)
		{
			GeneralTransform result;
			try
			{
				result = element.TransformToVisual(visual);
			}
			catch (ArgumentException)
			{
				result = new TranslateTransform();
			}
			return result;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00013794 File Offset: 0x00011994
		private void InitializeRootVisual()
		{
			if (this._rootVisual == null)
			{
				this._rootVisual = (Application.Current.RootVisual as PhoneApplicationFrame);
				if (this._rootVisual != null)
				{
					this._rootVisual.MouseMove += new MouseEventHandler(this.HandleRootVisualMouseMove);
					this._rootVisual.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.HandleRootVisualManipulationCompleted);
					this._rootVisual.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(this.HandleEventThatClosesContextMenu);
				}
			}
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0001380C File Offset: 0x00011A0C
		private void FocusNextItem(bool down)
		{
			int count = base.Items.Count;
			int num = down ? -1 : count;
			MenuItem menuItem = FocusManager.GetFocusedElement() as MenuItem;
			if (menuItem != null && this == menuItem.ParentMenuBase)
			{
				num = base.ItemContainerGenerator.IndexFromContainer(menuItem);
			}
			int num2 = num;
			for (;;)
			{
				num2 = (num2 + count + (down ? 1 : -1)) % count;
				MenuItem menuItem2 = base.ItemContainerGenerator.ContainerFromIndex(num2) as MenuItem;
				if (menuItem2 != null && menuItem2.IsEnabled && menuItem2.Focus())
				{
					break;
				}
				if (num2 == num)
				{
					return;
				}
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001388F File Offset: 0x00011A8F
		internal void ChildMenuItemClicked()
		{
			this.ClosePopup();
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00013897 File Offset: 0x00011A97
		private void HandleContextMenuOrRootVisualSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.UpdateContextMenuPlacement();
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0001389F File Offset: 0x00011A9F
		private void HandleOverlayMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.ClosePopup();
			e.Handled = true;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x000138B0 File Offset: 0x00011AB0
		private void UpdateContextMenuPlacement()
		{
			if (this._rootVisual != null && this._overlay != null)
			{
				double num = this._popupAlignmentPoint.X;
				double num2 = this._popupAlignmentPoint.Y;
				num2 += this.VerticalOffset;
				bool flag = (this._rootVisual.Orientation & 1) == 1;
				double num3 = flag ? this._rootVisual.ActualWidth : this._rootVisual.ActualHeight;
				double num4 = flag ? this._rootVisual.ActualHeight : this._rootVisual.ActualWidth;
				Rect rect;
				rect..ctor(0.0, 0.0, num3, num4);
				if (this._page != null)
				{
					rect = ContextMenu.SafeTransformToVisual(this._page, this._rootVisual).TransformBounds(new Rect(0.0, 0.0, this._page.ActualWidth, this._page.ActualHeight));
				}
				num = rect.Left;
				base.Width = rect.Width;
				num2 = Math.Min(num2, rect.Bottom - base.ActualHeight);
				num2 = Math.Max(num2, rect.Top);
				num = Math.Max(num, 0.0);
				num2 = Math.Max(num2, 0.0);
				Canvas.SetLeft(this, num);
				Canvas.SetTop(this, num2);
				this._overlay.Width = num3;
				this._overlay.Height = num4;
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00013A28 File Offset: 0x00011C28
		private void OpenPopup(Point position)
		{
			this._popupAlignmentPoint = position;
			this.InitializeRootVisual();
			this._overlay = new Canvas
			{
				Background = new SolidColorBrush(Colors.Transparent)
			};
			this._overlay.MouseLeftButtonDown += new MouseButtonEventHandler(this.HandleOverlayMouseButtonDown);
			this._overlay.Children.Add(this);
			if (this.IsZoomEnabled && this._rootVisual != null)
			{
				bool flag = 1 == (1 & this._rootVisual.Orientation);
				double num = flag ? this._rootVisual.ActualWidth : this._rootVisual.ActualHeight;
				double num2 = flag ? this._rootVisual.ActualHeight : this._rootVisual.ActualWidth;
				UIElement uielement = new Rectangle
				{
					Width = num,
					Height = num2,
					Fill = (Brush)Application.Current.Resources["PhoneBackgroundBrush"]
				};
				this._overlay.Children.Insert(0, uielement);
				WriteableBitmap writeableBitmap = new WriteableBitmap((int)num, (int)num2);
				writeableBitmap.Render(this._rootVisual, null);
				writeableBitmap.Invalidate();
				Transform transform = new ScaleTransform
				{
					CenterX = num / 2.0,
					CenterY = num2 / 2.0
				};
				UIElement uielement2 = new Image
				{
					Source = writeableBitmap,
					RenderTransform = transform
				};
				this._overlay.Children.Insert(1, uielement2);
				FrameworkElement frameworkElement = this._owner as FrameworkElement;
				if (frameworkElement != null)
				{
					Point point = ContextMenu.SafeTransformToVisual(frameworkElement, this._rootVisual).Transform(default(Point));
					UIElement uielement3 = new Rectangle
					{
						Width = frameworkElement.ActualWidth,
						Height = frameworkElement.ActualHeight,
						Fill = (Brush)Application.Current.Resources["PhoneBackgroundBrush"]
					};
					Canvas.SetLeft(uielement3, point.X);
					Canvas.SetTop(uielement3, point.Y);
					this._overlay.Children.Insert(2, uielement3);
					UIElement uielement4 = new Image
					{
						Source = new WriteableBitmap(frameworkElement, null)
					};
					Canvas.SetLeft(uielement4, point.X);
					Canvas.SetTop(uielement4, point.Y);
					this._overlay.Children.Insert(3, uielement4);
				}
				double num3 = 1.0;
				double num4 = 0.94;
				TimeSpan timeSpan = TimeSpan.FromSeconds(0.4);
				IEasingFunction easingFunction = new ExponentialEase
				{
					EasingMode = 2
				};
				this._backgroundResizeStoryboard = new Storyboard();
				DoubleAnimation doubleAnimation = new DoubleAnimation
				{
					From = new double?(num3),
					To = new double?(num4),
					Duration = timeSpan,
					EasingFunction = easingFunction
				};
				Storyboard.SetTarget(doubleAnimation, transform);
				Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(ScaleTransform.ScaleXProperty));
				this._backgroundResizeStoryboard.Children.Add(doubleAnimation);
				DoubleAnimation doubleAnimation2 = new DoubleAnimation
				{
					From = new double?(num3),
					To = new double?(num4),
					Duration = timeSpan,
					EasingFunction = easingFunction
				};
				Storyboard.SetTarget(doubleAnimation2, transform);
				Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(ScaleTransform.ScaleYProperty));
				this._backgroundResizeStoryboard.Children.Add(doubleAnimation2);
				this._backgroundResizeStoryboard.Begin();
			}
			TransformGroup transformGroup = new TransformGroup();
			if (this._rootVisual != null)
			{
				PageOrientation orientation = this._rootVisual.Orientation;
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
							Y = this._rootVisual.ActualHeight
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
						X = this._rootVisual.ActualWidth
					});
				}
			}
			this._overlay.RenderTransform = transformGroup;
			if (this._page != null && this._page.ApplicationBar != null && this._page.ApplicationBar.Buttons != null)
			{
				foreach (object obj in this._page.ApplicationBar.Buttons)
				{
					ApplicationBarIconButton applicationBarIconButton = obj as ApplicationBarIconButton;
					if (applicationBarIconButton != null)
					{
						applicationBarIconButton.Click += new EventHandler(this.HandleEventThatClosesContextMenu);
						this._applicationBarIconButtons.Add(applicationBarIconButton);
					}
				}
			}
			this._popup = new Popup
			{
				Child = this._overlay
			};
			base.SizeChanged += new SizeChangedEventHandler(this.HandleContextMenuOrRootVisualSizeChanged);
			if (this._rootVisual != null)
			{
				this._rootVisual.SizeChanged += new SizeChangedEventHandler(this.HandleContextMenuOrRootVisualSizeChanged);
			}
			this.UpdateContextMenuPlacement();
			if (base.ReadLocalValue(FrameworkElement.DataContextProperty) == DependencyProperty.UnsetValue)
			{
				DependencyObject source = this.Owner ?? this._rootVisual;
				base.SetBinding(FrameworkElement.DataContextProperty, new Binding("DataContext")
				{
					Source = source
				});
			}
			this._popup.IsOpen = true;
			base.Focus();
			this._settingIsOpen = true;
			this.IsOpen = true;
			this._settingIsOpen = false;
			this.OnOpened(new RoutedEventArgs());
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001405C File Offset: 0x0001225C
		private void ClosePopup()
		{
			if (this._backgroundResizeStoryboard != null)
			{
				foreach (Timeline timeline in this._backgroundResizeStoryboard.Children)
				{
					DoubleAnimation doubleAnimation = (DoubleAnimation)timeline;
					double value = doubleAnimation.From.Value;
					doubleAnimation.From = doubleAnimation.To;
					doubleAnimation.To = new double?(value);
				}
				Popup popup = this._popup;
				Panel overlay = this._overlay;
				this._backgroundResizeStoryboard.Completed += delegate(object A_1, EventArgs A_2)
				{
					if (popup != null)
					{
						popup.IsOpen = false;
						popup.Child = null;
					}
					if (overlay != null)
					{
						overlay.Children.Clear();
					}
				};
				this._backgroundResizeStoryboard.Begin();
				this._backgroundResizeStoryboard = null;
				this._popup = null;
				this._overlay = null;
			}
			else
			{
				if (this._popup != null)
				{
					this._popup.IsOpen = false;
					this._popup.Child = null;
					this._popup = null;
				}
				if (this._overlay != null)
				{
					this._overlay.Children.Clear();
					this._overlay = null;
				}
			}
			base.SizeChanged -= new SizeChangedEventHandler(this.HandleContextMenuOrRootVisualSizeChanged);
			if (this._rootVisual != null)
			{
				this._rootVisual.SizeChanged -= new SizeChangedEventHandler(this.HandleContextMenuOrRootVisualSizeChanged);
			}
			foreach (ApplicationBarIconButton applicationBarIconButton in this._applicationBarIconButtons)
			{
				applicationBarIconButton.Click -= new EventHandler(this.HandleEventThatClosesContextMenu);
			}
			this._applicationBarIconButtons.Clear();
			this._settingIsOpen = true;
			this.IsOpen = false;
			this._settingIsOpen = false;
			this.OnClosed(new RoutedEventArgs());
		}

		// Token: 0x04000246 RID: 582
		private const string VisibilityGroupName = "VisibilityStates";

		// Token: 0x04000247 RID: 583
		private const string OpenVisibilityStateName = "Open";

		// Token: 0x04000248 RID: 584
		private const string ClosedVisibilityStateName = "Closed";

		// Token: 0x04000249 RID: 585
		private PhoneApplicationPage _page;

		// Token: 0x0400024A RID: 586
		private readonly List<ApplicationBarIconButton> _applicationBarIconButtons = new List<ApplicationBarIconButton>();

		// Token: 0x0400024B RID: 587
		private Storyboard _backgroundResizeStoryboard;

		// Token: 0x0400024C RID: 588
		private Storyboard _openingStoryboard;

		// Token: 0x0400024D RID: 589
		private bool _openingStoryboardPlaying;

		// Token: 0x0400024E RID: 590
		private DateTime _openingStoryboardReleaseThreshold;

		// Token: 0x0400024F RID: 591
		private PhoneApplicationFrame _rootVisual;

		// Token: 0x04000250 RID: 592
		private Point _mousePosition;

		// Token: 0x04000251 RID: 593
		private DependencyObject _owner;

		// Token: 0x04000252 RID: 594
		private Popup _popup;

		// Token: 0x04000253 RID: 595
		private Panel _overlay;

		// Token: 0x04000254 RID: 596
		private Point _popupAlignmentPoint;

		// Token: 0x04000255 RID: 597
		private bool _settingIsOpen;

		// Token: 0x04000256 RID: 598
		public static readonly DependencyProperty IsZoomEnabledProperty = DependencyProperty.Register("IsZoomEnabled", typeof(bool), typeof(ContextMenu), new PropertyMetadata(true));

		// Token: 0x04000257 RID: 599
		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(ContextMenu), new PropertyMetadata(0.0, new PropertyChangedCallback(ContextMenu.OnHorizontalVerticalOffsetChanged)));

		// Token: 0x04000258 RID: 600
		public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(ContextMenu), new PropertyMetadata(false, new PropertyChangedCallback(ContextMenu.OnIsOpenChanged)));

		// Token: 0x0400025B RID: 603
		private static readonly DependencyProperty ApplicationBarMirrorProperty = DependencyProperty.Register("ApplicationBarMirror", typeof(IApplicationBar), typeof(ContextMenu), new PropertyMetadata(new PropertyChangedCallback(ContextMenu.OnApplicationBarMirrorChanged)));
	}
}
