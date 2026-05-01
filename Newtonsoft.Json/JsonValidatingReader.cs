using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000014 RID: 20
	public class JsonValidatingReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000C8 RID: 200 RVA: 0x00005FA4 File Offset: 0x000041A4
		// (remove) Token: 0x060000C9 RID: 201 RVA: 0x00005FDC File Offset: 0x000041DC
		public event ValidationEventHandler ValidationEventHandler;

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00006011 File Offset: 0x00004211
		public override object Value
		{
			get
			{
				return this._reader.Value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000CB RID: 203 RVA: 0x0000601E File Offset: 0x0000421E
		public override int Depth
		{
			get
			{
				return this._reader.Depth;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000CC RID: 204 RVA: 0x0000602B File Offset: 0x0000422B
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00006038 File Offset: 0x00004238
		public override char QuoteChar
		{
			get
			{
				return this._reader.QuoteChar;
			}
			protected internal set
			{
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000CE RID: 206 RVA: 0x0000603A File Offset: 0x0000423A
		public override JsonToken TokenType
		{
			get
			{
				return this._reader.TokenType;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00006047 File Offset: 0x00004247
		public override Type ValueType
		{
			get
			{
				return this._reader.ValueType;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006054 File Offset: 0x00004254
		private void Push(JsonValidatingReader.SchemaScope scope)
		{
			this._stack.Push(scope);
			this._currentScope = scope;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000606C File Offset: 0x0000426C
		private JsonValidatingReader.SchemaScope Pop()
		{
			JsonValidatingReader.SchemaScope result = this._stack.Pop();
			this._currentScope = ((this._stack.Count != 0) ? this._stack.Peek() : null);
			return result;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x000060A7 File Offset: 0x000042A7
		private IEnumerable<JsonSchemaModel> CurrentSchemas
		{
			get
			{
				return this._currentScope.Schemas;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000060B4 File Offset: 0x000042B4
		private IEnumerable<JsonSchemaModel> CurrentMemberSchemas
		{
			get
			{
				if (this._currentScope == null)
				{
					return new List<JsonSchemaModel>(new JsonSchemaModel[]
					{
						this._model
					});
				}
				if (this._currentScope.Schemas == null || this._currentScope.Schemas.Count == 0)
				{
					return Enumerable.Empty<JsonSchemaModel>();
				}
				switch (this._currentScope.TokenType)
				{
				case JTokenType.None:
					return this._currentScope.Schemas;
				case JTokenType.Object:
				{
					if (this._currentScope.CurrentPropertyName == null)
					{
						throw new Exception("CurrentPropertyName has not been set on scope.");
					}
					IList<JsonSchemaModel> list = new List<JsonSchemaModel>();
					foreach (JsonSchemaModel jsonSchemaModel in this.CurrentSchemas)
					{
						JsonSchemaModel jsonSchemaModel2;
						if (jsonSchemaModel.Properties != null && jsonSchemaModel.Properties.TryGetValue(this._currentScope.CurrentPropertyName, ref jsonSchemaModel2))
						{
							list.Add(jsonSchemaModel2);
						}
						if (jsonSchemaModel.PatternProperties != null)
						{
							foreach (KeyValuePair<string, JsonSchemaModel> keyValuePair in jsonSchemaModel.PatternProperties)
							{
								if (Regex.IsMatch(this._currentScope.CurrentPropertyName, keyValuePair.Key))
								{
									list.Add(keyValuePair.Value);
								}
							}
						}
						if (list.Count == 0 && jsonSchemaModel.AllowAdditionalProperties && jsonSchemaModel.AdditionalProperties != null)
						{
							list.Add(jsonSchemaModel.AdditionalProperties);
						}
					}
					return list;
				}
				case JTokenType.Array:
				{
					IList<JsonSchemaModel> list2 = new List<JsonSchemaModel>();
					foreach (JsonSchemaModel jsonSchemaModel3 in this.CurrentSchemas)
					{
						if (!CollectionUtils.IsNullOrEmpty<JsonSchemaModel>(jsonSchemaModel3.Items))
						{
							if (jsonSchemaModel3.Items.Count == 1)
							{
								list2.Add(jsonSchemaModel3.Items[0]);
							}
							else if (jsonSchemaModel3.Items.Count > this._currentScope.ArrayItemCount - 1)
							{
								list2.Add(jsonSchemaModel3.Items[this._currentScope.ArrayItemCount - 1]);
							}
						}
						if (jsonSchemaModel3.AllowAdditionalProperties && jsonSchemaModel3.AdditionalProperties != null)
						{
							list2.Add(jsonSchemaModel3.AdditionalProperties);
						}
					}
					return list2;
				}
				case JTokenType.Constructor:
					return Enumerable.Empty<JsonSchemaModel>();
				default:
					throw new ArgumentOutOfRangeException("TokenType", "Unexpected token type: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						this._currentScope.TokenType
					}));
				}
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000636C File Offset: 0x0000456C
		private void RaiseError(string message, JsonSchemaModel schema)
		{
			string message2 = ((IJsonLineInfo)this).HasLineInfo() ? (message + " Line {0}, position {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				((IJsonLineInfo)this).LineNumber,
				((IJsonLineInfo)this).LinePosition
			})) : message;
			this.OnValidationEvent(new JsonSchemaException(message2, null, ((IJsonLineInfo)this).LineNumber, ((IJsonLineInfo)this).LinePosition));
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000063DC File Offset: 0x000045DC
		private void OnValidationEvent(JsonSchemaException exception)
		{
			ValidationEventHandler validationEventHandler = this.ValidationEventHandler;
			if (validationEventHandler != null)
			{
				validationEventHandler(this, new ValidationEventArgs(exception));
				return;
			}
			throw exception;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006402 File Offset: 0x00004602
		public JsonValidatingReader(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			this._reader = reader;
			this._stack = new Stack<JsonValidatingReader.SchemaScope>();
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00006427 File Offset: 0x00004627
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x0000642F File Offset: 0x0000462F
		public JsonSchema Schema
		{
			get
			{
				return this._schema;
			}
			set
			{
				if (this.TokenType != JsonToken.None)
				{
					throw new Exception("Cannot change schema while validating JSON.");
				}
				this._schema = value;
				this._model = null;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00006452 File Offset: 0x00004652
		public JsonReader Reader
		{
			get
			{
				return this._reader;
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000645C File Offset: 0x0000465C
		private void ValidateInEnumAndNotDisallowed(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			JToken jtoken = new JValue(this._reader.Value);
			if (schema.Enum != null)
			{
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				jtoken.WriteTo(new JsonTextWriter(stringWriter), new JsonConverter[0]);
				if (!schema.Enum.ContainsValue(jtoken, new JTokenEqualityComparer()))
				{
					this.RaiseError("Value {0} is not defined in enum.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						stringWriter.ToString()
					}), schema);
				}
			}
			JsonSchemaType? currentNodeSchemaType = this.GetCurrentNodeSchemaType();
			if (currentNodeSchemaType != null && JsonSchemaGenerator.HasFlag(new JsonSchemaType?(schema.Disallow), currentNodeSchemaType.Value))
			{
				this.RaiseError("Type {0} is disallowed.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					currentNodeSchemaType
				}), schema);
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006530 File Offset: 0x00004730
		private JsonSchemaType? GetCurrentNodeSchemaType()
		{
			switch (this._reader.TokenType)
			{
			case JsonToken.StartObject:
				return new JsonSchemaType?(JsonSchemaType.Object);
			case JsonToken.StartArray:
				return new JsonSchemaType?(JsonSchemaType.Array);
			case JsonToken.Integer:
				return new JsonSchemaType?(JsonSchemaType.Integer);
			case JsonToken.Float:
				return new JsonSchemaType?(JsonSchemaType.Float);
			case JsonToken.String:
				return new JsonSchemaType?(JsonSchemaType.String);
			case JsonToken.Boolean:
				return new JsonSchemaType?(JsonSchemaType.Boolean);
			case JsonToken.Null:
				return new JsonSchemaType?(JsonSchemaType.Null);
			}
			return default(JsonSchemaType?);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000065BC File Offset: 0x000047BC
		public override byte[] ReadAsBytes()
		{
			byte[] result = this._reader.ReadAsBytes();
			this.ValidateCurrentToken();
			return result;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000065DC File Offset: 0x000047DC
		public override decimal? ReadAsDecimal()
		{
			decimal? result = this._reader.ReadAsDecimal();
			this.ValidateCurrentToken();
			return result;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000065FC File Offset: 0x000047FC
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? result = this._reader.ReadAsDateTimeOffset();
			this.ValidateCurrentToken();
			return result;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000661C File Offset: 0x0000481C
		public override bool Read()
		{
			if (!this._reader.Read())
			{
				return false;
			}
			if (this._reader.TokenType == JsonToken.Comment)
			{
				return true;
			}
			this.ValidateCurrentToken();
			return true;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006644 File Offset: 0x00004844
		private void ValidateCurrentToken()
		{
			if (this._model == null)
			{
				JsonSchemaModelBuilder jsonSchemaModelBuilder = new JsonSchemaModelBuilder();
				this._model = jsonSchemaModelBuilder.Build(this._schema);
			}
			switch (this._reader.TokenType)
			{
			case JsonToken.StartObject:
			{
				this.ProcessValue();
				IList<JsonSchemaModel> schemas = Enumerable.ToList<JsonSchemaModel>(Enumerable.Where<JsonSchemaModel>(this.CurrentMemberSchemas, new Func<JsonSchemaModel, bool>(this.ValidateObject)));
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Object, schemas));
				return;
			}
			case JsonToken.StartArray:
			{
				this.ProcessValue();
				IList<JsonSchemaModel> schemas2 = Enumerable.ToList<JsonSchemaModel>(Enumerable.Where<JsonSchemaModel>(this.CurrentMemberSchemas, new Func<JsonSchemaModel, bool>(this.ValidateArray)));
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Array, schemas2));
				return;
			}
			case JsonToken.StartConstructor:
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Constructor, null));
				return;
			case JsonToken.PropertyName:
				using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentSchemas.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JsonSchemaModel schema = enumerator.Current;
						this.ValidatePropertyName(schema);
					}
					return;
				}
				break;
			case JsonToken.Comment:
				goto IL_2E9;
			case JsonToken.Raw:
			case JsonToken.Undefined:
			case JsonToken.Date:
				return;
			case JsonToken.Integer:
				break;
			case JsonToken.Float:
				goto IL_163;
			case JsonToken.String:
				goto IL_1A3;
			case JsonToken.Boolean:
				goto IL_1E3;
			case JsonToken.Null:
				goto IL_223;
			case JsonToken.EndObject:
				goto IL_263;
			case JsonToken.EndArray:
				foreach (JsonSchemaModel schema2 in this.CurrentSchemas)
				{
					this.ValidateEndArray(schema2);
				}
				this.Pop();
				return;
			case JsonToken.EndConstructor:
				this.Pop();
				return;
			default:
				goto IL_2E9;
			}
			this.ProcessValue();
			using (IEnumerator<JsonSchemaModel> enumerator3 = this.CurrentMemberSchemas.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					JsonSchemaModel schema3 = enumerator3.Current;
					this.ValidateInteger(schema3);
				}
				return;
			}
			IL_163:
			this.ProcessValue();
			using (IEnumerator<JsonSchemaModel> enumerator4 = this.CurrentMemberSchemas.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					JsonSchemaModel schema4 = enumerator4.Current;
					this.ValidateFloat(schema4);
				}
				return;
			}
			IL_1A3:
			this.ProcessValue();
			using (IEnumerator<JsonSchemaModel> enumerator5 = this.CurrentMemberSchemas.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					JsonSchemaModel schema5 = enumerator5.Current;
					this.ValidateString(schema5);
				}
				return;
			}
			IL_1E3:
			this.ProcessValue();
			using (IEnumerator<JsonSchemaModel> enumerator6 = this.CurrentMemberSchemas.GetEnumerator())
			{
				while (enumerator6.MoveNext())
				{
					JsonSchemaModel schema6 = enumerator6.Current;
					this.ValidateBoolean(schema6);
				}
				return;
			}
			IL_223:
			this.ProcessValue();
			using (IEnumerator<JsonSchemaModel> enumerator7 = this.CurrentMemberSchemas.GetEnumerator())
			{
				while (enumerator7.MoveNext())
				{
					JsonSchemaModel schema7 = enumerator7.Current;
					this.ValidateNull(schema7);
				}
				return;
			}
			IL_263:
			foreach (JsonSchemaModel schema8 in this.CurrentSchemas)
			{
				this.ValidateEndObject(schema8);
			}
			this.Pop();
			return;
			IL_2E9:
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000069BC File Offset: 0x00004BBC
		private void ValidateEndObject(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			Dictionary<string, bool> requiredProperties = this._currentScope.RequiredProperties;
			if (requiredProperties != null)
			{
				List<string> list = Enumerable.ToList<string>(Enumerable.Select<KeyValuePair<string, bool>, string>(Enumerable.Where<KeyValuePair<string, bool>>(requiredProperties, (KeyValuePair<string, bool> kv) => !kv.Value), (KeyValuePair<string, bool> kv) => kv.Key));
				if (list.Count > 0)
				{
					this.RaiseError("Required properties are missing from object: {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						string.Join(", ", list.ToArray())
					}), schema);
				}
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006A64 File Offset: 0x00004C64
		private void ValidateEndArray(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			int arrayItemCount = this._currentScope.ArrayItemCount;
			if (schema.MaximumItems != null && arrayItemCount > schema.MaximumItems)
			{
				this.RaiseError("Array item count {0} exceeds maximum count of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					arrayItemCount,
					schema.MaximumItems
				}), schema);
			}
			if (schema.MinimumItems != null && arrayItemCount < schema.MinimumItems)
			{
				this.RaiseError("Array item count {0} is less than minimum count of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					arrayItemCount,
					schema.MinimumItems
				}), schema);
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006B55 File Offset: 0x00004D55
		private void ValidateNull(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Null))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006B6E File Offset: 0x00004D6E
		private void ValidateBoolean(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Boolean))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006B88 File Offset: 0x00004D88
		private void ValidateString(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.String))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
			string text = this._reader.Value.ToString();
			if (schema.MaximumLength != null && text.Length > schema.MaximumLength)
			{
				this.RaiseError("String '{0}' exceeds maximum length of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					text,
					schema.MaximumLength
				}), schema);
			}
			if (schema.MinimumLength != null && text.Length < schema.MinimumLength)
			{
				this.RaiseError("String '{0}' is less than minimum length of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					text,
					schema.MinimumLength
				}), schema);
			}
			if (schema.Patterns != null)
			{
				foreach (string text2 in schema.Patterns)
				{
					if (!Regex.IsMatch(text, text2))
					{
						this.RaiseError("String '{0}' does not match regex pattern '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							text,
							text2
						}), schema);
					}
				}
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006D0C File Offset: 0x00004F0C
		private void ValidateInteger(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Integer))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
			long num = Convert.ToInt64(this._reader.Value, CultureInfo.InvariantCulture);
			if (schema.Maximum != null)
			{
				double num2 = (double)num;
				double? maximum = schema.Maximum;
				if (num2 > maximum.GetValueOrDefault() && maximum != null)
				{
					this.RaiseError("Integer {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						num,
						schema.Maximum
					}), schema);
				}
				if (schema.ExclusiveMaximum)
				{
					double num3 = (double)num;
					double? maximum2 = schema.Maximum;
					if (num3 == maximum2.GetValueOrDefault() && maximum2 != null)
					{
						this.RaiseError("Integer {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							num,
							schema.Maximum
						}), schema);
					}
				}
			}
			if (schema.Minimum != null)
			{
				double num4 = (double)num;
				double? minimum = schema.Minimum;
				if (num4 < minimum.GetValueOrDefault() && minimum != null)
				{
					this.RaiseError("Integer {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						num,
						schema.Minimum
					}), schema);
				}
				if (schema.ExclusiveMinimum)
				{
					double num5 = (double)num;
					double? minimum2 = schema.Minimum;
					if (num5 == minimum2.GetValueOrDefault() && minimum2 != null)
					{
						this.RaiseError("Integer {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							num,
							schema.Minimum
						}), schema);
					}
				}
			}
			if (schema.DivisibleBy != null && !JsonValidatingReader.IsZero((double)num % schema.DivisibleBy.Value))
			{
				this.RaiseError("Integer {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JsonConvert.ToString(num),
					schema.DivisibleBy
				}), schema);
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006F50 File Offset: 0x00005150
		private void ProcessValue()
		{
			if (this._currentScope != null && this._currentScope.TokenType == JTokenType.Array)
			{
				this._currentScope.ArrayItemCount++;
				foreach (JsonSchemaModel jsonSchemaModel in this.CurrentSchemas)
				{
					if (jsonSchemaModel != null && jsonSchemaModel.Items != null && jsonSchemaModel.Items.Count > 1 && this._currentScope.ArrayItemCount >= jsonSchemaModel.Items.Count)
					{
						this.RaiseError("Index {0} has not been defined and the schema does not allow additional items.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							this._currentScope.ArrayItemCount
						}), jsonSchemaModel);
					}
				}
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00007028 File Offset: 0x00005228
		private void ValidateFloat(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Float))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
			double num = Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture);
			if (schema.Maximum != null)
			{
				double num2 = num;
				double? maximum = schema.Maximum;
				if (num2 > maximum.GetValueOrDefault() && maximum != null)
				{
					this.RaiseError("Float {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						JsonConvert.ToString(num),
						schema.Maximum
					}), schema);
				}
				if (schema.ExclusiveMaximum)
				{
					double num3 = num;
					double? maximum2 = schema.Maximum;
					if (num3 == maximum2.GetValueOrDefault() && maximum2 != null)
					{
						this.RaiseError("Float {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							JsonConvert.ToString(num),
							schema.Maximum
						}), schema);
					}
				}
			}
			if (schema.Minimum != null)
			{
				double num4 = num;
				double? minimum = schema.Minimum;
				if (num4 < minimum.GetValueOrDefault() && minimum != null)
				{
					this.RaiseError("Float {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						JsonConvert.ToString(num),
						schema.Minimum
					}), schema);
				}
				if (schema.ExclusiveMinimum)
				{
					double num5 = num;
					double? minimum2 = schema.Minimum;
					if (num5 == minimum2.GetValueOrDefault() && minimum2 != null)
					{
						this.RaiseError("Float {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							JsonConvert.ToString(num),
							schema.Minimum
						}), schema);
					}
				}
			}
			if (schema.DivisibleBy != null && !JsonValidatingReader.IsZero(num % schema.DivisibleBy.Value))
			{
				this.RaiseError("Float {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JsonConvert.ToString(num),
					schema.DivisibleBy
				}), schema);
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00007268 File Offset: 0x00005468
		private static bool IsZero(double value)
		{
			double num = 2.220446049250313E-16;
			return Math.Abs(value) < 10.0 * num;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00007294 File Offset: 0x00005494
		private void ValidatePropertyName(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			string text = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
			if (this._currentScope.RequiredProperties.ContainsKey(text))
			{
				this._currentScope.RequiredProperties[text] = true;
			}
			if (!schema.AllowAdditionalProperties && !this.IsPropertyDefinied(schema, text))
			{
				this.RaiseError("Property '{0}' has not been defined and the schema does not allow additional properties.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					text
				}), schema);
			}
			this._currentScope.CurrentPropertyName = text;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00007324 File Offset: 0x00005524
		private bool IsPropertyDefinied(JsonSchemaModel schema, string propertyName)
		{
			if (schema.Properties != null && schema.Properties.ContainsKey(propertyName))
			{
				return true;
			}
			if (schema.PatternProperties != null)
			{
				foreach (string text in schema.PatternProperties.Keys)
				{
					if (Regex.IsMatch(propertyName, text))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000073A0 File Offset: 0x000055A0
		private bool ValidateArray(JsonSchemaModel schema)
		{
			return schema == null || this.TestType(schema, JsonSchemaType.Array);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000073B0 File Offset: 0x000055B0
		private bool ValidateObject(JsonSchemaModel schema)
		{
			return schema == null || this.TestType(schema, JsonSchemaType.Object);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000073C0 File Offset: 0x000055C0
		private bool TestType(JsonSchemaModel currentSchema, JsonSchemaType currentType)
		{
			if (!JsonSchemaGenerator.HasFlag(new JsonSchemaType?(currentSchema.Type), currentType))
			{
				this.RaiseError("Invalid type. Expected {0} but got {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					currentSchema.Type,
					currentType
				}), currentSchema);
				return false;
			}
			return true;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00007418 File Offset: 0x00005618
		bool IJsonLineInfo.HasLineInfo()
		{
			IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
			return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x0000743C File Offset: 0x0000563C
		int IJsonLineInfo.LineNumber
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00007460 File Offset: 0x00005660
		int IJsonLineInfo.LinePosition
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		// Token: 0x0400005A RID: 90
		private readonly JsonReader _reader;

		// Token: 0x0400005B RID: 91
		private readonly Stack<JsonValidatingReader.SchemaScope> _stack;

		// Token: 0x0400005C RID: 92
		private JsonSchema _schema;

		// Token: 0x0400005D RID: 93
		private JsonSchemaModel _model;

		// Token: 0x0400005E RID: 94
		private JsonValidatingReader.SchemaScope _currentScope;

		// Token: 0x02000015 RID: 21
		private class SchemaScope
		{
			// Token: 0x1700002C RID: 44
			// (get) Token: 0x060000F4 RID: 244 RVA: 0x00007484 File Offset: 0x00005684
			// (set) Token: 0x060000F5 RID: 245 RVA: 0x0000748C File Offset: 0x0000568C
			public string CurrentPropertyName { get; set; }

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x060000F6 RID: 246 RVA: 0x00007495 File Offset: 0x00005695
			// (set) Token: 0x060000F7 RID: 247 RVA: 0x0000749D File Offset: 0x0000569D
			public int ArrayItemCount { get; set; }

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x060000F8 RID: 248 RVA: 0x000074A6 File Offset: 0x000056A6
			public IList<JsonSchemaModel> Schemas
			{
				get
				{
					return this._schemas;
				}
			}

			// Token: 0x1700002F RID: 47
			// (get) Token: 0x060000F9 RID: 249 RVA: 0x000074AE File Offset: 0x000056AE
			public Dictionary<string, bool> RequiredProperties
			{
				get
				{
					return this._requiredProperties;
				}
			}

			// Token: 0x17000030 RID: 48
			// (get) Token: 0x060000FA RID: 250 RVA: 0x000074B6 File Offset: 0x000056B6
			public JTokenType TokenType
			{
				get
				{
					return this._tokenType;
				}
			}

			// Token: 0x060000FB RID: 251 RVA: 0x000074C4 File Offset: 0x000056C4
			public SchemaScope(JTokenType tokenType, IList<JsonSchemaModel> schemas)
			{
				this._tokenType = tokenType;
				this._schemas = schemas;
				this._requiredProperties = Enumerable.ToDictionary<string, string, bool>(Enumerable.Distinct<string>(Enumerable.SelectMany<JsonSchemaModel, string>(schemas, new Func<JsonSchemaModel, IEnumerable<string>>(this.GetRequiredProperties))), (string p) => p, (string p) => false);
			}

			// Token: 0x060000FC RID: 252 RVA: 0x00007558 File Offset: 0x00005758
			private IEnumerable<string> GetRequiredProperties(JsonSchemaModel schema)
			{
				if (schema == null || schema.Properties == null)
				{
					return Enumerable.Empty<string>();
				}
				return Enumerable.Select<KeyValuePair<string, JsonSchemaModel>, string>(Enumerable.Where<KeyValuePair<string, JsonSchemaModel>>(schema.Properties, (KeyValuePair<string, JsonSchemaModel> p) => p.Value.Required), (KeyValuePair<string, JsonSchemaModel> p) => p.Key);
			}

			// Token: 0x04000062 RID: 98
			private readonly JTokenType _tokenType;

			// Token: 0x04000063 RID: 99
			private readonly IList<JsonSchemaModel> _schemas;

			// Token: 0x04000064 RID: 100
			private readonly Dictionary<string, bool> _requiredProperties;
		}
	}
}
