using System;
using System.Collections.ObjectModel;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000057 RID: 87
	internal class EnumValues<T> : KeyedCollection<string, EnumValue<T>> where T : struct
	{
		// Token: 0x06000434 RID: 1076 RVA: 0x00011732 File Offset: 0x0000F932
		protected override string GetKeyForItem(EnumValue<T> item)
		{
			return item.Name;
		}
	}
}
