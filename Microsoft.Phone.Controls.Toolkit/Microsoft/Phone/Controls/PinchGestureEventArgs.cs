using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000071 RID: 113
	public class PinchGestureEventArgs : MultiTouchGestureEventArgs
	{
		// Token: 0x0600046B RID: 1131 RVA: 0x00012F64 File Offset: 0x00011164
		internal PinchGestureEventArgs(Point gestureOrigin, Point gestureOrigin2, Point position, Point position2) : base(gestureOrigin, gestureOrigin2, position, position2)
		{
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x00012F74 File Offset: 0x00011174
		public double DistanceRatio
		{
			get
			{
				double num = Math.Max(MathHelpers.GetDistance(base.GestureOrigin, base.GestureOrigin2), 1.0);
				double num2 = Math.Max(MathHelpers.GetDistance(base.TouchPosition, base.TouchPosition2), 1.0);
				return num2 / num;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x00012FC4 File Offset: 0x000111C4
		public double TotalAngleDelta
		{
			get
			{
				double angle = MathHelpers.GetAngle(base.GestureOrigin2.X - base.GestureOrigin.X, base.GestureOrigin2.Y - base.GestureOrigin.Y);
				double angle2 = MathHelpers.GetAngle(base.TouchPosition2.X - base.TouchPosition.X, base.TouchPosition2.Y - base.TouchPosition.Y);
				return angle2 - angle;
			}
		}
	}
}
