using System;
using System.Windows;

namespace Microsoft.Expression.Media
{
	// Token: 0x0200003E RID: 62
	internal static class IGeometrySourceExtensions
	{
		// Token: 0x06000239 RID: 569 RVA: 0x0000DF30 File Offset: 0x0000C130
		public static double GetHalfStrokeThickness(this IGeometrySourceParameters parameter)
		{
			if (parameter.Stroke != null)
			{
				double strokeThickness = parameter.StrokeThickness;
				if (!double.IsNaN(strokeThickness) && !double.IsInfinity(strokeThickness))
				{
					return Math.Abs(strokeThickness) / 2.0;
				}
			}
			return 0.0;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000DF78 File Offset: 0x0000C178
		public static GeometryEffect GetGeometryEffect(this IGeometrySourceParameters parameters)
		{
			DependencyObject dependencyObject = parameters as DependencyObject;
			if (dependencyObject == null)
			{
				return null;
			}
			GeometryEffect geometryEffect = GeometryEffect.GetGeometryEffect(dependencyObject);
			if (geometryEffect == null || !dependencyObject.Equals(geometryEffect.Parent))
			{
				return null;
			}
			return geometryEffect;
		}
	}
}
