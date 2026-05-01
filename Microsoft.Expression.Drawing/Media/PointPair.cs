using System;
using System.Windows;

namespace Microsoft.Expression.Media
{
	// Token: 0x0200002F RID: 47
	internal struct PointPair
	{
		// Token: 0x060001E6 RID: 486 RVA: 0x0000BD34 File Offset: 0x00009F34
		public PointPair(Point p1, Point p2)
		{
			this.Item1 = p1;
			this.Item2 = p2;
		}

		// Token: 0x04000091 RID: 145
		public Point Item1;

		// Token: 0x04000092 RID: 146
		public Point Item2;
	}
}
