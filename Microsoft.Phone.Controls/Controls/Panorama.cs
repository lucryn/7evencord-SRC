using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Gestures;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000018 RID: 24
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(PanoramaItem))]
	[TemplatePart(Name = "TitleLayer", Type = typeof(PanningLayer))]
	[TemplatePart(Name = "BackgroundLayer", Type = typeof(PanningLayer))]
	[TemplatePart(Name = "ItemsLayer", Type = typeof(PanningLayer))]
	public class Panorama : TemplatedItemsControl<PanoramaItem>
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000039AE File Offset: 0x000029AE
		// (set) Token: 0x0600009B RID: 155 RVA: 0x000039B6 File Offset: 0x000029B6
		internal PanoramaPanel Panel { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000039BF File Offset: 0x000029BF
		// (set) Token: 0x0600009D RID: 157 RVA: 0x000039C7 File Offset: 0x000029C7
		internal int ItemsWidth { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000039D0 File Offset: 0x000029D0
		// (set) Token: 0x0600009F RID: 159 RVA: 0x000039D8 File Offset: 0x000029D8
		internal int ViewportWidth { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000039E1 File Offset: 0x000029E1
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x000039E9 File Offset: 0x000029E9
		internal int ViewportHeight { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000039F2 File Offset: 0x000029F2
		internal int AdjustedViewportWidth
		{
			get
			{
				return Math.Max(0, this.ViewportWidth - 48);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000A3 RID: 163 RVA: 0x00003A03 File Offset: 0x00002A03
		// (remove) Token: 0x060000A4 RID: 164 RVA: 0x00003A1C File Offset: 0x00002A1C
		public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003A35 File Offset: 0x00002A35
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003A42 File Offset: 0x00002A42
		public object Title
		{
			get
			{
				return base.GetValue(Panorama.TitleProperty);
			}
			set
			{
				base.SetValue(Panorama.TitleProperty, value);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00003A50 File Offset: 0x00002A50
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00003A62 File Offset: 0x00002A62
		public DataTemplate TitleTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(Panorama.TitleTemplateProperty);
			}
			set
			{
				base.SetValue(Panorama.TitleTemplateProperty, value);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003A70 File Offset: 0x00002A70
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00003A82 File Offset: 0x00002A82
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(Panorama.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(Panorama.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003A90 File Offset: 0x00002A90
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00003A9D File Offset: 0x00002A9D
		public object SelectedItem
		{
			get
			{
				return base.GetValue(Panorama.SelectedItemProperty);
			}
			private set
			{
				base.SetValue(Panorama.SelectedItemProperty, value);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003AAB File Offset: 0x00002AAB
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00003ABD File Offset: 0x00002ABD
		public int SelectedIndex
		{
			get
			{
				return (int)base.GetValue(Panorama.SelectedIndexProperty);
			}
			private set
			{
				base.SetValue(Panorama.SelectedIndexProperty, value);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00003AD0 File Offset: 0x00002AD0
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00003ADD File Offset: 0x00002ADD
		public object DefaultItem
		{
			get
			{
				return base.GetValue(Panorama.DefaultItemProperty);
			}
			set
			{
				base.SetValue(Panorama.DefaultItemProperty, value);
				this.OnDefaultItemSet();
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003AF1 File Offset: 0x00002AF1
		internal PanoramaItem GetDefaultItemContainer()
		{
			return base.GetContainer(this.DefaultItem);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003B24 File Offset: 0x00002B24
		public Panorama()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.Panorama);
			base.DefaultStyleKey = typeof(Panorama);
			GestureHelper gestureHelper = GestureHelper.Create(this, true);
			gestureHelper.GestureStart += delegate(object sender, GestureEventArgs args)
			{
				this.GestureStart(args);
			};
			gestureHelper.HorizontalDrag += delegate(object sender, DragEventArgs args)
			{
				this.HorizontalDrag(args);
			};
			gestureHelper.Flick += delegate(object sender, FlickEventArgs args)
			{
				this.Flick(args);
			};
			gestureHelper.GestureEnd += delegate(object sender, EventArgs args)
			{
				this.GestureEnd();
			};
			base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
			if (DesignerProperties.IsInDesignTool)
			{
				base.Loaded += new RoutedEventHandler(this.OnLoaded);
				base.Unloaded += new RoutedEventHandler(this.OnUnloaded);
			}
			else
			{
				CompositionTarget.Rendering += new EventHandler(this.EntranceAnimationCallback);
			}
			base.Loaded += new RoutedEventHandler(this.Panorama_Loaded);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003C2F File Offset: 0x00002C2F
		private void Panorama_Loaded(object sender, RoutedEventArgs e)
		{
			base.Loaded -= new RoutedEventHandler(this.Panorama_Loaded);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.Panorama);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003C52 File Offset: 0x00002C52
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			this._loaded = true;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003C5B File Offset: 0x00002C5B
		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			this._loaded = false;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003C64 File Offset: 0x00002C64
		public override void OnApplyTemplate()
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_APPLYTEMPLATE, PerfLog.Panorama);
			base.OnApplyTemplate();
			this._panningBackground = (base.GetTemplateChild("BackgroundLayer") as PanningLayer);
			this._panningTitle = (base.GetTemplateChild("TitleLayer") as PanningLayer);
			this._panningItems = (base.GetTemplateChild("ItemsLayer") as PanningLayer);
			if (this._panningBackground != null)
			{
				this._panningBackground.Owner = this;
			}
			if (this._panningTitle != null)
			{
				this._panningTitle.Owner = this;
			}
			if (this._panningItems != null)
			{
				this._panningItems.Owner = this;
			}
			Binding binding = new Binding("Background");
			binding.RelativeSource = new RelativeSource(2);
			base.SetBinding(Panorama.BackgroundShadowProperty, binding);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_APPLYTEMPLATE, PerfLog.Panorama);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003D38 File Offset: 0x00002D38
		protected override Size MeasureOverride(Size availableSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.Panorama);
			if (Application.Current.Host.Content.ActualWidth > 0.0)
			{
				this.ViewportWidth = (int)((!double.IsInfinity(availableSize.Width)) ? availableSize.Width : Application.Current.Host.Content.ActualWidth);
				this.ViewportHeight = (int)((!double.IsInfinity(availableSize.Height)) ? availableSize.Height : Application.Current.Host.Content.ActualHeight);
			}
			else
			{
				this.ViewportWidth = (int)Math.Min(availableSize.Width, 480.0);
				this.ViewportHeight = (int)Math.Min(availableSize.Height, 800.0);
			}
			base.MeasureOverride(new Size(double.PositiveInfinity, (double)this.ViewportHeight));
			if (double.IsInfinity(availableSize.Width))
			{
				availableSize.Width = (double)this.ViewportWidth;
			}
			if (double.IsInfinity(availableSize.Height))
			{
				availableSize.Height = (double)this.ViewportHeight;
			}
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.Panorama);
			return availableSize;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003E75 File Offset: 0x00002E75
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (this.Panel != null)
			{
				this.Panel.ResetItemPositions();
			}
			this.RequestAdjustSelection();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003E97 File Offset: 0x00002E97
		internal void RequestAdjustSelection()
		{
			if (!this._adjustSelectedRequested)
			{
				base.LayoutUpdated += new EventHandler(this.LayoutUpdatedAdjustSelection);
				this._adjustSelectedRequested = true;
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003EBA File Offset: 0x00002EBA
		private void LayoutUpdatedAdjustSelection(object sender, EventArgs e)
		{
			this._adjustSelectedRequested = false;
			base.LayoutUpdated -= new EventHandler(this.LayoutUpdatedAdjustSelection);
			this.AdjustSelection();
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003EDC File Offset: 0x00002EDC
		private void AdjustSelection()
		{
			if (!DesignerProperties.IsInDesignTool)
			{
				object selectedItem = this.SelectedItem;
				object obj = null;
				bool flag = false;
				bool flag2 = false;
				if (this.Panel != null && this.Panel.VisibleChildren.Count > 0)
				{
					if (selectedItem == null)
					{
						obj = base.GetItem(this.Panel.VisibleChildren[0]);
					}
					else
					{
						PanoramaItem container = base.GetContainer(selectedItem);
						flag2 = this._entranceAnimationPlayed;
						if (container == null || !this.Panel.VisibleChildren.Contains(container))
						{
							obj = base.GetItem(this.Panel.VisibleChildren[0]);
						}
						else
						{
							obj = selectedItem;
						}
					}
				}
				else
				{
					this._targetOffset = 0;
					this.GoTo(this._targetOffset, Panorama.Immediately);
				}
				if (flag)
				{
					this.SelectedItem = obj;
				}
				else
				{
					this.SetSelectionInternal(obj);
				}
				this.UpdateItemPositions();
				if (flag2)
				{
					PanoramaItem container2 = base.GetContainer(obj);
					if (container2 != null)
					{
						this._targetOffset = -container2.StartPosition;
						this.GoTo(this._targetOffset, Panorama.Immediately);
					}
				}
				return;
			}
			if (!this._loaded)
			{
				return;
			}
			this._targetOffset = 0;
			this.GoTo(this._targetOffset, Panorama.Immediately);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004000 File Offset: 0x00003000
		private void UpdateItemPositions()
		{
			bool flag = true;
			if (this.Panel != null)
			{
				if (this.Panel.VisibleChildren.Count > 2 && this.SelectedItem != null)
				{
					PanoramaItem container = base.GetContainer(this.SelectedItem);
					if (container != null)
					{
						int num = this.Panel.VisibleChildren.IndexOf(container);
						if (num == this.Panel.VisibleChildren.Count - 1)
						{
							this.Panel.ShowFirstItemOnRight();
							flag = false;
						}
						else if (num == 0)
						{
							this.Panel.ShowLastItemOnLeft();
							flag = false;
						}
					}
				}
				if (flag)
				{
					this.Panel.ResetItemPositions();
				}
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004098 File Offset: 0x00003098
		protected override Size ArrangeOverride(Size finalSize)
		{
			PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.Panorama);
			Size size = finalSize;
			size.Width = base.DesiredSize.Width;
			base.ArrangeOverride(finalSize);
			PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.Panorama);
			return finalSize;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000040E4 File Offset: 0x000030E4
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			PanoramaItem panoramaItem = element as PanoramaItem;
			if (panoramaItem != null)
			{
				if (panoramaItem.Content == null && panoramaItem != item)
				{
					panoramaItem.Content = item;
				}
				if (panoramaItem.HeaderTemplate == null && element.ReadLocalValue(PanoramaItem.HeaderTemplateProperty) == DependencyProperty.UnsetValue)
				{
					panoramaItem.HeaderTemplate = this.HeaderTemplate;
				}
				if (panoramaItem.Header == null && !(item is UIElement) && panoramaItem.ReadLocalValue(PanoramaItem.HeaderProperty) == DependencyProperty.UnsetValue)
				{
					panoramaItem.Header = item;
				}
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004165 File Offset: 0x00003165
		private void GestureStart(GestureEventArgs args)
		{
			this._targetOffset = (int)this._panningItems.ActualOffset;
			this._flickDirection = 0;
			this._cumulativeDragDelta = 0;
			this._dragged = false;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004190 File Offset: 0x00003190
		private void HorizontalDrag(DragEventArgs args)
		{
			if (this._flickDirection == 0)
			{
				this._cumulativeDragDelta = (int)args.CumulativeDistance.X;
				this._targetOffset += (int)args.DeltaDistance.X;
				if (Math.Abs(this._cumulativeDragDelta) > this.ViewportWidth)
				{
					return;
				}
				this._dragged = true;
				this.GoTo(this._targetOffset, Panorama.PanDuration);
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004202 File Offset: 0x00003202
		private void Flick(FlickEventArgs e)
		{
			if (e.Angle == 180.0)
			{
				this._flickDirection = -1;
				return;
			}
			if (e.Angle == 0.0)
			{
				this._flickDirection = 1;
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004244 File Offset: 0x00003244
		private void GestureEnd()
		{
			if (this._flickDirection != 0)
			{
				this.ProcessFlick();
				return;
			}
			if (!this._dragged)
			{
				return;
			}
			int offset;
			int num;
			PanoramaItem container;
			bool flag;
			this.Panel.GetSnapOffset(this._targetOffset, this.ViewportWidth, Math.Sign(this._cumulativeDragDelta), out offset, out num, out container, out flag);
			if (flag)
			{
				this.WrapAround(Math.Sign(this._cumulativeDragDelta));
			}
			object item = base.GetItem(container);
			if (item != null)
			{
				this.SelectedItem = item;
			}
			this.UpdateItemPositions();
			this.GoTo(offset, Panorama.SnapDuration, delegate()
			{
				this._panningItems.Refresh();
			});
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000042F8 File Offset: 0x000032F8
		private void ProcessFlick()
		{
			if (this._flickDirection == 0)
			{
				return;
			}
			int offset = (int)this._panningItems.ActualOffset;
			PanoramaPanel.ItemStop itemStop;
			PanoramaPanel.ItemStop itemStop2;
			PanoramaPanel.ItemStop itemStop3;
			this.Panel.GetStops(offset, this.ItemsWidth, out itemStop, out itemStop2, out itemStop3);
			if (itemStop == itemStop2 && itemStop2 == itemStop3 && itemStop3 == null)
			{
				return;
			}
			this._targetOffset = ((this._flickDirection < 0) ? (-itemStop3.Position) : (-itemStop.Position));
			bool flag = Math.Sign((double)this._targetOffset - this._panningItems.ActualOffset) != Math.Sign(this._flickDirection);
			if (flag)
			{
				this.WrapAround(Math.Sign(this._flickDirection));
			}
			this.SelectedItem = base.GetItem((this._flickDirection < 0) ? itemStop3.Item : itemStop.Item);
			this.UpdateItemPositions();
			this.GoTo(this._targetOffset, Panorama.FlickDuration, delegate()
			{
				this._panningItems.Refresh();
			});
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000043E4 File Offset: 0x000033E4
		private void GoTo(int offset, Duration duration, Action completionAction)
		{
			if (this._panningBackground != null)
			{
				this._panningBackground.GoTo(offset, duration, null);
			}
			if (this._panningTitle != null)
			{
				this._panningTitle.GoTo(offset, duration, null);
			}
			if (this._panningItems != null)
			{
				this._panningItems.GoTo(offset, duration, completionAction);
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004433 File Offset: 0x00003433
		private void GoTo(int offset)
		{
			this.GoTo(offset, null);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004440 File Offset: 0x00003440
		private void GoTo(int offset, Action completionAction)
		{
			int num = Math.Abs((int)this._panningItems.ActualOffset - offset);
			this.GoTo(offset, TimeSpan.FromMilliseconds((double)(num * 2)), completionAction);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004477 File Offset: 0x00003477
		private void GoTo(int offset, Duration duration)
		{
			this.GoTo(offset, duration, null);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004482 File Offset: 0x00003482
		private void WrapAround(int direction)
		{
			this._panningBackground.Wraparound(direction);
			this._panningTitle.Wraparound(direction);
			this._panningItems.Wraparound(direction);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000044A8 File Offset: 0x000034A8
		private void SetSelectionInternal(object selectedItem)
		{
			this._suppressSelectionChangedEvent = true;
			this.SelectedItem = selectedItem;
			this._suppressSelectionChangedEvent = false;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004530 File Offset: 0x00003530
		private static void OnSelectionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			Panorama panorama = obj as Panorama;
			if (panorama != null)
			{
				panorama.SelectedIndex = panorama.Items.IndexOf(args.NewValue);
				if (!panorama._suppressSelectionChangedEvent && panorama.Items.Contains(args.NewValue))
				{
					SafeRaise.Raise<SelectionChangedEventArgs>(panorama.SelectionChanged, panorama, () => new SelectionChangedEventArgs((args.OldValue == null) ? new object[0] : new object[]
					{
						args.OldValue
					}, (args.NewValue == null) ? new object[0] : new object[]
					{
						args.NewValue
					}));
				}
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000045AF File Offset: 0x000035AF
		private static void OnDefaultItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			((Panorama)obj).OnDefaultItemSet();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000045BC File Offset: 0x000035BC
		private void OnDefaultItemSet()
		{
			if (this.Panel != null)
			{
				this.Panel.NotifyDefaultItemChanged();
				if (this.Panel.VisibleChildren.Count > 0)
				{
					this.SelectedItem = this.DefaultItem;
				}
				if (this.Panel != null)
				{
					this.Panel.ResetItemPositions();
				}
				this._panningItems.Refresh();
				this.UpdateItemPositions();
				this.GoTo(0, Panorama.Immediately);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000462C File Offset: 0x0000362C
		private static void OnBackgroundShadowChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			Panorama panorama = (Panorama)obj;
			if (!panorama._updateBackgroundPending)
			{
				panorama.UpdateBackground();
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000466C File Offset: 0x0000366C
		private void UpdateBackground()
		{
			this._updateBackgroundPending = false;
			this._panningBackground.ContentPresenter.Height = (double)this.ViewportHeight;
			if (base.Background is SolidColorBrush)
			{
				this._panningBackground.ContentPresenter.Width = (double)this.ViewportWidth;
				this._panningBackground.IsStatic = true;
				return;
			}
			if (base.Background is GradientBrush)
			{
				this._panningBackground.ContentPresenter.Width = (double)Math.Max(this.ItemsWidth, this.ViewportWidth);
				this._panningBackground.IsStatic = (this._panningBackground.ContentPresenter.Width == (double)this.ViewportWidth);
				return;
			}
			if (base.Background is ImageBrush)
			{
				ImageBrush imageBrush = (ImageBrush)base.Background;
				BitmapImage bmp = imageBrush.ImageSource as BitmapImage;
				if (this._panningBackground.ContentPresenter != null && bmp != null)
				{
					if (!string.IsNullOrEmpty(bmp.UriSource.OriginalString))
					{
						if (bmp.PixelWidth == 0)
						{
							bmp.ImageOpened -= new EventHandler<RoutedEventArgs>(this.OnBackgroundImageOpened);
							bmp.ImageOpened += new EventHandler<RoutedEventArgs>(this.OnBackgroundImageOpened);
							base.Dispatcher.BeginInvoke(delegate()
							{
								this.AsyncUpdateBackground(bmp);
							});
						}
						this._panningBackground.ContentPresenter.Width = (double)bmp.PixelWidth;
						if (this.previousBackgroundWidth == (float)bmp.PixelWidth || bmp.PixelHeight >= this.ViewportHeight)
						{
							this._panningBackground.Refresh();
						}
						this.previousBackgroundWidth = (float)bmp.PixelWidth;
					}
					else
					{
						this._panningBackground.Refresh();
					}
				}
				this._panningBackground.IsStatic = false;
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000485E File Offset: 0x0000385E
		private void OnBackgroundImageOpened(object sender, RoutedEventArgs e)
		{
			this.AsyncUpdateBackground((BitmapImage)sender);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000486C File Offset: 0x0000386C
		private void AsyncUpdateBackground(BitmapImage img)
		{
			img.ImageOpened -= new EventHandler<RoutedEventArgs>(this.OnBackgroundImageOpened);
			this.UpdateBackground();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004888 File Offset: 0x00003888
		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.ViewportWidth = (int)e.NewSize.Width;
			this.ViewportHeight = (int)e.NewSize.Height;
			this.ItemsWidth = (int)this.Panel.ActualWidth;
			this.UpdateBackground();
			base.Clip = new RectangleGeometry
			{
				Rect = new Rect(0.0, 0.0, e.NewSize.Width, e.NewSize.Height)
			};
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004920 File Offset: 0x00003920
		private void EntranceAnimationCallback(object sender, EventArgs e)
		{
			switch (this._frameCount++)
			{
			case 0:
				this.GoTo(this.ViewportWidth, Panorama.Immediately);
				return;
			case 1:
				this.GoTo(0, Panorama.EntranceDuration);
				this._entranceAnimationPlayed = true;
				CompositionTarget.Rendering -= new EventHandler(this.EntranceAnimationCallback);
				return;
			default:
				return;
			}
		}

		// Token: 0x04000049 RID: 73
		internal const int Spacing = 48;

		// Token: 0x0400004A RID: 74
		internal const double PanningOpacity = 0.7;

		// Token: 0x0400004B RID: 75
		private const string BackgroundLayerElement = "BackgroundLayer";

		// Token: 0x0400004C RID: 76
		private const string TitleLayerElement = "TitleLayer";

		// Token: 0x0400004D RID: 77
		private const string ItemsLayerElement = "ItemsLayer";

		// Token: 0x0400004E RID: 78
		internal static readonly Duration Immediately = new Duration(TimeSpan.Zero);

		// Token: 0x0400004F RID: 79
		private static readonly Duration DefaultDuration = new Duration(TimeSpan.FromMilliseconds(800.0));

		// Token: 0x04000050 RID: 80
		private static readonly Duration EntranceDuration = Panorama.DefaultDuration;

		// Token: 0x04000051 RID: 81
		private static readonly Duration FlickDuration = Panorama.DefaultDuration;

		// Token: 0x04000052 RID: 82
		private static readonly Duration SnapDuration = Panorama.DefaultDuration;

		// Token: 0x04000053 RID: 83
		private static readonly Duration PanDuration = new Duration(TimeSpan.FromMilliseconds(150.0));

		// Token: 0x04000054 RID: 84
		private int _cumulativeDragDelta;

		// Token: 0x04000055 RID: 85
		private int _flickDirection;

		// Token: 0x04000056 RID: 86
		private int _targetOffset;

		// Token: 0x04000057 RID: 87
		private bool _dragged;

		// Token: 0x04000058 RID: 88
		private int _frameCount;

		// Token: 0x04000059 RID: 89
		private bool _updateBackgroundPending = true;

		// Token: 0x0400005A RID: 90
		private bool _entranceAnimationPlayed;

		// Token: 0x0400005B RID: 91
		private PanningLayer _panningBackground;

		// Token: 0x0400005C RID: 92
		private PanningLayer _panningTitle;

		// Token: 0x0400005D RID: 93
		private PanningLayer _panningItems;

		// Token: 0x0400005E RID: 94
		private bool _adjustSelectedRequested;

		// Token: 0x0400005F RID: 95
		private bool _suppressSelectionChangedEvent;

		// Token: 0x04000060 RID: 96
		private bool _loaded;

		// Token: 0x04000061 RID: 97
		private float previousBackgroundWidth;

		// Token: 0x04000063 RID: 99
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(object), typeof(Panorama), null);

		// Token: 0x04000064 RID: 100
		public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(Panorama), null);

		// Token: 0x04000065 RID: 101
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Panorama), null);

		// Token: 0x04000066 RID: 102
		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(Panorama), new PropertyMetadata(null, new PropertyChangedCallback(Panorama.OnSelectionChanged)));

		// Token: 0x04000067 RID: 103
		public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Panorama), new PropertyMetadata(-1));

		// Token: 0x04000068 RID: 104
		public static readonly DependencyProperty DefaultItemProperty = DependencyProperty.Register("DefaultItem", typeof(object), typeof(Panorama), new PropertyMetadata(null, new PropertyChangedCallback(Panorama.OnDefaultItemChanged)));

		// Token: 0x04000069 RID: 105
		private static readonly DependencyProperty BackgroundShadowProperty = DependencyProperty.Register("BackgroundShadow", typeof(Brush), typeof(Panorama), new PropertyMetadata(null, new PropertyChangedCallback(Panorama.OnBackgroundShadowChanged)));
	}
}
