using System;

namespace System.Windows.Interactivity
{
	// Token: 0x02000014 RID: 20
	internal sealed class NameResolvedEventArgs : EventArgs
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000038E5 File Offset: 0x00001AE5
		public object OldObject
		{
			get
			{
				return this.oldObject;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000038ED File Offset: 0x00001AED
		public object NewObject
		{
			get
			{
				return this.newObject;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000038F5 File Offset: 0x00001AF5
		public NameResolvedEventArgs(object oldObject, object newObject)
		{
			this.oldObject = oldObject;
			this.newObject = newObject;
		}

		// Token: 0x04000028 RID: 40
		private object oldObject;

		// Token: 0x04000029 RID: 41
		private object newObject;
	}
}
