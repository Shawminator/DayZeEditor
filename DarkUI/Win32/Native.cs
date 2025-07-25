using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DarkUI.Win32
{
	// Token: 0x0200000B RID: 11
	internal sealed class Native
	{
		// Token: 0x06000045 RID: 69
		[DllImport("user32.dll")]
		internal static extern IntPtr WindowFromPoint(Point point);

		// Token: 0x06000046 RID: 70
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
	}
}
