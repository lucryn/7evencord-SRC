using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200003E RID: 62
	public interface IContractResolver
	{
		// Token: 0x060003AA RID: 938
		JsonContract ResolveContract(Type type);
	}
}
