using System;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200004D RID: 77
	public class DefaultSerializationBinder : SerializationBinder
	{
		// Token: 0x060003FC RID: 1020 RVA: 0x0001090C File Offset: 0x0000EB0C
		private static Type GetTypeFromTypeNameKey(DefaultSerializationBinder.TypeNameKey typeNameKey)
		{
			string assemblyName = typeNameKey.AssemblyName;
			string typeName = typeNameKey.TypeName;
			if (assemblyName == null)
			{
				return Type.GetType(typeName);
			}
			Assembly assembly = Assembly.Load(assemblyName);
			if (assembly == null)
			{
				throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					assemblyName
				}));
			}
			Type type = assembly.GetType(typeName);
			if (type == null)
			{
				throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					typeName,
					assembly.FullName
				}));
			}
			return type;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0001099B File Offset: 0x0000EB9B
		public override Type BindToType(string assemblyName, string typeName)
		{
			return this._typeCache.Get(new DefaultSerializationBinder.TypeNameKey(assemblyName, typeName));
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000109AF File Offset: 0x0000EBAF
		public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			assemblyName = null;
			typeName = serializedType.AssemblyQualifiedName;
		}

		// Token: 0x04000110 RID: 272
		internal static readonly DefaultSerializationBinder Instance = new DefaultSerializationBinder();

		// Token: 0x04000111 RID: 273
		private readonly ThreadSafeStore<DefaultSerializationBinder.TypeNameKey, Type> _typeCache = new ThreadSafeStore<DefaultSerializationBinder.TypeNameKey, Type>(new Func<DefaultSerializationBinder.TypeNameKey, Type>(DefaultSerializationBinder.GetTypeFromTypeNameKey));

		// Token: 0x0200004E RID: 78
		internal struct TypeNameKey : IEquatable<DefaultSerializationBinder.TypeNameKey>
		{
			// Token: 0x06000401 RID: 1025 RVA: 0x000109E7 File Offset: 0x0000EBE7
			public TypeNameKey(string assemblyName, string typeName)
			{
				this.AssemblyName = assemblyName;
				this.TypeName = typeName;
			}

			// Token: 0x06000402 RID: 1026 RVA: 0x000109F7 File Offset: 0x0000EBF7
			public override int GetHashCode()
			{
				return ((this.AssemblyName != null) ? this.AssemblyName.GetHashCode() : 0) ^ ((this.TypeName != null) ? this.TypeName.GetHashCode() : 0);
			}

			// Token: 0x06000403 RID: 1027 RVA: 0x00010A26 File Offset: 0x0000EC26
			public override bool Equals(object obj)
			{
				return obj is DefaultSerializationBinder.TypeNameKey && this.Equals((DefaultSerializationBinder.TypeNameKey)obj);
			}

			// Token: 0x06000404 RID: 1028 RVA: 0x00010A3E File Offset: 0x0000EC3E
			public bool Equals(DefaultSerializationBinder.TypeNameKey other)
			{
				return this.AssemblyName == other.AssemblyName && this.TypeName == other.TypeName;
			}

			// Token: 0x04000112 RID: 274
			internal readonly string AssemblyName;

			// Token: 0x04000113 RID: 275
			internal readonly string TypeName;
		}
	}
}
