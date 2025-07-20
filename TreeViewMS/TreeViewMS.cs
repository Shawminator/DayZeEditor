using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TreeViewMS
{
    public class TreeViewMS : TreeView
	{
        [Category("Behavior")]
        public event EventHandler<NodeRequestTextEventArgs> RequestDisplayText;

        [Category("Behavior")]
        public event EventHandler<NodeRequestTextEventArgs> RequestEditText;

        protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
        {
            if (e.Label != null) // if the user cancelled the edit this event is still raised, just with a null label
            {
                NodeRequestTextEventArgs displayTextArgs;

                displayTextArgs = new NodeRequestTextEventArgs(e.Node, e.Label);
                this.OnRequestDisplayText(displayTextArgs);

                e.CancelEdit = true; // cancel the built in operation so we can substitute our own

                if (!displayTextArgs.Cancel)
                    e.Node.Text = displayTextArgs.Label;
            }

            base.OnAfterLabelEdit(e);
        }

        protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
        {
            NodeRequestTextEventArgs editTextArgs;

            // get the text to apply to the label
            editTextArgs = new NodeRequestTextEventArgs(e.Node, e.Node.Text);
            this.OnRequestEditText(editTextArgs);

            // cancel the edit if required
            if (editTextArgs.Cancel)
                e.CancelEdit = true;

            // apply the text to the EDIT control
            if (!e.CancelEdit)
            {
                IntPtr editHandle;

                editHandle = NativeMethods.SendMessage(this.Handle, NativeMethods.TVM_GETEDITCONTROL, IntPtr.Zero, IntPtr.Zero); // Get the handle of the EDIT control
                if (editHandle != IntPtr.Zero)
                    NativeMethods.SendMessage(editHandle, NativeMethods.WM_SETTEXT, IntPtr.Zero, editTextArgs.Label); // And apply the text. Simples.
            }

            base.OnBeforeLabelEdit(e);
        }

        /// <summary>
        /// Raises the <see cref="RequestDisplayText" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnRequestDisplayText(NodeRequestTextEventArgs e)
        {
            EventHandler<NodeRequestTextEventArgs> handler;

            handler = this.RequestDisplayText;

            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnRequestEditText(NodeRequestTextEventArgs e)
        {
            EventHandler<NodeRequestTextEventArgs> handler;

            handler = this.RequestEditText;

            if (handler != null)
                handler(this, e);
        }

        public bool SetMultiselect
		{
			get
			{
				return this.m_CanMultiSelect;
			}
			set
			{
				this.m_CanMultiSelect = value;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002072 File Offset: 0x00001072
		public TreeViewMS()
		{
			this.m_coll = new ArrayList();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000208E File Offset: 0x0000108E
		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000209C File Offset: 0x0000109C
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000020B4 File Offset: 0x000010B4
		public ArrayList SelectedNodes
		{
			get
			{
				return this.m_coll;
			}
			set
			{
				this.removePaintFromNodes();
				this.m_coll.Clear();
				this.m_coll = value;
				this.paintSelectedNodes();
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020D8 File Offset: 0x000010D8
		protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
		{
			base.OnBeforeSelect(e);
			bool bControl = Control.ModifierKeys == Keys.Control;
			bool bShift = Control.ModifierKeys == Keys.Shift;
			bool flag = bControl && this.m_coll.Contains(e.Node);
			if (flag)
			{
				e.Cancel = true;
				this.removePaintFromNodes();
				this.m_coll.Remove(e.Node);
				this.paintSelectedNodes();
			}
			else
			{
				this.m_lastNode = e.Node;
				bool flag2 = !bShift;
				if (flag2)
				{
					this.m_firstNode = e.Node;
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002170 File Offset: 0x00001170
		protected override void OnAfterSelect(TreeViewEventArgs e)
		{
			base.OnAfterSelect(e);
			bool bControl = Control.ModifierKeys == Keys.Control;
			bool bShift = Control.ModifierKeys == Keys.Shift;
			bool flag = !this.m_CanMultiSelect;
			if (flag)
			{
				bControl = false;
				bShift = false;
			}
			bool flag2 = bControl;
			if (flag2)
			{
				bool flag3 = !this.m_coll.Contains(e.Node);
				if (flag3)
				{
					this.m_coll.Add(e.Node);
				}
				else
				{
					this.removePaintFromNodes();
					this.m_coll.Remove(e.Node);
				}
				this.paintSelectedNodes();
			}
			else
			{
				bool flag4 = bShift;
				if (flag4)
				{
					Queue myQueue = new Queue();
					TreeNode uppernode = this.m_firstNode;
					TreeNode bottomnode = e.Node;
					bool bParent = this.isParent(this.m_firstNode, e.Node);
					bool flag5 = !bParent;
					if (flag5)
					{
						bParent = this.isParent(bottomnode, uppernode);
						bool flag6 = bParent;
						if (flag6)
						{
							TreeNode t = uppernode;
							uppernode = bottomnode;
							bottomnode = t;
						}
					}
					bool flag7 = bParent;
					if (flag7)
					{
						for (TreeNode i = bottomnode; i != uppernode.Parent; i = i.Parent)
						{
							bool flag8 = !this.m_coll.Contains(i);
							if (flag8)
							{
								myQueue.Enqueue(i);
							}
						}
					}
					else
					{
						bool flag9 = (uppernode.Parent == null && bottomnode.Parent == null) || (uppernode.Parent != null && uppernode.Parent.Nodes.Contains(bottomnode));
						if (flag9)
						{
							int nIndexUpper = uppernode.Index;
							int nIndexBottom = bottomnode.Index;
							bool flag10 = nIndexBottom < nIndexUpper;
							if (flag10)
							{
								TreeNode t2 = uppernode;
								uppernode = bottomnode;
								bottomnode = t2;
								nIndexUpper = uppernode.Index;
								nIndexBottom = bottomnode.Index;
							}
							TreeNode j = uppernode;
							while (nIndexUpper <= nIndexBottom)
							{
								bool flag11 = !this.m_coll.Contains(j);
								if (flag11)
								{
									myQueue.Enqueue(j);
								}
								j = j.NextNode;
								nIndexUpper++;
							}
						}
						else
						{
							bool flag12 = !this.m_coll.Contains(uppernode);
							if (flag12)
							{
								myQueue.Enqueue(uppernode);
							}
							bool flag13 = !this.m_coll.Contains(bottomnode);
							if (flag13)
							{
								myQueue.Enqueue(bottomnode);
							}
						}
					}
					this.m_coll.AddRange(myQueue);
					this.paintSelectedNodes();
					this.m_firstNode = e.Node;
				}
				else
				{
					bool flag14 = this.m_coll != null && this.m_coll.Count > 0;
					if (flag14)
					{
						this.removePaintFromNodes();
						this.m_coll.Clear();
					}
					this.m_coll.Add(e.Node);
				}
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002444 File Offset: 0x00001444
		protected bool isParent(TreeNode parentNode, TreeNode childNode)
		{
			bool flag = parentNode == childNode;
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				TreeNode i = childNode;
				bool bFound = false;
				while (!bFound && i != null)
				{
					i = i.Parent;
					bFound = i == parentNode;
				}
				flag2 = bFound;
			}
			return flag2;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002488 File Offset: 0x00001488
		protected void paintSelectedNodes()
		{
			foreach (object obj in this.m_coll)
			{
				TreeNode i = (TreeNode)obj;
				i.BackColor = SystemColors.Highlight;
				i.ForeColor = SystemColors.HighlightText;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000024F8 File Offset: 0x000014F8
		protected void removePaintFromNodes()
		{
			bool flag = this.m_coll.Count == 0;
			if (!flag)
			{
				TreeNode n0 = (TreeNode)this.m_coll[0];
				bool flag2 = n0.TreeView == null;
				if (!flag2)
				{
					Color back = n0.TreeView.BackColor;
					Color fore = n0.TreeView.ForeColor;
					foreach (object obj in this.m_coll)
					{
						TreeNode i = (TreeNode)obj;
						i.BackColor = back;
						i.ForeColor = fore;
					}
				}
			}
		}

		// Token: 0x04000001 RID: 1
		protected ArrayList m_coll;

		// Token: 0x04000002 RID: 2
		protected TreeNode m_lastNode;

		// Token: 0x04000003 RID: 3
		protected TreeNode m_firstNode;

		// Token: 0x04000004 RID: 4
		protected bool m_CanMultiSelect = true;
	}
}
