using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000079 RID: 121
	internal static class StringUtils
	{
		// Token: 0x060005F6 RID: 1526 RVA: 0x00017EA3 File Offset: 0x000160A3
		public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
		{
			ValidationUtils.ArgumentNotNull(format, "format");
			return string.Format(provider, format, args);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00017EB8 File Offset: 0x000160B8
		public static bool ContainsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (char.IsWhiteSpace(s.get_Chars(i)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00017EF8 File Offset: 0x000160F8
		public static bool IsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsWhiteSpace(s.get_Chars(i)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00017F40 File Offset: 0x00016140
		public static string EnsureEndsWith(string target, string value)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (target.Length >= value.Length)
			{
				if (string.Compare(target, target.Length - value.Length, value, 0, value.Length, 5) == 0)
				{
					return target;
				}
				string text = target.TrimEnd(null);
				if (string.Compare(text, text.Length - value.Length, value, 0, value.Length, 5) == 0)
				{
					return target;
				}
			}
			return target + value;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00017FC6 File Offset: 0x000161C6
		public static bool IsNullOrEmptyOrWhiteSpace(string s)
		{
			return string.IsNullOrEmpty(s) || StringUtils.IsWhiteSpace(s);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00017FDD File Offset: 0x000161DD
		public static void IfNotNullOrEmpty(string value, Action<string> action)
		{
			StringUtils.IfNotNullOrEmpty(value, action, null);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00017FE7 File Offset: 0x000161E7
		private static void IfNotNullOrEmpty(string value, Action<string> trueAction, Action<string> falseAction)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (trueAction != null)
				{
					trueAction.Invoke(value);
					return;
				}
			}
			else if (falseAction != null)
			{
				falseAction.Invoke(value);
			}
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00018006 File Offset: 0x00016206
		public static string Indent(string s, int indentation)
		{
			return StringUtils.Indent(s, indentation, ' ');
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0001803C File Offset: 0x0001623C
		public static string Indent(string s, int indentation, char indentChar)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (indentation <= 0)
			{
				throw new ArgumentException("Must be greater than zero.", "indentation");
			}
			StringReader textReader = new StringReader(s);
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			StringUtils.ActionTextReaderLine(textReader, stringWriter, delegate(TextWriter tw, string line)
			{
				tw.Write(new string(indentChar, indentation));
				tw.Write(line);
			});
			return stringWriter.ToString();
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x000180B0 File Offset: 0x000162B0
		private static void ActionTextReaderLine(TextReader textReader, TextWriter textWriter, StringUtils.ActionLine lineAction)
		{
			bool flag = true;
			string line;
			while ((line = textReader.ReadLine()) != null)
			{
				if (!flag)
				{
					textWriter.WriteLine();
				}
				else
				{
					flag = false;
				}
				lineAction(textWriter, line);
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00018128 File Offset: 0x00016328
		public static string NumberLines(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			StringReader textReader = new StringReader(s);
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			int lineNumber = 1;
			StringUtils.ActionTextReaderLine(textReader, stringWriter, delegate(TextWriter tw, string line)
			{
				tw.Write(lineNumber.ToString(CultureInfo.InvariantCulture).PadLeft(4));
				tw.Write(". ");
				tw.Write(line);
				lineNumber++;
			});
			return stringWriter.ToString();
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001817B File Offset: 0x0001637B
		public static string NullEmptyString(string s)
		{
			if (!string.IsNullOrEmpty(s))
			{
				return s;
			}
			return null;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00018188 File Offset: 0x00016388
		public static string ReplaceNewLines(string s, string replacement)
		{
			StringReader stringReader = new StringReader(s);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			string text;
			while ((text = stringReader.ReadLine()) != null)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(replacement);
				}
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000181CD File Offset: 0x000163CD
		public static string Truncate(string s, int maximumLength)
		{
			return StringUtils.Truncate(s, maximumLength, "...");
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x000181DC File Offset: 0x000163DC
		public static string Truncate(string s, int maximumLength, string suffix)
		{
			if (suffix == null)
			{
				throw new ArgumentNullException("suffix");
			}
			if (maximumLength <= 0)
			{
				throw new ArgumentException("Maximum length must be greater than zero.", "maximumLength");
			}
			int num = maximumLength - suffix.Length;
			if (num <= 0)
			{
				throw new ArgumentException("Length of suffix string is greater or equal to maximumLength");
			}
			if (s != null && s.Length > maximumLength)
			{
				string text = s.Substring(0, num);
				text = text.Trim();
				return text + suffix;
			}
			return s;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001824C File Offset: 0x0001644C
		public static StringWriter CreateStringWriter(int capacity)
		{
			StringBuilder stringBuilder = new StringBuilder(capacity);
			return new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00018270 File Offset: 0x00016470
		public static int? GetLength(string value)
		{
			if (value == null)
			{
				return default(int?);
			}
			return new int?(value.Length);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00018298 File Offset: 0x00016498
		public static string ToCharAsUnicode(char c)
		{
			char c2 = MathUtils.IntToHex((int)(c >> 12 & '\u000f'));
			char c3 = MathUtils.IntToHex((int)(c >> 8 & '\u000f'));
			char c4 = MathUtils.IntToHex((int)(c >> 4 & '\u000f'));
			char c5 = MathUtils.IntToHex((int)(c & '\u000f'));
			return new string(new char[]
			{
				'\\',
				'u',
				c2,
				c3,
				c4,
				c5
			});
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00018304 File Offset: 0x00016504
		public static void WriteCharAsUnicode(TextWriter writer, char c)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			char c2 = MathUtils.IntToHex((int)(c >> 12 & '\u000f'));
			char c3 = MathUtils.IntToHex((int)(c >> 8 & '\u000f'));
			char c4 = MathUtils.IntToHex((int)(c >> 4 & '\u000f'));
			char c5 = MathUtils.IntToHex((int)(c & '\u000f'));
			writer.Write('\\');
			writer.Write('u');
			writer.Write(c2);
			writer.Write(c3);
			writer.Write(c4);
			writer.Write(c5);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x000183BC File Offset: 0x000165BC
		public static TSource ForgivingCaseSensitiveFind<TSource>(this IEnumerable<TSource> source, Func<TSource, string> valueSelector, string testValue)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (valueSelector == null)
			{
				throw new ArgumentNullException("valueSelector");
			}
			IEnumerable<TSource> enumerable = Enumerable.Where<TSource>(source, (TSource s) => string.Compare(valueSelector.Invoke(s), testValue, 5) == 0);
			if (Enumerable.Count<TSource>(enumerable) <= 1)
			{
				return Enumerable.SingleOrDefault<TSource>(enumerable);
			}
			IEnumerable<TSource> enumerable2 = Enumerable.Where<TSource>(source, (TSource s) => string.Compare(valueSelector.Invoke(s), testValue, 4) == 0);
			return Enumerable.SingleOrDefault<TSource>(enumerable2);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00018444 File Offset: 0x00016644
		public static string ToCamelCase(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			if (!char.IsUpper(s.get_Chars(0)))
			{
				return s;
			}
			string text = char.ToLower(s.get_Chars(0), CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
			if (s.Length > 1)
			{
				text += s.Substring(1);
			}
			return text;
		}

		// Token: 0x040001A4 RID: 420
		public const string CarriageReturnLineFeed = "\r\n";

		// Token: 0x040001A5 RID: 421
		public const string Empty = "";

		// Token: 0x040001A6 RID: 422
		public const char CarriageReturn = '\r';

		// Token: 0x040001A7 RID: 423
		public const char LineFeed = '\n';

		// Token: 0x040001A8 RID: 424
		public const char Tab = '\t';

		// Token: 0x0200007A RID: 122
		// (Invoke) Token: 0x0600060C RID: 1548
		private delegate void ActionLine(TextWriter textWriter, string line);
	}
}
