using System;
using System.Collections.Generic;
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
    public partial class weather
    {

        private weatherOvercast overcastField;

        private weatherFog fogField;

        private weatherRain rainField;

        private weatherWind windField;

        private weatherStorm stormField;

        private int resetField;

        private int enableField;

        /// <remarks/>
        public weatherOvercast overcast
        {
            get
            {
                return this.overcastField;
            }
            set
            {
                this.overcastField = value;
            }
        }

        /// <remarks/>
        public weatherFog fog
        {
            get
            {
                return this.fogField;
            }
            set
            {
                this.fogField = value;
            }
        }

        /// <remarks/>
        public weatherRain rain
        {
            get
            {
                return this.rainField;
            }
            set
            {
                this.rainField = value;
            }
        }

        /// <remarks/>
        public weatherWind wind
        {
            get
            {
                return this.windField;
            }
            set
            {
                this.windField = value;
            }
        }

        /// <remarks/>
        public weatherStorm storm
        {
            get
            {
                return this.stormField;
            }
            set
            {
                this.stormField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int reset
        {
            get
            {
                return this.resetField;
            }
            set
            {
                this.resetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int enable
        {
            get
            {
                return this.enableField;
            }
            set
            {
                this.enableField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherOvercast
    {

        private weatherOvercastCurrent currentField;

        private weatherOvercastLimits limitsField;

        private weatherOvercastTimelimits timelimitsField;

        private weatherOvercastChangelimits changelimitsField;

        /// <remarks/>
        public weatherOvercastCurrent current
        {
            get
            {
                return this.currentField;
            }
            set
            {
                this.currentField = value;
            }
        }

        /// <remarks/>
        public weatherOvercastLimits limits
        {
            get
            {
                return this.limitsField;
            }
            set
            {
                this.limitsField = value;
            }
        }

        /// <remarks/>
        public weatherOvercastTimelimits timelimits
        {
            get
            {
                return this.timelimitsField;
            }
            set
            {
                this.timelimitsField = value;
            }
        }

        /// <remarks/>
        public weatherOvercastChangelimits changelimits
        {
            get
            {
                return this.changelimitsField;
            }
            set
            {
                this.changelimitsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherOvercastCurrent
    {

        private decimal actualField;

        private int timeField;

        private int durationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal actual
        {
            get
            {
                return this.actualField;
            }
            set
            {
                this.actualField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherOvercastLimits
    {

        private decimal minField;

        private decimal maxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherOvercastTimelimits
    {

        private int minField;

        private int maxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherOvercastChangelimits
    {

        private decimal minField;

        private decimal maxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherFog
    {

        private weatherFogCurrent currentField;

        private weatherFogLimits limitsField;

        private weatherFogTimelimits timelimitsField;

        private weatherFogChangelimits changelimitsField;

        /// <remarks/>
        public weatherFogCurrent current
        {
            get
            {
                return this.currentField;
            }
            set
            {
                this.currentField = value;
            }
        }

        /// <remarks/>
        public weatherFogLimits limits
        {
            get
            {
                return this.limitsField;
            }
            set
            {
                this.limitsField = value;
            }
        }

        /// <remarks/>
        public weatherFogTimelimits timelimits
        {
            get
            {
                return this.timelimitsField;
            }
            set
            {
                this.timelimitsField = value;
            }
        }

        /// <remarks/>
        public weatherFogChangelimits changelimits
        {
            get
            {
                return this.changelimitsField;
            }
            set
            {
                this.changelimitsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherFogCurrent
    {

        private decimal actualField;

        private int timeField;

        private int durationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal actual
        {
            get
            {
                return this.actualField;
            }
            set
            {
                this.actualField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherFogLimits
    {

        private decimal minField;

        private decimal maxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherFogTimelimits
    {

        private int minField;

        private int maxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherFogChangelimits
    {

        private decimal minField;

        private decimal maxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherRain
    {

        private weatherRainCurrent currentField;

        private weatherRainLimit limitField;

        private weatherRainTimelimits timelimitsField;

        private weatherRainChangelimits changelimitsField;

        private weatherRainThresholds thresholdsField;

        /// <remarks/>
        public weatherRainCurrent current
        {
            get
            {
                return this.currentField;
            }
            set
            {
                this.currentField = value;
            }
        }

        /// <remarks/>
        public weatherRainLimit limit
        {
            get
            {
                return this.limitField;
            }
            set
            {
                this.limitField = value;
            }
        }

        /// <remarks/>
        public weatherRainTimelimits timelimits
        {
            get
            {
                return this.timelimitsField;
            }
            set
            {
                this.timelimitsField = value;
            }
        }

        /// <remarks/>
        public weatherRainChangelimits changelimits
        {
            get
            {
                return this.changelimitsField;
            }
            set
            {
                this.changelimitsField = value;
            }
        }

        /// <remarks/>
        public weatherRainThresholds thresholds
        {
            get
            {
                return this.thresholdsField;
            }
            set
            {
                this.thresholdsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherRainCurrent
    {

        private decimal actualField;

        private int timeField;

        private int durationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal actual
        {
            get
            {
                return this.actualField;
            }
            set
            {
                this.actualField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherRainLimit
    {

        private decimal minField;

        private decimal maxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherRainTimelimits
    {

        private int minField;

        private int maxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherRainChangelimits
    {

        private decimal minField;

        private decimal maxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherRainThresholds
    {

        private decimal minField;

        private decimal maxField;

        private int endField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int end
        {
            get
            {
                return this.endField;
            }
            set
            {
                this.endField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherWind
    {

        private int maxspeedField;

        private weatherWindParams paramsField;

        /// <remarks/>
        public int maxspeed
        {
            get
            {
                return this.maxspeedField;
            }
            set
            {
                this.maxspeedField = value;
            }
        }

        /// <remarks/>
        public weatherWindParams @params
        {
            get
            {
                return this.paramsField;
            }
            set
            {
                this.paramsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherWindParams
    {

        private decimal minField;

        private decimal maxField;

        private int frequencyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int frequency
        {
            get
            {
                return this.frequencyField;
            }
            set
            {
                this.frequencyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class weatherStorm
    {

        private decimal densityField;

        private decimal thresholdField;

        private int timeoutField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal density
        {
            get
            {
                return this.densityField;
            }
            set
            {
                this.densityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal threshold
        {
            get
            {
                return this.thresholdField;
            }
            set
            {
                this.thresholdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int timeout
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
    }


}
