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
        public weather()
        {
            overcast = new weatherOvercast();
            fog = new weatherFog();
            rain = new weatherRain();
            wind = new weatherWind();
            storm = new weatherStorm();

        }

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
        public weatherOvercast()
        {
            current = new weatherOvercastCurrent();
            limits = new weatherOvercastLimits();
            timelimits = new weatherOvercastTimelimits();
            changelimits = new weatherOvercastChangelimits();
        }

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
        public weatherOvercastCurrent()
        {
            actual = (decimal)0.10;
            time = 120;
            duration = 240;
        }


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
        public weatherOvercastLimits()
        {
            min = (decimal)0.0;
            max = (decimal)0.55;
        }
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
        public weatherOvercastTimelimits()
        {
            min = 900;
            max = 1200;
        }
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
        public weatherOvercastChangelimits()
        {
            min = (decimal)0.1;
            max = (decimal)0.5;
        }
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
        public weatherFog()
        {
            current = new weatherFogCurrent();
            limits = new weatherFogLimits();
            timelimits = new weatherFogTimelimits();
            changelimits = new weatherFogChangelimits();
        }
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
        public weatherFogCurrent()
        {
            actual = (decimal)0.0;
            time = 120;
            duration = 240;
        }
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
        public weatherFogLimits()
        {
            min = (decimal)0.00;
            max = (decimal)0.01;
        }
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
        public weatherFogTimelimits()
        {
            min = 900;
            max = 1800;
        }
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
        public weatherFogChangelimits()
        {
            min = (decimal)0.1;
            max = (decimal)0.5;
        }
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
        public weatherRain()
        {
            current = new weatherRainCurrent();
            limits = new weatherRainLimits();
            timelimits = new weatherRainTimelimits();
            changelimits = new weatherRainChangelimits();
            thresholds = new weatherRainThresholds();
        }
        private weatherRainCurrent currentField;
        private weatherRainLimits limitsField;
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
        public weatherRainLimits limits
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
        public weatherRainCurrent()
        {
            actual = 0;
            time = 120;
            duration = 240;
        }
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
    public partial class weatherRainLimits
    {
        public weatherRainLimits()
        {
            min = (decimal)0.0;
            max = (decimal)0.35;
        }
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
        public weatherRainTimelimits()
        {
            min = 300;
            max = 600;
        }
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
        public weatherRainChangelimits()
        {
            min = 0;
            max = (decimal)0.25;
        }
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
        public weatherRainThresholds()
        {
            min = (decimal)0.5;
            max = (decimal)1.0;
            end = 120;
        }
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
        public weatherWind()
        {
            maxspeed = 10;
            @params = new weatherWindParams();
        }
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
        public weatherWindParams()
        {
            min = 0;
            max = 1;
            frequency = 60;
        }
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
        public weatherStorm()
        {
            density = (decimal)1.0;
            threshold = (decimal)0.75;
            timeout = 30;
        }
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
