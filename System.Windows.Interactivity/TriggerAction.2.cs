using System;

namespace System.Windows.Interactivity
{
	// Token: 0x02000012 RID: 18
	public abstract class TriggerAction<T> : TriggerAction where T : DependencyObject
	{
		// Token: 0x0600007E RID: 126 RVA: 0x00003732 File Offset: 0x00001932
		protected TriggerAction() : base(typeof(T))
		{
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003744 File Offset: 0x00001944
		protected new T AssociatedObject
		{
			get
			{
				return (T)((object)base.AssociatedObject);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003751 File Offset: 0x00001951
		protected sealed override Type AssociatedObjectTypeConstraint
		{
			get
			{
				return base.AssociatedObjectTypeConstraint;
			}
		}
	}
}
