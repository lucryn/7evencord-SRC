using System;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200009C RID: 156
	public class JRaw : JValue
	{
		// Token: 0x06000777 RID: 1911 RVA: 0x0001BF5D File Offset: 0x0001A15D
		public JRaw(JRaw other) : base(other)
		{
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001BF66 File Offset: 0x0001A166
		public JRaw(object rawJson) : base(rawJson, JTokenType.Raw)
		{
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0001BF74 File Offset: 0x0001A174
		public static JRaw Create(JsonReader reader)
		{
			JRaw result;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
				{
					jsonTextWriter.WriteToken(reader);
					result = new JRaw(stringWriter.ToString());
				}
			}
			return result;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001BFDC File Offset: 0x0001A1DC
		internal override JToken CloneToken()
		{
			return new JRaw(this);
		}
	}
}
