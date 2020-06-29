using System;

namespace Stealer.modules
{
	// Token: 0x02000007 RID: 7
	internal sealed class Pack
	{
		// Token: 0x06000015 RID: 21 RVA: 0x0000289F File Offset: 0x00000A9F
		private Pack()
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000028A7 File Offset: 0x00000AA7
		internal static void UInt32_To_BE(uint n, byte[] bs)
		{
			bs[0] = (byte)(n >> 24);
			bs[1] = (byte)(n >> 16);
			bs[2] = (byte)(n >> 8);
			bs[3] = (byte)n;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000028C5 File Offset: 0x00000AC5
		internal static void UInt32_To_BE(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)(n >> 24);
			bs[++off] = (byte)(n >> 16);
			bs[++off] = (byte)(n >> 8);
			bs[++off] = (byte)n;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000028F2 File Offset: 0x00000AF2
		internal static uint BE_To_UInt32(byte[] bs)
		{
			return (uint)((int)bs[0] << 24 | (int)bs[1] << 16 | (int)bs[2] << 8 | (int)bs[3]);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000290B File Offset: 0x00000B0B
		internal static uint BE_To_UInt32(byte[] bs, int off)
		{
			return (uint)((int)bs[off] << 24 | (int)bs[++off] << 16 | (int)bs[++off] << 8 | (int)bs[++off]);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002934 File Offset: 0x00000B34
		internal static ulong BE_To_UInt64(byte[] bs)
		{
			uint num = Pack.BE_To_UInt32(bs);
			uint num2 = Pack.BE_To_UInt32(bs, 4);
			return (ulong)num << 32 | (ulong)num2;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002958 File Offset: 0x00000B58
		internal static ulong BE_To_UInt64(byte[] bs, int off)
		{
			uint num = Pack.BE_To_UInt32(bs, off);
			uint num2 = Pack.BE_To_UInt32(bs, off + 4);
			return (ulong)num << 32 | (ulong)num2;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000297F File Offset: 0x00000B7F
		internal static void UInt64_To_BE(ulong n, byte[] bs)
		{
			Pack.UInt32_To_BE((uint)(n >> 32), bs);
			Pack.UInt32_To_BE((uint)n, bs, 4);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002995 File Offset: 0x00000B95
		internal static void UInt64_To_BE(ulong n, byte[] bs, int off)
		{
			Pack.UInt32_To_BE((uint)(n >> 32), bs, off);
			Pack.UInt32_To_BE((uint)n, bs, off + 4);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000029AE File Offset: 0x00000BAE
		internal static void UInt32_To_LE(uint n, byte[] bs)
		{
			bs[0] = (byte)n;
			bs[1] = (byte)(n >> 8);
			bs[2] = (byte)(n >> 16);
			bs[3] = (byte)(n >> 24);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000029CC File Offset: 0x00000BCC
		internal static void UInt32_To_LE(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)n;
			bs[++off] = (byte)(n >> 8);
			bs[++off] = (byte)(n >> 16);
			bs[++off] = (byte)(n >> 24);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000029F9 File Offset: 0x00000BF9
		internal static uint LE_To_UInt32(byte[] bs)
		{
			return (uint)((int)bs[0] | (int)bs[1] << 8 | (int)bs[2] << 16 | (int)bs[3] << 24);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002A12 File Offset: 0x00000C12
		internal static uint LE_To_UInt32(byte[] bs, int off)
		{
			return (uint)((int)bs[off] | (int)bs[++off] << 8 | (int)bs[++off] << 16 | (int)bs[++off] << 24);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002A3C File Offset: 0x00000C3C
		internal static ulong LE_To_UInt64(byte[] bs)
		{
			uint num = Pack.LE_To_UInt32(bs);
			return (ulong)Pack.LE_To_UInt32(bs, 4) << 32 | (ulong)num;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002A60 File Offset: 0x00000C60
		internal static ulong LE_To_UInt64(byte[] bs, int off)
		{
			uint num = Pack.LE_To_UInt32(bs, off);
			return (ulong)Pack.LE_To_UInt32(bs, off + 4) << 32 | (ulong)num;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002A85 File Offset: 0x00000C85
		internal static void UInt64_To_LE(ulong n, byte[] bs)
		{
			Pack.UInt32_To_LE((uint)n, bs);
			Pack.UInt32_To_LE((uint)(n >> 32), bs, 4);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002A9B File Offset: 0x00000C9B
		internal static void UInt64_To_LE(ulong n, byte[] bs, int off)
		{
			Pack.UInt32_To_LE((uint)n, bs, off);
			Pack.UInt32_To_LE((uint)(n >> 32), bs, off + 4);
		}
	}
}
