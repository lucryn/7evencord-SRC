using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.Phone.Controls
{
	// Token: 0x02000006 RID: 6
	internal static class PhoneHelper
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002802 File Offset: 0x00000A02
		public static bool TryGetPhoneApplicationFrame(out PhoneApplicationFrame phoneApplicationFrame)
		{
			phoneApplicationFrame = (Application.Current.RootVisual as PhoneApplicationFrame);
			return phoneApplicationFrame != null;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002820 File Offset: 0x00000A20
		public static bool IsPortrait(this PhoneApplicationFrame phoneApplicationFrame)
		{
			PageOrientation pageOrientation = 13;
			return (pageOrientation & phoneApplicationFrame.Orientation) == phoneApplicationFrame.Orientation;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002840 File Offset: 0x00000A40
		public static double GetUsefulWidth(this PhoneApplicationFrame phoneApplicationFrame)
		{
			if (!phoneApplicationFrame.IsPortrait())
			{
				return phoneApplicationFrame.ActualHeight;
			}
			return phoneApplicationFrame.ActualWidth;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002857 File Offset: 0x00000A57
		public static double GetUsefulHeight(this PhoneApplicationFrame phoneApplicationFrame)
		{
			if (!phoneApplicationFrame.IsPortrait())
			{
				return phoneApplicationFrame.ActualWidth;
			}
			return phoneApplicationFrame.ActualHeight;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000286E File Offset: 0x00000A6E
		public static Size GetUsefulSize(this PhoneApplicationFrame phoneApplicationFrame)
		{
			return new Size(phoneApplicationFrame.GetUsefulWidth(), phoneApplicationFrame.GetUsefulHeight());
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002881 File Offset: 0x00000A81
		private static bool TryGetFocusedTextBox(out TextBox textBox)
		{
			textBox = (FocusManager.GetFocusedElement() as TextBox);
			return textBox != null;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002898 File Offset: 0x00000A98
		public static bool IsSipShown()
		{
			TextBox textBox;
			return PhoneHelper.TryGetFocusedTextBox(out textBox);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000028AC File Offset: 0x00000AAC
		public static bool IsSipTextCompletionShown(this TextBox textBox)
		{
			if (textBox.InputScope == null)
			{
				return false;
			}
			IList names = textBox.InputScope.Names;
			foreach (object obj in names)
			{
				InputScopeName inputScopeName = (InputScopeName)obj;
				switch (inputScopeName.NameValue)
				{
				case 49:
				case 50:
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002938 File Offset: 0x00000B38
		public static Size GetSipCoveredSize(this PhoneApplicationFrame phoneApplicationFrame)
		{
			if (!PhoneHelper.IsSipShown())
			{
				return new Size(0.0, 0.0);
			}
			double usefulWidth = phoneApplicationFrame.GetUsefulWidth();
			double num = phoneApplicationFrame.IsPortrait() ? 339.0 : 259.0;
			TextBox textBox;
			if (PhoneHelper.TryGetFocusedTextBox(out textBox) && textBox.IsSipTextCompletionShown())
			{
				num += 62.0;
			}
			return new Size(usefulWidth, num);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000029AC File Offset: 0x00000BAC
		public static Size GetSipUncoveredSize(this PhoneApplicationFrame phoneApplicationFrame)
		{
			double usefulWidth = phoneApplicationFrame.GetUsefulWidth();
			double num = phoneApplicationFrame.GetUsefulHeight() - phoneApplicationFrame.GetSipCoveredSize().Height;
			return new Size(usefulWidth, num);
		}

		// Token: 0x04000014 RID: 20
		public const double SipLandscapeHeight = 259.0;

		// Token: 0x04000015 RID: 21
		public const double SipPortraitHeight = 339.0;

		// Token: 0x04000016 RID: 22
		public const double SipTextCompletionHeight = 62.0;
	}
}
