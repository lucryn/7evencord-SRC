using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Expression.Drawing.Core;
using Microsoft.Expression.Media;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000004 RID: 4
	internal abstract class GeometryStrategy : DependencyObject
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000278A File Offset: 0x0000098A
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002792 File Offset: 0x00000992
		private protected LayoutPath LayoutPath { protected get; private set; }

		// Token: 0x0600001B RID: 27
		protected abstract PathGeometry UpdateGeometry();

		// Token: 0x0600001C RID: 28
		public abstract bool HasGeometryChanged();

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000279B File Offset: 0x0000099B
		public IList<PolylineData> Polylines
		{
			get
			{
				if (this.polylineCache == null)
				{
					this.UpdatePolyline(this.UpdateGeometry());
				}
				return this.polylineCache;
			}
		}

		// Token: 0x0600001E RID: 30
		public abstract IList<GeneralTransform> ComputeTransforms();

		// Token: 0x0600001F RID: 31 RVA: 0x000027B7 File Offset: 0x000009B7
		protected GeometryStrategy(LayoutPath layoutPath)
		{
			this.LayoutPath = layoutPath;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000027C8 File Offset: 0x000009C8
		public static GeometryStrategy Create(LayoutPath layoutPath)
		{
			if (layoutPath == null)
			{
				throw new ArgumentNullException("layoutPath");
			}
			if (layoutPath.SourceElement == null)
			{
				throw new InvalidOperationException();
			}
			if (layoutPath.SourceElement is IShape)
			{
				return new IShapeStrategy(layoutPath);
			}
			if (layoutPath.SourceElement is Path)
			{
				return new PathStrategy(layoutPath);
			}
			if (layoutPath.SourceElement is Rectangle)
			{
				return new RectangleStrategy(layoutPath);
			}
			if (layoutPath.SourceElement is Ellipse)
			{
				return new EllipseStrategy(layoutPath);
			}
			if (layoutPath.SourceElement is Line)
			{
				return new LineStrategy(layoutPath);
			}
			if (layoutPath.SourceElement is Polygon)
			{
				return new PolygonStrategy(layoutPath);
			}
			if (layoutPath.SourceElement is Polyline)
			{
				return new PolylineStrategy(layoutPath);
			}
			if (layoutPath.SourceElement is Shape)
			{
				return new ShapeStrategy(layoutPath);
			}
			return new FrameworkElementStrategy(layoutPath);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002897 File Offset: 0x00000A97
		public void InvalidatePolylineCache()
		{
			this.polylineCache = null;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000028A0 File Offset: 0x00000AA0
		public virtual void Unhook()
		{
			this.LayoutPath = null;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000028AC File Offset: 0x00000AAC
		protected static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GeometryStrategy geometryStrategy = d as GeometryStrategy;
			if (geometryStrategy == null || geometryStrategy.LayoutPath == null || !geometryStrategy.LayoutPath.IsAttached)
			{
				return;
			}
			geometryStrategy.LayoutPath.IsLayoutDirty = true;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000028E8 File Offset: 0x00000AE8
		protected void SetListenerBinding(DependencyProperty targetProperty, string sourceProperty)
		{
			BindingOperations.SetBinding(this, targetProperty, new Binding(sourceProperty)
			{
				Source = this.LayoutPath.SourceElement,
				Mode = 1
			});
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002920 File Offset: 0x00000B20
		private void UpdatePolyline(PathGeometry pathGeometry)
		{
			if (pathGeometry == null)
			{
				this.polylineCache = new List<PolylineData>();
				return;
			}
			List<PolylineData> list = new List<PolylineData>();
			foreach (PathFigure figure in pathGeometry.Figures)
			{
				List<Point> list2 = new List<Point>();
				bool removeRepeat = true;
				PathFigureHelper.FlattenFigure(figure, list2, 0.1, removeRepeat);
				if (list2.Count > 0)
				{
					if (list2.Count == 1)
					{
						list2.Add(list2[0]);
					}
					list.Add(new PolylineData(list2));
				}
			}
			this.polylineCache = list;
		}

		// Token: 0x04000007 RID: 7
		private const double FlatteningTolerance = 0.1;

		// Token: 0x04000008 RID: 8
		private List<PolylineData> polylineCache;
	}
}
