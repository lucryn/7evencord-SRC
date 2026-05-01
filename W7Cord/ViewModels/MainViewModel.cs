using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace W7Cord.ViewModels
{
	// Token: 0x02000006 RID: 6
	public class MainViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000034 RID: 52 RVA: 0x0000363C File Offset: 0x0000183C
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00003653 File Offset: 0x00001853
		public ObservableCollection<GuildViewModel> Guilds { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000036 RID: 54 RVA: 0x0000365C File Offset: 0x0000185C
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00003673 File Offset: 0x00001873
		public ObservableCollection<ChannelViewModel> DirectMessages { get; set; }

		// Token: 0x06000038 RID: 56 RVA: 0x0000367C File Offset: 0x0000187C
		public MainViewModel()
		{
			this.Guilds = new ObservableCollection<GuildViewModel>();
			this.DirectMessages = new ObservableCollection<ChannelViewModel>();
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000039 RID: 57 RVA: 0x000036A0 File Offset: 0x000018A0
		// (remove) Token: 0x0600003A RID: 58 RVA: 0x000036DC File Offset: 0x000018DC
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600003B RID: 59 RVA: 0x00003718 File Offset: 0x00001918
		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
			}
		}
	}
}
