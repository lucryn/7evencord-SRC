using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200003C RID: 60
	internal class EnumValue<T> where T : struct
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0000F90C File Offset: 0x0000DB0C
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000F914 File Offset: 0x0000DB14
		public T Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000F91C File Offset: 0x0000DB1C
		public EnumValue(string name, T value)
		{
			this._name = name;
			this._value = value;
		}

		// Token: 0x040000F6 RID: 246
		private string _name;

		// Token: 0x040000F7 RID: 247
		private T _value;
	}
}
