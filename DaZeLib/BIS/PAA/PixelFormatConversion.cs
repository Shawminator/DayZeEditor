using BIS.Core;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace BIS.PAA
{
  public static class PixelFormatConversion
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetColor(byte[] img, int offset, byte a, byte r, byte g, byte b)
    {
      img[offset] = b;
      img[offset + 1] = g;
      img[offset + 2] = r;
      img[offset + 3] = a;
    }

    public static byte[] ARGB16ToARGB32(byte[] src)
    {
      byte[] img = new byte[src.Length * 2];
      int num1 = src.Length / 2;
      for (int index = 0; index < num1; ++index)
      {
        byte num2 = src[index * 2 + 1];
        byte num3 = src[index * 2];
        byte num4 = (byte) ((uint) num2 & 15U);
        byte num5 = (byte) (((int) num2 & 240) >> 4);
        byte num6 = (byte) ((uint) num3 & 15U);
        byte num7 = (byte) (((int) num3 & 240) >> 4);
        byte b = (byte) ((int) num4 * (int) byte.MaxValue / 15);
        byte a = (byte) ((int) num5 * (int) byte.MaxValue / 15);
        byte r = (byte) ((int) num6 * (int) byte.MaxValue / 15);
        byte g = (byte) ((int) num7 * (int) byte.MaxValue / 15);
        PixelFormatConversion.SetColor(img, index * 4, a, r, g, b);
      }
      return img;
    }

    public static byte[] ARGB1555ToARGB32(byte[] src)
    {
      byte[] img = new byte[src.Length * 2];
      int num1 = src.Length / 2;
      for (int index = 0; index < num1; ++index)
      {
        ushort uint16 = BitConverter.ToUInt16(src, index * 2);
        bool flag = ((int) uint16 & 32768) >> 15 == 1;
        byte num2 = (byte) (((int) uint16 & 31744) >> 10);
        byte num3 = (byte) (((int) uint16 & 992) >> 5);
        byte num4 = (byte) ((uint) uint16 & 31U);
        byte b = (byte) ((int) num2 * (int) byte.MaxValue / 31);
        byte maxValue = flag ? byte.MaxValue : (byte) 0;
        byte r = (byte) ((int) num4 * (int) byte.MaxValue / 31);
        byte g = (byte) ((int) num3 * (int) byte.MaxValue / 31);
        PixelFormatConversion.SetColor(img, index * 4, maxValue, r, g, b);
      }
      return img;
    }

    public static byte[] AI88ToARGB32(byte[] src)
    {
      byte[] img = new byte[src.Length * 2];
      int num1 = src.Length / 2;
      for (int index = 0; index < num1; ++index)
      {
        byte num2 = src[index * 2];
        byte a = src[index * 2 + 1];
        PixelFormatConversion.SetColor(img, index * 4, a, num2, num2, num2);
      }
      return img;
    }

    public static byte[] P8ToARGB32(byte[] src, Palette palette)
    {
      byte[] img = new byte[src.Length * 4];
      PackedColor[] colors = palette.Colors;
      int length = src.Length;
      for (int index = 0; index < length; ++index)
      {
        PackedColor packedColor = colors[(int) src[index]];
        PixelFormatConversion.SetColor(img, index * 4, packedColor.A8, packedColor.R8, packedColor.G8, packedColor.B8);
      }
      return img;
    }

    internal static byte[] DXTToARGB32(byte[] imageData, int width, int height, int dxtType)
    {
      using (MemoryStream imageStream = new MemoryStream(imageData))
        return PixelFormatConversion.DXTToARGB32((Stream) imageStream, width, height, dxtType);
    }

    internal static byte[] DXTToARGB32(Stream imageStream, int width, int height, int dxtType)
    {
      Action<BinaryReader, int, int, int, int, int, byte[]> action;
      switch (dxtType)
      {
        case 1:
          action = new Action<BinaryReader, int, int, int, int, int, byte[]>(PixelFormatConversion.DecompressDxt1Block);
          break;
        case 2:
        case 3:
          action = new Action<BinaryReader, int, int, int, int, int, byte[]>(PixelFormatConversion.DecompressDxt3Block);
          break;
        case 4:
        case 5:
          action = new Action<BinaryReader, int, int, int, int, int, byte[]>(PixelFormatConversion.DecompressDxt5Block);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (dxtType), (object) dxtType, "Value needs to be within interval 1-5");
      }
      byte[] argB32 = new byte[width * height * 4];
      using (BinaryReader binaryReader = new BinaryReader(imageStream))
      {
        int num1 = (width + 3) / 4;
        int num2 = (height + 3) / 4;
        for (int index1 = 0; index1 < num2; ++index1)
        {
          for (int index2 = 0; index2 < num1; ++index2)
            action(binaryReader, index2, index1, num1, width, height, argB32);
        }
      }
      return argB32;
    }

    private static void DecompressDxt1Block(
      BinaryReader imageReader,
      int x,
      int y,
      int blockCountX,
      int width,
      int height,
      byte[] imageData)
    {
      ushort color1 = imageReader.ReadUInt16();
      ushort color2 = imageReader.ReadUInt16();
      byte r1;
      byte g1;
      byte b1;
      PixelFormatConversion.ConvertRgb565ToRgb888(color1, out r1, out g1, out b1);
      byte r2;
      byte g2;
      byte b2;
      PixelFormatConversion.ConvertRgb565ToRgb888(color2, out r2, out g2, out b2);
      uint num1 = imageReader.ReadUInt32();
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          byte r3 = 0;
          byte g3 = 0;
          byte b3 = 0;
          byte a = byte.MaxValue;
          uint num2 = num1 >> 2 * (4 * index1 + index2) & 3U;
          if ((int) color1 > (int) color2)
          {
            switch (num2)
            {
              case 0:
                r3 = r1;
                g3 = g1;
                b3 = b1;
                break;
              case 1:
                r3 = r2;
                g3 = g2;
                b3 = b2;
                break;
              case 2:
                r3 = (byte) ((2 * (int) r1 + (int) r2) / 3);
                g3 = (byte) ((2 * (int) g1 + (int) g2) / 3);
                b3 = (byte) ((2 * (int) b1 + (int) b2) / 3);
                break;
              case 3:
                r3 = (byte) (((int) r1 + 2 * (int) r2) / 3);
                g3 = (byte) (((int) g1 + 2 * (int) g2) / 3);
                b3 = (byte) (((int) b1 + 2 * (int) b2) / 3);
                break;
            }
          }
          else
          {
            switch (num2)
            {
              case 0:
                r3 = r1;
                g3 = g1;
                b3 = b1;
                break;
              case 1:
                r3 = r2;
                g3 = g2;
                b3 = b2;
                break;
              case 2:
                r3 = (byte) (((int) r1 + (int) r2) / 2);
                g3 = (byte) (((int) g1 + (int) g2) / 2);
                b3 = (byte) (((int) b1 + (int) b2) / 2);
                break;
              case 3:
                r3 = (byte) 0;
                g3 = (byte) 0;
                b3 = (byte) 0;
                a = (byte) 0;
                break;
            }
          }
          int num3 = (x << 2) + index2;
          int num4 = (y << 2) + index1;
          if (num3 < width && num4 < height)
          {
            int offset = num4 * width + num3 << 2;
            PixelFormatConversion.SetColor(imageData, offset, a, r3, g3, b3);
          }
        }
      }
    }

    private static void DecompressDxt3Block(
      BinaryReader imageReader,
      int x,
      int y,
      int blockCountX,
      int width,
      int height,
      byte[] imageData)
    {
      byte num1 = imageReader.ReadByte();
      byte num2 = imageReader.ReadByte();
      byte num3 = imageReader.ReadByte();
      byte num4 = imageReader.ReadByte();
      byte num5 = imageReader.ReadByte();
      byte num6 = imageReader.ReadByte();
      byte num7 = imageReader.ReadByte();
      byte num8 = imageReader.ReadByte();
      ushort color1 = imageReader.ReadUInt16();
      ushort color2 = imageReader.ReadUInt16();
      byte r1;
      byte g1;
      byte b1;
      PixelFormatConversion.ConvertRgb565ToRgb888(color1, out r1, out g1, out b1);
      byte r2;
      byte g2;
      byte b2;
      PixelFormatConversion.ConvertRgb565ToRgb888(color2, out r2, out g2, out b2);
      uint num9 = imageReader.ReadUInt32();
      int num10 = 0;
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          byte r3 = 0;
          byte g3 = 0;
          byte b3 = 0;
          byte a = 0;
          uint num11 = num9 >> 2 * (4 * index1 + index2) & 3U;
          switch (num10)
          {
            case 0:
              a = (byte) ((int) num1 & 15 | ((int) num1 & 15) << 4);
              break;
            case 1:
              a = (byte) ((int) num1 & 240 | ((int) num1 & 240) >> 4);
              break;
            case 2:
              a = (byte) ((int) num2 & 15 | ((int) num2 & 15) << 4);
              break;
            case 3:
              a = (byte) ((int) num2 & 240 | ((int) num2 & 240) >> 4);
              break;
            case 4:
              a = (byte) ((int) num3 & 15 | ((int) num3 & 15) << 4);
              break;
            case 5:
              a = (byte) ((int) num3 & 240 | ((int) num3 & 240) >> 4);
              break;
            case 6:
              a = (byte) ((int) num4 & 15 | ((int) num4 & 15) << 4);
              break;
            case 7:
              a = (byte) ((int) num4 & 240 | ((int) num4 & 240) >> 4);
              break;
            case 8:
              a = (byte) ((int) num5 & 15 | ((int) num5 & 15) << 4);
              break;
            case 9:
              a = (byte) ((int) num5 & 240 | ((int) num5 & 240) >> 4);
              break;
            case 10:
              a = (byte) ((int) num6 & 15 | ((int) num6 & 15) << 4);
              break;
            case 11:
              a = (byte) ((int) num6 & 240 | ((int) num6 & 240) >> 4);
              break;
            case 12:
              a = (byte) ((int) num7 & 15 | ((int) num7 & 15) << 4);
              break;
            case 13:
              a = (byte) ((int) num7 & 240 | ((int) num7 & 240) >> 4);
              break;
            case 14:
              a = (byte) ((int) num8 & 15 | ((int) num8 & 15) << 4);
              break;
            case 15:
              a = (byte) ((int) num8 & 240 | ((int) num8 & 240) >> 4);
              break;
          }
          ++num10;
          switch (num11)
          {
            case 0:
              r3 = r1;
              g3 = g1;
              b3 = b1;
              break;
            case 1:
              r3 = r2;
              g3 = g2;
              b3 = b2;
              break;
            case 2:
              r3 = (byte) ((2 * (int) r1 + (int) r2) / 3);
              g3 = (byte) ((2 * (int) g1 + (int) g2) / 3);
              b3 = (byte) ((2 * (int) b1 + (int) b2) / 3);
              break;
            case 3:
              r3 = (byte) (((int) r1 + 2 * (int) r2) / 3);
              g3 = (byte) (((int) g1 + 2 * (int) g2) / 3);
              b3 = (byte) (((int) b1 + 2 * (int) b2) / 3);
              break;
          }
          int num12 = (x << 2) + index2;
          int num13 = (y << 2) + index1;
          if (num12 < width && num13 < height)
          {
            int offset = num13 * width + num12 << 2;
            PixelFormatConversion.SetColor(imageData, offset, a, r3, g3, b3);
          }
        }
      }
    }

    private static void DecompressDxt5Block(
      BinaryReader imageReader,
      int x,
      int y,
      int blockCountX,
      int width,
      int height,
      byte[] imageData)
    {
      byte num1 = imageReader.ReadByte();
      byte num2 = imageReader.ReadByte();
      ulong num3 = (ulong) imageReader.ReadByte() + ((ulong) imageReader.ReadByte() << 8) + ((ulong) imageReader.ReadByte() << 16) + ((ulong) imageReader.ReadByte() << 24) + ((ulong) imageReader.ReadByte() << 32) + ((ulong) imageReader.ReadByte() << 40);
      ushort color1 = imageReader.ReadUInt16();
      ushort color2 = imageReader.ReadUInt16();
      byte r1;
      byte g1;
      byte b1;
      PixelFormatConversion.ConvertRgb565ToRgb888(color1, out r1, out g1, out b1);
      byte r2;
      byte g2;
      byte b2;
      PixelFormatConversion.ConvertRgb565ToRgb888(color2, out r2, out g2, out b2);
      uint num4 = imageReader.ReadUInt32();
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          byte r3 = 0;
          byte g3 = 0;
          byte b3 = 0;
          uint num5 = num4 >> 2 * (4 * index1 + index2) & 3U;
          uint num6 = (uint) (num3 >> 3 * (4 * index1 + index2) & 7UL);
          byte a;
          switch (num6)
          {
            case 0:
              a = num1;
              break;
            case 1:
              a = num2;
              break;
            default:
              if ((int) num1 > (int) num2)
              {
                a = (byte) ((uint) ((8 - (int) num6) * (int) num1 + ((int) num6 - 1) * (int) num2) / 7U);
                break;
              }
              switch (num6)
              {
                case 6:
                  a = (byte) 0;
                  break;
                case 7:
                  a = byte.MaxValue;
                  break;
                default:
                  a = (byte) ((uint) ((6 - (int) num6) * (int) num1 + ((int) num6 - 1) * (int) num2) / 5U);
                  break;
              }
              break;
          }
          switch (num5)
          {
            case 0:
              r3 = r1;
              g3 = g1;
              b3 = b1;
              break;
            case 1:
              r3 = r2;
              g3 = g2;
              b3 = b2;
              break;
            case 2:
              r3 = (byte) ((2 * (int) r1 + (int) r2) / 3);
              g3 = (byte) ((2 * (int) g1 + (int) g2) / 3);
              b3 = (byte) ((2 * (int) b1 + (int) b2) / 3);
              break;
            case 3:
              r3 = (byte) (((int) r1 + 2 * (int) r2) / 3);
              g3 = (byte) (((int) g1 + 2 * (int) g2) / 3);
              b3 = (byte) (((int) b1 + 2 * (int) b2) / 3);
              break;
          }
          int num7 = (x << 2) + index2;
          int num8 = (y << 2) + index1;
          if (num7 < width && num8 < height)
          {
            int offset = num8 * width + num7 << 2;
            PixelFormatConversion.SetColor(imageData, offset, a, r3, g3, b3);
          }
        }
      }
    }

    private static void ConvertRgb565ToRgb888(ushort color, out byte r, out byte g, out byte b)
    {
      int num1 = ((int) color >> 11) * (int) byte.MaxValue + 16;
      r = (byte) ((num1 / 32 + num1) / 32);
      int num2 = (((int) color & 2016) >> 5) * (int) byte.MaxValue + 32;
      g = (byte) ((num2 / 64 + num2) / 64);
      int num3 = ((int) color & 31) * (int) byte.MaxValue + 16;
      b = (byte) ((num3 / 32 + num3) / 32);
    }
  }
}
