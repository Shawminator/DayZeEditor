using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class DZE
    {
        public string MapName { get; set; }
        public float[] CameraPosition { get; set; }
        public List<Editorobject> EditorObjects { get; set; }
        public List<object> EditorDeletedObjects { get; set; }

       public DZE()
        {
            EditorObjects = new List<Editorobject>();
            EditorDeletedObjects = new List<object>();
        }
    }

    public class Editorobject
    {
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public float[] Position { get; set; }
        public float[] Orientation { get; set; }
        public float Scale { get; set; }
        public int Flags { get; set; }

        public Editorobject()
        {
            Scale = 1;
            Flags = 2147483647;
        }
    }

}
