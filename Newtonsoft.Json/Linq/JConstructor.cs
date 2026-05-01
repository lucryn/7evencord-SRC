using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000052 RID: 82
	public class JConstructor : JContainer
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00010EC0 File Offset: 0x0000F0C0
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00010EC8 File Offset: 0x0000F0C8
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x00010ED0 File Offset: 0x0000F0D0
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00010ED9 File Offset: 0x0000F0D9
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Constructor;
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00010EDC File Offset: 0x0000F0DC
		public JConstructor()
		{
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00010EEF File Offset: 0x0000F0EF
		public JConstructor(JConstructor other) : base(other)
		{
			this._name = other.Name;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00010F0F File Offset: 0x0000F10F
		public JConstructor(string name, params object[] content) : this(name, content)
		{
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00010F19 File Offset: 0x0000F119
		public JConstructor(string name, object content) : this(name)
		{
			this.Add(content);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00010F29 File Offset: 0x0000F129
		public JConstructor(string name)
		{
			ValidationUtils.ArgumentNotNullOrEmpty(name, "name");
			this._name = name;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00010F50 File Offset: 0x0000F150
		internal override bool DeepEquals(JToken node)
		{
			JConstructor jconstructor = node as JConstructor;
			return jconstructor != null && this._name == jconstructor.Name && base.ContentsEqual(jconstructor);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00010F83 File Offset: 0x0000F183
		internal override JToken CloneToken()
		{
			return new JConstructor(this);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00010F8C File Offset: 0x0000F18C
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartConstructor(this._name);
			foreach (JToken jtoken in this.Children())
			{
				jtoken.WriteTo(writer, converters);
			}
			writer.WriteEndConstructor();
		}

		// Token: 0x170000CD RID: 205
		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				return this.GetItem((int)key);
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				this.SetItem((int)key, value);
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00011097 File Offset: 0x0000F297
		internal override int GetDeepHashCode()
		{
			return this._name.GetHashCode() ^ base.ContentsHashCode();
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000110AC File Offset: 0x0000F2AC
		public new static JConstructor Load(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw new Exception("Error reading JConstructor from JsonReader.");
			}
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw new Exception("Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					reader.TokenType
				}));
			}
			JConstructor jconstructor = new JConstructor((string)reader.Value);
			jconstructor.SetLineInfo(reader as IJsonLineInfo);
			jconstructor.ReadTokenFrom(reader);
			return jconstructor;
		}

		// Token: 0x04000118 RID: 280
		private string _name;

		// Token: 0x04000119 RID: 281
		private IList<JToken> _values = new List<JToken>();
	}
}
