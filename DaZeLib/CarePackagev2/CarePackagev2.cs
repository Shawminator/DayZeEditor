using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class CarePackagev2
    {
        public BindingList<Locations> Locations { get; set; }
        public BindingList<Packages> Packages { get; set; }
        public BindingList<string> InfectedTypes { get; set; }
        public int MinutesBetweenPackages { get; set; }
        public int DropHeight { get; set; }
        public int DropTime { get; set; }
        public int PackageCallDelay { get; set; }
        public int MinutesAway { get; set; }
        public int MinimumPlayers { get; set; }
        public int PackagesBeingRan { get; set; }
        public int ZombiesToSpawn { get; set; }
        public int LocationHistoryCheckRange { get; set; }
        public int LootSpawnType { get; set; }
        public int LockPackages { get; set; }
        public string Title { get; set; }
        public string DroppedMessage { get; set; }
        public string StartMessage { get; set; }
        public string MinutesAwayPrefix { get; set; }
        public string MinutesAwaySuffix { get; set; }

      
    }

    public class Locations
    {
        public string Name { get; set; }
        public int[] Location { get; set; }
        public int Accuracy { get; set; }
        public BindingList<int> AllowedPackageIDs { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Packages
    {
        public string Package_name { get; set; }
        public string object_type { get; set; }
        public string parachute_type { get; set; }
        public int MinWeapons { get; set; }
        public int MaxWeapons { get; set; }
        public int MinMiscItems { get; set; }
        public int MaxMiscItems { get; set; }
        public BindingList<int> AllowedPackageIDs { get; set; }
        public BindingList<Items> Items { get; set; }
        public BindingList<Weapons> Weapons { get; set; }

        public override string ToString()
        {
            return Package_name;
        }
    }

    public class Items
    {
        public string Item { get; set; }
        public int MinQty { get; set; }
        public int MaxQty { get; set; }
        public BindingList<string> Attachments { get; set; }
        
        public override string ToString()
        {
            return Item;
        }
    }

    public class Weapons
    {
        public string Item { get; set; }
        public BindingList<string> Attachments { get; set; }

        public override string ToString()
        {
            return Item;
        }
    }

}
