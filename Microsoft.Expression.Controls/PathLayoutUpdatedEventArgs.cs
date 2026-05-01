using System;

namespace Microsoft.Expression.Controls
{
	// Token: 0x0200000F RID: 15
	public sealed class PathLayoutUpdatedEventArgs : EventArgs
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003853 File Offset: 0x00001A53
		// (set) Token: 0x06000074 RID: 116 RVA: 0x0000385B File Offset: 0x00001A5B
		public ChangedPathLayoutProperties ChangedProperties { get; private set; }

		// Token: 0x06000075 RID: 117 RVA: 0x00003864 File Offset: 0x00001A64
		public PathLayoutUpdatedEventArgs(ChangedPathLayoutProperties changedProperties)
		{
			this.ChangedProperties = changedProperties;
		}
	}
}
