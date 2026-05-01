using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000070 RID: 112
	public class PinchStartedGestureEventArgs : MultiTouchGestureEventArgs
	{
		// Token: 0x06000468 RID: 1128 RVA: 0x00012EF8 File Offset: 0x000110F8
		internal PinchStartedGestureEventArgs(Point gestureOrigin, Point gestureOrigin2, Point pinch, Point pinch2) : base(gestureOrigin, gestureOrigin2, pinch, pinch2)
		{
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x00012F05 File Offset: 0x00011105
		public double Distance
		{
			get
			{
				return MathHelpers.GetDistance(base.TouchPosition, base.TouchPosition2);
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x00012F18 File Offset: 0x00011118
		public double Angle
		{
			get
			{
				return MathHelpers.GetAngle(base.TouchPosition2.X - base.TouchPosition.X, base.TouchPosition2.Y - base.TouchPosition.Y);
			}
		}
	}
}
