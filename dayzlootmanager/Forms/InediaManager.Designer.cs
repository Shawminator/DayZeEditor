
namespace DayZeEditor
{
    partial class InediaManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InediaManager));
            this.darkToolStrip21 = new DarkUI.Controls.DarkToolStrip2();
            this.SaveFileButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.InediaModTV = new TreeViewMS.TreeViewMS();
            this.darkToolStrip21.SuspendLayout();
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
            // InediaModTV
            // 
            this.InediaModTV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.InediaModTV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InediaModTV.ForeColor = System.Drawing.SystemColors.Control;
            this.InediaModTV.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.InediaModTV.Location = new System.Drawing.Point(0, 45);
            this.InediaModTV.Name = "InediaModTV";
            this.InediaModTV.SelectedNodes = ((System.Collections.ArrayList)(resources.GetObject("InediaModTV.SelectedNodes")));
            this.InediaModTV.SetMultiselect = false;
            this.InediaModTV.Size = new System.Drawing.Size(800, 405);
            this.InediaModTV.TabIndex = 245;
            // 
            // InediaManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.InediaModTV);
            this.Controls.Add(this.darkToolStrip21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "InediaManager";
            this.Text = "ABVManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IndeiaManager_FormClosing);
            this.Load += new System.EventHandler(this.InediaManager_Load);
            this.darkToolStrip21.ResumeLayout(false);
            this.darkToolStrip21.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkToolStrip2 darkToolStrip21;
        private System.Windows.Forms.ToolStripButton SaveFileButton;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private TreeViewMS.TreeViewMS InediaModTV;
    }
}