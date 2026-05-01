using System;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
	// Token: 0x02000002 RID: 2
	public interface IGeometrySourceParameters
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1
		Stretch Stretch { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2
		Brush Stroke { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000003 RID: 3
		double StrokeThickness { get; }
	}
}
