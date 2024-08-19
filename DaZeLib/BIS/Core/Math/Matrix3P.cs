using BIS.Core.Streams;

namespace BIS.Core.Math
{
  public class Matrix3P
  {
    private Vector3P[] columns;

    public Vector3P Aside => this.columns[0];

    public Vector3P Up => this.columns[1];

    public Vector3P Dir => this.columns[2];

    public Vector3P this[int col] => this.columns[col];

    public float this[int row, int col]
    {
      get => this[col][row];
      set => this[col][row] = value;
    }

    public Matrix3P()
      : this(0.0f)
    {
    }

    public Matrix3P(float val)
      : this(new Vector3P(val), new Vector3P(val), new Vector3P(val))
    {
    }

    public Matrix3P(BinaryReaderEx input)
      : this(new Vector3P(input), new Vector3P(input), new Vector3P(input))
    {
    }

    private Matrix3P(Vector3P aside, Vector3P up, Vector3P dir) => this.columns = new Vector3P[3]
    {
      aside,
      up,
      dir
    };

    public static Matrix3P operator -(Matrix3P a) => new Matrix3P(-a.Aside, -a.Up, -a.Dir);

    public static Matrix3P operator *(Matrix3P a, Matrix3P b)
    {
      Matrix3P matrix3P = new Matrix3P();
      float num1 = b[0, 0];
      float num2 = b[1, 0];
      float num3 = b[2, 0];
      matrix3P[0, 0] = (float) ((double) a[0, 0] * (double) num1 + (double) a[0, 1] * (double) num2 + (double) a[0, 2] * (double) num3);
      matrix3P[1, 0] = (float) ((double) a[1, 0] * (double) num1 + (double) a[1, 1] * (double) num2 + (double) a[1, 2] * (double) num3);
      matrix3P[2, 0] = (float) ((double) a[2, 0] * (double) num1 + (double) a[2, 1] * (double) num2 + (double) a[2, 2] * (double) num3);
      float num4 = b[0, 1];
      float num5 = b[1, 1];
      float num6 = b[2, 1];
      matrix3P[0, 1] = (float) ((double) a[0, 0] * (double) num4 + (double) a[0, 1] * (double) num5 + (double) a[0, 2] * (double) num6);
      matrix3P[1, 1] = (float) ((double) a[1, 0] * (double) num4 + (double) a[1, 1] * (double) num5 + (double) a[1, 2] * (double) num6);
      matrix3P[2, 1] = (float) ((double) a[2, 0] * (double) num4 + (double) a[2, 1] * (double) num5 + (double) a[2, 2] * (double) num6);
      float num7 = b[0, 2];
      float num8 = b[1, 2];
      float num9 = b[2, 2];
      matrix3P[0, 2] = (float) ((double) a[0, 0] * (double) num7 + (double) a[0, 1] * (double) num8 + (double) a[0, 2] * (double) num9);
      matrix3P[1, 2] = (float) ((double) a[1, 0] * (double) num7 + (double) a[1, 1] * (double) num8 + (double) a[1, 2] * (double) num9);
      matrix3P[2, 2] = (float) ((double) a[2, 0] * (double) num7 + (double) a[2, 1] * (double) num8 + (double) a[2, 2] * (double) num9);
      return matrix3P;
    }

    public void SetTilda(Vector3P a)
    {
      this.Aside.Y = -a.Z;
      this.Aside.Z = a.Y;
      this.Up.X = a.Z;
      this.Up.Z = -a.X;
      this.Dir.X = -a.Y;
      this.Dir.Y = a.X;
    }

    public void Write(BinaryWriterEx output)
    {
      this.Aside.Write(output);
      this.Up.Write(output);
      this.Dir.Write(output);
    }

    public override string ToString() => string.Format("{0}, {1}, {2},\n{3}, {4}, {5},\n{6}, {7}, {8}", (object) this[0, 0], (object) this[0, 1], (object) this[0, 2], (object) this[1, 0], (object) this[1, 1], (object) this[1, 2], (object) this[2, 0], (object) this[2, 1], (object) this[2, 2]);
  }
}
