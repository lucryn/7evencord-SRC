using System;
using System.Reflection;

namespace System.Windows.Interactivity
{
	// Token: 0x0200000B RID: 11
	public sealed class EventObserver : IDisposable
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002900 File Offset: 0x00000B00
		public EventObserver(EventInfo eventInfo, object target, Delegate handler)
		{
			if (eventInfo == null)
			{
				throw new ArgumentNullException("eventInfo");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this.eventInfo = eventInfo;
			this.target = target;
			this.handler = handler;
			this.eventInfo.AddEventHandler(this.target, handler);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002956 File Offset: 0x00000B56
		public void Dispose()
		{
			this.eventInfo.RemoveEventHandler(this.target, this.handler);
		}

		// Token: 0x04000010 RID: 16
		private EventInfo eventInfo;

		// Token: 0x04000011 RID: 17
		private object target;

		// Token: 0x04000012 RID: 18
		private Delegate handler;
	}
}
