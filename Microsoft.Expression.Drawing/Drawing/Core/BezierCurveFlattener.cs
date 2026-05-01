using System;
using System.Collections.Generic;
using System.Windows;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x02000016 RID: 22
	internal static class BezierCurveFlattener
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x00006390 File Offset: 0x00004590
		public static void FlattenCubic(Point[] controlPoints, double errorTolerance, ICollection<Point> resultPolyline, bool skipFirstPoint, ICollection<double> resultParameters = null)
		{
			if (resultPolyline == null)
			{
				throw new ArgumentNullException("resultPolyline");
			}
			if (controlPoints == null)
			{
				throw new ArgumentNullException("controlPoints");
			}
			if (controlPoints.Length != 4)
			{
				throw new ArgumentOutOfRangeException("controlPoints");
			}
			BezierCurveFlattener.EnsureErrorTolerance(ref errorTolerance);
			if (!skipFirstPoint)
			{
				resultPolyline.Add(controlPoints[0]);
				if (resultParameters != null)
				{
					resultParameters.Add(0.0);
				}
			}
			if (BezierCurveFlattener.IsCubicChordMonotone(controlPoints, errorTolerance * errorTolerance))
			{
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener = new BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener(controlPoints, errorTolerance, errorTolerance, true);
				Point point = default(Point);
				double num = 0.0;
				while (adaptiveForwardDifferencingCubicFlattener.Next(ref point, ref num))
				{
					resultPolyline.Add(point);
					if (resultParameters != null)
					{
						resultParameters.Add(num);
					}
				}
			}
			else
			{
				double x = controlPoints[3].X - controlPoints[2].X + controlPoints[1].X - controlPoints[0].X;
				double y = controlPoints[3].Y - controlPoints[2].Y + controlPoints[1].Y - controlPoints[0].Y;
				double num2 = 1.0 / errorTolerance;
				uint num3 = BezierCurveFlattener.Log8UnsignedInt32((uint)(MathHelper.Hypotenuse(x, y) * num2 + 0.5));
				if (num3 > 0U)
				{
					num3 -= 1U;
				}
				if (num3 > 0U)
				{
					BezierCurveFlattener.DoCubicMidpointSubdivision(controlPoints, num3, 0.0, 1.0, 0.75 * num2, resultPolyline, resultParameters);
				}
				else
				{
					BezierCurveFlattener.DoCubicForwardDifferencing(controlPoints, 0.0, 1.0, 0.75 * num2, resultPolyline, resultParameters);
				}
			}
			resultPolyline.Add(controlPoints[3]);
			if (resultParameters != null)
			{
				resultParameters.Add(1.0);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006564 File Offset: 0x00004764
		public static void FlattenQuadratic(Point[] controlPoints, double errorTolerance, ICollection<Point> resultPolyline, bool skipFirstPoint, ICollection<double> resultParameters = null)
		{
			if (resultPolyline == null)
			{
				throw new ArgumentNullException("resultPolyline");
			}
			if (controlPoints == null)
			{
				throw new ArgumentNullException("controlPoints");
			}
			if (controlPoints.Length != 3)
			{
				throw new ArgumentOutOfRangeException("controlPoints");
			}
			BezierCurveFlattener.EnsureErrorTolerance(ref errorTolerance);
			BezierCurveFlattener.FlattenCubic(new Point[]
			{
				controlPoints[0],
				GeometryHelper.Lerp(controlPoints[0], controlPoints[1], 0.6666666666666666),
				GeometryHelper.Lerp(controlPoints[1], controlPoints[2], 0.3333333333333333),
				controlPoints[2]
			}, errorTolerance, resultPolyline, skipFirstPoint, resultParameters);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000664B File Offset: 0x0000484B
		private static void EnsureErrorTolerance(ref double errorTolerance)
		{
			if (errorTolerance <= 0.0)
			{
				errorTolerance = 0.25;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006668 File Offset: 0x00004868
		private static uint Log8UnsignedInt32(uint i)
		{
			uint num = 0U;
			while (i > 0U)
			{
				i >>= 3;
				num += 1U;
			}
			return num;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006688 File Offset: 0x00004888
		private static uint Log4UnsignedInt32(uint i)
		{
			uint num = 0U;
			while (i > 0U)
			{
				i >>= 2;
				num += 1U;
			}
			return num;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000066A8 File Offset: 0x000048A8
		private static uint Log4Double(double d)
		{
			uint num = 0U;
			while (d > 1.0)
			{
				d *= 0.25;
				num += 1U;
			}
			return num;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000066D8 File Offset: 0x000048D8
		private static void DoCubicMidpointSubdivision(Point[] controlPoints, uint depth, double leftParameter, double rightParameter, double inverseErrorTolerance, ICollection<Point> resultPolyline, ICollection<double> resultParameters)
		{
			Point[] array = new Point[]
			{
				controlPoints[0],
				controlPoints[1],
				controlPoints[2],
				controlPoints[3]
			};
			Point[] array2 = new Point[]
			{
				default(Point),
				default(Point),
				default(Point),
				array[3]
			};
			array[3] = GeometryHelper.Midpoint(array[3], array[2]);
			array[2] = GeometryHelper.Midpoint(array[2], array[1]);
			array[1] = GeometryHelper.Midpoint(array[1], array[0]);
			array2[2] = array[3];
			array[3] = GeometryHelper.Midpoint(array[3], array[2]);
			array[2] = GeometryHelper.Midpoint(array[2], array[1]);
			array2[1] = array[3];
			array[3] = GeometryHelper.Midpoint(array[3], array[2]);
			array2[0] = array[3];
			depth -= 1U;
			double num = (leftParameter + rightParameter) * 0.5;
			if (depth > 0U)
			{
				BezierCurveFlattener.DoCubicMidpointSubdivision(array, depth, leftParameter, num, inverseErrorTolerance, resultPolyline, resultParameters);
				resultPolyline.Add(array2[0]);
				if (resultParameters != null)
				{
					resultParameters.Add(num);
				}
				BezierCurveFlattener.DoCubicMidpointSubdivision(array2, depth, num, rightParameter, inverseErrorTolerance, resultPolyline, resultParameters);
				return;
			}
			BezierCurveFlattener.DoCubicForwardDifferencing(array, leftParameter, num, inverseErrorTolerance, resultPolyline, resultParameters);
			resultPolyline.Add(array2[0]);
			if (resultParameters != null)
			{
				resultParameters.Add(num);
			}
			BezierCurveFlattener.DoCubicForwardDifferencing(array2, num, rightParameter, inverseErrorTolerance, resultPolyline, resultParameters);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000693C File Offset: 0x00004B3C
		private static void DoCubicForwardDifferencing(Point[] controlPoints, double leftParameter, double rightParameter, double inverseErrorTolerance, ICollection<Point> resultPolyline, ICollection<double> resultParameters)
		{
			double num = controlPoints[1].X - controlPoints[0].X;
			double num2 = controlPoints[1].Y - controlPoints[0].Y;
			double num3 = controlPoints[2].X - controlPoints[1].X;
			double num4 = controlPoints[2].Y - controlPoints[1].Y;
			double num5 = controlPoints[3].X - controlPoints[2].X;
			double num6 = controlPoints[3].Y - controlPoints[2].Y;
			double num7 = num3 - num;
			double num8 = num4 - num2;
			double num9 = num5 - num3;
			double num10 = num6 - num4;
			double num11 = num9 - num7;
			double num12 = num10 - num8;
			Vector vector = controlPoints[3].Subtract(controlPoints[0]);
			double length = vector.Length;
			double num13;
			if (!MathHelper.IsVerySmall(length))
			{
				num13 = Math.Max(0.0, Math.Max(Math.Abs((num7 * vector.Y - num8 * vector.X) / length), Math.Abs((num9 * vector.Y - num10 * vector.X) / length)));
			}
			else
			{
				num13 = Math.Max(0.0, Math.Max(GeometryHelper.Distance(controlPoints[1], controlPoints[0]), GeometryHelper.Distance(controlPoints[2], controlPoints[0])));
			}
			uint num14 = 0U;
			if (num13 > 0.0)
			{
				double num15 = num13 * inverseErrorTolerance;
				num14 = ((num15 < 2147483647.0) ? BezierCurveFlattener.Log4UnsignedInt32((uint)(num15 + 0.5)) : BezierCurveFlattener.Log4Double(num15));
			}
			int num16 = (int)(-(int)num14);
			int num17 = num16 + num16;
			int exp = num17 + num16;
			double num18 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num7, num17);
			double num19 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num8, num17);
			double num20 = MathHelper.DoubleFromMantissaAndExponent(6.0 * num11, exp);
			double num21 = MathHelper.DoubleFromMantissaAndExponent(6.0 * num12, exp);
			double num22 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num, num16) + num18 + 0.16666666666666666 * num20;
			double num23 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num2, num16) + num19 + 0.16666666666666666 * num21;
			double num24 = 2.0 * num18 + num20;
			double num25 = 2.0 * num19 + num21;
			double num26 = controlPoints[0].X;
			double num27 = controlPoints[0].Y;
			Point point;
			point..ctor(0.0, 0.0);
			int num28 = 1 << (int)num14;
			double num29 = (num28 > 0) ? ((rightParameter - leftParameter) / (double)num28) : 0.0;
			double num30 = leftParameter;
			for (int i = 1; i < num28; i++)
			{
				num26 += num22;
				num27 += num23;
				point.X = num26;
				point.Y = num27;
				resultPolyline.Add(point);
				num30 += num29;
				if (resultParameters != null)
				{
					resultParameters.Add(num30);
				}
				num22 += num24;
				num23 += num25;
				num24 += num20;
				num25 += num21;
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006CB4 File Offset: 0x00004EB4
		private static bool IsCubicChordMonotone(Point[] controlPoints, double squaredTolerance)
		{
			double num = GeometryHelper.SquaredDistance(controlPoints[0], controlPoints[3]);
			if (num <= squaredTolerance)
			{
				return false;
			}
			Vector lhs = controlPoints[3].Subtract(controlPoints[0]);
			Vector rhs = controlPoints[1].Subtract(controlPoints[0]);
			double num2 = GeometryHelper.Dot(lhs, rhs);
			if (num2 < 0.0 || num2 > num)
			{
				return false;
			}
			Vector rhs2 = controlPoints[2].Subtract(controlPoints[0]);
			double num3 = GeometryHelper.Dot(lhs, rhs2);
			return num3 >= 0.0 && num3 <= num && num2 <= num3;
		}

		// Token: 0x04000039 RID: 57
		public const double StandardFlatteningTolerance = 0.25;

		// Token: 0x02000017 RID: 23
		private class AdaptiveForwardDifferencingCubicFlattener
		{
			// Token: 0x060000F2 RID: 242 RVA: 0x00006D84 File Offset: 0x00004F84
			internal AdaptiveForwardDifferencingCubicFlattener(Point[] controlPoints, double flatnessTolerance, double distanceTolerance, bool doParameters)
			{
				this.flatnessTolerance = 3.0 * flatnessTolerance;
				this.distanceTolerance = distanceTolerance;
				this.doParameters = doParameters;
				this.aX = -controlPoints[0].X + 3.0 * (controlPoints[1].X - controlPoints[2].X) + controlPoints[3].X;
				this.aY = -controlPoints[0].Y + 3.0 * (controlPoints[1].Y - controlPoints[2].Y) + controlPoints[3].Y;
				this.bX = 3.0 * (controlPoints[0].X - 2.0 * controlPoints[1].X + controlPoints[2].X);
				this.bY = 3.0 * (controlPoints[0].Y - 2.0 * controlPoints[1].Y + controlPoints[2].Y);
				this.cX = 3.0 * (-controlPoints[0].X + controlPoints[1].X);
				this.cY = 3.0 * (-controlPoints[0].Y + controlPoints[1].Y);
				this.dX = controlPoints[0].X;
				this.dY = controlPoints[0].Y;
			}

			// Token: 0x060000F3 RID: 243 RVA: 0x00006F4D File Offset: 0x0000514D
			private AdaptiveForwardDifferencingCubicFlattener()
			{
			}

			// Token: 0x060000F4 RID: 244 RVA: 0x00006F6C File Offset: 0x0000516C
			internal bool Next(ref Point p, ref double u)
			{
				while (this.MustSubdivide(this.flatnessTolerance))
				{
					this.HalveStepSize();
				}
				if ((this.numSteps & 1) == 0)
				{
					while (this.numSteps > 1 && !this.MustSubdivide(this.flatnessTolerance * 0.25))
					{
						this.DoubleStepSize();
					}
				}
				this.IncrementDifferencesAndParameter();
				p.X = this.dX;
				p.Y = this.dY;
				u = this.parameter;
				return this.numSteps != 0;
			}

			// Token: 0x060000F5 RID: 245 RVA: 0x00006FF4 File Offset: 0x000051F4
			private void DoubleStepSize()
			{
				this.aX *= 8.0;
				this.aY *= 8.0;
				this.bX *= 4.0;
				this.bY *= 4.0;
				this.cX += this.cX;
				this.cY += this.cY;
				if (this.doParameters)
				{
					this.dParameter *= 2.0;
				}
				this.numSteps >>= 1;
			}

			// Token: 0x060000F6 RID: 246 RVA: 0x000070AC File Offset: 0x000052AC
			private void HalveStepSize()
			{
				this.aX *= 0.125;
				this.aY *= 0.125;
				this.bX *= 0.25;
				this.bY *= 0.25;
				this.cX *= 0.5;
				this.cY *= 0.5;
				if (this.doParameters)
				{
					this.dParameter *= 0.5;
				}
				this.numSteps <<= 1;
			}

			// Token: 0x060000F7 RID: 247 RVA: 0x0000716C File Offset: 0x0000536C
			private void IncrementDifferencesAndParameter()
			{
				this.dX = this.aX + this.bX + this.cX + this.dX;
				this.dY = this.aY + this.bY + this.cY + this.dY;
				this.cX = this.aX + this.aX + this.aX + this.bX + this.bX + this.cX;
				this.cY = this.aY + this.aY + this.aY + this.bY + this.bY + this.cY;
				this.bX = this.aX + this.aX + this.aX + this.bX;
				this.bY = this.aY + this.aY + this.aY + this.bY;
				this.numSteps--;
				this.parameter += this.dParameter;
			}

			// Token: 0x060000F8 RID: 248 RVA: 0x0000727C File Offset: 0x0000547C
			private bool MustSubdivide(double flatnessTolerance)
			{
				double num = -(this.aY + this.bY + this.cY);
				double num2 = this.aX + this.bX + this.cX;
				double num3 = Math.Abs(num) + Math.Abs(num2);
				if (num3 > this.distanceTolerance)
				{
					num3 *= flatnessTolerance;
					return Math.Abs(this.cX * num + this.cY * num2) > num3 || Math.Abs((this.bX + this.cX + this.cX) * num + (this.bY + this.cY + this.cY) * num2) > num3;
				}
				return false;
			}

			// Token: 0x0400003A RID: 58
			private double aX;

			// Token: 0x0400003B RID: 59
			private double aY;

			// Token: 0x0400003C RID: 60
			private double bX;

			// Token: 0x0400003D RID: 61
			private double bY;

			// Token: 0x0400003E RID: 62
			private double cX;

			// Token: 0x0400003F RID: 63
			private double cY;

			// Token: 0x04000040 RID: 64
			private double dX;

			// Token: 0x04000041 RID: 65
			private double dY;

			// Token: 0x04000042 RID: 66
			private int numSteps = 1;

			// Token: 0x04000043 RID: 67
			private double flatnessTolerance;

			// Token: 0x04000044 RID: 68
			private double distanceTolerance;

			// Token: 0x04000045 RID: 69
			private bool doParameters;

			// Token: 0x04000046 RID: 70
			private double parameter;

			// Token: 0x04000047 RID: 71
			private double dParameter = 1.0;
		}
	}
}
