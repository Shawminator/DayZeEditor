using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeEditor
{
    
    public partial class CategoryList : DarkForm
    {
        public MarketCategories MarketCats { get; set; }
        public List<marketItem> marketitems { get; set; }
        public Categories currentcat;

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox lb = sender as ListBox;
            e.DrawBackground();
            Brush myBrush = Brushes.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            }
            else
            {
                myBrush = Brushes.White;
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(60, 63, 65)), e.Bounds);
            }
            e.Graphics.DrawString(lb.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds);
            e.DrawFocusRectangle();
        }
        public CategoryList()
        {
            InitializeComponent();
            Form_Controls_AddfromType.InitializeForm_Controls
            (
                this,
                panel1,
                TitleLabel,
                CloseButton
            );
        }

        private void CategoryList_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "Name";
            listBox1.ValueMember = "Value";
            listBox1.DataSource = marketitems;

            
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Value";
            comboBox1.DataSource = MarketCats.CatList;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentcat = (Categories)comboBox1.SelectedItem;
            listBox2.DisplayMember = "Name";
            listBox2.ValueMember = "Value";
            listBox2.DataSource = currentcat.Items;
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            marketItem item = listBox1.SelectedItem as marketItem;
            Categories currentitemcurretncat = MarketCats.GetCat(item);
            MarketCats.removeitemfromcat(item);
            currentcat.Items.Add(item);
            currentcat.isDirty = true;
            if (currentitemcurretncat.Items.Count == 0)
            {
                MarketCats.RemoveCat(currentitemcurretncat);
            }
        }
    }
}
