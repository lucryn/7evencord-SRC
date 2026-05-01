using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000017 RID: 23
	public class JsonSerializerSettings
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00007765 File Offset: 0x00005965
		// (set) Token: 0x06000116 RID: 278 RVA: 0x0000776D File Offset: 0x0000596D
		public ReferenceLoopHandling ReferenceLoopHandling { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00007776 File Offset: 0x00005976
		// (set) Token: 0x06000118 RID: 280 RVA: 0x0000777E File Offset: 0x0000597E
		public MissingMemberHandling MissingMemberHandling { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00007787 File Offset: 0x00005987
		// (set) Token: 0x0600011A RID: 282 RVA: 0x0000778F File Offset: 0x0000598F
		public ObjectCreationHandling ObjectCreationHandling { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00007798 File Offset: 0x00005998
		// (set) Token: 0x0600011C RID: 284 RVA: 0x000077A0 File Offset: 0x000059A0
		public NullValueHandling NullValueHandling { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600011D RID: 285 RVA: 0x000077A9 File Offset: 0x000059A9
		// (set) Token: 0x0600011E RID: 286 RVA: 0x000077B1 File Offset: 0x000059B1
		public DefaultValueHandling DefaultValueHandling { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600011F RID: 287 RVA: 0x000077BA File Offset: 0x000059BA
		// (set) Token: 0x06000120 RID: 288 RVA: 0x000077C2 File Offset: 0x000059C2
		public IList<JsonConverter> Converters { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000121 RID: 289 RVA: 0x000077CB File Offset: 0x000059CB
		// (set) Token: 0x06000122 RID: 290 RVA: 0x000077D3 File Offset: 0x000059D3
		public PreserveReferencesHandling PreserveReferencesHandling { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000123 RID: 291 RVA: 0x000077DC File Offset: 0x000059DC
		// (set) Token: 0x06000124 RID: 292 RVA: 0x000077E4 File Offset: 0x000059E4
		public TypeNameHandling TypeNameHandling { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000125 RID: 293 RVA: 0x000077ED File Offset: 0x000059ED
		// (set) Token: 0x06000126 RID: 294 RVA: 0x000077F5 File Offset: 0x000059F5
		public FormatterAssemblyStyle TypeNameAssemblyFormat { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000127 RID: 295 RVA: 0x000077FE File Offset: 0x000059FE
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00007806 File Offset: 0x00005A06
		public ConstructorHandling ConstructorHandling { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000129 RID: 297 RVA: 0x0000780F File Offset: 0x00005A0F
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00007817 File Offset: 0x00005A17
		public IContractResolver ContractResolver { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00007820 File Offset: 0x00005A20
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00007828 File Offset: 0x00005A28
		public IReferenceResolver ReferenceResolver { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00007831 File Offset: 0x00005A31
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00007839 File Offset: 0x00005A39
		public SerializationBinder Binder { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00007842 File Offset: 0x00005A42
		// (set) Token: 0x06000130 RID: 304 RVA: 0x0000784A File Offset: 0x00005A4A
		public EventHandler<ErrorEventArgs> Error { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00007853 File Offset: 0x00005A53
		// (set) Token: 0x06000132 RID: 306 RVA: 0x0000785B File Offset: 0x00005A5B
		public StreamingContext Context { get; set; }

		// Token: 0x06000133 RID: 307 RVA: 0x00007864 File Offset: 0x00005A64
		public JsonSerializerSettings()
		{
			this.ReferenceLoopHandling = ReferenceLoopHandling.Error;
			this.MissingMemberHandling = MissingMemberHandling.Ignore;
			this.ObjectCreationHandling = ObjectCreationHandling.Auto;
			this.NullValueHandling = NullValueHandling.Include;
			this.DefaultValueHandling = DefaultValueHandling.Include;
			this.PreserveReferencesHandling = PreserveReferencesHandling.None;
			this.TypeNameHandling = TypeNameHandling.None;
			this.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
			this.Context = JsonSerializerSettings.DefaultContext;
			this.Converters = new List<JsonConverter>();
		}

		// Token: 0x04000074 RID: 116
		internal const ReferenceLoopHandling DefaultReferenceLoopHandling = ReferenceLoopHandling.Error;

		// Token: 0x04000075 RID: 117
		internal const MissingMemberHandling DefaultMissingMemberHandling = MissingMemberHandling.Ignore;

		// Token: 0x04000076 RID: 118
		internal const NullValueHandling DefaultNullValueHandling = NullValueHandling.Include;

		// Token: 0x04000077 RID: 119
		internal const DefaultValueHandling DefaultDefaultValueHandling = DefaultValueHandling.Include;

		// Token: 0x04000078 RID: 120
		internal const ObjectCreationHandling DefaultObjectCreationHandling = ObjectCreationHandling.Auto;

		// Token: 0x04000079 RID: 121
		internal const PreserveReferencesHandling DefaultPreserveReferencesHandling = PreserveReferencesHandling.None;

		// Token: 0x0400007A RID: 122
		internal const ConstructorHandling DefaultConstructorHandling = ConstructorHandling.Default;

		// Token: 0x0400007B RID: 123
		internal const TypeNameHandling DefaultTypeNameHandling = TypeNameHandling.None;

		// Token: 0x0400007C RID: 124
		internal const FormatterAssemblyStyle DefaultTypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;

		// Token: 0x0400007D RID: 125
		internal static readonly StreamingContext DefaultContext = default(StreamingContext);
	}
}
