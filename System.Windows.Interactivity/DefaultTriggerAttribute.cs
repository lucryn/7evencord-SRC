using System;
using System.Collections;
using System.Globalization;

namespace System.Windows.Interactivity
{
	// Token: 0x02000009 RID: 9
	[AttributeUsage(132, AllowMultiple = true)]
	[CLSCompliant(false)]
	public sealed class DefaultTriggerAttribute : Attribute
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000271A File Offset: 0x0000091A
		public Type TargetType
		{
			get
			{
				return this.targetType;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002722 File Offset: 0x00000922
		public Type TriggerType
		{
			get
			{
				return this.triggerType;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000272A File Offset: 0x0000092A
		public IEnumerable Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002734 File Offset: 0x00000934
		public DefaultTriggerAttribute(Type targetType, Type triggerType, object parameter) : this(targetType, triggerType, new object[]
		{
			parameter
		})
		{
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002758 File Offset: 0x00000958
		public DefaultTriggerAttribute(Type targetType, Type triggerType, params object[] parameters)
		{
			if (!typeof(TriggerBase).IsAssignableFrom(triggerType))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage, new object[]
				{
					triggerType.Name
				}));
			}
			this.targetType = targetType;
			this.triggerType = triggerType;
			this.parameters = parameters;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000027B8 File Offset: 0x000009B8
		public TriggerBase Instantiate()
		{
			object obj = null;
			try
			{
				obj = Activator.CreateInstance(this.TriggerType, this.parameters);
			}
			catch
			{
			}
			return (TriggerBase)obj;
		}

		// Token: 0x0400000D RID: 13
		private Type targetType;

		// Token: 0x0400000E RID: 14
		private Type triggerType;

		// Token: 0x0400000F RID: 15
		private object[] parameters;
	}
}
