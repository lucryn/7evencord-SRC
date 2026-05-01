using System;
using System.Windows;

namespace Microsoft.Expression.Media
{
	// Token: 0x0200001B RID: 27
	internal class DrawingPropertyMetadata : PropertyMetadata
	{
		// Token: 0x0600010B RID: 267 RVA: 0x00007A6F File Offset: 0x00005C6F
		public DrawingPropertyMetadata(object defaultValue) : this(defaultValue, DrawingPropertyMetadataOptions.None, null)
		{
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00007A7A File Offset: 0x00005C7A
		public DrawingPropertyMetadata(PropertyChangedCallback propertyChangedCallback) : this(DependencyProperty.UnsetValue, DrawingPropertyMetadataOptions.None, propertyChangedCallback)
		{
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00007A89 File Offset: 0x00005C89
		public DrawingPropertyMetadata(object defaultValue, DrawingPropertyMetadataOptions options) : this(defaultValue, options, null)
		{
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00007A94 File Offset: 0x00005C94
		public DrawingPropertyMetadata(object defaultValue, DrawingPropertyMetadataOptions options, PropertyChangedCallback propertyChangedCallback) : base(defaultValue, DrawingPropertyMetadata.AttachCallback(defaultValue, options, propertyChangedCallback))
		{
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600010F RID: 271 RVA: 0x00007AA8 File Offset: 0x00005CA8
		// (remove) Token: 0x06000110 RID: 272 RVA: 0x00007ADC File Offset: 0x00005CDC
		public static event EventHandler<DrawingPropertyChangedEventArgs> DrawingPropertyChanged;

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00007B0F File Offset: 0x00005D0F
		public bool AffectsRender
		{
			get
			{
				return (this.options & DrawingPropertyMetadataOptions.AffectsRender) != DrawingPropertyMetadataOptions.None;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00007B20 File Offset: 0x00005D20
		public bool AffectsMeasure
		{
			get
			{
				return (this.options & DrawingPropertyMetadataOptions.AffectsMeasure) != DrawingPropertyMetadataOptions.None;
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00007B30 File Offset: 0x00005D30
		private DrawingPropertyMetadata(DrawingPropertyMetadataOptions options, object defaultValue) : base(defaultValue)
		{
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00007B3C File Offset: 0x00005D3C
		private static PropertyChangedCallback AttachCallback(object defaultValue, DrawingPropertyMetadataOptions options, PropertyChangedCallback propertyChangedCallback)
		{
			return new PropertyChangedCallback(new DrawingPropertyMetadata(options, defaultValue)
			{
				options = options,
				propertyChangedCallback = propertyChangedCallback
			}.InternalCallback);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00007B6C File Offset: 0x00005D6C
		private void InternalCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (DrawingPropertyMetadata.DrawingPropertyChanged != null)
			{
				DrawingPropertyMetadata.DrawingPropertyChanged.Invoke(sender, new DrawingPropertyChangedEventArgs
				{
					Metadata = this,
					IsAnimated = (sender.GetAnimationBaseValue(e.Property) != e.NewValue)
				});
			}
			if (this.propertyChangedCallback != null)
			{
				this.propertyChangedCallback.Invoke(sender, e);
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00007C09 File Offset: 0x00005E09
		static DrawingPropertyMetadata()
		{
			DrawingPropertyMetadata.DrawingPropertyChanged += delegate(object sender, DrawingPropertyChangedEventArgs args)
			{
				IShape shape = sender as IShape;
				if (shape != null && args.Metadata.AffectsRender)
				{
					InvalidateGeometryReasons invalidateGeometryReasons = InvalidateGeometryReasons.PropertyChanged;
					if (args.IsAnimated)
					{
						invalidateGeometryReasons |= InvalidateGeometryReasons.IsAnimated;
					}
					shape.InvalidateGeometry(invalidateGeometryReasons);
				}
			};
		}

		// Token: 0x0400004E RID: 78
		private DrawingPropertyMetadataOptions options;

		// Token: 0x0400004F RID: 79
		private PropertyChangedCallback propertyChangedCallback;
	}
}
