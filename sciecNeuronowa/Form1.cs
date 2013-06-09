using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using pl.edu.pk.NeuralNetwork;

namespace sciecNeuronowa
{
    public partial class Form1 : Form
    {
        private Bitmap img;
        private List<Bitmap> foundLetters;
        private Network n;
        private FileStream plik;
        private StreamWriter zapisuj;
        private StreamReader czytaj;
        private string path;

        public Form1()
        {
            InitializeComponent();
            openButton.Click += OnOpenButtonClick;
            recognizeButton.Click += OnRecognizeButtonClick;
            button1.Click += button1_Click;
            button3.Click += button3_Click;
            btn_select.Click += btn_select_Click;
            btn_teach.Click += btn_teach_Click;
            foundLetters = new List<Bitmap>();
            n = new Network(144, 10, 20);
        }

        void button3_Click(object sender, EventArgs e)
        {
            string sLine = "";

            if(!String.IsNullOrEmpty(textBox2.Text))
            {
                plik = new FileStream(textBox2.Text, FileMode.Open, FileAccess.Read);
                czytaj = new StreamReader(plik);
                    
                ArrayList arrText = new ArrayList();
                List<double> output = new List<double>();
                List<double[] > input = new List<double[]>();
                    
                    
                while (sLine != null)
                {
                    sLine = czytaj.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }

                foreach (string line in arrText)
                {
                    string[] splitedLine = line.Split(':');

                    string[] numbers = splitedLine[1].Split(',');

                    double[] tmp = new double[144];

                    output.Add(Convert.ToDouble(splitedLine[0]));

                    for (int i = 0; i < numbers.Length - 1; i++ )
                    {
                        tmp[i] = Convert.ToDouble(numbers[i]);
                    }

                    input.Add(tmp);
                }
                    
                czytaj.Close();
                plik.Close();
                Console.WriteLine("Rozpoczeto uczenie");
                richTextBox2.AppendText("Zaczynam uczenie... To moze trochę potrwać...\n");
                n.fakeTeach(input.ToArray(), output.ToArray());
                richTextBox2.AppendText("Zakończono uczenie!\n");

            }
        }

        void btn_teach_Click(object sender, EventArgs e)
        {
            path = tbx_path.Text;
            string[] files = Directory.GetFiles(path);
            richTextBox2.Text = "";
            plik = new FileStream(path + "\\dane.txt", FileMode.Create, FileAccess.Write);
            zapisuj = new StreamWriter(plik);
            //zapisuj.Write(plik);
            zapisuj.Close();
            plik.Close();
            richTextBox2.AppendText("Aktualnie przetwarzane pliki: \n\n");
            
            foreach (var file in files)
            {
                richTextBox2.AppendText(file + "\n");
                richTextBox2.SelectionStart = this.richTextBox2.Text.Length;
                richTextBox2.ScrollToCaret();

                if(file.Contains(".jpg") || file.Contains(".JPG") || file.Contains(".JPEG"))
                {
                    img = new Bitmap(file);
                    pictureBox1.Image = img;
                    richTextBox2.AppendText("Numer pliku: " + getFileNumer(file) + "\n");

                    plik = new FileStream(path + "\\dane.txt", FileMode.Append, FileAccess.Write);
                    zapisuj = new StreamWriter(plik);
                    zapisuj.Write("\r\n");
                    zapisuj.Write(getFileNumer(file)+":");
                    zapisuj.Close();
                    plik.Close();
                    OnRecognizeButtonClick(this, EventArgs.Empty);
                }
            }
            MessageBox.Show("Uczenie zakonczone");
        }

        

        private string getFileNumer(string file)
        {
            string[] names = file.Split('\\');

            string[] fileNumbers = names[names.Length - 1].Split('_');

            string filename = fileNumbers[0];
            
            return filename;
        }

        void btn_select_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            
            if (!String.IsNullOrEmpty(fbd.SelectedPath))
            {
                tbx_path.Text = fbd.SelectedPath;
            }
        }

        void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = open.FileName;
            } 
        }

        void OnRecognizeButtonClick(object sender, EventArgs e)
        {
            richTextBox1.Text += "Rozpoczynam wyszukiwanie liter\n";
            this.Refresh();
            CharFinder cf = new CharFinder(img);
            cf.search();
            pictureBox1.Image = new Bitmap(img);
            richTextBox1.Text += "Zakończyłem wyszukiwanie liter\n";
            foreach (int[] vector in cf.getVectors())
            {
                richTextBox1.Text += "Rozpoznano: " + n.eval(vector) + "\n";
            }
            /*
            if(!String.IsNullOrEmpty(path))
            {
                plik = new FileStream(path + "\\dane.txt", FileMode.Append, FileAccess.Write);
                zapisuj = new StreamWriter(plik);
                int i = 0;
                foreach(int[] vector in cf.getVectors() )
                {

                    richTextBox1.Text += "Wektor [" + i++ + "]: ";
                    foreach (int vector_element in vector)
                    {
                        //richTextBox1.Text += vector_element + " ";
                        
                        zapisuj.Write(vector_element+",");
                    }
                    zapisuj.Write("\r\n");
                    richTextBox1.Text += "\n";
                }
                zapisuj.Close();
                plik.Close();
            }*/
        }


        void OnOpenButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "Image Files(*.jpg; *.png; *.gif; *.bmp)|*.jpg; *.png; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                img = new Bitmap(open.FileName);
                pictureBox1.Image = img;
                textBox1.Text = open.FileName;
                richTextBox1.Text += "Wczytałem plik " + open.FileName + "\n"; 

            } 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            btn_teach_Click(this,EventArgs.Empty);
        }



       
    }
}
