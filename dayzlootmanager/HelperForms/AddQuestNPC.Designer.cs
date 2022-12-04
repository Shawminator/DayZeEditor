
namespace DayZeEditor
{
    partial class AddQuestNPC
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
            this.darkButton1 = new DarkUI.Controls.DarkButton();
            this.QuestNPCLB = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(239, 28);
            this.panel1.TabIndex = 10;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.BackColor = System.Drawing.Color.Black;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseButton.ForeColor = System.Drawing.Color.DarkRed;
            this.CloseButton.Location = new System.Drawing.Point(195, -1);
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
            this.TitleLabel.Size = new System.Drawing.Size(91, 15);
            this.TitleLabel.TabIndex = 6;
            this.TitleLabel.Text = "Add Quest NPC";
            // 
            // darkButton1
            // 
            this.darkButton1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.darkButton1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.darkButton1.Location = new System.Drawing.Point(8, 421);
            this.darkButton1.Name = "darkButton1";
            this.darkButton1.Padding = new System.Windows.Forms.Padding(5);
            this.darkButton1.Size = new System.Drawing.Size(225, 23);
            this.darkButton1.TabIndex = 98;
            this.darkButton1.Text = "Select";
            this.darkButton1.Click += new System.EventHandler(this.darkButton1_Click);
            // 
            // QuestNPCLB
            // 
            this.QuestNPCLB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.QuestNPCLB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.QuestNPCLB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.QuestNPCLB.ForeColor = System.Drawing.SystemColors.Control;
            this.QuestNPCLB.FormattingEnabled = true;
            this.QuestNPCLB.Items.AddRange(new object[] {
            "CJ_LootChest_Brown_Small",
            "CJ_LootChest_Brown_Medium",
            "CJ_LootChest_Brown_Large",
            "CJ_LootChest_Green_Small",
            "CJ_LootChest_Green_Medium",
            "CJ_LootChest_Green_Large",
            "CJ_LootChest_Grey_Small",
            "CJ_LootChest_Grey_Medium",
            "CJ_LootChest_Grey_Large",
            "CJ_LootChest_Olive_Small",
            "CJ_LootChest_Olive_Medium",
            "CJ_LootChest_Olive_Large",
            "CJ_LootChest_Tan_Small",
            "CJ_LootChest_Tan_Medium",
            "CJ_LootChest_Tan_Large",
            "CJ_LootChest_Camo_Small",
            "CJ_LootChest_Camo_Medium",
            "CJ_LootChest_Camo_Large",
            "CJ_LootChest_Wood_Brown_Small",
            "CJ_LootChest_Wood_Brown_Medium",
            "CJ_LootChest_Wood_Brown_Large",
            "CJ_LootChest_Wood_Green_Small",
            "CJ_LootChest_Wood_Green_Medium",
            "CJ_LootChest_Wood_Green_Large",
            "CJ_LootChest_Wood_Brown_Tools_Small",
            "CJ_LootChest_Wood_Brown_Foods_Small",
            "CJ_LootChest_Wood_Brown_Medics_Small",
            "CJ_LootChest_Wood_Brown_Tools_Medium",
            "CJ_LootChest_Wood_Brown_Foods_Medium",
            "CJ_LootChest_Wood_Brown_Medics_Medium",
            "CJ_LootChest_Wood_Brown_Tools_Large",
            "CJ_LootChest_Wood_Brown_Foods_Large",
            "CJ_LootChest_Wood_Brown_Medics_Large",
            "CJ_LootChest_Wood_Green_Tools_Small",
            "CJ_LootChest_Wood_Green_Foods_Small",
            "CJ_LootChest_Wood_Green_Medics_Small",
            "CJ_LootChest_Wood_Green_Tools_Medium",
            "CJ_LootChest_Wood_Green_Foods_Medium",
            "CJ_LootChest_Wood_Green_Medics_Medium",
            "CJ_LootChest_Wood_Green_Tools_Large",
            "CJ_LootChest_Wood_Green_Foods_Large",
            "CJ_LootChest_Wood_Green_Medics_Large"});
            this.QuestNPCLB.Location = new System.Drawing.Point(8, 34);
            this.QuestNPCLB.Name = "QuestNPCLB";
            this.QuestNPCLB.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.QuestNPCLB.Size = new System.Drawing.Size(226, 381);
            this.QuestNPCLB.TabIndex = 97;
            this.QuestNPCLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_DrawItem);
            // 
            // AddQuestNPC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 453);
            this.Controls.Add(this.darkButton1);
            this.Controls.Add(this.QuestNPCLB);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddQuestNPC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddQuestNPC";
            this.Load += new System.EventHandler(this.AddQuestNPC_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label TitleLabel;
        private DarkUI.Controls.DarkButton darkButton1;
        private System.Windows.Forms.ListBox QuestNPCLB;
    }
}