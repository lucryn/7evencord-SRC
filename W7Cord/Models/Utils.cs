using System;
using System.Windows;

namespace W7Cord.Models
{
	// Token: 0x02000004 RID: 4
	public static class Utils
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00002DDE File Offset: 0x00000FDE
		public static void SetImmediate(Action action)
		{
			Deployment.Current.Dispatcher.BeginInvoke(action);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002DF4 File Offset: 0x00000FF4
		public static string Truncate(string text, int maxLength)
		{
			string result;
			if (string.IsNullOrEmpty(text))
			{
				result = "";
			}
			else if (text.Length <= maxLength)
			{
				result = text;
			}
			else
			{
				result = text.Substring(0, maxLength - 3) + "...";
			}
			return result;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002E40 File Offset: 0x00001040
		public static string FormatTimestamp(string timestamp)
		{
			string result;
			if (string.IsNullOrEmpty(timestamp))
			{
				result = "";
			}
			else
			{
				try
				{
					DateTime dateTime = DateTime.Parse(timestamp);
					DateTime now = DateTime.Now;
					if (dateTime.Date == now.Date)
					{
						result = dateTime.ToString("h:mm tt");
					}
					else if (dateTime.Year == now.Year)
					{
						result = dateTime.ToString("MMM d");
					}
					else
					{
						result = dateTime.ToString("MMM d, yyyy");
					}
				}
				catch
				{
					result = timestamp;
				}
			}
			return result;
		}
	}
}
