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
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            NNTest();
        }

        static void NNTest()
        {
            System.Console.WriteLine("Uszanowanko");
            Network nn = new Network(2, 1, 1);
            //Before teaching
            System.Console.WriteLine("Before teaching:");
            List<double> list = new List<double>();
            list.Add(-1.0); list.Add(-1.0);
            System.Console.WriteLine("Wynik F & F: " + nn.output(list)[0]);
            list = new List<double>();
            list.Add(-1.0); list.Add(1.0);
            System.Console.WriteLine("Wynik F & T: " + nn.output(list)[0]);
            list = new List<double>();
            list.Add(1.0); list.Add(-1.0);
            System.Console.WriteLine("Wynik T & F: " + nn.output(list)[0]);
            list = new List<double>();
            list.Add(1.0); list.Add(1.0);
            System.Console.WriteLine("Wynik T & T: " + nn.output(list)[0]);

            //Teaching
            System.Console.WriteLine("Teaching...");
            for (int i = 0; i < 10000; i++)
            {
                List<double> oList = new List<double>();
                oList.Add(-1.0);
                list = new List<double>();
                list.Add(-1.0); list.Add(-1.0);
                nn.teachOne(list, oList);

                oList = new List<double>();
                oList.Add(-1.0);
                list = new List<double>();
                list.Add(-1.0); list.Add(1.0);
                nn.teachOne(list, oList);

                oList = new List<double>();
                oList.Add(-1.0);
                list = new List<double>();
                list.Add(1.0); list.Add(-1.0);
                nn.teachOne(list, oList);

                oList = new List<double>();
                oList.Add(1.0);
                list = new List<double>();
                list.Add(1.0); list.Add(1.0);
                nn.teachOne(list, oList);
            }

            //After teaching
            System.Console.WriteLine("After teaching:");
            list = new List<double>();
            list.Add(-1.0); list.Add(-1.0);
            System.Console.WriteLine("Wynik F & F: " + nn.output(list)[0]);
            list = new List<double>();
            list.Add(-1.0); list.Add(1.0);
            System.Console.WriteLine("Wynik F & T: " + nn.output(list)[0]);
            list = new List<double>();
            list.Add(1.0); list.Add(-1.0);
            System.Console.WriteLine("Wynik T & F: " + nn.output(list)[0]);
            list = new List<double>();
            list.Add(1.0); list.Add(1.0);
            System.Console.WriteLine("Wynik T & T: " + nn.output(list)[0]);
        }
    }
}
