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
    public class QuestObjectivesTreasureHunt: QuestObjectivesBase
    {
        public int ShowDistance { get; set; }
        public Treasurehunt TreasureHunt { get; set; }

        public QuestObjectivesTreasureHunt()
        {
            TreasureHunt = new Treasurehunt();
        }

        public override string ToString()
        {
            return ObjectiveText;
        }
    }

    public class Treasurehunt
    {
        public BindingList<float[]> Positions { get; set; }
        public Dictionary<string,int> Items { get; set; }
        [JsonIgnore]
        public BindingList<TreasureHuntItems> ListItems { get; set; }

        public Treasurehunt()
        {
            Items = new Dictionary<string, int>();
        }
        public void ConvertDictToList()
        {
            var initialList = new BindingList<TreasureHuntItems>();
            foreach (KeyValuePair<string, int> item in Items)
            {
                if (initialList.Any(x => x.ClassName == item.Key))
                {
                    MessageBox.Show(item.Key + " is allready in the list, please check and remove manaully....");
                }
                else
                {
                    TreasureHuntItems ti = new TreasureHuntItems() { ClassName = item.Key, Amount = item.Value };
                    initialList.Add(ti);
                }

            }
            ListItems = new BindingList<TreasureHuntItems>(new BindingList<TreasureHuntItems>(initialList.OrderBy(x => x.ClassName).ToList()));
        }   
        public void ConvertListToDict()
        {
            Items = new Dictionary<string, int>();
            foreach(TreasureHuntItems thi in ListItems)
            {
                Items.Add(thi.ClassName, thi.Amount);
            }
        }
    }
    public class TreasureHuntItems
    {
        public string ClassName { get; set; }
        public int Amount { get; set; }
        
        public TreasureHuntItems() { }


        public override string ToString()
        {
            return ClassName;
        }
    }
}
