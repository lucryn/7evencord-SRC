using System;

namespace System.Windows.Interactivity
{
	// Token: 0x0200001B RID: 27
	public sealed class TriggerCollection : AttachableCollection<TriggerBase>
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x000040DB File Offset: 0x000022DB
		internal TriggerCollection()
		{
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000040E4 File Offset: 0x000022E4
		protected override void OnAttached()
		{
			foreach (TriggerBase triggerBase in this)
			{
				triggerBase.Attach(base.AssociatedObject);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004134 File Offset: 0x00002334
		protected override void OnDetaching()
		{
			foreach (TriggerBase triggerBase in this)
			{
				triggerBase.Detach();
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000417C File Offset: 0x0000237C
		internal override void ItemAdded(TriggerBase item)
		{
			if (base.AssociatedObject != null)
			{
				item.Attach(base.AssociatedObject);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004192 File Offset: 0x00002392
		internal override void ItemRemoved(TriggerBase item)
		{
			if (((IAttachedObject)item).AssociatedObject != null)
			{
				item.Detach();
			}
		}
	}
}
