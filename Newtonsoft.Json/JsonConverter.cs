using System;
using Newtonsoft.Json.Schema;

namespace Newtonsoft.Json
{
	// Token: 0x02000037 RID: 55
	public abstract class JsonConverter
	{
		// Token: 0x0600036E RID: 878
		public abstract void WriteJson(JsonWriter writer, object value, JsonSerializer serializer);

		// Token: 0x0600036F RID: 879
		public abstract object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);

		// Token: 0x06000370 RID: 880
		public abstract bool CanConvert(Type objectType);

		// Token: 0x06000371 RID: 881 RVA: 0x0000E2C8 File Offset: 0x0000C4C8
		public virtual JsonSchema GetSchema()
		{
			return null;
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000E2CB File Offset: 0x0000C4CB
		public virtual bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000373 RID: 883 RVA: 0x0000E2CE File Offset: 0x0000C4CE
		public virtual bool CanWrite
		{
			get
			{
				return true;
			}
		}
	}
}
