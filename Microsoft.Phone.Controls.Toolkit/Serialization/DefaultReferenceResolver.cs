using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200006C RID: 108
	internal class DefaultReferenceResolver : IReferenceResolver
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x000162AC File Offset: 0x000144AC
		private BidirectionalDictionary<string, object> GetMappings(object context)
		{
			JsonSerializerInternalBase jsonSerializerInternalBase;
			if (context is JsonSerializerInternalBase)
			{
				jsonSerializerInternalBase = (JsonSerializerInternalBase)context;
			}
			else
			{
				if (!(context is JsonSerializerProxy))
				{
					throw new Exception("The DefaultReferenceResolver can only be used internally.");
				}
				jsonSerializerInternalBase = ((JsonSerializerProxy)context).GetInternalSerializer();
			}
			return jsonSerializerInternalBase.DefaultReferenceMappings;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000162F4 File Offset: 0x000144F4
		public object ResolveReference(object context, string reference)
		{
			object result;
			this.GetMappings(context).TryGetByFirst(reference, out result);
			return result;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00016314 File Offset: 0x00014514
		public string GetReference(object context, object value)
		{
			BidirectionalDictionary<string, object> mappings = this.GetMappings(context);
			string text;
			if (!mappings.TryGetBySecond(value, out text))
			{
				this._referenceCount++;
				text = this._referenceCount.ToString(CultureInfo.InvariantCulture);
				mappings.Add(text, value);
			}
			return text;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0001635C File Offset: 0x0001455C
		public void AddReference(object context, string reference, object value)
		{
			this.GetMappings(context).Add(reference, value);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001636C File Offset: 0x0001456C
		public bool IsReferenced(object context, object value)
		{
			string text;
			return this.GetMappings(context).TryGetBySecond(value, out text);
		}

		// Token: 0x04000146 RID: 326
		private int _referenceCount;
	}
}
