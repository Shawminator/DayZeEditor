﻿using System.Text.Json.Serialization;

namespace DayZeLib
{

    public class AbandonedVehicleRemover
    {
        public int Lifetime { get; set; }
        public int UpdateInterval { get; set; }
        public int SaveInterval { get; set; }
        public int Logging { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;
    }

}
