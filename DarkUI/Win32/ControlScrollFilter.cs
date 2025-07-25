using System;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Win32
{
	// Token: 0x02000008 RID: 8
	public class ControlScrollFilter : IMessageFilter
	{
		// Token: 0x06000035 RID: 53 RVA: 0x0000280C File Offset: 0x00000A0C
		public bool PreFilterMessage(ref Message m)
		{
			int msg = m.Msg;
			int num = msg;
			bool flag;
			if (num != 522 && num != 526)
			{
				flag = false;
			}
			else
			{
				IntPtr hControlUnderMouse = Native.WindowFromPoint(new Point((int)m.LParam));
				bool flag2 = hControlUnderMouse == m.HWnd;
				if (flag2)
				{
					flag = false;
				}
				else
				{
					Native.SendMessage(hControlUnderMouse, (uint)m.Msg, m.WParam, m.LParam);
					flag = true;
				}
			}
			return flag;
		}
	}
}
