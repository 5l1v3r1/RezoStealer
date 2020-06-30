using System;

namespace Stealer.modules
{
	// Token: 0x0200000B RID: 11
	internal abstract class GcmUtilities
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002BD4 File Offset: 0x00000DD4
		internal static byte[] OneAsBytes()
		{
			byte[] array = new byte[16];
			array[0] = 128;
			return array;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002BF4 File Offset: 0x00000DF4
		internal static uint[] OneAsUints()
		{
			uint[] array = new uint[4];
			array[0] = 2147483648U;
			return array;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002C14 File Offset: 0x00000E14
		internal static uint[] AsUints(byte[] bs)
		{
			return new uint[]
			{
				Pack.BE_To_UInt32(bs, 0),
				Pack.BE_To_UInt32(bs, 4),
				Pack.BE_To_UInt32(bs, 8),
				Pack.BE_To_UInt32(bs, 12)
			};
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002C54 File Offset: 0x00000E54
		internal static void Multiply(byte[] block, byte[] val)
		{
			byte[] array = Arrays.Clone(block);
			byte[] array2 = new byte[16];
			for (int i = 0; i < 16; i++)
			{
				byte b = val[i];
				for (int j = 7; j >= 0; j--)
				{
					if (((int)b & 1 << j) != 0)
					{
						GcmUtilities.Xor(array2, array);
					}
					bool flag = (array[15] & 1) != 0;
					GcmUtilities.ShiftRight(array);
					if (flag)
					{
						byte[] array3 = array;
						int num = 0;
						array3[num] ^= 225;
					}
				}
			}
			Array.Copy(array2, 0, block, 0, 16);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002CE0 File Offset: 0x00000EE0
		internal static void MultiplyP(uint[] x)
		{
			bool flag = (x[3] & 1U) != 0U;
			GcmUtilities.ShiftRight(x);
			if (flag)
			{
				x[0] ^= 3774873600U;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002D1C File Offset: 0x00000F1C
		internal static void MultiplyP8(uint[] x)
		{
			uint num = x[3];
			GcmUtilities.ShiftRightN(x, 8);
			for (int i = 7; i >= 0; i--)
			{
				if (((ulong)num & (ulong)(1L << (i & 31))) != 0UL)
				{
					x[0] ^= 3774873600U >> 7 - i;
				}
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002D70 File Offset: 0x00000F70
		internal static void ShiftRight(byte[] block)
		{
			int num = 0;
			byte b = 0;
			for (;;)
			{
				byte b2 = block[num];
				block[num] = (byte)(b2 >> 1 | (int)b);
				if (++num == 16)
				{
					break;
				}
				b = (byte)(b2 << 7);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002DA0 File Offset: 0x00000FA0
		internal static void ShiftRight(uint[] block)
		{
			int num = 0;
			uint num2 = 0U;
			for (;;)
			{
				uint num3 = block[num];
				block[num] = (num3 >> 1 | num2);
				if (++num == 4)
				{
					break;
				}
				num2 = num3 << 31;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002DCC File Offset: 0x00000FCC
		internal static void ShiftRightN(uint[] block, int n)
		{
			int num = 0;
			uint num2 = 0U;
			for (;;)
			{
				uint num3 = block[num];
				block[num] = (num3 >> n | num2);
				if (++num == 4)
				{
					break;
				}
				num2 = num3 << 32 - n;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002E00 File Offset: 0x00001000
		internal static void Xor(byte[] block, byte[] val)
		{
			for (int i = 15; i >= 0; i--)
			{
				int num = i;
				block[num] ^= val[i];
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002E34 File Offset: 0x00001034
		internal static void Xor(uint[] block, uint[] val)
		{
			for (int i = 3; i >= 0; i--)
			{
				block[i] ^= val[i];
			}
		}
	}
}
