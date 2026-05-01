using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000002 RID: 2
	internal sealed class OpacityAnimator
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002150 File Offset: 0x00001150
		public OpacityAnimator(UIElement target)
		{
			this._sbRunning.Completed += new EventHandler(this.OnCompleted);
			this._sbRunning.Children.Add(this._daRunning);
			Storyboard.SetTarget(this._daRunning, target);
			Storyboard.SetTargetProperty(this._daRunning, OpacityAnimator.OpacityPropertyPath);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000021C2 File Offset: 0x000011C2
		public void GoTo(double targetOpacity, Duration duration)
		{
			this.GoTo(targetOpacity, duration, null, null);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000021CE File Offset: 0x000011CE
		public void GoTo(double targetOpacity, Duration duration, Action completionAction)
		{
			this.GoTo(targetOpacity, duration, null, completionAction);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000021DC File Offset: 0x000011DC
		public void GoTo(double targetOpacity, Duration duration, IEasingFunction easingFunction, Action completionAction)
		{
			this._daRunning.To = new double?(targetOpacity);
			this._daRunning.Duration = duration;
			this._daRunning.EasingFunction = easingFunction;
			this._sbRunning.Begin();
			this._suppressChangeNotification = true;
			this._sbRunning.SeekAlignedToLastTick(TimeSpan.Zero);
			this._oneTimeAction = completionAction;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000223C File Offset: 0x0000123C
		private void OnCompleted(object sender, EventArgs e)
		{
			Action oneTimeAction = this._oneTimeAction;
			if (oneTimeAction != null)
			{
				this._oneTimeAction = null;
				oneTimeAction.Invoke();
			}
			if (!this._suppressChangeNotification)
			{
				this._suppressChangeNotification = false;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000226F File Offset: 0x0000126F
		public static void EnsureAnimator(UIElement targetElement, ref OpacityAnimator animator)
		{
			if (animator == null)
			{
				animator = new OpacityAnimator(targetElement);
			}
			if (animator == null)
			{
				throw new InvalidOperationException("The animation system could not be prepared for the target element.");
			}
		}

		// Token: 0x04000001 RID: 1
		private static readonly PropertyPath OpacityPropertyPath = new PropertyPath("Opacity", new object[0]);

		// Token: 0x04000002 RID: 2
		private readonly Storyboard _sbRunning = new Storyboard();

		// Token: 0x04000003 RID: 3
		private readonly DoubleAnimation _daRunning = new DoubleAnimation();

		// Token: 0x04000004 RID: 4
		private bool _suppressChangeNotification;

		// Token: 0x04000005 RID: 5
		private Action _oneTimeAction;
	}
}
