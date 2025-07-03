using BIS.Core;
using DarkUI.Controls;
using DarkUI.Forms;
using DayZeLib;
using FastColoredTextBoxNS;
using OpenTK.Graphics.OpenGL;
using PVZ.DarkHorde.Other.EventManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace DayZeEditor
{
    public partial class TerjeManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;
        public string Projectname;
        private bool useraction = false;
        public string TerjeSettingsPath { get; set; }
        public BindingList<TerjeCFGFIle> cfgfiles;
        public TerjeRecipes TerjeCraftingFiles;
        public BindingList<TerjeProtectionIDs> protectionIDsList;
        public TerjeLoadouts TerjeLoadouts;
        public TerjeRespawns TerjeRespawns;
        public TerjeFaces TerjeFaces;
        public TerjeGeneral TerjeGeneral;
        public TerjeScriptableAreas TerjeScriptableAreas;
        public TerjeCFGFIle currentCFGFIle { get; set; }
        public TerjeCFGLine currentCFGline { get; set; }
        private Dictionary<string, List<ComboBoxItem>> perkMap = new Dictionary<string, List<ComboBoxItem>>();

        public TerjeManager()
        {
            InitializeComponent();
        }
        private void TerjeManager_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            useraction = false;

            LoadSkillsAndPerksFromSingleFile();
            PopulateSkillsCheckBoxes(SAFLP);
            //loading cfg files.
            cfgfiles = new BindingList<TerjeCFGFIle>();
            TerjeSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TerjeSettings\\";
            DirectoryInfo d = new DirectoryInfo(TerjeSettingsPath); //Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.cfg"); //Getting Text files
            foreach(FileInfo info in Files)
            {
                Console.Write("serializing " + Path.GetFileName(info.FullName));
                TerjeCFGFIle newcfgfile = new TerjeCFGFIle()
                {
                    Filename = info.FullName
                };
                newcfgfile.Read(info.FullName);
                if (newcfgfile != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("  OK....");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                cfgfiles.Add(newcfgfile);
            }
            if(File.Exists(TerjeSettingsPath + "CustomCrafting\\Recipes.xml"))
            {
                Console.Write("serializing " + Path.GetFileName(TerjeSettingsPath + "CustomCrafting\\Recipes.xml"));
                XmlSerializer serializer = new XmlSerializer(typeof(TerjeRecipes));
                using (StreamReader reader = new StreamReader(TerjeSettingsPath + "CustomCrafting\\Recipes.xml"))
                {
                    TerjeCraftingFiles = (TerjeRecipes)serializer.Deserialize(reader);
                    TerjeCraftingFiles.Filename = TerjeSettingsPath + "CustomCrafting\\Recipes.xml";
                }
                if (TerjeCraftingFiles != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("  OK....");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            if(Directory.Exists(TerjeSettingsPath + "CustomProtection"))
            {
                DirectoryInfo dir = new DirectoryInfo(TerjeSettingsPath + "CustomProtection"); //Assuming Test is your Folder
                FileInfo[] protectionfiles = dir.GetFiles("*.txt"); //Getting Text files
                protectionIDsList = new BindingList<TerjeProtectionIDs>();
                foreach (FileInfo info in protectionfiles)
                {
                    Console.Write("serializing " + Path.GetFileName(info.FullName));
                    TerjeProtectionIDs newids = new TerjeProtectionIDs(info.FullName);
                    protectionIDsList.Add(newids);
                    if (newids != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            if (Directory.Exists(TerjeSettingsPath + "StartScreen"))
            {
                if (File.Exists(TerjeSettingsPath + "StartScreen\\Loadouts.xml"))
                {
                    Console.Write("serializing " + Path.GetFileName(TerjeSettingsPath + "StartScreen\\Loadouts.xml"));
                    XmlSerializer mySerializer = new XmlSerializer(typeof(TerjeLoadouts));
                    using (StreamReader reader = new StreamReader(TerjeSettingsPath + "StartScreen\\Loadouts.xml"))
                    {
                        TerjeLoadouts = (TerjeLoadouts)mySerializer.Deserialize(reader);
                        TerjeLoadouts.Filename = TerjeSettingsPath + "StartScreen\\Loadouts.xml";
                    }
                    if (TerjeLoadouts != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                if (File.Exists(TerjeSettingsPath + "StartScreen\\Respawns.xml"))
                {
                    Console.Write("serializing " + Path.GetFileName(TerjeSettingsPath + "StartScreen\\Respawns.xml"));
                    XmlSerializer mySerializer = new XmlSerializer(typeof(TerjeRespawns));
                    using (StreamReader reader = new StreamReader(TerjeSettingsPath + "StartScreen\\Respawns.xml"))
                    {
                        TerjeRespawns = (TerjeRespawns)mySerializer.Deserialize(reader);
                        TerjeRespawns.Filename = TerjeSettingsPath + "StartScreen\\Respawns.xml";
                    }
                    if (TerjeRespawns != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                if (File.Exists(TerjeSettingsPath + "StartScreen\\Faces.xml"))
                {
                    Console.Write("serializing " + Path.GetFileName(TerjeSettingsPath + "StartScreen\\Faces.xml"));
                    XmlSerializer mySerializer = new XmlSerializer(typeof(TerjeFaces));
                    using (StreamReader reader = new StreamReader(TerjeSettingsPath + "StartScreen\\Faces.xml"))
                    {
                        TerjeFaces = (TerjeFaces)mySerializer.Deserialize(reader);
                        TerjeFaces.Filename = TerjeSettingsPath + "StartScreen\\Faces.xml";
                    }
                    if (TerjeFaces != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                if (File.Exists(TerjeSettingsPath + "StartScreen\\General.xml"))
                {
                    Console.Write("serializing " + Path.GetFileName(TerjeSettingsPath + "StartScreen\\General.xml"));
                    XmlSerializer mySerializer = new XmlSerializer(typeof(TerjeGeneral));
                    using (StreamReader reader = new StreamReader(TerjeSettingsPath + "StartScreen\\General.xml"))
                    {
                        TerjeGeneral = (TerjeGeneral)mySerializer.Deserialize(reader);
                        TerjeGeneral.Filename = TerjeSettingsPath + "StartScreen\\General.xml";
                    }
                    if (TerjeGeneral != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            if (Directory.Exists(TerjeSettingsPath + "ScriptableAreas"))
            {
                if (File.Exists(TerjeSettingsPath + "ScriptableAreas\\ScriptableAreasSpawner.xml"))
                {
                    Console.Write("serializing " + Path.GetFileName(TerjeSettingsPath + "ScriptableAreas\\ScriptableAreasSpawner.xml"));
                    XmlSerializer mySerializer = new XmlSerializer(typeof(TerjeScriptableAreas));
                    using (StreamReader reader = new StreamReader(TerjeSettingsPath + "ScriptableAreas\\ScriptableAreasSpawner.xml"))
                    {
                        TerjeScriptableAreas = (TerjeScriptableAreas)mySerializer.Deserialize(reader);
                        TerjeScriptableAreas.Filename = TerjeSettingsPath + "ScriptableAreas\\ScriptableAreasSpawner.xml";
                    }
                    if (TerjeScriptableAreas != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }

            SetToolTips();
            LoadCFGtoTreeview();
            useraction = true;
        }

        private void SetToolTips()
        {
            var toolTips = new Dictionary<Control, string>
            {
                { label11, "(required) skill identifier (you can see all skills identifiers in config.cpp CfgTerjeSkills section." },
                { label24, "(required) perk identifier (you can see all perks identifiers in config.cpp CfgTerjeSkills section." },
                { label22, "(required) perk level must be equal to or higher than this value to have access to this loadout/respawn/recipe." },
                { label54, "(optional, default \"false\") when this attribute is set to \"true\", the loadout/respawn will be hidden for players who do not equal this condition." },
                { label37, "(required) loadout identifier,\nMust be short and unique for each individual loadout." },
                { label36, "(required) name of loadout that the player will see in the game UI.\nCan be used key from stringtable.csv for localication to all supported languages." },
                { label39, "(required) item classname" },
                { label40, "(optional) overrides the name of the item when used in the selection menu" },
                { label42, "(optional) quantity of item, number from 0 to 1 (where 1 is 100%, 0.5 - 50%, etc). \nCan be used as a range \"0.0:1.0\" for random result between 2 values." },
                { label41, "(optional) items count in the stack.\nCan be used as a range \"10:20\" for random result between 2 values."},
                { label46, "(optional) health of item, number from 0 to 1 (where 1 is 100%, 0.5 - 50%, etc).\nCan be used as a range \"0.0:1.0\" for random result between 2 values."},
                { label45, "(optional) spawning position:\r\n\"@Attachment\" or name of the attachment slot in which the item will be spawned.\r\n\"@Magazine\" to spawn as weapon magazine.\r\n\"@InHands\" to spawn in player hands.\r\n\"@Cargo\" to spawn item in first empty place in cargo.\r\n\"@Cargo:0:3:h\" to spawn in specific place in cargo (col and row position with v(vertical) or h(horizontal) orientation)"},
                { label44 , "(optional) liquid classname from vanilla 'cfgLiquidDefinitions' or from modded 'CfgTerjeCustomLiquids'"},
                { label43 , "(optional) item temperature in degrees Celsius.\nCan be used as a range \"36.6:41.0\" for random result between 2 values."},
                { label53, "(optional) the stage of the food: \nRAW\nBAKED\nBOILED\nDRIED\nBURNEDz\nROTTEN" },
                { label52, "(optional) disinfection (sterility) condition of item: 0 - no, 1 - yes." },
                { label51, "(optional) list of agents separated by comma: \nCHOLERA, INFLUENZA, SALMONELLA, BRAIN, FOOD_POISON, CHEMICAL_POISON, WOUND_AGENT, NERVE_AGENT, HEAVYMETAL" },
                { label50, "(optional) sets the item to quickbar slot (number from 0 to 9)" },
                { label49, "(optional) sets the ammo type to spawn inside a magazine or weapon." },
                { label48, "(optional) sets the amount of ammo to be spawned inside a magazine or weapon.\nCan be used as a range \"10:20\" for random result between 2 values." },
                { label47, "(optional) sets the number of points as a cost for this item,\nIf used inside a selector with a points." }
            };

            ToolTip toolTip = new ToolTip();

            foreach (var pair in toolTips)
            {
                toolTip.SetToolTip(pair.Key, pair.Value);
            }

        }

        private void LoadSkillsAndPerksFromSingleFile()
        {
            var skills = new List<ComboBoxItem>();
            perkMap.Clear();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TraderNPCs", "skills_and_perks.txt");

            foreach (var line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('|');
                if (parts.Length < 2) continue;

                string skillDisplay = parts[0].Trim();
                string skillValue = parts[1].Trim();

                // Add to skills list
                var skillItem = new ComboBoxItem { Display = skillDisplay, Value = skillValue };
                skills.Add(skillItem);

                // Parse perks if available
                if (parts.Length >= 3)
                {
                    var perksList = new List<ComboBoxItem>();
                    var perkItems = parts[2].Split(',');

                    foreach (var perk in perkItems)
                    {
                        var perkParts = perk.Split(':');
                        if (perkParts.Length == 2)
                        {
                            perksList.Add(new ComboBoxItem
                            {
                                Display = perkParts[0].Trim(),
                                Value = perkParts[1].Trim()
                            });
                        }
                    }

                    perkMap[skillValue] = perksList;
                }
            }

            // Assign to ComboBoxes
            CRSLskillIdCB.DataSource = new List<ComboBoxItem>(skills);
            CRSPskillIDCB.DataSource = new List<ComboBoxItem>(skills);
        }
        private void PopulateSkillsCheckBoxes(FlowLayoutPanel groupBox)
        {
            // Clear existing controls
            groupBox.Controls.Clear();

            // Path to your external skills file
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TraderNPCs", "skills_and_perks.txt");

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Skills file not found: " + filePath);
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                // Format expected: Display|Value|Perk1|Perk2|...
                var parts = line.Split('|');
                if (parts.Length < 2) continue;

                string display = parts[0];
                string value = parts[1];

                CheckBox checkBox = new CheckBox
                {
                    Text = display,
                    Tag = value, // store the skill ID for later use
                };
                // Attach the event handler
                checkBox.CheckedChanged += SkillsCheckBox_CheckedChanged;
                groupBox.Controls.Add(checkBox);
            }
        }

        private void LoadCFGtoTreeview()
        {
            TerjeTV.Nodes.Clear();
            TreeNode RootNode = new TreeNode("Treje Manager")
            {
                Tag = "Root"
            };
            CreateCFGNodes(RootNode);
            CreateCraftingNodes(RootNode);
            CreateProtectionsNodes(RootNode);
            CrteatescriptableNodes(RootNode);
            CreateStartScreenNodes(RootNode);

            TerjeTV.Nodes.Add(RootNode);
        }
        private void CreateStartScreenNodes(TreeNode RootNode)
        {
            TreeNode StartScreenNode = new TreeNode("Start Screen")
            {
                Tag = "StartScreen"
            };
            CreateFaceNode(StartScreenNode);
            CreateGeneralNodes(StartScreenNode);
            CreateLoadOutNodes(StartScreenNode);
            CreateSpawnNodes(StartScreenNode);
            RootNode.Nodes.Add(StartScreenNode);
        }
        private void CreateCFGNodes(TreeNode RootNode)
        {
            if (cfgfiles.Count > 0)
            {
                TreeNode CFGNodes = new TreeNode("CFG Files")
                {
                    Tag = "CFGFiles"
                };
                foreach (TerjeCFGFIle cfgfile in cfgfiles)
                {
                    CFGNodes.Nodes.Add(GetCfgnodes(cfgfile));
                }
                RootNode.Nodes.Add(CFGNodes);
            }
        }
        private void CreateCraftingNodes(TreeNode RootNode)
        {
            TreeNode TerjeCraftingNodes = new TreeNode("Crafting Recipes")
            {
                Tag = "TerjeCrafting"
            };
            if (TerjeCraftingFiles != null)
            {
                foreach (Object tc in TerjeCraftingFiles.Items)
                {
                    if (tc is TerjeRecipe)
                        TerjeCraftingNodes.Nodes.Add(GetRecipeNodes(tc as TerjeRecipe));
                    else if (tc is TerjeConditions)
                    {
                        TreeNode ConditionsNode = new TreeNode("Conditions")
                        {
                            Tag = tc as TerjeConditions
                        };
                        TerjeConditions cond = tc as TerjeConditions;
                        if (cond != null)
                        {
                            getConditionNodes(cond, ConditionsNode);
                        }
                        TerjeCraftingNodes.Nodes.Add(ConditionsNode);
                    }

                }
            }
            RootNode.Nodes.Add(TerjeCraftingNodes);
        }
        private void CreateProtectionsNodes(TreeNode RootNode)
        {
            TreeNode Protectionnode = new TreeNode("Protection")
            {
                Tag = "Protectionnode"
            };
            if (protectionIDsList != null && protectionIDsList.Count > 0)
            {
                foreach (TerjeProtectionIDs tc in protectionIDsList)
                {
                    Protectionnode.Nodes.Add(GetProtectionnodes(tc));
                }
            }
            RootNode.Nodes.Add(Protectionnode);
        }
        private void CreateFaceNode(TreeNode rootNode)
        {
            if (TerjeFaces != null)
            {
                TreeNode TerjeFacesNode = new TreeNode("Faces")
                {
                    Tag = TerjeFaces
                };
                foreach(TerjeFace face in TerjeFaces.Face)
                {
                    TerjeFacesNode.Nodes.Add(new TreeNode(face.classname)
                    {
                        Tag = face
                    });
                }
                rootNode.Nodes.Add(TerjeFacesNode);
            }
        }
        private void CreateGeneralNodes(TreeNode rootNode)
        {
            if (TerjeGeneral != null)
            {
                TreeNode TerjeGeneralNode = new TreeNode("General")
                {
                    Tag = TerjeGeneral
                };
                PropertyInfo[] properties = typeof(TerjeGeneral).GetProperties();
                foreach (var property in properties)
                {
                    if (property.Name == "Filename" || property.Name == "isDirty") continue;
                    var value = property.GetValue(TerjeGeneral);
                    if (value != null)
                    {
                        TreeNode propertyNode = new TreeNode(property.Name)
                        {
                            Tag = value
                        };
                        if (value is TerjeValueString)
                        {
                            TerjeValueString tstring = (TerjeValueString)value;
                             propertyNode.Nodes.Add(new TreeNode(tstring.value)
                            {
                                Tag = tstring
                             });
                        }
                        TerjeGeneralNode.Nodes.Add(propertyNode);
                    }
                    else if (value != null)
                    {
                        Console.WriteLine($"Property {property.Name} has a null value.");
                    }
                }
                rootNode.Nodes.Add(TerjeGeneralNode);
            }
        }
        private void CreateLoadOutNodes(TreeNode RootNode)
        {
            if (TerjeLoadouts != null)
            {
                TreeNode TerjeLoadoutsNode = new TreeNode("Loadouts")
                {
                    Tag = TerjeLoadouts
                };
                foreach (TerjeLoadout ta in TerjeLoadouts.Loadout)
                {
                    TerjeLoadoutsNode.Nodes.Add(GetLoadoutNodes(ta));
                }
                RootNode.Nodes.Add(TerjeLoadoutsNode);
            }
        }
        private void CreateSpawnNodes(TreeNode RootNode)
        {
            if (TerjeRespawns != null)
            {
                TreeNode TerjeSpawnsNode = new TreeNode("Player Spawns")
                {
                    Tag = TerjeRespawns
                };
                foreach (TerjeRespawn ta in TerjeRespawns.Respawn)
                {
                    TerjeSpawnsNode.Nodes.Add(GetSpanNodes(ta));
                }
                RootNode.Nodes.Add(TerjeSpawnsNode);
            }
        }
        private void CrteatescriptableNodes(TreeNode RootNode)
        {
            if (TerjeScriptableAreas != null)
            {
                TreeNode ScriptableNode = new TreeNode("Scriptable Areas")
                {
                    Tag = TerjeScriptableAreas
                };
                foreach (TerjeScriptableArea ta in TerjeScriptableAreas.Area)
                {
                    ScriptableNode.Nodes.Add(getScriptablearea(ta));
                }
                RootNode.Nodes.Add(ScriptableNode);
            }
        }
        private TreeNode GetLoadoutNodes(TerjeLoadout ta)
        {
            TreeNode loadoutnode = new TreeNode(ta.displayName)
            {
                Tag = ta
            };
            if(ta.Items != null)
            {
                TreeNode itemsnode = new TreeNode("Items")
                {
                    Tag = ta.Items
                };
                foreach(object item in ta.Items.Items)
                {
                    if(item is TerjeLoadoutItem)
                    {
                        itemsnode.Nodes.Add(GetLoadoutitem(item as TerjeLoadoutItem));
                    }
                    else if (item is TerjeLoadoutSelector)
                    {
                        TerjeLoadoutSelector selector = item as TerjeLoadoutSelector;
                        TreeNode selectorNode = new TreeNode($"Selector Type:{selector.type} , Display Name:{selector.displayName}")
                        {
                            Tag = selector
                        };
                        if(selector.Item.Count > 0)
                        {
                            foreach(TerjeLoadoutItem litem in selector.Item)
                            {
                                TreeNode itemnode = new TreeNode($"Classname:{litem.classname}")
                                {
                                    Tag = litem
                                };
                                selectorNode.Nodes.Add(itemnode);
                            }
                        }
                        if(selector.Group.Count > 0)
                        {
                            foreach (TerjeLoadoutGroup litem in selector.Group)
                            {
                                TreeNode itemnode = new TreeNode($"Group")
                                {
                                    Tag = litem
                                };
                                if (litem.Item.Count > 0)
                                {
                                    foreach (TerjeLoadoutItem Gitem in litem.Item)
                                    {
                                        TreeNode gitemnode = new TreeNode($"Classname:{Gitem.classname}")
                                        {
                                            Tag = Gitem
                                        };
                                        itemnode.Nodes.Add(gitemnode);
                                    }
                                }
                                selectorNode.Nodes.Add(itemnode);
                            }
                        }
                        itemsnode.Nodes.Add(selectorNode);
                    }
                }
                loadoutnode.Nodes.Add(itemsnode);
            }
            if (ta.Conditions != null)
            {
                TreeNode ConditionsNode = new TreeNode("Conditions")
                {
                    Tag = ta.Conditions
                };
                TerjeConditions conditions = ta.Conditions as TerjeConditions;
                if(conditions.Timeout != null)
                {
                    ConditionsNode.Nodes.Add(new TreeNode("Time Out")
                    {
                        Tag = conditions.Timeout
                    });
                }
                if(conditions.SkillLevel != null)
                {
                    ConditionsNode.Nodes.Add(new TreeNode("Skill Level")
                    {
                        Tag = conditions.SkillLevel
                    });
                }
                if (conditions.SkillPerk != null)
                {
                    ConditionsNode.Nodes.Add(new TreeNode("Skill Perk")
                    {
                        Tag = conditions.SkillPerk
                    });
                }
                if( conditions.SpecificPlayers != null)
                {
                    ConditionsNode.Nodes.Add(GetSpecificLoadoutPlayers(conditions.SpecificPlayers));
                }
                if(conditions.CustomCondition != null)
                {
                    
                }
                loadoutnode.Nodes.Add(ConditionsNode);
            }

            return loadoutnode;
        }
        private TreeNode GetLoadoutitem(TerjeLoadoutItem item)
        {
            TerjeLoadoutItem litem = item as TerjeLoadoutItem;
            TreeNode itemnode = new TreeNode($"Classname:{litem.classname}")
            {
                Tag = item
            };
            if(litem.Item.Count > 0)
            {
                foreach(TerjeLoadoutItem iitem in litem.Item)
                {
                    itemnode.Nodes.Add(GetLoadoutitem(iitem));
                }
            }
            return itemnode;
            
        }
        private TreeNode GetSpanNodes(TerjeRespawn ta)
        {
            TreeNode Spawnnode = new TreeNode(ta.displayName)
            {
                Tag = ta
            };
            if(ta.Options != null)
            {
                TreeNode SpawnOptionsNode = new TreeNode("Options")
                {
                    Tag = ta.Options
                };
                PropertyInfo[] properties = typeof(TerjeRespawnOptions).GetProperties();
                foreach (var property in properties)
                {
                    var value = property.GetValue(ta.Options);
                    if (value != null)
                    {
                        TreeNode propertyNode = new TreeNode(property.Name)
                        {
                            Tag = value
                        };

                        SpawnOptionsNode.Nodes.Add(propertyNode);
                    }
                    else if (value != null)
                    {
                        Console.WriteLine($"Property {property.Name} has a null value.");
                    }
                }

                Spawnnode.Nodes.Add(SpawnOptionsNode);
            }
            if(ta.Points.Count > 0)
            {
                TreeNode SpawnPointsNode = new TreeNode("Points")
                {
                    Tag = ta.Points
                };
                foreach(TerjeRespawnPoint point in ta.Points)
                {
                    TreeNode propertyNode = new TreeNode(point.pos)
                    {
                        Tag = point
                    };
                    SpawnPointsNode.Nodes.Add(propertyNode);
                }
                Spawnnode.Nodes.Add(SpawnPointsNode);
            }
            if(ta.Objects.Count > 0)
            {
                TreeNode SpawnObjectNode = new TreeNode("Objects")
                {
                    Tag = ta.Objects
                };
                foreach (TerjeRespawnObject objects in ta.Objects)
                {
                    TreeNode propertyNode = new TreeNode(objects.classname)
                    {
                        Tag = objects
                    };
                    SpawnObjectNode.Nodes.Add(propertyNode);
                }
                Spawnnode.Nodes.Add(SpawnObjectNode);
            }
            if(ta.Conditions != null)
            {
                TreeNode SpawnConditionsNode = new TreeNode("Conditions")
                {
                    Tag = ta.Conditions
                };
                PropertyInfo[] properties = typeof(TerjeConditions).GetProperties();
                foreach (var property in properties)
                {
                    var value = property.GetValue(ta.Conditions);
                    if (value != null)
                    {
                        TreeNode propertyNode = new TreeNode(property.Name)
                        {
                            Tag = value
                        };
                        if(value is TerjeSpecificPlayers)
                        {
                            TerjeSpecificPlayers players = (TerjeSpecificPlayers)value;
                            foreach (TerjeSpecificPlayer player in players.SpecificPlayer)
                            {
                                propertyNode.Nodes.Add(new TreeNode(player.steamGUID)
                                {
                                    Tag = player
                                });
                            }
                        }

                        SpawnConditionsNode.Nodes.Add(propertyNode);
                    }
                    else if (value != null)
                    {
                        // Handle unknown type
                        Console.WriteLine($"Unknown type for {property.Name}");
                    }
                }
                Spawnnode.Nodes.Add(SpawnConditionsNode);
            }

            return Spawnnode;
        }
        private TreeNode getScriptablearea(TerjeScriptableArea ta)
        {
            TreeNode na = new TreeNode(ta.Classname)
            {
                Tag = ta
            };
            na.Nodes.Add(new TreeNode("Data")
            {
                Tag = ta.Data
            });
            return na;
        }
        private TreeNode GetCfgnodes(TerjeCFGFIle cfgfile)
        {
            TreeNode CFGFile = new TreeNode(Path.GetFileName(cfgfile.Filename))
            {
                Tag = cfgfile
            };
            foreach(TerjeCFGLine line in cfgfile.CFGCOntents)
            {
                if (line.isComment)
                {
                    TreeNode node = new TreeNode($"{line.comment}")
                    {
                        Tag = line
                    };
                    CFGFile.Nodes.Add(node);
                }
                else
                {
                    TreeNode node = new TreeNode($"{line.cfgvariablename} = {line.cfgVariable.ToString()}")
                    {
                        Tag = line
                    };
                    CFGFile.Nodes.Add(node);
                }
            }
            return CFGFile;
        }
        private TreeNode GetRecipeNodes(TerjeRecipe recipe)
        {
            TreeNode recipeNode = new TreeNode(Path.GetFileName(recipe.DisplayName))
            {
                Tag = recipe
            };
            recipeNode.Nodes.Add(GetIngredientNodes(recipe.FirstIngredient, 0));
            recipeNode.Nodes.Add(GetIngredientNodes(recipe.SecondIngredient, 1));
            TreeNode resultsnode = new TreeNode("Crafting Results")
            {
                Tag = "CraftingResults"
            };
            foreach(TerjeCraftingResult cr in recipe.CraftingResults.Results)
            {
                resultsnode.Nodes.Add(new TreeNode(cr.ClassName)
                {
                    Tag = cr
                });
            }
            recipeNode.Nodes.Add(resultsnode);
            if(recipe.Conditions != null)
            {
                TreeNode ConditionsNode = new TreeNode("Conditions")
                {
                    Tag = recipe.Conditions
                };
                getConditionNodes(recipe.Conditions, ConditionsNode);
                recipeNode.Nodes.Add(ConditionsNode);
            }
            return recipeNode;
        }
        private static void getConditionNodes(TerjeConditions recipe, TreeNode ConditionsNode)
        {
            if (recipe.SkillLevel != null)
            {
                ConditionsNode.Nodes.Add(new TreeNode("Skill Level")
                {
                    Tag = recipe.SkillLevel
                });
            }
            if (recipe.SkillPerk != null)
            {
                ConditionsNode.Nodes.Add(new TreeNode("Skill Perk")
                {
                    Tag = recipe.SkillPerk
                });
            }
            if (recipe.SpecificPlayers != null)
            {
                ConditionsNode.Nodes.Add(GetSpecificPlayers(recipe.SpecificPlayers));
            }
        }
        private static TreeNode GetSpecificPlayers(TerjeSpecificPlayers players)
        {
            TreeNode Playernode = new TreeNode("Specific Players")
            {
                Tag = players
            };
            foreach (TerjeSpecificPlayer sp in players.SpecificPlayer)
            {
                Playernode.Nodes.Add(new TreeNode(sp.steamGUID)
                {
                    Tag = sp
                });
            }

            return Playernode;
        }
        private static TreeNode GetSpecificLoadoutPlayers(TerjeSpecificPlayers players)
        {
            TreeNode Playernode = new TreeNode("Specific Players")
            {
                Tag = players
            };
            foreach (TerjeSpecificPlayer sp in players.SpecificPlayer)
            {
                Playernode.Nodes.Add(new TreeNode(sp.steamGUID)
                {
                    Tag = sp
                });
            }

            return Playernode;
        }
        private TreeNode GetIngredientNodes(TerjeRecipeIngredient ingredient, int order)
        {
            string name = "";
            if (order == 0)
                name = "First Ingredient";
            else if (order == 1)
                name = "Second Ingredient";

            TreeNode Ingredientnode = new TreeNode(name)
            {
                Tag = ingredient
            };
            TreeNode Itemnode = new TreeNode("Items")
            {
                Tag = "Items"
            };
            foreach(string item in ingredient.Items)
            {
                Itemnode.Nodes.Add(new TreeNode(item)
                {
                    Tag = "IngredientItem"
                });
            }
            Ingredientnode.Nodes.Add(Itemnode);
            return Ingredientnode;
        }
        private TreeNode GetProtectionnodes(TerjeProtectionIDs protection)
        {
            TreeNode filenameNode = new TreeNode(protection.Filetype)
            {
                Tag = protection
            };
            foreach(string id in protection.IDList)
            {
                filenameNode.Nodes.Add(new TreeNode(id)
                {
                    Tag = "IDString"
                });
            }
            return filenameNode;
        }
        private void TerjeManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool savefiles = false;
            foreach (TerjeCFGFIle cfgf in cfgfiles)
            {
                if (cfgf.isDirty)
                {
                    savefiles = true;
                }
            }
            if (TerjeCraftingFiles != null && TerjeCraftingFiles.isDirty)
            {
                savefiles = true;
            }
            foreach (TerjeProtectionIDs prot in protectionIDsList)
            {
                if (prot.isDirty)
                {
                    savefiles = true;
                }
            }
            if (TerjeLoadouts != null && TerjeLoadouts.isDirty)
            {
                savefiles = true;
            }
            if (TerjeRespawns != null && TerjeRespawns.isDirty)
            {
                savefiles = true;
            }
            if (TerjeFaces != null && TerjeFaces.isDirty)
            {
                savefiles = true;
            }
            if (TerjeGeneral != null && TerjeGeneral.isDirty)
            {
                savefiles = true;
            }
            if (TerjeScriptableAreas != null && TerjeScriptableAreas.isDirty)
            {
                savefiles = true;
            }

            if (savefiles == true)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Savefiles();
                }
            }
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            Savefiles();
        }
        private void Savefiles()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            foreach(TerjeCFGFIle cfgf in cfgfiles)
            {
                if(cfgf.isDirty)
                {
                    cfgf.isDirty = false;
                    string[] newfile = cfgf.CreateStringArray();
                    File.WriteAllLines(cfgf.Filename, newfile);
                    midifiedfiles.Add(Path.GetFileName(cfgf.Filename));
                }
            }
            if (TerjeCraftingFiles != null && TerjeCraftingFiles.isDirty)
            {
                TerjeCraftingFiles.isDirty = false;
                var serializer = new XmlSerializer(typeof(TerjeRecipes));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var sw = new StringWriter();
                var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
                serializer.Serialize(xmlWriter, TerjeCraftingFiles, ns);
                File.WriteAllText(TerjeCraftingFiles.Filename, sw.ToString());
                midifiedfiles.Add(Path.GetFileName(TerjeCraftingFiles.Filename));
            }
            if (Directory.Exists(TerjeSettingsPath + "CustomProtection"))
            {
                foreach (TerjeProtectionIDs prot in protectionIDsList)
                {
                    if (prot.isDirty)
                    {
                        prot.isDirty = false;
                        File.WriteAllLines(prot.Filename, prot.IDList.ToArray());
                        midifiedfiles.Add(Path.GetFileName(prot.Filename));
                    }
                }
            }
            if (TerjeLoadouts != null && TerjeLoadouts.isDirty)
            {
                TerjeLoadouts.isDirty = false;
                var serializer = new XmlSerializer(typeof(TerjeLoadouts));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var sw = new StringWriter();
                var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
                serializer.Serialize(xmlWriter, TerjeLoadouts, ns);
                File.WriteAllText(TerjeLoadouts.Filename, sw.ToString());
                midifiedfiles.Add(Path.GetFileName(TerjeLoadouts.Filename));
            }
            if (TerjeRespawns != null && TerjeRespawns.isDirty)
            {
                TerjeRespawns.isDirty = false;
                var serializer = new XmlSerializer(typeof(TerjeRespawns));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var sw = new StringWriter();
                var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
                serializer.Serialize(xmlWriter, TerjeRespawns, ns);
                File.WriteAllText(TerjeRespawns.Filename, sw.ToString());
                midifiedfiles.Add(Path.GetFileName(TerjeRespawns.Filename));
            }
            if (TerjeFaces != null && TerjeFaces.isDirty)
            {
                TerjeFaces.isDirty = false;
                var serializer = new XmlSerializer(typeof(TerjeFaces));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var sw = new StringWriter();
                var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
                serializer.Serialize(xmlWriter, TerjeFaces, ns);
                File.WriteAllText(TerjeFaces.Filename, sw.ToString());
                midifiedfiles.Add(Path.GetFileName(TerjeFaces.Filename));
            }
            if (TerjeGeneral != null && TerjeGeneral.isDirty)
            {
                TerjeGeneral.isDirty = false;
                var serializer = new XmlSerializer(typeof(TerjeGeneral));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var sw = new StringWriter();
                var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
                serializer.Serialize(xmlWriter, TerjeGeneral, ns);
                File.WriteAllText(TerjeGeneral.Filename, sw.ToString());
                midifiedfiles.Add(Path.GetFileName(TerjeGeneral.Filename));
            }
            if (TerjeScriptableAreas != null && TerjeScriptableAreas.isDirty)
            {
                TerjeScriptableAreas.isDirty = false;
                var serializer = new XmlSerializer(typeof(TerjeScriptableAreas));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var sw = new StringWriter();
                var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
                serializer.Serialize(xmlWriter, TerjeScriptableAreas, ns);
                File.WriteAllText(TerjeScriptableAreas.Filename, sw.ToString());
                midifiedfiles.Add(Path.GetFileName(TerjeScriptableAreas.Filename));
            }

            string message = "The Following Files were saved....\n";
            int i = 0;
            foreach (string l in midifiedfiles)
            {
                if (i == 5)
                {
                    message += l + "\n";
                    i = 0;
                }
                else
                {
                    message += l + ", ";
                    i++;
                }

            }
            if (midifiedfiles.Count > 0)
                MessageBox.Show(message, "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
                MessageBox.Show("No changes were made.", "Nothing Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TerjeSettings");
        }
        public TreeNode currentTreeNode { get; set; }
        private void TerjeTV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            currentTreeNode = e.Node;
            groupBox1.Visible = false;
            FloatNUD.Visible = false;
            IntNUD.Visible = false;
            BoolCB.Visible = false;
            StringTB.Visible = false;
            CIngrdientGB.Visible = false;
            CResultCB.Visible = false;
            CRecipeGB.Visible = false;
            CRSLGB.Visible = false;
            CRSPGB.Visible = false;
            SAGB.Visible = false;
            SADGB.Visible = false;
            SAFGB.Visible = false;
            SSLGB.Visible = false;
            SCLItemsGB.Visible = false;
            useraction = false;

            if (e.Node.Tag.ToString() == "Root" || e.Node.Tag.ToString() == "CFGFiles")
            {

            }
            else if (e.Node.Tag is TerjeCFGFIle)
            {
                currentCFGFIle = e.Node.Tag as TerjeCFGFIle;
            }
            else if (e.Node.Tag is TerjeCFGLine)
            {
                currentCFGFIle = e.Node.Parent.Tag as TerjeCFGFIle;
                currentCFGline = e.Node.Tag as TerjeCFGLine;
                if (!currentCFGline.isComment)
                {
                    groupBox1.Visible = true;
                    CommentRTB.Text = currentCFGline.comment;
                    switch (currentCFGline.cfgvariabletype)
                    {
                        case "bool":
                            groupBox1.Text = "Bool";
                            BoolCB.Visible = true;
                            BoolCB.Checked = (bool)currentCFGline.cfgVariable;
                            break;
                        case "float":
                            groupBox1.Text = "Float";
                            FloatNUD.Visible = true;
                            FloatNUD.Value = (decimal)(float)currentCFGline.cfgVariable;
                            break;
                        case "int":
                            groupBox1.Text = "Int";
                            IntNUD.Visible = true;
                            IntNUD.Value = (int)currentCFGline.cfgVariable;
                            break;
                        case "string":
                            groupBox1.Text = "String";
                            StringTB.Visible = true;
                            StringTB.Text = currentCFGline.cfgVariable.ToString();
                            break;
                    }
                }
            }
            else if (e.Node.Tag is TerjeRecipe)
            {
                TerjeRecipe recipe = e.Node.Tag as TerjeRecipe;
                CRecipeGB.Visible = true;
                CRNameTB.Text = recipe.DisplayName;
                CREnabledCB.Checked = recipe.Enabled == 1 ? true : false;
                if (CRtimeSpecifiedCB.Checked = CRAnimationLengthNUD.Visible = recipe.TimeSpecified)
                    CRAnimationLengthNUD.Value = (decimal)recipe.Time;
            }
            else if (e.Node.Tag is TerjeRecipeIngredient)
            {
                TerjeRecipeIngredient ingredient = e.Node.Tag as TerjeRecipeIngredient;
                CIngrdientGB.Visible = true;
                if (CISingleUseSpecifiedCB.Checked = CIDeleteRequiredCB.Visible = ingredient.SingleUseSpecified)
                    CIDeleteRequiredCB.Checked = ingredient.SingleUse == 1 ? true : false;
                if (CIMinQuantitySpecifiedCB.Checked = CIMinQuantityNUD.Visible = ingredient.MinQuantitySpecified)
                    CIMinQuantityNUD.Value = (decimal)ingredient.MinQuantity;
                if (CIMaxQuantitySpecifiedCB.Checked = CIMaxQuantityNUD.Visible = ingredient.MaxQuantitySpecified)
                    CIMaxQuantityNUD.Value = (decimal)ingredient.MaxQuantity;
                if (CIMinDamageSpecifiedCB.Checked = CIMinDamageCB.Visible = ingredient.MinDamageSpecified)
                    CIMinDamageCB.SelectedIndex = (int)ingredient.MinDamage + 1;
                if (CIMaxDamageSpecifiedCB.Checked = CIMaxDamageCB.Visible = ingredient.MaxDamageSpecified)
                    CIMaxDamageCB.SelectedIndex = (int)ingredient.MaxDamage + 1;
                if (CIAddHealthSpecifiedCB.Checked = CIAddHealthNUD.Visible = ingredient.AddHealthSpecified)
                    CIAddHealthNUD.Value = (int)ingredient.AddHealth;
                if (CISetHealthSpecifiedCB.Checked = CISetHealthNUD.Visible = ingredient.SetHealthSpecified)
                    CISetHealthNUD.Value = (int)ingredient.SetHealth;
                if (CIAddQuantitySpecifiedCB.Checked = CIAddQuantityNUD.Visible = ingredient.AddQuantitySpecified)
                    CIAddQuantityNUD.Value = (int)ingredient.AddQuantity;
            }
            else if (e.Node.Tag is TerjeCraftingResult)
            {
                TerjeCraftingResult result = e.Node.Tag as TerjeCraftingResult;
                CResultCB.Visible = true;
                CRClassnameTB.Text = result.ClassName;
                if (CRSetFullQuantitySpecifiedCB.Checked = CRSetFullQuantityCB.Visible = result.SetFullQuantitySpecified)
                    CRSetFullQuantityCB.Checked = result.SetFullQuantity == 1 ? true : false;
                if (CRSetQuantitySpecifiedCB.Checked = CRSetQuantityNUD.Visible = result.SetQuantitySpecified)
                    CRSetQuantityNUD.Value = (decimal)result.SetQuantity;
                if (CRSetHealthSpecifiedCB.Checked = CRSetHealthNUD.Visible = result.SetHealthSpecified)
                    CRSetHealthNUD.Value = (decimal)result.SetHealth;
                if (CRInheritsHealthSpecifiedCB.Checked = CRInheritsHealthCB.Visible = result.InheritsHealthSpecified)
                    CRInheritsHealthCB.SelectedIndex = (int)result.InheritsHealth + 2;
                if (CRInheritsColorSpecifiedCB.Checked = CRInheritsColorCB.Visible = result.InheritsColorSpecified)
                    CRInheritsColorCB.SelectedIndex = (int)result.InheritsColor + 1;
                if (CRSpawnModeSpecifiedCB.Checked = CRToInventoryCB.Visible = result.SpawnModeSpecified)
                    CRToInventoryCB.SelectedIndex = (int)result.SpawnMode + 2;
            }
            else if (e.Node.Tag is TerjeSkillLevel)
            {
                CRSLGB.Visible = true;
                TerjeSkillLevel SL = e.Node.Tag as TerjeSkillLevel;
                CRSLskillIdCB.SelectedItem = SelectComboBoxByValue(CRSLskillIdCB, SL.skillId);
                CRSLrequiredlevelNUD.Value = (int)SL.requiredLevel;
            }
            else if (e.Node.Tag is TerjeSkillPerk)
            {
                CRSPGB.Visible = true;
                TerjeSkillPerk SP = e.Node.Tag as TerjeSkillPerk;
                CRSPskillIDCB.SelectedItem = SelectComboBoxByValue(CRSPskillIDCB, SP.skillId);
                CRSPperkIDCB.SelectedItem = SelectComboBoxByValue(CRSPperkIDCB, SP.perkId);
                CRSPrequiredlevelNUD.Value = (int)SP.requiredLevel;
            }
            else if (e.Node.Tag is TerjeScriptableArea)
            {
                SAGB.Visible = true;
                TerjeScriptableArea SA = e.Node.Tag as TerjeScriptableArea;
                SAActiveCB.Checked = SA.Active == 1 ? true : false;
                SAClassnameTB.Text = SA.Classname;
                SAPosXNUD.Value = (decimal)SA.PositionVec3.X;
                SAPosYNUD.Value = (decimal)SA.PositionVec3.Y;
                SAPosZNUD.Value = (decimal)SA.PositionVec3.Z;
                SASpawnChanceNUD.Value = (decimal)SA.SpawnChance;
                if(SA.FilterSpecified)
                {
                    SAFGB.Visible = true;
                    // Split the input string into a list of skill IDs
                    var selectedIds = SA.Filter.Split(',').Select(s => s.Trim()).ToHashSet();

                    // Loop through the CheckBoxes in the GroupBox
                    foreach (var checkBox in SAFLP.Controls.OfType<CheckBox>())
                    {
                        // Tag holds the skill ID
                        string skillId = checkBox.Tag?.ToString();

                        if (!string.IsNullOrEmpty(skillId))
                        {
                            checkBox.Checked = selectedIds.Contains(skillId);
                        }
                    }
                }
                
            }
            else if (e.Node.Tag is TerjeScriptableAreaData)
            {
                SADGB.Visible = true;
                TerjeScriptableArea SA = e.Node.Parent.Tag as TerjeScriptableArea;
                TerjeScriptableAreaData sad = e.Node.Tag as TerjeScriptableAreaData;
                if (SA.Classname == "TerjePsionicScriptableArea" || SA.Classname == "TerjeRadioactiveScriptableArea")
                {
                    SADOuterRadiusNUD.Visible = true;
                    label34.Visible = true;
                    SADOuterRadiusNUD.Value = (decimal)sad.OuterRadius;
                    label35.Text = "Inner Radius";
                    SADInnerRadiusNUD.Value = (decimal)sad.InnerRadius;
                    SADHeightMInNUD.Value = (decimal)sad.HeightMin;
                    SADHeightMaxNUD.Value = (decimal)sad.HeightMax;
                    SADPowerNUD.Value = (decimal)sad.Power;

                }
                else if (SA.Classname == "TerjeExperienceModScriptableArea")
                {
                    SADOuterRadiusNUD.Visible = false;
                    label34.Visible = false;
                    label35.Text = "Radius";
                    SADInnerRadiusNUD.Value = (decimal)sad.Radius;
                    SADHeightMInNUD.Value = (decimal)sad.HeightMin;
                    SADHeightMaxNUD.Value = (decimal)sad.HeightMax;
                    SADPowerNUD.Value = (decimal)sad.Power;
                }
            }
            else if (e.Node.Tag is TerjeLoadout)
            {
                TerjeLoadout loadout = e.Node.Tag as TerjeLoadout;
                SSLGB.Visible = true;
                SSLdisplayNameTB.Text = loadout.displayName;
                SSLidTB.Text = loadout.id;
            }
            else if (e.Node.Tag is TerjeLoadoutItem) 
            {
                TerjeLoadoutItem item = e.Node.Tag as TerjeLoadoutItem;
                SCLItemsGB.Visible = true;
                SCLItemsClassnameTB.Text = item.classname;
                // displayName
                if ((SCLItemsdisplayNameSpecifiedCB.Checked = SCLItemsdisplayNameTB.Visible = item.displayNameSpecified))
                    SCLItemsdisplayNameTB.Text = item.displayName;

                // quantity
                if ((SCLItemsquantitySpecifiedCB.Checked = SCLItemsquantityTB.Visible = item.quantitySpecified))
                    SCLItemsquantityTB.Text = item.quantity;

                // count
                if ((SCLItemscountSpecifiedCB.Checked = SCLItemscountTB.Visible = item.countSpecified))
                    SCLItemscountTB.Text = item.count;

                // health
                if ((SCLItemshealthSpecifiedCB.Checked = SCLItemshealthTB.Visible = item.healthSpecified))
                    SCLItemshealthTB.Text = item.health;

                // position
                if ((SCLItemspositionSpecifiedCB.Checked = SCLItemspositionTB.Visible = item.positionSpecified))
                    SCLItemspositionTB.Text = item.position;

                // liquid
                if ((SCLItemsliquidSpecifiedCB.Checked = SCLItemsliquidTB.Visible = item.liquidSpecified))
                    SCLItemsliquidTB.Text = item.liquid;

                // temperature
                if ((SCLItemstemperatureSpecifiedCB.Checked = SCLItemstemperatureTB.Visible = item.temperatureSpecified))
                    SCLItemstemperatureTB.Text = item.temperature;

                // foodStage
                if ((SCLItemsfoodStageSpecifiedCB.Checked = SCLItemsfoodStageCB.Visible = item.foodStageSpecified))
                    SCLItemsfoodStageCB.SelectedItem = item.foodStage;

                // disinfected
                if (SCLItemsdisinfectedSpecifiedCB.Checked = SCLItemsdisinfectedCB.Visible = item.disinfectedSpecified)
                    SCLItemsdisinfectedCB.Checked = item.disinfected == 1 ? true : false;

                // agents
                if ((SCLItemsagentsSpecifiedCB.Checked = SCLItemsagentsTB.Visible = item.agentsSpecified))
                    SCLItemsagentsTB.Text = item.agents;

                // quickbar
                if ((SCLItemsquickbarSpecifiedCB.Checked = SCLItemsquickbarNUD.Visible = item.quickbarSpecified))
                    SCLItemsquickbarNUD.Value = item.quickbar;

                // ammoType
                if ((SCLItemsammoTypeSpecifiedCB.Checked = SCLItemsammoTypeTB.Visible = item.ammoTypeSpecified))
                    SCLItemsammoTypeTB.Text = item.ammoType;

                // ammoCount
                if ((SCLItemsammoCountSpecifiedCB.Checked = SCLItemsammoCountTB.Visible = item.ammoCountSpecified))
                    SCLItemsammoCountTB.Text = item.ammoCount;

                // cost
                if ((SCLItemscostSpecifiedCB.Checked = SCLItemscostNUD.Visible = item.costSpecified))
                    SCLItemscostNUD.Value = item.cost;

               
            }
            useraction = true;
        }
        public ComboBoxItem SelectComboBoxByValue(ComboBox comboBox, string value)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Value == value)
                {
                    return item;
                }
            }
            return null;
        }
        private void TerjeTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            TerjeTV.SelectedNode = e.Node;
            currentTreeNode = e.Node;
            foreach (ToolStripItem item in contextMenuStrip1.Items)
            {
                item.Visible = false;
            }
            if (e.Button == MouseButtons.Right)
            {
                if(e.Node.Tag.ToString() == "TerjeCrafting")
                {
                    addNewCraftingRecipeToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeRecipe)
                {
                    removeSelectedRecipeToolStripMenuItem.Visible = true;
                    TerjeRecipe recipe = e.Node.Tag as TerjeRecipe;
                    if (recipe.Conditions == null)
                        addConditionsToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeLoadout)
                {
                    addConditionsToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag.ToString() == "CraftingResults")
                {
                    addCraftingResultToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeCraftingResult)
                {
                    removeCraftingResultToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag.ToString() == "Items")
                {
                    addIngredientItemToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag.ToString() == "IngredientItem")
                {
                    removeIngredientItemToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeProtectionIDs)
                {
                    addNewSteamIDToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag.ToString() == "IDString" || e.Node.Tag is TerjeSpecificPlayer)
                {
                    removeSteamIDToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeConditions)
                {
                    TerjeConditions conditions = e.Node.Tag as TerjeConditions;
                    if (conditions.SkillLevel == null)
                    {
                        addSkillLevelConditionToolStripMenuItem.Visible = true;
                    }
                    if (conditions.SkillPerk == null)
                    {
                        addSkillPerkConditionToolStripMenuItem.Visible = true;
                    }
                    if (conditions.SpecificPlayers == null)
                    {
                        addSpecificPlayerConditionToolStripMenuItem.Visible = true;
                    }
                    if (conditions.Timeout == null && (e.Node.Parent.Tag is TerjeRespawn || e.Node.Parent.Tag is TerjeLoadout))
                    {
                        addTimeoutConditionToolStripMenuItem.Visible = true;
                    }
                    if (conditions.CustomCondition == null && (e.Node.Parent.Tag is TerjeRespawn || e.Node.Parent.Tag is TerjeLoadout))
                    {
                        addCustomConditionToolStripMenuItem.Visible = true;
                    }
                    removeConditionsToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeSpecificPlayers)
                {
                    addNewSteamIDToolStripMenuItem.Visible = true;
                    removeConditionToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeSkillLevel || e.Node.Tag is TerjeSkillPerk || e.Node.Tag is TerjeTimeout || e.Node.Tag is TerjeCustomCondition)
                {
                    removeConditionToolStripMenuItem.Visible = true;
                }
                else if(e.Node.Tag is TerjeScriptableAreas)
                {
                    addNewScriptableAreaToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeScriptableArea)
                {
                    removeScriptableAreaToolStripMenuItem.Visible = true;
                }
                contextMenuStrip1.Show(Cursor.Position);
            }
            useraction = true;
        }
        private void BoolCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentCFGline.cfgVariable = BoolCB.Checked;
            currentTreeNode.Text = $"{currentCFGline.cfgvariablename} = {currentCFGline.cfgVariable.ToString()}";
            currentCFGFIle.isDirty = true;
        }
        private void FloatNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentCFGline.cfgVariable = (float)FloatNUD.Value;
            currentTreeNode.Text = $"{currentCFGline.cfgvariablename} = {currentCFGline.cfgVariable.ToString()}";
            currentCFGFIle.isDirty = true;
        }
        private void IntNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentCFGline.cfgVariable = (int)IntNUD.Value;
            currentTreeNode.Text = $"{currentCFGline.cfgvariablename} = {currentCFGline.cfgVariable.ToString()}";
            currentCFGFIle.isDirty = true;
        }
        private void StringTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentCFGline.cfgVariable = StringTB.Text;
            currentTreeNode.Text = $"{currentCFGline.cfgvariablename} = {currentCFGline.cfgVariable.ToString()}";
            currentCFGFIle.isDirty = true;
        }
        private void addNewCraftingRecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeRecipe newrecipe = new TerjeRecipe()
            {
                DisplayName = "New Recipe",
                Enabled = 0,
                Time = -1,
                FirstIngredient = new TerjeRecipeIngredient(),
                SecondIngredient = new TerjeRecipeIngredient(),
                CraftingResults = new TerjeCraftingResults()
                {
                    Results = new List<TerjeCraftingResult>()
                }
            };
            TerjeCraftingFiles.Items.Add(newrecipe);
            currentTreeNode.Nodes.Add(GetRecipeNodes(newrecipe));
            TerjeCraftingFiles.isDirty = true;
            TerjeTV.SelectedNode = currentTreeNode.LastNode;
        }
        private void removeSelectedRecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeRecipe recipe = currentTreeNode.Tag as TerjeRecipe;
            TerjeCraftingFiles.Items.Remove(recipe);
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            TerjeCraftingFiles.isDirty = true;
        }
        private void addCraftingResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
                foreach (string l in addedtypes)
                {
                    TerjeCraftingResult newitem = new TerjeCraftingResult()
                    {
                        ClassName = l,
                        SetFullQuantity = 1,
                        SetQuantity = (float)-1.0,
                        SetHealth = (float)-1.0,
                        InheritsHealth = -1,
                        InheritsColor = -1,
                        SpawnMode = -2
                    };
                    recipe.CraftingResults.Results.Add(newitem);
                    currentTreeNode.Nodes.Add(new TreeNode(newitem.ClassName)
                    {
                        Tag = newitem
                    });
                }
                currentTreeNode.Expand();
                TerjeCraftingFiles.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void removeCraftingResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeRecipe recipe = currentTreeNode.Parent.Parent.Tag as TerjeRecipe;
            recipe.CraftingResults.Results.Remove(currentTreeNode.Tag as TerjeCraftingResult);
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Tag as TerjeRecipe;
            recipe.DisplayName = CRNameTB.Text;
            currentTreeNode.Text = $"{recipe.DisplayName}";
            TerjeCraftingFiles.isDirty = true;
        }
        private void CREnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Tag as TerjeRecipe;
            recipe.Enabled = CREnabledCB.Checked == true ? 1 : 0;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRIsInstaRecipeCB_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void CRAnimationLengthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Tag as TerjeRecipe;
            recipe.Time = (float)CRAnimationLengthNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRtimeSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Tag as TerjeRecipe;
            recipe.TimeSpecified = CRAnimationLengthNUD.Visible = CRtimeSpecifiedCB.Checked;
            CRAnimationLengthNUD.Value = (decimal)recipe.Time;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIDeleteRequiredCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.SingleUse = CIDeleteRequiredCB.Checked == true ? 1 : 0;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMinQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MinQuantity = (float)CIMinQuantityNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMaxQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MaxQuantity = (float)CIMaxQuantityNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMinDamageCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MinDamage = CIMinDamageCB.SelectedIndex - 1;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMaxDamageCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MaxDamage = CIMaxDamageCB.SelectedIndex - 1;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIAddHealthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.AddHealth = (float)CIAddHealthNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CISetHealthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.SetHealth = (float)CISetHealthNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIAddQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.AddQuantity = (float)CIAddQuantityNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CISingleUseSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.SingleUseSpecified = CIDeleteRequiredCB.Visible = CISingleUseSpecifiedCB.Checked;
            CIDeleteRequiredCB.Checked = ingredient.SingleUse == 1 ? true:false;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMinQuantitySpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MinQuantitySpecified = CIMinQuantityNUD.Visible = CIMinQuantitySpecifiedCB.Checked;
            CIMinQuantityNUD.Value = (decimal)ingredient.MinQuantity;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMaxQuantitySpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if(!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MaxQuantitySpecified = CIMaxQuantityNUD.Visible = CIMaxQuantitySpecifiedCB.Checked;
            CIMaxQuantityNUD.Value = (decimal)ingredient.MaxQuantity;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMinDamageSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MinDamageSpecified = CIMinDamageCB.Visible = CIMinDamageSpecifiedCB.Checked;
            CIMinDamageCB.SelectedIndex = ingredient.MinDamage;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMaxDamageSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MaxDamageSpecified = CIMaxDamageCB.Visible = CIMaxDamageSpecifiedCB.Checked;
            CIMaxDamageCB.SelectedIndex = ingredient.MaxDamage;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIAddHealthSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.AddHealthSpecified = CIAddHealthNUD.Visible = CIAddHealthSpecifiedCB.Checked;
            CIAddHealthNUD.Value = (int)ingredient.AddHealth;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CISetHealthSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.SetHealthSpecified = CISetHealthNUD.Visible = CISetHealthSpecifiedCB.Checked;
            CISetHealthNUD.Value = (int)ingredient.SetHealth;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIAddQuantitySpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.AddQuantitySpecified = CIAddQuantityNUD.Visible = CIAddQuantitySpecifiedCB.Checked;
            CIAddQuantityNUD.Value = (int)ingredient.AddQuantity;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSetFullQuantityCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Parent.Tag as TerjeRecipe;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SetFullQuantity = CRSetFullQuantityCB.Checked==true?1:0;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSetQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Parent.Tag as TerjeRecipe;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SetQuantity = (float)CRSetQuantityNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSetHealthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Parent.Tag as TerjeRecipe;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SetHealth = (float)CRSetHealthNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRInheritsHealthCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Parent.Tag as TerjeRecipe;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.InheritsHealth = CRInheritsHealthCB.SelectedIndex - 2;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRInheritsColorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Parent.Tag as TerjeRecipe;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.InheritsColor = CRInheritsColorCB.SelectedIndex - 1;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRToInventoryCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SpawnMode = (int)CRToInventoryCB.SelectedIndex - 2;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRClassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.ClassName = CRClassnameTB.Text;
            currentTreeNode.Text = $"{result.ClassName}";
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSetFullQuantitySpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SetFullQuantitySpecified = CRSetFullQuantityCB.Visible = CRSetFullQuantitySpecifiedCB.Checked;
            CRSetFullQuantityCB.Checked = result.SetFullQuantity == 1 ? true : false;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSetQuantitySpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SetQuantitySpecified = CRSetQuantityNUD.Visible = CRSetQuantitySpecifiedCB.Checked;
            CRSetQuantityNUD.Value = (decimal)result.SetQuantity;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSetHealthSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SetHealthSpecified = CRSetHealthNUD.Visible = CRSetHealthSpecifiedCB.Checked;
            CRSetHealthNUD.Value = (decimal)result.SetHealth;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRInheritsHealthSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.InheritsHealthSpecified = CRInheritsHealthCB.Visible = CRInheritsHealthSpecifiedCB.Checked;
            CRInheritsHealthCB.SelectedIndex = result.InheritsHealth + 2;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRInheritsColorSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.InheritsColorSpecified = CRInheritsColorCB.Visible = CRInheritsColorSpecifiedCB.Checked;
            CRInheritsColorCB.SelectedIndex = result.InheritsColor + 1;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSpawnModeSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SpawnModeSpecified = CRToInventoryCB.Visible = CRSpawnModeSpecifiedCB.Checked;
            CRToInventoryCB.SelectedIndex = result.SpawnMode + 2;
            TerjeCraftingFiles.isDirty = true;
        }
        private void addIngredientItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                TerjeRecipeIngredient ingredient = currentTreeNode.Parent.Tag as TerjeRecipeIngredient;
                foreach (string l in addedtypes)
                {
                    ingredient.Items.Add(l);
                    currentTreeNode.Nodes.Add(new TreeNode(l)
                    {
                        Tag = "IngredientItem"
                    });
                }
                TerjeCraftingFiles.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }

        }
        private void removeIngredientItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeRecipeIngredient ingredient = currentTreeNode.Parent.Parent.Tag as TerjeRecipeIngredient;
            ingredient.Items.Remove(currentTreeNode.Text);
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            TerjeCraftingFiles.isDirty = true;
        }
        private void addNewSteamIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewfileName form = new AddNewfileName()
            {
                setdescription = "Please enter the steam id you want Protection for",
                SetTitle = "Add Steam ID",
                Setbutton = "Add"
            };
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (currentTreeNode.Tag is TerjeProtectionIDs)
                {
                    TerjeProtectionIDs prot = currentTreeNode.Tag as TerjeProtectionIDs;
                    prot.IDList.Add(form.NewFileName);
                    currentTreeNode.Nodes.Add(new TreeNode(form.NewFileName)
                    {
                        Tag = "IDString"
                    });
                    currentTreeNode.ExpandAll();
                    prot.isDirty = true;
                }
                else if (currentTreeNode.Tag is TerjeSpecificPlayers)
                {
                    TerjeSpecificPlayers SP = currentTreeNode.Tag as TerjeSpecificPlayers;
                    TerjeSpecificPlayer terjeCraftingSpecificPlayer = new TerjeSpecificPlayer()
                    {
                       steamGUID = form.NewFileName
                    };
                    SP.SpecificPlayer.Add(terjeCraftingSpecificPlayer);
                   TreeNode newTN = new TreeNode(form.NewFileName)
                    {
                        Tag = terjeCraftingSpecificPlayer
                    };
                    currentTreeNode.Nodes.Add(newTN);
                    if (currentTreeNode.Parent.Parent.Tag is TerjeRecipe)
                        TerjeCraftingFiles.isDirty = true;
                    else if (currentTreeNode.Parent.Parent.Tag is TerjeLoadout)
                        TerjeLoadouts.isDirty = true;
                    else if (currentTreeNode.Parent.Parent.Tag is TerjeRespawn)
                        TerjeLoadouts.isDirty = true;
                    currentTreeNode.Expand();
                    TerjeTV.SelectedNode = newTN;
                    

                }
            }
        }
        private void removeSteamIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentTreeNode.Parent.Tag is TerjeProtectionIDs)
            {
                TerjeProtectionIDs prot = currentTreeNode.Parent.Tag as TerjeProtectionIDs;
                prot.IDList.Remove(currentTreeNode.Text);
                currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
                prot.isDirty = true;
            }
            else if (currentTreeNode.Tag is TerjeSpecificPlayer)
            {
                TerjeSpecificPlayers SP = currentTreeNode.Parent.Tag as TerjeSpecificPlayers;
                SP.SpecificPlayer.Remove(currentTreeNode.Tag as TerjeSpecificPlayer);
                
                if (currentTreeNode.Parent.Parent.Parent.Tag is TerjeRecipe)
                    TerjeCraftingFiles.isDirty = true;
                else if (currentTreeNode.Parent.Parent.Parent.Tag is TerjeLoadout)
                    TerjeLoadouts.isDirty = true;
                else if (currentTreeNode.Parent.Parent.Parent.Tag is TerjeRespawn)
                    TerjeRespawns.isDirty = true;

                currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            }
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    CRClassnameTB.Text = l;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void CRSLSkillIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeSkillLevel SL = currentTreeNode.Tag as TerjeSkillLevel;
            var selectedSkill = CRSLskillIdCB.SelectedItem as ComboBoxItem;
            SL.skillId = selectedSkill.Value;
            if (currentTreeNode.Parent.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeLoadout)
                TerjeLoadouts.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;
        }
        private void CRSLRequiredlevelNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeSkillLevel SL = currentTreeNode.Tag as TerjeSkillLevel;
            SL.requiredLevel = (int)CRSLrequiredlevelNUD.Value;
            if (currentTreeNode.Parent.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeLoadout)
                TerjeLoadouts.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;
        }
        private void addTimeoutConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeTimeout timeout = new TerjeTimeout()
            {
                id = "Change me to somthing short",
                minutes = 15
            };
            TerjeConditions conditions = currentTreeNode.Tag as TerjeConditions;
            conditions.Timeout = timeout;
            if (currentTreeNode.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Tag is TerjeLoadout)
                TerjeLoadouts.isDirty = true;
            else if (currentTreeNode.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;

            currentTreeNode.Nodes.Add(new TreeNode("Time Out")
            {
                Tag = conditions.Timeout
            });
            currentTreeNode.Expand();
            TerjeTV.SelectedNode = currentTreeNode.LastNode;
        }
        private void addSkillLevelConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeSkillLevel SL = new TerjeSkillLevel()
            {
                skillId = "immunity",
                requiredLevel = 100
            };
            TerjeConditions conditions = currentTreeNode.Tag as TerjeConditions;
            conditions.SkillLevel = SL;
            if (currentTreeNode.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Tag is TerjeLoadout)
                TerjeLoadouts.isDirty = true;
            else if (currentTreeNode.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;

            currentTreeNode.Nodes.Add(new TreeNode("Skill Level")
            {
                Tag = conditions.SkillLevel
            });
            currentTreeNode.Expand();
            TerjeTV.SelectedNode = currentTreeNode.LastNode;
        }
        private void addSkillPerkConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeSkillPerk SL = new TerjeSkillPerk()
            {
                skillId = "immunity",
                perkId = "coldres",
                requiredLevel = 100
            };
            TerjeConditions conditions = currentTreeNode.Tag as TerjeConditions;
            if (conditions.SkillPerk != null)
            {
                MessageBox.Show("Conditions allready contains a skill Perk");
                return;
            }
            conditions.SkillPerk = SL;
            if (currentTreeNode.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Tag is TerjeLoadout)
                TerjeLoadouts.isDirty = true;
            else if (currentTreeNode.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;
            currentTreeNode.Nodes.Add(new TreeNode("Skill Perk")
            {
                Tag = conditions.SkillPerk
            });
            currentTreeNode.Expand();
            TerjeTV.SelectedNode = currentTreeNode.LastNode;
        }
        private void addSpecificPlayerConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeSpecificPlayers SP = new TerjeSpecificPlayers()
            {
                SpecificPlayer = new BindingList<TerjeSpecificPlayer>()
            };
            TerjeConditions conditions = currentTreeNode.Tag as TerjeConditions;
            if (conditions.SpecificPlayers != null)
            {
                MessageBox.Show("Conditions allready contains Specific Players");
                return;
            }
            conditions.SpecificPlayers = SP;
            if (currentTreeNode.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Tag is TerjeLoadout)
            {
                SP.hideOwnerWhenFalseSpecified = true;
                SP.hideOwnerWhenFalse = 1;
                TerjeLoadouts.isDirty = true;
            }
            else if (currentTreeNode.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;

            currentTreeNode.Nodes.Add(GetSpecificPlayers(SP));
            currentTreeNode.Expand();
            TerjeTV.SelectedNode = currentTreeNode.LastNode;
        }
        private void addCustomConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeCustomCondition custcon = new TerjeCustomCondition()
            {
                classname = "Change me to your custom class"
            };
            TerjeConditions conditions = currentTreeNode.Tag as TerjeConditions;
            conditions.CustomCondition = custcon;

            if (currentTreeNode.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Tag is TerjeLoadout)
                TerjeLoadouts.isDirty = true;
            else if (currentTreeNode.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;

            currentTreeNode.Nodes.Add(new TreeNode("Custom Condition")
            {
                Tag = conditions.CustomCondition
            });
            currentTreeNode.Expand();
            TerjeTV.SelectedNode = currentTreeNode.LastNode;
        }
        private void removeConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeConditions conditions = currentTreeNode.Parent.Tag as TerjeConditions;
            if (currentTreeNode.Parent.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeLoadout)
                TerjeLoadouts.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;
            if (currentTreeNode.Tag is TerjeSkillLevel)
                conditions.SkillLevel = null;
            else if (currentTreeNode.Tag is TerjeSkillPerk)
                conditions.SkillPerk = null;
            else if (currentTreeNode.Tag is TerjeSpecificPlayers)
                conditions.SpecificPlayers = null;
            else if (currentTreeNode.Tag is TerjeTimeout)
                conditions.Timeout = null;
            else if (currentTreeNode.Tag is TerjeCustomCondition)
                conditions.CustomCondition = null;
            
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
        }
        private void CRSPSkillIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedSkill = CRSPskillIDCB.SelectedItem as ComboBoxItem;
            if (selectedSkill != null && perkMap.TryGetValue(selectedSkill.Value, out var perks))
            {
                CRSPperkIDCB.DataSource = perks;
            }
            else
            {
                CRSPperkIDCB.DataSource = null;
            }
            if (!useraction) return;
            TerjeSkillPerk SP = currentTreeNode.Tag as TerjeSkillPerk;
            SP.skillId = selectedSkill.Value;
            if (currentTreeNode.Parent.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeLoadout)
                TerjeLoadouts.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;
        }
        private void CRSPPerkIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeSkillPerk SP = currentTreeNode.Tag as TerjeSkillPerk;
            var selectedPerk = CRSPperkIDCB.SelectedItem as ComboBoxItem;
            SP.perkId = selectedPerk.Value;
            if (currentTreeNode.Parent.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeLoadout)
                TerjeLoadouts.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;
        }
        private void CRSPRequiredlevelNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeSkillPerk SP = currentTreeNode.Tag as TerjeSkillPerk;
            SP.requiredLevel = (int)CRSPrequiredlevelNUD.Value;
            if (currentTreeNode.Parent.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeLoadout)
                TerjeLoadouts.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;
        }
        private void addConditionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeConditions tc = new TerjeConditions()
            {

            };
            if(currentTreeNode.Tag is TerjeLoadout)
            {
                TerjeLoadout loadout = currentTreeNode.Tag as TerjeLoadout;
                loadout.Conditions = tc;
                TerjeLoadouts.isDirty = true;
            }
            else if (currentTreeNode.Tag is TerjeRecipe)
            {
                TerjeRecipe tr = currentTreeNode.Tag as TerjeRecipe;
                tr.Conditions = tc;
                TerjeCraftingFiles.isDirty = true;
            }
            TreeNode ConditionsNode = new TreeNode("Conditions")
            {
                Tag = tc
            };
            getConditionNodes(tc, ConditionsNode);
            currentTreeNode.Nodes.Add(ConditionsNode);
            currentTreeNode.Expand();
            TerjeTV.SelectedNode = currentTreeNode.LastNode;
        }
        private void removeConditionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeConditions conditions = currentTreeNode.Tag as TerjeConditions;
            if (currentTreeNode.Parent.Tag is TerjeRecipe)
            {
                TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
                recipe.Conditions = null;
                TerjeCraftingFiles.isDirty = true;
            }
            else if (currentTreeNode.Parent.Tag is TerjeLoadout)
            {
                TerjeLoadout loadout = currentTreeNode.Parent.Tag as TerjeLoadout;
                loadout.Conditions = null;
                TerjeLoadouts.isDirty = true;
            }
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
        }
        private void addNewScriptableAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddnewTerjeScriptableArea form = new AddnewTerjeScriptableArea();
            if (form.ShowDialog() == DialogResult.OK)
            {
                TerjeScriptableArea newarea = new TerjeScriptableArea()
                {
                    Active = 0,
                    Classname = form.SelectedAreaType,
                    Position = "0 0 0",
                    SpawnChance = 1,
                };
                if (newarea.Classname == "TerjePsionicScriptableArea" || newarea.Classname == "TerjeRadioactiveScriptableArea")
                {
                    newarea.Data = new TerjeScriptableAreaData()
                    {
                        OuterRadiusSpecified = true,
                        OuterRadius = 100,
                        InnerRadiusSpecified = true,
                        InnerRadius = 50,
                        HeightMin = -100,
                        HeightMax = 100,
                        Power = 5
                    };
                }
                else if (newarea.Classname == "TerjeExperienceModScriptableArea")
                {
                    newarea.FilterSpecified = true;
                    newarea.Filter = "";
                    newarea.Data = new TerjeScriptableAreaData()
                    {
                        RadiusSpecified = true,
                        Radius = 100,
                        HeightMin = -100,
                        HeightMax = 100,
                        Power = 5
                    };
                }
                TerjeScriptableAreas.Area.Add(newarea);
                currentTreeNode.Nodes.Add(getScriptablearea(newarea));
                TerjeScriptableAreas.isDirty = true;
            }
        }
        private void removeScriptableAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            TerjeScriptableAreas.Area.Remove(area);
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            TerjeScriptableAreas.isDirty = true;
        }
        private void SAActiveCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            area.Active = SAActiveCB.Checked == true ? 1 : 0;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SAClassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
        }
        private void SAPosXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            area.PositionVec3.X = (float)SAPosXNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SAPosYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            area.PositionVec3.Y = (float)SAPosYNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SAPosZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            area.PositionVec3.Z = (float)SAPosZNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SASpawnChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            area.SpawnChance = (int)SASpawnChanceNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SkillsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            string currentSelectedSkills = GetCheckedSkillsCsv(SAFLP);
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            area.Filter = currentSelectedSkills;
            TerjeScriptableAreas.isDirty = true;
        }
        private string GetCheckedSkillsCsv(FlowLayoutPanel groupBox)
        {
            return string.Join(",",
                groupBox.Controls.OfType<CheckBox>()
                    .Where(cb => cb.Checked)
                    .Select(cb => cb.Tag.ToString()));
        }
        private void SADOuterRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableAreaData data = currentTreeNode.Tag as TerjeScriptableAreaData;
            data.OuterRadius = (int)SADOuterRadiusNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SADInnerRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableAreaData data = currentTreeNode.Tag as TerjeScriptableAreaData;
            if (data.InnerRadiusSpecified)
                data.InnerRadius = (int)SADInnerRadiusNUD.Value;
            else if (data.RadiusSpecified)
                data.Radius = (int)SADInnerRadiusNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SADHeightMInNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableAreaData data = currentTreeNode.Tag as TerjeScriptableAreaData;
            data.HeightMin = (int)SADHeightMInNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SADHeightMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableAreaData data = currentTreeNode.Tag as TerjeScriptableAreaData;
            data.HeightMax = (int)SADHeightMaxNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SADPowerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableAreaData data = currentTreeNode.Tag as TerjeScriptableAreaData;
            data.Power = (int)SADPowerNUD.Value;            
            TerjeScriptableAreas.isDirty = true;
        }
        private void SSLdisplayNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadout loadout = currentTreeNode.Tag as TerjeLoadout;
            loadout.displayName = SSLdisplayNameTB.Text;
            currentTreeNode.Text = loadout.displayName;
            TerjeLoadouts.isDirty = true;
        }
        private void SSLidTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadout loadout = currentTreeNode.Tag as TerjeLoadout;
            loadout.id = SSLidTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    SCLItemsClassnameTB.Text = l;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void SCLItemsClassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.classname = SCLItemsClassnameTB.Text;
            currentTreeNode.Text = $"Classname:{litem.classname}";
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsDisplayNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.displayName = SCLItemsdisplayNameTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsQuantityTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.quantity = SCLItemsquantityTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsCountTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.count = SCLItemscountTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsHealthTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.health = SCLItemshealthTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsPositionTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.position = SCLItemspositionTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsLiquidTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.liquid = SCLItemsliquidTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsTemperatureTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.temperature = SCLItemstemperatureTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsFoodStageCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.foodStage = SCLItemsfoodStageCB.GetItemText(SCLItemsfoodStageCB.SelectedItem);
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsDisinfectedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.disinfected = SCLItemsdisinfectedCB.Checked == true ? 1 : 0;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsAgentsTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.liquid = SCLItemsliquidTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsQuickbarNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.quickbar = (int)SCLItemsquickbarNUD.Value;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsAmmoTypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.ammoType = SCLItemsammoTypeTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsammoCountTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.ammoCount = SCLItemsammoCountTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsCostNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.cost = (int)SCLItemscostNUD.Value;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CheckBox check = sender as CheckBox;
            string namespecified = check.Name;
            string name = check.Tag.ToString();
            foreach (Control control in SCLItemsGB.Controls)
            {
                if (control.Name == name)
                {
                    TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
                    SetPropertyValue(litem, namespecified.Substring(8, namespecified.Length - 8 - 2), control.Visible = check.Checked);
                    if (control is TextBox)
                    {
                        TextBox tb = control as TextBox;
                        string n = name.Substring(8, name.Length - 8 - 2);
                        object value = GetPropertyValue(litem, n);
                        if (value == null)
                            tb.Text = "Change Me";
                        else
                            tb.Text = value.ToString();
                    }
                    else if(control is NumericUpDown)
                    {
                        NumericUpDown nud = control as NumericUpDown;
                        string n = name.Substring(8, name.Length - 8 - 3);
                        object value = GetPropertyValue(litem, n);
                        nud.Value = (int)value;
                    }
                    else if(control is CheckBox)
                    {
                        CheckBox cb = control as CheckBox;
                        string n = name.Substring(8, name.Length - 8 - 2);
                        object value = GetPropertyValue(litem, n);
                        if (value == null)
                            cb.Checked = true;
                        else
                            cb.Checked = (int)value ==1 ? true:false;
                    }
                    else if (control is ComboBox)
                    {
                        ComboBox cb = control as ComboBox;
                        string n = name.Substring(8, name.Length - 8 - 2);
                        object value = GetPropertyValue(litem, n);
                        if (value == null)
                            cb.SelectedIndex = 0;
                        else
                            cb.SelectedItem = value.ToString();
                    }
                }
            }
            TerjeLoadouts.isDirty = true;
        }
        static object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
                return null;

            PropertyInfo prop = obj.GetType().GetProperty(propertyName);
            if (prop == null)
                return null;

            return prop.GetValue(obj, null);
        }
        static void SetPropertyValue(object obj, string propertyName, object value)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
                return;

            PropertyInfo prop = obj.GetType().GetProperty(propertyName);
            if (prop == null || !prop.CanWrite)
                return;

            prop.SetValue(obj, value);
        }


    }
    public class ComboBoxItem
    {
        public string Display { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Display;
        }
    }
}
