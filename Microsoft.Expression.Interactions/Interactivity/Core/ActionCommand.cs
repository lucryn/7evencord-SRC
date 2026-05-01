using System;
using System.Windows.Input;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x02000002 RID: 2
	public sealed class ActionCommand : ICommand
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		public ActionCommand(Action action)
		{
			this.action = action;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020DF File Offset: 0x000002DF
		public ActionCommand(Action<object> objectAction)
		{
			this.objectAction = objectAction;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000003 RID: 3 RVA: 0x000020F0 File Offset: 0x000002F0
		// (remove) Token: 0x06000004 RID: 4 RVA: 0x00002128 File Offset: 0x00000328
		private event EventHandler CanExecuteChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000005 RID: 5 RVA: 0x0000215D File Offset: 0x0000035D
		// (remove) Token: 0x06000006 RID: 6 RVA: 0x00002166 File Offset: 0x00000366
		event EventHandler ICommand.CanExecuteChanged
		{
			add
			{
				this.CanExecuteChanged += value;
			}
			remove
			{
				this.CanExecuteChanged -= value;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000216F File Offset: 0x0000036F
		bool ICommand.CanExecute(object parameter)
		{
			return true;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002172 File Offset: 0x00000372
		public void Execute(object parameter)
		{
			if (this.objectAction != null)
			{
				this.objectAction.Invoke(parameter);
				return;
			}
			this.action.Invoke();
		}

		// Token: 0x04000001 RID: 1
		private Action action;

		// Token: 0x04000002 RID: 2
		private Action<object> objectAction;
	}
}
