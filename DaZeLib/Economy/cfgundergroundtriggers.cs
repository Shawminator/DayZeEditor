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
        public decimal[] Position { get; set; }
        public decimal[] Orientation { get; set; }
        public decimal[] Size { get; set; }
        public decimal EyeAccommodation { get; set; }
        public BindingList<Breadcrumb> Breadcrumbs { get; set; }
        public decimal? InterpolationSpeed { get; set; }

        public Trigger()
        {

        }

        public string gettriggertype()
        {
            if (EyeAccommodation == 1 && Breadcrumbs.Count == 0)
            {
                if (InterpolationSpeed == null)
                    InterpolationSpeed = 1;
                return "Outer";
            }
            else if (EyeAccommodation == 0 && Breadcrumbs.Count == 0)
            {
                if (InterpolationSpeed == null)
                    InterpolationSpeed = 1;
                return "Inner";
            }
            else
            {
                InterpolationSpeed = null;
                return "Transition";
            }
        }
    }

    public class Breadcrumb
    {
        public decimal[] Position { get; set; }
        public decimal EyeAccommodation { get; set; }
        public int UseRaycast { get; set; }
        public decimal Radius { get; set; }

        public Breadcrumb() 
        { 
        }
        public string getbreadcrumbtype()
        {
            switch (EyeAccommodation)
            {
                case 0:
                    return "Inner";
                case 1:
                    return "Outer";
                default:
                    return "Transition";
            }
        }
    }

}
