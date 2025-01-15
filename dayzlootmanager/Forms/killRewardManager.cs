using DarkUI.Forms;
using DayZeLib;
using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;

namespace DayZeEditor
{
    public enum animals
    {
        Wolf,
        Bear,
        Stag,
        Hind,
        Roebuck,
        Doe,
        Wild_Boar,
        Pig,
        Ram,
        Ewe,
        billy_goat,
        goat,
        Bull,
        Cow,
        Rooster,
        Hen
    };
    public partial class killRewardManager : DarkForm
    {
        public Project currentproject { get; set; }
        public string Projectname { get; private set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;

        private bool useraction;
        private bool _preventExpand;
        private DateTime _lastMouseDown;

        public KillRewardBase_Config KillRewardBase_Config { get; set; }
        public KillRewardGift_Config KillRewardGift_Config { get; set; }
        public KillRewardHunting_Config KillRewardHunting_Config { get; set; }
        public KillRewardPlayer_Config KillRewardPlayer_Config { get; set; }
        public KillRewardWeaponBox_Config KillRewardWeaponBox_Config { get;set; }
        public KillRewardZombie_Config KillRewardZombie_Config { get; set; }
        public TreeNode Currenttreeviewtag { get; private set; }

        public killRewardManager()
        {
            InitializeComponent();
        }
        private void killRewardManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (KillRewardBase_Config.isDirty || KillRewardGift_Config.isDirty || KillRewardHunting_Config.isDirty || KillRewardPlayer_Config.isDirty || KillRewardWeaponBox_Config.isDirty || KillRewardZombie_Config.isDirty)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveFile();
                }
            }
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        private void SaveFile()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (KillRewardBase_Config.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(KillRewardBase_Config.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(KillRewardBase_Config.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(KillRewardBase_Config.Filename, Path.GetDirectoryName(KillRewardBase_Config.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(KillRewardBase_Config.Filename) + ".bak", true);
                }
                KillRewardBase_Config.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(KillRewardBase_Config, options);
                File.WriteAllText(KillRewardBase_Config.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(KillRewardBase_Config.Filename));
            }
            if (KillRewardGift_Config.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(KillRewardGift_Config.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(KillRewardGift_Config.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(KillRewardGift_Config.Filename, Path.GetDirectoryName(KillRewardGift_Config.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(KillRewardGift_Config.Filename) + ".bak", true);
                }
                KillRewardGift_Config.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(KillRewardGift_Config, options);
                File.WriteAllText(KillRewardGift_Config.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(KillRewardGift_Config.Filename));
            }
            if (KillRewardHunting_Config.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(KillRewardHunting_Config.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(KillRewardHunting_Config.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(KillRewardHunting_Config.Filename, Path.GetDirectoryName(KillRewardHunting_Config.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(KillRewardHunting_Config.Filename) + ".bak", true);
                }
                KillRewardHunting_Config.isDirty = false;
                KillRewardHunting_Config.KillRewardHUNTING.SetAnimalList();
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(KillRewardHunting_Config, options);
                File.WriteAllText(KillRewardHunting_Config.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(KillRewardHunting_Config.Filename));
            }
            if (KillRewardPlayer_Config.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(KillRewardPlayer_Config.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(KillRewardPlayer_Config.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(KillRewardPlayer_Config.Filename, Path.GetDirectoryName(KillRewardPlayer_Config.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(KillRewardPlayer_Config.Filename) + ".bak", true);
                }
                KillRewardPlayer_Config.isDirty = false;
                KillRewardPlayer_Config.KillRewardPLAYER.SetPlayerList();
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(KillRewardPlayer_Config, options);
                File.WriteAllText(KillRewardPlayer_Config.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(KillRewardPlayer_Config.Filename));
            }
            if (KillRewardWeaponBox_Config.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(KillRewardWeaponBox_Config.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(KillRewardWeaponBox_Config.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(KillRewardWeaponBox_Config.Filename, Path.GetDirectoryName(KillRewardWeaponBox_Config.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(KillRewardWeaponBox_Config.Filename) + ".bak", true);
                }
                KillRewardWeaponBox_Config.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(KillRewardWeaponBox_Config, options);
                File.WriteAllText(KillRewardWeaponBox_Config.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(KillRewardWeaponBox_Config.Filename));
            }
            if (KillRewardZombie_Config.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(KillRewardZombie_Config.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(KillRewardZombie_Config.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(KillRewardZombie_Config.Filename, Path.GetDirectoryName(KillRewardZombie_Config.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(KillRewardZombie_Config.Filename) + ".bak", true);
                }
                KillRewardZombie_Config.isDirty = false;
                KillRewardZombie_Config.KillRewardZOMBIE.SetZombieist();
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(KillRewardZombie_Config, options);
                File.WriteAllText(KillRewardZombie_Config.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(KillRewardZombie_Config.Filename));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + KillrewardStatics.KillReward_CONFIG_FOLDER);
        }
        private void killRewardManager_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            useraction = false;
            string KillRewardBase_ConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + KillrewardStatics.KillRewardBase_CONFIG_JSON;
            KillRewardBase_Config = JsonSerializer.Deserialize<KillRewardBase_Config>(File.ReadAllText(KillRewardBase_ConfigPath));
            KillRewardBase_Config.isDirty = false;
            KillRewardBase_Config.Filename = KillRewardBase_ConfigPath;

            string KillRewardGift_ConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + KillrewardStatics.KillRewardGift_CONFIG_JSON;
            KillRewardGift_Config = JsonSerializer.Deserialize<KillRewardGift_Config>(File.ReadAllText(KillRewardGift_ConfigPath));
            KillRewardGift_Config.isDirty = false;
            KillRewardGift_Config.Filename = KillRewardGift_ConfigPath;

            string KillRewardHunting_ConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + KillrewardStatics.KillRewardHunting_CONFIG_JSON;
            KillRewardHunting_Config = JsonSerializer.Deserialize<KillRewardHunting_Config>(File.ReadAllText(KillRewardHunting_ConfigPath));
            KillRewardHunting_Config.KillRewardHUNTING.GetAnimaList();
            KillRewardHunting_Config.isDirty = false;
            KillRewardHunting_Config.Filename = KillRewardHunting_ConfigPath;

            string KillRewardPlayer_ConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + KillrewardStatics.KillRewardPlayer_CONFIG_JSON;
            KillRewardPlayer_Config = JsonSerializer.Deserialize<KillRewardPlayer_Config>(File.ReadAllText(KillRewardPlayer_ConfigPath));
            KillRewardPlayer_Config.KillRewardPLAYER.GetPlayerlist();
            KillRewardPlayer_Config.isDirty = false;
            KillRewardPlayer_Config.Filename = KillRewardPlayer_ConfigPath;

            string KillRewardWeaponBox_ConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + KillrewardStatics.KillRewardWeaponBox_CONFIG_JSON;
            KillRewardWeaponBox_Config = JsonSerializer.Deserialize<KillRewardWeaponBox_Config>(File.ReadAllText(KillRewardWeaponBox_ConfigPath));
            KillRewardWeaponBox_Config.isDirty = false;
            KillRewardWeaponBox_Config.Filename = KillRewardWeaponBox_ConfigPath;

            string KillRewardZombie_ConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + KillrewardStatics.KillRewardZombie_CONFIG_JSON;
            KillRewardZombie_Config = JsonSerializer.Deserialize<KillRewardZombie_Config>(File.ReadAllText(KillRewardZombie_ConfigPath));
            KillRewardZombie_Config.KillRewardZOMBIE.GetZombielist();
            KillRewardZombie_Config.isDirty = false;
            KillRewardZombie_Config.Filename = KillRewardZombie_ConfigPath;

            Loadsettings();
            useraction = true;

        }

        private void Loadsettings()
        {
            Cursor.Current = Cursors.WaitCursor;
            KillRewardTV.Nodes.Clear();
            TreeNode RootNode = new TreeNode("Kill Reward Settings:-")
            {
                Tag = "ParentRoot"
            };
            RootNode.Nodes.Add(AddKRBaseNode());
            RootNode.Nodes.Add(AddKRGiftNode());
            RootNode.Nodes.Add(AddHuntingNodes());
            TreeNode playerroot = new TreeNode("Kill Reward Player")
            {
                Tag = KillRewardPlayer_Config
            };
            playerroot.Nodes.Add(AddPlayerNegative());
            playerroot.Nodes.Add(AddPlayerPlayer());
            playerroot.Nodes.Add(addPlayerShootdistance());

            RootNode.Nodes.Add(playerroot);
            RootNode.Nodes.Add(AddweaponBoxNodes());
            RootNode.Nodes.Add(AddZombieNodes());
            KillRewardTV.Nodes.Add(RootNode);
            Cursor.Current = Cursors.Default;
        }
        private TreeNode AddKRBaseNode()
        {
            TreeNode KRBaseNode = new TreeNode("Kill Reward Base")
            {
                Tag = KillRewardBase_Config
            };
            TreeNode NotListedNode = new TreeNode($"NotListedIDs:-")
            {
                Tag = "NotListedIDs",
                Name = "NotListedIDs"
            };
            foreach (string Steamid in KillRewardBase_Config.KillRewardBASE.NotListedIDs)
            {
                TreeNode buildingnode = new TreeNode($"Steam ID: - {Steamid}")
                {
                    Tag = "SteamID",
                    Name = "SteamID"

                };
                NotListedNode.Nodes.Add(buildingnode);
            }
            KRBaseNode.Nodes.Add(NotListedNode);
            return KRBaseNode;
        }
        private TreeNode AddKRGiftNode()
        {
            TreeNode KRgiftNode = new TreeNode("Kill Reward Gift")
            {
                Tag = KillRewardGift_Config
            };
            TreeNode PlayerGiftEnabledtn = new TreeNode($"Player Gift Enabled:- {(KillRewardGift_Config.KillRewardGIFT.PlayerGift == 1 ? true : false)}")
            {
                Tag = "PlayerGiftEnabled",
                Name = "PlayerGiftEnabled"
            };
            KRgiftNode.Nodes.Add(PlayerGiftEnabledtn);
            TreeNode GiftLifetimetn = new TreeNode($"Gift Lifetime:- {KillRewardGift_Config.KillRewardGIFT.GiftLifetime}")
            {
                Tag = "GiftLifetime",
                Name = "GiftLifetime"
            };
            KRgiftNode.Nodes.Add(GiftLifetimetn);
            TreeNode GiftBoxNode = new TreeNode("Gift Boxes:-")
            {
                Tag = "GiftBoxes",
                Name = "GiftBoxes",
            };

            foreach (string gb in KillRewardGift_Config.KillRewardGIFT.Giftbox)
            {
                TreeNode gbnode = new TreeNode($"Box Type: - {gb}")
                {
                    Tag = "BoxType",
                    Name = "BoxType"

                };
                GiftBoxNode.Nodes.Add(gbnode);
            };
            KRgiftNode.Nodes.Add(GiftBoxNode);
            TreeNode GiftDrink = new TreeNode("Gift Drinks:-")
            {
                Tag = "GiftDrinks",
                Name = "GiftDrinks",
            };
            foreach (string gd in KillRewardGift_Config.KillRewardGIFT.Giftdrink)
            {
                TreeNode gdnode = new TreeNode($"Drink Type: - {gd}")
                {
                    Tag = "DrinkType",
                    Name = "DrinkType"

                };
                GiftDrink.Nodes.Add(gdnode);
            };
            KRgiftNode.Nodes.Add(GiftDrink);
            TreeNode GiftTreats = new TreeNode("Gift Food:-")
            {
                Tag = "GiftTreats",
                Name = "GiftTreats",
            };
            foreach (string gt in KillRewardGift_Config.KillRewardGIFT.Gifteat)
            {
                TreeNode gtnode = new TreeNode($"Food Type: - {gt}")
                {
                    Tag = "TreatType",
                    Name = "TreatType"

                };
                GiftTreats.Nodes.Add(gtnode);
            };
            KRgiftNode.Nodes.Add(GiftTreats);
            TreeNode GiftTools = new TreeNode("Gift Tools:-")
            {
                Tag = "GiftTools",
                Name = "GiftTools",
            };
            foreach (string gt in KillRewardGift_Config.KillRewardGIFT.Gifttools)
            {
                TreeNode gtnode = new TreeNode($"Tool Type: - {gt}")
                {
                    Tag = "Tooltype",
                    Name = "Tooltype"

                };
                GiftTools.Nodes.Add(gtnode);
            };
            KRgiftNode.Nodes.Add(GiftTools);
            TreeNode GiftMedical = new TreeNode("Gift Medical:-")
            {
                Tag = "GiftMedical",
                Name = "GiftMedical",
            };
            foreach (string gm in KillRewardGift_Config.KillRewardGIFT.Giftmedical)
            {
                TreeNode gmnode = new TreeNode($"Medical Type: - {gm}")
                {
                    Tag = "MedicalType",
                    Name = "MedicalType"

                };
                GiftMedical.Nodes.Add(gmnode);
            };
            KRgiftNode.Nodes.Add(GiftMedical);
            return KRgiftNode;
        }
        private TreeNode AddHuntingNodes()
        {
            TreeNode KRHuntingNode = new TreeNode("Kill Reward Hunting")
            {
                Tag = KillRewardHunting_Config
            };
            TreeNode HuntingRewardEnabledtn = new TreeNode($"Hunting Reward Enabled:- {(KillRewardHunting_Config.KillRewardHUNTING.HuntingReward == 1 ? true : false)}")
            {
                Tag = "HuntingKillRewardEnabled",
                Name = "HuntingKillRewardEnabled"
            };
            KRHuntingNode.Nodes.Add(HuntingRewardEnabledtn);
            foreach (KillReward_KillrewardHuntingAnimals animals in KillRewardHunting_Config.KillRewardHUNTING.KRAnimals)
            {
                TreeNode Animalnode = new TreeNode($"Animal:- {animals.AnimalName}")
                {
                    Tag = animals.AnimalName,
                    Name = "Animal"
                };
                TreeNode animapoints = new TreeNode($"Points:- {KillRewardHunting_Config.KillRewardHUNTING.getPoints(animals.AnimalName + "Points")}")
                {
                    Tag = "AnimalPoints",
                    Name = "AnimalPoints"
                };
                Animalnode.Nodes.Add(animapoints);

                TreeNode killsnode = new TreeNode($"Kills:Money")
                {
                    Tag = animals,
                    Name = animals.AnimalName
                };
                foreach (AnimalKillMoney akm in animals.killsmoney) 
                {
                    TreeNode akill = new TreeNode($"{akm.kills}:{akm.money}")
                    {
                        Tag = akm
                    };
                    killsnode.Nodes.Add(akill);
                }
                Animalnode.Nodes.Add(killsnode);
                KRHuntingNode.Nodes.Add( Animalnode );
            }
            return KRHuntingNode;
        }
        private TreeNode AddPlayerNegative()
        {
            TreeNode negativetn = new TreeNode("Negative")
            {
                Tag = KillRewardPlayer_Config.KillRewardNEGATIVE
            };
            TreeNode PlayerKillNegativeRewardtn = new TreeNode($"Player Kill Negative Reward Enabled:- {(KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeReward == 1 ? true : false)}")
            {
                Tag = "PlayerKillNegativeReward",
                Name = "PlayerKillNegativeReward"
            };
            negativetn.Nodes.Add(PlayerKillNegativeRewardtn);
            TreeNode PlayerKilltn = new TreeNode($"Player Kill:- {KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKill}")
            {
                Tag = "PlayerKill",
                Name = "PlayerKill"
            };
            negativetn.Nodes.Add(PlayerKilltn);
            TreeNode Pointlosetn = new TreeNode($"Point lose:- {KillRewardPlayer_Config.KillRewardNEGATIVE.Pointlose}")
            {
                Tag = "Pointlose",
                Name = "Pointlose"
            };
            negativetn.Nodes.Add(Pointlosetn);
            TreeNode LoseMoneytn = new TreeNode($"Lose Money:- {KillRewardPlayer_Config.KillRewardNEGATIVE.LoseMoney}")
            {
                Tag = "LoseMoney",
                Name = "LoseMoney"
            };
            negativetn.Nodes.Add(LoseMoneytn);
            TreeNode PlayerKillNegativeRewardBonusQuantitytn = new TreeNode($"Player Kill Negative Reward Bonus Quantity:- {KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeRewardBonusQuantity}")
            {
                Tag = "PlayerKillNegativeRewardBonusQuantity",
                Name = "PlayerKillNegativeRewardBonusQuantity"
            };
            negativetn.Nodes.Add(PlayerKillNegativeRewardBonusQuantitytn);
            TreeNode PlayerKillNegativeRewardBonustn = new TreeNode($"Player Kill Negative Reward Bonus:-")
            {
                Tag = "PlayerKillNegativeRewardBonus",
                Name = "PlayerKillNegativeRewardBonus"
            };
            foreach (string s in KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeRewardBonus)
            {
                PlayerKillNegativeRewardBonustn.Nodes.Add( new TreeNode($"Negative Reward Bonus Type:- {s}")
                {
                    Tag = "negativeRewardBonusType",
                    Name = "negativeRewardBonusType"
                });
            }
            negativetn.Nodes.Add(PlayerKillNegativeRewardBonustn);
            return negativetn;
        }
        private TreeNode AddPlayerPlayer()
        {
            TreeNode Playertn = new TreeNode("Player")
            {
                Tag = KillRewardPlayer_Config.KillRewardPLAYER
            };
            TreeNode PlayerKillRewardtn = new TreeNode($"Player Kill Reward Enabled:- {(KillRewardPlayer_Config.KillRewardPLAYER.PlayerKillReward == 1 ? true : false)}")
            {
                Tag = "PlayerKillReward",
                Name = "PlayerKillReward"
            };
            Playertn.Nodes.Add(PlayerKillRewardtn);
            TreeNode PlayerKillPointstn = new TreeNode($"Player Kill Points:- {KillRewardPlayer_Config.KillRewardPLAYER.PlayerKillPoints}")
            {
                Tag = "PlayerKillPoints",
                Name = "PlayerKillPoints"
            };
            Playertn.Nodes.Add(PlayerKillPointstn);
            TreeNode PlayerKillsMoneytn = new TreeNode("Player Kills : Money")
            {
                Tag = "Killreward_Player_Weapons",
                Name = "Killreward_Player_Weapons"
            };
            foreach(Killreward_Kilss_Money_Weapons KRpw in KillRewardPlayer_Config.KillRewardPLAYER._playuerWeapons)
            {
                TreeNode treeNode = new TreeNode($"{KRpw.kills}:{KRpw.money}")
                {
                    Tag = KRpw
                };
                if(KRpw.hasWeaponsBox) 
                {
                    treeNode.Nodes.Add(new TreeNode($"Player Weapon Box Number:- {KRpw.WeaponsBoxNumber}")
                    {
                        Tag = "PlayerWeaponBoxNumber",
                        Name = "PlayerWeaponBoxNumber"
                    });
                }
                PlayerKillsMoneytn.Nodes.Add(treeNode);
            }
            Playertn.Nodes.Add(PlayerKillsMoneytn);
            return Playertn;
        }
        private TreeNode addPlayerShootdistance()
        {
            TreeNode Distancetn = new TreeNode("Shooting Distance")
            {
                Tag = KillRewardPlayer_Config.KillRewardSHOOTDISTANCE
            };
            TreeNode SHOOTRewardtn = new TreeNode($"Shot Ditance Reward Enabled:- {(KillRewardPlayer_Config.KillRewardSHOOTDISTANCE.SHOOTReward == 1 ? true : false)}")
            {
                Tag = "SHOOTReward",
                Name = "SHOOTReward"
            };
            Distancetn.Nodes.Add(SHOOTRewardtn);
            TreeNode PlayerSHOOTDISTANCEtn = new TreeNode($"Player Shot Distance:- {KillRewardPlayer_Config.KillRewardSHOOTDISTANCE.PlayerSHOOTDISTANCE}")
            {
                Tag = "PlayerSHOOTDISTANCE",
                Name = "PlayerSHOOTDISTANCE"
            };
            Distancetn.Nodes.Add(PlayerSHOOTDISTANCEtn);
            return Distancetn;
        }
        private TreeNode AddweaponBoxNodes()
        {
            TreeNode WeaponBoxRoot = new TreeNode("Kill Reward weapon Boxes")
            {
                Tag = KillRewardWeaponBox_Config
            };
            foreach (KillReward_KillRewardWEAPONBOXSettings krwb in KillRewardWeaponBox_Config.KillRewardWEAPONBOX)
            {
                TreeNode weaponBoxNode = new TreeNode($"Weapon Box Number:- {krwb.BoxNumber}")
                {
                    Tag = krwb
                };
                weaponBoxNode.Nodes.Add(new TreeNode($"Box Type:- {krwb.Box}")
                {
                    Tag = "Box",
                    Name = "Box"
                });
                weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon:- {krwb.BoxWeapon}")
                {
                    Tag = "BoxWeapon",
                    Name = "BoxWeapon"
                });
                weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon Bayonet:- {krwb.BoxWeaponBayonet}")
                {
                    Tag = "BoxWeaponBayonet",
                    Name = "BoxWeaponBayonet"
                });
                weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon Buttstock:- {krwb.BoxWeaponButtstock}")
                {
                    Tag = "BoxWeaponButtstock",
                    Name = "BoxWeaponButtstock"
                });
                weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon Handguard:- {krwb.BoxWeaponHandguard}")
                {
                    Tag = "BoxWeaponHandguard",
                    Name = "BoxWeaponHandguard"
                });
                weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon suppressor:- {krwb.BoxWeaponsuppressor}")
                {
                    Tag = "BoxWeaponsuppressor",
                    Name = "BoxWeaponsuppressor"
                });
                weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon Sight:- {krwb.BoxWeaponSight}")
                {
                    Tag = "BoxWeaponSight",
                    Name = "BoxWeaponSight"
                });
                weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon Magazine:- {krwb.BoxWeaponMagazin}")
                {
                    Tag = "BoxWeaponMagazine",
                    Name = "BoxWeaponMagazine"
                });
                weaponBoxNode.Nodes.Add(new TreeNode($"Box Magazine Quantity:- {krwb.BoxMagazinQuantity}")
                {
                    Tag = "BoxMagazineQuantity",
                    Name = "BoxMagazineQuantity"
                });
                TreeNode BoxBonusroot = new TreeNode($"Box Bonus:-")
                {
                    Tag = "BoxBonus",
                    Name = "BoxBonus"
                };
                foreach (string BoxBonus in krwb.BoxBonus)
                {
                    TreeNode boxbonusnode = new TreeNode($"Box Bonus Type: - {BoxBonus}")
                    {
                        Tag = "BoxBonusType",
                        Name = "BoxBonusType"

                    };
                    BoxBonusroot.Nodes.Add(boxbonusnode);
                }
                weaponBoxNode.Nodes.Add(BoxBonusroot);
                WeaponBoxRoot.Nodes.Add(weaponBoxNode);
            }
            return WeaponBoxRoot;
        }
        private TreeNode AddZombieNodes()
        {
            TreeNode ZombieRoot = new TreeNode("Kill Reward Zombie")
            {
                Tag = KillRewardZombie_Config
            };
            TreeNode ZombieKillRewardEnabledtn = new TreeNode($"Zombie Kill Reward Enabled:- {(KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillReward == 1 ? true : false)}")
            {
                Tag = "ZombieKillRewardEnabled",
                Name = "ZombieKillRewardEnabled"
            };
            ZombieRoot.Nodes.Add(ZombieKillRewardEnabledtn);
            TreeNode PlayerKillPointstn = new TreeNode($"Zombie Kill Points:- {KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillPoints}")
            {
                Tag = "ZombieKillPoints",
                Name = "ZombieKillPoints"
            };
            ZombieRoot.Nodes.Add(PlayerKillPointstn);
            TreeNode ZombieKillsbyCarEnabledtn = new TreeNode($"Zombie Kill by Car Enabled:- {(KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillsbyCar == 1 ? true : false)}")
            {
                Tag = "ZombieKillsbyCarEnabled",
                Name = "ZombieKillsbyCarEnabled"
            };
            ZombieRoot.Nodes.Add(ZombieKillsbyCarEnabledtn);
            TreeNode ZonbieKillsMoneytn = new TreeNode("Zombie Kills : Money")
            {
                Tag = "Killreward_Zombie_Weapons",
                Name = "Killreward_Zombie_Weapons"
            };
            foreach (Killreward_Kilss_Money_Weapons KRpw in KillRewardZombie_Config.KillRewardZOMBIE._ZombieWeapons)
            {
                TreeNode treeNode = new TreeNode($"{KRpw.kills}:{KRpw.money}")
                {
                    Tag = KRpw
                };
                if (KRpw.hasWeaponsBox)
                {
                    treeNode.Nodes.Add(new TreeNode($"Zombie Weapon Box Number:- {KRpw.WeaponsBoxNumber}")
                    {
                        Tag = "ZombieWeaponBoxNumber",
                        Name = "ZombieWeaponBoxNumber"
                    });
                }
                ZonbieKillsMoneytn.Nodes.Add(treeNode);
            }
            ZombieRoot.Nodes.Add(ZonbieKillsMoneytn);
            return ZombieRoot;
        }
        private void KillRewardTV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Currenttreeviewtag = e.Node;
            KRBaseGB.Visible = false;
            useraction = false;
            if(e.Node.Tag.ToString() == "NotListedIDs" || e.Node.Tag.ToString() == "SteamID" || e.Node.Tag is KillRewardBase_Config)
            {
                KRBaseGB.Visible = true;
                ZombieGlobalMessageCB.Checked = KillRewardBase_Config.KillRewardBASE.ZombieGlobalMessage == 1 ? true : false;
                PlayerGlobalMessageCB.Checked = KillRewardBase_Config.KillRewardBASE.PlayerGlobalMessage == 1 ? true : false;
                MessageSystemCB.SelectedIndex = KillRewardBase_Config.KillRewardBASE.MessageSystem;
                PointLoseByDeathNUD.Value = KillRewardBase_Config.KillRewardBASE.PointLoseByDeath;
                StatsloseNUD.Value = KillRewardBase_Config.KillRewardBASE.Statslose;
                DebugLogCB.Checked = KillRewardBase_Config.KillRewardBASE.DebugLog == 1 ? true : false;
                PVPVECB.SelectedIndex = KillRewardBase_Config.KillRewardBASE.PVPVE;
            }
            useraction = true;
        }
        private void KillRewardTV_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            AnimalKillMoney akm = null;
            Killreward_Kilss_Money_Weapons krpw = null;
            akm = e.Node.Tag as AnimalKillMoney;
            krpw = e.Node.Tag as Killreward_Kilss_Money_Weapons;
            if (e.Node.Tag.ToString() != "SteamID" &&
                e.Node.Tag.ToString() != "PlayerGiftEnabled" &&
                e.Node.Tag.ToString() != "GiftLifetime" &&
                e.Node.Tag.ToString() != "HuntingKillRewardEnabled" &&
                e.Node.Tag.ToString() != "AnimalPoints" &&
                e.Node.Tag.ToString() != "PlayerKillNegativeReward" &&
                e.Node.Tag.ToString() != "PlayerKill" &&
                e.Node.Tag.ToString() != "Pointlose" &&
                e.Node.Tag.ToString() != "LoseMoney" &&
                e.Node.Tag.ToString() != "PlayerKillNegativeRewardBonusQuantity" &&
                e.Node.Tag.ToString() != "PlayerKillReward" &&
                e.Node.Tag.ToString() != "PlayerKillPoints" &&
                e.Node.Tag.ToString() != "SHOOTReward" &&
                e.Node.Tag.ToString() != "PlayerSHOOTDISTANCE" &&
                e.Node.Tag.ToString() != "BoxMagazineQuantity" &&
                e.Node.Tag.ToString() != "ZombieKillRewardEnabled" &&
                e.Node.Tag.ToString() != "ZombieKillPoints" &&
                e.Node.Tag.ToString() != "ZombieKillsbyCarEnabled" &&
                krpw == null &&
                akm == null)
                e.CancelEdit = true;
        }
        private void KillRewardTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Currenttreeviewtag = e.Node;
            KillRewardTV.SelectedNode = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                addSteamIDToolStripMenuItem.Visible = false;
                RemoveSteamIDToolStripMenuItem.Visible = false;
                addNewGiftBoxToolStripMenuItem.Visible = false;
                removeGiftBoxoolStripMenuItem.Visible = false;
                addDrinkToolStripMenuItem.Visible = false;
                removeDrinkStripMenuItem1.Visible = false;
                addNewTreatToolStripMenuItem.Visible = false;
                removeTreatStripMenuItem2.Visible = false;
                addToolToolStripMenuItem.Visible = false;
                removeToolToolStripMenuItem3.Visible = false;
                addNewMedicalToolStripMenuItem.Visible = false;
                removeMedicalToolStripMenuItem4.Visible = false;
                addNewKillMoneyAnimalToolStripMenuItem.Visible = false;
                removeKillMoneyAnimalToolStripMenuItem.Visible= false;
                sortToolStripMenuItem.Visible= false;
                addWeaponBoxToolStripMenuItem.Visible = false;
                removeWeaponBoxToolStripMenuItem1.Visible = false;
                addNegativeKillRewardBonusToolStripMenuItem.Visible = false;
                removeNegativeBonusToolStripMenuItem1.Visible = false;
                addNewKillMoneyPlayerToolStripMenuItem1.Visible = false;
                removeKillMoneyPlayerToolStripMenuItem1.Visible = false;
                addNewWeaponBoxToolStripMenuItem.Visible = false;
                removeWeaponBoxToolStripMenuItem.Visible = false;
                addNewBoxBonusToolStripMenuItem.Visible = false;
                removeBoxBonusToolStripMenuItem.Visible = false;
                addNewKillMoneyZombieToolStripMenuItem.Visible = false;
                if (e.Node.Tag.ToString() == "NotListedIDs")
                {
                    addSteamIDToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "SteamID")
                {
                    RemoveSteamIDToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "GiftBoxes")
                {
                    addNewGiftBoxToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "BoxType")
                {
                    removeGiftBoxoolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "GiftDrinks")
                {
                    addDrinkToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "DrinkType")
                {
                    removeDrinkStripMenuItem1.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "GiftTreats")
                {
                    addNewTreatToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "TreatType")
                {
                    removeTreatStripMenuItem2.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "GiftTools")
                {
                    addToolToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "Tooltype")
                {
                    removeToolToolStripMenuItem3.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "GiftMedical")
                {
                    addNewMedicalToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "MedicalType")
                {
                    removeMedicalToolStripMenuItem4.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag is KillReward_KillrewardHuntingAnimals)
                {
                    addNewKillMoneyAnimalToolStripMenuItem.Visible = true;
                    sortToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag is AnimalKillMoney)
                {
                    removeKillMoneyAnimalToolStripMenuItem.Visible= true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "Killreward_Player_Weapons")
                {
                    addNewKillMoneyPlayerToolStripMenuItem1.Visible = true;
                    sortToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag is Killreward_Kilss_Money_Weapons)
                {
                    addWeaponBoxToolStripMenuItem.Visible = true;
                    removeKillMoneyPlayerToolStripMenuItem1.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "PlayerWeaponBoxNumber" ||
                    e.Node.Tag.ToString() == "ZombieWeaponBoxNumber")
                {
                    removeWeaponBoxToolStripMenuItem1.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "PlayerKillNegativeRewardBonus")
                {
                    addNegativeKillRewardBonusToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "negativeRewardBonusType")
                {
                    removeNegativeBonusToolStripMenuItem1.Visible= true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag is KillRewardWeaponBox_Config)
                {
                    addNewWeaponBoxToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag is KillReward_KillRewardWEAPONBOXSettings)
                {
                    removeWeaponBoxToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "BoxBonus")
                {
                    addNewBoxBonusToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "BoxBonusType")
                {
                    removeBoxBonusToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "Killreward_Zombie_Weapons")
                {
                    addNewKillMoneyZombieToolStripMenuItem.Visible = true;
                    sortToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }
        private void KillRewardTV_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag.ToString() == "SteamID" ||
                e.Node.Tag.ToString() == "PlayerGiftEnabled" ||
                e.Node.Tag.ToString() == "GiftLifetime" ||
                e.Node.Tag.ToString() == "HuntingKillRewardEnabled" ||
                e.Node.Tag.ToString() == "AnimalPoints" ||
                e.Node.Tag is AnimalKillMoney ||
                e.Node.Tag.ToString() == "PlayerKillNegativeReward" ||
                e.Node.Tag.ToString() == "PlayerKill" ||
                e.Node.Tag.ToString() == "Pointlose" ||
                e.Node.Tag.ToString() == "LoseMoney" ||
                e.Node.Tag.ToString() == "PlayerKillNegativeRewardBonusQuantity" ||
                e.Node.Tag.ToString() == "PlayerKillReward" ||
                e.Node.Tag.ToString() == "PlayerKillPoints" ||
                e.Node.Tag is Killreward_Kilss_Money_Weapons ||
                e.Node.Tag.ToString() == "SHOOTReward" ||
                e.Node.Tag.ToString() == "PlayerSHOOTDISTANCE" ||
                e.Node.Tag.ToString() == "BoxMagazineQuantity" ||
                e.Node.Tag.ToString() == "ZombieKillRewardEnabled" ||
                e.Node.Tag.ToString() == "ZombieKillPoints" ||
                e.Node.Tag.ToString() == "ZombieKillsbyCarEnabled")
                e.Node.BeginEdit();
            else if (e.Node.Tag.ToString() == "negativeRewardBonusType")
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
                        string currentitem = e.Node.Text.Substring(29);
                        int index = KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeRewardBonus.IndexOf(currentitem);
                        KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeRewardBonus[index] = l;
                        e.Node.Text = $"Negative Reward Bonus Type:- {l}";
                        KillRewardPlayer_Config.isDirty = true;
                    }

                }
            }
            else if (e.Node.Tag.ToString() == "PlayerWeaponBoxNumber")
            {
                addKillRewardWeaponBox addnewweaponbox = new addKillRewardWeaponBox()
                {
                    WeaponBoxes = KillRewardWeaponBox_Config.KillRewardWEAPONBOX
                };
                DialogResult result = addnewweaponbox.ShowDialog();
                if (result == DialogResult.OK)
                {
                    KillReward_KillRewardWEAPONBOXSettings newwb = addnewweaponbox.SelectedWeaponBox;
                    Killreward_Kilss_Money_Weapons krpw = e.Node.Parent.Tag as Killreward_Kilss_Money_Weapons;
                    krpw.hasWeaponsBox = true;
                    krpw.WeaponsBoxNumber = newwb.BoxNumber;
                    e.Node.Text = $"Player Weapon Box Number:- {krpw.WeaponsBoxNumber}";
                    KillRewardPlayer_Config.isDirty = true;
                }
            }
            else if (e.Node.Parent.Tag is KillReward_KillRewardWEAPONBOXSettings &&
                e.Node.Tag.ToString() != "BoxBonus" ||
                e.Node.Tag.ToString() != "BoxBonusType")
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
                    if (e.Node.Parent.Tag is KillReward_KillRewardWEAPONBOXSettings)
                    {
                        KillReward_KillRewardWEAPONBOXSettings krwbs = Currenttreeviewtag.Parent.Tag as KillReward_KillRewardWEAPONBOXSettings;
                        List<string> addedtypes = form.addedtypes.ToList();
                        if (addedtypes.Count() == 0)
                        {
                            addedtypes.Add("");
                        }
                        foreach (string l in addedtypes)
                        {
                            switch (e.Node.Tag.ToString())
                            {
                                case "Box":
                                    krwbs.Box = l;
                                    e.Node.Text = $"Box Type:- {krwbs.Box}";
                                    break;
                                case "BoxWeapon":
                                    krwbs.BoxWeapon = l;
                                    e.Node.Text = $"Box Weapon:- {krwbs.BoxWeapon}";
                                    break;
                                case "BoxWeaponBayonet":
                                    krwbs.BoxWeaponBayonet = l;
                                    e.Node.Text = $"Box Weapon Bayonet:- {krwbs.BoxWeaponBayonet}";
                                    break;
                                case "BoxWeaponButtstock":
                                    krwbs.BoxWeaponButtstock = l;
                                    e.Node.Text = $"Box Weapon Buttstock:- {krwbs.BoxWeaponButtstock}";
                                    break;
                                case "BoxWeaponHandguard":
                                    krwbs.BoxWeaponHandguard = l;
                                    e.Node.Text = $"Box Weapon Handguard:- {krwbs.BoxWeaponHandguard}";
                                    break;
                                case "BoxWeaponsuppressor":
                                    krwbs.BoxWeaponsuppressor = l;
                                    e.Node.Text = $"Box Weapon suppressor:- {krwbs.BoxWeaponsuppressor}";
                                    break;
                                case "BoxWeaponSight":
                                    krwbs.BoxWeaponSight = l;
                                    e.Node.Text = $"Box Weapon Sight:- {krwbs.BoxWeaponSight}";
                                    break;
                                case "BoxWeaponMagazine":
                                    krwbs.BoxWeaponMagazin = l;
                                    e.Node.Text = $"Box Weapon Magazine:- {krwbs.BoxWeaponMagazin}";
                                    break;
                            }
                            KillRewardWeaponBox_Config.isDirty = true;
                        }
                    }
                }
            }
        }
        private void KillRewardTV_RequestDisplayText(object sender, TreeViewMS.NodeRequestTextEventArgs e)
        {
            if (e.Node.Tag.ToString() == "SteamID")
            {
                string steamid = e.Node.Text.Substring(12);
                int index = KillRewardBase_Config.KillRewardBASE.NotListedIDs.IndexOf(steamid);
                KillRewardBase_Config.KillRewardBASE.NotListedIDs[index] = e.Label;
                e.Node.Text = e.Label = $"Steam ID: - {e.Label}";
                KillRewardBase_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "PlayerGiftEnabled")
            {
                if (e.Label.ToLower() == "true")
                    KillRewardGift_Config.KillRewardGIFT.PlayerGift = 1;
                else if (e.Label.ToLower() == "false")
                    KillRewardGift_Config.KillRewardGIFT.PlayerGift = 0;
                else
                {
                    MessageBox.Show("Please Enter either True or False");
                    e.Node.Text = e.Label = $"Player Gift Enabled:- {(KillRewardGift_Config.KillRewardGIFT.PlayerGift == 1 ? true : false)}";
                    return;
                }
                e.Node.Text = e.Label = $"Player Gift Enabled:- {(KillRewardGift_Config.KillRewardGIFT.PlayerGift == 1 ? true : false)}";
                KillRewardGift_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "GiftLifetime")
            {
                KillRewardGift_Config.KillRewardGIFT.GiftLifetime = Convert.ToInt32(e.Label);
                e.Node.Text = e.Label = $"Gift Lifetime:- {e.Label}";
                KillRewardGift_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "HuntingKillRewardEnabled")
            {
                if (e.Label.ToLower() == "true")
                    KillRewardHunting_Config.KillRewardHUNTING.HuntingReward = 1;
                else if (e.Label.ToLower() == "false")
                    KillRewardHunting_Config.KillRewardHUNTING.HuntingReward = 0;
                else
                {
                    MessageBox.Show("Please Enter either True or False");
                    e.Node.Text = e.Label = $"Hunting Reward Enabled:- {(KillRewardHunting_Config.KillRewardHUNTING.HuntingReward == 1 ? true : false)}";
                    return;
                }
                e.Node.Text = e.Label = $"Hunting Reward Enabled:- {(KillRewardHunting_Config.KillRewardHUNTING.HuntingReward == 1 ? true : false)}";
                KillRewardHunting_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "AnimalPoints")
            {
                string animalname = e.Node.Parent.Tag.ToString();
                KillRewardHunting_Config.KillRewardHUNTING.setPoints(animalname + "Points", Convert.ToInt32(e.Label));
                e.Node.Text = e.Label = $"Points:- {e.Label}";
                KillRewardHunting_Config.isDirty = true;
            }
            else if (e.Node.Tag is AnimalKillMoney)
            {
                string[] info = e.Label.Split(':');
                string kills = info[0];
                string money = info[1];
                AnimalKillMoney akm = e.Node.Tag as AnimalKillMoney;
                akm.kills = Convert.ToInt32(kills);
                akm.money = Convert.ToInt32(money);
                e.Node.Text = e.Label = $"{akm.kills}:{akm.money}";
                KillRewardHunting_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "PlayerKillNegativeReward")
            {
                if (e.Label.ToLower() == "true")
                    KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeReward = 1;
                else if (e.Label.ToLower() == "false")
                    KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeReward = 0;
                else
                {
                    MessageBox.Show("Please Enter either True or False");
                    e.Node.Text = e.Label = $"Player Kill Negative Reward Enabled:- {(KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeReward == 1 ? true : false)}";
                    return;
                }
                e.Node.Text = e.Label = $"Player Kill Negative Reward Enabled:- {(KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeReward == 1 ? true : false)}";
                KillRewardPlayer_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "PlayerKill")
            {
                KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKill = Convert.ToInt32(e.Label);
                e.Node.Text = e.Label = $"Player Kill:- {KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKill}";
                KillRewardPlayer_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "Pointlose")
            {
                KillRewardPlayer_Config.KillRewardNEGATIVE.Pointlose = Convert.ToInt32(e.Label);
                e.Node.Text = e.Label = $"Point lose:- {KillRewardPlayer_Config.KillRewardNEGATIVE.Pointlose}";
                KillRewardPlayer_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "LoseMoney")
            {
                KillRewardPlayer_Config.KillRewardNEGATIVE.LoseMoney = Convert.ToInt32(e.Label);
                e.Node.Text = e.Label = $"Lose Money:- {KillRewardPlayer_Config.KillRewardNEGATIVE.LoseMoney}";
                KillRewardPlayer_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "PlayerKillNegativeRewardBonusQuantity")
            {
                KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeRewardBonusQuantity = Convert.ToInt32(e.Label);
                e.Node.Text = e.Label = $"Player Kill Negative Reward Bonus Quantity:- {KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeRewardBonusQuantity}";
                KillRewardPlayer_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "PlayerKillReward")
            {
                if (e.Label.ToLower() == "true")
                    KillRewardPlayer_Config.KillRewardPLAYER.PlayerKillReward = 1;
                else if (e.Label.ToLower() == "false")
                    KillRewardPlayer_Config.KillRewardPLAYER.PlayerKillReward = 0;
                else
                {
                    MessageBox.Show("Please Enter either True or False");
                    e.Node.Text = e.Label = $"Player Kill Reward Enabled:- {(KillRewardPlayer_Config.KillRewardPLAYER.PlayerKillReward == 1 ? true : false)}";
                    return;
                }
                e.Node.Text = e.Label = $"Player Kill Reward Enabled:- {(KillRewardPlayer_Config.KillRewardPLAYER.PlayerKillReward == 1 ? true : false)}";
                KillRewardPlayer_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "PlayerKillPoints")
            {
                KillRewardPlayer_Config.KillRewardPLAYER.PlayerKillPoints = Convert.ToInt32(e.Label);
                e.Node.Text = e.Label = $"Player Kill points:- {KillRewardPlayer_Config.KillRewardPLAYER.PlayerKillPoints}";
                KillRewardPlayer_Config.isDirty = true;
            }
            else if (e.Node.Tag is Killreward_Kilss_Money_Weapons)
            {
                string[] info = e.Label.Split(':');
                string kills = info[0];
                string money = info[1];
                Killreward_Kilss_Money_Weapons krpw = e.Node.Tag as Killreward_Kilss_Money_Weapons;
                krpw.kills = Convert.ToInt32(kills);
                krpw.money = Convert.ToInt32(money);
                e.Node.Text = e.Label = $"{krpw.kills}:{krpw.money}";
                if (e.Node.Parent.Parent.Tag is KillRewardZombie_Config)
                    KillRewardZombie_Config.isDirty = true;
                else if(e.Node.Parent.Parent.Tag is KillReward_KillRewardPLAYERSettings)
                    KillRewardPlayer_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "SHOOTReward")
            {
                if (e.Label.ToLower() == "true")
                    KillRewardPlayer_Config.KillRewardSHOOTDISTANCE.SHOOTReward = 1;
                else if (e.Label.ToLower() == "false")
                    KillRewardPlayer_Config.KillRewardSHOOTDISTANCE.SHOOTReward = 0;
                else
                {
                    MessageBox.Show("Please Enter either True or False");
                    e.Node.Text = e.Label = $"Shot Ditance Reward Enabled:- {(KillRewardPlayer_Config.KillRewardSHOOTDISTANCE.SHOOTReward == 1 ? true : false)}";
                    return;
                }
                e.Node.Text = e.Label = $"Shot Ditance Reward Enabled:- {(KillRewardPlayer_Config.KillRewardSHOOTDISTANCE.SHOOTReward == 1 ? true : false)}";
                KillRewardPlayer_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "PlayerSHOOTDISTANCE")
            {
                KillRewardPlayer_Config.KillRewardSHOOTDISTANCE.PlayerSHOOTDISTANCE = Convert.ToInt32(e.Label);
                e.Node.Text = e.Label = $"Player Shot Distance:- {KillRewardPlayer_Config.KillRewardSHOOTDISTANCE.PlayerSHOOTDISTANCE}";
                KillRewardPlayer_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "BoxMagazineQuantity")
            {
                KillReward_KillRewardWEAPONBOXSettings krwbs = Currenttreeviewtag.Parent.Tag as KillReward_KillRewardWEAPONBOXSettings;
                krwbs.BoxMagazinQuantity = Convert.ToInt32(e.Label);
                e.Node.Text = e.Label = $"Box Magazine Quantity:- {krwbs.BoxMagazinQuantity}";
                KillRewardWeaponBox_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "ZombieKillRewardEnabled")
            {
                if (e.Label.ToLower() == "true")
                    KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillReward = 1;
                else if (e.Label.ToLower() == "false")
                    KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillReward = 0;
                else
                {
                    MessageBox.Show("Please Enter either True or False");
                    e.Node.Text = e.Label = $"Player Gift Enabled:- {(KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillReward == 1 ? true : false)}";
                    return;
                }
                e.Node.Text = e.Label = $"Player Gift Enabled:- {(KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillReward == 1 ? true : false)}";
                KillRewardZombie_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "ZombieKillPoints")
            {
                KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillPoints = Convert.ToInt32(e.Label);
                e.Node.Text = e.Label = $"Zombie Kill Points:- {KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillPoints}";
                KillRewardZombie_Config.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "ZombieKillsbyCarEnabled")
            {
                if (e.Label.ToLower() == "true")
                    KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillsbyCar = 1;
                else if (e.Label.ToLower() == "false")
                    KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillsbyCar = 0;
                else
                {
                    MessageBox.Show("Please Enter either True or False");
                    e.Node.Text = e.Label = $"Zombie Kill by Car Enabled:- {(KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillsbyCar == 1 ? true : false)}";
                    return;
                }
                e.Node.Text = e.Label = $"Zombie Kill by Car Enabled:- {(KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillsbyCar == 1 ? true : false)}";
                KillRewardZombie_Config.isDirty = true;
            }
        }
        private void KillRewardTV_RequestEditText(object sender, TreeViewMS.NodeRequestTextEventArgs e)
        {
            if (e.Node.Tag.ToString() == "SteamID")
            {
                e.Label = e.Node.Text.Substring(12);
            }
            if (e.Node.Tag.ToString() == "PlayerGiftEnabled")
            {
                e.Label = (KillRewardGift_Config.KillRewardGIFT.PlayerGift == 1 ? true : false).ToString();
            }
            if (e.Node.Tag.ToString() == "GiftLifetime")
            {
                e.Label = e.Node.Text.Substring(16);
            }
            else if (e.Node.Tag.ToString() == "HuntingKillRewardEnabled")
            {
                e.Label = (KillRewardHunting_Config.KillRewardHUNTING.HuntingReward == 1 ? true : false).ToString();
            }
            else if (e.Node.Tag.ToString() == "AnimalPoints")
            {
                e.Label = e.Node.Text.Substring(9);
            }
            else if (e.Node.Tag.ToString() == "PlayerKillNegativeReward")
            {
                e.Label = (KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeReward == 1 ? true : false).ToString();
            }
            else if (e.Node.Tag.ToString() == "PlayerKill")
            {
                e.Label = e.Node.Text.Substring(14);
            }
            else if (e.Node.Tag.ToString() == "Pointlose")
            {
                e.Label = e.Node.Text.Substring(13);
            }
            else if (e.Node.Tag.ToString() == "LoseMoney")
            {
                e.Label = e.Node.Text.Substring(13);
            }
            else if (e.Node.Tag.ToString() == "PlayerKillNegativeRewardBonusQuantity")
            {
                e.Label = e.Node.Text.Substring(45);
            }
            else if (e.Node.Tag.ToString() == "PlayerKillReward")
            {
                e.Label = (KillRewardPlayer_Config.KillRewardPLAYER.PlayerKillReward == 1 ? true : false).ToString();
            }
            else if (e.Node.Tag.ToString() == "PlayerKillPoints")
            {
                e.Label = e.Node.Text.Substring(21);
            }
            else if ( e.Node.Tag.ToString() == "SHOOTReward")
            {
                e.Label = (KillRewardPlayer_Config.KillRewardSHOOTDISTANCE.SHOOTReward == 1 ? true : false).ToString();
            }
            else if (e.Node.Tag.ToString() == "PlayerSHOOTDISTANCE")
            {
                e.Label = e.Node.Text.Substring(23);
            }
            else if (e.Node.Tag.ToString() == "BoxMagazineQuantity")
            {
                e.Label = e.Node.Text.Substring(24);
            }
            else if (e.Node.Tag.ToString() == "ZombieKillRewardEnabled")
            {
                e.Label = (KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillReward == 1 ? true : false).ToString();
            }
            else if (e.Node.Tag.ToString() == "ZombieKillPoints")
            {
                e.Label = KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillPoints.ToString();
            }
            else if (e.Node.Tag.ToString() == "ZombieKillsbyCarEnabled")
            {
                e.Label = (KillRewardZombie_Config.KillRewardZOMBIE.ZombieKillsbyCar == 1 ? true : false).ToString();
            }
        }
        private void KillRewardTV_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = _preventExpand;
            _preventExpand = false;
        }
        private void KillRewardTV_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = _preventExpand;
            _preventExpand = false;
        }
        private void KillRewardTV_MouseDown(object sender, MouseEventArgs e)
        {
            int delta = (int)DateTime.Now.Subtract(_lastMouseDown).TotalMilliseconds;
            _preventExpand = (delta < SystemInformation.DoubleClickTime);
            _lastMouseDown = DateTime.Now;
        }
 
        private void baseSettingsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CheckBox cb = sender as CheckBox;
            KillRewardBase_Config.KillRewardBASE.SetBoolValue(cb.Name.Substring(0, cb.Name.Length - 2), cb.Checked == true ? 1 : 0);
            KillRewardBase_Config.isDirty = true;
        }
        private void baseSettingsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TextBox tb = sender as TextBox;
            KillRewardBase_Config.KillRewardBASE.SetTextValue(tb.Name.Substring(0, tb.Name.Length - 2), tb.Text);
            KillRewardBase_Config.isDirty = true;
        }
        private void baseSettingsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NumericUpDown nud = sender as NumericUpDown;
            if (nud.Tag.ToString() == "float")
                KillRewardBase_Config.KillRewardBASE.SetDecimalValue(nud.Name.Substring(0, nud.Name.Length - 3), nud.Value);
            else if (nud.Tag.ToString() == "int")
                KillRewardBase_Config.KillRewardBASE.SetIntValue(nud.Name.Substring(0, nud.Name.Length - 3), (int)nud.Value);
            KillRewardBase_Config.isDirty = true;
        }
        private void baseSettingsComboBox_IndexChanged(object sender , EventArgs e)
        {
            if (!useraction) return;
            ComboBox nud = sender as ComboBox;
            KillRewardBase_Config.KillRewardBASE.SetIntValue(nud.Name.Substring(0, nud.Name.Length - 2), (int)nud.SelectedIndex);
            KillRewardBase_Config.isDirty = true;
        }
        private void addSteamIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Steamid = "0";
            TreeNode NewSteamNode = new TreeNode($"Steam ID: - {Steamid}")
            {
                Tag = "SteamID",
                Name = "SteamID"

            };
            Currenttreeviewtag.Nodes.Add(NewSteamNode);
            KillRewardBase_Config.KillRewardBASE.NotListedIDs.Add(Steamid);
            KillRewardBase_Config.isDirty = true;
            KillRewardTV.SelectedNode = NewSteamNode;
        }
        private void RemoveSteamIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string steamid = Currenttreeviewtag.Text.ToString().Substring(12);
            KillRewardBase_Config.KillRewardBASE.NotListedIDs.Remove(steamid);
            KillRewardBase_Config.isDirty = true;
            Currenttreeviewtag.Parent.Nodes.Remove(Currenttreeviewtag);
        }

        private void addNewGiftBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindingList<string> liststring = new BindingList<string>()
            {
                "FS_GiftBox_Small_1",
                "FS_GiftBox_Small_2",
                "FS_GiftBox_Small_3",
                "FS_GiftBox_Small_4",
                "FS_GiftBox_Medium_1",
                "FS_GiftBox_Medium_2",
                "FS_GiftBox_Medium_3",
                "FS_GiftBox_Medium_4",
                "FS_GiftBox_Large_1",
                "FS_GiftBox_Large_2",
                "FS_GiftBox_Large_3",
                "FS_GiftBox_Large_4",
                "FS_GiftBox_Hardigg"
            };
            AddfromPredefinedItems form = new AddfromPredefinedItems
            {
                Stringlist = liststring,
                titellabel = "Add Items from giftbox list",
                isLootList = false,
                isRHTableList = false,
                isRewardTable = false,
                ispredefinedweapon = false,
                isRHPredefinedWeapon = false,
                isLootchest = false,
                isLootBoxList = false,
                isUtopiaAirdroplootPools = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                 foreach (string box in form.ReturnList)
                {
                    if (!KillRewardGift_Config.KillRewardGIFT.Giftbox.Any(x => x == box))
                    {
                        KillRewardGift_Config.KillRewardGIFT.Giftbox.Add(box);
                        KillRewardGift_Config.isDirty = true;
                        TreeNode gbnode = new TreeNode($"Box Type: - {box}")
                        {
                            Tag = "BoxType",
                            Name = "BoxType"

                        };
                        Currenttreeviewtag.Nodes.Add(gbnode);
                        KillRewardTV.SelectedNode = gbnode;
                    }
                    else
                    {
                        MessageBox.Show("Gift Box is allready in the list.....");
                    }
                }
            }
        }
        private void removeGiftBoxoolStripMenuItem_Click(object sender, EventArgs e)
        {
            string GiftBox = Currenttreeviewtag.Text.ToString().Substring(12);
            KillRewardGift_Config.KillRewardGIFT.Giftbox.Remove(GiftBox);
            KillRewardGift_Config.isDirty = true;
            Currenttreeviewtag.Parent.Nodes.Remove(Currenttreeviewtag);
        }
        private void addDrinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!KillRewardGift_Config.KillRewardGIFT.Giftdrink.Any(x => x == l))
                    {
                        KillRewardGift_Config.KillRewardGIFT.Giftdrink.Add(l);
                        KillRewardGift_Config.isDirty = true;
                        TreeNode gdnode = new TreeNode($"Drink Type: - {l}")
                        {
                            Tag = "DrinkType",
                            Name = "DrinkType"

                        };
                        Currenttreeviewtag.Nodes.Add(gdnode);
                    }
                }
                KillRewardTV.SelectedNode = Currenttreeviewtag.LastNode;
            }
        }
        private void removeDrinkStripMenuItem1_Click(object sender, EventArgs e)
        {
            string drink = Currenttreeviewtag.Text.ToString().Substring(14);
            KillRewardGift_Config.KillRewardGIFT.Giftdrink.Remove(drink);
            KillRewardGift_Config.isDirty = true;
            Currenttreeviewtag.Parent.Nodes.Remove(Currenttreeviewtag);
        }
        private void addNewTreatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!KillRewardGift_Config.KillRewardGIFT.Gifteat.Any(x => x == l))
                    {
                        KillRewardGift_Config.KillRewardGIFT.Gifteat.Add(l);
                        KillRewardGift_Config.isDirty = true;
                        TreeNode gtnode = new TreeNode($"Food Type: - {l}")
                        {
                            Tag = "TreatType",
                            Name = "TreatType"

                        };
                        Currenttreeviewtag.Nodes.Add(gtnode);
                    }
                }
                KillRewardTV.SelectedNode = Currenttreeviewtag.LastNode;
            }
        }
        private void removeTreatStripMenuItem2_Click(object sender, EventArgs e)
        {
            string treat = Currenttreeviewtag.Text.ToString().Substring(13);
            KillRewardGift_Config.KillRewardGIFT.Gifteat.Remove(treat);
            KillRewardGift_Config.isDirty = true;
            Currenttreeviewtag.Parent.Nodes.Remove(Currenttreeviewtag);
        }
        private void addToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!KillRewardGift_Config.KillRewardGIFT.Gifttools.Any(x => x == l))
                    {
                        KillRewardGift_Config.KillRewardGIFT.Gifttools.Add(l);
                        KillRewardGift_Config.isDirty = true;
                        TreeNode gtnode = new TreeNode($"Tool Type: - {l}")
                        {
                            Tag = "Tooltype",
                            Name = "Tooltype"

                        };
                        Currenttreeviewtag.Nodes.Add(gtnode);
                    }
                }
                KillRewardTV.SelectedNode = Currenttreeviewtag.LastNode;
            }
        }
        private void removeToolToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            string tool = Currenttreeviewtag.Text.ToString().Substring(13);
            KillRewardGift_Config.KillRewardGIFT.Gifttools.Remove(tool);
            KillRewardGift_Config.isDirty = true;
            Currenttreeviewtag.Parent.Nodes.Remove(Currenttreeviewtag);
        }
        private void addNewMedicalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!KillRewardGift_Config.KillRewardGIFT.Gifttools.Any(x => x == l))
                    {
                        KillRewardGift_Config.KillRewardGIFT.Gifttools.Add(l);
                        KillRewardGift_Config.isDirty = true;
                        TreeNode gmnode = new TreeNode($"Medical Type: - {l}")
                        {
                            Tag = "MedicalType",
                            Name = "MedicalType"

                        };
                        Currenttreeviewtag.Nodes.Add(gmnode);
                    }
                }
                KillRewardTV.SelectedNode = Currenttreeviewtag.LastNode;
            }
        }
        private void removeMedicalToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            string medical = Currenttreeviewtag.Text.ToString().Substring(16);
            KillRewardGift_Config.KillRewardGIFT.Giftmedical.Remove(medical);
            KillRewardGift_Config.isDirty = true;
            Currenttreeviewtag.Parent.Nodes.Remove(Currenttreeviewtag);
        }

        private void addNewKillMoneyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnimalKillMoney newakm = new AnimalKillMoney()
            {
                kills = 0,
                money = 0,
            };
            (Currenttreeviewtag.Tag as KillReward_KillrewardHuntingAnimals).killsmoney.Add(newakm);
            TreeNode akill = new TreeNode($"{newakm.kills}:{newakm.money}")
            {
                Tag = newakm
            };
            Currenttreeviewtag.Nodes.Add(akill);
            KillRewardHunting_Config.isDirty = true;
        }
        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Currenttreeviewtag.Nodes.Clear();
            if (Currenttreeviewtag.Tag is KillReward_KillrewardHuntingAnimals)
            {
                var sortedListInstance = new BindingList<AnimalKillMoney>((Currenttreeviewtag.Tag as KillReward_KillrewardHuntingAnimals).killsmoney.OrderBy(x => x.kills).ToList());
                foreach (AnimalKillMoney akmall in sortedListInstance)
                {
                    TreeNode akill = new TreeNode($"{akmall.kills}:{akmall.money}")
                    {
                        Tag = akmall
                    };
                    Currenttreeviewtag.Nodes.Add(akill);
                }
            }
            else if (Currenttreeviewtag.Tag.ToString() == "Killreward_Player_Weapons")
            {
                var sortedListInstance = new BindingList<Killreward_Kilss_Money_Weapons>(KillRewardPlayer_Config.KillRewardPLAYER._playuerWeapons.OrderBy(x => x.kills).ToList());
                foreach (Killreward_Kilss_Money_Weapons KRpw in sortedListInstance)
                {
                    TreeNode treeNode = new TreeNode($"{KRpw.kills}:{KRpw.money}")
                    {
                        Tag = KRpw
                    };
                    if (KRpw.hasWeaponsBox)
                    {
                        treeNode.Nodes.Add(new TreeNode($"Player Weapon Box Number:- {KRpw.WeaponsBoxNumber}")
                        {
                            Tag = "PlayerWeaponBoxNumber",
                            Name = "PlayerWeaponBoxNumber"
                        });
                    }
                    Currenttreeviewtag.Nodes.Add(treeNode);
                }
            }
            else if (Currenttreeviewtag.Tag.ToString() == "Killreward_Zombie_Weapons")
            {
                var sortedListInstance = new BindingList<Killreward_Kilss_Money_Weapons>(KillRewardZombie_Config.KillRewardZOMBIE._ZombieWeapons.OrderBy(x => x.kills).ToList());
                foreach (Killreward_Kilss_Money_Weapons KRpw in sortedListInstance)
                {
                    TreeNode treeNode = new TreeNode($"{KRpw.kills}:{KRpw.money}")
                    {
                        Tag = KRpw
                    };
                    if (KRpw.hasWeaponsBox)
                    {
                        treeNode.Nodes.Add(new TreeNode($"Zombie Weapon Box Number:- {KRpw.WeaponsBoxNumber}")
                        {
                            Tag = "ZombieWeaponBoxNumber",
                            Name = "ZombieWeaponBoxNumber"
                        });
                    }
                    Currenttreeviewtag.Nodes.Add(treeNode);
                }
            }
        }
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (Currenttreeviewtag.Parent.Tag as KillReward_KillrewardHuntingAnimals).killsmoney.Remove(Currenttreeviewtag.Tag as AnimalKillMoney);
            KillRewardHunting_Config.isDirty = true;
            Currenttreeviewtag.Parent.Nodes.Remove(Currenttreeviewtag);
        }
        private void addWeaponBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addKillRewardWeaponBox addnewweaponbox = new addKillRewardWeaponBox()
            {
                WeaponBoxes = KillRewardWeaponBox_Config.KillRewardWEAPONBOX
            };
            DialogResult result = addnewweaponbox.ShowDialog();
            if (result == DialogResult.OK)
            {
                KillReward_KillRewardWEAPONBOXSettings newwb = addnewweaponbox.SelectedWeaponBox;
                Killreward_Kilss_Money_Weapons krpw = Currenttreeviewtag.Tag as Killreward_Kilss_Money_Weapons;
                krpw.hasWeaponsBox = true;
                krpw.WeaponsBoxNumber = newwb.BoxNumber;

                if (Currenttreeviewtag.Parent.Tag.ToString() == "Killreward_Zombie_Weapons")
                {
                    Currenttreeviewtag.Nodes.Add(new TreeNode($"Zombie Weapon Box Number:- {krpw.WeaponsBoxNumber}")
                    {
                        Tag = "ZombieWeaponBoxNumber",
                        Name = "ZombieWeaponBoxNumber"
                    });
                    KillRewardZombie_Config.isDirty = true;
                }
                else if (Currenttreeviewtag.Parent.Tag.ToString() == "Killreward_Player_Weapons")
                {

                    Currenttreeviewtag.Nodes.Add(new TreeNode($"Player Weapon Box Number:- {krpw.WeaponsBoxNumber}")
                    {
                        Tag = "PlayerWeaponBoxNumber",
                        Name = "PlayerWeaponBoxNumber"
                    });
                    KillRewardPlayer_Config.isDirty = true;
                }
            }

        }
        private void removeWeaponBoxToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Killreward_Kilss_Money_Weapons krpw = Currenttreeviewtag.Parent.Tag as Killreward_Kilss_Money_Weapons;
            krpw.hasWeaponsBox = false;
            krpw.WeaponsBoxNumber = 0;
            Currenttreeviewtag.Parent.Nodes.Remove(Currenttreeviewtag);
            if (Currenttreeviewtag.Parent.Tag.ToString() == "Killreward_Zombie_Weapons")
            {
                KillRewardZombie_Config.isDirty = true;
            }
            else if (Currenttreeviewtag.Parent.Tag.ToString() == "Killreward_Player_Weapons")
            {
                KillRewardPlayer_Config.isDirty = true;
            }
        }
        private void addNegativeKillRewardBonusToolStripMenuItem_Click(object sender, EventArgs e)
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
                    KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeRewardBonus.Add(l);
                    Currenttreeviewtag.Nodes.Add(new TreeNode($"Negative Reward Bonus Type:- {l}")
                    {
                        Tag = "negativeRewardBonusType",
                        Name = "negativeRewardBonusType"
                    });
                    KillRewardPlayer_Config.isDirty = true;
                }

            }
        }
        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            KillRewardPlayer_Config.KillRewardNEGATIVE.PlayerKillNegativeRewardBonus.Remove(Currenttreeviewtag.Text.Substring(29));
            KillRewardPlayer_Config.isDirty = true;
            Currenttreeviewtag.Parent.Nodes.Remove(Currenttreeviewtag);
        }
        private void addNewKillMoneyPlayerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Killreward_Kilss_Money_Weapons KRpw = new Killreward_Kilss_Money_Weapons()
            {
                kills = 0,
                money = 0,
                hasWeaponsBox = false,
                WeaponsBoxNumber = 0
            };
            KillRewardPlayer_Config.KillRewardPLAYER._playuerWeapons.Add(KRpw);
            TreeNode pkill = new TreeNode($"{KRpw.kills}:{KRpw.money}")
            {
                Tag = KRpw
            };
            Currenttreeviewtag.Nodes.Add(pkill);
            KillRewardPlayer_Config.isDirty = true;
        }
        private void removeKillMoneyPlayerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Currenttreeviewtag.Parent.Tag.ToString() == "Killreward_Player_Weapons")
            {
                KillRewardPlayer_Config.KillRewardPLAYER._playuerWeapons.Remove(Currenttreeviewtag.Tag as Killreward_Kilss_Money_Weapons);
                KillRewardPlayer_Config.isDirty = true;
            }
            else if (Currenttreeviewtag.Parent.Tag.ToString() == "Killreward_Zombie_Weapons")
            {
                KillRewardZombie_Config.KillRewardZOMBIE._ZombieWeapons.Remove(Currenttreeviewtag.Tag as Killreward_Kilss_Money_Weapons);
                KillRewardZombie_Config.isDirty = true;
            }
            Currenttreeviewtag.Parent.Nodes.Remove(Currenttreeviewtag);
        }

        private void addNewWeaponBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KillReward_KillRewardWEAPONBOXSettings krwb = new KillReward_KillRewardWEAPONBOXSettings()
            {
                BoxNumber = KillRewardWeaponBox_Config.Getnextboxnumber(),
                Box = "WoodenCrate",
                BoxWeapon ="",
                BoxWeaponBayonet ="",
                BoxWeaponButtstock ="",
                BoxWeaponHandguard ="",
                BoxWeaponsuppressor ="",
                BoxWeaponSight ="",
                BoxWeaponMagazin ="",
                BoxMagazinQuantity = 1,
                BoxBonus = new BindingList<string>() { "WeaponCleaningKit", "WeaponCleaningKit","WeaponCleaningKit" }
            };
            KillRewardWeaponBox_Config.KillRewardWEAPONBOX.Add(krwb);
            KillRewardWeaponBox_Config.isDirty = true;
            TreeNode weaponBoxNode = new TreeNode($"Weapon Box Number:- {krwb.BoxNumber}")
            {
                Tag = krwb
            };
            weaponBoxNode.Nodes.Add(new TreeNode($"Box Type:- {krwb.Box}")
            {
                Tag = "Box",
                Name = "Box"
            });
            weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon:- {krwb.BoxWeapon}")
            {
                Tag = "BoxWeapon",
                Name = "BoxWeapon"
            });
            weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon Bayonet:- {krwb.BoxWeaponBayonet}")
            {
                Tag = "BoxWeaponBayonet",
                Name = "BoxWeaponBayonet"
            });
            weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon Buttstock:- {krwb.BoxWeaponButtstock}")
            {
                Tag = "BoxWeaponButtstock",
                Name = "BoxWeaponButtstock"
            });
            weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon Handguard:- {krwb.BoxWeaponHandguard}")
            {
                Tag = "BoxWeaponHandguard",
                Name = "BoxWeaponHandguard"
            });
            weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon suppressor:- {krwb.BoxWeaponsuppressor}")
            {
                Tag = "BoxWeaponsuppressor",
                Name = "BoxWeaponsuppressor"
            });
            weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon Sight:- {krwb.BoxWeaponSight}")
            {
                Tag = "BoxWeaponSight",
                Name = "BoxWeaponSight"
            });
            weaponBoxNode.Nodes.Add(new TreeNode($"Box Weapon Magazine:- {krwb.BoxWeaponMagazin}")
            {
                Tag = "BoxWeaponMagazin",
                Name = "BoxWeaponMagazin"
            });
            weaponBoxNode.Nodes.Add(new TreeNode($"Box Magazin Quantity:- {krwb.BoxMagazinQuantity}")
            {
                Tag = "BoxMagazinQuantity",
                Name = "BoxMagazinQuantity"
            });
            TreeNode BoxBonusroot = new TreeNode($"NotListedIDs:-")
            {
                Tag = "BoxBonus",
                Name = "BoxBonus"
            };
            foreach (string BoxBonus in krwb.BoxBonus)
            {
                TreeNode boxbonusnode = new TreeNode($"Box Bonus Type: - {BoxBonus}")
                {
                    Tag = "BoxBonusType",
                    Name = "BoxBonusType"

                };
                BoxBonusroot.Nodes.Add(boxbonusnode);
            }
            weaponBoxNode.Nodes.Add(BoxBonusroot);
            Currenttreeviewtag.Nodes.Add(weaponBoxNode);
        }
        private void removeWeaponBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KillRewardWeaponBox_Config.KillRewardWEAPONBOX.Remove(Currenttreeviewtag.Tag as KillReward_KillRewardWEAPONBOXSettings);
            KillRewardWeaponBox_Config.isDirty = true;
            Currenttreeviewtag.Parent.Nodes.Remove(Currenttreeviewtag);
        }
        private void addNewBoxBonusToolStripMenuItem_Click(object sender, EventArgs e)
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
                KillReward_KillRewardWEAPONBOXSettings krwbs = Currenttreeviewtag.Parent.Tag as KillReward_KillRewardWEAPONBOXSettings;
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    krwbs.BoxBonus.Add(l);
                    TreeNode boxbonusnode = new TreeNode($"Box Bonus Type: - {l}")
                    {
                        Tag = "BoxBonusType",
                        Name = "BoxBonusType"

                    };
                    Currenttreeviewtag.Nodes.Add(boxbonusnode);
                    KillRewardWeaponBox_Config.isDirty = true;
                }

            }
        }
        private void removeBoxBonusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KillReward_KillRewardWEAPONBOXSettings krwbs = Currenttreeviewtag.Parent.Parent.Tag as KillReward_KillRewardWEAPONBOXSettings;
            krwbs.BoxBonus.Remove(Currenttreeviewtag.Text.Substring(18));
            KillRewardWeaponBox_Config.isDirty = true;
            Currenttreeviewtag.Parent.Nodes.Remove(Currenttreeviewtag);
        }
        private void addNewKillMoneyZombieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Killreward_Kilss_Money_Weapons KRpw = new Killreward_Kilss_Money_Weapons()
            {
                kills = 0,
                money = 0,
                hasWeaponsBox = false,
                WeaponsBoxNumber = 0
            };
            KillRewardZombie_Config.KillRewardZOMBIE._ZombieWeapons.Add(KRpw);
            TreeNode pkill = new TreeNode($"{KRpw.kills}:{KRpw.money}")
            {
                Tag = KRpw
            };
            Currenttreeviewtag.Nodes.Add(pkill);
            KillRewardZombie_Config.isDirty = true;
        }

       }
}
