using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x0200006F RID: 111
	public class JsonSerializer
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000551 RID: 1361 RVA: 0x00016DA4 File Offset: 0x00014FA4
		// (remove) Token: 0x06000552 RID: 1362 RVA: 0x00016DDC File Offset: 0x00014FDC
		public virtual event EventHandler<ErrorEventArgs> Error;

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x00016E11 File Offset: 0x00015011
		// (set) Token: 0x06000554 RID: 1364 RVA: 0x00016E2C File Offset: 0x0001502C
		public virtual IReferenceResolver ReferenceResolver
		{
			get
			{
				if (this._referenceResolver == null)
				{
					this._referenceResolver = new DefaultReferenceResolver();
				}
				return this._referenceResolver;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Reference resolver cannot be null.");
				}
				this._referenceResolver = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x00016E48 File Offset: 0x00015048
		// (set) Token: 0x06000556 RID: 1366 RVA: 0x00016E50 File Offset: 0x00015050
		public virtual SerializationBinder Binder
		{
			get
			{
				return this._binder;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Serialization binder cannot be null.");
				}
				this._binder = value;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x00016E6C File Offset: 0x0001506C
		// (set) Token: 0x06000558 RID: 1368 RVA: 0x00016E74 File Offset: 0x00015074
		public virtual TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._typeNameHandling;
			}
			set
			{
				if (value < TypeNameHandling.None || value > TypeNameHandling.Auto)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._typeNameHandling = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x00016E90 File Offset: 0x00015090
		// (set) Token: 0x0600055A RID: 1370 RVA: 0x00016E98 File Offset: 0x00015098
		public virtual FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return this._typeNameAssemblyFormat;
			}
			set
			{
				if (value < FormatterAssemblyStyle.Simple || value > FormatterAssemblyStyle.Full)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._typeNameAssemblyFormat = value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x00016EB4 File Offset: 0x000150B4
		// (set) Token: 0x0600055C RID: 1372 RVA: 0x00016EBC File Offset: 0x000150BC
		public virtual PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._preserveReferencesHandling;
			}
			set
			{
				if (value < PreserveReferencesHandling.None || value > PreserveReferencesHandling.All)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._preserveReferencesHandling = value;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x00016ED8 File Offset: 0x000150D8
		// (set) Token: 0x0600055E RID: 1374 RVA: 0x00016EE0 File Offset: 0x000150E0
		public virtual ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._referenceLoopHandling;
			}
			set
			{
				if (value < ReferenceLoopHandling.Error || value > ReferenceLoopHandling.Serialize)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._referenceLoopHandling = value;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x00016EFC File Offset: 0x000150FC
		// (set) Token: 0x06000560 RID: 1376 RVA: 0x00016F04 File Offset: 0x00015104
		public virtual MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._missingMemberHandling;
			}
			set
			{
				if (value < MissingMemberHandling.Ignore || value > MissingMemberHandling.Error)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._missingMemberHandling = value;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x00016F20 File Offset: 0x00015120
		// (set) Token: 0x06000562 RID: 1378 RVA: 0x00016F28 File Offset: 0x00015128
		public virtual NullValueHandling NullValueHandling
		{
			get
			{
				return this._nullValueHandling;
			}
			set
			{
				if (value < NullValueHandling.Include || value > NullValueHandling.Ignore)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._nullValueHandling = value;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x00016F44 File Offset: 0x00015144
		// (set) Token: 0x06000564 RID: 1380 RVA: 0x00016F4C File Offset: 0x0001514C
		public virtual DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._defaultValueHandling;
			}
			set
			{
				if (value < DefaultValueHandling.Include || value > DefaultValueHandling.IgnoreAndPopulate)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._defaultValueHandling = value;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x00016F68 File Offset: 0x00015168
		// (set) Token: 0x06000566 RID: 1382 RVA: 0x00016F70 File Offset: 0x00015170
		public virtual ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._objectCreationHandling;
			}
			set
			{
				if (value < ObjectCreationHandling.Auto || value > ObjectCreationHandling.Replace)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._objectCreationHandling = value;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x00016F8C File Offset: 0x0001518C
		// (set) Token: 0x06000568 RID: 1384 RVA: 0x00016F94 File Offset: 0x00015194
		public virtual ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._constructorHandling;
			}
			set
			{
				if (value < ConstructorHandling.Default || value > ConstructorHandling.AllowNonPublicDefaultConstructor)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._constructorHandling = value;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x00016FB0 File Offset: 0x000151B0
		public virtual JsonConverterCollection Converters
		{
			get
			{
				if (this._converters == null)
				{
					this._converters = new JsonConverterCollection();
				}
				return this._converters;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x00016FCB File Offset: 0x000151CB
		// (set) Token: 0x0600056B RID: 1387 RVA: 0x00016FE6 File Offset: 0x000151E6
		public virtual IContractResolver ContractResolver
		{
			get
			{
				if (this._contractResolver == null)
				{
					this._contractResolver = DefaultContractResolver.Instance;
				}
				return this._contractResolver;
			}
			set
			{
				this._contractResolver = value;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x00016FEF File Offset: 0x000151EF
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x00016FF7 File Offset: 0x000151F7
		public virtual StreamingContext Context
		{
			get
			{
				return this._context;
			}
			set
			{
				this._context = value;
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00017000 File Offset: 0x00015200
		public JsonSerializer()
		{
			this._referenceLoopHandling = ReferenceLoopHandling.Error;
			this._missingMemberHandling = MissingMemberHandling.Ignore;
			this._nullValueHandling = NullValueHandling.Include;
			this._defaultValueHandling = DefaultValueHandling.Include;
			this._objectCreationHandling = ObjectCreationHandling.Auto;
			this._preserveReferencesHandling = PreserveReferencesHandling.None;
			this._constructorHandling = ConstructorHandling.Default;
			this._typeNameHandling = TypeNameHandling.None;
			this._context = JsonSerializerSettings.DefaultContext;
			this._binder = DefaultSerializationBinder.Instance;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00017064 File Offset: 0x00015264
		public static JsonSerializer Create(JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = new JsonSerializer();
			if (settings != null)
			{
				if (!CollectionUtils.IsNullOrEmpty<JsonConverter>(settings.Converters))
				{
					jsonSerializer.Converters.AddRange(settings.Converters);
				}
				jsonSerializer.TypeNameHandling = settings.TypeNameHandling;
				jsonSerializer.TypeNameAssemblyFormat = settings.TypeNameAssemblyFormat;
				jsonSerializer.PreserveReferencesHandling = settings.PreserveReferencesHandling;
				jsonSerializer.ReferenceLoopHandling = settings.ReferenceLoopHandling;
				jsonSerializer.MissingMemberHandling = settings.MissingMemberHandling;
				jsonSerializer.ObjectCreationHandling = settings.ObjectCreationHandling;
				jsonSerializer.NullValueHandling = settings.NullValueHandling;
				jsonSerializer.DefaultValueHandling = settings.DefaultValueHandling;
				jsonSerializer.ConstructorHandling = settings.ConstructorHandling;
				jsonSerializer.Context = settings.Context;
				if (settings.Error != null)
				{
					jsonSerializer.Error += settings.Error;
				}
				if (settings.ContractResolver != null)
				{
					jsonSerializer.ContractResolver = settings.ContractResolver;
				}
				if (settings.ReferenceResolver != null)
				{
					jsonSerializer.ReferenceResolver = settings.ReferenceResolver;
				}
				if (settings.Binder != null)
				{
					jsonSerializer.Binder = settings.Binder;
				}
			}
			return jsonSerializer;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00017164 File Offset: 0x00015364
		public void Populate(TextReader reader, object target)
		{
			this.Populate(new JsonTextReader(reader), target);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00017173 File Offset: 0x00015373
		public void Populate(JsonReader reader, object target)
		{
			this.PopulateInternal(reader, target);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00017180 File Offset: 0x00015380
		internal virtual void PopulateInternal(JsonReader reader, object target)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(target, "target");
			JsonSerializerInternalReader jsonSerializerInternalReader = new JsonSerializerInternalReader(this);
			jsonSerializerInternalReader.Populate(reader, target);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x000171B2 File Offset: 0x000153B2
		public object Deserialize(JsonReader reader)
		{
			return this.Deserialize(reader, null);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x000171BC File Offset: 0x000153BC
		public object Deserialize(TextReader reader, Type objectType)
		{
			return this.Deserialize(new JsonTextReader(reader), objectType);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x000171CB File Offset: 0x000153CB
		public T Deserialize<T>(JsonReader reader)
		{
			return (T)((object)this.Deserialize(reader, typeof(T)));
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x000171E3 File Offset: 0x000153E3
		public object Deserialize(JsonReader reader, Type objectType)
		{
			return this.DeserializeInternal(reader, objectType);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x000171F0 File Offset: 0x000153F0
		internal virtual object DeserializeInternal(JsonReader reader, Type objectType)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			JsonSerializerInternalReader jsonSerializerInternalReader = new JsonSerializerInternalReader(this);
			return jsonSerializerInternalReader.Deserialize(reader, objectType);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00017217 File Offset: 0x00015417
		public void Serialize(TextWriter textWriter, object value)
		{
			this.Serialize(new JsonTextWriter(textWriter), value);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00017226 File Offset: 0x00015426
		public void Serialize(JsonWriter jsonWriter, object value)
		{
			this.SerializeInternal(jsonWriter, value);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00017230 File Offset: 0x00015430
		internal virtual void SerializeInternal(JsonWriter jsonWriter, object value)
		{
			ValidationUtils.ArgumentNotNull(jsonWriter, "jsonWriter");
			JsonSerializerInternalWriter jsonSerializerInternalWriter = new JsonSerializerInternalWriter(this);
			jsonSerializerInternalWriter.Serialize(jsonWriter, value);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00017257 File Offset: 0x00015457
		internal JsonConverter GetMatchingConverter(Type type)
		{
			return JsonSerializer.GetMatchingConverter(this._converters, type);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00017268 File Offset: 0x00015468
		internal static JsonConverter GetMatchingConverter(IList<JsonConverter> converters, Type objectType)
		{
			ValidationUtils.ArgumentNotNull(objectType, "objectType");
			if (converters != null)
			{
				for (int i = 0; i < converters.Count; i++)
				{
					JsonConverter jsonConverter = converters[i];
					if (jsonConverter.CanConvert(objectType))
					{
						return jsonConverter;
					}
				}
			}
			return null;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x000172A8 File Offset: 0x000154A8
		internal void OnError(ErrorEventArgs e)
		{
			EventHandler<ErrorEventArgs> error = this.Error;
			if (error != null)
			{
				error.Invoke(this, e);
			}
		}

		// Token: 0x0400014D RID: 333
		private TypeNameHandling _typeNameHandling;

		// Token: 0x0400014E RID: 334
		private FormatterAssemblyStyle _typeNameAssemblyFormat;

		// Token: 0x0400014F RID: 335
		private PreserveReferencesHandling _preserveReferencesHandling;

		// Token: 0x04000150 RID: 336
		private ReferenceLoopHandling _referenceLoopHandling;

		// Token: 0x04000151 RID: 337
		private MissingMemberHandling _missingMemberHandling;

		// Token: 0x04000152 RID: 338
		private ObjectCreationHandling _objectCreationHandling;

		// Token: 0x04000153 RID: 339
		private NullValueHandling _nullValueHandling;

		// Token: 0x04000154 RID: 340
		private DefaultValueHandling _defaultValueHandling;

		// Token: 0x04000155 RID: 341
		private ConstructorHandling _constructorHandling;

		// Token: 0x04000156 RID: 342
		private JsonConverterCollection _converters;

		// Token: 0x04000157 RID: 343
		private IContractResolver _contractResolver;

		// Token: 0x04000158 RID: 344
		private IReferenceResolver _referenceResolver;

		// Token: 0x04000159 RID: 345
		private SerializationBinder _binder;

		// Token: 0x0400015A RID: 346
		private StreamingContext _context;
	}
}
