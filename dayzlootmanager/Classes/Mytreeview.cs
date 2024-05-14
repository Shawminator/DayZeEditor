using System.Windows.Forms;

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
