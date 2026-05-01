using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005F RID: 95
	internal static class CollectionUtils
	{
		// Token: 0x06000468 RID: 1128 RVA: 0x000122D7 File Offset: 0x000104D7
		public static IEnumerable<T> CastValid<T>(this IEnumerable enumerable)
		{
			ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
			return Enumerable.Cast<T>(Enumerable.Where<object>(Enumerable.Cast<object>(enumerable), (object o) => o is T));
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00012300 File Offset: 0x00010500
		public static List<T> CreateList<T>(params T[] values)
		{
			return new List<T>(values);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00012308 File Offset: 0x00010508
		public static bool IsNullOrEmpty(ICollection collection)
		{
			return collection == null || collection.Count == 0;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00012318 File Offset: 0x00010518
		public static bool IsNullOrEmpty<T>(ICollection<T> collection)
		{
			return collection == null || collection.Count == 0;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00012328 File Offset: 0x00010528
		public static bool IsNullOrEmptyOrDefault<T>(IList<T> list)
		{
			return CollectionUtils.IsNullOrEmpty<T>(list) || ReflectionUtils.ItemsUnitializedValue<T>(list);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0001233C File Offset: 0x0001053C
		public static IList<T> Slice<T>(IList<T> list, int? start, int? end)
		{
			return CollectionUtils.Slice<T>(list, start, end, default(int?));
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0001235C File Offset: 0x0001055C
		public static IList<T> Slice<T>(IList<T> list, int? start, int? end, int? step)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (step == 0)
			{
				throw new ArgumentException("Step cannot be zero.", "step");
			}
			List<T> list2 = new List<T>();
			if (list.Count == 0)
			{
				return list2;
			}
			int num = step ?? 1;
			int num2 = start ?? 0;
			int num3 = end ?? list.Count;
			num2 = ((num2 < 0) ? (list.Count + num2) : num2);
			num3 = ((num3 < 0) ? (list.Count + num3) : num3);
			num2 = Math.Max(num2, 0);
			num3 = Math.Min(num3, list.Count - 1);
			for (int i = num2; i < num3; i += num)
			{
				list2.Add(list[i]);
			}
			return list2;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00012450 File Offset: 0x00010650
		public static Dictionary<K, List<V>> GroupBy<K, V>(ICollection<V> source, Func<V, K> keySelector)
		{
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			Dictionary<K, List<V>> dictionary = new Dictionary<K, List<V>>();
			foreach (V v in source)
			{
				K k = keySelector.Invoke(v);
				List<V> list;
				if (!dictionary.TryGetValue(k, ref list))
				{
					list = new List<V>();
					dictionary.Add(k, list);
				}
				list.Add(v);
			}
			return dictionary;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000124D4 File Offset: 0x000106D4
		public static void AddRange<T>(this IList<T> initial, IEnumerable<T> collection)
		{
			if (initial == null)
			{
				throw new ArgumentNullException("initial");
			}
			if (collection == null)
			{
				return;
			}
			foreach (T t in collection)
			{
				initial.Add(t);
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00012530 File Offset: 0x00010730
		public static void AddRange(this IList initial, IEnumerable collection)
		{
			ValidationUtils.ArgumentNotNull(initial, "initial");
			ListWrapper<object> initial2 = new ListWrapper<object>(initial);
			initial2.AddRange(Enumerable.Cast<object>(collection));
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001255C File Offset: 0x0001075C
		public static List<T> Distinct<T>(List<T> collection)
		{
			List<T> list = new List<T>();
			foreach (T t in collection)
			{
				if (!list.Contains(t))
				{
					list.Add(t);
				}
			}
			return list;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x000125BC File Offset: 0x000107BC
		public static List<List<T>> Flatten<T>(params IList<T>[] lists)
		{
			List<List<T>> list = new List<List<T>>();
			Dictionary<int, T> currentSet = new Dictionary<int, T>();
			CollectionUtils.Recurse<T>(new List<IList<T>>(lists), 0, currentSet, list);
			return list;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000125E4 File Offset: 0x000107E4
		private static void Recurse<T>(IList<IList<T>> global, int current, Dictionary<int, T> currentSet, List<List<T>> flattenedResult)
		{
			IList<T> list = global[current];
			for (int i = 0; i < list.Count; i++)
			{
				currentSet[current] = list[i];
				if (current == global.Count - 1)
				{
					List<T> list2 = new List<T>();
					for (int j = 0; j < currentSet.Count; j++)
					{
						list2.Add(currentSet[j]);
					}
					flattenedResult.Add(list2);
				}
				else
				{
					CollectionUtils.Recurse<T>(global, current + 1, currentSet, flattenedResult);
				}
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0001265C File Offset: 0x0001085C
		public static List<T> CreateList<T>(ICollection collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			T[] array = new T[collection.Count];
			collection.CopyTo(array, 0);
			return new List<T>(array);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00012694 File Offset: 0x00010894
		public static bool ListEquals<T>(IList<T> a, IList<T> b)
		{
			if (a == null || b == null)
			{
				return a == null && b == null;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < a.Count; i++)
			{
				if (!@default.Equals(a[i], b[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x000126F1 File Offset: 0x000108F1
		public static bool TryGetSingleItem<T>(IList<T> list, out T value)
		{
			return CollectionUtils.TryGetSingleItem<T>(list, false, out value);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00012718 File Offset: 0x00010918
		public static bool TryGetSingleItem<T>(IList<T> list, bool returnDefaultIfEmpty, out T value)
		{
			return MiscellaneousUtils.TryAction<T>(() => CollectionUtils.GetSingleItem<T>(list, returnDefaultIfEmpty), out value);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001274B File Offset: 0x0001094B
		public static T GetSingleItem<T>(IList<T> list)
		{
			return CollectionUtils.GetSingleItem<T>(list, false);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00012754 File Offset: 0x00010954
		public static T GetSingleItem<T>(IList<T> list, bool returnDefaultIfEmpty)
		{
			if (list.Count == 1)
			{
				return list[0];
			}
			if (returnDefaultIfEmpty && list.Count == 0)
			{
				return default(T);
			}
			throw new Exception("Expected single {0} in list but got {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeof(T),
				list.Count
			}));
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x000127C0 File Offset: 0x000109C0
		public static IList<T> Minus<T>(IList<T> list, IList<T> minus)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			List<T> list2 = new List<T>(list.Count);
			foreach (T t in list)
			{
				if (minus == null || !minus.Contains(t))
				{
					list2.Add(t);
				}
			}
			return list2;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001282C File Offset: 0x00010A2C
		public static IList CreateGenericList(Type listType)
		{
			ValidationUtils.ArgumentNotNull(listType, "listType");
			return (IList)ReflectionUtils.CreateGeneric(typeof(List), listType, new object[0]);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00012854 File Offset: 0x00010A54
		public static IDictionary CreateGenericDictionary(Type keyType, Type valueType)
		{
			ValidationUtils.ArgumentNotNull(keyType, "keyType");
			ValidationUtils.ArgumentNotNull(valueType, "valueType");
			return (IDictionary)ReflectionUtils.CreateGeneric(typeof(Dictionary), keyType, new object[]
			{
				valueType
			});
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00012898 File Offset: 0x00010A98
		public static bool IsListType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			return type.IsArray || typeof(IList).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof(IList));
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000128D8 File Offset: 0x00010AD8
		public static bool IsCollectionType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			return type.IsArray || typeof(ICollection).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof(ICollection));
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00012918 File Offset: 0x00010B18
		public static bool IsDictionaryType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			return typeof(IDictionary).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof(IDictionary));
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00012994 File Offset: 0x00010B94
		public static IWrappedCollection CreateCollectionWrapper(object list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			Type collectionDefinition;
			if (ReflectionUtils.ImplementsGenericDefinition(list.GetType(), typeof(ICollection), out collectionDefinition))
			{
				Type collectionItemType = ReflectionUtils.GetCollectionItemType(collectionDefinition);
				Func<Type, IList<object>, object> instanceCreator = delegate(Type t, IList<object> a)
				{
					ConstructorInfo constructor = t.GetConstructor(new Type[]
					{
						collectionDefinition
					});
					return constructor.Invoke(new object[]
					{
						list
					});
				};
				return (IWrappedCollection)ReflectionUtils.CreateGeneric(typeof(CollectionWrapper<>), new Type[]
				{
					collectionItemType
				}, instanceCreator, new object[]
				{
					list
				});
			}
			if (list is IList)
			{
				return new CollectionWrapper<object>((IList)list);
			}
			throw new Exception("Can not create ListWrapper for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				list.GetType()
			}));
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00012AC8 File Offset: 0x00010CC8
		public static IWrappedList CreateListWrapper(object list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			Type listDefinition;
			if (ReflectionUtils.ImplementsGenericDefinition(list.GetType(), typeof(IList), out listDefinition))
			{
				Type collectionItemType = ReflectionUtils.GetCollectionItemType(listDefinition);
				Func<Type, IList<object>, object> instanceCreator = delegate(Type t, IList<object> a)
				{
					ConstructorInfo constructor = t.GetConstructor(new Type[]
					{
						listDefinition
					});
					return constructor.Invoke(new object[]
					{
						list
					});
				};
				return (IWrappedList)ReflectionUtils.CreateGeneric(typeof(ListWrapper<>), new Type[]
				{
					collectionItemType
				}, instanceCreator, new object[]
				{
					list
				});
			}
			if (list is IList)
			{
				return new ListWrapper<object>((IList)list);
			}
			throw new Exception("Can not create ListWrapper for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				list.GetType()
			}));
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00012BFC File Offset: 0x00010DFC
		public static IWrappedDictionary CreateDictionaryWrapper(object dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			Type dictionaryDefinition;
			if (ReflectionUtils.ImplementsGenericDefinition(dictionary.GetType(), typeof(IDictionary), out dictionaryDefinition))
			{
				Type dictionaryKeyType = ReflectionUtils.GetDictionaryKeyType(dictionaryDefinition);
				Type dictionaryValueType = ReflectionUtils.GetDictionaryValueType(dictionaryDefinition);
				Func<Type, IList<object>, object> instanceCreator = delegate(Type t, IList<object> a)
				{
					ConstructorInfo constructor = t.GetConstructor(new Type[]
					{
						dictionaryDefinition
					});
					return constructor.Invoke(new object[]
					{
						dictionary
					});
				};
				return (IWrappedDictionary)ReflectionUtils.CreateGeneric(typeof(DictionaryWrapper<, >), new Type[]
				{
					dictionaryKeyType,
					dictionaryValueType
				}, instanceCreator, new object[]
				{
					dictionary
				});
			}
			if (dictionary is IDictionary)
			{
				return new DictionaryWrapper<object, object>((IDictionary)dictionary);
			}
			throw new Exception("Can not create DictionaryWrapper for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				dictionary.GetType()
			}));
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00012D0C File Offset: 0x00010F0C
		public static object CreateAndPopulateList(Type listType, Action<IList, bool> populateList)
		{
			ValidationUtils.ArgumentNotNull(listType, "listType");
			ValidationUtils.ArgumentNotNull(populateList, "populateList");
			bool flag = false;
			IList list;
			Type type;
			if (listType.IsArray)
			{
				list = new List<object>();
				flag = true;
			}
			else if (ReflectionUtils.InheritsGenericDefinition(listType, typeof(ReadOnlyCollection), out type))
			{
				Type type2 = type.GetGenericArguments()[0];
				Type type3 = ReflectionUtils.MakeGenericType(typeof(IEnumerable), new Type[]
				{
					type2
				});
				bool flag2 = false;
				foreach (ConstructorInfo constructorInfo in listType.GetConstructors())
				{
					IList<ParameterInfo> parameters = constructorInfo.GetParameters();
					if (parameters.Count == 1 && type3.IsAssignableFrom(parameters[0].ParameterType))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					throw new Exception("Read-only type {0} does not have a public constructor that takes a type that implements {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						listType,
						type3
					}));
				}
				list = CollectionUtils.CreateGenericList(type2);
				flag = true;
			}
			else if (typeof(IList).IsAssignableFrom(listType))
			{
				if (ReflectionUtils.IsInstantiatableType(listType))
				{
					list = (IList)Activator.CreateInstance(listType);
				}
				else if (listType == typeof(IList))
				{
					list = new List<object>();
				}
				else
				{
					list = null;
				}
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(listType, typeof(ICollection)))
			{
				if (ReflectionUtils.IsInstantiatableType(listType))
				{
					list = CollectionUtils.CreateCollectionWrapper(Activator.CreateInstance(listType));
				}
				else
				{
					list = null;
				}
			}
			else
			{
				list = null;
			}
			if (list == null)
			{
				throw new Exception("Cannot create and populate list type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					listType
				}));
			}
			populateList.Invoke(list, flag);
			if (flag)
			{
				if (listType.IsArray)
				{
					list = CollectionUtils.ToArray(((List<object>)list).ToArray(), ReflectionUtils.GetCollectionItemType(listType));
				}
				else if (ReflectionUtils.InheritsGenericDefinition(listType, typeof(ReadOnlyCollection)))
				{
					list = (IList)ReflectionUtils.CreateInstance(listType, new object[]
					{
						list
					});
				}
			}
			else if (list is IWrappedCollection)
			{
				return ((IWrappedCollection)list).UnderlyingCollection;
			}
			return list;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00012F18 File Offset: 0x00011118
		public static Array ToArray(Array initial, Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Array array = Array.CreateInstance(type, initial.Length);
			Array.Copy(initial, 0, array, 0, initial.Length);
			return array;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00012F50 File Offset: 0x00011150
		public static bool AddDistinct<T>(this IList<T> list, T value)
		{
			return list.AddDistinct(value, EqualityComparer<T>.Default);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00012F5E File Offset: 0x0001115E
		public static bool AddDistinct<T>(this IList<T> list, T value, IEqualityComparer<T> comparer)
		{
			if (list.ContainsValue(value, comparer))
			{
				return false;
			}
			list.Add(value);
			return true;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00012F74 File Offset: 0x00011174
		public static bool ContainsValue<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
		{
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			foreach (TSource tsource in source)
			{
				if (comparer.Equals(tsource, value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00012FE0 File Offset: 0x000111E0
		public static bool AddRangeDistinct<T>(this IList<T> list, IEnumerable<T> values)
		{
			return list.AddRangeDistinct(values, EqualityComparer<T>.Default);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00012FF0 File Offset: 0x000111F0
		public static bool AddRangeDistinct<T>(this IList<T> list, IEnumerable<T> values, IEqualityComparer<T> comparer)
		{
			bool result = true;
			foreach (T value in values)
			{
				if (!list.AddDistinct(value, comparer))
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00013040 File Offset: 0x00011240
		public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
		{
			int num = 0;
			foreach (T t in collection)
			{
				if (predicate.Invoke(t))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00013098 File Offset: 0x00011298
		public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value) where TSource : IEquatable<TSource>
		{
			return list.IndexOf(value, EqualityComparer<TSource>.Default);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x000130A8 File Offset: 0x000112A8
		public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value, IEqualityComparer<TSource> comparer)
		{
			int num = 0;
			foreach (TSource tsource in list)
			{
				if (comparer.Equals(tsource, value))
				{
					return num;
				}
				num++;
			}
			return -1;
		}
	}
}
