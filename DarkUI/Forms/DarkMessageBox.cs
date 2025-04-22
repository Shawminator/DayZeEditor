using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Controls;

namespace DarkUI.Forms
{
	// Token: 0x02000013 RID: 19
	public partial class DarkMessageBox : DarkDialog
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00005034 File Offset: 0x00003234
		// (set) Token: 0x06000078 RID: 120 RVA: 0x0000504C File Offset: 0x0000324C
		[Description("Determines the maximum width of the message box when it autosizes around the displayed message.")]
		[DefaultValue(350)]
		public int MaximumWidth
		{
			get
			{
				return this._maximumWidth;
			}
			set
			{
				this._maximumWidth = value;
				this.CalculateSize();
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000505D File Offset: 0x0000325D
		public DarkMessageBox()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005080 File Offset: 0x00003280
		public DarkMessageBox(string message, string title, DarkMessageBoxIcon icon, DarkDialogButton buttons)
			: this()
		{
			this.Text = title;
			this._message = message;
			base.DialogButtons = buttons;
			this.SetIcon(icon);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000050AA File Offset: 0x000032AA
		public DarkMessageBox(string message)
			: this(message, null, DarkMessageBoxIcon.None, DarkDialogButton.Ok)
		{
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000050B8 File Offset: 0x000032B8
		public DarkMessageBox(string message, string title)
			: this(message, title, DarkMessageBoxIcon.None, DarkDialogButton.Ok)
		{
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000050C6 File Offset: 0x000032C6
		public DarkMessageBox(string message, string title, DarkDialogButton buttons)
			: this(message, title, DarkMessageBoxIcon.None, buttons)
		{
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000050D4 File Offset: 0x000032D4
		public DarkMessageBox(string message, string title, DarkMessageBoxIcon icon)
			: this(message, title, icon, DarkDialogButton.Ok)
		{
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000050E4 File Offset: 0x000032E4
		public static DialogResult ShowInformation(string message, string caption, DarkDialogButton buttons = DarkDialogButton.Ok)
		{
			return DarkMessageBox.ShowDialog(message, caption, DarkMessageBoxIcon.Information, buttons);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005100 File Offset: 0x00003300
		public static DialogResult ShowWarning(string message, string caption, DarkDialogButton buttons = DarkDialogButton.Ok)
		{
			return DarkMessageBox.ShowDialog(message, caption, DarkMessageBoxIcon.Warning, buttons);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000511C File Offset: 0x0000331C
		public static DialogResult ShowError(string message, string caption, DarkDialogButton buttons = DarkDialogButton.Ok)
		{
			return DarkMessageBox.ShowDialog(message, caption, DarkMessageBoxIcon.Error, buttons);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00005138 File Offset: 0x00003338
		private static DialogResult ShowDialog(string message, string caption, DarkMessageBoxIcon icon, DarkDialogButton buttons)
		{
			DialogResult dialogResult;
			using (DarkMessageBox dlg = new DarkMessageBox(message, caption, icon, buttons))
			{
				DialogResult result = dlg.ShowDialog();
				dialogResult = result;
			}
			return dialogResult;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00005178 File Offset: 0x00003378
		private void SetIcon(DarkMessageBoxIcon icon)
		{
			switch (icon)
			{
			case DarkMessageBoxIcon.None:
				this.picIcon.Visible = false;
				this.lblText.Left = 10;
				break;
			case DarkMessageBoxIcon.Information:
				this.picIcon.Image = MessageBoxIcons.info;
				break;
			case DarkMessageBoxIcon.Warning:
				this.picIcon.Image = MessageBoxIcons.warning;
				break;
			case DarkMessageBoxIcon.Error:
				this.picIcon.Image = MessageBoxIcons.error;
				break;
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000051F8 File Offset: 0x000033F8
		private void CalculateSize()
		{
			int width = 260;
			int height = 124;
			base.Size = new Size(width, height);
			this.lblText.Text = string.Empty;
			this.lblText.AutoSize = true;
			this.lblText.Text = this._message;
			int minWidth = Math.Max(width, base.TotalButtonSize + 15);
			int totalWidth = this.lblText.Right + 25;
			bool flag = totalWidth < this._maximumWidth;
			if (flag)
			{
				width = totalWidth;
				this.lblText.Top = this.picIcon.Top + this.picIcon.Height / 2 - this.lblText.Height / 2;
			}
			else
			{
				width = this._maximumWidth;
				int offsetHeight = base.Height - this.picIcon.Height;
				this.lblText.AutoUpdateHeight = true;
				this.lblText.Width = width - this.lblText.Left - 25;
				height = offsetHeight + this.lblText.Height;
			}
			bool flag2 = width < minWidth;
			if (flag2)
			{
				width = minWidth;
			}
			base.Size = new Size(width, height);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005323 File Offset: 0x00003523
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.CalculateSize();
		}

		// Token: 0x04000125 RID: 293
		private string _message;

		// Token: 0x04000126 RID: 294
		private int _maximumWidth = 350;
	}
}
