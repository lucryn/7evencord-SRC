using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200001A RID: 26
	public class Transition : ITransition
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000C3 RID: 195 RVA: 0x00004754 File Offset: 0x00002954
		// (remove) Token: 0x060000C4 RID: 196 RVA: 0x00004762 File Offset: 0x00002962
		public event EventHandler Completed
		{
			add
			{
				this._storyboard.Completed += value;
			}
			remove
			{
				this._storyboard.Completed -= value;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004770 File Offset: 0x00002970
		public Transition(UIElement element, Storyboard storyboard)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (storyboard == null)
			{
				throw new ArgumentNullException("storyboard");
			}
			this._element = element;
			this._storyboard = storyboard;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000047AA File Offset: 0x000029AA
		public void Begin()
		{
			this.Save();
			this._storyboard.Completed += delegate(object A_1, EventArgs A_2)
			{
				this.Restore();
			};
			this._storyboard.Begin();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000047D4 File Offset: 0x000029D4
		public ClockState GetCurrentState()
		{
			return this._storyboard.GetCurrentState();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000047E1 File Offset: 0x000029E1
		public TimeSpan GetCurrentTime()
		{
			return this._storyboard.GetCurrentTime();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000047EE File Offset: 0x000029EE
		public void Pause()
		{
			this._storyboard.Pause();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000047FB File Offset: 0x000029FB
		private void Restore()
		{
			if (!(this._cacheMode is BitmapCache))
			{
				this._element.CacheMode = this._cacheMode;
			}
			if (this._isHitTestVisible)
			{
				this._element.IsHitTestVisible = this._isHitTestVisible;
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004834 File Offset: 0x00002A34
		public void Resume()
		{
			this._storyboard.Resume();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004844 File Offset: 0x00002A44
		private void Save()
		{
			this._cacheMode = this._element.CacheMode;
			if (!(this._cacheMode is BitmapCache))
			{
				this._element.CacheMode = new BitmapCache();
			}
			this._isHitTestVisible = this._element.IsHitTestVisible;
			if (this._isHitTestVisible)
			{
				this._element.IsHitTestVisible = false;
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000048A4 File Offset: 0x00002AA4
		public void Seek(TimeSpan offset)
		{
			this._storyboard.Seek(offset);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000048B2 File Offset: 0x00002AB2
		public void SeekAlignedToLastTick(TimeSpan offset)
		{
			this._storyboard.SeekAlignedToLastTick(offset);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000048C0 File Offset: 0x00002AC0
		public void SkipToFill()
		{
			this._storyboard.SkipToFill();
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000048CD File Offset: 0x00002ACD
		public void Stop()
		{
			this._storyboard.Stop();
			this.Restore();
		}

		// Token: 0x04000042 RID: 66
		private CacheMode _cacheMode;

		// Token: 0x04000043 RID: 67
		private UIElement _element;

		// Token: 0x04000044 RID: 68
		private bool _isHitTestVisible;

		// Token: 0x04000045 RID: 69
		private Storyboard _storyboard;
	}
}
