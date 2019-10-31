using System;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
namespace hkxPoser
{
    public class Settings
    {
        Size _clientsize = new Size();
        public Size ClientSize
        {
            get
            {
                return _clientsize;
            }
            set
            {
                if (value.Width < 400 || value.Height < 400)
                {
                    value.Width = 400;
                    value.Height = 400;
             
                }

                _clientsize = value;
            }
        }
        public SharpDX.Color ScreenColor { get; set; }

        public bool Repeat { get; set; }

        public bool BoneViewing { get; set; }

        public bool SingleLaunch { get; set; }

        public static Settings Default
        {
            get
            {
                Settings settings = new Settings();
                settings.ClientSize = new Size(640, 640);
                settings.ScreenColor = new SharpDX.Color(192, 192, 192, 255);
                settings.Repeat = false;
                settings.BoneViewing = true;
                settings.SingleLaunch = false;
                return settings;
            }
        }


        public void Dump()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            FileStream outputFile = File.Create(Path.Combine(Application.StartupPath, @"config.xml"));
            XmlWriter writer = XmlWriter.Create(outputFile, settings);
            Console.Out.WriteLine("Changed Config.xml Saved!");
            //Console.Out.WriteLine("================ Config Save Dump Start ================");
            //XmlWriter outlog = XmlWriter.Create(Console.Out, settings);
            //serializer.Serialize(outlog, this);
            serializer.Serialize(writer, this);
            //outlog.Close();
            writer.Close();
            outputFile.Close();
           // Console.Out.WriteLine("\n================ Config Save Dump End ================");
        }

        public static Settings Load(string path)
        {
            if (!File.Exists(Path.Combine(Application.StartupPath, @"config.xml")))
                Default.Dump();

            XmlReader reader = XmlReader.Create(path);
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            Settings settings = serializer.Deserialize(reader) as Settings;
            reader.Close();

            return settings;
        }
    }
}
