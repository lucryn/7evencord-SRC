using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Resources;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200002C RID: 44
	internal static class Transitions
	{
		// Token: 0x06000160 RID: 352 RVA: 0x00006F88 File Offset: 0x00005188
		private static ITransition GetEnumStoryboard<T>(UIElement element, string name, T mode)
		{
			string name2 = name + Enum.GetName(typeof(T), mode);
			Storyboard storyboard = Transitions.GetStoryboard(name2);
			if (storyboard == null)
			{
				return null;
			}
			Storyboard.SetTarget(storyboard, element);
			return new Transition(element, storyboard);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006FCC File Offset: 0x000051CC
		private static Storyboard GetStoryboard(string name)
		{
			if (Transitions._storyboardXamlCache == null)
			{
				Transitions._storyboardXamlCache = new Dictionary<string, string>();
			}
			string text = null;
			if (Transitions._storyboardXamlCache.ContainsKey(name))
			{
				text = Transitions._storyboardXamlCache[name];
			}
			else
			{
				string text2 = "/Microsoft.Phone.Controls.Toolkit;component/Transitions/Storyboards/" + name + ".xaml";
				Uri uri = new Uri(text2, 2);
				StreamResourceInfo resourceStream = Application.GetResourceStream(uri);
				using (StreamReader streamReader = new StreamReader(resourceStream.Stream))
				{
					text = streamReader.ReadToEnd();
					Transitions._storyboardXamlCache[name] = text;
				}
			}
			return XamlReader.Load(text) as Storyboard;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007074 File Offset: 0x00005274
		public static ITransition Roll(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			Storyboard storyboard = Transitions.GetStoryboard("Roll");
			Storyboard.SetTarget(storyboard, element);
			element.Projection = new PlaneProjection
			{
				CenterOfRotationX = 0.5,
				CenterOfRotationY = 0.5
			};
			return new Transition(element, storyboard);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000070D4 File Offset: 0x000052D4
		public static ITransition Rotate(UIElement element, RotateTransitionMode rotateTransitionMode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (!Enum.IsDefined(typeof(RotateTransitionMode), rotateTransitionMode))
			{
				throw new ArgumentOutOfRangeException("rotateTransitionMode");
			}
			element.Projection = new PlaneProjection
			{
				CenterOfRotationX = 0.5,
				CenterOfRotationY = 0.5
			};
			return Transitions.GetEnumStoryboard<RotateTransitionMode>(element, "Rotate", rotateTransitionMode);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007148 File Offset: 0x00005348
		public static ITransition Slide(UIElement element, SlideTransitionMode slideTransitionMode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (!Enum.IsDefined(typeof(SlideTransitionMode), slideTransitionMode))
			{
				throw new ArgumentOutOfRangeException("slideTransitionMode");
			}
			element.RenderTransform = new TranslateTransform();
			return Transitions.GetEnumStoryboard<SlideTransitionMode>(element, string.Empty, slideTransitionMode);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000719C File Offset: 0x0000539C
		public static ITransition Swivel(UIElement element, SwivelTransitionMode swivelTransitionMode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (!Enum.IsDefined(typeof(SwivelTransitionMode), swivelTransitionMode))
			{
				throw new ArgumentOutOfRangeException("swivelTransitionMode");
			}
			element.Projection = new PlaneProjection();
			return Transitions.GetEnumStoryboard<SwivelTransitionMode>(element, "Swivel", swivelTransitionMode);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000071F0 File Offset: 0x000053F0
		public static ITransition Turnstile(UIElement element, TurnstileTransitionMode turnstileTransitionMode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (!Enum.IsDefined(typeof(TurnstileTransitionMode), turnstileTransitionMode))
			{
				throw new ArgumentOutOfRangeException("turnstileTransitionMode");
			}
			element.Projection = new PlaneProjection
			{
				CenterOfRotationX = 0.0
			};
			return Transitions.GetEnumStoryboard<TurnstileTransitionMode>(element, "Turnstile", turnstileTransitionMode);
		}

		// Token: 0x04000088 RID: 136
		private static Dictionary<string, string> _storyboardXamlCache;
	}
}
