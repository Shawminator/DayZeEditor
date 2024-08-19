using System;
using System.IO;

namespace BIS.Core.Compression
{
  public static class LZSS
  {
    public static uint ReadLZSS(
      Stream input,
      out byte[] dst,
      uint expectedSize,
      bool useSignedChecksum)
    {
      char[] chArray = new char[4113];
      dst = new byte[(int) expectedSize];
      if (expectedSize <= 0U)
        return 0;
      long position = input.Position;
      uint num1 = expectedSize;
      int num2 = 0;
      int num3 = 0;
      for (int index = 0; index < 4078; ++index)
        chArray[index] = ' ';
      int index1 = 4078;
      int num4 = 0;
      while (num1 > 0U)
      {
        if (((num4 >>= 1) & 256) == 0)
          num4 = input.ReadByte() | 65280;
        if ((num4 & 1) != 0)
        {
          int num5 = input.ReadByte();
          if (useSignedChecksum)
            num3 += (int) (sbyte) num5;
          else
            num3 += (int) (byte) num5;
          dst[num2++] = (byte) num5;
          --num1;
          chArray[index1] = (char) num5;
          index1 = index1 + 1 & 4095;
        }
        else
        {
          int num6 = input.ReadByte();
          int num7 = input.ReadByte();
          int num8 = num6 | (num7 & 240) << 4;
          int num9 = (num7 & 15) + 2;
          int num10 = index1 - num8;
          int num11 = num9 + num10;
          if ((long) (num9 + 1) > (long) num1)
            throw new ArgumentException("LZSS overflow");
          for (; num10 <= num11; ++num10)
          {
            int num12 = (int) (byte) chArray[num10 & 4095];
            if (useSignedChecksum)
              num3 += (int) (sbyte) num12;
            else
              num3 += (int) (byte) num12;
            dst[num2++] = (byte) num12;
            --num1;
            chArray[index1] = (char) num12;
            index1 = index1 + 1 & 4095;
          }
        }
      }
      byte[] buffer = new byte[4];
      input.Read(buffer, 0, 4);
      if (BitConverter.ToInt32(buffer, 0) != num3)
        throw new ArgumentException("Checksum mismatch");
      return (uint) (input.Position - position);
    }
  }
}
