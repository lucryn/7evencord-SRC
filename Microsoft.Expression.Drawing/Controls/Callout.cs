using System;
using System.Windows;
using Microsoft.Expression.Media;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000006 RID: 6
	public sealed class Callout : CompositeContentShape, ICalloutGeometrySourceParameters, IGeometrySourceParameters
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600003C RID: 60 RVA: 0x0000276D File Offset: 0x0000096D
		// (set) Token: 0x0600003D RID: 61 RVA: 0x0000277F File Offset: 0x0000097F
		public Point AnchorPoint
		{
			get
			{
				return (Point)base.GetValue(Callout.AnchorPointProperty);
			}
			set
			{
				base.SetValue(Callout.AnchorPointProperty, value);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002792 File Offset: 0x00000992
		// (set) Token: 0x0600003F RID: 63 RVA: 0x000027A4 File Offset: 0x000009A4
		public CalloutStyle CalloutStyle
		{
			get
			{
				return (CalloutStyle)base.GetValue(Callout.CalloutStyleProperty);
			}
			set
			{
				base.SetValue(Callout.CalloutStyleProperty, value);
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000027B7 File Offset: 0x000009B7
		public Callout()
		{
			base.DefaultStyleKey = typeof(Callout);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000027CF File Offset: 0x000009CF
		protected override IGeometrySource CreateGeometrySource()
		{
			return new CalloutGeometrySource();
		}

		// Token: 0x04000012 RID: 18
		public static readonly DependencyProperty AnchorPointProperty = DependencyProperty.Register("AnchorPoint", typeof(Point), typeof(Callout), new DrawingPropertyMetadata(default(Point), DrawingPropertyMetadataOptions.AffectsRender));

		// Token: 0x04000013 RID: 19
		public static readonly DependencyProperty CalloutStyleProperty = DependencyProperty.Register("CalloutStyle", typeof(CalloutStyle), typeof(Callout), new DrawingPropertyMetadata(CalloutStyle.RoundedRectangle, DrawingPropertyMetadataOptions.AffectsRender));
	}
}
