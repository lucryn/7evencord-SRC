using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200000F RID: 15
	public interface IReferenceResolver
	{
		// Token: 0x060000A5 RID: 165
		object ResolveReference(object context, string reference);

		// Token: 0x060000A6 RID: 166
		string GetReference(object context, object value);

		// Token: 0x060000A7 RID: 167
		bool IsReferenced(object context, object value);

		// Token: 0x060000A8 RID: 168
		void AddReference(object context, string reference, object value);
	}
}
