using System;
using System.Windows;

namespace Microsoft.Phone.Gestures
{
	// Token: 0x02000009 RID: 9
	internal class DragEventArgs : GestureEventArgs
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002388 File Offset: 0x00001388
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002390 File Offset: 0x00001390
		public bool IsTouchComplete { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002399 File Offset: 0x00001399
		// (set) Token: 0x0600001C RID: 28 RVA: 0x000023A1 File Offset: 0x000013A1
		public Point DeltaDistance { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000023AA File Offset: 0x000013AA
		// (set) Token: 0x0600001E RID: 30 RVA: 0x000023B2 File Offset: 0x000013B2
		public Point CumulativeDistance { get; internal set; }

		// Token: 0x0600001F RID: 31 RVA: 0x000023BB File Offset: 0x000013BB
		public DragEventArgs()
		{
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000023C3 File Offset: 0x000013C3
		public DragEventArgs(InputDeltaArgs args)
		{
			if (args != null)
			{
				this.CumulativeDistance = args.CumulativeTranslation;
				this.DeltaDistance = args.DeltaTranslation;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000023E6 File Offset: 0x000013E6
		public void MarkAsFinalTouchManipulation()
		{
			this.IsTouchComplete = true;
		}
	}
}
