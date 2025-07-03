using System.ComponentModel;
using System.Xml.Serialization;

namespace DayZeLib
{
    [XmlRoot("Loadouts")]
    public partial class TerjeLoadouts
    {
        [XmlIgnore]
        public string Filename { get; set; }
        [XmlIgnore]
        public bool isDirty { get; set; }


        private BindingList<TerjeLoadout> loadoutField;

        [XmlElementAttribute("Loadout")]
        public BindingList<TerjeLoadout> Loadout
        {
            get
            {
                return this.loadoutField;
            }
            set
            {
                this.loadoutField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeLoadout
    {
        //Elelments
        private TerjeLoadoutItems itemsField;
        private TerjeConditions conditionsField;

        //Attributes
        private string idField;
        private string displayNameField;

        public TerjeLoadoutItems Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
        public TerjeConditions Conditions
        {
            get
            {
                return this.conditionsField;
            }
            set
            {
                this.conditionsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string displayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
            }
        }
    }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeLoadoutItems
    {
        //elements
        private BindingList<object> itemsField;

        [System.Xml.Serialization.XmlElementAttribute("Item", typeof(TerjeLoadoutItem))]
        [System.Xml.Serialization.XmlElementAttribute("Selector", typeof(TerjeLoadoutSelector))]
        public BindingList<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeLoadoutItem
    {
        // Element
        private BindingList<TerjeLoadoutItem> itemField;

        // Attributes - private fields
        private string classnameField;
        private bool classnameFieldSpecified;

        private string displayNameField;
        private bool displayNameFieldSpecified;

        private string quantityField;
        private bool quantityFieldSpecified;

        private string countField;
        private bool countFieldSpecified;

        private string healthField;
        private bool healthFieldSpecified;

        private string positionField;
        private bool positionFieldSpecified;

        private string liquidField;
        private bool liquidFieldSpecified;

        private string temperatureField;
        private bool temperatureFieldSpecified;

        private string foodStageField;
        private bool foodStageFieldSpecified;

        private int disinfectedField;
        private bool disinfectedFieldSpecified;

        private string agentsField;
        private bool agentsFieldSpecified;

        private int quickbarField;
        private bool quickbarFieldSpecified;

        private string ammoTypeField;
        private bool ammoTypeFieldSpecified;

        private string ammoCountField;
        private bool ammoCountFieldSpecified;

        private int costField;
        private bool costFieldSpecified;

        // Properties

        [System.Xml.Serialization.XmlElementAttribute("Item")]
        public BindingList<TerjeLoadoutItem> Item
        {
            get { return this.itemField; }
            set { this.itemField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classname
        {
            get { return this.classnameField; }
            set { this.classnameField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool classnameSpecified
        {
            get { return this.classnameFieldSpecified; }
            set { this.classnameFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string displayName
        {
            get { return this.displayNameField; }
            set { this.displayNameField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool displayNameSpecified
        {
            get { return this.displayNameFieldSpecified; }
            set { this.displayNameFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string quantity
        {
            get { return this.quantityField; }
            set { this.quantityField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool quantitySpecified
        {
            get { return this.quantityFieldSpecified; }
            set { this.quantityFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string count
        {
            get { return this.countField; }
            set { this.countField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool countSpecified
        {
            get { return this.countFieldSpecified; }
            set { this.countFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string health
        {
            get { return this.healthField; }
            set { this.healthField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool healthSpecified
        {
            get { return this.healthFieldSpecified; }
            set { this.healthFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string position
        {
            get { return this.positionField; }
            set { this.positionField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool positionSpecified
        {
            get { return this.positionFieldSpecified; }
            set { this.positionFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string liquid
        {
            get { return this.liquidField; }
            set { this.liquidField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool liquidSpecified
        {
            get { return this.liquidFieldSpecified; }
            set { this.liquidFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string temperature
        {
            get { return this.temperatureField; }
            set { this.temperatureField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool temperatureSpecified
        {
            get { return this.temperatureFieldSpecified; }
            set { this.temperatureFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string foodStage
        {
            get { return this.foodStageField; }
            set { this.foodStageField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool foodStageSpecified
        {
            get { return this.foodStageFieldSpecified; }
            set { this.foodStageFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int disinfected
        {
            get { return this.disinfectedField; }
            set { this.disinfectedField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool disinfectedSpecified
        {
            get { return this.disinfectedFieldSpecified; }
            set { this.disinfectedFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agents
        {
            get { return this.agentsField; }
            set { this.agentsField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool agentsSpecified
        {
            get { return this.agentsFieldSpecified; }
            set { this.agentsFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int quickbar
        {
            get { return this.quickbarField; }
            set { this.quickbarField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool quickbarSpecified
        {
            get { return this.quickbarFieldSpecified; }
            set { this.quickbarFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ammoType
        {
            get { return this.ammoTypeField; }
            set { this.ammoTypeField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool ammoTypeSpecified
        {
            get { return this.ammoTypeFieldSpecified; }
            set { this.ammoTypeFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ammoCount
        {
            get { return this.ammoCountField; }
            set { this.ammoCountField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool ammoCountSpecified
        {
            get { return this.ammoCountFieldSpecified; }
            set { this.ammoCountFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int cost
        {
            get { return this.costField; }
            set { this.costField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool costSpecified
        {
            get { return this.costFieldSpecified; }
            set { this.costFieldSpecified = value; }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeLoadoutSelector
    {
        private BindingList<TerjeLoadoutItem> itemField;
        private BindingList<TerjeLoadoutGroup> groupField;
        private string typeField;
        private string displayNameField;
        private int pointsCountField;
        private bool pointsCountFieldSpecified;
        private string pointsHandlerField;
        private string pointsIconField;

        [System.Xml.Serialization.XmlElementAttribute("Item")]
        public BindingList<TerjeLoadoutItem> Item
        {
            get { return this.itemField; }
            set { this.itemField = value; }
        }

        [System.Xml.Serialization.XmlElementAttribute("Group")]
        public BindingList<TerjeLoadoutGroup> Group
        {
            get { return this.groupField; }
            set { this.groupField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get { return this.typeField; }
            set { this.typeField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string displayName
        {
            get { return this.displayNameField; }
            set { this.displayNameField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int pointsCount
        {
            get { return this.pointsCountField; }
            set { this.pointsCountField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pointsCountSpecified
        {
            get { return this.pointsCountFieldSpecified; }
            set { this.pointsCountFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public string pointsHandler
        {
            get { return this.pointsHandlerField; }
            set { this.pointsHandlerField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public string pointsIcon
        {
            get { return this.pointsIconField; }
            set { this.pointsIconField = value; }
        }
    }


    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeLoadoutGroup
    {
        private BindingList<TerjeLoadoutItem> itemField;
        private int costField;
        private bool costFieldSpecified;

        [System.Xml.Serialization.XmlElementAttribute("Item")]
        public BindingList<TerjeLoadoutItem> Item
        {
            get { return this.itemField; }
            set { this.itemField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int cost
        {
            get { return this.costField; }
            set { this.costField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool costSpecified
        {
            get { return this.costFieldSpecified; }
            set { this.costFieldSpecified = value; }
        }
    }


    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeConditions
    {
        private TerjeTimeout timeoutField;
        private TerjeSkillLevel skillLevelField;
        private TerjeSkillPerk skillPerkField;
        private TerjeSpecificPlayers specificPlayersField;
        private TerjeCustomCondition CustomConditionField;

        public TerjeTimeout Timeout
        {
            get
            {
                return this.timeoutField;
            }
            set
            {
                this.timeoutField = value;
            }
        }
        public TerjeSkillLevel SkillLevel
        {
            get
            {
                return this.skillLevelField;
            }
            set
            {
                this.skillLevelField = value;
            }
        }
        public TerjeSkillPerk SkillPerk
        {
            get
            {
                return this.skillPerkField;
            }
            set
            {
                this.skillPerkField = value;
            }
        }
        public TerjeSpecificPlayers SpecificPlayers
        {
            get
            {
                return this.specificPlayersField;
            }
            set
            {
                this.specificPlayersField = value;
            }
        }
        public TerjeCustomCondition CustomCondition
        {
            get
            {
                return this.CustomConditionField;
            }
            set
            {
                this.CustomConditionField = value;
            }
        }
    }

    public partial class TerjeTimeout
    {
        private string idField;
        private int hoursField;
        private bool hoursFieldSpecified;
        private int minutesField;
        private bool minutesFieldSpecified;
        private int secondsField;
        private bool secondsFieldSpecified;
        private int hideOwnerWhenFalseField;
        private bool hideOwnerWhenFalseFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int hours
        {
            get { return this.hoursField; }
            set { this.hoursField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hoursSpecified
        {
            get { return this.hoursFieldSpecified; }
            set { this.hoursFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int minutes
        {
            get { return this.minutesField; }
            set { this.minutesField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minutesSpecified
        {
            get { return this.minutesFieldSpecified; }
            set { this.minutesFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int seconds
        {
            get { return this.secondsField; }
            set { this.secondsField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool secondsSpecified
        {
            get { return this.secondsFieldSpecified; }
            set { this.secondsFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int hideOwnerWhenFalse
        {
            get { return this.hideOwnerWhenFalseField; }
            set { this.hideOwnerWhenFalseField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hideOwnerWhenFalseSpecified
        {
            get { return this.hideOwnerWhenFalseFieldSpecified; }
            set { this.hideOwnerWhenFalseFieldSpecified = value; }
        }
    }


    public partial class TerjeSkillLevel
    {
        private string skillIdField;
        private int requiredLevelField;
        private int hideOwnerWhenFalseField;
        private bool hideOwnerWhenFalseFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string skillId
        {
            get { return this.skillIdField; }
            set { this.skillIdField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int requiredLevel
        {
            get { return this.requiredLevelField; }
            set { this.requiredLevelField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int hideOwnerWhenFalse
        {
            get { return this.hideOwnerWhenFalseField; }
            set { this.hideOwnerWhenFalseField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hideOwnerWhenFalseSpecified
        {
            get { return this.hideOwnerWhenFalseFieldSpecified; }
            set { this.hideOwnerWhenFalseFieldSpecified = value; }
        }
    }


    public partial class TerjeSkillPerk
    {
        private string skillIdField;
        private string perkIdField;
        private int requiredLevelField;
        private int hideOwnerWhenFalseField;
        private bool hideOwnerWhenFalseFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string skillId
        {
            get { return this.skillIdField; }
            set { this.skillIdField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string perkId
        {
            get { return this.perkIdField; }
            set { this.perkIdField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int requiredLevel
        {
            get { return this.requiredLevelField; }
            set { this.requiredLevelField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int hideOwnerWhenFalse
        {
            get { return this.hideOwnerWhenFalseField; }
            set { this.hideOwnerWhenFalseField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hideOwnerWhenFalseSpecified
        {
            get { return this.hideOwnerWhenFalseFieldSpecified; }
            set { this.hideOwnerWhenFalseFieldSpecified = value; }
        }
    }


    public partial class TerjeSpecificPlayers
    {
        private BindingList<TerjeSpecificPlayer> specificPlayerField;
        private int hideOwnerWhenFalseField;
        private bool hideOwnerWhenFalseFieldSpecified;

        [System.Xml.Serialization.XmlElementAttribute("SpecificPlayer")]
        public BindingList<TerjeSpecificPlayer> SpecificPlayer
        {
            get { return this.specificPlayerField; }
            set { this.specificPlayerField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int hideOwnerWhenFalse
        {
            get { return this.hideOwnerWhenFalseField; }
            set { this.hideOwnerWhenFalseField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hideOwnerWhenFalseSpecified
        {
            get { return this.hideOwnerWhenFalseFieldSpecified; }
            set { this.hideOwnerWhenFalseFieldSpecified = value; }
        }
    }

    public partial class TerjeSpecificPlayer
    {
        private string steamGUIDField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string steamGUID
        {
            get { return this.steamGUIDField; }
            set { this.steamGUIDField = value; }
        }
    }


    public partial class TerjeCustomCondition
    {
        private string classnameField;
        private int hideOwnerWhenFalseField;
        private bool hideOwnerWhenFalseFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classname
        {
            get { return this.classnameField; }
            set { this.classnameField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int hideOwnerWhenFalse
        {
            get { return this.hideOwnerWhenFalseField; }
            set { this.hideOwnerWhenFalseField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hideOwnerWhenFalseSpecified
        {
            get { return this.hideOwnerWhenFalseFieldSpecified; }
            set { this.hideOwnerWhenFalseFieldSpecified = value; }
        }
    }

}
