using System;
using System.Reflection;
using System.Windows.Input;

namespace System.Windows.Interactivity
{
	// Token: 0x02000013 RID: 19
	public sealed class InvokeCommandAction : TriggerAction<DependencyObject>
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003759 File Offset: 0x00001959
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00003761 File Offset: 0x00001961
		public string CommandName
		{
			get
			{
				return this.commandName;
			}
			set
			{
				if (this.CommandName != value)
				{
					this.commandName = value;
				}
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003778 File Offset: 0x00001978
		// (set) Token: 0x06000084 RID: 132 RVA: 0x0000378A File Offset: 0x0000198A
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(InvokeCommandAction.CommandProperty);
			}
			set
			{
				base.SetValue(InvokeCommandAction.CommandProperty, value);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00003798 File Offset: 0x00001998
		// (set) Token: 0x06000086 RID: 134 RVA: 0x000037A5 File Offset: 0x000019A5
		public object CommandParameter
		{
			get
			{
				return base.GetValue(InvokeCommandAction.CommandParameterProperty);
			}
			set
			{
				base.SetValue(InvokeCommandAction.CommandParameterProperty, value);
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000037B4 File Offset: 0x000019B4
		protected override void Invoke(object parameter)
		{
			if (base.AssociatedObject != null)
			{
				ICommand command = this.ResolveCommand();
				if (command != null && command.CanExecute(this.CommandParameter))
				{
					command.Execute(this.CommandParameter);
				}
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000037F0 File Offset: 0x000019F0
		private ICommand ResolveCommand()
		{
			ICommand result = null;
			if (this.Command != null)
			{
				result = this.Command;
			}
			else if (base.AssociatedObject != null)
			{
				Type type = base.AssociatedObject.GetType();
				PropertyInfo[] properties = type.GetProperties(20);
				foreach (PropertyInfo propertyInfo in properties)
				{
					if (typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType) && string.Equals(propertyInfo.Name, this.CommandName, 4))
					{
						result = (ICommand)propertyInfo.GetValue(base.AssociatedObject, null);
					}
				}
			}
			return result;
		}

		// Token: 0x04000025 RID: 37
		private string commandName;

		// Token: 0x04000026 RID: 38
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandAction), null);

		// Token: 0x04000027 RID: 39
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeCommandAction), null);
	}
}
