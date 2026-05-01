using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Drawing.Core
{
	// Token: 0x02000027 RID: 39
	internal static class TransformExtensions
	{
		// Token: 0x060001A0 RID: 416 RVA: 0x0000A08C File Offset: 0x0000828C
		public static Point TransformPoint(this IEnumerable<GeneralTransform> transforms, Point point)
		{
			if (transforms == null)
			{
				return point;
			}
			foreach (GeneralTransform transform in transforms)
			{
				point = GeometryHelper.SafeTransform(transform, point);
			}
			return point;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000A0DC File Offset: 0x000082DC
		public static Transform CloneTransform(this Transform transform)
		{
			if (transform == null)
			{
				return null;
			}
			TranslateTransform translateTransform = transform as TranslateTransform;
			if (translateTransform != null)
			{
				return new TranslateTransform
				{
					X = translateTransform.X,
					Y = translateTransform.Y
				};
			}
			RotateTransform rotateTransform = transform as RotateTransform;
			if (rotateTransform != null)
			{
				return new RotateTransform
				{
					Angle = rotateTransform.Angle,
					CenterX = rotateTransform.CenterX,
					CenterY = rotateTransform.CenterY
				};
			}
			ScaleTransform scaleTransform = transform as ScaleTransform;
			if (scaleTransform != null)
			{
				return new ScaleTransform
				{
					ScaleX = scaleTransform.ScaleX,
					ScaleY = scaleTransform.ScaleY,
					CenterX = scaleTransform.CenterX,
					CenterY = scaleTransform.CenterY
				};
			}
			SkewTransform skewTransform = transform as SkewTransform;
			if (skewTransform != null)
			{
				return new SkewTransform
				{
					AngleX = skewTransform.AngleX,
					AngleY = skewTransform.AngleY,
					CenterX = skewTransform.CenterX,
					CenterY = skewTransform.CenterY
				};
			}
			CompositeTransform compositeTransform = transform as CompositeTransform;
			if (compositeTransform != null)
			{
				return new CompositeTransform
				{
					CenterX = compositeTransform.CenterX,
					CenterY = compositeTransform.CenterY,
					Rotation = compositeTransform.Rotation,
					ScaleX = compositeTransform.ScaleX,
					ScaleY = compositeTransform.ScaleY,
					SkewX = compositeTransform.SkewX,
					SkewY = compositeTransform.SkewY,
					TranslateX = compositeTransform.TranslateX,
					TranslateY = compositeTransform.TranslateY
				};
			}
			MatrixTransform matrixTransform = transform as MatrixTransform;
			if (matrixTransform != null)
			{
				return new MatrixTransform
				{
					Matrix = matrixTransform.Matrix
				};
			}
			TransformGroup transformGroup = transform as TransformGroup;
			if (transformGroup != null)
			{
				TransformGroup transformGroup2 = new TransformGroup();
				foreach (Transform transform2 in transformGroup.Children)
				{
					transformGroup2.Children.Add(transform2.CloneTransform());
				}
				return transformGroup2;
			}
			return transform.DeepCopy<Transform>();
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000A314 File Offset: 0x00008514
		public static bool TransformEquals(this Transform firstTransform, Transform secondTransform)
		{
			if (firstTransform == null && secondTransform == null)
			{
				return true;
			}
			if (firstTransform == null || secondTransform == null)
			{
				return false;
			}
			if (firstTransform == secondTransform)
			{
				return true;
			}
			TranslateTransform translateTransform = firstTransform as TranslateTransform;
			TranslateTransform translateTransform2 = secondTransform as TranslateTransform;
			if (translateTransform != null && translateTransform2 != null)
			{
				return TransformExtensions.TranslateTransformEquals(translateTransform, translateTransform2);
			}
			RotateTransform rotateTransform = firstTransform as RotateTransform;
			RotateTransform rotateTransform2 = secondTransform as RotateTransform;
			if (rotateTransform != null && rotateTransform2 != null)
			{
				return TransformExtensions.RotateTransformEquals(rotateTransform, rotateTransform2);
			}
			ScaleTransform scaleTransform = firstTransform as ScaleTransform;
			ScaleTransform scaleTransform2 = secondTransform as ScaleTransform;
			if (scaleTransform != null && scaleTransform2 != null)
			{
				return TransformExtensions.ScaleTransformEquals(scaleTransform, scaleTransform2);
			}
			SkewTransform skewTransform = firstTransform as SkewTransform;
			SkewTransform skewTransform2 = secondTransform as SkewTransform;
			if (skewTransform != null && skewTransform2 != null)
			{
				return TransformExtensions.SkewTransformEquals(skewTransform, skewTransform2);
			}
			MatrixTransform matrixTransform = firstTransform as MatrixTransform;
			MatrixTransform matrixTransform2 = secondTransform as MatrixTransform;
			if (matrixTransform != null && matrixTransform2 != null)
			{
				return TransformExtensions.MatrixTransformEquals(matrixTransform, matrixTransform2);
			}
			TransformGroup transformGroup = firstTransform as TransformGroup;
			TransformGroup transformGroup2 = secondTransform as TransformGroup;
			if (transformGroup != null && transformGroup2 != null)
			{
				return TransformExtensions.TransformGroupEquals(transformGroup, transformGroup2);
			}
			CompositeTransform compositeTransform = firstTransform as CompositeTransform;
			CompositeTransform compositeTransform2 = secondTransform as CompositeTransform;
			if (compositeTransform != null && compositeTransform2 != null)
			{
				return TransformExtensions.CompositeTransformEquals(compositeTransform, compositeTransform2);
			}
			TransformGroup transformGroup3 = new TransformGroup();
			transformGroup3.Children.Add(firstTransform);
			TransformGroup transformGroup4 = new TransformGroup();
			transformGroup4.Children.Add(secondTransform);
			return transformGroup3.Value == transformGroup4.Value;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000A454 File Offset: 0x00008654
		private static bool TranslateTransformEquals(TranslateTransform firstTransform, TranslateTransform secondTransform)
		{
			return firstTransform.X == secondTransform.X && firstTransform.Y == secondTransform.Y;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000A474 File Offset: 0x00008674
		private static bool RotateTransformEquals(RotateTransform firstTransform, RotateTransform secondTransform)
		{
			return firstTransform.Angle == secondTransform.Angle && firstTransform.CenterX == secondTransform.CenterX && firstTransform.CenterY == secondTransform.CenterY;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000A4A2 File Offset: 0x000086A2
		private static bool ScaleTransformEquals(ScaleTransform firstTransform, ScaleTransform secondTransform)
		{
			return firstTransform.ScaleX == secondTransform.ScaleX && firstTransform.ScaleY == secondTransform.ScaleY && firstTransform.CenterX == secondTransform.CenterX && firstTransform.CenterY == secondTransform.CenterY;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000A4DE File Offset: 0x000086DE
		private static bool SkewTransformEquals(SkewTransform firstTransform, SkewTransform secondTransform)
		{
			return firstTransform.AngleX == secondTransform.AngleX && firstTransform.AngleY == secondTransform.AngleY && firstTransform.CenterX == secondTransform.CenterX && firstTransform.CenterY == secondTransform.CenterY;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000A51C File Offset: 0x0000871C
		private static bool CompositeTransformEquals(CompositeTransform firstTransform, CompositeTransform secondTransform)
		{
			return firstTransform.CenterX == secondTransform.CenterX && firstTransform.CenterY == secondTransform.CenterY && firstTransform.Rotation == secondTransform.Rotation && firstTransform.ScaleX == secondTransform.ScaleX && firstTransform.ScaleY == secondTransform.ScaleY && firstTransform.SkewX == secondTransform.SkewX && firstTransform.SkewY == secondTransform.SkewY && firstTransform.TranslateX == secondTransform.TranslateX && firstTransform.TranslateY == secondTransform.TranslateY;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000A5AC File Offset: 0x000087AC
		private static bool TransformGroupEquals(TransformGroup firstTransform, TransformGroup secondTransform)
		{
			if (firstTransform.Children.Count != secondTransform.Children.Count)
			{
				return false;
			}
			for (int i = 0; i < firstTransform.Children.Count; i++)
			{
				if (!firstTransform.Children[i].TransformEquals(secondTransform.Children[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000A60B File Offset: 0x0000880B
		private static bool MatrixTransformEquals(MatrixTransform firstTransform, MatrixTransform secondTransform)
		{
			return firstTransform.Matrix == secondTransform.Matrix;
		}
	}
}
