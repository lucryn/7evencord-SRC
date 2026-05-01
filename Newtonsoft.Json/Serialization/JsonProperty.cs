using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000073 RID: 115
	public class JsonProperty
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x00017750 File Offset: 0x00015950
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x00017758 File Offset: 0x00015958
		public string PropertyName { get; set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x00017761 File Offset: 0x00015961
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x00017769 File Offset: 0x00015969
		public int? Order { get; set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00017772 File Offset: 0x00015972
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x0001777A File Offset: 0x0001597A
		public string UnderlyingName { get; set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x00017783 File Offset: 0x00015983
		// (set) Token: 0x060005B3 RID: 1459 RVA: 0x0001778B File Offset: 0x0001598B
		public IValueProvider ValueProvider { get; set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00017794 File Offset: 0x00015994
		// (set) Token: 0x060005B5 RID: 1461 RVA: 0x0001779C File Offset: 0x0001599C
		public Type PropertyType { get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x000177A5 File Offset: 0x000159A5
		// (set) Token: 0x060005B7 RID: 1463 RVA: 0x000177AD File Offset: 0x000159AD
		public JsonConverter Converter { get; set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x000177B6 File Offset: 0x000159B6
		// (set) Token: 0x060005B9 RID: 1465 RVA: 0x000177BE File Offset: 0x000159BE
		public JsonConverter MemberConverter { get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x000177C7 File Offset: 0x000159C7
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x000177CF File Offset: 0x000159CF
		public bool Ignored { get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x000177D8 File Offset: 0x000159D8
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x000177E0 File Offset: 0x000159E0
		public bool Readable { get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x000177E9 File Offset: 0x000159E9
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x000177F1 File Offset: 0x000159F1
		public bool Writable { get; set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x000177FA File Offset: 0x000159FA
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x00017802 File Offset: 0x00015A02
		public object DefaultValue { get; set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0001780B File Offset: 0x00015A0B
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x00017813 File Offset: 0x00015A13
		public Required Required { get; set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0001781C File Offset: 0x00015A1C
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x00017824 File Offset: 0x00015A24
		public bool? IsReference { get; set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x0001782D File Offset: 0x00015A2D
		// (set) Token: 0x060005C7 RID: 1479 RVA: 0x00017835 File Offset: 0x00015A35
		public NullValueHandling? NullValueHandling { get; set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0001783E File Offset: 0x00015A3E
		// (set) Token: 0x060005C9 RID: 1481 RVA: 0x00017846 File Offset: 0x00015A46
		public DefaultValueHandling? DefaultValueHandling { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x0001784F File Offset: 0x00015A4F
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x00017857 File Offset: 0x00015A57
		public ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x00017860 File Offset: 0x00015A60
		// (set) Token: 0x060005CD RID: 1485 RVA: 0x00017868 File Offset: 0x00015A68
		public ObjectCreationHandling? ObjectCreationHandling { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x00017871 File Offset: 0x00015A71
		// (set) Token: 0x060005CF RID: 1487 RVA: 0x00017879 File Offset: 0x00015A79
		public TypeNameHandling? TypeNameHandling { get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x00017882 File Offset: 0x00015A82
		// (set) Token: 0x060005D1 RID: 1489 RVA: 0x0001788A File Offset: 0x00015A8A
		public Predicate<object> ShouldSerialize { get; set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x00017893 File Offset: 0x00015A93
		// (set) Token: 0x060005D3 RID: 1491 RVA: 0x0001789B File Offset: 0x00015A9B
		public Predicate<object> GetIsSpecified { get; set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x000178A4 File Offset: 0x00015AA4
		// (set) Token: 0x060005D5 RID: 1493 RVA: 0x000178AC File Offset: 0x00015AAC
		public Action<object, object> SetIsSpecified { get; set; }

		// Token: 0x060005D6 RID: 1494 RVA: 0x000178B5 File Offset: 0x00015AB5
		public override string ToString()
		{
			return this.PropertyName;
		}
	}
}
