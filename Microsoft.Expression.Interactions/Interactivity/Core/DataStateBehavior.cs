using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x0200000F RID: 15
	public class DataStateBehavior : Behavior<FrameworkElement>
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003648 File Offset: 0x00001848
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00003655 File Offset: 0x00001855
		public object Binding
		{
			get
			{
				return base.GetValue(DataStateBehavior.BindingProperty);
			}
			set
			{
				base.SetValue(DataStateBehavior.BindingProperty, value);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003663 File Offset: 0x00001863
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00003670 File Offset: 0x00001870
		public object Value
		{
			get
			{
				return base.GetValue(DataStateBehavior.ValueProperty);
			}
			set
			{
				base.SetValue(DataStateBehavior.ValueProperty, value);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005C RID: 92 RVA: 0x0000367E File Offset: 0x0000187E
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00003690 File Offset: 0x00001890
		public string TrueState
		{
			get
			{
				return (string)base.GetValue(DataStateBehavior.TrueStateProperty);
			}
			set
			{
				base.SetValue(DataStateBehavior.TrueStateProperty, value);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005E RID: 94 RVA: 0x0000369E File Offset: 0x0000189E
		// (set) Token: 0x0600005F RID: 95 RVA: 0x000036B0 File Offset: 0x000018B0
		public string FalseState
		{
			get
			{
				return (string)base.GetValue(DataStateBehavior.FalseStateProperty);
			}
			set
			{
				base.SetValue(DataStateBehavior.FalseStateProperty, value);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000060 RID: 96 RVA: 0x000036BE File Offset: 0x000018BE
		private FrameworkElement TargetObject
		{
			get
			{
				return VisualStateUtilities.FindNearestStatefulControl(base.AssociatedObject);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000036CB File Offset: 0x000018CB
		protected override void OnAttached()
		{
			base.OnAttached();
			this.ValidateStateNamesDeferred();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000036E4 File Offset: 0x000018E4
		private void ValidateStateNamesDeferred()
		{
			FrameworkElement frameworkElement = base.AssociatedObject.Parent as FrameworkElement;
			if (frameworkElement != null && DataStateBehavior.IsElementLoaded(frameworkElement))
			{
				this.ValidateStateNames();
				return;
			}
			base.AssociatedObject.Loaded += delegate(object o, RoutedEventArgs e)
			{
				this.ValidateStateNames();
			};
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003734 File Offset: 0x00001934
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

		// Token: 0x06000064 RID: 100 RVA: 0x0000376B File Offset: 0x0000196B
		private void ValidateStateNames()
		{
			this.ValidateStateName(this.TrueState);
			this.ValidateStateName(this.FalseState);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003788 File Offset: 0x00001988
		private void ValidateStateName(string stateName)
		{
			if (base.AssociatedObject != null && !string.IsNullOrEmpty(stateName))
			{
				foreach (VisualState visualState in this.TargetedVisualStates)
				{
					if (stateName == visualState.Name)
					{
						return;
					}
				}
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.DataStateBehaviorStateNameNotFoundExceptionMessage, new object[]
				{
					stateName,
					(this.TargetObject != null) ? this.TargetObject.GetType().Name : "null"
				}));
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003834 File Offset: 0x00001A34
		private IEnumerable<VisualState> TargetedVisualStates
		{
			get
			{
				List<VisualState> list = new List<VisualState>();
				if (this.TargetObject != null)
				{
					IList visualStateGroups = VisualStateUtilities.GetVisualStateGroups(this.TargetObject);
					foreach (object obj in visualStateGroups)
					{
						VisualStateGroup visualStateGroup = (VisualStateGroup)obj;
						foreach (object obj2 in visualStateGroup.States)
						{
							VisualState visualState = (VisualState)obj2;
							list.Add(visualState);
						}
					}
				}
				return list;
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000038F8 File Offset: 0x00001AF8
		private static void OnBindingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			DataStateBehavior dataStateBehavior = (DataStateBehavior)obj;
			dataStateBehavior.Evaluate();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003914 File Offset: 0x00001B14
		private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			DataStateBehavior dataStateBehavior = (DataStateBehavior)obj;
			dataStateBehavior.Evaluate();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003930 File Offset: 0x00001B30
		private static void OnTrueStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			DataStateBehavior dataStateBehavior = (DataStateBehavior)obj;
			dataStateBehavior.ValidateStateName(dataStateBehavior.TrueState);
			dataStateBehavior.Evaluate();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003958 File Offset: 0x00001B58
		private static void OnFalseStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			DataStateBehavior dataStateBehavior = (DataStateBehavior)obj;
			dataStateBehavior.ValidateStateName(dataStateBehavior.FalseState);
			dataStateBehavior.Evaluate();
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003980 File Offset: 0x00001B80
		private void Evaluate()
		{
			if (this.TargetObject != null)
			{
				string stateName;
				if (ComparisonLogic.EvaluateImpl(this.Binding, ComparisonConditionType.Equal, this.Value))
				{
					stateName = this.TrueState;
				}
				else
				{
					stateName = this.FalseState;
				}
				VisualStateUtilities.GoToState(this.TargetObject, stateName, true);
			}
		}

		// Token: 0x04000020 RID: 32
		public static readonly DependencyProperty BindingProperty = DependencyProperty.Register("Binding", typeof(object), typeof(DataStateBehavior), new PropertyMetadata(new PropertyChangedCallback(DataStateBehavior.OnBindingChanged)));

		// Token: 0x04000021 RID: 33
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(DataStateBehavior), new PropertyMetadata(new PropertyChangedCallback(DataStateBehavior.OnValueChanged)));

		// Token: 0x04000022 RID: 34
		public static readonly DependencyProperty TrueStateProperty = DependencyProperty.Register("TrueState", typeof(string), typeof(DataStateBehavior), new PropertyMetadata(new PropertyChangedCallback(DataStateBehavior.OnTrueStateChanged)));

		// Token: 0x04000023 RID: 35
		public static readonly DependencyProperty FalseStateProperty = DependencyProperty.Register("FalseState", typeof(string), typeof(DataStateBehavior), new PropertyMetadata(new PropertyChangedCallback(DataStateBehavior.OnFalseStateChanged)));
	}
}
