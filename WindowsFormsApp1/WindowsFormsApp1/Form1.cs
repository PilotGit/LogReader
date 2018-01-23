using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        XmlDocument XMLdoc;
        string nameXML;
        public Form1()
        {
            InitializeComponent();
        }

        private void startXML()                                                                                 //создание xml файла
        {
            nameXML = Directory.GetFiles(@"save\", "newXML*").Last();
            MessageBox.Show( nameXML = Regex.Replace(nameXML, "[0-9]+", string.Empty));
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //создания окна выбора файла
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Текстовые формата (*.log)|*.log|Все файлы (*.*)|*.*";
            openFileDialog1.ShowDialog();
            try
            {
                //myText= File.ReadAllBytes(openFileDialog1.FileName);
                richTextBox1.LoadFile(openFileDialog1.FileName,RichTextBoxStreamType.PlainText);                //Не правильная кодировка в тексте
                //richTextBox1.Lines = File.ReadAllLines(openFileDialog1.FileName,Encoding.UTF8);
                //XmlTextWriter textWriter = new XmlTextWriter(openFileDialog1.FileName, Encoding.UTF8);          //cоздание xml файла
                startXML();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            catch { MessageBox.Show("Ошибка!"); }                                                               //Вывод ошибки !
        }
    }
}