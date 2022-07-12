using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
	public class NPCEmotes
	{
		public BindingList<Emote> Emotes { get; set; }

		public NPCEmotes(string filename)
        {
			Emotes = new BindingList<Emote>();
			foreach (string line in File.ReadLines(filename))
			{
				Emotes.Add(new Emote()
				{
					Name = line.Split('=')[0],
					Value = Convert.ToInt32(line.Split('=')[1])
				}
				);
			}
		}
	}

	public class Emote
	{
		public string Name { get; set; }
		public int Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
