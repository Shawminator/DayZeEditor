using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class playerspawnpoints
    {

        private playerspawnpointsFresh freshField;

        private playerspawnpointsHop hopField;

        private playerspawnpointsTravel travelField;

        /// <remarks/>
        public playerspawnpointsFresh fresh
        {
            get
            {
                return this.freshField;
            }
            set
            {
                this.freshField = value;
            }
        }

        /// <remarks/>
        public playerspawnpointsHop hop
        {
            get
            {
                return this.hopField;
            }
            set
            {
                this.hopField = value;
            }
        }

        /// <remarks/>
        public playerspawnpointsTravel travel
        {
            get
            {
                return this.travelField;
            }
            set
            {
                this.travelField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class playerspawnpointsFresh
    {

        private playerspawnpointsFreshSpawn_params spawn_paramsField;

        private playerspawnpointsFreshGenerator_params generator_paramsField;

        private BindingList<playerspawnpointsFreshPos> generator_posbubblesField;

        /// <remarks/>
        public playerspawnpointsFreshSpawn_params spawn_params
        {
            get
            {
                return this.spawn_paramsField;
            }
            set
            {
                this.spawn_paramsField = value;
            }
        }

        /// <remarks/>
        public playerspawnpointsFreshGenerator_params generator_params
        {
            get
            {
                return this.generator_paramsField;
            }
            set
            {
                this.generator_paramsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("pos", IsNullable = false)]
        public BindingList<playerspawnpointsFreshPos> generator_posbubbles
        {
            get
            {
                return this.generator_posbubblesField;
            }
            set
            {
                this.generator_posbubblesField = value;
            }
        }

    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class playerspawnpointsFreshSpawn_params
    {

        private decimal min_dist_infectedField;

        private decimal max_dist_infectedField;

        private decimal min_dist_playerField;

        private decimal max_dist_playerField;

        private decimal min_dist_staticField;

        private decimal max_dist_staticField;

        /// <remarks/>
        public decimal min_dist_infected
        {
            get
            {
                return this.min_dist_infectedField;
            }
            set
            {
                this.min_dist_infectedField = value;
            }
        }

        /// <remarks/>
        public decimal max_dist_infected
        {
            get
            {
                return this.max_dist_infectedField;
            }
            set
            {
                this.max_dist_infectedField = value;
            }
        }

        /// <remarks/>
        public decimal min_dist_player
        {
            get
            {
                return this.min_dist_playerField;
            }
            set
            {
                this.min_dist_playerField = value;
            }
        }

        /// <remarks/>
        public decimal max_dist_player
        {
            get
            {
                return this.max_dist_playerField;
            }
            set
            {
                this.max_dist_playerField = value;
            }
        }

        /// <remarks/>
        public decimal min_dist_static
        {
            get
            {
                return this.min_dist_staticField;
            }
            set
            {
                this.min_dist_staticField = value;
            }
        }

        /// <remarks/>
        public decimal max_dist_static
        {
            get
            {
                return this.max_dist_staticField;
            }
            set
            {
                this.max_dist_staticField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class playerspawnpointsFreshGenerator_params
    {

        private byte grid_densityField;

        private decimal grid_widthField;

        private decimal grid_heightField;

        private decimal min_dist_staticField;

        private decimal max_dist_staticField;

        private sbyte min_steepnessField;

        private byte max_steepnessField;

        /// <remarks/>
        public byte grid_density
        {
            get
            {
                return this.grid_densityField;
            }
            set
            {
                this.grid_densityField = value;
            }
        }

        /// <remarks/>
        public decimal grid_width
        {
            get
            {
                return this.grid_widthField;
            }
            set
            {
                this.grid_widthField = value;
            }
        }

        /// <remarks/>
        public decimal grid_height
        {
            get
            {
                return this.grid_heightField;
            }
            set
            {
                this.grid_heightField = value;
            }
        }

        /// <remarks/>
        public decimal min_dist_static
        {
            get
            {
                return this.min_dist_staticField;
            }
            set
            {
                this.min_dist_staticField = value;
            }
        }

        /// <remarks/>
        public decimal max_dist_static
        {
            get
            {
                return this.max_dist_staticField;
            }
            set
            {
                this.max_dist_staticField = value;
            }
        }

        /// <remarks/>
        public sbyte min_steepness
        {
            get
            {
                return this.min_steepnessField;
            }
            set
            {
                this.min_steepnessField = value;
            }
        }

        /// <remarks/>
        public byte max_steepness
        {
            get
            {
                return this.max_steepnessField;
            }
            set
            {
                this.max_steepnessField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class playerspawnpointsFreshPos
    {

        private float xField;

        private float zField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public float x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public float z
        {
            get
            {
                return this.zField;
            }
            set
            {
                this.zField = value;
            }
        }

        public override string ToString()
        {
            return "X:" + xField.ToString() + " , Z:" + zField.ToString();
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class playerspawnpointsHop
    {

        private BindingList<playerspawnpointsHopPos> generator_posbubblesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("pos", IsNullable = false)]
        public BindingList<playerspawnpointsHopPos> generator_posbubbles
        {
            get
            {
                return this.generator_posbubblesField;
            }
            set
            {
                this.generator_posbubblesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class playerspawnpointsHopPos
    {

        private float xField;

        private float zField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public float x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public float z
        {
            get
            {
                return this.zField;
            }
            set
            {
                this.zField = value;
            }
        }

        public override string ToString()
        {
            return "X:" + xField.ToString() + " , Z:" + zField.ToString();
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class playerspawnpointsTravel
    {

        private BindingList<playerspawnpointsTravelPos> generator_posbubblesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("pos", IsNullable = false)]
        public BindingList<playerspawnpointsTravelPos> generator_posbubbles
        {
            get
            {
                return this.generator_posbubblesField;
            }
            set
            {
                this.generator_posbubblesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class playerspawnpointsTravelPos
    {

        private float xField;

        private float zField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public float x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public float z
        {
            get
            {
                return this.zField;
            }
            set
            {
                this.zField = value;
            }
        }

        public override string ToString()
        {
            return "X:" + xField.ToString() + " , Z:" + zField.ToString();
        }
    }


}
