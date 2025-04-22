using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x02000031 RID: 49
	public class DarkDropdownList : Control
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060001BD RID: 445 RVA: 0x0000B074 File Offset: 0x00009274
		// (remove) Token: 0x060001BE RID: 446 RVA: 0x0000B0AC File Offset: 0x000092AC
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler SelectedItemChanged;

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000B0E4 File Offset: 0x000092E4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ObservableCollection<DarkDropdownItem> Items
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000B0FC File Offset: 0x000092FC
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x0000B114 File Offset: 0x00009314
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DarkDropdownItem SelectedItem
		{
			get
			{
				return this._selectedItem;
			}
			set
			{
				this._selectedItem = value;
				EventHandler selectedItemChanged = this.SelectedItemChanged;
				if (selectedItemChanged != null)
				{
					selectedItemChanged(this, new EventArgs());
				}
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000B138 File Offset: 0x00009338
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x0000B150 File Offset: 0x00009350
		[Category("Appearance")]
		[Description("Determines whether a border is drawn around the control.")]
		[DefaultValue(true)]
		public bool ShowBorder
		{
			get
			{
				return this._showBorder;
			}
			set
			{
				this._showBorder = value;
				base.Invalidate();
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000B164 File Offset: 0x00009364
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 26);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000B180 File Offset: 0x00009380
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DarkControlState ControlState
		{
			get
			{
				return this._controlState;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000B198 File Offset: 0x00009398
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x0000B1B0 File Offset: 0x000093B0
		[Category("Appearance")]
		[Description("Determines the height of the individual list view items.")]
		[DefaultValue(22)]
		public int ItemHeight
		{
			get
			{
				return this._itemHeight;
			}
			set
			{
				this._itemHeight = value;
				this.ResizeMenu();
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000B1C4 File Offset: 0x000093C4
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000B1DC File Offset: 0x000093DC
		[Category("Appearance")]
		[Description("Determines the maximum height of the dropdown panel.")]
		[DefaultValue(130)]
		public int MaxHeight
		{
			get
			{
				return this._maxHeight;
			}
			set
			{
				this._maxHeight = value;
				this.ResizeMenu();
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000B1F0 File Offset: 0x000093F0
		// (set) Token: 0x060001CB RID: 459 RVA: 0x0000B208 File Offset: 0x00009408
		[Category("Behavior")]
		[Description("Determines what location the dropdown list appears.")]
		[DefaultValue(ToolStripDropDownDirection.Default)]
		public ToolStripDropDownDirection DropdownDirection
		{
			get
			{
				return this._dropdownDirection;
			}
			set
			{
				this._dropdownDirection = value;
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000B214 File Offset: 0x00009414
		public DarkDropdownList()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.UserMouse | ControlStyles.OptimizedDoubleBuffer, true);
			this._menu.AutoSize = false;
			this._menu.Closed += this.Menu_Closed;
			this.Items.CollectionChanged += this.Items_CollectionChanged;
			this.SelectedItemChanged += this.DarkDropdownList_SelectedItemChanged;
			this.SetControlState(DarkControlState.Normal);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000B2DC File Offset: 0x000094DC
		private ToolStripMenuItem GetMenuItem(DarkDropdownItem item)
		{
			foreach (object obj in this._menu.Items)
			{
				ToolStripMenuItem menuItem = (ToolStripMenuItem)obj;
				bool flag = (DarkDropdownItem)menuItem.Tag == item;
				if (flag)
				{
					return menuItem;
				}
			}
			return null;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000B358 File Offset: 0x00009558
		private void SetControlState(DarkControlState controlState)
		{
			bool menuOpen = this._menuOpen;
			if (!menuOpen)
			{
				bool flag = this._controlState != controlState;
				if (flag)
				{
					this._controlState = controlState;
					base.Invalidate();
				}
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000B394 File Offset: 0x00009594
		private void ShowMenu()
		{
			bool visible = this._menu.Visible;
			if (!visible)
			{
				this.SetControlState(DarkControlState.Pressed);
				this._menuOpen = true;
				Point pos = new Point(0, base.ClientRectangle.Bottom);
				bool flag = this._dropdownDirection == ToolStripDropDownDirection.AboveLeft || this._dropdownDirection == ToolStripDropDownDirection.AboveRight;
				if (flag)
				{
					pos.Y = 0;
				}
				this._menu.Show(this, pos, this._dropdownDirection);
				bool flag2 = this.SelectedItem != null;
				if (flag2)
				{
					ToolStripMenuItem selectedItem = this.GetMenuItem(this.SelectedItem);
					selectedItem.Select();
				}
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000B438 File Offset: 0x00009638
		private void ResizeMenu()
		{
			int width = base.ClientRectangle.Width;
			int height = this._menu.Items.Count * this._itemHeight + 4;
			bool flag = height > this._maxHeight;
			if (flag)
			{
				height = this._maxHeight;
			}
			foreach (object obj in this._menu.Items)
			{
				ToolStripMenuItem item = (ToolStripMenuItem)obj;
				item.AutoSize = true;
				bool flag2 = item.Size.Width > width;
				if (flag2)
				{
					width = item.Size.Width;
				}
				item.AutoSize = false;
			}
			foreach (object obj2 in this._menu.Items)
			{
				ToolStripMenuItem item2 = (ToolStripMenuItem)obj2;
				item2.Size = new Size(width - 1, this._itemHeight);
			}
			this._menu.Size = new Size(width, height);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000B590 File Offset: 0x00009790
		private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			bool flag = e.Action == NotifyCollectionChangedAction.Add;
			if (flag)
			{
				foreach (object obj in e.NewItems)
				{
					DarkDropdownItem item = (DarkDropdownItem)obj;
					ToolStripMenuItem menuItem = new ToolStripMenuItem(item.Text)
					{
						Image = item.Icon,
						AutoSize = false,
						Height = this._itemHeight,
						Font = this.Font,
						Tag = item,
						TextAlign = ContentAlignment.MiddleLeft
					};
					this._menu.Items.Add(menuItem);
					menuItem.Click += this.Item_Select;
					bool flag2 = this.SelectedItem == null;
					if (flag2)
					{
						this.SelectedItem = item;
					}
				}
			}
			bool flag3 = e.Action == NotifyCollectionChangedAction.Remove;
			if (flag3)
			{
				foreach (object obj2 in e.OldItems)
				{
					DarkDropdownItem item2 = (DarkDropdownItem)obj2;
					foreach (object obj3 in this._menu.Items)
					{
						ToolStripMenuItem menuItem2 = (ToolStripMenuItem)obj3;
						bool flag4 = (DarkDropdownItem)menuItem2.Tag == item2;
						if (flag4)
						{
							this._menu.Items.Remove(menuItem2);
						}
					}
				}
			}
			this.ResizeMenu();
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000B770 File Offset: 0x00009970
		private void Item_Select(object sender, EventArgs e)
		{
			ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
			bool flag = menuItem == null;
			if (!flag)
			{
				DarkDropdownItem dropdownItem = (DarkDropdownItem)menuItem.Tag;
				bool flag2 = this._selectedItem != dropdownItem;
				if (flag2)
				{
					this.SelectedItem = dropdownItem;
				}
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000B7B4 File Offset: 0x000099B4
		private void DarkDropdownList_SelectedItemChanged(object sender, EventArgs e)
		{
			foreach (object obj in this._menu.Items)
			{
				ToolStripMenuItem item = (ToolStripMenuItem)obj;
				bool flag = (DarkDropdownItem)item.Tag == this.SelectedItem;
				if (flag)
				{
					item.BackColor = Colors.DarkBlueBackground;
					item.Font = new Font(this.Font, FontStyle.Bold);
				}
				else
				{
					item.BackColor = Colors.GreyBackground;
					item.Font = new Font(this.Font, FontStyle.Regular);
				}
			}
			base.Invalidate();
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000B874 File Offset: 0x00009A74
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.ResizeMenu();
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000B888 File Offset: 0x00009A88
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
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

		// Token: 0x060001D6 RID: 470 RVA: 0x0000B8E7 File Offset: 0x00009AE7
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.ShowMenu();
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000B8F9 File Offset: 0x00009AF9
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.SetControlState(DarkControlState.Normal);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000B90C File Offset: 0x00009B0C
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.SetControlState(DarkControlState.Normal);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000B920 File Offset: 0x00009B20
		protected override void OnMouseCaptureChanged(EventArgs e)
		{
			base.OnMouseCaptureChanged(e);
			Point location = Cursor.Position;
			bool flag = !base.ClientRectangle.Contains(location);
			if (flag)
			{
				this.SetControlState(DarkControlState.Normal);
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000B95A File Offset: 0x00009B5A
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			base.Invalidate();
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000B96C File Offset: 0x00009B6C
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
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

		// Token: 0x060001DC RID: 476 RVA: 0x0000B9B0 File Offset: 0x00009BB0
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			bool flag = e.KeyCode == Keys.Space;
			if (flag)
			{
				this.ShowMenu();
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000B9DC File Offset: 0x00009BDC
		private void Menu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			this._menuOpen = false;
			bool flag = !base.ClientRectangle.Contains(Control.MousePosition);
			if (flag)
			{
				this.SetControlState(DarkControlState.Normal);
			}
			else
			{
				this.SetControlState(DarkControlState.Hover);
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000BA20 File Offset: 0x00009C20
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			using (SolidBrush b = new SolidBrush(Colors.MediumBackground))
			{
				g.FillRectangle(b, base.ClientRectangle);
			}
			bool flag = this.ControlState == DarkControlState.Normal;
			if (flag)
			{
				bool showBorder = this.ShowBorder;
				if (showBorder)
				{
					using (Pen p = new Pen(Colors.LightBorder, 1f))
					{
						Rectangle modRect = new Rectangle(base.ClientRectangle.Left, base.ClientRectangle.Top, base.ClientRectangle.Width - 1, base.ClientRectangle.Height - 1);
						g.DrawRectangle(p, modRect);
					}
				}
			}
			bool flag2 = this.ControlState == DarkControlState.Hover;
			if (flag2)
			{
				using (SolidBrush b2 = new SolidBrush(Colors.DarkBorder))
				{
					g.FillRectangle(b2, base.ClientRectangle);
				}
				using (SolidBrush b3 = new SolidBrush(Colors.DarkBackground))
				{
					Rectangle arrowRect = new Rectangle(base.ClientRectangle.Right - DropdownIcons.small_arrow.Width - 8, base.ClientRectangle.Top, DropdownIcons.small_arrow.Width + 8, base.ClientRectangle.Height);
					g.FillRectangle(b3, arrowRect);
				}
				using (Pen p2 = new Pen(Colors.BlueSelection, 1f))
				{
					Rectangle modRect2 = new Rectangle(base.ClientRectangle.Left, base.ClientRectangle.Top, base.ClientRectangle.Width - 1 - DropdownIcons.small_arrow.Width - 8, base.ClientRectangle.Height - 1);
					g.DrawRectangle(p2, modRect2);
				}
			}
			bool flag3 = this.ControlState == DarkControlState.Pressed;
			if (flag3)
			{
				using (SolidBrush b4 = new SolidBrush(Colors.DarkBorder))
				{
					g.FillRectangle(b4, base.ClientRectangle);
				}
				using (SolidBrush b5 = new SolidBrush(Colors.BlueSelection))
				{
					Rectangle arrowRect2 = new Rectangle(base.ClientRectangle.Right - DropdownIcons.small_arrow.Width - 8, base.ClientRectangle.Top, DropdownIcons.small_arrow.Width + 8, base.ClientRectangle.Height);
					g.FillRectangle(b5, arrowRect2);
				}
			}
			using (Bitmap img = DropdownIcons.small_arrow)
			{
				g.DrawImageUnscaled(img, base.ClientRectangle.Right - img.Width - 4, base.ClientRectangle.Top + base.ClientRectangle.Height / 2 - img.Height / 2);
			}
			bool flag4 = this.SelectedItem != null;
			if (flag4)
			{
				bool hasIcon = this.SelectedItem.Icon != null;
				bool flag5 = hasIcon;
				if (flag5)
				{
					g.DrawImageUnscaled(this.SelectedItem.Icon, new Point(base.ClientRectangle.Left + 5, base.ClientRectangle.Top + base.ClientRectangle.Height / 2 - this._iconSize / 2));
				}
				using (SolidBrush b6 = new SolidBrush(Colors.LightText))
				{
					StringFormat stringFormat = new StringFormat
					{
						Alignment = StringAlignment.Near,
						LineAlignment = StringAlignment.Center
					};
					Rectangle rect = new Rectangle(base.ClientRectangle.Left + 2, base.ClientRectangle.Top, base.ClientRectangle.Width - 16, base.ClientRectangle.Height);
					bool flag6 = hasIcon;
					if (flag6)
					{
						rect.X += this._iconSize + 7;
						rect.Width -= this._iconSize + 7;
					}
					g.DrawString(this.SelectedItem.Text, this.Font, b6, rect, stringFormat);
				}
			}
		}

		// Token: 0x040001A2 RID: 418
		private DarkControlState _controlState = DarkControlState.Normal;

		// Token: 0x040001A3 RID: 419
		private ObservableCollection<DarkDropdownItem> _items = new ObservableCollection<DarkDropdownItem>();

		// Token: 0x040001A4 RID: 420
		private DarkDropdownItem _selectedItem;

		// Token: 0x040001A5 RID: 421
		private DarkContextMenu _menu = new DarkContextMenu();

		// Token: 0x040001A6 RID: 422
		private bool _menuOpen = false;

		// Token: 0x040001A7 RID: 423
		private bool _showBorder = true;

		// Token: 0x040001A8 RID: 424
		private int _itemHeight = 22;

		// Token: 0x040001A9 RID: 425
		private int _maxHeight = 130;

		// Token: 0x040001AA RID: 426
		private readonly int _iconSize = 16;

		// Token: 0x040001AB RID: 427
		private ToolStripDropDownDirection _dropdownDirection = ToolStripDropDownDirection.Default;
	}
}
