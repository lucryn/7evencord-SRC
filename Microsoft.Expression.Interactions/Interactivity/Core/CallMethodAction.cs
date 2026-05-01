using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Interactivity;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x02000003 RID: 3
	public class CallMethodAction : TriggerAction<DependencyObject>
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002194 File Offset: 0x00000394
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000021A1 File Offset: 0x000003A1
		public object TargetObject
		{
			get
			{
				return base.GetValue(CallMethodAction.TargetObjectProperty);
			}
			set
			{
				base.SetValue(CallMethodAction.TargetObjectProperty, value);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000021AF File Offset: 0x000003AF
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000021C1 File Offset: 0x000003C1
		public string MethodName
		{
			get
			{
				return (string)base.GetValue(CallMethodAction.MethodNameProperty);
			}
			set
			{
				base.SetValue(CallMethodAction.MethodNameProperty, value);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021CF File Offset: 0x000003CF
		public CallMethodAction()
		{
			this.methodDescriptors = new List<CallMethodAction.MethodDescriptor>();
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000021E2 File Offset: 0x000003E2
		private object Target
		{
			get
			{
				return this.TargetObject ?? base.AssociatedObject;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021F4 File Offset: 0x000003F4
		protected override void Invoke(object parameter)
		{
			if (base.AssociatedObject != null)
			{
				CallMethodAction.MethodDescriptor methodDescriptor = this.FindBestMethod(parameter);
				if (methodDescriptor != null)
				{
					ParameterInfo[] parameters = methodDescriptor.Parameters;
					if (parameters.Length == 0)
					{
						methodDescriptor.MethodInfo.Invoke(this.Target, null);
						return;
					}
					if (parameters.Length == 2 && base.AssociatedObject != null && parameter != null && parameters[0].ParameterType.IsAssignableFrom(base.AssociatedObject.GetType()) && parameters[1].ParameterType.IsAssignableFrom(parameter.GetType()))
					{
						methodDescriptor.MethodInfo.Invoke(this.Target, new object[]
						{
							base.AssociatedObject,
							parameter
						});
						return;
					}
				}
				else if (this.TargetObject != null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.CallMethodActionValidMethodNotFoundExceptionMessage, new object[]
					{
						this.MethodName,
						this.TargetObject.GetType().Name
					}));
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022EC File Offset: 0x000004EC
		protected override void OnAttached()
		{
			base.OnAttached();
			this.UpdateMethodInfo();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000022FA File Offset: 0x000004FA
		protected override void OnDetaching()
		{
			this.methodDescriptors.Clear();
			base.OnDetaching();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002344 File Offset: 0x00000544
		private CallMethodAction.MethodDescriptor FindBestMethod(object parameter)
		{
			if (parameter != null)
			{
				parameter.GetType();
			}
			return Enumerable.FirstOrDefault<CallMethodAction.MethodDescriptor>(this.methodDescriptors, (CallMethodAction.MethodDescriptor methodDescriptor) => !methodDescriptor.HasParameters || (parameter != null && methodDescriptor.SecondParameterType.IsAssignableFrom(parameter.GetType())));
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023CC File Offset: 0x000005CC
		private void UpdateMethodInfo()
		{
			this.methodDescriptors.Clear();
			if (this.Target != null && !string.IsNullOrEmpty(this.MethodName))
			{
				Type type = this.Target.GetType();
				foreach (MethodInfo methodInfo in type.GetMethods(20))
				{
					if (this.IsMethodValid(methodInfo))
					{
						ParameterInfo[] parameters = methodInfo.GetParameters();
						if (CallMethodAction.AreMethodParamsValid(parameters))
						{
							this.methodDescriptors.Add(new CallMethodAction.MethodDescriptor(methodInfo, parameters));
						}
					}
				}
				this.methodDescriptors = Enumerable.ToList<CallMethodAction.MethodDescriptor>(Enumerable.OrderByDescending<CallMethodAction.MethodDescriptor, int>(this.methodDescriptors, delegate(CallMethodAction.MethodDescriptor methodDescriptor)
				{
					int num = 0;
					if (methodDescriptor.HasParameters)
					{
						for (Type type2 = methodDescriptor.SecondParameterType; type2 != typeof(EventArgs); type2 = type2.BaseType)
						{
							num++;
						}
					}
					return methodDescriptor.ParameterCount + num;
				}));
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002486 File Offset: 0x00000686
		private bool IsMethodValid(MethodInfo method)
		{
			return string.Equals(method.Name, this.MethodName, 4) && method.ReturnType == typeof(void);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000024B4 File Offset: 0x000006B4
		private static bool AreMethodParamsValid(ParameterInfo[] methodParams)
		{
			if (methodParams.Length == 2)
			{
				if (methodParams[0].ParameterType != typeof(object))
				{
					return false;
				}
				if (!typeof(EventArgs).IsAssignableFrom(methodParams[1].ParameterType))
				{
					return false;
				}
			}
			else if (methodParams.Length != 0)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002500 File Offset: 0x00000700
		private static void OnMethodNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			CallMethodAction callMethodAction = (CallMethodAction)sender;
			callMethodAction.UpdateMethodInfo();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000251C File Offset: 0x0000071C
		private static void OnTargetObjectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			CallMethodAction callMethodAction = (CallMethodAction)sender;
			callMethodAction.UpdateMethodInfo();
		}

		// Token: 0x04000004 RID: 4
		private List<CallMethodAction.MethodDescriptor> methodDescriptors;

		// Token: 0x04000005 RID: 5
		public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(object), typeof(CallMethodAction), new PropertyMetadata(new PropertyChangedCallback(CallMethodAction.OnTargetObjectChanged)));

		// Token: 0x04000006 RID: 6
		public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register("MethodName", typeof(string), typeof(CallMethodAction), new PropertyMetadata(new PropertyChangedCallback(CallMethodAction.OnMethodNameChanged)));

		// Token: 0x02000004 RID: 4
		private class MethodDescriptor
		{
			// Token: 0x17000004 RID: 4
			// (get) Token: 0x0600001A RID: 26 RVA: 0x000025AD File Offset: 0x000007AD
			// (set) Token: 0x0600001B RID: 27 RVA: 0x000025B5 File Offset: 0x000007B5
			public MethodInfo MethodInfo { get; private set; }

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x0600001C RID: 28 RVA: 0x000025BE File Offset: 0x000007BE
			public bool HasParameters
			{
				get
				{
					return this.Parameters.Length > 0;
				}
			}

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x0600001D RID: 29 RVA: 0x000025CB File Offset: 0x000007CB
			public int ParameterCount
			{
				get
				{
					return this.Parameters.Length;
				}
			}

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x0600001E RID: 30 RVA: 0x000025D5 File Offset: 0x000007D5
			// (set) Token: 0x0600001F RID: 31 RVA: 0x000025DD File Offset: 0x000007DD
			public ParameterInfo[] Parameters { get; private set; }

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000020 RID: 32 RVA: 0x000025E6 File Offset: 0x000007E6
			public Type SecondParameterType
			{
				get
				{
					if (this.Parameters.Length >= 2)
					{
						return this.Parameters[1].ParameterType;
					}
					return null;
				}
			}

			// Token: 0x06000021 RID: 33 RVA: 0x00002602 File Offset: 0x00000802
			public MethodDescriptor(MethodInfo methodInfo, ParameterInfo[] methodParams)
			{
				this.MethodInfo = methodInfo;
				this.Parameters = methodParams;
			}
		}
	}
}
