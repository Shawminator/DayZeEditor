using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace DayZeLib
{

    public class DZE
    {
        [JsonIgnore]
        const int Version = 8;

        public string MapName { get; set; }
        public string AuthorID { get; set; }
        public string CreditsID { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime CreationDate { get; set; }
        public float[] CameraPosition { get; set; }
        public BindingList<Editorobject> EditorObjects { get; set; }
        public BindingList<Editordeletedobject> EditorHiddenObjects { get; set; }
        public BindingList<EditorCameraTrackData> CameraTracks { get; set; }

        public DZE()
        {
            EditorObjects = new BindingList<Editorobject>();
            EditorHiddenObjects = new BindingList<Editordeletedobject>();
        }
        public DZE(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                int Length = br.ReadInt32();
                string bincheck = Helper.ReadCString(br, 12);
                int fileversion = br.ReadInt32();
                if (fileversion > Version)
                {
                    MessageBox.Show("The version number of the this dze file Newer\nPlease let me know to update the DayZeEditor");
                    return;
                }
                MapName = Helper.ReadCString(br, br.ReadInt32());
                int loop = br.ReadInt32();
                CameraPosition = new float[3];
                for (int i = 0; i < loop; i++)
                {
                    CameraPosition[i] = (float)br.ReadSingle();
                }
                AuthorID = Helper.ReadCString(br, br.ReadInt32());
                int creditscount = br.ReadInt32();
                for (int i = 0; i < creditscount; i++)
                {
                    CreditsID = Helper.ReadCString(br, br.ReadInt32());
                }
                LastModified = new DateTime(br.ReadInt32());
                CreationDate = new DateTime(br.ReadInt32());
                int EditorobjectCount = br.ReadInt32();
                EditorObjects = new BindingList<Editorobject>();
                for (int j = 0; j < EditorobjectCount; j++)
                {
                    Editorobject obj = new Editorobject(br, fileversion);
                    obj.Uuid = "";
                    obj.m_Id = j;
                    EditorObjects.Add(obj);
                }
                int EditorDeletedObjectsCount = br.ReadInt32();
                EditorHiddenObjects = new BindingList<Editordeletedobject>();
                for (int j = 0; j < EditorDeletedObjectsCount; j++)
                {
                    EditorHiddenObjects.Add(new Editordeletedobject(br));
                }
            }
        }

        public ObjectSpawnerArr convertToObjectSpawner()
        {
            ObjectSpawnerArr newobjectspawner = new ObjectSpawnerArr();
            newobjectspawner.Objects = new BindingList<SpawnObjects>();
            foreach(Editorobject obj in EditorObjects)
            {
                SpawnObjects newobject = new SpawnObjects();
                newobject.name = obj.Type;
                newobject.pos = obj.Position;
                newobject.ypr = obj.Orientation;
                newobject.scale = obj.Scale;
                newobject.enableCEPersistency = false;
                newobjectspawner.Objects.Add(newobject);
            }
            return newobjectspawner;
        }
    }

    public class Editorobject
    {
        public string Uuid { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public float[] Position { get; set; }
        public float[] Orientation { get; set; }
        public float Scale { get; set; }
        public string Model { get; set; }
        public int Flags { get; set; }
        public int m_Id { get; set; }

        [JsonIgnore]
        public bool EditorOnly { get; set; }
        [JsonIgnore]
        public bool Locked { get; set; }
        [JsonIgnore]
        public bool AllowDamage { get; set; }
        [JsonIgnore]
        public bool Simulate { get; set; }
        [JsonIgnore]
        public Dictionary<int, Editorobject> Attachments { get; set; }
        [JsonIgnore]
        public int attachments_count { get; set; }
        [JsonIgnore]
        public int params_count { get; set; }




        public Editorobject()
        {
            Uuid = "";
            Scale = 1;
            Model = "";
            Flags = 2147483647;
        }
        public Editorobject(BinaryReader br, int fileversion)
        {
            Type = Helper.ReadCString(br, br.ReadInt32());
            DisplayName = Helper.ReadCString(br, br.ReadInt32());
            int loop = br.ReadInt32();
            Position = new float[3];
            for (int i = 0; i < loop; i++)
            {
                Position[i] = br.ReadSingle();
            }
            loop = br.ReadInt32();
            Orientation = new float[3];
            for (int i = 0; i < loop; i++)
            {
                Orientation[i] = br.ReadSingle();
            }
            Scale = br.ReadSingle();
            Flags = br.ReadInt32();

            if (fileversion < 2)
                return;

            //if (fileversion < 8)
            //{
            //    attachments_count = br.ReadInt32();
            //    Attachments = new List<string>();
            //    for (int k = 0; k < attachments_count; k++)
            //    {
            //        Attachments.Add(Helper.ReadCString(br, br.ReadInt32()));
            //    }
            //}

            params_count = br.ReadInt32();
            for (int k = 0; k < params_count; k++)
            {
                string param_key = Helper.ReadCString(br, br.ReadInt32());
                string param_type = Helper.ReadCString(br, br.ReadInt32());
                switch (param_type)
                {
                    case "SerializableParam1<bool>":
                        bool b = br.ReadInt32() == 1 ? true : false;
                        break;
                    case "SerializableParam1<int>":
                        int i = br.ReadInt32();
                        break;
                    case "SerializableParam1<float>":
                        float f = br.ReadSingle();
                        break;
                    default:
                        break;
                }


            }

            if (fileversion < 3)
                return;

            EditorOnly = br.ReadInt32() == 1 ? true : false;
            Locked = br.ReadInt32() == 1 ? true : false;
            AllowDamage = br.ReadInt32() == 1 ? true : false;
            Simulate = br.ReadInt32() == 1 ? true : false;

            if (fileversion < 5)
                return;

            Model = Helper.ReadCString(br, br.ReadInt32());
            long pos = br.BaseStream.Position;

            attachments_count = br.ReadInt32();
            Attachments = new Dictionary<int, Editorobject>();
            for (int k = 0; k < attachments_count; k++)
            {
                int slotid = br.ReadInt32();
                Attachments.Add(slotid, new Editorobject(br, fileversion));
            }
        }
    }

    public class Editordeletedobject
    {
        public string Type { get; set; }
        public string ModelName { get; set; }
        public float[] Position { get; set; }
        public int Flags { get; set; }

        public Editordeletedobject()
        {

        }
        public Editordeletedobject(BinaryReader br)
        {
            Type = Helper.ReadCString(br, br.ReadInt32());
            int loop = br.ReadInt32();
            Position = new float[3];
            for (int i = 0; i < loop; i++)
            {
                Position[i] = br.ReadSingle();
            }
            Flags = br.ReadInt32();
            ModelName = Helper.ReadCString(br, br.ReadInt32());
        }
    }
    public class EditorCameraTrackData
    {

    }
}
