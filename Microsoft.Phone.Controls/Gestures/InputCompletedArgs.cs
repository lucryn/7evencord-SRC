using System;
using System.Windows;

namespace Microsoft.Phone.Gestures
{
	// Token: 0x0200000D RID: 13
	internal abstract class InputCompletedArgs : InputBaseArgs
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000045 RID: 69
		public abstract Point TotalTranslation { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000046 RID: 70
		public abstract Point FinalLinearVelocity { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000047 RID: 71
		public abstract bool IsInertial { get; }

		// Token: 0x06000048 RID: 72 RVA: 0x00002982 File Offset: 0x00001982
		protected InputCompletedArgs(UIElement source, Point origin) : base(source, origin)
		{
		}
	}
}
