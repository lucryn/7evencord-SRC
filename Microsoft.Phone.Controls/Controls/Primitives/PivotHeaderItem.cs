using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x0200001E RID: 30
	[TemplateVisualState(Name = "Selected", GroupName = "SelectionStates")]
	[TemplateVisualState(Name = "Unselected", GroupName = "SelectionStates")]
	public class PivotHeaderItem : ContentControl
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00006AA2 File Offset: 0x00005AA2
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00006AB4 File Offset: 0x00005AB4
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(PivotHeaderItem.IsSelectedProperty);
			}
			set
			{
				base.SetValue(PivotHeaderItem.IsSelectedProperty, value);
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006AC8 File Offset: 0x00005AC8
		private static void OnIsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PivotHeaderItem pivotHeaderItem = d as PivotHeaderItem;
			if (pivotHeaderItem.ParentHeadersControl != null)
			{
				pivotHeaderItem.ParentHeadersControl.NotifyHeaderItemSelected(pivotHeaderItem, (bool)e.NewValue);
				pivotHeaderItem.UpdateVisualStates(true);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00006B03 File Offset: 0x00005B03
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00006B0B File Offset: 0x00005B0B
		internal PivotHeadersControl ParentHeadersControl { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00006B14 File Offset: 0x00005B14
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00006B1C File Offset: 0x00005B1C
		internal object Item { get; set; }

		// Token: 0x0600014B RID: 331 RVA: 0x00006B25 File Offset: 0x00005B25
		public PivotHeaderItem()
		{
			base.DefaultStyleKey = typeof(PivotHeaderItem);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006B3D File Offset: 0x00005B3D
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			if (e != null && !e.Handled)
			{
				e.Handled = true;
				if (this.ParentHeadersControl != null)
				{
					this.ParentHeadersControl.OnHeaderItemClicked(this);
				}
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00006B6C File Offset: 0x00005B6C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.UpdateVisualStates(false);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00006B7B File Offset: 0x00005B7B
		internal void UpdateVisualStateToUnselected()
		{
			VisualStateManager.GoToState(this, "Unselected", false);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00006B8A File Offset: 0x00005B8A
		internal void RestoreVisualStates()
		{
			this.UpdateVisualStates(false);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00006B93 File Offset: 0x00005B93
		private void UpdateVisualStates(bool useTransitions)
		{
			VisualStateManager.GoToState(this, this.IsSelected ? "Selected" : "Unselected", useTransitions);
		}

		// Token: 0x040000A3 RID: 163
		private const string SelectedState = "Selected";

		// Token: 0x040000A4 RID: 164
		private const string UnselectedState = "Unselected";

		// Token: 0x040000A5 RID: 165
		private const string SelectionStatesGroup = "SelectionStates";

		// Token: 0x040000A6 RID: 166
		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(PivotHeaderItem), new PropertyMetadata(false, new PropertyChangedCallback(PivotHeaderItem.OnIsSelectedPropertyChanged)));
	}
}
