using System.IO;

namespace BIS.Core.Streams
{
  public class BinaryWriterEx : BinaryWriter
  {
    public long Position
    {
      get => this.BaseStream.Position;
      set => this.BaseStream.Position = value;
    }

    public BinaryWriterEx(Stream dstStream)
      : base(dstStream)
    {
    }

    public void WriteAscii(string text, uint len)
    {
      this.Write(text.ToCharArray());
      uint num = (uint) ((ulong) len - (ulong) text.Length);
      for (int index = 0; (long) index < (long) num; ++index)
        this.Write(char.MinValue);
    }

    public void WriteAsciiz(string text)
    {
      this.Write(text.ToCharArray());
      this.Write(char.MinValue);
    }
  }
}
