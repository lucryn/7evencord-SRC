using System;

namespace Microsoft.Expression.Controls
{
	// Token: 0x02000011 RID: 17
	public interface IPathLayoutItem
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000087 RID: 135
		// (remove) Token: 0x06000088 RID: 136
		event EventHandler<PathLayoutUpdatedEventArgs> PathLayoutUpdated;

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000089 RID: 137
		int LayoutPathIndex { get; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600008A RID: 138
		int GlobalIndex { get; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600008B RID: 139
		int LocalIndex { get; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600008C RID: 140
		double GlobalOffset { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600008D RID: 141
		double LocalOffset { get; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600008E RID: 142
		double NormalAngle { get; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600008F RID: 143
		double OrientationAngle { get; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000090 RID: 144
		bool IsArranged { get; }

		// Token: 0x06000091 RID: 145
		void Update(PathLayoutData data);
	}
}
