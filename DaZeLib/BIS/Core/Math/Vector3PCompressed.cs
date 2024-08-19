using BIS.Core.Streams;

namespace BIS.Core.Math
{
  public class Vector3PCompressed
  {
    private int value;
    private const float scaleFactor = -0.00195694715f;

    public float X
    {
      get
      {
        int num = this.value & 1023;
        if (num > 511)
          num -= 1024;
        return (float) num * (-1f / 511f);
      }
    }

    public float Y
    {
      get
      {
        int num = this.value >> 10 & 1023;
        if (num > 511)
          num -= 1024;
        return (float) num * (-1f / 511f);
      }
    }

    public float Z
    {
      get
      {
        int num = this.value >> 20 & 1023;
        if (num > 511)
          num -= 1024;
        return (float) num * (-1f / 511f);
      }
    }

    public static implicit operator Vector3P(Vector3PCompressed src)
    {
      int num1 = src.value & 1023;
      int num2 = src.value >> 10 & 1023;
      int num3 = src.value >> 20 & 1023;
      if (num1 > 511)
        num1 -= 1024;
      if (num2 > 511)
        num2 -= 1024;
      if (num3 > 511)
        num3 -= 1024;
      return new Vector3P((float) num1 * (-1f / 511f), (float) num2 * (-1f / 511f), (float) num3 * (-1f / 511f));
    }

    public static implicit operator int(Vector3PCompressed src) => src.value;

    public static implicit operator Vector3PCompressed(int src) => new Vector3PCompressed(src);

    public Vector3PCompressed(int value) => this.value = value;

    public Vector3PCompressed(BinaryReaderEx input) => this.value = input.ReadInt32();
  }
}
