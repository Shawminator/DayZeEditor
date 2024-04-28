using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class DrJonesObjects
    {
        public string Filename { get; set; }
        public string path { get; set; }
        public bool isDirty { get; set; }

        public BindingList<DrJonesObject> DrJonesobjectslist { get; set; }

        public DrJonesObjects(string filename)
        {
            Filename = filename;
            path = Path.GetDirectoryName(filename);
            isDirty = false;

            DrJonesobjectslist = new BindingList<DrJonesObject>();

            FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader reader = new StreamReader(fs);
            string line_content = "";
            Console.WriteLine("trader Objects Found, Begin Parsing......");
            int markerCounter = 0;
            bool skipDirEntry = false;

            while (markerCounter <= 5000 && line_content.Contains("<FileEnd>") == false)
            {
                // Get Object Type ------------------------------------------------------------------------------------
                if (skipDirEntry)
                    skipDirEntry = false;
                else
                    line_content = Helper.SearchForNextTermInFile(reader, "<Object>", "<FileEnd>");

                if (!line_content.Contains("<Object>"))
                    break;
                
               

                line_content = line_content.Replace("<Object>", "");
                line_content = Helper.TrimComment(line_content);
                line_content = Helper.TrimSpaces(line_content);
                string traderObjectType = line_content;
                line_content = Helper.SearchForNextTermInFile(reader, "<ObjectPosition>", "<FileEnd>");

                line_content = line_content.Replace("<ObjectPosition>", "");
                line_content = Helper.TrimComment(line_content);

                string[] strso = line_content.Split(',');

                string traderObjectPosX = strso[0];
                traderObjectPosX = Helper.TrimSpaces(traderObjectPosX);

                string traderObjectPosY = strso[1];
                traderObjectPosY = Helper.TrimSpaces(traderObjectPosY);

                string traderObjectPosZ = strso[2];
                traderObjectPosZ = Helper.TrimSpaces(traderObjectPosZ);

                line_content = Helper.SearchForNextTermInFile(reader, "<ObjectOrientation>", "<FileEnd>");

                line_content =  line_content.Replace("<ObjectOrientation>", "");
                line_content = Helper.TrimComment(line_content);

                string[] strsod = line_content.Split(',');

                string traderObjectOriX = strsod[0];
                traderObjectOriX = Helper.TrimSpaces(traderObjectOriX);

                string traderObjectOriY = strsod[1];
                traderObjectOriY = Helper.TrimSpaces(traderObjectOriY);

                string traderObjectOriZ = strsod[2];
                traderObjectOriZ = Helper.TrimSpaces(traderObjectOriZ);


                int attachmentCounter = 0;
                List<string> Attchments = new List<string>();
                while (attachmentCounter <= 1000 && line_content.Contains("<Object>") == false)
                {
                    line_content = Helper.SearchForNextTermsInFile(reader, new string[] { "<ObjectAttachment>", "<OpenFile>" }, "<Object>");
                    if (line_content == string.Empty || line_content == "")
                    {
                        line_content = "<FileEnd>";
                        break;
                    }
                    //if (line_content.Contains("<OpenFile>"))
                    //{
                    //    if (OpenNewFileForReading(line_content, file_index))
                    //        continue;
                    //    else
                    //        return;
                    //}
                    if (line_content.Contains("<Object>"))
                    {
                        skipDirEntry = true;
                        markerCounter++;
                        break;
                    }
                    
                    line_content = line_content.Replace("<ObjectAttachment>", "");
                    line_content = Helper.TrimComment(line_content);

                    if (line_content == "NPC_DUMMY")
                    {
                        break;
                    }
                    else
                    {
                        Attchments.Add(line_content);
                    }
                     

                    attachmentCounter++;
                }

                DrJonesObject newobject = new DrJonesObject();
                newobject.DrJonesNPCClassname = traderObjectType;
                newobject.DrJonesNPCPosition = new string[] { traderObjectPosX, traderObjectPosY, traderObjectPosZ };
                newobject.DrJonesNPCOrientaion = new string[] { traderObjectOriX, traderObjectOriY, traderObjectOriZ };
                newobject.DrJonesAttchments = new BindingList<string>(Attchments);
                DrJonesobjectslist.Add(newobject);
            }
        }
    }
    public class DrJonesObject
    {
        public string DrJonesNPCClassname { get; set; }
        public string[] DrJonesNPCPosition { get; set; }
        public string[] DrJonesNPCOrientaion { get; set; }
        public BindingList<string> DrJonesAttchments { get; set; }

    }
}
