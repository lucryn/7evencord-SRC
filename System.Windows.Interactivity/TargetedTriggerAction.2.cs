using System;

namespace System.Windows.Interactivity
{
	// Token: 0x02000017 RID: 23
	public abstract class TargetedTriggerAction<T> : TargetedTriggerAction where T : class
	{
		// Token: 0x060000B9 RID: 185 RVA: 0x00003F6D File Offset: 0x0000216D
		protected TargetedTriggerAction() : base(typeof(T))
		{
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00003F7F File Offset: 0x0000217F
		protected new T Target
		{
			get
			{
				return (T)((object)base.Target);
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003F8C File Offset: 0x0000218C
		internal sealed override void OnTargetChangedImpl(object oldTarget, object newTarget)
		{
			base.OnTargetChangedImpl(oldTarget, newTarget);
			this.OnTargetChanged(oldTarget as T, newTarget as T);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003FB2 File Offset: 0x000021B2
		protected virtual void OnTargetChanged(T oldTarget, T newTarget)
		{
		}
	}
}
