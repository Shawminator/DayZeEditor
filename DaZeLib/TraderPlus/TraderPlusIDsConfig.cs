using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeLib
{
    public class TraderPlusIDsConfig
    {
        [JsonIgnore]
        public const string fileName = "TraderPlusIDsConfig.json";
        [JsonIgnore]
        public bool isDirty;
        [JsonIgnore]
        public const string m_Version = "2.5";

        public string Version { get; set; }
        public BindingList<IDs> IDs { get; set; }

        [JsonIgnore]
        public string FullFilename { get; set; }

        public bool CheckVersion()
        {
            if (Version != m_Version)
            {
                Version = m_Version;
                return false;
            }
            return true;
        }
        public IDs getTraderbyID(int id)
        {
            if(IDs.Any(x => x.Id == id))
                return IDs.Where(x => x.Id == id).First();
            MessageBox.Show("The following ID COuld not find matching NPC : " + id.ToString());
            return null;
        }
    }

    public class IDs
    {
        public int Id { get; set; }
        public BindingList<string> Categories { get; set; }
        public BindingList<string> LicencesRequired { get; set; }
        public BindingList<string> CurrenciesAccepted { get; set; }

    }
}
