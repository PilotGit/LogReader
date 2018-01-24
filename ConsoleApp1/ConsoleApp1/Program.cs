using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;

namespace ConsoleApp1
{
    [Serializable]
    public class Fw16Log
    {
        [XmlAttribute("dt-start")]
        public string dateStart { get; set; }
        [XmlAttribute("dt-end")]
        public string dateEnd { get; set; }
        [XmlAttribute("error-count")]
        public ushort error { get; set; }
        public Environment Environment { get; set; }

        public Fw16Log() { }
    }
        [Serializable]
        public class Environment
        {
            public Fw16 Fw16 { get; set; }
            public Ecr Ecr { get; set; }

            public Environment()
            {
                Fw16 = new Fw16();
            }
        }

            [Serializable]
            public class Fw16
            {
                [XmlAttribute("version")]
                public string version { get; set; }
                public string path { get; set; }

                public Fw16()
                {
                    version = "notSet";
                }
            }

            [Serializable]
            public class Ecr
            {

            }
            
        [Serializable]
        public class Shift
        {

        }

            [Serializable]
            public class Documents
            {

            }

                [Serializable]
                public class Receipt
                {

                }

                    [Serializable]
                    public class Error
                    {

                    }

            [Serializable]
            public class Summary
            {

            }

                [Serializable]
                public class Income
                {

                }

                [Serializable]
                public class IncomeBack
                {

                }


    class Program
    {
        static string nameXML=@"save/newxml.xml";                                                                                     //Имя нового файла

        /// <summary>
        /// Cоздание нового имени файла по имени полученного файла + ".XML"
        /// </summary>
        static void NameOfNewXML(string file,string directory="")                                                                                 //Получение нового имени базовым именем NewXML
        {
            try
            {
                File.OpenRead(file);
                nameXML = file;
                Console.Write(nameXML = @directory + file +".xml");
            }
            catch { Console.WriteLine("файл не существует"); }
            
        }

        
        static void Main(string[] args)
        {
            string directory;
            Console.Write("Укажите путь к файлу: ");
            directory = Console.ReadLine();
            if (directory == "def")                                                                 //Тестовый вариант пути к логам
            {
                try
                {
                    File.Delete("C:\\Users\\Gorionov\\source\\repos\\LogReader\\ConsoleApp1\\ConsoleApp1\\bin\\Debug\\save\\newxml.xml.xml");
                }
                catch { }
                NameOfNewXML("C:\\Users\\Gorionov\\source\\repos\\LogReader\\ConsoleApp1\\ConsoleApp1\\bin\\Debug\\save\\newxml.xml");
                directory = "C:\\Users\\Gorionov\\source\\repos\\LogReader\\ConsoleApp1\\ConsoleApp1\\bin\\Debug\\save\\newxml.xml";
            }
            else
            {
                NameOfNewXML(directory);
            }

            Fw16Log fw16Log = new Fw16Log();
            DateTime dateTime =  DateTime.Now;
            fw16Log.dateEnd = dateTime.ToString();
            fw16Log.Environment = new Environment();
            XmlSerializer formatter = new XmlSerializer(typeof(Fw16Log));

            using (FileStream fs = new FileStream(nameXML, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, fw16Log);
            }

            Console.ReadKey();
        }
    }
}


/*XmlDocument XMLdoc;
static string nameXML;

static void CreatXML()                                                                                 //создание xml файла с базовым именем NewXML
{
    int count;
    nameXML = Directory.GetFiles(@"save\", "newXML*").Last();                                          //cчитывание номера последного элемента с базовым именем
    count = 1 + Convert.ToInt32(Regex.Replace(nameXML, "[^0-9]+", string.Empty));
    nameXML = @"save\" + "newXML " + (count) + ".xml";
    XmlTextWriter textWritter = new XmlTextWriter(nameXML, Encoding.UTF8);
    textWritter.WriteStartDocument();
    textWritter.Close();
}
static void Main(string[] args)
{
    try
    {
        CreatXML();
    }
    catch (Exception ex) { Console.WriteLine(ex.Message); }

    Console.ReadKey();
}*/

