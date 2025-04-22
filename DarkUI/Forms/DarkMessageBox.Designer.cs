namespace DarkUI.Forms
{
	// Token: 0x02000013 RID: 19
	public partial class DarkMessageBox : global::DarkUI.Forms.DarkDialog
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00005338 File Offset: 0x00003538
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005370 File Offset: 0x00003570
		private void InitializeComponent()
		{
			this.picIcon = new global::System.Windows.Forms.PictureBox();
			this.lblText = new global::DarkUI.Controls.DarkLabel();
			((global::System.ComponentModel.ISupportInitialize)this.picIcon).BeginInit();
			base.SuspendLayout();
			this.picIcon.Location = new global::System.Drawing.Point(10, 10);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new global::System.Drawing.Size(32, 32);
			this.picIcon.TabIndex = 3;
			this.picIcon.TabStop = false;
			this.lblText.ForeColor = global::System.Drawing.Color.FromArgb(220, 220, 220);
			this.lblText.Location = new global::System.Drawing.Point(50, 9);
			this.lblText.Name = "lblText";
			this.lblText.Size = new global::System.Drawing.Size(185, 15);
			this.lblText.TabIndex = 4;
			this.lblText.Text = "Something something something";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(7f, 15f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(244, 86);
			base.Controls.Add(this.lblText);
			base.Controls.Add(this.picIcon);
			this.Font = new global::System.Drawing.Font("Segoe UI", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DarkMessageBox";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Message box";
			base.Controls.SetChildIndex(this.picIcon, 0);
			base.Controls.SetChildIndex(this.lblText, 0);
			((global::System.ComponentModel.ISupportInitialize)this.picIcon).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04000127 RID: 295
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000128 RID: 296
		private global::System.Windows.Forms.PictureBox picIcon;

		// Token: 0x04000129 RID: 297
		private global::DarkUI.Controls.DarkLabel lblText;
	}
}
