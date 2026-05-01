using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200002A RID: 42
	public class SelectorSelectionAdapter : ISelectionAdapter
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00006AC3 File Offset: 0x00004CC3
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00006ACB File Offset: 0x00004CCB
		private bool IgnoringSelectionChanged { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00006AD4 File Offset: 0x00004CD4
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00006ADC File Offset: 0x00004CDC
		public Selector SelectorControl
		{
			get
			{
				return this._selector;
			}
			set
			{
				if (this._selector != null)
				{
					this._selector.SelectionChanged -= new SelectionChangedEventHandler(this.OnSelectionChanged);
				}
				this._selector = value;
				if (this._selector != null)
				{
					this._selector.SelectionChanged += new SelectionChangedEventHandler(this.OnSelectionChanged);
				}
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000144 RID: 324 RVA: 0x00006B30 File Offset: 0x00004D30
		// (remove) Token: 0x06000145 RID: 325 RVA: 0x00006B68 File Offset: 0x00004D68
		public event SelectionChangedEventHandler SelectionChanged;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000146 RID: 326 RVA: 0x00006BA0 File Offset: 0x00004DA0
		// (remove) Token: 0x06000147 RID: 327 RVA: 0x00006BD8 File Offset: 0x00004DD8
		public event RoutedEventHandler Commit;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000148 RID: 328 RVA: 0x00006C10 File Offset: 0x00004E10
		// (remove) Token: 0x06000149 RID: 329 RVA: 0x00006C48 File Offset: 0x00004E48
		public event RoutedEventHandler Cancel;

		// Token: 0x0600014A RID: 330 RVA: 0x00006C7D File Offset: 0x00004E7D
		public SelectorSelectionAdapter()
		{
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00006C85 File Offset: 0x00004E85
		public SelectorSelectionAdapter(Selector selector)
		{
			this.SelectorControl = selector;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00006C94 File Offset: 0x00004E94
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00006CAB File Offset: 0x00004EAB
		public object SelectedItem
		{
			get
			{
				if (this.SelectorControl != null)
				{
					return this.SelectorControl.SelectedItem;
				}
				return null;
			}
			set
			{
				this.IgnoringSelectionChanged = true;
				if (this.SelectorControl != null)
				{
					this.SelectorControl.SelectedItem = value;
				}
				if (value == null)
				{
					this.ResetScrollViewer();
				}
				this.IgnoringSelectionChanged = false;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00006CD8 File Offset: 0x00004ED8
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00006CEF File Offset: 0x00004EEF
		public IEnumerable ItemsSource
		{
			get
			{
				if (this.SelectorControl != null)
				{
					return this.SelectorControl.ItemsSource;
				}
				return null;
			}
			set
			{
				if (this.SelectorControl != null)
				{
					this.SelectorControl.ItemsSource = value;
				}
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00006D08 File Offset: 0x00004F08
		private void ResetScrollViewer()
		{
			if (this.SelectorControl != null)
			{
				ScrollViewer scrollViewer = Enumerable.FirstOrDefault<ScrollViewer>(Enumerable.OfType<ScrollViewer>(this.SelectorControl.GetLogicalChildrenBreadthFirst()));
				if (scrollViewer != null)
				{
					scrollViewer.ScrollToVerticalOffset(0.0);
				}
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006D48 File Offset: 0x00004F48
		private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.IgnoringSelectionChanged)
			{
				return;
			}
			SelectionChangedEventHandler selectionChanged = this.SelectionChanged;
			if (selectionChanged != null)
			{
				selectionChanged.Invoke(sender, e);
			}
			this.OnCommit();
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006D78 File Offset: 0x00004F78
		protected void SelectedIndexIncrement()
		{
			if (this.SelectorControl != null)
			{
				this.SelectorControl.SelectedIndex = ((this.SelectorControl.SelectedIndex + 1 >= this.SelectorControl.Items.Count) ? -1 : (this.SelectorControl.SelectedIndex + 1));
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006DC8 File Offset: 0x00004FC8
		protected void SelectedIndexDecrement()
		{
			if (this.SelectorControl != null)
			{
				int selectedIndex = this.SelectorControl.SelectedIndex;
				if (selectedIndex >= 0)
				{
					this.SelectorControl.SelectedIndex--;
					return;
				}
				if (selectedIndex == -1)
				{
					this.SelectorControl.SelectedIndex = this.SelectorControl.Items.Count - 1;
				}
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00006E24 File Offset: 0x00005024
		public void HandleKeyDown(KeyEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			Key key = e.Key;
			if (key != 3)
			{
				if (key != 8)
				{
					switch (key)
					{
					case 15:
						this.SelectedIndexDecrement();
						e.Handled = true;
						return;
					case 16:
						break;
					case 17:
						if ((1 & Keyboard.Modifiers) == null)
						{
							this.SelectedIndexIncrement();
							e.Handled = true;
							return;
						}
						break;
					default:
						return;
					}
				}
				else
				{
					this.OnCancel();
					e.Handled = true;
				}
				return;
			}
			this.OnCommit();
			e.Handled = true;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00006EA4 File Offset: 0x000050A4
		protected virtual void OnCommit()
		{
			this.OnCommit(this, new RoutedEventArgs());
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00006EB4 File Offset: 0x000050B4
		private void OnCommit(object sender, RoutedEventArgs e)
		{
			RoutedEventHandler commit = this.Commit;
			if (commit != null)
			{
				commit.Invoke(sender, e);
			}
			this.AfterAdapterAction();
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006ED9 File Offset: 0x000050D9
		protected virtual void OnCancel()
		{
			this.OnCancel(this, new RoutedEventArgs());
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006EE8 File Offset: 0x000050E8
		private void OnCancel(object sender, RoutedEventArgs e)
		{
			RoutedEventHandler cancel = this.Cancel;
			if (cancel != null)
			{
				cancel.Invoke(sender, e);
			}
			this.AfterAdapterAction();
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00006F0D File Offset: 0x0000510D
		private void AfterAdapterAction()
		{
			this.IgnoringSelectionChanged = true;
			if (this.SelectorControl != null)
			{
				this.SelectorControl.SelectedItem = null;
				this.SelectorControl.SelectedIndex = -1;
			}
			this.IgnoringSelectionChanged = false;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00006F3D File Offset: 0x0000513D
		public AutomationPeer CreateAutomationPeer()
		{
			if (this._selector == null)
			{
				return null;
			}
			return FrameworkElementAutomationPeer.CreatePeerForElement(this._selector);
		}

		// Token: 0x04000081 RID: 129
		private Selector _selector;
	}
}
