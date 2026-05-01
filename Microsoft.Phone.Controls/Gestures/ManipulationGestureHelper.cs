using System;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.Phone.Gestures
{
	// Token: 0x0200000F RID: 15
	internal class ManipulationGestureHelper : GestureHelper
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00002996 File Offset: 0x00001996
		public ManipulationGestureHelper(UIElement target, bool shouldHandleAllDrags) : base(target, shouldHandleAllDrags)
		{
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000029A0 File Offset: 0x000019A0
		protected override void HookEvents()
		{
			base.Target.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(this.Target_ManipulationStarted);
			base.Target.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(this.Target_ManipulationDelta);
			base.Target.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.Target_ManipulationCompleted);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000029F2 File Offset: 0x000019F2
		private void Target_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
		{
			base.NotifyDown(new ManipulationGestureHelper.ManipulationBaseArgs(e));
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002A00 File Offset: 0x00001A00
		private void Target_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			base.NotifyMove(new ManipulationGestureHelper.ManipulationDeltaArgs(e));
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002A0E File Offset: 0x00001A0E
		private void Target_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			base.NotifyUp(new ManipulationGestureHelper.ManiulationCompletedArgs(e));
		}

		// Token: 0x02000010 RID: 16
		private class ManipulationBaseArgs : InputBaseArgs
		{
			// Token: 0x06000053 RID: 83 RVA: 0x00002A1C File Offset: 0x00001A1C
			public ManipulationBaseArgs(ManipulationStartedEventArgs args) : base(args.ManipulationContainer, args.ManipulationOrigin)
			{
			}
		}

		// Token: 0x02000011 RID: 17
		private class ManipulationDeltaArgs : InputDeltaArgs
		{
			// Token: 0x06000054 RID: 84 RVA: 0x00002A30 File Offset: 0x00001A30
			public ManipulationDeltaArgs(ManipulationDeltaEventArgs args) : base(args.ManipulationContainer, args.ManipulationOrigin)
			{
				this._args = args;
			}

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x06000055 RID: 85 RVA: 0x00002A4B File Offset: 0x00001A4B
			public override Point DeltaTranslation
			{
				get
				{
					return this._args.DeltaManipulation.Translation;
				}
			}

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x06000056 RID: 86 RVA: 0x00002A5D File Offset: 0x00001A5D
			public override Point CumulativeTranslation
			{
				get
				{
					return this._args.CumulativeManipulation.Translation;
				}
			}

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x06000057 RID: 87 RVA: 0x00002A6F File Offset: 0x00001A6F
			public override Point ExpansionVelocity
			{
				get
				{
					return this._args.Velocities.ExpansionVelocity;
				}
			}

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x06000058 RID: 88 RVA: 0x00002A81 File Offset: 0x00001A81
			public override Point LinearVelocity
			{
				get
				{
					return this._args.Velocities.LinearVelocity;
				}
			}

			// Token: 0x0400002E RID: 46
			private ManipulationDeltaEventArgs _args;
		}

		// Token: 0x02000012 RID: 18
		private class ManiulationCompletedArgs : InputCompletedArgs
		{
			// Token: 0x06000059 RID: 89 RVA: 0x00002A93 File Offset: 0x00001A93
			public ManiulationCompletedArgs(ManipulationCompletedEventArgs args) : base(args.ManipulationContainer, args.ManipulationOrigin)
			{
				this._args = args;
			}

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x0600005A RID: 90 RVA: 0x00002AAE File Offset: 0x00001AAE
			public override Point TotalTranslation
			{
				get
				{
					return this._args.TotalManipulation.Translation;
				}
			}

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x0600005B RID: 91 RVA: 0x00002AC0 File Offset: 0x00001AC0
			public override Point FinalLinearVelocity
			{
				get
				{
					if (this._args.FinalVelocities != null)
					{
						return this._args.FinalVelocities.LinearVelocity;
					}
					return new Point(0.0, 0.0);
				}
			}

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x0600005C RID: 92 RVA: 0x00002AF7 File Offset: 0x00001AF7
			public override bool IsInertial
			{
				get
				{
					return this._args.IsInertial;
				}
			}

			// Token: 0x0400002F RID: 47
			private ManipulationCompletedEventArgs _args;
		}
	}
}
