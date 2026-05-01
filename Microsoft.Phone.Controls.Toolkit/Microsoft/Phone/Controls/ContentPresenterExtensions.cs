using System;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000030 RID: 48
	internal static class ContentPresenterExtensions
	{
		// Token: 0x06000187 RID: 391 RVA: 0x00007E14 File Offset: 0x00006014
		public static void GetExtraData(this ContentPresenter cp, out int resolvedIndex, out double lastDesiredHeight)
		{
			ContentPresenterExtensions.ExtraData extraData = cp.Tag as ContentPresenterExtensions.ExtraData;
			if (extraData != null)
			{
				resolvedIndex = extraData.ResolvedIndex;
				lastDesiredHeight = extraData.LastDesiredHeight;
				return;
			}
			resolvedIndex = -1;
			lastDesiredHeight = 0.0;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00007E50 File Offset: 0x00006050
		public static void SetExtraData(this ContentPresenter cp, int resolvedIndex, double lastDesiredHeight)
		{
			ContentPresenterExtensions.ExtraData extraData = cp.Tag as ContentPresenterExtensions.ExtraData;
			if (extraData == null)
			{
				extraData = (cp.Tag = new ContentPresenterExtensions.ExtraData());
			}
			extraData.ResolvedIndex = resolvedIndex;
			extraData.LastDesiredHeight = lastDesiredHeight;
		}

		// Token: 0x02000031 RID: 49
		private class ExtraData
		{
			// Token: 0x04000099 RID: 153
			public int ResolvedIndex;

			// Token: 0x0400009A RID: 154
			public double LastDesiredHeight;
		}
	}
}
