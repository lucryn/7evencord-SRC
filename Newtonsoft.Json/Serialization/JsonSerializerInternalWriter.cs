using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000061 RID: 97
	internal class JsonSerializerInternalWriter : JsonSerializerInternalBase
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x00013100 File Offset: 0x00011300
		private List<object> SerializeStack
		{
			get
			{
				if (this._serializeStack == null)
				{
					this._serializeStack = new List<object>();
				}
				return this._serializeStack;
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0001311B File Offset: 0x0001131B
		public JsonSerializerInternalWriter(JsonSerializer serializer) : base(serializer)
		{
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00013124 File Offset: 0x00011324
		public void Serialize(JsonWriter jsonWriter, object value)
		{
			if (jsonWriter == null)
			{
				throw new ArgumentNullException("jsonWriter");
			}
			this.SerializeValue(jsonWriter, value, this.GetContractSafe(value), null, null);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00013145 File Offset: 0x00011345
		private JsonSerializerProxy GetInternalSerializer()
		{
			if (this._internalSerializer == null)
			{
				this._internalSerializer = new JsonSerializerProxy(this);
			}
			return this._internalSerializer;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00013161 File Offset: 0x00011361
		private JsonContract GetContractSafe(object value)
		{
			if (value == null)
			{
				return null;
			}
			return base.Serializer.ContractResolver.ResolveContract(value.GetType());
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00013180 File Offset: 0x00011380
		private void SerializePrimitive(JsonWriter writer, object value, JsonPrimitiveContract contract, JsonProperty member, JsonContract collectionValueContract)
		{
			if (contract.UnderlyingType == typeof(byte[]))
			{
				bool flag = this.ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionValueContract);
				if (flag)
				{
					writer.WriteStartObject();
					this.WriteTypeProperty(writer, contract.CreatedType);
					writer.WritePropertyName("$value");
					writer.WriteValue(value);
					writer.WriteEndObject();
					return;
				}
			}
			writer.WriteValue(value);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x000131E4 File Offset: 0x000113E4
		private void SerializeValue(JsonWriter writer, object value, JsonContract valueContract, JsonProperty member, JsonContract collectionValueContract)
		{
			JsonConverter jsonConverter = (member != null) ? member.Converter : null;
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			if ((jsonConverter != null || (jsonConverter = valueContract.Converter) != null || (jsonConverter = base.Serializer.GetMatchingConverter(valueContract.UnderlyingType)) != null || (jsonConverter = valueContract.InternalConverter) != null) && jsonConverter.CanWrite)
			{
				this.SerializeConvertable(writer, jsonConverter, value, valueContract);
				return;
			}
			if (valueContract is JsonPrimitiveContract)
			{
				this.SerializePrimitive(writer, value, (JsonPrimitiveContract)valueContract, member, collectionValueContract);
				return;
			}
			if (valueContract is JsonStringContract)
			{
				this.SerializeString(writer, value, (JsonStringContract)valueContract);
				return;
			}
			if (valueContract is JsonObjectContract)
			{
				this.SerializeObject(writer, value, (JsonObjectContract)valueContract, member, collectionValueContract);
				return;
			}
			if (valueContract is JsonDictionaryContract)
			{
				JsonDictionaryContract jsonDictionaryContract = (JsonDictionaryContract)valueContract;
				this.SerializeDictionary(writer, jsonDictionaryContract.CreateWrapper(value), jsonDictionaryContract, member, collectionValueContract);
				return;
			}
			if (valueContract is JsonArrayContract)
			{
				JsonArrayContract jsonArrayContract = (JsonArrayContract)valueContract;
				this.SerializeList(writer, jsonArrayContract.CreateWrapper(value), jsonArrayContract, member, collectionValueContract);
				return;
			}
			if (valueContract is JsonLinqContract)
			{
				((JToken)value).WriteTo(writer, (base.Serializer.Converters != null) ? Enumerable.ToArray<JsonConverter>(base.Serializer.Converters) : null);
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00013310 File Offset: 0x00011510
		private bool ShouldWriteReference(object value, JsonProperty property, JsonContract contract)
		{
			if (value == null)
			{
				return false;
			}
			if (contract is JsonPrimitiveContract)
			{
				return false;
			}
			bool? flag = default(bool?);
			if (property != null)
			{
				flag = property.IsReference;
			}
			if (flag == null)
			{
				flag = contract.IsReference;
			}
			if (flag == null)
			{
				if (contract is JsonArrayContract)
				{
					flag = new bool?(this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Arrays));
				}
				else
				{
					flag = new bool?(this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects));
				}
			}
			return flag.Value && base.Serializer.ReferenceResolver.IsReferenced(this, value);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000133B0 File Offset: 0x000115B0
		private void WriteMemberInfoProperty(JsonWriter writer, object memberValue, JsonProperty property, JsonContract contract)
		{
			string propertyName = property.PropertyName;
			object defaultValue = property.DefaultValue;
			if (property.NullValueHandling.GetValueOrDefault(base.Serializer.NullValueHandling) == NullValueHandling.Ignore && memberValue == null)
			{
				return;
			}
			if (this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling), DefaultValueHandling.Ignore) && MiscellaneousUtils.ValueEquals(memberValue, defaultValue))
			{
				return;
			}
			if (this.ShouldWriteReference(memberValue, property, contract))
			{
				writer.WritePropertyName(propertyName);
				this.WriteReference(writer, memberValue);
				return;
			}
			if (!this.CheckForCircularReference(memberValue, property.ReferenceLoopHandling, contract))
			{
				return;
			}
			if (memberValue == null && property.Required == Required.Always)
			{
				throw new JsonSerializationException("Cannot write a null value for property '{0}'. Property requires a value.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					property.PropertyName
				}));
			}
			writer.WritePropertyName(propertyName);
			this.SerializeValue(writer, memberValue, contract, property, null);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00013490 File Offset: 0x00011690
		private bool CheckForCircularReference(object value, ReferenceLoopHandling? referenceLoopHandling, JsonContract contract)
		{
			if (value == null || contract is JsonPrimitiveContract)
			{
				return true;
			}
			if (this.SerializeStack.IndexOf(value) == -1)
			{
				return true;
			}
			switch (referenceLoopHandling.GetValueOrDefault(base.Serializer.ReferenceLoopHandling))
			{
			case ReferenceLoopHandling.Error:
				throw new JsonSerializationException("Self referencing loop detected for type '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					value.GetType()
				}));
			case ReferenceLoopHandling.Ignore:
				return false;
			case ReferenceLoopHandling.Serialize:
				return true;
			default:
				throw new InvalidOperationException("Unexpected ReferenceLoopHandling value: '{0}'".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.Serializer.ReferenceLoopHandling
				}));
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001353E File Offset: 0x0001173E
		private void WriteReference(JsonWriter writer, object value)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("$ref");
			writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(this, value));
			writer.WriteEndObject();
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00013570 File Offset: 0x00011770
		internal static bool TryConvertToString(object value, Type type, out string s)
		{
			TypeConverter converter = ConvertUtils.GetConverter(type);
			if (converter != null && converter.GetType() != typeof(TypeConverter) && converter.CanConvertTo(typeof(string)))
			{
				s = converter.ConvertToString(value);
				return true;
			}
			if (value is Guid || value is Uri || value is TimeSpan)
			{
				s = value.ToString();
				return true;
			}
			if (value is Type)
			{
				s = ((Type)value).AssemblyQualifiedName;
				return true;
			}
			s = null;
			return false;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000135F4 File Offset: 0x000117F4
		private void SerializeString(JsonWriter writer, object value, JsonStringContract contract)
		{
			contract.InvokeOnSerializing(value, base.Serializer.Context);
			string value2;
			JsonSerializerInternalWriter.TryConvertToString(value, contract.UnderlyingType, out value2);
			writer.WriteValue(value2);
			contract.InvokeOnSerialized(value, base.Serializer.Context);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001363C File Offset: 0x0001183C
		private void SerializeObject(JsonWriter writer, object value, JsonObjectContract contract, JsonProperty member, JsonContract collectionValueContract)
		{
			contract.InvokeOnSerializing(value, base.Serializer.Context);
			this.SerializeStack.Add(value);
			writer.WriteStartObject();
			bool flag = contract.IsReference ?? this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects);
			if (flag)
			{
				writer.WritePropertyName("$id");
				writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(this, value));
			}
			if (this.ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionValueContract))
			{
				this.WriteTypeProperty(writer, contract.UnderlyingType);
			}
			int top = writer.Top;
			foreach (JsonProperty jsonProperty in contract.Properties)
			{
				try
				{
					if (!jsonProperty.Ignored && jsonProperty.Readable && this.ShouldSerialize(jsonProperty, value) && this.IsSpecified(jsonProperty, value))
					{
						object value2 = jsonProperty.ValueProvider.GetValue(value);
						JsonContract contractSafe = this.GetContractSafe(value2);
						this.WriteMemberInfoProperty(writer, value2, jsonProperty, contractSafe);
					}
				}
				catch (Exception ex)
				{
					if (!base.IsErrorHandled(value, contract, jsonProperty.PropertyName, ex))
					{
						throw;
					}
					this.HandleError(writer, top);
				}
			}
			writer.WriteEndObject();
			this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
			contract.InvokeOnSerialized(value, base.Serializer.Context);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x000137C8 File Offset: 0x000119C8
		private void WriteTypeProperty(JsonWriter writer, Type type)
		{
			writer.WritePropertyName("$type");
			writer.WriteValue(ReflectionUtils.GetTypeName(type, base.Serializer.TypeNameAssemblyFormat, base.Serializer.Binder));
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000137F7 File Offset: 0x000119F7
		private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x000137FF File Offset: 0x000119FF
		private bool HasFlag(PreserveReferencesHandling value, PreserveReferencesHandling flag)
		{
			return (value & flag) == flag;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00013807 File Offset: 0x00011A07
		private bool HasFlag(TypeNameHandling value, TypeNameHandling flag)
		{
			return (value & flag) == flag;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00013810 File Offset: 0x00011A10
		private void SerializeConvertable(JsonWriter writer, JsonConverter converter, object value, JsonContract contract)
		{
			if (this.ShouldWriteReference(value, null, contract))
			{
				this.WriteReference(writer, value);
				return;
			}
			if (!this.CheckForCircularReference(value, default(ReferenceLoopHandling?), contract))
			{
				return;
			}
			this.SerializeStack.Add(value);
			converter.WriteJson(writer, value, this.GetInternalSerializer());
			this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001387C File Offset: 0x00011A7C
		private void SerializeList(JsonWriter writer, IWrappedCollection values, JsonArrayContract contract, JsonProperty member, JsonContract collectionValueContract)
		{
			contract.InvokeOnSerializing(values.UnderlyingCollection, base.Serializer.Context);
			this.SerializeStack.Add(values.UnderlyingCollection);
			bool flag = contract.IsReference ?? this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Arrays);
			bool flag2 = this.ShouldWriteType(TypeNameHandling.Arrays, contract, member, collectionValueContract);
			if (flag || flag2)
			{
				writer.WriteStartObject();
				if (flag)
				{
					writer.WritePropertyName("$id");
					writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(this, values.UnderlyingCollection));
				}
				if (flag2)
				{
					this.WriteTypeProperty(writer, values.UnderlyingCollection.GetType());
				}
				writer.WritePropertyName("$values");
			}
			JsonContract collectionValueContract2 = base.Serializer.ContractResolver.ResolveContract(contract.CollectionItemType ?? typeof(object));
			writer.WriteStartArray();
			int top = writer.Top;
			int num = 0;
			foreach (object value in values)
			{
				try
				{
					JsonContract contractSafe = this.GetContractSafe(value);
					if (this.ShouldWriteReference(value, null, contractSafe))
					{
						this.WriteReference(writer, value);
					}
					else if (this.CheckForCircularReference(value, default(ReferenceLoopHandling?), contract))
					{
						this.SerializeValue(writer, value, contractSafe, null, collectionValueContract2);
					}
				}
				catch (Exception ex)
				{
					if (!base.IsErrorHandled(values.UnderlyingCollection, contract, num, ex))
					{
						throw;
					}
					this.HandleError(writer, top);
				}
				finally
				{
					num++;
				}
			}
			writer.WriteEndArray();
			if (flag || flag2)
			{
				writer.WriteEndObject();
			}
			this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
			contract.InvokeOnSerialized(values.UnderlyingCollection, base.Serializer.Context);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00013A90 File Offset: 0x00011C90
		private bool ShouldWriteType(TypeNameHandling typeNameHandlingFlag, JsonContract contract, JsonProperty member, JsonContract collectionValueContract)
		{
			if (this.HasFlag(((member != null) ? member.TypeNameHandling : default(TypeNameHandling?)) ?? base.Serializer.TypeNameHandling, typeNameHandlingFlag))
			{
				return true;
			}
			if (member != null)
			{
				if ((member.TypeNameHandling ?? base.Serializer.TypeNameHandling) == TypeNameHandling.Auto && contract.UnderlyingType != member.PropertyType)
				{
					JsonContract jsonContract = base.Serializer.ContractResolver.ResolveContract(member.PropertyType);
					if (contract.UnderlyingType != jsonContract.CreatedType)
					{
						return true;
					}
				}
			}
			else if (collectionValueContract != null && base.Serializer.TypeNameHandling == TypeNameHandling.Auto && contract.UnderlyingType != collectionValueContract.UnderlyingType)
			{
				return true;
			}
			return false;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00013B60 File Offset: 0x00011D60
		private void SerializeDictionary(JsonWriter writer, IWrappedDictionary values, JsonDictionaryContract contract, JsonProperty member, JsonContract collectionValueContract)
		{
			contract.InvokeOnSerializing(values.UnderlyingDictionary, base.Serializer.Context);
			this.SerializeStack.Add(values.UnderlyingDictionary);
			writer.WriteStartObject();
			bool flag = contract.IsReference ?? this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects);
			if (flag)
			{
				writer.WritePropertyName("$id");
				writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(this, values.UnderlyingDictionary));
			}
			if (this.ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionValueContract))
			{
				this.WriteTypeProperty(writer, values.UnderlyingDictionary.GetType());
			}
			JsonContract collectionValueContract2 = base.Serializer.ContractResolver.ResolveContract(contract.DictionaryValueType ?? typeof(object));
			int top = writer.Top;
			foreach (object obj in values)
			{
				DictionaryEntry entry = (DictionaryEntry)obj;
				string text = this.GetPropertyName(entry);
				text = ((contract.PropertyNameResolver != null) ? contract.PropertyNameResolver.Invoke(text) : text);
				try
				{
					object value = entry.Value;
					JsonContract contractSafe = this.GetContractSafe(value);
					if (this.ShouldWriteReference(value, null, contractSafe))
					{
						writer.WritePropertyName(text);
						this.WriteReference(writer, value);
					}
					else if (this.CheckForCircularReference(value, default(ReferenceLoopHandling?), contract))
					{
						writer.WritePropertyName(text);
						this.SerializeValue(writer, value, contractSafe, null, collectionValueContract2);
					}
				}
				catch (Exception ex)
				{
					if (!base.IsErrorHandled(values.UnderlyingDictionary, contract, text, ex))
					{
						throw;
					}
					this.HandleError(writer, top);
				}
			}
			writer.WriteEndObject();
			this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
			contract.InvokeOnSerialized(values.UnderlyingDictionary, base.Serializer.Context);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00013D7C File Offset: 0x00011F7C
		private string GetPropertyName(DictionaryEntry entry)
		{
			if (entry.Key is IConvertible)
			{
				return Convert.ToString(entry.Key, CultureInfo.InvariantCulture);
			}
			string result;
			if (JsonSerializerInternalWriter.TryConvertToString(entry.Key, entry.Key.GetType(), out result))
			{
				return result;
			}
			return entry.Key.ToString();
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00013DD3 File Offset: 0x00011FD3
		private void HandleError(JsonWriter writer, int initialDepth)
		{
			base.ClearErrorContext();
			while (writer.Top > initialDepth)
			{
				writer.WriteEnd();
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00013DEC File Offset: 0x00011FEC
		private bool ShouldSerialize(JsonProperty property, object target)
		{
			return property.ShouldSerialize == null || property.ShouldSerialize.Invoke(target);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00013E04 File Offset: 0x00012004
		private bool IsSpecified(JsonProperty property, object target)
		{
			return property.GetIsSpecified == null || property.GetIsSpecified.Invoke(target);
		}

		// Token: 0x04000129 RID: 297
		private JsonSerializerProxy _internalSerializer;

		// Token: 0x0400012A RID: 298
		private List<object> _serializeStack;
	}
}
