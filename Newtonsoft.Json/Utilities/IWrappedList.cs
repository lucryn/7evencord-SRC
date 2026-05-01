using System;
using System.Collections;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200007C RID: 124
	internal interface IWrappedList : IList, ICollection, IEnumerable
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000619 RID: 1561
		object UnderlyingList { get; }
	}
}
