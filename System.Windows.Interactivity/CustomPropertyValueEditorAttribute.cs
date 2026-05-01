using System;

namespace System.Windows.Interactivity
{
	// Token: 0x02000008 RID: 8
	[AttributeUsage(128, AllowMultiple = false, Inherited = true)]
	public sealed class CustomPropertyValueEditorAttribute : Attribute
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000026FA File Offset: 0x000008FA
		// (set) Token: 0x06000023 RID: 35 RVA: 0x00002702 File Offset: 0x00000902
		public CustomPropertyValueEditor CustomPropertyValueEditor { get; private set; }

		// Token: 0x06000024 RID: 36 RVA: 0x0000270B File Offset: 0x0000090B
		public CustomPropertyValueEditorAttribute(CustomPropertyValueEditor customPropertyValueEditor)
		{
			this.CustomPropertyValueEditor = customPropertyValueEditor;
		}
	}
}
