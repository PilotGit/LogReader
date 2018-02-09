using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using System.Data.SqlClient;

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
        /// Текст GETSTATUS
        /// </summary>
        [XmlAttribute("GETSTATUS")]
        public string GETSTATUS;
        /// <summary>
        /// Текст GETSUM
        /// </summary>
        [XmlAttribute("GETSUM")]
        public string GETSUM;
        /// <summary>
        /// Текст GETSUM
        /// </summary>
        [XmlAttribute("GETCOUNT")]
        public string GETCOUNT;
        /// <summary>
        /// дополнительная информация об ошибке 
        /// </summary>
        public string Message;
        public static ushort count;
        public Error() { count++; }
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
        private ushort nExt = 0;
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
            try
            {
                return Shift[nExt - 1];
            }
            catch
            {
                return NExtShift();
            }

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
        [XmlIgnore]
        public bool set = false;

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
        public string firmware { get; set; }
        [XmlIgnore]
        public bool setFirmware = false;

        public Ecr()
        {
            model = "notSet";
            firmware = "notSet";
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
        public string opened = "notSET";
        /// <summary>
        /// Атрибут конца смены
        /// </summary>
        [XmlAttribute]
        public string closed = "notSET";
        /// <summary>
        /// Тег содержащий информацию о типе документа и его содержание 
        /// </summary>
        public Documents Documents;
        /// <summary>
        /// Тег содержащий чек и суммы по итогу смены 
        /// </summary>
        public Summary Summary;

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
        /// Чек с его атрибутами ; нельзя использовать 
        /// </summary>
        [XmlElement]
        public List<Receipt> Receipt;
        /// <summary>
        /// Не фискальный Чек с его атрибутами ; нельзя использовать 
        /// </summary>
        [XmlElement]
        public List<Nonfiscal> Nonfiscal;
        /// <summary>
        /// Документ коррекции с его атрибутами ; нельзя использовать 
        /// </summary>
        [XmlElement]
        public List<CORRDOC> CORRDOC;
        [XmlIgnore]
        public string currentDoc;

        //созданеи чека
        private ushort nExt = 0;
        private bool flagShift = false;
        /// <summary>
        /// создание нового чека
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
        public Receipt currentReceipt()
        {
            try
            {
                return Receipt[nExt - 1];
            }
            catch
            {
                return null;
            }
        }


        //созданеи Не фискального чека
        private ushort NonfiscalnExt = 0;
        private bool NonfiscalflagShift = false;
        /// <summary>
        /// создание нового чека
        /// </summary>
        /// <returns>объект Не фискального чека</returns>
        public Nonfiscal NExtNonfiscal()
        {
            if (!NonfiscalflagShift) { NonfiscalflagShift = true; Nonfiscal = new List<Nonfiscal>(); }
            Nonfiscal.Add(new Nonfiscal());
            NonfiscalnExt++;
            return Nonfiscal[NonfiscalnExt - 1];
        }
        /// <summary>
        /// получение текущего объекта Не фискального чека
        /// </summary>
        /// <returns>объект Не фискального чека</returns>
        public Nonfiscal currentNonfiscal()
        {
            return Nonfiscal[NonfiscalnExt - 1];
        }


        //создание документа коррекции
        private ushort CORRDOCnExt = 0;
        private bool CORRDOCflagShift = false;
        /// <summary>
        /// создание нового чека
        /// </summary>
        /// <returns>объект Не фискального чека</returns>
        public CORRDOC NExtCORRDOC()
        {
            if (!CORRDOCflagShift) { CORRDOCflagShift = true; CORRDOC = new List<CORRDOC>(); }
            CORRDOC.Add(new CORRDOC());
            CORRDOCnExt++;
            return CORRDOC[CORRDOCnExt - 1];
        }
        /// <summary>
        /// получение текущего объекта Не фискального чека
        /// </summary>
        /// <returns>объект Не фискального чека</returns>
        public CORRDOC currentCORRDOC()
        {
            return CORRDOC[CORRDOCnExt - 1];
        }
    }

    /// <summary>
    /// Документ содержащий атрибуты: начала\конца чека, фскального номера, признак чека(приход, уход...),
    /// сумму, внесенную сумму наличными\электронными
    /// </summary>
    [Serializable]
    public class Nonfiscal : Errors
    {
        /// <summary>
        /// Начало документа
        /// </summary>
        [XmlAttribute("dt-start")]
        public string dtStart;
        /// <summary>
        /// Конец документа
        /// </summary>
        [XmlAttribute("dt-end")]
        public string dtEnd;
        /// <summary>
        /// признак чека(приход, уход...)
        /// </summary>
        [XmlAttribute("sign")]
        public string sign;
        /// <summary>
        /// текст в обработке
        /// </summary>
        [XmlAttribute]
        public string Title;
        /// <summary>
        /// суммa чека
        /// </summary>
        [XmlAttribute("total")]
        public decimal total;
        /// <summary>
        /// наличные 
        /// </summary>
        [XmlAttribute("cash")]
        public decimal cash;
        /// <summary>
        /// электронные 
        /// </summary>
        [XmlAttribute("electron")]
        public decimal electron;

    }

    /// <summary>
    /// Документ содержащий атрибуты: начала\конца чека, фскального номера, признак чека(приход, уход...),
    /// сумму, внесенную сумму наличными\электронными
    /// </summary>
    [Serializable]
    public class CORRDOC : Errors
    {
        /// <summary>
        /// Начало документа
        /// </summary>
        [XmlAttribute("dt-start")]
        public string dtStart;
        /// <summary>
        /// Конец документа
        /// </summary>
        [XmlAttribute("dt-end")]
        public string dtEnd;
        /// <summary>
        /// признак чека(приход, уход...)
        /// </summary>
        [XmlAttribute("sign")]
        public string sign;
        /// <summary>
        /// текст в обработке
        /// </summary>
        [XmlAttribute]
        public string Taxation;
        /// <summary>
        /// суммa чека
        /// </summary>
        [XmlAttribute("total")]
        public decimal total;
        /// <summary>
        /// наличные 
        /// </summary>
        [XmlAttribute("cash")]
        public decimal cash;
        /// <summary>
        /// электронные 
        /// </summary>
        [XmlAttribute("electron")]
        public decimal electron;

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
        public decimal total;
        /// <summary>
        /// наличные 
        /// </summary>
        [XmlAttribute("cash")]
        public decimal cash;
        /// <summary>
        /// электронные 
        /// </summary>
        [XmlAttribute("electron")]
        public decimal electron;
        /// <summary>
        /// Кол-во товара 
        /// </summary>
        [XmlAttribute("item-count")]
        public decimal itemСount;
        [XmlIgnore]
        public bool end = true;
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
        public decimal fdСount;
        /// <summary>
        /// кол-во нефискальных докуметов 
        /// </summary>
        [XmlAttribute("nfd-count")]
        public decimal nfdСount;
        /// <summary>
        /// сумма по чеку прихода
        /// </summary>
        public Income Income;
        /// <summary>
        /// сумма по чеку возврата прихода
        /// </summary>
        public IncomeBack IncomeBack;
        public Outcome Outcome;
        public OutcomeBack OutcomeBack;
    }

    [Serializable]
    public class Income : Errors
    {
        /// <summary>
        /// сумма наличных денег
        /// </summary>
        [XmlAttribute("cash")]
        public decimal cash;
        /// <summary>
        /// сумма электронных денег 
        /// </summary>
        [XmlAttribute("electron")]
        public decimal electron;
    }

    [Serializable]
    public class IncomeBack : Errors
    {
        /// <summary>
        /// сумма наличных денег
        /// </summary>
        [XmlAttribute("cash")]
        public decimal cash;
        /// <summary>
        /// сумма электронных денег 
        /// </summary>
        [XmlAttribute("electron")]
        public decimal electron;
    }

    [Serializable]
    public class Outcome : Errors
    {
        /// <summary>
        /// сумма наличных денег
        /// </summary>
        [XmlAttribute("cash")]
        public decimal cash;
        /// <summary>
        /// сумма электронных денег 
        /// </summary>
        [XmlAttribute("electron")]
        public decimal electron;
    }

    [Serializable]
    public class OutcomeBack : Errors
    {
        /// <summary>
        /// сумма наличных денег
        /// </summary>
        [XmlAttribute("cash")]
        public decimal cash;
        /// <summary>
        /// сумма электронных денег 
        /// </summary>
        [XmlAttribute("electron")]
        public decimal electron;
    }

    [Serializable]
    public class tree
    {
        [XmlElement("testB")]
        public List<Branch> branch;

        private ushort nExt = 0;
        private bool flagbranch = false;
        public Branch NExtbranch()
        {
            if (!flagbranch) { flagbranch = true; branch = new List<Branch>(); }
            branch.Add(new Branch());
            nExt++;
            return branch[nExt - 1];
        }
        public Branch currentShift()
        {
            try
            {
                return branch[nExt - 1];
            }
            catch
            {
                return NExtbranch();
            }

        }
    }

    [Serializable]
    public class Branch
    {
        [XmlElement]
        public string branch;
    }

    class Program
    {
        const string REGTime = @"\d+:\d+:\d+\.?\d+";
        /// <summary>
        /// путь к сохранению лога
        /// </summary>
        static string nameXML = @"save/newxml.xml";
        /// <summary>
        /// путь к исходному файлу
        /// </summary>
        static string directory;
        /// <summary>
        /// вывод информации для уточнения ошибки. либо так либо перпеписывать код
        /// </summary>
        static string NOTgotoButWorthIt = null;
        static string GETSUM, GETCOUNT, GETSTATUS;

        static public decimal progresprogressbar(StreamReader fileStream, decimal count)
        {
            while ((Convert.ToDecimal(fileStream.BaseStream.Length / 50)) * count < Convert.ToDecimal(fileStream.BaseStream.Position))
            {
                count++;
                Console.Write("|");
            }

            return count;
        }
        /// <summary>
        /// образование имени файла и расположения файла ?directory!=""
        /// </summary>
        /// <param name="file">путь к файлу</param>
        /// <param name="directorySave">путь сохранения</param>
        /// <returns></returns>
        static string NameOfNewXML(string file, string directorySave = "")                                                                                 //Получение нового имени базовым именем NewXML
        {
            directorySave = Regex.Replace(directorySave, "\"", "");
            try
            {
                file = Regex.Replace(file, "\"", "");
                File.OpenRead(file);
                if (Regex.Match(file, @"[^\\]*log").ToString() == "") { Console.WriteLine("файл имеет расширение не .log"); nameXML = "false"; return ""; }
                Directory.CreateDirectory((directorySave == "" ? Directory.GetParent(file).ToString() : directorySave) + "\\save");
                Console.WriteLine("Файл сохранен в: {0}", nameXML = ((directorySave == "") ? Directory.GetParent(file).ToString() : directorySave) + @"\save\" + Regex.Match(file, @"[^\\]*log").ToString() + ".xml");
                try { File.Delete(nameXML); } catch { }
                directorySave = file;
                directory = directorySave;
                Directory.CreateDirectory(Path.GetDirectoryName(nameXML));
                return directorySave;
            }
            catch { Console.WriteLine("файл не существует"); nameXML = "false"; return ""; }

        }
        /// <summary>
        /// Возврат номера команды в случае нахождения команды
        /// </summary>
        /// <param name="getCod"> строка для поиска команды</param>
        /// <returns>пустую строку либо код команды</returns>
        static string GetCod(string getCod, Fw16Log fw16Log)
        {
            try
            {
                if (!fw16Log.Environment.Fw16.set)                                      //чтение из заголовка информации о .dll
                    if (getCod.IndexOf("=== Старт") > 0)
                    {
                        fw16Log.Environment.Fw16.path = (Regex.Match(getCod, "[A-Z]:.*dll")).ToString();
                        fw16Log.Environment.Fw16.version = (Regex.Match(getCod, @"\d+\.\d+\.\d+\.\d+")).ToString();
                        fw16Log.Environment.Fw16.set = true;
                    }
            }
            catch { }
            if (getCod.IndexOf("Команда FW16: [") <= 0)
                return null;
            else
            {
                getCod = getCod.Remove(0, getCod.IndexOf("Команда FW16: [") + 15);  //15-кол-во символов искомой строки
                return getCod.Remove(3);
            }
        }
        /// <summary>
        /// проверка на ошибку и возврат строки в случае отсутствия ошибки
        /// </summary>
        /// <param name="strTic">строка из потока</param>
        /// <param name="streamReader">поток</param>
        /// <param name="obj">текущий объект для Serialize</param>
        /// <returns></returns>
        static public string StringOrError(string strTic, StreamReader streamReader, dynamic obj, bool isget = true)
        {
            string name = Regex.Match(strTic, @"\w* \(.*\)").ToString();    //получение названия команды из скобок
            while (((strTic = streamReader.ReadLine()).IndexOf("----------")) == -1)
            {
                if (strTic.IndexOf("Код ответа FW16: [0] OK") >= 0)
                {
                    return strTic;
                }
                else
                {
                    if (strTic.IndexOf("Код ответа FW16: [0] OK") == -1 && strTic.IndexOf("Код ответа FW16: [") >= 0)
                    {
                        Match match = Regex.Match(strTic, @"([\d:]*.[\d]*) Код ответа FW16: \[(.*)\]\s+(.*)");
                        obj.NExtError().cmd = name;
                        obj.currentError().code = match.Groups[2].ToString();
                        obj.currentError().text = match.Groups[3].ToString();
                        obj.currentError().dt = match.Groups[1].ToString();
                        while (((strTic = streamReader.ReadLine()).IndexOf("----------------------")) == -1) { obj.currentError().Message += Regex.Replace(strTic, "\0", string.Empty) + "\r\n"; }
                        //чтение команд GETCOUNT (129)\GETSUM (128)\GETSTATUS (101)
                        if (isget)
                        {
                            while ((strTic = streamReader.ReadLine()) != null)
                            {
                                if (strTic.IndexOf("Команда FW16: [1") > 0)
                                {
                                    switch (strTic = Regex.Match(strTic, @"\[([0-9]{3})\]").Groups[1].ToString())
                                    {
                                        case "101":
                                            obj.currentError().GETSTATUS += " Mod=" + Regex.Match(streamReader.ReadLine(), "value=([0-9]*)").Groups[1].ToString();
                                            if (StringOrError(strTic, streamReader, obj, false) != null)
                                                obj.currentError().GETSTATUS += Regex.Match(streamReader.ReadLine(), " value=[0-9]*").ToString();
                                            else
                                                obj.currentError().GETSTATUS += "err101";
                                            break;
                                        case "129":
                                            obj.currentError().GETCOUNT += " Сounter=" + Regex.Match(streamReader.ReadLine(), "value=([0-9]*)").Groups[1].ToString();
                                            if (StringOrError(strTic, streamReader, obj, false) != null)
                                                obj.currentError().GETCOUNT += Regex.Match(streamReader.ReadLine(), "value=[0-9]*").ToString();
                                            else
                                                obj.currentError().GETCOUNT += "err129";
                                            break;
                                        case "128":
                                            obj.currentError().GETSUM += " Сounter=" + Regex.Match(streamReader.ReadLine(), "value=([0-9]*)").Groups[1].ToString();
                                            if (StringOrError(strTic, streamReader, obj, false) != null)
                                                obj.currentError().GETSUM += Regex.Match(streamReader.ReadLine(), "value=[0-9]*").ToString();
                                            else
                                                obj.currentError().GETSUM += "err128";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (strTic.IndexOf("Команда FW16: [") > 0)
                                {
                                    NOTgotoButWorthIt = strTic;
                                    break;
                                }
                            }
                        }
                        //Конец вывода 
                        return null;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// обработка лога
        /// </summary>
        static public void xmlWrite()
        {
            if (directory != "")
            {
                Fw16Log fw16Log = new Fw16Log();
                Error.count = 0;
                StreamReader streamReader = File.OpenText(directory);
                decimal count = 1;
                Console.WriteLine("[||||||||||||||||||||Обработка|||||||||||||||||||]");
                {
                    string strTic, typeCod;
                    while ((strTic = streamReader.ReadLine()) != null)
                    {

                        count = progresprogressbar(streamReader, count);
                        if (NOTgotoButWorthIt != null)
                        {
                            strTic = NOTgotoButWorthIt;
                            NOTgotoButWorthIt = null;
                        }
                        typeCod = GetCod(strTic, fw16Log);

                        if (typeCod != null)

                            switch (typeCod)
                            {
                                case "0":
                                    Console.WriteLine(strTic);
                                    break;
                                case "122":
                                    GetCommand_122(strTic, streamReader, fw16Log);
                                    break;
                                case "121":
                                    if (fw16Log.Environment.Ecr.model == "notSet")
                                        GetCommand_121(strTic, streamReader, fw16Log);
                                    break;
                                case "400":
                                    GetCommand_400(strTic, streamReader, fw16Log);
                                    break;
                                case "401":
                                    GetCommand_401(strTic, streamReader, fw16Log);
                                    break;
                                case "402":
                                    GetCommand_402(strTic, streamReader, fw16Log);
                                    break;
                                case "403":
                                    GetCommand_403(strTic, streamReader, fw16Log);
                                    break;
                                case "407":
                                    GetCommand_407(strTic, streamReader, fw16Log);
                                    break;
                                case "408":
                                    GetCommand_408(strTic, streamReader, fw16Log);
                                    break;
                                case "420":
                                    GetCommand_420(strTic, streamReader, fw16Log);
                                    break;
                                case "421":
                                    GetCommand_421(strTic, streamReader, fw16Log);
                                    break;
                                case "409":
                                    GetCommand_409(strTic, streamReader, fw16Log);
                                    break;
                                case "413":
                                    GetCommand_413(strTic, streamReader, fw16Log);
                                    break;

                                default:
                                    StringOrError(strTic, streamReader, fw16Log);
                                    break;


                            }
                    }
                }
                streamReader.Close();
                fw16Log.errorCount = Error.count;
                if (nameXML != "false")
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(Fw16Log));                       //сборка xml файла 

                    using (FileStream fs = new FileStream(nameXML, FileMode.OpenOrCreate))
                    {
                        formatter.Serialize(fs, fw16Log);
                    }
                }
            }
            //РАБОТА С БД!!-------------------------------------------
            //using (SqlConnection con = new SqlConnection(@"Data Source=plt-dvl;Initial Catalog=PGAutoTest;Integrated Security=True"))
            //{
            //    con.Open();

            //    SqlCommand sqlCommand = new SqlCommand($"INSERT INTO testTable( [name], [date],[xml]) VALUES ('{System.Environment.UserName}', '{DateTime.Today}','{nameXML}');", con);
            //    sqlCommand.ExecuteNonQuery();
            //}
            //Конец работы с бд---------------------------------------
            Console.WriteLine();
        }
        /// <summary>
        /// обработка всех логов в директории
        /// </summary>
        /// <param name="directoryfile">путь к деректории логов</param>
        /// <param name="directory">путь куда сохранить</param>
        /// <param name="nameOfFile">Этот параметр может содержать сочетание допустимого литерального пути и подстановочных символов (* и ?) 
        /// По умолчанию *.log </param>
        static public void XMLWhileNotEnd(string directoryfile, string directorySave = "", string nameOfFile = "*.log")
        {
            directoryfile = Regex.Replace(directoryfile, "\"", "");
            directorySave = Regex.Replace(directorySave, "\"", "");
            foreach (var a in Directory.EnumerateFiles(directoryfile, nameOfFile))
            {
                directory = NameOfNewXML(a, directorySave);
                Error.count = 0;
                if (directory != "")
                    xmlWrite();
            }

        }

        /// <summary>
        /// получение firmware
        /// </summary>
        /// <param name="strTic">строка из потока</param>
        /// <param name="streamReader">поток</param>
        /// <param name="obj">текущий объект для Serialize</param>
        /// <returns></returns>
        static public string GetCommand_122(string strTic, StreamReader streamReader, Fw16Log fw16Log)
        {
            var obj = fw16Log.Environment.Ecr;
            if (!obj.setFirmware)
                if ((StringOrError(strTic, streamReader, obj)) != null)
                {
                    strTic = streamReader.ReadLine();
                    obj.firmware = ((Regex.Match(strTic, @" value=([\w]*)").Groups[1]).ToString());
                    obj.setFirmware = true;
                    return strTic;
                }
            return null;
        }

        /// <summary>
        /// получение Model
        /// </summary>
        /// <param name="strTic">строка из потока</param>
        /// <param name="streamReader">поток</param>
        /// <param name="obj">текущий объект для Serialize</param>
        /// <returns></returns>
        static public string GetCommand_121(string strTic, StreamReader streamReader, Fw16Log fw16Log)
        {
            var obj = fw16Log.Environment.Ecr;
            if ((StringOrError(strTic, streamReader, obj)) != null)
            {
                strTic = streamReader.ReadLine();
                strTic = streamReader.ReadLine();                                               //переход на строку с моделью 
                obj.model = ((Regex.Match(strTic, "Model=(.*)[[]").Groups[1]).ToString());
                obj.model = Regex.Replace(obj.model, "\0", string.Empty);
                return strTic;
            }
            return null;
        }

        /// <summary>
        /// получение начала смены
        /// </summary>
        /// <param name="strTic">строка из потока</param>
        /// <param name="streamReader">поток</param>
        /// <param name="obj">текущий объект для Serialize</param>
        /// <returns></returns>
        static public string GetCommand_400(string strTic, StreamReader streamReader, Fw16Log fw16Log)
        {
            var obj = fw16Log.NExtShift();
            if ((strTic = StringOrError(strTic, streamReader, obj)) != null)
            {
                obj.opened = ((Regex.Match(strTic, @"([\d:]*.[\d]*) Код").Groups[1]).ToString());
                return strTic;
            }
            return null;
        }

        /// <summary>
        /// получение конца смены
        /// </summary>
        /// <param name="strTic">строка из потока</param>
        /// <param name="streamReader">поток</param>
        /// <param name="obj">текущий объект для Serialize</param>
        /// <returns></returns>
        static public string GetCommand_401(string strTic, StreamReader streamReader, Fw16Log fw16Log)
        {
            var obj = fw16Log.currentShift();
            try
            {
                if (fw16Log.currentShift().Documents != null)
                    if (fw16Log.currentShift().Documents.currentReceipt() != null)
                    {
                        if (fw16Log.currentShift().Documents.currentReceipt() != null)
                            if (fw16Log.currentShift().Documents.currentReceipt().dtEnd == null)
                                fw16Log.currentShift().Documents.currentReceipt().NExtError().Message = "Документ не закрыт!!!";
                        if (fw16Log.currentShift().Documents.currentNonfiscal() != null)
                            if (fw16Log.currentShift().Documents.currentReceipt().dtEnd == null)
                                fw16Log.currentShift().Documents.currentNonfiscal().NExtError().Message = "Документ не закрыт!!!";
                    }
            }
            catch { }
            if ((strTic = StringOrError(strTic, streamReader, obj)) != null)
            {

                obj.closed = ((Regex.Match(strTic, @"([\d:]*.[\d]*) Код").Groups[1]).ToString());
                return strTic;
            }
            return null;
        }

        /// <summary>
        /// получение начала документа и его типа
        /// </summary>
        /// <param name="strTic">строка из потока</param>
        /// <param name="streamReader">поток</param>
        /// <param name="obj">текущий объект для Serialize</param>
        /// <returns></returns>
        static public string GetCommand_402(string strTic, StreamReader streamReader, Fw16Log fw16Log)
        {
            if (fw16Log.currentShift().Documents == null)
                fw16Log.currentShift().Documents = new Documents();
            var obj = fw16Log.currentShift().Documents.currentReceipt();
            if (obj != null)
                if (obj.dtEnd == null)
                {
                    obj.NExtError().Message = "Документ не закрыт!!!";
                }
            obj = fw16Log.currentShift().Documents.NExtReceipt();
            obj.end = false;
            obj.dtStart = ((Regex.Match(strTic, REGTime)).ToString());
            strTic = streamReader.ReadLine();
            strTic = streamReader.ReadLine();
            obj.sign = ((Regex.Match(strTic, @"Operation=(\w+)").Groups[1]).ToString());
            fw16Log.currentShift().Documents.currentDoc = "Receipt";
            if ((strTic = StringOrError(strTic, streamReader, obj)) != null)
            {
                return strTic;
            }
            return null;
        }

        /// <summary>
        /// получение последних данных о товаре в чеке 
        /// </summary>
        /// <param name="strTic">строка из потока</param>
        /// <param name="streamReader">поток</param>
        /// <param name="obj">текущий объект для Serialize</param>
        /// <returns></returns>
        static public string GetCommand_403(string strTic, StreamReader streamReader, Fw16Log fw16Log)
        {
            try
            {
                var obj = fw16Log.currentShift().Documents.currentReceipt();
                if ((strTic = StringOrError(strTic, streamReader, obj)) != null)
                {
                    strTic = streamReader.ReadLine();                                              //переход на строку 
                    obj.itemСount = Convert.ToDecimal((Regex.Match(strTic, @"Position=([0-9]*)").Groups[1]).ToString());
                    strTic = streamReader.ReadLine();
                    strTic = streamReader.ReadLine();
                    strTic = streamReader.ReadLine();
                    obj.total = Convert.ToDecimal((Regex.Match(strTic, @"Total=([0-9,.]*)").Groups[1]).ToString().Replace(".", ","));
                    strTic = StringOrError(strTic, streamReader, obj);
                }
                return null;
            }
            catch
            {
                fw16Log.NExtError().dt = ((Regex.Match(strTic, REGTime)).ToString());
                fw16Log.currentError().Message = "Поврежденный лог!(403)";
                return null;
            }
        }

        /// <summary>
        /// закрытие чека
        /// </summary>
        /// <param name="strTic">строка из потока</param>
        /// <param name="streamReader">поток</param>
        /// <param name="obj">текущий объект для Serialize</param>
        /// <returns></returns>
        static public string GetCommand_407(string strTic, StreamReader streamReader, Fw16Log fw16Log)
        {
            try
            {
                var obj = fw16Log.currentShift().Documents.currentReceipt();
                if ((strTic = StringOrError(strTic, streamReader, obj)) != null)
                {
                    obj.dtEnd = ((Regex.Match(strTic, REGTime)).ToString());
                    if (fw16Log.currentShift().Summary == null) fw16Log.currentShift().Summary = new Summary();
                    switch (obj.sign)
                    {
                        case "Income":
                            if (fw16Log.currentShift().Summary.Income == null) fw16Log.currentShift().Summary.Income = new Income();
                            fw16Log.currentShift().Summary.Income.cash += obj.cash;
                            fw16Log.currentShift().Summary.Income.electron += obj.electron;
                            break;
                        case "IncomeBack":
                            if (fw16Log.currentShift().Summary.IncomeBack == null) fw16Log.currentShift().Summary.IncomeBack = new IncomeBack();
                            fw16Log.currentShift().Summary.IncomeBack.cash += obj.cash;
                            fw16Log.currentShift().Summary.IncomeBack.electron += obj.electron;
                            break;
                        case "OutcomeBack":
                            if (fw16Log.currentShift().Summary.OutcomeBack == null) fw16Log.currentShift().Summary.OutcomeBack = new OutcomeBack();
                            fw16Log.currentShift().Summary.OutcomeBack.cash += obj.cash;
                            fw16Log.currentShift().Summary.OutcomeBack.electron += obj.electron;
                            break;
                        case "Outcome":
                            if (fw16Log.currentShift().Summary.Outcome == null) fw16Log.currentShift().Summary.Outcome = new Outcome();
                            fw16Log.currentShift().Summary.Outcome.cash += obj.cash;
                            fw16Log.currentShift().Summary.Outcome.electron += obj.electron;
                            break;
                        default:
                            break;
                    }
                    fw16Log.currentShift().Summary.fdСount += 1;
                    return strTic;
                }
                else
                {
                    if (obj.currentError().code == "9")
                    {
                        obj.dtEnd = obj.currentError().dt;
                        if (fw16Log.currentShift().Summary == null) fw16Log.currentShift().Summary = new Summary();
                        switch (obj.sign)
                        {
                            case "Income":
                                if (fw16Log.currentShift().Summary.Income == null) fw16Log.currentShift().Summary.Income = new Income();
                                fw16Log.currentShift().Summary.Income.cash += obj.cash;
                                fw16Log.currentShift().Summary.Income.electron += obj.electron;
                                break;
                            case "IncomeBack":
                                if (fw16Log.currentShift().Summary.IncomeBack == null) fw16Log.currentShift().Summary.IncomeBack = new IncomeBack();
                                fw16Log.currentShift().Summary.IncomeBack.cash += obj.cash;
                                fw16Log.currentShift().Summary.IncomeBack.electron += obj.electron;
                                break;
                            case "OutcomeBack":
                                if (fw16Log.currentShift().Summary.OutcomeBack == null) fw16Log.currentShift().Summary.OutcomeBack = new OutcomeBack();
                                fw16Log.currentShift().Summary.OutcomeBack.cash += obj.cash;
                                fw16Log.currentShift().Summary.OutcomeBack.electron += obj.electron;
                                break;
                            case "Outcome":
                                if (fw16Log.currentShift().Summary.Outcome == null) fw16Log.currentShift().Summary.Outcome = new Outcome();
                                fw16Log.currentShift().Summary.Outcome.cash += obj.cash;
                                fw16Log.currentShift().Summary.Outcome.electron += obj.electron;
                                break;
                            default:
                                break;
                        }
                        fw16Log.currentShift().Summary.fdСount += 1;
                        return strTic;
                    }
                }
                return null;
            }
            catch
            {
                fw16Log.NExtError().dt = ((Regex.Match(strTic, REGTime)).ToString());
                fw16Log.currentError().Message = "Поврежденный лог!(407)";
                return null;
            }
        }
        /// <summary>
        /// Прирывание чека 
        /// </summary>
        /// <param name="strTic">строка из потока</param>
        /// <param name="streamReader">поток</param>
        /// <param name="obj">текущий объект для Serialize</param>
        /// <returns></returns>
        static public string GetCommand_408(string strTic, StreamReader streamReader, Fw16Log fw16Log)
        {
            try
            {
                fw16Log.currentShift().Documents.currentDoc = null;
                //var obj = fw16Log.currentShift().Documents.currentReceipt();
                if (fw16Log.currentShift().Documents.currentReceipt() != null)
                {
                    if (fw16Log.currentShift().Documents.currentReceipt().dtEnd == null)
                    {
                        fw16Log.currentShift().Documents.currentReceipt().dtEnd = ((Regex.Match(strTic, REGTime)).ToString());
                        fw16Log.currentShift().Documents.currentReceipt().NExtError().Message = "ABORT";
                        fw16Log.currentShift().Documents.currentReceipt().currentError().dt = ((Regex.Match(strTic, REGTime)).ToString());
                        strTic = StringOrError(strTic, streamReader, fw16Log.currentShift().Documents.currentReceipt());
                        return null;
                    }
                }
                if (fw16Log.currentShift().Documents.currentNonfiscal() != null)
                    if (fw16Log.currentShift().Documents.currentNonfiscal().dtEnd == null)
                    {
                        fw16Log.currentShift().Documents.currentNonfiscal().dtEnd = ((Regex.Match(strTic, REGTime)).ToString());
                        fw16Log.currentShift().Documents.currentNonfiscal().NExtError().Message = "ABORT";
                        fw16Log.currentShift().Documents.currentNonfiscal().currentError().dt = ((Regex.Match(strTic, REGTime)).ToString());
                        strTic = StringOrError(strTic, streamReader, fw16Log.currentShift().Documents.currentNonfiscal());
                        return null;
                    }
                if (fw16Log.currentShift().Documents.currentReceipt().dtStart == null && fw16Log.currentShift().Documents.currentNonfiscal().dtStart == null)
                {
                    fw16Log.NExtError().dt = ((Regex.Match(strTic, REGTime)).ToString());
                    fw16Log.currentError().Message = "Поврежденный лог!(408f)";
                    return null;
                }
                return null;
            }
            catch
            {
                fw16Log.NExtError().dt = ((Regex.Match(strTic, REGTime)).ToString());
                fw16Log.currentError().Message = "Поврежденный лог!(408b) Закрыт документ без открытия";
                return null;
            }
        }

        /// <summary>
        /// Открыть нефискальный документ
        /// </summary>
        /// <param name="strTic">строка из потока</param>
        /// <param name="streamReader">поток</param>
        /// <param name="obj">текущий объект для Serialize</param>
        /// <returns></returns>
        static public string GetCommand_420(string strTic, StreamReader streamReader, Fw16Log fw16Log)
        {
            if (fw16Log.currentShift().Documents == null) fw16Log.currentShift().Documents = new Documents();
            fw16Log.currentShift().Documents.NExtNonfiscal().dtStart = ((Regex.Match(strTic, REGTime)).ToString());
            fw16Log.currentShift().Documents.currentDoc = "Nonfiscal";
            strTic = StringOrError(strTic, streamReader, fw16Log.currentShift().Documents.currentNonfiscal());
            return null;
        }

        /// <summary>
        /// Открыть нефискальный документ
        /// </summary>
        /// <param name="strTic">строка из потока</param>
        /// <param name="streamReader">поток</param>
        /// <param name="obj">текущий объект для Serialize</param>
        /// <returns></returns>
        static public string GetCommand_413(string strTic, StreamReader streamReader, Fw16Log fw16Log)
        {
            try
            {
                var curentCORRDOC = fw16Log.currentShift().Documents.NExtCORRDOC();
                curentCORRDOC.dtStart = Regex.Match(strTic, REGTime).ToString();
                streamReader.ReadLine();
                curentCORRDOC.sign = Regex.Match((strTic = streamReader.ReadLine()), "Income|Outcome").ToString();
                curentCORRDOC.Taxation = Regex.Match((strTic = streamReader.ReadLine()), "=(.)").Groups[1].ToString();
                streamReader.ReadLine();
                curentCORRDOC.total = GetDecimalAmount(streamReader.ReadLine());
                curentCORRDOC.cash = GetDecimalAmount(streamReader.ReadLine());
                curentCORRDOC.electron = GetDecimalAmount(streamReader.ReadLine());
                curentCORRDOC.dtEnd = Regex.Match(strTic, REGTime).ToString();
                if ((strTic = StringOrError(strTic, streamReader, curentCORRDOC)) != null)
                    curentCORRDOC.dtEnd = Regex.Match(strTic, REGTime).ToString();
            }
            catch
            {
                fw16Log.NExtError().dt = Regex.Match(strTic, REGTime).ToString();
                fw16Log.currentError().text = "Ошибка во время обработки (413)";
            }
            return null;
        }

        /// <summary>
        /// Закрыть нефискальный документ
        /// </summary>
        /// <param name="strTic">строка из потока</param>
        /// <param name="streamReader">поток</param>
        /// <param name="obj">текущий объект для Serialize</param>
        /// <returns></returns>
        static public string GetCommand_421(string strTic, StreamReader streamReader, Fw16Log fw16Log)
        {
            try
            {
                var obj = fw16Log.currentShift().Documents.currentNonfiscal();
                if ((strTic = StringOrError(strTic, streamReader, obj)) != null)
                {
                    obj.dtEnd = ((Regex.Match(strTic, REGTime)).ToString());
                    if (fw16Log.currentShift().Summary == null) fw16Log.currentShift().Summary = new Summary();
                    switch (obj.sign)
                    {
                        case "Income":
                            if (fw16Log.currentShift().Summary.Income == null) fw16Log.currentShift().Summary.Income = new Income();
                            fw16Log.currentShift().Summary.Income.cash += obj.cash;
                            fw16Log.currentShift().Summary.Income.electron += obj.electron;
                            break;
                        case "IncomeBack":
                            if (fw16Log.currentShift().Summary.IncomeBack == null) fw16Log.currentShift().Summary.IncomeBack = new IncomeBack();
                            fw16Log.currentShift().Summary.IncomeBack.cash += obj.cash;
                            fw16Log.currentShift().Summary.IncomeBack.electron += obj.electron;
                            break;
                        case "OutcomeBack":
                            if (fw16Log.currentShift().Summary.OutcomeBack == null) fw16Log.currentShift().Summary.OutcomeBack = new OutcomeBack();
                            fw16Log.currentShift().Summary.OutcomeBack.cash += obj.cash;
                            fw16Log.currentShift().Summary.OutcomeBack.electron += obj.electron;
                            break;
                        case "Outcome":
                            if (fw16Log.currentShift().Summary.Outcome == null) fw16Log.currentShift().Summary.Outcome = new Outcome();
                            fw16Log.currentShift().Summary.Outcome.cash += obj.cash;
                            fw16Log.currentShift().Summary.Outcome.electron += obj.electron;
                            break;
                        default:
                            break;
                    }
                    fw16Log.currentShift().Summary.nfdСount += 1;
                    return strTic;
                }
                return null;
            }
            catch
            {
                fw16Log.NExtError().dt = ((Regex.Match(strTic, REGTime)).ToString());
                fw16Log.currentError().Message = "Поврежденный лог!(407)";
                return null;
            }
        }

        /// <summary>
        /// Получение оплаты
        /// </summary>
        /// <param name="strTic">строка из потока</param>
        /// <param name="streamReader">поток</param>
        /// <param name="obj">текущий объект для Serialize</param>
        /// <returns></returns>
        static public string GetCommand_409(string strTic, StreamReader streamReader, Fw16Log fw16Log)
        {
            try
            {
                streamReader.ReadLine();
                strTic = streamReader.ReadLine();
                if ((strTic).IndexOf("Cash") > 0)
                {
                    strTic = streamReader.ReadLine();
                    if (fw16Log.currentShift().Documents.currentDoc == "Nonfiscal")
                    {
                        fw16Log.currentShift().Documents.currentNonfiscal().cash += GetDecimalAmount(strTic);
                        if ((strTic = StringOrError(strTic, streamReader, fw16Log.currentShift().Documents.currentNonfiscal())) != null)
                        {
                            strTic = streamReader.ReadLine();
                            fw16Log.currentShift().Documents.currentNonfiscal().cash += GetDecimalAmount(strTic) > 0 ? 0 : GetDecimalAmount(strTic);
                        }
                    }
                    if (fw16Log.currentShift().Documents.currentDoc == "Receipt")
                    {
                        fw16Log.currentShift().Documents.currentReceipt().cash += GetDecimalAmount(strTic);
                        if ((strTic = StringOrError(strTic, streamReader, fw16Log.currentShift().Documents.currentReceipt())) != null)
                        {
                            strTic = streamReader.ReadLine();
                            fw16Log.currentShift().Documents.currentReceipt().cash += GetDecimalAmount(strTic) > 0 ? 0 : GetDecimalAmount(strTic);
                        }
                    }
                }
                else
                {
                    strTic = streamReader.ReadLine();
                    if (fw16Log.currentShift().Documents.currentDoc == "Nonfiscal")
                    {
                        fw16Log.currentShift().Documents.currentNonfiscal().electron += GetDecimalAmount(strTic);
                        StringOrError(strTic, streamReader, fw16Log.currentShift().Documents.currentNonfiscal());
                    }
                    if (fw16Log.currentShift().Documents.currentDoc == "Receipt")
                    {
                        fw16Log.currentShift().Documents.currentReceipt().electron += GetDecimalAmount(strTic);
                        StringOrError(strTic, streamReader, fw16Log.currentShift().Documents.currentReceipt());
                    }
                }
                return strTic;
            }
            catch
            {
                fw16Log.NExtError().dt = ((Regex.Match(strTic, REGTime)).ToString());
                fw16Log.currentError().Message = "Поврежденный лог!(409)";
                return null;
            }
        }
        /// <summary>
        /// Возврат decimal числа после знака равно из строки
        /// </summary>
        /// <param name="strTic">строка</param>
        /// <returns></returns>
        static decimal GetDecimalAmount(string strTic)
        {
            decimal Amount;
            Amount = Convert.ToDecimal(Regex.Match(strTic, "[=|-]([-0-9,.]+)").Groups[1].ToString().Replace(".", ","));
            return Amount;
        }



        static void Main(string[] args)
        {
            string fileName = "";                                       // получить путь к файлу
            //Обработка аргументов
            switch (args.Length)
            {
                case 0:
                    fileName = "defTest";
                    break;
                case 1:
                    if (Directory.Exists(Regex.Replace(args[0], "\"", "")) && !File.Exists(Regex.Replace(args[0], "\"", "")))   //проверка: это папка?
                        XMLWhileNotEnd(args[0]);
                    else
                    {
                        NameOfNewXML(args[0]);
                        xmlWrite();
                    }
                    break;
                case 2:
                    if (Directory.Exists(Regex.Replace(args[0], "\"", "")) && !File.Exists(Regex.Replace(args[0], "\"", "")))   //проверка: это папка?
                        XMLWhileNotEnd(args[0], args[1]);
                    else
                    {
                        Console.WriteLine(args[0]);
                        NameOfNewXML(args[0], args[1]);
                        xmlWrite();
                    }
                    break;
                case 3:
                    if (Directory.Exists(Regex.Replace(args[0], "\"", "")) && !File.Exists(Regex.Replace(args[0], "\"", "")))   //проверка: это папка?
                        XMLWhileNotEnd(args[0], args[1], args[2]);
                    else
                        Console.WriteLine("Не верные параметры");
                    break;
            }
            //обработка консольного режима
            while (fileName != "")                                               //зацикливание
            {
                Console.Write("\nУкажите путь к файлу: ");
                fileName = Console.ReadLine();
                //Тестовый вариант пути к логам
                switch (fileName)
                {
                    case "tes":
                        fileName = Console.ReadLine();
                        XMLWhileNotEnd(Regex.Replace(fileName, "\"", ""));
                        break;
                    case "def":
                    case "defTest":
                        try
                        {
                            FileStream fileStream;
                            fileStream = File.OpenRead("TESTDEFOLT.log");
                            fileStream.Close();
                        }
                        catch
                        {
                            Console.Clear();
                            bool flag = true;
                            while (flag)
                            {
                                Console.WriteLine("Введите путь к файлу .log");
                                try
                                {
                                    File.Copy(Regex.Replace(Console.ReadLine(), "\"", ""), (Directory.GetCurrentDirectory() + "\\TESTDEFOLT.log"), true);
                                    flag = false;
                                }
                                catch { Console.Clear(); Console.WriteLine("не верный путь"); }
                            }
                        }
                        directory = NameOfNewXML("TESTDEFOLT.log");
                        break;
                    case "removeDefolt":
                        try { File.Delete("TESTDEFOLT.log"); } catch { }
                        break;
                    case "tree":
                        File.Delete(@"save/testTree.xml");
                        nameXML = @"save/testTree.xml";
                        break;
                    default:
                        if (Directory.Exists(fileName = Regex.Replace(fileName, "\"", "")) && !File.Exists(fileName))
                            XMLWhileNotEnd(fileName);
                        else
                        {
                            directory = NameOfNewXML(fileName);
                            xmlWrite();
                        }
                        break;
                }


                if (fileName == "defTest")            //тестирование и пример того как должно работать
                {

                    Fw16Log fw16Log = new Fw16Log();
                    // Просто тестирование
                    // Провел небольшое тестирование переполнения и при 1000 на 1000 циклах хром не хочет открыать xml но xml пишется без особой нагузки
                    fw16Log.dateEnd = "time";
                    fw16Log.errorCount = 5;
                    fw16Log.Environment.Fw16.path = "fw16.dll";
                    fw16Log.Environment.Fw16.version = "11122222";
                    fw16Log.NExtError().code = "ERRORTEXTTEST";
                    fw16Log.Environment.Ecr.NExtError().cmd = "ECRTESTERRORTEXT";
                    for (int i = 0; i < 3; i++)
                    {
                        fw16Log.NExtShift().closed = "true";
                        fw16Log.currentShift().opened = "now";
                        fw16Log.currentShift().Documents = new Documents();
                        for (int j = 0; j < 3; j++)
                        {
                            fw16Log.currentShift().Documents.NExtReceipt().dtStart = "2018-01-18T09:06:01";
                            fw16Log.currentShift().Documents.currentReceipt().dtEnd = "never";
                            fw16Log.currentShift().Documents.currentReceipt().electron = 100;
                            fw16Log.currentShift().Documents.currentReceipt().cash = 150;
                            fw16Log.currentShift().Documents.currentReceipt().fiscal = "test";
                            fw16Log.currentShift().Documents.currentReceipt().itemСount = 1500;
                            fw16Log.currentShift().Documents.currentReceipt().total = 5000;
                            fw16Log.currentShift().Documents.currentReceipt().sign = "test";
                            fw16Log.currentShift().Documents.currentReceipt().NExtError().cmd = "Document ERROR TEST";
                            fw16Log.currentShift().Documents.NExtNonfiscal().dtStart = "!!!!!!!!!!!!!!!";
                            fw16Log.currentShift().Documents.currentNonfiscal().dtEnd = "^^^^^^^^^^^";
                        }
                        fw16Log.currentShift().Summary = new Summary();
                        fw16Log.currentShift().Summary.Income = new Income();
                        fw16Log.currentShift().Summary.Income.cash = 123;
                        fw16Log.currentShift().Summary.Income.electron = 123;
                        fw16Log.currentShift().Summary.fdСount = 123;
                        fw16Log.NExtShift().opened = "123test";
                    }
                    if (nameXML != "false")
                    {
                        XmlSerializer formatter = new XmlSerializer(typeof(Fw16Log));                       //сборка xml файла 

                        using (FileStream fs = new FileStream(nameXML, FileMode.OpenOrCreate))
                        {
                            formatter.Serialize(fs, fw16Log);
                        }
                    }
                }
                //КОНЕЦ тестирования

                if (fileName == "def")
                {
                    xmlWrite();
                }

                if (fileName == "tree")
                {
                    StreamReader streamReader = File.OpenText(directory);
                    tree tree = new tree();
                    decimal progress = 1;
                    Console.WriteLine("[||||||||||||||||||||Обработка|||||||||||||||||||]");
                    string str = null;
                    int count = 0;
                    {
                        string strTic, typeCod;
                        while ((strTic = streamReader.ReadLine()) != null)
                        {
                            progress = progresprogressbar(streamReader, progress);
                            if (strTic.IndexOf("Команда FW16: [") <= 0)
                                typeCod = null;
                            else
                            {
                                typeCod = strTic.Remove(0, strTic.IndexOf("Команда FW16: [") + 15);  //15-кол-во символов искомой строки
                                typeCod = typeCod.Remove(3);
                            }
                            if (typeCod != null && typeCod != "128")
                            {
                                if (str == typeCod) count++;
                                else { str = typeCod; count = 0; }
                                tree.NExtbranch().branch = strTic + "   повторение команды №" + count;
                                // Console.WriteLine(strTic);
                            }

                        }
                    }
                    {
                        XmlSerializer formatter = new XmlSerializer(typeof(tree));                       //сборка xml файла 

                        using (FileStream fs = new FileStream(nameXML, FileMode.OpenOrCreate))
                        {
                            formatter.Serialize(fs, tree);
                        }
                    }
                }


                //Console.ReadKey();
            }
        }
    }
}
