
using TreeViewMS;

namespace DayZeLib
{
    partial class ExpansionLootControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpansionLootControl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ExpansionLootTV = new TreeViewMS.TreeViewMS();
            this.expansionLootItemGB = new System.Windows.Forms.GroupBox();
            this.numericUpDown31 = new System.Windows.Forms.NumericUpDown();
            this.darkLabel23 = new DarkUI.Controls.DarkLabel();
            this.darkLabel22 = new DarkUI.Controls.DarkLabel();
            this.numericUpDown33 = new System.Windows.Forms.NumericUpDown();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.darkLabel220 = new DarkUI.Controls.DarkLabel();
            this.darkLabel12 = new DarkUI.Controls.DarkLabel();
            this.numericUpDown12 = new System.Windows.Forms.NumericUpDown();
            this.darkLabel251 = new DarkUI.Controls.DarkLabel();
            this.expansionLootVarientGB = new System.Windows.Forms.GroupBox();
            this.ExpansionLootitemSetAllRandomChanceButton = new DarkUI.Controls.DarkButton();
            this.ExpansionLootitemSetAllChanceButton = new DarkUI.Controls.DarkButton();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.darkLabel2 = new DarkUI.Controls.DarkLabel();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addLootItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addLootVariantsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addLootAttachmentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeLootItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeLootAttachemntToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeLootVariantToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.expansionLootItemGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown31)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown33)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown12)).BeginInit();
            this.expansionLootVarientGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ExpansionLootTV);
            this.groupBox1.Controls.Add(this.expansionLootItemGB);
            this.groupBox1.Controls.Add(this.expansionLootVarientGB);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(561, 510);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Expansion Loot";
            // 
            // ExpansionLootTV
            // 
            this.ExpansionLootTV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ExpansionLootTV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.ExpansionLootTV.ForeColor = System.Drawing.SystemColors.Control;
            this.ExpansionLootTV.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ExpansionLootTV.Location = new System.Drawing.Point(6, 19);
            this.ExpansionLootTV.Name = "ExpansionLootTV";
            this.ExpansionLootTV.SelectedNodes = ((System.Collections.ArrayList)(resources.GetObject("ExpansionLootTV.SelectedNodes")));
            this.ExpansionLootTV.SetMultiselect = false;
            this.ExpansionLootTV.Size = new System.Drawing.Size(265, 485);
            this.ExpansionLootTV.TabIndex = 214;
            this.ExpansionLootTV.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ExpansionLootTV_NodeMouseClick);
            // 
            // expansionLootItemGB
            // 
            this.expansionLootItemGB.Controls.Add(this.numericUpDown31);
            this.expansionLootItemGB.Controls.Add(this.darkLabel23);
            this.expansionLootItemGB.Controls.Add(this.darkLabel22);
            this.expansionLootItemGB.Controls.Add(this.numericUpDown33);
            this.expansionLootItemGB.Controls.Add(this.trackBar1);
            this.expansionLootItemGB.Controls.Add(this.darkLabel220);
            this.expansionLootItemGB.Controls.Add(this.darkLabel12);
            this.expansionLootItemGB.Controls.Add(this.numericUpDown12);
            this.expansionLootItemGB.Controls.Add(this.darkLabel251);
            this.expansionLootItemGB.ForeColor = System.Drawing.SystemColors.Control;
            this.expansionLootItemGB.Location = new System.Drawing.Point(277, 19);
            this.expansionLootItemGB.Name = "expansionLootItemGB";
            this.expansionLootItemGB.Size = new System.Drawing.Size(276, 154);
            this.expansionLootItemGB.TabIndex = 224;
            this.expansionLootItemGB.TabStop = false;
            this.expansionLootItemGB.Text = "Expansion Loot Item";
            this.expansionLootItemGB.Visible = false;
            // 
            // numericUpDown31
            // 
            this.numericUpDown31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.numericUpDown31.DecimalPlaces = 2;
            this.numericUpDown31.ForeColor = System.Drawing.SystemColors.Control;
            this.numericUpDown31.Location = new System.Drawing.Point(114, 62);
            this.numericUpDown31.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            this.numericUpDown31.Name = "numericUpDown31";
            this.numericUpDown31.Size = new System.Drawing.Size(148, 20);
            this.numericUpDown31.TabIndex = 220;
            this.numericUpDown31.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown31.ValueChanged += new System.EventHandler(this.numericUpDown31_ValueChanged);
            // 
            // darkLabel23
            // 
            this.darkLabel23.AutoSize = true;
            this.darkLabel23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel23.Location = new System.Drawing.Point(172, 43);
            this.darkLabel23.Name = "darkLabel23";
            this.darkLabel23.Size = new System.Drawing.Size(15, 13);
            this.darkLabel23.TabIndex = 223;
            this.darkLabel23.Text = "%";
            // 
            // darkLabel22
            // 
            this.darkLabel22.AutoSize = true;
            this.darkLabel22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel22.Location = new System.Drawing.Point(4, 22);
            this.darkLabel22.Name = "darkLabel22";
            this.darkLabel22.Size = new System.Drawing.Size(50, 13);
            this.darkLabel22.TabIndex = 215;
            this.darkLabel22.Text = "Chance :";
            // 
            // numericUpDown33
            // 
            this.numericUpDown33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.numericUpDown33.ForeColor = System.Drawing.SystemColors.Control;
            this.numericUpDown33.Location = new System.Drawing.Point(114, 112);
            this.numericUpDown33.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown33.Name = "numericUpDown33";
            this.numericUpDown33.Size = new System.Drawing.Size(147, 20);
            this.numericUpDown33.TabIndex = 222;
            this.numericUpDown33.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown33.ValueChanged += new System.EventHandler(this.numericUpDown33_ValueChanged);
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(82, 13);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(191, 45);
            this.trackBar1.TabIndex = 216;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            this.trackBar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar1_MouseUp);
            // 
            // darkLabel220
            // 
            this.darkLabel220.AutoSize = true;
            this.darkLabel220.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel220.Location = new System.Drawing.Point(4, 114);
            this.darkLabel220.Name = "darkLabel220";
            this.darkLabel220.Size = new System.Drawing.Size(55, 13);
            this.darkLabel220.TabIndex = 221;
            this.darkLabel220.Text = "Min Count";
            // 
            // darkLabel12
            // 
            this.darkLabel12.AutoSize = true;
            this.darkLabel12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel12.Location = new System.Drawing.Point(4, 88);
            this.darkLabel12.Name = "darkLabel12";
            this.darkLabel12.Size = new System.Drawing.Size(58, 13);
            this.darkLabel12.TabIndex = 217;
            this.darkLabel12.Text = "Max Count";
            // 
            // numericUpDown12
            // 
            this.numericUpDown12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.numericUpDown12.ForeColor = System.Drawing.SystemColors.Control;
            this.numericUpDown12.Location = new System.Drawing.Point(114, 86);
            this.numericUpDown12.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown12.Name = "numericUpDown12";
            this.numericUpDown12.Size = new System.Drawing.Size(148, 20);
            this.numericUpDown12.TabIndex = 218;
            this.numericUpDown12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown12.ValueChanged += new System.EventHandler(this.numericUpDown12_ValueChanged);
            // 
            // darkLabel251
            // 
            this.darkLabel251.AutoSize = true;
            this.darkLabel251.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel251.Location = new System.Drawing.Point(4, 64);
            this.darkLabel251.Name = "darkLabel251";
            this.darkLabel251.Size = new System.Drawing.Size(86, 13);
            this.darkLabel251.TabIndex = 219;
            this.darkLabel251.Text = "Quantity Percent";
            // 
            // expansionLootVarientGB
            // 
            this.expansionLootVarientGB.Controls.Add(this.ExpansionLootitemSetAllRandomChanceButton);
            this.expansionLootVarientGB.Controls.Add(this.ExpansionLootitemSetAllChanceButton);
            this.expansionLootVarientGB.Controls.Add(this.darkLabel1);
            this.expansionLootVarientGB.Controls.Add(this.darkLabel2);
            this.expansionLootVarientGB.Controls.Add(this.trackBar2);
            this.expansionLootVarientGB.ForeColor = System.Drawing.SystemColors.Control;
            this.expansionLootVarientGB.Location = new System.Drawing.Point(277, 19);
            this.expansionLootVarientGB.Name = "expansionLootVarientGB";
            this.expansionLootVarientGB.Size = new System.Drawing.Size(276, 154);
            this.expansionLootVarientGB.TabIndex = 225;
            this.expansionLootVarientGB.TabStop = false;
            this.expansionLootVarientGB.Text = "Expansion Loot Varient";
            this.expansionLootVarientGB.Visible = false;
            // 
            // ExpansionLootitemSetAllRandomChanceButton
            // 
            this.ExpansionLootitemSetAllRandomChanceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ExpansionLootitemSetAllRandomChanceButton.Location = new System.Drawing.Point(8, 95);
            this.ExpansionLootitemSetAllRandomChanceButton.Name = "ExpansionLootitemSetAllRandomChanceButton";
            this.ExpansionLootitemSetAllRandomChanceButton.Padding = new System.Windows.Forms.Padding(5);
            this.ExpansionLootitemSetAllRandomChanceButton.Size = new System.Drawing.Size(261, 23);
            this.ExpansionLootitemSetAllRandomChanceButton.TabIndex = 225;
            this.ExpansionLootitemSetAllRandomChanceButton.Text = "Set All Random Chance";
            this.ExpansionLootitemSetAllRandomChanceButton.Click += new System.EventHandler(this.ExpansionLootitemSetAllRandomChanceButton_Click);
            // 
            // ExpansionLootitemSetAllChanceButton
            // 
            this.ExpansionLootitemSetAllChanceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ExpansionLootitemSetAllChanceButton.Location = new System.Drawing.Point(8, 66);
            this.ExpansionLootitemSetAllChanceButton.Name = "ExpansionLootitemSetAllChanceButton";
            this.ExpansionLootitemSetAllChanceButton.Padding = new System.Windows.Forms.Padding(5);
            this.ExpansionLootitemSetAllChanceButton.Size = new System.Drawing.Size(261, 23);
            this.ExpansionLootitemSetAllChanceButton.TabIndex = 224;
            this.ExpansionLootitemSetAllChanceButton.Text = "Set All Selected Chance";
            this.ExpansionLootitemSetAllChanceButton.Click += new System.EventHandler(this.ExpansionLootitemSetAllChanceButton_Click);
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(172, 43);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(15, 13);
            this.darkLabel1.TabIndex = 223;
            this.darkLabel1.Text = "%";
            // 
            // darkLabel2
            // 
            this.darkLabel2.AutoSize = true;
            this.darkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel2.Location = new System.Drawing.Point(4, 22);
            this.darkLabel2.Name = "darkLabel2";
            this.darkLabel2.Size = new System.Drawing.Size(50, 13);
            this.darkLabel2.TabIndex = 215;
            this.darkLabel2.Text = "Chance :";
            // 
            // trackBar2
            // 
            this.trackBar2.LargeChange = 1;
            this.trackBar2.Location = new System.Drawing.Point(82, 13);
            this.trackBar2.Maximum = 100;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(191, 45);
            this.trackBar2.TabIndex = 216;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            this.trackBar2.ValueChanged += new System.EventHandler(this.trackBar2_ValueChanged);
            this.trackBar2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar2_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLootItemsToolStripMenuItem,
            this.addLootVariantsToolStripMenuItem,
            this.addLootAttachmentsToolStripMenuItem,
            this.removeLootItemToolStripMenuItem,
            this.removeLootAttachemntToolStripMenuItem,
            this.removeLootVariantToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 136);
            // 
            // addLootItemsToolStripMenuItem
            // 
            this.addLootItemsToolStripMenuItem.Name = "addLootItemsToolStripMenuItem";
            this.addLootItemsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.addLootItemsToolStripMenuItem.Text = "Add Loot Items";
            this.addLootItemsToolStripMenuItem.Click += new System.EventHandler(this.addLootItemsToolStripMenuItem_Click);
            // 
            // addLootVariantsToolStripMenuItem
            // 
            this.addLootVariantsToolStripMenuItem.Name = "addLootVariantsToolStripMenuItem";
            this.addLootVariantsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.addLootVariantsToolStripMenuItem.Text = "Add Loot Variants";
            this.addLootVariantsToolStripMenuItem.Click += new System.EventHandler(this.addLootVariantsToolStripMenuItem_Click);
            // 
            // addLootAttachmentsToolStripMenuItem
            // 
            this.addLootAttachmentsToolStripMenuItem.Name = "addLootAttachmentsToolStripMenuItem";
            this.addLootAttachmentsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.addLootAttachmentsToolStripMenuItem.Text = "Add Loot Attachments";
            this.addLootAttachmentsToolStripMenuItem.Click += new System.EventHandler(this.addLootAttachmentsToolStripMenuItem_Click);
            // 
            // removeLootItemToolStripMenuItem
            // 
            this.removeLootItemToolStripMenuItem.Name = "removeLootItemToolStripMenuItem";
            this.removeLootItemToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.removeLootItemToolStripMenuItem.Text = "Remove Loot Item";
            this.removeLootItemToolStripMenuItem.Click += new System.EventHandler(this.removeLootItemToolStripMenuItem_Click);
            // 
            // removeLootAttachemntToolStripMenuItem
            // 
            this.removeLootAttachemntToolStripMenuItem.Name = "removeLootAttachemntToolStripMenuItem";
            this.removeLootAttachemntToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.removeLootAttachemntToolStripMenuItem.Text = "Remove Loot Attachemnt";
            this.removeLootAttachemntToolStripMenuItem.Click += new System.EventHandler(this.removeLootAttachemntToolStripMenuItem_Click);
            // 
            // removeLootVariantToolStripMenuItem
            // 
            this.removeLootVariantToolStripMenuItem.Name = "removeLootVariantToolStripMenuItem";
            this.removeLootVariantToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.removeLootVariantToolStripMenuItem.Text = "Remove Loot Variant";
            this.removeLootVariantToolStripMenuItem.Click += new System.EventHandler(this.removeLootVariantToolStripMenuItem_Click);
            // 
            // ExpansionLootControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "ExpansionLootControl";
            this.Size = new System.Drawing.Size(561, 510);
            this.groupBox1.ResumeLayout(false);
            this.expansionLootItemGB.ResumeLayout(false);
            this.expansionLootItemGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown31)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown33)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown12)).EndInit();
            this.expansionLootVarientGB.ResumeLayout(false);
            this.expansionLootVarientGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private TreeViewMS.TreeViewMS ExpansionLootTV;
        private System.Windows.Forms.NumericUpDown numericUpDown31;
        private DarkUI.Controls.DarkLabel darkLabel251;
        private System.Windows.Forms.NumericUpDown numericUpDown12;
        private DarkUI.Controls.DarkLabel darkLabel12;
        private System.Windows.Forms.TrackBar trackBar1;
        private DarkUI.Controls.DarkLabel darkLabel22;
        private System.Windows.Forms.NumericUpDown numericUpDown33;
        private DarkUI.Controls.DarkLabel darkLabel220;
        private DarkUI.Controls.DarkLabel darkLabel23;
        private System.Windows.Forms.GroupBox expansionLootVarientGB;
        private DarkUI.Controls.DarkLabel darkLabel1;
        private DarkUI.Controls.DarkLabel darkLabel2;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.GroupBox expansionLootItemGB;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addLootItemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addLootVariantsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addLootAttachmentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeLootItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeLootAttachemntToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeLootVariantToolStripMenuItem;
        private DarkUI.Controls.DarkButton ExpansionLootitemSetAllChanceButton;
        private DarkUI.Controls.DarkButton ExpansionLootitemSetAllRandomChanceButton;
    }
}
