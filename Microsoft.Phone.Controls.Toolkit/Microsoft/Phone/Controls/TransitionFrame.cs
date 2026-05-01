using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200005A RID: 90
	[TemplatePart(Name = "SecondContentPresenter", Type = typeof(ContentPresenter))]
	[TemplatePart(Name = "FirstContentPresenter", Type = typeof(ContentPresenter))]
	public class TransitionFrame : PhoneApplicationFrame
	{
		// Token: 0x06000344 RID: 836 RVA: 0x0000E858 File Offset: 0x0000CA58
		public TransitionFrame()
		{
			base.DefaultStyleKey = typeof(TransitionFrame);
			base.Navigating += delegate(object s, NavigatingCancelEventArgs e)
			{
				this._isForwardNavigation = (e.NavigationMode != 1);
			};
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000E894 File Offset: 0x0000CA94
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._firstContentPresenter = (base.GetTemplateChild("FirstContentPresenter") as ContentPresenter);
			this._secondContentPresenter = (base.GetTemplateChild("SecondContentPresenter") as ContentPresenter);
			if (base.Content != null)
			{
				this.OnContentChanged(null, base.Content);
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000E994 File Offset: 0x0000CB94
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			UIElement uielement = oldContent as UIElement;
			UIElement uielement2 = newContent as UIElement;
			if (this._firstContentPresenter == null || this._secondContentPresenter == null || uielement2 == null)
			{
				return;
			}
			if (this._useFirstAsNew)
			{
				this._newContentPresenter = this._firstContentPresenter;
				this._oldContentPresenter = this._secondContentPresenter;
			}
			else
			{
				this._newContentPresenter = this._secondContentPresenter;
				this._oldContentPresenter = this._firstContentPresenter;
			}
			this._useFirstAsNew = !this._useFirstAsNew;
			NavigationOutTransition navigationOutTransition = null;
			NavigationInTransition navigationInTransition = null;
			ITransition oldTransition = null;
			ITransition newTransition = null;
			if (uielement != null)
			{
				navigationOutTransition = TransitionService.GetNavigationOutTransition(uielement);
				TransitionElement transitionElement = null;
				if (navigationOutTransition != null)
				{
					transitionElement = (this._isForwardNavigation ? navigationOutTransition.Forward : navigationOutTransition.Backward);
				}
				if (transitionElement != null)
				{
					oldTransition = transitionElement.GetTransition(uielement);
					this._oldContentPresenter.CacheMode = new BitmapCache();
					this._oldContentPresenter.IsHitTestVisible = false;
				}
			}
			if (uielement2 != null)
			{
				navigationInTransition = TransitionService.GetNavigationInTransition(uielement2);
				TransitionElement transitionElement2 = null;
				if (navigationInTransition != null)
				{
					transitionElement2 = (this._isForwardNavigation ? navigationInTransition.Forward : navigationInTransition.Backward);
				}
				if (transitionElement2 != null)
				{
					uielement2.UpdateLayout();
					newTransition = transitionElement2.GetTransition(uielement2);
					this._newContentPresenter.CacheMode = new BitmapCache();
					this._newContentPresenter.IsHitTestVisible = false;
				}
			}
			this._newContentPresenter.Opacity = 0.0;
			this._newContentPresenter.Visibility = 0;
			this._newContentPresenter.Content = uielement2;
			this._oldContentPresenter.Opacity = 1.0;
			this._oldContentPresenter.Visibility = 0;
			this._oldContentPresenter.Content = uielement;
			if (oldTransition != null)
			{
				if (oldTransition.GetCurrentState() != 2)
				{
					oldTransition.Stop();
				}
				oldTransition.Completed += delegate(object A_1, EventArgs A_2)
				{
					oldTransition.Stop();
					this._oldContentPresenter.CacheMode = null;
					this._oldContentPresenter.IsHitTestVisible = true;
					if (navigationOutTransition != null)
					{
						navigationOutTransition.OnEndTransition();
					}
					this.TransitionNewElement(newTransition, navigationInTransition);
				};
				base.Dispatcher.BeginInvoke(delegate()
				{
					this.Dispatcher.BeginInvoke(delegate()
					{
						if (navigationOutTransition != null)
						{
							navigationOutTransition.OnBeginTransition();
						}
						oldTransition.Begin();
					});
				});
				return;
			}
			this.TransitionNewElement(newTransition, navigationInTransition);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000EC88 File Offset: 0x0000CE88
		private void TransitionNewElement(ITransition newTransition, NavigationInTransition navigationInTransition)
		{
			this._oldContentPresenter.Visibility = 1;
			this._oldContentPresenter.Content = null;
			if (newTransition == null)
			{
				this._newContentPresenter.IsHitTestVisible = true;
				this._newContentPresenter.Opacity = 1.0;
				return;
			}
			if (newTransition.GetCurrentState() != 2)
			{
				newTransition.Stop();
			}
			newTransition.Completed += delegate(object A_1, EventArgs A_2)
			{
				newTransition.Stop();
				this._newContentPresenter.CacheMode = null;
				this._newContentPresenter.IsHitTestVisible = true;
				if (navigationInTransition != null)
				{
					navigationInTransition.OnEndTransition();
				}
			};
			base.Dispatcher.BeginInvoke(delegate()
			{
				if (navigationInTransition != null)
				{
					navigationInTransition.OnBeginTransition();
				}
				this._newContentPresenter.Opacity = 1.0;
				newTransition.Begin();
			});
		}

		// Token: 0x040001B4 RID: 436
		private const string FirstTemplatePartName = "FirstContentPresenter";

		// Token: 0x040001B5 RID: 437
		private const string SecondTemplatePartName = "SecondContentPresenter";

		// Token: 0x040001B6 RID: 438
		private bool _isForwardNavigation;

		// Token: 0x040001B7 RID: 439
		private bool _useFirstAsNew;

		// Token: 0x040001B8 RID: 440
		private ContentPresenter _firstContentPresenter;

		// Token: 0x040001B9 RID: 441
		private ContentPresenter _secondContentPresenter;

		// Token: 0x040001BA RID: 442
		private ContentPresenter _oldContentPresenter;

		// Token: 0x040001BB RID: 443
		private ContentPresenter _newContentPresenter;
	}
}
