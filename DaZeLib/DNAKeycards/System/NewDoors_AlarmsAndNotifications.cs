using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class NewDoors_AlarmsAndNotifications
	{
		[JsonIgnore]
		public string Filename { get; set; }
		[JsonIgnore]
		public bool isDirty { get; set; }

		public bool dna_SoundAlarmOrangeLockout {get;set;}
		public int dna_NotificationRangeOrangeLockout {get;set;}

		public bool dna_SoundAlarmOrangeOneWay {get;set;}
		public int dna_NotificationRangeOrangeOneWay {get;set;}
		public bool dna_SoundAlarmOrangeWarp {get;set;}
		public int dna_NotificationRangeOrangeWarp {get;set;}

		public bool dna_SoundAlarmYellowOneWay {get;set;}
		public int dna_NotificationRangeYellowOneWay {get;set;}
		public bool dna_SoundAlarmYellowWarp {get;set;}
		public int dna_NotificationRangeYellowWarp {get;set;}

		public bool dna_SoundAlarmGreenOneWay {get;set;}
		public int dna_NotificationRangeGreenOneWay {get;set;}
		public bool dna_SoundAlarmGreenWarp {get;set;}
		public int dna_NotificationRangeGreenWarp {get;set;}

		public bool dna_SoundAlarmBlueOneWay {get;set;}
		public int dna_NotificationRangeBlueOneWay {get;set;}
		public bool dna_SoundAlarmBlueWarp {get;set;}
		public int dna_NotificationRangeBlueWarp {get;set;}

		public bool dna_SoundAlarmPurpleOneWay {get;set;}
		public int dna_NotificationRangePurpleOneWay {get;set;}
		public bool dna_SoundAlarmPurpleWarp {get;set;}
		public int dna_NotificationRangePurpleWarp {get;set;}

		public bool dna_SoundAlarmRedOneWay {get;set;}
		public int dna_NotificationRangeRedOneWay {get;set;}
		public bool dna_SoundAlarmRedWarp {get;set;}
		public int dna_NotificationRangeRedWarp {get;set;}

		public NewDoors_AlarmsAndNotifications() { }
		public void CreateDefaultAlarmSettings()
		{
			dna_SoundAlarmOrangeLockout = false;
			dna_NotificationRangeOrangeLockout = 0;
			dna_SoundAlarmOrangeOneWay = false;
			dna_NotificationRangeOrangeOneWay = 0;
			dna_SoundAlarmOrangeWarp = false;
			dna_NotificationRangeOrangeWarp = 0;
			dna_SoundAlarmYellowOneWay = false;
			dna_NotificationRangeYellowOneWay = 0;
			dna_SoundAlarmYellowWarp = false;
			dna_NotificationRangeYellowWarp = 0;
			dna_SoundAlarmGreenOneWay = false;
			dna_NotificationRangeGreenOneWay = 0;
			dna_SoundAlarmGreenWarp = false;
			dna_NotificationRangeGreenWarp = 0;
			dna_SoundAlarmBlueOneWay = false;
			dna_NotificationRangeBlueOneWay = 0;
			dna_SoundAlarmBlueWarp = false;
			dna_NotificationRangeBlueWarp = 0;
			dna_SoundAlarmPurpleOneWay = false;
			dna_NotificationRangePurpleOneWay = 0;
			dna_SoundAlarmPurpleWarp = false;
			dna_NotificationRangePurpleWarp = 0;
			dna_SoundAlarmRedOneWay = false;
			dna_NotificationRangeRedOneWay = 0;
			dna_SoundAlarmRedWarp = false;
			dna_NotificationRangeRedWarp = 0;
		}
	}
}
