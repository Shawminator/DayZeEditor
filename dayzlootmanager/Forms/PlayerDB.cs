using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class PlayerDB : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        public DataSet m_DataSet_Playerdb;
        public string Projectname;
        private bool useraction = false;
        private DataTable m_CurrentTable;

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            ListBox lb = sender as ListBox;
            var CurrentItemWidth = (int)this.CreateGraphics().MeasureString(lb.Items[lb.Items.Count - 1].ToString(), lb.Font, TextRenderer.MeasureText(lb.Items[lb.Items.Count - 1].ToString(), new Font("Arial", 20.0F))).Width;
            lb.HorizontalExtent = CurrentItemWidth + 5;
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
        public PlayerDB()
        {
            InitializeComponent();
        }
        private void PlayerDB_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();
            m_DataSet_Playerdb = currentproject.PlayerDB.m_DataSet;

            useraction = false;
            MorphBinaryColumns(m_DataSet_Playerdb.Tables[1]);

            dataGridView.DataSource = m_DataSet_Playerdb;
            dataGridView.DataMember = m_DataSet_Playerdb.Tables[0].TableName;

            PlayerDBTablesLB.Items.Clear();
            for (int i = 0; i < m_DataSet_Playerdb.Tables.Count; i++)
            {
                PlayerDBTablesLB.Items.Add(this.m_DataSet_Playerdb.Tables[i].TableName);
            }
            PlayerDBTablesLB.Refresh();

            useraction = true;
        }

        private void PlayerDBTablesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = PlayerDBTablesLB.SelectedIndex;
            PlayerDBTablesLB.Cursor = Cursors.WaitCursor;
            m_CurrentTable = m_DataSet_Playerdb.Tables[selectedIndex];
            dataGridView.DataMember = m_CurrentTable.TableName;
            PlayerDBTablesLB.Cursor = Cursors.Default;
        }
        private void MorphBinaryColumns(DataTable table)
        {
            var targetNames = table.Columns.Cast<DataColumn>().Where(col => col.DataType.Equals(typeof(byte[]))).Select(col => col.ColumnName).ToList();
            foreach (string colName in targetNames)
            {
                // add new column and put it where the old column was
                var tmpName = "new";
                table.Columns.Add(new DataColumn(tmpName, typeof(string)));
                table.Columns[tmpName].SetOrdinal(table.Columns[colName].Ordinal);

                // fill in values in new column for every row
                foreach (DataRow row in table.Rows)
                {
                    row[tmpName] = "0x" + string.Join("",((byte[])row[colName]).Select(b => b.ToString("X2")).ToArray());
                }

                // cleanup
                table.Columns.Remove(colName);
                table.Columns[tmpName].ColumnName = colName;
            }
        }
    }
}
