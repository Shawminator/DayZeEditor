using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DayZeLib
{
    public class Vec3PandR
    {
        public bool rotspecified { get; set; }
        public Vec3 Position { get; set; }
        public Vec3 Rotation { get; set; }

        public Vec3PandR() { }
        public Vec3PandR(float[] position, float[] rotaion, bool _rotspecified)
        {
            Position = new Vec3(position);
            rotspecified = _rotspecified;
            if (rotspecified)
                Rotation = new Vec3(rotaion);
            else
                Rotation = new Vec3(0, 0, 0);
        }
        public Vec3PandR(string Stuff)
        {
            rotspecified = false;
            if (Stuff.Contains('|'))
            {
                string[] posrotstring = Stuff.Split('|');
                string[] possplit = posrotstring[0].Split(' ');
                Position = new Vec3(Convert.ToSingle(possplit[0]), Convert.ToSingle(possplit[1]), Convert.ToSingle(possplit[2]));
                string[] rotsplit = posrotstring[1].Split(' ');
                Rotation = new Vec3(Convert.ToSingle(rotsplit[0]), Convert.ToSingle(rotsplit[1]), Convert.ToSingle(rotsplit[2]));
                rotspecified = true;
            }
            else
            {
                string[] possplit = Stuff.Split(' ');
                Position = new Vec3(Convert.ToSingle(possplit[0]), Convert.ToSingle(possplit[1]), Convert.ToSingle(possplit[2]));
                Rotation = new Vec3(0, 0, 0);
            }
        }
        public string GetString()
        {
            string posrot = "";
            if (rotspecified)
            {
                posrot = Position.X + " " + Position.Y + " " + Position.Z + "|" + Rotation.X + " " + Rotation.Y + " " + Rotation.Z;
            }
            else
            {
                posrot = Position.X + " " + Position.Y + " " + Position.Z;
            }
            return posrot;
        }
        public float[] GetPositionFloatArray()
        {
            return new float[] { (float)Position.X, (float)Position.Y, (float)Position.Z };
        }
        public float[] GetRotationFloatArray()
        {
            if (rotspecified)
            {
                return new float[] { (float)Rotation.X, (float)Rotation.Y, (float)Rotation.Z };
            }
            else return new float[] { 0, 0, 0 };
        }
        public override string ToString()
        {
            if (rotspecified)
            {
                return Position.X + " " + Position.Y + " " + Position.Z + "|" + Rotation.X + " " + Rotation.Y + " " + Rotation.Z; ;
            }
            else
            {
                return Position.X + " " + Position.Y + " " + Position.Z;
            }
        }
    }

    public class Vec3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Vec3(string[] floats)
        {
            var format = new NumberFormatInfo();
            format.NegativeSign = "-";
            format.NumberDecimalSeparator = ".";
            X = Convert.ToSingle(floats[0], format);
            Y = Convert.ToSingle(floats[1], format);
            Z = Convert.ToSingle(floats[2], format);
        }
        public Vec3(float[] floats)
        {
            X = floats[0];
            Y = floats[1];
            Z = floats[2];
        }
        public Vec3(decimal[] decimals)
        {
            X = (float)decimals[0];
            Y = (float)decimals[1];
            Z = (float)decimals[2];
        }
        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vec3() { }

        public override string ToString()
        {
            return X.ToString() + "," + Y.ToString() + "," + Z.ToString();
        }

        public float[] getfloatarray()
        {
            return new float[] { X, Y, Z };
        }
        public string GetString()
        {
            return X.ToString() + " " + Y.ToString() + " " + Z.ToString();
        }

        public decimal[] getDecimalArray()
        {
            return new decimal[] { (decimal)X, (decimal)Y, (decimal)Z };
        }
    }
    public class MapData
    {
        public bool FileExists { get; set; }
        public string FileName { get; set; }

        public MapData(string filename)
        {
            if (File.Exists(filename))
            {
                FileName = filename;
                FileExists = true;
            }
            else
                FileExists = false;
        }
        public void CreateNewData()
        {
            List<Vec3> points = new List<Vec3>();
            string[] lines = File.ReadAllLines(FileName);
            foreach (string line in lines)
            {
                string[] split = line.Split(' ');
                string[] newaary = new string[3];
                newaary[0] = split[0];
                newaary[1] = split[2];
                newaary[2] = split[1];
                points.Add(new Vec3(newaary));
            }
            OpenFileDialog savefiel = new OpenFileDialog();
            savefiel.Title = "Please Select the map you are creating the XYZ for?";
            savefiel.InitialDirectory = Application.StartupPath + "\\Maps";
            if (savefiel.ShowDialog() == DialogResult.OK)
            {
                using (FileStream writeStream = new FileStream(savefiel.FileName + ".xyz", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                using (BinaryWriter writeBinay = new BinaryWriter(writeStream))
                {
                    writeBinay.Write((long)points.Count());
                    foreach (Vec3 v in points)
                    {
                        writeBinay.Write(v.X);
                        writeBinay.Write(v.Y);
                        writeBinay.Write(v.Z);
                    }
                }
            }
        }
        public float gethieght(float v1, float v2)
        {
            //CreateNewData(); only used to bin map file
            List<Vec3> points = new List<Vec3>();
            byte[] bytearray = File.ReadAllBytes(FileName);
            using (MemoryStream ms = new MemoryStream(bytearray))
            using (BinaryReader br = new BinaryReader(ms))
            {
                long count = br.ReadInt64();
                for (int i = 0; i < count; i++)
                {
                    Vec3 newvec = new Vec3();
                    newvec.X = br.ReadSingle();
                    newvec.Y = br.ReadSingle();
                    newvec.Z = br.ReadSingle();
                    points.Add(newvec);
                }
            }
            //points id a List<vec3> containing X,Y,Z values.

            //List<Vec3> test = points
            //    .OrderBy(x => Math.Abs(v1 - x.X))
            //    .ToList();
            //List<Vec3> test2 = test
            //    .FindAll(x => x.X == test[0].X)
            //    .OrderBy(x => Math.Abs(v2 - x.Y))
            //    .ToList();
            //return test2[0].Z;

            Vec3[] closestPoints = points.OrderBy(p => Math.Abs(v1 - p.X) + Math.Abs(v2 - p.Y)).Take(4).ToArray();

            if (closestPoints.Length < 4)
            {
                throw new Exception("Not enough points to perform interpolation.");
            }


            closestPoints = closestPoints.OrderBy(p => p.X).ThenBy(p => p.Y).ToArray();
            Vec3 p1 = closestPoints[0];
            Vec3 p2 = closestPoints[1];
            Vec3 p3 = closestPoints[2];
            Vec3 p4 = closestPoints[3];


            float denom = (p2.X - p1.X) * (p4.Y - p1.Y);
            if (denom == 0)
            {
                return closestPoints.OrderBy(p => Math.Sqrt(Math.Pow(v1 - p.X, 2) + Math.Pow(v2 - p.Y, 2))).First().Z;
            }
            float z = (1 / denom) * (p1.Z * (p2.X - v1) * (p4.Y - v2) +
                                     p2.Z * (v1 - p1.X) * (p4.Y - v2) +
                                     p3.Z * (p2.X - v1) * (v2 - p1.Y) +
                                     p4.Z * (v1 - p1.X) * (v2 - p1.Y));

            return z;

        }
    }
}
