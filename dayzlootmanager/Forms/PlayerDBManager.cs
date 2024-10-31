using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;
using BIS.PAA;
using System.Text.RegularExpressions;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Web.UI.Design.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.VisualBasic;

namespace DayZeEditor
{
    public partial class PlayerDBManager : DarkForm
    {
        internal Project currentproject;
        private string DBPath { get; set; }
        public PlayerDB playerDB { get; private set; }

        public PlayerDBManager()
        {
            InitializeComponent();
            
        }


        private void SaveFileButton_Click(object sender, EventArgs e)
        {

        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void PlayerDBManager_Load(object sender, EventArgs e)
        {
            if (currentproject != null)
            {
                DBPath = toolStripTextBox1.Text = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\storage_1\\players.db";
                playerDB = new PlayerDB(DBPath);
                //foreach(DBPlayers dbp in playerDB.PlayerList)
                //{
                //    File.WriteAllBytes(Path.GetDirectoryName(DBPath) + "\\" + dbp.UID, dbp.PlayerdataBlob);
                //}

                LoadPlayerList();
            }
            
        }

        private void LoadPlayerList()
        {
            treeViewMS1.Nodes.Clear();
            TreeNode root = new TreeNode("Player List")
            {
                Tag = "Parent"
            };
            foreach(DBPlayers p in playerDB.PlayerList)
            {
                TreeNode player = new TreeNode(p.UID);
                player.Nodes.Add(new TreeNode($"Database ID: {p.DBID}") { Tag = "p:" + p.DBID });
                player.Nodes.Add(new TreeNode($"isAlive: {p.isAlive}") { Tag = "p:" + p.isAlive });
                TreeNode Serverstats = new TreeNode("Server Stats");
                foreach(ServerStat ss in p.playerdata.dBStats.ServerStats.serverStats)
                {
                    Serverstats.Nodes.Add(new TreeNode($"{ss.name}: {ss.Value.ToString()}"));
                }
                player.Nodes.Add(Serverstats);
                TreeNode inventory = new TreeNode("Inventory");
               
                foreach (DBItem pi in p.playerdata.InventoryItems)
                {
                    inventory.Nodes.Add(loadItems(pi));
                }
                player.Nodes.Add(inventory);
                root.Nodes.Add(player);
            }
            treeViewMS1.Nodes.Add(root);
        }

        private TreeNode loadItems(DBItem pi)
        {
            TreeNode itemNode = new TreeNode($"Slot: {pi.Slot}") { Tag = pi };

            itemNode.Nodes.Add(new TreeNode($"Classname:{pi.Classname}"));
            itemNode.Nodes.Add(new TreeNode($"ID: {pi.DBItemData.PersistentGuid}") { Tag = "GUID:" + pi.DBItemData.PersistentGuid });
            TreeNode cargo = new TreeNode("Cargo");
            TreeNode Attachemnts = new TreeNode("Attachments");
            foreach (var child in pi.Children)
            {
                TreeNode childitem = loadItems(child);
                if(child.Slot == "cargo")
                    cargo.Nodes.Add(childitem);
                else
                    Attachemnts.Nodes.Add(childitem);
            }
            if(cargo.Nodes.Count > 0) 
                itemNode.Nodes.Add(cargo);
            if(Attachemnts.Nodes.Count > 0)
                itemNode.Nodes.Add(Attachemnts);
            return itemNode;
        }

        private void darkButton60_Click(object sender, EventArgs e)
        {

        }
    }
}
