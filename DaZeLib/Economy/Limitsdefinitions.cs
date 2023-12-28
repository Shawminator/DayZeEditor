using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
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
            Console.Write("serializing " + Path.GetFileName(Filename));
            // To read the file, create a FileStream.
            try
            {
                using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    // Call the Deserialize method and cast to the object type.
                    lists = (lists)mySerializer.Deserialize(myFileStream);
                    if (lists != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Failed....");
                Console.ForegroundColor = ConsoleColor.White;
                var form = Application.OpenForms["SplashForm"];
                if (form != null)
                {
                    form.Invoke(new Action(() => { form.Close(); }));
                }
                MessageBox.Show("Error in " + Path.GetFileName(Filename) + "\n" + ex.Message.ToString() + "\n" + ex.InnerException.Message.ToString());
                Console.WriteLine("Error in " + Path.GetFileName(Filename) + "\n" + ex.Message.ToString() + "\n" + ex.InnerException.Message.ToString());
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
            Console.Write("serializing " + Path.GetFileName(Filename));
            // To read the file, create a FileStream.
            try
            {
                using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    // Call the Deserialize method and cast to the object type.
                    user_lists = (user_lists)mySerializer.Deserialize(myFileStream);
                    if (user_lists != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Failed....");
                Console.ForegroundColor = ConsoleColor.White;
                var form = Application.OpenForms["SplashForm"];
                if (form != null)
                {
                    form.Invoke(new Action(() => { form.Close(); }));
                }
                MessageBox.Show("Error in " + Path.GetFileName(Filename) + "\n" + ex.Message.ToString() + "\n" + ex.InnerException.Message.ToString());
                Console.WriteLine("Error in " + Path.GetFileName(Filename) + "\n" + ex.Message.ToString() + "\n" + ex.InnerException.Message.ToString());
            }
        }
    }
}

