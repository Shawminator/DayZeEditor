using System.Diagnostics;

namespace BIS.Core
{
  public struct PackedColor
  {
    private uint value;

    public byte A8 => (byte) (this.value >> 24 & (uint) byte.MaxValue);

    public byte R8 => (byte) (this.value >> 16 & (uint) byte.MaxValue);

    public byte G8 => (byte) (this.value >> 8 & (uint) byte.MaxValue);

    public byte B8 => (byte) (this.value & (uint) byte.MaxValue);

    public PackedColor(uint value) => this.value = value;

    public PackedColor(byte r, byte g, byte b, byte a = 255) => this.value = PackedColor.PackColor(r, g, b, a);

    public PackedColor(float r, float g, float b, float a)
    {
      Debug.Assert((double) r <= 1.0 && (double) r >= 0.0 && !float.IsNaN(r));
      Debug.Assert((double) g <= 1.0 && (double) g >= 0.0 && !float.IsNaN(g));
      Debug.Assert((double) b <= 1.0 && (double) b >= 0.0 && !float.IsNaN(b));
      Debug.Assert((double) a <= 1.0 && (double) a >= 0.0 && !float.IsNaN(a));
      this.value = PackedColor.PackColor((byte) ((double) r * (double) byte.MaxValue), (byte) ((double) g * (double) byte.MaxValue), (byte) ((double) b * (double) byte.MaxValue), (byte) ((double) a * (double) byte.MaxValue));
    }

    internal static uint PackColor(byte r, byte g, byte b, byte a) => (uint) ((int) a << 24 | (int) r << 16 | (int) g << 8) | (uint) b;
  }
}
