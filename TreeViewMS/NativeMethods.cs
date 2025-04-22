using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TreeViewMS
{
    public static class NativeMethods
    {

        #region Constants

        public const int TVN_FIRST = -400;

        public const int TVN_BEGINLABELEDITW = (TVN_FIRST - 59);

        public const int TVN_BEGINLABELEDIT = TVN_BEGINLABELEDITW;

        public const int TVM_GETEDITCONTROL = 0x110F;

        public const int WM_SETTEXT = 0xC;

        public const int WM_USER = 0x0400;

        public const int WM_NOTIFY = 0x004E;

        public const int WM_REFLECT = WM_USER + 0x1c00;

        #endregion

        #region Class Members

        [DllImport("USER32", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("USER32", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        #endregion

        #region Nested Types

        [StructLayout(LayoutKind.Sequential)]
        public struct NMHDR
        {
            public IntPtr hwndFrom;

            public IntPtr idFrom;

            public int code;
        }

        #endregion
    }
}
