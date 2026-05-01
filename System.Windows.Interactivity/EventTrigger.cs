using System;

namespace System.Windows.Interactivity
{
	// Token: 0x0200000F RID: 15
	public class EventTrigger : EventTriggerBase<object>
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00003380 File Offset: 0x00001580
		public EventTrigger()
		{
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003388 File Offset: 0x00001588
		public EventTrigger(string eventName)
		{
			this.EventName = eventName;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003397 File Offset: 0x00001597
		// (set) Token: 0x06000065 RID: 101 RVA: 0x000033A9 File Offset: 0x000015A9
		public string EventName
		{
			get
			{
				return (string)base.GetValue(EventTrigger.EventNameProperty);
			}
			set
			{
				base.SetValue(EventTrigger.EventNameProperty, value);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000033B7 File Offset: 0x000015B7
		protected override string GetEventName()
		{
			return this.EventName;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000033BF File Offset: 0x000015BF
		private static void OnEventNameChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			((EventTrigger)sender).OnEventNameChanged((string)args.OldValue, (string)args.NewValue);
		}

		// Token: 0x0400001E RID: 30
		public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register("EventName", typeof(string), typeof(EventTrigger), new PropertyMetadata("Loaded", new PropertyChangedCallback(EventTrigger.OnEventNameChanged)));
	}
}
