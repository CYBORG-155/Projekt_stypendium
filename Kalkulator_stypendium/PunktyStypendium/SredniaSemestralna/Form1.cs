using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SredniaSemestralna
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// <c>firstTime</c> jest potrzebna dla tego, żeby wiedziećczy przyciszk <c>buttonPressMe_Click</c> jest wkliknięty pierwszy czy poszczególny raz
        /// </summary>
        bool firstTime = true;
        /// <summary>
        /// zmienna x potrzebna dla rozmieszczenia położenia w przestrzeni. Zwykła koordynata x w przestrzeni x,y.
        /// </summary>
        int x = 0;
        /// <summary>
        /// pad to odstęp między groupboxami z przedmiotami i ocenami.
        /// </summary>
        int pad = 5;
        /// <summary>
        /// zmianna potrzebna do przechowywania informacji ile przedmiotów jest wpisane do programu. Wykorzystuje się w obliczeniu średniej. Ta liczba jest mianownikiem w obliczeniu średniej.
        /// </summary>
        double howMeany = 0;
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Metoda <c>countSubjects</c> poszukuje groupboxy i czy zaznaczone w nich radiobuttony
        /// </summary>
        double countSubjects(double input)
        {
            foreach (var group in this.Controls.OfType<GroupBox>())
            {
                foreach (var item in group.Controls.OfType<RadioButton>())
                {
                    if (item.Checked)
                    {
                        input += Convert.ToDouble(item.Text);
                    }
                }
            }
            return input;
        }
        /// <summary>
        /// Metoda <c>countSubjectsTest()</c> jest testem jednostkowym sprawdzajacym działanie radiobuttonów w groupboxach
        /// </summary>
        public double countSubjectsTest()
        {
            double sum = 0;
            var oneRadioButton = this.Controls.OfType<GroupBox>().Select(x => x.Controls.OfType<RadioButton>())
                .FirstOrDefault().Where(x => x.Text == "2").FirstOrDefault();
            oneRadioButton.Select();
            foreach (var group in this.Controls.OfType<GroupBox>()){
                foreach (var item in group.Controls.OfType<RadioButton>())
                {
                    if (item.Enabled)
                    {
                        sum = double.Parse(item.Text);
                    }
                }
            }
            return sum;
        }
        /// <summary>
        /// Metoda <c>makeMe</c> pobiera przedmioty z pliku przedmioty.txt zamienia przecinki i tworzy listę
        /// </summary>
       void makeMe()
        {
            List<string> przedmioty = new List<string>();
            using (var sr = new StreamReader("przedmioty.txt"))
            {
                string text = sr.ReadToEnd();
                przedmioty = text.Split(',').ToList();
            }
            howMeany = przedmioty.Count;
            foreach (var item in przedmioty)
            {
                subjectCreation(item);
            }
        }
        /// <summary>
        /// Metoda <c>subjectCreation</c> pobrane przedmioty rozmieszcza w groupboxie i tworzy radiobuttony z wszystkimi możliwymi ocenami {2, 3, 3.5, 4, 4.5, 5}.
        /// Groupboxy są rozmieszczane w wyznaczonych miejscach z odstępem w 5 pikseli. 
        /// </summary>
        void subjectCreation(string przedmiot)
        {
            List<RadioButton> list = new List<RadioButton>();
            GroupBox g = new GroupBox();
            g.AutoSize = true;
            g.Visible = true;
            g.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            var lb = new Label();
            lb.AutoSize = true;
            lb.Location = new Point(6, 16);
            lb.Text = przedmiot;
            g.Controls.Add(lb);

            list.Add(new RadioButton()
            {
                AutoSize = true,
                Location = new Point(6, 35),
                Text = "2",
                Name = radioButton2Przedmiot + przedmiot,
            });
            list.Add(new RadioButton()
            {
                AutoSize = true,
                Location = new Point(6, 58),
                Text = "3",
                Name = radioButton3Przedmiot + przedmiot,
            });
            list.Add(new RadioButton()
            {
                AutoSize = true,
                Location = new Point(6, 81),
                Text = "3,5",
                Name = radioButton35Przedmiot + przedmiot,
            });
            list.Add(new RadioButton()
            {
                AutoSize = true,
                Location = new Point(6, 104),
                Text = "4",
                Name = radioButton4Przedmiot + przedmiot,
            });
            list.Add(new RadioButton()
            {
                AutoSize = true,
                Location = new Point(6, 127),
                Text = "4,5",
                Name = radioButton45Przedmiot + przedmiot,
            });
            list.Add(new RadioButton()
            {
                AutoSize = true,
                Location = new Point(6, 150),
                Text = "5",
                Name = radioButton5Przedmiot + przedmiot,
            });
            for (int j = 0; j < 6; j++)
            {
                g.Controls.Add(list[j]);
            }
            this.Controls.Add(g);
            g.Location = new Point(x, 12); x = x + g.Width + pad;
            
        }
        /// <summary>
        /// <c>groupBox_Enter</c> robi niewidocznymi przycisk i groupbox z radiobuttonami
        /// </summary>
        private void groupBox_Enter(object sender, EventArgs e)
        {
            groupBox.Visible = false;
            buttonSum.Visible = false;
        }

        private void buttonSum_Click(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Metoda <c>buttonPressMe_Click</c> przy pierwszym kliknięciu wykonuje metode makeMe, co spowoduje do wykonania powyższych metod.
        /// więc przy kliknięciu <c>buttonPressMe_Click</c> pobiera się lista przedmiotów i tworzy się interface z groupboxów.
        /// 
        /// Przy drugim kliknięciu <c>buttonPressMe_Click</c> oblicza średnią za semestr oraz ilość punktów do stypendium.
        /// 
        /// Na każdym z etapów można przepisać dodatkowe punkty, kolejność ich wpisania nie ma żadnego znaczenia.
        /// </summary>
        private void buttonPressMe_Click(object sender, EventArgs e)
        {
            if (firstTime)
            {
                makeMe();
                firstTime = false;
                //textBox2.Text = this.Controls.OfType<GroupBox>().FirstOrDefault().Height.ToString(); jest potrzebne dla rozwoju programu
            }
            double sum = 0;
            sum = countSubjects(sum);
            sum -= 2;
            sum = sum / howMeany;
            textBoxAverage.Text = sum.ToString("#.00");
            sum = 50 * (sum * sum) - 200 * sum + 250;
            if (textBox1.Text != "")
            {
                sum += Convert.ToDouble(textBox1.Text);
            }
            textBoxPoints.Text = sum.ToString();
            
        }

        private void textBoxAverage_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPoints_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
     
    }
}
