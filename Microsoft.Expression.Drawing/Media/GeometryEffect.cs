using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
	// Token: 0x0200003A RID: 58
	[TypeConverter(typeof(GeometryEffectConverter))]
	public abstract class GeometryEffect : DependencyObject
	{
		// Token: 0x0600021C RID: 540 RVA: 0x0000DB9D File Offset: 0x0000BD9D
		public static GeometryEffect GetGeometryEffect(DependencyObject obj)
		{
			return (GeometryEffect)obj.GetValue(GeometryEffect.GeometryEffectProperty);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000DBAF File Offset: 0x0000BDAF
		public static void SetGeometryEffect(DependencyObject obj, GeometryEffect value)
		{
			obj.SetValue(GeometryEffect.GeometryEffectProperty, value);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000DBC0 File Offset: 0x0000BDC0
		private static void OnGeometryEffectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			GeometryEffect geometryEffect = e.OldValue as GeometryEffect;
			GeometryEffect geometryEffect2 = e.NewValue as GeometryEffect;
			if (geometryEffect != geometryEffect2)
			{
				if (geometryEffect != null && obj.Equals(geometryEffect.Parent))
				{
					geometryEffect.Detach();
				}
				if (geometryEffect2 != null)
				{
					if (geometryEffect2.Parent != null)
					{
						GeometryEffect geometryEffect3 = geometryEffect2.CloneCurrentValue();
						obj.SetValue(GeometryEffect.GeometryEffectProperty, geometryEffect3);
						return;
					}
					geometryEffect2.Attach(obj);
				}
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000DC28 File Offset: 0x0000BE28
		public GeometryEffect CloneCurrentValue()
		{
			return this.DeepCopy();
		}

		// Token: 0x06000220 RID: 544
		protected abstract GeometryEffect DeepCopy();

		// Token: 0x06000221 RID: 545
		public abstract bool Equals(GeometryEffect geometryEffect);

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000DC30 File Offset: 0x0000BE30
		public static GeometryEffect DefaultGeometryEffect
		{
			get
			{
				GeometryEffect result;
				if ((result = GeometryEffect.defaultGeometryEffect) == null)
				{
					result = (GeometryEffect.defaultGeometryEffect = new GeometryEffect.NoGeometryEffect());
				}
				return result;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000DC46 File Offset: 0x0000BE46
		public Geometry OutputGeometry
		{
			get
			{
				return this.cachedGeometry;
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000DC7C File Offset: 0x0000BE7C
		static GeometryEffect()
		{
			DrawingPropertyMetadata.DrawingPropertyChanged += delegate(object sender, DrawingPropertyChangedEventArgs args)
			{
				GeometryEffect geometryEffect = sender as GeometryEffect;
				if (geometryEffect != null && args.Metadata.AffectsRender)
				{
					geometryEffect.InvalidateGeometry(InvalidateGeometryReasons.PropertyChanged);
				}
			};
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000DCE6 File Offset: 0x0000BEE6
		public bool InvalidateGeometry(InvalidateGeometryReasons reasons)
		{
			if (!this.effectInvalidated)
			{
				this.effectInvalidated = true;
				if (reasons != InvalidateGeometryReasons.ParentInvalidated)
				{
					GeometryEffect.InvalidateParent(this.Parent);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000DD0C File Offset: 0x0000BF0C
		public bool ProcessGeometry(Geometry input)
		{
			bool flag = false;
			if (this.effectInvalidated)
			{
				flag |= this.UpdateCachedGeometry(input);
				this.effectInvalidated = false;
			}
			return flag;
		}

		// Token: 0x06000227 RID: 551
		protected abstract bool UpdateCachedGeometry(Geometry input);

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000DD35 File Offset: 0x0000BF35
		// (set) Token: 0x06000229 RID: 553 RVA: 0x0000DD3D File Offset: 0x0000BF3D
		protected internal DependencyObject Parent { get; private set; }

		// Token: 0x0600022A RID: 554 RVA: 0x0000DD46 File Offset: 0x0000BF46
		protected internal virtual void Detach()
		{
			this.effectInvalidated = true;
			this.cachedGeometry = null;
			if (this.Parent != null)
			{
				GeometryEffect.InvalidateParent(this.Parent);
				this.Parent = null;
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000DD71 File Offset: 0x0000BF71
		protected internal virtual void Attach(DependencyObject obj)
		{
			if (this.Parent != null)
			{
				this.Detach();
			}
			this.effectInvalidated = true;
			this.cachedGeometry = null;
			if (GeometryEffect.InvalidateParent(obj))
			{
				this.Parent = obj;
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000DDA0 File Offset: 0x0000BFA0
		private static bool InvalidateParent(DependencyObject parent)
		{
			IShape shape = parent as IShape;
			if (shape != null)
			{
				shape.InvalidateGeometry(InvalidateGeometryReasons.ChildInvalidated);
				return true;
			}
			GeometryEffect geometryEffect = parent as GeometryEffect;
			if (geometryEffect != null)
			{
				geometryEffect.InvalidateGeometry(InvalidateGeometryReasons.ChildInvalidated);
				return true;
			}
			return false;
		}

		// Token: 0x040000A4 RID: 164
		public static readonly DependencyProperty GeometryEffectProperty = DependencyProperty.RegisterAttached("GeometryEffect", typeof(GeometryEffect), typeof(GeometryEffect), new DrawingPropertyMetadata(GeometryEffect.DefaultGeometryEffect, DrawingPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(GeometryEffect.OnGeometryEffectChanged)));

		// Token: 0x040000A5 RID: 165
		private static GeometryEffect defaultGeometryEffect;

		// Token: 0x040000A6 RID: 166
		protected Geometry cachedGeometry;

		// Token: 0x040000A7 RID: 167
		private bool effectInvalidated;

		// Token: 0x0200003B RID: 59
		private class NoGeometryEffect : GeometryEffect
		{
			// Token: 0x0600022F RID: 559 RVA: 0x0000DDDD File Offset: 0x0000BFDD
			protected override bool UpdateCachedGeometry(Geometry input)
			{
				this.cachedGeometry = input;
				return false;
			}

			// Token: 0x06000230 RID: 560 RVA: 0x0000DDE7 File Offset: 0x0000BFE7
			protected override GeometryEffect DeepCopy()
			{
				return new GeometryEffect.NoGeometryEffect();
			}

			// Token: 0x06000231 RID: 561 RVA: 0x0000DDEE File Offset: 0x0000BFEE
			public override bool Equals(GeometryEffect geometryEffect)
			{
				return geometryEffect == null || geometryEffect is GeometryEffect.NoGeometryEffect;
			}
		}
	}
}
