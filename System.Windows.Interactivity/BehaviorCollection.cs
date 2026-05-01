using System;

namespace System.Windows.Interactivity
{
	// Token: 0x02000006 RID: 6
	public sealed class BehaviorCollection : AttachableCollection<Behavior>
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002634 File Offset: 0x00000834
		internal BehaviorCollection()
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000263C File Offset: 0x0000083C
		protected override void OnAttached()
		{
			foreach (Behavior behavior in this)
			{
				behavior.Attach(base.AssociatedObject);
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000268C File Offset: 0x0000088C
		protected override void OnDetaching()
		{
			foreach (Behavior behavior in this)
			{
				behavior.Detach();
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000026D4 File Offset: 0x000008D4
		internal override void ItemAdded(Behavior item)
		{
			if (base.AssociatedObject != null)
			{
				item.Attach(base.AssociatedObject);
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000026EA File Offset: 0x000008EA
		internal override void ItemRemoved(Behavior item)
		{
			if (((IAttachedObject)item).AssociatedObject != null)
			{
				item.Detach();
			}
		}
	}
}
