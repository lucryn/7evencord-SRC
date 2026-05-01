using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x02000076 RID: 118
	[TemplateVisualState(GroupName = "Common", Name = "Selected")]
	[TemplatePart(Name = "Transform", Type = typeof(TranslateTransform))]
	[TemplateVisualState(GroupName = "Common", Name = "Normal")]
	[TemplateVisualState(GroupName = "Common", Name = "Expanded")]
	public class LoopingSelectorItem : ContentControl
	{
		// Token: 0x060004B9 RID: 1209 RVA: 0x000148FC File Offset: 0x00012AFC
		public LoopingSelectorItem()
		{
			base.DefaultStyleKey = typeof(LoopingSelectorItem);
			base.MouseLeftButtonDown += new MouseButtonEventHandler(this.LoopingSelectorItem_MouseLeftButtonDown);
			base.MouseLeftButtonUp += new MouseButtonEventHandler(this.LoopingSelectorItem_MouseLeftButtonUp);
			base.LostMouseCapture += new MouseEventHandler(this.LoopingSelectorItem_LostMouseCapture);
			GestureService.GetGestureListener(this).Tap += new EventHandler<GestureEventArgs>(this.LoopingSelectorItem_Tap);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001496C File Offset: 0x00012B6C
		internal void SetState(LoopingSelectorItem.State newState, bool useTransitions)
		{
			if (this._state != newState)
			{
				this._state = newState;
				switch (this._state)
				{
				case LoopingSelectorItem.State.Normal:
					VisualStateManager.GoToState(this, "Normal", useTransitions);
					return;
				case LoopingSelectorItem.State.Expanded:
					VisualStateManager.GoToState(this, "Expanded", useTransitions);
					return;
				case LoopingSelectorItem.State.Selected:
					VisualStateManager.GoToState(this, "Selected", useTransitions);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000149CC File Offset: 0x00012BCC
		internal LoopingSelectorItem.State GetState()
		{
			return this._state;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x000149D4 File Offset: 0x00012BD4
		private void LoopingSelectorItem_Tap(object sender, GestureEventArgs e)
		{
			e.Handled = true;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000149DD File Offset: 0x00012BDD
		private void LoopingSelectorItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			base.CaptureMouse();
			this._shouldClick = true;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x000149ED File Offset: 0x00012BED
		private void LoopingSelectorItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			base.ReleaseMouseCapture();
			if (this._shouldClick)
			{
				this._shouldClick = false;
				SafeRaise.Raise(this.Click, this);
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00014A10 File Offset: 0x00012C10
		private void LoopingSelectorItem_LostMouseCapture(object sender, MouseEventArgs e)
		{
			this._shouldClick = false;
		}

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x060004C0 RID: 1216 RVA: 0x00014A1C File Offset: 0x00012C1C
		// (remove) Token: 0x060004C1 RID: 1217 RVA: 0x00014A54 File Offset: 0x00012C54
		public event EventHandler<EventArgs> Click;

		// Token: 0x060004C2 RID: 1218 RVA: 0x00014A89 File Offset: 0x00012C89
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.Transform = ((base.GetTemplateChild("Transform") as TranslateTransform) ?? new TranslateTransform());
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00014AB0 File Offset: 0x00012CB0
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x00014AB8 File Offset: 0x00012CB8
		internal LoopingSelectorItem Previous { get; private set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00014AC1 File Offset: 0x00012CC1
		// (set) Token: 0x060004C6 RID: 1222 RVA: 0x00014AC9 File Offset: 0x00012CC9
		internal LoopingSelectorItem Next { get; private set; }

		// Token: 0x060004C7 RID: 1223 RVA: 0x00014AD4 File Offset: 0x00012CD4
		internal void Remove()
		{
			if (this.Previous != null)
			{
				this.Previous.Next = this.Next;
			}
			if (this.Next != null)
			{
				this.Next.Previous = this.Previous;
			}
			this.Next = (this.Previous = null);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00014B23 File Offset: 0x00012D23
		internal void InsertAfter(LoopingSelectorItem after)
		{
			this.Next = after.Next;
			this.Previous = after;
			if (after.Next != null)
			{
				after.Next.Previous = this;
			}
			after.Next = this;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00014B53 File Offset: 0x00012D53
		internal void InsertBefore(LoopingSelectorItem before)
		{
			this.Next = before;
			this.Previous = before.Previous;
			if (before.Previous != null)
			{
				before.Previous.Next = this;
			}
			before.Previous = this;
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00014B83 File Offset: 0x00012D83
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x00014B8B File Offset: 0x00012D8B
		internal TranslateTransform Transform { get; private set; }

		// Token: 0x04000272 RID: 626
		private const string TransformPartName = "Transform";

		// Token: 0x04000273 RID: 627
		private const string CommonGroupName = "Common";

		// Token: 0x04000274 RID: 628
		private const string NormalStateName = "Normal";

		// Token: 0x04000275 RID: 629
		private const string ExpandedStateName = "Expanded";

		// Token: 0x04000276 RID: 630
		private const string SelectedStateName = "Selected";

		// Token: 0x04000277 RID: 631
		private bool _shouldClick;

		// Token: 0x04000278 RID: 632
		private LoopingSelectorItem.State _state;

		// Token: 0x02000077 RID: 119
		internal enum State
		{
			// Token: 0x0400027E RID: 638
			Normal,
			// Token: 0x0400027F RID: 639
			Expanded,
			// Token: 0x04000280 RID: 640
			Selected
		}
	}
}
