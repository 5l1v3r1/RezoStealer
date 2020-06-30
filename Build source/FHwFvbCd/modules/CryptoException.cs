using System;

namespace Stealer.modules
{
	// Token: 0x02000012 RID: 18
	public class CryptoException : Exception
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00006B88 File Offset: 0x00004D88
		public CryptoException()
		{
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00006B90 File Offset: 0x00004D90
		public CryptoException(string message) : base(message)
		{
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00006B99 File Offset: 0x00004D99
		public CryptoException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
