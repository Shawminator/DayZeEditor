using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class DNABasicConfig
    {
        public BindingList<M_Dnasidearm> m_DNASideArm { get; set; }
        public BindingList<M_Dnafood> m_DNAFood { get; set; }
        public BindingList<M_Dnadrink> m_DNADrink { get; set; }
        public BindingList<M_Dnatools> m_DNATools { get; set; }
        public BindingList<M_Dnamedical> m_DNAMedical { get; set; }
        public BindingList<M_Dnabuildingmats> m_DNABuildingMats { get; set; }
        public BindingList<M_Dnavaluables> m_DNAValuables { get; set; }
        public BindingList<M_Dnabasevars> m_DNABaseVars { get; set; }
    }

    public class M_Dnasidearm
    {
        public string dna_Weap { get; set; }
        public string dna_Optic { get; set; }
        public string dna_Suppressor { get; set; }
        public string dna_UnderBarrel { get; set; }
        public string dna_Mag { get; set; }
        public string dna_Ammo { get; set; }
    }

    public class M_Dnafood
    {
        public string dna_Food { get; set; }

        public override string ToString()
        {
            return dna_Food;
        }
    }

    public class M_Dnadrink
    {
        public string dna_Drink { get; set; }
        public override string ToString()
        {
            return dna_Drink;
        }
    }

    public class M_Dnatools
    {
        public string dna_Tool { get; set; }
        public override string ToString()
        {
            return dna_Tool;
        }
    }

    public class M_Dnamedical
    {
        public string dna_Medical { get; set; }
        public override string ToString()
        {
            return dna_Medical;
        }
    }

    public class M_Dnabuildingmats
    {
        public string dna_Material { get; set; }
        public override string ToString()
        {
            return dna_Material;
        }
    }

    public class M_Dnavaluables
    {
        public string dna_Valuable { get; set; }
        public override string ToString()
        {
            return dna_Valuable;
        }
    }

    public class M_Dnabasevars
    {
        public string dna_BaseOption { get; set; }
        public int dna_BaseVar { get; set; }
        public override string ToString()
        {
            return dna_BaseOption;
        }
    }

}
