
namespace DayZeEditor
{
    partial class ABVManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ABVManager));
            this.darkToolStrip21 = new DarkUI.Controls.DarkToolStrip2();
            this.SaveFileButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.LifetimeNUD = new System.Windows.Forms.NumericUpDown();
            this.darkLabel35 = new DarkUI.Controls.DarkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.darkLabel3 = new DarkUI.Controls.DarkLabel();
            this.LoggingCB = new System.Windows.Forms.CheckBox();
            this.SaveIntervalNUD = new System.Windows.Forms.NumericUpDown();
            this.darkLabel2 = new DarkUI.Controls.DarkLabel();
            this.UpdateIntervalNUD = new System.Windows.Forms.NumericUpDown();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.darkToolStrip21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LifetimeNUD)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SaveIntervalNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateIntervalNUD)).BeginInit();
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
            // LifetimeNUD
            // 
            this.LifetimeNUD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.LifetimeNUD.ForeColor = System.Drawing.SystemColors.Control;
            this.LifetimeNUD.Location = new System.Drawing.Point(111, 19);
            this.LifetimeNUD.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.LifetimeNUD.Name = "LifetimeNUD";
            this.LifetimeNUD.Size = new System.Drawing.Size(106, 20);
            this.LifetimeNUD.TabIndex = 156;
            this.LifetimeNUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.LifetimeNUD.ValueChanged += new System.EventHandler(this.LifetimeNUD_ValueChanged);
            // 
            // darkLabel35
            // 
            this.darkLabel35.AutoSize = true;
            this.darkLabel35.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel35.Location = new System.Drawing.Point(16, 21);
            this.darkLabel35.Name = "darkLabel35";
            this.darkLabel35.Size = new System.Drawing.Size(43, 13);
            this.darkLabel35.TabIndex = 157;
            this.darkLabel35.Text = "Lifetime";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.darkLabel3);
            this.groupBox1.Controls.Add(this.LoggingCB);
            this.groupBox1.Controls.Add(this.SaveIntervalNUD);
            this.groupBox1.Controls.Add(this.darkLabel2);
            this.groupBox1.Controls.Add(this.UpdateIntervalNUD);
            this.groupBox1.Controls.Add(this.darkLabel1);
            this.groupBox1.Controls.Add(this.LifetimeNUD);
            this.groupBox1.Controls.Add(this.darkLabel35);
            this.groupBox1.Location = new System.Drawing.Point(12, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(229, 133);
            this.groupBox1.TabIndex = 158;
            this.groupBox1.TabStop = false;
            // 
            // darkLabel3
            // 
            this.darkLabel3.AutoSize = true;
            this.darkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel3.Location = new System.Drawing.Point(16, 100);
            this.darkLabel3.Name = "darkLabel3";
            this.darkLabel3.Size = new System.Drawing.Size(45, 13);
            this.darkLabel3.TabIndex = 163;
            this.darkLabel3.Text = "Logging";
            // 
            // LoggingCB
            // 
            this.LoggingCB.AutoSize = true;
            this.LoggingCB.ForeColor = System.Drawing.SystemColors.Control;
            this.LoggingCB.Location = new System.Drawing.Point(111, 100);
            this.LoggingCB.Name = "LoggingCB";
            this.LoggingCB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.LoggingCB.Size = new System.Drawing.Size(15, 14);
            this.LoggingCB.TabIndex = 162;
            this.LoggingCB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LoggingCB.UseVisualStyleBackColor = true;
            this.LoggingCB.CheckedChanged += new System.EventHandler(this.LoggingCB_CheckedChanged);
            // 
            // SaveIntervalNUD
            // 
            this.SaveIntervalNUD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.SaveIntervalNUD.ForeColor = System.Drawing.SystemColors.Control;
            this.SaveIntervalNUD.Location = new System.Drawing.Point(111, 71);
            this.SaveIntervalNUD.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.SaveIntervalNUD.Name = "SaveIntervalNUD";
            this.SaveIntervalNUD.Size = new System.Drawing.Size(106, 20);
            this.SaveIntervalNUD.TabIndex = 160;
            this.SaveIntervalNUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SaveIntervalNUD.ValueChanged += new System.EventHandler(this.SaveIntervalNUD_ValueChanged);
            // 
            // darkLabel2
            // 
            this.darkLabel2.AutoSize = true;
            this.darkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel2.Location = new System.Drawing.Point(16, 73);
            this.darkLabel2.Name = "darkLabel2";
            this.darkLabel2.Size = new System.Drawing.Size(70, 13);
            this.darkLabel2.TabIndex = 161;
            this.darkLabel2.Text = "Save Interval";
            // 
            // UpdateIntervalNUD
            // 
            this.UpdateIntervalNUD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.UpdateIntervalNUD.ForeColor = System.Drawing.SystemColors.Control;
            this.UpdateIntervalNUD.Location = new System.Drawing.Point(111, 45);
            this.UpdateIntervalNUD.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.UpdateIntervalNUD.Name = "UpdateIntervalNUD";
            this.UpdateIntervalNUD.Size = new System.Drawing.Size(106, 20);
            this.UpdateIntervalNUD.TabIndex = 158;
            this.UpdateIntervalNUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UpdateIntervalNUD.ValueChanged += new System.EventHandler(this.UpdateIntervalNUD_ValueChanged);
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(16, 47);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(80, 13);
            this.darkLabel1.TabIndex = 159;
            this.darkLabel1.Text = "Update Interval";
            // 
            // ABVManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.darkToolStrip21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ABVManager";
            this.Text = "ABVManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ABVManager_FormClosing);
            this.Load += new System.EventHandler(this.ABVManager_Load);
            this.darkToolStrip21.ResumeLayout(false);
            this.darkToolStrip21.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LifetimeNUD)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SaveIntervalNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateIntervalNUD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkToolStrip2 darkToolStrip21;
        private System.Windows.Forms.ToolStripButton SaveFileButton;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.NumericUpDown LifetimeNUD;
        private DarkUI.Controls.DarkLabel darkLabel35;
        private System.Windows.Forms.GroupBox groupBox1;
        private DarkUI.Controls.DarkLabel darkLabel3;
        private System.Windows.Forms.CheckBox LoggingCB;
        private System.Windows.Forms.NumericUpDown SaveIntervalNUD;
        private DarkUI.Controls.DarkLabel darkLabel2;
        private System.Windows.Forms.NumericUpDown UpdateIntervalNUD;
        private DarkUI.Controls.DarkLabel darkLabel1;
    }
}