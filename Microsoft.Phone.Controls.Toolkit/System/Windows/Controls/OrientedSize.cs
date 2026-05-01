using System;

namespace System.Windows.Controls
{
	// Token: 0x0200003F RID: 63
	internal struct OrientedSize
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00008DD6 File Offset: 0x00006FD6
		public Orientation Orientation
		{
			get
			{
				return this._orientation;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00008DDE File Offset: 0x00006FDE
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00008DE6 File Offset: 0x00006FE6
		public double Direct
		{
			get
			{
				return this._direct;
			}
			set
			{
				this._direct = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00008DEF File Offset: 0x00006FEF
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00008DF7 File Offset: 0x00006FF7
		public double Indirect
		{
			get
			{
				return this._indirect;
			}
			set
			{
				this._indirect = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00008E00 File Offset: 0x00007000
		// (set) Token: 0x060001EC RID: 492 RVA: 0x00008E18 File Offset: 0x00007018
		public double Width
		{
			get
			{
				if (this.Orientation != 1)
				{
					return this.Indirect;
				}
				return this.Direct;
			}
			set
			{
				if (this.Orientation == 1)
				{
					this.Direct = value;
					return;
				}
				this.Indirect = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00008E32 File Offset: 0x00007032
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00008E4A File Offset: 0x0000704A
		public double Height
		{
			get
			{
				if (this.Orientation == 1)
				{
					return this.Indirect;
				}
				return this.Direct;
			}
			set
			{
				if (this.Orientation != 1)
				{
					this.Direct = value;
					return;
				}
				this.Indirect = value;
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00008E64 File Offset: 0x00007064
		public OrientedSize(Orientation orientation)
		{
			this = new OrientedSize(orientation, 0.0, 0.0);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00008E7F File Offset: 0x0000707F
		public OrientedSize(Orientation orientation, double width, double height)
		{
			this._orientation = orientation;
			this._direct = 0.0;
			this._indirect = 0.0;
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x040000C0 RID: 192
		private Orientation _orientation;

		// Token: 0x040000C1 RID: 193
		private double _direct;

		// Token: 0x040000C2 RID: 194
		private double _indirect;
	}
}
