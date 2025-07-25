using System;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Docking;

namespace DarkUI.Win32
{
	// Token: 0x02000009 RID: 9
	public class DockResizeFilter : IMessageFilter
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002890 File Offset: 0x00000A90
		public DockResizeFilter(DarkDockPanel dockPanel)
		{
			this._dockPanel = dockPanel;
			this._dragTimer = new Timer();
			this._dragTimer.Interval = 1;
			this._dragTimer.Tick += this.DragTimer_Tick;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000028DC File Offset: 0x00000ADC
		public bool PreFilterMessage(ref Message m)
		{
			bool flag = m.Msg != 512 && m.Msg != 513 && m.Msg != 514 && m.Msg != 515 && m.Msg != 516 && m.Msg != 517 && m.Msg != 518;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = m.Msg == 514;
				if (flag3)
				{
					bool isDragging = this._isDragging;
					if (isDragging)
					{
						this.StopDrag();
						return true;
					}
				}
				bool flag4 = m.Msg == 514 && !this._isDragging;
				if (flag4)
				{
					flag2 = false;
				}
				else
				{
					bool isDragging2 = this._isDragging;
					if (isDragging2)
					{
						Cursor.Current = this._activeSplitter.ResizeCursor;
					}
					bool flag5 = m.Msg == 512 && !this._isDragging && this._dockPanel.MouseButtonState > MouseButtons.None;
					if (flag5)
					{
						flag2 = false;
					}
					else
					{
						Control control = Control.FromHandle(m.HWnd);
						bool flag6 = control == null;
						if (flag6)
						{
							flag2 = false;
						}
						else
						{
							bool flag7 = control != this._dockPanel && !this._dockPanel.Contains(control);
							if (flag7)
							{
								flag2 = false;
							}
							else
							{
								this.CheckCursor();
								bool flag8 = m.Msg == 513;
								if (flag8)
								{
									DarkDockSplitter hotSplitter = this.HotSplitter();
									bool flag9 = hotSplitter != null;
									if (flag9)
									{
										this.StartDrag(hotSplitter);
										return true;
									}
								}
								bool flag10 = this.HotSplitter() != null;
								if (flag10)
								{
									flag2 = true;
								}
								else
								{
									bool isDragging3 = this._isDragging;
									flag2 = isDragging3;
								}
							}
						}
					}
				}
			}
			return flag2;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002AA4 File Offset: 0x00000CA4
		private void DragTimer_Tick(object sender, EventArgs e)
		{
			bool flag = this._dockPanel.MouseButtonState != MouseButtons.Left;
			if (flag)
			{
				this.StopDrag();
			}
			else
			{
				Point difference = new Point(this._initialContact.X - Cursor.Position.X, this._initialContact.Y - Cursor.Position.Y);
				this._activeSplitter.UpdateOverlay(difference);
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002B1C File Offset: 0x00000D1C
		private void StartDrag(DarkDockSplitter splitter)
		{
			this._activeSplitter = splitter;
			Cursor.Current = this._activeSplitter.ResizeCursor;
			this._initialContact = Cursor.Position;
			this._isDragging = true;
			this._activeSplitter.ShowOverlay();
			this._dragTimer.Start();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002B6C File Offset: 0x00000D6C
		private void StopDrag()
		{
			this._dragTimer.Stop();
			this._activeSplitter.HideOverlay();
			Point difference = new Point(this._initialContact.X - Cursor.Position.X, this._initialContact.Y - Cursor.Position.Y);
			this._activeSplitter.Move(difference);
			this._isDragging = false;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002BE0 File Offset: 0x00000DE0
		private DarkDockSplitter HotSplitter()
		{
			foreach (DarkDockSplitter splitter in this._dockPanel.Splitters)
			{
				bool flag = splitter.Bounds.Contains(Cursor.Position);
				if (flag)
				{
					return splitter;
				}
			}
			return null;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002C5C File Offset: 0x00000E5C
		private void CheckCursor()
		{
			bool isDragging = this._isDragging;
			if (!isDragging)
			{
				DarkDockSplitter hotSplitter = this.HotSplitter();
				bool flag = hotSplitter != null;
				if (flag)
				{
					Cursor.Current = hotSplitter.ResizeCursor;
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002C91 File Offset: 0x00000E91
		private void ResetCursor()
		{
			Cursor.Current = Cursors.Default;
			this.CheckCursor();
		}

		// Token: 0x0400000D RID: 13
		private DarkDockPanel _dockPanel;

		// Token: 0x0400000E RID: 14
		private Timer _dragTimer;

		// Token: 0x0400000F RID: 15
		private bool _isDragging;

		// Token: 0x04000010 RID: 16
		private Point _initialContact;

		// Token: 0x04000011 RID: 17
		private DarkDockSplitter _activeSplitter;
	}
}
