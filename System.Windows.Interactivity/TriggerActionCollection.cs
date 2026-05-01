using System;

namespace System.Windows.Interactivity
{
	// Token: 0x02000018 RID: 24
	public class TriggerActionCollection : AttachableCollection<TriggerAction>
	{
		// Token: 0x060000BD RID: 189 RVA: 0x00003FB4 File Offset: 0x000021B4
		internal TriggerActionCollection()
		{
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003FBC File Offset: 0x000021BC
		protected override void OnAttached()
		{
			foreach (TriggerAction triggerAction in this)
			{
				triggerAction.Attach(base.AssociatedObject);
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000400C File Offset: 0x0000220C
		protected override void OnDetaching()
		{
			foreach (TriggerAction triggerAction in this)
			{
				triggerAction.Detach();
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004054 File Offset: 0x00002254
		internal override void ItemAdded(TriggerAction item)
		{
			if (item.IsHosted)
			{
				throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerActionMultipleTimesExceptionMessage);
			}
			if (base.AssociatedObject != null)
			{
				item.Attach(base.AssociatedObject);
			}
			item.IsHosted = true;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004084 File Offset: 0x00002284
		internal override void ItemRemoved(TriggerAction item)
		{
			if (((IAttachedObject)item).AssociatedObject != null)
			{
				item.Detach();
			}
			item.IsHosted = false;
		}
	}
}
