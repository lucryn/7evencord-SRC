using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;

namespace Microsoft.Phone.Gestures
{
	// Token: 0x0200000A RID: 10
	internal abstract class GestureHelper
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000023EF File Offset: 0x000013EF
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000023F7 File Offset: 0x000013F7
		private protected bool ShouldHandleAllDrags { protected get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002400 File Offset: 0x00001400
		// (set) Token: 0x06000025 RID: 37 RVA: 0x00002408 File Offset: 0x00001408
		private protected UIElement Target { protected get; private set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000026 RID: 38 RVA: 0x00002411 File Offset: 0x00001411
		// (remove) Token: 0x06000027 RID: 39 RVA: 0x0000242A File Offset: 0x0000142A
		public event EventHandler<GestureEventArgs> GestureStart;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000028 RID: 40 RVA: 0x00002443 File Offset: 0x00001443
		// (remove) Token: 0x06000029 RID: 41 RVA: 0x0000245C File Offset: 0x0000145C
		public event EventHandler<FlickEventArgs> Flick;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600002A RID: 42 RVA: 0x00002475 File Offset: 0x00001475
		// (remove) Token: 0x0600002B RID: 43 RVA: 0x0000248E File Offset: 0x0000148E
		public event EventHandler<EventArgs> GestureEnd;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600002C RID: 44 RVA: 0x000024A7 File Offset: 0x000014A7
		// (remove) Token: 0x0600002D RID: 45 RVA: 0x000024C0 File Offset: 0x000014C0
		public event EventHandler<DragEventArgs> HorizontalDrag;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600002E RID: 46 RVA: 0x000024D9 File Offset: 0x000014D9
		// (remove) Token: 0x0600002F RID: 47 RVA: 0x000024F2 File Offset: 0x000014F2
		public event EventHandler<DragEventArgs> VerticalDrag;

		// Token: 0x06000030 RID: 48 RVA: 0x0000250B File Offset: 0x0000150B
		public static GestureHelper Create(UIElement target)
		{
			return GestureHelper.Create(target, true);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002514 File Offset: 0x00001514
		public static GestureHelper Create(UIElement target, bool shouldHandleAllDrags)
		{
			GestureHelper gestureHelper = new ManipulationGestureHelper(target, shouldHandleAllDrags);
			gestureHelper.Start();
			return gestureHelper;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002530 File Offset: 0x00001530
		protected GestureHelper(UIElement target, bool shouldHandleAllDrags)
		{
			this.Target = target;
			this.ShouldHandleAllDrags = shouldHandleAllDrags;
		}

		// Token: 0x06000033 RID: 51
		protected abstract void HookEvents();

		// Token: 0x06000034 RID: 52 RVA: 0x00002563 File Offset: 0x00001563
		public void Start()
		{
			this.HookEvents();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000256C File Offset: 0x0000156C
		protected void NotifyDown(InputBaseArgs args)
		{
			GestureEventArgs args2 = new GestureEventArgs();
			this._gestureSource = new WeakReference(args.Source);
			this._gestureOrigin = args.Origin;
			this._dragLock = GestureHelper.DragLock.Unset;
			this._dragging = false;
			this.RaiseGestureStart(args2);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000025B4 File Offset: 0x000015B4
		protected void NotifyMove(InputDeltaArgs args)
		{
			if (Math.Abs(args.CumulativeTranslation.X) > this.DeadZoneInPixels.Width || Math.Abs(args.CumulativeTranslation.Y) > this.DeadZoneInPixels.Height)
			{
				if (!this._dragging)
				{
					this.ReleaseMouseCaptureAtGestureOrigin();
				}
				this._dragging = true;
				if (this._dragLock == GestureHelper.DragLock.Unset)
				{
					double num = GestureHelper.AngleFromVector(args.CumulativeTranslation.X, args.CumulativeTranslation.Y) % 180.0;
					if (num <= 45.0 || num >= 135.0)
					{
						this._dragLock = GestureHelper.DragLock.Horizontal;
					}
					else if (num > 45.0 && num < 135.0)
					{
						this._dragLock = GestureHelper.DragLock.Vertical;
					}
					else
					{
						this._dragLock = GestureHelper.DragLock.Free;
					}
				}
			}
			if (!this._dragging)
			{
				return;
			}
			this.RaiseDragEvents(args);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000026B0 File Offset: 0x000016B0
		private void ReleaseMouseCaptureAtGestureOrigin()
		{
			if (this._gestureSource == null)
			{
				return;
			}
			FrameworkElement frameworkElement = this._gestureSource.Target as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			GeneralTransform generalTransform = frameworkElement.TransformToVisual(null);
			Point point = generalTransform.Transform(this._gestureOrigin);
			foreach (UIElement uielement in VisualTreeHelper.FindElementsInHostCoordinates(point, Application.Current.RootVisual))
			{
				uielement.ReleaseMouseCapture();
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002740 File Offset: 0x00001740
		protected void NotifyUp(InputCompletedArgs args)
		{
			EventArgs args2 = EventArgs.Empty;
			this._dragLock = GestureHelper.DragLock.Unset;
			this._dragging = false;
			if (args.IsInertial)
			{
				double num = GestureHelper.AngleFromVector(args.FinalLinearVelocity.X, args.FinalLinearVelocity.Y);
				if (num <= 45.0 || num >= 315.0)
				{
					num = 0.0;
				}
				else if (num >= 135.0 && num <= 225.0)
				{
					num = 180.0;
				}
				FlickEventArgs args3 = new FlickEventArgs
				{
					Angle = num
				};
				this.ReleaseMouseCaptureAtGestureOrigin();
				this.RaiseFlick(args3);
			}
			else if (args.TotalTranslation.X != 0.0 || args.TotalTranslation.Y != 0.0)
			{
				DragEventArgs dragEventArgs = new DragEventArgs
				{
					CumulativeDistance = args.TotalTranslation
				};
				dragEventArgs.MarkAsFinalTouchManipulation();
				args2 = dragEventArgs;
			}
			this.RaiseGestureEnd(args2);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002851 File Offset: 0x00001851
		private void RaiseGestureStart(GestureEventArgs args)
		{
			SafeRaise.Raise<GestureEventArgs>(this.GestureStart, this, args);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002860 File Offset: 0x00001860
		private void RaiseFlick(FlickEventArgs args)
		{
			SafeRaise.Raise<FlickEventArgs>(this.Flick, this, args);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000286F File Offset: 0x0000186F
		private void RaiseGestureEnd(EventArgs args)
		{
			SafeRaise.Raise<EventArgs>(this.GestureEnd, this, args);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002880 File Offset: 0x00001880
		private void RaiseDragEvents(InputDeltaArgs args)
		{
			DragEventArgs args2 = new DragEventArgs(args);
			if (args.DeltaTranslation.X != 0.0 && this._dragLock == GestureHelper.DragLock.Horizontal)
			{
				this.RaiseHorizontalDrag(args2);
				return;
			}
			if (args.DeltaTranslation.Y != 0.0 && this._dragLock == GestureHelper.DragLock.Vertical)
			{
				this.RaiseVerticalDrag(args2);
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000028E7 File Offset: 0x000018E7
		private void RaiseHorizontalDrag(DragEventArgs args)
		{
			SafeRaise.Raise<DragEventArgs>(this.HorizontalDrag, this, args);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000028F6 File Offset: 0x000018F6
		private void RaiseVerticalDrag(DragEventArgs args)
		{
			SafeRaise.Raise<DragEventArgs>(this.VerticalDrag, this, args);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002908 File Offset: 0x00001908
		private static double AngleFromVector(double x, double y)
		{
			double num = Math.Atan2(y, x);
			if (num < 0.0)
			{
				num = 6.283185307179586 + num;
			}
			return num * 360.0 / 6.283185307179586;
		}

		// Token: 0x0400001B RID: 27
		private GestureHelper.DragLock _dragLock;

		// Token: 0x0400001C RID: 28
		private bool _dragging;

		// Token: 0x0400001D RID: 29
		private WeakReference _gestureSource;

		// Token: 0x0400001E RID: 30
		private Point _gestureOrigin;

		// Token: 0x0400001F RID: 31
		private readonly Size DeadZoneInPixels = new Size(12.0, 12.0);

		// Token: 0x0200000B RID: 11
		private enum DragLock
		{
			// Token: 0x04000028 RID: 40
			Unset,
			// Token: 0x04000029 RID: 41
			Free,
			// Token: 0x0400002A RID: 42
			Vertical,
			// Token: 0x0400002B RID: 43
			Horizontal
		}
	}
}
