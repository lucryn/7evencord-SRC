using System;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200006A RID: 106
	public class GestureEventArgs : EventArgs
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x00012C5D File Offset: 0x00010E5D
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x00012C65 File Offset: 0x00010E65
		private protected Point GestureOrigin { protected get; private set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x00012C6E File Offset: 0x00010E6E
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x00012C76 File Offset: 0x00010E76
		private protected Point TouchPosition { protected get; private set; }

		// Token: 0x06000441 RID: 1089 RVA: 0x00012C7F File Offset: 0x00010E7F
		internal GestureEventArgs(Point gestureOrigin, Point position)
		{
			this.GestureOrigin = gestureOrigin;
			this.TouchPosition = position;
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00012C95 File Offset: 0x00010E95
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x00012C9D File Offset: 0x00010E9D
		public object OriginalSource { get; internal set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00012CA6 File Offset: 0x00010EA6
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x00012CAE File Offset: 0x00010EAE
		public bool Handled { get; set; }

		// Token: 0x06000446 RID: 1094 RVA: 0x00012CB7 File Offset: 0x00010EB7
		public Point GetPosition(UIElement relativeTo)
		{
			return GestureEventArgs.GetPosition(relativeTo, this.TouchPosition);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00012CC8 File Offset: 0x00010EC8
		protected static Point GetPosition(UIElement relativeTo, Point point)
		{
			if (relativeTo == null)
			{
				relativeTo = Application.Current.RootVisual;
			}
			if (relativeTo != null)
			{
				GeneralTransform inverse = relativeTo.TransformToVisual(null).Inverse;
				return inverse.Transform(point);
			}
			return point;
		}
	}
}
