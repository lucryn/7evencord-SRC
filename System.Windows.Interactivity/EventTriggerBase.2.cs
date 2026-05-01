using System;

namespace System.Windows.Interactivity
{
	// Token: 0x0200000E RID: 14
	public abstract class EventTriggerBase<T> : EventTriggerBase where T : class
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00003339 File Offset: 0x00001539
		protected EventTriggerBase() : base(typeof(T))
		{
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000334B File Offset: 0x0000154B
		public new T Source
		{
			get
			{
				return (T)((object)base.Source);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003358 File Offset: 0x00001558
		internal sealed override void OnSourceChangedImpl(object oldSource, object newSource)
		{
			base.OnSourceChangedImpl(oldSource, newSource);
			this.OnSourceChanged(oldSource as T, newSource as T);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000337E File Offset: 0x0000157E
		protected virtual void OnSourceChanged(T oldSource, T newSource)
		{
		}
	}
}
