using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000009 RID: 9
	internal class EllipseStrategy : ShapeStrategy
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00002FF1 File Offset: 0x000011F1
		public EllipseStrategy(LayoutPath layoutPath) : base(layoutPath)
		{
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002FFC File Offset: 0x000011FC
		protected override PathGeometry UpdateGeometry()
		{
			return new EllipseGeometry
			{
				RadiusX = base.Size.Width / 2.0,
				RadiusY = base.Size.Height / 2.0,
				Center = new Point(base.Size.Width / 2.0, base.Size.Height / 2.0)
			}.AsPathGeometry();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000308D File Offset: 0x0000128D
		public override IList<GeneralTransform> ComputeTransforms()
		{
			return null;
		}
	}
}
