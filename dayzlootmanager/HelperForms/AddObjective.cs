using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class AddObjectives : DarkForm
    {
        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            ListBox lb = sender as ListBox;
            e.DrawBackground();
            Brush myBrush = Brushes.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            }
            else
            {
                myBrush = Brushes.White;
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(60, 63, 65)), e.Bounds);
            }
            e.Graphics.DrawString(lb.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds);
            e.DrawFocusRectangle();
        }

        public BindingList<QuestObjectivesBase> QuestObjectives { get; set; }
        public List<QuestObjectivesBase> selectedquests { get; set; }
        public AddObjectives()
        {
            InitializeComponent();
            Form_Controls_AddfromType.InitializeForm_Controls
            (
                this,
                panel1,
                TitleLabel,
                CloseButton
            );
        }

        private void AddObjectives_Load(object sender, EventArgs e)
        {
            treeViewMS1.Nodes.Clear();
            TreeNode root = new TreeNode("Objectives")
            {
                Tag = "Parent"
            };
            TreeNode ObjectivesAction = new TreeNode("Action")
            {
                Tag = "Action"
            };
            TreeNode ObjectivesAICamp = new TreeNode("AICamp")
            {
                Tag = "AICamp"
            };
            TreeNode ObjectivesAIPatrol = new TreeNode("AIPatrol")
            {
                Tag = "AIPatrol"
            };
            TreeNode ObjectivesAIVIP = new TreeNode("AIVIP")
            {
                Tag = "AIVIP"
            };
            TreeNode ObjectivesCollection = new TreeNode("Collection")
            {
                Tag = "Collection"
            };
            TreeNode ObjectivesCrafting = new TreeNode("Crafting")
            {
                Tag = "Crafting"
            };
            TreeNode ObjectivesDelivery = new TreeNode("Delivery")
            {
                Tag = "Delivery"
            };
            TreeNode ObjectivesTarget = new TreeNode("Target")
            {
                Tag = "Target"
            };
            TreeNode ObjectivesTravel = new TreeNode("Travel")
            {
                Tag = "Travel"
            };
            TreeNode ObjectivesTreasureHunt = new TreeNode("TreasureHunt")
            {
                Tag = "TreasureHunt"
            };
            foreach (QuestObjectivesBase objs in QuestObjectives)
            {
                TreeNode newnode = new TreeNode(objs.ObjectiveText);
                newnode.Tag = objs;
                switch (objs.QuestType)
                {
                    case QuestType.TARGET:
                        ObjectivesTarget.Nodes.Add(newnode);
                        break;
                    case QuestType.TRAVEL:
                        ObjectivesTravel.Nodes.Add(newnode);
                        break;
                    case QuestType.COLLECT:
                        ObjectivesCollection.Nodes.Add(newnode);
                        break;
                    case QuestType.DELIVERY:
                        ObjectivesDelivery.Nodes.Add(newnode);
                        break;
                    case QuestType.TREASUREHUNT:
                        ObjectivesTreasureHunt.Nodes.Add(newnode);
                        break;
                    case QuestType.AIPATROL:
                        ObjectivesAIPatrol.Nodes.Add(newnode);
                        break;
                    case QuestType.AICAMP:
                        ObjectivesAICamp.Nodes.Add(newnode);
                        break;
                    case QuestType.AIVIP:
                        ObjectivesAIVIP.Nodes.Add(newnode);
                        break;
                    case QuestType.ACTION:
                        ObjectivesAction.Nodes.Add(newnode);
                        break;
                    case QuestType.CRAFTING:
                        ObjectivesCrafting.Nodes.Add(newnode);
                        break;
                    default:
                        break;
                }
            }
            root.Nodes.Add(ObjectivesAction);
            root.Nodes.Add(ObjectivesAICamp);
            root.Nodes.Add(ObjectivesAIPatrol);
            root.Nodes.Add(ObjectivesAIVIP);
            root.Nodes.Add(ObjectivesCollection);
            root.Nodes.Add(ObjectivesCrafting);
            root.Nodes.Add(ObjectivesDelivery);
            root.Nodes.Add(ObjectivesTarget);
            root.Nodes.Add(ObjectivesTravel);
            root.Nodes.Add(ObjectivesTreasureHunt);
            treeViewMS1.Nodes.Add(root);
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            selectedquests = new List<QuestObjectivesBase>();
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                QuestObjectivesBase quest = tn.Tag as QuestObjectivesBase;
                selectedquests.Add(quest);
            }
        }
    }
}
