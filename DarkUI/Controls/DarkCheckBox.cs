using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x0200002E RID: 46
	public class DarkCheckBox : CheckBox
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000A168 File Offset: 0x00008368
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Appearance Appearance
		{
			get
			{
				return base.Appearance;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000A180 File Offset: 0x00008380
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AutoEllipsis
		{
			get
			{
				return base.AutoEllipsis;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000A198 File Offset: 0x00008398
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0000A1B0 File Offset: 0x000083B0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000A1C8 File Offset: 0x000083C8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool FlatAppearance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000A1DC File Offset: 0x000083DC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new FlatStyle FlatStyle
		{
			get
			{
				return base.FlatStyle;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000184 RID: 388 RVA: 0x0000A1F4 File Offset: 0x000083F4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image Image
		{
			get
			{
				return base.Image;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000A20C File Offset: 0x0000840C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContentAlignment ImageAlign
		{
			get
			{
				return base.ImageAlign;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000A224 File Offset: 0x00008424
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int ImageIndex
		{
			get
			{
				return base.ImageIndex;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000A23C File Offset: 0x0000843C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string ImageKey
		{
			get
			{
				return base.ImageKey;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000A254 File Offset: 0x00008454
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ImageList ImageList
		{
			get
			{
				return base.ImageList;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000A26C File Offset: 0x0000846C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContentAlignment TextAlign
		{
			get
			{
				return base.TextAlign;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0000A284 File Offset: 0x00008484
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new TextImageRelation TextImageRelation
		{
			get
			{
				return base.TextImageRelation;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000A29C File Offset: 0x0000849C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool ThreeState
		{
			get
			{
				return base.ThreeState;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0000A2B4 File Offset: 0x000084B4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool UseCompatibleTextRendering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0000A2C8 File Offset: 0x000084C8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool UseVisualStyleBackColor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000A2DB File Offset: 0x000084DB
		public DarkCheckBox()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000A2FC File Offset: 0x000084FC
		private void SetControlState(DarkControlState controlState)
		{
			bool flag = this._controlState != controlState;
			if (flag)
			{
				this._controlState = controlState;
				base.Invalidate();
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000A32C File Offset: 0x0000852C
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

		// Token: 0x06000191 RID: 401 RVA: 0x0000A398 File Offset: 0x00008598
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			bool flag = !base.ClientRectangle.Contains(e.Location);
			if (!flag)
			{
				this.SetControlState(DarkControlState.Pressed);
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000A3D4 File Offset: 0x000085D4
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			bool spacePressed = this._spacePressed;
			if (!spacePressed)
			{
				this.SetControlState(DarkControlState.Normal);
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000A400 File Offset: 0x00008600
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			bool spacePressed = this._spacePressed;
			if (!spacePressed)
			{
				this.SetControlState(DarkControlState.Normal);
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000A42C File Offset: 0x0000862C
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

		// Token: 0x06000195 RID: 405 RVA: 0x0000A472 File Offset: 0x00008672
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			base.Invalidate();
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000A484 File Offset: 0x00008684
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

		// Token: 0x06000197 RID: 407 RVA: 0x0000A4D0 File Offset: 0x000086D0
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			bool flag = e.KeyCode == Keys.Space;
			if (flag)
			{
				this._spacePressed = true;
				this.SetControlState(DarkControlState.Pressed);
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000A508 File Offset: 0x00008708
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
					this.SetControlState(DarkControlState.Normal);
				}
				else
				{
					this.SetControlState(DarkControlState.Hover);
				}
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000A564 File Offset: 0x00008764
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(0, 0, base.ClientSize.Width, base.ClientSize.Height);
			int size = Consts.CheckBoxSize;
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
			using (SolidBrush b = new SolidBrush(Colors.BlackHighlight))
			{
				g.FillRectangle(b, rect);
			}
			using (Pen p = new Pen(borderColor))
			{
				Rectangle boxRect = new Rectangle(0, rect.Height / 2 - size / 2, size, size);
				g.DrawRectangle(p, boxRect);
			}
			bool @checked = base.Checked;
			if (@checked)
			{
				using (SolidBrush b2 = new SolidBrush(fillColor))
				{
					Rectangle boxRect2 = new Rectangle(2, rect.Height / 2 - (size - 4) / 2, size - 3, size - 3);
					g.FillRectangle(b2, boxRect2);
				}
			}
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

		// Token: 0x04000197 RID: 407
		private DarkControlState _controlState = DarkControlState.Normal;

		// Token: 0x04000198 RID: 408
		private bool _spacePressed;
	}
}
