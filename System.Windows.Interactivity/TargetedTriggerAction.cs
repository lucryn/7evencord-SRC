using System;
using System.Globalization;

namespace System.Windows.Interactivity
{
	// Token: 0x02000016 RID: 22
	public abstract class TargetedTriggerAction : TriggerAction
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003BCE File Offset: 0x00001DCE
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003BDB File Offset: 0x00001DDB
		public object TargetObject
		{
			get
			{
				return base.GetValue(TargetedTriggerAction.TargetObjectProperty);
			}
			set
			{
				base.SetValue(TargetedTriggerAction.TargetObjectProperty, value);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003BE9 File Offset: 0x00001DE9
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003BFB File Offset: 0x00001DFB
		public string TargetName
		{
			get
			{
				return (string)base.GetValue(TargetedTriggerAction.TargetNameProperty);
			}
			set
			{
				base.SetValue(TargetedTriggerAction.TargetNameProperty, value);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00003C0C File Offset: 0x00001E0C
		protected object Target
		{
			get
			{
				object obj = base.AssociatedObject;
				if (this.TargetObject != null)
				{
					obj = this.TargetObject;
				}
				else if (this.IsTargetNameSet)
				{
					obj = this.TargetResolver.Object;
				}
				if (obj != null && !this.TargetTypeConstraint.IsAssignableFrom(obj.GetType()))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.RetargetedTypeConstraintViolatedExceptionMessage, new object[]
					{
						base.GetType().Name,
						obj.GetType(),
						this.TargetTypeConstraint,
						"Target"
					}));
				}
				return obj;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00003CA4 File Offset: 0x00001EA4
		protected sealed override Type AssociatedObjectTypeConstraint
		{
			get
			{
				object[] customAttributes = base.GetType().GetCustomAttributes(typeof(TypeConstraintAttribute), true);
				int num = 0;
				if (num >= customAttributes.Length)
				{
					return typeof(DependencyObject);
				}
				TypeConstraintAttribute typeConstraintAttribute = (TypeConstraintAttribute)customAttributes[num];
				return typeConstraintAttribute.Constraint;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003CF0 File Offset: 0x00001EF0
		protected Type TargetTypeConstraint
		{
			get
			{
				return this.targetTypeConstraint;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00003CF8 File Offset: 0x00001EF8
		private bool IsTargetNameSet
		{
			get
			{
				return !string.IsNullOrEmpty(this.TargetName) || base.ReadLocalValue(TargetedTriggerAction.TargetNameProperty) != DependencyProperty.UnsetValue;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003D1E File Offset: 0x00001F1E
		private NameResolver TargetResolver
		{
			get
			{
				return this.targetResolver;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003D26 File Offset: 0x00001F26
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00003D2E File Offset: 0x00001F2E
		private bool IsTargetChangedRegistered
		{
			get
			{
				return this.isTargetChangedRegistered;
			}
			set
			{
				this.isTargetChangedRegistered = value;
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003D37 File Offset: 0x00001F37
		internal TargetedTriggerAction(Type targetTypeConstraint) : base(typeof(DependencyObject))
		{
			this.targetTypeConstraint = targetTypeConstraint;
			this.targetResolver = new NameResolver();
			this.RegisterTargetChanged();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003D61 File Offset: 0x00001F61
		internal virtual void OnTargetChangedImpl(object oldTarget, object newTarget)
		{
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003D64 File Offset: 0x00001F64
		protected override void OnAttached()
		{
			base.OnAttached();
			DependencyObject associatedObject = base.AssociatedObject;
			Behavior behavior = associatedObject as Behavior;
			this.RegisterTargetChanged();
			if (behavior != null)
			{
				associatedObject = ((IAttachedObject)behavior).AssociatedObject;
				behavior.AssociatedObjectChanged += new EventHandler(this.OnBehaviorHostChanged);
			}
			this.TargetResolver.NameScopeReferenceElement = (associatedObject as FrameworkElement);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003DB8 File Offset: 0x00001FB8
		protected override void OnDetaching()
		{
			Behavior behavior = base.AssociatedObject as Behavior;
			base.OnDetaching();
			this.OnTargetChangedImpl(this.TargetResolver.Object, null);
			this.UnregisterTargetChanged();
			if (behavior != null)
			{
				behavior.AssociatedObjectChanged -= new EventHandler(this.OnBehaviorHostChanged);
			}
			this.TargetResolver.NameScopeReferenceElement = null;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003E10 File Offset: 0x00002010
		private void OnBehaviorHostChanged(object sender, EventArgs e)
		{
			this.TargetResolver.NameScopeReferenceElement = (((IAttachedObject)sender).AssociatedObject as FrameworkElement);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003E2D File Offset: 0x0000202D
		private void RegisterTargetChanged()
		{
			if (!this.IsTargetChangedRegistered)
			{
				this.TargetResolver.ResolvedElementChanged += new EventHandler<NameResolvedEventArgs>(this.OnTargetChanged);
				this.IsTargetChangedRegistered = true;
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003E55 File Offset: 0x00002055
		private void UnregisterTargetChanged()
		{
			if (this.IsTargetChangedRegistered)
			{
				this.TargetResolver.ResolvedElementChanged -= new EventHandler<NameResolvedEventArgs>(this.OnTargetChanged);
				this.IsTargetChangedRegistered = false;
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003E80 File Offset: 0x00002080
		private static void OnTargetObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			TargetedTriggerAction targetedTriggerAction = (TargetedTriggerAction)obj;
			targetedTriggerAction.OnTargetChanged(obj, new NameResolvedEventArgs(args.OldValue, args.NewValue));
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003EB0 File Offset: 0x000020B0
		private static void OnTargetNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			TargetedTriggerAction targetedTriggerAction = (TargetedTriggerAction)obj;
			targetedTriggerAction.TargetResolver.Name = (string)args.NewValue;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003EDB File Offset: 0x000020DB
		private void OnTargetChanged(object sender, NameResolvedEventArgs e)
		{
			if (base.AssociatedObject != null)
			{
				this.OnTargetChangedImpl(e.OldObject, e.NewObject);
			}
		}

		// Token: 0x04000030 RID: 48
		private Type targetTypeConstraint;

		// Token: 0x04000031 RID: 49
		private bool isTargetChangedRegistered;

		// Token: 0x04000032 RID: 50
		private NameResolver targetResolver;

		// Token: 0x04000033 RID: 51
		public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(object), typeof(TargetedTriggerAction), new PropertyMetadata(new PropertyChangedCallback(TargetedTriggerAction.OnTargetObjectChanged)));

		// Token: 0x04000034 RID: 52
		public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register("TargetName", typeof(string), typeof(TargetedTriggerAction), new PropertyMetadata(new PropertyChangedCallback(TargetedTriggerAction.OnTargetNameChanged)));
	}
}
