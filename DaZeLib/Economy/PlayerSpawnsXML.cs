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
        private playerspawnpointssection freshField;
        private playerspawnpointssection hopField;
        private playerspawnpointssection travelField;

        /// <remarks/>
        public playerspawnpointssection fresh
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
        public playerspawnpointssection hop
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
        public playerspawnpointssection travel
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

        public playerspawnpoints()
        {
            fresh = new playerspawnpointssection();
            hop = new playerspawnpointssection();
            travel = new playerspawnpointssection();
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class playerspawnpointssection
    {
        private playerspawnpointsSpawn_params spawn_paramsField;
        private playerspawnpointsGenerator_params generator_paramsField;
        private playerspawnpointsGroup_params group_paramsField;
        private BindingList<playerspawnpointsGroup> generator_posbubblesField;

        /// <remarks/>
        public playerspawnpointsSpawn_params spawn_params
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
        public playerspawnpointsGenerator_params generator_params
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
        public playerspawnpointsGroup_params group_params
        {
            get
            {
                return this.group_paramsField;
            }
            set
            {
                this.group_paramsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("group", IsNullable = false)]
        public BindingList<playerspawnpointsGroup> generator_posbubbles
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

        public playerspawnpointssection()
        {
            spawn_params = new playerspawnpointsSpawn_params();
            generator_params = new playerspawnpointsGenerator_params();
            group_paramsField = new playerspawnpointsGroup_params();
            generator_posbubblesField = new BindingList<playerspawnpointsGroup>();
        }
    }
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class playerspawnpointsSpawn_params
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
    public partial class playerspawnpointsGenerator_params
    {
        private int grid_densityField;
        private int grid_widthField;
        private int grid_heightField;
        private int min_dist_staticField;
        private int max_dist_staticField;
        private int min_steepnessField;
        private int max_steepnessField;

        /// <remarks/>
        public int grid_density
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
        public int grid_width
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
        public int grid_height
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
        public int min_dist_static
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
        public int max_dist_static
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
        public int min_steepness
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
        public int max_steepness
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
    public partial class playerspawnpointsGroup_params
    {
        private bool enablegroupsField;
        private int lifetimeField;
        private int counterField;

        /// <remarks/>
        public bool enablegroups
        {
            get
            {
                return this.enablegroupsField;
            }
            set
            {
                this.enablegroupsField = value;
            }
        }

        /// <remarks/>
        public int lifetime
        {
            get
            {
                return this.lifetimeField;
            }
            set
            {
                this.lifetimeField = value;
            }
        }

        /// <remarks/>
        public int counter
        {
            get
            {
                return this.counterField;
            }
            set
            {
                this.counterField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class playerspawnpointsGroup
    {
        private BindingList<playerspawnpointsGroupPos> posField;
        private string nameField;
        private int lifetimeField;
        private bool lifetimeFieldSpecified;
        private int counterField;
        private bool counterFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("pos")]
        public BindingList<playerspawnpointsGroupPos> pos
        {
            get
            {
                return this.posField;
            }
            set
            {
                this.posField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int lifetime
        {
            get
            {
                return this.lifetimeField;
            }
            set
            {
                this.lifetimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lifetimeSpecified
        {
            get
            {
                return this.lifetimeFieldSpecified;
            }
            set
            {
                this.lifetimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int counter
        {
            get
            {
                return this.counterField;
            }
            set
            {
                this.counterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool counterSpecified
        {
            get
            {
                return this.counterFieldSpecified;
            }
            set
            {
                this.counterFieldSpecified = value;
            }
        }

        public override string ToString()
        {
            return name;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class playerspawnpointsGroupPos
    {

        private decimal xField;
        private decimal zField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal x
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
        public decimal z
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
