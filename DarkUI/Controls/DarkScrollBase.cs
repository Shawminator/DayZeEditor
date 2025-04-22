using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x0200003E RID: 62
	public abstract class DarkScrollBase : Control
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600029F RID: 671 RVA: 0x00010CAC File Offset: 0x0000EEAC
		// (remove) Token: 0x060002A0 RID: 672 RVA: 0x00010CE4 File Offset: 0x0000EEE4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler ViewportChanged;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060002A1 RID: 673 RVA: 0x00010D1C File Offset: 0x0000EF1C
		// (remove) Token: 0x060002A2 RID: 674 RVA: 0x00010D54 File Offset: 0x0000EF54
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler ContentSizeChanged;

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x00010D8C File Offset: 0x0000EF8C
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x00010DA4 File Offset: 0x0000EFA4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Rectangle Viewport
		{
			get
			{
				return this._viewport;
			}
			private set
			{
				this._viewport = value;
				bool flag = this.ViewportChanged != null;
				if (flag)
				{
					this.ViewportChanged(this, null);
				}
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00010DD4 File Offset: 0x0000EFD4
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x00010DEC File Offset: 0x0000EFEC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Size ContentSize
		{
			get
			{
				return this._contentSize;
			}
			set
			{
				this._contentSize = value;
				this.UpdateScrollBars();
				bool flag = this.ContentSizeChanged != null;
				if (flag)
				{
					this.ContentSizeChanged(this, null);
				}
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00010E24 File Offset: 0x0000F024
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Point OffsetMousePosition
		{
			get
			{
				return this._offsetMousePosition;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x00010E3C File Offset: 0x0000F03C
		// (set) Token: 0x060002A9 RID: 681 RVA: 0x00010E54 File Offset: 0x0000F054
		[Category("Behavior")]
		[Description("Determines the maximum scroll change when dragging.")]
		[DefaultValue(0)]
		public int MaxDragChange
		{
			get
			{
				return this._maxDragChange;
			}
			set
			{
				this._maxDragChange = value;
				base.Invalidate();
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00010E65 File Offset: 0x0000F065
		// (set) Token: 0x060002AB RID: 683 RVA: 0x00010E6D File Offset: 0x0000F06D
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsDragging { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002AC RID: 684 RVA: 0x00010E78 File Offset: 0x0000F078
		// (set) Token: 0x060002AD RID: 685 RVA: 0x00010E90 File Offset: 0x0000F090
		[Category("Behavior")]
		[Description("Determines whether scrollbars will remain visible when disabled.")]
		[DefaultValue(true)]
		public bool HideScrollBars
		{
			get
			{
				return this._hideScrollBars;
			}
			set
			{
				this._hideScrollBars = value;
				this.UpdateScrollBars();
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00010EA4 File Offset: 0x0000F0A4
		protected DarkScrollBase()
		{
			base.SetStyle(ControlStyles.Selectable | ControlStyles.UserMouse, true);
			this._vScrollBar = new DarkScrollBar
			{
				ScrollOrientation = DarkScrollOrientation.Vertical
			};
			this._hScrollBar = new DarkScrollBar
			{
				ScrollOrientation = DarkScrollOrientation.Horizontal
			};
			base.Controls.Add(this._vScrollBar);
			base.Controls.Add(this._hScrollBar);
			this._vScrollBar.ValueChanged += delegate
			{
				this.UpdateViewport();
			};
			this._hScrollBar.ValueChanged += delegate
			{
				this.UpdateViewport();
			};
			this._vScrollBar.MouseDown += delegate
			{
				base.Select();
			};
			this._hScrollBar.MouseDown += delegate
			{
				base.Select();
			};
			this._dragTimer = new Timer();
			this._dragTimer.Interval = 1;
			this._dragTimer.Tick += this.DragTimer_Tick;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00010FB0 File Offset: 0x0000F1B0
		private void UpdateScrollBars()
		{
			bool flag = this._vScrollBar.Maximum != this.ContentSize.Height;
			if (flag)
			{
				this._vScrollBar.Maximum = this.ContentSize.Height;
			}
			bool flag2 = this._hScrollBar.Maximum != this.ContentSize.Width;
			if (flag2)
			{
				this._hScrollBar.Maximum = this.ContentSize.Width;
			}
			int scrollSize = Consts.ScrollBarSize;
			this._vScrollBar.Location = new Point(base.ClientSize.Width - scrollSize, 0);
			this._vScrollBar.Size = new Size(scrollSize, base.ClientSize.Height);
			this._hScrollBar.Location = new Point(0, base.ClientSize.Height - scrollSize);
			this._hScrollBar.Size = new Size(base.ClientSize.Width, scrollSize);
			bool designMode = base.DesignMode;
			if (!designMode)
			{
				this.SetVisibleSize();
				this.SetScrollBarVisibility();
				this.SetVisibleSize();
				this.SetScrollBarVisibility();
				bool visible = this._vScrollBar.Visible;
				if (visible)
				{
					this._hScrollBar.Width -= scrollSize;
				}
				bool visible2 = this._hScrollBar.Visible;
				if (visible2)
				{
					this._vScrollBar.Height -= scrollSize;
				}
				this._vScrollBar.ViewSize = this._visibleSize.Height;
				this._hScrollBar.ViewSize = this._visibleSize.Width;
				this.UpdateViewport();
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0001116C File Offset: 0x0000F36C
		private void SetScrollBarVisibility()
		{
			this._vScrollBar.Enabled = this._visibleSize.Height < this.ContentSize.Height;
			this._hScrollBar.Enabled = this._visibleSize.Width < this.ContentSize.Width;
			bool hideScrollBars = this._hideScrollBars;
			if (hideScrollBars)
			{
				this._vScrollBar.Visible = this._vScrollBar.Enabled;
				this._hScrollBar.Visible = this._hScrollBar.Enabled;
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00011204 File Offset: 0x0000F404
		private void SetVisibleSize()
		{
			int scrollSize = Consts.ScrollBarSize;
			this._visibleSize = new Size(base.ClientSize.Width, base.ClientSize.Height);
			bool visible = this._vScrollBar.Visible;
			if (visible)
			{
				this._visibleSize.Width = this._visibleSize.Width - scrollSize;
			}
			bool visible2 = this._hScrollBar.Visible;
			if (visible2)
			{
				this._visibleSize.Height = this._visibleSize.Height - scrollSize;
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00011288 File Offset: 0x0000F488
		private void UpdateViewport()
		{
			int left = 0;
			int top = 0;
			int width = base.ClientSize.Width;
			int height = base.ClientSize.Height;
			bool visible = this._hScrollBar.Visible;
			if (visible)
			{
				left = this._hScrollBar.Value;
				height -= this._hScrollBar.Height;
			}
			bool visible2 = this._vScrollBar.Visible;
			if (visible2)
			{
				top = this._vScrollBar.Value;
				width -= this._vScrollBar.Width;
			}
			this.Viewport = new Rectangle(left, top, width, height);
			Point pos = base.PointToClient(Control.MousePosition);
			this._offsetMousePosition = new Point(pos.X + this.Viewport.Left, pos.Y + this.Viewport.Top);
			base.Invalidate();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00011371 File Offset: 0x0000F571
		public void ScrollTo(Point point)
		{
			this.HScrollTo(point.X);
			this.VScrollTo(point.Y);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00011390 File Offset: 0x0000F590
		public void VScrollTo(int value)
		{
			bool visible = this._vScrollBar.Visible;
			if (visible)
			{
				this._vScrollBar.Value = value;
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x000113BC File Offset: 0x0000F5BC
		public void HScrollTo(int value)
		{
			bool visible = this._hScrollBar.Visible;
			if (visible)
			{
				this._hScrollBar.Value = value;
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x000113E6 File Offset: 0x0000F5E6
		protected virtual void StartDrag()
		{
			this.IsDragging = true;
			this._dragTimer.Start();
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x000113FD File Offset: 0x0000F5FD
		protected virtual void StopDrag()
		{
			this.IsDragging = false;
			this._dragTimer.Stop();
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00011414 File Offset: 0x0000F614
		public Point PointToView(Point point)
		{
			return new Point(point.X - this.Viewport.Left, point.Y - this.Viewport.Top);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00011458 File Offset: 0x0000F658
		public Rectangle RectangleToView(Rectangle rect)
		{
			return new Rectangle(new Point(rect.Left - this.Viewport.Left, rect.Top - this.Viewport.Top), rect.Size);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000114A7 File Offset: 0x0000F6A7
		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			this.UpdateScrollBars();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000B95A File Offset: 0x00009B5A
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			base.Invalidate();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x000114B8 File Offset: 0x0000F6B8
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			base.Invalidate();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000114CA File Offset: 0x0000F6CA
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.UpdateScrollBars();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x000114DC File Offset: 0x0000F6DC
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this._offsetMousePosition = new Point(e.X + this.Viewport.Left, e.Y + this.Viewport.Top);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00011528 File Offset: 0x0000F728
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			bool flag = e.Button == MouseButtons.Right;
			if (flag)
			{
				base.Select();
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00011558 File Offset: 0x0000F758
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			bool horizontal = false;
			bool flag = this._hScrollBar.Visible && Control.ModifierKeys == Keys.Control;
			if (flag)
			{
				horizontal = true;
			}
			bool flag2 = this._hScrollBar.Visible && !this._vScrollBar.Visible;
			if (flag2)
			{
				horizontal = true;
			}
			bool flag3 = !horizontal;
			if (flag3)
			{
				bool flag4 = e.Delta > 0;
				if (flag4)
				{
					this._vScrollBar.ScrollByPhysical(3);
				}
				else
				{
					bool flag5 = e.Delta < 0;
					if (flag5)
					{
						this._vScrollBar.ScrollByPhysical(-3);
					}
				}
			}
			else
			{
				bool flag6 = e.Delta > 0;
				if (flag6)
				{
					this._hScrollBar.ScrollByPhysical(3);
				}
				else
				{
					bool flag7 = e.Delta < 0;
					if (flag7)
					{
						this._hScrollBar.ScrollByPhysical(-3);
					}
				}
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0001163C File Offset: 0x0000F83C
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			Keys keyCode = e.KeyCode;
			Keys keys = keyCode;
			if (keys - Keys.Left <= 3)
			{
				e.IsInputKey = true;
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00011670 File Offset: 0x0000F870
		private void DragTimer_Tick(object sender, EventArgs e)
		{
			Point pos = base.PointToClient(Control.MousePosition);
			int right = base.ClientRectangle.Right;
			int bottom = base.ClientRectangle.Bottom;
			bool visible = this._vScrollBar.Visible;
			if (visible)
			{
				right = this._vScrollBar.Left;
			}
			bool visible2 = this._hScrollBar.Visible;
			if (visible2)
			{
				bottom = this._hScrollBar.Top;
			}
			bool visible3 = this._vScrollBar.Visible;
			if (visible3)
			{
				bool flag = pos.Y < base.ClientRectangle.Top;
				if (flag)
				{
					int difference = (pos.Y - base.ClientRectangle.Top) * -1;
					bool flag2 = this.MaxDragChange > 0 && difference > this.MaxDragChange;
					if (flag2)
					{
						difference = this.MaxDragChange;
					}
					this._vScrollBar.Value = this._vScrollBar.Value - difference;
				}
				bool flag3 = pos.Y > bottom;
				if (flag3)
				{
					int difference2 = pos.Y - bottom;
					bool flag4 = this.MaxDragChange > 0 && difference2 > this.MaxDragChange;
					if (flag4)
					{
						difference2 = this.MaxDragChange;
					}
					this._vScrollBar.Value = this._vScrollBar.Value + difference2;
				}
			}
			bool visible4 = this._hScrollBar.Visible;
			if (visible4)
			{
				bool flag5 = pos.X < base.ClientRectangle.Left;
				if (flag5)
				{
					int difference3 = (pos.X - base.ClientRectangle.Left) * -1;
					bool flag6 = this.MaxDragChange > 0 && difference3 > this.MaxDragChange;
					if (flag6)
					{
						difference3 = this.MaxDragChange;
					}
					this._hScrollBar.Value = this._hScrollBar.Value - difference3;
				}
				bool flag7 = pos.X > right;
				if (flag7)
				{
					int difference4 = pos.X - right;
					bool flag8 = this.MaxDragChange > 0 && difference4 > this.MaxDragChange;
					if (flag8)
					{
						difference4 = this.MaxDragChange;
					}
					this._hScrollBar.Value = this._hScrollBar.Value + difference4;
				}
			}
		}

		// Token: 0x040001F0 RID: 496
		protected readonly DarkScrollBar _vScrollBar;

		// Token: 0x040001F1 RID: 497
		protected readonly DarkScrollBar _hScrollBar;

		// Token: 0x040001F2 RID: 498
		private Size _visibleSize;

		// Token: 0x040001F3 RID: 499
		private Size _contentSize;

		// Token: 0x040001F4 RID: 500
		private Rectangle _viewport;

		// Token: 0x040001F5 RID: 501
		private Point _offsetMousePosition;

		// Token: 0x040001F6 RID: 502
		private int _maxDragChange = 0;

		// Token: 0x040001F7 RID: 503
		private Timer _dragTimer;

		// Token: 0x040001F8 RID: 504
		private bool _hideScrollBars = true;
	}
}
