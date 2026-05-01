using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000059 RID: 89
	internal static class VisualStates
	{
		// Token: 0x06000341 RID: 833 RVA: 0x0000E798 File Offset: 0x0000C998
		public static void GoToState(Control control, bool useTransitions, params string[] stateNames)
		{
			foreach (string text in stateNames)
			{
				if (VisualStateManager.GoToState(control, text, useTransitions))
				{
					return;
				}
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000E7C4 File Offset: 0x0000C9C4
		public static FrameworkElement GetImplementationRoot(DependencyObject dependencyObject)
		{
			if (1 != VisualTreeHelper.GetChildrenCount(dependencyObject))
			{
				return null;
			}
			return VisualTreeHelper.GetChild(dependencyObject, 0) as FrameworkElement;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000E7FC File Offset: 0x0000C9FC
		public static VisualStateGroup TryGetVisualStateGroup(DependencyObject dependencyObject, string groupName)
		{
			FrameworkElement implementationRoot = VisualStates.GetImplementationRoot(dependencyObject);
			if (implementationRoot == null)
			{
				return null;
			}
			return Enumerable.FirstOrDefault<VisualStateGroup>(Enumerable.Where<VisualStateGroup>(Enumerable.OfType<VisualStateGroup>(VisualStateManager.GetVisualStateGroups(implementationRoot)), (VisualStateGroup group) => string.CompareOrdinal(groupName, group.Name) == 0));
		}

		// Token: 0x0400017A RID: 378
		public const string GroupCommon = "CommonStates";

		// Token: 0x0400017B RID: 379
		public const string StateNormal = "Normal";

		// Token: 0x0400017C RID: 380
		public const string StateReadOnly = "ReadOnly";

		// Token: 0x0400017D RID: 381
		public const string StateMouseOver = "MouseOver";

		// Token: 0x0400017E RID: 382
		public const string StatePressed = "Pressed";

		// Token: 0x0400017F RID: 383
		public const string StateDisabled = "Disabled";

		// Token: 0x04000180 RID: 384
		public const string GroupFocus = "FocusStates";

		// Token: 0x04000181 RID: 385
		public const string StateUnfocused = "Unfocused";

		// Token: 0x04000182 RID: 386
		public const string StateFocused = "Focused";

		// Token: 0x04000183 RID: 387
		public const string GroupSelection = "SelectionStates";

		// Token: 0x04000184 RID: 388
		public const string StateSelected = "Selected";

		// Token: 0x04000185 RID: 389
		public const string StateUnselected = "Unselected";

		// Token: 0x04000186 RID: 390
		public const string StateSelectedInactive = "SelectedInactive";

		// Token: 0x04000187 RID: 391
		public const string GroupExpansion = "ExpansionStates";

		// Token: 0x04000188 RID: 392
		public const string StateExpanded = "Expanded";

		// Token: 0x04000189 RID: 393
		public const string StateCollapsed = "Collapsed";

		// Token: 0x0400018A RID: 394
		public const string GroupPopup = "PopupStates";

		// Token: 0x0400018B RID: 395
		public const string StatePopupOpened = "PopupOpened";

		// Token: 0x0400018C RID: 396
		public const string StatePopupClosed = "PopupClosed";

		// Token: 0x0400018D RID: 397
		public const string GroupValidation = "ValidationStates";

		// Token: 0x0400018E RID: 398
		public const string StateValid = "Valid";

		// Token: 0x0400018F RID: 399
		public const string StateInvalidFocused = "InvalidFocused";

		// Token: 0x04000190 RID: 400
		public const string StateInvalidUnfocused = "InvalidUnfocused";

		// Token: 0x04000191 RID: 401
		public const string GroupExpandDirection = "ExpandDirectionStates";

		// Token: 0x04000192 RID: 402
		public const string StateExpandDown = "ExpandDown";

		// Token: 0x04000193 RID: 403
		public const string StateExpandUp = "ExpandUp";

		// Token: 0x04000194 RID: 404
		public const string StateExpandLeft = "ExpandLeft";

		// Token: 0x04000195 RID: 405
		public const string StateExpandRight = "ExpandRight";

		// Token: 0x04000196 RID: 406
		public const string GroupHasItems = "HasItemsStates";

		// Token: 0x04000197 RID: 407
		public const string StateHasItems = "HasItems";

		// Token: 0x04000198 RID: 408
		public const string StateNoItems = "NoItems";

		// Token: 0x04000199 RID: 409
		public const string GroupIncrease = "IncreaseStates";

		// Token: 0x0400019A RID: 410
		public const string StateIncreaseEnabled = "IncreaseEnabled";

		// Token: 0x0400019B RID: 411
		public const string StateIncreaseDisabled = "IncreaseDisabled";

		// Token: 0x0400019C RID: 412
		public const string GroupDecrease = "DecreaseStates";

		// Token: 0x0400019D RID: 413
		public const string StateDecreaseEnabled = "DecreaseEnabled";

		// Token: 0x0400019E RID: 414
		public const string StateDecreaseDisabled = "DecreaseDisabled";

		// Token: 0x0400019F RID: 415
		public const string GroupInteractionMode = "InteractionModeStates";

		// Token: 0x040001A0 RID: 416
		public const string StateEdit = "Edit";

		// Token: 0x040001A1 RID: 417
		public const string StateDisplay = "Display";

		// Token: 0x040001A2 RID: 418
		public const string GroupLocked = "LockedStates";

		// Token: 0x040001A3 RID: 419
		public const string StateLocked = "Locked";

		// Token: 0x040001A4 RID: 420
		public const string StateUnlocked = "Unlocked";

		// Token: 0x040001A5 RID: 421
		public const string StateActive = "Active";

		// Token: 0x040001A6 RID: 422
		public const string StateInactive = "Inactive";

		// Token: 0x040001A7 RID: 423
		public const string GroupActive = "ActiveStates";

		// Token: 0x040001A8 RID: 424
		public const string StateUnwatermarked = "Unwatermarked";

		// Token: 0x040001A9 RID: 425
		public const string StateWatermarked = "Watermarked";

		// Token: 0x040001AA RID: 426
		public const string GroupWatermark = "WatermarkStates";

		// Token: 0x040001AB RID: 427
		public const string StateCalendarButtonUnfocused = "CalendarButtonUnfocused";

		// Token: 0x040001AC RID: 428
		public const string StateCalendarButtonFocused = "CalendarButtonFocused";

		// Token: 0x040001AD RID: 429
		public const string GroupCalendarButtonFocus = "CalendarButtonFocusStates";

		// Token: 0x040001AE RID: 430
		public const string StateBusy = "Busy";

		// Token: 0x040001AF RID: 431
		public const string StateIdle = "Idle";

		// Token: 0x040001B0 RID: 432
		public const string GroupBusyStatus = "BusyStatusStates";

		// Token: 0x040001B1 RID: 433
		public const string StateVisible = "Visible";

		// Token: 0x040001B2 RID: 434
		public const string StateHidden = "Hidden";

		// Token: 0x040001B3 RID: 435
		public const string GroupVisibility = "VisibilityStates";
	}
}
