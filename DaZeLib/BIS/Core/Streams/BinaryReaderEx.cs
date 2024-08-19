using BIS.Core.Compression;
using System;
using System.Diagnostics;
using System.IO;

namespace BIS.Core.Streams
{
  public class BinaryReaderEx : BinaryReader
  {
    public bool UseCompressionFlag { get; set; }

    public bool UseLZOCompression { get; set; }

    public int Version { get; set; }

    public long Position
    {
      get => this.BaseStream.Position;
      set => this.BaseStream.Position = value;
    }

    public BinaryReaderEx(Stream stream)
      : base(stream)
    {
      this.UseCompressionFlag = false;
    }

    public uint ReadUInt24() => (uint) ((int) this.ReadByte() + ((int) this.ReadByte() << 8) + ((int) this.ReadByte() << 16));

    public string ReadAscii(int count)
    {
      string str = "";
      for (int index = 0; index < count; ++index)
        str += ((char) this.ReadByte()).ToString();
      return str;
    }

    public string ReadAscii() => this.ReadAscii((int) this.ReadUInt16());

    public string ReadAsciiz()
    {
      string str = "";
      char ch;
      while ((ch = (char) this.ReadByte()) > char.MinValue)
        str += ch.ToString();
      return str;
    }

    public T[] ReadArrayBase<T>(Func<BinaryReaderEx, T> readElement, int size)
    {
      T[] objArray = new T[size];
      for (int index = 0; index < size; ++index)
        objArray[index] = readElement(this);
      return objArray;
    }

    public T[] ReadArray<T>(Func<BinaryReaderEx, T> readElement) => this.ReadArrayBase<T>(readElement, this.ReadInt32());

    public float[] ReadFloatArray() => this.ReadArray<float>((Func<BinaryReaderEx, float>) (i => i.ReadSingle()));

    public int[] ReadIntArray() => this.ReadArray<int>((Func<BinaryReaderEx, int>) (i => i.ReadInt32()));

    public string[] ReadStringArray() => this.ReadArray<string>((Func<BinaryReaderEx, string>) (i => i.ReadAsciiz()));

    public T[] ReadCompressedArray<T>(Func<BinaryReaderEx, T> readElement, int elemSize)
    {
      int size = this.ReadInt32();
      return new BinaryReaderEx((Stream) new MemoryStream(this.ReadCompressed((uint) (size * elemSize)))).ReadArrayBase<T>(readElement, size);
    }

    public short[] ReadCompressedShortArray() => this.ReadCompressedArray<short>((Func<BinaryReaderEx, short>) (i => i.ReadInt16()), 2);

    public int[] ReadCompressedIntArray() => this.ReadCompressedArray<int>((Func<BinaryReaderEx, int>) (i => i.ReadInt32()), 4);

    public float[] ReadCompressedFloatArray() => this.ReadCompressedArray<float>((Func<BinaryReaderEx, float>) (i => i.ReadSingle()), 4);

    public T[] ReadCondensedArray<T>(Func<BinaryReaderEx, T> readElement, int sizeOfT)
    {
      int size = this.ReadInt32();
      T[] objArray = new T[size];
      if (this.ReadBoolean())
      {
        T obj = readElement(this);
        for (int index = 0; index < size; ++index)
          objArray[index] = obj;
        return objArray;
      }
      using (BinaryReaderEx binaryReaderEx = new BinaryReaderEx((Stream) new MemoryStream(this.ReadCompressed((uint) (size * sizeOfT)))))
        objArray = binaryReaderEx.ReadArrayBase<T>(readElement, size);
      return objArray;
    }

    public int[] ReadCondensedIntArray() => this.ReadCondensedArray<int>((Func<BinaryReaderEx, int>) (i => i.ReadInt32()), 4);

    public int ReadCompactInteger()
    {
      int num1 = (int) this.ReadByte();
      if ((num1 & 128) != 0)
      {
        int num2 = (int) this.ReadByte();
        num1 += (num2 - 1) * 128;
      }
      return num1;
    }

    public byte[] ReadCompressed(uint expectedSize)
    {
      if (expectedSize == 0U)
        return new byte[0];
      return this.UseLZOCompression ? this.ReadLZO(expectedSize) : this.ReadLZSS(expectedSize);
    }

    public byte[] ReadLZO(uint expectedSize)
    {
      bool flag = expectedSize >= 1024U;
      if (this.UseCompressionFlag)
        flag = this.ReadBoolean();
      return !flag ? this.ReadBytes((int) expectedSize) : LZO.ReadLZO(this.BaseStream, expectedSize);
    }

    public byte[] ReadLZSS(uint expectedSize, bool inPAA = false)
    {
      if (expectedSize < 1024U && !inPAA)
        return this.ReadBytes((int) expectedSize);
      byte[] dst = new byte[(int) expectedSize];
      int num = (int) LZSS.ReadLZSS(this.BaseStream, out dst, expectedSize, inPAA);
      return dst;
    }

    public byte[] ReadCompressedIndices(int bytesToRead, uint expectedSize)
    {
      byte[] numArray = new byte[(int) expectedSize];
      int num1 = 0;
      for (int index1 = 0; index1 < bytesToRead; ++index1)
      {
        byte num2 = this.ReadByte();
        if (((uint) num2 & 128U) > 0U)
        {
          byte num3 = (byte) ((uint) num2 - (uint) sbyte.MaxValue);
          byte num4 = this.ReadByte();
          for (int index2 = 0; index2 < (int) num3; ++index2)
            numArray[num1++] = num4;
        }
        else
        {
          for (int index3 = 0; index3 < (int) num2 + 1; ++index3)
            numArray[num1++] = this.ReadByte();
        }
      }
      Debug.Assert((long) num1 == (long) expectedSize);
      return numArray;
    }
  }
}
