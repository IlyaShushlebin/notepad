using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace notepad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            richTextBox1.SelectionIndent = 5;
            richTextBox1.SelectionRightIndent = 5;
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private string RegularExpressionsNumbers(string str)
        {
            string s = "";
            Regex regex = new Regex(@"\d");
            MatchCollection matchCollection = regex.Matches(str);
            if (matchCollection.Count > 0)
            {
                foreach(Match key in matchCollection)
                {
                    s += key.Value;                    
                }
            }
            return s;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {            
            //string regaxAll = @"\w\W*";
            string regaxNumbers = @"\b\d+(\.|,\d*)?\b";
            string regaxTags = @"</?[^>]+>";
            string regaxInvertedСommas = @"\""((?=[\w\W])[^\""])*\""";//кавычки
            string regaxOneLineComment = @"//.*";
            string regaxMultilineComment = @"/\*((?=[\w\W])[^/\*])*\*/";

            string regaxDate = @"\d{2}(\.|/|-)\d{2}\1\d{4}";
            //Построй соотношение слов с цветом, которым хочешь их выделить. Выделять будем все слова независимо от регистра
            KeyValuePair<Regex, Color>[] PAIRS = new KeyValuePair<Regex, Color>[]
            {
                //new KeyValuePair<Regex, Color>(new Regex(regaxAll, RegexOptions.IgnoreCase), Color.FromArgb(255,255,255)),
                new KeyValuePair<Regex, Color>(new Regex(regaxNumbers, RegexOptions.IgnoreCase), Color.FromArgb(151,255,168)),
                new KeyValuePair<Regex, Color>(new Regex(regaxTags, RegexOptions.IgnoreCase), Color.FromArgb(255,179,52)),
                new KeyValuePair<Regex, Color>(new Regex(regaxOneLineComment, RegexOptions.IgnoreCase), Color.Green),
                new KeyValuePair<Regex, Color>(new Regex(regaxMultilineComment, RegexOptions.IgnoreCase), Color.Green),
                new KeyValuePair<Regex, Color>(new Regex(regaxInvertedСommas, RegexOptions.IgnoreCase), Color.FromArgb(255,122,122)),
            };

            SetWhiteFontColor(richTextBox1);
            //При вводе в richtextbox проверь каждый из наборов на совпадение
            foreach (var element in PAIRS)
                ColourText(richTextBox1, element.Key, element.Value);

            SetFormateDate(richTextBox1, regaxDate);
        }

        private void SetWhiteFontColor(RichTextBox rtb)
        {
            int pos = rtb.SelectionStart;
            rtb.Select(5, rtb.SelectionLength);
            rtb.SelectionColor = Color.White;
            rtb.DeselectAll();
            rtb.SelectionStart = pos;

        }

        private void SetFormateDate(RichTextBox rtb, string regEx)
        {
            int pos = rtb.SelectionStart;
            Regex regex = new Regex(regEx);
            foreach (Match match in regex.Matches(rtb.Text))
            {
                if(DateTime.TryParse(match.Value, out DateTime date))
                {
                    int posStart = match.Index;
                    rtb.Text = rtb.Text.Remove(posStart, 10);
                    rtb.SelectionStart = posStart;
                    rtb.SelectedText += date.ToString("D");
                }
                else
                {
                    rtb.SelectionStart = pos;
                    rtb.SelectionColor = Color.Red;
                    rtb.SelectedText += " ТАКОЙ ДАТЫ НЕ СУЩЕСТВУЕТ! ПРОВЕРЬТЕ ДАТУ!";
                    rtb.SelectionColor = Color.White;
                }
            }
        }

        private void ColourText(RichTextBox rtb, Regex regEx, Color col)
        {
            int pos = rtb.SelectionStart;
            foreach (Match match in regEx.Matches(rtb.Text))
            {
                rtb.Select(match.Index, match.Length);
                rtb.SelectionColor = col;
                rtb.DeselectAll();
            }
            rtb.SelectionStart = pos;
            rtb.SelectionColor = Color.White;
            
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
