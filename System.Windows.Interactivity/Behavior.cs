using System;
using System.Globalization;

namespace System.Windows.Interactivity
{
	// Token: 0x02000004 RID: 4
	public abstract class Behavior : DependencyObject, IAttachedObject
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000010 RID: 16 RVA: 0x000024B0 File Offset: 0x000006B0
		// (remove) Token: 0x06000011 RID: 17 RVA: 0x000024E8 File Offset: 0x000006E8
		internal event EventHandler AssociatedObjectChanged;

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000012 RID: 18 RVA: 0x0000251D File Offset: 0x0000071D
		protected Type AssociatedType
		{
			get
			{
				return this.associatedType;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002525 File Offset: 0x00000725
		protected DependencyObject AssociatedObject
		{
			get
			{
				return this.associatedObject;
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000252D File Offset: 0x0000072D
		internal Behavior(Type associatedType)
		{
			this.associatedType = associatedType;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000253C File Offset: 0x0000073C
		protected virtual void OnAttached()
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000253E File Offset: 0x0000073E
		protected virtual void OnDetaching()
		{
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002540 File Offset: 0x00000740
		private void OnAssociatedObjectChanged()
		{
			if (this.AssociatedObjectChanged != null)
			{
				this.AssociatedObjectChanged.Invoke(this, new EventArgs());
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000255B File Offset: 0x0000075B
		DependencyObject IAttachedObject.AssociatedObject
		{
			get
			{
				return this.AssociatedObject;
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002564 File Offset: 0x00000764
		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != this.AssociatedObject)
			{
				if (this.AssociatedObject != null)
				{
					throw new InvalidOperationException(ExceptionStringTable.CannotHostBehaviorMultipleTimesExceptionMessage);
				}
				if (dependencyObject != null && !this.AssociatedType.IsAssignableFrom(dependencyObject.GetType()))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.TypeConstraintViolatedExceptionMessage, new object[]
					{
						base.GetType().Name,
						dependencyObject.GetType().Name,
						this.AssociatedType.Name
					}));
				}
				this.associatedObject = dependencyObject;
				this.OnAssociatedObjectChanged();
				this.OnAttached();
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002600 File Offset: 0x00000800
		public void Detach()
		{
			this.OnDetaching();
			this.associatedObject = null;
			this.OnAssociatedObjectChanged();
		}

		// Token: 0x04000003 RID: 3
		private Type associatedType;

		// Token: 0x04000004 RID: 4
		private DependencyObject associatedObject;
	}
}
