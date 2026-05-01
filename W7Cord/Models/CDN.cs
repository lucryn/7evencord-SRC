using System;

namespace W7Cord.Models
{
	// Token: 0x0200000B RID: 11
	public static class CDN
	{
		// Token: 0x06000062 RID: 98 RVA: 0x000044F8 File Offset: 0x000026F8
		public static void SetProxyBaseUrl(string proxyUrl)
		{
			CDN._proxyBaseUrl = (proxyUrl.EndsWith("/") ? proxyUrl : (proxyUrl + "/"));
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000451C File Offset: 0x0000271C
		public static string AvatarUrl(string userId, string avatarHash, string discriminator)
		{
			string result;
			if (!string.IsNullOrEmpty(avatarHash))
			{
				string text = avatarHash.StartsWith("a_") ? "gif" : "png";
				string text2 = string.Concat(new string[]
				{
					"https://cdn.discordapp.com/avatars/",
					userId,
					"/",
					avatarHash,
					".",
					text,
					"?size=128"
				});
				result = CDN._proxyBaseUrl + "discord-api.php?image=" + Uri.EscapeDataString(text2);
			}
			else
			{
				int num = 0;
				if (int.TryParse(discriminator, ref num))
				{
					num %= 5;
				}
				string text3 = "https://cdn.discordapp.com/embed/avatars/" + num + ".png";
				result = CDN._proxyBaseUrl + "discord-api.php?image=" + Uri.EscapeDataString(text3);
			}
			return result;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000045F8 File Offset: 0x000027F8
		public static string GuildIconUrl(string guildId, string iconHash)
		{
			string result;
			if (string.IsNullOrEmpty(iconHash))
			{
				result = null;
			}
			else
			{
				string text = iconHash.StartsWith("a_") ? "gif" : "png";
				string text2 = string.Concat(new string[]
				{
					"https://cdn.discordapp.com/icons/",
					guildId,
					"/",
					iconHash,
					".",
					text,
					"?size=128"
				});
				result = CDN._proxyBaseUrl + "discord-api.php?image=" + Uri.EscapeDataString(text2);
			}
			return result;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004690 File Offset: 0x00002890
		public static string DefaultAvatarUrl(int index)
		{
			index %= 5;
			string text = "https://cdn.discordapp.com/embed/avatars/" + index + ".png";
			return CDN._proxyBaseUrl + "discord-api.php?image=" + Uri.EscapeDataString(text);
		}

		// Token: 0x04000031 RID: 49
		private static string _proxyBaseUrl;
	}
}
