using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class ECMAdminConfig
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public BindingList<string> AdminGUIDs { get; set; }

        public ECMAdminConfig() 
        {
            AdminGUIDs = new BindingList<string>();
        }
        public void AddNewAdminGUID(string adminGUID)
        {
            AdminGUIDs.Add(adminGUID);
            isDirty = true;
        }
        public void RemoveAdminGUID(string adminGUID) 
        {
            AdminGUIDs.Remove(adminGUID);
            isDirty = true;
        }
    }
}
