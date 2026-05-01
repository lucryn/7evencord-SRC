using System;
using System.Windows.Data;

namespace System.Windows.Controls
{
	// Token: 0x02000004 RID: 4
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ContentPresenter))]
	public class HeaderedItemsControl : ItemsControl
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002281 File Offset: 0x00000481
		// (set) Token: 0x0600000E RID: 14 RVA: 0x00002289 File Offset: 0x00000489
		internal bool HeaderIsItem { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002292 File Offset: 0x00000492
		// (set) Token: 0x06000010 RID: 16 RVA: 0x0000229F File Offset: 0x0000049F
		public object Header
		{
			get
			{
				return base.GetValue(HeaderedItemsControl.HeaderProperty);
			}
			set
			{
				base.SetValue(HeaderedItemsControl.HeaderProperty, value);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000022B0 File Offset: 0x000004B0
		private static void OnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderedItemsControl headeredItemsControl = d as HeaderedItemsControl;
			headeredItemsControl.OnHeaderChanged(e.OldValue, e.NewValue);
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000022D8 File Offset: 0x000004D8
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000022EA File Offset: 0x000004EA
		public DataTemplate HeaderTemplate
		{
			get
			{
				return base.GetValue(HeaderedItemsControl.HeaderTemplateProperty) as DataTemplate;
			}
			set
			{
				base.SetValue(HeaderedItemsControl.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022F8 File Offset: 0x000004F8
		private static void OnHeaderTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderedItemsControl headeredItemsControl = d as HeaderedItemsControl;
			DataTemplate oldHeaderTemplate = e.OldValue as DataTemplate;
			DataTemplate newHeaderTemplate = e.NewValue as DataTemplate;
			headeredItemsControl.OnHeaderTemplateChanged(oldHeaderTemplate, newHeaderTemplate);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000232E File Offset: 0x0000052E
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002340 File Offset: 0x00000540
		public Style ItemContainerStyle
		{
			get
			{
				return base.GetValue(HeaderedItemsControl.ItemContainerStyleProperty) as Style;
			}
			set
			{
				base.SetValue(HeaderedItemsControl.ItemContainerStyleProperty, value);
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002350 File Offset: 0x00000550
		private static void OnItemContainerStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderedItemsControl headeredItemsControl = d as HeaderedItemsControl;
			Style itemContainerStyle = e.NewValue as Style;
			headeredItemsControl.ItemsControlHelper.UpdateItemContainerStyle(itemContainerStyle);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000237D File Offset: 0x0000057D
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002385 File Offset: 0x00000585
		internal ItemsControlHelper ItemsControlHelper { get; private set; }

		// Token: 0x0600001A RID: 26 RVA: 0x0000238E File Offset: 0x0000058E
		public HeaderedItemsControl()
		{
			base.DefaultStyleKey = typeof(HeaderedItemsControl);
			this.ItemsControlHelper = new ItemsControlHelper(this);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000023B2 File Offset: 0x000005B2
		protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000023B4 File Offset: 0x000005B4
		protected virtual void OnHeaderTemplateChanged(DataTemplate oldHeaderTemplate, DataTemplate newHeaderTemplate)
		{
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000023B6 File Offset: 0x000005B6
		public override void OnApplyTemplate()
		{
			this.ItemsControlHelper.OnApplyTemplate();
			base.OnApplyTemplate();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000023C9 File Offset: 0x000005C9
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			ItemsControlHelper.PrepareContainerForItemOverride(element, this.ItemContainerStyle);
			HeaderedItemsControl.PreparePrepareHeaderedItemsControlContainerForItemOverride(element, item, this, this.ItemContainerStyle);
			base.PrepareContainerForItemOverride(element, item);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000023F0 File Offset: 0x000005F0
		internal static void PreparePrepareHeaderedItemsControlContainerForItemOverride(DependencyObject element, object item, ItemsControl parent, Style parentItemContainerStyle)
		{
			HeaderedItemsControl headeredItemsControl = element as HeaderedItemsControl;
			if (headeredItemsControl != null)
			{
				HeaderedItemsControl.PrepareHeaderedItemsControlContainer(headeredItemsControl, item, parent, parentItemContainerStyle);
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002410 File Offset: 0x00000610
		private static void PrepareHeaderedItemsControlContainer(HeaderedItemsControl control, object item, ItemsControl parentItemsControl, Style parentItemContainerStyle)
		{
			if (control != item)
			{
				DataTemplate itemTemplate = parentItemsControl.ItemTemplate;
				if (itemTemplate != null)
				{
					control.SetValue(ItemsControl.ItemTemplateProperty, itemTemplate);
				}
				if (parentItemContainerStyle != null && HeaderedItemsControl.HasDefaultValue(control, HeaderedItemsControl.ItemContainerStyleProperty))
				{
					control.SetValue(HeaderedItemsControl.ItemContainerStyleProperty, parentItemContainerStyle);
				}
				if (control.HeaderIsItem || HeaderedItemsControl.HasDefaultValue(control, HeaderedItemsControl.HeaderProperty))
				{
					control.Header = item;
					control.HeaderIsItem = true;
				}
				if (itemTemplate != null)
				{
					control.SetValue(HeaderedItemsControl.HeaderTemplateProperty, itemTemplate);
				}
				if (parentItemContainerStyle != null && control.Style == null)
				{
					control.SetValue(FrameworkElement.StyleProperty, parentItemContainerStyle);
				}
				HierarchicalDataTemplate hierarchicalDataTemplate = itemTemplate as HierarchicalDataTemplate;
				if (hierarchicalDataTemplate != null)
				{
					if (hierarchicalDataTemplate.ItemsSource != null && HeaderedItemsControl.HasDefaultValue(control, ItemsControl.ItemsSourceProperty))
					{
						control.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
						{
							Converter = hierarchicalDataTemplate.ItemsSource.Converter,
							ConverterCulture = hierarchicalDataTemplate.ItemsSource.ConverterCulture,
							ConverterParameter = hierarchicalDataTemplate.ItemsSource.ConverterParameter,
							Mode = hierarchicalDataTemplate.ItemsSource.Mode,
							NotifyOnValidationError = hierarchicalDataTemplate.ItemsSource.NotifyOnValidationError,
							Path = hierarchicalDataTemplate.ItemsSource.Path,
							Source = control.Header,
							ValidatesOnExceptions = hierarchicalDataTemplate.ItemsSource.ValidatesOnExceptions
						});
					}
					if (hierarchicalDataTemplate.IsItemTemplateSet && control.ItemTemplate == itemTemplate)
					{
						control.ClearValue(ItemsControl.ItemTemplateProperty);
						if (hierarchicalDataTemplate.ItemTemplate != null)
						{
							control.ItemTemplate = hierarchicalDataTemplate.ItemTemplate;
						}
					}
					if (hierarchicalDataTemplate.IsItemContainerStyleSet && control.ItemContainerStyle == parentItemContainerStyle)
					{
						control.ClearValue(HeaderedItemsControl.ItemContainerStyleProperty);
						if (hierarchicalDataTemplate.ItemContainerStyle != null)
						{
							control.ItemContainerStyle = hierarchicalDataTemplate.ItemContainerStyle;
						}
					}
				}
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000025BD File Offset: 0x000007BD
		private static bool HasDefaultValue(Control control, DependencyProperty property)
		{
			return control.ReadLocalValue(property) == DependencyProperty.UnsetValue;
		}

		// Token: 0x0400000E RID: 14
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(HeaderedItemsControl), new PropertyMetadata(new PropertyChangedCallback(HeaderedItemsControl.OnHeaderPropertyChanged)));

		// Token: 0x0400000F RID: 15
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(HeaderedItemsControl), new PropertyMetadata(new PropertyChangedCallback(HeaderedItemsControl.OnHeaderTemplatePropertyChanged)));

		// Token: 0x04000010 RID: 16
		public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(HeaderedItemsControl), new PropertyMetadata(null, new PropertyChangedCallback(HeaderedItemsControl.OnItemContainerStylePropertyChanged)));
	}
}
