using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200006E RID: 110
	public class FlickGestureEventArgs : GestureEventArgs
	{
		// Token: 0x0600045D RID: 1117 RVA: 0x00012E26 File Offset: 0x00011026
		internal FlickGestureEventArgs(Point hostOrigin, Point velocity) : base(hostOrigin, hostOrigin)
		{
			this._velocity = velocity;
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x00012E37 File Offset: 0x00011037
		public double HorizontalVelocity
		{
			get
			{
				return this._velocity.X;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x00012E44 File Offset: 0x00011044
		public double VerticalVelocity
		{
			get
			{
				return this._velocity.Y;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00012E51 File Offset: 0x00011051
		public double Angle
		{
			get
			{
				return MathHelpers.GetAngle(this._velocity.X, this._velocity.Y);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00012E6E File Offset: 0x0001106E
		public Orientation Direction
		{
			get
			{
				if (Math.Abs(this._velocity.X) < Math.Abs(this._velocity.Y))
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x04000243 RID: 579
		private Point _velocity;
	}
}
