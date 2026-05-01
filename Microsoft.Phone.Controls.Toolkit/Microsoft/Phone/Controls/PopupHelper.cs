using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000040 RID: 64
	internal class PopupHelper
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00008EB4 File Offset: 0x000070B4
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x00008EBC File Offset: 0x000070BC
		public bool UsesClosingVisualState { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00008EC5 File Offset: 0x000070C5
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x00008ECD File Offset: 0x000070CD
		private Control Parent { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00008ED6 File Offset: 0x000070D6
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x00008EDE File Offset: 0x000070DE
		private Canvas OutsidePopupCanvas { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00008EE7 File Offset: 0x000070E7
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x00008EEF File Offset: 0x000070EF
		private Canvas PopupChildCanvas { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00008EF8 File Offset: 0x000070F8
		// (set) Token: 0x060001FA RID: 506 RVA: 0x00008F00 File Offset: 0x00007100
		public double MaxDropDownHeight { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00008F09 File Offset: 0x00007109
		// (set) Token: 0x060001FC RID: 508 RVA: 0x00008F11 File Offset: 0x00007111
		public Popup Popup { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00008F1A File Offset: 0x0000711A
		// (set) Token: 0x060001FE RID: 510 RVA: 0x00008F27 File Offset: 0x00007127
		public bool IsOpen
		{
			get
			{
				return this.Popup.IsOpen;
			}
			set
			{
				this.Popup.IsOpen = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00008F35 File Offset: 0x00007135
		// (set) Token: 0x06000200 RID: 512 RVA: 0x00008F3D File Offset: 0x0000713D
		private FrameworkElement PopupChild { get; set; }

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000201 RID: 513 RVA: 0x00008F48 File Offset: 0x00007148
		// (remove) Token: 0x06000202 RID: 514 RVA: 0x00008F80 File Offset: 0x00007180
		public event EventHandler Closed;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000203 RID: 515 RVA: 0x00008FB8 File Offset: 0x000071B8
		// (remove) Token: 0x06000204 RID: 516 RVA: 0x00008FF0 File Offset: 0x000071F0
		public event EventHandler FocusChanged;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000205 RID: 517 RVA: 0x00009028 File Offset: 0x00007228
		// (remove) Token: 0x06000206 RID: 518 RVA: 0x00009060 File Offset: 0x00007260
		public event EventHandler UpdateVisualStates;

		// Token: 0x06000207 RID: 519 RVA: 0x00009095 File Offset: 0x00007295
		public PopupHelper(Control parent)
		{
			this.Parent = parent;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000090A4 File Offset: 0x000072A4
		public PopupHelper(Control parent, Popup popup) : this(parent)
		{
			this.Popup = popup;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000090B4 File Offset: 0x000072B4
		private MatrixTransform GetControlMatrixTransform()
		{
			MatrixTransform result;
			try
			{
				result = (this.Parent.TransformToVisual(null) as MatrixTransform);
			}
			catch
			{
				this.OnClosed(EventArgs.Empty);
				result = null;
			}
			return result;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x000090F8 File Offset: 0x000072F8
		private static Point MatrixTransformPoint(MatrixTransform matrixTransform, Thickness margin)
		{
			double num = matrixTransform.Matrix.OffsetX + margin.Left;
			double num2 = matrixTransform.Matrix.OffsetY + margin.Top;
			return new Point(num, num2);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000913C File Offset: 0x0000733C
		private Size GetControlSize(Thickness margin, Size? finalSize)
		{
			double num = ((finalSize != null) ? finalSize.Value.Width : this.Parent.ActualWidth) - margin.Left - margin.Right;
			double num2 = ((finalSize != null) ? finalSize.Value.Height : this.Parent.ActualHeight) - margin.Top - margin.Bottom;
			return new Size(num, num2);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000091BC File Offset: 0x000073BC
		private Thickness GetMargin()
		{
			Thickness? thickness = this.Popup.Resources["PhoneTouchTargetOverhang"] as Thickness?;
			if (thickness != null)
			{
				return thickness.Value;
			}
			return new Thickness(0.0);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00009208 File Offset: 0x00007408
		private static bool IsChildAbove(Size displaySize, Size controlSize, Point controlOffset)
		{
			double y = controlOffset.Y;
			double num = displaySize.Height - controlSize.Height - y;
			return y > num;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00009233 File Offset: 0x00007433
		private static double Min0(double x, double y)
		{
			return Math.Max(Math.Min(x, y), 0.0);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000924C File Offset: 0x0000744C
		private Size AboveChildSize(Size controlSize, Point controlPoint)
		{
			double width = controlSize.Width;
			double x = controlPoint.Y - 2.0;
			double maxDropDownHeight = this.MaxDropDownHeight;
			double num = PopupHelper.Min0(x, maxDropDownHeight);
			return new Size(width, num);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000928C File Offset: 0x0000748C
		private Size BelowChildSize(Size displaySize, Size controlSize, Point controlPoint)
		{
			double width = controlSize.Width;
			double x = displaySize.Height - controlSize.Height - controlPoint.Y - 2.0;
			double maxDropDownHeight = this.MaxDropDownHeight;
			double num = PopupHelper.Min0(x, maxDropDownHeight);
			return new Size(width, num);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000092DC File Offset: 0x000074DC
		private Point AboveChildPoint(Thickness margin)
		{
			double left = margin.Left;
			double num = margin.Top - this.PopupChild.ActualHeight - 2.0;
			return new Point(left, num);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00009318 File Offset: 0x00007518
		private static Point BelowChildPoint(Thickness margin, Size controlSize)
		{
			double left = margin.Left;
			double num = margin.Top + controlSize.Height + 2.0;
			return new Point(left, num);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00009350 File Offset: 0x00007550
		public void Arrange(Size? finalSize)
		{
			if (this.Popup == null || this.PopupChild == null || Application.Current == null || this.OutsidePopupCanvas == null || Application.Current.Host == null || Application.Current.Host.Content == null)
			{
				return;
			}
			PhoneApplicationFrame phoneApplicationFrame;
			if (!PhoneHelper.TryGetPhoneApplicationFrame(out phoneApplicationFrame))
			{
				return;
			}
			Size usefulSize = phoneApplicationFrame.GetUsefulSize();
			Thickness margin = this.GetMargin();
			Size controlSize = this.GetControlSize(margin, finalSize);
			MatrixTransform controlMatrixTransform = this.GetControlMatrixTransform();
			if (controlMatrixTransform == null)
			{
				return;
			}
			Point point = PopupHelper.MatrixTransformPoint(controlMatrixTransform, margin);
			Size sipUncoveredSize = phoneApplicationFrame.GetSipUncoveredSize();
			bool flag = PopupHelper.IsChildAbove(sipUncoveredSize, controlSize, point);
			Size size = flag ? this.AboveChildSize(controlSize, point) : this.BelowChildSize(sipUncoveredSize, controlSize, point);
			if (usefulSize.Width == 0.0 || usefulSize.Height == 0.0 || size.Height == 0.0)
			{
				return;
			}
			Point point2 = flag ? this.AboveChildPoint(margin) : PopupHelper.BelowChildPoint(margin, controlSize);
			this.Popup.HorizontalOffset = 0.0;
			this.Popup.VerticalOffset = 0.0;
			this.PopupChild.Width = (this.PopupChild.MaxWidth = (this.PopupChild.MinWidth = controlSize.Width));
			this.PopupChild.MinHeight = 0.0;
			this.PopupChild.MaxHeight = size.Height;
			this.PopupChild.HorizontalAlignment = 0;
			this.PopupChild.VerticalAlignment = 0;
			Canvas.SetLeft(this.PopupChild, point2.X);
			Canvas.SetTop(this.PopupChild, point2.Y);
			this.OutsidePopupCanvas.Width = controlSize.Width;
			this.OutsidePopupCanvas.Height = usefulSize.Height;
			Matrix matrix = controlMatrixTransform.Matrix;
			Matrix matrix2;
			matrix.Invert(out matrix2);
			controlMatrixTransform.Matrix = matrix2;
			this.OutsidePopupCanvas.RenderTransform = controlMatrixTransform;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00009558 File Offset: 0x00007758
		private void OnClosed(EventArgs e)
		{
			EventHandler closed = this.Closed;
			if (closed != null)
			{
				closed.Invoke(this, e);
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00009578 File Offset: 0x00007778
		private void OnPopupClosedStateChanged(object sender, VisualStateChangedEventArgs e)
		{
			if (e != null && e.NewState != null && e.NewState.Name == "PopupClosed")
			{
				if (this.Popup != null)
				{
					this.Popup.IsOpen = false;
				}
				this.OnClosed(EventArgs.Empty);
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000095C8 File Offset: 0x000077C8
		public void BeforeOnApplyTemplate()
		{
			if (this.UsesClosingVisualState)
			{
				VisualStateGroup visualStateGroup = VisualStates.TryGetVisualStateGroup(this.Parent, "PopupStates");
				if (visualStateGroup != null)
				{
					visualStateGroup.CurrentStateChanged -= new EventHandler<VisualStateChangedEventArgs>(this.OnPopupClosedStateChanged);
					this.UsesClosingVisualState = false;
				}
			}
			if (this.Popup != null)
			{
				this.Popup.Closed -= new EventHandler(this.Popup_Closed);
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000962C File Offset: 0x0000782C
		public void AfterOnApplyTemplate()
		{
			if (this.Popup != null)
			{
				this.Popup.Closed += new EventHandler(this.Popup_Closed);
			}
			VisualStateGroup visualStateGroup = VisualStates.TryGetVisualStateGroup(this.Parent, "PopupStates");
			if (visualStateGroup != null)
			{
				visualStateGroup.CurrentStateChanged += new EventHandler<VisualStateChangedEventArgs>(this.OnPopupClosedStateChanged);
				this.UsesClosingVisualState = true;
			}
			if (this.Popup != null)
			{
				this.PopupChild = (this.Popup.Child as FrameworkElement);
				if (this.PopupChild != null && !this._hasControlLoaded)
				{
					this._hasControlLoaded = true;
					this.PopupChildCanvas = new Canvas();
					this.Popup.Child = this.PopupChildCanvas;
					this.OutsidePopupCanvas = new Canvas();
					this.OutsidePopupCanvas.Background = new SolidColorBrush(Colors.Transparent);
					this.OutsidePopupCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(this.OutsidePopup_MouseLeftButtonDown);
					this.PopupChildCanvas.Children.Add(this.OutsidePopupCanvas);
					this.PopupChildCanvas.Children.Add(this.PopupChild);
					this.PopupChild.GotFocus += new RoutedEventHandler(this.PopupChild_GotFocus);
					this.PopupChild.LostFocus += new RoutedEventHandler(this.PopupChild_LostFocus);
					this.PopupChild.MouseEnter += new MouseEventHandler(this.PopupChild_MouseEnter);
					this.PopupChild.MouseLeave += new MouseEventHandler(this.PopupChild_MouseLeave);
					this.PopupChild.SizeChanged += new SizeChangedEventHandler(this.PopupChild_SizeChanged);
				}
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000097B8 File Offset: 0x000079B8
		private void PopupChild_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.Arrange(default(Size?));
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000097D4 File Offset: 0x000079D4
		private void OutsidePopup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (this.Popup != null)
			{
				this.Popup.IsOpen = false;
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000097EA File Offset: 0x000079EA
		private void Popup_Closed(object sender, EventArgs e)
		{
			this.OnClosed(EventArgs.Empty);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000097F8 File Offset: 0x000079F8
		private void OnFocusChanged(EventArgs e)
		{
			EventHandler focusChanged = this.FocusChanged;
			if (focusChanged != null)
			{
				focusChanged.Invoke(this, e);
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00009818 File Offset: 0x00007A18
		private void OnUpdateVisualStates(EventArgs e)
		{
			EventHandler updateVisualStates = this.UpdateVisualStates;
			if (updateVisualStates != null)
			{
				updateVisualStates.Invoke(this, e);
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00009837 File Offset: 0x00007A37
		private void PopupChild_GotFocus(object sender, RoutedEventArgs e)
		{
			this.OnFocusChanged(EventArgs.Empty);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00009844 File Offset: 0x00007A44
		private void PopupChild_LostFocus(object sender, RoutedEventArgs e)
		{
			this.OnFocusChanged(EventArgs.Empty);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00009851 File Offset: 0x00007A51
		private void PopupChild_MouseEnter(object sender, MouseEventArgs e)
		{
			this.OnUpdateVisualStates(EventArgs.Empty);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000985E File Offset: 0x00007A5E
		private void PopupChild_MouseLeave(object sender, MouseEventArgs e)
		{
			this.OnUpdateVisualStates(EventArgs.Empty);
		}

		// Token: 0x040000C3 RID: 195
		private const double PopupOffset = 2.0;

		// Token: 0x040000C4 RID: 196
		private bool _hasControlLoaded;
	}
}
