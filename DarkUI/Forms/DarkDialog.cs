using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Controls;

namespace DarkUI.Forms
{
	// Token: 0x02000010 RID: 16
	public partial class DarkDialog : DarkForm
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000043CC File Offset: 0x000025CC
		// (set) Token: 0x06000065 RID: 101 RVA: 0x000043E4 File Offset: 0x000025E4
		[Description("Determines the type of the dialog window.")]
		[DefaultValue(DarkDialogButton.Ok)]
		public DarkDialogButton DialogButtons
		{
			get
			{
				return this._dialogButtons;
			}
			set
			{
				bool flag = this._dialogButtons == value;
				if (!flag)
				{
					this._dialogButtons = value;
					this.SetButtons();
				}
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000066 RID: 102 RVA: 0x0000440F File Offset: 0x0000260F
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00004417 File Offset: 0x00002617
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int TotalButtonSize { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00004420 File Offset: 0x00002620
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00004438 File Offset: 0x00002638
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new IButtonControl AcceptButton
		{
			get
			{
				return base.AcceptButton;
			}
			private set
			{
				base.AcceptButton = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00004444 File Offset: 0x00002644
		// (set) Token: 0x0600006B RID: 107 RVA: 0x0000445C File Offset: 0x0000265C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new IButtonControl CancelButton
		{
			get
			{
				return base.CancelButton;
			}
			private set
			{
				base.CancelButton = value;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004468 File Offset: 0x00002668
		public DarkDialog()
		{
			this.InitializeComponent();
			this._buttons = new List<DarkButton> { this.btnAbort, this.btnRetry, this.btnIgnore, this.btnOk, this.btnCancel, this.btnClose, this.btnYes, this.btnNo };
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004505 File Offset: 0x00002705
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.SetButtons();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004518 File Offset: 0x00002718
		private void SetButtons()
		{
			foreach (DarkButton btn in this._buttons)
			{
				btn.Visible = false;
			}
			switch (this._dialogButtons)
			{
			case DarkDialogButton.Ok:
				this.ShowButton(this.btnOk, true);
				this.AcceptButton = this.btnOk;
				break;
			case DarkDialogButton.Close:
				this.ShowButton(this.btnClose, true);
				this.AcceptButton = this.btnClose;
				this.CancelButton = this.btnClose;
				break;
			case DarkDialogButton.OkCancel:
				this.ShowButton(this.btnOk, false);
				this.ShowButton(this.btnCancel, true);
				this.AcceptButton = this.btnOk;
				this.CancelButton = this.btnCancel;
				break;
			case DarkDialogButton.YesNo:
				this.ShowButton(this.btnYes, false);
				this.ShowButton(this.btnNo, true);
				this.AcceptButton = this.btnYes;
				this.CancelButton = this.btnNo;
				break;
			case DarkDialogButton.YesNoCancel:
				this.ShowButton(this.btnYes, false);
				this.ShowButton(this.btnNo, false);
				this.ShowButton(this.btnCancel, true);
				this.AcceptButton = this.btnYes;
				this.CancelButton = this.btnCancel;
				break;
			case DarkDialogButton.AbortRetryIgnore:
				this.ShowButton(this.btnAbort, false);
				this.ShowButton(this.btnRetry, false);
				this.ShowButton(this.btnIgnore, true);
				this.AcceptButton = this.btnAbort;
				this.CancelButton = this.btnIgnore;
				break;
			case DarkDialogButton.RetryCancel:
				this.ShowButton(this.btnRetry, false);
				this.ShowButton(this.btnCancel, true);
				this.AcceptButton = this.btnRetry;
				this.CancelButton = this.btnCancel;
				break;
			}
			this.SetFlowSize();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004730 File Offset: 0x00002930
		private void ShowButton(DarkButton button, bool isLast = false)
		{
			button.SendToBack();
			bool flag = !isLast;
			if (flag)
			{
				button.Margin = new Padding(0, 0, 10, 0);
			}
			button.Visible = true;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004768 File Offset: 0x00002968
		private void SetFlowSize()
		{
			int width = this.flowInner.Padding.Horizontal;
			foreach (DarkButton btn in this._buttons)
			{
				bool visible = btn.Visible;
				if (visible)
				{
					width += btn.Width + btn.Margin.Right;
				}
			}
			this.flowInner.Width = width;
			this.TotalButtonSize = width;
		}

		// Token: 0x0400010E RID: 270
		private DarkDialogButton _dialogButtons = DarkDialogButton.Ok;

		// Token: 0x0400010F RID: 271
		private List<DarkButton> _buttons;
	}
}
