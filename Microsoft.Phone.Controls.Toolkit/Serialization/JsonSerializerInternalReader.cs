using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000062 RID: 98
	internal class JsonSerializerInternalReader : JsonSerializerInternalBase
	{
		// Token: 0x060004AD RID: 1197 RVA: 0x00013E1C File Offset: 0x0001201C
		public JsonSerializerInternalReader(JsonSerializer serializer) : base(serializer)
		{
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00013E28 File Offset: 0x00012028
		public void Populate(JsonReader reader, object target)
		{
			ValidationUtils.ArgumentNotNull(target, "target");
			Type type = target.GetType();
			JsonContract jsonContract = base.Serializer.ContractResolver.ResolveContract(type);
			if (reader.TokenType == JsonToken.None)
			{
				reader.Read();
			}
			if (reader.TokenType == JsonToken.StartArray)
			{
				if (jsonContract is JsonArrayContract)
				{
					this.PopulateList(CollectionUtils.CreateCollectionWrapper(target), reader, null, (JsonArrayContract)jsonContract);
					return;
				}
				throw new JsonSerializationException("Cannot populate JSON array onto type '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type
				}));
			}
			else
			{
				if (reader.TokenType != JsonToken.StartObject)
				{
					throw new JsonSerializationException("Unexpected initial token '{0}' when populating object. Expected JSON object or array.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						reader.TokenType
					}));
				}
				this.CheckedRead(reader);
				string id = null;
				if (reader.TokenType == JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$id", 4))
				{
					this.CheckedRead(reader);
					id = ((reader.Value != null) ? reader.Value.ToString() : null);
					this.CheckedRead(reader);
				}
				if (jsonContract is JsonDictionaryContract)
				{
					this.PopulateDictionary(CollectionUtils.CreateDictionaryWrapper(target), reader, (JsonDictionaryContract)jsonContract, id);
					return;
				}
				if (jsonContract is JsonObjectContract)
				{
					this.PopulateObject(target, reader, (JsonObjectContract)jsonContract, id);
					return;
				}
				throw new JsonSerializationException("Cannot populate JSON object onto type '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type
				}));
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00013F93 File Offset: 0x00012193
		private JsonContract GetContractSafe(Type type)
		{
			if (type == null)
			{
				return null;
			}
			return base.Serializer.ContractResolver.ResolveContract(type);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00013FAB File Offset: 0x000121AB
		private JsonContract GetContractSafe(Type type, object value)
		{
			if (value == null)
			{
				return this.GetContractSafe(type);
			}
			return base.Serializer.ContractResolver.ResolveContract(value.GetType());
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00013FCE File Offset: 0x000121CE
		public object Deserialize(JsonReader reader, Type objectType)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (reader.TokenType == JsonToken.None && !this.ReadForType(reader, objectType, null))
			{
				return null;
			}
			return this.CreateValueNonProperty(reader, objectType, this.GetContractSafe(objectType));
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00014002 File Offset: 0x00012202
		private JsonSerializerProxy GetInternalSerializer()
		{
			if (this._internalSerializer == null)
			{
				this._internalSerializer = new JsonSerializerProxy(this);
			}
			return this._internalSerializer;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00014020 File Offset: 0x00012220
		private JToken CreateJToken(JsonReader reader, JsonContract contract)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (contract != null && contract.UnderlyingType == typeof(JRaw))
			{
				return JRaw.Create(reader);
			}
			JToken token;
			using (JTokenWriter jtokenWriter = new JTokenWriter())
			{
				jtokenWriter.WriteToken(reader);
				token = jtokenWriter.Token;
			}
			return token;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00014088 File Offset: 0x00012288
		private JToken CreateJObject(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			JToken token;
			using (JTokenWriter jtokenWriter = new JTokenWriter())
			{
				jtokenWriter.WriteStartObject();
				if (reader.TokenType == JsonToken.PropertyName)
				{
					jtokenWriter.WriteToken(reader, reader.Depth - 1);
				}
				else
				{
					jtokenWriter.WriteEndObject();
				}
				token = jtokenWriter.Token;
			}
			return token;
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000140F0 File Offset: 0x000122F0
		private object CreateValueProperty(JsonReader reader, JsonProperty property, object target, bool gottenCurrentValue, object currentValue)
		{
			JsonContract contractSafe = this.GetContractSafe(property.PropertyType, currentValue);
			Type propertyType = property.PropertyType;
			JsonConverter converter = this.GetConverter(contractSafe, property.MemberConverter);
			if (converter != null && converter.CanRead)
			{
				if (!gottenCurrentValue && target != null && property.Readable)
				{
					currentValue = property.ValueProvider.GetValue(target);
				}
				return converter.ReadJson(reader, propertyType, currentValue, this.GetInternalSerializer());
			}
			return this.CreateValueInternal(reader, propertyType, contractSafe, property, currentValue);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00014168 File Offset: 0x00012368
		private object CreateValueNonProperty(JsonReader reader, Type objectType, JsonContract contract)
		{
			JsonConverter converter = this.GetConverter(contract, null);
			if (converter != null && converter.CanRead)
			{
				return converter.ReadJson(reader, objectType, null, this.GetInternalSerializer());
			}
			return this.CreateValueInternal(reader, objectType, contract, null, null);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x000141A4 File Offset: 0x000123A4
		private object CreateValueInternal(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, object existingValue)
		{
			if (contract is JsonLinqContract)
			{
				return this.CreateJToken(reader, contract);
			}
			for (;;)
			{
				switch (reader.TokenType)
				{
				case JsonToken.StartObject:
					goto IL_69;
				case JsonToken.StartArray:
					goto IL_77;
				case JsonToken.StartConstructor:
				case JsonToken.EndConstructor:
					goto IL_E9;
				case JsonToken.Comment:
					if (!reader.Read())
					{
						goto Block_8;
					}
					continue;
				case JsonToken.Raw:
					goto IL_11D;
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.Boolean:
				case JsonToken.Date:
				case JsonToken.Bytes:
					goto IL_86;
				case JsonToken.String:
					goto IL_99;
				case JsonToken.Null:
				case JsonToken.Undefined:
					goto IL_F7;
				}
				break;
			}
			goto IL_12E;
			IL_69:
			return this.CreateObject(reader, objectType, contract, member, existingValue);
			IL_77:
			return this.CreateList(reader, objectType, contract, member, existingValue, null);
			IL_86:
			return this.EnsureType(reader.Value, CultureInfo.InvariantCulture, objectType);
			IL_99:
			if (string.IsNullOrEmpty((string)reader.Value) && objectType != null && ReflectionUtils.IsNullableType(objectType))
			{
				return null;
			}
			if (objectType == typeof(byte[]))
			{
				return Convert.FromBase64String((string)reader.Value);
			}
			return this.EnsureType(reader.Value, CultureInfo.InvariantCulture, objectType);
			IL_E9:
			return reader.Value.ToString();
			IL_F7:
			if (objectType == typeof(DBNull))
			{
				return DBNull.Value;
			}
			return this.EnsureType(reader.Value, CultureInfo.InvariantCulture, objectType);
			IL_11D:
			return new JRaw((string)reader.Value);
			IL_12E:
			throw new JsonSerializationException("Unexpected token while deserializing object: " + reader.TokenType);
			Block_8:
			throw new JsonSerializationException("Unexpected end when deserializing object.");
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00014310 File Offset: 0x00012510
		private JsonConverter GetConverter(JsonContract contract, JsonConverter memberConverter)
		{
			JsonConverter result = null;
			if (memberConverter != null)
			{
				result = memberConverter;
			}
			else if (contract != null)
			{
				JsonConverter matchingConverter;
				if (contract.Converter != null)
				{
					result = contract.Converter;
				}
				else if ((matchingConverter = base.Serializer.GetMatchingConverter(contract.UnderlyingType)) != null)
				{
					result = matchingConverter;
				}
				else if (contract.InternalConverter != null)
				{
					result = contract.InternalConverter;
				}
			}
			return result;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00014364 File Offset: 0x00012564
		private object CreateObject(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, object existingValue)
		{
			this.CheckedRead(reader);
			string text = null;
			if (reader.TokenType == JsonToken.PropertyName)
			{
				string text3;
				string text4;
				Type type;
				for (;;)
				{
					string text2 = reader.Value.ToString();
					bool flag;
					if (string.Equals(text2, "$ref", 4))
					{
						this.CheckedRead(reader);
						if (reader.TokenType != JsonToken.String && reader.TokenType != JsonToken.Null)
						{
							break;
						}
						text3 = ((reader.Value != null) ? reader.Value.ToString() : null);
						this.CheckedRead(reader);
						if (text3 != null)
						{
							goto Block_5;
						}
						flag = true;
					}
					else if (string.Equals(text2, "$type", 4))
					{
						this.CheckedRead(reader);
						text4 = reader.Value.ToString();
						this.CheckedRead(reader);
						if ((((member != null) ? member.TypeNameHandling : default(TypeNameHandling?)) ?? base.Serializer.TypeNameHandling) != TypeNameHandling.None)
						{
							string typeName;
							string assemblyName;
							ReflectionUtils.SplitFullyQualifiedTypeName(text4, out typeName, out assemblyName);
							try
							{
								type = base.Serializer.Binder.BindToType(assemblyName, typeName);
							}
							catch (Exception innerException)
							{
								throw new JsonSerializationException("Error resolving type specified in JSON '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
								{
									text4
								}), innerException);
							}
							if (type == null)
							{
								goto Block_12;
							}
							if (objectType != null && !objectType.IsAssignableFrom(type))
							{
								goto Block_14;
							}
							objectType = type;
							contract = this.GetContractSafe(type);
						}
						flag = true;
					}
					else if (string.Equals(text2, "$id", 4))
					{
						this.CheckedRead(reader);
						text = ((reader.Value != null) ? reader.Value.ToString() : null);
						this.CheckedRead(reader);
						flag = true;
					}
					else
					{
						if (string.Equals(text2, "$values", 4))
						{
							goto Block_17;
						}
						flag = false;
					}
					if (!flag || reader.TokenType != JsonToken.PropertyName)
					{
						goto IL_287;
					}
				}
				throw new JsonSerializationException("JSON reference {0} property must have a string or null value.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					"$ref"
				}));
				Block_5:
				if (reader.TokenType == JsonToken.PropertyName)
				{
					throw new JsonSerializationException("Additional content found in JSON reference object. A JSON reference object should only have a {0} property.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						"$ref"
					}));
				}
				return base.Serializer.ReferenceResolver.ResolveReference(this, text3);
				Block_12:
				throw new JsonSerializationException("Type specified in JSON '{0}' was not resolved.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					text4
				}));
				Block_14:
				throw new JsonSerializationException("Type specified in JSON '{0}' is not compatible with '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type.AssemblyQualifiedName,
					objectType.AssemblyQualifiedName
				}));
				Block_17:
				this.CheckedRead(reader);
				object result = this.CreateList(reader, objectType, contract, member, existingValue, text);
				this.CheckedRead(reader);
				return result;
			}
			IL_287:
			if (!this.HasDefinedType(objectType))
			{
				return this.CreateJObject(reader);
			}
			if (contract == null)
			{
				throw new JsonSerializationException("Could not resolve type '{0}' to a JsonContract.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					objectType
				}));
			}
			JsonDictionaryContract jsonDictionaryContract = contract as JsonDictionaryContract;
			if (jsonDictionaryContract != null)
			{
				if (existingValue == null)
				{
					return this.CreateAndPopulateDictionary(reader, jsonDictionaryContract, text);
				}
				return this.PopulateDictionary(jsonDictionaryContract.CreateWrapper(existingValue), reader, jsonDictionaryContract, text);
			}
			else
			{
				JsonObjectContract jsonObjectContract = contract as JsonObjectContract;
				if (jsonObjectContract != null)
				{
					if (existingValue == null)
					{
						return this.CreateAndPopulateObject(reader, jsonObjectContract, text);
					}
					return this.PopulateObject(existingValue, reader, jsonObjectContract, text);
				}
				else
				{
					JsonPrimitiveContract jsonPrimitiveContract = contract as JsonPrimitiveContract;
					if (jsonPrimitiveContract != null && reader.TokenType == JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$value", 4))
					{
						this.CheckedRead(reader);
						object result2 = this.CreateValueInternal(reader, objectType, jsonPrimitiveContract, member, existingValue);
						this.CheckedRead(reader);
						return result2;
					}
					throw new JsonSerializationException("Cannot deserialize JSON object into type '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						objectType
					}));
				}
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00014708 File Offset: 0x00012908
		private JsonArrayContract EnsureArrayContract(Type objectType, JsonContract contract)
		{
			if (contract == null)
			{
				throw new JsonSerializationException("Could not resolve type '{0}' to a JsonContract.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					objectType
				}));
			}
			JsonArrayContract jsonArrayContract = contract as JsonArrayContract;
			if (jsonArrayContract == null)
			{
				throw new JsonSerializationException("Cannot deserialize JSON array into type '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					objectType
				}));
			}
			return jsonArrayContract;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00014765 File Offset: 0x00012965
		private void CheckedRead(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw new JsonSerializationException("Unexpected end when deserializing object.");
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001477C File Offset: 0x0001297C
		private object CreateList(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, object existingValue, string reference)
		{
			object result;
			if (this.HasDefinedType(objectType))
			{
				JsonArrayContract jsonArrayContract = this.EnsureArrayContract(objectType, contract);
				if (existingValue == null)
				{
					result = this.CreateAndPopulateList(reader, reference, jsonArrayContract);
				}
				else
				{
					result = this.PopulateList(jsonArrayContract.CreateWrapper(existingValue), reader, reference, jsonArrayContract);
				}
			}
			else
			{
				result = this.CreateJToken(reader, contract);
			}
			return result;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000147CB File Offset: 0x000129CB
		private bool HasDefinedType(Type type)
		{
			return type != null && type != typeof(object) && !typeof(JToken).IsAssignableFrom(type);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x000147F4 File Offset: 0x000129F4
		private object EnsureType(object value, CultureInfo culture, Type targetType)
		{
			if (targetType == null)
			{
				return value;
			}
			Type objectType = ReflectionUtils.GetObjectType(value);
			if (objectType != targetType)
			{
				try
				{
					return ConvertUtils.ConvertOrCast(value, culture, targetType);
				}
				catch (Exception innerException)
				{
					throw new JsonSerializationException("Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						this.FormatValueForPrint(value),
						targetType
					}), innerException);
				}
				return value;
			}
			return value;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001485C File Offset: 0x00012A5C
		private string FormatValueForPrint(object value)
		{
			if (value == null)
			{
				return "{null}";
			}
			if (value is string)
			{
				return "\"" + value + "\"";
			}
			return value.ToString();
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00014888 File Offset: 0x00012A88
		private void SetPropertyValue(JsonProperty property, JsonReader reader, object target)
		{
			if (property.Ignored)
			{
				reader.Skip();
				return;
			}
			object obj = null;
			bool flag = false;
			bool gottenCurrentValue = false;
			ObjectCreationHandling valueOrDefault = property.ObjectCreationHandling.GetValueOrDefault(base.Serializer.ObjectCreationHandling);
			if ((valueOrDefault == ObjectCreationHandling.Auto || valueOrDefault == ObjectCreationHandling.Reuse) && (reader.TokenType == JsonToken.StartArray || reader.TokenType == JsonToken.StartObject) && property.Readable)
			{
				obj = property.ValueProvider.GetValue(target);
				gottenCurrentValue = true;
				flag = (obj != null && !property.PropertyType.IsArray && !ReflectionUtils.InheritsGenericDefinition(property.PropertyType, typeof(ReadOnlyCollection)) && !property.PropertyType.IsValueType);
			}
			if (!property.Writable && !flag)
			{
				reader.Skip();
				return;
			}
			if (property.NullValueHandling.GetValueOrDefault(base.Serializer.NullValueHandling) == NullValueHandling.Ignore && reader.TokenType == JsonToken.Null)
			{
				reader.Skip();
				return;
			}
			if (this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling), DefaultValueHandling.Ignore) && JsonReader.IsPrimitiveToken(reader.TokenType) && MiscellaneousUtils.ValueEquals(reader.Value, property.DefaultValue))
			{
				reader.Skip();
				return;
			}
			object currentValue = flag ? obj : null;
			object obj2 = this.CreateValueProperty(reader, property, target, gottenCurrentValue, currentValue);
			if ((!flag || obj2 != obj) && this.ShouldSetPropertyValue(property, obj2))
			{
				property.ValueProvider.SetValue(target, obj2);
				if (property.SetIsSpecified != null)
				{
					property.SetIsSpecified.Invoke(target, true);
				}
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00014A0A File Offset: 0x00012C0A
		private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00014A14 File Offset: 0x00012C14
		private bool ShouldSetPropertyValue(JsonProperty property, object value)
		{
			return (property.NullValueHandling.GetValueOrDefault(base.Serializer.NullValueHandling) != NullValueHandling.Ignore || value != null) && (!this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling), DefaultValueHandling.Ignore) || !MiscellaneousUtils.ValueEquals(value, property.DefaultValue)) && property.Writable;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00014A80 File Offset: 0x00012C80
		private object CreateAndPopulateDictionary(JsonReader reader, JsonDictionaryContract contract, string id)
		{
			if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || base.Serializer.ConstructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
			{
				object dictionary = contract.DefaultCreator.Invoke();
				IWrappedDictionary wrappedDictionary = contract.CreateWrapper(dictionary);
				this.PopulateDictionary(wrappedDictionary, reader, contract, id);
				return wrappedDictionary.UnderlyingDictionary;
			}
			throw new JsonSerializationException("Unable to find a default constructor to use for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				contract.UnderlyingType
			}));
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00014AF8 File Offset: 0x00012CF8
		private object PopulateDictionary(IWrappedDictionary dictionary, JsonReader reader, JsonDictionaryContract contract, string id)
		{
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, id, dictionary.UnderlyingDictionary);
			}
			contract.InvokeOnDeserializing(dictionary.UnderlyingDictionary, base.Serializer.Context);
			int depth = reader.Depth;
			JsonToken tokenType;
			for (;;)
			{
				tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.PropertyName:
				{
					object obj = reader.Value;
					try
					{
						try
						{
							obj = this.EnsureType(obj, CultureInfo.InvariantCulture, contract.DictionaryKeyType);
						}
						catch (Exception innerException)
						{
							throw new JsonSerializationException("Could not convert string '{0}' to dictionary key type '{1}'. Create a TypeConverter to convert from the string to the key type object.".FormatWith(CultureInfo.InvariantCulture, new object[]
							{
								reader.Value,
								contract.DictionaryKeyType
							}), innerException);
						}
						if (!this.ReadForType(reader, contract.DictionaryValueType, null))
						{
							throw new JsonSerializationException("Unexpected end when deserializing object.");
						}
						dictionary[obj] = this.CreateValueNonProperty(reader, contract.DictionaryValueType, this.GetContractSafe(contract.DictionaryValueType));
						goto IL_144;
					}
					catch (Exception ex)
					{
						if (base.IsErrorHandled(dictionary, contract, obj, ex))
						{
							this.HandleError(reader, depth);
							goto IL_144;
						}
						throw;
					}
					goto IL_10B;
				}
				case JsonToken.Comment:
					goto IL_144;
				}
				break;
				IL_144:
				if (!reader.Read())
				{
					goto Block_5;
				}
			}
			if (tokenType != JsonToken.EndObject)
			{
				throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
			}
			IL_10B:
			contract.InvokeOnDeserialized(dictionary.UnderlyingDictionary, base.Serializer.Context);
			return dictionary.UnderlyingDictionary;
			Block_5:
			throw new JsonSerializationException("Unexpected end when deserializing object.");
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00014D68 File Offset: 0x00012F68
		private object CreateAndPopulateList(JsonReader reader, string reference, JsonArrayContract contract)
		{
			return CollectionUtils.CreateAndPopulateList(contract.CreatedType, delegate(IList l, bool isTemporaryListReference)
			{
				if (reference != null && isTemporaryListReference)
				{
					throw new JsonSerializationException("Cannot preserve reference to array or readonly list: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						contract.UnderlyingType
					}));
				}
				if (contract.OnSerializing != null && isTemporaryListReference)
				{
					throw new JsonSerializationException("Cannot call OnSerializing on an array or readonly list: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						contract.UnderlyingType
					}));
				}
				if (contract.OnError != null && isTemporaryListReference)
				{
					throw new JsonSerializationException("Cannot call OnError on an array or readonly list: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						contract.UnderlyingType
					}));
				}
				this.PopulateList(contract.CreateWrapper(l), reader, reference, contract);
			});
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00014DB4 File Offset: 0x00012FB4
		private bool ReadForTypeArrayHack(JsonReader reader, Type t)
		{
			bool result;
			try
			{
				result = this.ReadForType(reader, t, null);
			}
			catch (JsonReaderException)
			{
				if (reader.TokenType != JsonToken.EndArray)
				{
					throw;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00014DF0 File Offset: 0x00012FF0
		private object PopulateList(IWrappedCollection wrappedList, JsonReader reader, string reference, JsonArrayContract contract)
		{
			object underlyingCollection = wrappedList.UnderlyingCollection;
			if (wrappedList.IsFixedSize)
			{
				reader.Skip();
				return wrappedList.UnderlyingCollection;
			}
			if (reference != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, reference, underlyingCollection);
			}
			contract.InvokeOnDeserializing(underlyingCollection, base.Serializer.Context);
			int depth = reader.Depth;
			while (this.ReadForTypeArrayHack(reader, contract.CollectionItemType))
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					if (tokenType == JsonToken.EndArray)
					{
						contract.InvokeOnDeserialized(underlyingCollection, base.Serializer.Context);
						return wrappedList.UnderlyingCollection;
					}
					try
					{
						object obj = this.CreateValueNonProperty(reader, contract.CollectionItemType, this.GetContractSafe(contract.CollectionItemType));
						wrappedList.Add(obj);
					}
					catch (Exception ex)
					{
						if (!base.IsErrorHandled(underlyingCollection, contract, wrappedList.Count, ex))
						{
							throw;
						}
						this.HandleError(reader, depth);
					}
				}
			}
			throw new JsonSerializationException("Unexpected end when deserializing array.");
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00014EF4 File Offset: 0x000130F4
		private object CreateAndPopulateObject(JsonReader reader, JsonObjectContract contract, string id)
		{
			object obj = null;
			if (contract.UnderlyingType.IsInterface || contract.UnderlyingType.IsAbstract)
			{
				throw new JsonSerializationException("Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantated.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					contract.UnderlyingType
				}));
			}
			if (contract.OverrideConstructor != null)
			{
				if (contract.OverrideConstructor.GetParameters().Length > 0)
				{
					return this.CreateObjectFromNonDefaultConstructor(reader, contract, contract.OverrideConstructor, id);
				}
				obj = contract.OverrideConstructor.Invoke(null);
			}
			else if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || base.Serializer.ConstructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
			{
				obj = contract.DefaultCreator.Invoke();
			}
			else if (contract.ParametrizedConstructor != null)
			{
				return this.CreateObjectFromNonDefaultConstructor(reader, contract, contract.ParametrizedConstructor, id);
			}
			if (obj == null)
			{
				throw new JsonSerializationException("Unable to find a constructor to use for type {0}. A class should either have a default constructor, one constructor with arguments or a constructor marked with the JsonConstructor attribute.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					contract.UnderlyingType
				}));
			}
			this.PopulateObject(obj, reader, contract, id);
			return obj;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00015008 File Offset: 0x00013208
		private object CreateObjectFromNonDefaultConstructor(JsonReader reader, JsonObjectContract contract, ConstructorInfo constructorInfo, string id)
		{
			ValidationUtils.ArgumentNotNull(constructorInfo, "constructorInfo");
			Type underlyingType = contract.UnderlyingType;
			IDictionary<JsonProperty, object> dictionary = this.ResolvePropertyAndConstructorValues(contract, reader, underlyingType);
			IDictionary<ParameterInfo, object> dictionary2 = Enumerable.ToDictionary<ParameterInfo, ParameterInfo, object>(constructorInfo.GetParameters(), (ParameterInfo p) => p, (ParameterInfo p) => null);
			IDictionary<JsonProperty, object> dictionary3 = new Dictionary<JsonProperty, object>();
			foreach (KeyValuePair<JsonProperty, object> keyValuePair in dictionary)
			{
				ParameterInfo key = dictionary2.ForgivingCaseSensitiveFind((KeyValuePair<ParameterInfo, object> kv) => kv.Key.Name, keyValuePair.Key.UnderlyingName).Key;
				if (key != null)
				{
					dictionary2[key] = keyValuePair.Value;
				}
				else
				{
					dictionary3.Add(keyValuePair);
				}
			}
			object obj = constructorInfo.Invoke(Enumerable.ToArray<object>(dictionary2.Values));
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, id, obj);
			}
			contract.InvokeOnDeserializing(obj, base.Serializer.Context);
			foreach (KeyValuePair<JsonProperty, object> keyValuePair2 in dictionary3)
			{
				JsonProperty key2 = keyValuePair2.Key;
				object value = keyValuePair2.Value;
				if (this.ShouldSetPropertyValue(keyValuePair2.Key, keyValuePair2.Value))
				{
					key2.ValueProvider.SetValue(obj, value);
				}
				else if (!key2.Writable && value != null)
				{
					JsonContract jsonContract = base.Serializer.ContractResolver.ResolveContract(key2.PropertyType);
					if (jsonContract is JsonArrayContract)
					{
						JsonArrayContract jsonArrayContract = jsonContract as JsonArrayContract;
						object value2 = key2.ValueProvider.GetValue(obj);
						if (value2 == null)
						{
							continue;
						}
						IWrappedCollection wrappedCollection = jsonArrayContract.CreateWrapper(value2);
						IWrappedCollection wrappedCollection2 = jsonArrayContract.CreateWrapper(value);
						using (IEnumerator enumerator3 = wrappedCollection2.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								object obj2 = enumerator3.Current;
								wrappedCollection.Add(obj2);
							}
							continue;
						}
					}
					if (jsonContract is JsonDictionaryContract)
					{
						JsonDictionaryContract jsonDictionaryContract = jsonContract as JsonDictionaryContract;
						object value3 = key2.ValueProvider.GetValue(obj);
						if (value3 != null)
						{
							IWrappedDictionary wrappedDictionary = jsonDictionaryContract.CreateWrapper(value3);
							IWrappedDictionary wrappedDictionary2 = jsonDictionaryContract.CreateWrapper(value);
							foreach (object obj3 in wrappedDictionary2)
							{
								DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
								wrappedDictionary.Add(dictionaryEntry.Key, dictionaryEntry.Value);
							}
						}
					}
				}
			}
			contract.InvokeOnDeserialized(obj, base.Serializer.Context);
			return obj;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001535C File Offset: 0x0001355C
		private IDictionary<JsonProperty, object> ResolvePropertyAndConstructorValues(JsonObjectContract contract, JsonReader reader, Type objectType)
		{
			IDictionary<JsonProperty, object> dictionary = new Dictionary<JsonProperty, object>();
			bool flag = false;
			string text;
			for (;;)
			{
				JsonToken tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.PropertyName:
				{
					text = reader.Value.ToString();
					JsonProperty jsonProperty = contract.ConstructorParameters.GetClosestMatchProperty(text) ?? contract.Properties.GetClosestMatchProperty(text);
					if (jsonProperty != null)
					{
						if (!this.ReadForType(reader, jsonProperty.PropertyType, jsonProperty.Converter))
						{
							goto Block_5;
						}
						if (!jsonProperty.Ignored)
						{
							dictionary[jsonProperty] = this.CreateValueProperty(reader, jsonProperty, null, true, null);
						}
						else
						{
							reader.Skip();
						}
					}
					else
					{
						if (!reader.Read())
						{
							goto Block_7;
						}
						if (base.Serializer.MissingMemberHandling == MissingMemberHandling.Error)
						{
							goto Block_8;
						}
						reader.Skip();
					}
					break;
				}
				case JsonToken.Comment:
					break;
				default:
					if (tokenType != JsonToken.EndObject)
					{
						goto Block_2;
					}
					flag = true;
					break;
				}
				if (flag || !reader.Read())
				{
					return dictionary;
				}
			}
			Block_2:
			throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
			Block_5:
			throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				text
			}));
			Block_7:
			throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				text
			}));
			Block_8:
			throw new JsonSerializationException("Could not find member '{0}' on object of type '{1}'".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				text,
				objectType.Name
			}));
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000154C8 File Offset: 0x000136C8
		private bool ReadForType(JsonReader reader, Type t, JsonConverter propertyConverter)
		{
			bool flag = this.GetConverter(this.GetContractSafe(t), propertyConverter) != null;
			if (flag)
			{
				return reader.Read();
			}
			if (t == typeof(byte[]))
			{
				reader.ReadAsBytes();
				return true;
			}
			if (t == typeof(decimal) || t == typeof(decimal?))
			{
				reader.ReadAsDecimal();
				return true;
			}
			if (t == typeof(DateTimeOffset) || t == typeof(DateTimeOffset?))
			{
				reader.ReadAsDateTimeOffset();
				return true;
			}
			while (reader.Read())
			{
				if (reader.TokenType != JsonToken.Comment)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0001556C File Offset: 0x0001376C
		private object PopulateObject(object newObject, JsonReader reader, JsonObjectContract contract, string id)
		{
			contract.InvokeOnDeserializing(newObject, base.Serializer.Context);
			Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> dictionary = Enumerable.ToDictionary<JsonProperty, JsonProperty, JsonSerializerInternalReader.PropertyPresence>(contract.Properties, (JsonProperty m) => m, (JsonProperty m) => JsonSerializerInternalReader.PropertyPresence.None);
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, id, newObject);
			}
			int depth = reader.Depth;
			JsonToken tokenType;
			for (;;)
			{
				tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.PropertyName:
				{
					string text = reader.Value.ToString();
					try
					{
						JsonProperty closestMatchProperty = contract.Properties.GetClosestMatchProperty(text);
						if (closestMatchProperty == null)
						{
							if (base.Serializer.MissingMemberHandling == MissingMemberHandling.Error)
							{
								throw new JsonSerializationException("Could not find member '{0}' on object of type '{1}'".FormatWith(CultureInfo.InvariantCulture, new object[]
								{
									text,
									contract.UnderlyingType.Name
								}));
							}
							reader.Skip();
							goto IL_2C8;
						}
						else
						{
							if (!this.ReadForType(reader, closestMatchProperty.PropertyType, closestMatchProperty.Converter))
							{
								throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, new object[]
								{
									text
								}));
							}
							this.SetPropertyPresence(reader, closestMatchProperty, dictionary);
							this.SetPropertyValue(closestMatchProperty, reader, newObject);
							goto IL_2C8;
						}
					}
					catch (Exception ex)
					{
						if (base.IsErrorHandled(newObject, contract, text, ex))
						{
							this.HandleError(reader, depth);
							goto IL_2C8;
						}
						throw;
					}
					goto IL_176;
				}
				case JsonToken.Comment:
					goto IL_2C8;
				}
				break;
				IL_2C8:
				if (!reader.Read())
				{
					goto Block_8;
				}
			}
			if (tokenType != JsonToken.EndObject)
			{
				throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
			}
			IL_176:
			foreach (KeyValuePair<JsonProperty, JsonSerializerInternalReader.PropertyPresence> keyValuePair in dictionary)
			{
				JsonProperty key = keyValuePair.Key;
				switch (keyValuePair.Value)
				{
				case JsonSerializerInternalReader.PropertyPresence.None:
					if (key.Required == Required.AllowNull || key.Required == Required.Always)
					{
						throw new JsonSerializationException("Required property '{0}' not found in JSON.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							key.PropertyName
						}));
					}
					if (this.HasFlag(key.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling), DefaultValueHandling.Populate) && key.Writable)
					{
						key.ValueProvider.SetValue(newObject, this.EnsureType(key.DefaultValue, CultureInfo.InvariantCulture, key.PropertyType));
					}
					break;
				case JsonSerializerInternalReader.PropertyPresence.Null:
					if (key.Required == Required.Always)
					{
						throw new JsonSerializationException("Required property '{0}' expects a value but got null.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							key.PropertyName
						}));
					}
					break;
				}
			}
			contract.InvokeOnDeserialized(newObject, base.Serializer.Context);
			return newObject;
			Block_8:
			throw new JsonSerializationException("Unexpected end when deserializing object.");
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001588C File Offset: 0x00013A8C
		private void SetPropertyPresence(JsonReader reader, JsonProperty property, Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> requiredProperties)
		{
			if (property != null)
			{
				requiredProperties[property] = ((reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.Undefined) ? JsonSerializerInternalReader.PropertyPresence.Null : JsonSerializerInternalReader.PropertyPresence.Value);
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x000158B0 File Offset: 0x00013AB0
		private void HandleError(JsonReader reader, int initialDepth)
		{
			base.ClearErrorContext();
			reader.Skip();
			while (reader.Depth > initialDepth + 1)
			{
				reader.Read();
			}
		}

		// Token: 0x0400012B RID: 299
		private JsonSerializerProxy _internalSerializer;

		// Token: 0x02000063 RID: 99
		internal enum PropertyPresence
		{
			// Token: 0x04000132 RID: 306
			None,
			// Token: 0x04000133 RID: 307
			Null,
			// Token: 0x04000134 RID: 308
			Value
		}
	}
}
