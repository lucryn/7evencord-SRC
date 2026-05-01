using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000050 RID: 80
	public class JTokenEqualityComparer : IEqualityComparer<JToken>
	{
		// Token: 0x06000407 RID: 1031 RVA: 0x00010A8D File Offset: 0x0000EC8D
		public bool Equals(JToken x, JToken y)
		{
			return JToken.DeepEquals(x, y);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00010A96 File Offset: 0x0000EC96
		public int GetHashCode(JToken obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetDeepHashCode();
		}
	}
}
