using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200000A RID: 10
	internal class StringBuffer
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000040BF File Offset: 0x000022BF
		// (set) Token: 0x06000035 RID: 53 RVA: 0x000040C7 File Offset: 0x000022C7
		public int Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000040D0 File Offset: 0x000022D0
		public StringBuffer()
		{
			this._buffer = StringBuffer._emptyBuffer;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000040E3 File Offset: 0x000022E3
		public StringBuffer(int initalSize)
		{
			this._buffer = new char[initalSize];
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000040F8 File Offset: 0x000022F8
		public void Append(char value)
		{
			if (this._position == this._buffer.Length)
			{
				this.EnsureSize(1);
			}
			this._buffer[this._position++] = value;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004135 File Offset: 0x00002335
		public void Clear()
		{
			this._buffer = StringBuffer._emptyBuffer;
			this._position = 0;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000414C File Offset: 0x0000234C
		private void EnsureSize(int appendLength)
		{
			char[] array = new char[(this._position + appendLength) * 2];
			Array.Copy(this._buffer, array, this._position);
			this._buffer = array;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00004182 File Offset: 0x00002382
		public override string ToString()
		{
			return this.ToString(0, this._position);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004191 File Offset: 0x00002391
		public string ToString(int start, int length)
		{
			return new string(this._buffer, start, length);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000041A0 File Offset: 0x000023A0
		public char[] GetInternalBuffer()
		{
			return this._buffer;
		}

		// Token: 0x04000032 RID: 50
		private char[] _buffer;

		// Token: 0x04000033 RID: 51
		private int _position;

		// Token: 0x04000034 RID: 52
		private static readonly char[] _emptyBuffer = new char[0];
	}
}
