using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200005D RID: 93
	public static class Extensions
	{
		// Token: 0x06000456 RID: 1110 RVA: 0x00011C74 File Offset: 0x0000FE74
		public static bool IsValid(this JToken source, JsonSchema schema)
		{
			bool valid = true;
			source.Validate(schema, delegate(object sender, ValidationEventArgs args)
			{
				valid = false;
			});
			return valid;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00011CA7 File Offset: 0x0000FEA7
		public static void Validate(this JToken source, JsonSchema schema)
		{
			source.Validate(schema, null);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00011CB4 File Offset: 0x0000FEB4
		public static void Validate(this JToken source, JsonSchema schema, ValidationEventHandler validationEventHandler)
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			ValidationUtils.ArgumentNotNull(schema, "schema");
			using (JsonValidatingReader jsonValidatingReader = new JsonValidatingReader(source.CreateReader()))
			{
				jsonValidatingReader.Schema = schema;
				if (validationEventHandler != null)
				{
					jsonValidatingReader.ValidationEventHandler += validationEventHandler;
				}
				while (jsonValidatingReader.Read())
				{
				}
			}
		}
	}
}
