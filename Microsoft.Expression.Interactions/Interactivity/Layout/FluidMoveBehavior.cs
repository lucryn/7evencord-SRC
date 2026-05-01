using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Microsoft.Expression.Interactivity.Layout
{
	// Token: 0x0200001B RID: 27
	public sealed class FluidMoveBehavior : FluidMoveBehaviorBase
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000064F9 File Offset: 0x000046F9
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x0000650B File Offset: 0x0000470B
		public Duration Duration
		{
			get
			{
				return (Duration)base.GetValue(FluidMoveBehavior.DurationProperty);
			}
			set
			{
				base.SetValue(FluidMoveBehavior.DurationProperty, value);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000651E File Offset: 0x0000471E
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00006530 File Offset: 0x00004730
		public TagType InitialTag
		{
			get
			{
				return (TagType)base.GetValue(FluidMoveBehavior.InitialTagProperty);
			}
			set
			{
				base.SetValue(FluidMoveBehavior.InitialTagProperty, value);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00006543 File Offset: 0x00004743
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00006555 File Offset: 0x00004755
		public string InitialTagPath
		{
			get
			{
				return (string)base.GetValue(FluidMoveBehavior.InitialTagPathProperty);
			}
			set
			{
				base.SetValue(FluidMoveBehavior.InitialTagPathProperty, value);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006563 File Offset: 0x00004763
		private static object GetInitialIdentityTag(DependencyObject obj)
		{
			return obj.GetValue(FluidMoveBehavior.InitialIdentityTagProperty);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006570 File Offset: 0x00004770
		private static void SetInitialIdentityTag(DependencyObject obj, object value)
		{
			obj.SetValue(FluidMoveBehavior.InitialIdentityTagProperty, value);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006580 File Offset: 0x00004780
		private static void InitialIdentityTagChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement != null)
			{
				Canvas canvas = VisualTreeHelper.GetParent(frameworkElement) as Canvas;
				if (canvas != null)
				{
					canvas.InvalidateMeasure();
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000065AC File Offset: 0x000047AC
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x000065BE File Offset: 0x000047BE
		public bool FloatAbove
		{
			get
			{
				return (bool)base.GetValue(FluidMoveBehavior.FloatAboveProperty);
			}
			set
			{
				base.SetValue(FluidMoveBehavior.FloatAboveProperty, value);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000065D1 File Offset: 0x000047D1
		// (set) Token: 0x060000EB RID: 235 RVA: 0x000065E3 File Offset: 0x000047E3
		public IEasingFunction EaseX
		{
			get
			{
				return (IEasingFunction)base.GetValue(FluidMoveBehavior.EaseXProperty);
			}
			set
			{
				base.SetValue(FluidMoveBehavior.EaseXProperty, value);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000EC RID: 236 RVA: 0x000065F1 File Offset: 0x000047F1
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00006603 File Offset: 0x00004803
		public IEasingFunction EaseY
		{
			get
			{
				return (IEasingFunction)base.GetValue(FluidMoveBehavior.EaseYProperty);
			}
			set
			{
				base.SetValue(FluidMoveBehavior.EaseYProperty, value);
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006611 File Offset: 0x00004811
		private static object GetOverlay(DependencyObject obj)
		{
			return obj.GetValue(FluidMoveBehavior.OverlayProperty);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000661E File Offset: 0x0000481E
		private static void SetOverlay(DependencyObject obj, object value)
		{
			obj.SetValue(FluidMoveBehavior.OverlayProperty, value);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000662C File Offset: 0x0000482C
		private static object GetCacheDuringOverlay(DependencyObject obj)
		{
			return obj.GetValue(FluidMoveBehavior.CacheDuringOverlayProperty);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006639 File Offset: 0x00004839
		private static void SetCacheDuringOverlay(DependencyObject obj, object value)
		{
			obj.SetValue(FluidMoveBehavior.CacheDuringOverlayProperty, value);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006647 File Offset: 0x00004847
		private static bool GetHasTransformWrapper(DependencyObject obj)
		{
			return (bool)obj.GetValue(FluidMoveBehavior.HasTransformWrapperProperty);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006659 File Offset: 0x00004859
		private static void SetHasTransformWrapper(DependencyObject obj, bool value)
		{
			obj.SetValue(FluidMoveBehavior.HasTransformWrapperProperty, value);
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000666C File Offset: 0x0000486C
		protected override bool ShouldSkipInitialLayout
		{
			get
			{
				return base.ShouldSkipInitialLayout || this.InitialTag == TagType.DataContext;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006684 File Offset: 0x00004884
		protected override void EnsureTags(FrameworkElement child)
		{
			base.EnsureTags(child);
			if (this.InitialTag == TagType.DataContext)
			{
				object obj = child.ReadLocalValue(FluidMoveBehavior.InitialIdentityTagProperty);
				if (!(obj is BindingExpression))
				{
					child.SetBinding(FluidMoveBehavior.InitialIdentityTagProperty, new Binding(this.InitialTagPath));
				}
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00006798 File Offset: 0x00004998
		internal override void UpdateLayoutTransitionCore(FrameworkElement child, FrameworkElement root, object tag, FluidMoveBehaviorBase.TagData newTagData)
		{
			bool flag = false;
			bool flag2 = false;
			object initialIdentityTag = FluidMoveBehavior.GetInitialIdentityTag(child);
			FluidMoveBehaviorBase.TagData tagData;
			bool flag3 = FluidMoveBehaviorBase.TagDictionary.TryGetValue(tag, ref tagData);
			if (flag3 && tagData.InitialTag != initialIdentityTag)
			{
				flag3 = false;
				FluidMoveBehaviorBase.TagDictionary.Remove(tag);
			}
			Rect rect;
			if (!flag3)
			{
				FluidMoveBehaviorBase.TagData tagData2;
				if (initialIdentityTag != null && FluidMoveBehaviorBase.TagDictionary.TryGetValue(initialIdentityTag, ref tagData2))
				{
					rect = FluidMoveBehaviorBase.TranslateRect(tagData2.AppRect, root, newTagData.Parent);
					flag = true;
					flag2 = true;
				}
				else
				{
					rect = Rect.Empty;
				}
				tagData = new FluidMoveBehaviorBase.TagData
				{
					ParentRect = Rect.Empty,
					AppRect = Rect.Empty,
					Parent = newTagData.Parent,
					Child = child,
					Timestamp = DateTime.Now,
					InitialTag = initialIdentityTag
				};
				FluidMoveBehaviorBase.TagDictionary.Add(tag, tagData);
			}
			else if (tagData.Parent != VisualTreeHelper.GetParent(child))
			{
				rect = FluidMoveBehaviorBase.TranslateRect(tagData.AppRect, root, newTagData.Parent);
				flag = true;
			}
			else
			{
				rect = tagData.ParentRect;
			}
			Panel panel = VisualTreeHelper.GetParent(child) as Panel;
			if (panel != null && panel.IsItemsHost && child is Control)
			{
				VisualStateGroup visualStateGroup = ((FrameworkElement)VisualTreeHelper.GetChild(child, 0)).FindName("LayoutStates") as VisualStateGroup;
				if (visualStateGroup != null && visualStateGroup.CurrentState != null && visualStateGroup.CurrentState.Name == "BeforeUnloaded")
				{
					return;
				}
			}
			if (child != tagData.Child)
			{
				panel = (VisualTreeHelper.GetParent(tagData.Child) as Panel);
				if (panel != null && panel.IsItemsHost && child is Control && tagData.Child is Control)
				{
					VisualStateGroup visualStateGroup2 = ((FrameworkElement)VisualTreeHelper.GetChild(tagData.Child, 0)).FindName("LayoutStates") as VisualStateGroup;
					if (visualStateGroup2 != null && visualStateGroup2.CurrentState != null && visualStateGroup2.CurrentState.Name == "BeforeUnloaded")
					{
						VisualStateManager.GoToState((Control)child, "BeforeLoaded", false);
						VisualStateManager.GoToState((Control)child, "AfterLoaded", false);
						VisualStateManager.GoToState((Control)tagData.Child, "AfterLoaded", false);
						VisualStateManager.GoToState((Control)tagData.Child, "BeforeUnloaded", false);
					}
				}
			}
			FrameworkElement originalChild = child;
			if ((!FluidMoveBehavior.IsEmptyRect(rect) && !FluidMoveBehavior.IsEmptyRect(newTagData.ParentRect) && (!FluidMoveBehavior.IsClose(rect.Left, newTagData.ParentRect.Left) || !FluidMoveBehavior.IsClose(rect.Top, newTagData.ParentRect.Top))) || (child != tagData.Child && FluidMoveBehavior.TransitionStoryboardDictionary.ContainsKey(tag)))
			{
				Rect rect2 = rect;
				bool flag4 = false;
				Storyboard storyboard = null;
				if (FluidMoveBehavior.TransitionStoryboardDictionary.TryGetValue(tag, ref storyboard))
				{
					object overlay2 = FluidMoveBehavior.GetOverlay(tagData.Child);
					Popup popup = (Popup)overlay2;
					flag4 = (overlay2 != null);
					FrameworkElement child2 = tagData.Child;
					if (overlay2 != null)
					{
						Canvas canvas = popup.Child as Canvas;
						if (canvas != null)
						{
							child2 = (canvas.Children[0] as FrameworkElement);
						}
					}
					if (!flag2)
					{
						Transform transform = FluidMoveBehavior.GetTransform(child2);
						rect2 = transform.TransformBounds(rect2);
					}
					FluidMoveBehavior.TransitionStoryboardDictionary.Remove(tag);
					storyboard.Stop();
					storyboard = null;
					FluidMoveBehavior.RemoveTransform(child2);
					if (overlay2 != null)
					{
						popup.IsOpen = false;
						FluidMoveBehavior.TransferLocalValue(tagData.Child, FluidMoveBehavior.CacheDuringOverlayProperty, UIElement.OpacityProperty);
						FluidMoveBehavior.SetOverlay(tagData.Child, null);
					}
				}
				object overlay = null;
				if (flag4 || (flag && this.FloatAbove))
				{
					Canvas canvas2 = new Canvas
					{
						Width = root.ActualWidth,
						Height = root.ActualHeight,
						RenderTransform = (Transform)newTagData.Parent.TransformToVisual(root),
						IsHitTestVisible = false
					};
					Image image = new Image
					{
						Width = newTagData.ParentRect.Width,
						Height = newTagData.ParentRect.Height,
						IsHitTestVisible = false
					};
					Canvas.SetLeft(image, newTagData.ParentRect.Left);
					Canvas.SetTop(image, newTagData.ParentRect.Top);
					WriteableBitmap source = new WriteableBitmap(child, null);
					image.Source = source;
					canvas2.Children.Add(image);
					overlay = new Popup
					{
						HorizontalOffset = 0.0,
						VerticalOffset = 0.0,
						Child = canvas2,
						IsHitTestVisible = false,
						IsOpen = true
					};
					FluidMoveBehavior.SetOverlay(originalChild, overlay);
					FluidMoveBehavior.TransferLocalValue(child, UIElement.OpacityProperty, FluidMoveBehavior.CacheDuringOverlayProperty);
					child.Opacity = 0.0;
					child = image;
				}
				Rect parentRect = newTagData.ParentRect;
				Storyboard transitionStoryboard = this.CreateTransitionStoryboard(child, flag2, ref parentRect, ref rect2);
				FluidMoveBehavior.TransitionStoryboardDictionary.Add(tag, transitionStoryboard);
				transitionStoryboard.Completed += delegate(object sender, EventArgs e)
				{
					Storyboard storyboard2;
					if (FluidMoveBehavior.TransitionStoryboardDictionary.TryGetValue(tag, ref storyboard2) && storyboard2 == transitionStoryboard)
					{
						FluidMoveBehavior.TransitionStoryboardDictionary.Remove(tag);
						transitionStoryboard.Stop();
						FluidMoveBehavior.RemoveTransform(child);
						child.InvalidateMeasure();
						if (overlay != null)
						{
							((Popup)overlay).IsOpen = false;
							FluidMoveBehavior.TransferLocalValue(originalChild, FluidMoveBehavior.CacheDuringOverlayProperty, UIElement.OpacityProperty);
							FluidMoveBehavior.SetOverlay(originalChild, null);
						}
					}
				};
				transitionStoryboard.Begin();
			}
			tagData.ParentRect = newTagData.ParentRect;
			tagData.AppRect = newTagData.AppRect;
			tagData.Parent = newTagData.Parent;
			tagData.Child = newTagData.Child;
			tagData.Timestamp = newTagData.Timestamp;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006DF4 File Offset: 0x00004FF4
		private Storyboard CreateTransitionStoryboard(FrameworkElement child, bool usingBeforeLoaded, ref Rect layoutRect, ref Rect currentRect)
		{
			Duration duration = this.Duration;
			Storyboard storyboard = new Storyboard();
			storyboard.Duration = duration;
			double num = (!usingBeforeLoaded || layoutRect.Width == 0.0) ? 1.0 : (currentRect.Width / layoutRect.Width);
			double num2 = (!usingBeforeLoaded || layoutRect.Height == 0.0) ? 1.0 : (currentRect.Height / layoutRect.Height);
			double num3 = currentRect.Left - layoutRect.Left;
			double num4 = currentRect.Top - layoutRect.Top;
			TransformGroup transformGroup = new TransformGroup();
			transformGroup.Children.Add(new ScaleTransform
			{
				ScaleX = num,
				ScaleY = num2
			});
			transformGroup.Children.Add(new TranslateTransform
			{
				X = num3,
				Y = num4
			});
			FluidMoveBehavior.AddTransform(child, transformGroup);
			string text = "(FrameworkElement.RenderTransform).";
			TransformGroup transformGroup2 = child.RenderTransform as TransformGroup;
			if (transformGroup2 != null && FluidMoveBehavior.GetHasTransformWrapper(child))
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"(TransformGroup.Children)[",
					transformGroup2.Children.Count - 1,
					"]."
				});
			}
			if (usingBeforeLoaded)
			{
				if (num != 1.0)
				{
					DoubleAnimation doubleAnimation = new DoubleAnimation
					{
						Duration = duration,
						From = new double?(num),
						To = new double?(1.0)
					};
					Storyboard.SetTarget(doubleAnimation, child);
					Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(text + "(TransformGroup.Children)[0].(ScaleTransform.ScaleX)", new object[0]));
					doubleAnimation.EasingFunction = this.EaseX;
					storyboard.Children.Add(doubleAnimation);
				}
				if (num2 != 1.0)
				{
					DoubleAnimation doubleAnimation2 = new DoubleAnimation
					{
						Duration = duration,
						From = new double?(num2),
						To = new double?(1.0)
					};
					Storyboard.SetTarget(doubleAnimation2, child);
					Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(text + "(TransformGroup.Children)[0].(ScaleTransform.ScaleY)", new object[0]));
					doubleAnimation2.EasingFunction = this.EaseY;
					storyboard.Children.Add(doubleAnimation2);
				}
			}
			if (num3 != 0.0)
			{
				DoubleAnimation doubleAnimation3 = new DoubleAnimation
				{
					Duration = duration,
					From = new double?(num3),
					To = new double?(0.0)
				};
				Storyboard.SetTarget(doubleAnimation3, child);
				Storyboard.SetTargetProperty(doubleAnimation3, new PropertyPath(text + "(TransformGroup.Children)[1].(TranslateTransform.X)", new object[0]));
				doubleAnimation3.EasingFunction = this.EaseX;
				storyboard.Children.Add(doubleAnimation3);
			}
			if (num4 != 0.0)
			{
				DoubleAnimation doubleAnimation4 = new DoubleAnimation
				{
					Duration = duration,
					From = new double?(num4),
					To = new double?(0.0)
				};
				Storyboard.SetTarget(doubleAnimation4, child);
				Storyboard.SetTargetProperty(doubleAnimation4, new PropertyPath(text + "(TransformGroup.Children)[1].(TranslateTransform.Y)", new object[0]));
				doubleAnimation4.EasingFunction = this.EaseY;
				storyboard.Children.Add(doubleAnimation4);
			}
			return storyboard;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00007160 File Offset: 0x00005360
		private static void AddTransform(FrameworkElement child, Transform transform)
		{
			TransformGroup transformGroup = child.RenderTransform as TransformGroup;
			if (transformGroup == null)
			{
				transformGroup = new TransformGroup();
				transformGroup.Children.Add(child.RenderTransform);
				child.RenderTransform = transformGroup;
				FluidMoveBehavior.SetHasTransformWrapper(child, true);
			}
			transformGroup.Children.Add(transform);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000071B0 File Offset: 0x000053B0
		private static Transform GetTransform(FrameworkElement child)
		{
			TransformGroup transformGroup = child.RenderTransform as TransformGroup;
			if (transformGroup != null && transformGroup.Children.Count > 0)
			{
				return transformGroup.Children[transformGroup.Children.Count - 1];
			}
			return new TranslateTransform();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000071F8 File Offset: 0x000053F8
		private static void RemoveTransform(FrameworkElement child)
		{
			TransformGroup transformGroup = child.RenderTransform as TransformGroup;
			if (transformGroup != null)
			{
				if (FluidMoveBehavior.GetHasTransformWrapper(child))
				{
					child.RenderTransform = transformGroup.Children[0];
					FluidMoveBehavior.SetHasTransformWrapper(child, false);
					return;
				}
				transformGroup.Children.RemoveAt(transformGroup.Children.Count - 1);
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00007250 File Offset: 0x00005450
		private static void TransferLocalValue(FrameworkElement element, DependencyProperty source, DependencyProperty dest)
		{
			object obj = element.ReadLocalValue(source);
			BindingExpression bindingExpression = obj as BindingExpression;
			if (bindingExpression != null)
			{
				element.SetBinding(dest, bindingExpression.ParentBinding);
			}
			else if (obj == DependencyProperty.UnsetValue)
			{
				element.ClearValue(dest);
			}
			else
			{
				element.SetValue(dest, element.GetAnimationBaseValue(source));
			}
			element.ClearValue(source);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000072A5 File Offset: 0x000054A5
		private static bool IsClose(double a, double b)
		{
			return Math.Abs(a - b) < 1E-07;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000072BB File Offset: 0x000054BB
		private static bool IsEmptyRect(Rect rect)
		{
			return rect.IsEmpty || double.IsNaN(rect.Left) || double.IsNaN(rect.Top);
		}

		// Token: 0x0400004F RID: 79
		public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(Duration), typeof(FluidMoveBehavior), new PropertyMetadata(new Duration(TimeSpan.FromSeconds(1.0))));

		// Token: 0x04000050 RID: 80
		public static readonly DependencyProperty InitialTagProperty = DependencyProperty.Register("InitialTag", typeof(TagType), typeof(FluidMoveBehavior), new PropertyMetadata(TagType.Element));

		// Token: 0x04000051 RID: 81
		public static readonly DependencyProperty InitialTagPathProperty = DependencyProperty.Register("InitialTagPath", typeof(string), typeof(FluidMoveBehavior), new PropertyMetadata(string.Empty));

		// Token: 0x04000052 RID: 82
		private static readonly DependencyProperty InitialIdentityTagProperty = DependencyProperty.RegisterAttached("InitialIdentityTag", typeof(object), typeof(FluidMoveBehavior), new PropertyMetadata(null, new PropertyChangedCallback(FluidMoveBehavior.InitialIdentityTagChanged)));

		// Token: 0x04000053 RID: 83
		public static readonly DependencyProperty FloatAboveProperty = DependencyProperty.Register("FloatAbove", typeof(bool), typeof(FluidMoveBehavior), new PropertyMetadata(true));

		// Token: 0x04000054 RID: 84
		public static readonly DependencyProperty EaseXProperty = DependencyProperty.Register("EaseX", typeof(IEasingFunction), typeof(FluidMoveBehavior), new PropertyMetadata(null));

		// Token: 0x04000055 RID: 85
		public static readonly DependencyProperty EaseYProperty = DependencyProperty.Register("EaseY", typeof(IEasingFunction), typeof(FluidMoveBehavior), new PropertyMetadata(null));

		// Token: 0x04000056 RID: 86
		private static readonly DependencyProperty OverlayProperty = DependencyProperty.RegisterAttached("Overlay", typeof(object), typeof(FluidMoveBehavior), new PropertyMetadata(null));

		// Token: 0x04000057 RID: 87
		private static readonly DependencyProperty CacheDuringOverlayProperty = DependencyProperty.RegisterAttached("CacheDuringOverlay", typeof(object), typeof(FluidMoveBehavior), new PropertyMetadata(null));

		// Token: 0x04000058 RID: 88
		private static readonly DependencyProperty HasTransformWrapperProperty = DependencyProperty.RegisterAttached("HasTransformWrapper", typeof(bool), typeof(FluidMoveBehavior), new PropertyMetadata(false));

		// Token: 0x04000059 RID: 89
		private static Dictionary<object, Storyboard> TransitionStoryboardDictionary = new Dictionary<object, Storyboard>();
	}
}
