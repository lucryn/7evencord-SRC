using System;
using System.Windows.Media;

namespace System.Windows.Interactivity
{
	// Token: 0x02000010 RID: 16
	public static class Interaction
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00003420 File Offset: 0x00001620
		public static TriggerCollection GetTriggers(DependencyObject obj)
		{
			TriggerCollection triggerCollection = (TriggerCollection)obj.GetValue(Interaction.TriggersProperty);
			if (triggerCollection == null)
			{
				triggerCollection = new TriggerCollection();
				obj.SetValue(Interaction.TriggersProperty, triggerCollection);
			}
			return triggerCollection;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003454 File Offset: 0x00001654
		public static BehaviorCollection GetBehaviors(DependencyObject obj)
		{
			BehaviorCollection behaviorCollection = (BehaviorCollection)obj.GetValue(Interaction.BehaviorsProperty);
			if (behaviorCollection == null)
			{
				behaviorCollection = new BehaviorCollection();
				obj.SetValue(Interaction.BehaviorsProperty, behaviorCollection);
			}
			return behaviorCollection;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003488 File Offset: 0x00001688
		private static void OnBehaviorsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			BehaviorCollection behaviorCollection = (BehaviorCollection)args.OldValue;
			BehaviorCollection behaviorCollection2 = (BehaviorCollection)args.NewValue;
			if (behaviorCollection != behaviorCollection2)
			{
				if (behaviorCollection != null && ((IAttachedObject)behaviorCollection).AssociatedObject != null)
				{
					behaviorCollection.Detach();
				}
				if (behaviorCollection2 != null && obj != null)
				{
					if (((IAttachedObject)behaviorCollection2).AssociatedObject != null)
					{
						throw new InvalidOperationException(ExceptionStringTable.CannotHostBehaviorCollectionMultipleTimesExceptionMessage);
					}
					behaviorCollection2.Attach(obj);
				}
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000034E4 File Offset: 0x000016E4
		private static void OnTriggersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			TriggerCollection triggerCollection = args.OldValue as TriggerCollection;
			TriggerCollection triggerCollection2 = args.NewValue as TriggerCollection;
			if (triggerCollection != triggerCollection2)
			{
				if (triggerCollection != null && ((IAttachedObject)triggerCollection).AssociatedObject != null)
				{
					triggerCollection.Detach();
				}
				if (triggerCollection2 != null && obj != null)
				{
					if (((IAttachedObject)triggerCollection2).AssociatedObject != null)
					{
						throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerCollectionMultipleTimesExceptionMessage);
					}
					triggerCollection2.Attach(obj);
				}
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003540 File Offset: 0x00001740
		internal static bool IsElementLoaded(FrameworkElement element)
		{
			UIElement rootVisual = Application.Current.RootVisual;
			DependencyObject parent = element.Parent;
			if (parent == null)
			{
				parent = VisualTreeHelper.GetParent(element);
			}
			return parent != null || (rootVisual != null && element == rootVisual);
		}

		// Token: 0x0400001F RID: 31
		public static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached("Triggers", typeof(TriggerCollection), typeof(Interaction), new PropertyMetadata(new PropertyChangedCallback(Interaction.OnTriggersChanged)));

		// Token: 0x04000020 RID: 32
		public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("Behaviors", typeof(BehaviorCollection), typeof(Interaction), new PropertyMetadata(new PropertyChangedCallback(Interaction.OnBehaviorsChanged)));
	}
}
