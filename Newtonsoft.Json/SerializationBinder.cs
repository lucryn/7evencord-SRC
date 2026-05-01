using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200001D RID: 29
	public abstract class SerializationBinder
	{
		// Token: 0x06000175 RID: 373
		public abstract Type BindToType(string assemblyName, string typeName);

		// Token: 0x06000176 RID: 374 RVA: 0x000081B7 File Offset: 0x000063B7
		public virtual void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			assemblyName = null;
			typeName = null;
		}
	}
}
