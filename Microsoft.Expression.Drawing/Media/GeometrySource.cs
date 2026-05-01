using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Media
{
	// Token: 0x0200002B RID: 43
	public abstract class GeometrySource<TParameters> : IGeometrySource where TParameters : IGeometrySourceParameters
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000A61E File Offset: 0x0000881E
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x0000A626 File Offset: 0x00008826
		public Geometry Geometry { get; private set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000A62F File Offset: 0x0000882F
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x0000A637 File Offset: 0x00008837
		public Rect LogicalBounds { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000A640 File Offset: 0x00008840
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x0000A648 File Offset: 0x00008848
		public Rect LayoutBounds { get; private set; }

		// Token: 0x060001B9 RID: 441 RVA: 0x0000A651 File Offset: 0x00008851
		public bool InvalidateGeometry(InvalidateGeometryReasons reasons)
		{
			if ((reasons & InvalidateGeometryReasons.TemplateChanged) != (InvalidateGeometryReasons)0)
			{
				this.cachedGeometry = null;
			}
			if (!this.geometryInvalidated)
			{
				this.geometryInvalidated = true;
				return true;
			}
			return false;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000A674 File Offset: 0x00008874
		public bool UpdateGeometry(IGeometrySourceParameters parameters, Rect layoutBounds)
		{
			bool flag = false;
			if (parameters is TParameters)
			{
				Rect rect = this.ComputeLogicalBounds(layoutBounds, parameters);
				flag |= (this.LayoutBounds != layoutBounds || this.LogicalBounds != rect);
				if (this.geometryInvalidated || flag)
				{
					this.LayoutBounds = layoutBounds;
					this.LogicalBounds = rect;
					flag |= this.UpdateCachedGeometry((TParameters)((object)parameters));
					bool flag2 = flag;
					bool force = flag;
					flag = (flag2 | this.ApplyGeometryEffect(parameters, force));
				}
			}
			this.geometryInvalidated = false;
			return flag;
		}

		// Token: 0x060001BB RID: 443
		protected abstract bool UpdateCachedGeometry(TParameters parameters);

		// Token: 0x060001BC RID: 444 RVA: 0x0000A6F1 File Offset: 0x000088F1
		protected virtual Rect ComputeLogicalBounds(Rect layoutBounds, IGeometrySourceParameters parameters)
		{
			return GeometryHelper.Inflate(layoutBounds, -parameters.GetHalfStrokeThickness());
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000A700 File Offset: 0x00008900
		private bool ApplyGeometryEffect(IGeometrySourceParameters parameters, bool force)
		{
			bool result = false;
			Geometry outputGeometry = this.cachedGeometry;
			GeometryEffect geometryEffect = parameters.GetGeometryEffect();
			if (geometryEffect != null)
			{
				if (force)
				{
					result = true;
					geometryEffect.InvalidateGeometry(InvalidateGeometryReasons.ParentInvalidated);
				}
				if (geometryEffect.ProcessGeometry(this.cachedGeometry))
				{
					result = true;
					outputGeometry = geometryEffect.OutputGeometry;
				}
			}
			if (this.Geometry != outputGeometry)
			{
				result = true;
				this.Geometry = outputGeometry;
			}
			return result;
		}

		// Token: 0x04000075 RID: 117
		private bool geometryInvalidated;

		// Token: 0x04000076 RID: 118
		protected Geometry cachedGeometry;
	}
}
