using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x02000049 RID: 73
	[DebuggerDisplay("({X}, {Y})")]
	internal struct Vector
	{
		// Token: 0x06000293 RID: 659 RVA: 0x0000F545 File Offset: 0x0000D745
		public static bool operator ==(Vector vector1, Vector vector2)
		{
			return vector1.X == vector2.X && vector1.Y == vector2.Y;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000F569 File Offset: 0x0000D769
		public static bool operator !=(Vector vector1, Vector vector2)
		{
			return !(vector1 == vector2);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000F578 File Offset: 0x0000D778
		public static bool Equals(Vector vector1, Vector vector2)
		{
			return vector1.X.Equals(vector2.X) && vector1.Y.Equals(vector2.Y);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000F5B8 File Offset: 0x0000D7B8
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Vector))
			{
				return false;
			}
			Vector vector = (Vector)obj;
			return Vector.Equals(this, vector);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000F5E5 File Offset: 0x0000D7E5
		public bool Equals(Vector value)
		{
			return Vector.Equals(this, value);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000F5F4 File Offset: 0x0000D7F4
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000F61E File Offset: 0x0000D81E
		// (set) Token: 0x0600029A RID: 666 RVA: 0x0000F626 File Offset: 0x0000D826
		public double X { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000F62F File Offset: 0x0000D82F
		// (set) Token: 0x0600029C RID: 668 RVA: 0x0000F637 File Offset: 0x0000D837
		public double Y { get; set; }

		// Token: 0x0600029D RID: 669 RVA: 0x0000F640 File Offset: 0x0000D840
		public Vector(double x, double y)
		{
			this = default(Vector);
			this.X = x;
			this.Y = y;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000F657 File Offset: 0x0000D857
		public Vector(Point point)
		{
			this = default(Vector);
			this.X = point.X;
			this.Y = point.Y;
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000F67A File Offset: 0x0000D87A
		public double Length
		{
			get
			{
				return Math.Sqrt(this.X * this.X + this.Y * this.Y);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000F69C File Offset: 0x0000D89C
		public double LengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y;
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000F6BC File Offset: 0x0000D8BC
		public void Normalize()
		{
			this /= Math.Max(Math.Abs(this.X), Math.Abs(this.Y));
			this /= this.Length;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000F70C File Offset: 0x0000D90C
		public static double CrossProduct(Vector vector1, Vector vector2)
		{
			return vector1.X * vector2.Y - vector1.Y * vector2.X;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000F730 File Offset: 0x0000D930
		public static double AngleBetween(Vector vector1, Vector vector2)
		{
			double num = vector1.X * vector2.Y - vector2.X * vector1.Y;
			double num2 = vector1.X * vector2.X + vector1.Y * vector2.Y;
			return Math.Atan2(num, num2) * 57.29577951308232;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000F78E File Offset: 0x0000D98E
		public static Vector operator -(Vector vector)
		{
			return new Vector(-vector.X, -vector.Y);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000F7A5 File Offset: 0x0000D9A5
		public void Negate()
		{
			this.X = -this.X;
			this.Y = -this.Y;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000F7C1 File Offset: 0x0000D9C1
		public Size ToSize()
		{
			return new Size(Math.Abs(this.X), Math.Abs(this.Y));
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000F7DE File Offset: 0x0000D9DE
		public Point ToPoint()
		{
			return new Point(this.X, this.Y);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000F7F1 File Offset: 0x0000D9F1
		public static Vector operator +(Vector vector1, Vector vector2)
		{
			return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000F816 File Offset: 0x0000DA16
		public static Vector Add(Vector vector1, Vector vector2)
		{
			return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000F83B File Offset: 0x0000DA3B
		public static Vector operator -(Vector vector1, Vector vector2)
		{
			return new Vector(vector1.X - vector2.X, vector1.Y - vector2.Y);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000F860 File Offset: 0x0000DA60
		public static Vector Subtract(Vector vector1, Vector vector2)
		{
			return new Vector(vector1.X - vector2.X, vector1.Y - vector2.Y);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000F885 File Offset: 0x0000DA85
		public static Point operator +(Vector vector, Point point)
		{
			return new Point(point.X + vector.X, point.Y + vector.Y);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000F8AA File Offset: 0x0000DAAA
		public static Point Add(Vector vector, Point point)
		{
			return new Point(point.X + vector.X, point.Y + vector.Y);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000F8CF File Offset: 0x0000DACF
		public static Vector operator *(Vector vector, double scalar)
		{
			return new Vector(vector.X * scalar, vector.Y * scalar);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000F8E8 File Offset: 0x0000DAE8
		public static Vector Multiply(Vector vector, double scalar)
		{
			return new Vector(vector.X * scalar, vector.Y * scalar);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000F901 File Offset: 0x0000DB01
		public static Vector operator *(double scalar, Vector vector)
		{
			return new Vector(vector.X * scalar, vector.Y * scalar);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000F91A File Offset: 0x0000DB1A
		public static Vector Multiply(double scalar, Vector vector)
		{
			return new Vector(vector.X * scalar, vector.Y * scalar);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000F933 File Offset: 0x0000DB33
		public static Vector operator /(Vector vector, double scalar)
		{
			return vector * (1.0 / scalar);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000F946 File Offset: 0x0000DB46
		public static Vector Divide(Vector vector, double scalar)
		{
			return vector * (1.0 / scalar);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000F95C File Offset: 0x0000DB5C
		public static Vector operator *(Vector vector, Matrix matrix)
		{
			Point point = matrix.Transform(new Point(vector.X, vector.Y));
			point.X -= matrix.OffsetX;
			point.Y -= matrix.OffsetY;
			return new Vector(point);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000F9B4 File Offset: 0x0000DBB4
		public static Vector Multiply(Vector vector, Matrix matrix)
		{
			Point point = matrix.Transform(new Point(vector.X, vector.Y));
			point.X -= matrix.OffsetX;
			point.Y -= matrix.OffsetY;
			return new Vector(point);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000FA0C File Offset: 0x0000DC0C
		public static double operator *(Vector vector1, Vector vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000FA2D File Offset: 0x0000DC2D
		public static double Multiply(Vector vector1, Vector vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000FA4E File Offset: 0x0000DC4E
		public static double Determinant(Vector vector1, Vector vector2)
		{
			return vector1.X * vector2.Y - vector1.Y * vector2.X;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000FA6F File Offset: 0x0000DC6F
		public static explicit operator Size(Vector vector)
		{
			return vector.ToSize();
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000FA78 File Offset: 0x0000DC78
		public static explicit operator Point(Vector vector)
		{
			return vector.ToPoint();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000FA81 File Offset: 0x0000DC81
		public static Point operator +(Point point, Vector vector)
		{
			return new Point(point.X + vector.X, point.Y + vector.Y);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000FAA6 File Offset: 0x0000DCA6
		public static Point operator -(Point point, Vector vector)
		{
			return new Point(point.X - vector.X, point.Y - vector.Y);
		}
	}
}
