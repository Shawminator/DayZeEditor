﻿using System.Text.Json.Serialization;

namespace DayZeLib
{
    class ATMs
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public string PlayerID { get; set; }
        public int MoneyDeposited { get; set; }

        public override string ToString()
        {
            return Filename;
        }
    }
}
