using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace DayZeEditor
{
    public class MytreeNode : TreeNode
    {
        public MytreeNode(string text) : base(text)
        {
        }

        public int BuySell { get; set; }
    }
    public partial class myXmlDocument : XmlDocument
    {
        public bool isDirty { get; set; }
        public string Filename { get; set; }
    }
    public static class XmlNodeExtensions
    {
        public static IEnumerable<XmlNode> PreviousSiblings(this XmlNode node)
        {
            for (XmlNode sibling = node.PreviousSibling; sibling != null; sibling = sibling.PreviousSibling)
            {
                yield return sibling;
            }
        }

        public static IEnumerable<XmlNode> NextSiblings(this XmlNode node)
        {
            for (XmlNode sibling = node.NextSibling; sibling != null; sibling = sibling.NextSibling)
            {
                yield return sibling;
            }
        }
    }
}
