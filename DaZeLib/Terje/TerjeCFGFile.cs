using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DayZeLib
{
    public class TerjeCFGFIle
    {
        public string Filename { get; set; }
        public bool isDirty { get; set; }
        public BindingList<TerjeCFGLine> CFGCOntents { get; set; }

        public TerjeCFGFIle() 
        { 
        }
        public void Read(string filename)
        {
            string[] FileContents = File.ReadAllLines(Filename);
            CFGCOntents = new BindingList<TerjeCFGLine>();
            foreach (string cfgline in FileContents)
            {
                if (string.IsNullOrWhiteSpace(cfgline))
                    continue;
                CFGCOntents.Add(new TerjeCFGLine(cfgline));
            }
        }
        public string[] CreateStringArray()
        {
            List<string> FileContents = new List<string>();
            foreach(TerjeCFGLine line in CFGCOntents)
            {
                FileContents.Add(line.Formatline());
            }
            return FileContents.ToArray();
        }
    }
}
