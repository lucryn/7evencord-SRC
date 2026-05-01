using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000064 RID: 100
	internal static class SafeRaise
	{
		// Token: 0x06000379 RID: 889 RVA: 0x0000F7DB File Offset: 0x0000D9DB
		public static void Raise(EventHandler eventToRaise, object sender)
		{
			if (eventToRaise != null)
			{
				eventToRaise.Invoke(sender, EventArgs.Empty);
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000F7EC File Offset: 0x0000D9EC
		public static void Raise(EventHandler<EventArgs> eventToRaise, object sender)
		{
			SafeRaise.Raise<EventArgs>(eventToRaise, sender, EventArgs.Empty);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000F7FA File Offset: 0x0000D9FA
		public static void Raise<T>(EventHandler<T> eventToRaise, object sender, T args) where T : EventArgs
		{
			if (eventToRaise != null)
			{
				eventToRaise.Invoke(sender, args);
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000F807 File Offset: 0x0000DA07
		public static void Raise<T>(EventHandler<T> eventToRaise, object sender, SafeRaise.GetEventArgs<T> getEventArgs) where T : EventArgs
		{
			if (eventToRaise != null)
			{
				eventToRaise.Invoke(sender, getEventArgs());
			}
		}

		// Token: 0x02000065 RID: 101
		// (Invoke) Token: 0x0600037E RID: 894
		public delegate T GetEventArgs<T>() where T : EventArgs;
	}
}
