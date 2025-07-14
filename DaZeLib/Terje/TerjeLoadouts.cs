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
        private TerjeConditions ConditionsField;

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

        private string handlerField;
        private bool handlerFieldSpecified;

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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string handler
        {
            get { return this.handlerField; }
            set { this.handlerField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool handlerSpecified
        {
            get { return this.handlerFieldSpecified; }
            set { this.handlerFieldSpecified = value; }
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
        private bool displayNameFieldSepcified;
        private int pointsCountField;
        private bool pointsCountFieldSpecified;
        private string pointsHandlerField;
        private bool pointsHandlerFieldSpecified;
        private string pointsIconField;
        private bool pointsIconFielddSpecified;

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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool displayNameSpecified
        {
            get { return this.displayNameFieldSepcified; }
            set { this.displayNameFieldSepcified = value; }
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pointsHandler
        {
            get { return this.pointsHandlerField; }
            set { this.pointsHandlerField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pointsHandlerSpecified
        {
            get { return this.pointsHandlerFieldSpecified; }
            set { this.pointsHandlerFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pointsIcon
        {
            get { return this.pointsIconField; }
            set { this.pointsIconField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pointsIconSpecified
        {
            get { return this.pointsIconFielddSpecified; }
            set { this.pointsIconFielddSpecified = value; }
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
        private string handlerField;
        private bool handlerFieldSpecified;

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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string handler
        {
            get { return this.handlerField; }
            set { this.handlerField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool handlerSpecified
        {
            get { return this.handlerFieldSpecified; }
            set { this.handlerFieldSpecified = value; }
        }
    }




}
