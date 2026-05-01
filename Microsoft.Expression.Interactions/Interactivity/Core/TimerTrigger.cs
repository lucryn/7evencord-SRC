using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace Microsoft.Expression.Interactivity.Core
{
	// Token: 0x02000029 RID: 41
	public sealed class TimerTrigger : EventTrigger
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00008B32 File Offset: 0x00006D32
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00008B44 File Offset: 0x00006D44
		public double MillisecondsPerTick
		{
			get
			{
				return (double)base.GetValue(TimerTrigger.MillisecondsPerTickProperty);
			}
			set
			{
				base.SetValue(TimerTrigger.MillisecondsPerTickProperty, value);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00008B57 File Offset: 0x00006D57
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00008B69 File Offset: 0x00006D69
		public int TotalTicks
		{
			get
			{
				return (int)base.GetValue(TimerTrigger.TotalTicksProperty);
			}
			set
			{
				base.SetValue(TimerTrigger.TotalTicksProperty, value);
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008B7C File Offset: 0x00006D7C
		protected override void OnEvent(EventArgs eventArgs)
		{
			this.StopTimer();
			this.eventArgs = eventArgs;
			this.tickCount = 0;
			this.StartTimer();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00008B98 File Offset: 0x00006D98
		protected override void OnDetaching()
		{
			this.StopTimer();
			base.OnDetaching();
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00008BA8 File Offset: 0x00006DA8
		internal void StartTimer()
		{
			this.timer = new DispatcherTimer();
			this.timer.Interval = TimeSpan.FromMilliseconds(this.MillisecondsPerTick);
			this.timer.Tick += new EventHandler(this.OnTimerTick);
			this.timer.Start();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008BF8 File Offset: 0x00006DF8
		internal void StopTimer()
		{
			if (this.timer != null)
			{
				this.timer.Stop();
				this.timer = null;
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00008C14 File Offset: 0x00006E14
		private void OnTimerTick(object sender, EventArgs e)
		{
			if (this.TotalTicks > 0 && ++this.tickCount >= this.TotalTicks)
			{
				this.StopTimer();
			}
			base.InvokeActions(this.eventArgs);
		}

		// Token: 0x0400007B RID: 123
		public static readonly DependencyProperty MillisecondsPerTickProperty = DependencyProperty.Register("MillisecondsPerTick", typeof(double), typeof(TimerTrigger), new PropertyMetadata(1000.0));

		// Token: 0x0400007C RID: 124
		public static readonly DependencyProperty TotalTicksProperty = DependencyProperty.Register("TotalTicks", typeof(int), typeof(TimerTrigger), new PropertyMetadata(-1));

		// Token: 0x0400007D RID: 125
		private DispatcherTimer timer;

		// Token: 0x0400007E RID: 126
		private EventArgs eventArgs;

		// Token: 0x0400007F RID: 127
		private int tickCount;
	}
}
