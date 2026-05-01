using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Microsoft.Expression.Interactivity
{
	// Token: 0x0200000E RID: 14
	internal static class DataBindingHelper
	{
		// Token: 0x06000054 RID: 84 RVA: 0x000034D4 File Offset: 0x000016D4
		public static void EnsureDataBindingUpToDateOnMembers(DependencyObject dpObject)
		{
			IList<DependencyProperty> list = null;
			if (!DataBindingHelper.DependenciesPropertyCache.TryGetValue(dpObject.GetType(), ref list))
			{
				list = new List<DependencyProperty>();
				for (Type type = dpObject.GetType(); type != null; type = type.BaseType)
				{
					FieldInfo[] fields = type.GetFields();
					foreach (FieldInfo fieldInfo in fields)
					{
						if (fieldInfo.IsPublic && fieldInfo.FieldType == typeof(DependencyProperty))
						{
							DependencyProperty dependencyProperty = fieldInfo.GetValue(null) as DependencyProperty;
							if (dependencyProperty != null)
							{
								list.Add(dependencyProperty);
							}
						}
					}
				}
				DataBindingHelper.DependenciesPropertyCache[dpObject.GetType()] = list;
			}
			if (list == null)
			{
				return;
			}
			foreach (DependencyProperty dp in list)
			{
				DataBindingHelper.EnsureBindingUpToDate(dpObject, dp);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000035C4 File Offset: 0x000017C4
		public static void EnsureDataBindingOnActionsUpToDate(TriggerBase<DependencyObject> trigger)
		{
			foreach (TriggerAction dpObject in trigger.Actions)
			{
				DataBindingHelper.EnsureDataBindingUpToDateOnMembers(dpObject);
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003610 File Offset: 0x00001810
		public static void EnsureBindingUpToDate(DependencyObject target, DependencyProperty dp)
		{
			BindingExpression bindingExpression = target.ReadLocalValue(dp) as BindingExpression;
			if (bindingExpression != null)
			{
				target.ClearValue(dp);
				target.SetValue(dp, bindingExpression);
			}
		}

		// Token: 0x0400001F RID: 31
		private static Dictionary<Type, IList<DependencyProperty>> DependenciesPropertyCache = new Dictionary<Type, IList<DependencyProperty>>();
	}
}
