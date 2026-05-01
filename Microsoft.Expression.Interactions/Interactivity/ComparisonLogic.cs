using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Expression.Interactivity.Core;

namespace Microsoft.Expression.Interactivity
{
	// Token: 0x02000008 RID: 8
	internal static class ComparisonLogic
	{
		// Token: 0x06000043 RID: 67 RVA: 0x000030B8 File Offset: 0x000012B8
		internal static bool EvaluateImpl(object leftOperand, ComparisonConditionType operatorType, object rightOperand)
		{
			bool result = false;
			if (leftOperand != null)
			{
				Type type = leftOperand.GetType();
				if (rightOperand != null)
				{
					TypeConverter typeConverter = TypeConverterHelper.GetTypeConverter(type);
					rightOperand = TypeConverterHelper.DoConversionFrom(typeConverter, rightOperand);
				}
			}
			IComparable comparable = leftOperand as IComparable;
			IComparable comparable2 = rightOperand as IComparable;
			if (comparable != null && comparable2 != null)
			{
				return ComparisonLogic.EvaluateComparable(comparable, operatorType, comparable2);
			}
			switch (operatorType)
			{
			case ComparisonConditionType.Equal:
				result = object.Equals(leftOperand, rightOperand);
				break;
			case ComparisonConditionType.NotEqual:
				result = !object.Equals(leftOperand, rightOperand);
				break;
			case ComparisonConditionType.LessThan:
			case ComparisonConditionType.LessThanOrEqual:
			case ComparisonConditionType.GreaterThan:
			case ComparisonConditionType.GreaterThanOrEqual:
				if (comparable == null && comparable2 == null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.InvalidOperands, new object[]
					{
						(leftOperand != null) ? leftOperand.GetType().Name : "null",
						(rightOperand != null) ? rightOperand.GetType().Name : "null",
						operatorType.ToString()
					}));
				}
				if (comparable == null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.InvalidLeftOperand, new object[]
					{
						(leftOperand != null) ? leftOperand.GetType().Name : "null",
						operatorType.ToString()
					}));
				}
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.InvalidRightOperand, new object[]
				{
					(rightOperand != null) ? rightOperand.GetType().Name : "null",
					operatorType.ToString()
				}));
			}
			return result;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003244 File Offset: 0x00001444
		private static bool EvaluateComparable(IComparable leftOperand, ComparisonConditionType operatorType, IComparable rightOperand)
		{
			object obj = null;
			try
			{
				obj = Convert.ChangeType(rightOperand, leftOperand.GetType(), CultureInfo.CurrentCulture);
			}
			catch (FormatException)
			{
			}
			catch (InvalidCastException)
			{
			}
			if (obj == null)
			{
				return operatorType == ComparisonConditionType.NotEqual;
			}
			int num = leftOperand.CompareTo((IComparable)obj);
			bool result = false;
			switch (operatorType)
			{
			case ComparisonConditionType.Equal:
				result = (num == 0);
				break;
			case ComparisonConditionType.NotEqual:
				result = (num != 0);
				break;
			case ComparisonConditionType.LessThan:
				result = (num < 0);
				break;
			case ComparisonConditionType.LessThanOrEqual:
				result = (num <= 0);
				break;
			case ComparisonConditionType.GreaterThan:
				result = (num > 0);
				break;
			case ComparisonConditionType.GreaterThanOrEqual:
				result = (num >= 0);
				break;
			}
			return result;
		}
	}
}
