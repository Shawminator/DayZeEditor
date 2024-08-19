namespace BIS.Core.Math
{
  public struct ShortFloat
  {
    private const int MANTISSA_SIZE = 10;
    private const int SIGN_BIT = 32768;
    private const int M_MASK = 1024;
    private const int EXPONENT_SIZE = 4;
    private const int E_MASK = 16;
    private ushort value;

    public ShortFloat(ushort v) => this.value = v;

    public static implicit operator float(ShortFloat d) => (float) d.DoubleValue;

    public double DoubleValue
    {
      get
      {
        double num1 = ((int) this.value & 32768) != 0 ? -1.0 : 1.0;
        int num2 = ((int) this.value & (int) short.MaxValue) >> 10;
        double num3 = (double) ((int) this.value & 1023) / 1024.0;
        return num2 == 0 ? num1 / 16384.0 * (0.0 + num3) : num1 * System.Math.Pow(2.0, (double) (num2 - 15)) * (1.0 + num3);
      }
    }
  }
}
