using System;
using System.Text;

namespace Stealer.modules
{
	// Token: 0x02000006 RID: 6
	public sealed class Arrays
	{
		// Token: 0x06000006 RID: 6 RVA: 0x0000269C File Offset: 0x0000089C
		private Arrays()
		{
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000026A4 File Offset: 0x000008A4
		public static bool AreEqual(bool[] a, bool[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000026BB File Offset: 0x000008BB
		public static bool AreEqual(char[] a, char[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000026D2 File Offset: 0x000008D2
		public static bool AreEqual(byte[] a, byte[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000026E9 File Offset: 0x000008E9
		[Obsolete("Use 'AreEqual' method instead")]
		public static bool AreSame(byte[] a, byte[] b)
		{
			return Arrays.AreEqual(a, b);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000026F4 File Offset: 0x000008F4
		public static bool ConstantTimeAreEqual(byte[] a, byte[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			int num2 = 0;
			while (num != 0)
			{
				num--;
				num2 |= (int)(a[num] ^ b[num]);
			}
			return num2 == 0;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002726 File Offset: 0x00000926
		public static bool AreEqual(int[] a, int[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002740 File Offset: 0x00000940
		private static bool HaveSameContents(bool[] a, bool[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000276C File Offset: 0x0000096C
		private static bool HaveSameContents(char[] a, char[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002798 File Offset: 0x00000998
		private static bool HaveSameContents(byte[] a, byte[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000027C4 File Offset: 0x000009C4
		private static bool HaveSameContents(int[] a, int[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000027F0 File Offset: 0x000009F0
		public static string ToString(object[] a)
		{
			StringBuilder stringBuilder = new StringBuilder(91);
			if (a.Length != 0)
			{
				stringBuilder.Append(a[0]);
				for (int i = 1; i < a.Length; i++)
				{
					stringBuilder.Append(", ").Append(a[i]);
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002848 File Offset: 0x00000A48
		public static int GetHashCode(byte[] data)
		{
			if (data == null)
			{
				return 0;
			}
			int num = data.Length;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= (int)data[num];
			}
			return num2;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000287B File Offset: 0x00000A7B
		public static byte[] Clone(byte[] data)
		{
			if (data != null)
			{
				return (byte[])data.Clone();
			}
			return null;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000288D File Offset: 0x00000A8D
		public static int[] Clone(int[] data)
		{
			if (data != null)
			{
				return (int[])data.Clone();
			}
			return null;
		}
	}
}
