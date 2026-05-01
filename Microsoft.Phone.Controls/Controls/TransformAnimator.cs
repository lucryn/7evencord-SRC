using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000013 RID: 19
	internal sealed class TransformAnimator
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00002B04 File Offset: 0x00001B04
		public TransformAnimator(TranslateTransform translateTransform)
		{
			this._transform = translateTransform;
			this._sbRunning.Completed += new EventHandler(this.OnCompleted);
			this._sbRunning.Children.Add(this._daRunning);
			Storyboard.SetTarget(this._daRunning, this._transform);
			Storyboard.SetTargetProperty(this._daRunning, TransformAnimator.TranslateXPropertyPath);
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002B82 File Offset: 0x00001B82
		public double CurrentOffset
		{
			get
			{
				return this._transform.X;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002B8F File Offset: 0x00001B8F
		public void GoTo(double targetOffset, Duration duration)
		{
			this.GoTo(targetOffset, duration, null, null);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002B9B File Offset: 0x00001B9B
		public void GoTo(double targetOffset, Duration duration, Action completionAction)
		{
			this.GoTo(targetOffset, duration, null, completionAction);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002BA7 File Offset: 0x00001BA7
		public void GoTo(double targetOffset, Duration duration, IEasingFunction easingFunction)
		{
			this.GoTo(targetOffset, duration, easingFunction, null);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002BB4 File Offset: 0x00001BB4
		public void GoTo(double targetOffset, Duration duration, IEasingFunction easingFunction, Action completionAction)
		{
			this._daRunning.To = new double?(targetOffset);
			this._daRunning.Duration = duration;
			this._daRunning.EasingFunction = easingFunction;
			this._sbRunning.Begin();
			this._sbRunning.SeekAlignedToLastTick(TimeSpan.Zero);
			this._oneTimeAction = completionAction;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002C0D File Offset: 0x00001C0D
		public void UpdateEasingFunction(IEasingFunction ease)
		{
			if (this._daRunning != null && this._daRunning.EasingFunction != ease)
			{
				this._daRunning.EasingFunction = ease;
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002C31 File Offset: 0x00001C31
		public void UpdateDuration(Duration duration)
		{
			if (this._daRunning != null)
			{
				this._daRunning.Duration = duration;
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002C48 File Offset: 0x00001C48
		private void OnCompleted(object sender, EventArgs e)
		{
			Action oneTimeAction = this._oneTimeAction;
			if (oneTimeAction != null && this._sbRunning.GetCurrentState() != null)
			{
				this._oneTimeAction = null;
				oneTimeAction.Invoke();
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002C7C File Offset: 0x00001C7C
		public static void EnsureAnimator(FrameworkElement targetElement, ref TransformAnimator animator)
		{
			if (animator == null)
			{
				TranslateTransform translateTransform = TransformAnimator.GetTranslateTransform(targetElement);
				if (translateTransform != null)
				{
					animator = new TransformAnimator(translateTransform);
				}
			}
			if (animator == null)
			{
				throw new InvalidOperationException("The animation system could not be prepared for the target element.");
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002CC4 File Offset: 0x00001CC4
		public static TranslateTransform GetTranslateTransform(UIElement container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			TranslateTransform translateTransform = container.RenderTransform as TranslateTransform;
			if (translateTransform == null)
			{
				if (container.RenderTransform == null)
				{
					translateTransform = new TranslateTransform();
					container.RenderTransform = translateTransform;
				}
				else if (container.RenderTransform is TransformGroup)
				{
					TransformGroup transformGroup = container.RenderTransform as TransformGroup;
					translateTransform = Enumerable.FirstOrDefault<TranslateTransform>(Enumerable.Select<Transform, TranslateTransform>(Enumerable.Where<Transform>(transformGroup.Children, (Transform t) => t is TranslateTransform), (Transform t) => (TranslateTransform)t));
					if (translateTransform == null)
					{
						translateTransform = new TranslateTransform();
						transformGroup.Children.Add(translateTransform);
					}
				}
				else
				{
					TransformGroup transformGroup2 = new TransformGroup();
					Transform renderTransform = container.RenderTransform;
					container.RenderTransform = null;
					transformGroup2.Children.Add(renderTransform);
					translateTransform = new TranslateTransform();
					transformGroup2.Children.Add(translateTransform);
					container.RenderTransform = transformGroup2;
				}
			}
			return translateTransform;
		}

		// Token: 0x04000030 RID: 48
		private static readonly PropertyPath TranslateXPropertyPath = new PropertyPath("X", new object[0]);

		// Token: 0x04000031 RID: 49
		private readonly Storyboard _sbRunning = new Storyboard();

		// Token: 0x04000032 RID: 50
		private readonly DoubleAnimation _daRunning = new DoubleAnimation();

		// Token: 0x04000033 RID: 51
		private TranslateTransform _transform;

		// Token: 0x04000034 RID: 52
		private Action _oneTimeAction;
	}
}
