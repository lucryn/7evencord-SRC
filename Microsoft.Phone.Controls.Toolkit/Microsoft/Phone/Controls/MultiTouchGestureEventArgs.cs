using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200006F RID: 111
	public class MultiTouchGestureEventArgs : GestureEventArgs
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00012E95 File Offset: 0x00011095
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x00012E9D File Offset: 0x0001109D
		private protected Point GestureOrigin2 { protected get; private set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00012EA6 File Offset: 0x000110A6
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x00012EAE File Offset: 0x000110AE
		private protected Point TouchPosition2 { protected get; private set; }

		// Token: 0x06000466 RID: 1126 RVA: 0x00012EB7 File Offset: 0x000110B7
		internal MultiTouchGestureEventArgs(Point gestureOrigin, Point gestureOrigin2, Point position, Point position2) : base(gestureOrigin, position)
		{
			this.GestureOrigin2 = gestureOrigin2;
			this.TouchPosition2 = position2;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00012ED0 File Offset: 0x000110D0
		public Point GetPosition(UIElement relativeTo, int index)
		{
			if (index == 0)
			{
				return base.GetPosition(relativeTo);
			}
			if (index == 1)
			{
				return GestureEventArgs.GetPosition(relativeTo, this.TouchPosition2);
			}
			throw new ArgumentOutOfRangeException("index");
		}
	}
}
