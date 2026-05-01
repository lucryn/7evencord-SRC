using System;

namespace Microsoft.Phone.Controls
{
	// Token: 0x0200007B RID: 123
	internal static class AutoCompleteSearch
	{
		// Token: 0x060004DB RID: 1243 RVA: 0x000150C0 File Offset: 0x000132C0
		public static AutoCompleteFilterPredicate<string> GetFilter(AutoCompleteFilterMode FilterMode)
		{
			switch (FilterMode)
			{
			case AutoCompleteFilterMode.StartsWith:
				return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.StartsWith);
			case AutoCompleteFilterMode.StartsWithCaseSensitive:
				return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.StartsWithCaseSensitive);
			case AutoCompleteFilterMode.StartsWithOrdinal:
				return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.StartsWithOrdinal);
			case AutoCompleteFilterMode.StartsWithOrdinalCaseSensitive:
				return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.StartsWithOrdinalCaseSensitive);
			case AutoCompleteFilterMode.Contains:
				return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.Contains);
			case AutoCompleteFilterMode.ContainsCaseSensitive:
				return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.ContainsCaseSensitive);
			case AutoCompleteFilterMode.ContainsOrdinal:
				return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.ContainsOrdinal);
			case AutoCompleteFilterMode.ContainsOrdinalCaseSensitive:
				return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.ContainsOrdinalCaseSensitive);
			case AutoCompleteFilterMode.Equals:
				return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.Equals);
			case AutoCompleteFilterMode.EqualsCaseSensitive:
				return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.EqualsCaseSensitive);
			case AutoCompleteFilterMode.EqualsOrdinal:
				return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.EqualsOrdinal);
			case AutoCompleteFilterMode.EqualsOrdinalCaseSensitive:
				return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.EqualsOrdinalCaseSensitive);
			}
			return null;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x000151AF File Offset: 0x000133AF
		public static bool StartsWith(string text, string value)
		{
			return value.StartsWith(text, 1);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x000151B9 File Offset: 0x000133B9
		public static bool StartsWithCaseSensitive(string text, string value)
		{
			return value.StartsWith(text, 0);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x000151C3 File Offset: 0x000133C3
		public static bool StartsWithOrdinal(string text, string value)
		{
			return value.StartsWith(text, 5);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x000151CD File Offset: 0x000133CD
		public static bool StartsWithOrdinalCaseSensitive(string text, string value)
		{
			return value.StartsWith(text, 4);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x000151D7 File Offset: 0x000133D7
		public static bool Contains(string text, string value)
		{
			return value.Contains(text, 1);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000151E1 File Offset: 0x000133E1
		public static bool ContainsCaseSensitive(string text, string value)
		{
			return value.Contains(text, 0);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x000151EB File Offset: 0x000133EB
		public static bool ContainsOrdinal(string text, string value)
		{
			return value.Contains(text, 5);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x000151F5 File Offset: 0x000133F5
		public static bool ContainsOrdinalCaseSensitive(string text, string value)
		{
			return value.Contains(text, 4);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x000151FF File Offset: 0x000133FF
		public static bool Equals(string text, string value)
		{
			return value.Equals(text, 1);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00015209 File Offset: 0x00013409
		public static bool EqualsCaseSensitive(string text, string value)
		{
			return value.Equals(text, 0);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00015213 File Offset: 0x00013413
		public static bool EqualsOrdinal(string text, string value)
		{
			return value.Equals(text, 5);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001521D File Offset: 0x0001341D
		public static bool EqualsOrdinalCaseSensitive(string text, string value)
		{
			return value.Equals(text, 4);
		}
	}
}
