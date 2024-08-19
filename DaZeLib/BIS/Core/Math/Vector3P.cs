using BIS.Core.Streams;
using System;
using System.Globalization;

namespace BIS.Core.Math
{
  public class Vector3P
  {
    private float[] xyz;

    public float X
    {
      get => this.xyz[0];
      set => this.xyz[0] = value;
    }

    public float Y
    {
      get => this.xyz[1];
      set => this.xyz[1] = value;
    }

    public float Z
    {
      get => this.xyz[2];
      set => this.xyz[2] = value;
    }

    public Vector3P()
      : this(0.0f)
    {
    }

    public Vector3P(float val)
      : this(val, val, val)
    {
    }

    public Vector3P(BinaryReaderEx input)
      : this(input.ReadSingle(), input.ReadSingle(), input.ReadSingle())
    {
    }

    public Vector3P(int compressed)
      : this()
    {
      int num1 = compressed & 1023;
      int num2 = compressed >> 10 & 1023;
      int num3 = compressed >> 20 & 1023;
      if (num1 > 511)
        num1 -= 1024;
      if (num2 > 511)
        num2 -= 1024;
      if (num3 > 511)
        num3 -= 1024;
      this.X = (float) num1 * (-1f / 511f);
      this.Y = (float) num2 * (-1f / 511f);
      this.Z = (float) num3 * (-1f / 511f);
    }

    public Vector3P(float x, float y, float z) => this.xyz = new float[3]
    {
      x,
      y,
      z
    };

    public double Length => System.Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z);

    public float this[int i]
    {
      get => this.xyz[i];
      set => this.xyz[i] = value;
    }

    public static Vector3P operator -(Vector3P a) => new Vector3P(-a.X, -a.Y, -a.Z);

    public void Write(BinaryWriterEx output)
    {
      output.Write(this.X);
      output.Write(this.Y);
      output.Write(this.Z);
    }

    public static Vector3P operator *(Vector3P a, float b) => new Vector3P(a.X * b, a.Y * b, a.Z * b);

    public static float operator *(Vector3P a, Vector3P b) => (float) ((double) a.X * (double) b.X + (double) a.Y * (double) b.Y + (double) a.Z * (double) b.Z);

    public static Vector3P operator +(Vector3P a, Vector3P b) => new Vector3P(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vector3P operator -(Vector3P a, Vector3P b) => new Vector3P(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public override bool Equals(object obj) => obj is Vector3P other && base.Equals(obj) && this.Equals(other);

    public override int GetHashCode() => base.GetHashCode();

    public bool Equals(Vector3P other)
    {
      Func<float, float, bool> func = (Func<float, float, bool>) ((f1, f2) => (double) System.Math.Abs(f1 - f2) < 0.05);
      return func(this.X, other.X) && func(this.Y, other.Y) && func(this.Z, other.Z);
    }

    public override string ToString()
    {
      CultureInfo cultureInfo = new CultureInfo("en-GB");
      return "{" + this.X.ToString((IFormatProvider) cultureInfo.NumberFormat) + "," + this.Y.ToString((IFormatProvider) cultureInfo.NumberFormat) + "," + this.Z.ToString((IFormatProvider) cultureInfo.NumberFormat) + "}";
    }

    public float Distance(Vector3P v)
    {
      Vector3P vector3P = this - v;
      return (float) System.Math.Sqrt((double) vector3P.X * (double) vector3P.X + (double) vector3P.Y * (double) vector3P.Y + (double) vector3P.Z * (double) vector3P.Z);
    }

    public void Normalize()
    {
      float length = (float) this.Length;
      this.X /= length;
      this.Y /= length;
      this.Z /= length;
    }

    public static Vector3P CrossProduct(Vector3P a, Vector3P b) => new Vector3P((float) ((double) a.Y * (double) b.Z - (double) a.Z * (double) b.Y), (float) ((double) a.Z * (double) b.X - (double) a.X * (double) b.Z), (float) ((double) a.X * (double) b.Y - (double) a.Y * (double) b.X));
  }
}
