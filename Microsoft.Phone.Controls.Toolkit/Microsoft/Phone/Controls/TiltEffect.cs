using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200002D RID: 45
	public class TiltEffect : DependencyObject
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00007255 File Offset: 0x00005455
		// (set) Token: 0x06000168 RID: 360 RVA: 0x0000725C File Offset: 0x0000545C
		public static bool UseLogarithmicEase { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00007264 File Offset: 0x00005464
		// (set) Token: 0x0600016A RID: 362 RVA: 0x0000726B File Offset: 0x0000546B
		public static List<Type> TiltableItems { get; private set; }

		// Token: 0x0600016B RID: 363 RVA: 0x00007273 File Offset: 0x00005473
		private TiltEffect()
		{
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000727C File Offset: 0x0000547C
		static TiltEffect()
		{
			List<Type> list = new List<Type>();
			list.Add(typeof(ButtonBase));
			list.Add(typeof(ListBoxItem));
			TiltEffect.TiltableItems = list;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007343 File Offset: 0x00005543
		public static bool GetIsTiltEnabled(DependencyObject source)
		{
			return (bool)source.GetValue(TiltEffect.IsTiltEnabledProperty);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007355 File Offset: 0x00005555
		public static void SetIsTiltEnabled(DependencyObject source, bool value)
		{
			source.SetValue(TiltEffect.IsTiltEnabledProperty, value);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007368 File Offset: 0x00005568
		public static bool GetSuppressTilt(DependencyObject source)
		{
			return (bool)source.GetValue(TiltEffect.SuppressTiltProperty);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000737A File Offset: 0x0000557A
		public static void SetSuppressTilt(DependencyObject source, bool value)
		{
			source.SetValue(TiltEffect.SuppressTiltProperty, value);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007390 File Offset: 0x00005590
		private static void OnIsTiltEnabledChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
		{
			FrameworkElement frameworkElement = target as FrameworkElement;
			if (frameworkElement != null)
			{
				if ((bool)args.NewValue)
				{
					frameworkElement.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(TiltEffect.TiltEffect_ManipulationStarted);
					return;
				}
				frameworkElement.ManipulationStarted -= new EventHandler<ManipulationStartedEventArgs>(TiltEffect.TiltEffect_ManipulationStarted);
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000073DA File Offset: 0x000055DA
		private static void TiltEffect_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
		{
			TiltEffect.TryStartTiltEffect(sender as FrameworkElement, e);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000073E8 File Offset: 0x000055E8
		private static void TiltEffect_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			TiltEffect.ContinueTiltEffect(sender as FrameworkElement, e);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000073F6 File Offset: 0x000055F6
		private static void TiltEffect_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			TiltEffect.EndTiltEffect(TiltEffect.currentTiltElement);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007404 File Offset: 0x00005604
		private static void TryStartTiltEffect(FrameworkElement source, ManipulationStartedEventArgs e)
		{
			foreach (FrameworkElement frameworkElement in (e.OriginalSource as FrameworkElement).GetVisualAncestors())
			{
				foreach (Type type in TiltEffect.TiltableItems)
				{
					if (type.IsAssignableFrom(frameworkElement.GetType()) && !(bool)frameworkElement.GetValue(TiltEffect.SuppressTiltProperty))
					{
						FrameworkElement frameworkElement2 = VisualTreeHelper.GetChild(frameworkElement, 0) as FrameworkElement;
						FrameworkElement frameworkElement3 = e.ManipulationContainer as FrameworkElement;
						if (frameworkElement2 == null || frameworkElement3 == null)
						{
							return;
						}
						Point touchPoint = frameworkElement3.TransformToVisual(frameworkElement2).Transform(e.ManipulationOrigin);
						Point centerPoint;
						centerPoint..ctor(frameworkElement2.ActualWidth / 2.0, frameworkElement2.ActualHeight / 2.0);
						Point centerToCenterDelta = TiltEffect.GetCenterToCenterDelta(frameworkElement2, source);
						TiltEffect.BeginTiltEffect(frameworkElement2, touchPoint, centerPoint, centerToCenterDelta);
						return;
					}
				}
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000753C File Offset: 0x0000573C
		private static Point GetCenterToCenterDelta(FrameworkElement element, FrameworkElement container)
		{
			Point point;
			point..ctor(element.ActualWidth / 2.0, element.ActualHeight / 2.0);
			PhoneApplicationFrame phoneApplicationFrame = container as PhoneApplicationFrame;
			Point point2;
			if (phoneApplicationFrame != null)
			{
				if ((phoneApplicationFrame.Orientation & 2) == 2)
				{
					point2..ctor(container.ActualHeight / 2.0, container.ActualWidth / 2.0);
				}
				else
				{
					point2..ctor(container.ActualWidth / 2.0, container.ActualHeight / 2.0);
				}
			}
			else
			{
				point2..ctor(container.ActualWidth / 2.0, container.ActualHeight / 2.0);
			}
			Point point3 = element.TransformToVisual(container).Transform(point);
			Point result;
			result..ctor(point2.X - point3.X, point2.Y - point3.Y);
			return result;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007633 File Offset: 0x00005833
		private static void BeginTiltEffect(FrameworkElement element, Point touchPoint, Point centerPoint, Point centerDelta)
		{
			if (TiltEffect.tiltReturnStoryboard != null)
			{
				TiltEffect.StopTiltReturnStoryboardAndCleanup();
			}
			if (!TiltEffect.PrepareControlForTilt(element, centerDelta))
			{
				return;
			}
			TiltEffect.currentTiltElement = element;
			TiltEffect.currentTiltElementCenter = centerPoint;
			TiltEffect.PrepareTiltReturnStoryboard(element);
			TiltEffect.ApplyTiltEffect(TiltEffect.currentTiltElement, touchPoint, TiltEffect.currentTiltElementCenter);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007670 File Offset: 0x00005870
		private static bool PrepareControlForTilt(FrameworkElement element, Point centerDelta)
		{
			if (element.Projection != null || (element.RenderTransform != null && element.RenderTransform.GetType() != typeof(MatrixTransform)))
			{
				return false;
			}
			TiltEffect._originalCacheMode[element] = element.CacheMode;
			element.CacheMode = new BitmapCache();
			element.RenderTransform = new TranslateTransform
			{
				X = centerDelta.X,
				Y = centerDelta.Y
			};
			element.Projection = new PlaneProjection
			{
				GlobalOffsetX = -1.0 * centerDelta.X,
				GlobalOffsetY = -1.0 * centerDelta.Y
			};
			element.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(TiltEffect.TiltEffect_ManipulationDelta);
			element.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(TiltEffect.TiltEffect_ManipulationCompleted);
			return true;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000774C File Offset: 0x0000594C
		private static void RevertPrepareControlForTilt(FrameworkElement element)
		{
			element.ManipulationDelta -= new EventHandler<ManipulationDeltaEventArgs>(TiltEffect.TiltEffect_ManipulationDelta);
			element.ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(TiltEffect.TiltEffect_ManipulationCompleted);
			element.Projection = null;
			element.RenderTransform = null;
			CacheMode cacheMode;
			if (TiltEffect._originalCacheMode.TryGetValue(element, ref cacheMode))
			{
				element.CacheMode = cacheMode;
				TiltEffect._originalCacheMode.Remove(element);
				return;
			}
			element.CacheMode = null;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000077B8 File Offset: 0x000059B8
		private static void PrepareTiltReturnStoryboard(FrameworkElement element)
		{
			if (TiltEffect.tiltReturnStoryboard == null)
			{
				TiltEffect.tiltReturnStoryboard = new Storyboard();
				TiltEffect.tiltReturnStoryboard.Completed += new EventHandler(TiltEffect.TiltReturnStoryboard_Completed);
				TiltEffect.tiltReturnXAnimation = new DoubleAnimation();
				Storyboard.SetTargetProperty(TiltEffect.tiltReturnXAnimation, new PropertyPath(PlaneProjection.RotationXProperty));
				TiltEffect.tiltReturnXAnimation.BeginTime = new TimeSpan?(TiltEffect.TiltReturnAnimationDelay);
				TiltEffect.tiltReturnXAnimation.To = new double?(0.0);
				TiltEffect.tiltReturnXAnimation.Duration = TiltEffect.TiltReturnAnimationDuration;
				TiltEffect.tiltReturnYAnimation = new DoubleAnimation();
				Storyboard.SetTargetProperty(TiltEffect.tiltReturnYAnimation, new PropertyPath(PlaneProjection.RotationYProperty));
				TiltEffect.tiltReturnYAnimation.BeginTime = new TimeSpan?(TiltEffect.TiltReturnAnimationDelay);
				TiltEffect.tiltReturnYAnimation.To = new double?(0.0);
				TiltEffect.tiltReturnYAnimation.Duration = TiltEffect.TiltReturnAnimationDuration;
				TiltEffect.tiltReturnZAnimation = new DoubleAnimation();
				Storyboard.SetTargetProperty(TiltEffect.tiltReturnZAnimation, new PropertyPath(PlaneProjection.GlobalOffsetZProperty));
				TiltEffect.tiltReturnZAnimation.BeginTime = new TimeSpan?(TiltEffect.TiltReturnAnimationDelay);
				TiltEffect.tiltReturnZAnimation.To = new double?(0.0);
				TiltEffect.tiltReturnZAnimation.Duration = TiltEffect.TiltReturnAnimationDuration;
				if (TiltEffect.UseLogarithmicEase)
				{
					TiltEffect.tiltReturnXAnimation.EasingFunction = new TiltEffect.LogarithmicEase();
					TiltEffect.tiltReturnYAnimation.EasingFunction = new TiltEffect.LogarithmicEase();
					TiltEffect.tiltReturnZAnimation.EasingFunction = new TiltEffect.LogarithmicEase();
				}
				TiltEffect.tiltReturnStoryboard.Children.Add(TiltEffect.tiltReturnXAnimation);
				TiltEffect.tiltReturnStoryboard.Children.Add(TiltEffect.tiltReturnYAnimation);
				TiltEffect.tiltReturnStoryboard.Children.Add(TiltEffect.tiltReturnZAnimation);
			}
			Storyboard.SetTarget(TiltEffect.tiltReturnXAnimation, element.Projection);
			Storyboard.SetTarget(TiltEffect.tiltReturnYAnimation, element.Projection);
			Storyboard.SetTarget(TiltEffect.tiltReturnZAnimation, element.Projection);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000079AC File Offset: 0x00005BAC
		private static void ContinueTiltEffect(FrameworkElement element, ManipulationDeltaEventArgs e)
		{
			FrameworkElement frameworkElement = e.ManipulationContainer as FrameworkElement;
			if (frameworkElement == null || element == null)
			{
				return;
			}
			Point point = frameworkElement.TransformToVisual(element).Transform(e.ManipulationOrigin);
			if (!new Rect(0.0, 0.0, TiltEffect.currentTiltElement.ActualWidth, TiltEffect.currentTiltElement.ActualHeight).Contains(point))
			{
				TiltEffect.PauseTiltEffect();
				return;
			}
			TiltEffect.ApplyTiltEffect(TiltEffect.currentTiltElement, e.ManipulationOrigin, TiltEffect.currentTiltElementCenter);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007A30 File Offset: 0x00005C30
		private static void EndTiltEffect(FrameworkElement element)
		{
			if (element != null)
			{
				element.ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(TiltEffect.TiltEffect_ManipulationCompleted);
				element.ManipulationDelta -= new EventHandler<ManipulationDeltaEventArgs>(TiltEffect.TiltEffect_ManipulationDelta);
			}
			if (TiltEffect.tiltReturnStoryboard != null)
			{
				TiltEffect.wasPauseAnimation = false;
				if (TiltEffect.tiltReturnStoryboard.GetCurrentState() != null)
				{
					TiltEffect.tiltReturnStoryboard.Begin();
					return;
				}
			}
			else
			{
				TiltEffect.StopTiltReturnStoryboardAndCleanup();
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007A8D File Offset: 0x00005C8D
		private static void TiltReturnStoryboard_Completed(object sender, EventArgs e)
		{
			if (TiltEffect.wasPauseAnimation)
			{
				TiltEffect.ResetTiltEffect(TiltEffect.currentTiltElement);
				return;
			}
			TiltEffect.StopTiltReturnStoryboardAndCleanup();
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007AA8 File Offset: 0x00005CA8
		private static void ResetTiltEffect(FrameworkElement element)
		{
			PlaneProjection planeProjection = element.Projection as PlaneProjection;
			planeProjection.RotationY = 0.0;
			planeProjection.RotationX = 0.0;
			planeProjection.GlobalOffsetZ = 0.0;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007AEE File Offset: 0x00005CEE
		private static void StopTiltReturnStoryboardAndCleanup()
		{
			if (TiltEffect.tiltReturnStoryboard != null)
			{
				TiltEffect.tiltReturnStoryboard.Stop();
			}
			TiltEffect.RevertPrepareControlForTilt(TiltEffect.currentTiltElement);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00007B0B File Offset: 0x00005D0B
		private static void PauseTiltEffect()
		{
			if (TiltEffect.tiltReturnStoryboard != null && !TiltEffect.wasPauseAnimation)
			{
				TiltEffect.tiltReturnStoryboard.Stop();
				TiltEffect.wasPauseAnimation = true;
				TiltEffect.tiltReturnStoryboard.Begin();
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007B35 File Offset: 0x00005D35
		private static void ResetTiltReturnStoryboard()
		{
			TiltEffect.tiltReturnStoryboard.Stop();
			TiltEffect.wasPauseAnimation = false;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007B48 File Offset: 0x00005D48
		private static void ApplyTiltEffect(FrameworkElement element, Point touchPoint, Point centerPoint)
		{
			TiltEffect.ResetTiltReturnStoryboard();
			Point point;
			point..ctor(Math.Min(Math.Max(touchPoint.X / (centerPoint.X * 2.0), 0.0), 1.0), Math.Min(Math.Max(touchPoint.Y / (centerPoint.Y * 2.0), 0.0), 1.0));
			double num = Math.Abs(point.X - 0.5);
			double num2 = Math.Abs(point.Y - 0.5);
			double num3 = (double)(-(double)Math.Sign(point.X - 0.5));
			double num4 = (double)Math.Sign(point.Y - 0.5);
			double num5 = num + num2;
			double num6 = (num + num2 > 0.0) ? (num / (num + num2)) : 0.0;
			double num7 = num5 * 0.3 * 180.0 / 3.141592653589793;
			double num8 = (1.0 - num5) * 25.0;
			PlaneProjection planeProjection = element.Projection as PlaneProjection;
			planeProjection.RotationY = num7 * num6 * num3;
			planeProjection.RotationX = num7 * (1.0 - num6) * num4;
			planeProjection.GlobalOffsetZ = -num8;
		}

		// Token: 0x04000089 RID: 137
		private const double MaxAngle = 0.3;

		// Token: 0x0400008A RID: 138
		private const double MaxDepression = 25.0;

		// Token: 0x0400008B RID: 139
		private static Dictionary<DependencyObject, CacheMode> _originalCacheMode = new Dictionary<DependencyObject, CacheMode>();

		// Token: 0x0400008C RID: 140
		private static readonly TimeSpan TiltReturnAnimationDelay = TimeSpan.FromMilliseconds(200.0);

		// Token: 0x0400008D RID: 141
		private static readonly TimeSpan TiltReturnAnimationDuration = TimeSpan.FromMilliseconds(100.0);

		// Token: 0x0400008E RID: 142
		private static FrameworkElement currentTiltElement;

		// Token: 0x0400008F RID: 143
		private static Storyboard tiltReturnStoryboard;

		// Token: 0x04000090 RID: 144
		private static DoubleAnimation tiltReturnXAnimation;

		// Token: 0x04000091 RID: 145
		private static DoubleAnimation tiltReturnYAnimation;

		// Token: 0x04000092 RID: 146
		private static DoubleAnimation tiltReturnZAnimation;

		// Token: 0x04000093 RID: 147
		private static Point currentTiltElementCenter;

		// Token: 0x04000094 RID: 148
		private static bool wasPauseAnimation = false;

		// Token: 0x04000095 RID: 149
		public static readonly DependencyProperty IsTiltEnabledProperty = DependencyProperty.RegisterAttached("IsTiltEnabled", typeof(bool), typeof(TiltEffect), new PropertyMetadata(new PropertyChangedCallback(TiltEffect.OnIsTiltEnabledChanged)));

		// Token: 0x04000096 RID: 150
		public static readonly DependencyProperty SuppressTiltProperty = DependencyProperty.RegisterAttached("SuppressTilt", typeof(bool), typeof(TiltEffect), null);

		// Token: 0x0200002E RID: 46
		private class LogarithmicEase : EasingFunctionBase
		{
			// Token: 0x06000183 RID: 387 RVA: 0x00007CC8 File Offset: 0x00005EC8
			protected override double EaseInCore(double normalizedTime)
			{
				return Math.Log(normalizedTime + 1.0) / 0.693147181;
			}
		}
	}
}
