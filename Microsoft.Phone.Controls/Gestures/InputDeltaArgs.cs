using System;
using System.Windows;

namespace Microsoft.Phone.Gestures
{
	// Token: 0x0200000E RID: 14
	internal abstract class InputDeltaArgs : InputBaseArgs
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000049 RID: 73
		public abstract Point DeltaTranslation { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600004A RID: 74
		public abstract Point CumulativeTranslation { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004B RID: 75
		public abstract Point ExpansionVelocity { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004C RID: 76
		public abstract Point LinearVelocity { get; }

		// Token: 0x0600004D RID: 77 RVA: 0x0000298C File Offset: 0x0000198C
		protected InputDeltaArgs(UIElement source, Point origin) : base(source, origin)
		{
		}
	}
}
