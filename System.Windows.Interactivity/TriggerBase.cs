using System;
using System.Globalization;
using System.Windows.Markup;

namespace System.Windows.Interactivity
{
	// Token: 0x0200000C RID: 12
	[ContentProperty("Actions")]
	public abstract class TriggerBase : DependencyObject, IAttachedObject
	{
		// Token: 0x0600002E RID: 46 RVA: 0x00002970 File Offset: 0x00000B70
		internal TriggerBase(Type associatedObjectTypeConstraint)
		{
			this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
			TriggerActionCollection triggerActionCollection = new TriggerActionCollection();
			base.SetValue(TriggerBase.ActionsProperty, triggerActionCollection);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002F RID: 47 RVA: 0x0000299C File Offset: 0x00000B9C
		protected DependencyObject AssociatedObject
		{
			get
			{
				return this.associatedObject;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000029A4 File Offset: 0x00000BA4
		protected virtual Type AssociatedObjectTypeConstraint
		{
			get
			{
				return this.associatedObjectTypeConstraint;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000029AC File Offset: 0x00000BAC
		public TriggerActionCollection Actions
		{
			get
			{
				return (TriggerActionCollection)base.GetValue(TriggerBase.ActionsProperty);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000032 RID: 50 RVA: 0x000029C0 File Offset: 0x00000BC0
		// (remove) Token: 0x06000033 RID: 51 RVA: 0x000029F8 File Offset: 0x00000BF8
		public event EventHandler<PreviewInvokeEventArgs> PreviewInvoke;

		// Token: 0x06000034 RID: 52 RVA: 0x00002A30 File Offset: 0x00000C30
		protected void InvokeActions(object parameter)
		{
			if (this.PreviewInvoke != null)
			{
				PreviewInvokeEventArgs previewInvokeEventArgs = new PreviewInvokeEventArgs();
				this.PreviewInvoke.Invoke(this, previewInvokeEventArgs);
				if (previewInvokeEventArgs.Cancelling)
				{
					return;
				}
			}
			foreach (TriggerAction triggerAction in this.Actions)
			{
				triggerAction.CallInvoke(parameter);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002AA4 File Offset: 0x00000CA4
		protected virtual void OnAttached()
		{
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002AA6 File Offset: 0x00000CA6
		protected virtual void OnDetaching()
		{
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002AA8 File Offset: 0x00000CA8
		DependencyObject IAttachedObject.AssociatedObject
		{
			get
			{
				return this.AssociatedObject;
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002AB0 File Offset: 0x00000CB0
		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != this.AssociatedObject)
			{
				if (this.AssociatedObject != null)
				{
					throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerMultipleTimesExceptionMessage);
				}
				if (dependencyObject != null && !this.AssociatedObjectTypeConstraint.IsAssignableFrom(dependencyObject.GetType()))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.TypeConstraintViolatedExceptionMessage, new object[]
					{
						base.GetType().Name,
						dependencyObject.GetType().Name,
						this.AssociatedObjectTypeConstraint.Name
					}));
				}
				this.associatedObject = dependencyObject;
				this.Actions.Attach(dependencyObject);
				this.OnAttached();
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002B52 File Offset: 0x00000D52
		public void Detach()
		{
			this.OnDetaching();
			this.associatedObject = null;
			this.Actions.Detach();
		}

		// Token: 0x04000013 RID: 19
		private DependencyObject associatedObject;

		// Token: 0x04000014 RID: 20
		private Type associatedObjectTypeConstraint;

		// Token: 0x04000015 RID: 21
		public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register("Actions", typeof(TriggerActionCollection), typeof(TriggerBase), null);
	}
}
