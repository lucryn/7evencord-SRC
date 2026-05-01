using System;
using System.Collections;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200000C RID: 12
	internal interface IWrappedDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600007C RID: 124
		object UnderlyingDictionary { get; }
	}
}
