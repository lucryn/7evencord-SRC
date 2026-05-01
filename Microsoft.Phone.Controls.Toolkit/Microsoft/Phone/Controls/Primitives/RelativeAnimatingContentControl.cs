using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls.Primitives
{
	// Token: 0x02000035 RID: 53
	public class RelativeAnimatingContentControl : ContentControl
	{
		// Token: 0x06000198 RID: 408 RVA: 0x00008195 File Offset: 0x00006395
		public RelativeAnimatingContentControl()
		{
			base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000081B0 File Offset: 0x000063B0
		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e != null && e.NewSize.Height > 0.0 && e.NewSize.Width > 0.0)
			{
				this._knownWidth = e.NewSize.Width;
				this._knownHeight = e.NewSize.Height;
				base.Clip = new RectangleGeometry
				{
					Rect = new Rect(0.0, 0.0, this._knownWidth, this._knownHeight)
				};
				this.UpdateAnyAnimationValues();
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000825C File Offset: 0x0000645C
		private void UpdateAnyAnimationValues()
		{
			if (this._knownHeight > 0.0 && this._knownWidth > 0.0)
			{
				if (this._specialAnimations == null)
				{
					this._specialAnimations = new List<RelativeAnimatingContentControl.AnimationValueAdapter>();
					foreach (object obj in VisualStateManager.GetVisualStateGroups(this))
					{
						VisualStateGroup visualStateGroup = (VisualStateGroup)obj;
						if (visualStateGroup != null)
						{
							foreach (object obj2 in visualStateGroup.States)
							{
								VisualState visualState = (VisualState)obj2;
								if (visualState != null)
								{
									Storyboard storyboard = visualState.Storyboard;
									if (storyboard != null)
									{
										foreach (Timeline timeline in storyboard.Children)
										{
											DoubleAnimation doubleAnimation = timeline as DoubleAnimation;
											DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = timeline as DoubleAnimationUsingKeyFrames;
											if (doubleAnimation != null)
											{
												this.ProcessDoubleAnimation(doubleAnimation);
											}
											else if (doubleAnimationUsingKeyFrames != null)
											{
												this.ProcessDoubleAnimationWithKeys(doubleAnimationUsingKeyFrames);
											}
										}
									}
								}
							}
						}
					}
				}
				this.UpdateKnownAnimations();
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000083C0 File Offset: 0x000065C0
		private void UpdateKnownAnimations()
		{
			foreach (RelativeAnimatingContentControl.AnimationValueAdapter animationValueAdapter in this._specialAnimations)
			{
				animationValueAdapter.UpdateWithNewDimension(this._knownWidth, this._knownHeight);
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008420 File Offset: 0x00006620
		private void ProcessDoubleAnimationWithKeys(DoubleAnimationUsingKeyFrames da)
		{
			foreach (DoubleKeyFrame doubleKeyFrame in da.KeyFrames)
			{
				RelativeAnimatingContentControl.DoubleAnimationDimension? dimensionFromMagicNumber = RelativeAnimatingContentControl.GeneralAnimationValueAdapter<DoubleKeyFrame>.GetDimensionFromMagicNumber(doubleKeyFrame.Value);
				if (dimensionFromMagicNumber != null)
				{
					this._specialAnimations.Add(new RelativeAnimatingContentControl.DoubleAnimationFrameAdapter(dimensionFromMagicNumber.Value, doubleKeyFrame));
				}
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008494 File Offset: 0x00006694
		private void ProcessDoubleAnimation(DoubleAnimation da)
		{
			if (da.To != null)
			{
				RelativeAnimatingContentControl.DoubleAnimationDimension? dimensionFromMagicNumber = RelativeAnimatingContentControl.GeneralAnimationValueAdapter<DoubleAnimation>.GetDimensionFromMagicNumber(da.To.Value);
				if (dimensionFromMagicNumber != null)
				{
					this._specialAnimations.Add(new RelativeAnimatingContentControl.DoubleAnimationToAdapter(dimensionFromMagicNumber.Value, da));
				}
			}
			if (da.From != null)
			{
				RelativeAnimatingContentControl.DoubleAnimationDimension? dimensionFromMagicNumber2 = RelativeAnimatingContentControl.GeneralAnimationValueAdapter<DoubleAnimation>.GetDimensionFromMagicNumber(da.To.Value);
				if (dimensionFromMagicNumber2 != null)
				{
					this._specialAnimations.Add(new RelativeAnimatingContentControl.DoubleAnimationFromAdapter(dimensionFromMagicNumber2.Value, da));
				}
			}
		}

		// Token: 0x040000A8 RID: 168
		private const double SimpleDoubleComparisonEpsilon = 9E-06;

		// Token: 0x040000A9 RID: 169
		private double _knownWidth;

		// Token: 0x040000AA RID: 170
		private double _knownHeight;

		// Token: 0x040000AB RID: 171
		private List<RelativeAnimatingContentControl.AnimationValueAdapter> _specialAnimations;

		// Token: 0x02000036 RID: 54
		private enum DoubleAnimationDimension
		{
			// Token: 0x040000AD RID: 173
			Width,
			// Token: 0x040000AE RID: 174
			Height
		}

		// Token: 0x02000037 RID: 55
		private abstract class AnimationValueAdapter
		{
			// Token: 0x0600019E RID: 414 RVA: 0x0000852D File Offset: 0x0000672D
			public AnimationValueAdapter(RelativeAnimatingContentControl.DoubleAnimationDimension dimension)
			{
				this.Dimension = dimension;
			}

			// Token: 0x1700004F RID: 79
			// (get) Token: 0x0600019F RID: 415 RVA: 0x0000853C File Offset: 0x0000673C
			// (set) Token: 0x060001A0 RID: 416 RVA: 0x00008544 File Offset: 0x00006744
			public RelativeAnimatingContentControl.DoubleAnimationDimension Dimension { get; private set; }

			// Token: 0x060001A1 RID: 417
			public abstract void UpdateWithNewDimension(double width, double height);
		}

		// Token: 0x02000038 RID: 56
		private abstract class GeneralAnimationValueAdapter<T> : RelativeAnimatingContentControl.AnimationValueAdapter
		{
			// Token: 0x17000050 RID: 80
			// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000854D File Offset: 0x0000674D
			// (set) Token: 0x060001A3 RID: 419 RVA: 0x00008555 File Offset: 0x00006755
			protected T Instance { get; set; }

			// Token: 0x060001A4 RID: 420
			protected abstract double GetValue();

			// Token: 0x060001A5 RID: 421
			protected abstract void SetValue(double newValue);

			// Token: 0x17000051 RID: 81
			// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000855E File Offset: 0x0000675E
			// (set) Token: 0x060001A7 RID: 423 RVA: 0x00008566 File Offset: 0x00006766
			private protected double InitialValue { protected get; private set; }

			// Token: 0x060001A8 RID: 424 RVA: 0x0000856F File Offset: 0x0000676F
			public GeneralAnimationValueAdapter(RelativeAnimatingContentControl.DoubleAnimationDimension d, T instance) : base(d)
			{
				this.Instance = instance;
				this.InitialValue = this.StripMagicNumberOff(this.GetValue());
				this._ratio = this.InitialValue / 100.0;
			}

			// Token: 0x060001A9 RID: 425 RVA: 0x000085A7 File Offset: 0x000067A7
			public double StripMagicNumberOff(double number)
			{
				if (base.Dimension != RelativeAnimatingContentControl.DoubleAnimationDimension.Width)
				{
					return number - 0.2;
				}
				return number - 0.1;
			}

			// Token: 0x060001AA RID: 426 RVA: 0x000085C8 File Offset: 0x000067C8
			public static RelativeAnimatingContentControl.DoubleAnimationDimension? GetDimensionFromMagicNumber(double number)
			{
				double num = Math.Floor(number);
				double num2 = number - num;
				if (num2 >= 0.09999100000000001 && num2 <= 0.100009)
				{
					return new RelativeAnimatingContentControl.DoubleAnimationDimension?(RelativeAnimatingContentControl.DoubleAnimationDimension.Width);
				}
				if (num2 >= 0.199991 && num2 <= 0.20000900000000002)
				{
					return new RelativeAnimatingContentControl.DoubleAnimationDimension?(RelativeAnimatingContentControl.DoubleAnimationDimension.Height);
				}
				return default(RelativeAnimatingContentControl.DoubleAnimationDimension?);
			}

			// Token: 0x060001AB RID: 427 RVA: 0x00008628 File Offset: 0x00006828
			public override void UpdateWithNewDimension(double width, double height)
			{
				double sizeToUse = (base.Dimension == RelativeAnimatingContentControl.DoubleAnimationDimension.Width) ? width : height;
				this.UpdateValue(sizeToUse);
			}

			// Token: 0x060001AC RID: 428 RVA: 0x00008649 File Offset: 0x00006849
			private void UpdateValue(double sizeToUse)
			{
				this.SetValue(sizeToUse * this._ratio);
			}

			// Token: 0x040000B0 RID: 176
			private double _ratio;
		}

		// Token: 0x02000039 RID: 57
		private class DoubleAnimationToAdapter : RelativeAnimatingContentControl.GeneralAnimationValueAdapter<DoubleAnimation>
		{
			// Token: 0x060001AD RID: 429 RVA: 0x0000865C File Offset: 0x0000685C
			protected override double GetValue()
			{
				return base.Instance.To.Value;
			}

			// Token: 0x060001AE RID: 430 RVA: 0x0000867D File Offset: 0x0000687D
			protected override void SetValue(double newValue)
			{
				base.Instance.To = new double?(newValue);
			}

			// Token: 0x060001AF RID: 431 RVA: 0x00008690 File Offset: 0x00006890
			public DoubleAnimationToAdapter(RelativeAnimatingContentControl.DoubleAnimationDimension dimension, DoubleAnimation instance) : base(dimension, instance)
			{
			}
		}

		// Token: 0x0200003A RID: 58
		private class DoubleAnimationFromAdapter : RelativeAnimatingContentControl.GeneralAnimationValueAdapter<DoubleAnimation>
		{
			// Token: 0x060001B0 RID: 432 RVA: 0x0000869C File Offset: 0x0000689C
			protected override double GetValue()
			{
				return base.Instance.From.Value;
			}

			// Token: 0x060001B1 RID: 433 RVA: 0x000086BD File Offset: 0x000068BD
			protected override void SetValue(double newValue)
			{
				base.Instance.From = new double?(newValue);
			}

			// Token: 0x060001B2 RID: 434 RVA: 0x000086D0 File Offset: 0x000068D0
			public DoubleAnimationFromAdapter(RelativeAnimatingContentControl.DoubleAnimationDimension dimension, DoubleAnimation instance) : base(dimension, instance)
			{
			}
		}

		// Token: 0x0200003B RID: 59
		private class DoubleAnimationFrameAdapter : RelativeAnimatingContentControl.GeneralAnimationValueAdapter<DoubleKeyFrame>
		{
			// Token: 0x060001B3 RID: 435 RVA: 0x000086DA File Offset: 0x000068DA
			protected override double GetValue()
			{
				return base.Instance.Value;
			}

			// Token: 0x060001B4 RID: 436 RVA: 0x000086E7 File Offset: 0x000068E7
			protected override void SetValue(double newValue)
			{
				base.Instance.Value = newValue;
			}

			// Token: 0x060001B5 RID: 437 RVA: 0x000086F5 File Offset: 0x000068F5
			public DoubleAnimationFrameAdapter(RelativeAnimatingContentControl.DoubleAnimationDimension dimension, DoubleKeyFrame frame) : base(dimension, frame)
			{
			}
		}
	}
}
