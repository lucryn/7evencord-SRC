using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000069 RID: 105
	internal static class ConvertUtils
	{
		// Token: 0x060004F2 RID: 1266 RVA: 0x00015B00 File Offset: 0x00013D00
		private static Func<object, object> CreateCastConverter(ConvertUtils.TypeConvertKey t)
		{
			MethodInfo method = t.TargetType.GetMethod("op_Implicit", new Type[]
			{
				t.InitialType
			});
			if (method == null)
			{
				method = t.TargetType.GetMethod("op_Explicit", new Type[]
				{
					t.InitialType
				});
			}
			if (method == null)
			{
				return null;
			}
			MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return (object o) => call(null, new object[]
			{
				o
			});
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00015B80 File Offset: 0x00013D80
		public static bool CanConvertType(Type initialType, Type targetType, bool allowTypeNameToString)
		{
			ValidationUtils.ArgumentNotNull(initialType, "initialType");
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			if (ReflectionUtils.IsNullableType(targetType))
			{
				targetType = Nullable.GetUnderlyingType(targetType);
			}
			if (targetType == initialType)
			{
				return true;
			}
			if (typeof(IConvertible).IsAssignableFrom(initialType) && typeof(IConvertible).IsAssignableFrom(targetType))
			{
				return true;
			}
			if (initialType == typeof(DateTime) && targetType == typeof(DateTimeOffset))
			{
				return true;
			}
			if (initialType == typeof(Guid) && (targetType == typeof(Guid) || targetType == typeof(string)))
			{
				return true;
			}
			if (initialType == typeof(Type) && targetType == typeof(string))
			{
				return true;
			}
			TypeConverter converter = ConvertUtils.GetConverter(initialType);
			if (converter != null && !ConvertUtils.IsComponentConverter(converter) && converter.CanConvertTo(targetType) && (allowTypeNameToString || converter.GetType() != typeof(TypeConverter)))
			{
				return true;
			}
			TypeConverter converter2 = ConvertUtils.GetConverter(targetType);
			return (converter2 != null && !ConvertUtils.IsComponentConverter(converter2) && converter2.CanConvertFrom(initialType)) || (initialType == typeof(DBNull) && ReflectionUtils.IsNullable(targetType));
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00015CA7 File Offset: 0x00013EA7
		private static bool IsComponentConverter(TypeConverter converter)
		{
			return false;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00015CAA File Offset: 0x00013EAA
		public static T Convert<T>(object initialValue)
		{
			return ConvertUtils.Convert<T>(initialValue, CultureInfo.CurrentCulture);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00015CB7 File Offset: 0x00013EB7
		public static T Convert<T>(object initialValue, CultureInfo culture)
		{
			return (T)((object)ConvertUtils.Convert(initialValue, culture, typeof(T)));
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00015CD0 File Offset: 0x00013ED0
		public static object Convert(object initialValue, CultureInfo culture, Type targetType)
		{
			if (initialValue == null)
			{
				throw new ArgumentNullException("initialValue");
			}
			if (ReflectionUtils.IsNullableType(targetType))
			{
				targetType = Nullable.GetUnderlyingType(targetType);
			}
			Type type = initialValue.GetType();
			if (targetType == type)
			{
				return initialValue;
			}
			if (initialValue is string && typeof(Type).IsAssignableFrom(targetType))
			{
				return Type.GetType((string)initialValue, true);
			}
			if (targetType.IsInterface || targetType.IsGenericTypeDefinition || targetType.IsAbstract)
			{
				throw new ArgumentException("Target type {0} is not a value type or a non-abstract class.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					targetType
				}), "targetType");
			}
			if (initialValue is IConvertible && typeof(IConvertible).IsAssignableFrom(targetType))
			{
				if (targetType.IsEnum)
				{
					if (initialValue is string)
					{
						return Enum.Parse(targetType, initialValue.ToString(), true);
					}
					if (ConvertUtils.IsInteger(initialValue))
					{
						return Enum.ToObject(targetType, initialValue);
					}
				}
				return System.Convert.ChangeType(initialValue, targetType, culture);
			}
			if (initialValue is DateTime && targetType == typeof(DateTimeOffset))
			{
				return new DateTimeOffset((DateTime)initialValue);
			}
			if (initialValue is string)
			{
				if (targetType == typeof(Guid))
				{
					return new Guid((string)initialValue);
				}
				if (targetType == typeof(Uri))
				{
					return new Uri((string)initialValue);
				}
				if (targetType == typeof(TimeSpan))
				{
					return TimeSpan.Parse((string)initialValue);
				}
			}
			TypeConverter converter = ConvertUtils.GetConverter(type);
			if (converter != null && converter.CanConvertTo(targetType))
			{
				return converter.ConvertTo(initialValue, targetType);
			}
			TypeConverter converter2 = ConvertUtils.GetConverter(targetType);
			if (converter2 != null && converter2.CanConvertFrom(type))
			{
				return converter2.ConvertFrom(initialValue);
			}
			if (initialValue != DBNull.Value)
			{
				throw new Exception("Can not convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type,
					targetType
				}));
			}
			if (ReflectionUtils.IsNullable(targetType))
			{
				return ConvertUtils.EnsureTypeAssignable(null, type, targetType);
			}
			throw new Exception("Can not convert null {0} into non-nullable {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				type,
				targetType
			}));
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00015EE2 File Offset: 0x000140E2
		public static bool TryConvert<T>(object initialValue, out T convertedValue)
		{
			return ConvertUtils.TryConvert<T>(initialValue, CultureInfo.CurrentCulture, out convertedValue);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00015F28 File Offset: 0x00014128
		public static bool TryConvert<T>(object initialValue, CultureInfo culture, out T convertedValue)
		{
			return MiscellaneousUtils.TryAction<T>(delegate
			{
				object obj;
				ConvertUtils.TryConvert(initialValue, CultureInfo.CurrentCulture, typeof(T), out obj);
				return (T)((object)obj);
			}, out convertedValue);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00015F78 File Offset: 0x00014178
		public static bool TryConvert(object initialValue, CultureInfo culture, Type targetType, out object convertedValue)
		{
			return MiscellaneousUtils.TryAction<object>(() => ConvertUtils.Convert(initialValue, culture, targetType), out convertedValue);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00015FB2 File Offset: 0x000141B2
		public static T ConvertOrCast<T>(object initialValue)
		{
			return ConvertUtils.ConvertOrCast<T>(initialValue, CultureInfo.CurrentCulture);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00015FBF File Offset: 0x000141BF
		public static T ConvertOrCast<T>(object initialValue, CultureInfo culture)
		{
			return (T)((object)ConvertUtils.ConvertOrCast(initialValue, culture, typeof(T)));
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00015FD8 File Offset: 0x000141D8
		public static object ConvertOrCast(object initialValue, CultureInfo culture, Type targetType)
		{
			if (targetType == typeof(object))
			{
				return initialValue;
			}
			if (initialValue == null && ReflectionUtils.IsNullable(targetType))
			{
				return null;
			}
			object result;
			if (ConvertUtils.TryConvert(initialValue, culture, targetType, out result))
			{
				return result;
			}
			return ConvertUtils.EnsureTypeAssignable(initialValue, ReflectionUtils.GetObjectType(initialValue), targetType);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001601C File Offset: 0x0001421C
		public static bool TryConvertOrCast<T>(object initialValue, out T convertedValue)
		{
			return ConvertUtils.TryConvertOrCast<T>(initialValue, CultureInfo.CurrentCulture, out convertedValue);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00016064 File Offset: 0x00014264
		public static bool TryConvertOrCast<T>(object initialValue, CultureInfo culture, out T convertedValue)
		{
			return MiscellaneousUtils.TryAction<T>(delegate
			{
				object obj;
				ConvertUtils.TryConvertOrCast(initialValue, CultureInfo.CurrentCulture, typeof(T), out obj);
				return (T)((object)obj);
			}, out convertedValue);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x000160B4 File Offset: 0x000142B4
		public static bool TryConvertOrCast(object initialValue, CultureInfo culture, Type targetType, out object convertedValue)
		{
			return MiscellaneousUtils.TryAction<object>(() => ConvertUtils.ConvertOrCast(initialValue, culture, targetType), out convertedValue);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x000160F0 File Offset: 0x000142F0
		private static object EnsureTypeAssignable(object value, Type initialType, Type targetType)
		{
			Type type = (value != null) ? value.GetType() : null;
			if (value != null)
			{
				if (targetType.IsAssignableFrom(type))
				{
					return value;
				}
				Func<object, object> func = ConvertUtils.CastConverters.Get(new ConvertUtils.TypeConvertKey(type, targetType));
				if (func != null)
				{
					return func.Invoke(value);
				}
			}
			else if (ReflectionUtils.IsNullable(targetType))
			{
				return null;
			}
			throw new Exception("Could not cast or convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				(initialType != null) ? initialType.ToString() : "{null}",
				targetType
			}));
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00016172 File Offset: 0x00014372
		internal static TypeConverter GetConverter(Type t)
		{
			return JsonTypeReflector.GetTypeConverter(t);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001617C File Offset: 0x0001437C
		public static bool IsInteger(object value)
		{
			switch (System.Convert.GetTypeCode(value))
			{
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x0400013F RID: 319
		private static readonly ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>> CastConverters = new ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>>(new Func<ConvertUtils.TypeConvertKey, Func<object, object>>(ConvertUtils.CreateCastConverter));

		// Token: 0x0200006A RID: 106
		internal struct TypeConvertKey : IEquatable<ConvertUtils.TypeConvertKey>
		{
			// Token: 0x170000DD RID: 221
			// (get) Token: 0x06000505 RID: 1285 RVA: 0x000161D5 File Offset: 0x000143D5
			public Type InitialType
			{
				get
				{
					return this._initialType;
				}
			}

			// Token: 0x170000DE RID: 222
			// (get) Token: 0x06000506 RID: 1286 RVA: 0x000161DD File Offset: 0x000143DD
			public Type TargetType
			{
				get
				{
					return this._targetType;
				}
			}

			// Token: 0x06000507 RID: 1287 RVA: 0x000161E5 File Offset: 0x000143E5
			public TypeConvertKey(Type initialType, Type targetType)
			{
				this._initialType = initialType;
				this._targetType = targetType;
			}

			// Token: 0x06000508 RID: 1288 RVA: 0x000161F5 File Offset: 0x000143F5
			public override int GetHashCode()
			{
				return this._initialType.GetHashCode() ^ this._targetType.GetHashCode();
			}

			// Token: 0x06000509 RID: 1289 RVA: 0x0001620E File Offset: 0x0001440E
			public override bool Equals(object obj)
			{
				return obj is ConvertUtils.TypeConvertKey && this.Equals((ConvertUtils.TypeConvertKey)obj);
			}

			// Token: 0x0600050A RID: 1290 RVA: 0x00016226 File Offset: 0x00014426
			public bool Equals(ConvertUtils.TypeConvertKey other)
			{
				return this._initialType == other._initialType && this._targetType == other._targetType;
			}

			// Token: 0x04000140 RID: 320
			private readonly Type _initialType;

			// Token: 0x04000141 RID: 321
			private readonly Type _targetType;
		}
	}
}
