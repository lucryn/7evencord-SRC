using System;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
	// Token: 0x0200002A RID: 42
	public interface IGeometrySource
	{
		// Token: 0x060001AE RID: 430
		bool InvalidateGeometry(InvalidateGeometryReasons reasons);

		// Token: 0x060001AF RID: 431
		bool UpdateGeometry(IGeometrySourceParameters parameters, Rect layoutBounds);

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001B0 RID: 432
		Geometry Geometry { get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001B1 RID: 433
		Rect LogicalBounds { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001B2 RID: 434
		Rect LayoutBounds { get; }
	}
}
