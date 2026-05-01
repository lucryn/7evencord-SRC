using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200008A RID: 138
	internal class JsonSchemaNode
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x00018D1B File Offset: 0x00016F1B
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x00018D23 File Offset: 0x00016F23
		public string Id { get; private set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x00018D2C File Offset: 0x00016F2C
		// (set) Token: 0x06000682 RID: 1666 RVA: 0x00018D34 File Offset: 0x00016F34
		public ReadOnlyCollection<JsonSchema> Schemas { get; private set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x00018D3D File Offset: 0x00016F3D
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x00018D45 File Offset: 0x00016F45
		public Dictionary<string, JsonSchemaNode> Properties { get; private set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x00018D4E File Offset: 0x00016F4E
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x00018D56 File Offset: 0x00016F56
		public Dictionary<string, JsonSchemaNode> PatternProperties { get; private set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x00018D5F File Offset: 0x00016F5F
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x00018D67 File Offset: 0x00016F67
		public List<JsonSchemaNode> Items { get; private set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x00018D70 File Offset: 0x00016F70
		// (set) Token: 0x0600068A RID: 1674 RVA: 0x00018D78 File Offset: 0x00016F78
		public JsonSchemaNode AdditionalProperties { get; set; }

		// Token: 0x0600068B RID: 1675 RVA: 0x00018D84 File Offset: 0x00016F84
		public JsonSchemaNode(JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(new JsonSchema[]
			{
				schema
			});
			this.Properties = new Dictionary<string, JsonSchemaNode>();
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>();
			this.Items = new List<JsonSchemaNode>();
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00018DE0 File Offset: 0x00016FE0
		private JsonSchemaNode(JsonSchemaNode source, JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(Enumerable.ToList<JsonSchema>(Enumerable.Union<JsonSchema>(source.Schemas, new JsonSchema[]
			{
				schema
			})));
			this.Properties = new Dictionary<string, JsonSchemaNode>(source.Properties);
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>(source.PatternProperties);
			this.Items = new List<JsonSchemaNode>(source.Items);
			this.AdditionalProperties = source.AdditionalProperties;
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00018E6A File Offset: 0x0001706A
		public JsonSchemaNode Combine(JsonSchema schema)
		{
			return new JsonSchemaNode(this, schema);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00018E80 File Offset: 0x00017080
		public static string GetId(IEnumerable<JsonSchema> schemata)
		{
			return string.Join("-", Enumerable.ToArray<string>(Enumerable.OrderBy<string, string>(Enumerable.Select<JsonSchema, string>(schemata, (JsonSchema s) => s.InternalId), (string id) => id, StringComparer.Ordinal)));
		}
	}
}
