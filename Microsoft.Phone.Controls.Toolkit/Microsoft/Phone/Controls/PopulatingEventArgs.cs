using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200002B RID: 43
	public class PopulatingEventArgs : RoutedEventArgs
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00006F54 File Offset: 0x00005154
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00006F5C File Offset: 0x0000515C
		public string Parameter { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00006F65 File Offset: 0x00005165
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00006F6D File Offset: 0x0000516D
		public bool Cancel { get; set; }

		// Token: 0x0600015F RID: 351 RVA: 0x00006F76 File Offset: 0x00005176
		public PopulatingEventArgs(string parameter)
		{
			this.Parameter = parameter;
		}
	}
}
