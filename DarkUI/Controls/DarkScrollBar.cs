using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x02000043 RID: 67
	public class DarkScrollBar : Control
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060002F0 RID: 752 RVA: 0x00012344 File Offset: 0x00010544
		// (remove) Token: 0x060002F1 RID: 753 RVA: 0x0001237C File Offset: 0x0001057C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<ScrollValueEventArgs> ValueChanged;

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x000123B4 File Offset: 0x000105B4
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x000123CC File Offset: 0x000105CC
		[Category("Behavior")]
		[Description("The orientation type of the scrollbar.")]
		[DefaultValue(DarkScrollOrientation.Vertical)]
		public DarkScrollOrientation ScrollOrientation
		{
			get
			{
				return this._scrollOrientation;
			}
			set
			{
				this._scrollOrientation = value;
				this.UpdateScrollBar();
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x000123E0 File Offset: 0x000105E0
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x000123F8 File Offset: 0x000105F8
		[Category("Behavior")]
		[Description("The value that the scroll thumb position represents.")]
		[DefaultValue(0)]
		public int Value
		{
			get
			{
				return this._value;
			}
			set
			{
				bool flag = value < this.Minimum;
				if (flag)
				{
					value = this.Minimum;
				}
				int maximumValue = this.Maximum - this.ViewSize;
				bool flag2 = value > maximumValue;
				if (flag2)
				{
					value = maximumValue;
				}
				bool flag3 = this._value == value;
				if (!flag3)
				{
					this._value = value;
					this.UpdateThumb(true);
					bool flag4 = this.ValueChanged != null;
					if (flag4)
					{
						this.ValueChanged(this, new ScrollValueEventArgs(this.Value));
					}
				}
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0001247C File Offset: 0x0001067C
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x00012494 File Offset: 0x00010694
		[Category("Behavior")]
		[Description("The lower limit value of the scrollable range.")]
		[DefaultValue(0)]
		public int Minimum
		{
			get
			{
				return this._minimum;
			}
			set
			{
				this._minimum = value;
				this.UpdateScrollBar();
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x000124A8 File Offset: 0x000106A8
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x000124C0 File Offset: 0x000106C0
		[Category("Behavior")]
		[Description("The upper limit value of the scrollable range.")]
		[DefaultValue(100)]
		public int Maximum
		{
			get
			{
				return this._maximum;
			}
			set
			{
				this._maximum = value;
				this.UpdateScrollBar();
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002FA RID: 762 RVA: 0x000124D4 File Offset: 0x000106D4
		// (set) Token: 0x060002FB RID: 763 RVA: 0x000124EC File Offset: 0x000106EC
		[Category("Behavior")]
		[Description("The view size for the scrollable area.")]
		[DefaultValue(0)]
		public int ViewSize
		{
			get
			{
				return this._viewSize;
			}
			set
			{
				this._viewSize = value;
				this.UpdateScrollBar();
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00012500 File Offset: 0x00010700
		// (set) Token: 0x060002FD RID: 765 RVA: 0x00012518 File Offset: 0x00010718
		public new bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				bool flag = base.Visible == value;
				if (!flag)
				{
					base.Visible = value;
				}
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00012540 File Offset: 0x00010740
		public DarkScrollBar()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.Selectable, false);
			this._scrollTimer = new Timer();
			this._scrollTimer.Interval = 1;
			this._scrollTimer.Tick += this.ScrollTimerTick;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x000125AE File Offset: 0x000107AE
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.UpdateScrollBar();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000125C0 File Offset: 0x000107C0
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			bool flag = this._thumbArea.Contains(e.Location) && e.Button == MouseButtons.Left;
			if (flag)
			{
				this._isScrolling = true;
				this._initialContact = e.Location;
				bool flag2 = this._scrollOrientation == DarkScrollOrientation.Vertical;
				if (flag2)
				{
					this._initialValue = this._thumbArea.Top;
				}
				else
				{
					this._initialValue = this._thumbArea.Left;
				}
				base.Invalidate();
			}
			else
			{
				bool flag3 = this._upArrowArea.Contains(e.Location) && e.Button == MouseButtons.Left;
				if (flag3)
				{
					this._upArrowClicked = true;
					this._scrollTimer.Enabled = true;
					base.Invalidate();
				}
				else
				{
					bool flag4 = this._downArrowArea.Contains(e.Location) && e.Button == MouseButtons.Left;
					if (flag4)
					{
						this._downArrowClicked = true;
						this._scrollTimer.Enabled = true;
						base.Invalidate();
					}
					else
					{
						bool flag5 = this._trackArea.Contains(e.Location) && e.Button == MouseButtons.Left;
						if (flag5)
						{
							bool flag6 = this._scrollOrientation == DarkScrollOrientation.Vertical;
							if (flag6)
							{
								Rectangle modRect = new Rectangle(this._thumbArea.Left, this._trackArea.Top, this._thumbArea.Width, this._trackArea.Height);
								bool flag7 = !modRect.Contains(e.Location);
								if (flag7)
								{
									return;
								}
							}
							else
							{
								bool flag8 = this._scrollOrientation == DarkScrollOrientation.Horizontal;
								if (flag8)
								{
									Rectangle modRect2 = new Rectangle(this._trackArea.Left, this._thumbArea.Top, this._trackArea.Width, this._thumbArea.Height);
									bool flag9 = !modRect2.Contains(e.Location);
									if (flag9)
									{
										return;
									}
								}
							}
							bool flag10 = this._scrollOrientation == DarkScrollOrientation.Vertical;
							if (flag10)
							{
								int loc = e.Location.Y;
								loc -= this._upArrowArea.Bottom - 1;
								loc -= this._thumbArea.Height / 2;
								this.ScrollToPhysical(loc);
							}
							else
							{
								int loc2 = e.Location.X;
								loc2 -= this._upArrowArea.Right - 1;
								loc2 -= this._thumbArea.Width / 2;
								this.ScrollToPhysical(loc2);
							}
							this._isScrolling = true;
							this._initialContact = e.Location;
							this._thumbHot = true;
							bool flag11 = this._scrollOrientation == DarkScrollOrientation.Vertical;
							if (flag11)
							{
								this._initialValue = this._thumbArea.Top;
							}
							else
							{
								this._initialValue = this._thumbArea.Left;
							}
							base.Invalidate();
						}
					}
				}
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000128B2 File Offset: 0x00010AB2
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this._isScrolling = false;
			this._upArrowClicked = false;
			this._downArrowClicked = false;
			base.Invalidate();
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000128DC File Offset: 0x00010ADC
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			bool flag = !this._isScrolling;
			if (flag)
			{
				bool thumbHot = this._thumbArea.Contains(e.Location);
				bool flag2 = this._thumbHot != thumbHot;
				if (flag2)
				{
					this._thumbHot = thumbHot;
					base.Invalidate();
				}
				bool upArrowHot = this._upArrowArea.Contains(e.Location);
				bool flag3 = this._upArrowHot != upArrowHot;
				if (flag3)
				{
					this._upArrowHot = upArrowHot;
					base.Invalidate();
				}
				bool downArrowHot = this._downArrowArea.Contains(e.Location);
				bool flag4 = this._downArrowHot != downArrowHot;
				if (flag4)
				{
					this._downArrowHot = downArrowHot;
					base.Invalidate();
				}
			}
			bool isScrolling = this._isScrolling;
			if (isScrolling)
			{
				bool flag5 = e.Button != MouseButtons.Left;
				if (flag5)
				{
					this.OnMouseUp(null);
				}
				else
				{
					Point difference = new Point(e.Location.X - this._initialContact.X, e.Location.Y - this._initialContact.Y);
					bool flag6 = this._scrollOrientation == DarkScrollOrientation.Vertical;
					if (flag6)
					{
						int thumbPos = this._initialValue - this._trackArea.Top;
						int newPosition = thumbPos + difference.Y;
						this.ScrollToPhysical(newPosition);
					}
					else
					{
						bool flag7 = this._scrollOrientation == DarkScrollOrientation.Horizontal;
						if (flag7)
						{
							int thumbPos2 = this._initialValue - this._trackArea.Left;
							int newPosition2 = thumbPos2 + difference.X;
							this.ScrollToPhysical(newPosition2);
						}
					}
					this.UpdateScrollBar();
				}
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00012A8F File Offset: 0x00010C8F
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this._thumbHot = false;
			this._upArrowHot = false;
			this._downArrowHot = false;
			base.Invalidate();
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00012AB8 File Offset: 0x00010CB8
		private void ScrollTimerTick(object sender, EventArgs e)
		{
			bool flag = !this._upArrowClicked && !this._downArrowClicked;
			if (flag)
			{
				this._scrollTimer.Enabled = false;
			}
			else
			{
				bool upArrowClicked = this._upArrowClicked;
				if (upArrowClicked)
				{
					this.ScrollBy(-1);
				}
				else
				{
					bool downArrowClicked = this._downArrowClicked;
					if (downArrowClicked)
					{
						this.ScrollBy(1);
					}
				}
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00012B14 File Offset: 0x00010D14
		public void ScrollTo(int position)
		{
			this.Value = position;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00012B20 File Offset: 0x00010D20
		public void ScrollToPhysical(int positionInPixels)
		{
			int trackAreaSize = ((this._scrollOrientation == DarkScrollOrientation.Vertical) ? (this._trackArea.Height - this._thumbArea.Height) : (this._trackArea.Width - this._thumbArea.Width));
			float positionRatio = (float)positionInPixels / (float)trackAreaSize;
			int viewScrollSize = this.Maximum - this.ViewSize;
			int newValue = (int)(positionRatio * (float)viewScrollSize);
			this.Value = newValue;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00012B90 File Offset: 0x00010D90
		public void ScrollBy(int offset)
		{
			int newValue = this.Value + offset;
			this.ScrollTo(newValue);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00012BB0 File Offset: 0x00010DB0
		public void ScrollByPhysical(int offsetInPixels)
		{
			int thumbPos = ((this._scrollOrientation == DarkScrollOrientation.Vertical) ? (this._thumbArea.Top - this._trackArea.Top) : (this._thumbArea.Left - this._trackArea.Left));
			int newPosition = thumbPos - offsetInPixels;
			this.ScrollToPhysical(newPosition);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00012C08 File Offset: 0x00010E08
		public void UpdateScrollBar()
		{
			Rectangle area = base.ClientRectangle;
			bool flag = this._scrollOrientation == DarkScrollOrientation.Vertical;
			if (flag)
			{
				this._upArrowArea = new Rectangle(area.Left, area.Top, Consts.ArrowButtonSize, Consts.ArrowButtonSize);
				this._downArrowArea = new Rectangle(area.Left, area.Bottom - Consts.ArrowButtonSize, Consts.ArrowButtonSize, Consts.ArrowButtonSize);
			}
			else
			{
				bool flag2 = this._scrollOrientation == DarkScrollOrientation.Horizontal;
				if (flag2)
				{
					this._upArrowArea = new Rectangle(area.Left, area.Top, Consts.ArrowButtonSize, Consts.ArrowButtonSize);
					this._downArrowArea = new Rectangle(area.Right - Consts.ArrowButtonSize, area.Top, Consts.ArrowButtonSize, Consts.ArrowButtonSize);
				}
			}
			bool flag3 = this._scrollOrientation == DarkScrollOrientation.Vertical;
			if (flag3)
			{
				this._trackArea = new Rectangle(area.Left, area.Top + Consts.ArrowButtonSize, area.Width, area.Height - Consts.ArrowButtonSize * 2);
			}
			else
			{
				bool flag4 = this._scrollOrientation == DarkScrollOrientation.Horizontal;
				if (flag4)
				{
					this._trackArea = new Rectangle(area.Left + Consts.ArrowButtonSize, area.Top, area.Width - Consts.ArrowButtonSize * 2, area.Height);
				}
			}
			this.UpdateThumb(false);
			base.Invalidate();
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00012D70 File Offset: 0x00010F70
		private void UpdateThumb(bool forceRefresh = false)
		{
			bool flag = this.ViewSize >= this.Maximum;
			if (!flag)
			{
				int maximumValue = this.Maximum - this.ViewSize;
				bool flag2 = this.Value > maximumValue;
				if (flag2)
				{
					this.Value = maximumValue;
				}
				this._viewContentRatio = (float)this.ViewSize / (float)this.Maximum;
				int viewAreaSize = this.Maximum - this.ViewSize;
				float positionRatio = (float)this.Value / (float)viewAreaSize;
				bool flag3 = this._scrollOrientation == DarkScrollOrientation.Vertical;
				if (flag3)
				{
					int thumbSize = (int)((float)this._trackArea.Height * this._viewContentRatio);
					bool flag4 = thumbSize < Consts.MinimumThumbSize;
					if (flag4)
					{
						thumbSize = Consts.MinimumThumbSize;
					}
					int trackAreaSize = this._trackArea.Height - thumbSize;
					int thumbPosition = (int)((float)trackAreaSize * positionRatio);
					this._thumbArea = new Rectangle(this._trackArea.Left + 3, this._trackArea.Top + thumbPosition, Consts.ScrollBarSize - 6, thumbSize);
				}
				else
				{
					bool flag5 = this._scrollOrientation == DarkScrollOrientation.Horizontal;
					if (flag5)
					{
						int thumbSize2 = (int)((float)this._trackArea.Width * this._viewContentRatio);
						bool flag6 = thumbSize2 < Consts.MinimumThumbSize;
						if (flag6)
						{
							thumbSize2 = Consts.MinimumThumbSize;
						}
						int trackAreaSize2 = this._trackArea.Width - thumbSize2;
						int thumbPosition2 = (int)((float)trackAreaSize2 * positionRatio);
						this._thumbArea = new Rectangle(this._trackArea.Left + thumbPosition2, this._trackArea.Top + 3, thumbSize2, Consts.ScrollBarSize - 6);
					}
				}
				if (forceRefresh)
				{
					base.Invalidate();
					base.Update();
				}
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00012F10 File Offset: 0x00011110
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Bitmap upIcon = (this._upArrowHot ? ScrollIcons.scrollbar_arrow_hot : ScrollIcons.scrollbar_arrow_standard);
			bool upArrowClicked = this._upArrowClicked;
			if (upArrowClicked)
			{
				upIcon = ScrollIcons.scrollbar_arrow_clicked;
			}
			bool flag = !base.Enabled;
			if (flag)
			{
				upIcon = ScrollIcons.scrollbar_arrow_disabled;
			}
			bool flag2 = this._scrollOrientation == DarkScrollOrientation.Vertical;
			if (flag2)
			{
				upIcon.RotateFlip(RotateFlipType.Rotate180FlipX);
			}
			else
			{
				bool flag3 = this._scrollOrientation == DarkScrollOrientation.Horizontal;
				if (flag3)
				{
					upIcon.RotateFlip(RotateFlipType.Rotate90FlipNone);
				}
			}
			g.DrawImageUnscaled(upIcon, this._upArrowArea.Left + this._upArrowArea.Width / 2 - upIcon.Width / 2, this._upArrowArea.Top + this._upArrowArea.Height / 2 - upIcon.Height / 2);
			Bitmap downIcon = (this._downArrowHot ? ScrollIcons.scrollbar_arrow_hot : ScrollIcons.scrollbar_arrow_standard);
			bool downArrowClicked = this._downArrowClicked;
			if (downArrowClicked)
			{
				downIcon = ScrollIcons.scrollbar_arrow_clicked;
			}
			bool flag4 = !base.Enabled;
			if (flag4)
			{
				downIcon = ScrollIcons.scrollbar_arrow_disabled;
			}
			bool flag5 = this._scrollOrientation == DarkScrollOrientation.Horizontal;
			if (flag5)
			{
				downIcon.RotateFlip(RotateFlipType.Rotate270FlipNone);
			}
			g.DrawImageUnscaled(downIcon, this._downArrowArea.Left + this._downArrowArea.Width / 2 - downIcon.Width / 2, this._downArrowArea.Top + this._downArrowArea.Height / 2 - downIcon.Height / 2);
			bool enabled = base.Enabled;
			if (enabled)
			{
				Color scrollColor = (this._thumbHot ? Colors.GreyHighlight : Colors.GreySelection);
				bool isScrolling = this._isScrolling;
				if (isScrolling)
				{
					scrollColor = Colors.ActiveControl;
				}
				using (SolidBrush b = new SolidBrush(scrollColor))
				{
					g.FillRectangle(b, this._thumbArea);
				}
			}
		}

		// Token: 0x04000203 RID: 515
		private DarkScrollOrientation _scrollOrientation;

		// Token: 0x04000204 RID: 516
		private int _value;

		// Token: 0x04000205 RID: 517
		private int _minimum = 0;

		// Token: 0x04000206 RID: 518
		private int _maximum = 100;

		// Token: 0x04000207 RID: 519
		private int _viewSize;

		// Token: 0x04000208 RID: 520
		private Rectangle _trackArea;

		// Token: 0x04000209 RID: 521
		private float _viewContentRatio;

		// Token: 0x0400020A RID: 522
		private Rectangle _thumbArea;

		// Token: 0x0400020B RID: 523
		private Rectangle _upArrowArea;

		// Token: 0x0400020C RID: 524
		private Rectangle _downArrowArea;

		// Token: 0x0400020D RID: 525
		private bool _thumbHot;

		// Token: 0x0400020E RID: 526
		private bool _upArrowHot;

		// Token: 0x0400020F RID: 527
		private bool _downArrowHot;

		// Token: 0x04000210 RID: 528
		private bool _upArrowClicked;

		// Token: 0x04000211 RID: 529
		private bool _downArrowClicked;

		// Token: 0x04000212 RID: 530
		private bool _isScrolling;

		// Token: 0x04000213 RID: 531
		private int _initialValue;

		// Token: 0x04000214 RID: 532
		private Point _initialContact;

		// Token: 0x04000215 RID: 533
		private Timer _scrollTimer;
	}
}
