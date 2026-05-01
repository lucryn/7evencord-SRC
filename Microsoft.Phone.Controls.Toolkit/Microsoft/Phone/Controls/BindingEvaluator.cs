using System;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000028 RID: 40
	internal class BindingEvaluator<T> : FrameworkElement
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600012A RID: 298 RVA: 0x000069D8 File Offset: 0x00004BD8
		// (set) Token: 0x0600012B RID: 299 RVA: 0x000069EA File Offset: 0x00004BEA
		public T Value
		{
			get
			{
				return (T)((object)base.GetValue(BindingEvaluator<T>.ValueProperty));
			}
			set
			{
				base.SetValue(BindingEvaluator<T>.ValueProperty, value);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000069FD File Offset: 0x00004BFD
		// (set) Token: 0x0600012D RID: 301 RVA: 0x00006A05 File Offset: 0x00004C05
		public Binding ValueBinding
		{
			get
			{
				return this._binding;
			}
			set
			{
				this._binding = value;
				base.SetBinding(BindingEvaluator<T>.ValueProperty, this._binding);
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006A20 File Offset: 0x00004C20
		public BindingEvaluator()
		{
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006A28 File Offset: 0x00004C28
		public BindingEvaluator(Binding binding)
		{
			base.SetBinding(BindingEvaluator<T>.ValueProperty, binding);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006A3D File Offset: 0x00004C3D
		public void ClearDataContext()
		{
			base.DataContext = null;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006A48 File Offset: 0x00004C48
		public T GetDynamicValue(object o, bool clearDataContext)
		{
			base.DataContext = o;
			T value = this.Value;
			if (clearDataContext)
			{
				base.DataContext = null;
			}
			return value;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006A6E File Offset: 0x00004C6E
		public T GetDynamicValue(object o)
		{
			base.DataContext = o;
			return this.Value;
		}

		// Token: 0x0400007F RID: 127
		private Binding _binding;

		// Token: 0x04000080 RID: 128
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(T), typeof(BindingEvaluator<T>), new PropertyMetadata(default(T)));
	}
}
