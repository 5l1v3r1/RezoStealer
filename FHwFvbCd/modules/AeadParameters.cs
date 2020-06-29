using System;

namespace Stealer.modules
{
	// Token: 0x02000016 RID: 22
	public class AeadParameters : ICipherParameters
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00006C78 File Offset: 0x00004E78
		public virtual KeyParameter Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00006C80 File Offset: 0x00004E80
		public virtual int MacSize
		{
			get
			{
				return this.macSize;
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00006C88 File Offset: 0x00004E88
		public AeadParameters(KeyParameter key, int macSize, byte[] nonce, byte[] associatedText)
		{
			this.key = key;
			this.nonce = nonce;
			this.macSize = macSize;
			this.associatedText = associatedText;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00006CAD File Offset: 0x00004EAD
		public virtual byte[] GetAssociatedText()
		{
			return this.associatedText;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00006CB5 File Offset: 0x00004EB5
		public virtual byte[] GetNonce()
		{
			return this.nonce;
		}

		// Token: 0x04000033 RID: 51
		private readonly byte[] associatedText;

		// Token: 0x04000034 RID: 52
		private readonly byte[] nonce;

		// Token: 0x04000035 RID: 53
		private readonly KeyParameter key;

		// Token: 0x04000036 RID: 54
		private readonly int macSize;
	}
}
