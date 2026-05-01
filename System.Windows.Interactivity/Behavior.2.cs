using System;

namespace System.Windows.Interactivity
{
	// Token: 0x02000005 RID: 5
	public abstract class Behavior<T> : Behavior where T : DependencyObject
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002615 File Offset: 0x00000815
		protected Behavior() : base(typeof(T))
		{
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002627 File Offset: 0x00000827
		protected new T AssociatedObject
		{
			get
			{
				return (T)((object)base.AssociatedObject);
			}
		}
	}
}
