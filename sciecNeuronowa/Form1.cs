using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sciecNeuronowa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openButton.Click += OnOpenButtonClick;
            recognizeButton.Click += OnRecognizeButtonClick;
        }

        void OnRecognizeButtonClick(object sender, EventArgs e)
        {
            textBox1.Text = "Rozpoznawanie tekstu";
        }

        void OnOpenButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(open.FileName);
                textBox1.Text = open.FileName;
            } 
        }

       
    }
}
