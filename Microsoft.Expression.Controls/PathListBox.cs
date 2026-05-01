using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Microsoft.Expression.Controls
{
	// Token: 0x0200001A RID: 26
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(PathListBoxItem))]
	public sealed class PathListBox : ListBox
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000045E3 File Offset: 0x000027E3
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x000045F5 File Offset: 0x000027F5
		private ItemsPanelTemplate ItemsPanelListener
		{
			get
			{
				return (ItemsPanelTemplate)base.GetValue(PathListBox.ItemsPanelListenerProperty);
			}
			set
			{
				base.SetValue(PathListBox.ItemsPanelListenerProperty, value);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004603 File Offset: 0x00002803
		public LayoutPathCollection LayoutPaths
		{
			get
			{
				return (LayoutPathCollection)base.GetValue(PathListBox.LayoutPathsProperty);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00004615 File Offset: 0x00002815
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00004627 File Offset: 0x00002827
		public double StartItemIndex
		{
			get
			{
				return (double)base.GetValue(PathListBox.StartItemIndexProperty);
			}
			set
			{
				base.SetValue(PathListBox.StartItemIndexProperty, value);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000CD RID: 205 RVA: 0x0000463A File Offset: 0x0000283A
		// (set) Token: 0x060000CE RID: 206 RVA: 0x0000464C File Offset: 0x0000284C
		public bool WrapItems
		{
			get
			{
				return (bool)base.GetValue(PathListBox.WrapItemsProperty);
			}
			set
			{
				base.SetValue(PathListBox.WrapItemsProperty, value);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004660 File Offset: 0x00002860
		public PathListBox()
		{
			base.DefaultStyleKey = typeof(PathListBox);
			LayoutPathCollection layoutPathCollection = new LayoutPathCollection();
			base.SetValue(PathListBox.LayoutPathsProperty, layoutPathCollection);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004695 File Offset: 0x00002895
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.itemsPanel = null;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000046A4 File Offset: 0x000028A4
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new PathListBoxItem();
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000046AB File Offset: 0x000028AB
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is PathListBoxItem;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000046B8 File Offset: 0x000028B8
		protected override Size ArrangeOverride(Size finalSize)
		{
			if (this.ItemsPanelListener == null)
			{
				base.SetBinding(PathListBox.ItemsPanelListenerProperty, new Binding("ItemsPanel")
				{
					Source = this,
					Mode = 1
				});
			}
			if (this.itemsPanel == null)
			{
				this.itemsPanel = (this.GetItemsHost() as PathPanel);
				if (this.itemsPanel != null)
				{
					this.RefreshSourceElementBindings();
					this.SetOneWayBinding(this.itemsPanel, PathPanel.LayoutPathsProperty, "LayoutPaths");
					this.SetOneWayBinding(this.itemsPanel, PathPanel.StartItemIndexProperty, "StartItemIndex");
					this.SetOneWayBinding(this.itemsPanel, PathPanel.WrapItemsProperty, "WrapItems");
				}
			}
			return base.ArrangeOverride(finalSize);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004764 File Offset: 0x00002964
		private void RefreshSourceElementBindings()
		{
			foreach (LayoutPath layoutPath in this.LayoutPaths)
			{
				if (layoutPath.SourceElement == null)
				{
					BindingExpression bindingExpression = layoutPath.ReadLocalValue(LayoutPath.SourceElementProperty) as BindingExpression;
					if (bindingExpression != null)
					{
						layoutPath.ClearValue(LayoutPath.SourceElementProperty);
						layoutPath.SetValue(LayoutPath.SourceElementProperty, bindingExpression);
					}
				}
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000047E0 File Offset: 0x000029E0
		private static void ItemsPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PathListBox pathListBox = d as PathListBox;
			if (pathListBox == null)
			{
				return;
			}
			if (e.Property == PathListBox.ItemsPanelListenerProperty && e.NewValue != e.OldValue)
			{
				pathListBox.itemsPanel = null;
			}
			pathListBox.InvalidateArrange();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004824 File Offset: 0x00002A24
		private void SetOneWayBinding(DependencyObject target, DependencyProperty targetProperty, string sourceProperty)
		{
			BindingOperations.SetBinding(target, targetProperty, new Binding(sourceProperty)
			{
				Source = this,
				Mode = 1
			});
		}

		// Token: 0x04000057 RID: 87
		private PathPanel itemsPanel;

		// Token: 0x04000058 RID: 88
		private static readonly DependencyProperty ItemsPanelListenerProperty = DependencyProperty.Register("ItemsPanelListener", typeof(ItemsPanelTemplate), typeof(PathListBox), new PropertyMetadata(new PropertyChangedCallback(PathListBox.ItemsPanelChanged)));

		// Token: 0x04000059 RID: 89
		public static readonly DependencyProperty LayoutPathsProperty = DependencyProperty.Register("LayoutPaths", typeof(LayoutPathCollection), typeof(PathListBox), new PropertyMetadata(null));

		// Token: 0x0400005A RID: 90
		public static readonly DependencyProperty StartItemIndexProperty = DependencyProperty.Register("StartItemIndex", typeof(double), typeof(PathListBox), new PropertyMetadata(0.0));

		// Token: 0x0400005B RID: 91
		public static readonly DependencyProperty WrapItemsProperty = DependencyProperty.Register("WrapItems", typeof(bool), typeof(PathListBox), new PropertyMetadata(false));
	}
}
