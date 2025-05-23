﻿
namespace DayZeEditor
{
    partial class RagTysonBaseBuildingManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RagTysonBaseBuildingManager));
            this.darkToolStrip21 = new DarkUI.Controls.DarkToolStrip2();
            this.SaveFileButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.darkButton19 = new DarkUI.Controls.DarkButton();
            this.darkButton20 = new DarkUI.Controls.DarkButton();
            this.CurrentBBPListLB = new System.Windows.Forms.ListBox();
            this.BBPlistsLB = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.BBPCraftingCB = new System.Windows.Forms.CheckBox();
            this.BBPCraftingNUD = new System.Windows.Forms.NumericUpDown();
            this.BBPCraftingLB = new System.Windows.Forms.ListBox();
            this.BBPIntsNUD = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BBPBoolsCB = new System.Windows.Forms.CheckBox();
            this.BBPConfigLB = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.BBMaterialsNUD = new System.Windows.Forms.NumericUpDown();
            this.BBMaterailsLB = new System.Windows.Forms.ListBox();
            this.BBPDecimalNUD = new System.Windows.Forms.NumericUpDown();
            this.darkToolStrip21.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BBPCraftingNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BBPIntsNUD)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BBMaterialsNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BBPDecimalNUD)).BeginInit();
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
            this.darkToolStrip21.Size = new System.Drawing.Size(935, 45);
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.darkButton19);
            this.groupBox3.Controls.Add(this.darkButton20);
            this.groupBox3.Controls.Add(this.CurrentBBPListLB);
            this.groupBox3.Controls.Add(this.BBPlistsLB);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Location = new System.Drawing.Point(279, 48);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(211, 422);
            this.groupBox3.TabIndex = 103;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "RAG BB Tool Lists";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 241;
            this.label1.Text = "Items";
            // 
            // darkButton19
            // 
            this.darkButton19.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.darkButton19.Location = new System.Drawing.Point(185, 393);
            this.darkButton19.Name = "darkButton19";
            this.darkButton19.Padding = new System.Windows.Forms.Padding(5);
            this.darkButton19.Size = new System.Drawing.Size(20, 20);
            this.darkButton19.TabIndex = 240;
            this.darkButton19.Text = "-";
            this.darkButton19.Click += new System.EventHandler(this.darkButton19_Click);
            // 
            // darkButton20
            // 
            this.darkButton20.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.darkButton20.Location = new System.Drawing.Point(6, 393);
            this.darkButton20.Name = "darkButton20";
            this.darkButton20.Padding = new System.Windows.Forms.Padding(5);
            this.darkButton20.Size = new System.Drawing.Size(20, 20);
            this.darkButton20.TabIndex = 239;
            this.darkButton20.Text = "+";
            this.darkButton20.Click += new System.EventHandler(this.darkButton20_Click);
            // 
            // CurrentBBPListLB
            // 
            this.CurrentBBPListLB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.CurrentBBPListLB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CurrentBBPListLB.ForeColor = System.Drawing.SystemColors.Control;
            this.CurrentBBPListLB.FormattingEnabled = true;
            this.CurrentBBPListLB.Location = new System.Drawing.Point(6, 188);
            this.CurrentBBPListLB.Name = "CurrentBBPListLB";
            this.CurrentBBPListLB.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.CurrentBBPListLB.Size = new System.Drawing.Size(199, 199);
            this.CurrentBBPListLB.TabIndex = 91;
            this.CurrentBBPListLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_DrawItem);
            // 
            // BBPlistsLB
            // 
            this.BBPlistsLB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.BBPlistsLB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.BBPlistsLB.ForeColor = System.Drawing.SystemColors.Control;
            this.BBPlistsLB.FormattingEnabled = true;
            this.BBPlistsLB.Location = new System.Drawing.Point(6, 19);
            this.BBPlistsLB.Name = "BBPlistsLB";
            this.BBPlistsLB.Size = new System.Drawing.Size(199, 147);
            this.BBPlistsLB.TabIndex = 90;
            this.BBPlistsLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_DrawItem);
            this.BBPlistsLB.SelectedIndexChanged += new System.EventHandler(this.BBPlistsLB_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BBPCraftingCB);
            this.groupBox2.Controls.Add(this.BBPCraftingNUD);
            this.groupBox2.Controls.Add(this.BBPCraftingLB);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Location = new System.Drawing.Point(712, 48);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(211, 422);
            this.groupBox2.TabIndex = 102;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RAG BB  Crafting";
            // 
            // BBPCraftingCB
            // 
            this.BBPCraftingCB.AutoSize = true;
            this.BBPCraftingCB.Location = new System.Drawing.Point(69, 393);
            this.BBPCraftingCB.Name = "BBPCraftingCB";
            this.BBPCraftingCB.Size = new System.Drawing.Size(65, 17);
            this.BBPCraftingCB.TabIndex = 130;
            this.BBPCraftingCB.Text = "Enabled";
            this.BBPCraftingCB.UseVisualStyleBackColor = true;
            this.BBPCraftingCB.CheckedChanged += new System.EventHandler(this.BBPCraftingCB_CheckedChanged);
            // 
            // BBPCraftingNUD
            // 
            this.BBPCraftingNUD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.BBPCraftingNUD.DecimalPlaces = 6;
            this.BBPCraftingNUD.ForeColor = System.Drawing.SystemColors.Control;
            this.BBPCraftingNUD.Location = new System.Drawing.Point(6, 390);
            this.BBPCraftingNUD.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.BBPCraftingNUD.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.BBPCraftingNUD.Name = "BBPCraftingNUD";
            this.BBPCraftingNUD.Size = new System.Drawing.Size(199, 20);
            this.BBPCraftingNUD.TabIndex = 129;
            this.BBPCraftingNUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BBPCraftingNUD.ValueChanged += new System.EventHandler(this.BBPCraftingNUD_ValueChanged);
            // 
            // BBPCraftingLB
            // 
            this.BBPCraftingLB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.BBPCraftingLB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.BBPCraftingLB.ForeColor = System.Drawing.SystemColors.Control;
            this.BBPCraftingLB.FormattingEnabled = true;
            this.BBPCraftingLB.Location = new System.Drawing.Point(6, 19);
            this.BBPCraftingLB.Name = "BBPCraftingLB";
            this.BBPCraftingLB.Size = new System.Drawing.Size(199, 368);
            this.BBPCraftingLB.TabIndex = 90;
            this.BBPCraftingLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_DrawItem);
            this.BBPCraftingLB.SelectedIndexChanged += new System.EventHandler(this.BBPCraftingLB_SelectedIndexChanged);
            // 
            // BBPIntsNUD
            // 
            this.BBPIntsNUD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.BBPIntsNUD.ForeColor = System.Drawing.SystemColors.Control;
            this.BBPIntsNUD.Location = new System.Drawing.Point(6, 392);
            this.BBPIntsNUD.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.BBPIntsNUD.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.BBPIntsNUD.Name = "BBPIntsNUD";
            this.BBPIntsNUD.Size = new System.Drawing.Size(249, 20);
            this.BBPIntsNUD.TabIndex = 127;
            this.BBPIntsNUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BBPIntsNUD.ValueChanged += new System.EventHandler(this.BBPIntsNUD_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BBPBoolsCB);
            this.groupBox1.Controls.Add(this.BBPConfigLB);
            this.groupBox1.Controls.Add(this.BBPDecimalNUD);
            this.groupBox1.Controls.Add(this.BBPIntsNUD);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Location = new System.Drawing.Point(12, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 422);
            this.groupBox1.TabIndex = 101;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RAG BB Config";
            // 
            // BBPBoolsCB
            // 
            this.BBPBoolsCB.AutoSize = true;
            this.BBPBoolsCB.Location = new System.Drawing.Point(93, 394);
            this.BBPBoolsCB.Name = "BBPBoolsCB";
            this.BBPBoolsCB.Size = new System.Drawing.Size(65, 17);
            this.BBPBoolsCB.TabIndex = 93;
            this.BBPBoolsCB.Text = "Enabled";
            this.BBPBoolsCB.UseVisualStyleBackColor = true;
            this.BBPBoolsCB.CheckedChanged += new System.EventHandler(this.BBPBoolsCB_CheckedChanged);
            // 
            // BBPConfigLB
            // 
            this.BBPConfigLB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.BBPConfigLB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.BBPConfigLB.ForeColor = System.Drawing.SystemColors.Control;
            this.BBPConfigLB.FormattingEnabled = true;
            this.BBPConfigLB.Location = new System.Drawing.Point(6, 19);
            this.BBPConfigLB.Name = "BBPConfigLB";
            this.BBPConfigLB.Size = new System.Drawing.Size(249, 368);
            this.BBPConfigLB.TabIndex = 92;
            this.BBPConfigLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_DrawItem);
            this.BBPConfigLB.SelectedIndexChanged += new System.EventHandler(this.BBPConfigLB_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.BBMaterialsNUD);
            this.groupBox4.Controls.Add(this.BBMaterailsLB);
            this.groupBox4.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox4.Location = new System.Drawing.Point(496, 48);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(211, 422);
            this.groupBox4.TabIndex = 242;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "RAG BB Materials";
            // 
            // BBMaterialsNUD
            // 
            this.BBMaterialsNUD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.BBMaterialsNUD.ForeColor = System.Drawing.SystemColors.Control;
            this.BBMaterialsNUD.Location = new System.Drawing.Point(6, 391);
            this.BBMaterialsNUD.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.BBMaterialsNUD.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.BBMaterialsNUD.Name = "BBMaterialsNUD";
            this.BBMaterialsNUD.Size = new System.Drawing.Size(199, 20);
            this.BBMaterialsNUD.TabIndex = 128;
            this.BBMaterialsNUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BBMaterialsNUD.ValueChanged += new System.EventHandler(this.BBMaterialsNUD_ValueChanged);
            // 
            // BBMaterailsLB
            // 
            this.BBMaterailsLB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.BBMaterailsLB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.BBMaterailsLB.ForeColor = System.Drawing.SystemColors.Control;
            this.BBMaterailsLB.FormattingEnabled = true;
            this.BBMaterailsLB.Location = new System.Drawing.Point(6, 17);
            this.BBMaterailsLB.Name = "BBMaterailsLB";
            this.BBMaterailsLB.Size = new System.Drawing.Size(199, 368);
            this.BBMaterailsLB.TabIndex = 90;
            this.BBMaterailsLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_DrawItem);
            this.BBMaterailsLB.SelectedIndexChanged += new System.EventHandler(this.BBMaterailsLB_SelectedIndexChanged);
            // 
            // BBPDecimalNUD
            // 
            this.BBPDecimalNUD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.BBPDecimalNUD.DecimalPlaces = 6;
            this.BBPDecimalNUD.ForeColor = System.Drawing.SystemColors.Control;
            this.BBPDecimalNUD.Location = new System.Drawing.Point(6, 392);
            this.BBPDecimalNUD.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.BBPDecimalNUD.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.BBPDecimalNUD.Name = "BBPDecimalNUD";
            this.BBPDecimalNUD.Size = new System.Drawing.Size(249, 20);
            this.BBPDecimalNUD.TabIndex = 128;
            this.BBPDecimalNUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BBPDecimalNUD.ValueChanged += new System.EventHandler(this.BBPDecimalNUD_ValueChanged);
            // 
            // RagTysonBaseBuildingManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 481);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.darkToolStrip21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RagTysonBaseBuildingManager";
            this.Text = "ABVManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RagTysonBaseBuildingManager_FormClosing);
            this.Load += new System.EventHandler(this.RagTysonBaseBuildingManager_Load);
            this.darkToolStrip21.ResumeLayout(false);
            this.darkToolStrip21.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BBPCraftingNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BBPIntsNUD)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BBMaterialsNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BBPDecimalNUD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkToolStrip2 darkToolStrip21;
        private System.Windows.Forms.ToolStripButton SaveFileButton;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private DarkUI.Controls.DarkButton darkButton19;
        private DarkUI.Controls.DarkButton darkButton20;
        private System.Windows.Forms.ListBox CurrentBBPListLB;
        private System.Windows.Forms.ListBox BBPlistsLB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox BBPCraftingLB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox BBPBoolsCB;
        private System.Windows.Forms.ListBox BBPConfigLB;
        private System.Windows.Forms.NumericUpDown BBPIntsNUD;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown BBMaterialsNUD;
        private System.Windows.Forms.ListBox BBMaterailsLB;
        private System.Windows.Forms.NumericUpDown BBPDecimalNUD;
        private System.Windows.Forms.CheckBox BBPCraftingCB;
        private System.Windows.Forms.NumericUpDown BBPCraftingNUD;
    }
}