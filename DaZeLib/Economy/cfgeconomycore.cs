using System.ComponentModel;
using System.Linq;

namespace DayZeLib
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class economycore
    {

        private BindingList<economycoreRootclass> classesField;

        private BindingList<economycoreDefault> defaultsField;

        private BindingList<economycoreCE> ceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("rootclass", IsNullable = false)]
        public BindingList<economycoreRootclass> classes
        {
            get
            {
                return this.classesField;
            }
            set
            {
                this.classesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("default", IsNullable = false)]
        public BindingList<economycoreDefault> defaults
        {
            get
            {
                return this.defaultsField;
            }
            set
            {
                this.defaultsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ce")]
        public BindingList<economycoreCE> ce
        {
            get
            {
                return this.ceField;
            }
            set
            {
                this.ceField = value;
            }
        }

        public economycoreCE findFile(string modname)
        {
            foreach (economycoreCE ce in ce)
            {
                if (ce.file.Any(x => x.name == modname + ".xml"))
                {
                    return ce;
                }
            }
            return null;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class economycoreRootclass
    {

        private string nameField;

        private string reportMemoryLODField;

        private string actField;

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
        public string reportMemoryLOD
        {
            get
            {
                return this.reportMemoryLODField;
            }
            set
            {
                this.reportMemoryLODField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string act
        {
            get
            {
                return this.actField;
            }
            set
            {
                this.actField = value;
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
    public partial class economycoreDefault
    {

        private string nameField;

        private string valueField;

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
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
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
    public partial class economycoreCE
    {

        private BindingList<economycoreCEFile> fileField;

        private string folderField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("file")]
        public BindingList<economycoreCEFile> file
        {
            get
            {
                return this.fileField;
            }
            set
            {
                this.fileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string folder
        {
            get
            {
                return this.folderField;
            }
            set
            {
                this.folderField = value;
            }
        }

        public economycoreCEFile getfile(string modname)
        {
            foreach (economycoreCEFile _file in file)
            {
                if (_file.name == modname + ".xml")
                {
                    return _file;
                }
            }
            return null;
        }
        public void removefile(economycoreCEFile _file)
        {
            file.Remove(_file);
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class economycoreCEFile
    {

        private string nameField;

        private string typeField;

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
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }


        public override string ToString()
        {
            return name;
        }
    }


}
