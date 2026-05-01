using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000016 RID: 22
	[AttributeUsage(2432, AllowMultiple = false)]
	public sealed class JsonPropertyAttribute : Attribute
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000075C0 File Offset: 0x000057C0
		// (set) Token: 0x06000102 RID: 258 RVA: 0x000075E6 File Offset: 0x000057E6
		public NullValueHandling NullValueHandling
		{
			get
			{
				NullValueHandling? nullValueHandling = this._nullValueHandling;
				if (nullValueHandling == null)
				{
					return NullValueHandling.Include;
				}
				return nullValueHandling.GetValueOrDefault();
			}
			set
			{
				this._nullValueHandling = new NullValueHandling?(value);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000075F4 File Offset: 0x000057F4
		// (set) Token: 0x06000104 RID: 260 RVA: 0x0000761A File Offset: 0x0000581A
		public DefaultValueHandling DefaultValueHandling
		{
			get
			{
				DefaultValueHandling? defaultValueHandling = this._defaultValueHandling;
				if (defaultValueHandling == null)
				{
					return DefaultValueHandling.Include;
				}
				return defaultValueHandling.GetValueOrDefault();
			}
			set
			{
				this._defaultValueHandling = new DefaultValueHandling?(value);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00007628 File Offset: 0x00005828
		// (set) Token: 0x06000106 RID: 262 RVA: 0x0000764E File Offset: 0x0000584E
		public ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				ReferenceLoopHandling? referenceLoopHandling = this._referenceLoopHandling;
				if (referenceLoopHandling == null)
				{
					return ReferenceLoopHandling.Error;
				}
				return referenceLoopHandling.GetValueOrDefault();
			}
			set
			{
				this._referenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000107 RID: 263 RVA: 0x0000765C File Offset: 0x0000585C
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00007682 File Offset: 0x00005882
		public ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				ObjectCreationHandling? objectCreationHandling = this._objectCreationHandling;
				if (objectCreationHandling == null)
				{
					return ObjectCreationHandling.Auto;
				}
				return objectCreationHandling.GetValueOrDefault();
			}
			set
			{
				this._objectCreationHandling = new ObjectCreationHandling?(value);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00007690 File Offset: 0x00005890
		// (set) Token: 0x0600010A RID: 266 RVA: 0x000076B6 File Offset: 0x000058B6
		public TypeNameHandling TypeNameHandling
		{
			get
			{
				TypeNameHandling? typeNameHandling = this._typeNameHandling;
				if (typeNameHandling == null)
				{
					return TypeNameHandling.None;
				}
				return typeNameHandling.GetValueOrDefault();
			}
			set
			{
				this._typeNameHandling = new TypeNameHandling?(value);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600010B RID: 267 RVA: 0x000076C4 File Offset: 0x000058C4
		// (set) Token: 0x0600010C RID: 268 RVA: 0x000076EA File Offset: 0x000058EA
		public bool IsReference
		{
			get
			{
				return this._isReference ?? false;
			}
			set
			{
				this._isReference = new bool?(value);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000076F8 File Offset: 0x000058F8
		// (set) Token: 0x0600010E RID: 270 RVA: 0x0000771E File Offset: 0x0000591E
		public int Order
		{
			get
			{
				int? order = this._order;
				if (order == null)
				{
					return 0;
				}
				return order.GetValueOrDefault();
			}
			set
			{
				this._order = new int?(value);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0000772C File Offset: 0x0000592C
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00007734 File Offset: 0x00005934
		public string PropertyName { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000773D File Offset: 0x0000593D
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00007745 File Offset: 0x00005945
		public Required Required { get; set; }

		// Token: 0x06000113 RID: 275 RVA: 0x0000774E File Offset: 0x0000594E
		public JsonPropertyAttribute()
		{
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00007756 File Offset: 0x00005956
		public JsonPropertyAttribute(string propertyName)
		{
			this.PropertyName = propertyName;
		}

		// Token: 0x0400006B RID: 107
		internal NullValueHandling? _nullValueHandling;

		// Token: 0x0400006C RID: 108
		internal DefaultValueHandling? _defaultValueHandling;

		// Token: 0x0400006D RID: 109
		internal ReferenceLoopHandling? _referenceLoopHandling;

		// Token: 0x0400006E RID: 110
		internal ObjectCreationHandling? _objectCreationHandling;

		// Token: 0x0400006F RID: 111
		internal TypeNameHandling? _typeNameHandling;

		// Token: 0x04000070 RID: 112
		internal bool? _isReference;

		// Token: 0x04000071 RID: 113
		internal int? _order;
	}
}
