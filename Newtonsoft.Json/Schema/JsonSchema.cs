using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000097 RID: 151
	public class JsonSchema
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0001B066 File Offset: 0x00019266
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x0001B06E File Offset: 0x0001926E
		public string Id { get; set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0001B077 File Offset: 0x00019277
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x0001B07F File Offset: 0x0001927F
		public string Title { get; set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0001B088 File Offset: 0x00019288
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x0001B090 File Offset: 0x00019290
		public bool? Required { get; set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0001B099 File Offset: 0x00019299
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x0001B0A1 File Offset: 0x000192A1
		public bool? ReadOnly { get; set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0001B0AA File Offset: 0x000192AA
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x0001B0B2 File Offset: 0x000192B2
		public bool? Hidden { get; set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x0001B0BB File Offset: 0x000192BB
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x0001B0C3 File Offset: 0x000192C3
		public bool? Transient { get; set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x0001B0CC File Offset: 0x000192CC
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x0001B0D4 File Offset: 0x000192D4
		public string Description { get; set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0001B0DD File Offset: 0x000192DD
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x0001B0E5 File Offset: 0x000192E5
		public JsonSchemaType? Type { get; set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x0001B0EE File Offset: 0x000192EE
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x0001B0F6 File Offset: 0x000192F6
		public string Pattern { get; set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x0001B0FF File Offset: 0x000192FF
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x0001B107 File Offset: 0x00019307
		public int? MinimumLength { get; set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x0001B110 File Offset: 0x00019310
		// (set) Token: 0x0600070F RID: 1807 RVA: 0x0001B118 File Offset: 0x00019318
		public int? MaximumLength { get; set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x0001B121 File Offset: 0x00019321
		// (set) Token: 0x06000711 RID: 1809 RVA: 0x0001B129 File Offset: 0x00019329
		public double? DivisibleBy { get; set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x0001B132 File Offset: 0x00019332
		// (set) Token: 0x06000713 RID: 1811 RVA: 0x0001B13A File Offset: 0x0001933A
		public double? Minimum { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x0001B143 File Offset: 0x00019343
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x0001B14B File Offset: 0x0001934B
		public double? Maximum { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x0001B154 File Offset: 0x00019354
		// (set) Token: 0x06000717 RID: 1815 RVA: 0x0001B15C File Offset: 0x0001935C
		public bool? ExclusiveMinimum { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x0001B165 File Offset: 0x00019365
		// (set) Token: 0x06000719 RID: 1817 RVA: 0x0001B16D File Offset: 0x0001936D
		public bool? ExclusiveMaximum { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x0001B176 File Offset: 0x00019376
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x0001B17E File Offset: 0x0001937E
		public int? MinimumItems { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x0001B187 File Offset: 0x00019387
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x0001B18F File Offset: 0x0001938F
		public int? MaximumItems { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0001B198 File Offset: 0x00019398
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x0001B1A0 File Offset: 0x000193A0
		public IList<JsonSchema> Items { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0001B1A9 File Offset: 0x000193A9
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x0001B1B1 File Offset: 0x000193B1
		public IDictionary<string, JsonSchema> Properties { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x0001B1BA File Offset: 0x000193BA
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x0001B1C2 File Offset: 0x000193C2
		public JsonSchema AdditionalProperties { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x0001B1CB File Offset: 0x000193CB
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x0001B1D3 File Offset: 0x000193D3
		public IDictionary<string, JsonSchema> PatternProperties { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0001B1DC File Offset: 0x000193DC
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x0001B1E4 File Offset: 0x000193E4
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0001B1ED File Offset: 0x000193ED
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x0001B1F5 File Offset: 0x000193F5
		public string Requires { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x0001B1FE File Offset: 0x000193FE
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x0001B206 File Offset: 0x00019406
		public IList<string> Identity { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0001B20F File Offset: 0x0001940F
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x0001B217 File Offset: 0x00019417
		public IList<JToken> Enum { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0001B220 File Offset: 0x00019420
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x0001B228 File Offset: 0x00019428
		public IDictionary<JToken, string> Options { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0001B231 File Offset: 0x00019431
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x0001B239 File Offset: 0x00019439
		public JsonSchemaType? Disallow { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x0001B242 File Offset: 0x00019442
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x0001B24A File Offset: 0x0001944A
		public JToken Default { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x0001B253 File Offset: 0x00019453
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x0001B25B File Offset: 0x0001945B
		public JsonSchema Extends { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x0001B264 File Offset: 0x00019464
		// (set) Token: 0x06000737 RID: 1847 RVA: 0x0001B26C File Offset: 0x0001946C
		public string Format { get; set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x0001B275 File Offset: 0x00019475
		internal string InternalId
		{
			get
			{
				return this._internalId;
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001B280 File Offset: 0x00019480
		public JsonSchema()
		{
			this.AllowAdditionalProperties = true;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001B2B2 File Offset: 0x000194B2
		public static JsonSchema Read(JsonReader reader)
		{
			return JsonSchema.Read(reader, new JsonSchemaResolver());
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001B2C0 File Offset: 0x000194C0
		public static JsonSchema Read(JsonReader reader, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			JsonSchemaBuilder jsonSchemaBuilder = new JsonSchemaBuilder(resolver);
			return jsonSchemaBuilder.Parse(reader);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001B2F1 File Offset: 0x000194F1
		public static JsonSchema Parse(string json)
		{
			return JsonSchema.Parse(json, new JsonSchemaResolver());
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001B300 File Offset: 0x00019500
		public static JsonSchema Parse(string json, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(json, "json");
			JsonReader reader = new JsonTextReader(new StringReader(json));
			return JsonSchema.Read(reader, resolver);
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0001B32B File Offset: 0x0001952B
		public void WriteTo(JsonWriter writer)
		{
			this.WriteTo(writer, new JsonSchemaResolver());
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0001B33C File Offset: 0x0001953C
		public void WriteTo(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			JsonSchemaWriter jsonSchemaWriter = new JsonSchemaWriter(writer, resolver);
			jsonSchemaWriter.WriteSchema(this);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0001B370 File Offset: 0x00019570
		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			this.WriteTo(new JsonTextWriter(stringWriter)
			{
				Formatting = Formatting.Indented
			});
			return stringWriter.ToString();
		}

		// Token: 0x040001FB RID: 507
		private readonly string _internalId = Guid.NewGuid().ToString("N");
	}
}
