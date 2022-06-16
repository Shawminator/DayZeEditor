using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeLib
{
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
        public Vec3() { }
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
                //string line1 = Helper.TrimSpaces(line);
                //line1 = line1.Replace(" ", ",");
                //int firstindex = line1.IndexOf(",");
                //int lastindex = line1.LastIndexOf(',');
                //StringBuilder sb = new StringBuilder(line);
                //sb[firstindex] = '|';
                //sb[lastindex] = '|';
                //line1 = sb.ToString();
                //line1 = line1.Replace(",", "");
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
                using (FileStream writeStream = new FileStream(savefiel.FileName + ".xyz", FileMode.Create))
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

            List<Vec3> test = points
                .OrderBy(x => Math.Abs(v1 - x.X))
                .ToList();
            List<Vec3> test2 = test
                .FindAll(x => x.X == test[0].X)
                .OrderBy(x => Math.Abs(v2 - x.Y))
                .ToList();
            return test2[0].Z;

        }
    }
}
