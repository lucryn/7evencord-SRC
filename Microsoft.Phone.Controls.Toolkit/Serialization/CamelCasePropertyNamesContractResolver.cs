using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008F RID: 143
	public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
	{
		// Token: 0x060006D2 RID: 1746 RVA: 0x0001A3AA File Offset: 0x000185AA
		public CamelCasePropertyNamesContractResolver() : base(true)
		{
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001A3B3 File Offset: 0x000185B3
		protected internal override string ResolvePropertyName(string propertyName)
		{
			return StringUtils.ToCamelCase(propertyName);
		}
	}
}
