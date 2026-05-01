using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000007 RID: 7
	public class JsonSchemaResolver
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000029E1 File Offset: 0x00000BE1
		// (set) Token: 0x06000016 RID: 22 RVA: 0x000029E9 File Offset: 0x00000BE9
		public IList<JsonSchema> LoadedSchemas { get; protected set; }

		// Token: 0x06000017 RID: 23 RVA: 0x000029F2 File Offset: 0x00000BF2
		public JsonSchemaResolver()
		{
			this.LoadedSchemas = new List<JsonSchema>();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002A20 File Offset: 0x00000C20
		public virtual JsonSchema GetSchema(string id)
		{
			return Enumerable.SingleOrDefault<JsonSchema>(this.LoadedSchemas, (JsonSchema s) => s.Id == id);
		}
	}
}
