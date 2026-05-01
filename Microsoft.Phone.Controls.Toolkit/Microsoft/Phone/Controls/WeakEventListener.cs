using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200004D RID: 77
	internal class WeakEventListener<TInstance, TSource, TEventArgs> where TInstance : class
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000D7A5 File Offset: 0x0000B9A5
		// (set) Token: 0x060002EC RID: 748 RVA: 0x0000D7AD File Offset: 0x0000B9AD
		public Action<TInstance, TSource, TEventArgs> OnEventAction { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000D7B6 File Offset: 0x0000B9B6
		// (set) Token: 0x060002EE RID: 750 RVA: 0x0000D7BE File Offset: 0x0000B9BE
		public Action<WeakEventListener<TInstance, TSource, TEventArgs>> OnDetachAction { get; set; }

		// Token: 0x060002EF RID: 751 RVA: 0x0000D7C7 File Offset: 0x0000B9C7
		public WeakEventListener(TInstance instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			this._weakInstance = new WeakReference(instance);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000D7F4 File Offset: 0x0000B9F4
		public void OnEvent(TSource source, TEventArgs eventArgs)
		{
			TInstance tinstance = (TInstance)((object)this._weakInstance.Target);
			if (tinstance != null)
			{
				if (this.OnEventAction != null)
				{
					this.OnEventAction.Invoke(tinstance, source, eventArgs);
					return;
				}
			}
			else
			{
				this.Detach();
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000D837 File Offset: 0x0000BA37
		public void Detach()
		{
			if (this.OnDetachAction != null)
			{
				this.OnDetachAction.Invoke(this);
				this.OnDetachAction = null;
			}
		}

		// Token: 0x0400014B RID: 331
		private WeakReference _weakInstance;
	}
}
