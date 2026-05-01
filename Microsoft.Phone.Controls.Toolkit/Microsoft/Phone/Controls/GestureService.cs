using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000078 RID: 120
	public static class GestureService
	{
		// Token: 0x060004CC RID: 1228 RVA: 0x00014B94 File Offset: 0x00012D94
		public static GestureListener GetGestureListener(DependencyObject obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return GestureService.GetGestureListenerInternal(obj, true);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00014BAC File Offset: 0x00012DAC
		internal static GestureListener GetGestureListenerInternal(DependencyObject obj, bool createIfMissing)
		{
			GestureListener gestureListener = (GestureListener)obj.GetValue(GestureService.GestureListenerProperty);
			if (gestureListener == null && createIfMissing)
			{
				gestureListener = new GestureListener();
				GestureService.SetGestureListenerInternal(obj, gestureListener);
			}
			return gestureListener;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00014BDE File Offset: 0x00012DDE
		[Obsolete("Do not add handlers using this method. Instead, use GetGestureListener, which will create a new instance if one is not already set, to add your handlers to an element.", true)]
		public static void SetGestureListener(DependencyObject obj, GestureListener value)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			GestureService.SetGestureListenerInternal(obj, value);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00014BF5 File Offset: 0x00012DF5
		private static void SetGestureListenerInternal(DependencyObject obj, GestureListener value)
		{
			obj.SetValue(GestureService.GestureListenerProperty, value);
		}

		// Token: 0x04000281 RID: 641
		public static readonly DependencyProperty GestureListenerProperty = DependencyProperty.RegisterAttached("GestureListener", typeof(GestureListener), typeof(GestureService), new PropertyMetadata(null));
	}
}
