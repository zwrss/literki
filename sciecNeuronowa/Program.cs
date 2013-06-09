using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using pl.edu.pk.NeuralNetwork;
using System.Diagnostics;

namespace sciecNeuronowa
{
    static class Program
    {
        public static Random random = new Random();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
