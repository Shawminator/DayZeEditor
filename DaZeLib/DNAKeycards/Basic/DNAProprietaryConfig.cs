using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class DNAProprietaryConfig
    {
        public BindingList<M_Dnapropyellow> m_DNAPropYellow { get; set; }
        public BindingList<M_Dnapropgreen> m_DNAPropGreen { get; set; }
        public BindingList<M_Dnapropblue> m_DNAPropBlue { get; set; }
        public BindingList<M_Dnaproppurple> m_DNAPropPurple { get; set; }
        public BindingList<M_Dnapropred> m_DNAPropRed { get; set; }
    }

    public class M_Dnapropyellow
    {
        public string dna_Item { get; set; }
        public override string ToString()
        {
            return dna_Item;
        }
    }

    public class M_Dnapropgreen
    {
        public string dna_Item { get; set; }
        public override string ToString()
        {
            return dna_Item;
        }
    }

    public class M_Dnapropblue
    {
        public string dna_Item { get; set; }
        public override string ToString()
        {
            return dna_Item;
        }
    }

    public class M_Dnaproppurple
    {
        public string dna_Item { get; set; }
        public override string ToString()
        {
            return dna_Item;
        }
    }

    public class M_Dnapropred
    {
        public string dna_Item { get; set; }
        public override string ToString()
        {
            return dna_Item;
        }
    }

}
