using System;

namespace System.Windows.Interactivity
{
	// Token: 0x02000015 RID: 21
	internal sealed class NameResolver
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600008E RID: 142 RVA: 0x0000390C File Offset: 0x00001B0C
		// (remove) Token: 0x0600008F RID: 143 RVA: 0x00003944 File Offset: 0x00001B44
		public event EventHandler<NameResolvedEventArgs> ResolvedElementChanged;

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00003979 File Offset: 0x00001B79
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00003984 File Offset: 0x00001B84
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				DependencyObject @object = this.Object;
				this.name = value;
				this.UpdateObjectFromName(@object);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000092 RID: 146 RVA: 0x000039A6 File Offset: 0x00001BA6
		public DependencyObject Object
		{
			get
			{
				if (string.IsNullOrEmpty(this.Name) && this.HasAttempedResolve)
				{
					return this.NameScopeReferenceElement;
				}
				return this.ResolvedObject;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000039CA File Offset: 0x00001BCA
		// (set) Token: 0x06000094 RID: 148 RVA: 0x000039D4 File Offset: 0x00001BD4
		public FrameworkElement NameScopeReferenceElement
		{
			get
			{
				return this.nameScopeReferenceElement;
			}
			set
			{
				FrameworkElement oldNameScopeReference = this.NameScopeReferenceElement;
				this.nameScopeReferenceElement = value;
				this.OnNameScopeReferenceElementChanged(oldNameScopeReference);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000039F6 File Offset: 0x00001BF6
		private FrameworkElement ActualNameScopeReferenceElement
		{
			get
			{
				if (this.NameScopeReferenceElement == null || !Interaction.IsElementLoaded(this.NameScopeReferenceElement))
				{
					return null;
				}
				return this.GetActualNameScopeReference(this.NameScopeReferenceElement);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003A1B File Offset: 0x00001C1B
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00003A23 File Offset: 0x00001C23
		private DependencyObject ResolvedObject { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003A2C File Offset: 0x00001C2C
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00003A34 File Offset: 0x00001C34
		private bool PendingReferenceElementLoad { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00003A3D File Offset: 0x00001C3D
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00003A45 File Offset: 0x00001C45
		private bool HasAttempedResolve { get; set; }

		// Token: 0x0600009C RID: 156 RVA: 0x00003A4E File Offset: 0x00001C4E
		private void OnNameScopeReferenceElementChanged(FrameworkElement oldNameScopeReference)
		{
			if (this.PendingReferenceElementLoad)
			{
				oldNameScopeReference.Loaded -= new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
				this.PendingReferenceElementLoad = false;
			}
			this.HasAttempedResolve = false;
			this.UpdateObjectFromName(this.Object);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003A84 File Offset: 0x00001C84
		private void UpdateObjectFromName(DependencyObject oldObject)
		{
			DependencyObject resolvedObject = null;
			this.ResolvedObject = null;
			if (this.NameScopeReferenceElement != null)
			{
				if (!Interaction.IsElementLoaded(this.NameScopeReferenceElement))
				{
					this.NameScopeReferenceElement.Loaded += new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
					this.PendingReferenceElementLoad = true;
					return;
				}
				if (!string.IsNullOrEmpty(this.Name))
				{
					FrameworkElement actualNameScopeReferenceElement = this.ActualNameScopeReferenceElement;
					if (actualNameScopeReferenceElement != null)
					{
						resolvedObject = (actualNameScopeReferenceElement.FindName(this.Name) as DependencyObject);
					}
				}
			}
			this.HasAttempedResolve = true;
			this.ResolvedObject = resolvedObject;
			if (oldObject != this.Object)
			{
				this.OnObjectChanged(oldObject, this.Object);
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003B1D File Offset: 0x00001D1D
		private void OnObjectChanged(DependencyObject oldTarget, DependencyObject newTarget)
		{
			if (this.ResolvedElementChanged != null)
			{
				this.ResolvedElementChanged.Invoke(this, new NameResolvedEventArgs(oldTarget, newTarget));
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003B3C File Offset: 0x00001D3C
		private FrameworkElement GetActualNameScopeReference(FrameworkElement initialReferenceElement)
		{
			FrameworkElement frameworkElement = initialReferenceElement;
			if (this.IsNameScope(initialReferenceElement))
			{
				frameworkElement = ((initialReferenceElement.Parent as FrameworkElement) ?? frameworkElement);
			}
			return frameworkElement;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003B68 File Offset: 0x00001D68
		private bool IsNameScope(FrameworkElement frameworkElement)
		{
			FrameworkElement frameworkElement2 = frameworkElement.Parent as FrameworkElement;
			if (frameworkElement2 != null)
			{
				object obj = frameworkElement2.FindName(this.Name);
				return obj != null;
			}
			return false;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003B9A File Offset: 0x00001D9A
		private void OnNameScopeReferenceLoaded(object sender, RoutedEventArgs e)
		{
			this.PendingReferenceElementLoad = false;
			this.NameScopeReferenceElement.Loaded -= new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
			this.UpdateObjectFromName(this.Object);
		}

		// Token: 0x0400002A RID: 42
		private string name;

		// Token: 0x0400002B RID: 43
		private FrameworkElement nameScopeReferenceElement;
	}
}
