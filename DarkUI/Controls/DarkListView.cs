using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x0200003D RID: 61
	public class DarkListView : DarkScrollView
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600027E RID: 638 RVA: 0x0000FB08 File Offset: 0x0000DD08
		// (remove) Token: 0x0600027F RID: 639 RVA: 0x0000FB40 File Offset: 0x0000DD40
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler SelectedIndicesChanged;

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000FB78 File Offset: 0x0000DD78
		// (set) Token: 0x06000281 RID: 641 RVA: 0x0000FB90 File Offset: 0x0000DD90
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ObservableCollection<DarkListItem> Items
		{
			get
			{
				return this._items;
			}
			set
			{
				bool flag = this._items != null;
				if (flag)
				{
					this._items.CollectionChanged -= this.Items_CollectionChanged;
				}
				this._items = value;
				this._items.CollectionChanged += this.Items_CollectionChanged;
				this.UpdateListBox();
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000FBEC File Offset: 0x0000DDEC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<int> SelectedIndices
		{
			get
			{
				return this._selectedIndices;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000FC04 File Offset: 0x0000DE04
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0000FC1C File Offset: 0x0000DE1C
		[Category("Appearance")]
		[Description("Determines the height of the individual list view items.")]
		[DefaultValue(20)]
		public int ItemHeight
		{
			get
			{
				return this._itemHeight;
			}
			set
			{
				this._itemHeight = value;
				this.UpdateListBox();
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000FC30 File Offset: 0x0000DE30
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0000FC48 File Offset: 0x0000DE48
		[Category("Behaviour")]
		[Description("Determines whether multiple list view items can be selected at once.")]
		[DefaultValue(false)]
		public bool MultiSelect
		{
			get
			{
				return this._multiSelect;
			}
			set
			{
				this._multiSelect = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000FC52 File Offset: 0x0000DE52
		// (set) Token: 0x06000288 RID: 648 RVA: 0x0000FC5A File Offset: 0x0000DE5A
		[Category("Appearance")]
		[Description("Determines whether icons are rendered with the list items.")]
		[DefaultValue(false)]
		public bool ShowIcons { get; set; }

		// Token: 0x06000289 RID: 649 RVA: 0x0000FC63 File Offset: 0x0000DE63
		public DarkListView()
		{
			this.Items = new ObservableCollection<DarkListItem>();
			this._selectedIndices = new List<int>();
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000FCA4 File Offset: 0x0000DEA4
		private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			bool flag = e.NewItems != null;
			if (flag)
			{
				using (Graphics g = base.CreateGraphics())
				{
					foreach (object obj in e.NewItems)
					{
						DarkListItem item = (DarkListItem)obj;
						item.TextChanged += this.Item_TextChanged;
						this.UpdateItemSize(item, g);
					}
				}
				bool flag2 = e.NewStartingIndex < this.Items.Count - 1;
				if (flag2)
				{
					for (int i = e.NewStartingIndex; i <= this.Items.Count - 1; i++)
					{
						this.UpdateItemPosition(this.Items[i], i);
					}
				}
			}
			bool flag3 = e.OldItems != null;
			if (flag3)
			{
				foreach (object obj2 in e.OldItems)
				{
					DarkListItem item2 = (DarkListItem)obj2;
					item2.TextChanged -= this.Item_TextChanged;
				}
				bool flag4 = e.OldStartingIndex < this.Items.Count - 1;
				if (flag4)
				{
					for (int j = e.OldStartingIndex; j <= this.Items.Count - 1; j++)
					{
						this.UpdateItemPosition(this.Items[j], j);
					}
				}
			}
			bool flag5 = this.Items.Count == 0;
			if (flag5)
			{
				bool flag6 = this._selectedIndices.Count > 0;
				if (flag6)
				{
					this._selectedIndices.Clear();
					bool flag7 = this.SelectedIndicesChanged != null;
					if (flag7)
					{
						this.SelectedIndicesChanged(this, null);
					}
				}
			}
			this.UpdateContentSize();
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000FEDC File Offset: 0x0000E0DC
		private void Item_TextChanged(object sender, EventArgs e)
		{
			DarkListItem item = (DarkListItem)sender;
			this.UpdateItemSize(item);
			this.UpdateContentSize(item);
			base.Invalidate();
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000FF08 File Offset: 0x0000E108
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			bool flag = this.Items.Count == 0;
			if (!flag)
			{
				bool flag2 = e.Button != MouseButtons.Left && e.Button != MouseButtons.Right;
				if (!flag2)
				{
					Point pos = base.OffsetMousePosition;
					List<int> range = this.ItemIndexesInView().ToList<int>();
					int top = range.Min();
					int bottom = range.Max();
					int width = Math.Max(base.ContentSize.Width, base.Viewport.Width);
					for (int i = top; i <= bottom; i++)
					{
						Rectangle rect = new Rectangle(0, i * this.ItemHeight, width, this.ItemHeight);
						bool flag3 = rect.Contains(pos);
						if (flag3)
						{
							bool flag4 = this.MultiSelect && Control.ModifierKeys == Keys.Shift;
							if (flag4)
							{
								this.SelectAnchoredRange(i);
							}
							else
							{
								bool flag5 = this.MultiSelect && Control.ModifierKeys == Keys.Control;
								if (flag5)
								{
									this.ToggleItem(i);
								}
								else
								{
									this.SelectItem(i);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0001004C File Offset: 0x0000E24C
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			bool flag = this.Items.Count == 0;
			if (!flag)
			{
				bool flag2 = e.KeyCode != Keys.Down && e.KeyCode != Keys.Up;
				if (!flag2)
				{
					bool flag3 = this.MultiSelect && Control.ModifierKeys == Keys.Shift;
					if (flag3)
					{
						bool flag4 = e.KeyCode == Keys.Up;
						if (flag4)
						{
							bool flag5 = this._anchoredItemEnd - 1 >= 0;
							if (flag5)
							{
								this.SelectAnchoredRange(this._anchoredItemEnd - 1);
								this.EnsureVisible();
							}
						}
						else
						{
							bool flag6 = e.KeyCode == Keys.Down;
							if (flag6)
							{
								bool flag7 = this._anchoredItemEnd + 1 <= this.Items.Count - 1;
								if (flag7)
								{
									this.SelectAnchoredRange(this._anchoredItemEnd + 1);
								}
							}
						}
					}
					else
					{
						bool flag8 = e.KeyCode == Keys.Up;
						if (flag8)
						{
							bool flag9 = this._anchoredItemEnd - 1 >= 0;
							if (flag9)
							{
								this.SelectItem(this._anchoredItemEnd - 1);
							}
						}
						else
						{
							bool flag10 = e.KeyCode == Keys.Down;
							if (flag10)
							{
								bool flag11 = this._anchoredItemEnd + 1 <= this.Items.Count - 1;
								if (flag11)
								{
									this.SelectItem(this._anchoredItemEnd + 1);
								}
							}
						}
					}
					this.EnsureVisible();
				}
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000101C0 File Offset: 0x0000E3C0
		public int GetItemIndex(DarkListItem item)
		{
			return this.Items.IndexOf(item);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000101E0 File Offset: 0x0000E3E0
		public void SelectItem(int index)
		{
			bool flag = index < 0 || index > this.Items.Count - 1;
			if (!flag)
			{
				this._selectedIndices.Clear();
				this._selectedIndices.Add(index);
				bool flag2 = this.SelectedIndicesChanged != null;
				if (flag2)
				{
					this.SelectedIndicesChanged(this, null);
				}
				this._anchoredItemStart = index;
				this._anchoredItemEnd = index;
				base.Invalidate();
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00010254 File Offset: 0x0000E454
		public void SelectItems(IEnumerable<int> indexes)
		{
			this._selectedIndices.Clear();
			List<int> list = indexes.ToList<int>();
			foreach (int index in list)
			{
				bool flag = index < 0 || index > this.Items.Count - 1;
				if (flag)
				{
					throw new IndexOutOfRangeException(string.Format("Value '{0}' is outside of valid range.", index));
				}
				this._selectedIndices.Add(index);
			}
			bool flag2 = this.SelectedIndicesChanged != null;
			if (flag2)
			{
				this.SelectedIndicesChanged(this, null);
			}
			this._anchoredItemStart = list[list.Count - 1];
			this._anchoredItemEnd = list[list.Count - 1];
			base.Invalidate();
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00010340 File Offset: 0x0000E540
		public void ToggleItem(int index)
		{
			bool flag = this._selectedIndices.Contains(index);
			if (flag)
			{
				this._selectedIndices.Remove(index);
				bool flag2 = this._anchoredItemStart == index && this._anchoredItemEnd == index;
				if (flag2)
				{
					bool flag3 = this._selectedIndices.Count > 0;
					if (flag3)
					{
						this._anchoredItemStart = this._selectedIndices[0];
						this._anchoredItemEnd = this._selectedIndices[0];
					}
					else
					{
						this._anchoredItemStart = -1;
						this._anchoredItemEnd = -1;
					}
				}
				bool flag4 = this._anchoredItemStart == index;
				if (flag4)
				{
					bool flag5 = this._anchoredItemEnd < index;
					if (flag5)
					{
						this._anchoredItemStart = index - 1;
					}
					else
					{
						bool flag6 = this._anchoredItemEnd > index;
						if (flag6)
						{
							this._anchoredItemStart = index + 1;
						}
						else
						{
							this._anchoredItemStart = this._anchoredItemEnd;
						}
					}
				}
				bool flag7 = this._anchoredItemEnd == index;
				if (flag7)
				{
					bool flag8 = this._anchoredItemStart < index;
					if (flag8)
					{
						this._anchoredItemEnd = index - 1;
					}
					else
					{
						bool flag9 = this._anchoredItemStart > index;
						if (flag9)
						{
							this._anchoredItemEnd = index + 1;
						}
						else
						{
							this._anchoredItemEnd = this._anchoredItemStart;
						}
					}
				}
			}
			else
			{
				this._selectedIndices.Add(index);
				this._anchoredItemStart = index;
				this._anchoredItemEnd = index;
			}
			bool flag10 = this.SelectedIndicesChanged != null;
			if (flag10)
			{
				this.SelectedIndicesChanged(this, null);
			}
			base.Invalidate();
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000104B8 File Offset: 0x0000E6B8
		public void SelectItems(int startRange, int endRange)
		{
			this._selectedIndices.Clear();
			bool flag = startRange == endRange;
			if (flag)
			{
				this._selectedIndices.Add(startRange);
			}
			bool flag2 = startRange < endRange;
			if (flag2)
			{
				for (int i = startRange; i <= endRange; i++)
				{
					this._selectedIndices.Add(i);
				}
			}
			else
			{
				bool flag3 = startRange > endRange;
				if (flag3)
				{
					for (int j = startRange; j >= endRange; j--)
					{
						this._selectedIndices.Add(j);
					}
				}
			}
			bool flag4 = this.SelectedIndicesChanged != null;
			if (flag4)
			{
				this.SelectedIndicesChanged(this, null);
			}
			base.Invalidate();
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0001056A File Offset: 0x0000E76A
		private void SelectAnchoredRange(int index)
		{
			this._anchoredItemEnd = index;
			this.SelectItems(this._anchoredItemStart, index);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00010584 File Offset: 0x0000E784
		private void UpdateListBox()
		{
			using (Graphics g = base.CreateGraphics())
			{
				for (int i = 0; i <= this.Items.Count - 1; i++)
				{
					DarkListItem item = this.Items[i];
					this.UpdateItemSize(item, g);
					this.UpdateItemPosition(item, i);
				}
			}
			this.UpdateContentSize();
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00010600 File Offset: 0x0000E800
		private void UpdateItemSize(DarkListItem item)
		{
			using (Graphics g = base.CreateGraphics())
			{
				this.UpdateItemSize(item, g);
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00010640 File Offset: 0x0000E840
		private void UpdateItemSize(DarkListItem item, Graphics g)
		{
			SizeF size = g.MeasureString(item.Text, this.Font);
			float width = size.Width;
			size.Width = width + 1f;
			bool showIcons = this.ShowIcons;
			if (showIcons)
			{
				size.Width += (float)(this._iconSize + 8);
			}
			item.Area = new Rectangle(item.Area.Left, item.Area.Top, (int)size.Width, item.Area.Height);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000106D8 File Offset: 0x0000E8D8
		private void UpdateItemPosition(DarkListItem item, int index)
		{
			item.Area = new Rectangle(2, index * this.ItemHeight, item.Area.Width, this.ItemHeight);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00010710 File Offset: 0x0000E910
		private void UpdateContentSize()
		{
			int highestWidth = 0;
			foreach (DarkListItem item in this.Items)
			{
				bool flag = item.Area.Right + 1 > highestWidth;
				if (flag)
				{
					highestWidth = item.Area.Right + 1;
				}
			}
			int width = highestWidth;
			int height = this.Items.Count * this.ItemHeight;
			bool flag2 = base.ContentSize.Width != width || base.ContentSize.Height != height;
			if (flag2)
			{
				base.ContentSize = new Size(width, height);
				base.Invalidate();
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x000107E8 File Offset: 0x0000E9E8
		private void UpdateContentSize(DarkListItem item)
		{
			int itemWidth = item.Area.Right + 1;
			bool flag = itemWidth == base.ContentSize.Width;
			if (flag)
			{
				this.UpdateContentSize();
			}
			else
			{
				bool flag2 = itemWidth > base.ContentSize.Width;
				if (flag2)
				{
					base.ContentSize = new Size(itemWidth, base.ContentSize.Height);
					base.Invalidate();
				}
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00010864 File Offset: 0x0000EA64
		public void EnsureVisible()
		{
			bool flag = this.SelectedIndices.Count == 0;
			if (!flag)
			{
				bool flag2 = !this.MultiSelect;
				int itemTop;
				if (flag2)
				{
					itemTop = this.SelectedIndices[0] * this.ItemHeight;
				}
				else
				{
					itemTop = this._anchoredItemEnd * this.ItemHeight;
				}
				int itemBottom = itemTop + this.ItemHeight;
				bool flag3 = itemTop < base.Viewport.Top;
				if (flag3)
				{
					base.VScrollTo(itemTop);
				}
				bool flag4 = itemBottom > base.Viewport.Bottom;
				if (flag4)
				{
					base.VScrollTo(itemBottom - base.Viewport.Height);
				}
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00010918 File Offset: 0x0000EB18
		private IEnumerable<int> ItemIndexesInView()
		{
			int top = base.Viewport.Top / this.ItemHeight - 1;
			bool flag = top < 0;
			if (flag)
			{
				top = 0;
			}
			int bottom = (base.Viewport.Top + base.Viewport.Height) / this.ItemHeight + 1;
			bool flag2 = bottom > this.Items.Count;
			if (flag2)
			{
				bottom = this.Items.Count;
			}
			return Enumerable.Range(top, bottom - top);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000109A4 File Offset: 0x0000EBA4
		private IEnumerable<DarkListItem> ItemsInView()
		{
			IEnumerable<int> indexes = this.ItemIndexesInView();
			return indexes.Select((int index) => this.Items[index]).ToList<DarkListItem>();
		}

		// Token: 0x0600029D RID: 669 RVA: 0x000109D8 File Offset: 0x0000EBD8
		protected override void PaintContent(Graphics g)
		{
			List<int> range = this.ItemIndexesInView().ToList<int>();
			bool flag = range.Count == 0;
			if (!flag)
			{
				int top = range.Min();
				int bottom = range.Max();
				for (int i = top; i <= bottom; i++)
				{
					int width = Math.Max(base.ContentSize.Width, base.Viewport.Width);
					Rectangle rect = new Rectangle(0, i * this.ItemHeight, width, this.ItemHeight);
					bool odd = i % 2 != 0;
					Color bgColor = ((!odd) ? Colors.HeaderBackground : Colors.GreyBackground);
					bool flag2 = this.SelectedIndices.Count > 0 && this.SelectedIndices.Contains(i);
					if (flag2)
					{
						bgColor = (this.Focused ? Colors.GreyHighlight : Colors.GreySelection);
					}
					using (SolidBrush b = new SolidBrush(bgColor))
					{
						g.FillRectangle(b, rect);
					}
					using (Pen p = new Pen(Colors.DarkBorder))
					{
						g.DrawLine(p, new Point(rect.Left, rect.Bottom - 1), new Point(rect.Right, rect.Bottom - 1));
					}
					bool flag3 = this.ShowIcons && this.Items[i].Icon != null;
					if (flag3)
					{
						g.DrawImageUnscaled(this.Items[i].Icon, new Point(rect.Left + 5, rect.Top + rect.Height / 2 - this._iconSize / 2));
					}
					using (SolidBrush b2 = new SolidBrush(this.Items[i].TextColor))
					{
						StringFormat stringFormat = new StringFormat
						{
							Alignment = StringAlignment.Near,
							LineAlignment = StringAlignment.Center
						};
						Font modFont = new Font(this.Font, this.Items[i].FontStyle);
						Rectangle modRect = new Rectangle(rect.Left + 2, rect.Top, rect.Width, rect.Height);
						bool showIcons = this.ShowIcons;
						if (showIcons)
						{
							modRect.X += this._iconSize + 8;
						}
						g.DrawString(this.Items[i].Text, modFont, b2, modRect, stringFormat);
					}
				}
			}
		}

		// Token: 0x040001E6 RID: 486
		private int _itemHeight = 20;

		// Token: 0x040001E7 RID: 487
		private bool _multiSelect;

		// Token: 0x040001E8 RID: 488
		private readonly int _iconSize = 16;

		// Token: 0x040001E9 RID: 489
		private ObservableCollection<DarkListItem> _items;

		// Token: 0x040001EA RID: 490
		private List<int> _selectedIndices;

		// Token: 0x040001EB RID: 491
		private int _anchoredItemStart = -1;

		// Token: 0x040001EC RID: 492
		private int _anchoredItemEnd = -1;
	}
}
