using BIS.Core.Streams;
using System;

namespace BIS.PAA
{
  public class Mipmap
  {
    private const ushort MAGIC_LZW_W = 1234;
    private const ushort MAGIC_LZW_H = 8765;
    private bool hasMagicLZW;

    public int Offset { get; }

    public int DataOffset { get; }

    public bool IsLZOCompressed { get; }

    public ushort Width { get; }

    public ushort Height { get; }

    public uint DataSize { get; }

    public Mipmap(BinaryReaderEx input, int offset)
    {
      this.Offset = offset;
      this.IsLZOCompressed = false;
      this.hasMagicLZW = false;
      this.Width = input.ReadUInt16();
      this.Height = input.ReadUInt16();
      if (this.Width == (ushort) 1234 && this.Height == (ushort) 8765)
      {
        this.hasMagicLZW = true;
        this.Width = input.ReadUInt16();
        this.Height = input.ReadUInt16();
      }
      if (((uint) this.Width & 32768U) > 0U)
      {
        this.Width = (ushort) ((uint) this.Width & (uint) short.MaxValue);
        this.IsLZOCompressed = true;
      }
      this.DataSize = input.ReadUInt24();
      this.DataOffset = (int) input.Position;
      input.Position += (long) this.DataSize;
    }

    public byte[] GetRawPixelData(BinaryReaderEx input, PAAType type)
    {
      input.Position = (long) this.DataOffset;
      uint expectedSize1 = (uint) this.Width * (uint) this.Height;
      byte[] numArray = new byte[(int) expectedSize1];
      switch (type)
      {
        case PAAType.DXT1:
          expectedSize1 /= 2U;
          goto case PAAType.DXT2;
        case PAAType.DXT2:
        case PAAType.DXT3:
        case PAAType.DXT4:
        case PAAType.DXT5:
          return !this.IsLZOCompressed ? input.ReadBytes((int) this.DataSize) : input.ReadLZO(expectedSize1);
        case PAAType.RGBA_5551:
        case PAAType.RGBA_4444:
        case PAAType.AI88:
          uint expectedSize2 = expectedSize1 * 2U;
          return input.ReadLZSS(expectedSize2, true);
        case PAAType.RGBA_8888:
          uint expectedSize3 = expectedSize1 * 4U;
          return input.ReadLZSS(expectedSize3, true);
        case PAAType.P8:
          return !this.hasMagicLZW ? input.ReadCompressedIndices((int) this.DataSize, expectedSize1) : input.ReadLZSS(expectedSize1, true);
        default:
          throw new ArgumentException("Unexpected PAA type", nameof (type));
      }
    }
  }
}
