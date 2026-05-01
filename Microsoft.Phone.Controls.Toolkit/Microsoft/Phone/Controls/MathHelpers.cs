using System;
using System.Windows;
using Microsoft.Xna.Framework;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200003D RID: 61
	internal static class MathHelpers
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x00008ADC File Offset: 0x00006CDC
		public static double GetAngle(double deltaX, double deltaY)
		{
			double num = Math.Atan2(deltaY, deltaX);
			if (num < 0.0)
			{
				num = 6.283185307179586 + num;
			}
			return num * 360.0 / 6.283185307179586;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00008B20 File Offset: 0x00006D20
		public static double GetDistance(Point p0, Point p1)
		{
			double num = p0.X - p1.X;
			double num2 = p0.Y - p1.Y;
			return Math.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00008B59 File Offset: 0x00006D59
		public static Point ToPoint(this Vector2 v)
		{
			return new Point((double)v.X, (double)v.Y);
		}
	}
}
