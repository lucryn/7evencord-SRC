using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200006C RID: 108
	public class DragDeltaGestureEventArgs : GestureEventArgs
	{
		// Token: 0x0600044B RID: 1099 RVA: 0x00012D1F File Offset: 0x00010F1F
		internal DragDeltaGestureEventArgs(Point gestureOrigin, Point currentPosition, Point change, Orientation direction) : base(gestureOrigin, currentPosition)
		{
			this.HorizontalChange = change.X;
			this.VerticalChange = change.Y;
			this.Direction = direction;
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x00012D4B File Offset: 0x00010F4B
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x00012D53 File Offset: 0x00010F53
		public double HorizontalChange { get; private set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00012D5C File Offset: 0x00010F5C
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x00012D64 File Offset: 0x00010F64
		public double VerticalChange { get; private set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x00012D6D File Offset: 0x00010F6D
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x00012D75 File Offset: 0x00010F75
		public Orientation Direction { get; private set; }
	}
}
