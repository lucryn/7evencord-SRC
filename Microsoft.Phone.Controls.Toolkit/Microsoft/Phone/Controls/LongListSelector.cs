using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Phone.Shell;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000047 RID: 71
	[TemplatePart(Name = "ItemsPanel", Type = typeof(Panel))]
	[TemplatePart(Name = "PanningTransform", Type = typeof(TranslateTransform))]
	[TemplatePart(Name = "VerticalScrollBar", Type = typeof(ScrollBar))]
	public class LongListSelector : Control
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00009A85 File Offset: 0x00007C85
		// (set) Token: 0x06000240 RID: 576 RVA: 0x00009A97 File Offset: 0x00007C97
		public IEnumerable ItemsSource
		{
			get
			{
				return (IEnumerable)base.GetValue(LongListSelector.ItemsSourceProperty);
			}
			set
			{
				base.SetValue(LongListSelector.ItemsSourceProperty, value);
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00009AA5 File Offset: 0x00007CA5
		private static void OnItemsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			((LongListSelector)obj).OnItemsSourceChanged();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00009AB2 File Offset: 0x00007CB2
		private void OnItemsSourceChanged()
		{
			this._flattenedItems = null;
			if (this._isLoaded)
			{
				this.EnsureData();
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00009AC9 File Offset: 0x00007CC9
		// (set) Token: 0x06000244 RID: 580 RVA: 0x00009AD6 File Offset: 0x00007CD6
		public object ListHeader
		{
			get
			{
				return base.GetValue(LongListSelector.ListHeaderProperty);
			}
			set
			{
				base.SetValue(LongListSelector.ListHeaderProperty, value);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00009AE4 File Offset: 0x00007CE4
		// (set) Token: 0x06000246 RID: 582 RVA: 0x00009AF6 File Offset: 0x00007CF6
		public DataTemplate ListHeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(LongListSelector.ListHeaderTemplateProperty);
			}
			set
			{
				base.SetValue(LongListSelector.ListHeaderTemplateProperty, value);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00009B04 File Offset: 0x00007D04
		// (set) Token: 0x06000248 RID: 584 RVA: 0x00009B11 File Offset: 0x00007D11
		public object ListFooter
		{
			get
			{
				return base.GetValue(LongListSelector.ListFooterProperty);
			}
			set
			{
				base.SetValue(LongListSelector.ListFooterProperty, value);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000249 RID: 585 RVA: 0x00009B1F File Offset: 0x00007D1F
		// (set) Token: 0x0600024A RID: 586 RVA: 0x00009B31 File Offset: 0x00007D31
		public DataTemplate ListFooterTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(LongListSelector.ListFooterTemplateProperty);
			}
			set
			{
				base.SetValue(LongListSelector.ListFooterTemplateProperty, value);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00009B3F File Offset: 0x00007D3F
		// (set) Token: 0x0600024C RID: 588 RVA: 0x00009B51 File Offset: 0x00007D51
		public DataTemplate GroupHeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(LongListSelector.GroupHeaderProperty);
			}
			set
			{
				base.SetValue(LongListSelector.GroupHeaderProperty, value);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00009B5F File Offset: 0x00007D5F
		// (set) Token: 0x0600024E RID: 590 RVA: 0x00009B71 File Offset: 0x00007D71
		public DataTemplate GroupFooterTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(LongListSelector.GroupFooterProperty);
			}
			set
			{
				base.SetValue(LongListSelector.GroupFooterProperty, value);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00009B7F File Offset: 0x00007D7F
		// (set) Token: 0x06000250 RID: 592 RVA: 0x00009B91 File Offset: 0x00007D91
		public DataTemplate ItemTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(LongListSelector.ItemsTemplateProperty);
			}
			set
			{
				base.SetValue(LongListSelector.ItemsTemplateProperty, value);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00009B9F File Offset: 0x00007D9F
		// (set) Token: 0x06000252 RID: 594 RVA: 0x00009BB1 File Offset: 0x00007DB1
		public DataTemplate GroupItemTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(LongListSelector.GroupItemTemplateProperty);
			}
			set
			{
				base.SetValue(LongListSelector.GroupItemTemplateProperty, value);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00009BBF File Offset: 0x00007DBF
		// (set) Token: 0x06000254 RID: 596 RVA: 0x00009BD1 File Offset: 0x00007DD1
		public ItemsPanelTemplate GroupItemsPanel
		{
			get
			{
				return (ItemsPanelTemplate)base.GetValue(LongListSelector.GroupItemsPanelProperty);
			}
			set
			{
				base.SetValue(LongListSelector.GroupItemsPanelProperty, value);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000255 RID: 597 RVA: 0x00009BDF File Offset: 0x00007DDF
		// (set) Token: 0x06000256 RID: 598 RVA: 0x00009BF1 File Offset: 0x00007DF1
		public bool IsBouncy
		{
			get
			{
				return (bool)base.GetValue(LongListSelector.IsBouncyProperty);
			}
			set
			{
				base.SetValue(LongListSelector.IsBouncyProperty, value);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00009C04 File Offset: 0x00007E04
		// (set) Token: 0x06000258 RID: 600 RVA: 0x00009C16 File Offset: 0x00007E16
		public bool IsScrolling
		{
			get
			{
				return (bool)base.GetValue(LongListSelector.IsScrollingProperty);
			}
			private set
			{
				base.SetValue(LongListSelector.IsScrollingProperty, value);
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00009C2C File Offset: 0x00007E2C
		private static void OnIsScrollingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			LongListSelector longListSelector = (LongListSelector)obj;
			if ((bool)e.NewValue)
			{
				VisualStateManager.GoToState(longListSelector, "Scrolling", true);
				SafeRaise.Raise(longListSelector.ScrollingStarted, obj);
				return;
			}
			VisualStateManager.GoToState(longListSelector, "NotScrolling", true);
			longListSelector.BounceBack(false);
			SafeRaise.Raise(longListSelector.ScrollingCompleted, obj);
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00009C89 File Offset: 0x00007E89
		// (set) Token: 0x0600025B RID: 603 RVA: 0x00009C9B File Offset: 0x00007E9B
		public bool ShowListHeader
		{
			get
			{
				return (bool)base.GetValue(LongListSelector.ShowListHeaderProperty);
			}
			set
			{
				base.SetValue(LongListSelector.ShowListHeaderProperty, value);
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00009CB0 File Offset: 0x00007EB0
		private static void OnShowListHeaderChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			LongListSelector longListSelector = (LongListSelector)obj;
			if (longListSelector.HasListHeader && longListSelector._flattenedItems != null)
			{
				if (longListSelector.ShowListHeader)
				{
					longListSelector.OnAdd(0, LongListSelector.ItemType.ListHeader, null, new object[]
					{
						longListSelector.ListHeader
					});
				}
				else
				{
					longListSelector.OnRemove(0, 1);
				}
				longListSelector.StopScrolling();
				longListSelector.ResetMinMax();
				longListSelector.Balance();
				longListSelector.BounceBack(true);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00009D1A File Offset: 0x00007F1A
		// (set) Token: 0x0600025E RID: 606 RVA: 0x00009D2C File Offset: 0x00007F2C
		public bool ShowListFooter
		{
			get
			{
				return (bool)base.GetValue(LongListSelector.ShowListFooterProperty);
			}
			set
			{
				base.SetValue(LongListSelector.ShowListFooterProperty, value);
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00009D40 File Offset: 0x00007F40
		private static void OnShowListFooterChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			LongListSelector longListSelector = (LongListSelector)obj;
			if (longListSelector.HasListFooter && longListSelector._flattenedItems != null)
			{
				if (longListSelector.ShowListFooter)
				{
					longListSelector.OnAdd(longListSelector._flattenedItems.Count, LongListSelector.ItemType.ListFooter, null, new object[]
					{
						longListSelector.ListFooter
					});
				}
				else
				{
					longListSelector.OnRemove(longListSelector._flattenedItems.Count - 1, 1);
				}
				longListSelector.StopScrolling();
				longListSelector.ResetMinMax();
				longListSelector.Balance();
				longListSelector.BounceBack(true);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00009DC0 File Offset: 0x00007FC0
		// (set) Token: 0x06000261 RID: 609 RVA: 0x00009DCD File Offset: 0x00007FCD
		public object SelectedItem
		{
			get
			{
				return base.GetValue(LongListSelector.SelectedItemProperty);
			}
			set
			{
				base.SetValue(LongListSelector.SelectedItemProperty, value);
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00009DDB File Offset: 0x00007FDB
		private static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			((LongListSelector)obj).OnSelectedItemChanged(e);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00009DE9 File Offset: 0x00007FE9
		private void OnSelectedItemChanged(DependencyPropertyChangedEventArgs e)
		{
			this._selectedItemChanged = true;
			if (!this._setSelectionInternal)
			{
				this.RaiseSelectionChangedEvent(e.NewValue);
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009E08 File Offset: 0x00008008
		private void RaiseSelectionChangedEvent(object newSelection)
		{
			SelectionChangedEventHandler selectionChanged = this.SelectionChanged;
			if (selectionChanged != null)
			{
				this._selectionList[0] = newSelection;
				selectionChanged.Invoke(this, new SelectionChangedEventArgs(this.EmptyList, this._selectionList));
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009E40 File Offset: 0x00008040
		private void SetSelectedItemInternal(object newSelectedItem)
		{
			this._setSelectionInternal = true;
			this._selectedItemChanged = false;
			this.SelectedItem = newSelectedItem;
			if (this._selectedItemChanged)
			{
				this.RaiseSelectionChangedEvent(newSelectedItem);
			}
			this._setSelectionInternal = false;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000266 RID: 614 RVA: 0x00009E6D File Offset: 0x0000806D
		// (set) Token: 0x06000267 RID: 615 RVA: 0x00009E7F File Offset: 0x0000807F
		public double BufferSize
		{
			get
			{
				return (double)base.GetValue(LongListSelector.BufferSizeProperty);
			}
			set
			{
				base.SetValue(LongListSelector.BufferSizeProperty, value);
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00009E94 File Offset: 0x00008094
		private static void OnBufferSizeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			double num = (double)e.NewValue;
			if (num < 0.0)
			{
				throw new ArgumentOutOfRangeException("BufferSize");
			}
			((LongListSelector)obj)._bufferSizeCache = num;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00009ED1 File Offset: 0x000080D1
		// (set) Token: 0x0600026A RID: 618 RVA: 0x00009EE3 File Offset: 0x000080E3
		public double MaximumFlickVelocity
		{
			get
			{
				return (double)base.GetValue(LongListSelector.MaximumFlickVelocityProperty);
			}
			set
			{
				base.SetValue(LongListSelector.MaximumFlickVelocityProperty, value);
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00009EF8 File Offset: 0x000080F8
		private static void OnMaximumFlickVelocityChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			double num = (double)e.NewValue;
			double num2 = Math.Min(MotionParameters.MaximumSpeed, Math.Max(num, 1.0));
			if (num != num2)
			{
				((LongListSelector)obj).MaximumFlickVelocity = MotionParameters.MaximumSpeed;
				throw new ArgumentOutOfRangeException("MaximumFlickVelocity");
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00009F4B File Offset: 0x0000814B
		// (set) Token: 0x0600026D RID: 621 RVA: 0x00009F5D File Offset: 0x0000815D
		public bool DisplayAllGroups
		{
			get
			{
				return (bool)base.GetValue(LongListSelector.DisplayAllGroupsProperty);
			}
			set
			{
				base.SetValue(LongListSelector.DisplayAllGroupsProperty, value);
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00009F70 File Offset: 0x00008170
		private static void OnDisplayAllGroupsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			LongListSelector longListSelector = (LongListSelector)obj;
			longListSelector._flattenedItems = null;
			if (longListSelector._isLoaded)
			{
				longListSelector.EnsureData();
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600026F RID: 623 RVA: 0x00009F9C File Offset: 0x0000819C
		// (remove) Token: 0x06000270 RID: 624 RVA: 0x00009FD4 File Offset: 0x000081D4
		public event SelectionChangedEventHandler SelectionChanged;

		// Token: 0x06000271 RID: 625 RVA: 0x0000A00C File Offset: 0x0000820C
		public LongListSelector()
		{
			base.DefaultStyleKey = typeof(LongListSelector);
			base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
			GestureListener gestureListener = GestureService.GetGestureListener(this);
			gestureListener.GestureBegin += new EventHandler<GestureEventArgs>(this.listener_GestureBegin);
			gestureListener.GestureCompleted += new EventHandler<GestureEventArgs>(this.listener_GestureCompleted);
			gestureListener.DragStarted += new EventHandler<DragStartedGestureEventArgs>(this.listener_DragStarted);
			gestureListener.DragDelta += new EventHandler<DragDeltaGestureEventArgs>(this.listener_DragDelta);
			gestureListener.DragCompleted += new EventHandler<DragCompletedGestureEventArgs>(this.listener_DragCompleted);
			gestureListener.Flick += new EventHandler<FlickGestureEventArgs>(this.listener_Flick);
			gestureListener.Tap += new EventHandler<GestureEventArgs>(this.listener_Tap);
			base.Loaded += new RoutedEventHandler(this.LongListSelector_Loaded);
			base.Unloaded += new RoutedEventHandler(this.LongListSelector_Unloaded);
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000272 RID: 626 RVA: 0x0000A174 File Offset: 0x00008374
		// (remove) Token: 0x06000273 RID: 627 RVA: 0x0000A1AC File Offset: 0x000083AC
		public event EventHandler ScrollingStarted;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000274 RID: 628 RVA: 0x0000A1E4 File Offset: 0x000083E4
		// (remove) Token: 0x06000275 RID: 629 RVA: 0x0000A21C File Offset: 0x0000841C
		public event EventHandler ScrollingCompleted;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000276 RID: 630 RVA: 0x0000A254 File Offset: 0x00008454
		// (remove) Token: 0x06000277 RID: 631 RVA: 0x0000A28C File Offset: 0x0000848C
		public event EventHandler StretchingTop;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000278 RID: 632 RVA: 0x0000A2C4 File Offset: 0x000084C4
		// (remove) Token: 0x06000279 RID: 633 RVA: 0x0000A2FC File Offset: 0x000084FC
		public event EventHandler StretchingBottom;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600027A RID: 634 RVA: 0x0000A334 File Offset: 0x00008534
		// (remove) Token: 0x0600027B RID: 635 RVA: 0x0000A36C File Offset: 0x0000856C
		public event EventHandler StretchingCompleted;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x0600027C RID: 636 RVA: 0x0000A3A4 File Offset: 0x000085A4
		// (remove) Token: 0x0600027D RID: 637 RVA: 0x0000A3DC File Offset: 0x000085DC
		public event EventHandler<LinkUnlinkEventArgs> Link;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x0600027E RID: 638 RVA: 0x0000A414 File Offset: 0x00008614
		// (remove) Token: 0x0600027F RID: 639 RVA: 0x0000A44C File Offset: 0x0000864C
		public event EventHandler<LinkUnlinkEventArgs> Unlink;

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000A481 File Offset: 0x00008681
		// (set) Token: 0x06000281 RID: 641 RVA: 0x0000A489 File Offset: 0x00008689
		public bool IsFlatList { get; set; }

		// Token: 0x06000282 RID: 642 RVA: 0x0000A494 File Offset: 0x00008694
		public void ScrollTo(object item)
		{
			this.EnsureData();
			base.UpdateLayout();
			ContentPresenter contentPresenter;
			int num = this.GetResolvedIndex(item, out contentPresenter);
			if (num != -1)
			{
				this.StopScrolling();
				this._panningTransform.Y = -Canvas.GetTop(contentPresenter);
				this.Balance();
			}
			num = this.GetFlattenedIndex(item);
			if (num != -1)
			{
				this.RecycleAllItems();
				this.ResetMinMax();
				this.StopScrolling();
				this._panningTransform.Y = 0.0;
				this._resolvedFirstIndex = num;
				this.Balance();
				this._panningTransform.Y = this.GetCoercedScrollPosition(this._panningTransform.Y, false);
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000A538 File Offset: 0x00008738
		public void AnimateTo(object item)
		{
			this.EnsureData();
			base.UpdateLayout();
			ContentPresenter contentPresenter;
			int num = this.GetResolvedIndex(item, out contentPresenter);
			if (num != -1)
			{
				double coercedScrollPosition = this.GetCoercedScrollPosition(-Canvas.GetTop(contentPresenter), false);
				double num2 = -coercedScrollPosition + this._panningTransform.Y;
				double num3 = Math.Abs(num2) / this.MaximumFlickVelocity;
				IEasingFunction ease = PhysicsConstants.GetEasingFunction(num3);
				if (this._scrollingTowards != -1)
				{
					this._scrollingTowards = -1;
					ease = new ExponentialEase
					{
						EasingMode = 0
					};
				}
				this.IsFlicking = true;
				this.AnimatePanel(new Duration(TimeSpan.FromSeconds(num3)), ease, coercedScrollPosition);
				return;
			}
			num = this.GetFlattenedIndex(item);
			if (num != -1)
			{
				this._scrollingTowards = num;
				this.ScrollTowards();
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000A5EE File Offset: 0x000087EE
		public ICollection<object> GetItemsInView()
		{
			return this.GetItemsWithContainers(true, false);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000A5F8 File Offset: 0x000087F8
		public ICollection<object> GetItemsWithContainers(bool onlyItemsInView, bool getContainers)
		{
			int num = onlyItemsInView ? this._screenFirstIndex : this._resolvedFirstIndex;
			int num2 = onlyItemsInView ? this._screenCount : this._resolvedCount;
			object[] array = new object[num2];
			for (int i = num; i < num + num2; i++)
			{
				array[i - num] = (getContainers ? this._flattenedItems[i].ContentPresenter : this._flattenedItems[i].Item);
			}
			return array;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000A66B File Offset: 0x0000886B
		// (set) Token: 0x06000287 RID: 647 RVA: 0x0000A673 File Offset: 0x00008873
		private bool IsPanning
		{
			get
			{
				return this._isPanning;
			}
			set
			{
				this._isPanning = value;
				this.IsScrolling = (this.IsPanning || this.IsFlicking);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000A693 File Offset: 0x00008893
		// (set) Token: 0x06000289 RID: 649 RVA: 0x0000A69B File Offset: 0x0000889B
		private bool IsFlicking
		{
			get
			{
				return this._isFlicking;
			}
			set
			{
				this._isFlicking = value;
				this.IsScrolling = (this.IsPanning || this.IsFlicking);
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000A6BB File Offset: 0x000088BB
		// (set) Token: 0x0600028B RID: 651 RVA: 0x0000A6C4 File Offset: 0x000088C4
		private bool IsStretching
		{
			get
			{
				return this._isStretching;
			}
			set
			{
				if (this._isStretching != value)
				{
					this._isStretching = value;
					if (this._isStretching)
					{
						if (this._dragTarget < this._minimumPanelScroll)
						{
							SafeRaise.Raise(this.StretchingBottom, this);
							return;
						}
						SafeRaise.Raise(this.StretchingTop, this);
						return;
					}
					else
					{
						SafeRaise.Raise(this.StretchingCompleted, this);
					}
				}
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000A71D File Offset: 0x0000891D
		private bool HasListHeader
		{
			get
			{
				return this.ListHeaderTemplate != null || this.ListHeader is UIElement;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000A737 File Offset: 0x00008937
		private bool HasListFooter
		{
			get
			{
				return this.ListFooterTemplate != null || this.ListFooter is UIElement;
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000A751 File Offset: 0x00008951
		private void LongListSelector_Loaded(object sender, RoutedEventArgs e)
		{
			this._isLoaded = true;
			this.EnsureData();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000A760 File Offset: 0x00008960
		private void LongListSelector_Unloaded(object sender, RoutedEventArgs e)
		{
			this._isLoaded = false;
			this.RecycleAllItems();
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000A770 File Offset: 0x00008970
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._itemsPanel = ((base.GetTemplateChild("ItemsPanel") as Panel) ?? new Canvas());
			this._panningTransform = ((base.GetTemplateChild("PanningTransform") as TranslateTransform) ?? new TranslateTransform());
			this._verticalScrollbar = ((base.GetTemplateChild("VerticalScrollBar") as ScrollBar) ?? new ScrollBar());
			this._panelAnimation = new DoubleAnimation();
			Storyboard.SetTarget(this._panelAnimation, this._panningTransform);
			Storyboard.SetTargetProperty(this._panelAnimation, new PropertyPath("Y", new object[0]));
			this._panelStoryboard = new Storyboard();
			this._panelStoryboard.Children.Add(this._panelAnimation);
			this._panelStoryboard.Completed += new EventHandler(this.PanelStoryboardCompleted);
			this.Balance();
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000A855 File Offset: 0x00008A55
		protected override Size MeasureOverride(Size availableSize)
		{
			this._availableSize.Width = availableSize.Width;
			this._availableSize.Height = double.PositiveInfinity;
			return base.MeasureOverride(availableSize);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000A884 File Offset: 0x00008A84
		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			base.Clip = new RectangleGeometry
			{
				Rect = new Rect(0.0, 0.0, e.NewSize.Width, e.NewSize.Height)
			};
			if (this._isLoaded)
			{
				this.Balance();
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000A8E8 File Offset: 0x00008AE8
		private void listener_GestureBegin(object sender, GestureEventArgs e)
		{
			if (this.IsScrolling)
			{
				this._ignoreNextTap = true;
			}
			this._gestureStart = DateTime.Now;
			if (this._isFlicking)
			{
				this._stopTimer.Tick -= new EventHandler(this._stopTimer_Tick);
				this._stopTimer.Tick += new EventHandler(this._stopTimer_Tick);
				this._stopTimer.Start();
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000A950 File Offset: 0x00008B50
		private void _stopTimer_Tick(object sender, EventArgs e)
		{
			this.StopScrolling();
			this.IsPanning = (this.IsFlicking = false);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000A973 File Offset: 0x00008B73
		private void listener_GestureCompleted(object sender, GestureEventArgs e)
		{
			this._stopTimer.Tick -= new EventHandler(this._stopTimer_Tick);
			this._ignoreNextTap = false;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000A993 File Offset: 0x00008B93
		private void listener_DragStarted(object sender, DragStartedGestureEventArgs e)
		{
			if (e.Direction != null)
			{
				return;
			}
			this._stopTimer.Tick -= new EventHandler(this._stopTimer_Tick);
			this._dragTarget = this._panningTransform.Y;
			e.Handled = true;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000A9D0 File Offset: 0x00008BD0
		private void listener_DragDelta(object sender, DragDeltaGestureEventArgs e)
		{
			if (e.Direction != null)
			{
				return;
			}
			TimeSpan timeSpan = DateTime.Now - this._gestureStart;
			e.Handled = true;
			if (timeSpan > LongListSelector._flickStopWaitTime || Math.Sign(e.VerticalChange) != Math.Sign(this._lastFlickVelocity))
			{
				this.IsPanning = true;
				this.IsFlicking = false;
				if (this._firstDragDuringFlick)
				{
					this.StopScrolling();
					this._firstDragDuringFlick = false;
					return;
				}
				this.AnimatePanel(LongListSelector._panDuration, this._panEase, this._dragTarget += e.VerticalChange);
				this.IsStretching = (this.IsBouncy && this.GetCoercedScrollPosition(this._dragTarget, true) != this._dragTarget);
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000AA97 File Offset: 0x00008C97
		private void listener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
		{
			if (e.Direction != null)
			{
				return;
			}
			this.IsPanning = false;
			this.IsStretching = false;
			e.Handled = true;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000AAB8 File Offset: 0x00008CB8
		private void listener_Flick(object sender, FlickGestureEventArgs e)
		{
			if (e.Direction != null)
			{
				return;
			}
			this._stopTimer.Tick -= new EventHandler(this._stopTimer_Tick);
			double maximumFlickVelocity = this.MaximumFlickVelocity;
			this._lastFlickVelocity = Math.Min(maximumFlickVelocity, Math.Max(e.VerticalVelocity, -maximumFlickVelocity));
			Point initialVelocity;
			initialVelocity..ctor(0.0, this._lastFlickVelocity);
			double stopTime = PhysicsConstants.GetStopTime(initialVelocity);
			Point stopPoint = PhysicsConstants.GetStopPoint(initialVelocity);
			IEasingFunction easingFunction = PhysicsConstants.GetEasingFunction(stopTime);
			this.AnimatePanel(new Duration(TimeSpan.FromSeconds(stopTime)), easingFunction, this._panningTransform.Y + stopPoint.Y);
			this.IsFlicking = true;
			this._firstDragDuringFlick = true;
			this._scrollingTowards = -1;
			e.Handled = true;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000AB74 File Offset: 0x00008D74
		private void listener_Tap(object sender, GestureEventArgs e)
		{
			this.StopScrolling();
			this.IsPanning = (this.IsFlicking = false);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000AB98 File Offset: 0x00008D98
		private void StopScrolling()
		{
			double y = Math.Round(this._panningTransform.Y);
			this.StopAnimation();
			this._panningTransform.Y = y;
			this._stopTimer.Tick -= new EventHandler(this._stopTimer_Tick);
			this._scrollingTowards = -1;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000ABE6 File Offset: 0x00008DE6
		private void GroupHeaderTap(object sender, GestureEventArgs e)
		{
			this._stopTimer.Tick -= new EventHandler(this._stopTimer_Tick);
			if (!this._ignoreNextTap)
			{
				this.DisplayGroupView();
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000AC10 File Offset: 0x00008E10
		private void OnItemTap(object sender, GestureEventArgs e)
		{
			if (!this._ignoreNextTap)
			{
				ContentPresenter contentPresenter = (ContentPresenter)sender;
				this.SetSelectedItemInternal(contentPresenter.Content);
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000AC38 File Offset: 0x00008E38
		private void HandleGesture(object sender, GestureEventArgs e)
		{
			e.Handled = true;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000AC41 File Offset: 0x00008E41
		private bool IsReady()
		{
			return this._itemsPanel != null && this.ItemsSource != null && base.ActualHeight > 0.0;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000AC68 File Offset: 0x00008E68
		private void Balance()
		{
			if (!this.IsReady() || this._flattenedItems.Count == 0)
			{
				this.CollapseRecycledElements();
				this._resolvedFirstIndex = (this._resolvedCount = (this._screenFirstIndex = (this._screenCount = 0)));
				return;
			}
			double actualHeight = base.ActualHeight;
			double num = -(this._bufferSizeCache * actualHeight);
			double num2 = (this._bufferSizeCache + 1.0) * actualHeight;
			double y = this._panningTransform.Y;
			ContentPresenter contentPresenter;
			double num3;
			double num4;
			if (this._resolvedCount > 0)
			{
				contentPresenter = this.FirstResolved.ContentPresenter;
				num3 = Canvas.GetTop(contentPresenter);
				num4 = num3 + contentPresenter.DesiredSize.Height;
				while (this._resolvedCount > 0 && num4 + y < num)
				{
					this.RecycleFirst();
					if (this._resolvedCount > 0)
					{
						contentPresenter = this.FirstResolved.ContentPresenter;
						num3 = Canvas.GetTop(contentPresenter);
						num4 = num3 + contentPresenter.DesiredSize.Height;
					}
				}
				if (this._resolvedCount > 0)
				{
					contentPresenter = this.LastResolved.ContentPresenter;
					num3 = Canvas.GetTop(contentPresenter);
					num4 = num3 + contentPresenter.DesiredSize.Height;
					while (this._resolvedCount > 0 && num3 + y > num2)
					{
						this.RecycleLast();
						if (this._resolvedCount > 0)
						{
							contentPresenter = this.LastResolved.ContentPresenter;
							num3 = Canvas.GetTop(contentPresenter);
							num4 = num3 + contentPresenter.DesiredSize.Height;
						}
					}
				}
				if (this._resolvedCount == 0)
				{
					this.ResetMinMax();
				}
			}
			bool flag = this._resolvedCount == 0 && this._resolvedFirstIndex == 0;
			if (this._resolvedCount == 0)
			{
				this._resolvedFirstIndex = Math.Max(0, Math.Min(this._resolvedFirstIndex, this._flattenedItems.Count - 1));
				LongListSelector.ItemTuple tuple = this._flattenedItems[this._resolvedFirstIndex];
				contentPresenter = this.GetAndAddElementFor(tuple);
				contentPresenter.SetExtraData(this._resolvedFirstIndex, contentPresenter.DesiredSize.Height);
				num3 = 0.0;
				Canvas.SetTop(contentPresenter, num3);
				num4 = contentPresenter.DesiredSize.Height;
				if (this._resolvedFirstIndex == 0)
				{
					this._maximumPanelScroll = 0.0;
				}
			}
			contentPresenter = this.FirstResolved.ContentPresenter;
			num3 = Canvas.GetTop(contentPresenter);
			num4 = num3 + contentPresenter.DesiredSize.Height;
			if (num3 + y >= num && this._resolvedFirstIndex == 0)
			{
				this._maximumPanelScroll = -num3;
				this.BrakeIfGoingTooFar();
			}
			else
			{
				while (num3 + y >= num && this._resolvedFirstIndex > 0)
				{
					flag = false;
					this._resolvedFirstIndex = Math.Max(0, Math.Min(this._resolvedFirstIndex, this._flattenedItems.Count - 1));
					LongListSelector.ItemTuple tuple2 = this._flattenedItems[--this._resolvedFirstIndex];
					contentPresenter = this.GetAndAddElementFor(tuple2);
					contentPresenter.SetExtraData(this._resolvedFirstIndex, contentPresenter.DesiredSize.Height);
					num4 = num3;
					num3 = num4 - contentPresenter.DesiredSize.Height;
					Canvas.SetTop(contentPresenter, num3);
					if (this._resolvedFirstIndex == 0)
					{
						this._maximumPanelScroll = -num3;
						this.BrakeIfGoingTooFar();
					}
				}
			}
			contentPresenter = this.LastResolved.ContentPresenter;
			num3 = Canvas.GetTop(contentPresenter);
			num4 = num3 + contentPresenter.DesiredSize.Height;
			if (flag)
			{
				num2 += base.ActualHeight * this._bufferSizeCache;
			}
			while (num4 + y <= num2 && this._resolvedFirstIndex + this._resolvedCount < this._flattenedItems.Count)
			{
				this._resolvedFirstIndex = Math.Max(0, Math.Min(this._resolvedFirstIndex, this._flattenedItems.Count - 1));
				LongListSelector.ItemTuple tuple3 = this._flattenedItems[this._resolvedFirstIndex + this._resolvedCount];
				contentPresenter = this.GetAndAddElementFor(tuple3);
				contentPresenter.SetExtraData(this._resolvedFirstIndex + this._resolvedCount - 1, contentPresenter.DesiredSize.Height);
				num3 = num4;
				Canvas.SetTop(contentPresenter, num3);
				num4 = num3 + contentPresenter.DesiredSize.Height;
			}
			if (this._resolvedFirstIndex + this._resolvedCount == this._flattenedItems.Count)
			{
				this._minimumPanelScroll = base.ActualHeight - num4;
				if (this._minimumPanelScroll > this._maximumPanelScroll)
				{
					this._minimumPanelScroll = this._maximumPanelScroll;
				}
				this.BrakeIfGoingTooFar();
			}
			this._screenFirstIndex = 0;
			this._screenCount = 0;
			for (int i = this._resolvedFirstIndex; i < this._resolvedFirstIndex + this._resolvedCount; i++)
			{
				ContentPresenter contentPresenter2 = this._flattenedItems[i].ContentPresenter;
				num3 = Canvas.GetTop(contentPresenter2) + y;
				num4 = num3 + contentPresenter2.DesiredSize.Height;
				if ((num3 >= 0.0 && num3 <= base.ActualHeight) || (num3 < 0.0 && num4 > 0.0))
				{
					if (this._screenCount == 0)
					{
						this._screenFirstIndex = i;
					}
					this._screenCount++;
				}
				else if (this._screenCount != 0)
				{
					break;
				}
			}
			double num5 = (double)Math.Max(1, this._flattenedItems.Count - this._screenCount);
			this._verticalScrollbar.Maximum = num5;
			this._verticalScrollbar.Value = Math.Min(num5, (double)this._screenFirstIndex);
			if (Math.Abs((double)this._screenCount - this._verticalScrollbar.Value) > 1.0)
			{
				this._verticalScrollbar.ViewportSize = (double)Math.Max(1, this._screenCount);
			}
			this.CollapseRecycledElements();
			if (this._scrollingTowards >= this._resolvedFirstIndex && this._scrollingTowards < this._resolvedFirstIndex + this._resolvedCount)
			{
				this.AnimateTo(this._flattenedItems[this._scrollingTowards].Item);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000B2A1 File Offset: 0x000094A1
		private LongListSelector.ItemTuple FirstResolved
		{
			get
			{
				return this._flattenedItems[this._resolvedFirstIndex];
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000B2B4 File Offset: 0x000094B4
		private LongListSelector.ItemTuple LastResolved
		{
			get
			{
				int num = this._resolvedFirstIndex + this._resolvedCount - 1;
				return this._flattenedItems[num];
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000B2E0 File Offset: 0x000094E0
		private void RecycleFirst()
		{
			if (this._resolvedCount > 0)
			{
				LongListSelector.ItemTuple tuple = this._flattenedItems[this._resolvedFirstIndex++];
				this.RemoveAndAddToRecycleBin(tuple);
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000B31C File Offset: 0x0000951C
		private void RecycleLast()
		{
			if (this._resolvedCount > 0)
			{
				LongListSelector.ItemTuple tuple = this._flattenedItems[this._resolvedFirstIndex + this._resolvedCount - 1];
				this.RemoveAndAddToRecycleBin(tuple);
			}
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000B354 File Offset: 0x00009554
		private void RemoveAndAddToRecycleBin(LongListSelector.ItemTuple tuple)
		{
			ContentPresenter contentPresenter = tuple.ContentPresenter;
			switch (tuple.ItemType)
			{
			case LongListSelector.ItemType.Item:
				this._recycledItems.Push(contentPresenter);
				break;
			case LongListSelector.ItemType.GroupHeader:
				this._recycledGroupHeaders.Push(contentPresenter);
				break;
			case LongListSelector.ItemType.GroupFooter:
				this._recycledGroupFooters.Push(contentPresenter);
				break;
			case LongListSelector.ItemType.ListHeader:
				this._recycledListHeader = contentPresenter;
				break;
			case LongListSelector.ItemType.ListFooter:
				this._recycledListFooter = contentPresenter;
				break;
			}
			EventHandler<LinkUnlinkEventArgs> unlink = this.Unlink;
			if (unlink != null)
			{
				unlink.Invoke(this, new LinkUnlinkEventArgs(contentPresenter));
			}
			tuple.ContentPresenter = null;
			contentPresenter.Content = null;
			contentPresenter.SetExtraData(-1, 0.0);
			this._resolvedCount--;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000B40C File Offset: 0x0000960C
		private void CollapseRecycledElements()
		{
			foreach (ContentPresenter contentPresenter in this._recycledItems)
			{
				contentPresenter.Visibility = 1;
			}
			foreach (ContentPresenter contentPresenter2 in this._recycledGroupHeaders)
			{
				contentPresenter2.Visibility = 1;
			}
			foreach (ContentPresenter contentPresenter3 in this._recycledGroupFooters)
			{
				contentPresenter3.Visibility = 1;
			}
			if (this._recycledListHeader != null)
			{
				this._recycledListHeader.Visibility = 1;
			}
			if (this._recycledListFooter != null)
			{
				this._recycledListFooter.Visibility = 1;
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000B510 File Offset: 0x00009710
		private void EmptyRecycleBin()
		{
			if (this._recycledItems != null)
			{
				this._recycledItems.Clear();
			}
			if (this._recycledGroupHeaders != null)
			{
				this._recycledGroupHeaders.Clear();
			}
			if (this._recycledGroupFooters != null)
			{
				this._recycledGroupFooters.Clear();
			}
			this._recycledListHeader = null;
			this._recycledListFooter = null;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000B564 File Offset: 0x00009764
		private void RecycleAllItems()
		{
			while (this._resolvedCount > 0)
			{
				this.RecycleFirst();
			}
			this._resolvedFirstIndex = 0;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000B580 File Offset: 0x00009780
		private ContentPresenter GetAndAddElementFor(LongListSelector.ItemTuple tuple)
		{
			ContentPresenter contentPresenter = null;
			bool flag = false;
			switch (tuple.ItemType)
			{
			case LongListSelector.ItemType.Item:
				if (this._recycledItems.Count > 0)
				{
					contentPresenter = this._recycledItems.Pop();
				}
				else
				{
					flag = true;
					contentPresenter = new ContentPresenter();
					contentPresenter.ContentTemplate = this.ItemTemplate;
					GestureService.GetGestureListener(contentPresenter).Tap += new EventHandler<GestureEventArgs>(this.OnItemTap);
				}
				break;
			case LongListSelector.ItemType.GroupHeader:
				if (this._recycledGroupHeaders.Count > 0)
				{
					contentPresenter = this._recycledGroupHeaders.Pop();
				}
				else
				{
					flag = true;
					contentPresenter = new ContentPresenter();
					contentPresenter.ContentTemplate = this.GroupHeaderTemplate;
					GestureService.GetGestureListener(contentPresenter).Tap += new EventHandler<GestureEventArgs>(this.GroupHeaderTap);
				}
				break;
			case LongListSelector.ItemType.GroupFooter:
				if (this._recycledGroupFooters.Count > 0)
				{
					contentPresenter = this._recycledGroupFooters.Pop();
				}
				else
				{
					flag = true;
					contentPresenter = new ContentPresenter();
					contentPresenter.ContentTemplate = this.GroupFooterTemplate;
				}
				break;
			case LongListSelector.ItemType.ListHeader:
				if (this._recycledListHeader != null)
				{
					contentPresenter = this._recycledListHeader;
					this._recycledListHeader = null;
				}
				else
				{
					flag = true;
					contentPresenter = new ContentPresenter();
					contentPresenter.ContentTemplate = this.ListHeaderTemplate;
				}
				break;
			case LongListSelector.ItemType.ListFooter:
				if (this._recycledListFooter != null)
				{
					contentPresenter = this._recycledListFooter;
					this._recycledListFooter = null;
				}
				else
				{
					flag = true;
					contentPresenter = new ContentPresenter();
					contentPresenter.ContentTemplate = this.ListFooterTemplate;
				}
				break;
			}
			if (flag)
			{
				contentPresenter.CacheMode = new BitmapCache();
				this._itemsPanel.Children.Add(contentPresenter);
				contentPresenter.SizeChanged += new SizeChangedEventHandler(this.OnItemSizeChanged);
			}
			if (contentPresenter != null)
			{
				if (contentPresenter.Width != this._availableSize.Width)
				{
					contentPresenter.Width = this._availableSize.Width;
				}
				contentPresenter.Content = tuple.Item;
				contentPresenter.Visibility = 0;
			}
			EventHandler<LinkUnlinkEventArgs> link = this.Link;
			if (link != null)
			{
				link.Invoke(this, new LinkUnlinkEventArgs(contentPresenter));
			}
			tuple.ContentPresenter = contentPresenter;
			contentPresenter.Measure(this._availableSize);
			this._resolvedCount++;
			return contentPresenter;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000B788 File Offset: 0x00009988
		private void OnItemSizeChanged(object sender, SizeChangedEventArgs e)
		{
			ContentPresenter contentPresenter = sender as ContentPresenter;
			double num = e.NewSize.Height - e.PreviousSize.Height;
			this._minimumPanelScroll -= num;
			if (contentPresenter != null)
			{
				int i;
				double num2;
				contentPresenter.GetExtraData(out i, out num2);
				if (num2 == e.NewSize.Height)
				{
					return;
				}
				contentPresenter.SetExtraData(i, e.NewSize.Height);
				if (i < this._screenFirstIndex)
				{
					while (i >= this._resolvedFirstIndex)
					{
						double num3 = Canvas.GetTop(this._flattenedItems[i].ContentPresenter);
						num3 -= num;
						Canvas.SetTop(this._flattenedItems[i].ContentPresenter, num3);
						i--;
					}
				}
				else
				{
					int num4 = this._resolvedFirstIndex + this._resolvedCount - 1;
					for (i++; i <= num4; i++)
					{
						double num5 = Canvas.GetTop(this._flattenedItems[i].ContentPresenter);
						num5 += num;
						Canvas.SetTop(this._flattenedItems[i].ContentPresenter, num5);
					}
				}
				if (!this._balanceNeededForSizeChanged)
				{
					this._balanceNeededForSizeChanged = true;
					base.LayoutUpdated += new EventHandler(this.LongListSelector_LayoutUpdated);
				}
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000B8CA File Offset: 0x00009ACA
		private void LongListSelector_LayoutUpdated(object sender, EventArgs e)
		{
			this._balanceNeededForSizeChanged = false;
			base.LayoutUpdated -= new EventHandler(this.LongListSelector_LayoutUpdated);
			this.Balance();
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000B8EB File Offset: 0x00009AEB
		private void EnsureData()
		{
			if (this._flattenedItems == null || this._flattenedItems.Count == 0)
			{
				this.FlattenData();
				this.Balance();
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000B910 File Offset: 0x00009B10
		private void FlattenData()
		{
			bool flag = this.GroupHeaderTemplate != null;
			bool flag2 = this.GroupFooterTemplate != null;
			bool displayAllGroups = this.DisplayAllGroups;
			this._flattenedItems = new List<LongListSelector.ItemTuple>();
			this._firstGroup = null;
			this.SetSelectedItemInternal(null);
			this._resolvedFirstIndex = 0;
			this._resolvedCount = 0;
			this._firstGroup = null;
			if (this._panningTransform != null)
			{
				this.StopAnimation();
				this._panningTransform.Y = 0.0;
				this.ResetMinMax();
			}
			this.ResetMinMax();
			if (this._itemsPanel != null)
			{
				this._itemsPanel.Children.Clear();
			}
			this.EmptyRecycleBin();
			if (this._rootCollection != null)
			{
				this._rootCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnRootCollectionChanged);
			}
			this._rootCollection = null;
			if (this._groupCollections != null)
			{
				foreach (INotifyCollectionChanged notifyCollectionChanged in this._groupCollections)
				{
					if (notifyCollectionChanged != null)
					{
						notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnGroupCollectionChanged);
					}
				}
			}
			this._groupCollections = new List<INotifyCollectionChanged>();
			if (this.ItemsSource == null)
			{
				return;
			}
			if (this.HasListHeader && this.ShowListHeader)
			{
				this._flattenedItems.Add(new LongListSelector.ItemTuple
				{
					ItemType = LongListSelector.ItemType.ListHeader,
					Item = this.ListHeader
				});
			}
			foreach (object obj in this.ItemsSource)
			{
				object obj2 = null;
				if (this.IsFlatList)
				{
					this._flattenedItems.Add(new LongListSelector.ItemTuple
					{
						ItemType = LongListSelector.ItemType.Item,
						Group = obj,
						Item = obj
					});
				}
				else
				{
					IEnumerable enumerable = (IEnumerable)obj;
					IEnumerator enumerator3 = enumerable.GetEnumerator();
					bool flag3 = enumerator3.MoveNext();
					if (flag3 || displayAllGroups)
					{
						if (flag)
						{
							this._flattenedItems.Add(new LongListSelector.ItemTuple
							{
								ItemType = LongListSelector.ItemType.GroupHeader,
								Group = obj,
								Item = obj
							});
						}
						if (flag2)
						{
							obj2 = obj;
						}
						if (this._firstGroup == null)
						{
							this._firstGroup = obj;
						}
					}
					if (flag3)
					{
						while (flag3)
						{
							this._flattenedItems.Add(new LongListSelector.ItemTuple
							{
								ItemType = LongListSelector.ItemType.Item,
								Group = obj,
								Item = enumerator3.Current
							});
							flag3 = enumerator3.MoveNext();
						}
					}
					if (obj2 != null)
					{
						this._flattenedItems.Add(new LongListSelector.ItemTuple
						{
							ItemType = LongListSelector.ItemType.GroupFooter,
							Group = obj,
							Item = obj
						});
					}
					this.AddGroupNotifyCollectionChanged(obj);
				}
			}
			this._rootCollection = (this.ItemsSource as INotifyCollectionChanged);
			if (this._rootCollection != null)
			{
				this._rootCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnRootCollectionChanged);
			}
			if (this.HasListFooter && this.ShowListFooter)
			{
				this._flattenedItems.Add(new LongListSelector.ItemTuple
				{
					ItemType = LongListSelector.ItemType.ListFooter,
					Item = this.ListFooter
				});
			}
			if (this._verticalScrollbar != null)
			{
				this._verticalScrollbar.Maximum = (double)this._flattenedItems.Count;
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000BCA0 File Offset: 0x00009EA0
		private void AddGroupNotifyCollectionChanged(object group)
		{
			INotifyCollectionChanged notifyCollectionChanged = group as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				this._groupCollections.Add(notifyCollectionChanged);
				notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnGroupCollectionChanged);
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000BCD8 File Offset: 0x00009ED8
		private void RemoveGroupNotifyCollectionChanged(object group)
		{
			INotifyCollectionChanged notifyCollectionChanged = group as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				this._groupCollections.Remove(notifyCollectionChanged);
				notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnGroupCollectionChanged);
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000BD10 File Offset: 0x00009F10
		private void OnRootCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			int num = (this.HasListHeader && this.ShowListHeader) ? 1 : 0;
			if (!this.IsFlatList)
			{
				bool displayAllGroups = this.DisplayAllGroups;
				int num2 = (this.GroupHeaderTemplate != null) ? 1 : 0;
				int num3 = (this.GroupFooterTemplate != null) ? 1 : 0;
				switch (e.Action)
				{
				case 0:
				{
					IList list = (IList)this.ItemsSource;
					object obj = (e.NewStartingIndex < list.Count) ? list[e.NewStartingIndex] : null;
					int num4 = (obj != null) ? this.GetGroupOffset(obj) : list.Count;
					using (IEnumerator enumerator = e.NewItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							this.AddGroupNotifyCollectionChanged(obj2);
							IList itemsInGroup = LongListSelector.GetItemsInGroup(obj2);
							if (itemsInGroup.Count > 0 || displayAllGroups)
							{
								if (num2 == 1)
								{
									this.OnAdd(num4, LongListSelector.ItemType.GroupHeader, obj2, new object[]
									{
										obj2
									});
									num4++;
								}
								if (itemsInGroup.Count > 0)
								{
									this.OnAdd(num4, LongListSelector.ItemType.Item, obj2, itemsInGroup);
									num4 += itemsInGroup.Count;
								}
								if (num3 == 1)
								{
									this.OnAdd(num4, LongListSelector.ItemType.GroupFooter, obj2, new object[]
									{
										obj2
									});
									num4++;
								}
							}
						}
						goto IL_2AE;
					}
					break;
				}
				case 1:
					break;
				case 2:
					goto IL_29B;
				case 3:
					goto IL_2AE;
				case 4:
					this._flattenedItems = null;
					this.EnsureData();
					goto IL_2AE;
				default:
					goto IL_2AE;
				}
				using (IEnumerator enumerator2 = e.OldItems.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						object group = enumerator2.Current;
						this.RemoveGroupNotifyCollectionChanged(group);
						IList itemsInGroup2 = LongListSelector.GetItemsInGroup(group);
						int groupOffset = this.GetGroupOffset(group);
						int num5 = itemsInGroup2.Count;
						if (displayAllGroups || itemsInGroup2.Count > 0)
						{
							num5 += num2 + num3;
						}
						this.OnRemove(groupOffset, num5);
					}
					goto IL_2AE;
				}
				IL_29B:
				throw new NotSupportedException();
			}
			switch (e.Action)
			{
			case 0:
				this.OnAdd(num + e.NewStartingIndex, LongListSelector.ItemType.Item, null, e.NewItems);
				break;
			case 1:
				this.OnRemove(num + e.OldStartingIndex, e.OldItems.Count);
				break;
			case 2:
				if (e.NewItems.Count != 1)
				{
					throw new NotSupportedException();
				}
				this.OnReplace(num + e.NewStartingIndex, e.NewItems[0]);
				break;
			case 4:
				this._flattenedItems = null;
				this.EnsureData();
				break;
			}
			IL_2AE:
			this.ResetMinMax();
			this.Balance();
			if (this.BounceBack(true))
			{
				this.Balance();
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000C004 File Offset: 0x0000A204
		private static IList GetItemsInGroup(object group)
		{
			IList list = group as IList;
			if (list != null)
			{
				return list;
			}
			List<object> list2 = new List<object>();
			IEnumerator enumerator = ((IEnumerable)group).GetEnumerator();
			bool flag = enumerator.MoveNext();
			while (flag)
			{
				list2.Add(enumerator.Current);
				flag = enumerator.MoveNext();
			}
			return list2;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000C050 File Offset: 0x0000A250
		private static bool IsGroupEmpty(object group)
		{
			IList list = group as IList;
			if (list != null)
			{
				return list.Count == 0;
			}
			IEnumerator enumerator = ((IEnumerable)group).GetEnumerator();
			return !enumerator.MoveNext();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000C088 File Offset: 0x0000A288
		private void OnGroupCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			int num = (this.GroupHeaderTemplate != null) ? 1 : 0;
			int num2 = (this.GroupFooterTemplate != null) ? 1 : 0;
			bool displayAllGroups = this.DisplayAllGroups;
			int num4;
			int num3 = this.GetGroupOffset(sender, out num4);
			IList list = sender as IList;
			if (!displayAllGroups && list != null && e.Action == null && list.Count == e.NewItems.Count)
			{
				if (num == 1)
				{
					this.OnAdd(num3, LongListSelector.ItemType.GroupHeader, sender, new object[]
					{
						sender
					});
				}
				if (num2 == 1)
				{
					this.OnAdd(num3 + num, LongListSelector.ItemType.GroupFooter, sender, new object[]
					{
						sender
					});
				}
			}
			num3 += num;
			switch (e.Action)
			{
			case 0:
				this.OnAdd(e.NewStartingIndex + num3, LongListSelector.ItemType.Item, sender, e.NewItems);
				break;
			case 1:
			{
				int num5 = e.OldItems.Count;
				if (LongListSelector.IsGroupEmpty(sender) && !displayAllGroups)
				{
					num3 -= num;
					num5 += num + num2;
				}
				this.OnRemove(e.OldStartingIndex + num3, num5);
				break;
			}
			case 2:
				if (e.NewItems.Count != 1)
				{
					throw new NotSupportedException();
				}
				this.OnReplace(num3 + e.NewStartingIndex, e.NewItems[0]);
				break;
			case 4:
				if (this.DisplayAllGroups)
				{
					this.OnRemove(num3, num4);
				}
				else if (num4 > 0)
				{
					this.OnRemove(num3 - num, num4 + num + num2);
				}
				break;
			}
			this.ResetMinMax();
			this.Balance();
			if (this.BounceBack(true))
			{
				this.Balance();
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000C224 File Offset: 0x0000A424
		private int GetGroupOffset(object group)
		{
			int num = (this.HasListHeader && this.ShowListHeader) ? 1 : 0;
			bool displayAllGroups = this.DisplayAllGroups;
			int num2 = (this.GroupHeaderTemplate != null) ? 1 : 0;
			int num3 = (this.GroupFooterTemplate != null) ? 1 : 0;
			int num4 = num;
			foreach (object obj in this.ItemsSource)
			{
				if (obj.Equals(group))
				{
					break;
				}
				int num5 = 0;
				IList list = obj as IList;
				if (list != null)
				{
					num5 = list.Count;
				}
				else
				{
					IEnumerable enumerable = obj as IEnumerable;
					if (enumerable != null)
					{
						IEnumerator enumerator2 = enumerable.GetEnumerator();
						while (enumerator2.MoveNext())
						{
							num5++;
						}
					}
				}
				if (displayAllGroups || num5 > 0)
				{
					num4 += num2 + num3;
				}
				num4 += num5;
			}
			return num4;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000C318 File Offset: 0x0000A518
		private int GetGroupOffset(object group, out int itemsInGroupCount)
		{
			int groupOffset = this.GetGroupOffset(group);
			int num = groupOffset;
			itemsInGroupCount = 0;
			while (num < this._flattenedItems.Count && group.Equals(this._flattenedItems[num].Group))
			{
				if (this._flattenedItems[num].ItemType == LongListSelector.ItemType.Item)
				{
					itemsInGroupCount++;
				}
				num++;
			}
			return groupOffset;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000C37C File Offset: 0x0000A57C
		private void OnAdd(int startingIndex, LongListSelector.ItemType itemType, object group, IList newItems)
		{
			int num = this._resolvedFirstIndex + this._resolvedCount - 1;
			LongListSelector.ItemTuple[] array = new LongListSelector.ItemTuple[newItems.Count];
			for (int i = 0; i < newItems.Count; i++)
			{
				array[i] = new LongListSelector.ItemTuple
				{
					ItemType = itemType,
					Group = group,
					Item = newItems[i]
				};
			}
			if (startingIndex <= this._resolvedFirstIndex || startingIndex > num)
			{
				if (startingIndex <= this._resolvedFirstIndex)
				{
					if (this._resolvedCount > 0)
					{
						this._resolvedFirstIndex += newItems.Count;
					}
					else
					{
						this._resolvedFirstIndex = 0;
					}
				}
			}
			else if (startingIndex < this._screenFirstIndex)
			{
				int num2 = startingIndex - this._resolvedFirstIndex + 1;
				while (num2-- > 0)
				{
					this.RecycleFirst();
				}
				this._resolvedFirstIndex += newItems.Count;
			}
			else
			{
				int num3 = this._resolvedCount - startingIndex + this._resolvedFirstIndex;
				while (num3-- > 0)
				{
					this.RecycleLast();
				}
			}
			this._flattenedItems.InsertRange(startingIndex, array);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000C48C File Offset: 0x0000A68C
		private void OnRemove(int startingIndex, int count)
		{
			int num = startingIndex + count - 1;
			int num2 = this._resolvedFirstIndex + this._resolvedCount - 1;
			if (num < this._resolvedFirstIndex || startingIndex > num2)
			{
				if (startingIndex < this._resolvedFirstIndex && startingIndex + count - 1 < this._resolvedFirstIndex)
				{
					this._resolvedFirstIndex -= count;
				}
			}
			else if (startingIndex >= this._screenFirstIndex)
			{
				int num3 = num2 - startingIndex + 1;
				while (num3-- > 0)
				{
					this.RecycleLast();
				}
			}
			else
			{
				int num4 = Math.Min(this._resolvedCount, startingIndex - this._resolvedFirstIndex + 1);
				while (num4-- > 0)
				{
					this.RecycleFirst();
				}
				this._resolvedFirstIndex -= count;
			}
			this._flattenedItems.RemoveRange(startingIndex, count);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000C544 File Offset: 0x0000A744
		private void OnReplace(int index, object item)
		{
			int num = this._resolvedFirstIndex + this._resolvedCount - 1;
			if (index >= this._resolvedFirstIndex && index <= num)
			{
				if (index < this._resolvedFirstIndex)
				{
					int num2 = this._resolvedFirstIndex - index + 1;
					while (num2-- > 0)
					{
						this.RecycleFirst();
					}
				}
				else
				{
					int num3 = num - index + 1;
					while (num3-- > 0)
					{
						this.RecycleLast();
					}
				}
			}
			this._flattenedItems[index].Item = item;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000C5BC File Offset: 0x0000A7BC
		private void ResetMinMax()
		{
			this._minimumPanelScroll = -3.4028234663852886E+38;
			this._maximumPanelScroll = 3.4028234663852886E+38;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000C5DC File Offset: 0x0000A7DC
		private void AnimatePanel(Duration duration, IEasingFunction ease, double to)
		{
			double coercedScrollPosition = this.GetCoercedScrollPosition(to, this.IsBouncy);
			if (to != coercedScrollPosition)
			{
				double num = Math.Max(Math.Abs(this._panningTransform.Y - to), 1.0);
				double num2 = Math.Abs(this._panningTransform.Y - coercedScrollPosition);
				double num3 = num2 / num;
				duration = ((num3 <= 1.0 && num3 >= 0.0 && duration.HasTimeSpan) ? new Duration(TimeSpan.FromMilliseconds((double)duration.TimeSpan.Milliseconds * num3)) : LongListSelector._panDuration);
				to = coercedScrollPosition;
			}
			double y = this._panningTransform.Y;
			this.StopAnimation();
			CompositionTarget.Rendering += new EventHandler(this.AnimationPerFrameCallback);
			this._panelAnimation.Duration = duration;
			this._panelAnimation.EasingFunction = ease;
			this._panelAnimation.From = new double?(y);
			this._panelAnimation.To = new double?(to);
			this._panelStoryboard.Begin();
			this._panelStoryboard.SeekAlignedToLastTick(TimeSpan.Zero);
			this._isAnimating = true;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000C704 File Offset: 0x0000A904
		private void BrakeIfGoingTooFar()
		{
			if (this._isAnimating && this._panelAnimation.To != null)
			{
				double value = this._panelAnimation.To.Value;
				double coercedScrollPosition = this.GetCoercedScrollPosition(value, this.IsBouncy);
				if (coercedScrollPosition != this._panelAnimation.To.Value)
				{
					double num = Math.Max(this._panelAnimation.To.Value - this._panelAnimation.From.Value, 1.0);
					double num2 = coercedScrollPosition - this._panningTransform.Y;
					double num3 = num2 / num;
					Duration duration = (num3 <= 1.0 && num3 >= 0.0 && this._panelAnimation.Duration.HasTimeSpan) ? new Duration(TimeSpan.FromMilliseconds((double)this._panelAnimation.Duration.TimeSpan.Milliseconds * num3)) : LongListSelector._panDuration;
					this.AnimatePanel(duration, this._panelAnimation.EasingFunction, coercedScrollPosition);
				}
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000C835 File Offset: 0x0000AA35
		private void AnimationPerFrameCallback(object sender, EventArgs e)
		{
			this.Balance();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000C83D File Offset: 0x0000AA3D
		private void PanelStoryboardCompleted(object sender, EventArgs e)
		{
			CompositionTarget.Rendering -= new EventHandler(this.AnimationPerFrameCallback);
			if (this._scrollingTowards != -1)
			{
				this.ScrollTowards();
				return;
			}
			this._isAnimating = false;
			this.IsFlicking = false;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000C86E File Offset: 0x0000AA6E
		private void StopAnimation()
		{
			this._panelStoryboard.Stop();
			CompositionTarget.Rendering -= new EventHandler(this.AnimationPerFrameCallback);
			this._isAnimating = false;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000C894 File Offset: 0x0000AA94
		private bool BounceBack(bool immediately)
		{
			if (this._panningTransform == null || this._resolvedCount == 0)
			{
				return false;
			}
			double y = this._panningTransform.Y;
			double top = Canvas.GetTop(this.FirstResolved.ContentPresenter);
			double num = (top > -y) ? (-top) : this.GetCoercedScrollPosition(y, false);
			if (y != num)
			{
				if (immediately)
				{
					this._panningTransform.Y = num;
				}
				else
				{
					this.AnimatePanel(LongListSelector._panDuration, this._panEase, num);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000C90C File Offset: 0x0000AB0C
		public void ScrollToGroup(object group)
		{
			double num = 0.0;
			bool flag = false;
			if (group == null)
			{
				return;
			}
			for (int i = this._resolvedFirstIndex; i < this._resolvedFirstIndex + this._resolvedCount; i++)
			{
				ContentPresenter contentPresenter = this._flattenedItems[i].ContentPresenter;
				if (contentPresenter.Content != null && contentPresenter.Content.Equals(group) && this._flattenedItems[i].ItemType == LongListSelector.ItemType.GroupHeader)
				{
					num = -Canvas.GetTop(contentPresenter);
					flag = true;
					break;
				}
				i++;
			}
			if (!flag)
			{
				int groupOffset = this.GetGroupOffset(group);
				if (groupOffset == -1)
				{
					return;
				}
				if (groupOffset != -1)
				{
					this.RecycleAllItems();
					this.CollapseRecycledElements();
					this._resolvedFirstIndex = groupOffset;
					num = 0.0;
					this.ResetMinMax();
				}
			}
			num = this.GetCoercedScrollPosition(num, false);
			this.StopAnimation();
			this._panningTransform.Y = num;
			this.Balance();
			if (group.Equals(this._firstGroup))
			{
				this._panningTransform.Y = this._maximumPanelScroll;
			}
			this.BounceBack(true);
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000CA15 File Offset: 0x0000AC15
		private double BounceDistance
		{
			get
			{
				return base.ActualHeight / 4.0;
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000CA28 File Offset: 0x0000AC28
		private double GetCoercedScrollPosition(double value, bool isBouncy)
		{
			double num = isBouncy ? this.BounceDistance : 0.0;
			return Math.Max(this._minimumPanelScroll - num, Math.Min(this._maximumPanelScroll + num, value));
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000CA68 File Offset: 0x0000AC68
		private void ScrollTowards()
		{
			if (this._scrollingTowards == -1)
			{
				return;
			}
			double num = (double)((this._scrollingTowards < this._resolvedFirstIndex) ? 10000000 : -10000000);
			double coercedScrollPosition = this.GetCoercedScrollPosition(this._panningTransform.Y + num, false);
			double num2 = coercedScrollPosition - this._panningTransform.Y;
			double num3 = Math.Abs(num2) / this.MaximumFlickVelocity;
			this.IsFlicking = true;
			this.AnimatePanel(new Duration(TimeSpan.FromSeconds(num3)), null, coercedScrollPosition);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000CAE8 File Offset: 0x0000ACE8
		private int GetFlattenedIndex(object item)
		{
			int count = this._flattenedItems.Count;
			for (int i = 0; i < count; i++)
			{
				if (item == this._flattenedItems[i].Item)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000CB24 File Offset: 0x0000AD24
		private int GetResolvedIndex(object item, out ContentPresenter contentPresenter)
		{
			if (this._resolvedCount > 0)
			{
				for (int i = this._resolvedFirstIndex; i < this._resolvedFirstIndex + this._resolvedCount; i++)
				{
					if (this._flattenedItems[i].Item == item)
					{
						contentPresenter = this._flattenedItems[i].ContentPresenter;
						return i;
					}
				}
			}
			contentPresenter = null;
			return -1;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000CB84 File Offset: 0x0000AD84
		public void DisplayGroupView()
		{
			this.StopScrolling();
			if (this.GroupItemTemplate != null && !this.IsFlatList)
			{
				this.OpenPopup();
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000CBA2 File Offset: 0x0000ADA2
		public void CloseGroupView()
		{
			this.ClosePopup(null, false);
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060002C8 RID: 712 RVA: 0x0000CBB0 File Offset: 0x0000ADB0
		// (remove) Token: 0x060002C9 RID: 713 RVA: 0x0000CBE8 File Offset: 0x0000ADE8
		public event EventHandler<GroupViewOpenedEventArgs> GroupViewOpened;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060002CA RID: 714 RVA: 0x0000CC20 File Offset: 0x0000AE20
		// (remove) Token: 0x060002CB RID: 715 RVA: 0x0000CC58 File Offset: 0x0000AE58
		public event EventHandler<GroupViewClosingEventArgs> GroupViewClosing;

		// Token: 0x060002CC RID: 716 RVA: 0x0000CC9A File Offset: 0x0000AE9A
		private void OpenPopup()
		{
			this.SaveSystemState(false, false);
			this.BuildPopup();
			this.AttachToPageEvents();
			this._groupSelectorPopup.IsOpen = true;
			base.UpdateLayout();
			SafeRaise.Raise<GroupViewOpenedEventArgs>(this.GroupViewOpened, this, () => new GroupViewOpenedEventArgs(this._itemsControl));
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000CD24 File Offset: 0x0000AF24
		private bool ClosePopup(object selectedGroup, bool raiseEvent)
		{
			if (raiseEvent)
			{
				GroupViewClosingEventArgs args = null;
				SafeRaise.Raise<GroupViewClosingEventArgs>(this.GroupViewClosing, this, () => args = new GroupViewClosingEventArgs(this._itemsControl, selectedGroup));
				if (args != null && args.Cancel)
				{
					return false;
				}
			}
			if (this._groupSelectorPopup != null)
			{
				this.RestoreSystemState();
				this._groupSelectorPopup.IsOpen = false;
				this.DetachFromPageEvents();
				this._groupSelectorPopup.Child = null;
				this._border = null;
				this._itemsControl = null;
				this._groupSelectorPopup = null;
			}
			return true;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000CDD0 File Offset: 0x0000AFD0
		private void BuildPopup()
		{
			this._groupSelectorPopup = new Popup();
			Color color = (Color)base.Resources["PhoneBackgroundColor"];
			this._border = new Border
			{
				Background = new SolidColorBrush(Color.FromArgb(160, color.R, color.G, color.B))
			};
			GestureListener gestureListener = GestureService.GetGestureListener(this._border);
			gestureListener.GestureBegin += new EventHandler<GestureEventArgs>(this.HandleGesture);
			gestureListener.GestureCompleted += new EventHandler<GestureEventArgs>(this.HandleGesture);
			gestureListener.DoubleTap += new EventHandler<GestureEventArgs>(this.HandleGesture);
			gestureListener.DragCompleted += new EventHandler<DragCompletedGestureEventArgs>(this.HandleGesture);
			gestureListener.DragDelta += new EventHandler<DragDeltaGestureEventArgs>(this.HandleGesture);
			gestureListener.DragStarted += new EventHandler<DragStartedGestureEventArgs>(this.HandleGesture);
			gestureListener.Flick += new EventHandler<FlickGestureEventArgs>(this.HandleGesture);
			gestureListener.Hold += new EventHandler<GestureEventArgs>(this.HandleGesture);
			gestureListener.PinchCompleted += new EventHandler<PinchGestureEventArgs>(this.HandleGesture);
			gestureListener.PinchDelta += new EventHandler<PinchGestureEventArgs>(this.HandleGesture);
			gestureListener.PinchStarted += new EventHandler<PinchStartedGestureEventArgs>(this.HandleGesture);
			gestureListener.Tap += new EventHandler<GestureEventArgs>(this.HandleGesture);
			this._itemsControl = new LongListSelector.LongListSelectorItemsControl();
			this._itemsControl.ItemTemplate = this.GroupItemTemplate;
			this._itemsControl.ItemsPanel = this.GroupItemsPanel;
			this._itemsControl.ItemsSource = this.ItemsSource;
			this._itemsControl.GroupSelected += this.itemsControl_GroupSelected;
			this._groupSelectorPopup.Child = this._border;
			ScrollViewer scrollViewer = new ScrollViewer
			{
				HorizontalScrollBarVisibility = 0
			};
			this._border.Child = scrollViewer;
			scrollViewer.Content = this._itemsControl;
			this.SetItemsControlSize();
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000CFB0 File Offset: 0x0000B1B0
		private void SetItemsControlSize()
		{
			Rect transformedRect = LongListSelector.GetTransformedRect();
			if (this._border != null)
			{
				this._border.Padding = new Thickness(18.0);
				this._border.RenderTransform = LongListSelector.GetTransform();
				this._border.Width = transformedRect.Width;
				this._border.Height = transformedRect.Height;
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000D018 File Offset: 0x0000B218
		private void itemsControl_GroupSelected(object sender, LongListSelector.GroupSelectedEventArgs e)
		{
			if (this.ClosePopup(e.Group, true))
			{
				this.ScrollToGroup(e.Group);
			}
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000D038 File Offset: 0x0000B238
		private void AttachToPageEvents()
		{
			PhoneApplicationFrame phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
			if (phoneApplicationFrame != null)
			{
				this._page = (phoneApplicationFrame.Content as PhoneApplicationPage);
				if (this._page != null)
				{
					this._page.BackKeyPress += new EventHandler<CancelEventArgs>(this.page_BackKeyPress);
					this._page.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(this.page_OrientationChanged);
				}
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000D09F File Offset: 0x0000B29F
		private void DetachFromPageEvents()
		{
			if (this._page != null)
			{
				this._page.BackKeyPress -= new EventHandler<CancelEventArgs>(this.page_BackKeyPress);
				this._page.OrientationChanged -= new EventHandler<OrientationChangedEventArgs>(this.page_OrientationChanged);
				this._page = null;
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000D0DE File Offset: 0x0000B2DE
		private void page_BackKeyPress(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.ClosePopup(null, true);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000D0F0 File Offset: 0x0000B2F0
		private void page_OrientationChanged(object sender, OrientationChangedEventArgs e)
		{
			this.SetItemsControlSize();
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000D0F8 File Offset: 0x0000B2F8
		private static Rect GetTransformedRect()
		{
			bool flag = LongListSelector.IsLandscape(LongListSelector.GetPageOrientation());
			return new Rect(0.0, 0.0, flag ? LongListSelector._screenHeight : LongListSelector._screenWidth, flag ? LongListSelector._screenWidth : LongListSelector._screenHeight);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000D148 File Offset: 0x0000B348
		private static Transform GetTransform()
		{
			PageOrientation pageOrientation = LongListSelector.GetPageOrientation();
			PageOrientation pageOrientation2 = pageOrientation;
			if (pageOrientation2 == 2 || pageOrientation2 == 18)
			{
				return new CompositeTransform
				{
					Rotation = 90.0,
					TranslateX = LongListSelector._screenWidth
				};
			}
			if (pageOrientation2 != 34)
			{
				return null;
			}
			return new CompositeTransform
			{
				Rotation = -90.0,
				TranslateY = LongListSelector._screenHeight
			};
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000D1B2 File Offset: 0x0000B3B2
		private static bool IsLandscape(PageOrientation orientation)
		{
			return orientation == 2 || orientation == 18 || orientation == 34;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000D1C4 File Offset: 0x0000B3C4
		private static PageOrientation GetPageOrientation()
		{
			PhoneApplicationFrame phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
			if (phoneApplicationFrame != null)
			{
				PhoneApplicationPage phoneApplicationPage = phoneApplicationFrame.Content as PhoneApplicationPage;
				return phoneApplicationPage.Orientation;
			}
			return 0;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000D1F8 File Offset: 0x0000B3F8
		private void SaveSystemState(bool newSystemTrayVisible, bool newApplicationBarVisible)
		{
			this._systemTrayVisible = SystemTray.IsVisible;
			SystemTray.IsVisible = newSystemTrayVisible;
			PhoneApplicationFrame phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
			if (phoneApplicationFrame != null)
			{
				PhoneApplicationPage phoneApplicationPage = phoneApplicationFrame.Content as PhoneApplicationPage;
				if (phoneApplicationPage != null && phoneApplicationPage.ApplicationBar != null)
				{
					this._applicationBarVisible = phoneApplicationPage.ApplicationBar.IsVisible;
					phoneApplicationPage.ApplicationBar.IsVisible = newApplicationBarVisible;
				}
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000D260 File Offset: 0x0000B460
		private void RestoreSystemState()
		{
			SystemTray.IsVisible = this._systemTrayVisible;
			PhoneApplicationFrame phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
			if (phoneApplicationFrame != null)
			{
				PhoneApplicationPage phoneApplicationPage = phoneApplicationFrame.Content as PhoneApplicationPage;
				if (phoneApplicationPage != null && phoneApplicationPage.ApplicationBar != null)
				{
					phoneApplicationPage.ApplicationBar.IsVisible = this._applicationBarVisible;
				}
			}
		}

		// Token: 0x040000EB RID: 235
		private const string ItemsPanelName = "ItemsPanel";

		// Token: 0x040000EC RID: 236
		private const string PanningTransformName = "PanningTransform";

		// Token: 0x040000ED RID: 237
		private const string VerticalScrollBarName = "VerticalScrollBar";

		// Token: 0x040000EE RID: 238
		private const double BufferSizeDefault = 1.0;

		// Token: 0x040000EF RID: 239
		private Panel _itemsPanel;

		// Token: 0x040000F0 RID: 240
		private TranslateTransform _panningTransform;

		// Token: 0x040000F1 RID: 241
		private ScrollBar _verticalScrollbar;

		// Token: 0x040000F2 RID: 242
		private Popup _groupSelectorPopup;

		// Token: 0x040000F3 RID: 243
		private Storyboard _panelStoryboard;

		// Token: 0x040000F4 RID: 244
		private DoubleAnimation _panelAnimation;

		// Token: 0x040000F5 RID: 245
		private DateTime _gestureStart;

		// Token: 0x040000F6 RID: 246
		private DispatcherTimer _stopTimer = new DispatcherTimer
		{
			Interval = TimeSpan.FromMilliseconds(50.0)
		};

		// Token: 0x040000F7 RID: 247
		private bool _ignoreNextTap;

		// Token: 0x040000F8 RID: 248
		private bool _firstDragDuringFlick;

		// Token: 0x040000F9 RID: 249
		private double _lastFlickVelocity;

		// Token: 0x040000FA RID: 250
		private static readonly Duration _panDuration = new Duration(TimeSpan.FromMilliseconds(100.0));

		// Token: 0x040000FB RID: 251
		private readonly IEasingFunction _panEase = new ExponentialEase();

		// Token: 0x040000FC RID: 252
		private static readonly TimeSpan _flickStopWaitTime = TimeSpan.FromMilliseconds(20.0);

		// Token: 0x040000FD RID: 253
		private int _scrollingTowards = -1;

		// Token: 0x040000FE RID: 254
		private double _bufferSizeCache = 1.0;

		// Token: 0x040000FF RID: 255
		private double _minimumPanelScroll = -3.4028234663852886E+38;

		// Token: 0x04000100 RID: 256
		private double _maximumPanelScroll;

		// Token: 0x04000101 RID: 257
		private bool _isLoaded;

		// Token: 0x04000102 RID: 258
		private bool _isPanning;

		// Token: 0x04000103 RID: 259
		private bool _isFlicking;

		// Token: 0x04000104 RID: 260
		private bool _isStretching;

		// Token: 0x04000105 RID: 261
		private double _dragTarget;

		// Token: 0x04000106 RID: 262
		private bool _isAnimating;

		// Token: 0x04000107 RID: 263
		private Size _availableSize;

		// Token: 0x04000108 RID: 264
		private Stack<ContentPresenter> _recycledGroupHeaders = new Stack<ContentPresenter>();

		// Token: 0x04000109 RID: 265
		private Stack<ContentPresenter> _recycledGroupFooters = new Stack<ContentPresenter>();

		// Token: 0x0400010A RID: 266
		private Stack<ContentPresenter> _recycledItems = new Stack<ContentPresenter>();

		// Token: 0x0400010B RID: 267
		private ContentPresenter _recycledListHeader;

		// Token: 0x0400010C RID: 268
		private ContentPresenter _recycledListFooter;

		// Token: 0x0400010D RID: 269
		private List<LongListSelector.ItemTuple> _flattenedItems;

		// Token: 0x0400010E RID: 270
		private object _firstGroup;

		// Token: 0x0400010F RID: 271
		private INotifyCollectionChanged _rootCollection;

		// Token: 0x04000110 RID: 272
		private List<INotifyCollectionChanged> _groupCollections;

		// Token: 0x04000111 RID: 273
		private int _resolvedFirstIndex;

		// Token: 0x04000112 RID: 274
		private int _resolvedCount;

		// Token: 0x04000113 RID: 275
		private int _screenFirstIndex;

		// Token: 0x04000114 RID: 276
		private int _screenCount;

		// Token: 0x04000115 RID: 277
		private bool _balanceNeededForSizeChanged;

		// Token: 0x04000116 RID: 278
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(LongListSelector), new PropertyMetadata(null, new PropertyChangedCallback(LongListSelector.OnItemsSourceChanged)));

		// Token: 0x04000117 RID: 279
		public static readonly DependencyProperty ListHeaderProperty = DependencyProperty.Register("ListHeader", typeof(object), typeof(LongListSelector), new PropertyMetadata(null));

		// Token: 0x04000118 RID: 280
		public static readonly DependencyProperty ListHeaderTemplateProperty = DependencyProperty.Register("ListHeaderTemplate", typeof(DataTemplate), typeof(LongListSelector), new PropertyMetadata(null));

		// Token: 0x04000119 RID: 281
		public static readonly DependencyProperty ListFooterProperty = DependencyProperty.Register("ListFooter", typeof(object), typeof(LongListSelector), new PropertyMetadata(null));

		// Token: 0x0400011A RID: 282
		public static readonly DependencyProperty ListFooterTemplateProperty = DependencyProperty.Register("ListFooterTemplate", typeof(DataTemplate), typeof(LongListSelector), new PropertyMetadata(null));

		// Token: 0x0400011B RID: 283
		public static readonly DependencyProperty GroupHeaderProperty = DependencyProperty.Register("GroupHeaderTemplate", typeof(DataTemplate), typeof(LongListSelector), new PropertyMetadata(null));

		// Token: 0x0400011C RID: 284
		public static readonly DependencyProperty GroupFooterProperty = DependencyProperty.Register("GroupFooterTemplate", typeof(DataTemplate), typeof(LongListSelector), new PropertyMetadata(null));

		// Token: 0x0400011D RID: 285
		public static readonly DependencyProperty ItemsTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(LongListSelector), new PropertyMetadata(null));

		// Token: 0x0400011E RID: 286
		public static readonly DependencyProperty GroupItemTemplateProperty = DependencyProperty.Register("GroupItemTemplate", typeof(DataTemplate), typeof(LongListSelector), new PropertyMetadata(null));

		// Token: 0x0400011F RID: 287
		public static readonly DependencyProperty GroupItemsPanelProperty = DependencyProperty.Register("GroupItemsPanel", typeof(ItemsPanelTemplate), typeof(LongListSelector), new PropertyMetadata(null));

		// Token: 0x04000120 RID: 288
		public static readonly DependencyProperty IsBouncyProperty = DependencyProperty.Register("IsBouncy", typeof(bool), typeof(LongListSelector), new PropertyMetadata(true));

		// Token: 0x04000121 RID: 289
		public static readonly DependencyProperty IsScrollingProperty = DependencyProperty.Register("IsScrolling", typeof(bool), typeof(LongListSelector), new PropertyMetadata(false, new PropertyChangedCallback(LongListSelector.OnIsScrollingChanged)));

		// Token: 0x04000122 RID: 290
		public static readonly DependencyProperty ShowListHeaderProperty = DependencyProperty.Register("ShowListHeader", typeof(bool), typeof(LongListSelector), new PropertyMetadata(true, new PropertyChangedCallback(LongListSelector.OnShowListHeaderChanged)));

		// Token: 0x04000123 RID: 291
		public static readonly DependencyProperty ShowListFooterProperty = DependencyProperty.Register("ShowListFooter", typeof(bool), typeof(LongListSelector), new PropertyMetadata(true, new PropertyChangedCallback(LongListSelector.OnShowListFooterChanged)));

		// Token: 0x04000124 RID: 292
		private bool _setSelectionInternal;

		// Token: 0x04000125 RID: 293
		private bool _selectedItemChanged;

		// Token: 0x04000126 RID: 294
		private object[] EmptyList = new object[0];

		// Token: 0x04000127 RID: 295
		private object[] _selectionList = new object[1];

		// Token: 0x04000128 RID: 296
		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(LongListSelector), new PropertyMetadata(new PropertyChangedCallback(LongListSelector.OnSelectedItemChanged)));

		// Token: 0x04000129 RID: 297
		public static readonly DependencyProperty BufferSizeProperty = DependencyProperty.Register("BufferSize", typeof(double), typeof(LongListSelector), new PropertyMetadata(1.0, new PropertyChangedCallback(LongListSelector.OnBufferSizeChanged)));

		// Token: 0x0400012A RID: 298
		public static readonly DependencyProperty MaximumFlickVelocityProperty = DependencyProperty.Register("MaximumFlickVelocity", typeof(double), typeof(LongListSelector), new PropertyMetadata(MotionParameters.MaximumSpeed, new PropertyChangedCallback(LongListSelector.OnMaximumFlickVelocityChanged)));

		// Token: 0x0400012B RID: 299
		public static readonly DependencyProperty DisplayAllGroupsProperty = DependencyProperty.Register("DisplayAllGroups", typeof(bool), typeof(LongListSelector), new PropertyMetadata(false, new PropertyChangedCallback(LongListSelector.OnDisplayAllGroupsChanged)));

		// Token: 0x04000134 RID: 308
		private PhoneApplicationPage _page;

		// Token: 0x04000135 RID: 309
		private bool _systemTrayVisible;

		// Token: 0x04000136 RID: 310
		private bool _applicationBarVisible;

		// Token: 0x04000137 RID: 311
		private Border _border;

		// Token: 0x04000138 RID: 312
		private LongListSelector.LongListSelectorItemsControl _itemsControl;

		// Token: 0x04000139 RID: 313
		private static readonly double _screenWidth = Application.Current.Host.Content.ActualWidth;

		// Token: 0x0400013A RID: 314
		private static readonly double _screenHeight = Application.Current.Host.Content.ActualHeight;

		// Token: 0x02000048 RID: 72
		private enum ItemType
		{
			// Token: 0x0400013F RID: 319
			Unknown,
			// Token: 0x04000140 RID: 320
			Item,
			// Token: 0x04000141 RID: 321
			GroupHeader,
			// Token: 0x04000142 RID: 322
			GroupFooter,
			// Token: 0x04000143 RID: 323
			ListHeader,
			// Token: 0x04000144 RID: 324
			ListFooter
		}

		// Token: 0x02000049 RID: 73
		private class ItemTuple
		{
			// Token: 0x04000145 RID: 325
			public LongListSelector.ItemType ItemType;

			// Token: 0x04000146 RID: 326
			public object Group;

			// Token: 0x04000147 RID: 327
			public object Item;

			// Token: 0x04000148 RID: 328
			public ContentPresenter ContentPresenter;
		}

		// Token: 0x0200004A RID: 74
		private class GroupSelectedEventArgs : EventArgs
		{
			// Token: 0x060002DE RID: 734 RVA: 0x0000D696 File Offset: 0x0000B896
			public GroupSelectedEventArgs(object group)
			{
				this.Group = group;
			}

			// Token: 0x1700008A RID: 138
			// (get) Token: 0x060002DF RID: 735 RVA: 0x0000D6A5 File Offset: 0x0000B8A5
			// (set) Token: 0x060002E0 RID: 736 RVA: 0x0000D6AD File Offset: 0x0000B8AD
			public object Group { get; private set; }
		}

		// Token: 0x0200004B RID: 75
		// (Invoke) Token: 0x060002E2 RID: 738
		private delegate void GroupSelectedEventHandler(object sender, LongListSelector.GroupSelectedEventArgs e);

		// Token: 0x0200004C RID: 76
		private class LongListSelectorItemsControl : ItemsControl
		{
			// Token: 0x1400001D RID: 29
			// (add) Token: 0x060002E5 RID: 741 RVA: 0x0000D6B8 File Offset: 0x0000B8B8
			// (remove) Token: 0x060002E6 RID: 742 RVA: 0x0000D6F0 File Offset: 0x0000B8F0
			public event LongListSelector.GroupSelectedEventHandler GroupSelected;

			// Token: 0x060002E7 RID: 743 RVA: 0x0000D725 File Offset: 0x0000B925
			protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
			{
				base.PrepareContainerForItemOverride(element, item);
				GestureService.GetGestureListener(element).Tap += new EventHandler<GestureEventArgs>(this.LongListSelectorItemsControl_Tap);
			}

			// Token: 0x060002E8 RID: 744 RVA: 0x0000D746 File Offset: 0x0000B946
			protected override void ClearContainerForItemOverride(DependencyObject element, object item)
			{
				base.ClearContainerForItemOverride(element, item);
				GestureService.GetGestureListener(element).Tap -= new EventHandler<GestureEventArgs>(this.LongListSelectorItemsControl_Tap);
			}

			// Token: 0x060002E9 RID: 745 RVA: 0x0000D768 File Offset: 0x0000B968
			private void LongListSelectorItemsControl_Tap(object sender, GestureEventArgs e)
			{
				ContentPresenter contentPresenter = sender as ContentPresenter;
				if (contentPresenter != null)
				{
					LongListSelector.GroupSelectedEventHandler groupSelected = this.GroupSelected;
					if (groupSelected != null)
					{
						LongListSelector.GroupSelectedEventArgs e2 = new LongListSelector.GroupSelectedEventArgs(contentPresenter.Content);
						groupSelected(this, e2);
					}
				}
			}
		}
	}
}
