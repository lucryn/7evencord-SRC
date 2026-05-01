using System;

namespace System.Windows.Interactivity
{
	// Token: 0x02000019 RID: 25
	public abstract class TriggerBase<T> : TriggerBase where T : DependencyObject
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x0000409B File Offset: 0x0000229B
		protected TriggerBase() : base(typeof(T))
		{
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000040AD File Offset: 0x000022AD
		protected new T AssociatedObject
		{
			get
			{
				return (T)((object)base.AssociatedObject);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000040BA File Offset: 0x000022BA
		protected sealed override Type AssociatedObjectTypeConstraint
		{
			get
			{
				return base.AssociatedObjectTypeConstraint;
			}
		}
	}
}
