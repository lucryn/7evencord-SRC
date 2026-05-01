using System;
using System.Collections;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200004A RID: 74
	internal interface IWrappedCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003E0 RID: 992
		object UnderlyingCollection { get; }
	}
}
