using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls.Primitives;

namespace System.Windows.Interactivity
{
	// Token: 0x02000011 RID: 17
	[DefaultTrigger(typeof(ButtonBase), typeof(EventTrigger), "Click")]
	[DefaultTrigger(typeof(UIElement), typeof(EventTrigger), "MouseLeftButtonDown")]
	public abstract class TriggerAction : DependencyObject, IAttachedObject
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000035ED File Offset: 0x000017ED
		// (set) Token: 0x06000070 RID: 112 RVA: 0x000035FF File Offset: 0x000017FF
		[DefaultValue(true)]
		public bool IsEnabled
		{
			get
			{
				return (bool)base.GetValue(TriggerAction.IsEnabledProperty);
			}
			set
			{
				base.SetValue(TriggerAction.IsEnabledProperty, value);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00003612 File Offset: 0x00001812
		protected DependencyObject AssociatedObject
		{
			get
			{
				return this.associatedObject;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000072 RID: 114 RVA: 0x0000361A File Offset: 0x0000181A
		protected virtual Type AssociatedObjectTypeConstraint
		{
			get
			{
				return this.associatedObjectTypeConstraint;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003622 File Offset: 0x00001822
		// (set) Token: 0x06000074 RID: 116 RVA: 0x0000362A File Offset: 0x0000182A
		internal bool IsHosted
		{
			get
			{
				return this.isHosted;
			}
			set
			{
				this.isHosted = value;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003633 File Offset: 0x00001833
		internal TriggerAction(Type associatedObjectTypeConstraint)
		{
			this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003642 File Offset: 0x00001842
		internal void CallInvoke(object parameter)
		{
			if (this.IsEnabled)
			{
				this.Invoke(parameter);
			}
		}

		// Token: 0x06000077 RID: 119
		protected abstract void Invoke(object parameter);

		// Token: 0x06000078 RID: 120 RVA: 0x00003653 File Offset: 0x00001853
		protected virtual void OnAttached()
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003655 File Offset: 0x00001855
		protected virtual void OnDetaching()
		{
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003657 File Offset: 0x00001857
		DependencyObject IAttachedObject.AssociatedObject
		{
			get
			{
				return this.AssociatedObject;
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003660 File Offset: 0x00001860
		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != this.AssociatedObject)
			{
				if (this.AssociatedObject != null)
				{
					throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerActionMultipleTimesExceptionMessage);
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
				this.OnAttached();
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000036F3 File Offset: 0x000018F3
		public void Detach()
		{
			this.OnDetaching();
			this.associatedObject = null;
		}

		// Token: 0x04000021 RID: 33
		private bool isHosted;

		// Token: 0x04000022 RID: 34
		private DependencyObject associatedObject;

		// Token: 0x04000023 RID: 35
		private Type associatedObjectTypeConstraint;

		// Token: 0x04000024 RID: 36
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(TriggerAction), new PropertyMetadata(true));
	}
}
