using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000016 RID: 22
	public sealed class LayoutPath : DependencyObject
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000392B File Offset: 0x00001B2B
		// (set) Token: 0x06000096 RID: 150 RVA: 0x0000393D File Offset: 0x00001B3D
		public FrameworkElement SourceElement
		{
			get
			{
				return (FrameworkElement)base.GetValue(LayoutPath.SourceElementProperty);
			}
			set
			{
				base.SetValue(LayoutPath.SourceElementProperty, value);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000097 RID: 151 RVA: 0x0000394B File Offset: 0x00001B4B
		// (set) Token: 0x06000098 RID: 152 RVA: 0x0000395D File Offset: 0x00001B5D
		public Distribution Distribution
		{
			get
			{
				return (Distribution)base.GetValue(LayoutPath.DistributionProperty);
			}
			set
			{
				base.SetValue(LayoutPath.DistributionProperty, value);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00003970 File Offset: 0x00001B70
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00003982 File Offset: 0x00001B82
		[TypeConverter(typeof(LayoutPathCapacityConverter))]
		public double Capacity
		{
			get
			{
				return (double)base.GetValue(LayoutPath.CapacityProperty);
			}
			set
			{
				base.SetValue(LayoutPath.CapacityProperty, value);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00003995 File Offset: 0x00001B95
		// (set) Token: 0x0600009C RID: 156 RVA: 0x000039A7 File Offset: 0x00001BA7
		public double Padding
		{
			get
			{
				return (double)base.GetValue(LayoutPath.PaddingProperty);
			}
			set
			{
				base.SetValue(LayoutPath.PaddingProperty, value);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600009D RID: 157 RVA: 0x000039BA File Offset: 0x00001BBA
		// (set) Token: 0x0600009E RID: 158 RVA: 0x000039CC File Offset: 0x00001BCC
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(LayoutPath.OrientationProperty);
			}
			set
			{
				base.SetValue(LayoutPath.OrientationProperty, value);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000039DF File Offset: 0x00001BDF
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x000039F1 File Offset: 0x00001BF1
		public double Start
		{
			get
			{
				return (double)base.GetValue(LayoutPath.StartProperty);
			}
			set
			{
				base.SetValue(LayoutPath.StartProperty, value);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00003A04 File Offset: 0x00001C04
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003A16 File Offset: 0x00001C16
		public double Span
		{
			get
			{
				return (double)base.GetValue(LayoutPath.SpanProperty);
			}
			set
			{
				base.SetValue(LayoutPath.SpanProperty, value);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003A29 File Offset: 0x00001C29
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003A3B File Offset: 0x00001C3B
		public FillBehavior FillBehavior
		{
			get
			{
				return (FillBehavior)base.GetValue(LayoutPath.FillBehaviorProperty);
			}
			set
			{
				base.SetValue(LayoutPath.FillBehaviorProperty, value);
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003A4E File Offset: 0x00001C4E
		public LayoutPath()
		{
			this.oldTransformedTestPoints = new Point[LayoutPath.testPoints.Length];
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00003A76 File Offset: 0x00001C76
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00003A7E File Offset: 0x00001C7E
		public double ActualCapacity { get; internal set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00003A88 File Offset: 0x00001C88
		public bool IsValid
		{
			get
			{
				if (this.SourceElement == null || this.pathPanel == null)
				{
					this.isValid = default(bool?);
					return false;
				}
				if (this.isValid != null)
				{
					return this.isValid.Value;
				}
				this.isValid = new bool?(true);
				if (this.SourceElement is PathPanel || this.SourceElement is PathListBox)
				{
					this.isValid = new bool?(false);
				}
				else
				{
					for (DependencyObject parent = VisualTreeHelper.GetParent(this.SourceElement); parent != null; parent = VisualTreeHelper.GetParent(parent))
					{
						PathPanel pathPanel = parent as PathPanel;
						if (pathPanel != null && pathPanel == this.pathPanel)
						{
							this.isValid = new bool?(false);
							break;
						}
					}
				}
				return this.isValid.Value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003B44 File Offset: 0x00001D44
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00003B4C File Offset: 0x00001D4C
		internal bool IsLayoutDirty
		{
			get
			{
				return this.isLayoutDirty;
			}
			set
			{
				if (value && this.IsAttached)
				{
					this.transformedPolylines = null;
					this.strategy.InvalidatePolylineCache();
					this.pathPanel.InvalidateArrange();
				}
				this.isLayoutDirty = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003B8A File Offset: 0x00001D8A
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00003B94 File Offset: 0x00001D94
		internal bool IsRenderDirty
		{
			get
			{
				return this.isRenderDirty;
			}
			set
			{
				if (value && this.IsAttached)
				{
					this.transformedPolylines = null;
					this.pathPanel.InvalidateArrange();
				}
				this.isRenderDirty = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003BC7 File Offset: 0x00001DC7
		internal IList<PolylineData> Polylines
		{
			get
			{
				if (!this.IsAttached)
				{
					return new List<PolylineData>();
				}
				return this.transformedPolylines;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00003BE0 File Offset: 0x00001DE0
		internal double TotalLength
		{
			get
			{
				double num = 0.0;
				if (this.Polylines != null)
				{
					foreach (PolylineData polylineData in this.Polylines)
					{
						num += polylineData.TotalLength;
					}
				}
				return num;
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003C44 File Offset: 0x00001E44
		internal void CheckLayoutState()
		{
			if (this.IsLayoutDirty)
			{
				return;
			}
			if (!this.IsAttached)
			{
				return;
			}
			if (this.strategy.HasGeometryChanged())
			{
				this.IsLayoutDirty = true;
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003C6C File Offset: 0x00001E6C
		internal void CheckRenderState()
		{
			if (this.IsRenderDirty)
			{
				return;
			}
			if (!this.IsAttached)
			{
				return;
			}
			if (this.HaveTestPointsChanged())
			{
				this.IsRenderDirty = true;
				return;
			}
			PathStrategy pathStrategy = this.strategy as PathStrategy;
			if (pathStrategy != null)
			{
				if (this.isSourceParentCanvas)
				{
					this.CheckLayoutState();
					return;
				}
				if (pathStrategy.HaveStartPointsChanged())
				{
					this.IsLayoutDirty = true;
				}
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003CC8 File Offset: 0x00001EC8
		internal void Attach(PathPanel pathPanel)
		{
			this.Detach();
			this.pathPanel = pathPanel;
			if (this.IsValid)
			{
				this.strategy = GeometryStrategy.Create(this);
				this.isSourceParentCanvas = (this.SourceElement.Parent is Canvas);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00003D04 File Offset: 0x00001F04
		internal bool IsAttached
		{
			get
			{
				return this.pathPanel != null && this.SourceElement != null && this.strategy != null;
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003D24 File Offset: 0x00001F24
		internal void Detach()
		{
			if (this.strategy != null)
			{
				this.strategy.Unhook();
				this.strategy = null;
			}
			this.pathPanel = null;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003DA0 File Offset: 0x00001FA0
		internal void UpdateCache()
		{
			if (this.IsRenderDirty || this.IsLayoutDirty)
			{
				IEnumerable<GeneralTransform> transforms = this.ComputeTransforms();
				LayoutPath.testPoints.ForEach(delegate(Point p, int i)
				{
					this.oldTransformedTestPoints[i] = transforms.TransformPoint(p);
				});
				this.transformedPolylines = new List<PolylineData>(this.strategy.Polylines.Count);
				this.transformedPolylines.EnsureListCount(this.strategy.Polylines.Count, null);
				for (int j = 0; j < this.strategy.Polylines.Count; j++)
				{
					PolylineData polylineData = this.strategy.Polylines[j];
					Point[] points = new Point[polylineData.Count];
					polylineData.Points.ForEach(delegate(Point p, int i)
					{
						points[i] = transforms.TransformPoint(p);
					});
					this.transformedPolylines[j] = new PolylineData(points);
				}
				this.IsLayoutDirty = false;
				this.IsRenderDirty = false;
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003EAE File Offset: 0x000020AE
		internal int Distribute(int pathIndex, int childIndex)
		{
			if (!this.IsAttached)
			{
				throw new InvalidOperationException();
			}
			this.UpdateCache();
			return DistributionStrategy.Distribute(this.pathPanel, pathIndex, childIndex);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003ED4 File Offset: 0x000020D4
		internal double GetLengthTo(PolylineData line, MarchLocation location)
		{
			double num = 0.0;
			foreach (PolylineData polylineData in this.Polylines)
			{
				if (polylineData == line)
				{
					break;
				}
				num += polylineData.TotalLength;
			}
			num += location.GetArcLength(line.AccumulatedLength);
			return num;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003F44 File Offset: 0x00002144
		private static void LayoutPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			LayoutPath layoutPath = d as LayoutPath;
			if (layoutPath == null || layoutPath.pathPanel == null)
			{
				return;
			}
			if (e.Property == LayoutPath.SourceElementProperty && e.NewValue != e.OldValue)
			{
				layoutPath.isValid = default(bool?);
				layoutPath.Attach(layoutPath.pathPanel);
			}
			layoutPath.pathPanel.InvalidateArrange();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003FA8 File Offset: 0x000021A8
		private IEnumerable<GeneralTransform> ComputeTransforms()
		{
			IList<GeneralTransform> list = this.strategy.ComputeTransforms() ?? new List<GeneralTransform>();
			list.Add(GeometryHelper.RelativeTransform(this.SourceElement, this.pathPanel));
			return list;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003FE4 File Offset: 0x000021E4
		private bool HaveTestPointsChanged()
		{
			IEnumerable<GeneralTransform> transforms = this.ComputeTransforms();
			for (int i = 0; i < LayoutPath.testPoints.Length; i++)
			{
				if (this.oldTransformedTestPoints[i] != transforms.TransformPoint(LayoutPath.testPoints[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000042 RID: 66
		private PathPanel pathPanel;

		// Token: 0x04000043 RID: 67
		private GeometryStrategy strategy;

		// Token: 0x04000044 RID: 68
		private bool isLayoutDirty = true;

		// Token: 0x04000045 RID: 69
		private bool isRenderDirty = true;

		// Token: 0x04000046 RID: 70
		private bool? isValid;

		// Token: 0x04000047 RID: 71
		private static readonly Point[] testPoints = new Point[]
		{
			new Point(0.0, 0.0),
			new Point(1.0, 0.0),
			new Point(0.0, 1.0),
			new Point(1.0, 1.0)
		};

		// Token: 0x04000048 RID: 72
		private Point[] oldTransformedTestPoints;

		// Token: 0x04000049 RID: 73
		private List<PolylineData> transformedPolylines;

		// Token: 0x0400004A RID: 74
		private bool isSourceParentCanvas;

		// Token: 0x0400004B RID: 75
		public static readonly DependencyProperty SourceElementProperty = DependencyProperty.Register("SourceElement", typeof(FrameworkElement), typeof(LayoutPath), new PropertyMetadata(new PropertyChangedCallback(LayoutPath.LayoutPathChanged)));

		// Token: 0x0400004C RID: 76
		public static readonly DependencyProperty DistributionProperty = DependencyProperty.Register("Distribution", typeof(Distribution), typeof(LayoutPath), new PropertyMetadata(Distribution.Padded, new PropertyChangedCallback(LayoutPath.LayoutPathChanged)));

		// Token: 0x0400004D RID: 77
		public static readonly DependencyProperty CapacityProperty = DependencyProperty.Register("Capacity", typeof(double), typeof(LayoutPath), new PropertyMetadata(double.NaN, new PropertyChangedCallback(LayoutPath.LayoutPathChanged)));

		// Token: 0x0400004E RID: 78
		public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(double), typeof(LayoutPath), new PropertyMetadata(10.0, new PropertyChangedCallback(LayoutPath.LayoutPathChanged)));

		// Token: 0x0400004F RID: 79
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(LayoutPath), new PropertyMetadata(Orientation.None, new PropertyChangedCallback(LayoutPath.LayoutPathChanged)));

		// Token: 0x04000050 RID: 80
		public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(double), typeof(LayoutPath), new PropertyMetadata(0.0, new PropertyChangedCallback(LayoutPath.LayoutPathChanged)));

		// Token: 0x04000051 RID: 81
		public static readonly DependencyProperty SpanProperty = DependencyProperty.Register("Span", typeof(double), typeof(LayoutPath), new PropertyMetadata(1.0, new PropertyChangedCallback(LayoutPath.LayoutPathChanged)));

		// Token: 0x04000052 RID: 82
		public static readonly DependencyProperty FillBehaviorProperty = DependencyProperty.Register("FillBehavior", typeof(FillBehavior), typeof(LayoutPath), new PropertyMetadata(FillBehavior.FullSpan, new PropertyChangedCallback(LayoutPath.LayoutPathChanged)));
	}
}
