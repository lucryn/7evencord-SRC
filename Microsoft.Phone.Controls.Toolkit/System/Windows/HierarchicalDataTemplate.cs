using System;
using System.Windows.Data;

namespace System.Windows
{
	// Token: 0x0200004F RID: 79
	public class HierarchicalDataTemplate : DataTemplate
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000DCF2 File Offset: 0x0000BEF2
		// (set) Token: 0x06000300 RID: 768 RVA: 0x0000DCFA File Offset: 0x0000BEFA
		public Binding ItemsSource { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000DD03 File Offset: 0x0000BF03
		// (set) Token: 0x06000302 RID: 770 RVA: 0x0000DD0B File Offset: 0x0000BF0B
		internal bool IsItemTemplateSet { get; private set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000DD14 File Offset: 0x0000BF14
		// (set) Token: 0x06000304 RID: 772 RVA: 0x0000DD1C File Offset: 0x0000BF1C
		public DataTemplate ItemTemplate
		{
			get
			{
				return this._itemTemplate;
			}
			set
			{
				this.IsItemTemplateSet = true;
				this._itemTemplate = value;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000DD2C File Offset: 0x0000BF2C
		// (set) Token: 0x06000306 RID: 774 RVA: 0x0000DD34 File Offset: 0x0000BF34
		internal bool IsItemContainerStyleSet { get; private set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000DD3D File Offset: 0x0000BF3D
		// (set) Token: 0x06000308 RID: 776 RVA: 0x0000DD45 File Offset: 0x0000BF45
		public Style ItemContainerStyle
		{
			get
			{
				return this._itemContainerStyle;
			}
			set
			{
				this.IsItemContainerStyleSet = true;
				this._itemContainerStyle = value;
			}
		}

		// Token: 0x04000164 RID: 356
		private DataTemplate _itemTemplate;

		// Token: 0x04000165 RID: 357
		private Style _itemContainerStyle;
	}
}
