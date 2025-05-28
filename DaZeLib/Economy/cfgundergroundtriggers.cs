using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class cfgundergroundtriggers
    {
        public BindingList<Trigger> Triggers { get; set; }

        public cfgundergroundtriggers()
        {
            Triggers = new BindingList<Trigger>();
        }
    }

    public class Trigger
    {
        public float[] Position { get; set; }
        public int[] Orientation { get; set; }
        public float[] Size { get; set; }
        public int EyeAccommodation { get; set; }
        public BindingList<Breadcrumb> Breadcrumbs { get; set; }
        public int InterpolationSpeed { get; set; }

        public Trigger()
        {

        }
    }

    public class Breadcrumb
    {
        public float[] Position { get; set; }
        public float EyeAccommodation { get; set; }
        public int UseRaycast { get; set; }
        public int Radius { get; set; }

        public Breadcrumb() 
        { 
        }
    }

}
