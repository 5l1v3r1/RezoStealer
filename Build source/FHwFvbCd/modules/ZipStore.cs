using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Stealer.modules
{
	// Token: 0x0200003D RID: 61
	public class ZipStore
	{
		// Token: 0x06000144 RID: 324 RVA: 0x0000E108 File Offset: 0x0000C308
		public static void PackedZip(string pathFolder, string zipName)
		{
			ZipStore.ZipStorer zipStorer = ZipStore.ZipStorer.Create(zipName + ".zip", "");
			zipStorer.EncodeUTF8 = true;
			foreach (string text in Directory.GetFiles(pathFolder, "*.*", SearchOption.AllDirectories))
			{
				string filenameInZip = text.Replace(pathFolder, "");
				zipStorer.AddFile(ZipStore.ZipStorer.Compression.Deflate, text, filenameInZip, "");
			}
			zipStorer.Close();
		}

		// Token: 0x0200003E RID: 62
		private class ZipStorer : IDisposable
		{
			// Token: 0x06000146 RID: 326 RVA: 0x0000E184 File Offset: 0x0000C384
			static ZipStorer()
			{
				for (int i = 0; i < ZipStore.ZipStorer.CrcTable.Length; i++)
				{
					uint num = (uint)i;
					for (int j = 0; j < 8; j++)
					{
						if ((num & 1U) > 0U)
						{
							num = (3988292384U ^ num >> 1);
						}
						else
						{
							num >>= 1;
						}
					}
					ZipStore.ZipStorer.CrcTable[i] = num;
				}
			}

			// Token: 0x06000147 RID: 327 RVA: 0x0000E1F0 File Offset: 0x0000C3F0
			public void AddFile(ZipStore.ZipStorer.Compression _method, string _pathname, string _filenameInZip, string _comment)
			{
				if (this.Access == FileAccess.Read)
				{
					throw new InvalidOperationException("Writing is not alowed");
				}
				FileStream fileStream = new FileStream(_pathname, FileMode.Open, FileAccess.Read);
				this.AddStream(_method, _filenameInZip, fileStream, File.GetLastWriteTime(_pathname), _comment);
				fileStream.Close();
			}

			// Token: 0x06000148 RID: 328 RVA: 0x0000E234 File Offset: 0x0000C434
			public void AddStream(ZipStore.ZipStorer.Compression _method, string _filenameInZip, Stream _source, DateTime _modTime, string _comment)
			{
				if (this.Access == FileAccess.Read)
				{
					throw new InvalidOperationException("Writing is not alowed");
				}
				if (this.Files.Count != 0)
				{
					ZipStore.ZipStorer.ZipFileEntry zipFileEntry = this.Files[this.Files.Count - 1];
				}
				ZipStore.ZipStorer.ZipFileEntry item = new ZipStore.ZipStorer.ZipFileEntry
				{
					Method = _method,
					EncodeUTF8 = this.EncodeUTF8,
					FilenameInZip = this.NormalizedFilename(_filenameInZip),
					Comment = ((_comment == null) ? "" : _comment),
					Crc32 = 0U,
					HeaderOffset = (uint)this.ZipFileStream.Position,
					ModifyTime = _modTime
				};
				this.WriteLocalHeader(ref item);
				item.FileOffset = (uint)this.ZipFileStream.Position;
				this.Store(ref item, _source);
				_source.Close();
				this.UpdateCrcAndSizes(ref item);
				this.Files.Add(item);
			}

			// Token: 0x06000149 RID: 329 RVA: 0x0000E320 File Offset: 0x0000C520
			public void Close()
			{
				if (this.Access != FileAccess.Read)
				{
					uint offset = (uint)this.ZipFileStream.Position;
					uint num = 0U;
					if (this.CentralDirImage != null)
					{
						this.ZipFileStream.Write(this.CentralDirImage, 0, this.CentralDirImage.Length);
					}
					for (int i = 0; i < this.Files.Count; i++)
					{
						long position = this.ZipFileStream.Position;
						this.WriteCentralDirRecord(this.Files[i]);
						num += (uint)(this.ZipFileStream.Position - position);
					}
					if (this.CentralDirImage != null)
					{
						this.WriteEndRecord(num + (uint)this.CentralDirImage.Length, offset);
					}
					else
					{
						this.WriteEndRecord(num, offset);
					}
				}
				if (this.ZipFileStream == null)
				{
					return;
				}
				this.ZipFileStream.Flush();
				this.ZipFileStream.Dispose();
				this.ZipFileStream = null;
			}

			// Token: 0x0600014A RID: 330 RVA: 0x0000E3F8 File Offset: 0x0000C5F8
			public static ZipStore.ZipStorer Create(Stream _stream, string _comment)
			{
				return new ZipStore.ZipStorer
				{
					Comment = _comment,
					ZipFileStream = _stream,
					Access = FileAccess.Write
				};
			}

			// Token: 0x0600014B RID: 331 RVA: 0x0000E424 File Offset: 0x0000C624
			public static ZipStore.ZipStorer Create(string _filename, string _comment)
			{
				ZipStore.ZipStorer zipStorer = ZipStore.ZipStorer.Create(new FileStream(_filename, FileMode.Create, FileAccess.ReadWrite), _comment);
				zipStorer.Comment = _comment;
				zipStorer.FileName = _filename;
				return zipStorer;
			}

			// Token: 0x0600014C RID: 332 RVA: 0x0000E450 File Offset: 0x0000C650
			private uint DateTimeToDosTime(DateTime _dt)
			{
				return (uint)(_dt.Second / 2 | _dt.Minute << 5 | _dt.Hour << 11 | _dt.Day << 16 | _dt.Month << 21 | _dt.Year - 1980 << 25);
			}

			// Token: 0x0600014D RID: 333 RVA: 0x0000E4A2 File Offset: 0x0000C6A2
			public void Dispose()
			{
				this.Close();
			}

			// Token: 0x0600014E RID: 334 RVA: 0x0000E4AA File Offset: 0x0000C6AA
			private DateTime DosTimeToDateTime(uint _dt)
			{
				return new DateTime((int)((_dt >> 25) + 1980U), (int)(_dt >> 21 & 15U), (int)(_dt >> 16 & 31U), (int)(_dt >> 11 & 31U), (int)(_dt >> 5 & 63U), (int)((_dt & 31U) * 2U));
			}

			// Token: 0x0600014F RID: 335 RVA: 0x0000E4DC File Offset: 0x0000C6DC
			public bool ExtractFile(ZipStore.ZipStorer.ZipFileEntry _zfe, Stream _stream)
			{
				if (!_stream.CanWrite)
				{
					throw new InvalidOperationException("Stream cannot be written");
				}
				byte[] array = new byte[4];
				this.ZipFileStream.Seek((long)((ulong)_zfe.HeaderOffset), SeekOrigin.Begin);
				this.ZipFileStream.Read(array, 0, 4);
				if (BitConverter.ToUInt32(array, 0) == 67324752U)
				{
					Stream stream;
					if (_zfe.Method == ZipStore.ZipStorer.Compression.Store)
					{
						stream = this.ZipFileStream;
					}
					else
					{
						if (_zfe.Method != ZipStore.ZipStorer.Compression.Deflate)
						{
							return false;
						}
						stream = new DeflateStream(this.ZipFileStream, CompressionMode.Decompress, true);
					}
					byte[] array2 = new byte[16384];
					this.ZipFileStream.Seek((long)((ulong)_zfe.FileOffset), SeekOrigin.Begin);
					int num2;
					for (uint num = _zfe.FileSize; num > 0U; num -= (uint)num2)
					{
						num2 = stream.Read(array2, 0, (int)Math.Min((long)((ulong)num), (long)array2.Length));
						_stream.Write(array2, 0, num2);
					}
					_stream.Flush();
					if (_zfe.Method == ZipStore.ZipStorer.Compression.Deflate)
					{
						stream.Dispose();
					}
					return true;
				}
				return false;
			}

			// Token: 0x06000150 RID: 336 RVA: 0x0000E5D0 File Offset: 0x0000C7D0
			public bool ExtractFile(ZipStore.ZipStorer.ZipFileEntry _zfe, string _filename)
			{
				string directoryName = Path.GetDirectoryName(_filename);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				bool result;
				if (Directory.Exists(_filename))
				{
					result = true;
				}
				else
				{
					Stream stream = new FileStream(_filename, FileMode.Create, FileAccess.Write);
					bool flag = this.ExtractFile(_zfe, stream);
					if (flag)
					{
						stream.Close();
					}
					File.SetCreationTime(_filename, _zfe.ModifyTime);
					File.SetLastWriteTime(_filename, _zfe.ModifyTime);
					result = flag;
				}
				return result;
			}

			// Token: 0x06000151 RID: 337 RVA: 0x0000E638 File Offset: 0x0000C838
			private uint GetFileOffset(uint _headerOffset)
			{
				byte[] array = new byte[2];
				this.ZipFileStream.Seek((long)((ulong)(_headerOffset + 26U)), SeekOrigin.Begin);
				this.ZipFileStream.Read(array, 0, 2);
				ushort num = BitConverter.ToUInt16(array, 0);
				this.ZipFileStream.Read(array, 0, 2);
				ushort num2 = BitConverter.ToUInt16(array, 0);
				return (uint)(30 + num + num2) + _headerOffset;
			}

			// Token: 0x06000152 RID: 338 RVA: 0x0000E694 File Offset: 0x0000C894
			private string NormalizedFilename(string _filename)
			{
				string text = _filename.Replace('\\', '/');
				int num = text.IndexOf(':');
				if (num >= 0)
				{
					text = text.Remove(0, num + 1);
				}
				return text.Trim(new char[]
				{
					'/'
				});
			}

			// Token: 0x06000153 RID: 339 RVA: 0x0000E6D8 File Offset: 0x0000C8D8
			public static ZipStore.ZipStorer Open(Stream _stream, FileAccess _access)
			{
				if (!_stream.CanSeek && _access != FileAccess.Read)
				{
					throw new InvalidOperationException("Stream cannot seek");
				}
				ZipStore.ZipStorer zipStorer = new ZipStore.ZipStorer
				{
					ZipFileStream = _stream,
					Access = _access
				};
				if (!zipStorer.ReadFileInfo())
				{
					throw new InvalidDataException();
				}
				return zipStorer;
			}

			// Token: 0x06000154 RID: 340 RVA: 0x0000E724 File Offset: 0x0000C924
			public static ZipStore.ZipStorer Open(string _filename, FileAccess _access)
			{
				ZipStore.ZipStorer zipStorer = ZipStore.ZipStorer.Open(new FileStream(_filename, FileMode.Open, (_access == FileAccess.Read) ? FileAccess.Read : FileAccess.ReadWrite), _access);
				zipStorer.FileName = _filename;
				return zipStorer;
			}

			// Token: 0x06000155 RID: 341 RVA: 0x0000E750 File Offset: 0x0000C950
			public List<ZipStore.ZipStorer.ZipFileEntry> ReadCentralDir()
			{
				if (this.CentralDirImage == null)
				{
					throw new InvalidOperationException("Central directory currently does not exist");
				}
				List<ZipStore.ZipStorer.ZipFileEntry> list = new List<ZipStore.ZipStorer.ZipFileEntry>();
				int num = 0;
				while (num < this.CentralDirImage.Length && BitConverter.ToUInt32(this.CentralDirImage, num) == 33639248U)
				{
					bool flag = (BitConverter.ToUInt16(this.CentralDirImage, num + 8) & 2048) > 0;
					ushort method = BitConverter.ToUInt16(this.CentralDirImage, num + 10);
					uint dt = BitConverter.ToUInt32(this.CentralDirImage, num + 12);
					uint crc = BitConverter.ToUInt32(this.CentralDirImage, num + 16);
					uint compressedSize = BitConverter.ToUInt32(this.CentralDirImage, num + 20);
					uint fileSize = BitConverter.ToUInt32(this.CentralDirImage, num + 24);
					ushort num2 = BitConverter.ToUInt16(this.CentralDirImage, num + 28);
					ushort num3 = BitConverter.ToUInt16(this.CentralDirImage, num + 30);
					ushort num4 = BitConverter.ToUInt16(this.CentralDirImage, num + 32);
					uint headerOffset = BitConverter.ToUInt32(this.CentralDirImage, num + 42);
					uint headerSize = (uint)(46 + num2 + num3 + num4);
					Encoding encoding = flag ? Encoding.UTF8 : ZipStore.ZipStorer.DefaultEncoding;
					ZipStore.ZipStorer.ZipFileEntry item = new ZipStore.ZipStorer.ZipFileEntry
					{
						Method = (ZipStore.ZipStorer.Compression)method,
						FilenameInZip = encoding.GetString(this.CentralDirImage, num + 46, (int)num2),
						FileOffset = this.GetFileOffset(headerOffset),
						FileSize = fileSize,
						CompressedSize = compressedSize,
						HeaderOffset = headerOffset,
						HeaderSize = headerSize,
						Crc32 = crc,
						ModifyTime = this.DosTimeToDateTime(dt)
					};
					if (num4 > 0)
					{
						item.Comment = encoding.GetString(this.CentralDirImage, num + 46 + (int)num2 + (int)num3, (int)num4);
					}
					list.Add(item);
					num += (int)(46 + num2 + num3 + num4);
				}
				return list;
			}

			// Token: 0x06000156 RID: 342 RVA: 0x0000E928 File Offset: 0x0000CB28
			private bool ReadFileInfo()
			{
				if (this.ZipFileStream.Length >= 22L)
				{
					try
					{
						this.ZipFileStream.Seek(-17L, SeekOrigin.End);
						BinaryReader binaryReader = new BinaryReader(this.ZipFileStream);
						do
						{
							this.ZipFileStream.Seek(-5L, SeekOrigin.Current);
							if (binaryReader.ReadUInt32() == 101010256U)
							{
								goto IL_60;
							}
						}
						while (this.ZipFileStream.Position > 0L);
						return false;
						IL_60:
						this.ZipFileStream.Seek(6L, SeekOrigin.Current);
						ushort existingFiles = binaryReader.ReadUInt16();
						int num = binaryReader.ReadInt32();
						uint num2 = binaryReader.ReadUInt32();
						if (this.ZipFileStream.Position + (long)((ulong)binaryReader.ReadUInt16()) != this.ZipFileStream.Length)
						{
							return false;
						}
						this.ExistingFiles = existingFiles;
						this.CentralDirImage = new byte[num];
						this.ZipFileStream.Seek((long)((ulong)num2), SeekOrigin.Begin);
						this.ZipFileStream.Read(this.CentralDirImage, 0, num);
						this.ZipFileStream.Seek((long)((ulong)num2), SeekOrigin.Begin);
						return true;
					}
					catch
					{
					}
					return false;
				}
				return false;
			}

			// Token: 0x06000157 RID: 343 RVA: 0x0000EA40 File Offset: 0x0000CC40
			public static bool RemoveEntries(ref ZipStore.ZipStorer _zip, List<ZipStore.ZipStorer.ZipFileEntry> _zfes)
			{
				if (!(_zip.ZipFileStream is FileStream))
				{
					throw new InvalidOperationException("RemoveEntries is allowed just over streams of type FileStream");
				}
				List<ZipStore.ZipStorer.ZipFileEntry> list = _zip.ReadCentralDir();
				string tempFileName = Path.GetTempFileName();
				string tempFileName2 = Path.GetTempFileName();
				try
				{
					ZipStore.ZipStorer zipStorer = ZipStore.ZipStorer.Create(tempFileName, string.Empty);
					foreach (ZipStore.ZipStorer.ZipFileEntry zipFileEntry in list)
					{
						if (!_zfes.Contains(zipFileEntry) && _zip.ExtractFile(zipFileEntry, tempFileName2))
						{
							zipStorer.AddFile(zipFileEntry.Method, tempFileName2, zipFileEntry.FilenameInZip, zipFileEntry.Comment);
						}
					}
					_zip.Close();
					zipStorer.Close();
					File.Delete(_zip.FileName);
					File.Move(tempFileName, _zip.FileName);
					_zip = ZipStore.ZipStorer.Open(_zip.FileName, _zip.Access);
				}
				catch
				{
					return false;
				}
				finally
				{
					if (File.Exists(tempFileName))
					{
						File.Delete(tempFileName);
					}
					if (File.Exists(tempFileName2))
					{
						File.Delete(tempFileName2);
					}
				}
				return true;
			}

			// Token: 0x06000158 RID: 344 RVA: 0x0000EB74 File Offset: 0x0000CD74
			private void Store(ref ZipStore.ZipStorer.ZipFileEntry _zfe, Stream _source)
			{
				byte[] array = new byte[16384];
				uint num = 0U;
				long position = this.ZipFileStream.Position;
				long position2 = _source.Position;
				Stream stream = (_zfe.Method != ZipStore.ZipStorer.Compression.Store) ? new DeflateStream(this.ZipFileStream, CompressionMode.Compress, true) : this.ZipFileStream;
				_zfe.Crc32 = uint.MaxValue;
				int num2;
				do
				{
					num2 = _source.Read(array, 0, array.Length);
					num += (uint)num2;
					if (num2 > 0)
					{
						stream.Write(array, 0, num2);
						uint num3 = 0U;
						while ((ulong)num3 < (ulong)((long)num2))
						{
							_zfe.Crc32 = (ZipStore.ZipStorer.CrcTable[(int)((IntPtr)((long)((ulong)((_zfe.Crc32 ^ (uint)array[(int)((uint)((UIntPtr)num3))]) & 255U))))] ^ _zfe.Crc32 >> 8);
							num3 += 1U;
						}
					}
				}
				while (num2 == array.Length);
				stream.Flush();
				if (_zfe.Method == ZipStore.ZipStorer.Compression.Deflate)
				{
					stream.Dispose();
				}
				_zfe.Crc32 ^= uint.MaxValue;
				_zfe.FileSize = num;
				_zfe.CompressedSize = (uint)(this.ZipFileStream.Position - position);
				if (_zfe.Method != ZipStore.ZipStorer.Compression.Deflate || this.ForceDeflating || !_source.CanSeek || _zfe.CompressedSize <= _zfe.FileSize)
				{
					return;
				}
				_zfe.Method = ZipStore.ZipStorer.Compression.Store;
				this.ZipFileStream.Position = position;
				this.ZipFileStream.SetLength(position);
				_source.Position = position2;
				this.Store(ref _zfe, _source);
			}

			// Token: 0x06000159 RID: 345 RVA: 0x0000ECD4 File Offset: 0x0000CED4
			private void UpdateCrcAndSizes(ref ZipStore.ZipStorer.ZipFileEntry _zfe)
			{
				long position = this.ZipFileStream.Position;
				this.ZipFileStream.Position = (long)((ulong)(_zfe.HeaderOffset + 8U));
				this.ZipFileStream.Write(BitConverter.GetBytes((ushort)_zfe.Method), 0, 2);
				this.ZipFileStream.Position = (long)((ulong)(_zfe.HeaderOffset + 14U));
				this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.Crc32), 0, 4);
				this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.CompressedSize), 0, 4);
				this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.FileSize), 0, 4);
				this.ZipFileStream.Position = position;
			}

			// Token: 0x0600015A RID: 346 RVA: 0x0000ED90 File Offset: 0x0000CF90
			private void WriteCentralDirRecord(ZipStore.ZipStorer.ZipFileEntry _zfe)
			{
				Encoding encoding = _zfe.EncodeUTF8 ? Encoding.UTF8 : ZipStore.ZipStorer.DefaultEncoding;
				byte[] bytes = encoding.GetBytes(_zfe.FilenameInZip);
				byte[] bytes2 = encoding.GetBytes(_zfe.Comment);
				this.ZipFileStream.Write(new byte[]
				{
					80,
					75,
					1,
					2,
					23,
					11,
					20,
					0
				}, 0, 8);
				this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.EncodeUTF8 ? 2048 : 0), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes((ushort)_zfe.Method), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes(this.DateTimeToDosTime(_zfe.ModifyTime)), 0, 4);
				this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.Crc32), 0, 4);
				this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.CompressedSize), 0, 4);
				this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.FileSize), 0, 4);
				this.ZipFileStream.Write(BitConverter.GetBytes((ushort)bytes.Length), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes(0), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes((ushort)bytes2.Length), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes(0), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes(0), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes(0), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes(33024), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.HeaderOffset), 0, 4);
				this.ZipFileStream.Write(bytes, 0, bytes.Length);
				this.ZipFileStream.Write(bytes2, 0, bytes2.Length);
			}

			// Token: 0x0600015B RID: 347 RVA: 0x0000EF68 File Offset: 0x0000D168
			private void WriteEndRecord(uint _size, uint _offset)
			{
				byte[] bytes = (this.EncodeUTF8 ? Encoding.UTF8 : ZipStore.ZipStorer.DefaultEncoding).GetBytes(this.Comment);
				this.ZipFileStream.Write(new byte[]
				{
					80,
					75,
					5,
					6,
					0,
					0,
					0,
					0
				}, 0, 8);
				this.ZipFileStream.Write(BitConverter.GetBytes((int)((ushort)this.Files.Count + this.ExistingFiles)), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes((int)((ushort)this.Files.Count + this.ExistingFiles)), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes(_size), 0, 4);
				this.ZipFileStream.Write(BitConverter.GetBytes(_offset), 0, 4);
				this.ZipFileStream.Write(BitConverter.GetBytes((ushort)bytes.Length), 0, 2);
				this.ZipFileStream.Write(bytes, 0, bytes.Length);
			}

			// Token: 0x0600015C RID: 348 RVA: 0x0000F058 File Offset: 0x0000D258
			private void WriteLocalHeader(ref ZipStore.ZipStorer.ZipFileEntry _zfe)
			{
				long position = this.ZipFileStream.Position;
				byte[] bytes = (_zfe.EncodeUTF8 ? Encoding.UTF8 : ZipStore.ZipStorer.DefaultEncoding).GetBytes(_zfe.FilenameInZip);
				this.ZipFileStream.Write(new byte[]
				{
					80,
					75,
					3,
					4,
					20,
					0
				}, 0, 6);
				this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.EncodeUTF8 ? 2048 : 0), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes((ushort)_zfe.Method), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes(this.DateTimeToDosTime(_zfe.ModifyTime)), 0, 4);
				this.ZipFileStream.Write(new byte[12], 0, 12);
				this.ZipFileStream.Write(BitConverter.GetBytes((ushort)bytes.Length), 0, 2);
				this.ZipFileStream.Write(BitConverter.GetBytes(0), 0, 2);
				this.ZipFileStream.Write(bytes, 0, bytes.Length);
				_zfe.HeaderSize = (uint)(this.ZipFileStream.Position - position);
			}

			// Token: 0x04000096 RID: 150
			private string Comment = "";

			// Token: 0x04000097 RID: 151
			private List<ZipStore.ZipStorer.ZipFileEntry> Files = new List<ZipStore.ZipStorer.ZipFileEntry>();

			// Token: 0x04000098 RID: 152
			private static uint[] CrcTable = new uint[256];

			// Token: 0x04000099 RID: 153
			private static Encoding DefaultEncoding = Encoding.GetEncoding(437);

			// Token: 0x0400009A RID: 154
			private FileAccess Access;

			// Token: 0x0400009B RID: 155
			private byte[] CentralDirImage;

			// Token: 0x0400009C RID: 156
			public bool EncodeUTF8;

			// Token: 0x0400009D RID: 157
			private ushort ExistingFiles;

			// Token: 0x0400009E RID: 158
			private string FileName;

			// Token: 0x0400009F RID: 159
			public bool ForceDeflating;

			// Token: 0x040000A0 RID: 160
			private Stream ZipFileStream;

			// Token: 0x0200003F RID: 63
			public enum Compression : ushort
			{
				// Token: 0x040000A2 RID: 162
				Store,
				// Token: 0x040000A3 RID: 163
				Deflate = 8
			}

			// Token: 0x02000040 RID: 64
			public struct ZipFileEntry
			{
				// Token: 0x0600015E RID: 350 RVA: 0x0000F187 File Offset: 0x0000D387
				public override string ToString()
				{
					return this.FilenameInZip;
				}

				// Token: 0x040000A4 RID: 164
				public ZipStore.ZipStorer.Compression Method;

				// Token: 0x040000A5 RID: 165
				public string FilenameInZip;

				// Token: 0x040000A6 RID: 166
				public uint FileSize;

				// Token: 0x040000A7 RID: 167
				public uint CompressedSize;

				// Token: 0x040000A8 RID: 168
				public uint HeaderOffset;

				// Token: 0x040000A9 RID: 169
				public uint FileOffset;

				// Token: 0x040000AA RID: 170
				public uint HeaderSize;

				// Token: 0x040000AB RID: 171
				public uint Crc32;

				// Token: 0x040000AC RID: 172
				public DateTime ModifyTime;

				// Token: 0x040000AD RID: 173
				public string Comment;

				// Token: 0x040000AE RID: 174
				public bool EncodeUTF8;
			}
		}
	}
}
