using System;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Forms;

namespace DarkUI.Docking
{
	// Token: 0x02000021 RID: 33
	public class DarkDockSplitter
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00009034 File Offset: 0x00007234
		// (set) Token: 0x06000113 RID: 275 RVA: 0x0000903C File Offset: 0x0000723C
		public Rectangle Bounds { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00009045 File Offset: 0x00007245
		// (set) Token: 0x06000115 RID: 277 RVA: 0x0000904D File Offset: 0x0000724D
		public Cursor ResizeCursor { get; private set; }

		// Token: 0x06000116 RID: 278 RVA: 0x00009058 File Offset: 0x00007258
		public DarkDockSplitter(Control parentControl, Control control, DarkSplitterType splitterType)
		{
			this._parentControl = parentControl;
			this._control = control;
			this._splitterType = splitterType;
			DarkSplitterType splitterType2 = this._splitterType;
			DarkSplitterType darkSplitterType = splitterType2;
			if (darkSplitterType > DarkSplitterType.Right)
			{
				if (darkSplitterType - DarkSplitterType.Top <= 1)
				{
					this.ResizeCursor = Cursors.SizeNS;
				}
			}
			else
			{
				this.ResizeCursor = Cursors.SizeWE;
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000090B5 File Offset: 0x000072B5
		public void ShowOverlay()
		{
			this._overlayForm = new DarkTranslucentForm(Color.Black, 0.6);
			this._overlayForm.Visible = true;
			this.UpdateOverlay(new Point(0, 0));
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000090EC File Offset: 0x000072EC
		public void HideOverlay()
		{
			this._overlayForm.Visible = false;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000090FC File Offset: 0x000072FC
		public void UpdateOverlay(Point difference)
		{
			Rectangle bounds = new Rectangle(this.Bounds.Location, this.Bounds.Size);
			switch (this._splitterType)
			{
			case DarkSplitterType.Left:
			{
				int leftX = Math.Max(bounds.Location.X - difference.X, this._minimum);
				bool flag = this._maximum != 0 && leftX > this._maximum;
				if (flag)
				{
					leftX = this._maximum;
				}
				bounds.Location = new Point(leftX, bounds.Location.Y);
				break;
			}
			case DarkSplitterType.Right:
			{
				int rightX = Math.Max(bounds.Location.X - difference.X, this._minimum);
				bool flag2 = this._maximum != 0 && rightX > this._maximum;
				if (flag2)
				{
					rightX = this._maximum;
				}
				bounds.Location = new Point(rightX, bounds.Location.Y);
				break;
			}
			case DarkSplitterType.Top:
			{
				int topY = Math.Max(bounds.Location.Y - difference.Y, this._minimum);
				bool flag3 = this._maximum != 0 && topY > this._maximum;
				if (flag3)
				{
					topY = this._maximum;
				}
				bounds.Location = new Point(bounds.Location.X, topY);
				break;
			}
			case DarkSplitterType.Bottom:
			{
				int bottomY = Math.Max(bounds.Location.Y - difference.Y, this._minimum);
				bool flag4 = this._maximum != 0 && bottomY > this._maximum;
				if (flag4)
				{
					int topY = this._maximum;
				}
				bounds.Location = new Point(bounds.Location.X, bottomY);
				break;
			}
			}
			this._overlayForm.Bounds = bounds;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00009304 File Offset: 0x00007504
		public void Move(Point difference)
		{
			switch (this._splitterType)
			{
			case DarkSplitterType.Left:
				this._control.Width += difference.X;
				break;
			case DarkSplitterType.Right:
				this._control.Width -= difference.X;
				break;
			case DarkSplitterType.Top:
				this._control.Height += difference.Y;
				break;
			case DarkSplitterType.Bottom:
				this._control.Height -= difference.Y;
				break;
			}
			this.UpdateBounds();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000093AC File Offset: 0x000075AC
		public void UpdateBounds()
		{
			Rectangle bounds = this._parentControl.RectangleToScreen(this._control.Bounds);
			switch (this._splitterType)
			{
			case DarkSplitterType.Left:
				this.Bounds = new Rectangle(bounds.Left - 2, bounds.Top, 5, bounds.Height);
				this._maximum = bounds.Right - 2 - this._control.MinimumSize.Width;
				break;
			case DarkSplitterType.Right:
				this.Bounds = new Rectangle(bounds.Right - 2, bounds.Top, 5, bounds.Height);
				this._minimum = bounds.Left - 2 + this._control.MinimumSize.Width;
				break;
			case DarkSplitterType.Top:
				this.Bounds = new Rectangle(bounds.Left, bounds.Top - 2, bounds.Width, 5);
				this._maximum = bounds.Bottom - 2 - this._control.MinimumSize.Height;
				break;
			case DarkSplitterType.Bottom:
				this.Bounds = new Rectangle(bounds.Left, bounds.Bottom - 2, bounds.Width, 5);
				this._minimum = bounds.Top - 2 + this._control.MinimumSize.Height;
				break;
			}
		}

		// Token: 0x04000163 RID: 355
		private Control _parentControl;

		// Token: 0x04000164 RID: 356
		private Control _control;

		// Token: 0x04000165 RID: 357
		private DarkSplitterType _splitterType;

		// Token: 0x04000166 RID: 358
		private int _minimum;

		// Token: 0x04000167 RID: 359
		private int _maximum;

		// Token: 0x04000168 RID: 360
		private DarkTranslucentForm _overlayForm;
	}
}
