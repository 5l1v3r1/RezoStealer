using System;

namespace Stealer.modules
{
	// Token: 0x02000013 RID: 19
	public class DataLengthException : CryptoException
	{
		// Token: 0x06000074 RID: 116 RVA: 0x00006BA3 File Offset: 0x00004DA3
		public DataLengthException()
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00006BAB File Offset: 0x00004DAB
		public DataLengthException(string message) : base(message)
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00006BB4 File Offset: 0x00004DB4
		public DataLengthException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
