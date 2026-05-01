using System;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000080 RID: 128
	public class JsonObjectContract : JsonContract
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x00018AB8 File Offset: 0x00016CB8
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x00018AC0 File Offset: 0x00016CC0
		public MemberSerialization MemberSerialization { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x00018AC9 File Offset: 0x00016CC9
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x00018AD1 File Offset: 0x00016CD1
		public JsonPropertyCollection Properties { get; private set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x00018ADA File Offset: 0x00016CDA
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x00018AE2 File Offset: 0x00016CE2
		public JsonPropertyCollection ConstructorParameters { get; private set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x00018AEB File Offset: 0x00016CEB
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x00018AF3 File Offset: 0x00016CF3
		public ConstructorInfo OverrideConstructor { get; set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x00018AFC File Offset: 0x00016CFC
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x00018B04 File Offset: 0x00016D04
		public ConstructorInfo ParametrizedConstructor { get; set; }

		// Token: 0x06000658 RID: 1624 RVA: 0x00018B0D File Offset: 0x00016D0D
		public JsonObjectContract(Type underlyingType) : base(underlyingType)
		{
			this.Properties = new JsonPropertyCollection(base.UnderlyingType);
			this.ConstructorParameters = new JsonPropertyCollection(base.UnderlyingType);
		}
	}
}
