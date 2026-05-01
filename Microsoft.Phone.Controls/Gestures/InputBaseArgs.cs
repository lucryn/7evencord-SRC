using System;
using System.Windows;

namespace Microsoft.Phone.Gestures
{
	// Token: 0x0200000C RID: 12
	internal class InputBaseArgs
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000040 RID: 64 RVA: 0x0000294A File Offset: 0x0000194A
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002952 File Offset: 0x00001952
		public UIElement Source { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000295B File Offset: 0x0000195B
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002963 File Offset: 0x00001963
		public Point Origin { get; private set; }

		// Token: 0x06000044 RID: 68 RVA: 0x0000296C File Offset: 0x0000196C
		protected InputBaseArgs(UIElement source, Point origin)
		{
			this.Source = source;
			this.Origin = origin;
		}
	}
}
