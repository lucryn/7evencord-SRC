using System;
using System.Windows.Controls;
using Microsoft.Phone.Controls.Primitives;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000010 RID: 16
	internal abstract class DataSource : ILoopingSelectorDataSource
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00004220 File Offset: 0x00002420
		public object GetNext(object relativeTo)
		{
			DateTime? relativeTo2 = this.GetRelativeTo(((DateTimeWrapper)relativeTo).DateTime, 1);
			if (relativeTo2 == null)
			{
				return null;
			}
			return new DateTimeWrapper(relativeTo2.Value);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004258 File Offset: 0x00002458
		public object GetPrevious(object relativeTo)
		{
			DateTime? relativeTo2 = this.GetRelativeTo(((DateTimeWrapper)relativeTo).DateTime, -1);
			if (relativeTo2 == null)
			{
				return null;
			}
			return new DateTimeWrapper(relativeTo2.Value);
		}

		// Token: 0x060000A2 RID: 162
		protected abstract DateTime? GetRelativeTo(DateTime relativeDate, int delta);

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000428F File Offset: 0x0000248F
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00004298 File Offset: 0x00002498
		public object SelectedItem
		{
			get
			{
				return this._selectedItem;
			}
			set
			{
				if (value != this._selectedItem)
				{
					DateTimeWrapper dateTimeWrapper = (DateTimeWrapper)value;
					if (dateTimeWrapper == null || this._selectedItem == null || dateTimeWrapper.DateTime != this._selectedItem.DateTime)
					{
						object selectedItem = this._selectedItem;
						this._selectedItem = dateTimeWrapper;
						EventHandler<SelectionChangedEventArgs> selectionChanged = this.SelectionChanged;
						if (selectionChanged != null)
						{
							selectionChanged.Invoke(this, new SelectionChangedEventArgs(new object[]
							{
								selectedItem
							}, new object[]
							{
								this._selectedItem
							}));
						}
					}
				}
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000A5 RID: 165 RVA: 0x0000431C File Offset: 0x0000251C
		// (remove) Token: 0x060000A6 RID: 166 RVA: 0x00004354 File Offset: 0x00002554
		public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

		// Token: 0x04000040 RID: 64
		private DateTimeWrapper _selectedItem;
	}
}
