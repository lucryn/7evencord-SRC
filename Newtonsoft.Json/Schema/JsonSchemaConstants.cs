using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000AB RID: 171
	internal static class JsonSchemaConstants
	{
		// Token: 0x0600081E RID: 2078 RVA: 0x0001E228 File Offset: 0x0001C428
		// Note: this type is marked as 'beforefieldinit'.
		static JsonSchemaConstants()
		{
			Dictionary<string, JsonSchemaType> dictionary = new Dictionary<string, JsonSchemaType>();
			dictionary.Add("string", JsonSchemaType.String);
			dictionary.Add("object", JsonSchemaType.Object);
			dictionary.Add("integer", JsonSchemaType.Integer);
			dictionary.Add("number", JsonSchemaType.Float);
			dictionary.Add("null", JsonSchemaType.Null);
			dictionary.Add("boolean", JsonSchemaType.Boolean);
			dictionary.Add("array", JsonSchemaType.Array);
			dictionary.Add("any", JsonSchemaType.Any);
			JsonSchemaConstants.JsonSchemaTypeMapping = dictionary;
		}

		// Token: 0x0400026A RID: 618
		public const string TypePropertyName = "type";

		// Token: 0x0400026B RID: 619
		public const string PropertiesPropertyName = "properties";

		// Token: 0x0400026C RID: 620
		public const string ItemsPropertyName = "items";

		// Token: 0x0400026D RID: 621
		public const string RequiredPropertyName = "required";

		// Token: 0x0400026E RID: 622
		public const string PatternPropertiesPropertyName = "patternProperties";

		// Token: 0x0400026F RID: 623
		public const string AdditionalPropertiesPropertyName = "additionalProperties";

		// Token: 0x04000270 RID: 624
		public const string RequiresPropertyName = "requires";

		// Token: 0x04000271 RID: 625
		public const string IdentityPropertyName = "identity";

		// Token: 0x04000272 RID: 626
		public const string MinimumPropertyName = "minimum";

		// Token: 0x04000273 RID: 627
		public const string MaximumPropertyName = "maximum";

		// Token: 0x04000274 RID: 628
		public const string ExclusiveMinimumPropertyName = "exclusiveMinimum";

		// Token: 0x04000275 RID: 629
		public const string ExclusiveMaximumPropertyName = "exclusiveMaximum";

		// Token: 0x04000276 RID: 630
		public const string MinimumItemsPropertyName = "minItems";

		// Token: 0x04000277 RID: 631
		public const string MaximumItemsPropertyName = "maxItems";

		// Token: 0x04000278 RID: 632
		public const string PatternPropertyName = "pattern";

		// Token: 0x04000279 RID: 633
		public const string MaximumLengthPropertyName = "maxLength";

		// Token: 0x0400027A RID: 634
		public const string MinimumLengthPropertyName = "minLength";

		// Token: 0x0400027B RID: 635
		public const string EnumPropertyName = "enum";

		// Token: 0x0400027C RID: 636
		public const string OptionsPropertyName = "options";

		// Token: 0x0400027D RID: 637
		public const string ReadOnlyPropertyName = "readonly";

		// Token: 0x0400027E RID: 638
		public const string TitlePropertyName = "title";

		// Token: 0x0400027F RID: 639
		public const string DescriptionPropertyName = "description";

		// Token: 0x04000280 RID: 640
		public const string FormatPropertyName = "format";

		// Token: 0x04000281 RID: 641
		public const string DefaultPropertyName = "default";

		// Token: 0x04000282 RID: 642
		public const string TransientPropertyName = "transient";

		// Token: 0x04000283 RID: 643
		public const string DivisibleByPropertyName = "divisibleBy";

		// Token: 0x04000284 RID: 644
		public const string HiddenPropertyName = "hidden";

		// Token: 0x04000285 RID: 645
		public const string DisallowPropertyName = "disallow";

		// Token: 0x04000286 RID: 646
		public const string ExtendsPropertyName = "extends";

		// Token: 0x04000287 RID: 647
		public const string IdPropertyName = "id";

		// Token: 0x04000288 RID: 648
		public const string OptionValuePropertyName = "value";

		// Token: 0x04000289 RID: 649
		public const string OptionLabelPropertyName = "label";

		// Token: 0x0400028A RID: 650
		public const string ReferencePropertyName = "$ref";

		// Token: 0x0400028B RID: 651
		public static readonly IDictionary<string, JsonSchemaType> JsonSchemaTypeMapping;
	}
}
