using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
            if (checkBox1.Checked)
            {
                LoadFilenameText();
            }
            else
            {
                LoadObjectiveText();
            }
        }
        private void LoadFilenameText()
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
                //TreeNode newnode = new TreeNode(objs.ObjectiveText);
                TreeNode newnode = new TreeNode();
                newnode.Tag = objs;
                switch (objs._ObjectiveTypeEnum)
                {
                    case QuExpansionQuestObjectiveTypeestType.TARGET:
                        QuestObjectivesTarget t = objs as QuestObjectivesTarget;
                        newnode.Text = t.Filename;
                        ObjectivesTarget.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TRAVEL:
                        QuestObjectivesTravel tr = objs as QuestObjectivesTravel;
                        newnode.Text = tr.Filename;
                        ObjectivesTravel.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.COLLECT:
                        QuestObjectivesCollection coll = objs as QuestObjectivesCollection;
                        newnode.Text = coll.Filename;
                        ObjectivesCollection.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.DELIVERY:
                        QuestObjectivesDelivery del = objs as QuestObjectivesDelivery;
                        newnode.Text = del.Filename;
                        ObjectivesDelivery.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TREASUREHUNT:
                        QuestObjectivesTreasureHunt th = objs as QuestObjectivesTreasureHunt;
                        newnode.Text = th.Filename;
                        ObjectivesTreasureHunt.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIPATROL:
                        QuestObjectivesAIPatrol aip = objs as QuestObjectivesAIPatrol;
                        newnode.Text = aip.Filename;
                        ObjectivesAIPatrol.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AICAMP:
                        QuestObjectivesAICamp aic = objs as QuestObjectivesAICamp;
                        newnode.Text = aic.Filename;
                        ObjectivesAICamp.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIVIP:
                        QuestObjectivesAIVIP aivip = objs as QuestObjectivesAIVIP;
                        newnode.Text = aivip.Filename;
                        ObjectivesAIVIP.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.ACTION:
                        QuestObjectivesAction a = objs as QuestObjectivesAction;
                        newnode.Text = a.Filename;
                        ObjectivesAction.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.CRAFTING:
                        QuestObjectivesCrafting c = objs as QuestObjectivesCrafting;
                        newnode.Text = c.Filename;
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
        private void LoadObjectiveText()
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
                //TreeNode newnode = new TreeNode(objs.ObjectiveText);
                TreeNode newnode = new TreeNode();
                newnode.Tag = objs;
                switch (objs._ObjectiveTypeEnum)
                {
                    case QuExpansionQuestObjectiveTypeestType.TARGET:
                        QuestObjectivesTarget t = objs as QuestObjectivesTarget;
                        newnode.Text = t.ObjectiveText;
                        ObjectivesTarget.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TRAVEL:
                        QuestObjectivesTravel tr = objs as QuestObjectivesTravel;
                        newnode.Text = tr.ObjectiveText;
                        ObjectivesTravel.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.COLLECT:
                        QuestObjectivesCollection coll = objs as QuestObjectivesCollection;
                        newnode.Text = coll.ObjectiveText;
                        ObjectivesCollection.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.DELIVERY:
                        QuestObjectivesDelivery del = objs as QuestObjectivesDelivery;
                        newnode.Text = del.ObjectiveText;
                        ObjectivesDelivery.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TREASUREHUNT:
                        QuestObjectivesTreasureHunt th = objs as QuestObjectivesTreasureHunt;
                        newnode.Text = th.ObjectiveText;
                        ObjectivesTreasureHunt.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIPATROL:
                        QuestObjectivesAIPatrol aip = objs as QuestObjectivesAIPatrol;
                        newnode.Text = aip.ObjectiveText;
                        ObjectivesAIPatrol.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AICAMP:
                        QuestObjectivesAICamp aic = objs as QuestObjectivesAICamp;
                        newnode.Text = aic.ObjectiveText;
                        ObjectivesAICamp.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIVIP:
                        QuestObjectivesAIVIP aivip = objs as QuestObjectivesAIVIP;
                        newnode.Text = aivip.ObjectiveText;
                        ObjectivesAIVIP.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.ACTION:
                        QuestObjectivesAction a = objs as QuestObjectivesAction;
                        newnode.Text = a.ObjectiveText;
                        ObjectivesAction.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.CRAFTING:
                        QuestObjectivesCrafting c = objs as QuestObjectivesCrafting;
                        newnode.Text = c.ObjectiveText;
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                LoadFilenameText();
            }
            else
            {
                LoadObjectiveText();
            }
        }
    }
}
