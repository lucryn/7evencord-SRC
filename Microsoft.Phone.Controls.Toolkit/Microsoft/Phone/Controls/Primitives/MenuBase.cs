using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x02000042 RID: 66
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(MenuItem))]
	public abstract class MenuBase : ItemsControl
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000986B File Offset: 0x00007A6B
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000987D File Offset: 0x00007A7D
		public Style ItemContainerStyle
		{
			get
			{
				return (Style)base.GetValue(MenuBase.ItemContainerStyleProperty);
			}
			set
			{
				base.SetValue(MenuBase.ItemContainerStyleProperty, value);
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00009893 File Offset: 0x00007A93
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is MenuItem || item is Separator;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x000098A8 File Offset: 0x00007AA8
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new MenuItem();
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000098B0 File Offset: 0x00007AB0
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			MenuItem menuItem = element as MenuItem;
			if (menuItem != null)
			{
				menuItem.ParentMenuBase = this;
				if (menuItem != item)
				{
					DataTemplate itemTemplate = base.ItemTemplate;
					Style itemContainerStyle = this.ItemContainerStyle;
					if (itemTemplate != null)
					{
						menuItem.SetValue(ItemsControl.ItemTemplateProperty, itemTemplate);
					}
					if (itemContainerStyle != null && MenuBase.HasDefaultValue(menuItem, HeaderedItemsControl.ItemContainerStyleProperty))
					{
						menuItem.SetValue(HeaderedItemsControl.ItemContainerStyleProperty, itemContainerStyle);
					}
					if (MenuBase.HasDefaultValue(menuItem, HeaderedItemsControl.HeaderProperty))
					{
						menuItem.Header = item;
					}
					if (itemTemplate != null)
					{
						menuItem.SetValue(HeaderedItemsControl.HeaderTemplateProperty, itemTemplate);
					}
					if (itemContainerStyle != null)
					{
						menuItem.SetValue(FrameworkElement.StyleProperty, itemContainerStyle);
					}
				}
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00009945 File Offset: 0x00007B45
		private static bool HasDefaultValue(Control control, DependencyProperty property)
		{
			return control.ReadLocalValue(property) == DependencyProperty.UnsetValue;
		}

		// Token: 0x040000CF RID: 207
		public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(MenuBase), null);
	}
}
