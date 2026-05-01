using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200006B RID: 107
	public class DragStartedGestureEventArgs : GestureEventArgs
	{
		// Token: 0x06000448 RID: 1096 RVA: 0x00012CFD File Offset: 0x00010EFD
		internal DragStartedGestureEventArgs(Point gestureOrigin, Orientation direction) : base(gestureOrigin, gestureOrigin)
		{
			this.Direction = direction;
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x00012D0E File Offset: 0x00010F0E
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x00012D16 File Offset: 0x00010F16
		public Orientation Direction { get; private set; }
	}
}
