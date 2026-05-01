using System;
using System.IO;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000003 RID: 3
	internal class Base64Encoder
	{
		// Token: 0x06000004 RID: 4 RVA: 0x0000207F File Offset: 0x0000027F
		public Base64Encoder(TextWriter writer)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = writer;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020A8 File Offset: 0x000002A8
		public void Encode(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this._leftOverBytesCount > 0)
			{
				int leftOverBytesCount = this._leftOverBytesCount;
				while (leftOverBytesCount < 3 && count > 0)
				{
					this._leftOverBytes[leftOverBytesCount++] = buffer[index++];
					count--;
				}
				if (count == 0 && leftOverBytesCount < 3)
				{
					this._leftOverBytesCount = leftOverBytesCount;
					return;
				}
				int count2 = Convert.ToBase64CharArray(this._leftOverBytes, 0, 3, this._charsLine, 0);
				this.WriteChars(this._charsLine, 0, count2);
			}
			this._leftOverBytesCount = count % 3;
			if (this._leftOverBytesCount > 0)
			{
				count -= this._leftOverBytesCount;
				if (this._leftOverBytes == null)
				{
					this._leftOverBytes = new byte[3];
				}
				for (int i = 0; i < this._leftOverBytesCount; i++)
				{
					this._leftOverBytes[i] = buffer[index + count + i];
				}
			}
			int num = index + count;
			int num2 = 57;
			while (index < num)
			{
				if (index + num2 > num)
				{
					num2 = num - index;
				}
				int count3 = Convert.ToBase64CharArray(buffer, index, num2, this._charsLine, 0);
				this.WriteChars(this._charsLine, 0, count3);
				index += num2;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021EC File Offset: 0x000003EC
		public void Flush()
		{
			if (this._leftOverBytesCount > 0)
			{
				int count = Convert.ToBase64CharArray(this._leftOverBytes, 0, this._leftOverBytesCount, this._charsLine, 0);
				this.WriteChars(this._charsLine, 0, count);
				this._leftOverBytesCount = 0;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002231 File Offset: 0x00000431
		private void WriteChars(char[] chars, int index, int count)
		{
			this._writer.Write(chars, index, count);
		}

		// Token: 0x04000002 RID: 2
		private const int Base64LineSize = 76;

		// Token: 0x04000003 RID: 3
		private const int LineSizeInBytes = 57;

		// Token: 0x04000004 RID: 4
		private readonly char[] _charsLine = new char[76];

		// Token: 0x04000005 RID: 5
		private readonly TextWriter _writer;

		// Token: 0x04000006 RID: 6
		private byte[] _leftOverBytes;

		// Token: 0x04000007 RID: 7
		private int _leftOverBytesCount;
	}
}
