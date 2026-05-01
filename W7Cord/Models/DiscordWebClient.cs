using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;

namespace W7Cord.Models
{
	// Token: 0x0200000A RID: 10
	public class DiscordWebClient
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00003C94 File Offset: 0x00001E94
		public DiscordWebClient(string token, string bridgeUrl)
		{
			this._token = token;
			this._bridgeBaseUrl = (bridgeUrl.EndsWith("/") ? bridgeUrl : (bridgeUrl + "/"));
			this._hiddenBrowser = new WebBrowser();
			this._hiddenBrowser.IsScriptEnabled = true;
			this._hiddenBrowser.Visibility = 0;
			this._hiddenBrowser.LoadCompleted += delegate(object s, NavigationEventArgs e)
			{
				Debug.WriteLine("Browser: LoadCompleted");
				this._pollAttempts = 0;
				this._lastJsonData = "";
				if (this._waitingForResponse && !this._pollTimer.IsEnabled)
				{
					this._pollTimer.Start();
				}
			};
			this._pollTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(0.8)
			};
			this._pollTimer.Tick += new EventHandler(this.PollTimer_Tick);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003D54 File Offset: 0x00001F54
		public WebBrowser GetBrowser()
		{
			return this._hiddenBrowser;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003D6C File Offset: 0x00001F6C
		private T SafeDeserialize<T>(string json)
		{
			T result;
			try
			{
				result = JsonConvert.DeserializeObject<T>(json);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Deserialize error: " + ex.Message);
				result = default(T);
			}
			return result;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003DE4 File Offset: 0x00001FE4
		private void ExecuteRequest(string path, Action<string> jsonCallback, Action<string> errorCallback)
		{
			this._callback = jsonCallback;
			this._errorCallback = errorCallback;
			this._waitingForResponse = true;
			this._pollAttempts = 0;
			this._lastJsonData = "";
			string url = string.Format("{0}bridge.html?path={1}&token={2}", this._bridgeBaseUrl, Uri.EscapeDataString(path), Uri.EscapeDataString(this._token));
			Debug.WriteLine("Navigate to: " + url);
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
				this._hiddenBrowser.Navigate(new Uri(url, 1));
			});
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003EA4 File Offset: 0x000020A4
		public void GetCurrentUser(Action<User> cb, Action<string> err)
		{
			this.ExecuteRequest("users/@me", delegate(string j)
			{
				cb.Invoke(this.SafeDeserialize<User>(j));
			}, err);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003F04 File Offset: 0x00002104
		public void GetGuilds(int limit, Action<Guild[]> callback, Action<string> errorCallback)
		{
			string path = "users/@me/guilds?limit=" + limit;
			this.ExecuteRequest(path, delegate(string j)
			{
				callback.Invoke(this.SafeDeserialize<Guild[]>(j));
			}, errorCallback);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003F4D File Offset: 0x0000214D
		public void GetGuilds(Action<Guild[]> callback, Action<string> errorCallback)
		{
			this.GetGuilds(20, callback, errorCallback);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003F80 File Offset: 0x00002180
		public void GetChannels(string guildId, Action<Channel[]> cb, Action<string> err)
		{
			this.ExecuteRequest("guilds/" + guildId + "/channels", delegate(string j)
			{
				cb.Invoke(this.SafeDeserialize<Channel[]>(j));
			}, err);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003FEC File Offset: 0x000021EC
		public void GetDirectMessageChannels(Action<DirectMessageChannel[]> cb, Action<string> err)
		{
			this.ExecuteRequest("users/@me/channels", delegate(string j)
			{
				cb.Invoke(this.SafeDeserialize<DirectMessageChannel[]>(j));
			}, err);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000404C File Offset: 0x0000224C
		public void GetMessages(string channelId, int limit, Action<Message[]> cb, Action<string> err)
		{
			int num = Math.Min(limit, 30);
			this.ExecuteRequest(string.Concat(new object[]
			{
				"channels/",
				channelId,
				"/messages?limit=",
				num
			}), delegate(string j)
			{
				cb.Invoke(this.SafeDeserialize<Message[]>(j));
			}, err);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004138 File Offset: 0x00002338
		public void SendMessage(string channelId, string content, Action<Message> cb, Action<string> err)
		{
			string text = "channels/" + channelId + "/messages";
			string url = string.Format("{0}bridge.html?path={1}&token={2}&method=POST&data={3}", new object[]
			{
				this._bridgeBaseUrl,
				Uri.EscapeDataString(text),
				Uri.EscapeDataString(this._token),
				Uri.EscapeDataString(content)
			});
			Debug.WriteLine("Sending message to: " + url);
			this._callback = delegate(string json)
			{
				try
				{
					Message message = JsonConvert.DeserializeObject<Message>(json);
					cb.Invoke(message);
				}
				catch (Exception ex)
				{
					err.Invoke("Parse error: " + ex.Message);
				}
			};
			this._errorCallback = err;
			this._waitingForResponse = true;
			this._pollAttempts = 0;
			this._lastJsonData = "";
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
				this._hiddenBrowser.Navigate(new Uri(url, 1));
			});
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004220 File Offset: 0x00002420
		private void PollTimer_Tick(object sender, EventArgs e)
		{
			if (!this._waitingForResponse)
			{
				this._pollTimer.Stop();
			}
			else
			{
				this._pollAttempts++;
				Debug.WriteLine(string.Concat(new object[]
				{
					"Poll attempt ",
					this._pollAttempts,
					"/",
					60
				}));
				if (this._pollAttempts >= 60)
				{
					this._pollTimer.Stop();
					this._waitingForResponse = false;
					if (this._errorCallback != null)
					{
						this._errorCallback.Invoke("Timeout waiting for response");
					}
				}
				else
				{
					try
					{
						string text = "";
						try
						{
							text = (this._hiddenBrowser.InvokeScript("eval", new string[]
							{
								"document.getElementById('raw-output').innerText"
							}) as string);
						}
						catch (Exception ex)
						{
							Debug.WriteLine("InvokeScript failed: " + ex.Message);
							return;
						}
						if (string.IsNullOrEmpty(text))
						{
							Debug.WriteLine("No data yet");
						}
						else
						{
							string text2 = text.Trim();
							Debug.WriteLine("Got data length: " + text2.Length);
							if (text2.Length == 0)
							{
								Debug.WriteLine("Empty data after trim");
							}
							else
							{
								char c = text2.get_Chars(0);
								char c2 = text2.get_Chars(text2.Length - 1);
								Debug.WriteLine("First char: '" + c + "'");
								Debug.WriteLine("Last char: '" + c2 + "'");
								if ((c == '[' && c2 == ']') || (c == '{' && c2 == '}'))
								{
									Debug.WriteLine("Complete JSON detected!");
									this._pollTimer.Stop();
									this._waitingForResponse = false;
									if (this._callback != null)
									{
										this._callback.Invoke(text2);
									}
								}
								else if ((c == '[' || c == '{') && text2.Length > 100)
								{
									Debug.WriteLine("JSON incomplete, waiting for more data...");
								}
								else
								{
									Debug.WriteLine("Not JSON yet: " + ((text2.Length > 100) ? text2.Substring(0, 100) : text2));
								}
							}
						}
					}
					catch (Exception ex)
					{
						Debug.WriteLine("Poll error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x04000027 RID: 39
		private const int MAX_ATTEMPTS = 60;

		// Token: 0x04000028 RID: 40
		private string _token;

		// Token: 0x04000029 RID: 41
		private string _bridgeBaseUrl;

		// Token: 0x0400002A RID: 42
		private WebBrowser _hiddenBrowser;

		// Token: 0x0400002B RID: 43
		private DispatcherTimer _pollTimer;

		// Token: 0x0400002C RID: 44
		private Action<string> _callback;

		// Token: 0x0400002D RID: 45
		private Action<string> _errorCallback;

		// Token: 0x0400002E RID: 46
		private int _pollAttempts;

		// Token: 0x0400002F RID: 47
		private bool _waitingForResponse;

		// Token: 0x04000030 RID: 48
		private string _lastJsonData;
	}
}
