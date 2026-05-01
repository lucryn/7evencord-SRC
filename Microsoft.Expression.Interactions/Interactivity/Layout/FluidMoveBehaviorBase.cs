using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;
using Microsoft.Expression.Interactivity.Core;

namespace Microsoft.Expression.Interactivity.Layout
{
	// Token: 0x02000018 RID: 24
	public abstract class FluidMoveBehaviorBase : Behavior<FrameworkElement>
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00005F16 File Offset: 0x00004116
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00005F28 File Offset: 0x00004128
		public FluidMoveScope AppliesTo
		{
			get
			{
				return (FluidMoveScope)base.GetValue(FluidMoveBehaviorBase.AppliesToProperty);
			}
			set
			{
				base.SetValue(FluidMoveBehaviorBase.AppliesToProperty, value);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00005F3B File Offset: 0x0000413B
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00005F4D File Offset: 0x0000414D
		public bool IsActive
		{
			get
			{
				return (bool)base.GetValue(FluidMoveBehaviorBase.IsActiveProperty);
			}
			set
			{
				base.SetValue(FluidMoveBehaviorBase.IsActiveProperty, value);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005F60 File Offset: 0x00004160
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00005F72 File Offset: 0x00004172
		public TagType Tag
		{
			get
			{
				return (TagType)base.GetValue(FluidMoveBehaviorBase.TagProperty);
			}
			set
			{
				base.SetValue(FluidMoveBehaviorBase.TagProperty, value);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005F85 File Offset: 0x00004185
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00005F97 File Offset: 0x00004197
		public string TagPath
		{
			get
			{
				return (string)base.GetValue(FluidMoveBehaviorBase.TagPathProperty);
			}
			set
			{
				base.SetValue(FluidMoveBehaviorBase.TagPathProperty, value);
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005FA5 File Offset: 0x000041A5
		protected static object GetIdentityTag(DependencyObject obj)
		{
			return obj.GetValue(FluidMoveBehaviorBase.IdentityTagProperty);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005FB2 File Offset: 0x000041B2
		protected static void SetIdentityTag(DependencyObject obj, object value)
		{
			obj.SetValue(FluidMoveBehaviorBase.IdentityTagProperty, value);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005FC0 File Offset: 0x000041C0
		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.LayoutUpdated += new EventHandler(this.AssociatedObject_LayoutUpdated);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005FDF File Offset: 0x000041DF
		protected override void OnDetaching()
		{
			base.OnDetaching();
			base.AssociatedObject.LayoutUpdated -= new EventHandler(this.AssociatedObject_LayoutUpdated);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00006000 File Offset: 0x00004200
		private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
		{
			if (!this.IsActive)
			{
				return;
			}
			if (DateTime.Now - FluidMoveBehaviorBase.LastPurgeTick >= FluidMoveBehaviorBase.MinTickDelta)
			{
				List<object> list = null;
				foreach (KeyValuePair<object, FluidMoveBehaviorBase.TagData> keyValuePair in FluidMoveBehaviorBase.TagDictionary)
				{
					if (keyValuePair.Value.Timestamp < FluidMoveBehaviorBase.NextToLastPurgeTick)
					{
						if (list == null)
						{
							list = new List<object>();
						}
						list.Add(keyValuePair.Key);
					}
				}
				if (list != null)
				{
					foreach (object obj in list)
					{
						FluidMoveBehaviorBase.TagDictionary.Remove(obj);
					}
				}
				FluidMoveBehaviorBase.NextToLastPurgeTick = FluidMoveBehaviorBase.LastPurgeTick;
				FluidMoveBehaviorBase.LastPurgeTick = DateTime.Now;
			}
			if (this.AppliesTo == FluidMoveScope.Self)
			{
				this.UpdateLayoutTransition(base.AssociatedObject);
				return;
			}
			Panel panel = base.AssociatedObject as Panel;
			if (panel != null)
			{
				foreach (UIElement uielement in panel.Children)
				{
					FrameworkElement child = (FrameworkElement)uielement;
					this.UpdateLayoutTransition(child);
				}
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000616C File Offset: 0x0000436C
		private void UpdateLayoutTransition(FrameworkElement child)
		{
			if (child.Visibility == 1 && this.ShouldSkipInitialLayout)
			{
				return;
			}
			FrameworkElement visualRoot = FluidMoveBehaviorBase.GetVisualRoot(child);
			FluidMoveBehaviorBase.TagData tagData = new FluidMoveBehaviorBase.TagData();
			tagData.Parent = (VisualTreeHelper.GetParent(child) as FrameworkElement);
			tagData.ParentRect = ExtendedVisualStateManager.GetLayoutRect(child);
			tagData.Child = child;
			tagData.Timestamp = DateTime.Now;
			try
			{
				tagData.AppRect = FluidMoveBehaviorBase.TranslateRect(tagData.ParentRect, tagData.Parent, visualRoot);
			}
			catch (ArgumentException)
			{
				if (this.ShouldSkipInitialLayout)
				{
					return;
				}
			}
			this.EnsureTags(child);
			object obj = FluidMoveBehaviorBase.GetIdentityTag(child);
			if (obj == null)
			{
				obj = child;
			}
			this.UpdateLayoutTransitionCore(child, visualRoot, obj, tagData);
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x0000621C File Offset: 0x0000441C
		protected virtual bool ShouldSkipInitialLayout
		{
			get
			{
				return this.Tag == TagType.DataContext;
			}
		}

		// Token: 0x060000CA RID: 202
		internal abstract void UpdateLayoutTransitionCore(FrameworkElement child, FrameworkElement root, object tag, FluidMoveBehaviorBase.TagData newTagData);

		// Token: 0x060000CB RID: 203 RVA: 0x00006228 File Offset: 0x00004428
		protected virtual void EnsureTags(FrameworkElement child)
		{
			if (this.Tag == TagType.DataContext)
			{
				object obj = child.ReadLocalValue(FluidMoveBehaviorBase.IdentityTagProperty);
				if (!(obj is BindingExpression))
				{
					child.SetBinding(FluidMoveBehaviorBase.IdentityTagProperty, new Binding(this.TagPath));
				}
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000626C File Offset: 0x0000446C
		private static FrameworkElement GetVisualRoot(FrameworkElement child)
		{
			for (;;)
			{
				FrameworkElement frameworkElement = VisualTreeHelper.GetParent(child) as FrameworkElement;
				if (frameworkElement == null)
				{
					break;
				}
				child = frameworkElement;
			}
			return child;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006290 File Offset: 0x00004490
		internal static Rect TranslateRect(Rect rect, FrameworkElement from, FrameworkElement to)
		{
			if (from == null || to == null)
			{
				return rect;
			}
			Point point;
			point..ctor(rect.Left, rect.Top);
			point = from.TransformToVisual(to).Transform(point);
			return new Rect(point.X, point.Y, rect.Width, rect.Height);
		}

		// Token: 0x04000040 RID: 64
		public static readonly DependencyProperty AppliesToProperty = DependencyProperty.Register("AppliesTo", typeof(FluidMoveScope), typeof(FluidMoveBehaviorBase), new PropertyMetadata(FluidMoveScope.Self));

		// Token: 0x04000041 RID: 65
		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(FluidMoveBehaviorBase), new PropertyMetadata(true));

		// Token: 0x04000042 RID: 66
		public static readonly DependencyProperty TagProperty = DependencyProperty.Register("Tag", typeof(TagType), typeof(FluidMoveBehaviorBase), new PropertyMetadata(TagType.Element));

		// Token: 0x04000043 RID: 67
		public static readonly DependencyProperty TagPathProperty = DependencyProperty.Register("TagPath", typeof(string), typeof(FluidMoveBehaviorBase), new PropertyMetadata(string.Empty));

		// Token: 0x04000044 RID: 68
		protected static readonly DependencyProperty IdentityTagProperty = DependencyProperty.RegisterAttached("IdentityTag", typeof(object), typeof(FluidMoveBehaviorBase), new PropertyMetadata(null));

		// Token: 0x04000045 RID: 69
		internal static Dictionary<object, FluidMoveBehaviorBase.TagData> TagDictionary = new Dictionary<object, FluidMoveBehaviorBase.TagData>();

		// Token: 0x04000046 RID: 70
		private static DateTime NextToLastPurgeTick = DateTime.MinValue;

		// Token: 0x04000047 RID: 71
		private static DateTime LastPurgeTick = DateTime.MinValue;

		// Token: 0x04000048 RID: 72
		private static TimeSpan MinTickDelta = TimeSpan.FromSeconds(0.5);

		// Token: 0x02000019 RID: 25
		internal class TagData
		{
			// Token: 0x1700002B RID: 43
			// (get) Token: 0x060000D0 RID: 208 RVA: 0x00006412 File Offset: 0x00004612
			// (set) Token: 0x060000D1 RID: 209 RVA: 0x0000641A File Offset: 0x0000461A
			public FrameworkElement Child { get; set; }

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x060000D2 RID: 210 RVA: 0x00006423 File Offset: 0x00004623
			// (set) Token: 0x060000D3 RID: 211 RVA: 0x0000642B File Offset: 0x0000462B
			public FrameworkElement Parent { get; set; }

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x060000D4 RID: 212 RVA: 0x00006434 File Offset: 0x00004634
			// (set) Token: 0x060000D5 RID: 213 RVA: 0x0000643C File Offset: 0x0000463C
			public Rect ParentRect { get; set; }

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x060000D6 RID: 214 RVA: 0x00006445 File Offset: 0x00004645
			// (set) Token: 0x060000D7 RID: 215 RVA: 0x0000644D File Offset: 0x0000464D
			public Rect AppRect { get; set; }

			// Token: 0x1700002F RID: 47
			// (get) Token: 0x060000D8 RID: 216 RVA: 0x00006456 File Offset: 0x00004656
			// (set) Token: 0x060000D9 RID: 217 RVA: 0x0000645E File Offset: 0x0000465E
			public DateTime Timestamp { get; set; }

			// Token: 0x17000030 RID: 48
			// (get) Token: 0x060000DA RID: 218 RVA: 0x00006467 File Offset: 0x00004667
			// (set) Token: 0x060000DB RID: 219 RVA: 0x0000646F File Offset: 0x0000466F
			public object InitialTag { get; set; }
		}
	}
}
