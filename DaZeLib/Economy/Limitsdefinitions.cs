using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DayZeLib
{
    public class Limitsdefinitions
    {
        public lists lists { get; set; }
        public string Filename { get; set; }
        public Limitsdefinitions(string filename)
        {
            Filename = filename;
            var mySerializer = new XmlSerializer(typeof(lists));
            // To read the file, create a FileStream.
            using (var myFileStream = new FileStream(filename, FileMode.Open))
            {
                // Call the Deserialize method and cast to the object type.
                lists = (lists)mySerializer.Deserialize(myFileStream);
            }
        }
    }
    public class Limitsdefinitionsuser
    {
        public user_lists user_lists { get; set; }
        public string Filename { get; set; }
        public Limitsdefinitionsuser(string filename)
        {
            Filename = filename;
            var mySerializer = new XmlSerializer(typeof(user_lists));
            // To read the file, create a FileStream.
            using (var myFileStream = new FileStream(filename, FileMode.Open))
            {
                // Call the Deserialize method and cast to the object type.
                user_lists = (user_lists)mySerializer.Deserialize(myFileStream);
            }
        }
    }
    public class cfgplayerspawnpoints
    {
        public playerspawnpoints playerspawnpoints { get; set; }
        public string Filename { get; set; }
        public cfgplayerspawnpoints(string filename)
        {
            Filename = filename;
            var mySerializer = new XmlSerializer(typeof(playerspawnpoints));
            // To read the file, create a FileStream.
            using (var myFileStream = new FileStream(filename, FileMode.Open))
            {
                try
                {
                    // Call the Deserialize method and cast to the object type.
                    playerspawnpoints = (playerspawnpoints)mySerializer.Deserialize(myFileStream);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}

