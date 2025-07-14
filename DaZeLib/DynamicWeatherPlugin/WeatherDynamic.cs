using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class DynamicWeatherPlugin
    {
        [JsonIgnore]
        public bool isDirty { get; set; }
        [JsonIgnore]
        public string Filename { get; set; }

        public BindingList<WeatherDynamic> m_Dynamics { get; set; }


    }
    public class WeatherDynamic
    {
        public bool chat_message { get; set; }
        public bool notify_message { get; set; }
        public string name { get; set; }
        public decimal transition_min { get; set; }
        public decimal transition_max { get; set; }
        public decimal duration_min { get; set; }
        public decimal duration_max { get; set; }
        public decimal overcast_min { get; set; }
        public decimal overcast_max { get; set; }
        public bool use_dyn_vol_fog { get; set; }
        public decimal dyn_vol_fog_dist_min { get; set; }
        public decimal dyn_vol_fog_dist_max { get; set; }
        public decimal dyn_vol_fog_height_min { get; set; }
        public decimal dyn_vol_fog_height_max { get; set; }
        public decimal dyn_vol_fog_bias { get; set; }
        public decimal fog_transition_time { get; set; }
        public decimal fog_min { get; set; }
        public decimal fog_max { get; set; }
        public decimal wind_speed_min { get; set; }
        public decimal wind_speed_max { get; set; }
        public decimal wind_dir_min { get; set; }
        public decimal wind_dir_max { get; set; }
        public decimal rain_min { get; set; }
        public decimal rain_max { get; set; }
        public decimal snowfall_min { get; set; }
        public decimal snowfall_max { get; set; }
        public decimal snowflake_scale_min { get; set; }
        public decimal snowflake_scale_max { get; set; }
        public bool use_snowflake_scale { get; set; }
        public bool storm { get; set; }
        public decimal thunder_timeout { get; set; }
        public bool use_global_temperature { get; set; }
        public decimal global_temperature_override { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
