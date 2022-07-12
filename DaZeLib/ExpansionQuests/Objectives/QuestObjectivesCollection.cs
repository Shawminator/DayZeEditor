using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class QuestObjectivesCollection: QuestObjectivesBase
    {
        public Collection Collection { get; set; }

        public QuestObjectivesCollection()
        {
            Collection = new Collection();
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }

    public class Collection
    {
        public int Amount { get; set; }
        public string ClassName { get; set; }

        public Collection()
        { }

        public override string ToString()
        {
            return ClassName;
        }
    }

}
