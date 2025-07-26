using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DayZeLib
{
    [XmlRoot("Areas")]
    public class TerjeScriptableAreas
    {
        [XmlIgnore]
        public string Filename { get; set; }
        [XmlIgnore]
        public bool isDirty { get; set; }

        private BindingList<TerjeScriptableArea> areaField;

        [System.Xml.Serialization.XmlElementAttribute("Area")]
        public BindingList<TerjeScriptableArea> Area
        {
            get => areaField;
            set => areaField = value;
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeScriptableArea
    {
        private int activeField;
        private string classnameField;
        private Vec3 positionField;
        private decimal spawnChanceField;
        private string filterField;
        private bool filterFieldSpecified;
        private TerjeScriptableAreaData dataField;

        public int Active
        {
            get => activeField;
            set => activeField = value;
        }

        public string Classname
        {
            get => classnameField;
            set => classnameField = value;
        }

        public string Position
        {
            get => positionField.GetString();
            set => positionField = Vec3.Parse(value);
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public Vec3 PositionVec3
        {
            get => positionField;
            set => positionField = value;
        }

        public decimal SpawnChance
        {
            get => spawnChanceField;
            set => spawnChanceField = value;
        }

        public string Filter
        {
            get => filterField;
            set => filterField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FilterSpecified
        {
            get => filterFieldSpecified;
            set => filterFieldSpecified = value;
        }
        public TerjeScriptableAreaData Data
        {
            get => dataField;
            set => dataField = value;
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeScriptableAreaData
    {

        private int outerRadiusField;
        private bool outerRadiusFieldSpecified;
        private int innerRadiusField;
        private bool innerRadiusFieldSpecified;
        private int heightMinField;
        private int heightMaxField;
        private int radiusField;
        private bool radiusFieldSpecified;
        private decimal powerField;

        public int OuterRadius
        {
            get
            {
                return this.outerRadiusField;
            }
            set
            {
                this.outerRadiusField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OuterRadiusSpecified
        {
            get => outerRadiusFieldSpecified;
            set => outerRadiusFieldSpecified = value;
        }

        public int InnerRadius
        {
            get => innerRadiusField;
            set => innerRadiusField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InnerRadiusSpecified
        {
            get => innerRadiusFieldSpecified;
            set => innerRadiusFieldSpecified = value;
        }

        public int HeightMin
        {
            get => heightMinField;
            set => heightMinField = value;
        }

        public int HeightMax
        {
            get => heightMaxField;
            set => heightMaxField = value;
        }

        public int Radius
        {
            get => radiusField;
            set => radiusField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RadiusSpecified
        {
            get => radiusFieldSpecified;
            set => radiusFieldSpecified = value;
        }

        public decimal Power
        {
            get => powerField;
            set => powerField = value;
        }
    }




}
