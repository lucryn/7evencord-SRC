using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Expression.Controls
{
	// Token: 0x0200001B RID: 27
	public sealed class PathListBoxItem : ListBoxItem, IPathLayoutItem
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000D8 RID: 216 RVA: 0x00004920 File Offset: 0x00002B20
		// (remove) Token: 0x060000D9 RID: 217 RVA: 0x00004958 File Offset: 0x00002B58
		public event EventHandler<PathLayoutUpdatedEventArgs> PathLayoutUpdated;

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000DA RID: 218 RVA: 0x0000498D File Offset: 0x00002B8D
		// (set) Token: 0x060000DB RID: 219 RVA: 0x0000499F File Offset: 0x00002B9F
		public int LayoutPathIndex
		{
			get
			{
				return (int)base.GetValue(PathListBoxItem.LayoutPathIndexProperty);
			}
			internal set
			{
				base.SetValue(PathListBoxItem.LayoutPathIndexProperty, value);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000049B2 File Offset: 0x00002BB2
		// (set) Token: 0x060000DD RID: 221 RVA: 0x000049C4 File Offset: 0x00002BC4
		public int GlobalIndex
		{
			get
			{
				return (int)base.GetValue(PathListBoxItem.GlobalIndexProperty);
			}
			internal set
			{
				base.SetValue(PathListBoxItem.GlobalIndexProperty, value);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000049D7 File Offset: 0x00002BD7
		// (set) Token: 0x060000DF RID: 223 RVA: 0x000049E9 File Offset: 0x00002BE9
		public int LocalIndex
		{
			get
			{
				return (int)base.GetValue(PathListBoxItem.LocalIndexProperty);
			}
			internal set
			{
				base.SetValue(PathListBoxItem.LocalIndexProperty, value);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000049FC File Offset: 0x00002BFC
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00004A0E File Offset: 0x00002C0E
		public double GlobalOffset
		{
			get
			{
				return (double)base.GetValue(PathListBoxItem.GlobalOffsetProperty);
			}
			internal set
			{
				base.SetValue(PathListBoxItem.GlobalOffsetProperty, value);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00004A21 File Offset: 0x00002C21
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00004A33 File Offset: 0x00002C33
		public double LocalOffset
		{
			get
			{
				return (double)base.GetValue(PathListBoxItem.LocalOffsetProperty);
			}
			internal set
			{
				base.SetValue(PathListBoxItem.LocalOffsetProperty, value);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00004A46 File Offset: 0x00002C46
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00004A58 File Offset: 0x00002C58
		public double NormalAngle
		{
			get
			{
				return (double)base.GetValue(PathListBoxItem.NormalAngleProperty);
			}
			internal set
			{
				base.SetValue(PathListBoxItem.NormalAngleProperty, value);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00004A6B File Offset: 0x00002C6B
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00004A7D File Offset: 0x00002C7D
		public double OrientationAngle
		{
			get
			{
				return (double)base.GetValue(PathListBoxItem.OrientationAngleProperty);
			}
			internal set
			{
				base.SetValue(PathListBoxItem.OrientationAngleProperty, value);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00004A90 File Offset: 0x00002C90
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00004AA2 File Offset: 0x00002CA2
		public bool IsArranged
		{
			get
			{
				return (bool)base.GetValue(PathListBoxItem.IsArrangedProperty);
			}
			internal set
			{
				base.SetValue(PathListBoxItem.IsArrangedProperty, value);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004AB5 File Offset: 0x00002CB5
		public PathListBoxItem()
		{
			base.DefaultStyleKey = typeof(PathListBoxItem);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004AD0 File Offset: 0x00002CD0
		public void Update(PathLayoutData data)
		{
			ChangedPathLayoutProperties changedPathLayoutProperties = ChangedPathLayoutProperties.None;
			if (this.LayoutPathIndex != data.LayoutPathIndex)
			{
				changedPathLayoutProperties |= ChangedPathLayoutProperties.LayoutPathIndex;
				this.LayoutPathIndex = data.LayoutPathIndex;
			}
			if (this.GlobalIndex != data.GlobalIndex)
			{
				changedPathLayoutProperties |= ChangedPathLayoutProperties.GlobalIndex;
				this.GlobalIndex = data.GlobalIndex;
			}
			if (this.LocalIndex != data.LocalIndex)
			{
				changedPathLayoutProperties |= ChangedPathLayoutProperties.LocalIndex;
				this.LocalIndex = data.LocalIndex;
			}
			if (this.GlobalOffset != data.GlobalOffset)
			{
				changedPathLayoutProperties |= ChangedPathLayoutProperties.GlobalOffset;
				this.GlobalOffset = data.GlobalOffset;
			}
			if (this.LocalOffset != data.LocalOffset)
			{
				changedPathLayoutProperties |= ChangedPathLayoutProperties.LocalOffset;
				this.LocalOffset = data.LocalOffset;
			}
			if (this.NormalAngle != data.NormalAngle)
			{
				changedPathLayoutProperties |= ChangedPathLayoutProperties.NormalAngle;
				this.NormalAngle = data.NormalAngle;
			}
			if (this.OrientationAngle != data.OrientationAngle)
			{
				changedPathLayoutProperties |= ChangedPathLayoutProperties.OrientationAngle;
				this.OrientationAngle = data.OrientationAngle;
			}
			if (this.IsArranged != data.IsArranged)
			{
				changedPathLayoutProperties |= ChangedPathLayoutProperties.IsArranged;
				this.IsArranged = data.IsArranged;
			}
			this.OnPathLayoutUpdated(new PathLayoutUpdatedEventArgs(changedPathLayoutProperties));
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004BE2 File Offset: 0x00002DE2
		internal void OnPathLayoutUpdated(PathLayoutUpdatedEventArgs pathLayoutArgs)
		{
			if (this.PathLayoutUpdated != null)
			{
				this.PathLayoutUpdated.Invoke(this, pathLayoutArgs);
			}
		}

		// Token: 0x0400005D RID: 93
		public static readonly DependencyProperty LayoutPathIndexProperty = DependencyProperty.Register("LayoutPathIndex", typeof(int), typeof(PathListBoxItem), new PropertyMetadata(0));

		// Token: 0x0400005E RID: 94
		public static readonly DependencyProperty GlobalIndexProperty = DependencyProperty.Register("GlobalIndex", typeof(int), typeof(PathListBoxItem), new PropertyMetadata(0));

		// Token: 0x0400005F RID: 95
		public static readonly DependencyProperty LocalIndexProperty = DependencyProperty.Register("LocalIndex", typeof(int), typeof(PathListBoxItem), new PropertyMetadata(0));

		// Token: 0x04000060 RID: 96
		public static readonly DependencyProperty GlobalOffsetProperty = DependencyProperty.Register("GlobalOffset", typeof(double), typeof(PathListBoxItem), new PropertyMetadata(0.0));

		// Token: 0x04000061 RID: 97
		public static readonly DependencyProperty LocalOffsetProperty = DependencyProperty.Register("LocalOffset", typeof(double), typeof(PathListBoxItem), new PropertyMetadata(0.0));

		// Token: 0x04000062 RID: 98
		public static readonly DependencyProperty NormalAngleProperty = DependencyProperty.Register("NormalAngle", typeof(double), typeof(PathListBoxItem), new PropertyMetadata(0.0));

		// Token: 0x04000063 RID: 99
		public static readonly DependencyProperty OrientationAngleProperty = DependencyProperty.Register("OrientationAngle", typeof(double), typeof(PathListBoxItem), new PropertyMetadata(0.0));

		// Token: 0x04000064 RID: 100
		public static readonly DependencyProperty IsArrangedProperty = DependencyProperty.Register("IsArranged", typeof(bool), typeof(PathListBoxItem), new PropertyMetadata(false));
	}
}
