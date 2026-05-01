using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Xna.Framework.Input.Touch;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000069 RID: 105
	public class GestureListener
	{
		// Token: 0x06000407 RID: 1031 RVA: 0x00011A18 File Offset: 0x0000FC18
		static GestureListener()
		{
			Touch.FrameReported += new TouchFrameEventHandler(GestureListener.Touch_FrameReported);
			TouchPanel.EnabledGestures = 999;
			GestureListener._timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(100.0)
			};
			GestureListener._timer.Tick += new EventHandler(GestureListener.OnTimerTick);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00011A78 File Offset: 0x0000FC78
		private static void Touch_FrameReported(object sender, TouchFrameEventArgs e)
		{
			bool flag = false;
			Point position;
			position..ctor(0.0, 0.0);
			foreach (TouchPoint touchPoint in e.GetTouchPoints(null))
			{
				if (touchPoint.Action != 3)
				{
					position = touchPoint.Position;
					flag = true;
					break;
				}
			}
			if (!GestureListener._isInTouch && flag)
			{
				GestureListener._gestureOrigin = position;
				GestureListener.TouchStart();
			}
			else if (GestureListener._isInTouch && !flag)
			{
				GestureListener.TouchComplete();
			}
			else if (GestureListener._isInTouch)
			{
				GestureListener.TouchDelta();
			}
			else
			{
				GestureListener.TouchStart();
			}
			GestureListener._isInTouch = flag;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00011B4C File Offset: 0x0000FD4C
		private static void TouchStart()
		{
			GestureListener._cumulativeDelta.X = (GestureListener._cumulativeDelta.Y = (GestureListener._cumulativeDelta2.X = (GestureListener._cumulativeDelta2.Y = 0.0)));
			GestureListener._finalVelocity.X = (GestureListener._finalVelocity.Y = 0.0);
			GestureListener._isDragging = (GestureListener._flicked = false);
			GestureListener._elements = new List<UIElement>(VisualTreeHelper.FindElementsInHostCoordinates(GestureListener._gestureOrigin, Application.Current.RootVisual));
			GestureListener._gestureOriginChanged = false;
			GestureListener.RaiseGestureEvent<GestureEventArgs>((GestureListener helper) => helper.GestureBegin, () => new GestureEventArgs(GestureListener._gestureOrigin, GestureListener._gestureOrigin), false);
			GestureListener.ProcessTouchPanelEvents();
			GestureListener._timer.Start();
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00011C32 File Offset: 0x0000FE32
		private static void TouchDelta()
		{
			GestureListener.ProcessTouchPanelEvents();
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00011C54 File Offset: 0x0000FE54
		private static void TouchComplete()
		{
			GestureListener.ProcessTouchPanelEvents();
			GestureListener.RaiseGestureEvent<GestureEventArgs>((GestureListener helper) => helper.GestureCompleted, () => new GestureEventArgs(GestureListener._gestureOrigin, GestureListener._lastSamplePosition), false);
			GestureListener._elements = null;
			GestureListener._gestureOrientation = default(Orientation?);
			GestureListener._timer.Stop();
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00011CC1 File Offset: 0x0000FEC1
		private static void OnTimerTick(object sender, EventArgs e)
		{
			GestureListener.ProcessTouchPanelEvents();
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00011E50 File Offset: 0x00010050
		private static void ProcessTouchPanelEvents()
		{
			GestureListener.<>c__DisplayClass2f CS$<>8__locals1 = new GestureListener.<>c__DisplayClass2f();
			CS$<>8__locals1.delta = new Point(0.0, 0.0);
			GeneralTransform generalTransform = null;
			while (TouchPanel.IsGestureAvailable)
			{
				GestureSample gestureSample = TouchPanel.ReadGesture();
				Point samplePosition = gestureSample.Position.ToPoint();
				Point samplePosition2 = gestureSample.Position2.ToPoint();
				Point sampleDelta = gestureSample.Delta.ToPoint();
				GestureListener.GetTranslatedDelta(ref generalTransform, ref sampleDelta, ref GestureListener._cumulativeDelta, gestureSample.GestureType != 128);
				Point point = gestureSample.Delta2.ToPoint();
				GestureListener.GetTranslatedDelta(ref generalTransform, ref point, ref GestureListener._cumulativeDelta2, gestureSample.GestureType != 128);
				if (GestureListener._elements == null || GestureListener._gestureOriginChanged)
				{
					GestureListener._gestureOrigin = samplePosition;
					GestureListener._elements = new List<UIElement>(VisualTreeHelper.FindElementsInHostCoordinates(GestureListener._gestureOrigin, Application.Current.RootVisual));
					GestureListener._gestureOriginChanged = false;
				}
				if (GestureListener._gestureOrientation == null && (sampleDelta.X != 0.0 || sampleDelta.Y != 0.0))
				{
					GestureListener._gestureOrientation = new Orientation?((Math.Abs(sampleDelta.X) >= Math.Abs(sampleDelta.Y)) ? 1 : 0);
				}
				GestureType gestureType = gestureSample.GestureType;
				if (gestureType <= 64)
				{
					switch (gestureType)
					{
					case 1:
						GestureListener.RaiseGestureEvent<GestureEventArgs>((GestureListener helper) => helper.Tap, () => new GestureEventArgs(GestureListener._gestureOrigin, samplePosition), false);
						break;
					case 2:
						GestureListener.RaiseGestureEvent<GestureEventArgs>((GestureListener helper) => helper.DoubleTap, () => new GestureEventArgs(GestureListener._gestureOrigin, samplePosition), false);
						break;
					case 3:
						break;
					case 4:
						GestureListener.RaiseGestureEvent<GestureEventArgs>((GestureListener helper) => helper.Hold, () => new GestureEventArgs(GestureListener._gestureOrigin, samplePosition), false);
						break;
					default:
						if (gestureType != 32)
						{
							if (gestureType == 64)
							{
								if (!GestureListener._isPinching)
								{
									GestureListener._isPinching = true;
									GestureListener._pinchOrigin = samplePosition;
									GestureListener._pinchOrigin2 = samplePosition2;
									GestureListener.RaiseGestureEvent<PinchStartedGestureEventArgs>((GestureListener helper) => helper.PinchStarted, () => new PinchStartedGestureEventArgs(GestureListener._pinchOrigin, GestureListener._pinchOrigin2, GestureListener._pinchOrigin, GestureListener._pinchOrigin2), true);
								}
								GestureListener._lastSamplePosition = samplePosition;
								GestureListener._lastSamplePosition2 = samplePosition2;
								GestureListener.RaiseGestureEvent<PinchGestureEventArgs>((GestureListener helper) => helper.PinchDelta, () => new PinchGestureEventArgs(GestureListener._pinchOrigin, GestureListener._pinchOrigin2, samplePosition, samplePosition2), false);
							}
						}
						else if (sampleDelta.X != 0.0 || sampleDelta.Y != 0.0)
						{
							if (!GestureListener._isDragging)
							{
								GestureListener.RaiseGestureEvent<DragStartedGestureEventArgs>((GestureListener helper) => helper.DragStarted, () => new DragStartedGestureEventArgs(GestureListener._gestureOrigin, GestureListener._gestureOrientation.Value), true);
								GestureListener._isDragging = true;
							}
							GestureListener.<>c__DisplayClass2f CS$<>8__locals3 = CS$<>8__locals1;
							CS$<>8__locals3.delta.X = CS$<>8__locals3.delta.X + sampleDelta.X;
							GestureListener.<>c__DisplayClass2f CS$<>8__locals4 = CS$<>8__locals1;
							CS$<>8__locals4.delta.Y = CS$<>8__locals4.delta.Y + sampleDelta.Y;
							GestureListener._lastSamplePosition = samplePosition;
						}
						break;
					}
				}
				else if (gestureType != 128)
				{
					if (gestureType != 256)
					{
						if (gestureType == 512)
						{
							GestureListener._isPinching = false;
							GestureListener.RaiseGestureEvent<PinchGestureEventArgs>((GestureListener helper) => helper.PinchCompleted, () => new PinchGestureEventArgs(GestureListener._pinchOrigin, GestureListener._pinchOrigin2, GestureListener._lastSamplePosition, GestureListener._lastSamplePosition2), false);
							GestureListener._cumulativeDelta.X = (GestureListener._cumulativeDelta.Y = (GestureListener._cumulativeDelta2.X = (GestureListener._cumulativeDelta2.Y = 0.0)));
							GestureListener._gestureOriginChanged = true;
						}
					}
					else
					{
						if (!GestureListener._flicked && (CS$<>8__locals1.delta.X != 0.0 || CS$<>8__locals1.delta.Y != 0.0))
						{
							GestureListener.RaiseGestureEvent<DragDeltaGestureEventArgs>((GestureListener helper) => helper.DragDelta, () => new DragDeltaGestureEventArgs(GestureListener._gestureOrigin, samplePosition, CS$<>8__locals1.delta, GestureListener._gestureOrientation.Value), false);
							CS$<>8__locals1.delta.X = (CS$<>8__locals1.delta.Y = 0.0);
						}
						if (GestureListener._isDragging)
						{
							GestureListener.RaiseGestureEvent<DragCompletedGestureEventArgs>((GestureListener helper) => helper.DragCompleted, () => new DragCompletedGestureEventArgs(GestureListener._gestureOrigin, GestureListener._lastSamplePosition, GestureListener._cumulativeDelta, GestureListener._gestureOrientation.Value, GestureListener._finalVelocity), false);
							CS$<>8__locals1.delta.X = (CS$<>8__locals1.delta.Y = 0.0);
						}
						GestureListener._cumulativeDelta.X = (GestureListener._cumulativeDelta.Y = 0.0);
						GestureListener._flicked = (GestureListener._isDragging = false);
						GestureListener._gestureOriginChanged = true;
					}
				}
				else
				{
					GestureListener._flicked = true;
					GestureListener._finalVelocity = sampleDelta;
					GestureListener.RaiseGestureEvent<FlickGestureEventArgs>((GestureListener helper) => helper.Flick, () => new FlickGestureEventArgs(GestureListener._gestureOrigin, sampleDelta), true);
				}
			}
			if (!GestureListener._flicked && (CS$<>8__locals1.delta.X != 0.0 || CS$<>8__locals1.delta.Y != 0.0))
			{
				GestureListener.RaiseGestureEvent<DragDeltaGestureEventArgs>((GestureListener helper) => helper.DragDelta, () => new DragDeltaGestureEventArgs(GestureListener._gestureOrigin, GestureListener._lastSamplePosition, CS$<>8__locals1.delta, GestureListener._gestureOrientation.Value), false);
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001251C File Offset: 0x0001071C
		private static void GetTranslatedDelta(ref GeneralTransform deltaTransform, ref Point sampleDelta, ref Point cumulativeDelta, bool addToCumulative)
		{
			if (sampleDelta.X != 0.0 || sampleDelta.Y != 0.0)
			{
				if (deltaTransform == null)
				{
					deltaTransform = GestureListener.GetInverseRootTransformNoOffset();
				}
				sampleDelta = deltaTransform.Transform(sampleDelta);
				if (addToCumulative)
				{
					cumulativeDelta.X += sampleDelta.X;
					cumulativeDelta.Y += sampleDelta.Y;
				}
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00012594 File Offset: 0x00010794
		private static GeneralTransform GetInverseRootTransformNoOffset()
		{
			GeneralTransform inverse = Application.Current.RootVisual.TransformToVisual(null).Inverse;
			MatrixTransform matrixTransform = inverse as MatrixTransform;
			if (matrixTransform != null)
			{
				Matrix matrix = matrixTransform.Matrix;
				matrix.OffsetX = (matrix.OffsetY = 0.0);
				matrixTransform.Matrix = matrix;
			}
			return inverse;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00012630 File Offset: 0x00010830
		private static void RaiseGestureEvent<T>(Func<GestureListener, EventHandler<T>> eventGetter, Func<T> argsGetter, bool releaseMouseCapture) where T : GestureEventArgs
		{
			T args = default(T);
			FrameworkElement originalSource = null;
			bool flag = false;
			foreach (UIElement uielement in GestureListener._elements)
			{
				FrameworkElement frameworkElement = (FrameworkElement)uielement;
				if (releaseMouseCapture)
				{
					frameworkElement.ReleaseMouseCapture();
				}
				if (!flag)
				{
					if (originalSource == null)
					{
						originalSource = frameworkElement;
					}
					GestureListener gestureListenerInternal = GestureService.GetGestureListenerInternal(frameworkElement, false);
					if (gestureListenerInternal != null)
					{
						SafeRaise.Raise<T>(eventGetter.Invoke(gestureListenerInternal), frameworkElement, delegate()
						{
							if (args == null)
							{
								args = argsGetter.Invoke();
								args.OriginalSource = originalSource;
							}
							return args;
						});
					}
					if (args != null && args.Handled)
					{
						flag = true;
					}
				}
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06000411 RID: 1041 RVA: 0x00012718 File Offset: 0x00010918
		// (remove) Token: 0x06000412 RID: 1042 RVA: 0x00012750 File Offset: 0x00010950
		public event EventHandler<GestureEventArgs> GestureBegin;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06000413 RID: 1043 RVA: 0x00012788 File Offset: 0x00010988
		// (remove) Token: 0x06000414 RID: 1044 RVA: 0x000127C0 File Offset: 0x000109C0
		public event EventHandler<GestureEventArgs> GestureCompleted;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06000415 RID: 1045 RVA: 0x000127F8 File Offset: 0x000109F8
		// (remove) Token: 0x06000416 RID: 1046 RVA: 0x00012830 File Offset: 0x00010A30
		public event EventHandler<GestureEventArgs> Tap;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06000417 RID: 1047 RVA: 0x00012868 File Offset: 0x00010A68
		// (remove) Token: 0x06000418 RID: 1048 RVA: 0x000128A0 File Offset: 0x00010AA0
		public event EventHandler<GestureEventArgs> DoubleTap;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06000419 RID: 1049 RVA: 0x000128D8 File Offset: 0x00010AD8
		// (remove) Token: 0x0600041A RID: 1050 RVA: 0x00012910 File Offset: 0x00010B10
		public event EventHandler<GestureEventArgs> Hold;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x0600041B RID: 1051 RVA: 0x00012948 File Offset: 0x00010B48
		// (remove) Token: 0x0600041C RID: 1052 RVA: 0x00012980 File Offset: 0x00010B80
		public event EventHandler<DragStartedGestureEventArgs> DragStarted;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x0600041D RID: 1053 RVA: 0x000129B8 File Offset: 0x00010BB8
		// (remove) Token: 0x0600041E RID: 1054 RVA: 0x000129F0 File Offset: 0x00010BF0
		public event EventHandler<DragDeltaGestureEventArgs> DragDelta;

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x0600041F RID: 1055 RVA: 0x00012A28 File Offset: 0x00010C28
		// (remove) Token: 0x06000420 RID: 1056 RVA: 0x00012A60 File Offset: 0x00010C60
		public event EventHandler<DragCompletedGestureEventArgs> DragCompleted;

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06000421 RID: 1057 RVA: 0x00012A98 File Offset: 0x00010C98
		// (remove) Token: 0x06000422 RID: 1058 RVA: 0x00012AD0 File Offset: 0x00010CD0
		public event EventHandler<FlickGestureEventArgs> Flick;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06000423 RID: 1059 RVA: 0x00012B08 File Offset: 0x00010D08
		// (remove) Token: 0x06000424 RID: 1060 RVA: 0x00012B40 File Offset: 0x00010D40
		public event EventHandler<PinchStartedGestureEventArgs> PinchStarted;

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06000425 RID: 1061 RVA: 0x00012B78 File Offset: 0x00010D78
		// (remove) Token: 0x06000426 RID: 1062 RVA: 0x00012BB0 File Offset: 0x00010DB0
		public event EventHandler<PinchGestureEventArgs> PinchDelta;

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06000427 RID: 1063 RVA: 0x00012BE8 File Offset: 0x00010DE8
		// (remove) Token: 0x06000428 RID: 1064 RVA: 0x00012C20 File Offset: 0x00010E20
		public event EventHandler<PinchGestureEventArgs> PinchCompleted;

		// Token: 0x04000207 RID: 519
		private static DispatcherTimer _timer;

		// Token: 0x04000208 RID: 520
		private static bool _isInTouch;

		// Token: 0x04000209 RID: 521
		private static List<UIElement> _elements;

		// Token: 0x0400020A RID: 522
		private static Point _gestureOrigin;

		// Token: 0x0400020B RID: 523
		private static bool _gestureOriginChanged;

		// Token: 0x0400020C RID: 524
		private static Orientation? _gestureOrientation;

		// Token: 0x0400020D RID: 525
		private static Point _cumulativeDelta;

		// Token: 0x0400020E RID: 526
		private static Point _cumulativeDelta2;

		// Token: 0x0400020F RID: 527
		private static Point _finalVelocity;

		// Token: 0x04000210 RID: 528
		private static Point _pinchOrigin;

		// Token: 0x04000211 RID: 529
		private static Point _pinchOrigin2;

		// Token: 0x04000212 RID: 530
		private static Point _lastSamplePosition;

		// Token: 0x04000213 RID: 531
		private static Point _lastSamplePosition2;

		// Token: 0x04000214 RID: 532
		private static bool _isPinching;

		// Token: 0x04000215 RID: 533
		private static bool _flicked;

		// Token: 0x04000216 RID: 534
		private static bool _isDragging;
	}
}
