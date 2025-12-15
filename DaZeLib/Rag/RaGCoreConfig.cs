using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class RaGCoreConfig
    {
        const int CurrentVersion = 3;

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public int Version { get; set; }
        public bool EnableDebugLog { get; set; }
        public bool EnableInfoLog { get; set; }
        public bool EnableWarningLog { get; set; }
        public bool EnableErrorLog { get; set; }
        public bool EnableCFToolsDismantleLog { get; set; }
        public bool EnableCFToolsOpenCloseLog { get; set; }
        public bool EnableGeneralActivityLog { get; set; }
        public bool EnableVehicleActivityLog { get; set; }
        public bool EnableExtendedActivityLog { get; set; }
        public bool EnableHeavyExtendedActivityLog { get; set; }

        public RaGCoreConfig()
        {
            Version = CurrentVersion;
            EnableDebugLog = false;
            EnableInfoLog = false;
            EnableWarningLog = false;
            EnableErrorLog = true;

            EnableCFToolsDismantleLog = true;
            EnableCFToolsOpenCloseLog = true;

            EnableGeneralActivityLog = true;
            EnableVehicleActivityLog = true;
            EnableExtendedActivityLog = false;
            EnableHeavyExtendedActivityLog = false;
        }
        public bool checkver()
        {
            if (Version != CurrentVersion)
            {
                Version = CurrentVersion;
                isDirty = true;
                return true;
            }
            return false;
        }
    }
}
