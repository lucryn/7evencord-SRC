using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000008 RID: 8
	internal class JsonSchemaBuilder
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002A53 File Offset: 0x00000C53
		private void Push(JsonSchema value)
		{
			this._currentSchema = value;
			this._stack.Add(value);
			this._resolver.LoadedSchemas.Add(value);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002A7C File Offset: 0x00000C7C
		private JsonSchema Pop()
		{
			JsonSchema currentSchema = this._currentSchema;
			this._stack.RemoveAt(this._stack.Count - 1);
			this._currentSchema = Enumerable.LastOrDefault<JsonSchema>(this._stack);
			return currentSchema;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002ABA File Offset: 0x00000CBA
		private JsonSchema CurrentSchema
		{
			get
			{
				return this._currentSchema;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002AC2 File Offset: 0x00000CC2
		public JsonSchemaBuilder(JsonSchemaResolver resolver)
		{
			this._stack = new List<JsonSchema>();
			this._resolver = resolver;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002ADC File Offset: 0x00000CDC
		internal JsonSchema Parse(JsonReader reader)
		{
			this._reader = reader;
			if (reader.TokenType == JsonToken.None)
			{
				this._reader.Read();
			}
			return this.BuildSchema();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B00 File Offset: 0x00000D00
		private JsonSchema BuildSchema()
		{
			if (this._reader.TokenType != JsonToken.StartObject)
			{
				throw new Exception("Expected StartObject while parsing schema object, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					this._reader.TokenType
				}));
			}
			this._reader.Read();
			if (this._reader.TokenType == JsonToken.EndObject)
			{
				this.Push(new JsonSchema());
				return this.Pop();
			}
			string text = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
			this._reader.Read();
			if (!(text == "$ref"))
			{
				this.Push(new JsonSchema());
				this.ProcessSchemaProperty(text);
				while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
				{
					text = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
					this._reader.Read();
					this.ProcessSchemaProperty(text);
				}
				return this.Pop();
			}
			string text2 = (string)this._reader.Value;
			while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
			{
				if (this._reader.TokenType == JsonToken.StartObject)
				{
					throw new Exception("Found StartObject within the schema reference with the Id '{0}'".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						text2
					}));
				}
			}
			JsonSchema schema = this._resolver.GetSchema(text2);
			if (schema == null)
			{
				throw new Exception("Could not resolve schema reference for Id '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					text2
				}));
			}
			return schema;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002CA0 File Offset: 0x00000EA0
		private void ProcessSchemaProperty(string propertyName)
		{
			if (propertyName != null)
			{
				if (<PrivateImplementationDetails>{012CD1F2-DE45-4E7E-8398-226032FA1116}.$$method0x600001e-1 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(29);
					dictionary.Add("type", 0);
					dictionary.Add("id", 1);
					dictionary.Add("title", 2);
					dictionary.Add("description", 3);
					dictionary.Add("properties", 4);
					dictionary.Add("items", 5);
					dictionary.Add("additionalProperties", 6);
					dictionary.Add("patternProperties", 7);
					dictionary.Add("required", 8);
					dictionary.Add("requires", 9);
					dictionary.Add("identity", 10);
					dictionary.Add("minimum", 11);
					dictionary.Add("maximum", 12);
					dictionary.Add("exclusiveMinimum", 13);
					dictionary.Add("exclusiveMaximum", 14);
					dictionary.Add("maxLength", 15);
					dictionary.Add("minLength", 16);
					dictionary.Add("maxItems", 17);
					dictionary.Add("minItems", 18);
					dictionary.Add("divisibleBy", 19);
					dictionary.Add("disallow", 20);
					dictionary.Add("default", 21);
					dictionary.Add("hidden", 22);
					dictionary.Add("readonly", 23);
					dictionary.Add("format", 24);
					dictionary.Add("pattern", 25);
					dictionary.Add("options", 26);
					dictionary.Add("enum", 27);
					dictionary.Add("extends", 28);
					<PrivateImplementationDetails>{012CD1F2-DE45-4E7E-8398-226032FA1116}.$$method0x600001e-1 = dictionary;
				}
				int num;
				if (<PrivateImplementationDetails>{012CD1F2-DE45-4E7E-8398-226032FA1116}.$$method0x600001e-1.TryGetValue(propertyName, ref num))
				{
					switch (num)
					{
					case 0:
						this.CurrentSchema.Type = this.ProcessType();
						return;
					case 1:
						this.CurrentSchema.Id = (string)this._reader.Value;
						return;
					case 2:
						this.CurrentSchema.Title = (string)this._reader.Value;
						return;
					case 3:
						this.CurrentSchema.Description = (string)this._reader.Value;
						return;
					case 4:
						this.ProcessProperties();
						return;
					case 5:
						this.ProcessItems();
						return;
					case 6:
						this.ProcessAdditionalProperties();
						return;
					case 7:
						this.ProcessPatternProperties();
						return;
					case 8:
						this.CurrentSchema.Required = new bool?((bool)this._reader.Value);
						return;
					case 9:
						this.CurrentSchema.Requires = (string)this._reader.Value;
						return;
					case 10:
						this.ProcessIdentity();
						return;
					case 11:
						this.CurrentSchema.Minimum = new double?(Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 12:
						this.CurrentSchema.Maximum = new double?(Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 13:
						this.CurrentSchema.ExclusiveMinimum = new bool?((bool)this._reader.Value);
						return;
					case 14:
						this.CurrentSchema.ExclusiveMaximum = new bool?((bool)this._reader.Value);
						return;
					case 15:
						this.CurrentSchema.MaximumLength = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 16:
						this.CurrentSchema.MinimumLength = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 17:
						this.CurrentSchema.MaximumItems = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 18:
						this.CurrentSchema.MinimumItems = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 19:
						this.CurrentSchema.DivisibleBy = new double?(Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 20:
						this.CurrentSchema.Disallow = this.ProcessType();
						return;
					case 21:
						this.ProcessDefault();
						return;
					case 22:
						this.CurrentSchema.Hidden = new bool?((bool)this._reader.Value);
						return;
					case 23:
						this.CurrentSchema.ReadOnly = new bool?((bool)this._reader.Value);
						return;
					case 24:
						this.CurrentSchema.Format = (string)this._reader.Value;
						return;
					case 25:
						this.CurrentSchema.Pattern = (string)this._reader.Value;
						return;
					case 26:
						this.ProcessOptions();
						return;
					case 27:
						this.ProcessEnum();
						return;
					case 28:
						this.ProcessExtends();
						return;
					}
				}
			}
			this._reader.Skip();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003197 File Offset: 0x00001397
		private void ProcessExtends()
		{
			this.CurrentSchema.Extends = this.BuildSchema();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000031AC File Offset: 0x000013AC
		private void ProcessEnum()
		{
			if (this._reader.TokenType != JsonToken.StartArray)
			{
				throw new Exception("Expected StartArray token while parsing enum values, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					this._reader.TokenType
				}));
			}
			this.CurrentSchema.Enum = new List<JToken>();
			while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
			{
				JToken jtoken = JToken.ReadFrom(this._reader);
				this.CurrentSchema.Enum.Add(jtoken);
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003244 File Offset: 0x00001444
		private void ProcessOptions()
		{
			this.CurrentSchema.Options = new Dictionary<JToken, string>(new JTokenEqualityComparer());
			JsonToken tokenType = this._reader.TokenType;
			if (tokenType != JsonToken.StartArray)
			{
				throw new Exception("Expected array token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					this._reader.TokenType
				}));
			}
			while (this._reader.Read())
			{
				if (this._reader.TokenType == JsonToken.EndArray)
				{
					return;
				}
				if (this._reader.TokenType != JsonToken.StartObject)
				{
					throw new Exception("Expect object token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						this._reader.TokenType
					}));
				}
				string text = null;
				JToken jtoken = null;
				while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
				{
					string text2 = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
					this._reader.Read();
					string text3;
					if ((text3 = text2) != null)
					{
						if (text3 == "value")
						{
							jtoken = JToken.ReadFrom(this._reader);
							continue;
						}
						if (text3 == "label")
						{
							text = (string)this._reader.Value;
							continue;
						}
					}
					throw new Exception("Unexpected property in JSON schema option: {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						text2
					}));
				}
				if (jtoken == null)
				{
					throw new Exception("No value specified for JSON schema option.");
				}
				if (this.CurrentSchema.Options.ContainsKey(jtoken))
				{
					throw new Exception("Duplicate value in JSON schema option collection: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						jtoken
					}));
				}
				this.CurrentSchema.Options.Add(jtoken, text);
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000341C File Offset: 0x0000161C
		private void ProcessDefault()
		{
			this.CurrentSchema.Default = JToken.ReadFrom(this._reader);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003434 File Offset: 0x00001634
		private void ProcessIdentity()
		{
			this.CurrentSchema.Identity = new List<string>();
			JsonToken tokenType = this._reader.TokenType;
			if (tokenType == JsonToken.StartArray)
			{
				while (this._reader.Read())
				{
					if (this._reader.TokenType == JsonToken.EndArray)
					{
						return;
					}
					if (this._reader.TokenType != JsonToken.String)
					{
						throw new Exception("Exception JSON property name string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							this._reader.TokenType
						}));
					}
					this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
				}
				return;
			}
			if (tokenType == JsonToken.String)
			{
				this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
				return;
			}
			throw new Exception("Expected array or JSON property name string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				this._reader.TokenType
			}));
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003539 File Offset: 0x00001739
		private void ProcessAdditionalProperties()
		{
			if (this._reader.TokenType == JsonToken.Boolean)
			{
				this.CurrentSchema.AllowAdditionalProperties = (bool)this._reader.Value;
				return;
			}
			this.CurrentSchema.AdditionalProperties = this.BuildSchema();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003578 File Offset: 0x00001778
		private void ProcessPatternProperties()
		{
			Dictionary<string, JsonSchema> dictionary = new Dictionary<string, JsonSchema>();
			if (this._reader.TokenType != JsonToken.StartObject)
			{
				throw new Exception("Expected start object token.");
			}
			while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
			{
				string text = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
				this._reader.Read();
				if (dictionary.ContainsKey(text))
				{
					throw new Exception("Property {0} has already been defined in schema.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						text
					}));
				}
				dictionary.Add(text, this.BuildSchema());
			}
			this.CurrentSchema.PatternProperties = dictionary;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003628 File Offset: 0x00001828
		private void ProcessItems()
		{
			this.CurrentSchema.Items = new List<JsonSchema>();
			switch (this._reader.TokenType)
			{
			case JsonToken.StartObject:
				this.CurrentSchema.Items.Add(this.BuildSchema());
				return;
			case JsonToken.StartArray:
				while (this._reader.Read())
				{
					if (this._reader.TokenType == JsonToken.EndArray)
					{
						return;
					}
					this.CurrentSchema.Items.Add(this.BuildSchema());
				}
				return;
			default:
				throw new Exception("Expected array or JSON schema object token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					this._reader.TokenType
				}));
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000036E0 File Offset: 0x000018E0
		private void ProcessProperties()
		{
			IDictionary<string, JsonSchema> dictionary = new Dictionary<string, JsonSchema>();
			if (this._reader.TokenType != JsonToken.StartObject)
			{
				throw new Exception("Expected StartObject token while parsing schema properties, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					this._reader.TokenType
				}));
			}
			while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
			{
				string text = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
				this._reader.Read();
				if (dictionary.ContainsKey(text))
				{
					throw new Exception("Property {0} has already been defined in schema.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						text
					}));
				}
				dictionary.Add(text, this.BuildSchema());
			}
			this.CurrentSchema.Properties = dictionary;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000037B8 File Offset: 0x000019B8
		private JsonSchemaType? ProcessType()
		{
			JsonToken tokenType = this._reader.TokenType;
			if (tokenType == JsonToken.StartArray)
			{
				JsonSchemaType? jsonSchemaType = new JsonSchemaType?(JsonSchemaType.None);
				while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
				{
					if (this._reader.TokenType != JsonToken.String)
					{
						throw new Exception("Exception JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							this._reader.TokenType
						}));
					}
					jsonSchemaType |= JsonSchemaBuilder.MapType(this._reader.Value.ToString());
				}
				return jsonSchemaType;
			}
			if (tokenType == JsonToken.String)
			{
				return new JsonSchemaType?(JsonSchemaBuilder.MapType(this._reader.Value.ToString()));
			}
			throw new Exception("Expected array or JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				this._reader.TokenType
			}));
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000038D4 File Offset: 0x00001AD4
		internal static JsonSchemaType MapType(string type)
		{
			JsonSchemaType result;
			if (!JsonSchemaConstants.JsonSchemaTypeMapping.TryGetValue(type, ref result))
			{
				throw new Exception("Invalid JSON schema type: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type
				}));
			}
			return result;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000392C File Offset: 0x00001B2C
		internal static string MapType(JsonSchemaType type)
		{
			return Enumerable.Single<KeyValuePair<string, JsonSchemaType>>(JsonSchemaConstants.JsonSchemaTypeMapping, (KeyValuePair<string, JsonSchemaType> kv) => kv.Value == type).Key;
		}

		// Token: 0x0400002B RID: 43
		private JsonReader _reader;

		// Token: 0x0400002C RID: 44
		private readonly IList<JsonSchema> _stack;

		// Token: 0x0400002D RID: 45
		private readonly JsonSchemaResolver _resolver;

		// Token: 0x0400002E RID: 46
		private JsonSchema _currentSchema;
	}
}
