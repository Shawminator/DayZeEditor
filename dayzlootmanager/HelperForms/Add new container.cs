using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DayZeLib;

namespace DayZeEditor
{
    public partial class Add_new_container : DarkForm
    {
        public string containerType
        {
            get { return getContainerString((ContainerTypes)comboBox1.SelectedItem); }
        }
        public decimal getFallspeedValue
        {
            get { return numericUpDown1.Value;  } 
        }
        public int getUsagevalue
        {
            get { return (int)numericUpDown8.Value;  }
        }
        public decimal getWeightValue
        {
            get { return numericUpDown9.Value; }
        }
        public int getItemevalue
        {
            get { return (int)numericUpDown10.Value; }
        }
        public int getinfectedcountValue
        {
            get { return (int)numericUpDown11.Value; }
        }
        public bool GetSpawnInfected
        {
            get { return checkBox5.Checked; }
        }

        public Add_new_container()
        {
            InitializeComponent();
            Form_Controls_AddfromType.InitializeForm_Controls
            (
                this,
                panel1,
                TitleLabel,
                CloseButton
            );
            comboBox1.DataSource = Enum.GetValues(typeof(ContainerTypes));
        }
        public static String getContainerString(ContainerTypes containertype)
        {
            switch (containertype)
            {
                case ContainerTypes.ExpansionAirdropContainer:
                    return "ExpansionAirdropContainer";
                case ContainerTypes.ExpansionAirdropContainer_Medical:
                    return "ExpansionAirdropContainer_Medical";
                case ContainerTypes.ExpansionAirdropContainer_Military:
                    return "ExpansionAirdropContainer_Military";
                case ContainerTypes.ExpansionAirdropContainer_Basebuilding:
                    return "ExpansionAirdropContainer_Basebuilding";
                case ContainerTypes.ExpansionAirdropContainer_Grey:
                    return "ExpansionAirdropContainer_Grey";
                case ContainerTypes.ExpansionAirdropContainer_Blue:
                    return "ExpansionAirdropContainer_Blue";
                case ContainerTypes.ExpansionAirdropContainer_Olive:
                    return "ExpansionAirdropContainer_Olive";
                case ContainerTypes.ExpansionAirdropContainer_Military_GreenCamo:
                    return "ExpansionAirdropContainer_Military_GreenCamo";
                case ContainerTypes.ExpansionAirdropContainer_Military_MarineCamo:
                    return "ExpansionAirdropContainer_Military_MarineCamo";
                case ContainerTypes.ExpansionAirdropContainer_Military_OliveCamo:
                    return "ExpansionAirdropContainer_Military_OliveCamo";
                case ContainerTypes.ExpansionAirdropContainer_Military_OliveCamo2:
                    return "ExpansionAirdropContainer_Military_OliveCamo2";
                case ContainerTypes.ExpansionAirdropContainer_Military_WinterCamo:
                    return "ExpansionAirdropContainer_Military_WinterCamo";
                default:
                    return "ExpansionAirdropContainer";
            }
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
