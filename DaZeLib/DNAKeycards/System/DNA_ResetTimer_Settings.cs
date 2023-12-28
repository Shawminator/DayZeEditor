using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
	public class DNA_ResetTimer_Settings
	{
		[JsonIgnore]
		public string Filename { get; set; }
		[JsonIgnore]
		public bool isDirty { get; set; }

		public string dna_WARNING { get; set; }
		public string dna_WARNING_CONTINUED { get; set; }

		public bool dna_UseResetTimer { get; set; }
		public int dna_TimeBetweenChecks { get; set; }
		public decimal dna_Min_Distance_Between_Nearest_Player { get; set; }

		public bool dna_ResetCrates { get; set; }
		public int dna_TimeUntilYellowCrateResets { get; set; }
		public int dna_TimeUntilGreenCrateResets { get; set; }
		public int dna_TimeUntilBlueCrateResets { get; set; }
		public int dna_TimeUntilPurpleCrateResets { get; set; }
		public int dna_TimeUntilRedCrateResets { get; set; }

		public bool dna_ResetStrongrooms { get; set; }
		public int dna_TimeUntilYellowSRoomResets { get; set; }
		public int dna_TimeUntilGreenSRoomResets { get; set; }
		public int dna_TimeUntilBlueSRoomResets { get; set; }
		public int dna_TimeUntilPurpleSRoomResets { get; set; }
		public int dna_TimeUntilRedSRoomResets { get; set; }

		public bool dna_ResetLockouts { get; set; }
		public int dna_TimeUntilYellowLockoutResets { get; set; }
		public int dna_TimeUntilGreenLockoutResets { get; set; }
		public int dna_TimeUntilBlueLockoutResets { get; set; }
		public int dna_TimeUntilPurpleLockoutResets { get; set; }
		public int dna_TimeUntilRedLockoutResets { get; set; }
		public int dna_TimeUntilOrangeLockoutResets { get; set; }

		public bool dna_ResetOneWayDoors { get; set; }
		public int dna_TimeUntilYellowOWDoorResets { get; set; }
		public int dna_TimeUntilGreenOWDoorResets { get; set; }
		public int dna_TimeUntilBlueOWDoorResets { get; set; }
		public int dna_TimeUntilPurpleOWDoorResets { get; set; }
		public int dna_TimeUntilRedOWDoorResets { get; set; }
		public int dna_TimeUntilOrangeOWDoorResets { get; set; }

		public bool dna_ResetWarpDoors { get; set; }
		public int dna_TimeUntilYellowWarpDoorResets { get; set; }
		public int dna_TimeUntilGreenWarpDoorResets { get; set; }
		public int dna_TimeUntilBlueWarpDoorResets { get; set; }
		public int dna_TimeUntilPurpleWarpDoorResets { get; set; }
		public int dna_TimeUntilRedWarpDoorResets { get; set; }
		public int dna_TimeUntilOrangeWarpDoorResets { get; set; }

		public DNA_ResetTimer_Settings() { }
		public void CreateDefaultSettings()
		{
			dna_WARNING = "You must turn on and configure all types and tiers individually, distance to nearsest player is the only shared value.";
			dna_WARNING_CONTINUED = "Distance is in meters, and all time is in whole minutes. To turn off a tier, just set to longer than intended uptime.";

			dna_UseResetTimer = false;

			dna_TimeBetweenChecks = 5;

			dna_Min_Distance_Between_Nearest_Player = (decimal)500.0;

			dna_ResetCrates = false;
			dna_TimeUntilYellowCrateResets = 30;
			dna_TimeUntilGreenCrateResets = 30;
			dna_TimeUntilBlueCrateResets = 30;
			dna_TimeUntilPurpleCrateResets = 30;
			dna_TimeUntilRedCrateResets = 30;

			dna_ResetStrongrooms = false;
			dna_TimeUntilYellowSRoomResets = 30;
			dna_TimeUntilGreenSRoomResets = 30;
			dna_TimeUntilBlueSRoomResets = 30;
			dna_TimeUntilPurpleSRoomResets = 30;
			dna_TimeUntilRedSRoomResets = 30;

			dna_ResetLockouts = false;
			dna_TimeUntilYellowLockoutResets = 30;
			dna_TimeUntilGreenLockoutResets = 30;
			dna_TimeUntilBlueLockoutResets = 30;
			dna_TimeUntilPurpleLockoutResets = 30;
			dna_TimeUntilRedLockoutResets = 30;
			dna_TimeUntilOrangeLockoutResets = 30;

			dna_ResetOneWayDoors = false;
			dna_TimeUntilYellowOWDoorResets = 30;
			dna_TimeUntilGreenOWDoorResets = 30;
			dna_TimeUntilBlueOWDoorResets = 30;
			dna_TimeUntilPurpleOWDoorResets = 30;
			dna_TimeUntilRedOWDoorResets = 30;
			dna_TimeUntilOrangeOWDoorResets = 30;

			dna_ResetWarpDoors = false;
			dna_TimeUntilYellowWarpDoorResets = 30;
			dna_TimeUntilGreenWarpDoorResets = 30;
			dna_TimeUntilBlueWarpDoorResets = 30;
			dna_TimeUntilPurpleWarpDoorResets = 30;
			dna_TimeUntilRedWarpDoorResets = 30;
			dna_TimeUntilOrangeWarpDoorResets = 30;
		}


    }
}
