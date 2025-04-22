using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeViewMS
{
    public class NodeRequestTextEventArgs : CancelEventArgs
    {
        #region Constructors

        public NodeRequestTextEventArgs(TreeNode node, string label)
          : this()
        {
            this.Node = node;
            this.Label = label;
        }

        protected NodeRequestTextEventArgs()
        { }

        #endregion

        #region Properties

        public string Label { get; set; }

        public TreeNode Node { get; protected set; }

        #endregion
    }
}
