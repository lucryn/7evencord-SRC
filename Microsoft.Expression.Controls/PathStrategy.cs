using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Controls
{
	// Token: 0x0200000D RID: 13
	internal class PathStrategy : ShapeStrategy
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003581 File Offset: 0x00001781
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00003593 File Offset: 0x00001793
		private Geometry DataListener
		{
			get
			{
				return (Geometry)base.GetValue(PathStrategy.DataListenerProperty);
			}
			set
			{
				base.SetValue(PathStrategy.DataListenerProperty, value);
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000035A1 File Offset: 0x000017A1
		public PathStrategy(LayoutPath layoutPath) : base(layoutPath)
		{
			this.sourcePath = (Path)layoutPath.SourceElement;
			base.SetListenerBinding(PathStrategy.DataListenerProperty, "Data");
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000035CC File Offset: 0x000017CC
		public override bool HasGeometryChanged()
		{
			bool startPointsOnly = false;
			return this.HasGeometryChangedInternal(startPointsOnly);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000035E4 File Offset: 0x000017E4
		internal bool HaveStartPointsChanged()
		{
			bool startPointsOnly = true;
			return this.HasGeometryChangedInternal(startPointsOnly);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000035FC File Offset: 0x000017FC
		protected override PathGeometry UpdateGeometry()
		{
			if (this.sourcePath == null || this.sourcePath.Data == null)
			{
				this.oldGeometry = null;
				this.oldGeometryString = null;
				return null;
			}
			string currentGeometryString = this.GetCurrentGeometryString();
			Geometry currentGeometry = this.GetCurrentGeometry(currentGeometryString);
			if (currentGeometry == null)
			{
				this.oldGeometry = null;
				this.oldGeometryString = null;
				return null;
			}
			if (string.IsNullOrEmpty(currentGeometryString))
			{
				this.oldGeometryString = null;
			}
			else
			{
				this.oldGeometryString = currentGeometryString;
			}
			this.oldGeometry = currentGeometry.CloneCurrentValue();
			return this.oldGeometry.AsPathGeometry();
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003680 File Offset: 0x00001880
		public override IList<GeneralTransform> ComputeTransforms()
		{
			IList<GeneralTransform> list = base.ComputeTransforms() ?? new List<GeneralTransform>();
			if (this.sourcePath != null && this.sourcePath.Data != null)
			{
				list.Add(this.sourcePath.Data.Transform);
			}
			return list;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000036CC File Offset: 0x000018CC
		internal static bool StartPointsEqual(PathGeometry firstGeometry, PathGeometry secondGeomety)
		{
			if (firstGeometry == secondGeomety)
			{
				return true;
			}
			if (firstGeometry == null || secondGeomety == null)
			{
				return false;
			}
			if (firstGeometry.Figures.Count != secondGeomety.Figures.Count)
			{
				return false;
			}
			for (int i = 0; i < firstGeometry.Figures.Count; i++)
			{
				PathFigure pathFigure = firstGeometry.Figures[i];
				PathFigure pathFigure2 = secondGeomety.Figures[i];
				if (pathFigure.StartPoint != pathFigure2.StartPoint)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003748 File Offset: 0x00001948
		private bool HasGeometryChangedInternal(bool startPointsOnly)
		{
			if (this.sourcePath == null || this.sourcePath.Data == null)
			{
				return this.oldGeometry != null || this.oldGeometryString != null;
			}
			string currentGeometryString = this.GetCurrentGeometryString();
			if (!string.IsNullOrEmpty(this.oldGeometryString))
			{
				return this.oldGeometryString != currentGeometryString;
			}
			Geometry currentGeometry = this.GetCurrentGeometry(currentGeometryString);
			if (startPointsOnly)
			{
				return !PathStrategy.StartPointsEqual(this.oldGeometry as PathGeometry, currentGeometry as PathGeometry);
			}
			return !GeometryHelper.GeometryEquals(this.oldGeometry, currentGeometry);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000037D8 File Offset: 0x000019D8
		private string GetCurrentGeometryString()
		{
			PathGeometry pathGeometry = this.sourcePath.Data as PathGeometry;
			if (pathGeometry != null)
			{
				return pathGeometry.ToString();
			}
			return null;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003801 File Offset: 0x00001A01
		private Geometry GetCurrentGeometry(string pathString)
		{
			if (!string.IsNullOrEmpty(pathString))
			{
				return PathGeometryHelper.ConvertToPathGeometry(pathString);
			}
			return this.sourcePath.Data;
		}

		// Token: 0x04000022 RID: 34
		private Path sourcePath;

		// Token: 0x04000023 RID: 35
		private static readonly DependencyProperty DataListenerProperty = DependencyProperty.Register("DataListener", typeof(Geometry), typeof(PathStrategy), new PropertyMetadata(new PropertyChangedCallback(GeometryStrategy.LayoutPropertyChanged)));

		// Token: 0x04000024 RID: 36
		private Geometry oldGeometry;

		// Token: 0x04000025 RID: 37
		private string oldGeometryString;
	}
}
