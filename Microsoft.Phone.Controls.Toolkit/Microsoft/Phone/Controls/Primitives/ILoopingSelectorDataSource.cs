using System;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x0200000A RID: 10
	public interface ILoopingSelectorDataSource
	{
		// Token: 0x06000042 RID: 66
		object GetNext(object relativeTo);

		// Token: 0x06000043 RID: 67
		object GetPrevious(object relativeTo);

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000044 RID: 68
		// (set) Token: 0x06000045 RID: 69
		object SelectedItem { get; set; }

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000046 RID: 70
		// (remove) Token: 0x06000047 RID: 71
		event EventHandler<SelectionChangedEventArgs> SelectionChanged;
	}
}
