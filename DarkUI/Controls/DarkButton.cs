using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x0200003F RID: 63
	[ToolboxBitmap(typeof(Button))]
	[DefaultEvent("Click")]
	public class DarkButton : Button
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x000118C8 File Offset: 0x0000FAC8
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x000118E0 File Offset: 0x0000FAE0
		public new string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				base.Invalidate();
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x000118F4 File Offset: 0x0000FAF4
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0001190C File Offset: 0x0000FB0C
		public new bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
				base.Invalidate();
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002CB RID: 715 RVA: 0x00011920 File Offset: 0x0000FB20
		// (set) Token: 0x060002CC RID: 716 RVA: 0x00011938 File Offset: 0x0000FB38
		[Category("Appearance")]
		[Description("Determines the style of the button.")]
		[DefaultValue(DarkButtonStyle.Normal)]
		public DarkButtonStyle ButtonStyle
		{
			get
			{
				return this._style;
			}
			set
			{
				this._style = value;
				base.Invalidate();
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0001194C File Offset: 0x0000FB4C
		// (set) Token: 0x060002CE RID: 718 RVA: 0x00011964 File Offset: 0x0000FB64
		[Category("Appearance")]
		[Description("Determines the amount of padding between the image and text.")]
		[DefaultValue(5)]
		public int ImagePadding
		{
			get
			{
				return this._imagePadding;
			}
			set
			{
				this._imagePadding = value;
				base.Invalidate();
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002CF RID: 719 RVA: 0x00011978 File Offset: 0x0000FB78
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AutoEllipsis
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0001198C File Offset: 0x0000FB8C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DarkControlState ButtonState
		{
			get
			{
				return this._buttonState;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x000119A4 File Offset: 0x0000FBA4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContentAlignment ImageAlign
		{
			get
			{
				return base.ImageAlign;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x000119BC File Offset: 0x0000FBBC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool FlatAppearance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x000119D0 File Offset: 0x0000FBD0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new FlatStyle FlatStyle
		{
			get
			{
				return base.FlatStyle;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x000119E8 File Offset: 0x0000FBE8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContentAlignment TextAlign
		{
			get
			{
				return base.TextAlign;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x00011A00 File Offset: 0x0000FC00
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool UseCompatibleTextRendering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x00011A14 File Offset: 0x0000FC14
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool UseVisualStyleBackColor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00011A28 File Offset: 0x0000FC28
		public DarkButton()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			base.UseVisualStyleBackColor = false;
			base.UseCompatibleTextRendering = false;
			this.SetButtonState(DarkControlState.Normal);
			base.Padding = new Padding(this._padding);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00011A98 File Offset: 0x0000FC98
		private void SetButtonState(DarkControlState buttonState)
		{
			bool flag = this._buttonState != buttonState;
			if (flag)
			{
				this._buttonState = buttonState;
				base.Invalidate();
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00011AC8 File Offset: 0x0000FCC8
		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			Form form = base.FindForm();
			bool flag = form != null;
			if (flag)
			{
				bool flag2 = form.AcceptButton == this;
				if (flag2)
				{
					this._isDefault = true;
				}
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00011B04 File Offset: 0x0000FD04
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
						this.SetButtonState(DarkControlState.Pressed);
					}
					else
					{
						this.SetButtonState(DarkControlState.Hover);
					}
				}
				else
				{
					this.SetButtonState(DarkControlState.Hover);
				}
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00011B70 File Offset: 0x0000FD70
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			bool flag = !base.ClientRectangle.Contains(e.Location);
			if (!flag)
			{
				this.SetButtonState(DarkControlState.Pressed);
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00011BAC File Offset: 0x0000FDAC
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			bool spacePressed = this._spacePressed;
			if (!spacePressed)
			{
				this.SetButtonState(DarkControlState.Normal);
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00011BD8 File Offset: 0x0000FDD8
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			bool spacePressed = this._spacePressed;
			if (!spacePressed)
			{
				this.SetButtonState(DarkControlState.Normal);
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00011C04 File Offset: 0x0000FE04
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
					this.SetButtonState(DarkControlState.Normal);
				}
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000A472 File Offset: 0x00008672
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			base.Invalidate();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00011C4C File Offset: 0x0000FE4C
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this._spacePressed = false;
			Point location = Cursor.Position;
			bool flag = !base.ClientRectangle.Contains(location);
			if (flag)
			{
				this.SetButtonState(DarkControlState.Normal);
			}
			else
			{
				this.SetButtonState(DarkControlState.Hover);
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00011C98 File Offset: 0x0000FE98
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			bool flag = e.KeyCode == Keys.Space;
			if (flag)
			{
				this._spacePressed = true;
				this.SetButtonState(DarkControlState.Pressed);
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00011CD0 File Offset: 0x0000FED0
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			bool flag = e.KeyCode == Keys.Space;
			if (flag)
			{
				this._spacePressed = false;
				Point location = Cursor.Position;
				bool flag2 = !base.ClientRectangle.Contains(location);
				if (flag2)
				{
					this.SetButtonState(DarkControlState.Normal);
				}
				else
				{
					this.SetButtonState(DarkControlState.Hover);
				}
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00011D2C File Offset: 0x0000FF2C
		public override void NotifyDefault(bool value)
		{
			base.NotifyDefault(value);
			bool flag = !base.DesignMode;
			if (!flag)
			{
				this._isDefault = value;
				base.Invalidate();
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00011D60 File Offset: 0x0000FF60
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(0, 0, base.ClientSize.Width, base.ClientSize.Height);
			Color textColor = Colors.LightText;
			Color borderColor = Colors.GreySelection;
			Color fillColor = (this._isDefault ? Colors.DarkBlueBackground : Colors.LightBackground);
			bool enabled = this.Enabled;
			if (enabled)
			{
				bool flag = this.ButtonStyle == DarkButtonStyle.Normal;
				if (flag)
				{
					bool flag2 = this.Focused && base.TabStop;
					if (flag2)
					{
						borderColor = Colors.BlackHighlight;
					}
					DarkControlState buttonState = this.ButtonState;
					DarkControlState darkControlState = buttonState;
					if (darkControlState != DarkControlState.Hover)
					{
						if (darkControlState == DarkControlState.Pressed)
						{
							fillColor = (this._isDefault ? Colors.DarkBackground : Colors.DarkBackground);
						}
					}
					else
					{
						fillColor = (this._isDefault ? Colors.BlueBackground : Colors.LighterBackground);
					}
				}
				else
				{
					bool flag3 = this.ButtonStyle == DarkButtonStyle.Flat;
					if (flag3)
					{
						switch (this.ButtonState)
						{
						case DarkControlState.Normal:
							fillColor = Colors.MediumBackground;
							break;
						case DarkControlState.Hover:
							fillColor = Colors.GreyBackground;
							break;
						case DarkControlState.Pressed:
							fillColor = Colors.DarkBackground;
							break;
						}
					}
				}
			}
			else
			{
				textColor = Colors.DisabledText;
				fillColor = Colors.DarkGreySelection;
			}
			using (SolidBrush b = new SolidBrush(fillColor))
			{
				g.FillRectangle(b, rect);
			}
			bool flag4 = this.ButtonStyle == DarkButtonStyle.Normal;
			if (flag4)
			{
				using (Pen p = new Pen(borderColor, 1f))
				{
					Rectangle modRect = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
					g.DrawRectangle(p, modRect);
				}
			}
			int textOffsetX = 0;
			int textOffsetY = 0;
			bool flag5 = base.Image != null;
			if (flag5)
			{
				SizeF stringSize = g.MeasureString(this.Text, this.Font, rect.Size);
				int x = base.ClientSize.Width / 2 - base.Image.Size.Width / 2;
				int y = base.ClientSize.Height / 2 - base.Image.Size.Height / 2;
				TextImageRelation textImageRelation = base.TextImageRelation;
				TextImageRelation textImageRelation2 = textImageRelation;
				switch (textImageRelation2)
				{
				case TextImageRelation.ImageAboveText:
					textOffsetY = base.Image.Size.Height / 2 + this.ImagePadding / 2;
					y -= (int)(stringSize.Height / 2f) + this.ImagePadding / 2;
					break;
				case TextImageRelation.TextAboveImage:
					textOffsetY = (base.Image.Size.Height / 2 + this.ImagePadding / 2) * -1;
					y += (int)(stringSize.Height / 2f) + this.ImagePadding / 2;
					break;
				case (TextImageRelation)3:
					break;
				case TextImageRelation.ImageBeforeText:
					textOffsetX = base.Image.Size.Width + this.ImagePadding * 2;
					x = this.ImagePadding;
					break;
				default:
					if (textImageRelation2 == TextImageRelation.TextBeforeImage)
					{
						x += (int)stringSize.Width;
					}
					break;
				}
				g.DrawImageUnscaled(base.Image, x, y);
			}
			using (SolidBrush b2 = new SolidBrush(textColor))
			{
				Rectangle modRect2 = new Rectangle(rect.Left + textOffsetX + base.Padding.Left, rect.Top + textOffsetY + base.Padding.Top, rect.Width - base.Padding.Horizontal, rect.Height - base.Padding.Vertical);
				StringFormat stringFormat = new StringFormat
				{
					LineAlignment = StringAlignment.Center,
					Alignment = StringAlignment.Center,
					Trimming = StringTrimming.EllipsisCharacter
				};
				g.DrawString(this.Text, this.Font, b2, modRect2, stringFormat);
			}
		}

		// Token: 0x040001FA RID: 506
		private DarkButtonStyle _style = DarkButtonStyle.Normal;

		// Token: 0x040001FB RID: 507
		private DarkControlState _buttonState = DarkControlState.Normal;

		// Token: 0x040001FC RID: 508
		private bool _isDefault;

		// Token: 0x040001FD RID: 509
		private bool _spacePressed;

		// Token: 0x040001FE RID: 510
		private int _padding = Consts.Padding / 2;

		// Token: 0x040001FF RID: 511
		private int _imagePadding = 5;
	}
}
