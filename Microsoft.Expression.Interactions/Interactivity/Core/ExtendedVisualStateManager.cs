using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x02000013 RID: 19
	public class ExtendedVisualStateManager : VisualStateManager
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00003CB0 File Offset: 0x00001EB0
		public static bool GoToElementState(FrameworkElement root, string stateName, bool useTransitions)
		{
			ExtendedVisualStateManager extendedVisualStateManager = VisualStateManager.GetCustomVisualStateManager(root) as ExtendedVisualStateManager;
			return extendedVisualStateManager != null && extendedVisualStateManager.GoToStateInternal(root, stateName, useTransitions);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003CD8 File Offset: 0x00001ED8
		private bool GoToStateInternal(FrameworkElement stateGroupsRoot, string stateName, bool useTransitions)
		{
			VisualStateGroup visualStateGroup;
			VisualState visualState;
			return ExtendedVisualStateManager.TryGetState(stateGroupsRoot, stateName, out visualStateGroup, out visualState) && this.GoToStateCore(null, stateGroupsRoot, stateName, visualStateGroup, visualState, useTransitions);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003D00 File Offset: 0x00001F00
		private static bool TryGetState(FrameworkElement element, string stateName, out VisualStateGroup group, out VisualState state)
		{
			group = null;
			state = null;
			IList visualStateGroups = VisualStateManager.GetVisualStateGroups(element);
			foreach (object obj in visualStateGroups)
			{
				VisualStateGroup visualStateGroup = (VisualStateGroup)obj;
				foreach (object obj2 in visualStateGroup.States)
				{
					VisualState visualState = (VisualState)obj2;
					if (visualState.Name == stateName)
					{
						group = visualStateGroup;
						state = visualState;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003DC4 File Offset: 0x00001FC4
		public static bool IsRunningFluidLayoutTransition
		{
			get
			{
				return ExtendedVisualStateManager.LayoutTransitionStoryboard != null;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003DD1 File Offset: 0x00001FD1
		public static bool GetUseFluidLayout(DependencyObject obj)
		{
			return (bool)obj.GetValue(ExtendedVisualStateManager.UseFluidLayoutProperty);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003DE3 File Offset: 0x00001FE3
		public static void SetUseFluidLayout(DependencyObject obj, bool value)
		{
			obj.SetValue(ExtendedVisualStateManager.UseFluidLayoutProperty, value);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003DF6 File Offset: 0x00001FF6
		public static DependencyProperty GetRuntimeVisibilityProperty(DependencyObject obj)
		{
			return (DependencyProperty)obj.GetValue(ExtendedVisualStateManager.RuntimeVisibilityPropertyProperty);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003E08 File Offset: 0x00002008
		public static void SetRuntimeVisibilityProperty(DependencyObject obj, DependencyProperty value)
		{
			obj.SetValue(ExtendedVisualStateManager.RuntimeVisibilityPropertyProperty, value);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003E16 File Offset: 0x00002016
		internal static List<ExtendedVisualStateManager.OriginalLayoutValueRecord> GetOriginalLayoutValues(DependencyObject obj)
		{
			return (List<ExtendedVisualStateManager.OriginalLayoutValueRecord>)obj.GetValue(ExtendedVisualStateManager.OriginalLayoutValuesProperty);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003E28 File Offset: 0x00002028
		internal static void SetOriginalLayoutValues(DependencyObject obj, List<ExtendedVisualStateManager.OriginalLayoutValueRecord> value)
		{
			obj.SetValue(ExtendedVisualStateManager.OriginalLayoutValuesProperty, value);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003E36 File Offset: 0x00002036
		internal static Storyboard GetLayoutStoryboard(DependencyObject obj)
		{
			return (Storyboard)obj.GetValue(ExtendedVisualStateManager.LayoutStoryboardProperty);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003E48 File Offset: 0x00002048
		internal static void SetLayoutStoryboard(DependencyObject obj, Storyboard value)
		{
			obj.SetValue(ExtendedVisualStateManager.LayoutStoryboardProperty, value);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003E56 File Offset: 0x00002056
		internal static VisualState GetCurrentState(DependencyObject obj)
		{
			return (VisualState)obj.GetValue(ExtendedVisualStateManager.CurrentStateProperty);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003E68 File Offset: 0x00002068
		internal static void SetCurrentState(DependencyObject obj, VisualState value)
		{
			obj.SetValue(ExtendedVisualStateManager.CurrentStateProperty, value);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003E76 File Offset: 0x00002076
		private static bool IsVisibilityProperty(DependencyProperty property)
		{
			return (ExtendedVisualStateManager.RuntimeVisibility != null && property == ExtendedVisualStateManager.RuntimeVisibility) || property == UIElement.VisibilityProperty;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003E94 File Offset: 0x00002094
		private static DependencyProperty LayoutPropertyFromTimeline(Timeline timeline, bool forceRuntimeProperty)
		{
			PropertyPath targetProperty = Storyboard.GetTargetProperty(timeline);
			DependencyProperty runtimeVisibilityProperty = ExtendedVisualStateManager.GetRuntimeVisibilityProperty(timeline);
			if (runtimeVisibilityProperty != null)
			{
				if (ExtendedVisualStateManager.RuntimeVisibility == null)
				{
					ExtendedVisualStateManager.LayoutProperties.Add(runtimeVisibilityProperty);
					ExtendedVisualStateManager.RuntimeVisibility = runtimeVisibilityProperty;
				}
				if (forceRuntimeProperty)
				{
					return runtimeVisibilityProperty;
				}
				return UIElement.VisibilityProperty;
			}
			else
			{
				DependencyProperty dependencyProperty;
				if (targetProperty != null && ExtendedVisualStateManager.PathToPropertyMap.TryGetValue(targetProperty.Path, ref dependencyProperty) && ExtendedVisualStateManager.LayoutProperties.Contains(dependencyProperty))
				{
					return dependencyProperty;
				}
				return null;
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003F24 File Offset: 0x00002124
		protected override bool GoToStateCore(Control control, FrameworkElement templateRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
		{
			if (control == null)
			{
				control = new ContentControl();
			}
			if (group == null || state == null)
			{
				return false;
			}
			if (!ExtendedVisualStateManager.GetUseFluidLayout(group))
			{
				return base.GoToStateCore(control, templateRoot, stateName, group, state, useTransitions);
			}
			VisualState currentState = ExtendedVisualStateManager.GetCurrentState(group);
			if (currentState == state)
			{
				if (!useTransitions && ExtendedVisualStateManager.LayoutTransitionStoryboard != null)
				{
					ExtendedVisualStateManager.StopAnimations();
				}
				return true;
			}
			ExtendedVisualStateManager.SetCurrentState(group, state);
			Storyboard storyboard = ExtendedVisualStateManager.ExtractLayoutStoryboard(state);
			List<ExtendedVisualStateManager.OriginalLayoutValueRecord> list = ExtendedVisualStateManager.GetOriginalLayoutValues(group);
			if (list == null)
			{
				list = new List<ExtendedVisualStateManager.OriginalLayoutValueRecord>();
				ExtendedVisualStateManager.SetOriginalLayoutValues(group, list);
			}
			if (!useTransitions)
			{
				if (ExtendedVisualStateManager.LayoutTransitionStoryboard != null)
				{
					ExtendedVisualStateManager.StopAnimations();
				}
				base.GoToStateCore(control, templateRoot, stateName, group, state, useTransitions);
				ExtendedVisualStateManager.SetLayoutStoryboardProperties(control, templateRoot, storyboard, list);
				return true;
			}
			if (storyboard.Children.Count == 0 && list.Count == 0)
			{
				return base.GoToStateCore(control, templateRoot, stateName, group, state, useTransitions);
			}
			templateRoot.UpdateLayout();
			List<FrameworkElement> list2 = ExtendedVisualStateManager.FindTargetElements(control, templateRoot, storyboard, list, ExtendedVisualStateManager.MovingElements);
			Dictionary<FrameworkElement, Rect> rectsOfTargets = ExtendedVisualStateManager.GetRectsOfTargets(list2, ExtendedVisualStateManager.MovingElements);
			Dictionary<FrameworkElement, double> oldOpacities = ExtendedVisualStateManager.GetOldOpacities(control, templateRoot, storyboard, list, ExtendedVisualStateManager.MovingElements);
			if (ExtendedVisualStateManager.LayoutTransitionStoryboard != null)
			{
				templateRoot.LayoutUpdated -= new EventHandler(ExtendedVisualStateManager.control_LayoutUpdated);
				ExtendedVisualStateManager.StopAnimations();
				templateRoot.UpdateLayout();
			}
			base.GoToStateCore(control, templateRoot, stateName, group, state, useTransitions);
			ExtendedVisualStateManager.SetLayoutStoryboardProperties(control, templateRoot, storyboard, list);
			templateRoot.UpdateLayout();
			Dictionary<FrameworkElement, Rect> rectsOfTargets2 = ExtendedVisualStateManager.GetRectsOfTargets(list2, null);
			ExtendedVisualStateManager.MovingElements = new List<FrameworkElement>();
			foreach (FrameworkElement frameworkElement in list2)
			{
				if (rectsOfTargets[frameworkElement] != rectsOfTargets2[frameworkElement])
				{
					ExtendedVisualStateManager.MovingElements.Add(frameworkElement);
				}
			}
			foreach (FrameworkElement frameworkElement2 in oldOpacities.Keys)
			{
				if (!ExtendedVisualStateManager.MovingElements.Contains(frameworkElement2))
				{
					ExtendedVisualStateManager.MovingElements.Add(frameworkElement2);
				}
			}
			ExtendedVisualStateManager.WrapMovingElementsInCanvases(ExtendedVisualStateManager.MovingElements, rectsOfTargets, rectsOfTargets2);
			VisualTransition transition = ExtendedVisualStateManager.FindTransition(group, currentState, state);
			templateRoot.LayoutUpdated += new EventHandler(ExtendedVisualStateManager.control_LayoutUpdated);
			ExtendedVisualStateManager.LayoutTransitionStoryboard = ExtendedVisualStateManager.CreateLayoutTransitionStoryboard(transition, ExtendedVisualStateManager.MovingElements, oldOpacities);
			ExtendedVisualStateManager.LayoutTransitionStoryboard.Completed += delegate(object sender, EventArgs args)
			{
				templateRoot.LayoutUpdated -= new EventHandler(ExtendedVisualStateManager.control_LayoutUpdated);
				ExtendedVisualStateManager.StopAnimations();
			};
			ExtendedVisualStateManager.LayoutTransitionStoryboard.Begin();
			return true;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004200 File Offset: 0x00002400
		private static void control_LayoutUpdated(object sender, EventArgs e)
		{
			if (ExtendedVisualStateManager.LayoutTransitionStoryboard != null)
			{
				foreach (FrameworkElement frameworkElement in ExtendedVisualStateManager.MovingElements)
				{
					ExtendedVisualStateManager.WrapperCanvas wrapperCanvas = frameworkElement.Parent as ExtendedVisualStateManager.WrapperCanvas;
					if (wrapperCanvas != null)
					{
						Rect layoutRect = ExtendedVisualStateManager.GetLayoutRect(wrapperCanvas);
						Rect newRect = wrapperCanvas.NewRect;
						TranslateTransform translateTransform = wrapperCanvas.RenderTransform as TranslateTransform;
						double num = (translateTransform == null) ? 0.0 : translateTransform.X;
						double num2 = (translateTransform == null) ? 0.0 : translateTransform.Y;
						double num3 = newRect.Left - layoutRect.Left;
						double num4 = newRect.Top - layoutRect.Top;
						if (num != num3 || num2 != num4)
						{
							if (translateTransform == null)
							{
								translateTransform = new TranslateTransform();
								wrapperCanvas.RenderTransform = translateTransform;
							}
							translateTransform.X = num3;
							translateTransform.Y = num4;
						}
					}
				}
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000430C File Offset: 0x0000250C
		private static void StopAnimations()
		{
			if (ExtendedVisualStateManager.LayoutTransitionStoryboard != null)
			{
				ExtendedVisualStateManager.LayoutTransitionStoryboard.Stop();
				ExtendedVisualStateManager.LayoutTransitionStoryboard = null;
			}
			if (ExtendedVisualStateManager.MovingElements != null)
			{
				ExtendedVisualStateManager.UnwrapMovingElementsFromCanvases(ExtendedVisualStateManager.MovingElements);
				ExtendedVisualStateManager.MovingElements = null;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000433C File Offset: 0x0000253C
		private static VisualTransition FindTransition(VisualStateGroup group, VisualState previousState, VisualState state)
		{
			string text = (previousState != null) ? previousState.Name : string.Empty;
			string text2 = (state != null) ? state.Name : string.Empty;
			int num = -1;
			VisualTransition result = null;
			if (group.Transitions != null)
			{
				foreach (object obj in group.Transitions)
				{
					VisualTransition visualTransition = (VisualTransition)obj;
					int num2 = 0;
					if (visualTransition.From == text)
					{
						num2++;
					}
					else if (!string.IsNullOrEmpty(visualTransition.From))
					{
						continue;
					}
					if (visualTransition.To == text2)
					{
						num2 += 2;
					}
					else if (!string.IsNullOrEmpty(visualTransition.To))
					{
						continue;
					}
					if (num2 > num)
					{
						num = num2;
						result = visualTransition;
					}
				}
			}
			return result;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004420 File Offset: 0x00002620
		private static Storyboard ExtractLayoutStoryboard(VisualState state)
		{
			Storyboard storyboard = null;
			if (state.Storyboard != null)
			{
				storyboard = ExtendedVisualStateManager.GetLayoutStoryboard(state.Storyboard);
				if (storyboard == null)
				{
					storyboard = new Storyboard();
					for (int i = state.Storyboard.Children.Count - 1; i >= 0; i--)
					{
						Timeline timeline = state.Storyboard.Children[i];
						if (ExtendedVisualStateManager.LayoutPropertyFromTimeline(timeline, false) != null)
						{
							state.Storyboard.Children.RemoveAt(i);
							storyboard.Children.Add(timeline);
						}
					}
					ExtendedVisualStateManager.SetLayoutStoryboard(state.Storyboard, storyboard);
				}
			}
			if (storyboard == null)
			{
				return new Storyboard();
			}
			return storyboard;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000044B8 File Offset: 0x000026B8
		private static List<FrameworkElement> FindTargetElements(Control control, FrameworkElement templateRoot, Storyboard layoutStoryboard, List<ExtendedVisualStateManager.OriginalLayoutValueRecord> originalValueRecords, List<FrameworkElement> movingElements)
		{
			List<FrameworkElement> list = new List<FrameworkElement>();
			if (movingElements != null)
			{
				list.AddRange(movingElements);
			}
			foreach (Timeline timeline in layoutStoryboard.Children)
			{
				FrameworkElement frameworkElement = (FrameworkElement)ExtendedVisualStateManager.GetTimelineTarget(control, templateRoot, timeline);
				if (frameworkElement != null)
				{
					if (!list.Contains(frameworkElement))
					{
						list.Add(frameworkElement);
					}
					if (ExtendedVisualStateManager.ChildAffectingLayoutProperties.Contains(ExtendedVisualStateManager.LayoutPropertyFromTimeline(timeline, false)))
					{
						Panel panel = frameworkElement as Panel;
						if (panel != null)
						{
							foreach (UIElement uielement in panel.Children)
							{
								FrameworkElement frameworkElement2 = (FrameworkElement)uielement;
								if (!list.Contains(frameworkElement2) && !(frameworkElement2 is ExtendedVisualStateManager.WrapperCanvas))
								{
									list.Add(frameworkElement2);
								}
							}
						}
					}
				}
			}
			foreach (ExtendedVisualStateManager.OriginalLayoutValueRecord originalLayoutValueRecord in originalValueRecords)
			{
				if (!list.Contains(originalLayoutValueRecord.Element))
				{
					list.Add(originalLayoutValueRecord.Element);
				}
				if (ExtendedVisualStateManager.ChildAffectingLayoutProperties.Contains(originalLayoutValueRecord.Property))
				{
					Panel panel2 = originalLayoutValueRecord.Element as Panel;
					if (panel2 != null)
					{
						foreach (UIElement uielement2 in panel2.Children)
						{
							FrameworkElement frameworkElement3 = (FrameworkElement)uielement2;
							if (!list.Contains(frameworkElement3) && !(frameworkElement3 is ExtendedVisualStateManager.WrapperCanvas))
							{
								list.Add(frameworkElement3);
							}
						}
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				FrameworkElement frameworkElement4 = list[i];
				FrameworkElement frameworkElement5 = VisualTreeHelper.GetParent(frameworkElement4) as FrameworkElement;
				if (movingElements != null && movingElements.Contains(frameworkElement4) && frameworkElement5 is ExtendedVisualStateManager.WrapperCanvas)
				{
					frameworkElement5 = (VisualTreeHelper.GetParent(frameworkElement5) as FrameworkElement);
				}
				if (frameworkElement5 != null)
				{
					if (!list.Contains(frameworkElement5))
					{
						list.Add(frameworkElement5);
					}
					for (int j = 0; j < VisualTreeHelper.GetChildrenCount(frameworkElement5); j++)
					{
						FrameworkElement frameworkElement6 = VisualTreeHelper.GetChild(frameworkElement5, j) as FrameworkElement;
						if (frameworkElement6 != null && !list.Contains(frameworkElement6) && !(frameworkElement6 is ExtendedVisualStateManager.WrapperCanvas))
						{
							list.Add(frameworkElement6);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004744 File Offset: 0x00002944
		private static object GetTimelineTarget(Control control, FrameworkElement templateRoot, Timeline timeline)
		{
			string targetName = Storyboard.GetTargetName(timeline);
			if (string.IsNullOrEmpty(targetName))
			{
				return null;
			}
			if (control is UserControl)
			{
				return control.FindName(targetName);
			}
			return templateRoot.FindName(targetName);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000477C File Offset: 0x0000297C
		private static Dictionary<FrameworkElement, Rect> GetRectsOfTargets(List<FrameworkElement> targets, List<FrameworkElement> movingElements)
		{
			Dictionary<FrameworkElement, Rect> dictionary = new Dictionary<FrameworkElement, Rect>();
			foreach (FrameworkElement frameworkElement in targets)
			{
				Rect layoutRect;
				if (movingElements != null && movingElements.Contains(frameworkElement) && frameworkElement.Parent is ExtendedVisualStateManager.WrapperCanvas)
				{
					ExtendedVisualStateManager.WrapperCanvas wrapperCanvas = frameworkElement.Parent as ExtendedVisualStateManager.WrapperCanvas;
					layoutRect = ExtendedVisualStateManager.GetLayoutRect(wrapperCanvas);
					TranslateTransform translateTransform = wrapperCanvas.RenderTransform as TranslateTransform;
					double left = Canvas.GetLeft(frameworkElement);
					double top = Canvas.GetTop(frameworkElement);
					layoutRect..ctor(layoutRect.Left + (double.IsNaN(left) ? 0.0 : left) + ((translateTransform == null) ? 0.0 : translateTransform.X), layoutRect.Top + (double.IsNaN(top) ? 0.0 : top) + ((translateTransform == null) ? 0.0 : translateTransform.Y), frameworkElement.ActualWidth, frameworkElement.ActualHeight);
				}
				else
				{
					layoutRect = ExtendedVisualStateManager.GetLayoutRect(frameworkElement);
				}
				dictionary.Add(frameworkElement, layoutRect);
			}
			return dictionary;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000048B4 File Offset: 0x00002AB4
		internal static Rect GetLayoutRect(FrameworkElement element)
		{
			double num = element.ActualWidth;
			double num2 = element.ActualHeight;
			if (element is Image || element is MediaElement)
			{
				if (element.Parent.GetType() == typeof(Canvas))
				{
					num = (double.IsNaN(element.Width) ? num : element.Width);
					num2 = (double.IsNaN(element.Height) ? num2 : element.Height);
				}
				else
				{
					num = element.RenderSize.Width;
					num2 = element.RenderSize.Height;
				}
			}
			num = ((element.Visibility == 1) ? 0.0 : num);
			num2 = ((element.Visibility == 1) ? 0.0 : num2);
			Thickness margin = element.Margin;
			Rect layoutSlot = LayoutInformation.GetLayoutSlot(element);
			if (element.Parent is Canvas)
			{
				layoutSlot..ctor(Canvas.GetLeft(element), Canvas.GetTop(element), num, num2);
			}
			double num3 = 0.0;
			double num4 = 0.0;
			switch (element.HorizontalAlignment)
			{
			case 0:
				num3 = layoutSlot.Left + margin.Left;
				break;
			case 1:
				num3 = (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - num / 2.0;
				break;
			case 2:
				num3 = layoutSlot.Right - margin.Right - num;
				break;
			case 3:
				num3 = Math.Max(layoutSlot.Left + margin.Left, (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - num / 2.0);
				break;
			}
			switch (element.VerticalAlignment)
			{
			case 0:
				num4 = layoutSlot.Top + margin.Top;
				break;
			case 1:
				num4 = (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - num2 / 2.0;
				break;
			case 2:
				num4 = layoutSlot.Bottom - margin.Bottom - num2;
				break;
			case 3:
				num4 = Math.Max(layoutSlot.Top + margin.Top, (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - num2 / 2.0);
				break;
			}
			return new Rect(num3, num4, num, num2);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004B70 File Offset: 0x00002D70
		private static Dictionary<FrameworkElement, double> GetOldOpacities(Control control, FrameworkElement templateRoot, Storyboard layoutStoryboard, List<ExtendedVisualStateManager.OriginalLayoutValueRecord> originalValueRecords, List<FrameworkElement> movingElements)
		{
			Dictionary<FrameworkElement, double> dictionary = new Dictionary<FrameworkElement, double>();
			if (movingElements != null)
			{
				foreach (FrameworkElement frameworkElement in movingElements)
				{
					ExtendedVisualStateManager.WrapperCanvas wrapperCanvas = frameworkElement.Parent as ExtendedVisualStateManager.WrapperCanvas;
					if (wrapperCanvas != null)
					{
						dictionary.Add(frameworkElement, wrapperCanvas.Opacity);
					}
				}
			}
			for (int i = originalValueRecords.Count - 1; i >= 0; i--)
			{
				ExtendedVisualStateManager.OriginalLayoutValueRecord originalLayoutValueRecord = originalValueRecords[i];
				double num;
				if (ExtendedVisualStateManager.IsVisibilityProperty(originalLayoutValueRecord.Property) && !dictionary.TryGetValue(originalLayoutValueRecord.Element, ref num))
				{
					num = (((Visibility)originalLayoutValueRecord.Element.GetValue(originalLayoutValueRecord.Property) == null) ? 1.0 : 0.0);
					dictionary.Add(originalLayoutValueRecord.Element, num);
				}
			}
			foreach (Timeline timeline in layoutStoryboard.Children)
			{
				FrameworkElement frameworkElement2 = (FrameworkElement)ExtendedVisualStateManager.GetTimelineTarget(control, templateRoot, timeline);
				DependencyProperty dependencyProperty = ExtendedVisualStateManager.LayoutPropertyFromTimeline(timeline, true);
				double num2;
				if (frameworkElement2 != null && ExtendedVisualStateManager.IsVisibilityProperty(dependencyProperty) && !dictionary.TryGetValue(frameworkElement2, ref num2))
				{
					num2 = (((Visibility)frameworkElement2.GetValue(dependencyProperty) == null) ? 1.0 : 0.0);
					dictionary.Add(frameworkElement2, num2);
				}
			}
			return dictionary;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004CF8 File Offset: 0x00002EF8
		private static void SetLayoutStoryboardProperties(Control control, FrameworkElement templateRoot, Storyboard layoutStoryboard, List<ExtendedVisualStateManager.OriginalLayoutValueRecord> originalValueRecords)
		{
			foreach (ExtendedVisualStateManager.OriginalLayoutValueRecord originalLayoutValueRecord in originalValueRecords)
			{
				ExtendedVisualStateManager.ReplaceCachedLocalValueHelper(originalLayoutValueRecord.Element, originalLayoutValueRecord.Property, originalLayoutValueRecord.Value);
			}
			originalValueRecords.Clear();
			foreach (Timeline timeline in layoutStoryboard.Children)
			{
				FrameworkElement frameworkElement = (FrameworkElement)ExtendedVisualStateManager.GetTimelineTarget(control, templateRoot, timeline);
				DependencyProperty dependencyProperty = ExtendedVisualStateManager.LayoutPropertyFromTimeline(timeline, true);
				if (frameworkElement != null && dependencyProperty != null)
				{
					object obj = null;
					bool flag = false;
					ObjectAnimationUsingKeyFrames objectAnimationUsingKeyFrames = timeline as ObjectAnimationUsingKeyFrames;
					if (objectAnimationUsingKeyFrames != null)
					{
						flag = true;
						obj = objectAnimationUsingKeyFrames.KeyFrames[0].Value;
					}
					else
					{
						DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = timeline as DoubleAnimationUsingKeyFrames;
						if (doubleAnimationUsingKeyFrames != null)
						{
							flag = true;
							obj = doubleAnimationUsingKeyFrames.KeyFrames[0].Value;
						}
						else
						{
							DoubleAnimation doubleAnimation = timeline as DoubleAnimation;
							if (doubleAnimation != null)
							{
								flag = true;
								obj = doubleAnimation.To;
							}
						}
					}
					if (ExtendedVisualStateManager.IsVisibilityProperty(dependencyProperty) && !(obj is Visibility))
					{
						obj = Enum.Parse(typeof(Visibility), obj.ToString(), true);
					}
					else if (dependencyProperty == FrameworkElement.HorizontalAlignmentProperty && !(obj is HorizontalAlignment))
					{
						obj = Enum.Parse(typeof(HorizontalAlignment), obj.ToString(), true);
					}
					else if (dependencyProperty == FrameworkElement.VerticalAlignmentProperty && !(obj is VerticalAlignment))
					{
						obj = Enum.Parse(typeof(VerticalAlignment), obj.ToString(), true);
					}
					else if (dependencyProperty == StackPanel.OrientationProperty && !(obj is Orientation))
					{
						obj = Enum.Parse(typeof(Orientation), obj.ToString(), true);
					}
					else if (dependencyProperty == FrameworkElement.MarginProperty && !(obj is Thickness))
					{
						string[] array = obj.ToString().Split(new char[]
						{
							','
						});
						double num = double.Parse(array[0], CultureInfo.InvariantCulture);
						double num2 = (array.Length < 1) ? num : double.Parse(array[1], CultureInfo.InvariantCulture);
						double num3 = (array.Length < 2) ? num : double.Parse(array[2], CultureInfo.InvariantCulture);
						double num4 = (array.Length < 3) ? num2 : double.Parse(array[3], CultureInfo.InvariantCulture);
						obj = new Thickness(num, num2, num3, num4);
					}
					if ((dependencyProperty == FrameworkElement.WidthProperty || dependencyProperty == FrameworkElement.HeightProperty) && (double)obj == -1.0)
					{
						obj = double.NaN;
					}
					if (flag)
					{
						originalValueRecords.Add(new ExtendedVisualStateManager.OriginalLayoutValueRecord
						{
							Element = frameworkElement,
							Property = dependencyProperty,
							Value = ExtendedVisualStateManager.CacheLocalValueHelper(frameworkElement, dependencyProperty)
						});
						frameworkElement.SetValue(dependencyProperty, obj);
					}
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000501C File Offset: 0x0000321C
		private static void WrapMovingElementsInCanvases(List<FrameworkElement> movingElements, Dictionary<FrameworkElement, Rect> oldRects, Dictionary<FrameworkElement, Rect> newRects)
		{
			foreach (FrameworkElement frameworkElement in movingElements)
			{
				FrameworkElement frameworkElement2 = VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement;
				ExtendedVisualStateManager.WrapperCanvas wrapperCanvas = new ExtendedVisualStateManager.WrapperCanvas();
				wrapperCanvas.OldRect = oldRects[frameworkElement];
				wrapperCanvas.NewRect = newRects[frameworkElement];
				foreach (DependencyProperty dependencyProperty in ExtendedVisualStateManager.LayoutProperties)
				{
					if (!ExtendedVisualStateManager.ChildAffectingLayoutProperties.Contains(dependencyProperty))
					{
						object obj = ExtendedVisualStateManager.CacheLocalValueHelper(frameworkElement, dependencyProperty);
						if (obj is TemplateBindingExpression)
						{
							return;
						}
					}
				}
				bool flag = true;
				Panel panel = frameworkElement2 as Panel;
				if (panel != null && !panel.IsItemsHost)
				{
					int num = panel.Children.IndexOf(frameworkElement);
					panel.Children.RemoveAt(num);
					panel.Children.Insert(num, wrapperCanvas);
				}
				else
				{
					Border border = frameworkElement2 as Border;
					if (border != null)
					{
						border.Child = wrapperCanvas;
					}
					else
					{
						flag = false;
					}
				}
				if (flag)
				{
					wrapperCanvas.Children.Add(frameworkElement);
					ExtendedVisualStateManager.CopyLayoutProperties(frameworkElement, wrapperCanvas, false);
				}
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005184 File Offset: 0x00003384
		private static void UnwrapMovingElementsFromCanvases(List<FrameworkElement> movingElements)
		{
			foreach (FrameworkElement frameworkElement in movingElements)
			{
				ExtendedVisualStateManager.WrapperCanvas wrapperCanvas = frameworkElement.Parent as ExtendedVisualStateManager.WrapperCanvas;
				if (wrapperCanvas != null)
				{
					FrameworkElement frameworkElement2 = VisualTreeHelper.GetParent(wrapperCanvas) as FrameworkElement;
					wrapperCanvas.Children.Remove(frameworkElement);
					Panel panel = frameworkElement2 as Panel;
					if (panel != null)
					{
						int num = panel.Children.IndexOf(wrapperCanvas);
						panel.Children.RemoveAt(num);
						panel.Children.Insert(num, frameworkElement);
					}
					else
					{
						Border border = frameworkElement2 as Border;
						if (border != null)
						{
							border.Child = frameworkElement;
						}
					}
					ExtendedVisualStateManager.CopyLayoutProperties(wrapperCanvas, frameworkElement, true);
				}
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000524C File Offset: 0x0000344C
		private static void CopyLayoutProperties(FrameworkElement source, FrameworkElement target, bool restoring)
		{
			ExtendedVisualStateManager.WrapperCanvas wrapperCanvas = (restoring ? source : target) as ExtendedVisualStateManager.WrapperCanvas;
			if (wrapperCanvas.LocalValueCache == null)
			{
				wrapperCanvas.LocalValueCache = new Dictionary<DependencyProperty, object>();
			}
			foreach (DependencyProperty dependencyProperty in ExtendedVisualStateManager.LayoutProperties)
			{
				if (!ExtendedVisualStateManager.ChildAffectingLayoutProperties.Contains(dependencyProperty))
				{
					if (restoring)
					{
						ExtendedVisualStateManager.ReplaceCachedLocalValueHelper(target, dependencyProperty, wrapperCanvas.LocalValueCache[dependencyProperty]);
					}
					else
					{
						object value = target.GetValue(dependencyProperty);
						object obj = ExtendedVisualStateManager.CacheLocalValueHelper(source, dependencyProperty);
						wrapperCanvas.LocalValueCache[dependencyProperty] = obj;
						if (obj is TemplateBindingExpression)
						{
							string text = null;
							foreach (KeyValuePair<string, DependencyProperty> keyValuePair in ExtendedVisualStateManager.PathToPropertyMap)
							{
								if (keyValuePair.Value == dependencyProperty || (dependencyProperty == ExtendedVisualStateManager.RuntimeVisibility && keyValuePair.Value == UIElement.VisibilityProperty))
								{
									text = keyValuePair.Key;
								}
							}
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.ChangePropertyActionCannotSetValueExceptionMessage, new object[]
							{
								typeof(TemplateBindingExpression).Name,
								text,
								((value == null) ? typeof(object) : value.GetType()).Name
							}));
						}
						if (ExtendedVisualStateManager.IsVisibilityProperty(dependencyProperty))
						{
							wrapperCanvas.DestinationVisibilityCache = (Visibility)source.GetValue(dependencyProperty);
						}
						else
						{
							target.SetValue(dependencyProperty, source.GetValue(dependencyProperty));
						}
						source.SetValue(dependencyProperty, value);
					}
				}
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000541C File Offset: 0x0000361C
		private static Storyboard CreateLayoutTransitionStoryboard(VisualTransition transition, List<FrameworkElement> movingElements, Dictionary<FrameworkElement, double> oldOpacities)
		{
			Duration duration = (transition != null) ? transition.GeneratedDuration : new Duration(TimeSpan.Zero);
			IEasingFunction easingFunction = (transition != null) ? transition.GeneratedEasingFunction : null;
			Storyboard storyboard = new Storyboard();
			storyboard.Duration = duration;
			foreach (FrameworkElement frameworkElement in movingElements)
			{
				ExtendedVisualStateManager.WrapperCanvas wrapperCanvas = frameworkElement.Parent as ExtendedVisualStateManager.WrapperCanvas;
				if (wrapperCanvas != null)
				{
					DoubleAnimation doubleAnimation = new DoubleAnimation
					{
						From = new double?(1.0),
						To = new double?(0.0),
						Duration = duration
					};
					doubleAnimation.EasingFunction = easingFunction;
					Storyboard.SetTarget(doubleAnimation, wrapperCanvas);
					Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(ExtendedVisualStateManager.WrapperCanvas.SimulationProgressProperty));
					storyboard.Children.Add(doubleAnimation);
					wrapperCanvas.SimulationProgress = 1.0;
					Rect newRect = wrapperCanvas.NewRect;
					if (!ExtendedVisualStateManager.IsClose(wrapperCanvas.Width, newRect.Width))
					{
						DoubleAnimation doubleAnimation2 = new DoubleAnimation
						{
							From = new double?(newRect.Width),
							To = new double?(newRect.Width),
							Duration = duration
						};
						Storyboard.SetTarget(doubleAnimation2, wrapperCanvas);
						Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(FrameworkElement.WidthProperty));
						storyboard.Children.Add(doubleAnimation2);
					}
					if (!ExtendedVisualStateManager.IsClose(wrapperCanvas.Height, newRect.Height))
					{
						DoubleAnimation doubleAnimation3 = new DoubleAnimation
						{
							From = new double?(newRect.Height),
							To = new double?(newRect.Height),
							Duration = duration
						};
						Storyboard.SetTarget(doubleAnimation3, wrapperCanvas);
						Storyboard.SetTargetProperty(doubleAnimation3, new PropertyPath(FrameworkElement.HeightProperty));
						storyboard.Children.Add(doubleAnimation3);
					}
					if (wrapperCanvas.DestinationVisibilityCache == 1)
					{
						Thickness margin = wrapperCanvas.Margin;
						if (!ExtendedVisualStateManager.IsClose(margin.Left, 0.0) || !ExtendedVisualStateManager.IsClose(margin.Top, 0.0) || !ExtendedVisualStateManager.IsClose(margin.Right, 0.0) || !ExtendedVisualStateManager.IsClose(margin.Bottom, 0.0))
						{
							ObjectAnimationUsingKeyFrames objectAnimationUsingKeyFrames = new ObjectAnimationUsingKeyFrames
							{
								Duration = duration
							};
							DiscreteObjectKeyFrame discreteObjectKeyFrame = new DiscreteObjectKeyFrame
							{
								KeyTime = TimeSpan.Zero,
								Value = default(Thickness)
							};
							objectAnimationUsingKeyFrames.KeyFrames.Add(discreteObjectKeyFrame);
							Storyboard.SetTarget(objectAnimationUsingKeyFrames, wrapperCanvas);
							Storyboard.SetTargetProperty(objectAnimationUsingKeyFrames, new PropertyPath(FrameworkElement.MarginProperty));
							storyboard.Children.Add(objectAnimationUsingKeyFrames);
						}
						if (!ExtendedVisualStateManager.IsClose(wrapperCanvas.MinWidth, 0.0))
						{
							DoubleAnimation doubleAnimation4 = new DoubleAnimation
							{
								From = new double?(0.0),
								To = new double?(0.0),
								Duration = duration
							};
							Storyboard.SetTarget(doubleAnimation4, wrapperCanvas);
							Storyboard.SetTargetProperty(doubleAnimation4, new PropertyPath(FrameworkElement.MinWidthProperty));
							storyboard.Children.Add(doubleAnimation4);
						}
						if (!ExtendedVisualStateManager.IsClose(wrapperCanvas.MinHeight, 0.0))
						{
							DoubleAnimation doubleAnimation5 = new DoubleAnimation
							{
								From = new double?(0.0),
								To = new double?(0.0),
								Duration = duration
							};
							Storyboard.SetTarget(doubleAnimation5, wrapperCanvas);
							Storyboard.SetTargetProperty(doubleAnimation5, new PropertyPath(FrameworkElement.MinHeightProperty));
							storyboard.Children.Add(doubleAnimation5);
						}
					}
				}
			}
			foreach (FrameworkElement frameworkElement2 in oldOpacities.Keys)
			{
				ExtendedVisualStateManager.WrapperCanvas wrapperCanvas2 = frameworkElement2.Parent as ExtendedVisualStateManager.WrapperCanvas;
				if (wrapperCanvas2 != null)
				{
					double num = oldOpacities[frameworkElement2];
					double num2 = (wrapperCanvas2.DestinationVisibilityCache == null) ? 1.0 : 0.0;
					if (!ExtendedVisualStateManager.IsClose(num, 1.0) || !ExtendedVisualStateManager.IsClose(num2, 1.0))
					{
						DoubleAnimation doubleAnimation6 = new DoubleAnimation
						{
							From = new double?(num),
							To = new double?(num2),
							Duration = duration
						};
						doubleAnimation6.EasingFunction = easingFunction;
						Storyboard.SetTarget(doubleAnimation6, wrapperCanvas2);
						Storyboard.SetTargetProperty(doubleAnimation6, new PropertyPath(UIElement.OpacityProperty));
						storyboard.Children.Add(doubleAnimation6);
					}
				}
			}
			return storyboard;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000591C File Offset: 0x00003B1C
		private static object CacheLocalValueHelper(DependencyObject dependencyObject, DependencyProperty property)
		{
			return dependencyObject.ReadLocalValue(property);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005928 File Offset: 0x00003B28
		private static void ReplaceCachedLocalValueHelper(FrameworkElement element, DependencyProperty property, object value)
		{
			if (value == DependencyProperty.UnsetValue)
			{
				element.ClearValue(property);
				return;
			}
			BindingExpression bindingExpression = value as BindingExpression;
			if (bindingExpression != null)
			{
				element.SetBinding(property, bindingExpression.ParentBinding);
				return;
			}
			element.SetValue(property, value);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005966 File Offset: 0x00003B66
		private static bool IsClose(double a, double b)
		{
			return Math.Abs(a - b) < 1E-07;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000597C File Offset: 0x00003B7C
		// Note: this type is marked as 'beforefieldinit'.
		static ExtendedVisualStateManager()
		{
			List<DependencyProperty> list = new List<DependencyProperty>();
			list.Add(Grid.ColumnProperty);
			list.Add(Grid.ColumnSpanProperty);
			list.Add(Grid.RowProperty);
			list.Add(Grid.RowSpanProperty);
			list.Add(Canvas.LeftProperty);
			list.Add(Canvas.TopProperty);
			list.Add(FrameworkElement.WidthProperty);
			list.Add(FrameworkElement.HeightProperty);
			list.Add(FrameworkElement.MinWidthProperty);
			list.Add(FrameworkElement.MinHeightProperty);
			list.Add(FrameworkElement.MaxWidthProperty);
			list.Add(FrameworkElement.MaxHeightProperty);
			list.Add(FrameworkElement.MarginProperty);
			list.Add(FrameworkElement.HorizontalAlignmentProperty);
			list.Add(FrameworkElement.VerticalAlignmentProperty);
			list.Add(UIElement.VisibilityProperty);
			list.Add(StackPanel.OrientationProperty);
			ExtendedVisualStateManager.LayoutProperties = list;
			List<DependencyProperty> list2 = new List<DependencyProperty>();
			list2.Add(StackPanel.OrientationProperty);
			ExtendedVisualStateManager.ChildAffectingLayoutProperties = list2;
			Dictionary<string, DependencyProperty> dictionary = new Dictionary<string, DependencyProperty>();
			dictionary.Add("(Grid.Column)", Grid.ColumnProperty);
			dictionary.Add("(Grid.ColumnSpan)", Grid.ColumnSpanProperty);
			dictionary.Add("(Grid.Row)", Grid.RowProperty);
			dictionary.Add("(Grid.RowSpan)", Grid.RowSpanProperty);
			dictionary.Add("(Canvas.Left)", Canvas.LeftProperty);
			dictionary.Add("(Canvas.Top)", Canvas.TopProperty);
			dictionary.Add("(FrameworkElement.Width)", FrameworkElement.WidthProperty);
			dictionary.Add("(FrameworkElement.Height)", FrameworkElement.HeightProperty);
			dictionary.Add("(FrameworkElement.MinWidth)", FrameworkElement.MinWidthProperty);
			dictionary.Add("(FrameworkElement.MinHeight)", FrameworkElement.MinHeightProperty);
			dictionary.Add("(FrameworkElement.MaxWidth)", FrameworkElement.MaxWidthProperty);
			dictionary.Add("(FrameworkElement.MaxHeight)", FrameworkElement.MaxHeightProperty);
			dictionary.Add("(FrameworkElement.Margin)", FrameworkElement.MarginProperty);
			dictionary.Add("(FrameworkElement.HorizontalAlignment)", FrameworkElement.HorizontalAlignmentProperty);
			dictionary.Add("(FrameworkElement.VerticalAlignment)", FrameworkElement.VerticalAlignmentProperty);
			dictionary.Add("(UIElement.Visibility)", UIElement.VisibilityProperty);
			dictionary.Add("(Control.Width)", FrameworkElement.WidthProperty);
			dictionary.Add("(Control.Height)", FrameworkElement.HeightProperty);
			dictionary.Add("(Control.MinWidth)", FrameworkElement.MinWidthProperty);
			dictionary.Add("(Control.MinHeight)", FrameworkElement.MinHeightProperty);
			dictionary.Add("(Control.MaxWidth)", FrameworkElement.MaxWidthProperty);
			dictionary.Add("(Control.MaxHeight)", FrameworkElement.MaxHeightProperty);
			dictionary.Add("(Control.Margin)", FrameworkElement.MarginProperty);
			dictionary.Add("(Control.HorizontalAlignment)", FrameworkElement.HorizontalAlignmentProperty);
			dictionary.Add("(Control.VerticalAlignment)", FrameworkElement.VerticalAlignmentProperty);
			dictionary.Add("(Control.Visibility)", UIElement.VisibilityProperty);
			dictionary.Add("(StackPanel.Orientation)", StackPanel.OrientationProperty);
			ExtendedVisualStateManager.PathToPropertyMap = dictionary;
		}

		// Token: 0x04000027 RID: 39
		public static readonly DependencyProperty UseFluidLayoutProperty = DependencyProperty.RegisterAttached("UseFluidLayout", typeof(bool), typeof(ExtendedVisualStateManager), new PropertyMetadata(false));

		// Token: 0x04000028 RID: 40
		public static readonly DependencyProperty RuntimeVisibilityPropertyProperty = DependencyProperty.RegisterAttached("RuntimeVisibilityProperty", typeof(DependencyProperty), typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

		// Token: 0x04000029 RID: 41
		internal static readonly DependencyProperty OriginalLayoutValuesProperty = DependencyProperty.RegisterAttached("OriginalLayoutValues", typeof(List<ExtendedVisualStateManager.OriginalLayoutValueRecord>), typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

		// Token: 0x0400002A RID: 42
		internal static readonly DependencyProperty LayoutStoryboardProperty = DependencyProperty.RegisterAttached("LayoutStoryboard", typeof(Storyboard), typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

		// Token: 0x0400002B RID: 43
		internal static readonly DependencyProperty CurrentStateProperty = DependencyProperty.RegisterAttached("CurrentState", typeof(VisualState), typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

		// Token: 0x0400002C RID: 44
		private static List<FrameworkElement> MovingElements;

		// Token: 0x0400002D RID: 45
		private static Storyboard LayoutTransitionStoryboard;

		// Token: 0x0400002E RID: 46
		private static List<DependencyProperty> LayoutProperties;

		// Token: 0x0400002F RID: 47
		private static List<DependencyProperty> ChildAffectingLayoutProperties;

		// Token: 0x04000030 RID: 48
		private static DependencyProperty RuntimeVisibility;

		// Token: 0x04000031 RID: 49
		private static Dictionary<string, DependencyProperty> PathToPropertyMap;

		// Token: 0x02000014 RID: 20
		internal class WrapperCanvas : Canvas
		{
			// Token: 0x1700001E RID: 30
			// (get) Token: 0x060000A7 RID: 167 RVA: 0x00005CFD File Offset: 0x00003EFD
			// (set) Token: 0x060000A8 RID: 168 RVA: 0x00005D05 File Offset: 0x00003F05
			public Rect OldRect { get; set; }

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x060000A9 RID: 169 RVA: 0x00005D0E File Offset: 0x00003F0E
			// (set) Token: 0x060000AA RID: 170 RVA: 0x00005D16 File Offset: 0x00003F16
			public Rect NewRect { get; set; }

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x060000AB RID: 171 RVA: 0x00005D1F File Offset: 0x00003F1F
			// (set) Token: 0x060000AC RID: 172 RVA: 0x00005D27 File Offset: 0x00003F27
			public Dictionary<DependencyProperty, object> LocalValueCache { get; set; }

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x060000AD RID: 173 RVA: 0x00005D30 File Offset: 0x00003F30
			// (set) Token: 0x060000AE RID: 174 RVA: 0x00005D38 File Offset: 0x00003F38
			public Visibility DestinationVisibilityCache { get; set; }

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x060000AF RID: 175 RVA: 0x00005D41 File Offset: 0x00003F41
			// (set) Token: 0x060000B0 RID: 176 RVA: 0x00005D53 File Offset: 0x00003F53
			public double SimulationProgress
			{
				get
				{
					return (double)base.GetValue(ExtendedVisualStateManager.WrapperCanvas.SimulationProgressProperty);
				}
				set
				{
					base.SetValue(ExtendedVisualStateManager.WrapperCanvas.SimulationProgressProperty, value);
				}
			}

			// Token: 0x060000B1 RID: 177 RVA: 0x00005D68 File Offset: 0x00003F68
			private static void SimulationProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
			{
				ExtendedVisualStateManager.WrapperCanvas wrapperCanvas = d as ExtendedVisualStateManager.WrapperCanvas;
				double num = (double)e.NewValue;
				if (wrapperCanvas != null && wrapperCanvas.Children.Count > 0)
				{
					FrameworkElement frameworkElement = wrapperCanvas.Children[0] as FrameworkElement;
					frameworkElement.Width = Math.Max(0.0, wrapperCanvas.OldRect.Width * num + wrapperCanvas.NewRect.Width * (1.0 - num));
					frameworkElement.Height = Math.Max(0.0, wrapperCanvas.OldRect.Height * num + wrapperCanvas.NewRect.Height * (1.0 - num));
					Canvas.SetLeft(frameworkElement, num * (wrapperCanvas.OldRect.Left - wrapperCanvas.NewRect.Left));
					Canvas.SetTop(frameworkElement, num * (wrapperCanvas.OldRect.Top - wrapperCanvas.NewRect.Top));
				}
			}

			// Token: 0x04000032 RID: 50
			internal static readonly DependencyProperty SimulationProgressProperty = DependencyProperty.Register("SimulationProgress", typeof(double), typeof(ExtendedVisualStateManager.WrapperCanvas), new PropertyMetadata(0.0, new PropertyChangedCallback(ExtendedVisualStateManager.WrapperCanvas.SimulationProgressChanged)));
		}

		// Token: 0x02000015 RID: 21
		internal class OriginalLayoutValueRecord
		{
			// Token: 0x17000023 RID: 35
			// (get) Token: 0x060000B4 RID: 180 RVA: 0x00005EDB File Offset: 0x000040DB
			// (set) Token: 0x060000B5 RID: 181 RVA: 0x00005EE3 File Offset: 0x000040E3
			public FrameworkElement Element { get; set; }

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x060000B6 RID: 182 RVA: 0x00005EEC File Offset: 0x000040EC
			// (set) Token: 0x060000B7 RID: 183 RVA: 0x00005EF4 File Offset: 0x000040F4
			public DependencyProperty Property { get; set; }

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x060000B8 RID: 184 RVA: 0x00005EFD File Offset: 0x000040FD
			// (set) Token: 0x060000B9 RID: 185 RVA: 0x00005F05 File Offset: 0x00004105
			public object Value { get; set; }
		}
	}
}
