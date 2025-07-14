using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DayZeLib
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeConditionBase
    {
        private int hideOwnerWhenFalseField;
        private bool hideOwnerWhenFalseSpecifiedField;

        private string displayTextField;
        private bool displayTextSpecifiedField;

        private string successTextField;
        private bool successTextSpecifiedField;

        private string failTextField;
        private bool failTextSpecifiedField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int hideOwnerWhenFalse
        {
            get => hideOwnerWhenFalseField;
            set => hideOwnerWhenFalseField = value;
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hideOwnerWhenFalseSpecified
        {
            get => hideOwnerWhenFalseSpecifiedField;
            set => hideOwnerWhenFalseSpecifiedField = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string displayText
        {
            get => displayTextField;
            set => displayTextField = value;
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool displayTextSpecified
        {
            get => displayTextSpecifiedField;
            set => displayTextSpecifiedField = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string successText
        {
            get => successTextField;
            set => successTextField = value;
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool successTextSpecified
        {
            get => successTextSpecifiedField;
            set => successTextSpecifiedField = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string failText
        {
            get => failTextField;
            set => failTextField = value;
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool failTextSpecified
        {
            get => failTextSpecifiedField;
            set => failTextSpecifiedField = value;
        }
    }



    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeConditions
    {
        private BindingList<object> itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Timeout", typeof(TerjeTimeout))]
        [System.Xml.Serialization.XmlElementAttribute("SkillLevel", typeof(TerjeSkillLevel))]
        [System.Xml.Serialization.XmlElementAttribute("SkillPerk", typeof(TerjeSkillPerk))]
        [System.Xml.Serialization.XmlElementAttribute("SpecificPlayers", typeof(TerjeSpecificPlayers))]
        [System.Xml.Serialization.XmlElementAttribute("CustomCondition", typeof(TerjeCustomCondition))]
        [System.Xml.Serialization.XmlElementAttribute("Set", typeof(TerjeSetUserVariable))]
        [System.Xml.Serialization.XmlElementAttribute("Equal", typeof(TerjeComapreUserVariablesEqual))]
        [System.Xml.Serialization.XmlElementAttribute("NotEqual", typeof(TerjeComapreUserVariablesNotEqual))]
        [System.Xml.Serialization.XmlElementAttribute("LessThen", typeof(TerjeComapreUserVariablesLessThen))]
        [System.Xml.Serialization.XmlElementAttribute("GreaterThen", typeof(TerjeComapreUserVariablesGreaterThen))]
        [System.Xml.Serialization.XmlElementAttribute("LessOrEqual", typeof(TerjeComapreUserVariablesLessOrEqual))]
        [System.Xml.Serialization.XmlElementAttribute("GreaterOrEqual", typeof(TerjeComapreUserVariablesGreaterOrEqual))]
        [System.Xml.Serialization.XmlElementAttribute("Sum", typeof(TerjeMathWithUserVariableSum))]
        [System.Xml.Serialization.XmlElementAttribute("Subtract", typeof(TerjeMathWithUserVariableSubtract))]
        [System.Xml.Serialization.XmlElementAttribute("Multiply", typeof(TerjeMathWithUserVariableMultiply))]
        [System.Xml.Serialization.XmlElementAttribute("Divide", typeof(TerjeMathWithUserVariableDivide))]
        [System.Xml.Serialization.XmlElementAttribute("All", typeof(TerjeSpecialConditionsAll))]
        [System.Xml.Serialization.XmlElementAttribute("Any", typeof(TerjeSpecialConditionsAny))]
        [System.Xml.Serialization.XmlElementAttribute("One", typeof(TerjeSpecialConditionsOne))]
        public BindingList<object> items
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
    public partial class TerjeTimeout : TerjeConditionBase
    {
        private string idField;
        private int hoursField;
        private bool hoursFieldSpecified;
        private int minutesField;
        private bool minutesFieldSpecified;
        private int secondsField;
        private bool secondsFieldSpecified;

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
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeSkillLevel : TerjeConditionBase
    {
        private string skillIdField;
        private int requiredLevelField;

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

    }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeSkillPerk : TerjeConditionBase
    {
        private string skillIdField;
        private string perkIdField;
        private int requiredLevelField;

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

    }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeSpecificPlayers : TerjeConditionBase
    {
        private BindingList<TerjeSpecificPlayer> specificPlayerField;

        [System.Xml.Serialization.XmlElementAttribute("SpecificPlayer")]
        public BindingList<TerjeSpecificPlayer> SpecificPlayer
        {
            get { return this.specificPlayerField; }
            set { this.specificPlayerField = value; }
        }
    }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
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
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeCustomCondition : TerjeConditionBase
    {
        private string classnameField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classname
        {
            get { return this.classnameField; }
            set { this.classnameField = value; }
        }
    }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeSetUserVariable
    {
        private string nameField;
        private int valueField;
        private int persistField;
        private bool persistFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int value
        {
            get { return this.valueField; }
            set { this.valueField = value; }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int persist
        {
            get { return this.persistField; }
            set { this.persistField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool persistSpecified
        {
            get { return this.persistFieldSpecified; }
            set { this.persistFieldSpecified = value; }
        }
    }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeComapreUserVariables : TerjeConditionBase
    {
        private string nameField;
        private int valueField;
        private int persistField;
        private bool persistFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int value
        {
            get { return this.valueField; }
            set { this.valueField = value; }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int persist
        {
            get { return this.persistField; }
            set { this.persistField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool persistSpecified
        {
            get { return this.persistFieldSpecified; }
            set { this.persistFieldSpecified = value; }
        }
    }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeComapreUserVariablesEqual : TerjeComapreUserVariables { }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeComapreUserVariablesNotEqual : TerjeComapreUserVariables { }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeComapreUserVariablesLessThen : TerjeComapreUserVariables { }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeComapreUserVariablesGreaterThen : TerjeComapreUserVariables { }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeComapreUserVariablesLessOrEqual : TerjeComapreUserVariables { }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeComapreUserVariablesGreaterOrEqual : TerjeComapreUserVariables { }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeMathWithUserVariable
    {
        private string nameField;
        private int valueField;
        private int minField;
        private bool minFieldSpecified;
        private int maxField;
        private bool maxFieldSpecified;
        private int persistField;
        private bool persistFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int value
        {
            get { return this.valueField; }
            set { this.valueField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int min
        {
            get { return this.minField; }
            set { this.minField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minSpecified
        {
            get { return this.minFieldSpecified; }
            set { this.minFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int max
        {
            get { return this.maxField; }
            set { this.maxField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxSpecified
        {
            get { return this.maxFieldSpecified; }
            set { this.maxFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int persist
        {
            get { return this.persistField; }
            set { this.persistField = value; }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool persistSpecified
        {
            get { return this.persistFieldSpecified; }
            set { this.persistFieldSpecified = value; }
        }
    }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeMathWithUserVariableSum : TerjeMathWithUserVariable { }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeMathWithUserVariableSubtract : TerjeMathWithUserVariable { }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeMathWithUserVariableMultiply : TerjeMathWithUserVariable { }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeMathWithUserVariableDivide : TerjeMathWithUserVariable { }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeSpecialConditions : TerjeConditionBase
    {
        private BindingList<object> itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Timeout", typeof(TerjeTimeout))]
        [System.Xml.Serialization.XmlElementAttribute("SkillLevel", typeof(TerjeSkillLevel))]
        [System.Xml.Serialization.XmlElementAttribute("SkillPerk", typeof(TerjeSkillPerk))]
        [System.Xml.Serialization.XmlElementAttribute("SpecificPlayers", typeof(TerjeSpecificPlayers))]
        [System.Xml.Serialization.XmlElementAttribute("CustomCondition", typeof(TerjeCustomCondition))]
        [System.Xml.Serialization.XmlElementAttribute("Set", typeof(TerjeSetUserVariable))]
        [System.Xml.Serialization.XmlElementAttribute("Equal", typeof(TerjeComapreUserVariablesEqual))]
        [System.Xml.Serialization.XmlElementAttribute("NotEqual", typeof(TerjeComapreUserVariablesNotEqual))]
        [System.Xml.Serialization.XmlElementAttribute("LessThen", typeof(TerjeComapreUserVariablesLessThen))]
        [System.Xml.Serialization.XmlElementAttribute("GreaterThen", typeof(TerjeComapreUserVariablesGreaterThen))]
        [System.Xml.Serialization.XmlElementAttribute("LessOrEqual", typeof(TerjeComapreUserVariablesLessOrEqual))]
        [System.Xml.Serialization.XmlElementAttribute("GreaterOrEqual", typeof(TerjeComapreUserVariablesGreaterOrEqual))]
        [System.Xml.Serialization.XmlElementAttribute("Sum", typeof(TerjeMathWithUserVariableSum))]
        [System.Xml.Serialization.XmlElementAttribute("Subtract", typeof(TerjeMathWithUserVariableSubtract))]
        [System.Xml.Serialization.XmlElementAttribute("Multiply", typeof(TerjeMathWithUserVariableMultiply))]
        [System.Xml.Serialization.XmlElementAttribute("Divide", typeof(TerjeMathWithUserVariableDivide))]
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
    public partial class TerjeSpecialConditionsAll : TerjeSpecialConditions { }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeSpecialConditionsAny : TerjeSpecialConditions { }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeSpecialConditionsOne : TerjeSpecialConditions { }
}
