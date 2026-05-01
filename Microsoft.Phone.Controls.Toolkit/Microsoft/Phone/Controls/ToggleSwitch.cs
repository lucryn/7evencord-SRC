using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000075 RID: 117
	[TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
	[TemplatePart(Name = "Switch", Type = typeof(ToggleButton))]
	public class ToggleSwitch : ContentControl
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x00014337 File Offset: 0x00012537
		// (set) Token: 0x0600049D RID: 1181 RVA: 0x00014344 File Offset: 0x00012544
		public object Header
		{
			get
			{
				return base.GetValue(ToggleSwitch.HeaderProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.HeaderProperty, value);
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x00014352 File Offset: 0x00012552
		// (set) Token: 0x0600049F RID: 1183 RVA: 0x00014364 File Offset: 0x00012564
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ToggleSwitch.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x00014372 File Offset: 0x00012572
		// (set) Token: 0x060004A1 RID: 1185 RVA: 0x00014384 File Offset: 0x00012584
		public Brush SwitchForeground
		{
			get
			{
				return (Brush)base.GetValue(ToggleSwitch.SwitchForegroundProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.SwitchForegroundProperty, value);
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x00014392 File Offset: 0x00012592
		// (set) Token: 0x060004A3 RID: 1187 RVA: 0x000143A4 File Offset: 0x000125A4
		[TypeConverter(typeof(NullableBoolConverter))]
		public bool? IsChecked
		{
			get
			{
				return (bool?)base.GetValue(ToggleSwitch.IsCheckedProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.IsCheckedProperty, value);
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x000143B8 File Offset: 0x000125B8
		private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ToggleSwitch toggleSwitch = (ToggleSwitch)d;
			if (toggleSwitch._toggleButton != null)
			{
				toggleSwitch._toggleButton.IsChecked = (bool?)e.NewValue;
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x060004A5 RID: 1189 RVA: 0x000143EC File Offset: 0x000125EC
		// (remove) Token: 0x060004A6 RID: 1190 RVA: 0x00014424 File Offset: 0x00012624
		public event EventHandler<RoutedEventArgs> Checked;

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x060004A7 RID: 1191 RVA: 0x0001445C File Offset: 0x0001265C
		// (remove) Token: 0x060004A8 RID: 1192 RVA: 0x00014494 File Offset: 0x00012694
		public event EventHandler<RoutedEventArgs> Unchecked;

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x060004A9 RID: 1193 RVA: 0x000144CC File Offset: 0x000126CC
		// (remove) Token: 0x060004AA RID: 1194 RVA: 0x00014504 File Offset: 0x00012704
		public event EventHandler<RoutedEventArgs> Indeterminate;

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x060004AB RID: 1195 RVA: 0x0001453C File Offset: 0x0001273C
		// (remove) Token: 0x060004AC RID: 1196 RVA: 0x00014574 File Offset: 0x00012774
		public event EventHandler<RoutedEventArgs> Click;

		// Token: 0x060004AD RID: 1197 RVA: 0x000145A9 File Offset: 0x000127A9
		public ToggleSwitch()
		{
			base.DefaultStyleKey = typeof(ToggleSwitch);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x000145C4 File Offset: 0x000127C4
		private void SetDefaultContent()
		{
			Binding binding = new Binding("IsChecked")
			{
				Source = this,
				Converter = new OffOnConverter()
			};
			base.SetBinding(ContentControl.ContentProperty, binding);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x000145FD File Offset: 0x000127FD
		private void ChangeVisualState(bool useTransitions)
		{
			if (base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
				return;
			}
			VisualStateManager.GoToState(this, "Disabled", useTransitions);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00014622 File Offset: 0x00012822
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			this._wasContentSet = true;
			if (DesignerProperties.IsInDesignTool && newContent == null && base.GetBindingExpression(ContentControl.ContentProperty) == null)
			{
				this.SetDefaultContent();
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001465C File Offset: 0x0001285C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (!this._wasContentSet && base.GetBindingExpression(ContentControl.ContentProperty) == null)
			{
				this.SetDefaultContent();
			}
			if (this._toggleButton != null)
			{
				this._toggleButton.Checked -= new RoutedEventHandler(this.CheckedHandler);
				this._toggleButton.Unchecked -= new RoutedEventHandler(this.UncheckedHandler);
				this._toggleButton.Indeterminate -= new RoutedEventHandler(this.IndeterminateHandler);
				this._toggleButton.Click -= new RoutedEventHandler(this.ClickHandler);
			}
			this._toggleButton = (base.GetTemplateChild("Switch") as ToggleButton);
			if (this._toggleButton != null)
			{
				this._toggleButton.Checked += new RoutedEventHandler(this.CheckedHandler);
				this._toggleButton.Unchecked += new RoutedEventHandler(this.UncheckedHandler);
				this._toggleButton.Indeterminate += new RoutedEventHandler(this.IndeterminateHandler);
				this._toggleButton.Click += new RoutedEventHandler(this.ClickHandler);
				this._toggleButton.IsChecked = this.IsChecked;
			}
			base.IsEnabledChanged += delegate(object A_1, DependencyPropertyChangedEventArgs A_2)
			{
				this.ChangeVisualState(true);
			};
			this.ChangeVisualState(false);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00014792 File Offset: 0x00012992
		private void CheckedHandler(object sender, RoutedEventArgs e)
		{
			this.IsChecked = new bool?(true);
			SafeRaise.Raise<RoutedEventArgs>(this.Checked, this, e);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000147AD File Offset: 0x000129AD
		private void UncheckedHandler(object sender, RoutedEventArgs e)
		{
			this.IsChecked = new bool?(false);
			SafeRaise.Raise<RoutedEventArgs>(this.Unchecked, this, e);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x000147C8 File Offset: 0x000129C8
		private void IndeterminateHandler(object sender, RoutedEventArgs e)
		{
			this.IsChecked = default(bool?);
			SafeRaise.Raise<RoutedEventArgs>(this.Indeterminate, this, e);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000147F1 File Offset: 0x000129F1
		private void ClickHandler(object sender, RoutedEventArgs e)
		{
			SafeRaise.Raise<RoutedEventArgs>(this.Click, this, e);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00014800 File Offset: 0x00012A00
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{{ToggleSwitch IsChecked={0}, Content={1}}}", new object[]
			{
				this.IsChecked,
				base.Content
			});
		}

		// Token: 0x04000264 RID: 612
		private const string CommonStates = "CommonStates";

		// Token: 0x04000265 RID: 613
		private const string NormalState = "Normal";

		// Token: 0x04000266 RID: 614
		private const string DisabledState = "Disabled";

		// Token: 0x04000267 RID: 615
		private const string SwitchPart = "Switch";

		// Token: 0x04000268 RID: 616
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(ToggleSwitch), new PropertyMetadata(null));

		// Token: 0x04000269 RID: 617
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(ToggleSwitch), new PropertyMetadata(null));

		// Token: 0x0400026A RID: 618
		public static readonly DependencyProperty SwitchForegroundProperty = DependencyProperty.Register("SwitchForeground", typeof(Brush), typeof(ToggleSwitch), null);

		// Token: 0x0400026B RID: 619
		public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool?), typeof(ToggleSwitch), new PropertyMetadata(false, new PropertyChangedCallback(ToggleSwitch.OnIsCheckedChanged)));

		// Token: 0x04000270 RID: 624
		private ToggleButton _toggleButton;

		// Token: 0x04000271 RID: 625
		private bool _wasContentSet;
	}
}
