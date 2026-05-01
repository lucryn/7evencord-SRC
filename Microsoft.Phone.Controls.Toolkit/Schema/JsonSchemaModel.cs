using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000070 RID: 112
	internal class JsonSchemaModel
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x000172C7 File Offset: 0x000154C7
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x000172CF File Offset: 0x000154CF
		public bool Required { get; set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x000172D8 File Offset: 0x000154D8
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x000172E0 File Offset: 0x000154E0
		public JsonSchemaType Type { get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x000172E9 File Offset: 0x000154E9
		// (set) Token: 0x06000583 RID: 1411 RVA: 0x000172F1 File Offset: 0x000154F1
		public int? MinimumLength { get; set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x000172FA File Offset: 0x000154FA
		// (set) Token: 0x06000585 RID: 1413 RVA: 0x00017302 File Offset: 0x00015502
		public int? MaximumLength { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x0001730B File Offset: 0x0001550B
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x00017313 File Offset: 0x00015513
		public double? DivisibleBy { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0001731C File Offset: 0x0001551C
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x00017324 File Offset: 0x00015524
		public double? Minimum { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0001732D File Offset: 0x0001552D
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x00017335 File Offset: 0x00015535
		public double? Maximum { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0001733E File Offset: 0x0001553E
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x00017346 File Offset: 0x00015546
		public bool ExclusiveMinimum { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x0001734F File Offset: 0x0001554F
		// (set) Token: 0x0600058F RID: 1423 RVA: 0x00017357 File Offset: 0x00015557
		public bool ExclusiveMaximum { get; set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x00017360 File Offset: 0x00015560
		// (set) Token: 0x06000591 RID: 1425 RVA: 0x00017368 File Offset: 0x00015568
		public int? MinimumItems { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x00017371 File Offset: 0x00015571
		// (set) Token: 0x06000593 RID: 1427 RVA: 0x00017379 File Offset: 0x00015579
		public int? MaximumItems { get; set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x00017382 File Offset: 0x00015582
		// (set) Token: 0x06000595 RID: 1429 RVA: 0x0001738A File Offset: 0x0001558A
		public IList<string> Patterns { get; set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00017393 File Offset: 0x00015593
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x0001739B File Offset: 0x0001559B
		public IList<JsonSchemaModel> Items { get; set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x000173A4 File Offset: 0x000155A4
		// (set) Token: 0x06000599 RID: 1433 RVA: 0x000173AC File Offset: 0x000155AC
		public IDictionary<string, JsonSchemaModel> Properties { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x000173B5 File Offset: 0x000155B5
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x000173BD File Offset: 0x000155BD
		public IDictionary<string, JsonSchemaModel> PatternProperties { get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x000173C6 File Offset: 0x000155C6
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x000173CE File Offset: 0x000155CE
		public JsonSchemaModel AdditionalProperties { get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x000173D7 File Offset: 0x000155D7
		// (set) Token: 0x0600059F RID: 1439 RVA: 0x000173DF File Offset: 0x000155DF
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x000173E8 File Offset: 0x000155E8
		// (set) Token: 0x060005A1 RID: 1441 RVA: 0x000173F0 File Offset: 0x000155F0
		public IList<JToken> Enum { get; set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x000173F9 File Offset: 0x000155F9
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x00017401 File Offset: 0x00015601
		public JsonSchemaType Disallow { get; set; }

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001740A File Offset: 0x0001560A
		public JsonSchemaModel()
		{
			this.Type = JsonSchemaType.Any;
			this.AllowAdditionalProperties = true;
			this.Required = false;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00017428 File Offset: 0x00015628
		public static JsonSchemaModel Create(IList<JsonSchema> schemata)
		{
			JsonSchemaModel jsonSchemaModel = new JsonSchemaModel();
			foreach (JsonSchema schema in schemata)
			{
				JsonSchemaModel.Combine(jsonSchemaModel, schema);
			}
			return jsonSchemaModel;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00017478 File Offset: 0x00015678
		private static void Combine(JsonSchemaModel model, JsonSchema schema)
		{
			model.Required = (model.Required || (schema.Required ?? false));
			model.Type &= (schema.Type ?? JsonSchemaType.Any);
			model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
			model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
			model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
			model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
			model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
			model.ExclusiveMinimum = (model.ExclusiveMinimum || (schema.ExclusiveMinimum ?? false));
			model.ExclusiveMaximum = (model.ExclusiveMaximum || (schema.ExclusiveMaximum ?? false));
			model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
			model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
			model.AllowAdditionalProperties = (model.AllowAdditionalProperties && schema.AllowAdditionalProperties);
			if (schema.Enum != null)
			{
				if (model.Enum == null)
				{
					model.Enum = new List<JToken>();
				}
				model.Enum.AddRangeDistinct(schema.Enum, new JTokenEqualityComparer());
			}
			model.Disallow |= (schema.Disallow ?? JsonSchemaType.None);
			if (schema.Pattern != null)
			{
				if (model.Patterns == null)
				{
					model.Patterns = new List<string>();
				}
				model.Patterns.AddDistinct(schema.Pattern);
			}
		}
	}
}
