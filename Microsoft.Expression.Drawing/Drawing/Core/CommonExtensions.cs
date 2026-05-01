using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x02000018 RID: 24
	internal static class CommonExtensions
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x00007324 File Offset: 0x00005524
		public static void ForEach(this IEnumerable items, Action<object> action)
		{
			foreach (object obj in items)
			{
				action.Invoke(obj);
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00007374 File Offset: 0x00005574
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (T t in items)
			{
				action.Invoke(t);
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000073BC File Offset: 0x000055BC
		public static void ForEach<T>(this IList<T> list, Action<T, int> action)
		{
			for (int i = 0; i < list.Count; i++)
			{
				action.Invoke(list[i], i);
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000073E8 File Offset: 0x000055E8
		public static bool EnsureListCount<T>(this IList<T> list, int count, Func<T> factory = null)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (list.EnsureListCountAtLeast(count, factory))
			{
				return true;
			}
			if (list.Count > count)
			{
				List<T> list2 = list as List<T>;
				if (list2 != null)
				{
					list2.RemoveRange(count, list.Count - count);
				}
				else
				{
					for (int i = list.Count - 1; i >= count; i--)
					{
						list.RemoveAt(i);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00007460 File Offset: 0x00005660
		public static bool EnsureListCountAtLeast<T>(this IList<T> list, int count, Func<T> factory = null)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (list.Count < count)
			{
				List<T> list2 = list as List<T>;
				if (list2 != null && factory == null)
				{
					list2.AddRange(new T[count - list.Count]);
				}
				else
				{
					for (int i = list.Count; i < count; i++)
					{
						list.Add((factory == null) ? default(T) : factory.Invoke());
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000074E4 File Offset: 0x000056E4
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> newItems)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			List<T> list = collection as List<T>;
			if (list != null)
			{
				list.AddRange(newItems);
				return;
			}
			foreach (T t in newItems)
			{
				collection.Add(t);
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000754C File Offset: 0x0000574C
		public static T Last<T>(this IList<T> list)
		{
			return list[list.Count - 1];
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000755C File Offset: 0x0000575C
		public static void RemoveLast<T>(this IList<T> list)
		{
			list.RemoveAt(list.Count - 1);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00007588 File Offset: 0x00005788
		internal static T DeepCopy<T>(this T obj) where T : class
		{
			if (obj == null)
			{
				return default(T);
			}
			Type type = obj.GetType();
			if (type.IsValueType || type == typeof(string))
			{
				return obj;
			}
			if (typeof(IList).IsAssignableFrom(type))
			{
				IList collection = (IList)Activator.CreateInstance(type);
				((IList)((object)obj)).ForEach(delegate(object o)
				{
					collection.Add(o.DeepCopy<object>());
				});
				return (T)((object)collection);
			}
			if (type.IsClass)
			{
				object obj2 = Activator.CreateInstance(obj.GetType());
				PropertyInfo[] properties = type.GetProperties(20);
				foreach (PropertyInfo propertyInfo in properties)
				{
					if (propertyInfo.CanRead && propertyInfo.CanWrite)
					{
						object value = propertyInfo.GetValue(obj, null);
						object value2 = propertyInfo.GetValue(obj2, null);
						if (value != value2)
						{
							propertyInfo.SetValue(obj2, value.DeepCopy<object>(), null);
						}
					}
				}
				return (T)((object)obj2);
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000076B4 File Offset: 0x000058B4
		public static bool SetIfDifferent(this DependencyObject dependencyObject, DependencyProperty dependencyProperty, object value)
		{
			object value2 = dependencyObject.GetValue(dependencyProperty);
			if (!object.Equals(value2, value))
			{
				dependencyObject.SetValue(dependencyProperty, value);
				return true;
			}
			return false;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000076E0 File Offset: 0x000058E0
		public static bool ClearIfSet(this DependencyObject dependencyObject, DependencyProperty dependencyProperty)
		{
			object obj = dependencyObject.ReadLocalValue(dependencyProperty);
			if (obj != DependencyProperty.UnsetValue)
			{
				dependencyObject.ClearValue(dependencyProperty);
				return true;
			}
			return false;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000078F0 File Offset: 0x00005AF0
		public static IEnumerable<T> FindVisualDesendent<T>(this DependencyObject parent, Func<T, bool> condition) where T : DependencyObject
		{
			Queue<DependencyObject> queue = new Queue<DependencyObject>();
			parent.GetVisualChildren().ForEach(delegate(DependencyObject child)
			{
				queue.Enqueue(child);
			});
			while (queue.Count > 0)
			{
				DependencyObject next = queue.Dequeue();
				next.GetVisualChildren().ForEach(delegate(DependencyObject child)
				{
					queue.Enqueue(child);
				});
				T candidate = next as T;
				if (candidate != null && condition.Invoke(candidate))
				{
					yield return candidate;
				}
			}
			yield break;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00007A28 File Offset: 0x00005C28
		public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject parent)
		{
			int N = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < N; i++)
			{
				yield return VisualTreeHelper.GetChild(parent, i);
			}
			yield break;
		}
	}
}
