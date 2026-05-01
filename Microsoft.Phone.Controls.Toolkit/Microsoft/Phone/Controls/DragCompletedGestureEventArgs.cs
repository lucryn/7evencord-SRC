using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200006D RID: 109
	public class DragCompletedGestureEventArgs : GestureEventArgs
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x00012D80 File Offset: 0x00010F80
		internal DragCompletedGestureEventArgs(Point gestureOrigin, Point currentPosition, Point change, Orientation direction, Point finalVelocity) : base(gestureOrigin, currentPosition)
		{
			this.HorizontalChange = change.X;
			this.VerticalChange = change.Y;
			this.Direction = direction;
			this.HorizontalVelocity = finalVelocity.X;
			this.VerticalVelocity = finalVelocity.Y;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x00012DD1 File Offset: 0x00010FD1
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x00012DD9 File Offset: 0x00010FD9
		public double HorizontalChange { get; private set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x00012DE2 File Offset: 0x00010FE2
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x00012DEA File Offset: 0x00010FEA
		public double VerticalChange { get; private set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00012DF3 File Offset: 0x00010FF3
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x00012DFB File Offset: 0x00010FFB
		public Orientation Direction { get; private set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00012E04 File Offset: 0x00011004
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x00012E0C File Offset: 0x0001100C
		public double HorizontalVelocity { get; private set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x00012E15 File Offset: 0x00011015
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x00012E1D File Offset: 0x0001101D
		public double VerticalVelocity { get; private set; }
	}
}
