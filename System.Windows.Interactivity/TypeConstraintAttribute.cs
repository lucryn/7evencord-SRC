using System;

namespace System.Windows.Interactivity
{
	// Token: 0x0200001C RID: 28
	[AttributeUsage(4, AllowMultiple = false, Inherited = false)]
	public sealed class TypeConstraintAttribute : Attribute
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000041A2 File Offset: 0x000023A2
		// (set) Token: 0x060000CE RID: 206 RVA: 0x000041AA File Offset: 0x000023AA
		public Type Constraint { get; private set; }

		// Token: 0x060000CF RID: 207 RVA: 0x000041B3 File Offset: 0x000023B3
		public TypeConstraintAttribute(Type constraint)
		{
			this.Constraint = constraint;
		}
	}
}
