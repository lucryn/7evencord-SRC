using System;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Interactivity
{
	// Token: 0x0200000D RID: 13
	public abstract class EventTriggerBase : TriggerBase
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002B94 File Offset: 0x00000D94
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

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002BE0 File Offset: 0x00000DE0
		protected Type SourceTypeConstraint
		{
			get
			{
				return this.sourceTypeConstraint;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002BE8 File Offset: 0x00000DE8
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002BF5 File Offset: 0x00000DF5
		public object SourceObject
		{
			get
			{
				return base.GetValue(EventTriggerBase.SourceObjectProperty);
			}
			set
			{
				base.SetValue(EventTriggerBase.SourceObjectProperty, value);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002C03 File Offset: 0x00000E03
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002C15 File Offset: 0x00000E15
		public string SourceName
		{
			get
			{
				return (string)base.GetValue(EventTriggerBase.SourceNameProperty);
			}
			set
			{
				base.SetValue(EventTriggerBase.SourceNameProperty, value);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002C24 File Offset: 0x00000E24
		public object Source
		{
			get
			{
				object obj = base.AssociatedObject;
				if (this.SourceObject != null)
				{
					obj = this.SourceObject;
				}
				else if (this.IsSourceNameSet)
				{
					obj = this.SourceNameResolver.Object;
					if (obj != null && !this.SourceTypeConstraint.IsAssignableFrom(obj.GetType()))
					{
						throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.RetargetedTypeConstraintViolatedExceptionMessage, new object[]
						{
							base.GetType().Name,
							obj.GetType(),
							this.SourceTypeConstraint,
							"Source"
						}));
					}
				}
				return obj;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002CB9 File Offset: 0x00000EB9
		private NameResolver SourceNameResolver
		{
			get
			{
				return this.sourceNameResolver;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002CC1 File Offset: 0x00000EC1
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002CC9 File Offset: 0x00000EC9
		private bool IsSourceChangedRegistered
		{
			get
			{
				return this.isSourceChangedRegistered;
			}
			set
			{
				this.isSourceChangedRegistered = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002CD2 File Offset: 0x00000ED2
		private bool IsSourceNameSet
		{
			get
			{
				return !string.IsNullOrEmpty(this.SourceName) || base.ReadLocalValue(EventTriggerBase.SourceNameProperty) != DependencyProperty.UnsetValue;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002CF8 File Offset: 0x00000EF8
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00002D00 File Offset: 0x00000F00
		private bool IsLoadedRegistered { get; set; }

		// Token: 0x06000048 RID: 72 RVA: 0x00002D09 File Offset: 0x00000F09
		internal EventTriggerBase(Type sourceTypeConstraint) : base(typeof(DependencyObject))
		{
			this.sourceTypeConstraint = sourceTypeConstraint;
			this.sourceNameResolver = new NameResolver();
			this.RegisterSourceChanged();
		}

		// Token: 0x06000049 RID: 73
		protected abstract string GetEventName();

		// Token: 0x0600004A RID: 74 RVA: 0x00002D33 File Offset: 0x00000F33
		protected virtual void OnEvent(EventArgs eventArgs)
		{
			base.InvokeActions(eventArgs);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002D3C File Offset: 0x00000F3C
		private void OnSourceChanged(object oldSource, object newSource)
		{
			if (base.AssociatedObject != null)
			{
				this.OnSourceChangedImpl(oldSource, newSource);
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002D50 File Offset: 0x00000F50
		internal virtual void OnSourceChangedImpl(object oldSource, object newSource)
		{
			if (string.IsNullOrEmpty(this.GetEventName()))
			{
				return;
			}
			if (string.Compare(this.GetEventName(), "Loaded", 4) != 0)
			{
				if (oldSource != null && this.SourceTypeConstraint.IsAssignableFrom(oldSource.GetType()))
				{
					this.UnregisterEvent(oldSource, this.GetEventName());
				}
				if (newSource != null && this.SourceTypeConstraint.IsAssignableFrom(newSource.GetType()))
				{
					this.RegisterEvent(newSource, this.GetEventName());
				}
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002DC4 File Offset: 0x00000FC4
		protected override void OnAttached()
		{
			base.OnAttached();
			DependencyObject associatedObject = base.AssociatedObject;
			Behavior behavior = associatedObject as Behavior;
			FrameworkElement frameworkElement = associatedObject as FrameworkElement;
			this.RegisterSourceChanged();
			if (behavior != null)
			{
				associatedObject = ((IAttachedObject)behavior).AssociatedObject;
				behavior.AssociatedObjectChanged += new EventHandler(this.OnBehaviorHostChanged);
			}
			else
			{
				if (this.SourceObject == null)
				{
					if (frameworkElement != null)
					{
						goto IL_5C;
					}
				}
				try
				{
					this.OnSourceChanged(null, this.Source);
					goto IL_68;
				}
				catch (InvalidOperationException)
				{
					goto IL_68;
				}
				IL_5C:
				this.SourceNameResolver.NameScopeReferenceElement = frameworkElement;
			}
			IL_68:
			bool flag = string.Compare(this.GetEventName(), "Loaded", 4) == 0;
			if (flag && frameworkElement != null && !Interaction.IsElementLoaded(frameworkElement))
			{
				this.RegisterLoaded(frameworkElement);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002E74 File Offset: 0x00001074
		protected override void OnDetaching()
		{
			base.OnDetaching();
			Behavior behavior = base.AssociatedObject as Behavior;
			FrameworkElement frameworkElement = base.AssociatedObject as FrameworkElement;
			try
			{
				this.OnSourceChanged(this.Source, null);
			}
			catch (InvalidOperationException)
			{
			}
			this.UnregisterSourceChanged();
			if (behavior != null)
			{
				behavior.AssociatedObjectChanged -= new EventHandler(this.OnBehaviorHostChanged);
			}
			this.SourceNameResolver.NameScopeReferenceElement = null;
			bool flag = string.Compare(this.GetEventName(), "Loaded", 4) == 0;
			if (flag && frameworkElement != null)
			{
				this.UnregisterLoaded(frameworkElement);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002F0C File Offset: 0x0000110C
		private void OnBehaviorHostChanged(object sender, EventArgs e)
		{
			this.SourceNameResolver.NameScopeReferenceElement = (((IAttachedObject)sender).AssociatedObject as FrameworkElement);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002F2C File Offset: 0x0000112C
		private static void OnSourceObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EventTriggerBase eventTriggerBase = (EventTriggerBase)obj;
			object @object = eventTriggerBase.SourceNameResolver.Object;
			if (args.NewValue == null)
			{
				eventTriggerBase.OnSourceChanged(args.OldValue, @object);
				return;
			}
			if (args.OldValue == null && @object != null)
			{
				eventTriggerBase.UnregisterEvent(@object, eventTriggerBase.GetEventName());
			}
			eventTriggerBase.OnSourceChanged(args.OldValue, args.NewValue);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002F94 File Offset: 0x00001194
		private static void OnSourceNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EventTriggerBase eventTriggerBase = (EventTriggerBase)obj;
			eventTriggerBase.SourceNameResolver.Name = (string)args.NewValue;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002FBF File Offset: 0x000011BF
		private void RegisterSourceChanged()
		{
			if (!this.IsSourceChangedRegistered)
			{
				this.SourceNameResolver.ResolvedElementChanged += new EventHandler<NameResolvedEventArgs>(this.OnSourceNameResolverElementChanged);
				this.IsSourceChangedRegistered = true;
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002FE7 File Offset: 0x000011E7
		private void UnregisterSourceChanged()
		{
			if (this.IsSourceChangedRegistered)
			{
				this.SourceNameResolver.ResolvedElementChanged -= new EventHandler<NameResolvedEventArgs>(this.OnSourceNameResolverElementChanged);
				this.IsSourceChangedRegistered = false;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000300F File Offset: 0x0000120F
		private void OnSourceNameResolverElementChanged(object sender, NameResolvedEventArgs e)
		{
			if (this.SourceObject == null)
			{
				this.OnSourceChanged(e.OldObject, e.NewObject);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000302B File Offset: 0x0000122B
		private void RegisterLoaded(FrameworkElement associatedElement)
		{
			if (!this.IsLoadedRegistered && associatedElement != null)
			{
				associatedElement.Loaded += new RoutedEventHandler(this.OnEventImpl);
				this.IsLoadedRegistered = true;
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003051 File Offset: 0x00001251
		private void UnregisterLoaded(FrameworkElement associatedElement)
		{
			if (this.IsLoadedRegistered && associatedElement != null)
			{
				associatedElement.Loaded -= new RoutedEventHandler(this.OnEventImpl);
				this.IsLoadedRegistered = false;
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003078 File Offset: 0x00001278
		private void RegisterEvent(object obj, string eventName)
		{
			Type type = obj.GetType();
			EventInfo @event = type.GetEvent(eventName);
			if (@event == null)
			{
				if (this.SourceObject != null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.EventTriggerCannotFindEventNameExceptionMessage, new object[]
					{
						eventName,
						obj.GetType().Name
					}));
				}
				return;
			}
			else
			{
				if (EventTriggerBase.IsValidEvent(@event))
				{
					this.eventHandlerMethodInfo = typeof(EventTriggerBase).GetMethod("OnEventImpl", 36);
					@event.AddEventHandler(obj, Delegate.CreateDelegate(@event.EventHandlerType, this, this.eventHandlerMethodInfo));
					return;
				}
				if (this.SourceObject != null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.EventTriggerBaseInvalidEventExceptionMessage, new object[]
					{
						eventName,
						obj.GetType().Name
					}));
				}
				return;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003144 File Offset: 0x00001344
		private static bool IsValidEvent(EventInfo eventInfo)
		{
			Type eventHandlerType = eventInfo.EventHandlerType;
			if (typeof(Delegate).IsAssignableFrom(eventInfo.EventHandlerType))
			{
				MethodInfo method = eventHandlerType.GetMethod("Invoke");
				ParameterInfo[] parameters = method.GetParameters();
				return parameters.Length == 2 && typeof(object).IsAssignableFrom(parameters[0].ParameterType) && typeof(EventArgs).IsAssignableFrom(parameters[1].ParameterType);
			}
			return false;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000031BC File Offset: 0x000013BC
		private void UnregisterEvent(object obj, string eventName)
		{
			if (string.Compare(eventName, "Loaded", 4) == 0)
			{
				FrameworkElement frameworkElement = obj as FrameworkElement;
				if (frameworkElement != null)
				{
					this.UnregisterLoaded(frameworkElement);
					return;
				}
			}
			else
			{
				this.UnregisterEventImpl(obj, eventName);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000031F4 File Offset: 0x000013F4
		private void UnregisterEventImpl(object obj, string eventName)
		{
			Type type = obj.GetType();
			if (this.eventHandlerMethodInfo == null)
			{
				return;
			}
			EventInfo @event = type.GetEvent(eventName);
			@event.RemoveEventHandler(obj, Delegate.CreateDelegate(@event.EventHandlerType, this, this.eventHandlerMethodInfo));
			this.eventHandlerMethodInfo = null;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003239 File Offset: 0x00001439
		private void OnEventImpl(object sender, EventArgs eventArgs)
		{
			this.OnEvent(eventArgs);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003244 File Offset: 0x00001444
		internal void OnEventNameChanged(string oldEventName, string newEventName)
		{
			if (base.AssociatedObject != null)
			{
				FrameworkElement frameworkElement = this.Source as FrameworkElement;
				if (frameworkElement != null && string.Compare(oldEventName, "Loaded", 4) == 0)
				{
					this.UnregisterLoaded(frameworkElement);
				}
				else if (!string.IsNullOrEmpty(oldEventName))
				{
					this.UnregisterEvent(this.Source, oldEventName);
				}
				if (frameworkElement != null && string.Compare(newEventName, "Loaded", 4) == 0)
				{
					this.RegisterLoaded(frameworkElement);
					return;
				}
				if (!string.IsNullOrEmpty(newEventName))
				{
					this.RegisterEvent(this.Source, newEventName);
				}
			}
		}

		// Token: 0x04000017 RID: 23
		private Type sourceTypeConstraint;

		// Token: 0x04000018 RID: 24
		private bool isSourceChangedRegistered;

		// Token: 0x04000019 RID: 25
		private NameResolver sourceNameResolver;

		// Token: 0x0400001A RID: 26
		private MethodInfo eventHandlerMethodInfo;

		// Token: 0x0400001B RID: 27
		public static readonly DependencyProperty SourceObjectProperty = DependencyProperty.Register("SourceObject", typeof(object), typeof(EventTriggerBase), new PropertyMetadata(new PropertyChangedCallback(EventTriggerBase.OnSourceObjectChanged)));

		// Token: 0x0400001C RID: 28
		public static readonly DependencyProperty SourceNameProperty = DependencyProperty.Register("SourceName", typeof(string), typeof(EventTriggerBase), new PropertyMetadata(new PropertyChangedCallback(EventTriggerBase.OnSourceNameChanged)));
	}
}
