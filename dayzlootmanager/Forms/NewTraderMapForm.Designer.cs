
namespace DayZeEditor
{
    partial class NewTraderMapForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.listBox16 = new System.Windows.Forms.ListBox();
            this.darkButton22 = new DarkUI.Controls.DarkButton();
            this.darkButton26 = new DarkUI.Controls.DarkButton();
            this.panel1.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.CloseButton);
            this.panel1.Controls.Add(this.TitleLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(418, 28);
            this.panel1.TabIndex = 10;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.BackColor = System.Drawing.Color.Black;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseButton.ForeColor = System.Drawing.Color.DarkRed;
            this.CloseButton.Location = new System.Drawing.Point(374, -1);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(41, 28);
            this.CloseButton.TabIndex = 7;
            this.CloseButton.Text = "X";
            this.CloseButton.UseVisualStyleBackColor = false;
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(110)))), ((int)(((byte)(175)))));
            this.TitleLabel.Location = new System.Drawing.Point(5, 6);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(96, 15);
            this.TitleLabel.TabIndex = 6;
            this.TitleLabel.Text = "Add To Category";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(8, 34);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(127, 381);
            this.listBox1.TabIndex = 11;
            this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_DrawItem);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // textBox15
            // 
            this.textBox15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.textBox15.ForeColor = System.Drawing.SystemColors.Control;
            this.textBox15.Location = new System.Drawing.Point(176, 34);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(198, 20);
            this.textBox15.TabIndex = 13;
            this.textBox15.TextChanged += new System.EventHandler(this.textBox15_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(141, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "NPC";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.listBox16);
            this.groupBox12.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox12.Location = new System.Drawing.Point(144, 60);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(264, 354);
            this.groupBox12.TabIndex = 93;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "TraderList";
            // 
            // listBox16
            // 
            this.listBox16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.listBox16.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox16.ForeColor = System.Drawing.SystemColors.Control;
            this.listBox16.FormattingEnabled = true;
            this.listBox16.Location = new System.Drawing.Point(7, 17);
            this.listBox16.Name = "listBox16";
            this.listBox16.Size = new System.Drawing.Size(250, 329);
            this.listBox16.TabIndex = 0;
            this.listBox16.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_DrawItem);
            this.listBox16.SelectedIndexChanged += new System.EventHandler(this.listBox16_SelectedIndexChanged);
            // 
            // darkButton22
            // 
            this.darkButton22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.darkButton22.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.darkButton22.Location = new System.Drawing.Point(8, 421);
            this.darkButton22.Name = "darkButton22";
            this.darkButton22.Padding = new System.Windows.Forms.Padding(5);
            this.darkButton22.Size = new System.Drawing.Size(400, 23);
            this.darkButton22.TabIndex = 95;
            this.darkButton22.Text = "Create Trader Map Entry";
            // 
            // darkButton26
            // 
            this.darkButton26.Location = new System.Drawing.Point(380, 33);
            this.darkButton26.Name = "darkButton26";
            this.darkButton26.Padding = new System.Windows.Forms.Padding(5);
            this.darkButton26.Size = new System.Drawing.Size(28, 20);
            this.darkButton26.TabIndex = 99;
            this.darkButton26.Text = "+";
            this.darkButton26.Click += new System.EventHandler(this.darkButton26_Click);
            // 
            // NewTraderMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 450);
            this.Controls.Add(this.darkButton26);
            this.Controls.Add(this.darkButton22);
            this.Controls.Add(this.groupBox12);
            this.Controls.Add(this.textBox15);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NewTraderMapForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NewTraderMapForm";
            this.Load += new System.EventHandler(this.NewTraderMapForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBox15;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.ListBox listBox16;
        private DarkUI.Controls.DarkButton darkButton22;
        private DarkUI.Controls.DarkButton darkButton26;
    }
}