using System;
using System.Windows;

namespace Microsoft.Expression.Interactivity.Layout
{
	// Token: 0x0200001A RID: 26
	public sealed class FluidMoveSetTagBehavior : FluidMoveBehaviorBase
	{
		// Token: 0x060000DD RID: 221 RVA: 0x00006480 File Offset: 0x00004680
		internal override void UpdateLayoutTransitionCore(FrameworkElement child, FrameworkElement root, object tag, FluidMoveBehaviorBase.TagData newTagData)
		{
			FluidMoveBehaviorBase.TagData tagData;
			if (!FluidMoveBehaviorBase.TagDictionary.TryGetValue(tag, ref tagData))
			{
				tagData = new FluidMoveBehaviorBase.TagData();
				FluidMoveBehaviorBase.TagDictionary.Add(tag, tagData);
			}
			tagData.ParentRect = newTagData.ParentRect;
			tagData.AppRect = newTagData.AppRect;
			tagData.Parent = newTagData.Parent;
			tagData.Child = newTagData.Child;
			tagData.Timestamp = newTagData.Timestamp;
		}
	}
}
