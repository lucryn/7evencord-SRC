using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200003D RID: 61
	public class JsonPropertyCollection : KeyedCollection<string, JsonProperty>
	{
		// Token: 0x060003A5 RID: 933 RVA: 0x0000F932 File Offset: 0x0000DB32
		public JsonPropertyCollection(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			this._type = type;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000F94C File Offset: 0x0000DB4C
		protected override string GetKeyForItem(JsonProperty item)
		{
			return item.PropertyName;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000F954 File Offset: 0x0000DB54
		public void AddProperty(JsonProperty property)
		{
			if (base.Contains(property.PropertyName))
			{
				if (property.Ignored)
				{
					return;
				}
				JsonProperty jsonProperty = base[property.PropertyName];
				if (!jsonProperty.Ignored)
				{
					throw new JsonSerializationException("A member with the name '{0}' already exists on '{1}'. Use the JsonPropertyAttribute to specify another name.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						property.PropertyName,
						this._type
					}));
				}
				base.Remove(jsonProperty);
			}
			base.Add(property);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000F9CC File Offset: 0x0000DBCC
		public JsonProperty GetClosestMatchProperty(string propertyName)
		{
			JsonProperty property = this.GetProperty(propertyName, 4);
			if (property == null)
			{
				property = this.GetProperty(propertyName, 5);
			}
			return property;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000F9F0 File Offset: 0x0000DBF0
		public JsonProperty GetProperty(string propertyName, StringComparison comparisonType)
		{
			foreach (JsonProperty jsonProperty in this)
			{
				if (string.Equals(propertyName, jsonProperty.PropertyName, comparisonType))
				{
					return jsonProperty;
				}
			}
			return null;
		}

		// Token: 0x040000F8 RID: 248
		private readonly Type _type;
	}
}
