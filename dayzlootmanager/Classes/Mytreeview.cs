using DarkUI.Forms;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.Encodings.Web;
using Cyotek.Windows.Forms;
using DayZeLib;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Text;

namespace DayZeEditor
{
    public class MytreeNode : TreeNode
    {
        public MytreeNode(string text) : base(text)
        {
        }

        public int BuySell { get; set; }
    }
}
