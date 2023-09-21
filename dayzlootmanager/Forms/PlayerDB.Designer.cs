
namespace DayZeEditor
{
    partial class PlayerDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerDB));
            this.darkToolStrip21 = new DarkUI.Controls.DarkToolStrip2();
            this.SaveFileButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PlayerDBTablesLB = new System.Windows.Forms.ListBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.darkToolStrip21.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
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
            this.darkToolStrip21.Size = new System.Drawing.Size(1220, 45);
            this.darkToolStrip21.TabIndex = 48;
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
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PlayerDBTablesLB);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Location = new System.Drawing.Point(12, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 422);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Player Db Tables";
            // 
            // PlayerDBTablesLB
            // 
            this.PlayerDBTablesLB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.PlayerDBTablesLB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.PlayerDBTablesLB.ForeColor = System.Drawing.SystemColors.Control;
            this.PlayerDBTablesLB.FormattingEnabled = true;
            this.PlayerDBTablesLB.Location = new System.Drawing.Point(6, 19);
            this.PlayerDBTablesLB.Name = "PlayerDBTablesLB";
            this.PlayerDBTablesLB.Size = new System.Drawing.Size(249, 394);
            this.PlayerDBTablesLB.TabIndex = 92;
            this.PlayerDBTablesLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_DrawItem);
            this.PlayerDBTablesLB.SelectedIndexChanged += new System.EventHandler(this.PlayerDBTablesLB_SelectedIndexChanged);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(281, 67);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(32, 15, 32, 15);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(678, 403);
            this.dataGridView.TabIndex = 50;
            // 
            // PlayerDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 697);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.darkToolStrip21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PlayerDB";
            this.Text = "BaseBuildingPlus";
            this.Load += new System.EventHandler(this.PlayerDB_Load);
            this.darkToolStrip21.ResumeLayout(false);
            this.darkToolStrip21.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkToolStrip2 darkToolStrip21;
        private System.Windows.Forms.ToolStripButton SaveFileButton;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox PlayerDBTablesLB;
        private System.Windows.Forms.DataGridView dataGridView;
    }
}