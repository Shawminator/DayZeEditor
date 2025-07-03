
namespace DayZeEditor
{
    partial class PVZCZManager
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PVZCZManager));
            this.darkToolStrip21 = new DarkUI.Controls.DarkToolStrip2();
            this.SaveFileButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.PVZModTV = new TreeViewMS.TreeViewMS();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.darkButton8 = new DarkUI.Controls.DarkButton();
            this.darkButton9 = new DarkUI.Controls.DarkButton();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBoxAttributeName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxComment = new System.Windows.Forms.Label();
            this.textBoxAttributeValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSelectedZombieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewCustomZoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSelectedCutomZoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewSafeZoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSelectedSafeZoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkToolStrip21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // darkToolStrip21
            // 
            this.darkToolStrip21.AutoSize = false;
            this.darkToolStrip21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.darkToolStrip21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkToolStrip21.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveFileButton,
            this.toolStripButton2});
            this.darkToolStrip21.Location = new System.Drawing.Point(0, 0);
            this.darkToolStrip21.Name = "darkToolStrip21";
            this.darkToolStrip21.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.darkToolStrip21.Size = new System.Drawing.Size(800, 45);
            this.darkToolStrip21.TabIndex = 49;
            this.darkToolStrip21.Text = "darkToolStrip21";
            // 
            // SaveFileButton
            // 
            this.SaveFileButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.SaveFileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveFileButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(110)))), ((int)(((byte)(175)))));
            this.SaveFileButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveFileButton.Image")));
            this.SaveFileButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SaveFileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveFileButton.Name = "SaveFileButton";
            this.SaveFileButton.Size = new System.Drawing.Size(42, 42);
            this.SaveFileButton.Text = "toolStripButton1";
            this.SaveFileButton.ToolTipText = "Save Dirty Files";
            this.SaveFileButton.Click += new System.EventHandler(this.SaveFileButton_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(110)))), ((int)(((byte)(175)))));
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(42, 42);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.ToolTipText = "Open Settings folder";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // PVZModTV
            // 
            this.PVZModTV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.PVZModTV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PVZModTV.ForeColor = System.Drawing.SystemColors.Control;
            this.PVZModTV.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.PVZModTV.Location = new System.Drawing.Point(0, 0);
            this.PVZModTV.Name = "PVZModTV";
            this.PVZModTV.SelectedNodes = ((System.Collections.ArrayList)(resources.GetObject("PVZModTV.SelectedNodes")));
            this.PVZModTV.SetMultiselect = false;
            this.PVZModTV.Size = new System.Drawing.Size(266, 405);
            this.PVZModTV.TabIndex = 244;
            this.PVZModTV.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.PVZModTV_AfterSelect);
            this.PVZModTV.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.PVZModTV_NodeMouseClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 45);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.PVZModTV);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(800, 405);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 245;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(524, 405);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.numericUpDown2);
            this.groupBox1.Controls.Add(this.darkButton8);
            this.groupBox1.Controls.Add(this.darkButton9);
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.textBoxAttributeName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxComment);
            this.groupBox1.Controls.Add(this.textBoxAttributeValue);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(439, 270);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.comboBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 43);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(403, 21);
            this.comboBox1.TabIndex = 203;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.numericUpDown2.ForeColor = System.Drawing.SystemColors.Control;
            this.numericUpDown2.Location = new System.Drawing.Point(6, 103);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(105, 20);
            this.numericUpDown2.TabIndex = 202;
            this.numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // darkButton8
            // 
            this.darkButton8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.darkButton8.Location = new System.Drawing.Point(256, 211);
            this.darkButton8.Name = "darkButton8";
            this.darkButton8.Padding = new System.Windows.Forms.Padding(5);
            this.darkButton8.Size = new System.Drawing.Size(153, 23);
            this.darkButton8.TabIndex = 201;
            this.darkButton8.Text = "Add From Types";
            this.darkButton8.Click += new System.EventHandler(this.darkButton8_Click);
            // 
            // darkButton9
            // 
            this.darkButton9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.darkButton9.Location = new System.Drawing.Point(256, 240);
            this.darkButton9.Name = "darkButton9";
            this.darkButton9.Padding = new System.Windows.Forms.Padding(5);
            this.darkButton9.Size = new System.Drawing.Size(153, 23);
            this.darkButton9.TabIndex = 200;
            this.darkButton9.Text = "Remove Selected";
            this.darkButton9.Click += new System.EventHandler(this.darkButton9_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(6, 129);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(244, 134);
            this.listBox1.TabIndex = 198;
            this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_DrawItem);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // textBoxAttributeName
            // 
            this.textBoxAttributeName.AutoSize = true;
            this.textBoxAttributeName.Location = new System.Drawing.Point(73, 27);
            this.textBoxAttributeName.Name = "textBoxAttributeName";
            this.textBoxAttributeName.Size = new System.Drawing.Size(57, 13);
            this.textBoxAttributeName.TabIndex = 4;
            this.textBoxAttributeName.Text = "Comment:-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Attribute:-";
            // 
            // textBoxComment
            // 
            this.textBoxComment.Location = new System.Drawing.Point(73, 66);
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Size = new System.Drawing.Size(336, 32);
            this.textBoxComment.TabIndex = 1;
            this.textBoxComment.Text = "Comment:-";
            // 
            // textBoxAttributeValue
            // 
            this.textBoxAttributeValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.textBoxAttributeValue.ForeColor = System.Drawing.SystemColors.Control;
            this.textBoxAttributeValue.Location = new System.Drawing.Point(6, 43);
            this.textBoxAttributeValue.Name = "textBoxAttributeValue";
            this.textBoxAttributeValue.Size = new System.Drawing.Size(403, 20);
            this.textBoxAttributeValue.TabIndex = 2;
            this.textBoxAttributeValue.TextChanged += new System.EventHandler(this.textBoxAttributeValue_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Comment:-";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewToolStripMenuItem,
            this.removeSelectedZombieToolStripMenuItem,
            this.addNewCustomZoneToolStripMenuItem,
            this.removeSelectedCutomZoneToolStripMenuItem,
            this.addNewSafeZoneToolStripMenuItem,
            this.removeSelectedSafeZoneToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(235, 136);
            // 
            // addNewToolStripMenuItem
            // 
            this.addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            this.addNewToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.addNewToolStripMenuItem.Text = "Add New Zombie";
            this.addNewToolStripMenuItem.Click += new System.EventHandler(this.addNewToolStripMenuItem_Click);
            // 
            // removeSelectedZombieToolStripMenuItem
            // 
            this.removeSelectedZombieToolStripMenuItem.Name = "removeSelectedZombieToolStripMenuItem";
            this.removeSelectedZombieToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.removeSelectedZombieToolStripMenuItem.Text = "Remove Selected Zombie";
            this.removeSelectedZombieToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedZombieToolStripMenuItem_Click);
            // 
            // addNewCustomZoneToolStripMenuItem
            // 
            this.addNewCustomZoneToolStripMenuItem.Name = "addNewCustomZoneToolStripMenuItem";
            this.addNewCustomZoneToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.addNewCustomZoneToolStripMenuItem.Text = "Add New Custom Zone";
            this.addNewCustomZoneToolStripMenuItem.Click += new System.EventHandler(this.addNewCustomZoneToolStripMenuItem_Click);
            // 
            // removeSelectedCutomZoneToolStripMenuItem
            // 
            this.removeSelectedCutomZoneToolStripMenuItem.Name = "removeSelectedCutomZoneToolStripMenuItem";
            this.removeSelectedCutomZoneToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.removeSelectedCutomZoneToolStripMenuItem.Text = "Remove Selected Cutom Zone";
            this.removeSelectedCutomZoneToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedCutomZoneToolStripMenuItem_Click);
            // 
            // addNewSafeZoneToolStripMenuItem
            // 
            this.addNewSafeZoneToolStripMenuItem.Name = "addNewSafeZoneToolStripMenuItem";
            this.addNewSafeZoneToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.addNewSafeZoneToolStripMenuItem.Text = "Add New Safe Zone";
            this.addNewSafeZoneToolStripMenuItem.Click += new System.EventHandler(this.addNewSafeZoneToolStripMenuItem_Click);
            // 
            // removeSelectedSafeZoneToolStripMenuItem
            // 
            this.removeSelectedSafeZoneToolStripMenuItem.Name = "removeSelectedSafeZoneToolStripMenuItem";
            this.removeSelectedSafeZoneToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.removeSelectedSafeZoneToolStripMenuItem.Text = "Remove Selected SafeZone";
            this.removeSelectedSafeZoneToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedSafeZoneToolStripMenuItem_Click);
            // 
            // PVZCZManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.darkToolStrip21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PVZCZManager";
            this.Text = "ABVManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ABVManager_FormClosing);
            this.Load += new System.EventHandler(this.PVZCZManager_Load);
            this.darkToolStrip21.ResumeLayout(false);
            this.darkToolStrip21.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkToolStrip2 darkToolStrip21;
        private System.Windows.Forms.ToolStripButton SaveFileButton;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private TreeViewMS.TreeViewMS PVZModTV;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox textBoxAttributeValue;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label textBoxComment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label textBoxAttributeName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSelectedZombieToolStripMenuItem;
        private System.Windows.Forms.ListBox listBox1;
        private DarkUI.Controls.DarkButton darkButton8;
        private DarkUI.Controls.DarkButton darkButton9;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ToolStripMenuItem addNewCustomZoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSelectedCutomZoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewSafeZoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSelectedSafeZoneToolStripMenuItem;
    }
}