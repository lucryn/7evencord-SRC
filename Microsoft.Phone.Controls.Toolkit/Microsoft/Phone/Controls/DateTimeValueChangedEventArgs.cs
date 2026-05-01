using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000060 RID: 96
	public class DateTimeValueChangedEventArgs : EventArgs
	{
		// Token: 0x06000362 RID: 866 RVA: 0x0000F02A File Offset: 0x0000D22A
		public DateTimeValueChangedEventArgs(DateTime? oldDateTime, DateTime? newDateTime)
		{
			this.OldDateTime = oldDateTime;
			this.NewDateTime = newDateTime;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000F040 File Offset: 0x0000D240
		// (set) Token: 0x06000364 RID: 868 RVA: 0x0000F048 File Offset: 0x0000D248
		public DateTime? OldDateTime { get; private set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0000F051 File Offset: 0x0000D251
		// (set) Token: 0x06000366 RID: 870 RVA: 0x0000F059 File Offset: 0x0000D259
		public DateTime? NewDateTime { get; private set; }
	}
}
