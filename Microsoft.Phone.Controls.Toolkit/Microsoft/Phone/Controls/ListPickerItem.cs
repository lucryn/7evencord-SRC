using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000068 RID: 104
	[TemplateVisualState(GroupName = "SelectionStates", Name = "Selected")]
	[TemplateVisualState(GroupName = "SelectionStates", Name = "Unselected")]
	public class ListPickerItem : ContentControl
	{
		// Token: 0x06000403 RID: 1027 RVA: 0x000119AD File Offset: 0x0000FBAD
		public ListPickerItem()
		{
			base.DefaultStyleKey = typeof(ListPickerItem);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000119C5 File Offset: 0x0000FBC5
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			VisualStateManager.GoToState(this, this.IsSelected ? "Selected" : "Unselected", false);
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x000119E9 File Offset: 0x0000FBE9
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x000119F1 File Offset: 0x0000FBF1
		internal bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				this._isSelected = value;
				VisualStateManager.GoToState(this, this._isSelected ? "Selected" : "Unselected", true);
			}
		}

		// Token: 0x04000203 RID: 515
		private const string SelectionStatesGroupName = "SelectionStates";

		// Token: 0x04000204 RID: 516
		private const string SelectionStatesUnselectedStateName = "Unselected";

		// Token: 0x04000205 RID: 517
		private const string SelectionStatesSelectedStateName = "Selected";

		// Token: 0x04000206 RID: 518
		private bool _isSelected;
	}
}
