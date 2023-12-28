using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
	public class CrateSmol_TimerSettings
	{
		[JsonIgnore]
		public string Filename { get; set; }
		[JsonIgnore]
		public bool isDirty { get; set; }

		public string dna_Description { get; set; }
		public string dna_DescriptionCont{get;set;}
		public BindingList<DNA_Smol_Crate_Spawns> dna_CrateSpawns { get; set; }
		public bool dna_SpawnSmolCrates { get; set; }
		public bool dna_ResetSmolCrates{get;set;}
		public int dna_TimeUntilYellowSmolCrateResets { get; set; }
		public int dna_TimeUntilGreenSmolCrateResets { get; set; }
		public int dna_TimeUntilBlueSmolCrateResets { get; set; }
		public int dna_TimeUntilPurpleSmolCrateResets { get; set; }
		public int dna_TimeUntilRedSmolCrateResets { get; set; }

		public CrateSmol_TimerSettings()
		{
			dna_CrateSpawns = new BindingList<DNA_Smol_Crate_Spawns>();
		}
		public void CreateDefaultConfig()
		{
			CreateDefault();
		}
		public void CreateDefault()
		{
			dna_Description = "Config for spawning and respawning smol crates. In order to use this to spawn smol crates, set dna_SpawnSmolCrates to true, and add your";
			dna_DescriptionCont = "locations, and orientations along with the desired tier. To use timer settings, the timer must be activated in ResetTimer_json.";
			dna_ResetSmolCrates = false;
			dna_TimeUntilYellowSmolCrateResets = 30;
			dna_TimeUntilGreenSmolCrateResets = 30;
			dna_TimeUntilBlueSmolCrateResets = 30;
			dna_TimeUntilPurpleSmolCrateResets = 30;
			dna_TimeUntilRedSmolCrateResets = 30;
			dna_SpawnSmolCrates = false;
			dna_CrateSpawns.Add(new DNA_Smol_Crate_Spawns("Yellow", "0.0 0.0 0.0", "0.0 0.0 0.0"));
			dna_CrateSpawns.Add(new DNA_Smol_Crate_Spawns("Green", "0.0 0.0 0.0", "0.0 0.0 0.0"));
			dna_CrateSpawns.Add(new DNA_Smol_Crate_Spawns("Blue", "0.0 0.0 0.0", "0.0 0.0 0.0"));
			dna_CrateSpawns.Add(new DNA_Smol_Crate_Spawns("Purple", "0.0 0.0 0.0", "0.0 0.0 0.0"));
			dna_CrateSpawns.Add(new DNA_Smol_Crate_Spawns("Red", "0.0 0.0 0.0", "0.0 0.0 0.0"));
		}
	}
	public class DNA_Smol_Crate_Spawns
	{
		public string dna_Tier { get; set; }
		public string dna_Location { get; set; }
		public string dna_Rotation { get; set; }

		public DNA_Smol_Crate_Spawns() { }
		public DNA_Smol_Crate_Spawns(string tier, string location, string rotation)
		{
			dna_Tier = tier;
			dna_Location = location;
			dna_Rotation = rotation;
		}
	}
}
