using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DayZeLib
{
    [XmlRoot("Respawns")]
    public partial class TerjeRespawns
    {
        [XmlIgnore]
        public string Filename { get; set; }
        [XmlIgnore]
        public bool isDirty { get; set; }

        private BindingList<TerjeRespawn> respawnField;

        [XmlElementAttribute("Respawn")]
        public BindingList<TerjeRespawn> Respawn
        {
            get
            {
                return this.respawnField;
            }
            set
            {
                this.respawnField = value;
            }
        }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeRespawn
    {
        private string idField;
        private string displayNameField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get => idField;
            set => idField = value;
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string displayName
        {
            get => displayNameField;
            set => displayNameField = value;
        }


        private BindingList<TerjeRespawnPoint> pointsField;
        private BindingList<TerjeRespawnObject> objectsField;
        private TerjeDeathPoint DeathPointField;
        private TerjeRespawnOptions optionsField;
        private TerjeConditions ConditionsField;

        [System.Xml.Serialization.XmlArrayItemAttribute("Point")]
        public BindingList<TerjeRespawnPoint> Points
        {
            get => pointsField;
            set => pointsField = value;
        }
        [System.Xml.Serialization.XmlArrayItemAttribute("Object")]
        public BindingList<TerjeRespawnObject> Objects
        {
            get => objectsField;
            set => objectsField = value;
        }
        [System.Xml.Serialization.XmlElementAttribute("DeathPoint")]
        public TerjeDeathPoint DeathPoint
        {
            get => DeathPointField;
            set => DeathPointField = value;
        }
        [System.Xml.Serialization.XmlElementAttribute("Options")]
        public TerjeRespawnOptions Options
        {
            get => optionsField;
            set => optionsField = value;
        }
        [System.Xml.Serialization.XmlElementAttribute("Conditions")]
        public TerjeConditions Conditions
        {
            get
            {
                return this.ConditionsField;
            }
            set
            {
                this.ConditionsField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeRespawnPoint
    {
        private string posField;
        private bool posFieldSpecified;
        private decimal xField;
        private bool xFieldSpecified;
        private decimal yField;
        private bool yFieldSpecified;
        private decimal zField;
        private bool zFieldSpecified;
        private decimal angleField;
        private bool angleFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pos
        {
            get => posField;
            set => posField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool posSpecified
        {
            get => posFieldSpecified;
            set => posFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal x
        {
            get => xField;
            set => xField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool xSpecified
        {
            get => xFieldSpecified;
            set => xFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal y
        {
            get => yField;
            set => yField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ySpecified
        {
            get => yFieldSpecified;
            set => yFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal z
        {
            get => zField;
            set => zField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool zSpecified
        {
            get => zFieldSpecified;
            set => zFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal angle
        {
            get => angleField;
            set => angleField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool angleSpecified
        {
            get => angleFieldSpecified;
            set => angleFieldSpecified = value;
        }
    }



    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeRespawnObject
    {
        private string classnameField;
        private int singleBindField;
        private bool singleBindFieldSpecified;
        private string handlerField;
        private bool handlerFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classname
        {
            get => classnameField;
            set => classnameField = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int singleBind
        {
            get => singleBindField;
            set => singleBindField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool singleBindSpecified
        {
            get => singleBindFieldSpecified;
            set => singleBindFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string handler
        {
            get => handlerField;
            set => handlerField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool handlerSpecified
        {
            get => handlerFieldSpecified;
            set => handlerFieldSpecified = value;
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeDeathPoint
    {
        private int requireBodyField;
        private bool requireBodyFieldSpecified;
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int requireBody
        {
            get => requireBodyField;
            set => requireBodyField = value;
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool requireBodySpecified
        {
            get => requireBodyFieldSpecified;
            set => requireBodyFieldSpecified = value;
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeRespawnOptions
    {
        private TerjeSafeRadius safeRadiusField;
        private TerjeMapImage mapImageField;
        private TerjeMapRender mapRenderField;
        private TerjePlayerStats playerStatsField;

        public TerjeSafeRadius SafeRadius
        {
            get
            {
                return this.safeRadiusField;
            }
            set
            {
                this.safeRadiusField = value;
            }
        }
        public TerjeMapImage MapImage
        {
            get
            {
                return this.mapImageField;
            }
            set
            {
                this.mapImageField = value;
            }
        }
        public TerjeMapRender MapRender
        {
            get
            {
                return this.mapRenderField;
            }
            set
            {
                this.mapRenderField = value;
            }
        }
        public TerjePlayerStats PlayerStats
        {
            get
            {
                return this.playerStatsField;
            }
            set
            {
                this.playerStatsField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeSafeRadius
    {
        private int zombieField;
        private bool zombieFieldSpecified;
        private int animalField;
        private bool animalFieldSpecified;
        private int playerField;
        private bool playerFieldSpecified;
        private int otherField;
        private bool otherFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int zombie
        {
            get => zombieField;
            set => zombieField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool zombieSpecified
        {
            get => zombieFieldSpecified;
            set => zombieFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int animal
        {
            get => animalField;
            set => animalField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool animalSpecified
        {
            get => animalFieldSpecified;
            set => animalFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int player
        {
            get => playerField;
            set => playerField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool playerSpecified
        {
            get => playerFieldSpecified;
            set => playerFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int other
        {
            get => otherField;
            set => otherField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool otherSpecified
        {
            get => otherFieldSpecified;
            set => otherFieldSpecified = value;
        }
    }


    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeMapImage
    {
        private string pathField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string path
        {
            get { return this.pathField; }
            set { this.pathField = value; }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeMapRender
    {
        private string posField;
        private bool posFieldSpecified;

        private decimal xField;
        private bool xFieldSpecified;

        private decimal yField;
        private bool yFieldSpecified;

        private decimal zField;
        private bool zFieldSpecified;

        private float zoomField;
        private bool zoomFieldSpecified;

        private string showPointsField;
        private bool showPointsFieldSpecified;

        private string showMarkerField;
        private bool showMarkerFieldSpecified;

        private int showMarkerNameField;
        private bool showMarkerNameFieldSpecified;

        private int allowInteractionField;
        private bool allowInteractionFieldSpecified;

        private string markerPathField;
        private bool markerPathFieldSpecified;

        private string pointsPathField;
        private bool pointsPathFieldSpecified;

        private string activePointsColorField;
        private bool activePointsColorFieldSpecified;

        private string inactivePointsColorField;
        private bool inactivePointsColorFieldSpecified;

        private string activeMarkerColorField;
        private bool activeMarkerColorFieldSpecified;

        private string inactiveMarkerColorField;
        private bool inactiveMarkerColorFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pos
        {
            get => posField;
            set => posField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool posSpecified
        {
            get => posFieldSpecified;
            set => posFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal x
        {
            get => xField;
            set => xField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool xSpecified
        {
            get => xFieldSpecified;
            set => xFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal y
        {
            get => yField;
            set => yField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ySpecified
        {
            get => yFieldSpecified;
            set => yFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal z
        {
            get => zField;
            set => zField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool zSpecified
        {
            get => zFieldSpecified;
            set => zFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public float zoom
        {
            get => zoomField;
            set => zoomField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool zoomSpecified
        {
            get => zoomFieldSpecified;
            set => zoomFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string showPoints
        {
            get => showPointsField;
            set => showPointsField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool showPointsSpecified
        {
            get => showPointsFieldSpecified;
            set => showPointsFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string showMarker
        {
            get => showMarkerField;
            set => showMarkerField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool showMarkerSpecified
        {
            get => showMarkerFieldSpecified;
            set => showMarkerFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int showMarkerName
        {
            get => showMarkerNameField;
            set => showMarkerNameField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool showMarkerNameSpecified
        {
            get => showMarkerNameFieldSpecified;
            set => showMarkerNameFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int allowInteraction
        {
            get => allowInteractionField;
            set => allowInteractionField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool allowInteractionSpecified
        {
            get => allowInteractionFieldSpecified;
            set => allowInteractionFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string markerPath
        {
            get => markerPathField;
            set => markerPathField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool markerPathSpecified
        {
            get => markerPathFieldSpecified;
            set => markerPathFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pointsPath
        {
            get => pointsPathField;
            set => pointsPathField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pointsPathSpecified
        {
            get => pointsPathFieldSpecified;
            set => pointsPathFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string activePointsColor
        {
            get => activePointsColorField;
            set => activePointsColorField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool activePointsColorSpecified
        {
            get => activePointsColorFieldSpecified;
            set => activePointsColorFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string inactivePointsColor
        {
            get => inactivePointsColorField;
            set => inactivePointsColorField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool inactivePointsColorSpecified
        {
            get => inactivePointsColorFieldSpecified;
            set => inactivePointsColorFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string activeMarkerColor
        {
            get => activeMarkerColorField;
            set => activeMarkerColorField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool activeMarkerColorSpecified
        {
            get => activeMarkerColorFieldSpecified;
            set => activeMarkerColorFieldSpecified = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string inactiveMarkerColor
        {
            get => inactiveMarkerColorField;
            set => inactiveMarkerColorField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool inactiveMarkerColorSpecified
        {
            get => inactiveMarkerColorFieldSpecified;
            set => inactiveMarkerColorFieldSpecified = value;
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjePlayerStats
    {
        private int healthField;
        private bool healthFieldSpecified;
        private int bloodField;
        private bool bloodFieldSpecified;
        private int shockField;
        private bool shockFieldSpecified;
        private int energyField;
        private bool energyFieldSpecified;
        private int waterField;
        private bool waterFieldSpecified;
        private int sleepField;
        private bool sleepFieldSpecified;
        private int mindField;
        private bool mindFieldSpecified;
        private int heatComfortField;
        private bool heatComfortFieldSpecified;
        private int heatBufferField;
        private bool heatBufferFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int health
        {
            get { return this.healthField; }
            set { this.healthField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool healthSpecified
        {
            get { return this.healthFieldSpecified; }
            set { this.healthFieldSpecified = value; }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int blood
        {
            get { return this.bloodField; }
            set { this.bloodField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool bloodSpecified
        {
            get { return this.bloodFieldSpecified; }
            set { this.bloodFieldSpecified = value; }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int shock
        {
            get { return this.shockField; }
            set { this.shockField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool shockSpecified
        {
            get { return this.shockFieldSpecified; }
            set { this.shockFieldSpecified = value; }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int energy
        {
            get { return this.energyField; }
            set { this.energyField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool energySpecified
        {
            get { return this.energyFieldSpecified; }
            set { this.energyFieldSpecified = value; }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int water
        {
            get { return this.waterField; }
            set { this.waterField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool waterSpecified
        {
            get { return this.waterFieldSpecified; }
            set { this.waterFieldSpecified = value; }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int sleep
        {
            get { return this.sleepField; }
            set { this.sleepField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sleepSpecified
        {
            get { return this.sleepFieldSpecified; }
            set { this.sleepFieldSpecified = value; }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int mind
        {
            get { return this.mindField; }
            set { this.mindField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool mindSpecified
        {
            get { return this.mindFieldSpecified; }
            set { this.mindFieldSpecified = value; }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int heatComfort
        {
            get { return this.heatComfortField; }
            set { this.heatComfortField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool heatComfortSpecified
        {
            get { return this.heatComfortFieldSpecified; }
            set { this.heatComfortFieldSpecified = value; }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int heatBuffer
        {
            get { return this.heatBufferField; }
            set { this.heatBufferField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool heatBufferSpecified
        {
            get { return this.heatBufferFieldSpecified; }
            set { this.heatBufferFieldSpecified = value; }
        }
    }
}
