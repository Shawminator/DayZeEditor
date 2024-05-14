using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace DayZeLib
{
    public class DrJonesvehicleParts
    {
        public BindingList<VehicleParts> Vehicles { get; set; }
        public string Filename { get; set; }
        public bool isDirty { get; set; }

        public DrJonesvehicleParts(string filename)
        {
            Filename = filename;
            Console.WriteLine("Parsing " + Path.GetFileName(filename) + "....");
            Vehicles = new BindingList<VehicleParts>();
            isDirty = false;
            using (StreamReader reader = new StreamReader(Filename))
            {
                bool skipLine = false;
                int vehicleCounter = 0;
                string line_content = "";
                while (vehicleCounter <= 5000 && line_content.Contains("<FileEnd>") == false)
                {
                    // Get Vehicle Name Entrys:
                    if (!skipLine)
                        line_content = Helper.SearchForNextTermsInFile(reader, new string[] { "<VehicleParts>", "<OpenFile>" }, "<FileEnd>");
                    else
                        skipLine = false;
                    if (line_content.Contains("<OpenFile>"))
                    {
                        //need to see about this
                        continue;
                    }
                    if (!line_content.Contains("<VehicleParts>"))
                        continue;
                    line_content = line_content.Replace("<VehicleParts>", "");
                    line_content = Helper.TrimComment(line_content);
                    line_content = Helper.TrimSpaces(line_content);
                    Console.WriteLine("Vehicle Found..... " + line_content);

                    VehicleParts VParts = new VehicleParts();
                    VParts.ClasssName = line_content;

                    int char_count = 0;
                    int vehiclePartsCounter = 0;
                    while (true)
                    {
                        // Get Vehicle Parts Entrys:
                        line_content = reader.ReadLine();
                        char_count = line_content.Count();
                        line_content = Helper.TrimComment(line_content);
                        line_content = Helper.TrimSpaces(line_content);
                        if (line_content == "")
                            continue;
                        if (line_content.Contains("<OpenFile>"))
                        {
                            //need to see about this
                            continue;
                        }
                        if (line_content.Contains("<VehicleParts>"))
                        {
                            skipLine = true;
                            break;
                        }
                        if (line_content.Contains("<FileEnd>") || char_count == -1 || vehiclePartsCounter > 5000)
                        {
                            line_content = "<FileEnd>";
                            break;
                        }
                        VParts.AddPart(line_content);
                        Console.WriteLine("Adding Part ..... " + line_content);
                        vehiclePartsCounter++;
                    }
                    vehicleCounter++;
                    Vehicles.Add(VParts);
                }
            }
        }
        public bool saveVehicleParts(string SaveTime)
        {
            if (!isDirty) return false;
            StringBuilder sb = new StringBuilder();
            foreach (VehicleParts Vehicles in Vehicles)
            {
                sb.Append("<VehicleParts> " + Vehicles.ClasssName + Environment.NewLine);
                foreach (string vp in Vehicles.Parts)
                {
                    sb.Append("\t" + vp + Environment.NewLine);
                }
                sb.Append(Environment.NewLine);
            }
            sb.Append("<FileEnd> // This has to be on the End of this File and is very importand!");
            Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + SaveTime);
            File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileName(Filename), true);
            File.WriteAllText(Filename, sb.ToString());
            isDirty = false;
            return true;
        }

    }
    public class VehicleParts
    {
        public string ClasssName { get; set; }
        public BindingList<string> Parts { get; set; }

        public void AddPart(string partname)
        {
            if (Parts == null)
                Parts = new BindingList<string>();
            Parts.Add(partname);
        }
        public void removepart(string partname)
        {
            Parts.Remove(partname);
        }
        public override string ToString()
        {
            return ClasssName;
        }
    }
}
