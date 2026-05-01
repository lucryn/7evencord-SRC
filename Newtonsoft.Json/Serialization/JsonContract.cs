using System;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200001B RID: 27
	public abstract class JsonContract
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00007D0C File Offset: 0x00005F0C
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00007D14 File Offset: 0x00005F14
		public Type UnderlyingType { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00007D1D File Offset: 0x00005F1D
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00007D25 File Offset: 0x00005F25
		public Type CreatedType { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00007D2E File Offset: 0x00005F2E
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00007D36 File Offset: 0x00005F36
		public bool? IsReference { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00007D3F File Offset: 0x00005F3F
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00007D47 File Offset: 0x00005F47
		public JsonConverter Converter { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00007D50 File Offset: 0x00005F50
		// (set) Token: 0x0600015A RID: 346 RVA: 0x00007D58 File Offset: 0x00005F58
		internal JsonConverter InternalConverter { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00007D61 File Offset: 0x00005F61
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00007D69 File Offset: 0x00005F69
		public MethodInfo OnDeserialized { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00007D72 File Offset: 0x00005F72
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00007D7A File Offset: 0x00005F7A
		public MethodInfo OnDeserializing { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00007D83 File Offset: 0x00005F83
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00007D8B File Offset: 0x00005F8B
		public MethodInfo OnSerialized { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00007D94 File Offset: 0x00005F94
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00007D9C File Offset: 0x00005F9C
		public MethodInfo OnSerializing { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00007DA5 File Offset: 0x00005FA5
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00007DAD File Offset: 0x00005FAD
		public Func<object> DefaultCreator { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00007DB6 File Offset: 0x00005FB6
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00007DBE File Offset: 0x00005FBE
		public bool DefaultCreatorNonPublic { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00007DC7 File Offset: 0x00005FC7
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00007DCF File Offset: 0x00005FCF
		public MethodInfo OnError { get; set; }

		// Token: 0x06000169 RID: 361 RVA: 0x00007DD8 File Offset: 0x00005FD8
		internal void InvokeOnSerializing(object o, StreamingContext context)
		{
			if (this.OnSerializing != null)
			{
				this.OnSerializing.Invoke(o, new object[]
				{
					context
				});
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007E0C File Offset: 0x0000600C
		internal void InvokeOnSerialized(object o, StreamingContext context)
		{
			if (this.OnSerialized != null)
			{
				this.OnSerialized.Invoke(o, new object[]
				{
					context
				});
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00007E40 File Offset: 0x00006040
		internal void InvokeOnDeserializing(object o, StreamingContext context)
		{
			if (this.OnDeserializing != null)
			{
				this.OnDeserializing.Invoke(o, new object[]
				{
					context
				});
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007E74 File Offset: 0x00006074
		internal void InvokeOnDeserialized(object o, StreamingContext context)
		{
			if (this.OnDeserialized != null)
			{
				this.OnDeserialized.Invoke(o, new object[]
				{
					context
				});
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007EA8 File Offset: 0x000060A8
		internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
		{
			if (this.OnError != null)
			{
				this.OnError.Invoke(o, new object[]
				{
					context,
					errorContext
				});
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007EDF File Offset: 0x000060DF
		internal JsonContract(Type underlyingType)
		{
			ValidationUtils.ArgumentNotNull(underlyingType, "underlyingType");
			this.UnderlyingType = underlyingType;
			this.CreatedType = underlyingType;
		}
	}
}
