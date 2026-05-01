using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Expression.Drawing.Core;
using Microsoft.Expression.Media;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000007 RID: 7
	internal class IShapeStrategy : FrameworkElementStrategy
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00002DD8 File Offset: 0x00000FD8
		public IShapeStrategy(LayoutPath layoutPath) : base(layoutPath)
		{
			this.sourceIShape = (IShape)layoutPath.SourceElement;
			this.sourceIShape.RenderedGeometryChanged += new EventHandler(this.RenderedGeometryChanged);
			this.sourceShape = (layoutPath.SourceElement as Shape);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002E25 File Offset: 0x00001025
		public override void Unhook()
		{
			this.sourceIShape.RenderedGeometryChanged -= new EventHandler(this.RenderedGeometryChanged);
			base.Unhook();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002E44 File Offset: 0x00001044
		public override IList<GeneralTransform> ComputeTransforms()
		{
			IList<GeneralTransform> list = base.ComputeTransforms() ?? new List<GeneralTransform>();
			if (this.sourceShape != null)
			{
				list.Add(this.sourceShape.GeometryTransform);
			}
			return list;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002E7B File Offset: 0x0000107B
		protected override PathGeometry UpdateGeometry()
		{
			return this.sourceIShape.RenderedGeometry.AsPathGeometry();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002E8D File Offset: 0x0000108D
		private void RenderedGeometryChanged(object sender, EventArgs e)
		{
			base.LayoutPath.IsLayoutDirty = true;
		}

		// Token: 0x04000012 RID: 18
		private IShape sourceIShape;

		// Token: 0x04000013 RID: 19
		private Shape sourceShape;
	}
}
