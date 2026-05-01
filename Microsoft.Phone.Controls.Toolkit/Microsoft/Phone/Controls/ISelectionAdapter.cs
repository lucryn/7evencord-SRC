using System;
using System.Collections;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000029 RID: 41
	public interface ISelectionAdapter
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000134 RID: 308
		// (set) Token: 0x06000135 RID: 309
		object SelectedItem { get; set; }

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000136 RID: 310
		// (remove) Token: 0x06000137 RID: 311
		event SelectionChangedEventHandler SelectionChanged;

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000138 RID: 312
		// (set) Token: 0x06000139 RID: 313
		IEnumerable ItemsSource { get; set; }

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600013A RID: 314
		// (remove) Token: 0x0600013B RID: 315
		event RoutedEventHandler Commit;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600013C RID: 316
		// (remove) Token: 0x0600013D RID: 317
		event RoutedEventHandler Cancel;

		// Token: 0x0600013E RID: 318
		void HandleKeyDown(KeyEventArgs e);

		// Token: 0x0600013F RID: 319
		AutomationPeer CreateAutomationPeer();
	}
}
