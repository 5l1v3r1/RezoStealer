using System;

namespace Stealer.modules
{
	// Token: 0x0200000F RID: 15
	public class GcmBlockCipher : IAeadBlockCipher
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003170 File Offset: 0x00001370
		public virtual string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/GCM";
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003187 File Offset: 0x00001387
		public GcmBlockCipher(IBlockCipher c) : this(c, null)
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003194 File Offset: 0x00001394
		public GcmBlockCipher(IBlockCipher c, IGcmMultiplier m)
		{
			if (c.GetBlockSize() != 16)
			{
				throw new ArgumentException("cipher required with a block size of " + 16 + ".");
			}
			if (m == null)
			{
				m = new Tables8kGcmMultiplier();
			}
			this.cipher = c;
			this.multiplier = m;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000031E5 File Offset: 0x000013E5
		public virtual int GetBlockSize()
		{
			return 16;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000031EC File Offset: 0x000013EC
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
			this.macBlock = null;
			if (parameters is AeadParameters)
			{
				AeadParameters aeadParameters = (AeadParameters)parameters;
				this.nonce = aeadParameters.GetNonce();
				this.A = aeadParameters.GetAssociatedText();
				int num = aeadParameters.MacSize;
				if (num < 96 || num > 128 || num % 8 != 0)
				{
					throw new ArgumentException("Invalid value for MAC size: " + num);
				}
				this.macSize = num / 8;
				this.keyParam = aeadParameters.Key;
			}
			else
			{
				if (!(parameters is ParametersWithIV))
				{
					throw new ArgumentException("invalid parameters passed to GCM");
				}
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				this.nonce = parametersWithIV.GetIV();
				this.A = null;
				this.macSize = 16;
				this.keyParam = (KeyParameter)parametersWithIV.Parameters;
			}
			int num2 = forEncryption ? 16 : (16 + this.macSize);
			this.bufBlock = new byte[num2];
			if (this.nonce == null || this.nonce.Length < 1)
			{
				throw new ArgumentException("IV must be at least 1 byte");
			}
			if (this.A == null)
			{
				this.A = new byte[0];
			}
			this.cipher.Init(true, this.keyParam);
			this.H = new byte[16];
			this.cipher.ProcessBlock(this.H, 0, this.H, 0);
			this.multiplier.Init(this.H);
			this.initS = this.gHASH(this.A);
			if (this.nonce.Length == 12)
			{
				this.J0 = new byte[16];
				Array.Copy(this.nonce, 0, this.J0, 0, this.nonce.Length);
				this.J0[15] = 1;
			}
			else
			{
				this.J0 = this.gHASH(this.nonce);
				byte[] array = new byte[16];
				GcmBlockCipher.packLength((ulong)((long)this.nonce.Length * 8L), array, 8);
				GcmUtilities.Xor(this.J0, array);
				this.multiplier.MultiplyH(this.J0);
			}
			this.S = Arrays.Clone(this.initS);
			this.counter = Arrays.Clone(this.J0);
			this.bufOff = 0;
			this.totalLength = 0UL;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003423 File Offset: 0x00001623
		public virtual byte[] GetMac()
		{
			return Arrays.Clone(this.macBlock);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003430 File Offset: 0x00001630
		public virtual int GetOutputSize(int len)
		{
			if (!this.forEncryption)
			{
				return len + this.bufOff - this.macSize;
			}
			return len + this.bufOff + this.macSize;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003459 File Offset: 0x00001659
		public virtual int GetUpdateOutputSize(int len)
		{
			return (len + this.bufOff) / 16 * 16;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003469 File Offset: 0x00001669
		public virtual int ProcessByte(byte input, byte[] output, int outOff)
		{
			return this.Process(input, output, outOff);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003474 File Offset: 0x00001674
		public virtual int ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
		{
			int num = 0;
			for (int num2 = 0; num2 != len; num2++)
			{
				this.bufBlock[this.bufOff++] = input[inOff + num2];
				if (this.bufOff == this.bufBlock.Length)
				{
					this.gCTRBlock(this.bufBlock, 16, output, outOff + num);
					if (!this.forEncryption)
					{
						Array.Copy(this.bufBlock, 16, this.bufBlock, 0, this.macSize);
					}
					this.bufOff = this.bufBlock.Length - 16;
					num += 16;
				}
			}
			return num;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000350C File Offset: 0x0000170C
		private int Process(byte input, byte[] output, int outOff)
		{
			this.bufBlock[this.bufOff++] = input;
			if (this.bufOff != this.bufBlock.Length)
			{
				return 0;
			}
			this.gCTRBlock(this.bufBlock, 16, output, outOff);
			if (!this.forEncryption)
			{
				Array.Copy(this.bufBlock, 16, this.bufBlock, 0, this.macSize);
			}
			this.bufOff = this.bufBlock.Length - 16;
			return 16;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000358C File Offset: 0x0000178C
		public int DoFinal(byte[] output, int outOff)
		{
			int num = this.bufOff;
			if (!this.forEncryption)
			{
				if (num < this.macSize)
				{
					throw new InvalidCipherTextException("data too short");
				}
				num -= this.macSize;
			}
			if (num > 0)
			{
				byte[] array = new byte[16];
				Array.Copy(this.bufBlock, 0, array, 0, num);
				this.gCTRBlock(array, num, output, outOff);
			}
			byte[] array2 = new byte[16];
			GcmBlockCipher.packLength((ulong)((long)this.A.Length * 8L), array2, 0);
			GcmBlockCipher.packLength(this.totalLength * 8UL, array2, 8);
			GcmUtilities.Xor(this.S, array2);
			this.multiplier.MultiplyH(this.S);
			byte[] array3 = new byte[16];
			this.cipher.ProcessBlock(this.J0, 0, array3, 0);
			GcmUtilities.Xor(array3, this.S);
			int num2 = num;
			this.macBlock = new byte[this.macSize];
			Array.Copy(array3, 0, this.macBlock, 0, this.macSize);
			if (this.forEncryption)
			{
				Array.Copy(this.macBlock, 0, output, outOff + this.bufOff, this.macSize);
				num2 += this.macSize;
			}
			else
			{
				byte[] array4 = new byte[this.macSize];
				Array.Copy(this.bufBlock, num, array4, 0, this.macSize);
				if (!Arrays.ConstantTimeAreEqual(this.macBlock, array4))
				{
					throw new InvalidCipherTextException("mac check in GCM failed");
				}
			}
			this.Reset(false);
			return num2;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000036F8 File Offset: 0x000018F8
		public virtual void Reset()
		{
			this.Reset(true);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003704 File Offset: 0x00001904
		private void Reset(bool clearMac)
		{
			this.S = Arrays.Clone(this.initS);
			this.counter = Arrays.Clone(this.J0);
			this.bufOff = 0;
			this.totalLength = 0UL;
			if (this.bufBlock != null)
			{
				Array.Clear(this.bufBlock, 0, this.bufBlock.Length);
			}
			if (clearMac)
			{
				this.macBlock = null;
			}
			this.cipher.Reset();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003774 File Offset: 0x00001974
		private void gCTRBlock(byte[] buf, int bufCount, byte[] output, int outOff)
		{
			for (int i = 15; i >= 12; i--)
			{
				byte[] array = this.counter;
				int num = i;
				if ((array[num] += 1) != 0)
				{
					break;
				}
			}
			byte[] array2 = new byte[16];
			this.cipher.ProcessBlock(this.counter, 0, array2, 0);
			byte[] val;
			if (this.forEncryption)
			{
				Array.Copy(GcmBlockCipher.Zeroes, bufCount, array2, bufCount, 16 - bufCount);
				val = array2;
			}
			else
			{
				val = buf;
			}
			for (int j = bufCount - 1; j >= 0; j--)
			{
				byte[] array3 = array2;
				int num2 = j;
				array3[num2] ^= buf[j];
				output[outOff + j] = array2[j];
			}
			GcmUtilities.Xor(this.S, val);
			this.multiplier.MultiplyH(this.S);
			this.totalLength += (ulong)((long)bufCount);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003848 File Offset: 0x00001A48
		private byte[] gHASH(byte[] b)
		{
			byte[] array = new byte[16];
			for (int i = 0; i < b.Length; i += 16)
			{
				byte[] array2 = new byte[16];
				int length = Math.Min(b.Length - i, 16);
				Array.Copy(b, i, array2, 0, length);
				GcmUtilities.Xor(array, array2);
				this.multiplier.MultiplyH(array);
			}
			return array;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000389F File Offset: 0x00001A9F
		private static void packLength(ulong len, byte[] bs, int off)
		{
			Pack.UInt32_To_BE((uint)(len >> 32), bs, off);
			Pack.UInt32_To_BE((uint)len, bs, off + 4);
		}

		// Token: 0x0400000B RID: 11
		private const int BlockSize = 16;

		// Token: 0x0400000C RID: 12
		private static readonly byte[] Zeroes = new byte[16];

		// Token: 0x0400000D RID: 13
		private readonly IBlockCipher cipher;

		// Token: 0x0400000E RID: 14
		private readonly IGcmMultiplier multiplier;

		// Token: 0x0400000F RID: 15
		private bool forEncryption;

		// Token: 0x04000010 RID: 16
		private int macSize;

		// Token: 0x04000011 RID: 17
		private byte[] nonce;

		// Token: 0x04000012 RID: 18
		private byte[] A;

		// Token: 0x04000013 RID: 19
		private KeyParameter keyParam;

		// Token: 0x04000014 RID: 20
		private byte[] H;

		// Token: 0x04000015 RID: 21
		private byte[] initS;

		// Token: 0x04000016 RID: 22
		private byte[] J0;

		// Token: 0x04000017 RID: 23
		private byte[] bufBlock;

		// Token: 0x04000018 RID: 24
		private byte[] macBlock;

		// Token: 0x04000019 RID: 25
		private byte[] S;

		// Token: 0x0400001A RID: 26
		private byte[] counter;

		// Token: 0x0400001B RID: 27
		private int bufOff;

		// Token: 0x0400001C RID: 28
		private ulong totalLength;
	}
}
