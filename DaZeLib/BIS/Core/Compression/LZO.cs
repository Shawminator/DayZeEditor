using System;
using System.Diagnostics;
using System.IO;

namespace BIS.Core.Compression
{
  public static class LZO
  {
    private static readonly uint M2_MAX_OFFSET = 2048;

    public static unsafe uint Decompress(byte* input, byte* output, uint expectedSize)
    {
      byte* numPtr1 = output + expectedSize;
      byte* numPtr2 = output;
      byte* numPtr3 = input;
      uint num1;
      if (*numPtr3 > (byte) 17)
      {
        num1 = (uint) *numPtr3++ - 17U;
        if (num1 >= 4U)
        {
          Debug.Assert(num1 > 0U);
          if (numPtr1 - numPtr2 < (long) num1)
            throw new OverflowException("Outpur Overrun");
          do
          {
            *numPtr2++ = *numPtr3++;
          }
          while (--num1 > 0U);
          goto label_19;
        }
        else
          goto label_63;
      }
label_5:
      byte* numPtr4 = numPtr3;
      byte* numPtr5 = numPtr4 + 1;
      uint num2 = (uint) *numPtr4;
      if (num2 < 16U)
      {
        if (num2 == 0U)
        {
          for (; *numPtr5 == (byte) 0; ++numPtr5)
            num2 += (uint) byte.MaxValue;
          num2 += 15U + (uint) *numPtr5++;
        }
        Debug.Assert(num2 > 0U);
        if (numPtr1 - numPtr2 < (long) (num2 + 3U))
          throw new OverflowException("Output Overrun");
        *(int*) numPtr2 = (int) *(uint*) numPtr5;
        numPtr2 += 4;
        numPtr3 = numPtr5 + 4;
        uint num3;
        if ((num3 = num2 - 1U) > 0U)
        {
          if (num3 >= 4U)
          {
            do
            {
              *(int*) numPtr2 = (int) *(uint*) numPtr3;
              numPtr2 += 4;
              numPtr3 += 4;
              num3 -= 4U;
            }
            while (num3 >= 4U);
            if (num3 > 0U)
            {
              do
              {
                *numPtr2++ = *numPtr3++;
              }
              while (--num3 > 0U);
            }
          }
          else
          {
            do
            {
              *numPtr2++ = *numPtr3++;
            }
            while (--num3 > 0U);
          }
        }
      }
      else
        goto label_25;
label_19:
      byte* numPtr6 = numPtr3;
      numPtr5 = numPtr6 + 1;
      num2 = (uint) *numPtr6;
      if (num2 < 16U)
      {
        byte* numPtr7 = numPtr2 - (1U + LZO.M2_MAX_OFFSET) - (num2 >> 2);
        byte* numPtr8 = numPtr5;
        numPtr3 = numPtr8 + 1;
        int num4 = (int) *numPtr8 << 2;
        byte* numPtr9 = numPtr7 - num4;
        if (numPtr9 < output || numPtr9 >= numPtr2)
          throw new OverflowException("Lookbehind Overrun");
        byte* numPtr10 = numPtr1 - numPtr2 >= 3L ? numPtr2 : throw new OverflowException("Output Overrun");
        byte* numPtr11 = numPtr10 + 1;
        byte* numPtr12 = numPtr9;
        byte* numPtr13 = numPtr12 + 1;
        int num5 = (int) *numPtr12;
        *numPtr10 = (byte) num5;
        byte* numPtr14 = numPtr11;
        byte* numPtr15 = numPtr14 + 1;
        byte* numPtr16 = numPtr13;
        byte* numPtr17 = numPtr16 + 1;
        int num6 = (int) *numPtr16;
        *numPtr14 = (byte) num6;
        byte* numPtr18 = numPtr15;
        numPtr2 = numPtr18 + 1;
        int num7 = (int) *numPtr17;
        *numPtr18 = (byte) num7;
        goto label_62;
      }
label_25:
      byte* numPtr19;
      uint num8;
      if (num2 >= 64U)
      {
        byte* numPtr20 = numPtr2 - 1 - (num2 >> 2 & 7U);
        byte* numPtr21 = numPtr5;
        numPtr3 = numPtr21 + 1;
        int num9 = (int) *numPtr21 << 3;
        numPtr19 = numPtr20 - num9;
        num8 = (num2 >> 5) - 1U;
        if (numPtr19 < output || numPtr19 >= numPtr2)
          throw new OverflowException("Lookbehind Overrun");
        Debug.Assert(num8 > 0U);
        if (numPtr1 - numPtr2 < (long) (num8 + 2U))
          throw new OverflowException("Output Overrun");
      }
      else
      {
        if (num2 >= 32U)
        {
          num8 = num2 & 31U;
          if (num8 == 0U)
          {
            for (; *numPtr5 == (byte) 0; ++numPtr5)
              num8 += (uint) byte.MaxValue;
            num8 += 31U + (uint) *numPtr5++;
          }
          numPtr19 = numPtr2 - 1 - (((int) *numPtr5 >> 2) + ((int) numPtr5[1] << 6));
          numPtr3 = numPtr5 + 2;
        }
        else if (num2 >= 16U)
        {
          byte* numPtr22 = numPtr2 - (uint) (((int) num2 & 8) << 11);
          num8 = num2 & 7U;
          if (num8 == 0U)
          {
            for (; *numPtr5 == (byte) 0; ++numPtr5)
              num8 += (uint) byte.MaxValue;
            num8 += 7U + (uint) *numPtr5++;
          }
          byte* numPtr23 = numPtr22 - (((int) *numPtr5 >> 2) + ((int) numPtr5[1] << 6));
          numPtr3 = numPtr5 + 2;
          if (numPtr23 == numPtr2)
          {
            int num10 = (int) (numPtr2 - output);
            Debug.Assert(num8 == 1U);
            if (numPtr23 != numPtr1)
              throw new OverflowException("Output Underrun");
            return (uint) (numPtr3 - input);
          }
          numPtr19 = numPtr23 - 16384;
        }
        else
        {
          byte* numPtr24 = numPtr2 - 1 - (num2 >> 2);
          byte* numPtr25 = numPtr5;
          numPtr3 = numPtr25 + 1;
          int num11 = (int) *numPtr25 << 2;
          byte* numPtr26 = numPtr24 - num11;
          if (numPtr26 < output || numPtr26 >= numPtr2)
            throw new OverflowException("Lookbehind Overrun");
          byte* numPtr27 = numPtr1 - numPtr2 >= 2L ? numPtr2 : throw new OverflowException("Output Overrun");
          byte* numPtr28 = numPtr27 + 1;
          byte* numPtr29 = numPtr26;
          byte* numPtr30 = numPtr29 + 1;
          int num12 = (int) *numPtr29;
          *numPtr27 = (byte) num12;
          byte* numPtr31 = numPtr28;
          numPtr2 = numPtr31 + 1;
          int num13 = (int) *numPtr30;
          *numPtr31 = (byte) num13;
          goto label_62;
        }
        if (numPtr19 < output || numPtr19 >= numPtr2)
          throw new OverflowException("Lookbehind Overrun");
        Debug.Assert(num8 > 0U);
        if (numPtr1 - numPtr2 < (long) (num8 + 2U))
          throw new OverflowException("Output Overrun");
        if (num8 >= 6U && numPtr2 - numPtr19 >= 4L)
        {
          *(int*) numPtr2 = (int) *(uint*) numPtr19;
          numPtr2 += 4;
          byte* numPtr32 = numPtr19 + 4;
          uint num14 = num8 - 2U;
          do
          {
            *(int*) numPtr2 = (int) *(uint*) numPtr32;
            numPtr2 += 4;
            numPtr32 += 4;
            num14 -= 4U;
          }
          while (num14 >= 4U);
          if (num14 > 0U)
          {
            do
            {
              *numPtr2++ = *numPtr32++;
            }
            while (--num14 > 0U);
            goto label_62;
          }
          else
            goto label_62;
        }
      }
      byte* numPtr33 = numPtr2;
      byte* numPtr34 = numPtr33 + 1;
      byte* numPtr35 = numPtr19;
      byte* numPtr36 = numPtr35 + 1;
      int num15 = (int) *numPtr35;
      *numPtr33 = (byte) num15;
      byte* numPtr37 = numPtr34;
      numPtr2 = numPtr37 + 1;
      byte* numPtr38 = numPtr36;
      byte* numPtr39 = numPtr38 + 1;
      int num16 = (int) *numPtr38;
      *numPtr37 = (byte) num16;
      do
      {
        *numPtr2++ = *numPtr39++;
      }
      while (--num8 > 0U);
label_62:
      num1 = (uint) numPtr3[-2] & 3U;
      if (num1 == 0U)
        goto label_5;
label_63:
      Debug.Assert(num1 > 0U && num1 < 4U);
      if (numPtr1 - numPtr2 < (long) num1)
        throw new OverflowException("Output Overrun");
      byte* numPtr40 = numPtr2++;
      byte* numPtr41 = numPtr3;
      byte* numPtr42 = numPtr41 + 1;
      int num17 = (int) *numPtr41;
      *numPtr40 = (byte) num17;
      if (num1 > 1U)
      {
        *numPtr2++ = *numPtr42++;
        if (num1 > 2U)
          *numPtr2++ = *numPtr42++;
      }
      byte* numPtr43 = numPtr42;
      numPtr5 = numPtr43 + 1;
      num2 = (uint) *numPtr43;
      goto label_25;
    }

    private static byte ip(Stream i)
    {
      byte num = (byte) i.ReadByte();
      --i.Position;
      return num;
    }

    private static byte ip(Stream i, short offset)
    {
      i.Position += (long) offset;
      byte num = (byte) i.ReadByte();
      i.Position -= (long) ((int) offset + 1);
      return num;
    }

    private static byte next(Stream i) => (byte) i.ReadByte();

    public static unsafe uint Decompress(Stream i, byte* output, uint expectedSize)
    {
      long position = i.Position;
      byte* numPtr1 = output + expectedSize;
      byte* numPtr2 = output;
      uint num1;
      if (LZO.ip(i) > (byte) 17)
      {
        num1 = (uint) LZO.next(i) - 17U;
        if (num1 >= 4U)
        {
          Debug.Assert(num1 > 0U);
          if (numPtr1 - numPtr2 < (long) num1)
            throw new OverflowException("Outpur Overrun");
          do
          {
            *numPtr2++ = LZO.next(i);
          }
          while (--num1 > 0U);
          goto label_19;
        }
        else
          goto label_63;
      }
label_5:
      uint num2 = (uint) LZO.next(i);
      if (num2 < 16U)
      {
        if (num2 == 0U)
        {
          while (LZO.ip(i) == (byte) 0)
          {
            num2 += (uint) byte.MaxValue;
            ++i.Position;
          }
          num2 += 15U + (uint) LZO.next(i);
        }
        Debug.Assert(num2 > 0U);
        if (numPtr1 - numPtr2 < (long) (num2 + 3U))
          throw new OverflowException("Output Overrun");
        byte* numPtr3 = numPtr2;
        byte* numPtr4 = numPtr3 + 1;
        int num3 = (int) LZO.next(i);
        *numPtr3 = (byte) num3;
        byte* numPtr5 = numPtr4;
        byte* numPtr6 = numPtr5 + 1;
        int num4 = (int) LZO.next(i);
        *numPtr5 = (byte) num4;
        byte* numPtr7 = numPtr6;
        byte* numPtr8 = numPtr7 + 1;
        int num5 = (int) LZO.next(i);
        *numPtr7 = (byte) num5;
        byte* numPtr9 = numPtr8;
        numPtr2 = numPtr9 + 1;
        int num6 = (int) LZO.next(i);
        *numPtr9 = (byte) num6;
        uint num7;
        if ((num7 = num2 - 1U) > 0U)
        {
          if (num7 >= 4U)
          {
            do
            {
              byte* numPtr10 = numPtr2;
              byte* numPtr11 = numPtr10 + 1;
              int num8 = (int) LZO.next(i);
              *numPtr10 = (byte) num8;
              byte* numPtr12 = numPtr11;
              byte* numPtr13 = numPtr12 + 1;
              int num9 = (int) LZO.next(i);
              *numPtr12 = (byte) num9;
              byte* numPtr14 = numPtr13;
              byte* numPtr15 = numPtr14 + 1;
              int num10 = (int) LZO.next(i);
              *numPtr14 = (byte) num10;
              byte* numPtr16 = numPtr15;
              numPtr2 = numPtr16 + 1;
              int num11 = (int) LZO.next(i);
              *numPtr16 = (byte) num11;
              num7 -= 4U;
            }
            while (num7 >= 4U);
            if (num7 > 0U)
            {
              do
              {
                *numPtr2++ = LZO.next(i);
              }
              while (--num7 > 0U);
            }
          }
          else
          {
            do
            {
              *numPtr2++ = LZO.next(i);
            }
            while (--num7 > 0U);
          }
        }
      }
      else
        goto label_25;
label_19:
      num2 = (uint) LZO.next(i);
      if (num2 < 16U)
      {
        byte* numPtr17 = numPtr2 - (1U + LZO.M2_MAX_OFFSET) - (num2 >> 2) - ((int) LZO.next(i) << 2);
        if (numPtr17 < output || numPtr17 >= numPtr2)
          throw new OverflowException("Lookbehind Overrun");
        byte* numPtr18 = numPtr1 - numPtr2 >= 3L ? numPtr2 : throw new OverflowException("Output Overrun");
        byte* numPtr19 = numPtr18 + 1;
        byte* numPtr20 = numPtr17;
        byte* numPtr21 = numPtr20 + 1;
        int num12 = (int) *numPtr20;
        *numPtr18 = (byte) num12;
        byte* numPtr22 = numPtr19;
        byte* numPtr23 = numPtr22 + 1;
        byte* numPtr24 = numPtr21;
        byte* numPtr25 = numPtr24 + 1;
        int num13 = (int) *numPtr24;
        *numPtr22 = (byte) num13;
        byte* numPtr26 = numPtr23;
        numPtr2 = numPtr26 + 1;
        int num14 = (int) *numPtr25;
        *numPtr26 = (byte) num14;
        goto label_62;
      }
label_25:
      byte* numPtr27;
      uint num15;
      if (num2 >= 64U)
      {
        numPtr27 = numPtr2 - 1 - (num2 >> 2 & 7U) - ((int) LZO.next(i) << 3);
        num15 = (num2 >> 5) - 1U;
        if (numPtr27 < output || numPtr27 >= numPtr2)
          throw new OverflowException("Lookbehind Overrun");
        Debug.Assert(num15 > 0U);
        if (numPtr1 - numPtr2 < (long) (num15 + 2U))
          throw new OverflowException("Output Overrun");
      }
      else
      {
        if (num2 >= 32U)
        {
          num15 = num2 & 31U;
          if (num15 == 0U)
          {
            while (LZO.ip(i) == (byte) 0)
            {
              num15 += (uint) byte.MaxValue;
              ++i.Position;
            }
            num15 += 31U + (uint) LZO.next(i);
          }
          numPtr27 = numPtr2 - 1 - (((int) LZO.ip(i, (short) 0) >> 2) + ((int) LZO.ip(i, (short) 1) << 6));
          i.Position += 2L;
        }
        else if (num2 >= 16U)
        {
          byte* numPtr28 = numPtr2 - (uint) (((int) num2 & 8) << 11);
          num15 = num2 & 7U;
          if (num15 == 0U)
          {
            while (LZO.ip(i) == (byte) 0)
            {
              num15 += (uint) byte.MaxValue;
              ++i.Position;
            }
            num15 += 7U + (uint) LZO.next(i);
          }
          byte* numPtr29 = numPtr28 - (((int) LZO.ip(i, (short) 0) >> 2) + ((int) LZO.ip(i, (short) 1) << 6));
          i.Position += 2L;
          if (numPtr29 == numPtr2)
          {
            int num16 = (int) (numPtr2 - output);
            Debug.Assert(num15 == 1U);
            if (numPtr29 != numPtr1)
              throw new OverflowException("Output Underrun");
            return (uint) (i.Position - position);
          }
          numPtr27 = numPtr29 - 16384;
        }
        else
        {
          byte* numPtr30 = numPtr2 - 1 - (num2 >> 2) - ((int) LZO.next(i) << 2);
          if (numPtr30 < output || numPtr30 >= numPtr2)
            throw new OverflowException("Lookbehind Overrun");
          byte* numPtr31 = numPtr1 - numPtr2 >= 2L ? numPtr2 : throw new OverflowException("Output Overrun");
          byte* numPtr32 = numPtr31 + 1;
          byte* numPtr33 = numPtr30;
          byte* numPtr34 = numPtr33 + 1;
          int num17 = (int) *numPtr33;
          *numPtr31 = (byte) num17;
          byte* numPtr35 = numPtr32;
          numPtr2 = numPtr35 + 1;
          int num18 = (int) *numPtr34;
          *numPtr35 = (byte) num18;
          goto label_62;
        }
        if (numPtr27 < output || numPtr27 >= numPtr2)
          throw new OverflowException("Lookbehind Overrun");
        Debug.Assert(num15 > 0U);
        if (numPtr1 - numPtr2 < (long) (num15 + 2U))
          throw new OverflowException("Output Overrun");
        if (num15 >= 6U && numPtr2 - numPtr27 >= 4L)
        {
          *(int*) numPtr2 = (int) *(uint*) numPtr27;
          numPtr2 += 4;
          byte* numPtr36 = numPtr27 + 4;
          uint num19 = num15 - 2U;
          do
          {
            *(int*) numPtr2 = (int) *(uint*) numPtr36;
            numPtr2 += 4;
            numPtr36 += 4;
            num19 -= 4U;
          }
          while (num19 >= 4U);
          if (num19 > 0U)
          {
            do
            {
              *numPtr2++ = *numPtr36++;
            }
            while (--num19 > 0U);
            goto label_62;
          }
          else
            goto label_62;
        }
      }
      byte* numPtr37 = numPtr2;
      byte* numPtr38 = numPtr37 + 1;
      byte* numPtr39 = numPtr27;
      byte* numPtr40 = numPtr39 + 1;
      int num20 = (int) *numPtr39;
      *numPtr37 = (byte) num20;
      byte* numPtr41 = numPtr38;
      numPtr2 = numPtr41 + 1;
      byte* numPtr42 = numPtr40;
      byte* numPtr43 = numPtr42 + 1;
      int num21 = (int) *numPtr42;
      *numPtr41 = (byte) num21;
      do
      {
        *numPtr2++ = *numPtr43++;
      }
      while (--num15 > 0U);
label_62:
      num1 = (uint) LZO.ip(i, (short) -2) & 3U;
      if (num1 == 0U)
        goto label_5;
label_63:
      Debug.Assert(num1 > 0U && num1 < 4U);
      if (numPtr1 - numPtr2 < (long) num1)
        throw new OverflowException("Output Overrun");
      *numPtr2++ = LZO.next(i);
      if (num1 > 1U)
      {
        *numPtr2++ = LZO.next(i);
        if (num1 > 2U)
          *numPtr2++ = LZO.next(i);
      }
      num2 = (uint) LZO.next(i);
      goto label_25;
    }

    public static unsafe uint ReadLZO(Stream input, out byte[] dst, uint expectedSize)
    {
      dst = new byte[(int) expectedSize];
      fixed (byte* output = &dst[0])
        return LZO.Decompress(input, output, expectedSize);
    }

    public static unsafe byte[] ReadLZO(Stream input, uint expectedSize)
    {
      byte[] numArray = new byte[(int) expectedSize];
      fixed (byte* output = &numArray[0])
      {
        int num = (int) LZO.Decompress(input, output, expectedSize);
      }
      return numArray;
    }
  }
}
