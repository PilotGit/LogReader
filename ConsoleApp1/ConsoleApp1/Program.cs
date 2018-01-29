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
    /// <summary>
    /// добовление сообщения об ошибки
    /// </summary>
    [Serializable]  
    public class Error
    {
        /// <summary>
        /// время ошибки
        /// </summary>
        [XmlAttribute("dt")]
        public string dt;
        /// <summary>
        /// Команда вызвовшая ошибку
        /// </summary>
        [XmlAttribute("cmd")]
        public string cmd;
        /// <summary>
        /// Код ошибки
        /// </summary>
        [XmlAttribute("code")]
        public string code;
        /// <summary>
        /// Текст ошибки
        /// </summary>
        [XmlAttribute("text")]
        public string text;
        /// <summary>
        /// дополнительная информация об ошибке 
        /// </summary>
        public string Message;
    }
    /// <summary>
    /// Класс созданный только ради наследования ^-^
    /// </summary>
    public class Errors
    {
        /// <summary>
        /// добовление сообщения об ошибки
        /// </summary>
        [XmlElement]
        public List<Error> Error;

        //созданеи Сообщения об ошибке
        private ushort nExtError = 0;
        private bool flagError = false;
        /// <summary>
        /// создание нового сообщения об ошибке
        /// </summary>
        /// <returns>объект ошибки</returns>
        public Error NExtError()
        {
            if (!flagError) { flagError = true; Error = new List<Error>(); }
            Error.Add(new Error());
            nExtError++;
            return Error[nExtError - 1];
        }
        /// <summary>
        /// получение текущего объекта ошибки
        /// </summary>
        /// <returns>объект ошибки</returns>
        public Error currentError()
        {
            return Error[nExtError - 1];
        }


    }
    /// <summary>
    /// Корневой тег. Содержит в себе: атрибуты начало и конец ведения лога, кол-во ошибок, тег информации о ККТ, массив тегов рабочих сменн
    /// </summary>
    [Serializable]
    public class Fw16Log 
    {
        /// <summary>
        /// Атрибут значенея времени начала лога 
        /// </summary>
        [XmlAttribute("dt-start")]
        public string dateStart { get; set; }
        /// <summary>
        /// Атрибут значения времени конца лога
        /// </summary>
        [XmlAttribute("dt-end")]
        public string dateEnd { get; set; }
        /// <summary>
        /// Атрибут суммы ошибок
        /// </summary>
        [XmlAttribute("error-count")]
        public ushort errorCount { get; set; }
        /// <summary>
        /// Тег состояния ККТ
        /// </summary>
        public Environment Environment = new Environment();
        /// <summary>
        /// Начало смены содержит атрибуты начала и конца смены; нельзя использовать 
        /// </summary>
        [XmlElement]
        public List<Shift> Shift;
        /// <summary>
        /// добовление сообщения об ошибки
        /// </summary>
        public List<Error> Error;

        //созданеи Сообщения об ошибке
        private ushort nExtError = 0;
        private bool flagError = false;
        /// <summary>
        /// создание нового сообщения об ошибке
        /// </summary>
        /// <returns>объект ошибки</returns>
        public Error NExtError()
        {
            if (!flagError) { flagError = true; Error = new List<Error>(); }
            Error.Add(new Error());
            nExtError++;
            return Error[nExtError - 1];
        }
        /// <summary>
        /// получение текущего объекта ошибки
        /// </summary>
        /// <returns>объект ошибки</returns>
        public Error currentError()
        {
            return Error[nExtError - 1];
        }



        //созданеи смены
        private ushort nExt=0;
        private bool flagShift = false;
        /// <summary>
        /// создание новой смены
        /// </summary>
        /// <returns>объект смены</returns>
        public Shift NExtShift()
        {
            if (!flagShift) { flagShift = true; Shift = new List<Shift>(); }
            Shift.Add(new Shift());
            nExt++;
            return Shift[nExt - 1];
        }
        /// <summary>
        /// получение текущего объекта смены
        /// </summary>
        /// <returns>объект смены</returns>
        public Shift currentShift()
        {
           return Shift[nExt - 1];
        }

        public Fw16Log() { }
    }

    /// <summary>
    /// Тег состояния ККТ Содержит теги Fw16 и Ecr
    /// </summary>
    /// <remarks>Fw16 -содержание версии и расположение dll</remarks>
    /// <remarks>Ecr -содержание Модели и firmware </remarks>
    [Serializable]
    public class Environment : Errors
    {
        /// <summary>
        /// Fw16 -содержание версии и расположение dll
        /// </summary>
        public Fw16 Fw16 { get; set; }
        /// <summary>
        /// Ecr -содержание Модели и firmware 
        /// </summary>
        public Ecr Ecr { get; set; }

        public Environment()
        {
            Fw16 = new Fw16();
            Ecr = new Ecr();
        }
    }

    /// <summary>
    /// Fw16 -содержание версии и расположение dll
    /// </summary>
    [Serializable]
    public class Fw16 : Errors
    {
        /// <summary>
        /// Версия Пример: "1.2.6296.29040"
        /// </summary>
        [XmlAttribute("version")]
        public string version { get; set; }
        /// <summary>
        /// Пример: path="C:\GISS\Fw16.dll"
        /// </summary>
        [XmlAttribute]
        public string path { get; set; }

        public Fw16()
        {
        }
    }

    /// <summary>
    /// Ecr -содержание Модели и firmware поумолчанию notSet
    /// </summary>
    [Serializable]
    public class Ecr : Errors
    {
        /// <summary>
        /// Пример: model="POSprint FP510-Ф"
        /// </summary>
        [XmlAttribute]
        public string model { get; set; }
        /// <summary>
        /// Пример: "h05f011s180d31032017"
        /// </summary>
        [XmlAttribute]
        public string firmvare { get; set; }
        /// <summary>
        /// добовление сообщения об ошибки
        /// </summary>
        public Error error;

        public Ecr()
        {
            model = "notSet";
            firmvare = "notSet";
        }
    }
    /// <summary>
    /// Начало смены содержит атрибуты начала и конца смены
    /// </summary>
    [Serializable]
    public class Shift : Errors
    {

        /// <summary>
        /// Атрибут содержащий время начала смены
        /// </summary>
        [XmlAttribute]
        public string opened;
        /// <summary>
        /// Атрибут конца смены
        /// </summary>
        [XmlAttribute]
        public string closed;
        /// <summary>
        /// Тег содержащий информацию о типе документа и его содержание 
        /// </summary>
        public Documents Documents;
        /// <summary>
        /// Тег содержащий чек и суммы по итогу смены 
        /// </summary>
        public Summary Summary;
        /// <summary>
        /// добовление сообщения об ошибки
        /// </summary>
        public Error error;

        public Shift()
        {
        }
    }
    /// <summary>
    /// Тег содержащий Чек и Сумму в конце смены
    /// </summary>
    [Serializable]
    public class Documents : Errors
    {
        /// <summary>
        /// Чек с его атрибутами 
        /// </summary>
        [XmlElement]
        public List<Receipt> Receipt;
        /// <summary>
        /// добовление сообщения об ошибки
        /// </summary>
        [XmlElement]
        public List<Error> Error;

        //созданеи Сообщения об ошибке
        private ushort nExtError = 0;
        private bool flagError = false;
        /// <summary>
        /// создание нового сообщения об ошибке
        /// </summary>
        /// <returns>объект ошибки</returns>
        public Error NExtError()
        {
            if (!flagError) { flagError = true; Error = new List<Error>(); }
            Error.Add(new Error());
            nExtError++;
            return Error[nExtError - 1];
        }
        /// <summary>
        /// получение текущего объекта ошибки
        /// </summary>
        /// <returns>объект ошибки</returns>
        public Error currentError()
        {
            return Error[nExtError - 1];
        }

        //созданеи чека
        private ushort nExt = 0;
        private bool flagShift = false;
        /// <summary>
        /// создание нового чека; нельзя использовать 
        /// </summary>
        /// <returns>объект чека</returns>
        public Receipt NExtReceipt()
        {
            if (!flagShift) { flagShift = true; Receipt = new List<Receipt>(); }
            Receipt.Add(new Receipt());
            nExt++;
            return Receipt[nExt - 1];
        }
        /// <summary>
        /// получение текущего объекта чека
        /// </summary>
        /// <returns>объект чека</returns>
        public Receipt currentShift()
        {
            return Receipt[nExt - 1];
        }
    }
    /// <summary>
    /// Чек содержащий атрибуты: начала\конца чека, фскального номера, признак чека(приход, уход...),
    /// сумму чека, внесенную сумму наличными\электронными, количество товаров)
    /// </summary>
    [Serializable]
    public class Receipt : Errors
    {
        /// <summary>
        /// Начало чека
        /// </summary>
        [XmlAttribute("dt-start")]
        public string dtStart;
        /// <summary>
        /// Конец чека
        /// </summary>
        [XmlAttribute("dt-end")]
        public string dtEnd;
        /// <summary>
        /// Фискальный номер
        /// </summary>
        [XmlAttribute("fiscal-number")]
        public string fiscal;
        /// <summary>
        /// признак чека(приход, уход...)
        /// </summary>
        [XmlAttribute("sign")]
        public string sign;
        /// <summary>
        /// суммa чека
        /// </summary>
        [XmlAttribute("total")]
        public string total;
        /// <summary>
        /// наличные 
        /// </summary>
        [XmlAttribute("cash")]
        public string cash;
        /// <summary>
        /// электронные 
        /// </summary>
        [XmlAttribute("electron")]
        public string electron;
        /// <summary>
        /// Кол-во товара 
        /// </summary>
        [XmlAttribute("item-count")]
        public string itemСount;
        /// <summary>
        /// добовление сообщения об ошибки
        /// </summary>
        public Error error;
    }
    /// <summary>
    /// сумма в конце смены; атрибуты кол-во фискальных\нефискальных докуметов
    /// </summary>
    [Serializable]
    public class Summary : Errors
    {
        /// <summary>
        /// кол-во фискальных докуметов 
        /// </summary>
        [XmlAttribute("fd-count")]
        public string fdСount;
        /// <summary>
        /// кол-во нефискальных докуметов 
        /// </summary>
        [XmlAttribute("nfd-count")]
        public string nfdСount;
        /// <summary>
        /// сумма по чеку прихода
        /// </summary>
        public Income Income;
        /// <summary>
        /// сумма по чеку возврата прихода
        /// </summary>
        public IncomeBack IncomeBack;

    }

    [Serializable]
    public class Income : Errors
    {
        /// <summary>
        /// сумма наличных денег
        /// </summary>
        [XmlAttribute("cash")]
        public string cash;
        /// <summary>
        /// сумма электронных денег 
        /// </summary>
        [XmlAttribute("electron")]
        public string electron;
    }

    [Serializable]
    public class IncomeBack : Errors
    {
        /// <summary>
        /// сумма наличных денег
        /// </summary>
        [XmlAttribute("cash")]
        public string cash;
        /// <summary>
        /// сумма электронных денег 
        /// </summary>
        [XmlAttribute("electron")]
        public string electron;
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
            catch { Console.WriteLine("файл не существует");nameXML = "false"; }
            
        }

        
        static void Main(string[] args)
        {
            string directory="def";
            Console.Write("Укажите путь к файлу: ");
            //directory = Console.ReadLine();                                       // получить путь к файлу
            //while (directory != "")                                               //зацикливание
            {
                if (directory == "def")                                                                 //Тестовый вариант пути к логам
                {
                    try
                    {
                        File.Delete("save\\newxml.xml.xml");
                    }
                    catch { }
                    NameOfNewXML("save\\newxml.xml");
                    directory = "FP180119.log";
                }   
                else
                {
                    NameOfNewXML(directory);
                }

                // Просто тестирование
                // Провел небольшое тестирование переполнения и при 1000 на 1000 циклах хром не хочет открыать xml но xml пишется без особой нагузки
                Fw16Log fw16Log = new Fw16Log();
                fw16Log.dateEnd = "time";
                fw16Log.errorCount = 5;
                fw16Log.Environment.Fw16.path = "fw16.dll";
                fw16Log.Environment.Fw16.version = "11122222";
                fw16Log.NExtError().code = "ERRORTEXTTEST";
                fw16Log.Environment.Ecr.NExtError().cmd = "ECRTESTERRORTEXT";
                for (int i = 0; i < 10; i++)
                {
                    fw16Log.NExtShift().closed = "true";
                    fw16Log.currentShift().opened = "now";
                    fw16Log.currentShift().Documents = new Documents();
                    for (int j = 0; j < 10; j++)
                    {
                        fw16Log.currentShift().Documents.NExtReceipt().dtStart = "2018-01-18T09:06:01";
                        fw16Log.currentShift().Documents.currentShift().dtEnd = "never";
                        fw16Log.currentShift().Documents.currentShift().electron = "test";
                        fw16Log.currentShift().Documents.currentShift().cash = "test";
                        fw16Log.currentShift().Documents.currentShift().fiscal = "test";
                        fw16Log.currentShift().Documents.currentShift().itemСount = "test";
                        fw16Log.currentShift().Documents.currentShift().total = "test";
                        fw16Log.currentShift().Documents.currentShift().sign = "test";
                    }
                    fw16Log.currentShift().Summary = new Summary();
                    fw16Log.currentShift().Summary.Income = new Income();
                    fw16Log.currentShift().Summary.Income.cash = "123";
                    fw16Log.currentShift().Summary.Income.electron = "123";
                    fw16Log.currentShift().Summary.fdСount = "1";
                    fw16Log.NExtShift().opened = "123test";
                }
                //КОНЕЦ тестирования
                if (nameXML != "false")
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(Fw16Log));                       //сборка xml файла 

                    using (FileStream fs = new FileStream(nameXML, FileMode.OpenOrCreate))
                    {
                        formatter.Serialize(fs, fw16Log);
                    }
                }
                Console.ReadKey();
            }
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

