using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x02000017 RID: 23
	public class TemplatedItemsControl<T> : ItemsControl where T : FrameworkElement, new()
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003800 File Offset: 0x00002800
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00003812 File Offset: 0x00002812
		public Style ItemContainerStyle
		{
			get
			{
				return base.GetValue(TemplatedItemsControl<T>.ItemContainerStyleProperty) as Style;
			}
			set
			{
				base.SetValue(TemplatedItemsControl<T>.ItemContainerStyleProperty, value);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000383E File Offset: 0x0000283E
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is T;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000384C File Offset: 0x0000284C
		protected override DependencyObject GetContainerForItemOverride()
		{
			T t = Activator.CreateInstance<T>();
			this.ApplyItemContainerStyle(t);
			return t;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003871 File Offset: 0x00002871
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			this.ApplyItemContainerStyle(element);
			base.PrepareContainerForItemOverride(element, item);
			this._itemToContainer[item] = (T)((object)element);
			this._containerToItem[(T)((object)element)] = item;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000038A6 File Offset: 0x000028A6
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			this._itemToContainer.Remove(item);
			this._containerToItem.Remove((T)((object)element));
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000038D0 File Offset: 0x000028D0
		protected virtual void ApplyItemContainerStyle(DependencyObject container)
		{
			T t = container as T;
			if (t == null || t.ReadLocalValue(FrameworkElement.StyleProperty) != DependencyProperty.UnsetValue)
			{
				return;
			}
			Style itemContainerStyle = this.ItemContainerStyle;
			if (itemContainerStyle != null)
			{
				t.Style = itemContainerStyle;
				return;
			}
			t.ClearValue(FrameworkElement.StyleProperty);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003938 File Offset: 0x00002938
		protected object GetItem(T container)
		{
			object result = null;
			if (container != null)
			{
				this._containerToItem.TryGetValue(container, ref result);
			}
			return result;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003960 File Offset: 0x00002960
		protected T GetContainer(object item)
		{
			T result = default(T);
			if (item != null)
			{
				this._itemToContainer.TryGetValue(item, ref result);
			}
			return result;
		}

		// Token: 0x04000046 RID: 70
		private readonly Dictionary<object, T> _itemToContainer = new Dictionary<object, T>();

		// Token: 0x04000047 RID: 71
		private readonly Dictionary<T, object> _containerToItem = new Dictionary<T, object>();

		// Token: 0x04000048 RID: 72
		public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(TemplatedItemsControl<T>), null);
	}
}
