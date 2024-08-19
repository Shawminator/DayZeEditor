using BIS.Core.Streams;
using System.IO;

namespace BIS.Core.Math
{
  public class Matrix4P
  {
    public Matrix3P Orientation { get; }

    public Vector3P Position { get; }

    public float this[int row, int col]
    {
      get => col == 3 ? this.Position[row] : this.Orientation[col][row];
      set
      {
        if (col == 3)
          this.Position[row] = value;
        else
          this.Orientation[col][row] = value;
      }
    }

    public Matrix4P()
      : this(0.0f)
    {
    }

    public Matrix4P(float val)
      : this(new Matrix3P(val), new Vector3P(val))
    {
    }

    public Matrix4P(BinaryReaderEx input)
      : this(new Matrix3P(input), new Vector3P(input))
    {
    }

    private Matrix4P(Matrix3P orientation, Vector3P position)
    {
      this.Orientation = orientation;
      this.Position = position;
    }

    public static Matrix4P operator *(Matrix4P a, Matrix4P b)
    {
      Matrix4P matrix4P = new Matrix4P();
      float num1 = b[0, 0];
      float num2 = b[1, 0];
      float num3 = b[2, 0];
      matrix4P[0, 0] = (float) ((double) a[0, 0] * (double) num1 + (double) a[0, 1] * (double) num2 + (double) a[0, 2] * (double) num3);
      matrix4P[1, 0] = (float) ((double) a[1, 0] * (double) num1 + (double) a[1, 1] * (double) num2 + (double) a[1, 2] * (double) num3);
      matrix4P[2, 0] = (float) ((double) a[2, 0] * (double) num1 + (double) a[2, 1] * (double) num2 + (double) a[2, 2] * (double) num3);
      float num4 = b[0, 1];
      float num5 = b[1, 1];
      float num6 = b[2, 1];
      matrix4P[0, 1] = (float) ((double) a[0, 0] * (double) num4 + (double) a[0, 1] * (double) num5 + (double) a[0, 2] * (double) num6);
      matrix4P[1, 1] = (float) ((double) a[1, 0] * (double) num4 + (double) a[1, 1] * (double) num5 + (double) a[1, 2] * (double) num6);
      matrix4P[2, 1] = (float) ((double) a[2, 0] * (double) num4 + (double) a[2, 1] * (double) num5 + (double) a[2, 2] * (double) num6);
      float num7 = b[0, 2];
      float num8 = b[1, 2];
      float num9 = b[2, 2];
      matrix4P[0, 2] = (float) ((double) a[0, 0] * (double) num7 + (double) a[0, 1] * (double) num8 + (double) a[0, 2] * (double) num9);
      matrix4P[1, 2] = (float) ((double) a[1, 0] * (double) num7 + (double) a[1, 1] * (double) num8 + (double) a[1, 2] * (double) num9);
      matrix4P[2, 2] = (float) ((double) a[2, 0] * (double) num7 + (double) a[2, 1] * (double) num8 + (double) a[2, 2] * (double) num9);
      float x = b.Position.X;
      float y = b.Position.Y;
      float z = b.Position.Z;
      matrix4P.Position.X = (float) ((double) a[0, 0] * (double) x + (double) a[0, 1] * (double) y + (double) a[0, 2] * (double) z) + a.Position.X;
      matrix4P.Position.Y = (float) ((double) a[1, 0] * (double) x + (double) a[1, 1] * (double) y + (double) a[1, 2] * (double) z) + a.Position.Y;
      matrix4P.Position.Z = (float) ((double) a[2, 0] * (double) x + (double) a[2, 1] * (double) y + (double) a[2, 2] * (double) z) + a.Position.Z;
      return matrix4P;
    }

    public static Matrix4P ReadMatrix4Quat16b(BinaryReaderEx input)
    {
      Quaternion quaternion = Quaternion.ReadCompressed((BinaryReader) input);
      ShortFloat x = new ShortFloat(input.ReadUInt16());
      ShortFloat y = new ShortFloat(input.ReadUInt16());
      ShortFloat z = new ShortFloat(input.ReadUInt16());
      return new Matrix4P(quaternion.AsRotationMatrix(), new Vector3P((float) x, (float) y, (float) z));
    }

    public void Write(BinaryWriterEx output)
    {
      this.Orientation.Write(output);
      this.Position.Write(output);
    }

    public override string ToString() => string.Format("{0}, {1}, {2}, {3},\n{4}, {5}, {6}, {7},\n{8}, {9}, {10}, {11},\n{12}, {13}, {14}, 1", (object) this[0, 0], (object) this[0, 1], (object) this[0, 2], (object) this[0, 3], (object) this[1, 0], (object) this[1, 1], (object) this[1, 2], (object) this[1, 3], (object) this[2, 0], (object) this[2, 1], (object) this[2, 2], (object) this[2, 3], (object) this[3, 0], (object) this[3, 1], (object) this[3, 2]);
  }
}
