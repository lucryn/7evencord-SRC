using System;

namespace Microsoft.Expression.Media
{
	// Token: 0x0200001A RID: 26
	internal class DrawingPropertyChangedEventArgs : EventArgs
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00007A45 File Offset: 0x00005C45
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00007A4D File Offset: 0x00005C4D
		public DrawingPropertyMetadata Metadata { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00007A56 File Offset: 0x00005C56
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00007A5E File Offset: 0x00005C5E
		public bool IsAnimated { get; set; }
	}
}
