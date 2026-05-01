using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000003 RID: 3
	internal static class SafeRaise
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000022A3 File Offset: 0x000012A3
		public static void Raise(EventHandler<EventArgs> eventToRaise, object sender)
		{
			SafeRaise.Raise<EventArgs>(eventToRaise, sender, EventArgs.Empty);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022B1 File Offset: 0x000012B1
		public static void Raise<T>(EventHandler<T> eventToRaise, object sender, T args) where T : EventArgs
		{
			if (eventToRaise != null)
			{
				eventToRaise.Invoke(sender, args);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000022BE File Offset: 0x000012BE
		public static void Raise<T>(EventHandler<T> eventToRaise, object sender, SafeRaise.GetEventArgs<T> getEventArgs) where T : EventArgs
		{
			if (eventToRaise != null)
			{
				eventToRaise.Invoke(sender, getEventArgs());
			}
		}

		// Token: 0x02000004 RID: 4
		// (Invoke) Token: 0x0600000C RID: 12
		public delegate T GetEventArgs<T>() where T : EventArgs;
	}
}
