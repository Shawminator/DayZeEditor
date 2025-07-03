using System.ComponentModel;
using System.IO;

namespace DayZeLib
{
    public class TerjeProtectionIDs
    {
        public string Filename { get; set; }
        public string Filetype { get; set; }
        public bool isDirty { get; set; }
        public BindingList<string> IDList { get; set; }

        public TerjeProtectionIDs(string fullName)
        {
            Filename = fullName;
            Filetype = Path.GetFileNameWithoutExtension(fullName);
            string[] lines = File.ReadAllLines(Filename);
            IDList = new BindingList<string>();
            foreach (string line in lines)
            {
                if(line.StartsWith("//")) continue;
                if (line == "") continue;
                IDList.Add(line);
            }
        }
    }
}
