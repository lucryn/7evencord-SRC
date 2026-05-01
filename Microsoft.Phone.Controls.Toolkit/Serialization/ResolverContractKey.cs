using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000AD RID: 173
	internal struct ResolverContractKey : IEquatable<ResolverContractKey>
	{
		// Token: 0x06000824 RID: 2084 RVA: 0x0001E344 File Offset: 0x0001C544
		public ResolverContractKey(Type resolverType, Type contractType)
		{
			this._resolverType = resolverType;
			this._contractType = contractType;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001E354 File Offset: 0x0001C554
		public override int GetHashCode()
		{
			return this._resolverType.GetHashCode() ^ this._contractType.GetHashCode();
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0001E36D File Offset: 0x0001C56D
		public override bool Equals(object obj)
		{
			return obj is ResolverContractKey && this.Equals((ResolverContractKey)obj);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0001E385 File Offset: 0x0001C585
		public bool Equals(ResolverContractKey other)
		{
			return this._resolverType == other._resolverType && this._contractType == other._contractType;
		}

		// Token: 0x0400028E RID: 654
		private readonly Type _resolverType;

		// Token: 0x0400028F RID: 655
		private readonly Type _contractType;
	}
}
