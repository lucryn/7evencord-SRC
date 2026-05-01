using System;
using System.Collections;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000073 RID: 115
	public class PopulatedEventArgs : RoutedEventArgs
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x00014317 File Offset: 0x00012517
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x0001431F File Offset: 0x0001251F
		public IEnumerable Data { get; private set; }

		// Token: 0x0600049B RID: 1179 RVA: 0x00014328 File Offset: 0x00012528
		public PopulatedEventArgs(IEnumerable data)
		{
			this.Data = data;
		}
	}
}
