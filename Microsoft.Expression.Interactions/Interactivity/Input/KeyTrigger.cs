using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Microsoft.Expression.Interactivity.Input
{
	// Token: 0x0200001E RID: 30
	public class KeyTrigger : EventTriggerBase<UIElement>
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00007668 File Offset: 0x00005868
		// (set) Token: 0x0600010D RID: 269 RVA: 0x0000767A File Offset: 0x0000587A
		public Key Key
		{
			get
			{
				return (Key)base.GetValue(KeyTrigger.KeyProperty);
			}
			set
			{
				base.SetValue(KeyTrigger.KeyProperty, value);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600010E RID: 270 RVA: 0x0000768D File Offset: 0x0000588D
		// (set) Token: 0x0600010F RID: 271 RVA: 0x0000769F File Offset: 0x0000589F
		public ModifierKeys Modifiers
		{
			get
			{
				return (ModifierKeys)base.GetValue(KeyTrigger.ModifiersProperty);
			}
			set
			{
				base.SetValue(KeyTrigger.ModifiersProperty, value);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000076B2 File Offset: 0x000058B2
		// (set) Token: 0x06000111 RID: 273 RVA: 0x000076C4 File Offset: 0x000058C4
		public KeyTriggerFiredOn FiredOn
		{
			get
			{
				return (KeyTriggerFiredOn)base.GetValue(KeyTrigger.FiredOnProperty);
			}
			set
			{
				base.SetValue(KeyTrigger.FiredOnProperty, value);
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000076D7 File Offset: 0x000058D7
		protected override string GetEventName()
		{
			return "Loaded";
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000076DE File Offset: 0x000058DE
		private void OnKeyPress(object sender, KeyEventArgs e)
		{
			if (e.Key == this.Key && Keyboard.Modifiers == KeyTrigger.GetActualModifiers(e.Key, this.Modifiers))
			{
				base.InvokeActions(e);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000770D File Offset: 0x0000590D
		private static ModifierKeys GetActualModifiers(Key key, ModifierKeys modifiers)
		{
			if (key == 5)
			{
				modifiers |= 2;
			}
			else if (key == 6)
			{
				modifiers |= 1;
			}
			else if (key == 4)
			{
				modifiers |= 4;
			}
			return modifiers;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00007730 File Offset: 0x00005930
		protected override void OnEvent(EventArgs eventArgs)
		{
			this.targetElement = Application.Current.RootVisual;
			if (this.FiredOn == KeyTriggerFiredOn.KeyDown)
			{
				this.targetElement.KeyDown += new KeyEventHandler(this.OnKeyPress);
				return;
			}
			this.targetElement.KeyUp += new KeyEventHandler(this.OnKeyPress);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00007784 File Offset: 0x00005984
		protected override void OnDetaching()
		{
			if (this.targetElement != null)
			{
				if (this.FiredOn == KeyTriggerFiredOn.KeyDown)
				{
					this.targetElement.KeyDown -= new KeyEventHandler(this.OnKeyPress);
				}
				else
				{
					this.targetElement.KeyUp -= new KeyEventHandler(this.OnKeyPress);
				}
			}
			base.OnDetaching();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000077D8 File Offset: 0x000059D8
		private static UIElement GetRoot(DependencyObject current)
		{
			UIElement result = null;
			while (current != null)
			{
				result = (current as UIElement);
				current = VisualTreeHelper.GetParent(current);
			}
			return result;
		}

		// Token: 0x04000060 RID: 96
		public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(Key), typeof(KeyTrigger), null);

		// Token: 0x04000061 RID: 97
		public static readonly DependencyProperty ModifiersProperty = DependencyProperty.Register("Modifiers", typeof(ModifierKeys), typeof(KeyTrigger), null);

		// Token: 0x04000062 RID: 98
		public static readonly DependencyProperty FiredOnProperty = DependencyProperty.Register("FiredOn", typeof(KeyTriggerFiredOn), typeof(KeyTrigger), null);

		// Token: 0x04000063 RID: 99
		private UIElement targetElement;
	}
}
