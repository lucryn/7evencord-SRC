using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200001E RID: 30
	public interface IJEnumerable<T> : IEnumerable<T>, IEnumerable where T : JToken
	{
		// Token: 0x1700005A RID: 90
		IJEnumerable<JToken> this[object key]
		{
			get;
		}
	}
}
