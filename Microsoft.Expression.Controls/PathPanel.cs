using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Expression.Drawing.Core;

namespace Microsoft.Expression.Controls
{
	// Token: 0x0200001C RID: 28
	public sealed class PathPanel : Panel
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00004D99 File Offset: 0x00002F99
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00004DAB File Offset: 0x00002FAB
		public LayoutPathCollection LayoutPaths
		{
			get
			{
				return (LayoutPathCollection)base.GetValue(PathPanel.LayoutPathsProperty);
			}
			set
			{
				base.SetValue(PathPanel.LayoutPathsProperty, value);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00004DB9 File Offset: 0x00002FB9
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00004DCB File Offset: 0x00002FCB
		public double StartItemIndex
		{
			get
			{
				return (double)base.GetValue(PathPanel.StartItemIndexProperty);
			}
			set
			{
				base.SetValue(PathPanel.StartItemIndexProperty, value);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00004DDE File Offset: 0x00002FDE
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00004DF0 File Offset: 0x00002FF0
		public bool WrapItems
		{
			get
			{
				return (bool)base.GetValue(PathPanel.WrapItemsProperty);
			}
			set
			{
				base.SetValue(PathPanel.WrapItemsProperty, value);
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004E04 File Offset: 0x00003004
		public PathPanel()
		{
			LayoutPathCollection layoutPathCollection = new LayoutPathCollection();
			base.SetValue(PathPanel.LayoutPathsProperty, layoutPathCollection);
			base.Loaded += new RoutedEventHandler(this.PathPanel_Loaded);
			base.Unloaded += new RoutedEventHandler(this.PathPanel_Unloaded);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004E50 File Offset: 0x00003050
		protected override Size MeasureOverride(Size availableSize)
		{
			foreach (UIElement uielement in base.Children)
			{
				if (uielement != null)
				{
					uielement.Measure(PathPanel.InfinteSize);
				}
			}
			return base.MeasureOverride(availableSize);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004ECC File Offset: 0x000030CC
		protected override Size ArrangeOverride(Size finalSize)
		{
			Size result = base.ArrangeOverride(finalSize);
			if (this.LayoutPaths != null && this.LayoutPaths.Count > 0)
			{
				foreach (LayoutPath layoutPath in this.LayoutPaths)
				{
					if (!layoutPath.IsAttached)
					{
						layoutPath.Attach(this);
					}
				}
				this.ValidPaths = new List<LayoutPath>(Enumerable.Where<LayoutPath>(this.LayoutPaths, (LayoutPath path) => path.IsAttached && path.SourceElement.Visibility != 1));
			}
			else
			{
				this.ValidPaths = null;
			}
			if (base.Children.Count == 0)
			{
				return result;
			}
			if (this.ValidPaths == null || this.ValidPaths.Count == 0)
			{
				this.ArrangeFirstChild();
				return result;
			}
			if (!this.UpdateIndirection())
			{
				return result;
			}
			this.lastPoint = default(Point);
			this.previousLength = 0.0;
			foreach (LayoutPath layoutPath2 in this.ValidPaths)
			{
				layoutPath2.UpdateCache();
			}
			this.totalLength = 0.0;
			foreach (LayoutPath layoutPath3 in this.ValidPaths)
			{
				this.totalLength += layoutPath3.TotalLength;
			}
			int i = 0;
			for (int j = 0; j < this.LayoutPaths.Count; j++)
			{
				if (i >= this.Count)
				{
					break;
				}
				LayoutPath layoutPath4 = this.LayoutPaths[j];
				if (layoutPath4.IsAttached && layoutPath4.SourceElement.Visibility != 1)
				{
					i = layoutPath4.Distribute(j, i);
					this.previousLength += layoutPath4.TotalLength;
				}
			}
			while (i < this.Count)
			{
				UIElement hiddenChild = base.Children[this.indices[i]];
				PathPanel.HideAtPoint(hiddenChild, this.lastPoint);
				i++;
			}
			return result;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00005118 File Offset: 0x00003318
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00005120 File Offset: 0x00003320
		internal IList<LayoutPath> ValidPaths { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00005129 File Offset: 0x00003329
		internal int Count
		{
			get
			{
				if (this.indices == null)
				{
					return 0;
				}
				return this.indices.Length;
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005140 File Offset: 0x00003340
		internal void ArrangeChild(int indirectIndex, int pathIndex, PolylineData polyline, MarchLocation location, int localIndex)
		{
			LayoutPath layoutPath = this.LayoutPaths[pathIndex];
			int num = this.indices[indirectIndex];
			UIElement uielement = base.Children[num];
			this.lastPoint = location.GetPoint(polyline.Points);
			if (this.shouldLayoutHiddenChildren)
			{
				int num2 = Math.Min(base.Children.Count, Math.Max(0, (int)Math.Round(this.StartItemIndex)));
				for (int i = 0; i < num2; i++)
				{
					PathPanel.HideAtPoint(base.Children[i], this.lastPoint);
				}
				this.shouldLayoutHiddenChildren = false;
			}
			IPathLayoutItem pathLayoutItem = uielement as IPathLayoutItem;
			if (pathLayoutItem != null)
			{
				Vector vector = -location.GetNormal(polyline, 10.0);
				double num3 = Vector.AngleBetween(PathPanel.Up, vector);
				double lengthTo = layoutPath.GetLengthTo(polyline, location);
				double rhs = layoutPath.TotalLength;
				pathLayoutItem.Update(new PathLayoutData
				{
					LayoutPathIndex = pathIndex,
					GlobalIndex = num,
					LocalIndex = localIndex,
					NormalAngle = num3,
					OrientationAngle = ((layoutPath.Orientation == Orientation.OrientToPath) ? num3 : 0.0),
					LocalOffset = MathHelper.SafeDivide(lengthTo, rhs, 0.0),
					GlobalOffset = MathHelper.SafeDivide(this.previousLength + lengthTo, this.totalLength, 0.0),
					IsArranged = true
				});
			}
			Rect rect;
			rect..ctor(this.lastPoint, default(Size));
			double num4 = uielement.DesiredSize.Width / 2.0;
			double num5 = uielement.DesiredSize.Height / 2.0;
			rect = GeometryHelper.Inflate(rect, new Thickness(num4, num5, num4, num5));
			uielement.Arrange(rect);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005324 File Offset: 0x00003524
		internal double GetChildRadius(int indirectIndex)
		{
			UIElement uielement = base.Children[this.indices[indirectIndex]];
			return Math.Max(uielement.DesiredSize.Width, uielement.DesiredSize.Height) / 2.0;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005370 File Offset: 0x00003570
		private bool UpdateIndirection()
		{
			int num = base.Children.Count;
			int num2 = Math.Max(0, (int)Math.Round(this.StartItemIndex));
			if (!this.WrapItems)
			{
				num2 = Math.Min(num, num2);
				num -= num2;
				if (num <= 0)
				{
					foreach (UIElement uielement in base.Children)
					{
						uielement.Arrange(PathPanel.ZeroRect);
						PathPanel.RemovePathLayoutProperties(uielement as PathListBoxItem, false);
					}
					return false;
				}
				this.shouldLayoutHiddenChildren = true;
			}
			else
			{
				num2 %= num;
			}
			if (this.indices == null || this.indices.Length != num)
			{
				this.indices = new int[num];
				int num3 = num2;
				for (int i = 0; i < num; i++)
				{
					this.indices[i] = num3;
					num3 = (num3 + 1) % base.Children.Count;
				}
			}
			return true;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005464 File Offset: 0x00003664
		private static void HideAtPoint(UIElement hiddenChild, Point point)
		{
			Point point2 = point.Minus(new Point(hiddenChild.DesiredSize.Width / 2.0, hiddenChild.DesiredSize.Height / 2.0));
			PathPanel.RemovePathLayoutProperties(hiddenChild as PathListBoxItem, false);
			hiddenChild.Arrange(new Rect(point2, new Size(0.0, 0.0)));
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000054DC File Offset: 0x000036DC
		private void ArrangeFirstChild()
		{
			UIElement uielement = base.Children[0];
			uielement.Arrange(new Rect(default(Point), base.Children[0].DesiredSize));
			bool isArranged = true;
			PathPanel.RemovePathLayoutProperties(uielement as PathListBoxItem, isArranged);
			for (int i = 1; i < base.Children.Count; i++)
			{
				UIElement uielement2 = base.Children[i];
				uielement2.Arrange(PathPanel.ZeroRect);
				PathPanel.RemovePathLayoutProperties(uielement2 as PathListBoxItem, false);
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005565 File Offset: 0x00003765
		private void PathPanel_Loaded(object sender, RoutedEventArgs e)
		{
			base.InvalidateArrange();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000556D File Offset: 0x0000376D
		private void PathPanel_Unloaded(object sender, RoutedEventArgs e)
		{
			CompositionTarget.Rendering -= new EventHandler(this.CheckOnRenderHandler);
			base.LayoutUpdated -= new EventHandler(this.CheckOnLayoutHandler);
			this.isListening = false;
			this.isUnloaded = true;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000055A0 File Offset: 0x000037A0
		private static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PathPanel pathPanel = d as PathPanel;
			if (pathPanel == null)
			{
				return;
			}
			if (e.Property == PathPanel.StartItemIndexProperty || e.Property == PathPanel.WrapItemsProperty)
			{
				pathPanel.indices = null;
			}
			else if (e.Property == PathPanel.LayoutPathsProperty)
			{
				if (e.NewValue == e.OldValue)
				{
					return;
				}
				LayoutPathCollection layoutPathCollection = e.OldValue as LayoutPathCollection;
				if (layoutPathCollection != null)
				{
					foreach (LayoutPath layoutPath in layoutPathCollection)
					{
						layoutPath.Detach();
					}
					layoutPathCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(pathPanel.LayoutPaths_CollectionChanged);
				}
				LayoutPathCollection layoutPathCollection2 = e.NewValue as LayoutPathCollection;
				if (layoutPathCollection2 != null)
				{
					foreach (LayoutPath layoutPath2 in layoutPathCollection2)
					{
						layoutPath2.Attach(pathPanel);
					}
					layoutPathCollection2.CollectionChanged += new NotifyCollectionChangedEventHandler(pathPanel.LayoutPaths_CollectionChanged);
				}
				PathPanel.UpdateListeners(pathPanel);
			}
			pathPanel.InvalidateArrange();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000056D0 File Offset: 0x000038D0
		private void LayoutPaths_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			bool flag = false;
			if (e.OldItems != null)
			{
				foreach (object obj in e.OldItems)
				{
					LayoutPath layoutPath = (LayoutPath)obj;
					layoutPath.Detach();
					flag = true;
				}
			}
			if (e.NewItems != null)
			{
				foreach (object obj2 in e.NewItems)
				{
					LayoutPath layoutPath2 = (LayoutPath)obj2;
					layoutPath2.Attach(this);
					flag = true;
				}
			}
			if (flag)
			{
				PathPanel.UpdateListeners(this);
				base.InvalidateArrange();
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000057A0 File Offset: 0x000039A0
		private static void UpdateListeners(PathPanel pathPanel)
		{
			if (pathPanel.isUnloaded)
			{
				return;
			}
			bool flag = pathPanel.LayoutPaths != null && pathPanel.LayoutPaths.Count > 0;
			if (!pathPanel.isListening && flag)
			{
				CompositionTarget.Rendering += new EventHandler(pathPanel.CheckOnRenderHandler);
				pathPanel.LayoutUpdated += new EventHandler(pathPanel.CheckOnLayoutHandler);
				pathPanel.isListening = true;
				return;
			}
			if (pathPanel.isListening && !flag)
			{
				CompositionTarget.Rendering -= new EventHandler(pathPanel.CheckOnRenderHandler);
				pathPanel.LayoutUpdated -= new EventHandler(pathPanel.CheckOnLayoutHandler);
				pathPanel.isListening = false;
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000583C File Offset: 0x00003A3C
		private void CheckOnRenderHandler(object sender, EventArgs e)
		{
			if (this.LayoutPaths == null || this.LayoutPaths.Count == 0)
			{
				return;
			}
			foreach (LayoutPath layoutPath in this.LayoutPaths)
			{
				if (layoutPath.IsAttached)
				{
					layoutPath.CheckRenderState();
				}
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000058A8 File Offset: 0x00003AA8
		private void CheckOnLayoutHandler(object sender, EventArgs e)
		{
			if (this.LayoutPaths == null || this.LayoutPaths.Count == 0)
			{
				return;
			}
			foreach (LayoutPath layoutPath in this.LayoutPaths)
			{
				if (layoutPath.IsAttached)
				{
					layoutPath.CheckLayoutState();
				}
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005914 File Offset: 0x00003B14
		private static void RemovePathLayoutProperties(IPathLayoutItem pathLayoutItem, bool isArranged = false)
		{
			if (pathLayoutItem == null)
			{
				return;
			}
			pathLayoutItem.Update(new PathLayoutData
			{
				LayoutPathIndex = 0,
				GlobalIndex = 0,
				LocalIndex = 0,
				NormalAngle = 0.0,
				OrientationAngle = 0.0,
				LocalOffset = 0.0,
				GlobalOffset = 0.0,
				IsArranged = isArranged
			});
		}

		// Token: 0x04000065 RID: 101
		private const double SmoothNormalRange = 10.0;

		// Token: 0x04000066 RID: 102
		private static readonly Rect ZeroRect = new Rect(0.0, 0.0, 0.0, 0.0);

		// Token: 0x04000067 RID: 103
		private static readonly Size InfinteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

		// Token: 0x04000068 RID: 104
		private static readonly Vector Up = new Vector(0.0, -1.0);

		// Token: 0x04000069 RID: 105
		private Point lastPoint;

		// Token: 0x0400006A RID: 106
		private double totalLength;

		// Token: 0x0400006B RID: 107
		private double previousLength;

		// Token: 0x0400006C RID: 108
		private bool shouldLayoutHiddenChildren;

		// Token: 0x0400006D RID: 109
		private bool isListening;

		// Token: 0x0400006E RID: 110
		private bool isUnloaded;

		// Token: 0x0400006F RID: 111
		private int[] indices;

		// Token: 0x04000070 RID: 112
		public static readonly DependencyProperty LayoutPathsProperty = DependencyProperty.Register("LayoutPaths", typeof(LayoutPathCollection), typeof(PathPanel), new PropertyMetadata(null, new PropertyChangedCallback(PathPanel.LayoutPropertyChanged)));

		// Token: 0x04000071 RID: 113
		public static readonly DependencyProperty StartItemIndexProperty = DependencyProperty.Register("StartItemIndex", typeof(double), typeof(PathPanel), new PropertyMetadata(0.0, new PropertyChangedCallback(PathPanel.LayoutPropertyChanged)));

		// Token: 0x04000072 RID: 114
		public static readonly DependencyProperty WrapItemsProperty = DependencyProperty.Register("WrapItems", typeof(bool), typeof(PathPanel), new PropertyMetadata(false, new PropertyChangedCallback(PathPanel.LayoutPropertyChanged)));
	}
}
