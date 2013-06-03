using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public Form1()
        {
            InitializeComponent();
            openButton.Click += OnOpenButtonClick;
            recognizeButton.Click += OnRecognizeButtonClick;
            button1.Click += button1_Click;
            foundLetters = new List<Bitmap>();
            n = new Network(64, 32, 10);
        }

        void button1_Click(object sender, EventArgs e)
        {
            n = new Network(64, Convert.ToInt32(textBox2.Text), 10);
        }

        void OnRecognizeButtonClick(object sender, EventArgs e)
        {
            richTextBox1.Text += "Rozpoczynam wyszukiwanie liter\n";
            this.Refresh();
            CharFinder cf = new CharFinder(img);
            cf.search();
            pictureBox1.Image = new Bitmap(img);
            richTextBox1.Text += "Zakończyłem wyszukiwanie liter\n"; 
        }


        void OnOpenButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                img = new Bitmap(open.FileName);
                pictureBox1.Image = img;
                textBox1.Text = open.FileName;
                richTextBox1.Text += "Wczytałem plik " + open.FileName + "\n"; 

            } 
        }

       
    }
}
