using System;
using System.Windows.Media;

namespace System.Windows.Controls
{
	// Token: 0x02000032 RID: 50
	internal sealed class ItemsControlHelper
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00007E8F File Offset: 0x0000608F
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00007E97 File Offset: 0x00006097
		private ItemsControl ItemsControl { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00007EA0 File Offset: 0x000060A0
		internal Panel ItemsHost
		{
			get
			{
				if (this._itemsHost == null && this.ItemsControl != null && this.ItemsControl.ItemContainerGenerator != null)
				{
					DependencyObject dependencyObject = this.ItemsControl.ItemContainerGenerator.ContainerFromIndex(0);
					if (dependencyObject != null)
					{
						this._itemsHost = (VisualTreeHelper.GetParent(dependencyObject) as Panel);
					}
				}
				return this._itemsHost;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00007EF8 File Offset: 0x000060F8
		internal ScrollViewer ScrollHost
		{
			get
			{
				if (this._scrollHost == null)
				{
					Panel itemsHost = this.ItemsHost;
					if (itemsHost != null)
					{
						DependencyObject dependencyObject = itemsHost;
						while (dependencyObject != this.ItemsControl && dependencyObject != null)
						{
							ScrollViewer scrollViewer = dependencyObject as ScrollViewer;
							if (scrollViewer != null)
							{
								this._scrollHost = scrollViewer;
								break;
							}
							dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
						}
					}
				}
				return this._scrollHost;
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00007F47 File Offset: 0x00006147
		internal ItemsControlHelper(ItemsControl control)
		{
			this.ItemsControl = control;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00007F56 File Offset: 0x00006156
		internal void OnApplyTemplate()
		{
			this._itemsHost = null;
			this._scrollHost = null;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00007F68 File Offset: 0x00006168
		internal static void PrepareContainerForItemOverride(DependencyObject element, Style parentItemContainerStyle)
		{
			Control control = element as Control;
			if (parentItemContainerStyle != null && control != null && control.Style == null)
			{
				control.SetValue(FrameworkElement.StyleProperty, parentItemContainerStyle);
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007F98 File Offset: 0x00006198
		internal void UpdateItemContainerStyle(Style itemContainerStyle)
		{
			if (itemContainerStyle == null)
			{
				return;
			}
			Panel itemsHost = this.ItemsHost;
			if (itemsHost == null || itemsHost.Children == null)
			{
				return;
			}
			foreach (UIElement uielement in itemsHost.Children)
			{
				FrameworkElement frameworkElement = uielement as FrameworkElement;
				if (frameworkElement.Style == null)
				{
					frameworkElement.Style = itemContainerStyle;
				}
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000800C File Offset: 0x0000620C
		internal void ScrollIntoView(FrameworkElement element)
		{
			ScrollViewer scrollHost = this.ScrollHost;
			if (scrollHost == null)
			{
				return;
			}
			GeneralTransform generalTransform = null;
			try
			{
				generalTransform = element.TransformToVisual(scrollHost);
			}
			catch (ArgumentException)
			{
				return;
			}
			Rect rect;
			rect..ctor(generalTransform.Transform(default(Point)), generalTransform.Transform(new Point(element.ActualWidth, element.ActualHeight)));
			double num = scrollHost.VerticalOffset;
			double num2 = 0.0;
			double viewportHeight = scrollHost.ViewportHeight;
			double bottom = rect.Bottom;
			if (viewportHeight < bottom)
			{
				num2 = bottom - viewportHeight;
				num += num2;
			}
			double top = rect.Top;
			if (top - num2 < 0.0)
			{
				num -= num2 - top;
			}
			scrollHost.ScrollToVerticalOffset(num);
			double num3 = scrollHost.HorizontalOffset;
			double num4 = 0.0;
			double viewportWidth = scrollHost.ViewportWidth;
			double right = rect.Right;
			if (viewportWidth < right)
			{
				num4 = right - viewportWidth;
				num3 += num4;
			}
			double left = rect.Left;
			if (left - num4 < 0.0)
			{
				num3 -= num4 - left;
			}
			scrollHost.ScrollToHorizontalOffset(num3);
		}

		// Token: 0x0400009B RID: 155
		private Panel _itemsHost;

		// Token: 0x0400009C RID: 156
		private ScrollViewer _scrollHost;
	}
}
