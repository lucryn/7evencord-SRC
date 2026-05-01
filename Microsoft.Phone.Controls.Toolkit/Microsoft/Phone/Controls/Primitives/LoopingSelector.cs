using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x02000021 RID: 33
	[TemplatePart(Name = "PanningTransform", Type = typeof(TranslateTransform))]
	[TemplatePart(Name = "ItemsPanel", Type = typeof(Panel))]
	[TemplatePart(Name = "CenteringTransform", Type = typeof(TranslateTransform))]
	public class LoopingSelector : Control
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00004DB1 File Offset: 0x00002FB1
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00004DC4 File Offset: 0x00002FC4
		public ILoopingSelectorDataSource DataSource
		{
			get
			{
				return (ILoopingSelectorDataSource)base.GetValue(LoopingSelector.DataSourceProperty);
			}
			set
			{
				if (this.DataSource != null)
				{
					this.DataSource.SelectionChanged -= new EventHandler<SelectionChangedEventArgs>(this.value_SelectionChanged);
				}
				base.SetValue(LoopingSelector.DataSourceProperty, value);
				if (value != null)
				{
					value.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.value_SelectionChanged);
				}
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004E14 File Offset: 0x00003014
		private void value_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!this.IsReady)
			{
				return;
			}
			if (!this._isSelecting && e.AddedItems.Count == 1)
			{
				object obj = e.AddedItems[0];
				foreach (UIElement uielement in this._itemsPanel.Children)
				{
					LoopingSelectorItem loopingSelectorItem = (LoopingSelectorItem)uielement;
					if (loopingSelectorItem.DataContext == obj)
					{
						this.SelectAndSnapTo(loopingSelectorItem);
						return;
					}
				}
				this.UpdateData();
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004EA8 File Offset: 0x000030A8
		private static void OnDataModelChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			LoopingSelector loopingSelector = (LoopingSelector)obj;
			loopingSelector.UpdateData();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004EC4 File Offset: 0x000030C4
		private void DataModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!this.IsReady)
			{
				return;
			}
			if (!this._isSelecting && e.AddedItems.Count == 1)
			{
				object obj = e.AddedItems[0];
				foreach (UIElement uielement in this._itemsPanel.Children)
				{
					LoopingSelectorItem loopingSelectorItem = (LoopingSelectorItem)uielement;
					if (loopingSelectorItem.DataContext == obj)
					{
						this.SelectAndSnapTo(loopingSelectorItem);
						break;
					}
				}
				this.UpdateData();
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00004F58 File Offset: 0x00003158
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00004F6A File Offset: 0x0000316A
		public DataTemplate ItemTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(LoopingSelector.ItemTemplateProperty);
			}
			set
			{
				base.SetValue(LoopingSelector.ItemTemplateProperty, value);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00004F78 File Offset: 0x00003178
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00004F80 File Offset: 0x00003180
		public Size ItemSize { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00004F89 File Offset: 0x00003189
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00004F91 File Offset: 0x00003191
		public Thickness ItemMargin { get; set; }

		// Token: 0x060000EA RID: 234 RVA: 0x00004F9C File Offset: 0x0000319C
		public LoopingSelector()
		{
			base.DefaultStyleKey = typeof(LoopingSelector);
			this.CreateEventHandlers();
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00005002 File Offset: 0x00003202
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00005014 File Offset: 0x00003214
		public bool IsExpanded
		{
			get
			{
				return (bool)base.GetValue(LoopingSelector.IsExpandedProperty);
			}
			set
			{
				base.SetValue(LoopingSelector.IsExpandedProperty, value);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060000ED RID: 237 RVA: 0x00005028 File Offset: 0x00003228
		// (remove) Token: 0x060000EE RID: 238 RVA: 0x00005060 File Offset: 0x00003260
		public event DependencyPropertyChangedEventHandler IsExpandedChanged;

		// Token: 0x060000EF RID: 239 RVA: 0x00005098 File Offset: 0x00003298
		private static void OnIsExpandedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			LoopingSelector loopingSelector = (LoopingSelector)sender;
			loopingSelector.UpdateItemState();
			if (!loopingSelector.IsExpanded)
			{
				loopingSelector.SelectAndSnapToClosest();
			}
			if (loopingSelector._state == LoopingSelector.State.Normal || loopingSelector._state == LoopingSelector.State.Expanded)
			{
				loopingSelector._state = (loopingSelector.IsExpanded ? LoopingSelector.State.Expanded : LoopingSelector.State.Normal);
			}
			DependencyPropertyChangedEventHandler isExpandedChanged = loopingSelector.IsExpandedChanged;
			if (isExpandedChanged != null)
			{
				isExpandedChanged.Invoke(loopingSelector, e);
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000050F8 File Offset: 0x000032F8
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._itemsPanel = ((base.GetTemplateChild("ItemsPanel") as Panel) ?? new Canvas());
			this._centeringTransform = ((base.GetTemplateChild("CenteringTransform") as TranslateTransform) ?? new TranslateTransform());
			this._panningTransform = ((base.GetTemplateChild("PanningTransform") as TranslateTransform) ?? new TranslateTransform());
			this.CreateVisuals();
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005170 File Offset: 0x00003370
		private void LoopingSelector_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (this._isAnimating)
			{
				double y = this._panningTransform.Y;
				this.StopAnimation();
				this._panningTransform.Y = y;
				this._isAnimating = false;
				this._state = LoopingSelector.State.Dragging;
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000051B1 File Offset: 0x000033B1
		private void LoopingSelector_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (this._selectedItem != sender && this._state == LoopingSelector.State.Dragging && !this._isAnimating)
			{
				this.SelectAndSnapToClosest();
				this._state = LoopingSelector.State.Expanded;
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000051DA File Offset: 0x000033DA
		private void listener_Tap(object sender, GestureEventArgs e)
		{
			if (this._panningTransform != null)
			{
				this.SelectAndSnapToClosest();
				e.Handled = true;
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000051F4 File Offset: 0x000033F4
		private void listener_DragStarted(object sender, DragStartedGestureEventArgs e)
		{
			if (e.Direction == null)
			{
				this._state = LoopingSelector.State.Dragging;
				e.Handled = true;
				this._selectedItem = null;
				if (!this.IsExpanded)
				{
					this.IsExpanded = true;
				}
				this._dragTarget = this._panningTransform.Y;
				this.UpdateItemState();
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005244 File Offset: 0x00003444
		private void listener_DragDelta(object sender, DragDeltaGestureEventArgs e)
		{
			if (e.Direction == null && this._state == LoopingSelector.State.Dragging)
			{
				this.AnimatePanel(LoopingSelector._panDuration, this._panEase, this._dragTarget += e.VerticalChange);
				e.Handled = true;
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005290 File Offset: 0x00003490
		private void listener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
		{
			if (this._state == LoopingSelector.State.Dragging)
			{
				this.SelectAndSnapToClosest();
			}
			this._state = LoopingSelector.State.Expanded;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000052A8 File Offset: 0x000034A8
		private void listener_Flick(object sender, FlickGestureEventArgs e)
		{
			if (e.Direction == null)
			{
				this._state = LoopingSelector.State.Flicking;
				this._selectedItem = null;
				if (!this.IsExpanded)
				{
					this.IsExpanded = true;
				}
				Point initialVelocity;
				initialVelocity..ctor(0.0, e.VerticalVelocity);
				double stopTime = PhysicsConstants.GetStopTime(initialVelocity);
				Point stopPoint = PhysicsConstants.GetStopPoint(initialVelocity);
				IEasingFunction easingFunction = PhysicsConstants.GetEasingFunction(stopTime);
				this.AnimatePanel(new Duration(TimeSpan.FromSeconds(stopTime)), easingFunction, this._panningTransform.Y + stopPoint.Y);
				e.Handled = true;
				this._selectedItem = null;
				this.UpdateItemState();
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005344 File Offset: 0x00003544
		private void LoopingSelector_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this._centeringTransform.Y = Math.Round(e.NewSize.Height / 2.0);
			base.Clip = new RectangleGeometry
			{
				Rect = new Rect(0.0, 0.0, e.NewSize.Width, e.NewSize.Height)
			};
			this.UpdateData();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000053C8 File Offset: 0x000035C8
		private void wrapper_Click(object sender, EventArgs e)
		{
			if (this._state == LoopingSelector.State.Normal)
			{
				this._state = LoopingSelector.State.Expanded;
				this.IsExpanded = true;
				return;
			}
			if (this._state == LoopingSelector.State.Expanded)
			{
				if (!this._isAnimating && sender == this._selectedItem)
				{
					this._state = LoopingSelector.State.Normal;
					this.IsExpanded = false;
					return;
				}
				if (sender != this._selectedItem && !this._isAnimating)
				{
					this.SelectAndSnapTo((LoopingSelectorItem)sender);
				}
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005470 File Offset: 0x00003670
		private void SelectAndSnapTo(LoopingSelectorItem item)
		{
			if (item == null)
			{
				return;
			}
			if (this._selectedItem != null)
			{
				this._selectedItem.SetState(this.IsExpanded ? LoopingSelectorItem.State.Expanded : LoopingSelectorItem.State.Normal, true);
			}
			if (this._selectedItem != item)
			{
				this._selectedItem = item;
				base.Dispatcher.BeginInvoke(delegate()
				{
					this._isSelecting = true;
					this.DataSource.SelectedItem = item.DataContext;
					this._isSelecting = false;
				});
			}
			this._selectedItem.SetState(LoopingSelectorItem.State.Selected, true);
			TranslateTransform transform = item.Transform;
			if (transform != null)
			{
				double num = -transform.Y - Math.Round(item.ActualHeight / 2.0);
				if (this._panningTransform.Y != num)
				{
					this.AnimatePanel(LoopingSelector._selectDuration, this._selectEase, num);
				}
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005554 File Offset: 0x00003754
		private void UpdateData()
		{
			if (!this.IsReady)
			{
				return;
			}
			this._temporaryItemsPool = new Queue<LoopingSelectorItem>(this._itemsPanel.Children.Count);
			foreach (UIElement uielement in this._itemsPanel.Children)
			{
				LoopingSelectorItem loopingSelectorItem = (LoopingSelectorItem)uielement;
				if (loopingSelectorItem.GetState() == LoopingSelectorItem.State.Selected)
				{
					loopingSelectorItem.SetState(LoopingSelectorItem.State.Normal, false);
				}
				this._temporaryItemsPool.Enqueue(loopingSelectorItem);
				loopingSelectorItem.Remove();
			}
			this._itemsPanel.Children.Clear();
			this.StopAnimation();
			this._panningTransform.Y = 0.0;
			this._minimumPanelScroll = -3.4028234663852886E+38;
			this._maximumPanelScroll = 3.4028234663852886E+38;
			this.Balance();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000563C File Offset: 0x0000383C
		private void AnimatePanel(Duration duration, IEasingFunction ease, double to)
		{
			double num = Math.Max(this._minimumPanelScroll, Math.Min(this._maximumPanelScroll, to));
			if (to != num)
			{
				double num2 = Math.Abs(this._panningTransform.Y - to);
				double num3 = Math.Abs(this._panningTransform.Y - num);
				double num4 = num3 / num2;
				duration..ctor(TimeSpan.FromMilliseconds((double)duration.TimeSpan.Milliseconds * num4));
				to = num;
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

		// Token: 0x060000FD RID: 253 RVA: 0x00005732 File Offset: 0x00003932
		private void StopAnimation()
		{
			this._panelStoryboard.Stop();
			CompositionTarget.Rendering -= new EventHandler(this.AnimationPerFrameCallback);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005750 File Offset: 0x00003950
		private void Brake(double newStoppingPoint)
		{
			double num = this._panelAnimation.To.Value - this._panelAnimation.From.Value;
			double num2 = newStoppingPoint - this._panningTransform.Y;
			double num3 = num2 / num;
			Duration duration;
			duration..ctor(TimeSpan.FromMilliseconds((double)this._panelAnimation.Duration.TimeSpan.Milliseconds * num3));
			this.AnimatePanel(duration, this._panelAnimation.EasingFunction, newStoppingPoint);
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000057D8 File Offset: 0x000039D8
		private bool IsReady
		{
			get
			{
				return base.ActualHeight > 0.0 && this.DataSource != null && this._itemsPanel != null;
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005804 File Offset: 0x00003A04
		private void Balance()
		{
			if (!this.IsReady)
			{
				return;
			}
			double actualItemWidth = this.ActualItemWidth;
			double actualItemHeight = this.ActualItemHeight;
			this._additionalItemsCount = (int)Math.Round(base.ActualHeight * 1.5 / actualItemHeight);
			LoopingSelectorItem loopingSelectorItem;
			if (this._itemsPanel.Children.Count == 0)
			{
				loopingSelectorItem = (this._selectedItem = this.CreateAndAddItem(this._itemsPanel, this.DataSource.SelectedItem));
				loopingSelectorItem.Transform.Y = -actualItemHeight / 2.0;
				loopingSelectorItem.Transform.X = (base.ActualWidth - actualItemWidth) / 2.0;
				loopingSelectorItem.SetState(LoopingSelectorItem.State.Selected, false);
			}
			else
			{
				int closestItem = this.GetClosestItem();
				loopingSelectorItem = (LoopingSelectorItem)this._itemsPanel.Children[closestItem];
			}
			int i;
			LoopingSelectorItem loopingSelectorItem2 = LoopingSelector.GetFirstItem(loopingSelectorItem, out i);
			int j;
			LoopingSelectorItem loopingSelectorItem3 = LoopingSelector.GetLastItem(loopingSelectorItem, out j);
			if (i >= j)
			{
				if (i >= this._additionalItemsCount)
				{
					goto IL_209;
				}
			}
			while (i < this._additionalItemsCount)
			{
				object previous = this.DataSource.GetPrevious(loopingSelectorItem2.DataContext);
				if (previous == null)
				{
					this._maximumPanelScroll = -loopingSelectorItem2.Transform.Y - actualItemHeight / 2.0;
					if (this._isAnimating && this._panelAnimation.To.Value > this._maximumPanelScroll)
					{
						this.Brake(this._maximumPanelScroll);
						break;
					}
					break;
				}
				else
				{
					LoopingSelectorItem loopingSelectorItem4;
					if (j > this._additionalItemsCount)
					{
						loopingSelectorItem4 = loopingSelectorItem3;
						loopingSelectorItem3 = loopingSelectorItem3.Previous;
						loopingSelectorItem4.Remove();
						loopingSelectorItem4.Content = (loopingSelectorItem4.DataContext = previous);
					}
					else
					{
						loopingSelectorItem4 = this.CreateAndAddItem(this._itemsPanel, previous);
						loopingSelectorItem4.Transform.X = (base.ActualWidth - actualItemWidth) / 2.0;
					}
					loopingSelectorItem4.Transform.Y = loopingSelectorItem2.Transform.Y - actualItemHeight;
					loopingSelectorItem4.InsertBefore(loopingSelectorItem2);
					loopingSelectorItem2 = loopingSelectorItem4;
					i++;
				}
			}
			IL_209:
			if (j >= i)
			{
				if (j >= this._additionalItemsCount)
				{
					goto IL_336;
				}
			}
			while (j < this._additionalItemsCount)
			{
				object next = this.DataSource.GetNext(loopingSelectorItem3.DataContext);
				if (next == null)
				{
					this._minimumPanelScroll = -loopingSelectorItem3.Transform.Y - actualItemHeight / 2.0;
					if (this._isAnimating && this._panelAnimation.To.Value < this._minimumPanelScroll)
					{
						this.Brake(this._minimumPanelScroll);
						break;
					}
					break;
				}
				else
				{
					LoopingSelectorItem loopingSelectorItem5;
					if (i > this._additionalItemsCount)
					{
						loopingSelectorItem5 = loopingSelectorItem2;
						loopingSelectorItem2 = loopingSelectorItem2.Next;
						loopingSelectorItem5.Remove();
						loopingSelectorItem5.Content = (loopingSelectorItem5.DataContext = next);
					}
					else
					{
						loopingSelectorItem5 = this.CreateAndAddItem(this._itemsPanel, next);
						loopingSelectorItem5.Transform.X = (base.ActualWidth - actualItemWidth) / 2.0;
					}
					loopingSelectorItem5.Transform.Y = loopingSelectorItem3.Transform.Y + actualItemHeight;
					loopingSelectorItem5.InsertAfter(loopingSelectorItem3);
					loopingSelectorItem3 = loopingSelectorItem5;
					j++;
				}
			}
			IL_336:
			this._temporaryItemsPool = null;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005B4E File Offset: 0x00003D4E
		private static LoopingSelectorItem GetFirstItem(LoopingSelectorItem item, out int count)
		{
			count = 0;
			while (item.Previous != null)
			{
				count++;
				item = item.Previous;
			}
			return item;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005B6C File Offset: 0x00003D6C
		private static LoopingSelectorItem GetLastItem(LoopingSelectorItem item, out int count)
		{
			count = 0;
			while (item.Next != null)
			{
				count++;
				item = item.Next;
			}
			return item;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005B8A File Offset: 0x00003D8A
		private void AnimationPerFrameCallback(object sender, EventArgs e)
		{
			this.Balance();
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005B94 File Offset: 0x00003D94
		private int GetClosestItem()
		{
			if (!this.IsReady)
			{
				return -1;
			}
			double actualItemHeight = this.ActualItemHeight;
			int count = this._itemsPanel.Children.Count;
			double y = this._panningTransform.Y;
			double num = actualItemHeight / 2.0;
			int result = -1;
			double num2 = double.MaxValue;
			for (int i = 0; i < count; i++)
			{
				LoopingSelectorItem loopingSelectorItem = (LoopingSelectorItem)this._itemsPanel.Children[i];
				double num3 = Math.Abs(loopingSelectorItem.Transform.Y + num + y);
				if (num3 <= num)
				{
					result = i;
					break;
				}
				if (num2 > num3)
				{
					num2 = num3;
					result = i;
				}
			}
			return result;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005C44 File Offset: 0x00003E44
		private void PanelStoryboardCompleted(object sender, EventArgs e)
		{
			CompositionTarget.Rendering -= new EventHandler(this.AnimationPerFrameCallback);
			this._isAnimating = false;
			if (this._state != LoopingSelector.State.Dragging)
			{
				this.SelectAndSnapToClosest();
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005C70 File Offset: 0x00003E70
		private void SelectAndSnapToClosest()
		{
			if (!this.IsReady)
			{
				return;
			}
			int closestItem = this.GetClosestItem();
			if (closestItem == -1)
			{
				return;
			}
			LoopingSelectorItem item = (LoopingSelectorItem)this._itemsPanel.Children[closestItem];
			this.SelectAndSnapTo(item);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005CB0 File Offset: 0x00003EB0
		private void UpdateItemState()
		{
			if (!this.IsReady)
			{
				return;
			}
			bool isExpanded = this.IsExpanded;
			foreach (UIElement uielement in this._itemsPanel.Children)
			{
				LoopingSelectorItem loopingSelectorItem = (LoopingSelectorItem)uielement;
				if (loopingSelectorItem == this._selectedItem)
				{
					loopingSelectorItem.SetState(LoopingSelectorItem.State.Selected, true);
				}
				else
				{
					loopingSelectorItem.SetState(isExpanded ? LoopingSelectorItem.State.Expanded : LoopingSelectorItem.State.Normal, true);
				}
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00005D34 File Offset: 0x00003F34
		private double ActualItemWidth
		{
			get
			{
				return base.Padding.Left + base.Padding.Right + this.ItemSize.Width;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00005D70 File Offset: 0x00003F70
		private double ActualItemHeight
		{
			get
			{
				return base.Padding.Top + base.Padding.Bottom + this.ItemSize.Height;
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005DAC File Offset: 0x00003FAC
		private void CreateVisuals()
		{
			this._panelAnimation = new DoubleAnimation();
			Storyboard.SetTarget(this._panelAnimation, this._panningTransform);
			Storyboard.SetTargetProperty(this._panelAnimation, new PropertyPath("Y", new object[0]));
			this._panelStoryboard = new Storyboard();
			this._panelStoryboard.Children.Add(this._panelAnimation);
			this._panelStoryboard.Completed += new EventHandler(this.PanelStoryboardCompleted);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005E28 File Offset: 0x00004028
		private void CreateEventHandlers()
		{
			base.SizeChanged += new SizeChangedEventHandler(this.LoopingSelector_SizeChanged);
			GestureListener gestureListener = GestureService.GetGestureListener(this);
			gestureListener.DragStarted += new EventHandler<DragStartedGestureEventArgs>(this.listener_DragStarted);
			gestureListener.DragDelta += new EventHandler<DragDeltaGestureEventArgs>(this.listener_DragDelta);
			gestureListener.DragCompleted += new EventHandler<DragCompletedGestureEventArgs>(this.listener_DragCompleted);
			gestureListener.Flick += new EventHandler<FlickGestureEventArgs>(this.listener_Flick);
			gestureListener.Tap += new EventHandler<GestureEventArgs>(this.listener_Tap);
			base.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.LoopingSelector_MouseLeftButtonDown), true);
			base.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.LoopingSelector_MouseLeftButtonUp), true);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005ED8 File Offset: 0x000040D8
		private LoopingSelectorItem CreateAndAddItem(Panel parent, object content)
		{
			bool flag = this._temporaryItemsPool != null && this._temporaryItemsPool.Count > 0;
			LoopingSelectorItem loopingSelectorItem = flag ? this._temporaryItemsPool.Dequeue() : new LoopingSelectorItem();
			if (!flag)
			{
				loopingSelectorItem.ContentTemplate = this.ItemTemplate;
				loopingSelectorItem.Width = this.ItemSize.Width;
				loopingSelectorItem.Height = this.ItemSize.Height;
				loopingSelectorItem.Padding = this.ItemMargin;
				loopingSelectorItem.Click += new EventHandler<EventArgs>(this.wrapper_Click);
			}
			FrameworkElement frameworkElement = loopingSelectorItem;
			loopingSelectorItem.Content = content;
			frameworkElement.DataContext = content;
			parent.Children.Add(loopingSelectorItem);
			if (!flag)
			{
				loopingSelectorItem.ApplyTemplate();
			}
			return loopingSelectorItem;
		}

		// Token: 0x04000046 RID: 70
		private const string ItemsPanelName = "ItemsPanel";

		// Token: 0x04000047 RID: 71
		private const string CenteringTransformName = "CenteringTransform";

		// Token: 0x04000048 RID: 72
		private const string PanningTransformName = "PanningTransform";

		// Token: 0x04000049 RID: 73
		private static readonly Duration _selectDuration = new Duration(TimeSpan.FromMilliseconds(500.0));

		// Token: 0x0400004A RID: 74
		private readonly IEasingFunction _selectEase = new ExponentialEase
		{
			EasingMode = 2
		};

		// Token: 0x0400004B RID: 75
		private static readonly Duration _panDuration = new Duration(TimeSpan.FromMilliseconds(100.0));

		// Token: 0x0400004C RID: 76
		private readonly IEasingFunction _panEase = new ExponentialEase();

		// Token: 0x0400004D RID: 77
		private DoubleAnimation _panelAnimation;

		// Token: 0x0400004E RID: 78
		private Storyboard _panelStoryboard;

		// Token: 0x0400004F RID: 79
		private Panel _itemsPanel;

		// Token: 0x04000050 RID: 80
		private TranslateTransform _panningTransform;

		// Token: 0x04000051 RID: 81
		private TranslateTransform _centeringTransform;

		// Token: 0x04000052 RID: 82
		private bool _isSelecting;

		// Token: 0x04000053 RID: 83
		private LoopingSelectorItem _selectedItem;

		// Token: 0x04000054 RID: 84
		private Queue<LoopingSelectorItem> _temporaryItemsPool;

		// Token: 0x04000055 RID: 85
		private double _minimumPanelScroll = -3.4028234663852886E+38;

		// Token: 0x04000056 RID: 86
		private double _maximumPanelScroll = 3.4028234663852886E+38;

		// Token: 0x04000057 RID: 87
		private int _additionalItemsCount;

		// Token: 0x04000058 RID: 88
		private bool _isAnimating;

		// Token: 0x04000059 RID: 89
		private LoopingSelector.State _state;

		// Token: 0x0400005A RID: 90
		public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register("DataSource", typeof(ILoopingSelectorDataSource), typeof(LoopingSelector), new PropertyMetadata(null, new PropertyChangedCallback(LoopingSelector.OnDataModelChanged)));

		// Token: 0x0400005B RID: 91
		public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(LoopingSelector), new PropertyMetadata(null));

		// Token: 0x0400005C RID: 92
		public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(LoopingSelector), new PropertyMetadata(false, new PropertyChangedCallback(LoopingSelector.OnIsExpandedChanged)));

		// Token: 0x0400005E RID: 94
		private double _dragTarget;

		// Token: 0x02000022 RID: 34
		private enum State
		{
			// Token: 0x04000062 RID: 98
			Normal,
			// Token: 0x04000063 RID: 99
			Expanded,
			// Token: 0x04000064 RID: 100
			Dragging,
			// Token: 0x04000065 RID: 101
			Snapping,
			// Token: 0x04000066 RID: 102
			Flicking
		}
	}
}
