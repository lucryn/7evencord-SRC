using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x0200001A RID: 26
	public class PanoramaPanel : Panel
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00004D2F File Offset: 0x00003D2F
		internal IList<PanoramaItem> VisibleChildren
		{
			get
			{
				return this._visibleChildren;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00004D37 File Offset: 0x00003D37
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00004D3F File Offset: 0x00003D3F
		private Panorama Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				if (this._owner != value)
				{
					if (this._owner != null)
					{
						this._owner.Panel = null;
					}
					this._owner = value;
					if (this._owner != null)
					{
						this._owner.Panel = this;
					}
				}
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004D7C File Offset: 0x00003D7C
		public PanoramaPanel()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.PanoramaPanel);
			base.SizeChanged += new SizeChangedEventHandler(this.PanoramaPanel_SizeChanged);
			base.Loaded += new RoutedEventHandler(this.PanoramaPanel_Loaded);
			base.Unloaded += new RoutedEventHandler(this.PanoramaPanel_UnLoaded);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004DEA File Offset: 0x00003DEA
		private void PanoramaPanel_Loaded(object sender, RoutedEventArgs e)
		{
			base.Loaded -= new RoutedEventHandler(this.PanoramaPanel_Loaded);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.PanoramaPanel);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004E0D File Offset: 0x00003E0D
		private void PanoramaPanel_UnLoaded(object sender, RoutedEventArgs e)
		{
			this._owner = null;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004E18 File Offset: 0x00003E18
		private void PanoramaPanel_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.Owner.ItemsWidth = (int)e.NewSize.Width;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004E40 File Offset: 0x00003E40
		protected override Size MeasureOverride(Size availableSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.PanoramaPanel);
			if (this._owner == null)
			{
				this.FindOwner();
			}
			int defaultItemIndex = this.GetDefaultItemIndex();
			Size result;
			result..ctor(0.0, availableSize.Height);
			int adjustedViewportWidth = this.Owner.AdjustedViewportWidth;
			int num = (int)Math.Min(availableSize.Height, (double)this.Owner.ViewportHeight);
			Size size;
			size..ctor((double)adjustedViewportWidth, (double)num);
			Size size2;
			size2..ctor(double.PositiveInfinity, (double)num);
			int count = base.Children.Count;
			this._visibleChildren.Clear();
			for (int i = 0; i < count; i++)
			{
				int num2 = (i + defaultItemIndex) % count;
				PanoramaItem panoramaItem = (PanoramaItem)base.Children[num2];
				if (panoramaItem.Visibility == null)
				{
					this._visibleChildren.Add(panoramaItem);
					panoramaItem.Measure((panoramaItem.Orientation == null) ? size : size2);
					if (panoramaItem.Orientation == null)
					{
						result.Width += (double)adjustedViewportWidth;
					}
					else
					{
						result.Width += Math.Max((double)adjustedViewportWidth, panoramaItem.DesiredSize.Width);
					}
				}
			}
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.PanoramaPanel);
			return result;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004F94 File Offset: 0x00003F94
		protected override Size ArrangeOverride(Size finalSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.PanoramaPanel);
			this._itemStops.Clear();
			double num = 0.0;
			Rect rect;
			rect..ctor(0.0, 0.0, 0.0, finalSize.Height);
			for (int i = 0; i < this._visibleChildren.Count; i++)
			{
				PanoramaItem panoramaItem = this._visibleChildren[i];
				rect.X = (double)(panoramaItem.StartPosition = (int)num);
				this._itemStops.Add(new PanoramaPanel.ItemStop(panoramaItem, i, panoramaItem.StartPosition));
				if (panoramaItem.Orientation == null)
				{
					rect.Width = (double)this.Owner.AdjustedViewportWidth;
				}
				else
				{
					rect.Width = Math.Max((double)this.Owner.AdjustedViewportWidth, panoramaItem.DesiredSize.Width);
					if (rect.Width > (double)this.Owner.AdjustedViewportWidth)
					{
						this._itemStops.Add(new PanoramaPanel.ItemStop(panoramaItem, i, panoramaItem.StartPosition + (int)rect.Width - this.Owner.AdjustedViewportWidth));
					}
				}
				panoramaItem.ItemWidth = (int)rect.Width;
				panoramaItem.Arrange(rect);
				num += rect.Width;
			}
			this.Owner.RequestAdjustSelection();
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.PanoramaPanel);
			return finalSize;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005104 File Offset: 0x00004104
		private int GetDefaultItemIndex()
		{
			PanoramaItem defaultItemContainer = this.Owner.GetDefaultItemContainer();
			int num = (defaultItemContainer != null) ? base.Children.IndexOf(defaultItemContainer) : 0;
			if (num < 0)
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005138 File Offset: 0x00004138
		private void GetItemsInView(int offset, int viewportWidth, out int leftIndex, out int leftInView, out int centerIndex, out int rightIndex, out int rightInView)
		{
			leftIndex = (leftInView = (centerIndex = (rightIndex = (rightInView = -1))));
			int count = this.VisibleChildren.Count;
			if (count == 0)
			{
				return;
			}
			for (int i = 0; i < count; i++)
			{
				PanoramaItem panoramaItem = this._visibleChildren[i];
				int num = panoramaItem.StartPosition + offset;
				int num2 = num + panoramaItem.ItemWidth - 1;
				if (num <= 0 && num2 >= 0)
				{
					leftIndex = i;
					leftInView = Math.Min(viewportWidth, panoramaItem.ItemWidth + num);
				}
				if (num < viewportWidth && num2 >= viewportWidth)
				{
					rightIndex = i;
					rightInView = Math.Min(viewportWidth, viewportWidth - num);
				}
				if (num > 0 && num2 < viewportWidth)
				{
					centerIndex = i;
				}
				if (i == 0 && leftInView == -1)
				{
					leftInView = num;
				}
				if (i == count - 1 && rightInView == -1)
				{
					rightInView = viewportWidth - num2 - 1;
				}
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005214 File Offset: 0x00004214
		internal void GetStops(int offset, int totalWidth, out PanoramaPanel.ItemStop previous, out PanoramaPanel.ItemStop current, out PanoramaPanel.ItemStop next)
		{
			int num3;
			int num2;
			int num = num2 = (num3 = -1);
			PanoramaPanel.ItemStop itemStop;
			previous = (itemStop = null);
			PanoramaPanel.ItemStop itemStop2;
			current = (itemStop2 = itemStop);
			next = itemStop2;
			if (this.VisibleChildren.Count == 0)
			{
				return;
			}
			int num4 = -offset % totalWidth;
			int num5 = 0;
			foreach (PanoramaPanel.ItemStop itemStop3 in this._itemStops)
			{
				if (itemStop3.Position < num4)
				{
					num2 = num5;
				}
				else
				{
					if (itemStop3.Position > num4)
					{
						num3 = num5;
						break;
					}
					if (itemStop3.Position == num4)
					{
						num = num5;
					}
				}
				num5++;
			}
			if (num2 == -1)
			{
				num2 = this._itemStops.Count - 1;
			}
			if (num3 == -1)
			{
				num3 = 0;
			}
			previous = this._itemStops[num2];
			current = ((num != -1) ? this._itemStops[num] : null);
			next = this._itemStops[num3];
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005310 File Offset: 0x00004310
		internal void GetSnapOffset(int offset, int viewportWidth, int direction, out int snapTo, out int newDirection, out PanoramaItem newSelection, out bool wraparound)
		{
			int num = viewportWidth / 3;
			wraparound = false;
			snapTo = offset;
			newDirection = direction;
			newSelection = this._selectedItem;
			if (this.VisibleChildren.Count == 0)
			{
				return;
			}
			foreach (PanoramaPanel.ItemStop itemStop in this._itemStops)
			{
				if (itemStop.Position == -offset)
				{
					newSelection = itemStop.Item;
					return;
				}
			}
			int num2;
			int num3;
			int num4;
			int num5;
			int num6;
			this.GetItemsInView(offset, viewportWidth, out num2, out num3, out num4, out num5, out num6);
			if (num2 != num5 || num2 == -1)
			{
				bool flag = false;
				if (num2 == -1)
				{
					flag = true;
					num2 = this._visibleChildren.Count - 1;
				}
				bool flag2 = false;
				if (num5 == -1)
				{
					flag2 = true;
					num5 = 0;
				}
				int num7;
				if (direction < 0)
				{
					if (num6 > num)
					{
						num7 = PanoramaPanel.GetBestIndex(num4, num5, num2);
						newDirection = -1;
					}
					else
					{
						num7 = PanoramaPanel.GetBestIndex(num2, num4, num5);
						newDirection = 1;
					}
				}
				else if (direction > 0)
				{
					if (num3 > num)
					{
						num7 = PanoramaPanel.GetBestIndex(num2, num4, num5);
						newDirection = 1;
					}
					else
					{
						num7 = PanoramaPanel.GetBestIndex(num4, num5, num2);
						newDirection = -1;
					}
				}
				else if (num4 != -1)
				{
					num7 = num4;
					newDirection = -1;
				}
				else if (num3 > num6)
				{
					num7 = num2;
					newDirection = -1;
				}
				else
				{
					num7 = num5;
					newDirection = 1;
				}
				this._selectedItem = this._visibleChildren[num7];
				if (newDirection < 0)
				{
					snapTo = PanoramaPanel.GetLeftAlignedOffset(this._selectedItem, viewportWidth);
				}
				else
				{
					snapTo = PanoramaPanel.GetRightAlignedOffset(this._selectedItem, viewportWidth);
				}
				newSelection = this._selectedItem;
				if ((num7 == num2 && flag) || (num7 == num5 && flag2))
				{
					wraparound = true;
				}
				return;
			}
			newSelection = (this._selectedItem = this._visibleChildren[num2]);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000054CC File Offset: 0x000044CC
		private static int GetBestIndex(int n0, int n1, int n2)
		{
			if (n0 >= 0)
			{
				return n0;
			}
			if (n1 >= 0)
			{
				return n1;
			}
			if (n2 >= 0)
			{
				return n2;
			}
			throw new InvalidOperationException("No best index.");
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000054EA File Offset: 0x000044EA
		private static int GetLeftAlignedOffset(PanoramaItem movingTo, int viewportWidth)
		{
			return -movingTo.StartPosition;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000054F3 File Offset: 0x000044F3
		private static int GetRightAlignedOffset(PanoramaItem movingTo, int viewportWidth)
		{
			if (movingTo.Orientation != null)
			{
				return -movingTo.ItemWidth + viewportWidth - movingTo.StartPosition - 48;
			}
			return -movingTo.StartPosition;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005518 File Offset: 0x00004518
		private void FindOwner()
		{
			FrameworkElement frameworkElement = this;
			Panorama panorama;
			do
			{
				frameworkElement = (FrameworkElement)VisualTreeHelper.GetParent(frameworkElement);
				panorama = (frameworkElement as Panorama);
			}
			while (frameworkElement != null && panorama == null);
			this.Owner = panorama;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005549 File Offset: 0x00004549
		internal void NotifyDefaultItemChanged()
		{
			base.InvalidateMeasure();
			base.InvalidateArrange();
			base.UpdateLayout();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005560 File Offset: 0x00004560
		internal void ShowLastItemOnLeft()
		{
			this.ResetItemPositions();
			if (this.VisibleChildren.Count > 0)
			{
				PanoramaItem panoramaItem = this.VisibleChildren[this.VisibleChildren.Count - 1];
				panoramaItem.RenderTransform = new TranslateTransform
				{
					X = -base.ActualWidth
				};
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000055B4 File Offset: 0x000045B4
		internal void ShowFirstItemOnRight()
		{
			this.ResetItemPositions();
			if (this.VisibleChildren.Count > 0)
			{
				PanoramaItem panoramaItem = this.VisibleChildren[0];
				panoramaItem.RenderTransform = new TranslateTransform
				{
					X = base.ActualWidth
				};
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000055FC File Offset: 0x000045FC
		internal void ResetItemPositions()
		{
			foreach (PanoramaItem panoramaItem in this.VisibleChildren)
			{
				panoramaItem.RenderTransform = null;
			}
		}

		// Token: 0x04000073 RID: 115
		private const int SnapThresholdDivisor = 3;

		// Token: 0x04000074 RID: 116
		private Panorama _owner;

		// Token: 0x04000075 RID: 117
		private readonly List<PanoramaItem> _visibleChildren = new List<PanoramaItem>();

		// Token: 0x04000076 RID: 118
		private readonly List<PanoramaPanel.ItemStop> _itemStops = new List<PanoramaPanel.ItemStop>();

		// Token: 0x04000077 RID: 119
		private PanoramaItem _selectedItem;

		// Token: 0x0200001B RID: 27
		internal class ItemStop
		{
			// Token: 0x17000037 RID: 55
			// (get) Token: 0x060000FF RID: 255 RVA: 0x0000564C File Offset: 0x0000464C
			// (set) Token: 0x06000100 RID: 256 RVA: 0x00005654 File Offset: 0x00004654
			public int Index { get; private set; }

			// Token: 0x17000038 RID: 56
			// (get) Token: 0x06000101 RID: 257 RVA: 0x0000565D File Offset: 0x0000465D
			// (set) Token: 0x06000102 RID: 258 RVA: 0x00005665 File Offset: 0x00004665
			public int Position { get; private set; }

			// Token: 0x17000039 RID: 57
			// (get) Token: 0x06000103 RID: 259 RVA: 0x0000566E File Offset: 0x0000466E
			// (set) Token: 0x06000104 RID: 260 RVA: 0x00005676 File Offset: 0x00004676
			public PanoramaItem Item { get; private set; }

			// Token: 0x06000105 RID: 261 RVA: 0x0000567F File Offset: 0x0000467F
			public ItemStop(PanoramaItem item, int index, int position)
			{
				this.Item = item;
				this.Index = index;
				this.Position = position;
			}
		}
	}
}
