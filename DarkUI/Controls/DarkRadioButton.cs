using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x02000037 RID: 55
	public class DarkRadioButton : RadioButton
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000CA24 File Offset: 0x0000AC24
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Appearance Appearance
		{
			get
			{
				return base.Appearance;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000CA3C File Offset: 0x0000AC3C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AutoEllipsis
		{
			get
			{
				return base.AutoEllipsis;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000CA54 File Offset: 0x0000AC54
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000CA6C File Offset: 0x0000AC6C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000CA84 File Offset: 0x0000AC84
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool FlatAppearance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000CA98 File Offset: 0x0000AC98
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new FlatStyle FlatStyle
		{
			get
			{
				return base.FlatStyle;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000CAB0 File Offset: 0x0000ACB0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image Image
		{
			get
			{
				return base.Image;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000CAC8 File Offset: 0x0000ACC8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContentAlignment ImageAlign
		{
			get
			{
				return base.ImageAlign;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000CAE0 File Offset: 0x0000ACE0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int ImageIndex
		{
			get
			{
				return base.ImageIndex;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000CAF8 File Offset: 0x0000ACF8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string ImageKey
		{
			get
			{
				return base.ImageKey;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000CB10 File Offset: 0x0000AD10
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ImageList ImageList
		{
			get
			{
				return base.ImageList;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000CB28 File Offset: 0x0000AD28
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContentAlignment TextAlign
		{
			get
			{
				return base.TextAlign;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000CB40 File Offset: 0x0000AD40
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new TextImageRelation TextImageRelation
		{
			get
			{
				return base.TextImageRelation;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000CB58 File Offset: 0x0000AD58
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool UseCompatibleTextRendering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000CB6C File Offset: 0x0000AD6C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool UseVisualStyleBackColor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000CB7F File Offset: 0x0000AD7F
		public DarkRadioButton()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000CBA0 File Offset: 0x0000ADA0
		private void SetControlState(DarkControlState controlState)
		{
			bool flag = this._controlState != controlState;
			if (flag)
			{
				this._controlState = controlState;
				base.Invalidate();
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000CBD0 File Offset: 0x0000ADD0
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			bool spacePressed = this._spacePressed;
			if (!spacePressed)
			{
				bool flag = e.Button == MouseButtons.Left;
				if (flag)
				{
					bool flag2 = base.ClientRectangle.Contains(e.Location);
					if (flag2)
					{
						this.SetControlState(DarkControlState.Pressed);
					}
					else
					{
						this.SetControlState(DarkControlState.Hover);
					}
				}
				else
				{
					this.SetControlState(DarkControlState.Hover);
				}
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000CC3C File Offset: 0x0000AE3C
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			bool flag = !base.ClientRectangle.Contains(e.Location);
			if (!flag)
			{
				this.SetControlState(DarkControlState.Pressed);
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000CC78 File Offset: 0x0000AE78
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			bool spacePressed = this._spacePressed;
			if (!spacePressed)
			{
				this.SetControlState(DarkControlState.Normal);
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000CCA4 File Offset: 0x0000AEA4
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			bool spacePressed = this._spacePressed;
			if (!spacePressed)
			{
				this.SetControlState(DarkControlState.Normal);
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000CCD0 File Offset: 0x0000AED0
		protected override void OnMouseCaptureChanged(EventArgs e)
		{
			base.OnMouseCaptureChanged(e);
			bool spacePressed = this._spacePressed;
			if (!spacePressed)
			{
				Point location = Cursor.Position;
				bool flag = !base.ClientRectangle.Contains(location);
				if (flag)
				{
					this.SetControlState(DarkControlState.Normal);
				}
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000A472 File Offset: 0x00008672
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			base.Invalidate();
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000CD18 File Offset: 0x0000AF18
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this._spacePressed = false;
			Point location = Cursor.Position;
			bool flag = !base.ClientRectangle.Contains(location);
			if (flag)
			{
				this.SetControlState(DarkControlState.Normal);
			}
			else
			{
				this.SetControlState(DarkControlState.Hover);
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000CD64 File Offset: 0x0000AF64
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(0, 0, base.ClientSize.Width, base.ClientSize.Height);
			int size = Consts.RadioButtonSize;
			Color textColor = Colors.LightText;
			Color borderColor = Colors.LightText;
			Color fillColor = Colors.LightestBackground;
			bool enabled = base.Enabled;
			if (enabled)
			{
				bool focused = this.Focused;
				if (focused)
				{
					borderColor = Colors.BlueHighlight;
					fillColor = Colors.BlueSelection;
				}
				bool flag = this._controlState == DarkControlState.Hover;
				if (flag)
				{
					borderColor = Colors.BlueHighlight;
					fillColor = Colors.BlueSelection;
				}
				else
				{
					bool flag2 = this._controlState == DarkControlState.Pressed;
					if (flag2)
					{
						borderColor = Colors.GreyHighlight;
						fillColor = Colors.GreySelection;
					}
				}
			}
			else
			{
				textColor = Colors.DisabledText;
				borderColor = Colors.GreyHighlight;
				fillColor = Colors.GreySelection;
			}
			using (SolidBrush b = new SolidBrush(Colors.GreyBackground))
			{
				g.FillRectangle(b, rect);
			}
			g.SmoothingMode = SmoothingMode.HighQuality;
			using (Pen p = new Pen(borderColor))
			{
				Rectangle boxRect = new Rectangle(0, rect.Height / 2 - size / 2, size, size);
				g.DrawEllipse(p, boxRect);
			}
			bool @checked = base.Checked;
			if (@checked)
			{
				using (SolidBrush b2 = new SolidBrush(fillColor))
				{
					Rectangle boxRect2 = new Rectangle(3, rect.Height / 2 - (size - 7) / 2 - 1, size - 6, size - 6);
					g.FillEllipse(b2, boxRect2);
				}
			}
			g.SmoothingMode = SmoothingMode.Default;
			using (SolidBrush b3 = new SolidBrush(textColor))
			{
				StringFormat stringFormat = new StringFormat
				{
					LineAlignment = StringAlignment.Center,
					Alignment = StringAlignment.Near
				};
				Rectangle modRect = new Rectangle(size + 4, 0, rect.Width - size, rect.Height);
				g.DrawString(this.Text, this.Font, b3, modRect, stringFormat);
			}
		}

		// Token: 0x040001BD RID: 445
		private DarkControlState _controlState = DarkControlState.Normal;

		// Token: 0x040001BE RID: 446
		private bool _spacePressed;
	}
}
