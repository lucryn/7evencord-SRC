using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Gestures;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200001D RID: 29
	[TemplatePart(Name = "PivotItemPresenter", Type = typeof(ItemsPresenter))]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(PivotItem))]
	[TemplatePart(Name = "HeadersListElement", Type = typeof(PivotHeadersControl))]
	public class Pivot : TemplatedItemsControl<PivotItem>
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000106 RID: 262 RVA: 0x0000569C File Offset: 0x0000469C
		// (remove) Token: 0x06000107 RID: 263 RVA: 0x000056B5 File Offset: 0x000046B5
		public event EventHandler<PivotItemEventArgs> LoadingPivotItem;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000108 RID: 264 RVA: 0x000056CE File Offset: 0x000046CE
		// (remove) Token: 0x06000109 RID: 265 RVA: 0x000056E7 File Offset: 0x000046E7
		public event EventHandler<PivotItemEventArgs> LoadedPivotItem;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600010A RID: 266 RVA: 0x00005700 File Offset: 0x00004700
		// (remove) Token: 0x0600010B RID: 267 RVA: 0x00005719 File Offset: 0x00004719
		public event EventHandler<PivotItemEventArgs> UnloadingPivotItem;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600010C RID: 268 RVA: 0x00005732 File Offset: 0x00004732
		// (remove) Token: 0x0600010D RID: 269 RVA: 0x0000574B File Offset: 0x0000474B
		public event EventHandler<PivotItemEventArgs> UnloadedPivotItem;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600010E RID: 270 RVA: 0x00005764 File Offset: 0x00004764
		// (remove) Token: 0x0600010F RID: 271 RVA: 0x0000577D File Offset: 0x0000477D
		public event SelectionChangedEventHandler SelectionChanged;

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00005796 File Offset: 0x00004796
		// (set) Token: 0x06000111 RID: 273 RVA: 0x000057A8 File Offset: 0x000047A8
		public DataTemplate HeaderTemplate
		{
			get
			{
				return base.GetValue(Pivot.HeaderTemplateProperty) as DataTemplate;
			}
			set
			{
				base.SetValue(Pivot.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000057B6 File Offset: 0x000047B6
		// (set) Token: 0x06000113 RID: 275 RVA: 0x000057C8 File Offset: 0x000047C8
		public int SelectedIndex
		{
			get
			{
				return (int)base.GetValue(Pivot.SelectedIndexProperty);
			}
			set
			{
				base.SetValue(Pivot.SelectedIndexProperty, value);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000057DC File Offset: 0x000047DC
		private static void OnSelectedIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Pivot pivot = d as Pivot;
			if (pivot._ignorePropertyChange)
			{
				pivot._ignorePropertyChange = false;
				return;
			}
			pivot.UpdateSelectedIndex((int)e.OldValue, (int)e.NewValue);
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000115 RID: 277 RVA: 0x0000581E File Offset: 0x0000481E
		// (set) Token: 0x06000116 RID: 278 RVA: 0x0000582B File Offset: 0x0000482B
		public object SelectedItem
		{
			get
			{
				return base.GetValue(Pivot.SelectedItemProperty);
			}
			set
			{
				base.SetValue(Pivot.SelectedItemProperty, value);
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000583C File Offset: 0x0000483C
		private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Pivot pivot = d as Pivot;
			if (pivot._ignorePropertyChange)
			{
				pivot._ignorePropertyChange = false;
				return;
			}
			pivot.UpdateSelectedItem(e.OldValue, e.NewValue);
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00005874 File Offset: 0x00004874
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00005881 File Offset: 0x00004881
		public object Title
		{
			get
			{
				return base.GetValue(Pivot.TitleProperty);
			}
			set
			{
				base.SetValue(Pivot.TitleProperty, value);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600011A RID: 282 RVA: 0x0000588F File Offset: 0x0000488F
		// (set) Token: 0x0600011B RID: 283 RVA: 0x000058A1 File Offset: 0x000048A1
		public DataTemplate TitleTemplate
		{
			get
			{
				return base.GetValue(Pivot.TitleTemplateProperty) as DataTemplate;
			}
			set
			{
				base.SetValue(Pivot.TitleTemplateProperty, value);
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000058B0 File Offset: 0x000048B0
		public Pivot()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_STARTUP, PerfLog.Pivot);
			base.DefaultStyleKey = typeof(Pivot);
			base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
			GestureHelper gestureHelper = GestureHelper.Create(this);
			gestureHelper.GestureStart += new EventHandler<GestureEventArgs>(this.OnGestureStart);
			gestureHelper.HorizontalDrag += new EventHandler<DragEventArgs>(this.OnHorizontalDrag);
			gestureHelper.Flick += new EventHandler<FlickEventArgs>(this.OnFlick);
			gestureHelper.GestureEnd += new EventHandler<EventArgs>(this.OnGesturesComplete);
			this._isDesignTime = DesignerProperties.IsInDesignTool;
			this._queuedIndexChanges = new Queue<int>(5);
			base.Loaded += new RoutedEventHandler(this.Pivot_Loaded);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005983 File Offset: 0x00004983
		private void Pivot_Loaded(object sender, RoutedEventArgs e)
		{
			base.Loaded -= new RoutedEventHandler(this.Pivot_Loaded);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_STARTUP, PerfLog.Pivot);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000059A8 File Offset: 0x000049A8
		public override void OnApplyTemplate()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_APPLYTEMPLATE, PerfLog.Pivot);
			if (this._headers != null)
			{
				this._headers.SelectedIndexChanged -= new EventHandler<SelectedIndexChangedEventArgs>(this.OnHeaderSelectionChanged);
			}
			base.OnApplyTemplate();
			this._itemsPresenter = (base.GetTemplateChild("PivotItemPresenter") as ItemsPresenter);
			this._headers = (base.GetTemplateChild("HeadersListElement") as PivotHeadersControl);
			if (this._headers != null)
			{
				this._headers.SelectedIndexChanged += new EventHandler<SelectedIndexChangedEventArgs>(this.OnHeaderSelectionChanged);
				this.UpdateHeaders();
			}
			if (base.Items.Count > 0)
			{
				if (this.SelectedIndex < 0)
				{
					this.SelectedIndex = 0;
				}
				else
				{
					this.UpdateSelectedIndex(-1, this.SelectedIndex);
				}
			}
			this.UpdateVisibleContent(this.SelectedIndex);
			this.SetSelectedHeaderIndex(this.SelectedIndex);
			this._templateApplied = true;
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_APPLYTEMPLATE, PerfLog.Pivot);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005A98 File Offset: 0x00004A98
		protected override Size ArrangeOverride(Size finalSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_ARRANGE, PerfLog.Pivot);
			Size result = base.ArrangeOverride(finalSize);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_ARRANGE, PerfLog.Pivot);
			return result;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005ACC File Offset: 0x00004ACC
		protected override Size MeasureOverride(Size availableSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_MEASURE, PerfLog.Pivot);
			Size result = base.MeasureOverride(availableSize);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_MEASURE, PerfLog.Pivot);
			return result;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005B18 File Offset: 0x00004B18
		protected virtual void OnLoadingPivotItem(PivotItem item)
		{
			if (item != null && item.Visibility == 1)
			{
				item.Visibility = 0;
			}
			SafeRaise.Raise<PivotItemEventArgs>(this.LoadingPivotItem, this, () => new PivotItemEventArgs(item));
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005B84 File Offset: 0x00004B84
		protected virtual void OnLoadedPivotItem(PivotItem item)
		{
			SafeRaise.Raise<PivotItemEventArgs>(this.LoadedPivotItem, this, () => new PivotItemEventArgs(item));
			this.OptimizeVisuals();
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005C4C File Offset: 0x00004C4C
		private void OptimizeVisuals()
		{
			int selectedIndex = this.SelectedIndex;
			if (selectedIndex >= 0 && base.Items.Count > 1)
			{
				PivotItem next = base.GetContainer(base.Items[this.RollingIncrement(selectedIndex)]);
				PivotItem previous = base.GetContainer(base.Items[this.RollingDecrement(selectedIndex)]);
				bool flag = true;
				if (next != null && previous != null && next.Visibility == previous.Visibility && previous.Visibility == null)
				{
					flag = false;
				}
				if (flag)
				{
					base.Dispatcher.BeginInvoke(delegate()
					{
						if (next != null && next.Visibility == 1)
						{
							next.Visibility = 0;
						}
						if (previous != next)
						{
							this.Dispatcher.BeginInvoke(delegate()
							{
								if (previous != null && previous.Visibility == 1)
								{
									previous.Visibility = 0;
								}
							});
						}
					});
				}
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005D1C File Offset: 0x00004D1C
		protected virtual void OnUnloadingPivotItem(PivotItemEventArgs e)
		{
			EventHandler<PivotItemEventArgs> unloadingPivotItem = this.UnloadingPivotItem;
			if (unloadingPivotItem != null)
			{
				unloadingPivotItem.Invoke(this, e);
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005D3C File Offset: 0x00004D3C
		protected virtual void OnUnloadedPivotItem(PivotItemEventArgs e)
		{
			EventHandler<PivotItemEventArgs> unloadedPivotItem = this.UnloadedPivotItem;
			if (unloadedPivotItem != null)
			{
				unloadedPivotItem.Invoke(this, e);
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005D5C File Offset: 0x00004D5C
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			int selectedIndex = this.SelectedIndex;
			int? num = default(int?);
			int count = base.Items.Count;
			if (e != null)
			{
				switch (e.Action)
				{
				case 0:
					if (this._templateApplied && e.NewStartingIndex == selectedIndex)
					{
						num = new int?(selectedIndex);
					}
					break;
				case 1:
					num = new int?(selectedIndex);
					if (selectedIndex == e.OldStartingIndex || selectedIndex >= count)
					{
						num = new int?(0);
					}
					break;
				}
			}
			if (num != null)
			{
				this._animationHint = new AnimationDirection?((num < selectedIndex) ? AnimationDirection.Right : AnimationDirection.Left);
				this.SetSelectedIndexInternal(num.Value);
			}
			this.UpdateHeaders();
			this.OptimizeVisuals();
			base.OnItemsChanged(e);
			base.UpdateLayout();
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005E34 File Offset: 0x00004E34
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			PivotItem pivotItem = element as PivotItem;
			int selectedIndex = this.SelectedIndex;
			if (selectedIndex >= 0 && base.Items.Count > selectedIndex)
			{
				object obj = base.Items[selectedIndex];
				if (item == obj)
				{
					if (pivotItem != null && this._skippedLoadingPivotItem)
					{
						this.OnLoadingPivotItem(pivotItem);
						if (this._skippedSwapVisibleContent)
						{
							this.OnLoadedPivotItem(pivotItem);
						}
					}
					return;
				}
			}
			if (pivotItem != null)
			{
				this.UpdateItemVisibility(pivotItem, false);
				if (pivotItem.Visibility == null)
				{
					pivotItem.Visibility = 1;
				}
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00005EB8 File Offset: 0x00004EB8
		private void UpdateSelectedIndex(int oldIndex, int newIndex)
		{
			object selectedItem = null;
			int count = base.Items.Count;
			if (newIndex >= 0 && newIndex < count)
			{
				selectedItem = base.Items[newIndex];
			}
			else if (count > 0 && !this._isDesignTime)
			{
				this._ignorePropertyChange = true;
				this.SelectedIndex = oldIndex;
				throw new ArgumentException("SelectedIndex");
			}
			if (newIndex < 0 && base.Items.Count > 0 && !this._isDesignTime)
			{
				this._ignorePropertyChange = true;
				this.SelectedIndex = 0;
				throw new ArgumentException("SelectedIndex");
			}
			this.SelectedItem = selectedItem;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005F49 File Offset: 0x00004F49
		private void SetSelectedIndexInternal(int newIndex)
		{
			this._ignorePropertyChange = true;
			this.SelectedIndex = newIndex - 1;
			this.SelectedIndex = newIndex;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005F64 File Offset: 0x00004F64
		private void UpdateSelectedItem(object oldValue, object newValue)
		{
			if (newValue == null && base.Items.Count > 0 && !this._isDesignTime)
			{
				this._ignorePropertyChange = true;
				this.SelectedItem = oldValue;
				throw new ArgumentException("SelectedItem");
			}
			int num = base.Items.IndexOf(oldValue);
			int num2 = base.Items.IndexOf(newValue);
			if (this._animationHint == null && num != -1 && num2 != -1)
			{
				this._animationHint = new AnimationDirection?((this.RollingIncrement(num2) == num) ? AnimationDirection.Right : AnimationDirection.Left);
			}
			PivotItem container = base.GetContainer(newValue);
			PivotItem container2 = base.GetContainer(oldValue);
			this.BeginAnimateContent(num2, container2, this._animationHint.GetValueOrDefault());
			this.SetSelectedHeaderIndex(num2);
			if (this.SelectedIndex != num2)
			{
				this.SetSelectedIndexInternal(num2);
			}
			if (oldValue != null)
			{
				this.OnUnloadingPivotItem(new PivotItemEventArgs(container2));
			}
			if (container != null)
			{
				this.OnLoadingPivotItem(container);
			}
			else
			{
				this._skippedLoadingPivotItem = true;
			}
			List<object> list = new List<object>();
			list.Add(oldValue);
			IList list2 = list;
			List<object> list3 = new List<object>();
			list3.Add(newValue);
			this.OnSelectionChanged(new SelectionChangedEventArgs(list2, list3));
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006074 File Offset: 0x00005074
		private void SetSelectedHeaderIndex(int selectedIndex)
		{
			try
			{
				this._updatingHeaderItems = true;
				if (this._headers != null && base.Items.Count > 0)
				{
					this._headers.SelectedIndex = selectedIndex;
				}
			}
			finally
			{
				this._updatingHeaderItems = false;
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000060C4 File Offset: 0x000050C4
		private int RollingIncrement(int index)
		{
			index++;
			if (index >= base.Items.Count)
			{
				return 0;
			}
			return index;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000060DC File Offset: 0x000050DC
		private int RollingDecrement(int index)
		{
			index--;
			if (index >= 0)
			{
				return index;
			}
			return base.Items.Count - 1;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000060F8 File Offset: 0x000050F8
		protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			SelectionChangedEventHandler selectionChanged = this.SelectionChanged;
			if (selectionChanged != null)
			{
				selectionChanged.Invoke(this, e);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00006117 File Offset: 0x00005117
		private bool EnoughItemsForManipulation
		{
			get
			{
				return base.Items.Count > 1;
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006127 File Offset: 0x00005127
		private void OnGestureStart(object sender, GestureEventArgs e)
		{
			this._isHorizontalDragging = false;
			if (this._clickedHeadersControl != null)
			{
				this._clickedHeadersControl._wasClicked = false;
				this._clickedHeadersControl._cancelClick = false;
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006150 File Offset: 0x00005150
		private void OnGesturesComplete(object sender, EventArgs e)
		{
			if (this.EnoughItemsForManipulation)
			{
				DragEventArgs dragEventArgs = e as DragEventArgs;
				if (dragEventArgs != null && this._isHorizontalDragging)
				{
					double x = dragEventArgs.CumulativeDistance.X;
					double num = Math.Abs(x);
					if (x != 0.0 && num >= this._actualWidth / 3.0)
					{
						this.NavigateByIndexChange((x <= 0.0) ? 1 : -1);
					}
				}
				if (!this._animating && this._headers != null && this._itemsPresenter != null)
				{
					TransformAnimator.EnsureAnimator(this._itemsPresenter, ref this._panAnimator);
					this._panAnimator.GoTo(this.CalculateContentDestination(AnimationDirection.Center), Pivot.PivotAnimationDuration, this.QuarticEase);
					this._headers.RestoreHeaderPosition(Pivot.PivotAnimationDuration);
				}
			}
			this._isHorizontalDragging = false;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006224 File Offset: 0x00005224
		private void OnFlick(object sender, FlickEventArgs e)
		{
			if (this._clickedHeadersControl != null)
			{
				this._clickedHeadersControl._wasClicked = false;
				this._clickedHeadersControl._cancelClick = true;
			}
			if (this.EnoughItemsForManipulation)
			{
				int num = (int)e.Angle;
				if (num == 0 || num == 180)
				{
					this.NavigateByIndexChange((num == 180) ? 1 : -1);
				}
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006280 File Offset: 0x00005280
		private void OnHorizontalDrag(object sender, DragEventArgs e)
		{
			this._isHorizontalDragging = true;
			if (this._clickedHeadersControl != null)
			{
				this._clickedHeadersControl._cancelClick = true;
			}
			if (!this._animating && this.EnoughItemsForManipulation && this._itemsPresenter != null)
			{
				TransformAnimator.EnsureAnimator(this._itemsPresenter, ref this._panAnimator);
				double num = Math.Abs(e.DeltaDistance.X);
				if (!e.IsTouchComplete && !this._animating && this._panAnimator != null && this._headers != null)
				{
					TimeSpan timeSpan = TimeSpan.FromSeconds(num / 600.0);
					this._panAnimator.GoTo(e.CumulativeDistance.X, new Duration(timeSpan));
					this._headers.PanHeader(e.CumulativeDistance.X, this._actualWidth, new Duration(timeSpan));
				}
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006364 File Offset: 0x00005364
		private PivotHeaderItem CreateHeaderBindingControl(object item)
		{
			PivotHeaderItem pivotHeaderItem = new PivotHeaderItem
			{
				ContentTemplate = this.HeaderTemplate
			};
			Binding binding = new Binding
			{
				Source = item
			};
			if (item is PivotItem)
			{
				binding.Path = new PropertyPath("Header", new object[0]);
			}
			PivotHeaderItem result;
			try
			{
				binding.Mode = 1;
				pivotHeaderItem.SetBinding(ContentControl.ContentProperty, binding);
				result = pivotHeaderItem;
			}
			catch
			{
				if (!this._isDesignTime)
				{
					throw;
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000063F0 File Offset: 0x000053F0
		private void UpdateHeaders()
		{
			if (this._headers != null)
			{
				List<PivotHeaderItem> list = new List<PivotHeaderItem>();
				int count = base.Items.Count;
				for (int i = 0; i < count; i++)
				{
					object item = base.Items[i];
					list.Add(this.CreateHeaderBindingControl(item));
				}
				try
				{
					this._updatingHeaderItems = true;
					this._headers.ItemsSource = ((count == 0) ? null : list);
				}
				finally
				{
					this._updatingHeaderItems = false;
				}
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006470 File Offset: 0x00005470
		private void OnHeaderSelectionChanged(object s, SelectedIndexChangedEventArgs e)
		{
			if (!this._updatingHeaderItems)
			{
				this._animationHint = new AnimationDirection?(AnimationDirection.Left);
				this.SelectedIndex = e.SelectedIndex;
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006494 File Offset: 0x00005494
		private void NavigateByIndexChange(int indexDelta)
		{
			if (this._animating && this._queuedIndexChanges != null)
			{
				this._queuedIndexChanges.Enqueue(indexDelta);
				return;
			}
			int num = this.SelectedIndex;
			if (num != -1)
			{
				this._animationHint = new AnimationDirection?((indexDelta > 0) ? AnimationDirection.Left : AnimationDirection.Right);
				num += indexDelta;
				if (num >= base.Items.Count)
				{
					num = 0;
				}
				else if (num < 0)
				{
					num = base.Items.Count - 1;
				}
				if (this._clickedHeadersControl != null)
				{
					this._clickedHeadersControl._wasClicked = false;
					this._clickedHeadersControl._cancelClick = true;
				}
				this.SelectedIndex = num;
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000652C File Offset: 0x0000552C
		private int GetPreviousIndex()
		{
			int count = base.Items.Count;
			if (count > 0)
			{
				int num = this.SelectedIndex - 1;
				if (num < 0)
				{
					num = count - 1;
				}
				return num;
			}
			return 0;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006560 File Offset: 0x00005560
		private int GetNextIndex()
		{
			int count = base.Items.Count;
			if (count > 0)
			{
				int num = this.SelectedIndex + 1;
				if (num > count)
				{
					num = 0;
				}
				return num;
			}
			return 0;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006590 File Offset: 0x00005590
		private void UpdateVisibleContent(int index)
		{
			if (this.TryHasItemsHost())
			{
				for (int i = 0; i < this._itemsPanel.Children.Count; i++)
				{
					UIElement element = this._itemsPanel.Children[i];
					this.UpdateItemVisibility(element, i == index);
				}
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000065E0 File Offset: 0x000055E0
		private bool TryHasItemsHost()
		{
			if (this._itemsPanel != null)
			{
				return true;
			}
			if (base.ItemContainerGenerator != null)
			{
				DependencyObject dependencyObject = base.ItemContainerGenerator.ContainerFromIndex(0);
				if (dependencyObject != null)
				{
					this._itemsPanel = (VisualTreeHelper.GetParent(dependencyObject) as Panel);
					return this._itemsPanel != null;
				}
			}
			return false;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006630 File Offset: 0x00005630
		protected virtual void UpdateItemVisibility(UIElement element, bool toVisible)
		{
			if (element != null)
			{
				element.Opacity = (double)(toVisible ? 1 : 0);
				element.IsHitTestVisible = toVisible;
				if (toVisible && element.Visibility == 1)
				{
					element.Visibility = 0;
				}
				if (this._isDesignTime)
				{
					TranslateTransform translateTransform = TransformAnimator.GetTranslateTransform(element);
					if (translateTransform != null)
					{
						translateTransform.X = (toVisible ? 0.0 : (-base.ActualWidth));
					}
				}
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006698 File Offset: 0x00005698
		private double CalculateContentDestination(AnimationDirection direction)
		{
			double result = 0.0;
			double actualWidth = base.ActualWidth;
			switch (direction)
			{
			case AnimationDirection.Left:
				result = -actualWidth;
				break;
			case AnimationDirection.Right:
				result = actualWidth;
				break;
			}
			return result;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000066D4 File Offset: 0x000056D4
		private static AnimationDirection InvertAnimationDirection(AnimationDirection direction)
		{
			switch (direction)
			{
			case AnimationDirection.Left:
				return AnimationDirection.Right;
			case AnimationDirection.Right:
				return AnimationDirection.Left;
			default:
				return direction;
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000067D8 File Offset: 0x000057D8
		private void BeginAnimateContent(int newIndex, PivotItem oldItem, AnimationDirection animationDirection)
		{
			if (this._isDesignTime)
			{
				this.SwapVisibleContent(oldItem, newIndex);
				return;
			}
			if (this._itemsPresenter != null)
			{
				this._animating = true;
				TransformAnimator.EnsureAnimator(this._itemsPresenter, ref this._panAnimator);
				PivotItem container = base.GetContainer(this.SelectedItem);
				if (container != null)
				{
					container.MoveTo(AnimationDirection.Center);
				}
				if (this._headers != null && animationDirection != AnimationDirection.Center)
				{
					this._headers.AnimationDirection = animationDirection;
				}
				this._panAnimator.GoTo(this.CalculateContentDestination(animationDirection), Pivot.PivotAnimationDuration, delegate()
				{
					this._panAnimator.GoTo(this.CalculateContentDestination(Pivot.InvertAnimationDirection(animationDirection)), Pivot.ZeroDuration);
					this.SwapVisibleContent(oldItem, newIndex);
					PivotItem container2 = this.GetContainer(this.SelectedItem);
					if (container2 != null)
					{
						container2.MoveTo(animationDirection);
					}
					this._panAnimator.GoTo(0.0, Pivot.PivotAnimationDuration, this.QuarticEase, delegate()
					{
						this._animationHint = default(AnimationDirection?);
						this._animating = false;
						this.ProcessQueuedChanges();
					});
				});
				return;
			}
			this._skippedSwapVisibleContent = true;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000068B1 File Offset: 0x000058B1
		private void SwapVisibleContent(PivotItem oldItem, int newIndex)
		{
			if (oldItem != null)
			{
				this.OnUnloadedPivotItem(new PivotItemEventArgs(oldItem));
			}
			this.UpdateVisibleContent(newIndex);
			this.OnLoadedPivotItem(base.GetContainer(this.SelectedItem));
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000068DC File Offset: 0x000058DC
		private void ProcessQueuedChanges()
		{
			if (this._queuedIndexChanges != null && this._queuedIndexChanges.Count > 0 && !this._animating)
			{
				int indexDelta = this._queuedIndexChanges.Dequeue();
				this.NavigateByIndexChange(indexDelta);
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000691C File Offset: 0x0000591C
		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this._actualWidth = base.ActualWidth;
			base.Clip = new RectangleGeometry
			{
				Rect = new Rect(0.0, 0.0, this._actualWidth, base.ActualHeight)
			};
			if (this._isDesignTime)
			{
				this.UpdateVisibleContent(this.SelectedIndex);
			}
		}

		// Token: 0x0400007F RID: 127
		private const string ElementHeadersRowDefinitionName = "HeadersRowDefinition";

		// Token: 0x04000080 RID: 128
		private const string HeadersListElement = "HeadersListElement";

		// Token: 0x04000081 RID: 129
		private const string PivotItemPresenterElement = "PivotItemPresenter";

		// Token: 0x04000082 RID: 130
		internal const string ItemContainerStyleName = "ItemContainerStyle";

		// Token: 0x04000083 RID: 131
		private const double pixelsPerSecondTemporary = 600.0;

		// Token: 0x04000084 RID: 132
		internal const double PivotAnimationSeconds = 0.25;

		// Token: 0x04000085 RID: 133
		private static readonly TimeSpan PivotAnimationTimeSpan = TimeSpan.FromSeconds(0.25);

		// Token: 0x04000086 RID: 134
		internal static readonly Duration PivotAnimationDuration = new Duration(Pivot.PivotAnimationTimeSpan);

		// Token: 0x04000087 RID: 135
		internal static readonly Duration ZeroDuration = new Duration(TimeSpan.Zero);

		// Token: 0x04000088 RID: 136
		internal readonly IEasingFunction QuarticEase = new QuarticEase();

		// Token: 0x04000089 RID: 137
		private PivotHeadersControl _headers;

		// Token: 0x0400008A RID: 138
		private ItemsPresenter _itemsPresenter;

		// Token: 0x0400008B RID: 139
		private Panel _itemsPanel;

		// Token: 0x0400008C RID: 140
		private AnimationDirection? _animationHint = default(AnimationDirection?);

		// Token: 0x0400008D RID: 141
		private bool _updatingHeaderItems;

		// Token: 0x0400008E RID: 142
		private bool _ignorePropertyChange;

		// Token: 0x0400008F RID: 143
		internal PivotHeadersControl _clickedHeadersControl;

		// Token: 0x04000090 RID: 144
		private bool _animating;

		// Token: 0x04000091 RID: 145
		private bool _isHorizontalDragging;

		// Token: 0x04000092 RID: 146
		private double _actualWidth;

		// Token: 0x04000093 RID: 147
		private bool _isDesignTime;

		// Token: 0x04000094 RID: 148
		private bool _skippedLoadingPivotItem;

		// Token: 0x04000095 RID: 149
		private bool _skippedSwapVisibleContent;

		// Token: 0x04000096 RID: 150
		private bool _templateApplied;

		// Token: 0x04000097 RID: 151
		private Queue<int> _queuedIndexChanges;

		// Token: 0x04000098 RID: 152
		private TransformAnimator _panAnimator;

		// Token: 0x0400009E RID: 158
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Pivot), new PropertyMetadata(null));

		// Token: 0x0400009F RID: 159
		public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Pivot), new PropertyMetadata(new PropertyChangedCallback(Pivot.OnSelectedIndexPropertyChanged)));

		// Token: 0x040000A0 RID: 160
		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(Pivot), new PropertyMetadata(null, new PropertyChangedCallback(Pivot.OnSelectedItemPropertyChanged)));

		// Token: 0x040000A1 RID: 161
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(object), typeof(Pivot), new PropertyMetadata(null));

		// Token: 0x040000A2 RID: 162
		public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(Pivot), new PropertyMetadata(null));
	}
}
