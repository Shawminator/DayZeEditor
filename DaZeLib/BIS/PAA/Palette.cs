using BIS.Core;
using BIS.Core.Streams;
using System.Diagnostics;

namespace BIS.PAA
{
  public class Palette
  {
    public const int PicFlagAlpha = 1;
    public const int PicFlagTransparent = 2;

    public PackedColor[] Colors { get; private set; }

    public PackedColor AverageColor { get; private set; }

    public PackedColor MaxColor { get; private set; }

    public bool IsAlpha { get; private set; }

    public bool IsTransparent { get; private set; }

    public Palette(PAAType format)
    {
      this.MaxColor = new PackedColor(uint.MaxValue);
      switch (format)
      {
        case PAAType.RGBA_4444:
        case PAAType.AI88:
        case PAAType.RGBA_8888:
          this.AverageColor = new PackedColor(2160074784U);
          break;
        default:
          this.AverageColor = new PackedColor(4286586912U);
          break;
      }
    }

    public void Read(BinaryReaderEx input, int[] startOffsets)
    {
      while (input.ReadAscii(4) == "GGAT")
      {
        string str = input.ReadAscii(4);
        int num1 = input.ReadInt32();
        switch (str)
        {
          case "CXAM":
            Debug.Assert(num1 == 4);
            this.MaxColor = new PackedColor(input.ReadUInt32());
            break;
          case "CGVA":
            Debug.Assert(num1 == 4);
            this.AverageColor = new PackedColor(input.ReadUInt32());
            break;
          case "GALF":
            Debug.Assert(num1 == 4);
            int num2 = input.ReadInt32();
            if ((num2 & 1) != 0)
              this.IsAlpha = true;
            if ((num2 & 2) != 0)
            {
              this.IsTransparent = true;
              break;
            }
            break;
          case "SFFO":
            int num3 = num1 / 4;
            for (int index = 0; index < num3; ++index)
              startOffsets[index] = input.ReadInt32();
            break;
          case "ZIWS":
            input.Position += (long) num1;
            break;
          default:
            Debug.Fail("What is that unknown PAA tagg?");
            input.Position += (long) num1;
            break;
        }
      }
      input.Position -= 4L;
      ushort length = input.ReadUInt16();
      this.Colors = new PackedColor[(int) length];
      for (int index = 0; index < (int) length; ++index)
      {
        byte b = input.ReadByte();
        byte g = input.ReadByte();
        byte r = input.ReadByte();
        this.Colors[index] = new PackedColor(r, g, b);
      }
    }
  }
}
