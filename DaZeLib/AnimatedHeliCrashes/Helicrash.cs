using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class Helicrash
    {
        public int admin_log { get; set; }
        public int EnablePlaneCrashes { get; set; }
        public int HelicrashSpawnTime { get; set; }
        public int HelicrashDespawnTime { get; set; }
        public BindingList<CrashPoints> CrashPoints { get; set; }
        public HelicopterUS_ HelicopterUS_ { get; set; }
        public HelicopterPlane_ HelicopterPlane_ { get; set; }
        public AnimalSpawnArray AnimalSpawnArray { get; set; }
        public ZombieSpawnArray ZombieSpawnArray { get; set; }
        public BindingList<string> Loot_Helicrash { get; set; }
        public BindingList<string> Loot_Planecrash { get; set; }
        public BindingList<WeaponLootTables> WeaponLootTables { get; set; }
        public int AttackHelicopter_RoamingMode { get; set; }
        public int AttackHelicopter_Ammo { get; set; }
        public int EnableAttackHelicopter { get; set; }
        public int AttackHelicopterAttackDistance { get; set; }
        public int AttackHelicopter_FollowPlayer { get; set; }
        public int AttackHelicopter_ShootUnArmedAndProne { get; set; }

        [JsonIgnore]
        public bool isDirty;
    }

    public class CrashPoints
    {
        public float x { get; set; }
        public float y { get; set; }
        public float Radius { get; set; }
        public string Crash_Message { get; set; }

        public override string ToString()
        {
            return Crash_Message;
        }
    }
    public class HelicopterUS_
    {
        public int start_height { get; set; }
        public int minimum_height { get; set; }
        public int speed { get; set; }
        public int minimum_speed { get; set; }
        public int Maximum_Loot_Helicrash { get; set; }
        public int Maximum_Weapons_Helicrash { get; set; }
        public int Minimum_Loot_Helicrash { get; set; }
        public int Minimum_Weapons_Helicrash { get; set; }
    }
    public class HelicopterPlane_
    {
        public int start_height { get; set; }
        public int minimum_height { get; set; }
        public int speed { get; set; }
        public int minimum_speed { get; set; }
        public int Maximum_Loot_Planecrash { get; set; }
        public int Maximum_Weapons_Planecrash { get; set; }
        public int Minimum_Loot_Planecrash { get; set; }
        public int Minimum_Weapons_Planecrash { get; set; }

    }
    public class AnimalSpawnArray
    {
        public BindingList<string> animal_name { get; set; }
        public int radius { get; set; }
        public int amount_minimum { get; set; }
        public int amount_maximum { get; set; }
    }
    public class ZombieSpawnArray
    {
        public BindingList<string> zombie_name { get; set; }
        public int radius { get; set; }
        public int amount_minimum { get; set; }
        public int amount_maximum { get; set; }
    }
    public class WeaponLootTables
    {
        public string WeaponName { get; set; }
        public BindingList<string> Magazines { get; set; }
        public BindingList<string> Attachments { get; set; }
        public string Sight { get; set; }

        public override string ToString()
        {
            return WeaponName;
        }
    }
}
