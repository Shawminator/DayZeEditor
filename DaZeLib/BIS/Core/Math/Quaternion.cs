using System.IO;

namespace BIS.Core.Math
{
  public class Quaternion
  {
    private float x;
    private float y;
    private float z;
    private float w;

    public float X => this.x;

    public float Y => this.y;

    public float Z => this.z;

    public float W => this.w;

    public static Quaternion ReadCompressed(BinaryReader input) => new Quaternion((float) -input.ReadInt16() / 16384f, (float) input.ReadInt16() / 16384f, (float) -input.ReadInt16() / 16384f, (float) input.ReadInt16() / 16384f);

    public Quaternion()
    {
      this.w = 1f;
      this.x = 0.0f;
      this.y = 0.0f;
      this.z = 0.0f;
    }

    public Quaternion(float x, float y, float z, float w)
    {
      this.w = w;
      this.x = x;
      this.y = y;
      this.z = z;
    }

    public static Quaternion operator *(Quaternion a, Quaternion b)
    {
      float w = (float) ((double) a.w * (double) b.w - (double) a.x * (double) b.x - (double) a.y * (double) b.y - (double) a.z * (double) b.z);
      return new Quaternion((float) ((double) a.w * (double) b.x + (double) a.x * (double) b.w + (double) a.y * (double) b.z - (double) a.z * (double) b.y), (float) ((double) a.w * (double) b.y - (double) a.x * (double) b.z + (double) a.y * (double) b.w + (double) a.z * (double) b.x), (float) ((double) a.w * (double) b.z + (double) a.x * (double) b.y - (double) a.y * (double) b.x + (double) a.z * (double) b.w), w);
    }

    public Quaternion Inverse
    {
      get
      {
        this.Normalize();
        return this.Conjugate;
      }
    }

    public Quaternion Conjugate => new Quaternion(-this.x, -this.y, -this.z, this.w);

    public void Normalize()
    {
      float num = (float) (1.0 / System.Math.Sqrt((double) this.x * (double) this.x + (double) this.y * (double) this.y + (double) this.z * (double) this.z + (double) this.w * (double) this.w));
      this.x *= num;
      this.y *= num;
      this.z *= num;
      this.w *= num;
    }

    public Vector3P Transform(Vector3P xyz)
    {
      Quaternion quaternion = this * new Quaternion(xyz.X, xyz.Y, xyz.Z, 0.0f) * this.Inverse;
      return new Vector3P(quaternion.x, quaternion.y, quaternion.z);
    }

    public Matrix3P AsRotationMatrix()
    {
      Matrix3P matrix3P = new Matrix3P();
      double num1 = (double) this.x * (double) this.y;
      double num2 = (double) this.w * (double) this.z;
      double num3 = (double) this.w * (double) this.x;
      double num4 = (double) this.w * (double) this.y;
      double num5 = (double) this.x * (double) this.z;
      double num6 = (double) this.y * (double) this.z;
      double num7 = (double) this.z * (double) this.z;
      double num8 = (double) this.y * (double) this.y;
      double num9 = (double) this.x * (double) this.x;
      matrix3P[0, 0] = (float) (1.0 - 2.0 * (num8 + num7));
      matrix3P[0, 1] = (float) (2.0 * (num1 - num2));
      matrix3P[0, 2] = (float) (2.0 * (num5 + num4));
      matrix3P[1, 0] = (float) (2.0 * (num1 + num2));
      matrix3P[1, 1] = (float) (1.0 - 2.0 * (num9 + num7));
      matrix3P[1, 2] = (float) (2.0 * (num6 - num3));
      matrix3P[2, 0] = (float) (2.0 * (num5 - num4));
      matrix3P[2, 1] = (float) (2.0 * (num6 + num3));
      matrix3P[2, 2] = (float) (1.0 - 2.0 * (num9 + num8));
      return matrix3P;
    }
  }
}
