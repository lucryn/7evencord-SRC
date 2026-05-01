using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000059 RID: 89
	public abstract class DateTimeConverterBase : JsonConverter
	{
		// Token: 0x06000451 RID: 1105 RVA: 0x00011A3E File Offset: 0x0000FC3E
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime) || objectType == typeof(DateTime?) || (objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?));
		}
	}
}
