using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using Microsoft.Expression.Interactivity.Core;

namespace Microsoft.Expression.Interactivity.Layout
{
	// Token: 0x0200001F RID: 31
	public class MouseDragElementBehavior : Behavior<FrameworkElement>
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600011A RID: 282 RVA: 0x00007880 File Offset: 0x00005A80
		// (remove) Token: 0x0600011B RID: 283 RVA: 0x000078B8 File Offset: 0x00005AB8
		public event MouseEventHandler DragBegun;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600011C RID: 284 RVA: 0x000078F0 File Offset: 0x00005AF0
		// (remove) Token: 0x0600011D RID: 285 RVA: 0x00007928 File Offset: 0x00005B28
		public event MouseEventHandler Dragging;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600011E RID: 286 RVA: 0x00007960 File Offset: 0x00005B60
		// (remove) Token: 0x0600011F RID: 287 RVA: 0x00007998 File Offset: 0x00005B98
		public event MouseEventHandler DragFinished;

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000120 RID: 288 RVA: 0x000079CD File Offset: 0x00005BCD
		// (set) Token: 0x06000121 RID: 289 RVA: 0x000079DF File Offset: 0x00005BDF
		public double X
		{
			get
			{
				return (double)base.GetValue(MouseDragElementBehavior.XProperty);
			}
			set
			{
				base.SetValue(MouseDragElementBehavior.XProperty, value);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000122 RID: 290 RVA: 0x000079F2 File Offset: 0x00005BF2
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00007A04 File Offset: 0x00005C04
		public double Y
		{
			get
			{
				return (double)base.GetValue(MouseDragElementBehavior.YProperty);
			}
			set
			{
				base.SetValue(MouseDragElementBehavior.YProperty, value);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00007A17 File Offset: 0x00005C17
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00007A29 File Offset: 0x00005C29
		public bool ConstrainToParentBounds
		{
			get
			{
				return (bool)base.GetValue(MouseDragElementBehavior.ConstrainToParentBoundsProperty);
			}
			set
			{
				base.SetValue(MouseDragElementBehavior.ConstrainToParentBoundsProperty, value);
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00007A3C File Offset: 0x00005C3C
		private static void OnXChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			MouseDragElementBehavior mouseDragElementBehavior = (MouseDragElementBehavior)sender;
			mouseDragElementBehavior.UpdatePosition(new Point((double)args.NewValue, mouseDragElementBehavior.Y));
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00007A70 File Offset: 0x00005C70
		private static void OnYChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			MouseDragElementBehavior mouseDragElementBehavior = (MouseDragElementBehavior)sender;
			mouseDragElementBehavior.UpdatePosition(new Point(mouseDragElementBehavior.X, (double)args.NewValue));
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007AA4 File Offset: 0x00005CA4
		private static void OnConstrainToParentBoundsChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			MouseDragElementBehavior mouseDragElementBehavior = (MouseDragElementBehavior)sender;
			mouseDragElementBehavior.UpdatePosition(new Point(mouseDragElementBehavior.X, mouseDragElementBehavior.Y));
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00007AD0 File Offset: 0x00005CD0
		private Point ActualPosition
		{
			get
			{
				GeneralTransform transform = base.AssociatedObject.TransformToVisual(this.RootElement);
				Point transformOffset = MouseDragElementBehavior.GetTransformOffset(transform);
				return new Point(transformOffset.X, transformOffset.Y);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00007B0C File Offset: 0x00005D0C
		private Rect ElementBounds
		{
			get
			{
				Rect layoutRect = ExtendedVisualStateManager.GetLayoutRect(base.AssociatedObject);
				return new Rect(new Point(0.0, 0.0), new Size(layoutRect.Width, layoutRect.Height));
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00007B54 File Offset: 0x00005D54
		private FrameworkElement ParentElement
		{
			get
			{
				return base.AssociatedObject.Parent as FrameworkElement;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00007B68 File Offset: 0x00005D68
		private UIElement RootElement
		{
			get
			{
				DependencyObject dependencyObject = base.AssociatedObject;
				for (DependencyObject dependencyObject2 = dependencyObject; dependencyObject2 != null; dependencyObject2 = VisualTreeHelper.GetParent(dependencyObject))
				{
					dependencyObject = dependencyObject2;
				}
				return dependencyObject as UIElement;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00007B94 File Offset: 0x00005D94
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00007BDF File Offset: 0x00005DDF
		private Transform RenderTransform
		{
			get
			{
				if (this.cachedRenderTransform == null || !object.ReferenceEquals(this.cachedRenderTransform, base.AssociatedObject.RenderTransform))
				{
					Transform renderTransform = MouseDragElementBehavior.CloneTransform(base.AssociatedObject.RenderTransform);
					this.RenderTransform = renderTransform;
				}
				return this.cachedRenderTransform;
			}
			set
			{
				if (this.cachedRenderTransform != value)
				{
					this.cachedRenderTransform = value;
					base.AssociatedObject.RenderTransform = value;
				}
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00007C00 File Offset: 0x00005E00
		private void UpdatePosition(Point point)
		{
			if (!this.settingPosition && base.AssociatedObject != null)
			{
				GeneralTransform transform = base.AssociatedObject.TransformToVisual(this.RootElement);
				Point transformOffset = MouseDragElementBehavior.GetTransformOffset(transform);
				double x = double.IsNaN(point.X) ? 0.0 : (point.X - transformOffset.X);
				double y = double.IsNaN(point.Y) ? 0.0 : (point.Y - transformOffset.Y);
				this.ApplyTranslation(x, y);
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00007C90 File Offset: 0x00005E90
		private void ApplyTranslation(double x, double y)
		{
			if (this.ParentElement != null)
			{
				GeneralTransform transform = this.RootElement.TransformToVisual(this.ParentElement);
				Point point = MouseDragElementBehavior.TransformAsVector(transform, x, y);
				x = point.X;
				y = point.Y;
				if (this.ConstrainToParentBounds)
				{
					FrameworkElement parentElement = this.ParentElement;
					Rect rect;
					rect..ctor(0.0, 0.0, parentElement.ActualWidth, parentElement.ActualHeight);
					GeneralTransform generalTransform = base.AssociatedObject.TransformToVisual(parentElement);
					Rect rect2 = this.ElementBounds;
					rect2 = generalTransform.TransformBounds(rect2);
					Rect rect3 = rect2;
					rect3.X += x;
					rect3.Y += y;
					if (!MouseDragElementBehavior.RectContainsRect(rect, rect3))
					{
						if (rect3.X < rect.Left)
						{
							double num = rect3.X - rect.Left;
							x -= num;
						}
						else if (rect3.Right > rect.Right)
						{
							double num2 = rect3.Right - rect.Right;
							x -= num2;
						}
						if (rect3.Y < rect.Top)
						{
							double num3 = rect3.Y - rect.Top;
							y -= num3;
						}
						else if (rect3.Bottom > rect.Bottom)
						{
							double num4 = rect3.Bottom - rect.Bottom;
							y -= num4;
						}
					}
				}
				this.ApplyTranslationTransform(x, y);
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00007E04 File Offset: 0x00006004
		internal void ApplyTranslationTransform(double x, double y)
		{
			Transform renderTransform = this.RenderTransform;
			TranslateTransform translateTransform = renderTransform as TranslateTransform;
			if (translateTransform == null)
			{
				TransformGroup transformGroup = renderTransform as TransformGroup;
				MatrixTransform matrixTransform = renderTransform as MatrixTransform;
				CompositeTransform compositeTransform = renderTransform as CompositeTransform;
				if (compositeTransform != null)
				{
					compositeTransform.TranslateX += x;
					compositeTransform.TranslateY += y;
					return;
				}
				if (transformGroup != null)
				{
					if (transformGroup.Children.Count > 0)
					{
						translateTransform = (transformGroup.Children[transformGroup.Children.Count - 1] as TranslateTransform);
					}
					if (translateTransform == null)
					{
						translateTransform = new TranslateTransform();
						transformGroup.Children.Add(translateTransform);
					}
				}
				else
				{
					if (matrixTransform != null)
					{
						Matrix matrix = matrixTransform.Matrix;
						matrix.OffsetX += x;
						matrix.OffsetY += y;
						this.RenderTransform = new MatrixTransform
						{
							Matrix = matrix
						};
						return;
					}
					TransformGroup transformGroup2 = new TransformGroup();
					translateTransform = new TranslateTransform();
					if (renderTransform != null)
					{
						transformGroup2.Children.Add(renderTransform);
					}
					transformGroup2.Children.Add(translateTransform);
					this.RenderTransform = transformGroup2;
				}
			}
			translateTransform.X += x;
			translateTransform.Y += y;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00007F38 File Offset: 0x00006138
		internal static Transform CloneTransform(Transform transform)
		{
			if (transform == null)
			{
				return null;
			}
			transform.GetType();
			ScaleTransform scaleTransform;
			if ((scaleTransform = (transform as ScaleTransform)) != null)
			{
				return new ScaleTransform
				{
					CenterX = scaleTransform.CenterX,
					CenterY = scaleTransform.CenterY,
					ScaleX = scaleTransform.ScaleX,
					ScaleY = scaleTransform.ScaleY
				};
			}
			RotateTransform rotateTransform;
			if ((rotateTransform = (transform as RotateTransform)) != null)
			{
				return new RotateTransform
				{
					Angle = rotateTransform.Angle,
					CenterX = rotateTransform.CenterX,
					CenterY = rotateTransform.CenterY
				};
			}
			SkewTransform skewTransform;
			if ((skewTransform = (transform as SkewTransform)) != null)
			{
				return new SkewTransform
				{
					AngleX = skewTransform.AngleX,
					AngleY = skewTransform.AngleY,
					CenterX = skewTransform.CenterX,
					CenterY = skewTransform.CenterY
				};
			}
			TranslateTransform translateTransform;
			if ((translateTransform = (transform as TranslateTransform)) != null)
			{
				return new TranslateTransform
				{
					X = translateTransform.X,
					Y = translateTransform.Y
				};
			}
			MatrixTransform matrixTransform;
			if ((matrixTransform = (transform as MatrixTransform)) != null)
			{
				return new MatrixTransform
				{
					Matrix = matrixTransform.Matrix
				};
			}
			TransformGroup transformGroup;
			if ((transformGroup = (transform as TransformGroup)) != null)
			{
				TransformGroup transformGroup2 = new TransformGroup();
				foreach (Transform transform2 in transformGroup.Children)
				{
					transformGroup2.Children.Add(MouseDragElementBehavior.CloneTransform(transform2));
				}
				return transformGroup2;
			}
			CompositeTransform compositeTransform;
			if ((compositeTransform = (transform as CompositeTransform)) != null)
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
			return null;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00008184 File Offset: 0x00006384
		private void UpdatePosition()
		{
			GeneralTransform transform = base.AssociatedObject.TransformToVisual(this.RootElement);
			Point transformOffset = MouseDragElementBehavior.GetTransformOffset(transform);
			this.X = transformOffset.X;
			this.Y = transformOffset.Y;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000081C4 File Offset: 0x000063C4
		internal void StartDrag(Point positionInElementCoordinates)
		{
			this.relativePosition = positionInElementCoordinates;
			base.AssociatedObject.CaptureMouse();
			base.AssociatedObject.MouseMove += new MouseEventHandler(this.OnMouseMove);
			base.AssociatedObject.LostMouseCapture += new MouseEventHandler(this.OnLostMouseCapture);
			base.AssociatedObject.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonUp), false);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00008230 File Offset: 0x00006430
		internal void HandleDrag(Point newPositionInElementCoordinates)
		{
			double x = newPositionInElementCoordinates.X - this.relativePosition.X;
			double y = newPositionInElementCoordinates.Y - this.relativePosition.Y;
			GeneralTransform transform = base.AssociatedObject.TransformToVisual(this.RootElement);
			Point point = MouseDragElementBehavior.TransformAsVector(transform, x, y);
			this.settingPosition = true;
			this.ApplyTranslation(point.X, point.Y);
			this.UpdatePosition();
			this.settingPosition = false;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000082A8 File Offset: 0x000064A8
		internal void EndDrag()
		{
			base.AssociatedObject.MouseMove -= new MouseEventHandler(this.OnMouseMove);
			base.AssociatedObject.LostMouseCapture -= new MouseEventHandler(this.OnLostMouseCapture);
			base.AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonUp));
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000082FF File Offset: 0x000064FF
		private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.StartDrag(e.GetPosition(base.AssociatedObject));
			if (this.DragBegun != null)
			{
				this.DragBegun.Invoke(this, e);
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00008328 File Offset: 0x00006528
		private void OnLostMouseCapture(object sender, MouseEventArgs e)
		{
			this.EndDrag();
			if (this.DragFinished != null)
			{
				this.DragFinished.Invoke(this, e);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00008345 File Offset: 0x00006545
		private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			base.AssociatedObject.ReleaseMouseCapture();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00008352 File Offset: 0x00006552
		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			this.HandleDrag(e.GetPosition(base.AssociatedObject));
			if (this.Dragging != null)
			{
				this.Dragging.Invoke(this, e);
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000837C File Offset: 0x0000657C
		private static bool RectContainsRect(Rect rect1, Rect rect2)
		{
			return !rect1.IsEmpty && !rect2.IsEmpty && (rect1.X <= rect2.X && rect1.Y <= rect2.Y && rect1.X + rect1.Width >= rect2.X + rect2.Width) && rect1.Y + rect1.Height >= rect2.Y + rect2.Height;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00008404 File Offset: 0x00006604
		private static Point TransformAsVector(GeneralTransform transform, double x, double y)
		{
			Point point = transform.Transform(new Point(0.0, 0.0));
			Point point2 = transform.Transform(new Point(x, y));
			return new Point(point2.X - point.X, point2.Y - point.Y);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00008460 File Offset: 0x00006660
		private static Point GetTransformOffset(GeneralTransform transform)
		{
			return transform.Transform(new Point(0.0, 0.0));
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000847F File Offset: 0x0000667F
		protected override void OnAttached()
		{
			base.AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonDown), false);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000849E File Offset: 0x0000669E
		protected override void OnDetaching()
		{
			base.AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonDown));
		}

		// Token: 0x04000064 RID: 100
		private bool settingPosition;

		// Token: 0x04000065 RID: 101
		private Point relativePosition;

		// Token: 0x04000066 RID: 102
		private Transform cachedRenderTransform;

		// Token: 0x0400006A RID: 106
		public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double), typeof(MouseDragElementBehavior), new PropertyMetadata(double.NaN, new PropertyChangedCallback(MouseDragElementBehavior.OnXChanged)));

		// Token: 0x0400006B RID: 107
		public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(double), typeof(MouseDragElementBehavior), new PropertyMetadata(double.NaN, new PropertyChangedCallback(MouseDragElementBehavior.OnYChanged)));

		// Token: 0x0400006C RID: 108
		public static readonly DependencyProperty ConstrainToParentBoundsProperty = DependencyProperty.Register("ConstrainToParentBounds", typeof(bool), typeof(MouseDragElementBehavior), new PropertyMetadata(false, new PropertyChangedCallback(MouseDragElementBehavior.OnConstrainToParentBoundsChanged)));
	}
}
