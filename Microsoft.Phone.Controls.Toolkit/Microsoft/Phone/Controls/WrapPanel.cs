using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls.Properties;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000062 RID: 98
	public class WrapPanel : Panel
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0000F062 File Offset: 0x0000D262
		// (set) Token: 0x0600036C RID: 876 RVA: 0x0000F074 File Offset: 0x0000D274
		[TypeConverter(typeof(LengthConverter))]
		public double ItemHeight
		{
			get
			{
				return (double)base.GetValue(WrapPanel.ItemHeightProperty);
			}
			set
			{
				base.SetValue(WrapPanel.ItemHeightProperty, value);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0000F087 File Offset: 0x0000D287
		// (set) Token: 0x0600036E RID: 878 RVA: 0x0000F099 File Offset: 0x0000D299
		[TypeConverter(typeof(LengthConverter))]
		public double ItemWidth
		{
			get
			{
				return (double)base.GetValue(WrapPanel.ItemWidthProperty);
			}
			set
			{
				base.SetValue(WrapPanel.ItemWidthProperty, value);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0000F0AC File Offset: 0x0000D2AC
		// (set) Token: 0x06000370 RID: 880 RVA: 0x0000F0BE File Offset: 0x0000D2BE
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(WrapPanel.OrientationProperty);
			}
			set
			{
				base.SetValue(WrapPanel.OrientationProperty, value);
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000F0D4 File Offset: 0x0000D2D4
		private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			WrapPanel wrapPanel = (WrapPanel)d;
			Orientation orientation = (Orientation)e.NewValue;
			if (wrapPanel._ignorePropertyChange)
			{
				wrapPanel._ignorePropertyChange = false;
				return;
			}
			if (orientation != 1 && orientation != null)
			{
				wrapPanel._ignorePropertyChange = true;
				wrapPanel.SetValue(WrapPanel.OrientationProperty, (Orientation)e.OldValue);
				string text = string.Format(CultureInfo.InvariantCulture, Resources.WrapPanel_OnOrientationPropertyChanged_InvalidValue, new object[]
				{
					orientation
				});
				throw new ArgumentException(text, "value");
			}
			wrapPanel.InvalidateMeasure();
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000F16C File Offset: 0x0000D36C
		private static void OnItemHeightOrWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			WrapPanel wrapPanel = (WrapPanel)d;
			double num = (double)e.NewValue;
			if (wrapPanel._ignorePropertyChange)
			{
				wrapPanel._ignorePropertyChange = false;
				return;
			}
			if (!num.IsNaN() && (num <= 0.0 || double.IsPositiveInfinity(num)))
			{
				wrapPanel._ignorePropertyChange = true;
				wrapPanel.SetValue(e.Property, (double)e.OldValue);
				string text = string.Format(CultureInfo.InvariantCulture, Resources.WrapPanel_OnItemHeightOrWidthPropertyChanged_InvalidValue, new object[]
				{
					num
				});
				throw new ArgumentException(text, "value");
			}
			wrapPanel.InvalidateMeasure();
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000F214 File Offset: 0x0000D414
		protected override Size MeasureOverride(Size constraint)
		{
			Orientation orientation = this.Orientation;
			OrientedSize orientedSize = new OrientedSize(orientation);
			OrientedSize orientedSize2 = new OrientedSize(orientation);
			OrientedSize orientedSize3 = new OrientedSize(orientation, constraint.Width, constraint.Height);
			double itemWidth = this.ItemWidth;
			double itemHeight = this.ItemHeight;
			bool flag = !itemWidth.IsNaN();
			bool flag2 = !itemHeight.IsNaN();
			Size size;
			size..ctor(flag ? itemWidth : constraint.Width, flag2 ? itemHeight : constraint.Height);
			foreach (UIElement uielement in base.Children)
			{
				uielement.Measure(size);
				OrientedSize orientedSize4 = new OrientedSize(orientation, flag ? itemWidth : uielement.DesiredSize.Width, flag2 ? itemHeight : uielement.DesiredSize.Height);
				if (NumericExtensions.IsGreaterThan(orientedSize.Direct + orientedSize4.Direct, orientedSize3.Direct))
				{
					orientedSize2.Direct = Math.Max(orientedSize.Direct, orientedSize2.Direct);
					orientedSize2.Indirect += orientedSize.Indirect;
					orientedSize = orientedSize4;
					if (NumericExtensions.IsGreaterThan(orientedSize4.Direct, orientedSize3.Direct))
					{
						orientedSize2.Direct = Math.Max(orientedSize4.Direct, orientedSize2.Direct);
						orientedSize2.Indirect += orientedSize4.Indirect;
						orientedSize = new OrientedSize(orientation);
					}
				}
				else
				{
					orientedSize.Direct += orientedSize4.Direct;
					orientedSize.Indirect = Math.Max(orientedSize.Indirect, orientedSize4.Indirect);
				}
			}
			orientedSize2.Direct = Math.Max(orientedSize.Direct, orientedSize2.Direct);
			orientedSize2.Indirect += orientedSize.Indirect;
			return new Size(orientedSize2.Width, orientedSize2.Height);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000F43C File Offset: 0x0000D63C
		protected override Size ArrangeOverride(Size finalSize)
		{
			Orientation orientation = this.Orientation;
			OrientedSize orientedSize = new OrientedSize(orientation);
			OrientedSize orientedSize2 = new OrientedSize(orientation, finalSize.Width, finalSize.Height);
			double itemWidth = this.ItemWidth;
			double itemHeight = this.ItemHeight;
			bool flag = !itemWidth.IsNaN();
			bool flag2 = !itemHeight.IsNaN();
			double num = 0.0;
			double? directDelta = (orientation == 1) ? (flag ? new double?(itemWidth) : default(double?)) : (flag2 ? new double?(itemHeight) : default(double?));
			UIElementCollection children = base.Children;
			int count = children.Count;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				UIElement uielement = children[i];
				OrientedSize orientedSize3 = new OrientedSize(orientation, flag ? itemWidth : uielement.DesiredSize.Width, flag2 ? itemHeight : uielement.DesiredSize.Height);
				if (NumericExtensions.IsGreaterThan(orientedSize.Direct + orientedSize3.Direct, orientedSize2.Direct))
				{
					this.ArrangeLine(num2, i, directDelta, num, orientedSize.Indirect);
					num += orientedSize.Indirect;
					orientedSize = orientedSize3;
					if (NumericExtensions.IsGreaterThan(orientedSize3.Direct, orientedSize2.Direct))
					{
						this.ArrangeLine(i, ++i, directDelta, num, orientedSize3.Indirect);
						num += orientedSize.Indirect;
						orientedSize = new OrientedSize(orientation);
					}
					num2 = i;
				}
				else
				{
					orientedSize.Direct += orientedSize3.Direct;
					orientedSize.Indirect = Math.Max(orientedSize.Indirect, orientedSize3.Indirect);
				}
			}
			if (num2 < count)
			{
				this.ArrangeLine(num2, count, directDelta, num, orientedSize.Indirect);
			}
			return finalSize;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000F618 File Offset: 0x0000D818
		private void ArrangeLine(int lineStart, int lineEnd, double? directDelta, double indirectOffset, double indirectGrowth)
		{
			double num = 0.0;
			Orientation orientation = this.Orientation;
			bool flag = orientation == 1;
			UIElementCollection children = base.Children;
			for (int i = lineStart; i < lineEnd; i++)
			{
				UIElement uielement = children[i];
				OrientedSize orientedSize = new OrientedSize(orientation, uielement.DesiredSize.Width, uielement.DesiredSize.Height);
				double num2 = (directDelta != null) ? directDelta.Value : orientedSize.Direct;
				Rect rect = flag ? new Rect(num, indirectOffset, num2, indirectGrowth) : new Rect(indirectOffset, num, indirectGrowth, num2);
				uielement.Arrange(rect);
				num += num2;
			}
		}

		// Token: 0x040001CD RID: 461
		private bool _ignorePropertyChange;

		// Token: 0x040001CE RID: 462
		public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(WrapPanel), new PropertyMetadata(double.NaN, new PropertyChangedCallback(WrapPanel.OnItemHeightOrWidthPropertyChanged)));

		// Token: 0x040001CF RID: 463
		public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(WrapPanel), new PropertyMetadata(double.NaN, new PropertyChangedCallback(WrapPanel.OnItemHeightOrWidthPropertyChanged)));

		// Token: 0x040001D0 RID: 464
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(WrapPanel), new PropertyMetadata(1, new PropertyChangedCallback(WrapPanel.OnOrientationPropertyChanged)));
	}
}
