using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000020 RID: 32
	internal static class PhysicsConstants
	{
		// Token: 0x060000DC RID: 220 RVA: 0x00004C6C File Offset: 0x00002E6C
		internal static double GetStopTime(Point initialVelocity)
		{
			double num = Math.Min(Math.Sqrt(initialVelocity.X * initialVelocity.X + initialVelocity.Y * initialVelocity.Y), MotionParameters.MaximumSpeed);
			if (MotionParameters.ParkingSpeed >= num)
			{
				return 0.0;
			}
			return Math.Log(MotionParameters.ParkingSpeed / num) / Math.Log(MotionParameters.Friction);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004CD4 File Offset: 0x00002ED4
		internal static Point GetStopPoint(Point initialVelocity)
		{
			double num = Math.Sqrt(initialVelocity.X * initialVelocity.X + initialVelocity.Y * initialVelocity.Y);
			Point initialVelocity2 = initialVelocity;
			if (num > MotionParameters.MaximumSpeed && num > 0.0)
			{
				initialVelocity2.X *= MotionParameters.MaximumSpeed / num;
				initialVelocity2.Y *= MotionParameters.MaximumSpeed / num;
			}
			double num2 = (Math.Pow(MotionParameters.Friction, PhysicsConstants.GetStopTime(initialVelocity2)) - 1.0) / Math.Log(MotionParameters.Friction);
			return new Point(initialVelocity2.X * num2, initialVelocity2.Y * num2);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004D84 File Offset: 0x00002F84
		internal static IEasingFunction GetEasingFunction(double stopTime)
		{
			return new ExponentialEase
			{
				Exponent = stopTime * Math.Log(MotionParameters.Friction),
				EasingMode = 1
			};
		}
	}
}
