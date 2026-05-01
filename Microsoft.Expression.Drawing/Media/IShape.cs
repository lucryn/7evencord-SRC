using System;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
	// Token: 0x02000003 RID: 3
	public interface IShape
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000004 RID: 4
		// (set) Token: 0x06000005 RID: 5
		Brush Fill { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000006 RID: 6
		// (set) Token: 0x06000007 RID: 7
		Brush Stroke { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000008 RID: 8
		// (set) Token: 0x06000009 RID: 9
		double StrokeThickness { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000A RID: 10
		// (set) Token: 0x0600000B RID: 11
		Stretch Stretch { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000C RID: 12
		Geometry RenderedGeometry { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000D RID: 13
		Thickness GeometryMargin { get; }

		// Token: 0x0600000E RID: 14
		void InvalidateGeometry(InvalidateGeometryReasons reasons);

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600000F RID: 15
		// (remove) Token: 0x06000010 RID: 16
		event EventHandler RenderedGeometryChanged;
	}
}
