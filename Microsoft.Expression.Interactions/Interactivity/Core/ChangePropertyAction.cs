using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x02000005 RID: 5
	public class ChangePropertyAction : TargetedTriggerAction<object>
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002620 File Offset: 0x00000820
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002632 File Offset: 0x00000832
		public string PropertyName
		{
			get
			{
				return (string)base.GetValue(ChangePropertyAction.PropertyNameProperty);
			}
			set
			{
				base.SetValue(ChangePropertyAction.PropertyNameProperty, value);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002640 File Offset: 0x00000840
		// (set) Token: 0x06000026 RID: 38 RVA: 0x0000264D File Offset: 0x0000084D
		public object Value
		{
			get
			{
				return base.GetValue(ChangePropertyAction.ValueProperty);
			}
			set
			{
				base.SetValue(ChangePropertyAction.ValueProperty, value);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000265B File Offset: 0x0000085B
		// (set) Token: 0x06000028 RID: 40 RVA: 0x0000266D File Offset: 0x0000086D
		public Duration Duration
		{
			get
			{
				return (Duration)base.GetValue(ChangePropertyAction.DurationProperty);
			}
			set
			{
				base.SetValue(ChangePropertyAction.DurationProperty, value);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002680 File Offset: 0x00000880
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002692 File Offset: 0x00000892
		public bool Increment
		{
			get
			{
				return (bool)base.GetValue(ChangePropertyAction.IncrementProperty);
			}
			set
			{
				base.SetValue(ChangePropertyAction.IncrementProperty, value);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000026A5 File Offset: 0x000008A5
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000026B7 File Offset: 0x000008B7
		public IEasingFunction Ease
		{
			get
			{
				return (IEasingFunction)base.GetValue(ChangePropertyAction.EaseProperty);
			}
			set
			{
				base.SetValue(ChangePropertyAction.EaseProperty, value);
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000026C8 File Offset: 0x000008C8
		protected override void Invoke(object parameter)
		{
			if (base.AssociatedObject == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.PropertyName))
			{
				return;
			}
			if (base.Target == null)
			{
				return;
			}
			Type type = base.Target.GetType();
			PropertyInfo property = type.GetProperty(this.PropertyName);
			this.ValidateProperty(property);
			object obj = this.Value;
			TypeConverter typeConverter = TypeConverterHelper.GetTypeConverter(property.PropertyType);
			Exception ex = null;
			try
			{
				if (this.Value != null)
				{
					if (typeConverter != null && typeConverter.CanConvertFrom(this.Value.GetType()))
					{
						obj = typeConverter.ConvertFrom(this.Value);
					}
					else
					{
						typeConverter = TypeConverterHelper.GetTypeConverter(this.Value.GetType());
						if (typeConverter != null && typeConverter.CanConvertTo(property.PropertyType))
						{
							obj = typeConverter.ConvertTo(this.Value, property.PropertyType);
						}
					}
				}
				if (this.Duration.HasTimeSpan)
				{
					this.ValidateAnimationPossible(type);
					object currentPropertyValue = ChangePropertyAction.GetCurrentPropertyValue(base.Target, property);
					this.AnimatePropertyChange(property, currentPropertyValue, obj);
				}
				else
				{
					if (this.Increment)
					{
						obj = this.IncrementCurrentValue(property);
					}
					property.SetValue(base.Target, obj, new object[0]);
				}
			}
			catch (FormatException ex2)
			{
				ex = ex2;
			}
			catch (ArgumentException ex3)
			{
				ex = ex3;
			}
			catch (MethodAccessException ex4)
			{
				ex = ex4;
			}
			if (ex != null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.ChangePropertyActionCannotSetValueExceptionMessage, new object[]
				{
					(this.Value != null) ? this.Value.GetType().Name : "null",
					this.PropertyName,
					property.PropertyType.Name
				}), ex);
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000028B4 File Offset: 0x00000AB4
		private void AnimatePropertyChange(PropertyInfo propertyInfo, object fromValue, object newValue)
		{
			Storyboard storyboard = new Storyboard();
			Timeline timeline;
			if (typeof(double).IsAssignableFrom(propertyInfo.PropertyType))
			{
				timeline = this.CreateDoubleAnimation((double)fromValue, (double)newValue);
			}
			else if (typeof(Color).IsAssignableFrom(propertyInfo.PropertyType))
			{
				timeline = this.CreateColorAnimation((Color)fromValue, (Color)newValue);
			}
			else if (typeof(Point).IsAssignableFrom(propertyInfo.PropertyType))
			{
				timeline = this.CreatePointAnimation((Point)fromValue, (Point)newValue);
			}
			else
			{
				timeline = this.CreateKeyFrameAnimation(fromValue, newValue);
			}
			timeline.Duration = this.Duration;
			storyboard.Children.Add(timeline);
			Storyboard.SetTarget(storyboard, (DependencyObject)base.Target);
			Storyboard.SetTargetProperty(storyboard, new PropertyPath(propertyInfo.Name, new object[0]));
			storyboard.Completed += delegate(object o, EventArgs e)
			{
				propertyInfo.SetValue(this.Target, newValue, new object[0]);
			};
			storyboard.FillBehavior = 1;
			storyboard.Begin();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000029F8 File Offset: 0x00000BF8
		private static object GetCurrentPropertyValue(object target, PropertyInfo propertyInfo)
		{
			FrameworkElement frameworkElement = target as FrameworkElement;
			target.GetType();
			object obj = propertyInfo.GetValue(target, null);
			if (frameworkElement != null && (propertyInfo.Name == "Width" || propertyInfo.Name == "Height") && double.IsNaN((double)obj))
			{
				if (propertyInfo.Name == "Width")
				{
					obj = frameworkElement.ActualWidth;
				}
				else
				{
					obj = frameworkElement.ActualHeight;
				}
			}
			return obj;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002A80 File Offset: 0x00000C80
		private void ValidateAnimationPossible(Type targetType)
		{
			if (this.Increment)
			{
				throw new InvalidOperationException(ExceptionStringTable.ChangePropertyActionCannotIncrementAnimatedPropertyChangeExceptionMessage);
			}
			if (!typeof(DependencyObject).IsAssignableFrom(targetType))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.ChangePropertyActionCannotAnimateTargetTypeExceptionMessage, new object[]
				{
					targetType.Name
				}));
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002AD8 File Offset: 0x00000CD8
		private Timeline CreateKeyFrameAnimation(object newValue, object fromValue)
		{
			ObjectAnimationUsingKeyFrames objectAnimationUsingKeyFrames = new ObjectAnimationUsingKeyFrames();
			DiscreteObjectKeyFrame discreteObjectKeyFrame = new DiscreteObjectKeyFrame
			{
				KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0L)),
				Value = fromValue
			};
			DiscreteObjectKeyFrame discreteObjectKeyFrame2 = new DiscreteObjectKeyFrame
			{
				KeyTime = KeyTime.FromTimeSpan(this.Duration.TimeSpan),
				Value = newValue
			};
			objectAnimationUsingKeyFrames.KeyFrames.Add(discreteObjectKeyFrame);
			objectAnimationUsingKeyFrames.KeyFrames.Add(discreteObjectKeyFrame2);
			return objectAnimationUsingKeyFrames;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002B54 File Offset: 0x00000D54
		private Timeline CreatePointAnimation(Point fromValue, Point newValue)
		{
			return new PointAnimation
			{
				From = new Point?(fromValue),
				To = new Point?(newValue),
				EasingFunction = this.Ease
			};
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002B8C File Offset: 0x00000D8C
		private Timeline CreateColorAnimation(Color fromValue, Color newValue)
		{
			return new ColorAnimation
			{
				From = new Color?(fromValue),
				To = new Color?(newValue),
				EasingFunction = this.Ease
			};
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002BC4 File Offset: 0x00000DC4
		private Timeline CreateDoubleAnimation(double fromValue, double newValue)
		{
			return new DoubleAnimation
			{
				From = new double?(fromValue),
				To = new double?(newValue),
				EasingFunction = this.Ease
			};
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002BFC File Offset: 0x00000DFC
		private void ValidateProperty(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.ChangePropertyActionCannotFindPropertyNameExceptionMessage, new object[]
				{
					this.PropertyName,
					base.Target.GetType().Name
				}));
			}
			if (!propertyInfo.CanWrite)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.ChangePropertyActionPropertyIsReadOnlyExceptionMessage, new object[]
				{
					this.PropertyName,
					base.Target.GetType().Name
				}));
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002C88 File Offset: 0x00000E88
		private object IncrementCurrentValue(PropertyInfo propertyInfo)
		{
			if (!propertyInfo.CanRead)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.ChangePropertyActionCannotIncrementWriteOnlyPropertyExceptionMessage, new object[]
				{
					propertyInfo.Name
				}));
			}
			object value = propertyInfo.GetValue(base.Target, null);
			Type propertyType = propertyInfo.PropertyType;
			TypeConverter typeConverter = TypeConverterHelper.GetTypeConverter(propertyInfo.PropertyType);
			object obj = this.Value;
			if (obj == null || value == null)
			{
				return obj;
			}
			if (typeConverter.CanConvertFrom(obj.GetType()))
			{
				obj = TypeConverterHelper.DoConversionFrom(typeConverter, obj);
			}
			object result;
			if (typeof(double).IsAssignableFrom(propertyType))
			{
				result = (double)value + (double)obj;
			}
			else if (typeof(int).IsAssignableFrom(propertyType))
			{
				result = (int)value + (int)obj;
			}
			else if (typeof(float).IsAssignableFrom(propertyType))
			{
				result = (float)value + (float)obj;
			}
			else if (typeof(string).IsAssignableFrom(propertyType))
			{
				result = (string)value + (string)obj;
			}
			else
			{
				result = ChangePropertyAction.TryAddition(value, obj);
			}
			return result;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002DC0 File Offset: 0x00000FC0
		private static object TryAddition(object currentValue, object value)
		{
			Type type = value.GetType();
			Type type2 = currentValue.GetType();
			MethodInfo methodInfo = null;
			object obj = value;
			foreach (MethodInfo methodInfo2 in type2.GetMethods())
			{
				if (string.Compare(methodInfo2.Name, "op_Addition", 4) == 0)
				{
					ParameterInfo[] parameters = methodInfo2.GetParameters();
					Type parameterType = parameters[1].ParameterType;
					if (parameters[0].ParameterType.IsAssignableFrom(type2))
					{
						if (!parameterType.IsAssignableFrom(type))
						{
							TypeConverter typeConverter = TypeConverterHelper.GetTypeConverter(parameterType);
							if (!typeConverter.CanConvertFrom(type))
							{
								goto IL_BB;
							}
							obj = TypeConverterHelper.DoConversionFrom(typeConverter, value);
						}
						if (methodInfo != null)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.ChangePropertyActionAmbiguousAdditionOperationExceptionMessage, new object[]
							{
								type2.Name
							}));
						}
						methodInfo = methodInfo2;
					}
				}
				IL_BB:;
			}
			object result;
			if (methodInfo != null)
			{
				result = methodInfo.Invoke(null, new object[]
				{
					currentValue,
					obj
				});
			}
			else
			{
				result = value;
			}
			return result;
		}

		// Token: 0x0400000A RID: 10
		public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(ChangePropertyAction), null);

		// Token: 0x0400000B RID: 11
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(ChangePropertyAction), null);

		// Token: 0x0400000C RID: 12
		public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(Duration), typeof(ChangePropertyAction), null);

		// Token: 0x0400000D RID: 13
		public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(bool), typeof(ChangePropertyAction), null);

		// Token: 0x0400000E RID: 14
		public static readonly DependencyProperty EaseProperty = DependencyProperty.Register("Ease", typeof(IEasingFunction), typeof(ChangePropertyAction), null);
	}
}
