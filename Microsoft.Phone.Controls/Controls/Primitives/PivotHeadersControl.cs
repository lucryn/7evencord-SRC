using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x0200001F RID: 31
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(PivotHeaderItem))]
	[TemplatePart(Name = "Canvas", Type = typeof(Canvas))]
	public class PivotHeadersControl : TemplatedItemsControl<PivotHeaderItem>
	{
		// Token: 0x06000152 RID: 338 RVA: 0x00006BF0 File Offset: 0x00005BF0
		protected override Size ArrangeOverride(Size finalSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_ARRANGE, PerfLog.PivotHeadersControl);
			Size result = base.ArrangeOverride(finalSize);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_ARRANGE, PerfLog.PivotHeadersControl);
			return result;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006C24 File Offset: 0x00005C24
		protected override Size MeasureOverride(Size availableSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_MEASURE, PerfLog.PivotHeadersControl);
			Size result = base.MeasureOverride(availableSize);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_MEASURE, PerfLog.PivotHeadersControl);
			return result;
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00006C58 File Offset: 0x00005C58
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00006C6A File Offset: 0x00005C6A
		internal int SelectedIndex
		{
			get
			{
				return (int)base.GetValue(PivotHeadersControl.SelectedIndexProperty);
			}
			set
			{
				base.SetValue(PivotHeadersControl.SelectedIndexProperty, value);
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00006C80 File Offset: 0x00005C80
		private static void OnSelectedIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PivotHeadersControl pivotHeadersControl = d as PivotHeadersControl;
			int index = (int)e.NewValue;
			int previousIndex = (int)e.OldValue;
			if (!pivotHeadersControl._activeSelectionChange)
			{
				pivotHeadersControl.SelectOne(previousIndex, index);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00006CBE File Offset: 0x00005CBE
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00006CD0 File Offset: 0x00005CD0
		public int VisualFirstIndex
		{
			get
			{
				return (int)base.GetValue(PivotHeadersControl.VisualFirstIndexProperty);
			}
			set
			{
				base.SetValue(PivotHeadersControl.VisualFirstIndexProperty, value);
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00006CE4 File Offset: 0x00005CE4
		private static void OnVisualFirstIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PivotHeadersControl pivotHeadersControl = d as PivotHeadersControl;
			if (pivotHeadersControl._ignorePropertyChange)
			{
				pivotHeadersControl._ignorePropertyChange = false;
				return;
			}
			int num = (int)e.NewValue;
			pivotHeadersControl._previousVisualFirstIndex = (int)e.OldValue;
			int count = pivotHeadersControl.Items.Count;
			if (count > 0 && num >= count)
			{
				pivotHeadersControl._ignorePropertyChange = true;
				d.SetValue(e.Property, 0);
			}
			pivotHeadersControl.UpdateItemsLayout();
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600015A RID: 346 RVA: 0x00006D5B File Offset: 0x00005D5B
		// (remove) Token: 0x0600015B RID: 347 RVA: 0x00006D74 File Offset: 0x00005D74
		internal event EventHandler<SelectedIndexChangedEventArgs> SelectedIndexChanged;

		// Token: 0x0600015C RID: 348 RVA: 0x00006D90 File Offset: 0x00005D90
		public PivotHeadersControl()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_STARTUP, PerfLog.PivotHeadersControl);
			base.DefaultStyleKey = typeof(PivotHeadersControl);
			this._leftMirror = new Image();
			this._leftMirror.CacheMode = new BitmapCache();
			this._sizes = new Dictionary<Control, double>();
			this._translations = new Dictionary<Control, TranslateTransform>();
			this._opacities = new Dictionary<Control, OpacityAnimator>();
			this._isDesign = DesignerProperties.IsInDesignTool;
			this._queuedAnimations = new Queue<PivotHeadersControl.AnimationInstruction>();
			base.Loaded += new RoutedEventHandler(this.PivotHeadersControl_Loaded);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00006E31 File Offset: 0x00005E31
		private void PivotHeadersControl_Loaded(object sender, RoutedEventArgs e)
		{
			base.Loaded -= new RoutedEventHandler(this.PivotHeadersControl_Loaded);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_STARTUP, PerfLog.PanoramaItem);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00006E54 File Offset: 0x00005E54
		public override void OnApplyTemplate()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_APPLYTEMPLATE, PerfLog.PivotHeadersControl);
			this._pivot = null;
			if (this._canvas != null)
			{
				this._canvas.Children.Remove(this._leftMirror);
				this._leftMirror = null;
			}
			base.OnApplyTemplate();
			this._canvas = (base.GetTemplateChild("Canvas") as Canvas);
			if (this._canvas != null)
			{
				this._canvas.Children.Add(this._leftMirror);
				this._leftMirrorTranslation = TransformAnimator.GetTranslateTransform(this._leftMirror);
				if (!double.IsNaN(this._leftMirror.ActualWidth) && this._leftMirror.ActualWidth > 0.0)
				{
					this._leftMirrorTranslation.X = -this._leftMirror.ActualWidth;
				}
			}
			if (base.Items.Count > 0)
			{
				this.VisualFirstIndex = this.SelectedIndex;
			}
			DependencyObject dependencyObject = this;
			do
			{
				dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
				this._pivot = (dependencyObject as Pivot);
			}
			while (this._pivot == null && dependencyObject != null);
			if (this._pivot != null)
			{
				this._pivot._clickedHeadersControl = this;
			}
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_APPLYTEMPLATE, PerfLog.PivotHeadersControl);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00006F84 File Offset: 0x00005F84
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (base.Items.Count > 0)
			{
				this.UpdateItemsLayout();
				return;
			}
			this.VisualFirstIndex = 0;
			this.SelectedIndex = 0;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006FB0 File Offset: 0x00005FB0
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			PivotHeaderItem pivotHeaderItem = (PivotHeaderItem)element;
			pivotHeaderItem.ParentHeadersControl = null;
			pivotHeaderItem.Item = null;
			if (!object.ReferenceEquals(element, item))
			{
				pivotHeaderItem.Item = item;
			}
			Control control = item as Control;
			if (control != null)
			{
				control.SizeChanged -= new SizeChangedEventHandler(this.OnHeaderSizeChanged);
				this._sizes.Remove(control);
				this._translations.Remove(control);
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007020 File Offset: 0x00006020
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			PivotHeaderItem pivotHeaderItem = (PivotHeaderItem)element;
			pivotHeaderItem.ParentHeadersControl = this;
			int num = base.ItemContainerGenerator.IndexFromContainer(element);
			if (num != -1)
			{
				pivotHeaderItem.IsSelected = (this.SelectedIndex == num);
			}
			Control control = item as Control;
			if (control != null)
			{
				control.SizeChanged += new SizeChangedEventHandler(this.OnHeaderSizeChanged);
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007080 File Offset: 0x00006080
		private void OnHeaderSizeChanged(object sender, SizeChangedEventArgs e)
		{
			double width = e.NewSize.Width;
			double height = e.NewSize.Height;
			if (double.IsNaN(base.Height) || height > base.Height)
			{
				base.Height = height;
			}
			this._sizes[(Control)sender] = width;
			this.UpdateItemsLayout();
			if (this._leftMirrorTranslation.X == 0.0)
			{
				this.UpdateLeftMirrorImage(this.SelectedIndex);
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007102 File Offset: 0x00006102
		internal void OnHeaderItemClicked(PivotHeaderItem item)
		{
			if (!this._isAnimating)
			{
				if (this._cancelClick)
				{
					this._cancelClick = false;
					return;
				}
				this._wasClicked = true;
				item.IsSelected = true;
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000712C File Offset: 0x0000612C
		internal void NotifyHeaderItemSelected(PivotHeaderItem item, bool isSelected)
		{
			if (isSelected)
			{
				int num = base.ItemContainerGenerator.IndexFromContainer(item);
				int selectedIndex = this.SelectedIndex;
				this.SelectOne(selectedIndex, num);
				this.SelectedIndex = num;
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00007160 File Offset: 0x00006160
		private void SelectOne(int previousIndex, int index)
		{
			if (!this._activeSelectionChange)
			{
				this.UpdateLeftMirrorImage(index);
				if (index >= 0 && index < base.Items.Count)
				{
					try
					{
						this._activeSelectionChange = true;
						for (int i = 0; i < base.Items.Count; i++)
						{
							PivotHeaderItem pivotHeaderItem = (PivotHeaderItem)base.ItemContainerGenerator.ContainerFromIndex(i);
							if (pivotHeaderItem != null)
							{
								pivotHeaderItem.IsSelected = (index == i);
							}
						}
					}
					finally
					{
						SafeRaise.Raise<SelectedIndexChangedEventArgs>(this.SelectedIndexChanged, this, new SelectedIndexChangedEventArgs(index));
						this._activeSelectionChange = false;
						this.BeginAnimate(previousIndex, index);
					}
				}
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00007200 File Offset: 0x00006200
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00007208 File Offset: 0x00006208
		internal AnimationDirection AnimationDirection { get; set; }

		// Token: 0x06000168 RID: 360 RVA: 0x00007211 File Offset: 0x00006211
		internal void RestoreHeaderPosition(Duration duration)
		{
			if (this._canvas != null && !this._isAnimating)
			{
				TransformAnimator.EnsureAnimator(this._canvas, ref this._canvasAnimator);
				if (this._canvasAnimator != null)
				{
					this._canvasAnimator.GoTo(0.0, duration);
				}
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007254 File Offset: 0x00006254
		internal void PanHeader(double cumulative, double contentWidth, Duration duration)
		{
			if (!this._isAnimating && this._canvas != null)
			{
				TransformAnimator.EnsureAnimator(this._canvas, ref this._canvasAnimator);
				if (this._canvasAnimator != null)
				{
					double num = (cumulative < 0.0) ? this.GetItemWidth(this.SelectedIndex) : this.GetLeftMirrorWidth(this.SelectedIndex);
					this._canvasAnimator.GoTo(cumulative / contentWidth * num, duration);
				}
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000072C4 File Offset: 0x000062C4
		private void BeginAnimate(int previousIndex, int newIndex)
		{
			if (this._isDesign || this._canvas == null)
			{
				this.VisualFirstIndex = newIndex;
				return;
			}
			if ((newIndex != this.RollingIncrement(previousIndex) && newIndex != this.RollingDecrement(previousIndex)) || this._wasClicked)
			{
				this._wasClicked = false;
				int num2;
				for (int num = previousIndex; num != newIndex; num = num2)
				{
					num2 = this.RollingIncrement(num);
					PivotHeadersControl.AnimationInstruction animationInstruction = new PivotHeadersControl.AnimationInstruction(num, num2);
					animationInstruction._width = this.GetItemWidth(num);
					if (animationInstruction._width > 0.0)
					{
						this._queuedAnimations.Enqueue(animationInstruction);
					}
				}
				this.UpdateActiveAndQueuedAnimations();
			}
			else
			{
				if (this._queuedAnimations.Count == 0 && !this._isAnimating)
				{
					this.BeginAnimateInternal(previousIndex, newIndex, this.QuarticEase, default(Duration?));
					return;
				}
				PivotHeadersControl.AnimationInstruction animationInstruction2 = new PivotHeadersControl.AnimationInstruction(previousIndex, newIndex);
				animationInstruction2._ease = this.QuarticEase;
				animationInstruction2._width = this.GetItemWidth(previousIndex);
				this._queuedAnimations.Enqueue(animationInstruction2);
				this.UpdateActiveAndQueuedAnimations();
			}
			if (!this._isAnimating)
			{
				this.AnimateComplete();
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000073C8 File Offset: 0x000063C8
		private void UpdateActiveAndQueuedAnimations()
		{
			TransformAnimator.EnsureAnimator(this._canvas, ref this._canvasAnimator);
			if (this._canvasAnimator == null)
			{
				return;
			}
			double num = 0.0;
			foreach (PivotHeadersControl.AnimationInstruction animationInstruction in this._queuedAnimations)
			{
				num += animationInstruction._width;
			}
			if (this._isAnimating && this._animatingWidth > 0.0)
			{
				num += this._animatingWidth;
			}
			int num2 = 0;
			foreach (PivotHeadersControl.AnimationInstruction animationInstruction2 in this._queuedAnimations)
			{
				num2++;
				animationInstruction2._durationInSeconds = animationInstruction2._width / ((num == 0.0) ? 1.0 : num) * 0.5;
				animationInstruction2._ease = ((num2 == this._queuedAnimations.Count) ? this.QuarticEase : null);
				if (this._isAnimating)
				{
					this._canvasAnimator.UpdateEasingFunction(null);
				}
			}
			if (this._isAnimating)
			{
				double totalSeconds = (DateTime.Now - this._currentItemAnimationStarted).TotalSeconds;
				double num3 = totalSeconds / 0.5;
				double num4 = this._animatingWidth * num3 / ((num == 0.0) ? 1.0 : num);
				this._canvasAnimator.UpdateDuration(new Duration(TimeSpan.FromSeconds(num4 * num3 * 0.5)));
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007610 File Offset: 0x00006610
		private void BeginAnimateInternal(int previousIndex, int newIndex, IEasingFunction ease, Duration? optionalDuration)
		{
			if (previousIndex == newIndex || previousIndex < 0 || previousIndex >= base.Items.Count || this._isDesign || this._canvas == null)
			{
				if (this.VisualFirstIndex != newIndex)
				{
					this.VisualFirstIndex = newIndex;
				}
				this.AnimateComplete();
				return;
			}
			TransformAnimator.EnsureAnimator(this._canvas, ref this._canvasAnimator);
			this._isAnimating = true;
			bool flag = (base.Items.Count != 2) ? (newIndex == this.RollingIncrement(previousIndex)) : (this.AnimationDirection == AnimationDirection.Left);
			int index = flag ? previousIndex : newIndex;
			double itemWidth = this.GetItemWidth(index);
			this._animatingWidth = itemWidth;
			this._currentItemAnimationStarted = DateTime.Now;
			double targetOffset = -itemWidth + (flag ? 0.0 : this._canvasAnimator.CurrentOffset);
			double num = (itemWidth == 0.0) ? itemWidth : ((itemWidth - Math.Abs(this._canvasAnimator.CurrentOffset)) / itemWidth);
			if (num == 0.0)
			{
				num = 1.0;
			}
			Duration currentSampleDuration = (optionalDuration != null) ? optionalDuration.Value : new Duration(TimeSpan.FromSeconds(0.25 + Math.Abs(num * 0.25)));
			if (flag)
			{
				this._canvasAnimator.GoTo(targetOffset, currentSampleDuration, ease, delegate()
				{
					this.VisualFirstIndex = newIndex;
					this._canvasAnimator.GoTo(0.0, Pivot.ZeroDuration, new Action(this.AnimateComplete));
				});
				return;
			}
			this.VisualFirstIndex = newIndex;
			this._canvasAnimator.GoTo(targetOffset, Pivot.ZeroDuration, delegate()
			{
				this._canvasAnimator.GoTo(0.0, currentSampleDuration, ease, new Action(this.AnimateComplete));
			});
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000077FC File Offset: 0x000067FC
		private void AnimateComplete()
		{
			if (this._queuedAnimations.Count == 0)
			{
				this._isAnimating = false;
				return;
			}
			PivotHeadersControl.AnimationInstruction animationInstruction = this._queuedAnimations.Dequeue();
			Duration duration;
			duration..ctor(TimeSpan.FromSeconds(animationInstruction._durationInSeconds));
			this.BeginAnimateInternal(animationInstruction._previousIndex, animationInstruction._index, animationInstruction._ease, new Duration?(duration));
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000785A File Offset: 0x0000685A
		private double GetLeftMirrorWidth(int index)
		{
			return this.GetItemWidth(this.GetPreviousVisualIndex(index));
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000786C File Offset: 0x0000686C
		private double GetNextHeaderWidth()
		{
			int num = this.VisualFirstIndex + 1;
			if (num >= base.Items.Count)
			{
				num = 0;
			}
			return this.GetItemWidth(num);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000789C File Offset: 0x0000689C
		private double GetItemWidth(int index)
		{
			Control control = this.GetItemFromIndex(index) as Control;
			double num = 0.0;
			if (control != null && !this._sizes.TryGetValue(control, ref num))
			{
				num = control.ActualWidth;
				if (!double.IsNaN(num))
				{
					this._sizes[control] = num;
				}
			}
			return num;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000078F0 File Offset: 0x000068F0
		private int GetPreviousVisualIndex(int indexOfInterest)
		{
			int num = indexOfInterest - 1;
			if (num >= 0)
			{
				return num;
			}
			return base.Items.Count - 1;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007914 File Offset: 0x00006914
		private void UpdateLeftMirrorImage(int visualRootIndex)
		{
			if (this._leftMirrorTranslation != null && this._sizes != null && this._leftMirror != null)
			{
				if (base.Items.Count <= 1)
				{
					this._leftMirror = null;
					return;
				}
				int previousVisualIndex = this.GetPreviousVisualIndex(visualRootIndex);
				PivotHeaderItem pivotHeaderItem = this.GetItemFromIndex(previousVisualIndex) as PivotHeaderItem;
				if (pivotHeaderItem == null || !this._sizes.ContainsKey(pivotHeaderItem))
				{
					return;
				}
				double num = this._sizes[pivotHeaderItem];
				pivotHeaderItem.UpdateVisualStateToUnselected();
				try
				{
					WriteableBitmap source = new WriteableBitmap(pivotHeaderItem, new TranslateTransform());
					this._leftMirror.Source = source;
				}
				catch (Exception)
				{
					this._leftMirror.Source = null;
				}
				finally
				{
					pivotHeaderItem.RestoreVisualStates();
					this._leftMirrorTranslation.X = -num;
				}
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000079F0 File Offset: 0x000069F0
		private void UpdateItemsLayout()
		{
			int count = base.Items.Count;
			double num = 0.0;
			int visualFirstIndex = this.VisualFirstIndex;
			for (int i = visualFirstIndex; i < base.Items.Count; i++)
			{
				this.FadeInItemIfNeeded(i, visualFirstIndex, this._previousVisualFirstIndex, count);
				this.SetItemPosition(i, ref num);
			}
			if (this.VisualFirstIndex > 0)
			{
				for (int j = 0; j < this.VisualFirstIndex; j++)
				{
					this.FadeInItemIfNeeded(j, visualFirstIndex, this._previousVisualFirstIndex, count);
					this.SetItemPosition(j, ref num);
				}
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007A80 File Offset: 0x00006A80
		private void FadeInItemIfNeeded(int index, int visualFirstIndex, int previousVisualFirstIndex, int itemCount)
		{
			if (!this._isDesign && this.RollingIncrement(index) == visualFirstIndex && index == previousVisualFirstIndex)
			{
				if (itemCount > 1 && (itemCount != 2 || this.AnimationDirection != AnimationDirection.Right))
				{
					double num = 0.0;
					for (int num2 = this.RollingIncrement(index); num2 != index; num2 = this.RollingIncrement(num2))
					{
						num += this.GetItemWidth(num2);
					}
					if (num < base.ActualWidth)
					{
						this.FadeIn(index);
						return;
					}
				}
			}
			else
			{
				UIElement uielement = this.GetItemFromIndex(index) as UIElement;
				if (uielement != null)
				{
					uielement.Opacity = 1.0;
				}
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007B10 File Offset: 0x00006B10
		private object GetItemFromIndex(int index)
		{
			if (base.Items.Count > index)
			{
				return base.Items[index];
			}
			return null;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007B2E File Offset: 0x00006B2E
		private int RollingIncrement(int index)
		{
			index++;
			if (index >= base.Items.Count)
			{
				return 0;
			}
			return index;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007B46 File Offset: 0x00006B46
		private int RollingDecrement(int index)
		{
			index--;
			if (index >= 0)
			{
				return index;
			}
			return base.Items.Count - 1;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007B94 File Offset: 0x00006B94
		private void FadeIn(int index)
		{
			Control control = (Control)base.Items[index];
			OpacityAnimator oa;
			if (!this._opacities.TryGetValue(control, ref oa))
			{
				OpacityAnimator.EnsureAnimator(control, ref oa);
				this._opacities[control] = oa;
			}
			if (oa != null)
			{
				oa.GoTo(0.0, Pivot.ZeroDuration, delegate()
				{
					oa.GoTo(1.0, new Duration(TimeSpan.FromSeconds(0.125)));
				});
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007C20 File Offset: 0x00006C20
		private void SetItemPosition(int i, ref double offset)
		{
			Control control = this.GetItemFromIndex(i) as Control;
			if (control != null)
			{
				double num;
				if (!this._sizes.TryGetValue(control, ref num))
				{
					num = 0.0;
				}
				TranslateTransform translateTransform;
				if (!this._translations.TryGetValue(control, ref translateTransform))
				{
					translateTransform = TransformAnimator.GetTranslateTransform(control);
					this._translations[control] = translateTransform;
				}
				translateTransform.X = offset;
				offset += num;
			}
		}

		// Token: 0x040000A9 RID: 169
		private const string CanvasName = "Canvas";

		// Token: 0x040000AA RID: 170
		private const double PivotSeconds = 0.5;

		// Token: 0x040000AB RID: 171
		private bool _isDesign;

		// Token: 0x040000AC RID: 172
		private Canvas _canvas;

		// Token: 0x040000AD RID: 173
		private Dictionary<Control, double> _sizes;

		// Token: 0x040000AE RID: 174
		private Dictionary<Control, TranslateTransform> _translations;

		// Token: 0x040000AF RID: 175
		private Dictionary<Control, OpacityAnimator> _opacities;

		// Token: 0x040000B0 RID: 176
		private Image _leftMirror;

		// Token: 0x040000B1 RID: 177
		private TranslateTransform _leftMirrorTranslation;

		// Token: 0x040000B2 RID: 178
		private TransformAnimator _canvasAnimator;

		// Token: 0x040000B3 RID: 179
		internal readonly IEasingFunction QuarticEase = new QuarticEase();

		// Token: 0x040000B4 RID: 180
		internal bool _cancelClick;

		// Token: 0x040000B5 RID: 181
		private bool _activeSelectionChange;

		// Token: 0x040000B6 RID: 182
		private bool _isAnimating;

		// Token: 0x040000B7 RID: 183
		private Queue<PivotHeadersControl.AnimationInstruction> _queuedAnimations;

		// Token: 0x040000B8 RID: 184
		internal bool _wasClicked;

		// Token: 0x040000B9 RID: 185
		private Pivot _pivot;

		// Token: 0x040000BA RID: 186
		private double _animatingWidth;

		// Token: 0x040000BB RID: 187
		private DateTime _currentItemAnimationStarted;

		// Token: 0x040000BC RID: 188
		internal static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(PivotHeadersControl), new PropertyMetadata(0, new PropertyChangedCallback(PivotHeadersControl.OnSelectedIndexPropertyChanged)));

		// Token: 0x040000BD RID: 189
		public static readonly DependencyProperty VisualFirstIndexProperty = DependencyProperty.Register("VisualFirstIndex", typeof(int), typeof(PivotHeadersControl), new PropertyMetadata(0, new PropertyChangedCallback(PivotHeadersControl.OnVisualFirstIndexPropertyChanged)));

		// Token: 0x040000BE RID: 190
		private bool _ignorePropertyChange;

		// Token: 0x040000BF RID: 191
		private int _previousVisualFirstIndex;

		// Token: 0x02000020 RID: 32
		private class AnimationInstruction
		{
			// Token: 0x0600017B RID: 379 RVA: 0x00007D0D File Offset: 0x00006D0D
			public AnimationInstruction(int previous, int next)
			{
				this._previousIndex = previous;
				this._index = next;
			}

			// Token: 0x040000C2 RID: 194
			public int _previousIndex;

			// Token: 0x040000C3 RID: 195
			public int _index;

			// Token: 0x040000C4 RID: 196
			public IEasingFunction _ease;

			// Token: 0x040000C5 RID: 197
			public double _width;

			// Token: 0x040000C6 RID: 198
			public double _durationInSeconds;
		}
	}
}
