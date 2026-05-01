using System;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000019 RID: 25
	public interface ITransition
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000B8 RID: 184
		// (remove) Token: 0x060000B9 RID: 185
		event EventHandler Completed;

		// Token: 0x060000BA RID: 186
		ClockState GetCurrentState();

		// Token: 0x060000BB RID: 187
		TimeSpan GetCurrentTime();

		// Token: 0x060000BC RID: 188
		void Pause();

		// Token: 0x060000BD RID: 189
		void Resume();

		// Token: 0x060000BE RID: 190
		void Seek(TimeSpan offset);

		// Token: 0x060000BF RID: 191
		void SeekAlignedToLastTick(TimeSpan offset);

		// Token: 0x060000C0 RID: 192
		void SkipToFill();

		// Token: 0x060000C1 RID: 193
		void Begin();

		// Token: 0x060000C2 RID: 194
		void Stop();
	}
}
