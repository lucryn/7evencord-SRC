using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000043 RID: 67
	public static class Extensions
	{
		// Token: 0x060003B2 RID: 946 RVA: 0x0000FAD8 File Offset: 0x0000DCD8
		public static IJEnumerable<JToken> Ancestors<T>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return Enumerable.SelectMany<T, JToken>(source, (T j) => j.Ancestors()).AsJEnumerable();
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000FB0B File Offset: 0x0000DD0B
		public static IJEnumerable<JToken> Descendants<T>(this IEnumerable<T> source) where T : JContainer
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return Enumerable.SelectMany<T, JToken>(source, (T j) => j.Descendants()).AsJEnumerable();
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000FB37 File Offset: 0x0000DD37
		public static IJEnumerable<JProperty> Properties(this IEnumerable<JObject> source)
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return Enumerable.SelectMany<JObject, JProperty>(source, (JObject d) => d.Properties()).AsJEnumerable<JProperty>();
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000FB6C File Offset: 0x0000DD6C
		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source, object key)
		{
			return source.Values(key).AsJEnumerable();
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000FB7A File Offset: 0x0000DD7A
		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source)
		{
			return source.Values(null);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000FB83 File Offset: 0x0000DD83
		public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source, object key)
		{
			return source.Values(key);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000FB8C File Offset: 0x0000DD8C
		public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source)
		{
			return source.Values(null);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000FB95 File Offset: 0x0000DD95
		public static U Value<U>(this IEnumerable<JToken> value)
		{
			return value.Value<JToken, U>();
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000FBA0 File Offset: 0x0000DDA0
		public static U Value<T, U>(this IEnumerable<T> value) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(value, "source");
			JToken jtoken = value as JToken;
			if (jtoken == null)
			{
				throw new ArgumentException("Source value must be a JToken.");
			}
			return jtoken.Convert<JToken, U>();
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000FED0 File Offset: 0x0000E0D0
		internal static IEnumerable<U> Values<T, U>(this IEnumerable<T> source, object key) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			foreach (T t2 in source)
			{
				JToken token = t2;
				if (key == null)
				{
					if (token is JValue)
					{
						yield return ((JValue)token).Convert<JValue, U>();
					}
					else
					{
						foreach (JToken t in token.Children())
						{
							yield return t.Convert<JToken, U>();
						}
					}
				}
				else
				{
					JToken value = token[key];
					if (value != null)
					{
						yield return value.Convert<JToken, U>();
					}
				}
			}
			yield break;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000FEF4 File Offset: 0x0000E0F4
		public static IJEnumerable<JToken> Children<T>(this IEnumerable<T> source) where T : JToken
		{
			return source.Children<T, JToken>().AsJEnumerable();
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000FF15 File Offset: 0x0000E115
		public static IEnumerable<U> Children<T, U>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return Enumerable.SelectMany<T, JToken>(source, (T c) => c.Children()).Convert<JToken, U>();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000100E8 File Offset: 0x0000E2E8
		internal static IEnumerable<U> Convert<T, U>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			foreach (T t in source)
			{
				JToken token = t;
				yield return token.Convert<JToken, U>();
			}
			yield break;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00010108 File Offset: 0x0000E308
		internal static U Convert<T, U>(this T token) where T : JToken
		{
			if (token == null)
			{
				return default(U);
			}
			if (token is U && typeof(U) != typeof(IComparable) && typeof(U) != typeof(IFormattable))
			{
				return (U)((object)token);
			}
			JValue jvalue = token as JValue;
			if (jvalue == null)
			{
				throw new InvalidCastException("Cannot cast {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					token.GetType(),
					typeof(T)
				}));
			}
			if (jvalue.Value is U)
			{
				return (U)((object)jvalue.Value);
			}
			Type type = typeof(U);
			if (ReflectionUtils.IsNullableType(type))
			{
				if (jvalue.Value == null)
				{
					return default(U);
				}
				type = Nullable.GetUnderlyingType(type);
			}
			return (U)((object)System.Convert.ChangeType(jvalue.Value, type, CultureInfo.InvariantCulture));
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00010211 File Offset: 0x0000E411
		public static IJEnumerable<JToken> AsJEnumerable(this IEnumerable<JToken> source)
		{
			return source.AsJEnumerable<JToken>();
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00010219 File Offset: 0x0000E419
		public static IJEnumerable<T> AsJEnumerable<T>(this IEnumerable<T> source) where T : JToken
		{
			if (source == null)
			{
				return null;
			}
			if (source is IJEnumerable<T>)
			{
				return (IJEnumerable<T>)source;
			}
			return new JEnumerable<T>(source);
		}
	}
}
