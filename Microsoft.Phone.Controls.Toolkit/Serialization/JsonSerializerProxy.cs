using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200007F RID: 127
	internal class JsonSerializerProxy : JsonSerializer
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600062B RID: 1579 RVA: 0x0001885E File Offset: 0x00016A5E
		// (remove) Token: 0x0600062C RID: 1580 RVA: 0x0001886C File Offset: 0x00016A6C
		public override event EventHandler<ErrorEventArgs> Error
		{
			add
			{
				this._serializer.Error += value;
			}
			remove
			{
				this._serializer.Error -= value;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x0001887A File Offset: 0x00016A7A
		// (set) Token: 0x0600062E RID: 1582 RVA: 0x00018887 File Offset: 0x00016A87
		public override IReferenceResolver ReferenceResolver
		{
			get
			{
				return this._serializer.ReferenceResolver;
			}
			set
			{
				this._serializer.ReferenceResolver = value;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x00018895 File Offset: 0x00016A95
		public override JsonConverterCollection Converters
		{
			get
			{
				return this._serializer.Converters;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x000188A2 File Offset: 0x00016AA2
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x000188AF File Offset: 0x00016AAF
		public override DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._serializer.DefaultValueHandling;
			}
			set
			{
				this._serializer.DefaultValueHandling = value;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x000188BD File Offset: 0x00016ABD
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x000188CA File Offset: 0x00016ACA
		public override IContractResolver ContractResolver
		{
			get
			{
				return this._serializer.ContractResolver;
			}
			set
			{
				this._serializer.ContractResolver = value;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x000188D8 File Offset: 0x00016AD8
		// (set) Token: 0x06000635 RID: 1589 RVA: 0x000188E5 File Offset: 0x00016AE5
		public override MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._serializer.MissingMemberHandling;
			}
			set
			{
				this._serializer.MissingMemberHandling = value;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x000188F3 File Offset: 0x00016AF3
		// (set) Token: 0x06000637 RID: 1591 RVA: 0x00018900 File Offset: 0x00016B00
		public override NullValueHandling NullValueHandling
		{
			get
			{
				return this._serializer.NullValueHandling;
			}
			set
			{
				this._serializer.NullValueHandling = value;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x0001890E File Offset: 0x00016B0E
		// (set) Token: 0x06000639 RID: 1593 RVA: 0x0001891B File Offset: 0x00016B1B
		public override ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._serializer.ObjectCreationHandling;
			}
			set
			{
				this._serializer.ObjectCreationHandling = value;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x00018929 File Offset: 0x00016B29
		// (set) Token: 0x0600063B RID: 1595 RVA: 0x00018936 File Offset: 0x00016B36
		public override ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._serializer.ReferenceLoopHandling;
			}
			set
			{
				this._serializer.ReferenceLoopHandling = value;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x00018944 File Offset: 0x00016B44
		// (set) Token: 0x0600063D RID: 1597 RVA: 0x00018951 File Offset: 0x00016B51
		public override PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._serializer.PreserveReferencesHandling;
			}
			set
			{
				this._serializer.PreserveReferencesHandling = value;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x0001895F File Offset: 0x00016B5F
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x0001896C File Offset: 0x00016B6C
		public override TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._serializer.TypeNameHandling;
			}
			set
			{
				this._serializer.TypeNameHandling = value;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x0001897A File Offset: 0x00016B7A
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x00018987 File Offset: 0x00016B87
		public override FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return this._serializer.TypeNameAssemblyFormat;
			}
			set
			{
				this._serializer.TypeNameAssemblyFormat = value;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x00018995 File Offset: 0x00016B95
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x000189A2 File Offset: 0x00016BA2
		public override ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._serializer.ConstructorHandling;
			}
			set
			{
				this._serializer.ConstructorHandling = value;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x000189B0 File Offset: 0x00016BB0
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x000189BD File Offset: 0x00016BBD
		public override SerializationBinder Binder
		{
			get
			{
				return this._serializer.Binder;
			}
			set
			{
				this._serializer.Binder = value;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x000189CB File Offset: 0x00016BCB
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x000189D8 File Offset: 0x00016BD8
		public override StreamingContext Context
		{
			get
			{
				return this._serializer.Context;
			}
			set
			{
				this._serializer.Context = value;
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x000189E6 File Offset: 0x00016BE6
		internal JsonSerializerInternalBase GetInternalSerializer()
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader;
			}
			return this._serializerWriter;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x000189FD File Offset: 0x00016BFD
		public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
		{
			ValidationUtils.ArgumentNotNull(serializerReader, "serializerReader");
			this._serializerReader = serializerReader;
			this._serializer = serializerReader.Serializer;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00018A23 File Offset: 0x00016C23
		public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
		{
			ValidationUtils.ArgumentNotNull(serializerWriter, "serializerWriter");
			this._serializerWriter = serializerWriter;
			this._serializer = serializerWriter.Serializer;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00018A49 File Offset: 0x00016C49
		internal override object DeserializeInternal(JsonReader reader, Type objectType)
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader.Deserialize(reader, objectType);
			}
			return this._serializer.Deserialize(reader, objectType);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00018A6E File Offset: 0x00016C6E
		internal override void PopulateInternal(JsonReader reader, object target)
		{
			if (this._serializerReader != null)
			{
				this._serializerReader.Populate(reader, target);
				return;
			}
			this._serializer.Populate(reader, target);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00018A93 File Offset: 0x00016C93
		internal override void SerializeInternal(JsonWriter jsonWriter, object value)
		{
			if (this._serializerWriter != null)
			{
				this._serializerWriter.Serialize(jsonWriter, value);
				return;
			}
			this._serializer.Serialize(jsonWriter, value);
		}

		// Token: 0x040001AA RID: 426
		private readonly JsonSerializerInternalReader _serializerReader;

		// Token: 0x040001AB RID: 427
		private readonly JsonSerializerInternalWriter _serializerWriter;

		// Token: 0x040001AC RID: 428
		private readonly JsonSerializer _serializer;
	}
}
