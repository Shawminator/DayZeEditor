using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace DayZeLib
{
    public class PlayerDB
    {
        public BindingList<DBPlayers> PlayerList { get; set; }

        public PlayerDB(string dbpath)
        {

            PlayerList = new BindingList<DBPlayers>();
            using (var connection = new SQLiteConnection($"Data Source={dbpath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Players";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PlayerList.Add(new DBPlayers(reader.GetInt32(0), reader.GetBoolean(1), reader.GetString(2), reader["Data"] as byte[]));
                    }
                }
            }
        }
    }


    public class DBPlayers
    {
        public int DBID { get; }
        public bool isAlive { get; }
        public string UID { get; }
        public byte[] PlayerdataBlob { get; }

        public DBPlayerdata playerdata { get; set; }

        public DBPlayers(int v1, bool v2, string v3, byte[] bytes)
        {
            DBID = v1;
            isAlive = v2;
            UID = v3;
            PlayerdataBlob = bytes;
            playerdata = new DBPlayerdata(PlayerdataBlob);
            
        }
    }

    public class DBPlayerdata
    {
        //datablob decoding
        public int HeaderBlobLength { get; set; }
        public byte[] Headerblob { get; set; }
        public int CharacterNameLength { get; set; }
        public string CharacterName { get; set; }
        public int StatsblobLength { get; set; }
        public byte[] Statsblob { get; set; }
        public int InventoryItemCount { get; set; }
        public List<byte[]> InventoryItemsblobs { get; set; }

        public DBStats dBStats { get; set; }
        public List<DBItem> InventoryItems { get; set; }


        public DBPlayerdata(byte[] playerdatablob)
        {
            InventoryItemsblobs = new List<byte[]>();
            InventoryItems = new List<DBItem>();
            using (MemoryStream ms = new MemoryStream(playerdatablob))
            using (BinaryReader br = new BinaryReader(ms))
            {
                HeaderBlobLength = 16;
                Headerblob = br.ReadBytes(HeaderBlobLength);
                CharacterNameLength = br.ReadByte();
                CharacterName = Helper.ReadCString(br, CharacterNameLength);
                StatsblobLength = br.ReadInt32();
                Statsblob = br.ReadBytes(StatsblobLength);

                InventoryItemCount = br.ReadInt32();  //number of items 
                for (int i = 0; i < InventoryItemCount; i++)
                {
                    InventoryItemsblobs.Add(br.ReadBytes(br.ReadInt32()));
                }
            }
            dBStats = new DBStats(Statsblob);
            for(int i = 0;i < InventoryItemsblobs.Count;i++)
            {
                InventoryItems.Add(new DBItem(InventoryItemsblobs[i]));
            }
        }
    }

    public class DBStats
    {
        public int ServerStatBlobLength { get; set; }
        public byte[] ServerStatsblob { get; set; }
        public byte[] Otherstats { get; set; }
        public byte[] laststats { get; set; }

        public ServerStats ServerStats { get; set; }

        public DBStats(byte[] statsblob)
        {
            using (MemoryStream ms = new MemoryStream(statsblob))
            using (BinaryReader br = new BinaryReader(ms))
            {
                ServerStatBlobLength = br.ReadInt32();
                ServerStatsblob = br.ReadBytes(ServerStatBlobLength);
                int something = br.ReadInt32();
                string Guid = new Guid(br.ReadBytes(16)).ToString().ToUpper();
                Otherstats = br.ReadBytes(br.ReadInt32());
                laststats = br.ReadBytes(br.ReadInt32());
            }
            ServerStats = new ServerStats(ServerStatsblob);
        }
    }

    public class ServerStats
    {
        public List<ServerStat> serverStats { get; set; }
        public int SScount { get; set; }

        public ServerStats(byte[] serverstatsblob)
        {
            serverStats = new List<ServerStat>();
            using (MemoryStream ms = new MemoryStream(serverstatsblob))
            using (BinaryReader br = new BinaryReader(ms))
            {
                br.ReadInt16(); // two bytes 00 00
                SScount = br.ReadInt16();
                for (int i = 0; i < SScount; i++)
                {
                    serverStats.Add(new ServerStat(br));
                }
            }
        }
    }
    public class ServerStat
    {
        public string name { get; set; }
        public float Value { get; set; }

        public ServerStat(BinaryReader br) 
        {
            name = Helper.ReadCString(br, br.ReadByte()); 
            Value = br.ReadSingle();
        }
    }
    public class DBItem
    {

        public List<byte[]> ChildItemsblobs { get; set; }


        public string Classname { get; set; }
        public byte[] Somedata { get; set; }
        public string Slot { get; set; }
        public int itemDataBlobLength {get;set;}
        public byte[] ItemDataBlob { get; set; }
        
        public List<DBItem> Children { get; set; }
        public DBItemData DBItemData { get; set; }

        public DBItem(byte[] itemblob)
        {
            Children = new List<DBItem>();
            ChildItemsblobs = new List<byte[]>();

            using (MemoryStream ms = new MemoryStream(itemblob))
            using (BinaryReader br = new BinaryReader(ms))
            {
                Classname = Helper.ReadCString(br, br.ReadByte());
                Somedata = br.ReadBytes(6); //always 6 bytes 
                Slot = Helper.ReadCString(br, br.ReadByte());
                itemDataBlobLength = br.ReadInt32();// length of internal blob including ItemGUID and possible specific data.
                ItemDataBlob = br.ReadBytes(itemDataBlobLength);
                var ChildCount = br.ReadUInt32();
                for (var i = 0; i < ChildCount; i++)
                {
                    int childlen = br.ReadInt32();
                    ChildItemsblobs.Add(br.ReadBytes(childlen));
                }
            }
            for (var i = 0;i < ChildItemsblobs.Count;i++)
            {
                Children.Add(new DBItem(ChildItemsblobs[i]));
            }
            DBItemData = new DBItemData(ItemDataBlob);
        }
    }
    public class DBItemData
    {
        public string PersistentGuid { get; set; }
        public int colourcount { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int datalengthagain { get; set; }
        public byte[] specificitemdataperhaps { get; set; }

        public DBItemData(byte[] itemdatablob)
        {
            using (MemoryStream ms = new MemoryStream(itemdatablob))
            using (BinaryReader br = new BinaryReader(ms))
            {
                PersistentGuid = new Guid(br.ReadBytes(16)).ToString().ToUpper();
                colourcount = br.ReadInt32();
                //above count = 3, could be 3 bytes, could be colour?
                for (int i = 0; i < colourcount; i++)
                {
                    br.ReadByte();
                }
                datalengthagain = br.ReadInt32();
                specificitemdataperhaps = br.ReadBytes(datalengthagain);
            }
        }
    }
}
