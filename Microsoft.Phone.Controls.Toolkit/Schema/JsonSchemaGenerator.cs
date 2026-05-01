using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000095 RID: 149
	public class JsonSchemaGenerator
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0001A6A4 File Offset: 0x000188A4
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x0001A6AC File Offset: 0x000188AC
		public UndefinedSchemaIdHandling UndefinedSchemaIdHandling { get; set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0001A6B5 File Offset: 0x000188B5
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x0001A6CB File Offset: 0x000188CB
		public IContractResolver ContractResolver
		{
			get
			{
				if (this._contractResolver == null)
				{
					return DefaultContractResolver.Instance;
				}
				return this._contractResolver;
			}
			set
			{
				this._contractResolver = value;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001A6D4 File Offset: 0x000188D4
		private JsonSchema CurrentSchema
		{
			get
			{
				return this._currentSchema;
			}
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001A6DC File Offset: 0x000188DC
		private void Push(JsonSchemaGenerator.TypeSchema typeSchema)
		{
			this._currentSchema = typeSchema.Schema;
			this._stack.Add(typeSchema);
			this._resolver.LoadedSchemas.Add(typeSchema.Schema);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0001A70C File Offset: 0x0001890C
		private JsonSchemaGenerator.TypeSchema Pop()
		{
			JsonSchemaGenerator.TypeSchema result = this._stack[this._stack.Count - 1];
			this._stack.RemoveAt(this._stack.Count - 1);
			JsonSchemaGenerator.TypeSchema typeSchema = Enumerable.LastOrDefault<JsonSchemaGenerator.TypeSchema>(this._stack);
			if (typeSchema != null)
			{
				this._currentSchema = typeSchema.Schema;
			}
			else
			{
				this._currentSchema = null;
			}
			return result;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0001A76F File Offset: 0x0001896F
		public JsonSchema Generate(Type type)
		{
			return this.Generate(type, new JsonSchemaResolver(), false);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0001A77E File Offset: 0x0001897E
		public JsonSchema Generate(Type type, JsonSchemaResolver resolver)
		{
			return this.Generate(type, resolver, false);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0001A789 File Offset: 0x00018989
		public JsonSchema Generate(Type type, bool rootSchemaNullable)
		{
			return this.Generate(type, new JsonSchemaResolver(), rootSchemaNullable);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0001A798 File Offset: 0x00018998
		public JsonSchema Generate(Type type, JsonSchemaResolver resolver, bool rootSchemaNullable)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			this._resolver = resolver;
			return this.GenerateInternal(type, (!rootSchemaNullable) ? Required.Always : Required.Default, false);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001A7C8 File Offset: 0x000189C8
		private string GetTitle(Type type)
		{
			JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
			if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Title))
			{
				return jsonContainerAttribute.Title;
			}
			return null;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001A7F4 File Offset: 0x000189F4
		private string GetDescription(Type type)
		{
			JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
			if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Description))
			{
				return jsonContainerAttribute.Description;
			}
			DescriptionAttribute attribute = ReflectionUtils.GetAttribute<DescriptionAttribute>(type);
			if (attribute != null)
			{
				return attribute.Description;
			}
			return null;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001A834 File Offset: 0x00018A34
		private string GetTypeId(Type type, bool explicitOnly)
		{
			JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
			if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Id))
			{
				return jsonContainerAttribute.Id;
			}
			if (explicitOnly)
			{
				return null;
			}
			switch (this.UndefinedSchemaIdHandling)
			{
			case UndefinedSchemaIdHandling.UseTypeName:
				return type.FullName;
			case UndefinedSchemaIdHandling.UseAssemblyQualifiedName:
				return type.AssemblyQualifiedName;
			default:
				return null;
			}
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0001A8A4 File Offset: 0x00018AA4
		private JsonSchema GenerateInternal(Type type, Required valueRequired, bool required)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			string typeId = this.GetTypeId(type, false);
			string typeId2 = this.GetTypeId(type, true);
			if (!string.IsNullOrEmpty(typeId))
			{
				JsonSchema schema = this._resolver.GetSchema(typeId);
				if (schema != null)
				{
					if (valueRequired != Required.Always && !JsonSchemaGenerator.HasFlag(schema.Type, JsonSchemaType.Null))
					{
						schema.Type |= JsonSchemaType.Null;
					}
					if (required && schema.Required != true)
					{
						schema.Required = new bool?(true);
					}
					return schema;
				}
			}
			if (Enumerable.Any<JsonSchemaGenerator.TypeSchema>(this._stack, (JsonSchemaGenerator.TypeSchema tc) => tc.Type == type))
			{
				throw new Exception("Unresolved circular reference for type '{0}'. Explicitly define an Id for the type using a JsonObject/JsonArray attribute or automatically generate a type Id using the UndefinedSchemaIdHandling property.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type
				}));
			}
			JsonContract jsonContract = this.ContractResolver.ResolveContract(type);
			JsonConverter jsonConverter;
			if ((jsonConverter = jsonContract.Converter) != null || (jsonConverter = jsonContract.InternalConverter) != null)
			{
				JsonSchema schema2 = jsonConverter.GetSchema();
				if (schema2 != null)
				{
					return schema2;
				}
			}
			this.Push(new JsonSchemaGenerator.TypeSchema(type, new JsonSchema()));
			if (typeId2 != null)
			{
				this.CurrentSchema.Id = typeId2;
			}
			if (required)
			{
				this.CurrentSchema.Required = new bool?(true);
			}
			this.CurrentSchema.Title = this.GetTitle(type);
			this.CurrentSchema.Description = this.GetDescription(type);
			if (jsonConverter != null)
			{
				this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
			}
			else if (jsonContract is JsonDictionaryContract)
			{
				this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
				Type type2;
				Type type3;
				ReflectionUtils.GetDictionaryKeyValueTypes(type, out type2, out type3);
				if (type2 != null && typeof(IConvertible).IsAssignableFrom(type2))
				{
					this.CurrentSchema.AdditionalProperties = this.GenerateInternal(type3, Required.Default, false);
				}
			}
			else if (jsonContract is JsonArrayContract)
			{
				this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Array, valueRequired));
				this.CurrentSchema.Id = this.GetTypeId(type, false);
				JsonArrayAttribute jsonArrayAttribute = JsonTypeReflector.GetJsonContainerAttribute(type) as JsonArrayAttribute;
				bool flag = jsonArrayAttribute == null || jsonArrayAttribute.AllowNullItems;
				Type collectionItemType = ReflectionUtils.GetCollectionItemType(type);
				if (collectionItemType != null)
				{
					this.CurrentSchema.Items = new List<JsonSchema>();
					this.CurrentSchema.Items.Add(this.GenerateInternal(collectionItemType, (!flag) ? Required.Always : Required.Default, false));
				}
			}
			else
			{
				if (jsonContract is JsonPrimitiveContract)
				{
					this.CurrentSchema.Type = new JsonSchemaType?(this.GetJsonSchemaType(type, valueRequired));
					if (!(this.CurrentSchema.Type == JsonSchemaType.Integer) || !type.IsEnum || type.IsDefined(typeof(FlagsAttribute), true))
					{
						goto IL_4BF;
					}
					this.CurrentSchema.Enum = new List<JToken>();
					this.CurrentSchema.Options = new Dictionary<JToken, string>();
					EnumValues<long> namesAndValues = EnumUtils.GetNamesAndValues<long>(type);
					using (IEnumerator<EnumValue<long>> enumerator = namesAndValues.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							EnumValue<long> enumValue = enumerator.Current;
							JToken jtoken = JToken.FromObject(enumValue.Value);
							this.CurrentSchema.Enum.Add(jtoken);
							this.CurrentSchema.Options.Add(jtoken, enumValue.Name);
						}
						goto IL_4BF;
					}
				}
				if (jsonContract is JsonObjectContract)
				{
					this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
					this.CurrentSchema.Id = this.GetTypeId(type, false);
					this.GenerateObjectSchema(type, (JsonObjectContract)jsonContract);
				}
				else if (jsonContract is JsonStringContract)
				{
					JsonSchemaType jsonSchemaType = (!ReflectionUtils.IsNullable(jsonContract.UnderlyingType)) ? JsonSchemaType.String : this.AddNullType(JsonSchemaType.String, valueRequired);
					this.CurrentSchema.Type = new JsonSchemaType?(jsonSchemaType);
				}
				else
				{
					if (!(jsonContract is JsonLinqContract))
					{
						throw new Exception("Unexpected contract type: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							jsonContract
						}));
					}
					this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
				}
			}
			IL_4BF:
			return this.Pop().Schema;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0001AD8C File Offset: 0x00018F8C
		private JsonSchemaType AddNullType(JsonSchemaType type, Required valueRequired)
		{
			if (valueRequired != Required.Always)
			{
				return type | JsonSchemaType.Null;
			}
			return type;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001AD98 File Offset: 0x00018F98
		private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001ADA0 File Offset: 0x00018FA0
		private void GenerateObjectSchema(Type type, JsonObjectContract contract)
		{
			this.CurrentSchema.Properties = new Dictionary<string, JsonSchema>();
			foreach (JsonProperty jsonProperty in contract.Properties)
			{
				if (!jsonProperty.Ignored)
				{
					bool flag = jsonProperty.NullValueHandling == NullValueHandling.Ignore || this.HasFlag(jsonProperty.DefaultValueHandling.GetValueOrDefault(), DefaultValueHandling.Ignore) || jsonProperty.ShouldSerialize != null || jsonProperty.GetIsSpecified != null;
					JsonSchema jsonSchema = this.GenerateInternal(jsonProperty.PropertyType, jsonProperty.Required, !flag);
					if (jsonProperty.DefaultValue != null)
					{
						jsonSchema.Default = JToken.FromObject(jsonProperty.DefaultValue);
					}
					this.CurrentSchema.Properties.Add(jsonProperty.PropertyName, jsonSchema);
				}
			}
			if (type.IsSealed)
			{
				this.CurrentSchema.AllowAdditionalProperties = false;
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0001AEB0 File Offset: 0x000190B0
		internal static bool HasFlag(JsonSchemaType? value, JsonSchemaType flag)
		{
			if (value == null)
			{
				return true;
			}
			bool flag2 = (value & flag) == flag;
			return flag2 || (value == JsonSchemaType.Float && flag == JsonSchemaType.Integer);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001AF30 File Offset: 0x00019130
		private JsonSchemaType GetJsonSchemaType(Type type, Required valueRequired)
		{
			JsonSchemaType jsonSchemaType = JsonSchemaType.None;
			if (valueRequired != Required.Always && ReflectionUtils.IsNullable(type))
			{
				jsonSchemaType = JsonSchemaType.Null;
				if (ReflectionUtils.IsNullableType(type))
				{
					type = Nullable.GetUnderlyingType(type);
				}
			}
			TypeCode typeCode = Type.GetTypeCode(type);
			switch (typeCode)
			{
			case 0:
			case 1:
				return jsonSchemaType | JsonSchemaType.String;
			case 2:
				return jsonSchemaType | JsonSchemaType.Null;
			case 3:
				return jsonSchemaType | JsonSchemaType.Boolean;
			case 4:
				return jsonSchemaType | JsonSchemaType.String;
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
				return jsonSchemaType | JsonSchemaType.Integer;
			case 13:
			case 14:
			case 15:
				return jsonSchemaType | JsonSchemaType.Float;
			case 16:
				return jsonSchemaType | JsonSchemaType.String;
			case 18:
				return jsonSchemaType | JsonSchemaType.String;
			}
			throw new Exception("Unexpected type code '{0}' for type '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeCode,
				type
			}));
		}

		// Token: 0x040001F4 RID: 500
		private IContractResolver _contractResolver;

		// Token: 0x040001F5 RID: 501
		private JsonSchemaResolver _resolver;

		// Token: 0x040001F6 RID: 502
		private IList<JsonSchemaGenerator.TypeSchema> _stack = new List<JsonSchemaGenerator.TypeSchema>();

		// Token: 0x040001F7 RID: 503
		private JsonSchema _currentSchema;

		// Token: 0x02000096 RID: 150
		private class TypeSchema
		{
			// Token: 0x1700015C RID: 348
			// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0001B018 File Offset: 0x00019218
			// (set) Token: 0x060006F6 RID: 1782 RVA: 0x0001B020 File Offset: 0x00019220
			public Type Type { get; private set; }

			// Token: 0x1700015D RID: 349
			// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0001B029 File Offset: 0x00019229
			// (set) Token: 0x060006F8 RID: 1784 RVA: 0x0001B031 File Offset: 0x00019231
			public JsonSchema Schema { get; private set; }

			// Token: 0x060006F9 RID: 1785 RVA: 0x0001B03A File Offset: 0x0001923A
			public TypeSchema(Type type, JsonSchema schema)
			{
				ValidationUtils.ArgumentNotNull(type, "type");
				ValidationUtils.ArgumentNotNull(schema, "schema");
				this.Type = type;
				this.Schema = schema;
			}
		}
	}
}
