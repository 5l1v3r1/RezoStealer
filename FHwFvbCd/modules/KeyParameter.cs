using System;

namespace Stealer.modules
{
	// Token: 0x0200000A RID: 10
	public class KeyParameter : ICipherParameters
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002B31 File Offset: 0x00000D31
		public KeyParameter(byte[] key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.key = (byte[])key.Clone();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002B58 File Offset: 0x00000D58
		public KeyParameter(byte[] key, int keyOff, int keyLen)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (keyOff < 0 || keyOff > key.Length)
			{
				throw new ArgumentOutOfRangeException("keyOff");
			}
			if (keyLen < 0 || keyOff + keyLen > key.Length)
			{
				throw new ArgumentOutOfRangeException("keyLen");
			}
			this.key = new byte[keyLen];
			Array.Copy(key, keyOff, this.key, 0, keyLen);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002BC0 File Offset: 0x00000DC0
		public byte[] GetKey()
		{
			return (byte[])this.key.Clone();
		}

		// Token: 0x04000009 RID: 9
		private readonly byte[] key;
	}
}
