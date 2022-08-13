using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeLib
{
    public static class Helper
    {
        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        public static int getNextint(IEnumerable<int> ints)
        {
            int counter = ints.Count() > 0 ? ints.First() : 0;

            while (counter < int.MaxValue)
            {
                if (!ints.Contains(++counter)) return counter;
            }

            return -1;
        }
        public static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                string path = dirPath.Replace(sourcePath, targetPath);
                Directory.CreateDirectory(path);
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
        public static string CleanString(string _string, bool includespaces = false)
        {
            if (includespaces)
            {
                return Regex.Replace(_string, @"\s+", "");
            }
            else
            {
                return Regex.Replace(_string, @"\s+", " ");
            }
        }
        public static string removeComments(string _string)
        {
            if (_string.Contains("//"))
            {
                int index = _string.IndexOf("/");
                if (index > 0)
                    return _string.Substring(0, index).TrimEnd();
            }
            return _string;
        }
        public static string SearchForNextTermInFile(StreamReader reader, string searchTerm, string abortTerm)
        {
            int char_count = 0;
            while (char_count != -1)
            {
                string line_content = reader.ReadLine();
                char_count = line_content.Count();
                line_content = TrimComment(line_content);

                if (line_content.Contains(searchTerm) || (line_content.Contains(abortTerm) && abortTerm != ""))
                    return line_content;
            }
            return string.Empty;
        }

        public static float[] convertToRGBFloat(Color value)
        {
            return new float[] { value.R / 255.0F, value.G / 255.0f, value.B / 255.0f };
        }

        public static Color ConverToColor(float[] lightColorStart)
        {
            int R = (lightColorStart[0] >= 1.0 ? 255 : (lightColorStart[0] <= 0.0 ? 0 : (int)Math.Floor(lightColorStart[0] * 256.0)));
            int G = (lightColorStart[1] >= 1.0 ? 255 : (lightColorStart[1] <= 0.0 ? 0 : (int)Math.Floor(lightColorStart[1] * 256.0)));
            int B= (lightColorStart[2] >= 1.0 ? 255 : (lightColorStart[2] <= 0.0 ? 0 : (int)Math.Floor(lightColorStart[2] * 256.0)));
            return Color.FromArgb(R,G,B);
        }

        public static string SearchForNextTermsInFile(StreamReader reader, string[] searchTerms, string abortTerm)
        {
            int char_count = 0;
            while (char_count != -1)
            {
                string line_content = reader.ReadLine();
                if (line_content == null)
                    return string.Empty;
                char_count = line_content.Count();
                line_content = TrimComment(line_content);

                if (line_content.Contains(abortTerm) && abortTerm != "")
                    return line_content;
                for (int i = 0; i < searchTerms.Count(); i++)
                {
                    if (line_content.Contains(searchTerms[i]))
                        return line_content;
                }
            }
            return string.Empty;
        }
        public static string TrimComment(string line)
        {
            int to_substring_end = line.Length;

            for (int i = 0; i < to_substring_end; i++)
            {
                char sign = line[i];
                if (sign == '/' && i + 1 < to_substring_end)
                {
                    if (line[i + 1] == '/')
                    {
                        to_substring_end = i;
                        break;
                    }
                }
            }

            string lineOut = "";

            if (to_substring_end != 0)
                lineOut = line.Substring(0, to_substring_end);

            return TrimSpaces(lineOut);
        }
        public static string TrimSpaces(string line)
        {
            line.Replace("	", ""); // Replace Tabs("\t" or "	") with nothing.

            bool hasSpaces = true;
            while (hasSpaces)
            {
                line = line.Trim();

                if (line.Length > 0)
                    hasSpaces = line[0] == ' ' || line[line.Length - 1] == ' ';
                else
                    hasSpaces = false;
            }

            return line;
        }
        public static float PowCurveCalc(float minFrom, float maxFrom, float value, float minTo = 0, float maxTo = 1, float power = 1.0f)
        {
            value = Clamp((value - minFrom) / (maxFrom - minFrom), 0, 1); //! value must be equal to or between 0 and 1
            return (float)((1 - Math.Pow(1 - value, power)) * (maxTo - minTo) + minTo);
        }
        public static bool ContainsAllItems<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            return !b.Except(a).Any();
        }
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
        public static Bitmap MultiplyColorToBitmap(Bitmap sourceBitmap, Color color, int divisor, bool preserveAlfa)
        {
            if (sourceBitmap == null) return (null);
            int nPixels = sourceBitmap.Width * sourceBitmap.Height;
            Rectangle rect = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);

            int[] m_RawData = new int[nPixels];


            System.Drawing.Imaging.BitmapData bmpData = sourceBitmap.LockBits(rect,
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                sourceBitmap.PixelFormat);
            IntPtr ptrData = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(ptrData, m_RawData, 0, nPixels);
            sourceBitmap.UnlockBits(bmpData);

            //int maxr = 0;
            //int maxg = 0;
            //int maxb = 0;
            for (int k = 0; k < nPixels; k++)
            {

                Color mult = Color.FromArgb(m_RawData[k]);
                int a = mult.A;
                int r = (color.R * mult.R) / divisor;
                int g = (color.G * mult.G) / divisor;
                int b = (color.B * mult.B) / divisor;
                if (r > 255)
                    r = 255;
                if (g > 255)
                    g = 255;
                if (b > 255)
                    b = 255;
                // int a = (color.A * alfa) / 255;
                if (!preserveAlfa)
                {
                    a = 255;
                }
                m_RawData[k] = (int)((((((a << 8) | r) << 8) | g) << 8) | b);
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            rect = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);
            System.Drawing.Imaging.BitmapData bmpDest = resultBitmap.LockBits(rect,
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                resultBitmap.PixelFormat);
            // Get the address of the first line.
            IntPtr ptrDest = bmpDest.Scan0;

            // This code is specific to a bitmap with 32 bits per pixels.
            System.Runtime.InteropServices.Marshal.Copy(m_RawData, 0, ptrDest, nPixels);
            resultBitmap.UnlockBits(bmpDest);
            return (resultBitmap);
        }
        public static List<string> GetExpansionState(this TreeNodeCollection nodes)
        {
            return nodes.Descendants()
                        .Where(n => n.IsExpanded)
                        .Select(n => n.FullPath)
                        .ToList();
        }
        public static void SetExpansionState(this TreeNodeCollection nodes, List<string> savedExpansionState)
        {
            foreach (var node in nodes.Descendants().Where(n => savedExpansionState.Contains(n.FullPath)))
            {
                node.Expand();
            }
        }
        public static IEnumerable<TreeNode> Descendants(this TreeNodeCollection c)
        {
            foreach (var node in c.OfType<TreeNode>())
            {
                yield return node;

                foreach (var child in node.Nodes.Descendants())
                {
                    yield return child;
                }
            }
        }
        public static double ConvertMinutesToMilliseconds(double minutes)
        {
            return TimeSpan.FromMinutes(minutes).TotalMilliseconds;
        }
        public static double ConvertMillisecondsToMinutes(double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds).TotalMinutes;
        }
        public static double ConvertMinutesToSeconds(double minutes)
        {
            return TimeSpan.FromMinutes(minutes).TotalSeconds;
        }
        public static double ConvertSecondsToMinutes(double seconds)
        {
            return TimeSpan.FromSeconds(seconds).TotalMinutes;
        }
        public static List<string> GetPropertiesNameOfClass<T>(object pObject, string[] IgnoreNames = null, bool ignoreisspecific = false)
        {
            List<string> propertyList = new List<string>();
            if (pObject != null)
            {
                foreach (var prop in pObject.GetType().GetProperties())
                {
                    if(IgnoreNames == null)
                    {
                        if (prop.PropertyType == typeof(T))
                            propertyList.Add(prop.Name);
                    }
                    else if (ignoreisspecific == false)
                    {
                        if (prop.PropertyType == typeof(T) && !IgnoreNames.Contains(prop.Name))
                            propertyList.Add(prop.Name);
                    }
                    else if (ignoreisspecific == true)
                    {
                        if (prop.PropertyType == typeof(T) && IgnoreNames.Contains(prop.Name))
                            propertyList.Add(prop.Name);
                    }
                }
            }
            return propertyList;
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        public static void SetStringValue(object src, string mytype, string myvalue)
        {
            src.GetType().GetProperty(mytype).SetValue(src, myvalue, null);
        }
        public static void SetIntValue(object src, string mytype, int myvalue)
        {
            src.GetType().GetProperty(mytype).SetValue(src, myvalue, null);
        }
        public static void SetBoolValue(object src, string mytype, bool myvalue)
        {
            int myvalueasint = myvalue == true ? 1 : 0;
            src.GetType().GetProperty(mytype).SetValue(src, myvalueasint, null);
        }
        public static string ReadCString(BinaryReader br, int MaxLength = -1, long lOffset = -1, Encoding enc = null)
        {
            int Max;
            if (MaxLength == -1)
                Max = 255;
            else
                Max = MaxLength;

            long fTemp = br.BaseStream.Position;
            byte bTemp = 0;
            int i = 0;
            string result = "";

            if (lOffset > -1)
            {
                br.BaseStream.Seek(lOffset, SeekOrigin.Begin);
            }

            do
            {
                bTemp = br.ReadByte();
                if (bTemp == 0)
                    break;
                i += 1;
            } while (i < Max);

            if (MaxLength == -1)
                Max = i + 1;
            else
                Max = MaxLength;

            if (lOffset > -1)
            {
                br.BaseStream.Seek(lOffset, SeekOrigin.Begin);

                if (enc == null)
                    result = Encoding.ASCII.GetString(br.ReadBytes(i));
                else
                    result = enc.GetString(br.ReadBytes(i));

                br.BaseStream.Seek(fTemp, SeekOrigin.Begin);
            }
            else
            {
                br.BaseStream.Seek(fTemp, SeekOrigin.Begin);
                if (enc == null)
                    result = Encoding.ASCII.GetString(br.ReadBytes(i));
                else
                    result = enc.GetString(br.ReadBytes(i));

                br.BaseStream.Seek(fTemp + Max, SeekOrigin.Begin);
            }

            return result;
        }
    }
    public static class extenstions
    {
        private static Dictionary<Type, Action<Control>> controldefaults = new Dictionary<Type, Action<Control>>() {
            {typeof(TextBox), c => ((TextBox)c).Clear()},
            {typeof(CheckBox), c => ((CheckBox)c).Checked = false},
            {typeof(ListBox), c => ((ListBox)c).Items.Clear()},
            {typeof(RadioButton), c => ((RadioButton)c).Checked = false},
            {typeof(TreeView), c => ((TreeView)c).Nodes.Clear()},
            {typeof(NumericUpDown), c => ((NumericUpDown)c).Value = 0 }
        };
        private static void FindAndInvoke(Type type, Control control)
        {
            if (controldefaults.ContainsKey(type))
            {
                controldefaults[type].Invoke(control);
            }
        }
        public static void ClearControls(this Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                FindAndInvoke(control.GetType(), control);
            }
        }
        public static void ClearControls<T>(this Control.ControlCollection controls) where T : class
        {
            if (!controldefaults.ContainsKey(typeof(T))) return;

            foreach (Control control in controls)
            {
                if (control.GetType().Equals(typeof(T)))
                {
                    FindAndInvoke(typeof(T), control);
                }
            }

        }
    }
    public static class DZEHelpers
    {
        static string BIN_CHECK = "EditorBinned";
        public static bool IsBinnedFile(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                int Length = br.ReadInt32();
                if (Length == 12 && Helper.ReadCString(br, 12) == BIN_CHECK)
                    return true;
            }
            return false;
        }
        public static DZE LoadFile(string fileName)
        {
            if (IsBinnedFile(fileName))
                return ReadBinned (fileName);
            else
               return  ReadJson(fileName);
        }
        private static DZE ReadJson(string fileName)
        {
            return JsonSerializer.Deserialize<DZE>(File.ReadAllText(fileName));
        }
        private static DZE ReadBinned(string fileName)
        {
            return new DZE(fileName);
        }
    }
}
